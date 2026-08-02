# Singleton Pattern

## Overview
Singleton ensures a class has only one instance and provides a global access point to it.

## Implementation

### Thread-Safe Singleton
```csharp
public sealed class Logger
{
    private static readonly Logger _instance = new Logger();
    
    // Private constructor prevents instantiation
    private Logger() { }
    
    public static Logger Instance => _instance;
    
    public void Log(string message) => Console.WriteLine(message);
}

// Usage
Logger.Instance.Log("Hello");
Logger.Instance.Log("World");
// Both calls use same instance
```

### Lazy<T> Pattern
```csharp
public sealed class Logger
{
    private static readonly Lazy<Logger> _instance = 
        new Lazy<Logger>(() => new Logger());
    
    private Logger() { }
    
    public static Logger Instance => _instance.Value;
    
    public void Log(string message) => Console.WriteLine(message);
}
```

### Thread-Safe with Double-Check Locking
```csharp
public sealed class Logger
{
    private static Logger _instance;
    private static readonly object _lock = new object();
    
    private Logger() { }
    
    public static Logger Instance
    {
        get
        {
            // First check (no lock)
            if (_instance != null)
                return _instance;
            
            // Lock only if needed
            lock (_lock)
            {
                // Second check (with lock)
                if (_instance == null)
                    _instance = new Logger();
            }
            
            return _instance;
        }
    }
}
```

## Real-World Examples

### Logger Singleton
```csharp
public sealed class Logger
{
    private static readonly Lazy<Logger> _instance = 
        new Lazy<Logger>(() => new Logger());
    
    public static Logger Instance => _instance.Value;
    
    private Logger() { }
    
    public void Info(string message) => Console.WriteLine($"[INFO] {message}");
    public void Error(string message) => Console.WriteLine($"[ERROR] {message}");
    public void Warning(string message) => Console.WriteLine($"[WARN] {message}");
}

// Usage
Logger.Instance.Info("Application started");
Logger.Instance.Error("An error occurred");
```

### Database Connection Pool
```csharp
public sealed class ConnectionPool
{
    private static readonly Lazy<ConnectionPool> _instance = 
        new Lazy<ConnectionPool>(() => new ConnectionPool());
    
    public static ConnectionPool Instance => _instance.Value;
    
    private readonly Queue<DbConnection> _availableConnections;
    private readonly int _maxConnections = 10;
    
    private ConnectionPool()
    {
        _availableConnections = new Queue<DbConnection>(_maxConnections);
        InitializeConnections();
    }
    
    private void InitializeConnections()
    {
        for (int i = 0; i < _maxConnections; i++)
        {
            _availableConnections.Enqueue(new DbConnection());
        }
    }
    
    public DbConnection GetConnection()
    {
        lock (_availableConnections)
        {
            if (_availableConnections.Count == 0)
                throw new InvalidOperationException("No connections available");
            
            return _availableConnections.Dequeue();
        }
    }
    
    public void ReleaseConnection(DbConnection connection)
    {
        lock (_availableConnections)
        {
            _availableConnections.Enqueue(connection);
        }
    }
}

// Usage
var connection = ConnectionPool.Instance.GetConnection();
try
{
    // Use connection
}
finally
{
    ConnectionPool.Instance.ReleaseConnection(connection);
}
```

### Configuration Manager
```csharp
public sealed class ConfigManager
{
    private static readonly Lazy<ConfigManager> _instance = 
        new Lazy<ConfigManager>(() => new ConfigManager());
    
    public static ConfigManager Instance => _instance.Value;
    
    private readonly Dictionary<string, string> _config;
    
    private ConfigManager()
    {
        _config = LoadConfiguration();
    }
    
    private Dictionary<string, string> LoadConfiguration()
    {
        // Load from appsettings.json, environment variables, etc.
        return new Dictionary<string, string>
        {
            { "ConnectionString", "Server=localhost;Database=MyDb" },
            { "ApiUrl", "https://api.example.com" }
        };
    }
    
    public string Get(string key) => _config.TryGetValue(key, out var value) ? value : null;
}

// Usage
string connectionString = ConfigManager.Instance.Get("ConnectionString");
```

## Advantages

- Single instance ensures centralized resource management
- Global access point
- Lazy initialization possible
- Thread-safe implementation available
- Reduced memory usage (single instance)

## Disadvantages

- Hard to test (tight coupling to singleton)
- Can mask poor design
- Thread-safe implementation adds complexity
- Makes dependencies implicit
- Violates Single Responsibility Principle

## Testing Singleton

### Dependency Injection Alternative
```csharp
// Better approach: Use DI instead of Singleton
public interface ILogger
{
    void Log(string message);
}

public class Logger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}

// Register as singleton in DI container
services.AddSingleton<ILogger, Logger>();

// Testable: Can inject mock
public class MyService
{
    private readonly ILogger _logger;
    
    public MyService(ILogger logger)
    {
        _logger = logger;
    }
}

// Unit test
[Fact]
public void MyTest()
{
    var mockLogger = new Mock<ILogger>();
    var service = new MyService(mockLogger.Object);
    
    // Test service
    mockLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Once);
}
```

## Best Practices

1. **Use Thread-Safe Implementation**
```csharp
// Good: Thread-safe Lazy<T>
private static readonly Lazy<MyService> _instance = 
    new Lazy<MyService>(() => new MyService());
```

2. **Prefer Dependency Injection**
```csharp
// Good: Use DI instead of static Instance
services.AddSingleton<IMyService, MyService>();

// In constructor
public MyClass(IMyService service) { _service = service; }
```

3. **Seal the Class**
```csharp
// Good: Prevent inheritance
public sealed class SingletonService { }

// Bad: Can be subclassed
public class SingletonService { }
```

## Common Mistakes

1. **Not Thread-Safe**
```csharp
// Bad: Race condition
private static Logger _instance;

public static Logger Instance
{
    get
    {
        if (_instance == null)
            _instance = new Logger(); // Two threads might create two instances
        return _instance;
    }
}

// Good: Use Lazy<T>
private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
```

2. **Public Constructor**
```csharp
// Bad: Instance can be created elsewhere
public class Logger
{
    public Logger() { } // Public!
    
    public static Logger Instance { get; } = new Logger();
}

// Good: Private constructor
public class Logger
{
    private Logger() { }
    
    public static Logger Instance { get; } = new Logger();
}
```

3. **Hard Testing**
```csharp
// Bad: Can't mock singleton
public class UserService
{
    private Logger Logger => Logger.Instance; // Hard-coded
}

// Good: Inject interface
public class UserService
{
    private readonly ILogger _logger;
    
    public UserService(ILogger logger) => _logger = logger;
}
```

## Quick Summary
- Single instance created once
- Thread-safe with Lazy<T>
- Private constructor prevents instantiation
- Global access via static property
- Prefer dependency injection over singleton
- Seal class to prevent inheritance
- Hard to test - consider alternatives
- Good for stateless services

## Resources
- Singleton Pattern (Design Patterns)
- When to use Singleton
- Alternatives to Singleton
