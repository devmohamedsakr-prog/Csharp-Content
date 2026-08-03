# Hard Interview Questions: Scope and Lifetime

## Q11: Design a Thread-Safe Factory with Closures

### Question
Design a factory that creates thread-safe functions using closures. Handle multi-threaded access properly.

### Answer

**The Challenge**: Create a factory that generates closures with captured state that's safe for multi-threaded access.

```csharp
public class ThreadSafeFactory
{
    private object _lockObject = new object();
    private Dictionary<string, object> _state = new();
    
    /// <summary>
    /// Creates a thread-safe function that increments a counter.
    /// Each created function has its own captured counter.
    /// </summary>
    public Func<int> CreateCounter(string name, int initialValue = 0)
    {
        // IMPORTANT: Create new variables for each call
        // If we didn't, all counters would share state!
        int counter = initialValue;
        object counterLock = new object();
        
        return () =>
        {
            lock (counterLock)
            {
                return ++counter;
            }
        };
    }
    
    /// <summary>
    /// Creates multiple independent counters.
    /// </summary>
    public IEnumerable<Func<int>> CreateCounters(int count)
    {
        var counters = new List<Func<int>>();
        
        for (int i = 0; i < count; i++)
        {
            // CRITICAL: Create local copy each iteration
            int initialValue = i;
            
            int localCounter = 0;
            object localLock = new object();
            
            counters.Add(() =>
            {
                lock (localLock)
                {
                    return ++localCounter;
                }
            });
        }
        
        return counters;
    }
}

// Usage
public class ThreadSafeExample
{
    public static void Main()
    {
        var factory = new ThreadSafeFactory();
        
        // Each counter is independent with its own lock
        var counter1 = factory.CreateCounter("Counter1");
        var counter2 = factory.CreateCounter("Counter2");
        
        var tasks = new List<Task>();
        
        // Increment counter1 from multiple threads
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => counter1()));
        }
        
        // Increment counter2 from different threads
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => counter2()));
        }
        
        Task.WaitAll(tasks.ToArray());
        
        Console.WriteLine($"Counter1: {counter1()}"); // 11
        Console.WriteLine($"Counter2: {counter2()}"); // 11
    }
}
```

### Key Concepts

1. **Per-Closure Lock**: Each closure has its own lock
2. **Local Copies**: Create new variables in each iteration
3. **Thread Safety**: Lock before accessing shared state
4. **Independence**: Each closure is completely separate

### Common Mistakes
- Sharing lock across closures
- Not creating copy in loop
- Not synchronizing access
- Assuming GC handles thread safety

### Advanced Pattern: Immutable Closures

```csharp
public class ImmutableFactory
{
    /// <summary>
    /// Create immutable closures that can't have race conditions.
    /// </summary>
    public Func<int> CreateImmutableCounter(int value)
    {
        // Closure captures immutable value
        // No synchronization needed!
        return () => value;
    }
    
    public Func<string> CreateGreeter(string name)
    {
        // Immutable string captured
        return () => $"Hello, {name}!";
    }
}
```

---

## Q12: Analyze Memory and Performance Implications

### Question
Compare these two approaches. Which is better and why?

```csharp
// Approach 1: Create closure in loop (NAIVE)
public List<Func<string>> Approach1()
{
    var funcs = new List<Func<string>>();
    var hugeData = new byte[10_000_000]; // 10MB
    
    for (int i = 0; i < 100; i++)
    {
        funcs.Add(() => $"Item {i}"); // Captures hugeData!
    }
    
    return funcs;
}

// Approach 2: Extract pattern (OPTIMIZED)
public List<Func<string>> Approach2()
{
    var funcs = new List<Func<string>>();
    
    for (int i = 0; i < 100; i++)
    {
        int copy = i;
        funcs.Add(() => $"Item {copy}");
    }
    
    return funcs;
}
```

### Answer

**Approach 1 Problems:**
- Captures entire `hugeData` array in each closure
- 100 closures each hold reference to 10MB
- Keeps 1GB in memory unnecessarily
- GC can't collect `hugeData` until all closures are garbage

**Approach 2 Benefits:**
- Only captures `copy` (4 bytes)
- Minimal memory footprint per closure
- `hugeData` can be collected after method returns
- Total memory: ~400 bytes for closures

