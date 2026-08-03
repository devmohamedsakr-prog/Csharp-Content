# Exception Handling Interview Questions - Hard Level

## Question 1: Design Production Exception Handling Architecture

### Question
Design a comprehensive exception handling architecture for a large production system including logging, monitoring, and recovery strategies.

### Answer

**Multi-Layer Architecture:**
```csharp
// 1. Domain Exception Hierarchy
public abstract class DomainException : Exception {
    public string ErrorCode { get; protected set; }
    public DateTime OccurredAt { get; protected set; }
    
    protected DomainException(string message, string errorCode)
        : base(message) {
        ErrorCode = errorCode;
        OccurredAt = DateTime.UtcNow;
    }
}

public class ValidationException : DomainException {
    public List<ValidationError> Errors { get; }
    public ValidationException(List<ValidationError> errors)
        : base("Validation failed", "VALIDATION_ERROR") {
        Errors = errors;
    }
}

// 2. Application Exception Handling
public class ApplicationExceptionHandler {
    private readonly ILogger logger;
    private readonly IMetrics metrics;
    private readonly IAlertService alerts;
    
    public void HandleException(Exception ex, string context) {
        var errorId = GenerateErrorId();
        
        LogException(ex, errorId, context);
        TrackMetrics(ex);
        NotifyIfCritical(ex, errorId);
        
        if (ShouldRecover(ex)) {
            AttemptRecovery(ex);
        }
    }
    
    private void LogException(Exception ex, string errorId, string context) {
        var level = DetermineSeverity(ex);
        
        logger.Log(level, ex,
            "Exception {ErrorId} in {Context}: {Message}",
            errorId, context, ex.Message);
        
        // Log full details
        LogExceptionDetails(ex, errorId);
    }
    
    private void TrackMetrics(Exception ex) {
        metrics.Increment($"exceptions.{ex.GetType().Name}");
        
        if (IsDbError(ex)) {
            metrics.Increment("exceptions.database");
        }
        if (IsTimeout(ex)) {
            metrics.Increment("exceptions.timeout");
        }
    }
    
    private void NotifyIfCritical(Exception ex, string errorId) {
        if (IsCritical(ex)) {
            alerts.SendAlert(new {
                Severity = "Critical",
                ErrorId = errorId,
                Type = ex.GetType().Name,
                Message = ex.Message,
                Time = DateTime.UtcNow
            });
        }
    }
}

// 3. Global Exception Middleware
public class GlobalExceptionMiddleware {
    private readonly RequestDelegate next;
    private readonly IExceptionHandler handler;
    
    public async Task InvokeAsync(HttpContext context) {
        try {
            await next(context);
        } catch (Exception ex) {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception ex) {
        var errorId = GenerateErrorId();
        handler.HandleException(ex, context.Request.Path);
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = GetStatusCode(ex);
        
        await context.Response.WriteAsJsonAsync(new {
            errorId,
            message = GetUserMessage(ex),
            code = GetErrorCode(ex),
            timestamp = DateTime.UtcNow
        });
    }
}
```

**Key Features:**
- Centralized exception handling
- Structured logging with error IDs
- Metrics and monitoring
- Alert on critical errors
- Graceful user messages
- Error tracking and recovery

### Follow-up
- How do you handle cascading failures?
- What's the circuit breaker pattern?
- How do you coordinate retry logic?

---

## Question 2: Implement Async-Safe Exception Handling

### Question
Show how to handle exceptions in async/await code and IAsyncDisposable.

### Answer

