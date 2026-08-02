# Task Parallel Library (TPL)

## Overview
TPL provides APIs for parallel and concurrent programming with tasks, enabling multiple operations to run simultaneously.

## Tasks

### Creating Tasks
```csharp
// Task: No return value
Task task = new Task(() => Console.WriteLine("Work"));
task.Start();
task.Wait();

// Task<T>: Returns value
Task<int> task = new Task<int>(() => 42);
task.Start();
int result = task.Result;

// Task.Run: Create and start immediately
Task task = Task.Run(() => Console.WriteLine("Work"));
await task;

Task<string> task = Task.Run(() => "Result");
string result = await task;
```

### Task Status
```csharp
Task task = Task.Run(() => 
{
    Thread.Sleep(1000);
    Console.WriteLine("Done");
});

Console.WriteLine(task.Status); // Running
task.Wait();
Console.WriteLine(task.Status); // RanToCompletion

// Exception handling
Task<int> faultyTask = Task.Run(() => throw new Exception("Error"));
try { faultyTask.Wait(); }
catch { Console.WriteLine("Task faulted"); }
Console.WriteLine(faultyTask.Status); // Faulted
```

## Parallel.For and Parallel.ForEach

### Parallel Iteration
```csharp
var numbers = Enumerable.Range(1, 100).ToList();

// Parallel.For - indexed loop
Parallel.For(0, numbers.Count, i =>
{
    Console.WriteLine($"Processing {numbers[i]} on thread {Thread.CurrentThread.ManagedThreadId}");
});

// Parallel.ForEach - iterate collection
Parallel.ForEach(numbers, number =>
{
    // Each iteration runs on potentially different thread
    int result = ExpensiveOperation(number);
});
```

### Configuration
```csharp
var options = new ParallelOptions
{
    MaxDegreeOfParallelism = Environment.ProcessorCount, // Use all cores
    CancellationToken = cancellationToken
};

Parallel.ForEach(items, options, item =>
{
    ProcessItem(item);
});
```

## Parallel LINQ (PLINQ)

### Parallel Queries
```csharp
var numbers = Enumerable.Range(1, 1000000).ToList();

// Sequential LINQ
var result = numbers.Where(n => n % 2 == 0).Select(n => n * 2);

// Parallel LINQ - uses multiple threads
var parallelResult = numbers.AsParallel()
    .Where(n => n % 2 == 0)
    .Select(n => n * 2)
    .ToList();
```

### PLINQ Options
```csharp
var result = numbers.AsParallel()
    .WithDegreeOfParallelism(Environment.ProcessorCount)
    .WithCancellation(cancellationToken)
    .Where(n => ExpensiveFilter(n))
    .ToList();
```

## Task Composition

### ContinueWith
```csharp
Task<int> task = Task.Run(() => 42);

// Execute after first task completes
Task<string> continuation = task.ContinueWith(t =>
{
    int result = t.Result;
    return $"Result: {result}";
});

string final = continuation.Result; // "Result: 42"

// Chain multiple continuations
task.ContinueWith(t => Console.WriteLine("Step 1"))
    .ContinueWith(t => Console.WriteLine("Step 2"))
    .ContinueWith(t => Console.WriteLine("Step 3"))
    .Wait();
```

### ContinueWith with TaskScheduler
```csharp
// Run continuation on UI thread (if using UI framework)
task.ContinueWith(t =>
{
    UpdateUI(t.Result);
}, TaskScheduler.FromCurrentSynchronizationContext());
```

## Task Cancellation

### CancellationToken
```csharp
var cts = new CancellationTokenSource();

Task task = Task.Run(async () =>
{
    for (int i = 0; i < 100; i++)
    {
        cts.Token.ThrowIfCancellationRequested();
        await Task.Delay(100, cts.Token);
    }
});

// Cancel after 500ms
cts.CancelAfter(500);

try
{
    task.Wait();
}
catch (OperationCanceledException)
{
    Console.WriteLine("Task was cancelled");
}
```

## Handling Exceptions

