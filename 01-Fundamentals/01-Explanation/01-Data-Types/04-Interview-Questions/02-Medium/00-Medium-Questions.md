# Data Types Interview - Medium Level Questions

## Question 1: Design a Type for Storing Money

### Question
How would you design a type in C# to store money amounts? What considerations would you make?

### Answer
```csharp
public readonly struct Money {
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money(decimal amount, string currency) {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative");
        if (string.IsNullOrWhiteSpace(currency)) 
            throw new ArgumentException("Currency required");
        
        Amount = amount;
        Currency = currency;
    }
    
    // Value equality
    public override bool Equals(object obj) {
        return obj is Money m && 
               m.Amount == Amount && 
               m.Currency == Currency;
    }
    
    public override int GetHashCode() {
        return HashCode.Combine(Amount, Currency);
    }
    
    // Operators
    public static Money operator +(Money a, Money b) {
        if (a.Currency != b.Currency) 
            throw new InvalidOperationException("Cannot add different currencies");
        return new Money(a.Amount + b.Amount, a.Currency);
    }
}
```

**Considerations**:
- ✓ Use `readonly struct` for immutability
- ✓ Use `decimal` for precise amounts
- ✓ Validate inputs in constructor
- ✓ Include currency information
- ✓ Implement `Equals` for value semantics
- ✓ Implement operators for arithmetic
- ✓ Small enough for efficient copying

---

## Question 2: When Would You Use HashSet Instead of List?

### Question
When would you choose `HashSet<T>` over `List<T>`? Explain with performance implications.

### Answer
**Use HashSet when**:
- Checking membership frequently
- Need unique items only
- Order doesn't matter

**Performance Comparison**:
```csharp
// List - O(n) membership check
List<int> list = new(1000000);
bool contains = list.Contains(500000);  // Scans entire list

// HashSet - O(1) membership check
HashSet<int> set = new(1000000);
bool contains = set.Contains(500000);   // Direct lookup

// Practical difference in loop
List<int> approved = new() { 1, 2, 3, 4, 5 };
for (int i = 0; i < 1000000; i++) {
    if (approved.Contains(i)) {  // O(n) each time - very slow
        // Process
    }
}

HashSet<int> approvedSet = new(approved);
for (int i = 0; i < 1000000; i++) {
    if (approvedSet.Contains(i)) {  // O(1) each time - much faster
        // Process
    }
}
```

**Decision Table**:
| Need | Use | Reason |
|------|-----|--------|
| Frequent lookups | HashSet | O(1) vs O(n) |
| Maintain order | List | Lists preserve insertion order |
| Remove by value | Either | Both support Remove |
| Unique items | HashSet | Enforces uniqueness |
| Index access | List | HashSet has no indexer |

---

## Question 3: Explain Nullable Reference Types

### Question
What are nullable reference types? How do they improve code safety?

### Answer
```csharp
#nullable enable

public class Customer {
    // Non-nullable - must have value
    public string Name { get; set; }
    
    // Nullable - can be null
    public string? MiddleName { get; set; }
    
    public void Process() {
        // This is safe
        int length = Name.Length;
        
        // This requires null check
        if (MiddleName is not null) {
            int length = MiddleName.Length;
        }
        
        // This is a warning at compile time
        // string middle = MiddleName;  // Warning!
    }
}
```

**Benefits**:
- Compile-time nullability warnings
- Clearer API contracts
- Fewer NullReferenceExceptions
- Self-documenting code

**Without nullable reference types**:
```csharp
public class Customer {
    public string Name { get; set; }  // Could be null?
    public string MiddleName { get; set; }  // Could be null?
}
// Ambiguous - unclear what can be null
```

---

## Question 4: What Collection Would You Use For?

### Question
For each scenario, which collection would you choose and why?

### Answer
**Scenario 1**: Cache user data by ID
```csharp
Dictionary<int, User> userCache = new();
// Key-value lookup: O(1)
```

**Scenario 2**: Process items in order received
```csharp
Queue<Task> taskQueue = new();
task = taskQueue.Dequeue();  // FIFO
```

**Scenario 3**: Undo/Redo functionality
```csharp
Stack<Action> undoStack = new();
action = undoStack.Pop();  // LIFO
```

**Scenario 4**: Unique error codes
```csharp
HashSet<string> errorCodes = new();
if (!errorCodes.Add(errorCode)) {  // Duplicate
    // Handle duplicate
}
```

**Scenario 5**: Sorted by key
```csharp
SortedList<string, int> ages = new();
// Always sorted by key
```

**Scenario 6**: Simple list of items
```csharp
List<string> items = new();
// Default choice for ordered collection
```

---

## Question 5: How Does String Interning Work?

### Question
Explain string interning in C#. When does it happen and what are the implications?

### Answer
**String Interning** - Optimization where identical strings share memory.

```csharp
string s1 = "Hello";
string s2 = "Hello";
string s3 = "Hel" + "lo";

// Interning - all reference same object
Console.WriteLine(ReferenceEquals(s1, s2));  // true (interned)
Console.WriteLine(ReferenceEquals(s1, s3));  // true (interned)

// Constructed strings might not be interned
string s4 = new string(new[] { 'H', 'e', 'l', 'l', 'o' });
Console.WriteLine(ReferenceEquals(s1, s4));  // false (different object)

// Force interning
string interned = string.Intern(s4);
Console.WriteLine(ReferenceEquals(s1, interned));  // true (now interned)
```

