# Data Types Interview - Hard Level Questions

## Question 1: Implement a Generic Repository Pattern

### Question
Design a generic repository pattern that works with both value and reference types. What challenges arise?

### Answer
```csharp
public interface IRepository<T> where T : class {
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    IEnumerable<T> GetAll();
}

public class GenericRepository<T> : IRepository<T> where T : class {
    private readonly Dictionary<int, T> _storage = new();
    
    public T GetById(int id) {
        _storage.TryGetValue(id, out var entity);
        return entity;  // Could be null
    }
    
    public void Add(T entity) {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        var id = _storage.Count + 1;
        _storage[id] = entity;
    }
    
    public void Update(T entity) {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        // In real implementation, would track by ID
        // Issue: How to identify which entity to update?
    }
    
    public void Delete(int id) {
        _storage.Remove(id);
    }
    
    public IEnumerable<T> GetAll() {
        return _storage.Values;
    }
}

// Usage
public class User {
    public string Name { get; set; }
}

IRepository<User> userRepo = new GenericRepository<User>();
userRepo.Add(new User { Name = "Alice" });
```

**Challenges**:
- ✓ Null reference handling
- ✓ Type constraints (T : class)
- ✓ Identity tracking
- ✓ Value types vs reference types
- ✓ Lazy loading vs eager loading

---

## Question 2: Design a Type-Safe Event System

### Question
Design a type-safe event system that prevents common mistakes with delegates.

### Answer
```csharp
public interface IEvent {
    string Name { get; }
}

public class TypeSafeEventBus {
    private class EventHandler {
        public Delegate Handler { get; set; }
        public Type EventType { get; set; }
    }
    
    private readonly Dictionary<string, List<EventHandler>> _handlers = new();
    
    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent {
        var eventName = typeof(TEvent).Name;
        
        if (!_handlers.ContainsKey(eventName)) {
            _handlers[eventName] = new List<EventHandler>();
        }
        
        _handlers[eventName].Add(new EventHandler {
            Handler = handler,
            EventType = typeof(TEvent)
        });
    }
    
    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent {
        var eventName = typeof(TEvent).Name;
        
        if (_handlers.TryGetValue(eventName, out var handlers)) {
            foreach (var handler in handlers.ToList()) {
                try {
                    ((Action<TEvent>)handler.Handler)(@event);
                } catch (Exception ex) {
                    // Log error
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}

// Usage
public class UserCreatedEvent : IEvent {
    public string Name => nameof(UserCreatedEvent);
    public int UserId { get; set; }
}

var bus = new TypeSafeEventBus();
bus.Subscribe<UserCreatedEvent>(e => {
    Console.WriteLine($"User created: {e.UserId}");
});

bus.Publish(new UserCreatedEvent { UserId = 1 });
```

**Benefits**:
- Type safety at compile time
- Prevents signature mismatches
- Error handling per handler
- Proper unsubscribe support

---

## Question 3: Optimize a Large Collection Processing

### Question
You need to process 10 million records. How would you choose data structures and algorithms to minimize memory and CPU?