**Async Exception Handling:**
```csharp
public class AsyncService {
    // Proper async exception handling
    public async Task<Data> GetDataAsync(int id) {
        if (id <= 0) {
            throw new ArgumentException("Invalid ID");
        }
        
        try {
            var data = await FetchDataAsync(id);
            return data ?? throw new NotFoundException("Data not found");
        } catch (TimeoutException ex) {
            logger.Warn("Fetch timeout");
            return GetCachedData(id) ?? throw new DataAccessException(
                "Cannot retrieve data",
                ex
            );
        } catch (HttpRequestException ex) {
            logger.Error("Network error", ex);
            throw new DataAccessException("Network failure", ex);
        }
    }
    
    // Parallel operations with exception aggregation
    public async Task ProcessMultipleAsync(List<int> ids) {
        var tasks = ids.Select(id => ProcessAsync(id)).ToList();
        
        try {
            await Task.WhenAll(tasks);
        } catch (Exception ex) {
            // Log all failed tasks
            var faulted = tasks.Where(t => t.IsFaulted).ToList();
            foreach (var task in faulted) {
                logger.Error("Task failed", task.Exception);
            }
            
            throw new AggregateException("Multiple operations failed", faulted
                .Select(t => t.Exception)
                .OfType<Exception>());
        }
    }
    
    // Timeout handling
    public async Task<Data> GetDataWithTimeoutAsync(int id) {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        
        try {
            return await FetchDataAsync(id, cts.Token);
        } catch (OperationCanceledException) {
            logger.Warn("Operation timeout");
            throw new TimeoutException("Data fetch timed out");
        }
    }
}

// IAsyncDisposable
public class AsyncResource : IAsyncDisposable {
    private DbConnection connection;
    private bool disposed = false;
    
    public AsyncResource() {
        connection = new DbConnection();
    }
    
    public async Task<Data> GetDataAsync(int id) {
        if (disposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
        
        return await connection.QueryAsync(id);
    }
    
    public async ValueTask DisposeAsync() {
        if (!disposed) {
            try {
                await connection.CloseAsync();
            } catch (Exception ex) {
                logger.Error("Dispose failed", ex);
            }
            
            connection?.Dispose();
            disposed = true;
        }
    }
}

// Usage
await using (var resource = new AsyncResource()) {
    var data = await resource.GetDataAsync(1);
}  // Async dispose called
```

**Best Practices:**
- Await Task.WhenAll() to catch AggregateException
- Use CancellationToken for timeout
- Implement IAsyncDisposable for async cleanup
- Log all exceptions in parallel scenarios
- Handle OperationCanceledException explicitly

### Follow-up
- What's the difference between async and sync exception handling?
- How do you implement retry logic with async?
- What about ConfigureAwait in exception scenarios?

---

## Question 3: Design Resilience Patterns (Retry, Circuit Breaker)

### Question
Implement retry logic and circuit breaker pattern for handling transient failures.

### Answer

**Retry Pattern:**
```csharp
public class RetryPolicy {
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation,
        int maxRetries = 3,
        TimeSpan? initialDelay = null) {
        
        initialDelay ??= TimeSpan.FromMilliseconds(100);
        int attempt = 0;
        TimeSpan delay = initialDelay.Value;
        
        while (true) {
            try {
                return await operation();
            } catch (Exception ex) when (IsTransient(ex) && attempt < maxRetries) {
                attempt++;
                logger.Info($"Transient error, retry {attempt}/{maxRetries}");
                
                await Task.Delay(delay);
                delay = TimeSpan.FromMilliseconds(
                    Math.Min(delay.TotalMilliseconds * 2, 30000)  // Exponential backoff
                );
            }
        }
    }
    
    private bool IsTransient(Exception ex) {
        return ex switch {
            TimeoutException => true,
            HttpRequestException hre => hre.StatusCode >= 500,
            DbUpdateException => false,  // Not transient
            _ => false
        };
    }
}

// Circuit Breaker
public class CircuitBreaker {
    private enum State { Closed, Open, HalfOpen }
    
    private State state = State.Closed;
    private int failureCount = 0;
    private DateTime lastFailureTime = DateTime.MinValue;
    private readonly int failureThreshold = 5;
    private readonly TimeSpan openDuration = TimeSpan.FromSeconds(60);
    
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation) {
        if (state == State.Open) {
            if (DateTime.UtcNow - lastFailureTime > openDuration) {
                state = State.HalfOpen;
                failureCount = 0;
            } else {
                throw new CircuitBreakerOpenException("Circuit breaker is open");
            }
        }
        
        try {
            var result = await operation();
            OnSuccess();
            return result;
        } catch (Exception ex) {
            OnFailure();
            throw;
        }
    }
    
    private void OnSuccess() {
        failureCount = 0;
        if (state == State.HalfOpen) {
            state = State.Closed;
        }
    }
    
    private void OnFailure() {
        failureCount++;
        lastFailureTime = DateTime.UtcNow;
        
        if (failureCount >= failureThreshold) {
            state = State.Open;
        }
    }
}

// Combined Retry + Circuit Breaker
public class ResilientClient {
    private readonly RetryPolicy retryPolicy;
    private readonly CircuitBreaker circuitBreaker;
    
    public async Task<Data> GetDataAsync(int id) {
        return await circuitBreaker.ExecuteAsync(() =>
            retryPolicy.ExecuteAsync(() => FetchDataAsync(id))
        );
    }
}
```

