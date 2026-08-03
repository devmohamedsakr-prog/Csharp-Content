# Closures and Advanced Scope Concepts

## Overview

This category covers advanced scope concepts including closures (functions that capture variables), the classic loop closure bug, and modern resource management with using declarations. These concepts build on fundamentals and are essential for functional programming patterns in C#.

## Topics Covered

### 1. Variable Capture and Closures
**File**: `01-Variable-Capture/00-Variable-Capture.md`

Master how closures capture and maintain variables from their enclosing scope.

**Key Concepts**:
- What closures are and how they work
- Variable capture by reference
- Multiple closures sharing captured variables
- Captured variables keep objects alive
- Closures in LINQ queries
- Thread safety with closures
- Event handlers and closures
- Memory implications of closures
- Closure patterns (factory, memoization)

**When to Use**: Closure knowledge helps you:
- Use lambdas and anonymous methods effectively
- Understand LINQ query execution
- Design functional programming patterns
- Avoid unintended variable capture
- Prevent memory leaks from closures

**Example**:
```csharp
public Func<int> CreateCounter()
{
    int count = 0; // Captured variable
    
    return () => ++count; // Closure captures count
}

var counter = CreateCounter();
Console.WriteLine(counter()); // 1
Console.WriteLine(counter()); // 2 - count persists!
```

---

### 2. Loop Variable Closure Problem
**File**: `02-Loop-Variable-Closure/00-Loop-Variable-Closure.md`

Understand the classic loop closure bug and its solutions.

**Key Concepts**:
- The classic loop closure problem
- Why loop variables capture by reference
- Solutions: local copy, extract method, foreach, LINQ
- Common scenarios: lambdas, event handlers, LINQ, threading
- Modern C# improvements
- Anti-patterns and best practices

**When to Use**: Loop closure knowledge helps you:
- Avoid the most common closure bug
- Write correct concurrent code
- Use foreach and LINQ confidently
- Debug mysterious closure issues
- Understand deferred execution

**Example - The Bug**:
```csharp
var actions = new List<Action>();

for (int i = 0; i < 3; i++)
{
    actions.Add(() => Console.WriteLine(i));
}

foreach (var action in actions)
    action(); // Prints 3, 3, 3 - BUG!
```

**Example - The Fix**:
```csharp
for (int i = 0; i < 3; i++)
{
    int copy = i; // Create local copy
    actions.Add(() => Console.WriteLine(copy));
}

// Prints 0, 1, 2 - Correct!
```

---

### 3. Using Declarations
**File**: `03-Using-Declarations/00-Using-Declarations.md`

Learn modern resource management with automatic disposal.

