# Async/Await vs Tasks

## Overview
Understanding the relationship between async/await syntax and underlying Task infrastructure is critical for efficient asynchronous programming.

## Tasks as Foundation

### Task Represents Async Work
```csharp
// Task: represents asynchronous operation without result
Task task = SomeAsyncWork();
await task;

// Task<T>: represents async operation with result
Task<string> taskWithResult = GetDataAsync();
string data = await taskWithResult;

// Direct Task creation
Task<int> completed = Task.FromResult(42);
Task failed = Task.FromException(new Exception("Error"));
```

## Async/Await as Syntactic Sugar

### What async/await Generates
```csharp
// Async method (what you write)
public async Task<string> FetchAsync()
{
    var response = await GetAsync("url");
    return response.Content;
}

// What compiler generates (simplified)
public Task<string> FetchAsync()
{
    var stateMachine = new FetchAsyncStateMachine();
    stateMachine.builder = AsyncTaskMethodBuilder<string>.Create();
    stateMachine.state = 0;
    stateMachine.builder.Start(ref stateMachine);
    return stateMachine.builder.Task;
}

// The state machine handles await points
public struct FetchAsyncStateMachine : IAsyncStateMachine
{
    private int state;
    private TaskAwaiter<string> awaiter;
    
    public void MoveNext()
    {
        switch (state)
        {
            case 0: // Before first await
                // Setup first await
                awaiter = GetAsync("url").GetAwaiter();
                if (!awaiter.IsCompleted)
                {
                    state = 1;
                    // Return and resume when done
                    return;
                }
                goto case 1;
                
            case 1: // After first await
                // Get result from await
                var response = awaiter.GetResult();
                // Continue with rest of method
                break;
        }
    }
}
```

## Task Combinators vs Async

### Multiple Operations
```csharp
// Using Tasks directly
Task<string> task1 = GetAsync(1);
Task<string> task2 = GetAsync(2);

// Wait for both with Task.WhenAll
Task<string[]> combined = Task.WhenAll(task1, task2);
var results = await combined;

// Using async/await (more readable for complex flows)
public async Task<string> CombinedAsync()
{
    var task1 = GetAsync(1);
    var task2 = GetAsync(2);
    
    await Task.WhenAll(task1, task2);
    
    return task1.Result + task2.Result;
}
```

### Sequential vs Parallel
```csharp
// Sequential with async/await (most readable)
public async Task<Result> SequentialAsync()
{
    var first = await GetAsync(1);
    var second = await GetAsync(2);
    var third = await GetAsync(3);
    
    return Combine(first, second, third);
}

// Parallel with async/await
public async Task<Result> ParallelAsync()
{
    var task1 = GetAsync(1);
    var task2 = GetAsync(2);
    var task3 = GetAsync(3);
    
    await Task.WhenAll(task1, task2, task3);
    
    return Combine(task1.Result, task2.Result, task3.Result);
}

// Using Task.WhenAll directly (still valid)
public Task<Result> ParallelTasks()
{
    return Task.WhenAll(GetAsync(1), GetAsync(2), GetAsync(3))
        .ContinueWith(t => Combine(t.Result[0], t.Result[1], t.Result[2]));
}
```

## Async Methods vs Task-Returning Methods

### Key Differences
```csharp
// Async method (with await inside)
public async Task<string> AsyncMethod()
{
    await Task.Delay(1000);
    return "Done";
}

// Task-returning method (no await needed if already have Task)
public Task<string> TaskReturningMethod()
{
    return GetDataAsyncFromSomewhere();
}

// Both can be awaited
var result1 = await AsyncMethod();
var result2 = await TaskReturningMethod();

// But async has overhead
public async Task<string> UnnecessaryAsync() // Bad: no await!
{
    return "Result"; // Just return, no need for async
}

public Task<string> BetterWay() // Good: direct return
{
    return Task.FromResult("Result");
}
```

## Exception Handling

### Task Exception Behavior
```csharp
// Exceptions in Task are wrapped
Task failedTask = Task.Run(() => throw new InvalidOperationException());

try
{
    failedTask.Wait(); // Throws AggregateException
}
catch (AggregateException ex)
{
    Console.WriteLine(ex.InnerException.Message);
}

// Async/await unwraps the exception
public async Task<string> AsyncWithError()
{
    try
    {
        var result = await GetAsyncError();
        return result;
    }
    catch (InvalidOperationException ex) // Directly caught
    {
        Console.WriteLine(ex.Message);
    }
}
```

## Synchronization Context

### Task vs Async Behavior
```csharp
// Task: Uses thread pool, no context capture
Task task = Task.Run(() => 
{
    Thread.CurrentThread.ManagedThreadId; // Different thread
});

// Async: Captures and restores context
public async Task AsyncMethod()
{
    var contextThread = Thread.CurrentThread.ManagedThreadId;
    
    await Task.Delay(100); // May switch threads
    
    var resumeThread = Thread.CurrentThread.ManagedThreadId;
    // In UI app: usually same thread (context matters)
    // In console: usually different threads (no special context)
}
```

