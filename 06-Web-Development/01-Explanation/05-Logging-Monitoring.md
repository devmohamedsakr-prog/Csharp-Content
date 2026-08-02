# Logging and Monitoring

## Overview
Structured logging, log levels, and monitoring strategies for production systems.

## Structured Logging

### Serilog Configuration
```csharp
// Program.cs
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "MyApp")
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .CreateLogger();

try
{
    Log.Information("Application starting");
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Host.UseSerilog();
    
    var app = builder.Build();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
```

### Structured Logging with Context
```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    
    public UsersController(ILogger<UsersController> logger) => _logger = logger;
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var correlationId = Guid.NewGuid().ToString();
        using (_logger.BeginScope(new { CorrelationId = correlationId, UserId = request.Email }))
        {
            _logger.LogInformation("Creating user {Email}", request.Email);
            
            try
            {
                var user = await _userService.CreateAsync(request);
                _logger.LogInformation("User created successfully {UserId}", user.Id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user {Email}", request.Email);
                return StatusCode(500, "Error creating user");
            }
        }
    }
}
```

### Log Levels
```csharp
public class LoggingExample
{
    private readonly ILogger<LoggingExample> _logger;
    
    public LoggingExample(ILogger<LoggingExample> logger) => _logger = logger;
    
    public void DemonstrateLogLevels()
    {
        // Trace: Very detailed, usually disabled in production
        _logger.LogTrace("Method {Method} called with parameters", nameof(DemonstrateLogLevels));
        
        // Debug: Development diagnostics
        _logger.LogDebug("User object: {@User}", user);
        
        // Information: General flow (default level)
        _logger.LogInformation("User {UserId} logged in successfully", userId);
        
        // Warning: Something unexpected but recoverable
        _logger.LogWarning("User login failed for {Email}, attempt {AttemptNumber}", email, attempts);
        
        // Error: Error that doesn't stop the app
        _logger.LogError(ex, "Database connection failed for operation {Operation}", "GetUsers");
        
        // Critical: Critical error, app may stop
        _logger.LogCritical(ex, "Configuration error: missing API key");
    }
}
```

## Application Insights

### Configuration
```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

app.UseApplicationInsights();

// appsettings.json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-instrumentation-key"
  }
}
```

### Custom Telemetry
```csharp
public class TelemetryService
{
    private readonly TelemetryClient _telemetryClient;
    
    public TelemetryService(TelemetryClient telemetryClient) => _telemetryClient = telemetryClient;
    
    public void TrackUserAction(string action, string userId)
    {
        var properties = new Dictionary<string, string> { { "UserId", userId } };
        var metrics = new Dictionary<string, double> { { "Duration", 150 } };
        
        _telemetryClient.TrackEvent("UserAction", properties, metrics);
    }
    
    public void TrackException(Exception ex, string context)
    {
        var properties = new Dictionary<string, string> { { "Context", context } };
        _telemetryClient.TrackException(ex, properties);
    }
    
    public void TrackDependency(string name, string target, DateTime start, TimeSpan duration, bool success)
    {
        _telemetryClient.TrackDependency("SQL", target, name, start, duration, success);
    }
}

// Usage
[HttpPost]
public async Task<IActionResult> CreateUser(CreateUserRequest request)
{
    var startTime = DateTime.UtcNow;
    
    try
    {
        var user = await _userService.CreateAsync(request);
        _telemetryService.TrackUserAction("UserCreated", user.Id.ToString());
        
        var duration = DateTime.UtcNow - startTime;
        _telemetryService.TrackDependency("Database", "UserDb", startTime, duration, true);
        
        return Ok(user);
    }
    catch (Exception ex)
    {
        _telemetryService.TrackException(ex, "CreateUser");
        throw;
    }
}
```

## Health Checks

### Configuration
```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AddRedis(builder.Configuration.GetConnectionString("Redis"))
    .AddUrlGroup(new Uri("https://api.external-service.com"), "ExternalService")
    .AddCheck<CustomHealthCheck>("CustomCheck");

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
```

### Custom Health Check
```csharp
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _connectionFactory;
    
    public DatabaseHealthCheck(IDbConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await connection.ExecuteScalarAsync("SELECT 1");
            
            return HealthCheckResult.Healthy("Database connection successful");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection failed", ex);
        }
    }
}

// Registration
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database");
```

## Performance Monitoring

### Request/Response Logging Middleware
```csharp
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
    
    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var requestBody = await ReadRequestBody(context.Request);
        var stopwatch = Stopwatch.StartNew();
        
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        
        try
        {
            await _next(context);
            
            stopwatch.Stop();
            var responseBodyText = await ReadResponseBody(context.Response);
            
            _logger.LogInformation(
                "Request {Method} {Path} returned {StatusCode} in {Duration}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
            );
            
            await responseBody.CopyToAsync(originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
    
    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        var body = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }
    
    private async Task<string> ReadResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }
}

// Program.cs
app.UseMiddleware<RequestResponseLoggingMiddleware>();
```

## Best Practices

1. **Use Structured Logging**
```csharp
// Good: Structured data
_logger.LogInformation("User {UserId} created with email {Email}", user.Id, user.Email);

// Bad: String concatenation
_logger.LogInformation($"User {user.Id} created with email {user.Email}");
```

2. **Log at Appropriate Levels**
```csharp
// Good: Right level for context
try
{
    await _userService.CreateAsync(request);
    _logger.LogInformation("User created successfully");
}
catch (DuplicateEmailException ex)
{
    _logger.LogWarning(ex, "Failed: email already exists");
}
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error creating user");
}

// Bad: Everything at error level
_logger.LogError("User created");
```

3. **Include Correlation IDs**
```csharp
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    
    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;
    
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue("X-Correlation-ID", out var id)
            ? id.ToString()
            : Guid.NewGuid().ToString();
        
        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers.Add("X-Correlation-ID", correlationId);
        
        using (_logger.BeginScope(new { CorrelationId = correlationId }))
        {
            await _next(context);
        }
    }
}
```

## Common Mistakes

1. **Logging Sensitive Data**
```csharp
// Bad: Logging password or token
_logger.LogInformation("Login attempt: {Username} {Password}", username, password);

// Good: Log only non-sensitive info
_logger.LogInformation("Login attempt for {Username}", username);
```

2. **Over-Logging**
```csharp
// Bad: Logs every single operation
foreach (var item in items)
{
    _logger.LogInformation("Processing item {ItemId}", item.Id);
    _logger.LogInformation("Item {ItemId} processed", item.Id);
}

// Good: Log meaningful checkpoints
_logger.LogInformation("Processing {ItemCount} items", items.Count);
_logger.LogInformation("Processing complete");
```

3. **Not Using Context**
```csharp
// Bad: No context about request
_logger.LogError(ex, "Database error");

// Good: Include context
_logger.LogError(ex, "Database error while creating user {UserId}", userId);
```

## Quick Summary
- Structured logging > string concatenation
- Use Serilog for production logging
- Log levels: Trace, Debug, Information, Warning, Error, Critical
- Use Application Insights for monitoring
- Include correlation IDs for request tracing
- Implement health checks for dependencies
- Never log sensitive data
- Use log scopes for context
- Monitor request/response times
- Set up alerts for errors and critical issues
- Centralize logs for analysis
- Regular log retention policies

## Resources
- Serilog Documentation
- Application Insights
- Health Checks in ASP.NET Core
- Structured Logging Best Practices