**Key Concepts**:
- IDisposable interface and pattern
- Using statements (C# 7.0+)
- Using declarations (C# 8.0+)
- await using for async disposal
- Disposal order and guarantees
- Exception safety
- File operations with using
- Database connections
- HTTP requests
- Stream operations

**When to Use**: Using declarations help you:
- Ensure proper resource cleanup
- Write cleaner resource-management code
- Handle exceptions safely
- Work with files, databases, networks
- Prevent resource leaks

**Example**:
```csharp
// Modern using declaration (C# 8.0+)
using var reader = File.OpenText("data.txt");
string line = reader.ReadLine();
// reader.Dispose() automatically called at method end

// With multiple resources
using var file1 = File.OpenRead("file1.txt");
using var file2 = File.OpenRead("file2.txt");
// Both disposed at method end, in reverse order
```

---

## Learning Path

### Beginner
1. Understand what closures are
2. See closure in simple lambda examples
3. Learn the loop closure bug and its solution

### Intermediate
1. Use LINQ and closures together
2. Understand deferred execution
3. Implement IDisposable correctly
4. Use using statements for resources

### Advanced
1. Design with closures and patterns
2. Optimize closure memory usage
3. Handle thread-safety with closures
4. Design complex resource management scenarios

---

## Quick Reference

### Closure Capture

| Scenario | Captured | Shared |
|----------|----------|--------|
| Single lambda | Variables | No |
| Multiple lambdas | Same variables | Yes |
| Loop variable | By reference | All iterations see final value |
| Local copy | Separate copy | Each iteration separate |

### Using Statement Types

| Version | Syntax | Scope |
|---------|--------|-------|
| C# 7.0 | using (var x = ...) { } | Braces |
| C# 8.0 | using var x = ...; | Method end |
| Async | await using var x = ...; | Method end |

---

## Common Closure Patterns

### Pattern 1: Factory
```csharp
public Func<int, int> CreateMultiplier(int factor)
{
    return x => x * factor; // factor captured
}

var times2 = CreateMultiplier(2);
var times5 = CreateMultiplier(5);

Console.WriteLine(times2(10)); // 20
Console.WriteLine(times5(10)); // 50
```

### Pattern 2: State Machine
```csharp
public Func<int> CreateCounter(int initial)
{
    int count = initial;
    return () => ++count; // Maintains state
}
```

### Pattern 3: Memoization
```csharp
var cache = new Dictionary<int, int>();

Func<int, int> fibonacci = null;
fibonacci = n =>
{
    if (cache.TryGetValue(n, out int result))
        return result;
    
    if (n <= 1) result = n;
    else result = fibonacci(n - 1) + fibonacci(n - 2);
    
    cache[n] = result;
    return result;
};
```

---

## IDisposable Pattern

### Basic Implementation
```csharp
public class Resource : IDisposable
{
    private bool _disposed = false;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (disposing)
        {
            // Dispose managed resources
        }
        
        // Clean unmanaged resources
        
        _disposed = true;
    }
    
    ~Resource()
    {
        Dispose(false);
    }
}
```

### Usage
```csharp
using var resource = new Resource();
// Use resource
// Automatically disposed
```

---

## Thread Safety with Closures

### Problem: Race Condition
```csharp
var funcs = new List<Func<int>>();
int x = 0;

for (int i = 0; i < 10; i++)
{
    funcs.Add(() => ++x); // Captures x
}

// Multiple threads incrementing x - race condition!
Parallel.ForEach(funcs, f => f());
```

### Solution: Lock
```csharp
var funcs = new List<Func<int>>();
int x = 0;
object lockObj = new object();

for (int i = 0; i < 10; i++)
{
    funcs.Add(() =>
    {
        lock (lockObj)
        {
            return ++x;
        }
    });
}

Parallel.ForEach(funcs, f => f());
```

### Better Solution: Immutable
```csharp
var funcs = new List<Func<int>>();

for (int i = 0; i < 10; i++)
{
    int value = i; // Immutable copy
    funcs.Add(() => value * 2);
}

// No race condition - value never changes
```

---

## Best Practices in This Category

1. **Understand Capture Semantics**: Know what's being captured and why
2. **Loop Closure Fix**: Always create local copy in loops
3. **Resource Cleanup**: Always use using for IDisposable
4. **Thread Safety**: Synchronize or use immutable capture
5. **Clear Intent**: Make captures explicit through naming
6. **Memory Awareness**: Don't capture more than needed
7. **Modern C# Features**: Use C# 8.0+ for cleaner code
8. **Document Closures**: Explain what's captured and why

---

## Exercises

### Exercise 1: Closure Capture
```csharp
public List<Action> Exercise1()
{
    var actions = new List<Action>();
    
    for (int i = 0; i < 3; i++)
    {
        actions.Add(() => Console.WriteLine(i));
    }
    
    return actions;
}

// What prints when you call each action?
// How would you fix it?
```

**Answer**: Prints 3, 3, 3. Fix: Create local copy `int copy = i;`

### Exercise 2: Using Statement
```csharp
public void Exercise2(string filePath)
{
    var reader = File.OpenText(filePath);
    
    string line = reader.ReadLine();
    Console.WriteLine(line);
    
    // Missing: reader.Dispose()
}

// Rewrite using modern using declaration
```

**Solution**: Use `using var reader = ...`

### Exercise 3: Thread-Safe Counter
Design a closure-based counter that's safe for multi-threaded access:
```csharp
public Func<int> CreateThreadSafeCounter()
{
    // Implementation needed
}
```

---

## Related Topics

- **Scope Fundamentals**: Understanding scope boundaries
- **Lifetime and Memory**: How closures affect object lifetime
- **LINQ**: Uses closures extensively
- **Async/Await**: With using and closures

---

## Advanced Topics

### Closure Optimization
```csharp
// Compiler creates display class for closures
// Understand the generated code for performance

// Avoid capturing too much:
// Each captured field = more memory per closure

// Use static methods when no capture needed
```

### Memory Profiling Closures
```csharp
// Use memory profiler to see:
// - Display classes created
// - Objects kept alive
// - Closure memory usage
```

### Expression Trees
```csharp
// Closures work with expression trees too
Expression<Func<int, int>> expr = x => x * factor; // Captures factor
```

---

## Summary

Closures and advanced scope concepts enable powerful functional programming patterns in C#. Key takeaways:

1. **Closures are powerful**: Use them for factories, state machines, callbacks
2. **Loop closure bug is common**: Always create local copy in loops
3. **Using declarations are essential**: For safe resource management
4. **Thread safety matters**: Synchronize or use immutable capture
5. **Understand memory implications**: Closures keep objects alive

Master these concepts to:
- Write elegant functional code
- Avoid classic closure bugs
- Manage resources safely
- Design efficient patterns
- Debug closure-related issues

---

## Next Steps

1. Study each section carefully
2. Try the code examples
3. Run exercises and fix issues
4. Apply closures in your LINQ queries
5. Use using declarations for all resources
6. Move to **Best Practices and Interview** for reinforcement

Keep exploring advanced C# patterns!