### Memory Comparison

```
Approach 1:
Display class 1: [i, hugeData reference] 10MB+ per closure
Display class 2: [i, hugeData reference] 10MB+ per closure
...
Total: 100+ closures × 10MB = 1GB+

Approach 2:
Display class 1: [copy=0] 4 bytes
Display class 2: [copy=1] 4 bytes
...
Total: 100 closures × 4 bytes = 400 bytes
```

### Design Principles

```csharp
// PRINCIPLE 1: Be explicit about what you capture
public Func<int> GoodCapture()
{
    int value = 10;
    return () => value * 2; // Clear: captures only value
}

public Func<int> BadCapture()
{
    int value = 10;
    var hugeObject = new HugeData();
    return () => value * 2; // Captures both! Inefficient
}

// PRINCIPLE 2: Use parameters instead of capture when possible
public Func<int> BetterThanCapture(int value)
{
    return x => x * value; // Must capture value parameter
}

// PRINCIPLE 3: Limit closure lifetime
public void ShortLivedClosure()
{
    Func<int> func;
    {
        int x = 10;
        func = () => x * 2; // Captures x
    } // x still alive because func references it
    
    Console.WriteLine(func()); // Still works
}
```

### Performance Implications

1. **Allocation**: Each closure creates hidden display class on heap
2. **Memory**: Large captured objects kept alive longer
3. **GC Pressure**: More objects = more GC work
4. **Cache Locality**: Scattered objects hurt cache performance

### Optimization Techniques

```csharp
// Technique 1: Use value types when possible
public Func<int> FastClosure()
{
    int value = 10; // Value type on stack initially
    return () => value * 2;
}

// Technique 2: Extract shared state
private int _sharedValue = 10;
public Func<int> OptimizedClosure()
{
    return () => _sharedValue * 2; // Captures 'this'
}

// Technique 3: Use static methods (no capture)
private static int Multiply(int x) => x * 2;
public Func<int> NoCapture()
{
    return Multiply; // No closure at all!
}
```

---

## Q13: Inherit and Override Scope Rules

### Question
What happens with this inheritance scenario? Explain the scope resolution.

```csharp
public class Parent
{
    protected int Value = 5;
    
    protected virtual void PrintValue()
    {
        Console.WriteLine($"Parent: {Value}");
    }
}

public class Child : Parent
{
    private int Value = 10; // NEW - not override
    
    public override void PrintValue()
    {
        Console.WriteLine($"Child: {Value}");
    }
    
    public void TestScope()
    {
        // Which Value is accessible here?
        // How can we access both?
    }
}
```

### Answer

```csharp
public class Child : Parent
{
    private int Value = 10; // NEW field - shadows parent's field
    
    public override void PrintValue()
    {
        // In Child: access 'Value' (the new private int)
        Console.WriteLine($"Child: {Value}"); // Prints 10
        
        // Can still access parent's Value
        Console.WriteLine($"Parent: {base.Value}"); // Prints 5
    }
    
    public void TestScope()
    {
        // This.Value refers to Child's field
        Console.WriteLine(this.Value); // 10
        
        // base.Value accesses parent's field
        Console.WriteLine(base.Value); // 5
        
        // Can't do this in non-virtual context:
        // Parent p = this;
        // p.Value would access Parent's Value
    }
    
    public void DemonstratePolymorphism()
    {
        Parent p = this;
        p.PrintValue(); // Calls Child.PrintValue() - prints "Child: 10"
        
        Child c = this;
        c.PrintValue(); // Calls Child.PrintValue() - prints "Child: 10"
    }
}

public class MainProgram
{
    public static void Main()
    {
        var child = new Child();
        child.TestScope();
        // Output:
        // Child: 10
        // Parent: 5
        // 10
        // 5
        
        child.PrintValue();
        // Output: Child: 10
        
        Parent parentRef = child;
        parentRef.PrintValue();
        // Output: Child: 10 (polymorphic dispatch)
    }
}
```

### Key Concepts

1. **Field Shadowing**: Child.Value shadows Parent.Value
2. **base Keyword**: Access parent's members explicitly
3. **this Keyword**: Access current class's members
4. **Polymorphism**: Virtual method calls use actual type