### AggregateException
```csharp
// Multiple tasks can throw multiple exceptions
var tasks = Enumerable.Range(0, 10)
    .Select(i => Task.Run(() => 
    {
        if (i == 5) throw new InvalidOperationException($"Error {i}");
    }))
    .ToArray();

try
{
    Task.WaitAll(tasks);
}
catch (AggregateException ex)
{
    Console.WriteLine($"Caught {ex.InnerExceptions.Count} exceptions");
    foreach (var innerEx in ex.InnerExceptions)
    {
        Console.WriteLine($"  - {innerEx.Message}");
    }
}
```

### Handle and Flatten
```csharp
try
{
    Task.WaitAll(tasks);
}
catch (AggregateException ex)
{
    var flattened = ex.Flatten();
    foreach (var innerEx in flattened.InnerExceptions)
    {
        Console.WriteLine($"Handle: {innerEx.Message}");
    }
}
```

## Best Practices

1. **Use Task.Run for CPU-Bound Work**
```csharp
// Good: Offload CPU work to thread pool
public async Task<int> CalculateAsync()
{
    return await Task.Run(() => ExpensiveCalculation());
}
```

2. **Use Parallel.ForEach for CPU-Bound Iterations**
```csharp
// Good: Utilize multiple cores
Parallel.ForEach(largeCollection, item =>
{
    ProcessExpensiveItem(item);
});
```

3. **Be Careful with Parallel Overhead**
```csharp
// Bad: Too much parallelization overhead
Parallel.For(0, 10, i => Console.WriteLine(i));

// Good: Use parallel for significant work
Parallel.ForEach(millionItems, item => ExpensiveProcess(item));
```

4. **Handle Exceptions Properly**
```csharp
var tasks = items.Select(i => Task.Run(() => Process(i))).ToArray();

try
{
    Task.WaitAll(tasks);
}
catch (AggregateException ex)
{
    var errors = ex.Flatten().InnerExceptions;
    foreach (var error in errors)
    {
        Logger.LogError(error);
    }
}
```

## Common Mistakes

1. **Using Task.Result (Deadlock)**
```csharp
// Bad: Blocks thread, can deadlock
public void ProcessData()
{
    string data = GetDataAsync().Result; // BLOCKS!
}

// Good: Use async/await
public async Task ProcessDataAsync()
{
    string data = await GetDataAsync();
}
```

2. **Ignoring Exceptions in Parallel Operations**
```csharp
// Bad: Exceptions silently ignored
Parallel.ForEach(items, item =>
{
    try { Process(item); } // Catch but don't log!
    catch { }
});

// Good: Handle and log
Parallel.ForEach(items, item =>
{
    try { Process(item); }
    catch (Exception ex) { Logger.LogError(ex); }
});
```

3. **Too Much Parallelization**
```csharp
// Bad: Overhead exceeds benefit
var results = Parallel.ForEach(new[] { 1, 2, 3 }, x => x * 2);

// Good: Parallel for significant work
var results = Parallel.ForEach(millionItems, 
    new ParallelOptions { MaxDegreeOfParallelism = 4 },
    item => ExpensiveWork(item));
```

4. **Thread-Unsafe Collections**
```csharp
// Bad: Race condition
var results = new List<int>();
Parallel.ForEach(items, item =>
{
    results.Add(Process(item)); // Not thread-safe!
});

// Good: Use thread-safe collection
var results = new ConcurrentBag<int>();
Parallel.ForEach(items, item =>
{
    results.Add(Process(item));
});
```

## Quick Summary
- Task.Run offloads work to thread pool
- Parallel.For and ForEach for parallel iterations
- PLINQ for parallel LINQ queries
- ContinueWith chains task operations
- CancellationToken enables cancellation
- AggregateException handles multiple exceptions
- Measure parallelization benefit vs overhead
- Use thread-safe collections in parallel scenarios

## Resources
- Task Parallel Library (TPL) documentation
- Parallel Programming Best Practices
- PLINQ queries documentation