### Answer
```csharp
// Problem: Process 10M records, find unique users, calculate stats
public class RecordProcessor {
    // Bad approach - loads all in memory
    public void ProcessBad(IEnumerable<Record> records) {
        var list = records.ToList();  // Loads all 10M!
        var unique = list.Select(r => r.UserId).Distinct().ToList();
        var stats = new Dictionary<int, Stats>();
        foreach (var record in list) {
            // Process...
        }
    }
    
    // Good approach - streaming with appropriate collections
    public void ProcessGood(IEnumerable<Record> records) {
        var uniqueUsers = new HashSet<int>();  // O(1) lookups
        var stats = new Dictionary<int, Stats>();  // O(1) access
        
        // Stream processing - don't load all at once
        foreach (var record in records) {
            uniqueUsers.Add(record.UserId);  // Fast unique tracking
            
            if (!stats.ContainsKey(record.UserId)) {
                stats[record.UserId] = new Stats();
            }
            stats[record.UserId].Process(record);
            
            // Could yield partial results for even better memory
        }
    }
    
    // Best approach - truly streaming with custom enumerator
    public IEnumerable<Stats> ProcessStream(IEnumerable<Record> records) {
        var uniqueUsers = new HashSet<int>();
        var buffer = new Dictionary<int, Stats>();
        int bufferSize = 1000;
        
        foreach (var record in records) {
            uniqueUsers.Add(record.UserId);
            
            if (!buffer.ContainsKey(record.UserId)) {
                buffer[record.UserId] = new Stats();
            }
            buffer[record.UserId].Process(record);
            
            // Yield when buffer full
            if (buffer.Count >= bufferSize) {
                foreach (var stats in buffer.Values) {
                    yield return stats;
                }
                buffer.Clear();
            }
        }
        
        // Final batch
        foreach (var stats in buffer.Values) {
            yield return stats;
        }
    }
}

public class Record {
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

public class Stats {
    public int UserId { get; set; }
    public int Count { get; set; }
    public decimal Total { get; set; }
    
    public void Process(Record record) {
        Count++;
        Total += record.Amount;
    }
}
```

**Optimizations**:
- Use `HashSet<T>` for unique tracking (O(1))
- Use `Dictionary<K,V>` for fast access (O(1))
- Stream processing - don't load all in memory
- Buffer and yield results
- Appropriate collection for access patterns

---

## Question 4: Design an Immutable Collections API

### Question
Design an immutable collections API that prevents accidental mutations while maintaining good performance.

### Answer
```csharp
public interface IImmutableList<T> : IEnumerable<T> {
    T this[int index] { get; }
    int Count { get; }
    IImmutableList<T> Add(T item);
    IImmutableList<T> Remove(T item);
    IImmutableList<T> Replace(int index, T item);
}

public class ImmutableList<T> : IImmutableList<T> {
    private readonly T[] _items;
    
    private ImmutableList(T[] items) {
        _items = items;
    }
    
    public static ImmutableList<T> Empty { get; } = new(Array.Empty<T>());
    
    public T this[int index] => _items[index];
    public int Count => _items.Length;
    
    public IImmutableList<T> Add(T item) {
        var newItems = new T[_items.Length + 1];
        Array.Copy(_items, newItems, _items.Length);
        newItems[_items.Length] = item;
        return new ImmutableList<T>(newItems);
    }
    
    public IImmutableList<T> Remove(T item) {
        var index = Array.IndexOf(_items, item);
        if (index < 0) return this;
        
        var newItems = new T[_items.Length - 1];
        Array.Copy(_items, 0, newItems, 0, index);
        Array.Copy(_items, index + 1, newItems, index, _items.Length - index - 1);
        return new ImmutableList<T>(newItems);
    }
    
    public IImmutableList<T> Replace(int index, T item) {
        if (index < 0 || index >= _items.Length)
            throw new IndexOutOfRangeException();
        
        var newItems = (T[])_items.Clone();
        newItems[index] = item;
        return new ImmutableList<T>(newItems);
    }
    
    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_items).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

// Usage
var list = ImmutableList<int>.Empty
    .Add(1)
    .Add(2)
    .Add(3)
    .Replace(1, 5);  // [1, 5, 3]

// No mutations possible - returns new collection
var list2 = list.Remove(1);  // [5, 3]
// list unchanged
```

**Considerations**:
- Returns new collection on modifications
- Original unchanged (true immutability)
- Performance trade-off vs mutable
- Structure sharing possible for optimization

---

## Question 5: Handle Circular References and Memory Leaks

### Question
How would you design types to avoid circular reference memory leaks with event handlers?

