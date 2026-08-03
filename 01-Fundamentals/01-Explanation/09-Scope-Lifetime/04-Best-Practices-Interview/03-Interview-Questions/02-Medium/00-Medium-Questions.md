# Medium Interview Questions: Scope and Lifetime

## Q6: Identify and Fix the Loop Closure Bug

### Question
What's wrong with this code? How would you fix it?

```csharp
public List<Action> CreateActions()
{
    var actions = new List<Action>();
    
    for (int i = 0; i < 3; i++)
    {
        actions.Add(() => Console.WriteLine($"Action {i}"));
    }
    
    return actions;
}

public static void Main()
{
    var actions = CreateActions();
    actions[0](); // What prints?
    actions[1](); // What prints?
    actions[2](); // What prints?
}
```

### Answer

**The Bug**: All three actions print "Action 3"

**Why**: The loop variable `i` is captured by the closures. All three closures reference the SAME `i` variable, which ends up being 3 after the loop.

**Expected**: Actions 0, 1, 2
**Actual**: Actions 3, 3, 3

### Solution 1: Create a Local Copy

```csharp
public List<Action> CreateActionsFixed()
{
    var actions = new List<Action>();
    
    for (int i = 0; i < 3; i++)
    {
        int copy = i; // Create new variable for each iteration
        actions.Add(() => Console.WriteLine($"Action {copy}"));
    }
    
    return actions;
}
```

### Solution 2: Use Foreach

```csharp
public List<Action> CreateActionsWithForeach()
{
    var actions = new List<Action>();
    
    foreach (int i in new[] { 0, 1, 2 })
    {
        // In C# 5.0+, foreach creates new variable each iteration
        actions.Add(() => Console.WriteLine($"Action {i}"));
    }
    
    return actions;
}
```

### Solution 3: Use LINQ

```csharp
public List<Action> CreateActionsWithLinq()
{
    return Enumerable.Range(0, 3)
        .Select(i => (Action)(() => Console.WriteLine($"Action {i}")))
        .ToList();
}
```

### Key Points
- Loop variable is captured BY REFERENCE, not by value
- All closures see the final value of the loop variable
- Solution: Create a local copy or use foreach/LINQ
- This is one of the most common closure bugs

### Follow-up
"Why does foreach work but for doesn't?"
In C# 5.0+, foreach creates a new loop variable for each iteration, so each closure gets its own copy. The for loop uses a single variable for the entire loop.

---

## Q7: What Will This Code Print? Explain the Memory State

### Question
Trace through this code and explain what prints at each step:

```csharp
public void TraceClosures()
{
    int x = 5;
    
    Func<int> func1 = () => x * 2;
    Func<void> modify = () => x = 10;
    Func<int> func2 = () => x + 5;
    
    Console.WriteLine(func1()); // ?
    modify();
    Console.WriteLine(func1()); // ?
    Console.WriteLine(func2()); // ?
    
    x = 20;
    Console.WriteLine(func1()); // ?
}
```

### Answer

**Output:**
```
10   // func1: 5 * 2
20   // func1: 10 * 2
15   // func2: 10 + 5
40   // func1: 20 * 2
```

**Explanation:**
All three functions capture the SAME `x` variable. When any function modifies `x`, all see the change.

```
Step 1: x = 5
        func1() = 5 * 2 = 10

Step 2: modify() sets x = 10
        func1() = 10 * 2 = 20
        func2() = 10 + 5 = 15

Step 3: x = 20
        func1() = 20 * 2 = 40
```

### Memory Diagram

```
Stack:
  x (int): 5 -> 10 -> 20
  func1 --|
  func2 --|---> Captured x (same variable)
  modify-|

All functions see changes to x
```

### Key Points
- Closures capture variables, not values
- All closures sharing a variable see changes
- Useful for shared state patterns
- Can cause bugs if not careful

---

## Q8: Design a Cache Class That Prevents Memory Leaks

### Question
Design a class that caches data but can be disposed properly. Avoid memory leaks.

### Answer

```csharp
public class CachedDataService : IDisposable
{
    private Dictionary<string, object> _cache;
    private Timer _cleanupTimer;
    private bool _disposed = false;
    
    public CachedDataService()
    {
        _cache = new Dictionary<string, object>();
        
        // Periodic cleanup to prevent unbounded growth
        _cleanupTimer = new Timer(
            callback: _ => ClearExpiredEntries(),
            state: null,
            dueTime: TimeSpan.FromMinutes(5),
            period: TimeSpan.FromMinutes(5)
        );
    }
    
    public void AddToCache(string key, object value)
    {
        ThrowIfDisposed();
        _cache[key] = value;
    }
    
    public object GetFromCache(string key)
    {
        ThrowIfDisposed();
        return _cache.TryGetValue(key, out var value) ? value : null;
    }
    
    private void ClearExpiredEntries()
    {
        // Implementation of expiration logic
        // Remove old entries to prevent memory bloat
    }
    
    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(CachedDataService));
    }
    
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
            _cleanupTimer?.Dispose();
            _cache?.Clear();
            _cache = null;
        }
        
        _disposed = true;
    }
    
    ~CachedDataService()
    {
        Dispose(false);
    }
}

// Usage
using var cache = new CachedDataService();
cache.AddToCache("key1", "value1");
var value = cache.GetFromCache("key1");
// Properly disposed when using block ends
```

