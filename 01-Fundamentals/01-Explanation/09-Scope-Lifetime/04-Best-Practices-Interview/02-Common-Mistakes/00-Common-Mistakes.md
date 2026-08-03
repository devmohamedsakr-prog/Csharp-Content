# Common Scope and Lifetime Mistakes

## 1. Accessing Variables Out of Scope

### The Mistake
Trying to use a variable outside the block where it was declared.

```csharp
// MISTAKE
public void BadScope()
{
    if (condition)
    {
        int temp = 10;
    }
    
    Console.WriteLine(temp); // COMPILE ERROR: temp not in scope
}

// CORRECT
public void GoodScope()
{
    int temp;
    
    if (condition)
    {
        temp = 10;
    }
    
    Console.WriteLine(temp);
}

// BETTER: Declare close to use
public void BetterScope()
{
    if (condition)
    {
        int temp = 10;
        Console.WriteLine(temp);
    }
}
```

### Why It Happens
Developer forgets that block scope ends at the closing brace. Variables don't persist outside their block.

### Impact
Compile error - caught immediately but wastes time.

---

## 2. Loop Variable Capture in Closures

### The Mistake
Capturing loop variable directly in closures, causing all closures to see the final value.

```csharp
// MISTAKE
public List<Action> BadLoopCapture()
{
    var actions = new List<Action>();
    
    for (int i = 0; i < 5; i++)
    {
        actions.Add(() => Console.WriteLine(i)); // Captures i
    }
    
    return actions; // All print 5!
}

// CORRECT
public List<Action> GoodLoopCapture()
{
    var actions = new List<Action>();
    
    for (int i = 0; i < 5; i++)
    {
        int copy = i; // Create copy
        actions.Add(() => Console.WriteLine(copy));
    }
    
    return actions; // Prints 0, 1, 2, 3, 4
}
```

### Why It Happens
Loop variable is ONE variable for entire loop. Closures capture by reference, not by value.

### Impact
Logic error - hard to debug, wrong values printed or processed.

---

## 3. Variable Shadowing

### The Mistake
Declaring a variable with the same name as an outer scope variable.

```csharp
// MISTAKE
public class Shadowing
{
    private int value = 5;
    
    public void Method()
    {
        int value = 10; // Shadows field!
        
        Console.WriteLine(value); // 10 - but which did we intend?
        Console.WriteLine(this.value); // 5 - the class field
    }
}

// CORRECT: Use distinct names
public class NoShadowing
{
    private int _classValue = 5;
    
    public void Method()
    {
        int localValue = 10; // Clear intent, no shadowing
        Console.WriteLine(localValue); // 10
        Console.WriteLine(_classValue); // 5
    }
}
```

### Why It Happens
Similar variable names used without considering scope hierarchy.

### Impact
Confusion and potential bugs - uses wrong variable accidentally.

---

## 4. Not Disposing Resources

### The Mistake
Forgetting to dispose IDisposable objects, causing resource leaks.

```csharp
// MISTAKE
public void BadDisposal()
{
    var file = File.OpenText("data.txt");
    string content = file.ReadToEnd();
    // file.Dispose() never called - file handle leaked!
}

// CORRECT: Using statement
public void GoodDisposal()
{
    using var file = File.OpenText("data.txt");
    string content = file.ReadToEnd();
    // file.Dispose() called automatically
}

// ALSO CORRECT: Traditional using
public void AlsoGoodDisposal()
{
    using (var file = File.OpenText("data.txt"))
    {
        string content = file.ReadToEnd();
    }
}
```

### Why It Happens
Easy to forget, no compile error, resource leak may not be immediately obvious.

### Impact
Resource leak - files stay open, memory grows, handles exhausted.

---

## 5. Unintended Variable Capture

### The Mistake
Capturing more variables than intended in closures, keeping objects alive unnecessarily.

```csharp
// MISTAKE: Captures entire array
public Func<int> BadCapture(int[] largeArray)
{
    int index = 0;
    
    return () =>
    {
        // Captures entire largeArray!
        return largeArray[index];
    };
    
    // largeArray kept alive by closure
}

// CORRECT: Capture only what needed
public Func<int> GoodCapture(int[] largeArray)
{
    int value = largeArray[0]; // Extract value
    
    return () =>
    {
        // Only captures value
        return value;
    };
}
```

### Why It Happens
Not thinking about what's captured; compiler captures all local variables referenced.

### Impact
Memory leak - unnecessary objects kept alive, GC can't reclaim them.

---

## 6. Forgetting Static Access Modifiers

### The Mistake
Trying to access static members through instance or vice versa.

```csharp
// MISTAKE
public class Counter
{
    public static int Count = 0;
    
    public void Increment()
    {
        Count++; // Should use Counter.Count
        this.Count++; // ERROR: static member, can't use this
    }
}

// CORRECT
public class Counter
{
    public static int TotalCount = 0;
    
    public void Increment()
    {
        Counter.TotalCount++; // Correct static access
    }
}

// BETTER: Understand difference
public class BetterCounter
{
    public static int TotalCount { get; set; } // Shared across all instances
    
    public int InstanceCount { get; set; } // Unique per instance
}
```

### Why It Happens
Confusing static vs instance members, not thinking about scope.

### Impact
Compile errors or logic errors if forced to work around them.

---

## 7. Closure Over Uninitialized Variables

### The Mistake
Capturing variables before they're initialized.

