# Task Composition - Advanced Patterns

## Overview
Advanced task composition patterns for complex async scenarios beyond basic await and WhenAll.

## ContinueWith Pattern

### Sequential Task Chaining
```csharp
// ContinueWith: execute after task completes
Task<int> task1 = Task.Run(() => 42);

Task<int> task2 = task1.ContinueWith(t => 
{
    int result = t.Result;
    return result * 2; // 84
});

int final = task2.Result; // 84

// Chain multiple continuations
task1
    .ContinueWith(t => Console.WriteLine($"First: {t.Result}"))
    .ContinueWith(t => Console.WriteLine("Second"))
    .ContinueWith(t => Console.WriteLine("Third"))
    .Wait();
```

### ContinueWith with Error Handling
```csharp
// Handle exceptions
Task<int> failTask = Task.Run(() => throw new InvalidOperationException("Error"));

Task<int> handled = failTask.ContinueWith(t =>
{
    if (t.IsFaulted)
    {
        Console.WriteLine($"Error: {t.Exception.Message}");
        return -1;
    }
    else if (t.IsCanceled)
    {
        Console.WriteLine("Cancelled");
        return -2;
    }
    else
    {
        return t.Result * 2;
    }
});

// Better: OnlyOnRanToCompletion
failTask.ContinueWith(t => Console.WriteLine($"Success: {t.Result}"),
    TaskContinuationOptions.OnlyOnRanToCompletion);

failTask.ContinueWith(t => Console.WriteLine($"Failed: {t.Exception}"),
    TaskContinuationOptions.OnlyOnFaulted);

failTask.ContinueWith(t => Console.WriteLine("Cancelled"),
    TaskContinuationOptions.OnlyOnCanceled);
```

## WhenAll and WhenAny

### Wait for All
```csharp
// Start independent tasks
Task<int> task1 = GetAsync(1);
Task<int> task2 = GetAsync(2);
Task<int> task3 = GetAsync(3);

// Wait for all to complete
Task<int[]> all = Task.WhenAll(task1, task2, task3);
int[] results = all.Result; // [1, 2, 3]

// With exception
try
{
    Task<int> failTask = GetAsyncFail(1);
    Task<int> successTask = GetAsync(2);
    
    await Task.WhenAll(failTask, successTask);
}
catch (Exception ex) // Thrown from failTask
{
    Console.WriteLine($"One task failed: {ex.Message}");
}
```

### Wait for First
```csharp
// WhenAny: return first to complete
Task<string> task1 = GetAsync("source1", 1000);
Task<string> task2 = GetAsync("source2", 500);
Task<string> task3 = GetAsync("source3", 2000);

Task<Task<string>> raceTask = Task.WhenAny(task1, task2, task3);
Task<string> first = raceTask.Result;
string result = first.Result; // From source2 (fastest)

// Practical: get data from fastest source
public async Task<T> GetFromFirstAsync<T>(
    params Task<T>[] sources)
{
    Task<Task<T>> race = Task.WhenAny(sources);
    Task<T> winner = await race;
    return await winner;
}
```

## Task.Run Composition

### Offloading Work
```csharp
// CPU-bound work on thread pool
public async Task<int> CalculateAsync()
{
    return await Task.Run(() =>
    {
        // Expensive CPU work
        return Enumerable.Range(0, 1_000_000)
            .AsParallel()
            .Sum();
    });
}

// Chain with other tasks
var calculation = CalculateAsync();
var fileWrite = File.WriteAllTextAsync("result.txt", "working...");

await Task.WhenAll(calculation, fileWrite);
Console.WriteLine($"Done: {calculation.Result}");
```

## Custom Task Combinators

### Retry Pattern with Delays
```csharp
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

// Usage
var result = await RetryAsync(
    () => FetchDataAsync("https://api.example.com"),
    maxAttempts: 3
);
```

### Timeout with CancellationToken
```csharp
public static async Task<T> WithTimeoutAsync<T>(
    Task<T> task,
    TimeSpan timeout)
{
    var cts = new CancellationTokenSource(timeout);
    var delayTask = Task.Delay(timeout, cts.Token);
    
    var completed = await Task.WhenAny(task, delayTask);
    
    if (completed == delayTask)
    {
        throw new TimeoutException($"Operation timed out after {timeout.TotalSeconds}s");
    }
    
    cts.Cancel(); // Stop delay task
    return await task;
}

// Usage
try
{
    var result = await WithTimeoutAsync(
        FetchAsync("https://slow-api.com"),
        TimeSpan.FromSeconds(5)
    );
}
catch (TimeoutException)
{
    Console.WriteLine("Request took too long");
}
```

### Polling Pattern
```csharp
public static async Task<T> PollAsync<T>(
    Func<Task<(bool Found, T Value)>> check,
    TimeSpan interval,
    TimeSpan timeout)
{
    var stopwatch = Stopwatch.StartNew();
    
    while (stopwatch.Elapsed < timeout)
    {
        var (found, value) = await check();
        if (found)
            return value;
        
        await Task.Delay(interval);
    }
    
    throw new TimeoutException($"Polling timed out after {timeout.TotalSeconds}s");
}

// Usage
var result = await PollAsync(
    async () =>
    {
        var job = await GetJobStatusAsync(jobId);
        return (job.IsComplete, job.Result);
    },
    interval: TimeSpan.FromMilliseconds(500),
    timeout: TimeSpan.FromSeconds(30)
);
```

## Best Practices

1. **Use ContinueWith for Simple Chaining**
```csharp
// Good: Simple sequential
task.ContinueWith(t => Console.WriteLine("Done"));

// Better for complex: async/await
public async Task ProcessAsync()
{
    await task;
    Console.WriteLine("Done");
}
```

2. **Prefer WhenAll Over Manual Waiting**
```csharp
// Good: WhenAll
await Task.WhenAll(task1, task2, task3);

// Bad: Manual waiting
task1.Wait();
task2.Wait();
task3.Wait();
```

3. **Use TaskScheduler for UI Threads**
```csharp
// Good: Capture UI context
var uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

task.ContinueWith(t => UpdateUI(), uiScheduler);
```

## Common Mistakes

1. **Not Handling Task States**
```csharp
// Bad: Ignores error status
task.ContinueWith(t => 
{
    var result = t.Result; // Throws if faulted
});

// Good: Check status
task.ContinueWith(t =>
{
    if (t.IsCompletedSuccessfully)
        Console.WriteLine(t.Result);
    else if (t.IsFaulted)
        Console.WriteLine($"Error: {t.Exception}");
});
```

2. **Not Unwrapping Nested Tasks**
```csharp
// Bad: Task<Task<T>>
Task<Task<int>> nested = task.ContinueWith(t => GetAsync());

// Good: Unwrap
Task<int> unwrapped = task.Unwrap();
```

3. **Losing Exceptions in Combinators**
```csharp
// Bad: Exception lost
Task.WhenAll(failTask, successTask).Wait(); // Throws AggregateException

// Good: Handle properly
try
{
    await Task.WhenAll(failTask, successTask);
}
catch (Exception ex)
{
    Console.WriteLine($"Task failed: {ex}");
}
```

## Quick Summary
- ContinueWith for sequential chaining
- WhenAll waits for all tasks
- WhenAny returns first completed
- Retry pattern with exponential backoff
- Timeout with CancellationToken
- Polling for status checks
- Handle all task states
- Use TaskScheduler for context
- Unwrap nested Task<Task<T>>
- Prefer async/await over ContinueWith

## Resources
- Task Composition Patterns
- ContinueWith vs async/await
- Task Exception Handling
- Cancellation Patterns