### Answer
```csharp
// Problem - circular reference, memory leak
public class Publisher {
    private List<Action> _handlers = new();
    
    public void Subscribe(Subscriber subscriber) {
        _handlers.Add(() => subscriber.OnNotified());  // Keeps subscriber alive
    }
}

public class Subscriber {
    private Publisher _publisher;  // Reference to publisher
    
    public Subscriber(Publisher pub) {
        _publisher = pub;
        _publisher.Subscribe(this);  // Creates cycle
    }
    
    public void OnNotified() { }
}
// Memory leak: Subscriber keeps Publisher alive via _publisher
// Publisher keeps Subscriber alive via delegate
// Neither can be garbage collected

// Solution 1 - Weak References
public class PublisherWeak {
    private List<WeakReference<Action>> _handlers = new();
    
    public void Subscribe(Action handler) {
        _handlers.Add(new WeakReference<Action>(handler));
    }
    
    public void Notify() {
        var toRemove = new List<WeakReference<Action>>();
        
        foreach (var weakRef in _handlers) {
            if (weakRef.TryGetTarget(out var handler)) {
                handler();
            } else {
                toRemove.Add(weakRef);  // Already collected
            }
        }
        
        foreach (var wr in toRemove) {
            _handlers.Remove(wr);
        }
    }
}

// Solution 2 - Explicit Unsubscribe
public class PublisherExplicit {
    private List<Action> _handlers = new();
    
    public IDisposable Subscribe(Action handler) {
        _handlers.Add(handler);
        return new Subscription(this, handler);
    }
    
    private class Subscription : IDisposable {
        private readonly PublisherExplicit _publisher;
        private readonly Action _handler;
        
        public Subscription(PublisherExplicit publisher, Action handler) {
            _publisher = publisher;
            _handler = handler;
        }
        
        public void Dispose() {
            _publisher._handlers.Remove(_handler);
        }
    }
}

// Usage
using (var subscription = publisher.Subscribe(OnNotified)) {
    // Subscription active
}
// Unsubscribed and can be garbage collected
```

**Approaches**:
- Weak references (automatic cleanup)
- Explicit unsubscribe (manual control)
- Events pattern (standard in .NET)

---

## Question 6: Implement a Type-Safe Configuration System

### Question
Design a configuration system that is type-safe and supports different data types.

### Answer
```csharp
public interface IConfigValue { }

public class ConfigValue<T> : IConfigValue {
    public T Value { get; }
    public ConfigValue(T value) => Value = value;
}

public class TypeSafeConfig {
    private readonly Dictionary<string, IConfigValue> _config = new();
    
    public void Set<T>(string key, T value) {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException(nameof(key));
        
        _config[key] = new ConfigValue<T>(value);
    }
    
    public T Get<T>(string key, T defaultValue = default) {
        if (_config.TryGetValue(key, out var configValue)) {
            if (configValue is ConfigValue<T> typedValue) {
                return typedValue.Value;
            }
            throw new InvalidOperationException(
                $"Config key '{key}' has wrong type");
        }
        return defaultValue;
    }
    
    public bool TryGet<T>(string key, out T value) {
        if (_config.TryGetValue(key, out var configValue) &&
            configValue is ConfigValue<T> typedValue) {
            value = typedValue.Value;
            return true;
        }
        value = default;
        return false;
    }
}

// Usage
var config = new TypeSafeConfig();
config.Set("AppName", "MyApp");
config.Set("Timeout", 30);
config.Set("EnableFeature", true);

string appName = config.Get("AppName", "Default");
int timeout = config.Get("Timeout", 60);
bool enabled = config.Get("EnableFeature", false);
```

**Features**:
- Type-safe operations
- Default value support
- TryGet pattern
- Runtime type checking

---

## Hard Questions Summary

These questions test:
- Design skills
- Performance optimization
- Memory management
- Advanced type concepts
- Real-world patterns
- Edge case handling

---

**Final Note**: These hard questions demonstrate advanced thinking. In interviews, clear communication of your thought process matters as much as the solution.

---

**Preparation Complete**: Review all three levels before your interview.
