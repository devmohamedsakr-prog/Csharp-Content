# Connection Management and Connection Pooling

## Overview
Database connection pooling, connection strings, lifecycle management, and performance optimization.

## Connection Strings

### Configuration
```csharp
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyDb;User Id=sa;Password=password;",
    "ReadOnlyConnection": "Server=localhost;Database=MyDb;User Id=reader;Password=password;",
    "ReportConnection": "Server=report-server;Database=ReportDb;..."
  }
}

// Program.cs
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
```

### Connection String Parameters
```csharp
// SQL Server connection string components
public class ConnectionStringBuilder
{
    public void BuildConnectionString()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = "localhost",
            InitialCatalog = "MyDatabase",
            UserID = "sa",
            Password = "password",
            
            // Pooling options
            Pooling = true,
            MaxPoolSize = 100,
            MinPoolSize = 10,
            
            // Timeout settings
            ConnectTimeout = 30,
            
            // Application intent
            ApplicationIntent = ApplicationIntent.ReadOnly,
            
            // Encryption
            Encrypt = true,
            TrustServerCertificate = false
        };
        
        var connectionString = builder.ConnectionString;
    }
}
```

## Connection Pooling

### Default Pooling Behavior
```csharp
// Pooling is enabled by default
public class PoolingExample
{
    private readonly IDbConnectionFactory _factory;
    
    public async Task DemonstratePoolingAsync()
    {
        // Connection 1: New connection created
        using (var conn1 = await _factory.CreateConnectionAsync())
        {
            await conn1.OpenAsync();
            // Do work
        } // Connection returned to pool
        
        // Connection 2: Reuses connection from pool
        using (var conn2 = await _factory.CreateConnectionAsync())
        {
            await conn2.OpenAsync();
            // Same connection as conn1 reused
        } // Connection returned to pool
    }
}
```

### Pool Configuration
```csharp
// Program.cs
var connectionString = "Server=localhost;Database=MyDb;Max Pool Size=100;Min Pool Size=10;";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.MaxBatchSize(100);
        sqlOptions.CommandTimeout(30);
    });
});
```

### Pool Management
```csharp
public class PoolManagementService
{
    private readonly AppDbContext _context;
    
    // Clear pool when needed (e.g., after configuration change)
    public void ClearConnectionPool()
    {
        SqlConnection.ClearAllPools();
    }
    
    public void ClearSpecificPool(string connectionString)
    {
        SqlConnection.ClearPool(new SqlConnection(connectionString));
    }
}
```

## Connection Lifecycle

### DbContext Lifecycle
```csharp
public class DbContextLifecycleService
{
    private readonly AppDbContext _context;
    
    public async Task DemonstrateLifecycleAsync()
    {
        // DbContext is short-lived (per request in web apps)
        using var context = new AppDbContext();
        
        // Database connection acquired from pool on first use
        var user = await context.Users.FirstOrDefaultAsync();
        
        // Changes tracked
        user.Name = "Updated";
        await context.SaveChangesAsync();
        
        // Connection returned to pool when DbContext disposed
    }
}

// Program.cs - Scoped lifetime (recommended for web apps)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);
// DbContext instance per request, automatic disposal
```

### Explicit Connection Management
```csharp
public class ExplicitConnectionManagement
{
    public async Task ManageConnectionsAsync()
    {
        var connectionString = "Server=localhost;Database=MyDb;";
        
        using (var connection = new SqlConnection(connectionString))
        {
            // Connection acquired from pool, opened
            await connection.OpenAsync();
            
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Users";
                var count = await command.ExecuteScalarAsync();
            }
            
            // Connection closed, returned to pool
        }
    }
}
```

## Monitoring Connection Pool

### Health Monitoring
```csharp
public class ConnectionPoolMonitor
{
    private readonly IDbConnectionFactory _factory;
    private readonly ILogger<ConnectionPoolMonitor> _logger;
    
    public async Task MonitorPoolAsync()
    {
        // Simulate connections
        var connections = new List<DbConnection>();
        
        try
        {
            // Create multiple connections
            for (int i = 0; i < 50; i++)
            {
                var conn = await _factory.CreateConnectionAsync();
                await conn.OpenAsync();
                connections.Add(conn);
            }
            
            _logger.LogInformation("Active pool connections: {Count}", connections.Count);
        }
        finally
        {
            // Return to pool
            foreach (var conn in connections)
            {
                conn?.Dispose();
            }
        }
    }
}
```