**Implications**:
- Reduces memory for many identical strings
- `.Intern()` adds CPU overhead
- Automatic for string literals
- Use-case: Limited benefit for most applications

---

## Question 6: Compare Dictionary vs SortedDictionary

### Question
When would you use `Dictionary<K,V>` vs `SortedDictionary<K,V>`?

### Answer
| Aspect | Dictionary | SortedDictionary |
|--------|-----------|------------------|
| **Ordering** | Unordered | Sorted by key |
| **Lookup** | O(1) | O(log n) |
| **Insert** | O(1) | O(log n) |
| **Memory** | Less | More |
| **Iteration** | Random order | Sorted order |

**Example**:
```csharp
// Fast lookups, unordered
var dict = new Dictionary<int, string> {
    { 3, "Three" },
    { 1, "One" },
    { 2, "Two" }
};

foreach (var kvp in dict) {
    Console.WriteLine(kvp.Key);  // Order: 3, 1, 2
}

// Slower but sorted
var sorted = new SortedDictionary<int, string> {
    { 3, "Three" },
    { 1, "One" },
    { 2, "Two" }
};

foreach (var kvp in sorted) {
    Console.WriteLine(kvp.Key);  // Order: 1, 2, 3
}
```

**Choice**:
- **Dictionary**: Default, needs fast lookups
- **SortedDictionary**: Need sorted order

---

## Question 7: Design a Cache with Expiration

### Question
How would you design a simple cache that automatically expires entries after a time period?

### Answer
```csharp
public class ExpiringCache<TKey, TValue> {
    private class CacheEntry {
        public TValue Value { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
    
    private readonly Dictionary<TKey, CacheEntry> _cache = new();
    private readonly TimeSpan _defaultExpiry;
    
    public ExpiringCache(TimeSpan defaultExpiry) {
        _defaultExpiry = defaultExpiry;
    }
    
    public void Set(TKey key, TValue value) {
        _cache[key] = new CacheEntry {
            Value = value,
            ExpiryTime = DateTime.UtcNow.Add(_defaultExpiry)
        };
    }
    
    public bool TryGet(TKey key, out TValue value) {
        if (_cache.TryGetValue(key, out var entry)) {
            if (DateTime.UtcNow < entry.ExpiryTime) {
                value = entry.Value;
                return true;
            }
            // Expired - remove
            _cache.Remove(key);
        }
        value = default;
        return false;
    }
}

// Usage
var cache = new ExpiringCache<string, string>(TimeSpan.FromMinutes(5));
cache.Set("user1", "data1");

if (cache.TryGet("user1", out var data)) {
    Console.WriteLine(data);
}
```

**Considerations**:
- ✓ Simple time-based expiration
- ✓ Dictionary for O(1) lookups
- ✓ Lazy cleanup (on access)
- ✓ Generic design

---

## Question 8: When to Use ReadOnlyCollection vs List

### Question
Why would you return `IReadOnlyList<T>` instead of `List<T>` from a property?

### Answer
```csharp
// Bad - exposes internal list
public class Team {
    public List<string> Members { get; set; }  // Can be modified externally!
}

var team = new Team { Members = new() { "Alice" } };
team.Members.Add("Bob");
team.Members.Clear();  // Disaster!

// Good - returns read-only collection
public class Team {
    private readonly List<string> _members = new();
    
    public IReadOnlyList<string> Members => _members.AsReadOnly();
    
    public void AddMember(string name) {
        if (!string.IsNullOrWhiteSpace(name)) {
            _members.Add(name);
        }
    }
}

var team = new Team();
team.AddMember("Alice");
team.AddMember("Bob");

// Cannot modify from outside
// team.Members.Add("Charlie");  // Won't compile!
```

**Benefits**:
- Encapsulation - control over internal data
- Contract - clear that collection shouldn't be modified
- Validation - control additions through methods
- Thread safety - easier to make thread-safe

---

## Question 9: Explain Covariance and Contravariance

### Question
What is covariance and contravariance in C#? Show examples.

### Answer
**Covariance** - Can assign derived type to base type (out parameter)
```csharp
IEnumerable<string> strings = GetStrings();
IEnumerable<object> objects = strings;  // Covariant
// Can treat strings as objects
```

**Contravariance** - Can assign base type to derived type (in parameter)
```csharp
Action<object> processObject = Console.WriteLine;
Action<string> processString = processObject;  // Contravariant
processString("Hello");  // Works - object can handle anything
```

**Practical Example**:
```csharp
// Covariant - read-only
IEnumerable<Animal> animals = new List<Dog> { new Dog() };
foreach (Animal animal in animals) {
    animal.MakeSound();  // Works
}

// Contravariant - write-only
Action<Animal> processAnimal = a => Console.WriteLine(a.Name);
Action<Dog> processDog = processAnimal;
processDog(new Dog { Name = "Buddy" });  // Works
```

---

## Medium Questions Summary

Key concepts tested:
- Design decisions
- Collection selection
- Performance implications
- Memory considerations
- Real-world scenarios
- Code patterns

**Progress**: Complete Easy questions before moving to Hard.

---

**Next**: Move to [Hard Questions](../03-Hard/00-Hard-Questions.md)