**Exception Handling Strategy:**
```csharp
public class ExceptionClassifier {
    public ExceptionHandlingStrategy Classify(Exception ex) {
        return ex switch {
            // Transient - should retry
            TimeoutException => ExceptionHandlingStrategy.Retry,
            HttpRequestException hre when hre.StatusCode >= 500 
                => ExceptionHandlingStrategy.Retry,
            
            // Circuit breaker - too many failures
            CircuitBreakerOpenException => ExceptionHandlingStrategy.Fail,
            
            // Client error - don't retry
            ArgumentException => ExceptionHandlingStrategy.Fail,
            HttpRequestException hre when hre.StatusCode < 500
                => ExceptionHandlingStrategy.Fail,
            
            // Unknown - log and fail
            _ => ExceptionHandlingStrategy.LogAndFail
        };
    }
}
```

### Follow-up
- How do you test resilience patterns?
- When should you use circuit breaker vs retry?
- How do you monitor circuit breaker state?

---

## Question 4: Handle Complex IDisposable Hierarchies

### Question
Design disposing strategy for complex object graphs with interdependent resources.

### Answer

**Complex Disposal Hierarchy:**
```csharp
// Base resource
public abstract class BaseResource : IDisposable {
    protected bool disposed = false;
    
    protected abstract void CleanupManaged();
    protected abstract void CleanupUnmanaged();
    
    protected virtual void Dispose(bool disposing) {
        if (!disposed) {
            if (disposing) {
                CleanupManaged();
            }
            CleanupUnmanaged();
            disposed = true;
        }
    }
    
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    ~BaseResource() => Dispose(false);
}

// Database connection resource
public class DatabaseConnection : BaseResource {
    private DbConnection connection;
    private DbTransaction transaction;
    
    public DatabaseConnection(string connectionString) {
        connection = new DbConnection(connectionString);
        connection.Open();
    }
    
    public DbTransaction BeginTransaction() {
        return transaction = connection.BeginTransaction();
    }
    
    protected override void CleanupManaged() {
        transaction?.Dispose();
        connection?.Close();
    }
    
    protected override void CleanupUnmanaged() {
        // Clean native resources if any
    }
}

// Command wrapper
public class DatabaseCommand : BaseResource {
    private DbConnection connection;
    private DbCommand command;
    
    public DatabaseCommand(DatabaseConnection conn, string sql) {
        connection = conn ?? throw new ArgumentNullException(nameof(conn));
        command = connection.CreateCommand();
        command.CommandText = sql;
    }
    
    public async Task<DbDataReader> ExecuteReaderAsync() {
        if (disposed) throw new ObjectDisposedException(GetType().Name);
        return await command.ExecuteReaderAsync();
    }
    
    protected override void CleanupManaged() {
        command?.Dispose();
    }
    
    protected override void CleanupUnmanaged() { }
}

// Unit of work pattern
public class UnitOfWork : IDisposable {
    private readonly DatabaseConnection connection;
    private List<IDisposable> resources = new();
    private bool disposed = false;
    
    public UnitOfWork(string connectionString) {
        connection = new DatabaseConnection(connectionString);
    }
    
    public T Register<T>(T resource) where T : IDisposable {
        if (disposed) throw new ObjectDisposedException(GetType().Name);
        resources.Add(resource);
        return resource;
    }
    
    public async Task<int> ExecuteAsync(string sql) {
        var command = Register(new DatabaseCommand(connection, sql));
        return await command.ExecuteNonQueryAsync();
    }
    
    public void Dispose() {
        if (!disposed) {
            // Dispose in reverse order
            for (int i = resources.Count - 1; i >= 0; i--) {
                resources[i]?.Dispose();
            }
            connection?.Dispose();
            disposed = true;
        }
    }
}

// Usage
public async Task<int> ProcessDataAsync(string connStr, string sql) {
    using (var uow = new UnitOfWork(connStr)) {
        return await uow.ExecuteAsync(sql);
    }
    // All resources disposed in correct order
}
```