### Connection Pool Metrics
```csharp
public class PoolMetricsService
{
    private readonly AppDbContext _context;
    private readonly ILogger<PoolMetricsService> _logger;
    
    public async Task LogPoolMetricsAsync()
    {
        // Monitor connection pool behavior
        var metrics = new Dictionary<string, object>
        {
            { "Timestamp", DateTime.UtcNow },
            { "PoolSize", GetPoolSize() },
            { "ActiveConnections", GetActiveConnectionCount() }
        };
        
        _logger.LogInformation("Pool metrics: {@Metrics}", metrics);
    }
    
    private int GetPoolSize() => 0; // Implementation
    private int GetActiveConnectionCount() => 0; // Implementation
}
```

## Read Replicas and Multiple Databases

### Multiple Connections
```csharp
// Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddDbContext<ReadOnlyDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ReadOnlyConnection")
    )
);
```

### Selective Connection Usage
```csharp
public class DataAccessService
{
    private readonly AppDbContext _writeContext;
    private readonly ReadOnlyDbContext _readContext;
    
    public DataAccessService(
        AppDbContext writeContext,
        ReadOnlyDbContext readContext)
    {
        _writeContext = writeContext;
        _readContext = readContext;
    }
    
    // Read-heavy operations use read replica
    public async Task<List<User>> GetUsersAsync()
    {
        return await _readContext.Users.ToListAsync();
    }
    
    // Write operations use primary
    public async Task UpdateUserAsync(User user)
    {
        _writeContext.Users.Update(user);
        await _writeContext.SaveChangesAsync();
    }
}
```

## Best Practices

1. **Keep DbContext Short-Lived**
```csharp
// Good: Per-request scope in web apps
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString)
);

// Usage in controller
public class UsersController
{
    private readonly AppDbContext _context; // Fresh instance per request
    
    public UsersController(AppDbContext context) => _context = context;
}

// Bad: Static or long-lived DbContext
public static class GlobalContext
{
    public static AppDbContext Context { get; set; } // Thread-safety issues!
}
```

2. **Configure Pool Appropriately**
```csharp
// Good: Tune pool size based on workload
var connectionString = "Server=localhost;Database=MyDb;" +
    "Max Pool Size=100;Min Pool Size=10;Pooling=true;";

// Bad: Default pool size may be too small
var connectionString = "Server=localhost;Database=MyDb;";
```

3. **Handle Connection Failures**
```csharp
public class ResilientConnectionService
{
    private readonly ILogger<ResilientConnectionService> _logger;
    
    public async Task<T> ExecuteWithRetryAsync<T>(
        Func<SqlConnection, Task<T>> operation)
    {
        const int maxRetries = 3;
        int retryCount = 0;
        
        while (retryCount < maxRetries)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return await operation(connection);
            }
            catch (SqlException ex) when (IsTransientError(ex))
            {
                retryCount++;
                if (retryCount >= maxRetries) throw;
                
                var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
                _logger.LogWarning("Connection failed, retrying in {Delay}", delay);
                await Task.Delay(delay);
            }
        }
        
        throw new InvalidOperationException("Max retries exceeded");
    }
    
    private bool IsTransientError(SqlException ex)
    {
        // Transient error numbers: 40197, 40501, 40613, 64, 233, 20, 64
        return ex.Number == 40197 || ex.Number == 40501;
    }
}
```

## Common Mistakes

1. **Not Disposing DbContext**
```csharp
// Bad: Resource leak
public void GetUsers()
{
    var context = new AppDbContext();
    var users = context.Users.ToList();
    // context not disposed!
}

// Good: Always dispose
public void GetUsers()
{
    using var context = new AppDbContext();
    var users = context.Users.ToList();
} // Automatic disposal
```

2. **Sharing DbContext Across Requests**
```csharp
// Bad: DbContext shared across multiple requests
public class UserService
{
    private static AppDbContext _context = new();
    
    public void UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}

// Good: Injected per-request
public class UserService
{
    private readonly AppDbContext _context;
    
    public UserService(AppDbContext context) => _context = context;
}
```

3. **Disabling Pooling Without Reason**
```csharp
// Bad: Pooling disabled, creates new connection each time
var connectionString = "Server=localhost;Database=MyDb;Pooling=false;";

// Good: Keep pooling enabled
var connectionString = "Server=localhost;Database=MyDb;Pooling=true;";
```

## Quick Summary
- Connection pooling: Reuse connections efficiently
- Keep DbContext short-lived (scoped per request)
- Configure pool size appropriately
- Use connection strings with parameters
- Monitor pool metrics
- Handle transient connection errors with retry logic
- Dispose DbContext and connections properly
- Separate read/write connections when needed
- Avoid sharing DbContext across requests
- Enable connection pooling by default

## Resources
- Entity Framework Core Connection Strings
- SQL Server Connection Pooling
- DbContext Lifetime Configuration
- Connection Resilience Patterns
