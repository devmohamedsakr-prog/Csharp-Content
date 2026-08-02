# Advanced Async Patterns

## Overview
Advanced patterns for handling complex asynchronous scenarios and improving code quality.

## Async Factory Pattern

### Creating Async Initialization
```csharp
public class DatabaseConnection
{
    private string _connectionString;
    private bool _isConnected;
    
    private DatabaseConnection() { }
    
    // Async factory method
    public static async Task<DatabaseConnection> CreateAsync(string connectionString)
    {
        var connection = new DatabaseConnection { _connectionString = connectionString };
        await connection.InitializeAsync();
        return connection;
    }
    
    private async Task InitializeAsync()
    {
        // Perform async initialization
        await Task.Delay(100); // Simulate connection
        _isConnected = true;
    }
    
    public async Task ExecuteAsync(string query)
    {
        if (!_isConnected) throw new InvalidOperationException("Not connected");
        await Task.Delay(100); // Simulate query
    }
}

// Usage
var connection = await DatabaseConnection.CreateAsync("connection-string");
await connection.ExecuteAsync("SELECT * FROM Users");
```

## Async IDisposable

### Resource Management
```csharp
public class AsyncResource : IAsyncDisposable
{
    private HttpClient _client;
    
    public AsyncResource()
    {
        _client = new HttpClient();
    }
    
    public async Task<string> FetchAsync(string url)
    {
        return await _client.GetStringAsync(url);
    }
    
    // Cleanup async resources
    public async ValueTask DisposeAsync()
    {
        _client?.Dispose();
        await Task.CompletedTask;
    }
}

// Usage
await using var resource = new AsyncResource();
string data = await resource.FetchAsync("https://api.example.com");
// Automatically disposed asynchronously
```

## Async Iterators

### Async Enumerables
```csharp
// Returns IAsyncEnumerable<T>
public async IAsyncEnumerable<string> FetchDataAsync(
    [EnumeratorCancellation] CancellationToken cancellationToken)
{
    for (int i = 0; i < 10; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // Each iteration can await
        string data = await FetchItemAsync(i);
        yield return data;
    }
}

// Usage with await foreach
await foreach (var item in FetchDataAsync(cancellationToken))
{
    Console.WriteLine(item);
}

// Break out early (cancellation)
var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(5));
try
{
    await foreach (var item in FetchDataAsync(cts.Token))
    {
        Console.WriteLine(item);
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Enumeration cancelled");
}
```

## Progress Reporting

### IProgress<T>
```csharp
public class DownloadService
{
    public async Task DownloadAsync(string url, IProgress<DownloadProgress> progress)
    {
        using var client = new HttpClient();
        
        var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        
        var totalBytes = response.Content.Headers.ContentLength ?? -1L;
        long bytesRead = 0;
        
        using var stream = await response.Content.ReadAsStreamAsync();
        var buffer = new byte[8192];
        
        int read;
        while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            bytesRead += read;
            
            // Report progress
            progress?.Report(new DownloadProgress
            {
                BytesRead = bytesRead,
                TotalBytes = totalBytes,
                PercentComplete = totalBytes > 0 ? (int)(bytesRead * 100 / totalBytes) : 0
            });
        }
    }
}

public class DownloadProgress
{
    public long BytesRead { get; set; }
    public long TotalBytes { get; set; }
    public int PercentComplete { get; set; }
}

// Usage
var service = new DownloadService();
var progress = new Progress<DownloadProgress>(p =>
{
    Console.WriteLine($"Downloaded {p.PercentComplete}%");
});

await service.DownloadAsync("https://example.com/file.bin", progress);
```

## Retry Pattern

### Exponential Backoff
```csharp
public static class AsyncRetry
{
    public static async Task<T> RetryAsync<T>(
        Func<Task<T>> operation,
        int maxAttempts = 3,
        TimeSpan? initialDelay = null)
    {
        initialDelay ??= TimeSpan.FromMilliseconds(100);
        
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                var delay = TimeSpan.FromMilliseconds(
                    initialDelay.Value.TotalMilliseconds * Math.Pow(2, attempt - 1));
                
                Console.WriteLine($"Attempt {attempt} failed, retrying after {delay.TotalMilliseconds}ms");
                await Task.Delay(delay);
            }
        }
        
        throw new InvalidOperationException("All retry attempts failed");
    }
}

// Usage
var result = await AsyncRetry.RetryAsync(
    () => FetchDataAsync("https://api.example.com"),
    maxAttempts: 3,
    initialDelay: TimeSpan.FromMilliseconds(100)
);
```