### Scope Resolution Table

| Reference | Value Field | PrintValue() Method |
|-----------|------------|-------------------|
| Child obj | Child.Value (10) | Child.PrintValue() |
| Parent obj (pointing to Child) | Parent.Value (5) | Child.PrintValue() |
| base.Value | Parent.Value (5) | N/A |
| this.Value | Child.Value (10) | N/A |

### Design Lessons

```csharp
// GOOD: Use different names to avoid confusion
public class GoodChild : Parent
{
    private int _childValue = 10; // Different name
    
    protected override void PrintValue()
    {
        Console.WriteLine($"Parent: {Value}");
        Console.WriteLine($"Child: {_childValue}");
    }
}

// Avoid: Shadowing causes confusion
public class ConfusedChild : Parent
{
    private int Value = 10; // Shadows parent!
    
    protected override void PrintValue()
    {
        Console.WriteLine(Value); // Which one?
    }
}
```

---

## Q14: Design Pattern: Resource Pool with Scope

### Question
Design a thread-safe object pool that manages scope and lifetime properly.

### Answer

```csharp
public class ObjectPool<T> : IDisposable
where T : IDisposable, new()
{
    private readonly Stack<T> _availableObjects = new();
    private readonly HashSet<T> _checkedOutObjects = new();
    private readonly object _lockObject = new object();
    private int _maxPoolSize;
    private int _totalCreated = 0;
    
    public ObjectPool(int initialSize, int maxSize)
    {
        _maxPoolSize = maxSize;
        
        for (int i = 0; i < initialSize; i++)
        {
            _availableObjects.Push(new T());
            _totalCreated++;
        }
    }
    
    public PooledObject<T> AcquireObject()
    {
        lock (_lockObject)
        {
            T obj = _availableObjects.Count > 0 
                ? _availableObjects.Pop() 
                : CreateNew();
            
            _checkedOutObjects.Add(obj);
            
            // Return wrapped object that auto-returns to pool
            return new PooledObject<T>(obj, this);
        }
    }
    
    private T CreateNew()
    {
        if (_totalCreated >= _maxPoolSize)
            throw new InvalidOperationException("Pool exhausted");
        
        _totalCreated++;
        return new T();
    }
    
    internal void ReturnObject(T obj)
    {
        lock (_lockObject)
        {
            _checkedOutObjects.Remove(obj);
            _availableObjects.Push(obj);
        }
    }
    
    public void Dispose()
    {
        lock (_lockObject)
        {
            while (_availableObjects.Count > 0)
            {
                _availableObjects.Pop().Dispose();
            }
            
            foreach (var obj in _checkedOutObjects)
            {
                obj.Dispose();
            }
            
            _checkedOutObjects.Clear();
        }
    }
}

public class PooledObject<T> : IDisposable
where T : IDisposable
{
    private T _object;
    private ObjectPool<T> _pool;
    
    public PooledObject(T obj, ObjectPool<T> pool)
    {
        _object = obj;
        _pool = pool;
    }
    
    public T Object => _object;
    
    public void Dispose()
    {
        if (_object != null)
        {
            _pool.ReturnObject(_object);
            _object = null;
        }
    }
}

// Usage
public class PoolExample
{
    public static void Main()
    {
        using var pool = new ObjectPool<DatabaseConnection>(5, 10);
        
        // Acquire and use objects
        using (var pooledObj = pool.AcquireObject())
        {
            DatabaseConnection conn = pooledObj.Object;
            conn.DoWork();
        } // Auto-returned to pool
        
        using (var pooledObj = pool.AcquireObject())
        {
            DatabaseConnection conn = pooledObj.Object;
            conn.DoWork();
        } // Same connection from pool!
    }
}

public class DatabaseConnection : IDisposable
{
    public void DoWork() { }
    public void Dispose() { }
}
```

### Design Concepts

1. **Thread Safety**: Lock during acquisition/return
2. **Lifetime Management**: Pool owns lifetime until return
3. **Automatic Return**: PooledObject wrapper returns on disposal
4. **Resource Limits**: Prevent unbounded object creation
5. **Proper Cleanup**: Dispose all objects when pool disposed

---

