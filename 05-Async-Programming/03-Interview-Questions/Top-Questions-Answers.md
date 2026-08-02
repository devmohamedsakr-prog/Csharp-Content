# Async Programming - Interview Questions & Answers

## 1. What is asynchronous programming and why is it important?

**Answer:**

Asynchronous programming allows code to run without blocking the calling thread, improving responsiveness and scalability.

**Why Important**:
- **UI Responsiveness**: Keep UI responsive during long operations
- **Server Scalability**: Handle more requests with limited threads
- **Resource Efficiency**: Don't waste threads waiting for I/O

```csharp
// Synchronous - blocks thread
public string FetchData() {
    Thread.Sleep(2000);  // Thread blocked
    return "Data";
}

// Asynchronous - doesn't block
public async Task<string> FetchDataAsync() {
    await Task.Delay(2000);  // Thread can do other work
    return "Data";
}

// Usage
string data = FetchData();  // Waits 2 seconds

Task<string> task = FetchDataAsync();  // Returns immediately
string data = await task;  // Wait when needed
```

---

## 2. What is the difference between async/await and Task?

**Answer:**

**Task**: Represents asynchronous operation
**async/await**: Syntactic sugar to work with Tasks cleanly

```csharp
// Task-based (older style)
public Task<int> GetNumberAsync() {
    return Task.FromResult(42);
}

public void Process() {
    Task<int> task = GetNumberAsync();
    task.ContinueWith(t => Console.WriteLine(t.Result));
}

// async/await (modern, cleaner)
public async Task<int> GetNumberAsync() {
    await Task.Delay(1000);
    return 42;
}

public async void Process() {
    int result = await GetNumberAsync();
    Console.WriteLine(result);
}
```

**Key Points**:
- `async` keyword enables `await` in a method
- `await` pauses execution until Task completes
- Returns control to caller while waiting
- Much cleaner than Task.ContinueWith()

---

## 3. What is the difference between Task and Task<T>?

**Answer:**

```csharp
// Task - no return value
public async Task DoWorkAsync() {
    await Task.Delay(1000);
    Console.WriteLine("Work done");
}

// Task<T> - returns a value of type T
public async Task<int> GetNumberAsync() {
    await Task.Delay(1000);
    return 42;
}

// Usage
Task task = DoWorkAsync();
await task;

Task<int> task = GetNumberAsync();
int result = await task;  // result = 42
```

---

## 4. What are common async patterns and pitfalls?

**Answer:**

**✓ Correct Pattern**:
```csharp
public async Task ProcessAsync() {
    var data = await FetchDataAsync();
    var result = await ProcessDataAsync(data);
    return result;
}
```

**❌ Pitfall 1: Synchronous wrapper (creates deadlock)**
```csharp
public void Process() {
    var result = FetchDataAsync().Result;  // Can deadlock!
}
```

**❌ Pitfall 2: Async void (hard to track, can crash)**
```csharp
// Only use for event handlers!
public async void ButtonClick() {  // Bad for most cases
    await FetchDataAsync();
}

// Better
public async Task ButtonClickAsync() {
    await FetchDataAsync();
}
```

**❌ Pitfall 3: Fire and forget**
```csharp
// Dangerous - exception silently swallowed
FetchDataAsync();  // Not awaited, exception lost

// Better
_ = FetchDataAsync();  // Explicitly intentional
```

**❌ Pitfall 4: Wrong ConfigureAwait**
```csharp
// Library code should use ConfigureAwait(false)
public async Task<int> GetCountAsync() {
    // Avoids deadlock, doesn't need UI context
    var data = await FetchAsync().ConfigureAwait(false);
    return data.Length;
}
```

---

## 5. What is Task.WhenAll and Task.WhenAny?

**Answer:**

**Task.WhenAll**: Wait for all tasks to complete

```csharp
Task<int> task1 = FetchAsync(1);
Task<int> task2 = FetchAsync(2);
Task<int> task3 = FetchAsync(3);

// Wait for all to complete
var results = await Task.WhenAll(task1, task2, task3);
// results: [1, 2, 3]

// Shorter syntax
var results = await Task.WhenAll(
    FetchAsync(1),
    FetchAsync(2),
    FetchAsync(3)
);
```

**Task.WhenAny**: Wait for first to complete

```csharp
Task<int> task1 = FetchAsync(1);  // 5 seconds
Task<int> task2 = FetchAsync(2);  // 2 seconds
Task<int> task3 = FetchAsync(3);  // 4 seconds

// Returns when ANY completes (task2 first)
Task<int> firstCompleted = await Task.WhenAny(task1, task2, task3);
var result = await firstCompleted;  // Result from first completed
```

---

## 6. What is CancellationToken and how is it used?

**Answer:**

CancellationToken allows gracefully canceling async operations.

```csharp
public async Task FetchDataAsync(CancellationToken ct) {
    for (int i = 0; i < 100; i++) {
        ct.ThrowIfCancellationRequested();  // Throw if canceled
        await Task.Delay(1000, ct);  // Check during wait
    }
}

// Usage
var cts = new CancellationTokenSource();

// Start operation
Task task = FetchDataAsync(cts.Token);

// Cancel after 5 seconds
Task.Delay(5000).ContinueWith(_ => cts.Cancel());

try {
    await task;
} catch (OperationCanceledException) {
    Console.WriteLine("Operation was canceled");
}

// Timeout cancel
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
await FetchDataAsync(cts.Token);
```

---

## 7. What is the difference between Task.Run and Task.Factory.StartNew?

**Answer:**

