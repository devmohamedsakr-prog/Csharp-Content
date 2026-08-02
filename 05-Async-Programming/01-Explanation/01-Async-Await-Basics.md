# Async/Await Basics

## Overview
Async/await enables asynchronous programming, allowing long-running operations without blocking threads.

## Core Concepts

### Synchronous vs Asynchronous
```csharp
// Synchronous: Blocks calling thread
public string FetchData()
{
    Thread.Sleep(2000); // Simulates I/O work
    return "Data";
}

var data = FetchData(); // Blocks for 2 seconds

// Asynchronous: Doesn't block thread
public async Task<string> FetchDataAsync()
{
    await Task.Delay(2000); // Simulates I/O work
    return "Data";
}

var task = FetchDataAsync(); // Returns immediately
var data = await task; // Waits for result
```

## Async Methods

### Method Signatures
```csharp
// Returns Task - no value returned
public async Task DoWorkAsync()
{
    await Task.Delay(1000);
    Console.WriteLine("Work done");
}

// Returns Task<T> - value returned
public async Task<int> GetCountAsync()
{
    await Task.Delay(1000);
    return 42;
}

// Void only for event handlers (dangerous for other uses)
private async void OnButtonClick()
{
    await DoWorkAsync();
}

// Calling async methods
await DoWorkAsync();
int count = await GetCountAsync();
```

## Await Operator

### What Await Does
```csharp
// Await unwraps Task/Task<T> and returns the result
public async Task<string> ProcessAsync()
{
    // Task<string> returned from DownloadAsync
    string result = await DownloadAsync(); // Await unwraps it
    
    // Do something with result
    string processed = result.ToUpper();
    
    return processed;
}

// Without await - would return Task<Task<string>>
public Task<string> ProcessWrong()
{
    return DownloadAsync(); // Still returns Task<string>, but wrong pattern
}
```

### Await and Control Flow
```csharp
public async Task ExecuteAsync()
{
    Console.WriteLine("Before await");
    
    // Thread returns to caller while waiting
    await Task.Delay(1000);
    
    Console.WriteLine("After await"); // Resumes after delay
}

// Thread doesn't block during await
var task = ExecuteAsync();
// Other work can happen here
await task;
```

## Error Handling

### Try-Catch with Async
```csharp
public async Task<string> FetchDataAsync()
{
    try
    {
        string data = await HttpClient.GetStringAsync("https://api.example.com/data");
        return data;
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Network error: {ex.Message}");
        return null;
    }
    catch (OperationCanceledException ex)
    {
        Console.WriteLine("Request cancelled");
        return null;
    }
}
```

### Catching from Await
```csharp
// Exception thrown in async method is captured in Task
public async Task<int> GetNumberAsync()
{
    throw new InvalidOperationException("Bad operation");
}

// Exception rethrown when awaited
try
{
    int number = await GetNumberAsync();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}
```

## Multiple Async Operations

### Sequential Execution
```csharp
public async Task<string> ProcessSequentiallyAsync()
{
    // Wait for first to complete
    string data1 = await FetchDataAsync(1);
    
    // Then wait for second
    string data2 = await FetchDataAsync(2);
    
    // Then wait for third
    string data3 = await FetchDataAsync(3);
    
    return $"{data1}-{data2}-{data3}";
}
```

### Parallel Execution
```csharp
public async Task<string> ProcessParallelAsync()
{
    // Start all tasks without waiting
    Task<string> task1 = FetchDataAsync(1);
    Task<string> task2 = FetchDataAsync(2);
    Task<string> task3 = FetchDataAsync(3);
    
    // Wait for all to complete
    string[] results = await Task.WhenAll(task1, task2, task3);
    
    return $"{results[0]}-{results[1]}-{results[2]}";
}

// Even simpler
public async Task<string> ProcessParallelAsync()
{
    var results = await Task.WhenAll(
        FetchDataAsync(1),
        FetchDataAsync(2),
        FetchDataAsync(3)
    );
    
    return string.Join("-", results);
}
```

### WhenAny - First to Complete
```csharp
public async Task<string> GetFirstToCompleteAsync()
{
    var tasks = new[]
    {
        FetchDataAsync("source1"),
        FetchDataAsync("source2"),
        FetchDataAsync("source3")
    };
    
    // Returns first completed task
    Task<string> completed = await Task.WhenAny(tasks);
    
    return completed.Result;
}
```