## Timeout Pattern

### With CancellationToken
```csharp
public static async Task<T> WithTimeoutAsync<T>(
    Task<T> task,
    TimeSpan timeout)
{
    var cts = new CancellationTokenSource(timeout);
    try
    {
        return await task;
    }
    catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
    {
        throw new TimeoutException($"Operation timed out after {timeout.TotalSeconds} seconds");
    }
}

// Usage
try
{
    var result = await WithTimeoutAsync(
        FetchDataAsync("https://api.example.com"),
        TimeSpan.FromSeconds(5)
    );
}
catch (TimeoutException ex)
{
    Console.WriteLine(ex.Message);
}
```

## Circuit Breaker Pattern

### Preventing Cascading Failures
```csharp
public class CircuitBreaker
{
    private int _failureCount = 0;
    private DateTime _lastFailureTime = DateTime.MinValue;
    private const int FailureThreshold = 3;
    private const int ResetTimeoutSeconds = 60;
    
    public enum State { Closed, Open, HalfOpen }
    public State CurrentState { get; private set; } = State.Closed;
    
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        if (CurrentState == State.Open)
        {
            if (DateTime.UtcNow - _lastFailureTime > TimeSpan.FromSeconds(ResetTimeoutSeconds))
            {
                CurrentState = State.HalfOpen;
            }
            else
            {
                throw new InvalidOperationException("Circuit breaker is open");
            }
        }
        
        try
        {
            var result = await operation();
            
            if (CurrentState == State.HalfOpen)
            {
                _failureCount = 0;
                CurrentState = State.Closed;
            }
            
            return result;
        }
        catch (Exception)
        {
            _failureCount++;
            _lastFailureTime = DateTime.UtcNow;
            
            if (_failureCount >= FailureThreshold)
            {
                CurrentState = State.Open;
            }
            
            throw;
        }
    }
}

// Usage
var breaker = new CircuitBreaker();

try
{
    var result = await breaker.ExecuteAsync(() => FetchDataAsync());
}
catch (InvalidOperationException ex) when (ex.Message.Contains("Circuit breaker"))
{
    Console.WriteLine("Service temporarily unavailable");
}
```

## Best Practices

1. **Always Use Async Factory When Async Initialization Needed**
```csharp
// Good: Async initialization pattern
public class Service
{
    private Service() { }
    public static async Task<Service> CreateAsync()
    {
        var service = new Service();
        await service.InitializeAsync();
        return service;
    }
    private async Task InitializeAsync() { /* ... */ }
}
```

2. **Use ValueTask for Small/Fast Operations**
```csharp
// Good: Reduces allocation for sync completions
public ValueTask<string> GetCachedAsync(string key)
{
    if (_cache.TryGetValue(key, out var value))
        return new ValueTask<string>(value); // No allocation
    
    return new ValueTask<string>(FetchAsync(key)); // Allocation only if needed
}
```

3. **Always Report Progress for Long Operations**
```csharp
// Good: Users know operation is progressing
public async Task<File> DownloadAsync(string url, IProgress<ProgressReport> progress)
{
    // Report incremental progress
    progress?.Report(new ProgressReport { PercentComplete = 50 });
}
```

## Common Mistakes

1. **Not Handling Cancellation**
```csharp
// Bad: Ignores cancellation
public async Task LongOperationAsync()
{
    for (int i = 0; i < 100; i++)
    {
        await Task.Delay(100);
    }
}

// Good: Respect cancellation
public async Task LongOperationAsync(CancellationToken cancellationToken)
{
    for (int i = 0; i < 100; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await Task.Delay(100, cancellationToken);
    }
}
```

2. **Forgetting to Dispose IAsyncDisposable**
```csharp
// Bad: Resource leaked
var resource = new AsyncResource();
await resource.UseAsync();

// Good: Use await using
await using var resource = new AsyncResource();
await resource.UseAsync();
```

## Quick Summary
- Async factory pattern for async initialization
- IAsyncDisposable for async cleanup
- Async iterators with await foreach
- IProgress for progress reporting
- Retry pattern with exponential backoff
- Circuit breaker prevents cascading failures
- ValueTask for sync-path optimization
- CancellationToken always included

## Resources
- Async Patterns documentation
- Task-based Asynchronous Pattern (TAP)
- Best practices for async/await