```csharp
// Task.Run - recommended, simpler
Task task = Task.Run(() => {
    // Runs on thread pool
    Console.WriteLine("Running on thread pool");
});

// Task.Factory.StartNew - more options, older
Task task = Task.Factory.StartNew(() => {
    // More control
    Console.WriteLine("Running with more options");
});

// Practical difference (rare)
Task task = Task.Factory.StartNew(
    () => Console.WriteLine("Custom scheduler"),
    TaskScheduler.Default  // Can specify scheduler
);
```

**Use Task.Run** for most cases - simpler and default behavior is usually what you want.

---

## 8. What is a async method returning Task?

**Answer:**

Methods that perform asynchronous work and return Task.

```csharp
// Method with no return value
public async Task SaveAsync() {
    await db.SaveAsync();
    Console.WriteLine("Saved");
}

// Method with return value
public async Task<int> GetCountAsync() {
    var count = await db.CountAsync();
    return count;
}

// Event handler (only async void allowed)
public async void OnButtonClick(object sender, EventArgs e) {
    await SaveAsync();
}

// Calling
await SaveAsync();  // Wait for completion
int count = await GetCountAsync();  // Wait and get result
```

**Naming Convention**: Suffix `Async` for all async methods.

---

## 9. How do you handle exceptions in async code?

**Answer:**

```csharp
// Try-catch works normally
public async Task ProcessAsync() {
    try {
        var data = await FetchDataAsync();
        var result = await ProcessDataAsync(data);
    } catch (HttpRequestException ex) {
        Console.WriteLine($"Network error: {ex.Message}");
    } catch (Exception ex) {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

// Multiple operations
public async Task ProcessMultipleAsync() {
    try {
        var results = await Task.WhenAll(
            FetchAsync(1),
            FetchAsync(2),
            FetchAsync(3)
        );
    } catch (Exception ex) {
        // If ANY task throws, caught here
        Console.WriteLine($"Error: {ex.Message}");
    }
}

// Individual error handling
var tasks = new[] {
    FetchAsync(1),
    FetchAsync(2),
    FetchAsync(3)
};

var results = await Task.WhenAll(tasks)
    .ContinueWith(t => {
        if (t.IsFaulted) {
            foreach (var ex in t.Exception.InnerExceptions) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        return t.Result;
    });
```

---

## 10. What is async LINQ?

**Answer:**

Entity Framework provides async LINQ methods.

```csharp
// Synchronous LINQ
List<Student> students = dbContext.Students
    .Where(s => s.Score > 80)
    .ToList();  // Blocks

// Asynchronous LINQ
List<Student> students = await dbContext.Students
    .Where(s => s.Score > 80)
    .ToListAsync();  // Non-blocking

// Other async methods
var first = await query.FirstAsync();
var first = await query.FirstOrDefaultAsync();
var count = await query.CountAsync();
var any = await query.AnyAsync();
var single = await query.SingleAsync();
```

**Key Methods**:
- `ToListAsync()`
- `ToArrayAsync()`
- `FirstAsync()`
- `FirstOrDefaultAsync()`
- `SingleAsync()`
- `SingleOrDefaultAsync()`
- `CountAsync()`
- `AnyAsync()`
- `SumAsync()`
- `AverageAsync()`

---

## 11. What is async/await best practices?

**Answer:**

```csharp
// ✓ Use async/await instead of Task.Result
public async Task<string> GetDataAsync() {
    return await httpClient.GetStringAsync(url);
}

// ✓ Use ConfigureAwait in libraries
public async Task<int> GetCountAsync() {
    var result = await dbContext.Items
        .CountAsync()
        .ConfigureAwait(false);  // Don't need UI context
    return result;
}

// ✓ Return Task from async methods
public async Task SaveAsync() {
    await db.SaveAsync();
}

// ✓ Use CancellationToken parameter
public async Task FetchAsync(CancellationToken ct) {
    await httpClient.GetAsync(url, ct);
}

// ✓ Name methods with Async suffix
public async Task GetUserAsync() { }

// ✗ Avoid async void (except event handlers)
// ✗ Avoid blocking with .Result
// ✗ Avoid unnecessary ConfigureAwait in UI code
// ✗ Avoid fire-and-forget without tracking
```

---

## 12. What is parallel vs concurrent execution?

**Answer:**

**Concurrent**: Multiple operations that may overlap (async)
```csharp
public async Task ConcurrentAsync() {
    // Both start immediately, may overlap
    Task<int> task1 = FetchAsync(1);
    Task<int> task2 = FetchAsync(2);
    
    var results = await Task.WhenAll(task1, task2);
}
```

**Parallel**: Multiple operations running simultaneously (multithread)
```csharp
public void Parallel() {
    // Uses multiple threads
    Parallel.For(0, 100, i => {
        ProcessItem(i);
    });
}

// Or
Parallel.ForEach(items, item => {
    ProcessItem(item);
});
```

**Key Difference**:
- **Concurrent**: Single or multiple threads, optimized for I/O
- **Parallel**: Multiple threads, optimized for CPU-bound work

---

## Quick Tips for Interview

✓ Understand async/await vs Task
✓ Know async void is dangerous (except event handlers)
✓ Explain Task.Result blocks and can deadlock
✓ Understand Task.WhenAll vs Task.WhenAny
✓ Know CancellationToken for graceful cancellation
✓ Understand ConfigureAwait(false) for libraries
✓ Know async LINQ methods (ToListAsync, etc.)
✓ Comfortable with exception handling in async
✓ Understand concurrent vs parallel execution
