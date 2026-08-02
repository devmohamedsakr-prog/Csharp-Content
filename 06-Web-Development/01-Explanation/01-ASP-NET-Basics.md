# ASP.NET Core Basics

## Overview
ASP.NET Core is a cross-platform framework for building web applications, APIs, and microservices.

## Project Structure

### Core Components
```
MyWebApp/
├── Controllers/          # API endpoints
├── Models/              # Data models
├── Views/               # Razor templates (MVC)
├── wwwroot/             # Static files
├── appsettings.json     # Configuration
├── Program.cs           # App startup
└── Startup.cs           # Service configuration
```

## Middleware Pipeline

### Request Processing
```csharp
// Program.cs - Configure middleware
var builder = WebApplication.CreateBuilder(args);

// Add services to DI container
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure middleware pipeline
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
```

### Custom Middleware
```csharp
// Custom middleware class
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    
    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
        
        await _next(context);
        
        _logger.LogInformation($"Response: {context.Response.StatusCode}");
    }
}

// Register middleware
app.UseMiddleware<LoggingMiddleware>();
```

## Controllers

### Basic Controller
```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserAsync(int id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null)
            return NotFound();
        
        return Ok(user);
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUserAsync(CreateUserRequest request)
    {
        var user = await _userService.CreateUserAsync(request);
        return CreatedAtAction(nameof(GetUserAsync), new { id = user.Id }, user);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var result = await _userService.UpdateUserAsync(id, request);
        if (!result)
            return NotFound();
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result)
            return NotFound();
        
        return NoContent();
    }
}
```

## Routing

### Route Attributes
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // GET: api/products
    [HttpGet]
    public IActionResult GetAll() => Ok("all products");
    
    // GET: api/products/5
    [HttpGet("{id}")]
    public IActionResult Get(int id) => Ok($"product {id}");
    
    // GET: api/products/special/new
    [HttpGet("special/{category}")]
    public IActionResult GetSpecial(string category) => Ok($"special {category}");
    
    // POST: api/products
    [HttpPost]
    public IActionResult Create(CreateProductRequest request) => Created("", request);
}
```

## Dependency Injection

### Service Registration
```csharp
// Program.cs
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton<ICacheService, CacheService>();

// In controller
public class UsersController : ControllerBase
{
    public UsersController(IUserService userService, IEmailService emailService)
    {
        // Injected automatically
    }
}
```

### Lifetime Management
```csharp
// Transient: New instance every time
builder.Services.AddTransient<IEmailService, EmailService>();

// Scoped: New instance per HTTP request
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Singleton: Single instance for application lifetime
builder.Services.AddSingleton<ICacheService, CacheService>();
```

## Configuration

### appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyApp;Trusted_Connection=true;"
  },
  "AppSettings": {
    "JwtSecret": "your-secret-key",
    "ApiUrl": "https://api.example.com"
  }
}
```

### Reading Configuration
```csharp
public class AppService
{
    private readonly IConfiguration _config;
    
    public AppService(IConfiguration config)
    {
        _config = config;
    }
    
    public void Configure()
    {
        string connectionString = _config.GetConnectionString("DefaultConnection");
        string jwtSecret = _config["AppSettings:JwtSecret"];
        
        // Type-safe options
        var options = new AppOptions();
        _config.GetSection("AppSettings").Bind(options);
    }
}

public class AppOptions
{
    public string JwtSecret { get; set; }
    public string ApiUrl { get; set; }
}
```

## Error Handling

### Global Exception Handler
```csharp
// Middleware for global error handling
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    
    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsJsonAsync(new
            {
                message = "Internal server error",
                details = ex.Message
            });
        }
    }
}

// Register
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
```

## Best Practices

1. **Separate Controllers, Services, Repositories**
```csharp
// Controller: Request/Response
public class UsersController : ControllerBase
{
    public UsersController(IUserService service) { }
}

// Service: Business logic
public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    public async Task<User> GetUserAsync(int id) { }
}

// Repository: Data access
public class UserRepository : IUserRepository
{
    private readonly DbContext _context;
    public async Task<User> GetAsync(int id) { }
}
```

2. **Use DTOs for API Contracts**
```csharp
// DTO: What goes over wire
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// Entity: Database model
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
}
```

3. **Async All the Way**
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAsync()
{
    var users = await _userService.GetAllAsync();
    return Ok(users);
}
```

## Common Mistakes

1. **Synchronous Controller Actions**
```csharp
// Bad: Blocking
[HttpGet]
public IActionResult Get()
{
    var data = _service.GetData().Result; // BLOCKS!
    return Ok(data);
}

// Good: Async
[HttpGet]
public async Task<IActionResult> GetAsync()
{
    var data = await _service.GetDataAsync();
    return Ok(data);
}
```

2. **Not Using Dependency Injection**
```csharp
// Bad: Hard-coded dependencies
public class UserService
{
    private readonly UserRepository _repository = new UserRepository();
}

// Good: Injected dependencies
public class UserService
{
    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository) { _repository = repository; }
}
```

3. **Exposing Entity Models**
```csharp
// Bad: Returning database entity directly
[HttpGet("{id}")]
public User GetUser(int id) => _repository.Get(id);

// Good: Return DTO
[HttpGet("{id}")]
public UserDto GetUser(int id)
{
    var user = _repository.Get(id);
    return new UserDto { Id = user.Id, Name = user.Name };
}
```

## Quick Summary
- Middleware pipeline processes requests
- Controllers handle HTTP requests
- Dependency injection manages services
- Configuration from appsettings.json
- Global error handling middleware
- DTOs for API contracts
- Async all the way
- Separate concerns (Controller, Service, Repository)

## Resources
- ASP.NET Core documentation
- Tutorial: Create a web API
- Best practices and patterns