### Key Points
- Implement IDisposable for cleanup
- Clear caches in Dispose()
- Add automatic expiration to prevent growth
- Throw ObjectDisposedException if used after disposal
- Always use `using` when working with disposable objects

### Common Mistakes to Avoid
- Unbounded cache growth
- Forgetting to dispose timers
- Not clearing references in Dispose
- No protection against use-after-disposal

---

## Q9: Explain How Event Handlers Can Cause Memory Leaks

### Question
How can event handlers cause memory leaks? Show the problem and solution.

### Answer

**The Problem:**

```csharp
public class DataModel
{
    public event EventHandler DataChanged;
    
    public void RaiseDataChanged()
    {
        DataChanged?.Invoke(this, EventArgs.Empty);
    }
}

public class UIComponent
{
    private DataModel _model;
    
    public UIComponent(DataModel model)
    {
        _model = model;
        
        // Subscribe to event
        _model.DataChanged += OnDataChanged;
        
        // Problem: UIComponent now held alive by event subscription!
        // Even if UIComponent goes out of scope, it's still referenced
        // by the DataModel's event delegate
    }
    
    private void OnDataChanged(object sender, EventArgs e)
    {
        Console.WriteLine("Data changed!");
    }
}

// Memory leak: UIComponent can't be garbage collected while subscribed
var model = new DataModel();
var ui = new UIComponent(model);
ui = null; // Still referenced by event handler!
// ui is not garbage collected because model keeps it alive
```

**The Solution:**

```csharp
public class UIComponent : IDisposable
{
    private DataModel _model;
    private bool _disposed = false;
    
    public UIComponent(DataModel model)
    {
        _model = model;
        _model.DataChanged += OnDataChanged; // Subscribe
    }
    
    private void OnDataChanged(object sender, EventArgs e)
    {
        Console.WriteLine("Data changed!");
    }
    
    public void Dispose()
    {
        if (_disposed) return;
        
        _model.DataChanged -= OnDataChanged; // Unsubscribe!
        _disposed = true;
    }
}

// Usage
using (var ui = new UIComponent(model))
{
    // Use UI component
} // Disposed here - unsubscribed from event
// Now UIComponent can be garbage collected
```

### Why This Happens

1. UIComponent subscribes to event
2. Event handler stored in DataModel's event delegate
3. DataModel keeps UIComponent alive
4. UIComponent can't be garbage collected
5. Memory leak!

### Prevention Checklist
- [ ] Always unsubscribe from events
- [ ] Use IDisposable to guarantee unsubscription
- [ ] Implement Dispose(bool) properly
- [ ] Test by nullifying references and checking GC

---

## Q10: What Happens in This Multi-Method Scenario?

### Question
Trace through the scope and memory state at each step:

```csharp
public class ScopeChallenge
{
    private List<int> _classData = new();
    
    public void MethodA()
    {
        int localA = 10;
        MethodB(localA);
        
        Console.WriteLine($"A: {localA}"); // What prints?
    }
    
    public void MethodB(int param)
    {
        param = param * 2;
        
        if (param > 15)
        {
            int localB = 30;
            _classData.Add(localB);
        }
        
        Console.WriteLine($"B: {param}"); // What prints?
    }
    
    public void Main()
    {
        MethodA();
        Console.WriteLine($"Class: {_classData[0]}"); // What prints?
    }
}
```

### Answer

**Output:**
```
B: 20
A: 10
Class: 30
```

**Explanation:**

```
MethodA() called:
  localA = 10 (Stack frame A)
  
MethodB(10) called:
  param = 10 (value copy - Stack frame B)
  param = 20 (modified in B's scope)
  
  20 > 15? Yes
    localB = 30 (Stack frame B)
    _classData.Add(30) (adds to heap list)
  
  Print "B: 20"
  MethodB returns - Stack frame B destroyed
  param and localB no longer exist
  
Back in MethodA:
  localA still 10 (value types don't share)
  Print "A: 10"
  MethodA returns - Stack frame A destroyed

Main():
  _classData still has [30] (list on heap)
  Print "Class: 30"
```

### Key Points
- Value types passed by value (copied)
- Each method has own stack frame
- Class fields persist across methods
- Stack frames destroyed when method returns
- Heap objects persist until unreferenced

---

## Summary of Medium Topics

| Topic | Key Concept |
|-------|------------|
| Loop Closure | Capture variable by reference, not value |
| Shared Closures | Multiple closures see same variable changes |
| Cache Design | Prevent unbounded growth with disposal |
| Event Handlers | Remember to unsubscribe to prevent leaks |
| Stack Frames | Each method call gets separate scope |

## Self-Check

Before moving to Hard questions:
- [ ] Understand and fix loop closure bugs
- [ ] Trace closure variable changes
- [ ] Design disposable cache classes
- [ ] Prevent event handler memory leaks
- [ ] Trace multi-method scope scenarios
- [ ] Explain stack frame lifecycle

Ready for the challenge of Hard questions!