## ConfigureAwait

### Avoiding Context Capture
```csharp
// Bad: Library code captures UI context
public async Task<string> LibraryMethod()
{
    await GetDataAsync(); // Captures UI thread context
    return "Result"; // Returns on UI thread
}

// Good: Library code doesn't need context
public async Task<string> BetterLibraryMethod()
{
    await GetDataAsync().ConfigureAwait(false);
    return "Result"; // Returns on thread pool thread
}

// In UI code: async method doesn't use ConfigureAwait
public async void OnButtonClick()
{
    var result = await GetDataAsync();
    UpdateUI(result); // Must be UI thread
}
```

## Performance Implications

### Async Overhead
```csharp
// Task.Run has overhead
var task = Task.Run(() => 
{
    return ExpensiveCalculation();
});

// Async has state machine overhead
public async Task<int> AsyncCalculation()
{
    await Task.Delay(0);
    return ExpensiveCalculation();
}

// Direct synchronous is faster for non-blocking work
public int SynchronousCalculation()
{
    return ExpensiveCalculation();
}

// But async is essential for I/O
public async Task<string> GetFromNetwork()
{
    using var client = new HttpClient();
    return await client.GetStringAsync("url"); // Truly async
}
```

## Best Practices

1. **Use Async for I/O, Not CPU Work**
```csharp
// Good: I/O operation
public async Task<string> ReadFileAsync()
{
    return await File.ReadAllTextAsync("file.txt");
}

// Bad: CPU-bound as async (wastes threads)
public async Task<int> CalculateAsync()
{
    await Task.Delay(0); // Pointless
    return Enumerable.Range(0, 1_000_000).Sum();
}

// Good: CPU-bound on thread pool
public Task<int> CalculateOnThreadPool()
{
    return Task.Run(() => Enumerable.Range(0, 1_000_000).Sum());
}
```

2. **Async All the Way (Don't Block)**
```csharp
// Bad: Blocking on async operation
public string GetData()
{
    return GetDataAsync().Result; // Can deadlock!
}

// Good: Keep async
public async Task<string> GetDataAwait()
{
    return await GetDataAsync();
}

// Bad: Mixing sync and async
public class BadService
{
    public string GetData() => GetDataAsync().Result;
}

// Good: Consistent
public class GoodService
{
    public async Task<string> GetDataAsync()
    {
        return await GetDataAsync();
    }
}
```

3. **ConfigureAwait(false) in Libraries**
```csharp
// Library: Don't capture context
public async Task<Data> FetchAsync()
{
    var response = await client.GetAsync(url).ConfigureAwait(false);
    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    return Parse(content);
}

// App code: No need (or optional)
public async Task LoadDataAsync()
{
    var data = await _service.FetchAsync();
    UpdateUI(data); // Still on UI thread if needed
}
```

## Common Mistakes

1. **Async Without Await**
```csharp
// Bad: Fire and forget
public async Task ProcessAsync()
{
    await GetDataAsync(); // Starts but method returns immediately
}

// Bad: Unnecessary async
public async Task<string> GetString()
{
    return "result"; // No await needed
}

// Good
public Task<string> GetString()
{
    return Task.FromResult("result");
}
```

2. **Blocking on Async (Deadlock)**
```csharp
// DANGER: Can deadlock
public class MyService
{
    public void ProcessData()
    {
        var data = GetDataAsync().Result; // BLOCKS
    }
}

// SAFE
public class MyService
{
    public async Task ProcessDataAsync()
    {
        var data = await GetDataAsync(); // Non-blocking
    }
}
```

3. **Not Awaiting Tasks**
```csharp
// Bad: Task not awaited (compiler warning)
public async Task ProcessAsync()
{
    GetDataAsync(); // Warning: not awaited
}

// Good
public async Task ProcessAsync()
{
    await GetDataAsync();
}

// Or explicitly
public async Task ProcessAsync()
{
    _ = GetDataAsync(); // Intentional fire-and-forget
}
```

## Quick Summary
- Async/await generates state machines on top of Tasks
- Tasks are the foundation; async is the syntax
- Use async for I/O operations
- Use Task.Run for CPU-bound work on thread pool
- Don't block on async operations (.Result)
- Keep async all the way up the call stack
- Use ConfigureAwait(false) in libraries
- Exception handling is cleaner with async/await
- Performance overhead minimal for I/O scenarios
- Understand that .NET handles complexity

## Resources
- Async/Await Patterns
- Task-Based Asynchronous Pattern (TAP)
- ConfigureAwait Best Practices
- State Machine Implementation
