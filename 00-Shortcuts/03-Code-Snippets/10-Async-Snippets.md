# Async & Await Snippets

Generate asynchronous code with built-in snippets.

## async - Async Method

**Shortcut:** `async` + Tab (in some IDEs)

**Pattern:**
```csharp
public async Task<T> GetDataAsync()
{
    return await SomeAsyncOperation();
}
```

**Usage:**
```csharp
// Type async before method return type
public async Task FetchDataAsync()
{
    await Task.Delay(1000);
}
```

---

## await - Await Operation

**Shortcut:** `await` + Tab (in some IDEs)

**Pattern:**
```csharp
public async Task ProcessAsync()
{
    string result = await GetDataAsync();
    Console.WriteLine(result);
}
```

---

## Async Task (No Return Value)

**Pattern:**
```csharp
public async Task ProcessAsync()
{
    await Task.Delay(1000);
    Console.WriteLine("Done");
}
```

**Usage:**
```csharp
await ProcessAsync();
```

---

## Async Task<T> (With Return Value)

**Pattern:**
```csharp
public async Task<string> FetchDataAsync()
{
    using var client = new HttpClient();
    var response = await client.GetAsync("https://api.example.com/data");
    return await response.Content.ReadAsStringAsync();
}
```

**Usage:**
```csharp
string data = await FetchDataAsync();
Console.WriteLine(data);
```

---

## Async Method with Parameters

**Pattern:**
```csharp
public async Task<int> DownloadFileAsync(string url, string path)
{
    using var client = new HttpClient();
    var bytes = await client.GetByteArrayAsync(url);
    await File.WriteAllBytesAsync(path, bytes);
    return bytes.Length;
}
```

**Usage:**
```csharp
int size = await DownloadFileAsync("https://example.com/file.zip", "local.zip");
```

---

## Task.Run - Offload to Thread Pool

**Pattern:**
```csharp
public async Task ProcessAsync()
{
    var result = await Task.Run(() =>
    {
        // Long-running CPU-bound operation
        return ExpensiveCalculation();
    });
}

private int ExpensiveCalculation()
{
    // This runs on thread pool, not UI thread
    return Fibonacci(35);
}
```

---

## Multiple Awaits

**Sequential:**
```csharp
public async Task ProcessSequentialAsync()
{
    var data1 = await FetchData1Async();
    var data2 = await FetchData2Async();
    var data3 = await FetchData3Async();
    
    Combine(data1, data2, data3);
}
```

**Parallel:**
```csharp
public async Task ProcessParallelAsync()
{
    var task1 = FetchData1Async();
    var task2 = FetchData2Async();
    var task3 = FetchData3Async();
    
    await Task.WhenAll(task1, task2, task3);
    
    Combine(task1.Result, task2.Result, task3.Result);
}
```

---

## Task.WhenAll - Wait for Multiple Tasks

**Pattern:**
```csharp
public async Task DownloadAllAsync(List<string> urls)
{
    var tasks = urls.Select(url => DownloadAsync(url)).ToList();
    var results = await Task.WhenAll(tasks);
    return results;
}
```

---

## Task.WhenAny - First to Complete

**Pattern:**
```csharp
public async Task<string> FetchFastestAsync(List<string> urls)
{
    var tasks = urls.Select(url => FetchAsync(url)).ToList();
    var firstCompleted = await Task.WhenAny(tasks);
    return await (Task<string>)firstCompleted;
}
```

---

## ConfigureAwait - UI vs Non-UI

**Pattern:**
```csharp
// In UI app (preserve context)
public async Task UIMethodAsync()
{
    var data = await FetchDataAsync();  // Returns to UI thread
    UpdateUI(data);
}

// In library (don't capture context)
public async Task LibraryMethodAsync()
{
    var data = await FetchDataAsync().ConfigureAwait(false);
    return Process(data);
}
```

---

## Async Void (Avoid)

**Pattern (NOT Recommended):**
```csharp
// Avoid this - only for event handlers
public async void ButtonClick_OnClick(object sender, EventArgs e)
{
    await ProcessAsync();
}
```

**Why avoid:** Can't await, exceptions not caught, no way to know when done

---

## Exception Handling in Async

**Pattern:**
```csharp
public async Task ProcessWithErrorHandlingAsync()
{
    try
    {
        var data = await FetchDataAsync();
        Process(data);
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Network error: {ex.Message}");
    }
    catch (IOException ex)
    {
        Console.WriteLine($"IO error: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}
```

---

## CancellationToken - Cancel Operations

**Pattern:**
```csharp
public async Task FetchWithCancellationAsync(CancellationToken cancellationToken)
{
    using var client = new HttpClient();
    var response = await client.GetAsync("https://api.example.com/data", cancellationToken);
    return await response.Content.ReadAsStringAsync();
}
```

**Usage:**
```csharp
var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(5));  // Cancel after 5 seconds

try
{
    var result = await FetchWithCancellationAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation cancelled");
}
```

---

## Async Iterator (C# 8+)

**Pattern:**
```csharp
public async IAsyncEnumerable<string> FetchLinesAsync(string url)
{
    using var client = new HttpClient();
    using var response = await client.GetAsync(url);
    using var stream = await response.Content.ReadAsStreamAsync();
    using var reader = new StreamReader(stream);
    
    string line;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        yield return line;
    }
}

// Usage
await foreach (var line in FetchLinesAsync(url))
{
    Console.WriteLine(line);
}
```

---

## ValueTask - Optimized for Sync Completion

**Pattern:**
```csharp
public class CachedRepository
{
    private Dictionary<int, User> _cache = new();
    
    public ValueTask<User> GetUserAsync(int id)
    {
        if (_cache.TryGetValue(id, out var user))
        {
            // Already cached - synchronous completion
            return new ValueTask<User>(user);
        }
        
        // Not cached - async fetch
        return new ValueTask<User>(FetchFromDatabaseAsync(id));
    }
}
```

---

## Async LINQ (System.Linq.Async)

**Pattern:**
```csharp
public async Task ProcessAsync(IAsyncEnumerable<User> users)
{
    var activeUsers = users
        .Where(u => u.IsActive)
        .Select(u => u.Name);
    
    await foreach (var name in activeUsers)
    {
        Console.WriteLine(name);
    }
}
```

---

## Quick Reference

| Type | Purpose |
|------|---------|
| `async Task` | Async operation, no return |
| `async Task<T>` | Async operation, returns T |
| `await` | Wait for async operation |
| `Task.WhenAll` | Wait for multiple tasks |
| `Task.WhenAny` | Wait for first task |
| `Task.Run` | Run on thread pool |
| `ConfigureAwait` | Control context |
| `CancellationToken` | Cancel operation |
| `ValueTask` | Optimized for sync completion |
| `IAsyncEnumerable` | Async iteration |

---

## Best Practices

- Use `async Task` or `async Task<T>`, never `async void` (except events)
- Use `ConfigureAwait(false)` in libraries
- Use `Task.WhenAll` for parallel awaits
- Always await async operations
- Use `CancellationToken` for cancellation
- Catch specific exceptions
- Test async code thoroughly
- Avoid `Task.Wait()` - use await instead