**Key Principles:**
- Dispose resources in reverse order of creation
- Separate managed and unmanaged cleanup
- Use composition over inheritance when possible
- Track dependent resources
- Handle disposal failures gracefully

### Follow-up
- How do you handle circular dependencies in disposal?
- What about async resources in inheritance?
- How do you test disposal hierarchies?

---

## Question 5: Implement Exception Context and Correlation

### Question
Design exception tracking with correlation IDs across async boundaries.

### Answer

**Exception Context:**
```csharp
public class ExceptionContext {
    public string CorrelationId { get; set; }
    public string UserId { get; set; }
    public string RequestPath { get; set; }
    public Dictionary<string, object> AdditionalData { get; set; }
}

// AsyncLocal for context across async boundaries
public static class RequestContext {
    private static readonly AsyncLocal<ExceptionContext> context 
        = new AsyncLocal<ExceptionContext>();
    
    public static ExceptionContext Current {
        get => context.Value ??= new();
        set => context.Value = value;
    }
}

// Exception enrichment
public class ExceptionEnricher {
    public static Exception EnrichException(Exception ex) {
        var ctx = RequestContext.Current;
        
        var data = ex.Data;
        data["CorrelationId"] = ctx.CorrelationId;
        data["UserId"] = ctx.UserId;
        data["RequestPath"] = ctx.RequestPath;
        data["Timestamp"] = DateTime.UtcNow;
        
        foreach (var item in ctx.AdditionalData ?? new()) {
            data[item.Key] = item.Value;
        }
        
        return ex;
    }
}

// Middleware to set context
public class ContextMiddleware {
    private readonly RequestDelegate next;
    
    public async Task InvokeAsync(HttpContext httpContext) {
        var correlationId = httpContext.Request.Headers
            .FirstOrDefault(x => x.Key == "X-Correlation-ID").Value
            .ToString() ?? Guid.NewGuid().ToString();
        
        RequestContext.Current = new ExceptionContext {
            CorrelationId = correlationId,
            UserId = httpContext.User?.Identity?.Name,
            RequestPath = httpContext.Request.Path,
            AdditionalData = new()
        };
        
        try {
            await next(httpContext);
        } catch (Exception ex) {
            ExceptionEnricher.EnrichException(ex);
            throw;
        }
    }
}

// Structured logging with context
public class ContextualLogger {
    private readonly ILogger logger;
    
    public void LogException(Exception ex) {
        var ctx = RequestContext.Current;
        
        logger.LogError(
            "Exception occurred. CorrelationId: {CorrelationId}, UserId: {UserId}, Path: {Path}",
            ctx.CorrelationId,
            ctx.UserId,
            ctx.RequestPath
        );
        
        logger.LogError(ex, "Full exception");
        
        // Log exception data
        foreach (var item in ex.Data.Keys) {
            logger.LogError("Data[{Key}]: {Value}", item, ex.Data[item]);
        }
    }
}

// Usage
public class OrderService {
    private readonly ContextualLogger logger;
    
    public async Task<Order> CreateOrderAsync(OrderRequest request) {
        try {
            ValidateOrder(request);
            var order = await SaveOrderAsync(request);
            return order;
        } catch (Exception ex) {
            // Enriched automatically by middleware
            logger.LogException(ex);
            throw;
        }
    }
}
```