## CancellationToken

### Cancelling Operations
```csharp
// Create cancellation token
var cts = new CancellationTokenSource();

// Pass to async method
Task task = LongRunningOperationAsync(cts.Token);

// Cancel after delay
cts.CancelAfter(TimeSpan.FromSeconds(5));

// Or cancel immediately
cts.Cancel();

// Async method respects token
public async Task LongRunningOperationAsync(CancellationToken cancellationToken)
{
    for (int i = 0; i < 100; i++)
    {
        // Check if cancellation requested
        cancellationToken.ThrowIfCancellationRequested();
        
        await Task.Delay(1000, cancellationToken); // Respects cancellation
    }
}
```

## Task.Run

### Offloading Work to Thread Pool
```csharp
// CPU-bound work on thread pool
public async Task<int> CalculateAsync()
{
    return await Task.Run(() =>
    {
        // Runs on thread pool thread
        return ExpensiveCalculation();
    });
}

private int ExpensiveCalculation()
{
    // CPU-intensive work
    int result = 0;
    for (int i = 0; i < 1_000_000_000; i++)
    {
        result += i;
    }
    return result;
}

// Usage
int result = await CalculateAsync();
```

## Best Practices

1. **Use Async All the Way**
```csharp
// Bad: Async -> sync -> async (blocks thread)
public string GetData()
{
    return FetchDataAsync().Result; // Blocks! Can deadlock
}

// Good: Async all the way
public async Task<string> GetDataAsync()
{
    return await FetchDataAsync();
}
```

2. **Don't Use Async Void**
```csharp
// Bad: Can't await or catch exceptions
public async void ProcessAsync()
{
    await DoWorkAsync();
}

// Good: Use Task
public async Task ProcessAsync()
{
    await DoWorkAsync();
}

// Exception for event handlers
private async void OnButtonClick(object sender, EventArgs e)
{
    await ProcessAsync();
}
```

3. **Use ConfigureAwait for Libraries**
```csharp
// Library code - don't need UI context
public async Task<string> FetchAsync()
{
    using var response = await http.GetAsync(url).ConfigureAwait(false);
    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
}

// App code - might need UI context
public async Task DisplayAsync()
{
    string data = await FetchAsync(); // Resumes on UI thread
    UpdateUI(data);
}
```

## Common Mistakes

1. **Blocking on Async Result (Deadlock)**
```csharp
// BAD: Can cause deadlock
public class MyService
{
    public string GetData()
    {
        return FetchDataAsync().Result; // BLOCKS!
    }
    
    private async Task<string> FetchDataAsync()
    {
        await Task.Delay(1000);
        return "data";
    }
}

// GOOD: Use async all the way
public class MyService
{
    public async Task<string> GetDataAsync()
    {
        return await FetchDataAsync();
    }
    
    private async Task<string> FetchDataAsync()
    {
        await Task.Delay(1000);
        return "data";
    }
}
```

2. **Forgetting to Await**
```csharp
// Bad: Fire and forget, can't handle errors
public async Task ProcessAsync()
{
    FetchDataAsync(); // Not awaited! Runs in background
    Console.WriteLine("Done");
}

// Good: Await to wait for completion
public async Task ProcessAsync()
{
    await FetchDataAsync();
    Console.WriteLine("Done");
}

// If fire-and-forget intended, be explicit
public void ProcessFireAndForget()
{
    _ = FetchDataAsync(); // Explicitly ignored
}
```

3. **Returning Task from Async Method**
```csharp
// Bad: Double-wraps Task
public async Task<Task<string>> BadAsync()
{
    return await FetchDataAsync();
}

// Good: Return the actual value
public async Task<string> GoodAsync()
{
    return await FetchDataAsync();
}
```

## Quick Summary
- Async methods return Task or Task<T>
- Await unwraps Task and waits for result
- Doesn't block thread - enables scalability
- WhenAll for parallel operations
- WhenAny for first to complete
- CancellationToken for cancellation
- Always use async/await, not .Result
- Never use async void (except events)
- ConfigureAwait(false) for library code

## Resources
- Microsoft Async/Await Tutorial
- Async Best Practices
- Task-based Asynchronous Pattern (TAP)