## Q15: Multi-Level Analysis: Real-World Complexity

### Question
You have a complex codebase. Identify all scope/lifetime issues:

```csharp
public class EventPublisher
{
    private List<Action<string>> _subscribers = new();
    private Thread _worker;
    
    public void Subscribe(Action<string> handler)
    {
        _subscribers.Add(handler);
    }
    
    public void Start()
    {
        _worker = new Thread(() =>
        {
            while (true)
            {
                foreach (var handler in _subscribers)
                {
                    handler("Data");
                }
                Thread.Sleep(1000);
            }
        });
        
        _worker.Start();
    }
}

public class Client
{
    private EventPublisher _publisher;
    
    public Client(EventPublisher publisher)
    {
        _publisher = publisher;
        _publisher.Subscribe(OnData); // Subscribe
    }
    
    private void OnData(string data)
    {
        Console.WriteLine(data);
    }
}

// Usage
var publisher = new EventPublisher();
publisher.Start();

for (int i = 0; i < 5; i++)
{
    new Client(publisher); // Clients created but not stored
}

Thread.Sleep(5000);
// What happens?
```

### Issues Found

1. **Memory Leak**: Clients subscribe but are never unsubscribed
2. **Event Handler Closure**: Client.OnData keeps Client alive
3. **No Thread Shutdown**: Worker thread never stops
4. **No Disposal**: Resources not cleaned up
5. **Race Condition**: _subscribers modified during iteration

### Corrected Version

```csharp
public class EventPublisher : IDisposable
{
    private List<Action<string>> _subscribers = new();
    private Thread _worker;
    private CancellationTokenSource _cts;
    private bool _disposed = false;
    
    public void Subscribe(Action<string> handler)
    {
        lock (_subscribers)
        {
            _subscribers.Add(handler);
        }
    }
    
    public void Unsubscribe(Action<string> handler)
    {
        lock (_subscribers)
        {
            _subscribers.Remove(handler);
        }
    }
    
    public void Start()
    {
        _cts = new CancellationTokenSource();
        _worker = new Thread(() =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                List<Action<string>> handlersCopy;
                lock (_subscribers)
                {
                    handlersCopy = new List<Action<string>>(_subscribers);
                }
                
                foreach (var handler in handlersCopy)
                {
                    handler("Data");
                }
                
                Thread.Sleep(1000);
            }
        });
        
        _worker.Start();
    }
    
    public void Dispose()
    {
        if (_disposed) return;
        
        _cts?.Cancel();
        _worker?.Join();
        _cts?.Dispose();
        
        _disposed = true;
    }
}

public class Client : IDisposable
{
    private EventPublisher _publisher;
    private Action<string> _handler;
    
    public Client(EventPublisher publisher)
    {
        _publisher = publisher;
        _handler = OnData;
        _publisher.Subscribe(_handler);
    }
    
    private void OnData(string data)
    {
        Console.WriteLine(data);
    }
    
    public void Dispose()
    {
        _publisher.Unsubscribe(_handler);
    }
}

// Better Usage
var publisher = new EventPublisher();
publisher.Start();

var clients = new List<Client>();
for (int i = 0; i < 5; i++)
{
    clients.Add(new Client(publisher));
}

Thread.Sleep(5000);

// Cleanup
foreach (var client in clients)
{
    client.Dispose();
}
publisher.Dispose();
```

### Lessons

1. Always implement IDisposable for resources
2. Use locks for shared collections
3. Unsubscribe from events
4. Stop background threads properly
5. Don't keep short-lived objects when scope ends
6. Make defensive copies for iteration

---

## Summary: Hard Concepts

| Concept | Complexity |
|---------|-----------|
| Thread-safe closures | Multiple scopes + threads |
| Memory optimization | Closure + GC analysis |
| Inheritance scope | Virtual + base keywords |
| Object pooling | Lifetime + thread safety |
| Real-world complexity | Combining all concepts |

## Expert Checklist

- [ ] Design thread-safe factories
- [ ] Analyze memory implications
- [ ] Understand inheritance scope
- [ ] Implement object pools
- [ ] Find issues in complex code
- [ ] Apply best practices systematically

Congratulations! You've mastered scope and lifetime concepts at an expert level.