```csharp
// MISTAKE
public Func<int> BadInitialization()
{
    int x; // Declared but not initialized
    
    var func = () => x * 2; // Uses uninitialized x
    
    x = 10; // Initialize after closure created
    
    return func;
}

// CORRECT
public Func<int> GoodInitialization()
{
    int x = 0; // Initialize before capturing
    
    var func = () => x * 2;
    
    x = 10;
    
    return func;
}
```

### Why It Happens
Complex code flow, forgetting variable must be initialized before use.

### Impact
Runtime error or unexpected behavior - variable has default value initially.

---

## 8. Not Using Protected Internal Correctly

### The Mistake
Confusing access modifiers, accidentally exposing implementation details.

```csharp
// MISTAKE: Over-exposed
public class Data
{
    public List<object> _internalCache = new(); // Should be private!
    public void ResetCache() // Should be protected!
    {
        _internalCache.Clear();
    }
}

// CORRECT: Proper encapsulation
public class Data
{
    private List<object> _internalCache = new();
    
    protected void ResetCache()
    {
        _internalCache.Clear();
    }
}
```

### Why It Happens
Not thinking about intended access; defaulting to public.

### Impact
API abuse - external code depends on implementation details, refactoring becomes risky.

---

## 9. Recursive Method Stack Overflow

### The Mistake
Infinite recursion causing stack overflow.

```csharp
// MISTAKE: No base case
public int Factorial(int n)
{
    return n * Factorial(n - 1); // Infinite recursion!
}

// CORRECT: Add base case
public int Factorial(int n)
{
    if (n <= 1) return 1; // Base case
    return n * Factorial(n - 1);
}
```

### Why It Happens
Missing base case, logic error in recursion condition.

### Impact
StackOverflowException at runtime - crash.

---

## 10. Event Handler Memory Leaks

### The Mistake
Not unsubscribing from events, causing memory leaks.

```csharp
// MISTAKE: Event leak
public class DataReceiver
{
    private Publisher _publisher;
    
    public DataReceiver(Publisher publisher)
    {
        _publisher = publisher;
        _publisher.OnData += HandleData; // Subscribe
        // Never unsubscribed - memory leak!
    }
    
    private void HandleData(object sender, EventArgs e)
    {
        Console.WriteLine("Data received");
    }
}

// CORRECT: Unsubscribe
public class DataReceiver : IDisposable
{
    private Publisher _publisher;
    
    public DataReceiver(Publisher publisher)
    {
        _publisher = publisher;
        _publisher.OnData += HandleData;
    }
    
    private void HandleData(object sender, EventArgs e)
    {
        Console.WriteLine("Data received");
    }
    
    public void Dispose()
    {
        _publisher.OnData -= HandleData; // Unsubscribe
    }
}
```

### Why It Happens
Event subscription is subtle; easy to forget to unsubscribe.

### Impact
Memory leak - objects stay alive because they're still event subscribers.

---

## Summary of Common Mistakes

| Mistake | Cause | Impact | Solution |
|---------|-------|--------|----------|
| Out of scope | Forgot block ends | Compile error | Declare near use |
| Loop capture | Forgot capture by reference | Wrong values | Create copy |
| Shadowing | Same name in nested scope | Confusing code | Use distinct names |
| No disposal | Forgot to dispose | Resource leak | Use 'using' |
| Unintended capture | Didn't think about what's captured | Memory leak | Capture explicitly |
| Static confusion | Mixed static/instance | Compile error | Use class name |
| Uninitialized closure | Variable not initialized | Runtime error | Initialize first |
| Access exposure | Default to public | API abuse | Restrict properly |
| Infinite recursion | No base case | Stack overflow | Add base case |
| Event leak | Forgot to unsubscribe | Memory leak | Unsubscribe properly |

## How to Avoid These Mistakes

1. **Use Code Analysis**: Enable compiler warnings and analyzers
2. **Read Error Messages**: Carefully - they usually tell you exactly what's wrong
3. **IDE Warnings**: Pay attention to green squiggles and suggestions
4. **Unit Tests**: Catch logic errors early
5. **Code Review**: Have peers review for common patterns
6. **Learn from Mistakes**: Keep a checklist of your common errors
7. **Use Modern Tools**: Resharper, Roslyn analyzers catch many issues
8. **Static Analysis**: SonarQube, FxCop identify potential problems
9. **Documentation**: Comment non-obvious scope/lifetime decisions
10. **Consistent Patterns**: Follow established team practices

## Practice: Spotting Mistakes

```csharp
// Find the mistakes in this code
public class Exercise
{
    private int _count = 0;
    
    public void ProcessLoop()
    {
        var actions = new List<Action>();
        
        for (int i = 0; i < 3; i++)
        {
            var data = LoadData(i);
            actions.Add(() => Console.WriteLine($"Item {i}: {data}"));
        }
        
        data = null;
        
        foreach (var action in actions)
        {
            action();
        }
    }
    
    public void Cleanup()
    {
        var file = File.OpenText("data.txt");
        string content = file.ReadToEnd();
        Console.WriteLine(content);
    }
}

// Mistakes found:
// 1. Loop variable i captured directly - will print 3 for all
// 2. 'data' accessed outside its scope - compile error
// 3. File not disposed - resource leak
// 4. 'data' redeclared in different iteration - scope issue
```

---

## Conclusion

These mistakes are common but preventable. By understanding scope and lifetime rules, using proper tools and practices, and thinking carefully about variable management, you can avoid most of these pitfalls. When you do make them, learn from the experience and add to your mental checklist to prevent repeating them.