**Benefits:**
- Track requests across async boundaries
- Correlate errors with user sessions
- Add contextual data to exceptions
- Structured logging support
- Production debugging easier

### Follow-up
- How do you handle distributed tracing?
- How do you integrate with monitoring tools?
- What about correlation across services?

---

## Question 6: Analyze and Fix Complex Exception Scenario

### Question
Review this problematic code and explain all issues and improvements needed.

```csharp
public class DataProcessor {
    public void ProcessFile(string path) {
        StreamReader reader = new StreamReader(path);
        try {
            while (!reader.EndOfStream) {
                string line = reader.ReadLine();
                ProcessLine(line);
            }
        } catch (Exception) {
            // Silently ignore
        }
    }
    
    private void ProcessLine(string line) {
        int value = int.Parse(line);  // May throw
        if (value > 0) {
            SaveToDatabase(value);
        }
    }
    
    private void SaveToDatabase(int value) {
        // May throw exception
        database.Save(value);
    }
}
```

### Answer

**Issues Found:**
1. Empty catch block - exception swallowed silently
2. Resource leak - reader never disposed
3. No guard clauses - bad data passes through
4. Too broad try block - can't identify failure
5. No logging - impossible to debug
6. Wrong exception type - catches all

**Improved Version:**
```csharp
public class DataProcessor {
    private readonly ILogger logger;
    private readonly IDatabase database;
    
    public void ProcessFile(string path) {
        // Guard clause
        if (string.IsNullOrEmpty(path)) {
            throw new ArgumentNullException(nameof(path));
        }
        
        if (!File.Exists(path)) {
            throw new FileNotFoundException($"File not found: {path}");
        }
        
        // Use 'using' for guaranteed cleanup
        using (var reader = new StreamReader(path)) {
            try {
                while (!reader.EndOfStream) {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }
                    
                    try {
                        ProcessLine(line);
                    } catch (ValidationException ex) {
                        logger.Warn($"Validation error on line: {ex.Message}");
                        // Continue processing other lines
                    }
                }
            } catch (IOException ex) {
                logger.Error("Read error", ex);
                throw new DataProcessingException("Failed to read file", ex);
            }
        }
    }
    
    private void ProcessLine(string line) {
        // Validate input
        if (!int.TryParse(line, out int value)) {
            throw new ValidationException(
                $"Invalid number format: {line}",
                line
            );
        }
        
        // Guard clause
        if (value <= 0) {
            throw new ValidationException(
                "Value must be positive",
                value
            );
        }
        
        SaveToDatabase(value);
    }
    
    private void SaveToDatabase(int value) {
        try {
            database.Save(value);
        } catch (SqlException ex) {
            logger.Error($"Database error saving {value}", ex);
            throw new DataAccessException(
                "Failed to save data",
                ex
            );
        }
    }
}
```

**Improvements:**
- ✓ Specific exception types
- ✓ Using statement for cleanup
- ✓ Guard clauses for validation
- ✓ Proper logging
- ✓ Narrow try blocks
- ✓ Custom domain exceptions
- ✓ Appropriate error recovery

### Follow-up
- How would you add retry logic?
- How would you add performance monitoring?
- How would you structure this in production?

---

## Question 7: Design Testable Exception Handling

### Question
How do you write testable exception handling code? Give an example.

### Answer

