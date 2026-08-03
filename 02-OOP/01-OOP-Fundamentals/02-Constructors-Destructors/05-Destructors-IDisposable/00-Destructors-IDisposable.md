# Destructors and IDisposable Pattern

## Overview

Destructors clean up resources when objects are destroyed. However, destructors run at unpredictable times. The IDisposable pattern provides deterministic cleanup for managed and unmanaged resources.

## Destructors (Finalizers)

### What is a Destructor?

A destructor (~ClassName):
- Runs when an object is garbage collected
- Has the same name as class, prefixed with ~
- No parameters or return type
- Runs at unpredictable times
- Used for cleanup

```csharp
public class FileHandler
{
    private FileStream _file;
    
    public FileHandler(string path)
    {
        _file = new FileStream(path, FileMode.Open);
        Console.WriteLine("File opened");
    }
    
    // Destructor - runs when GC collects the object
    ~FileHandler()
    {
        Console.WriteLine("Destructor called");
        _file?.Close();  // Cleanup
    }
}

// Usage
var handler = new FileHandler("file.txt");
// When handler goes out of scope and GC runs, destructor is called
```

**Important:** Destructors are not reliable for timely cleanup because garbage collection is unpredictable.

## IDisposable Pattern - Recommended

Use IDisposable for deterministic (immediate) cleanup:

```csharp
public class DatabaseConnection : IDisposable
{
    private SqlConnection _connection;
    private bool _disposed = false;
    
    public DatabaseConnection(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        _connection.Open();
        Console.WriteLine("Connection opened");
    }
    
    public void ExecuteQuery(string query)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(DatabaseConnection));
        
        using (SqlCommand cmd = new SqlCommand(query, _connection))
        {
            cmd.ExecuteNonQuery();
        }
    }
    
    // Public Dispose - called by user or using statement
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);  // Tell GC to skip finalizer
    }
    
    // Protected virtual Dispose - allows subclasses to override
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Managed resources
                _connection?.Close();
                _connection?.Dispose();
            }
            // Unmanaged resources here (if any)
            _disposed = true;
        }
    }
    
    // Finalizer as safety net
    ~DatabaseConnection()
    {
        Dispose(false);
    }
}

// Usage with using statement - Dispose called automatically
using (var connection = new DatabaseConnection("connection-string"))
{
    connection.ExecuteQuery("SELECT * FROM Users");
}  // Dispose called here
```

## Using Statement

The `using` statement calls Dispose automatically:

```csharp
public class FileWriter : IDisposable
{
    private StreamWriter _writer;
    
    public FileWriter(string path)
    {
        _writer = new StreamWriter(path);
    }
    
    public void WriteLine(string text)
    {
        _writer.WriteLine(text);
    }
    
    public void Dispose()
    {
        _writer?.Dispose();
    }
}

// Usage - Dispose called automatically at end of using block
using (var writer = new FileWriter("output.txt"))
{
    writer.WriteLine("Hello");
    writer.WriteLine("World");
}  // Dispose() called automatically
```

## Declaration Pattern (C# 8+)

Simpler syntax for using:

```csharp
public class ResourceHandler : IDisposable
{
    private StreamReader _reader;
    
    public ResourceHandler(string path)
    {
        _reader = new StreamReader(path);
    }
    
    public string ReadLine()
    {
        return _reader?.ReadLine();
    }
    
    public void Dispose()
    {
        _reader?.Dispose();
    }
}

// Usage - simpler using syntax
using var reader = new ResourceHandler("input.txt");
string line = reader.ReadLine();
// Dispose() called automatically when out of scope
```

## IAsyncDisposable (Async Cleanup)

For asynchronous resource cleanup:

```csharp
public class AsyncHttpClient : IAsyncDisposable
{
    private HttpClient _client;
    
    public AsyncHttpClient()
    {
        _client = new HttpClient();
    }
    
    public async Task<string> FetchAsync(string url)
    {
        return await _client.GetStringAsync(url);
    }
    
    public async ValueTask DisposeAsync()
    {
        _client?.Dispose();
        await Task.CompletedTask;
    }
}

// Usage with await using (C# 8.0+)
await using var client = new AsyncHttpClient();
string data = await client.FetchAsync("https://api.example.com");
// DisposeAsync() called automatically
```

## Simple IDisposable Implementation

Minimal template for simple cases:

```csharp
public class SimpleResource : IDisposable
{
    private bool _disposed = false;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Clean managed resources
            }
            _disposed = true;
        }
    }
    
    ~SimpleResource()
    {
        Dispose(false);
    }
}
```

## Best Practices

### Always Use Using or Using Declaration

```csharp
// Good - Dispose guaranteed
using (var resource = new FileHandler("file.txt"))
{
    // Use resource
}  // Disposed automatically

// Also good (C# 8+)
using var resource = new FileHandler("file.txt");
// Disposed automatically when out of scope
```

### Never Forget GC.SuppressFinalize

```csharp
// Good - Suppress finalizer after Dispose
public void Dispose()
{
    CleanUpResources();
    GC.SuppressFinalize(this);
}

// Bad - Finalizer still runs
public void Dispose()
{
    CleanUpResources();
    // Forgot GC.SuppressFinalize(this);
}
```

### Check for Disposal Before Use

```csharp
public class Resource : IDisposable
{
    private bool _disposed = false;
    
    public void DoSomething()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(Resource));
        
        // Do work
    }
    
    public void Dispose()
    {
        _disposed = true;
    }
}
```

## Destructor vs IDisposable

| Aspect | Destructor | IDisposable |
|--------|-----------|------------|
| Timing | Unpredictable (GC) | Immediate (user control) |
| Reliability | Not guaranteed | Guaranteed with using |
| Performance | Can delay cleanup | Immediate cleanup |
| Best for | Safety net | Primary cleanup |

## Summary

- **Destructor** - Safety net, unpredictable timing
- **IDisposable** - Deterministic cleanup pattern
- **Using statement** - Calls Dispose automatically
- **GC.SuppressFinalize** - Prevent finalizer from running
- **Check _disposed** - Prevent use after disposal
- **Prefer IDisposable** - Over destructors for cleanup

## Next Steps

- Learn [Instance-Constructors](../01-Instance-Constructors/00-Instance-Constructors.md) for initialization
- Study [Constructor-Chaining](../02-Constructor-Chaining/00-Constructor-Chaining.md) with base classes
- Review [Initialization-Order](../06-Initialization-Order/00-Initialization-Order.md) for lifecycle