**Dependency Injection for Testability:**
```csharp
// Abstraction for exception handling
public interface IExceptionHandler {
    void Handle(Exception ex);
    bool CanHandle(Exception ex);
}

public class LoggingExceptionHandler : IExceptionHandler {
    private readonly ILogger logger;
    
    public LoggingExceptionHandler(ILogger logger) {
        this.logger = logger;
    }
    
    public void Handle(Exception ex) {
        logger.Error("Exception occurred", ex);
    }
    
    public bool CanHandle(Exception ex) {
        return true;  // Can handle any exception
    }
}

// Service with injectable handler
public class DataService {
    private readonly IExceptionHandler handler;
    private readonly ILogger logger;
    
    public DataService(IExceptionHandler handler, ILogger logger) {
        this.handler = handler;
        this.logger = logger;
    }
    
    public void ProcessData(string data) {
        try {
            ValidateData(data);
            SaveData(data);
        } catch (Exception ex) {
            handler.Handle(ex);
            throw;
        }
    }
}

// Tests
public class DataServiceTests {
    private readonly Mock<IExceptionHandler> mockHandler;
    private readonly Mock<ILogger> mockLogger;
    private DataService service;
    
    [SetUp]
    public void Setup() {
        mockHandler = new Mock<IExceptionHandler>();
        mockLogger = new Mock<ILogger>();
        service = new DataService(mockHandler.Object, mockLogger.Object);
    }
    
    [Test]
    public void ProcessData_WithValidData_Succeeds() {
        // Arrange
        var data = "valid data";
        
        // Act
        service.ProcessData(data);
        
        // Assert
        mockHandler.Verify(h => h.Handle(It.IsAny<Exception>()), Times.Never);
    }
    
    [Test]
    public void ProcessData_WithInvalidData_CallsHandler() {
        // Arrange
        var data = "";  // Invalid
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.ProcessData(data));
        mockHandler.Verify(h => h.Handle(It.IsAny<ArgumentException>()), Times.Once);
    }
    
    [Test]
    public void ProcessData_OnException_RethrowsAfterHandling() {
        // Arrange
        var data = "trigger error";
        mockHandler.Setup(h => h.Handle(It.IsAny<Exception>()))
            .Callback<Exception>(ex => {
                Assert.That(ex, Is.TypeOf<DataAccessException>());
            });
        
        // Act & Assert
        Assert.Throws<DataAccessException>(() => service.ProcessData(data));
        mockHandler.Verify(h => h.Handle(It.IsAny<Exception>()), Times.Once);
    }
}
```

**Testing Exception Scenarios:**
```csharp
[TestFixture]
public class ExceptionHandlingTests {
    [Test]
    public void GuardClause_WithNull_ThrowsArgumentNullException() {
        var service = new UserService();
        
        Assert.Throws<ArgumentNullException>(() =>
            service.ValidateUser(null)
        );
    }
    
    [Test]
    public void ExceptionHandling_PreservesStackTrace() {
        var service = new DataService();
        
        Exception caughtException = null;
        try {
            service.ProcessData("bad");
        } catch (Exception ex) {
            caughtException = ex;
            
            // Verify stack trace includes origin
            Assert.That(caughtException.StackTrace, Contains.Substring("ProcessData"));
        }
    }
    
    [Test]
    public void Disposal_WithException_StillCleansUp() {
        var resource = new TestResource();
        
        Assert.Throws<InvalidOperationException>(() => {
            using (resource) {
                throw new InvalidOperationException();
            }
        });
        
        Assert.That(resource.IsDisposed, Is.True);
    }
}
```

### Follow-up
- How do you mock exception scenarios?
- What assertions should you make?
- How do you test logging?

---

## Summary

**Hard Level Key Points:**
- Production architecture with logging/monitoring
- Async-safe exception handling patterns
- Resilience patterns (retry, circuit breaker)
- Complex disposal hierarchies
- Exception context and correlation
- Code analysis and problem solving
- Testable exception handling design

**Interview Success Tips:**
1. Think about production requirements
2. Consider scalability and monitoring
3. Show knowledge of patterns
4. Discuss trade-offs
5. Ask clarifying questions
6. Explain your reasoning

**Next Steps:**
1. Study real-world code
2. Implement patterns in projects
3. Practice system design
4. Review production systems
5. Stay current with best practices

