# Collections and Arrays - Medium Interview Questions

## Q1: Design a cache system using appropriate collections

**Answer:**
```csharp
public class CacheSystem<TKey, TValue> {
    private Dictionary<TKey, TValue> cache = 
        new Dictionary<TKey, TValue>();
    private LinkedList<TKey> accessOrder = new LinkedList<TKey>();
    private int maxSize = 100;
    
    public void Set(TKey key, TValue value) {
        if (cache.ContainsKey(key)) {
            cache[key] = value;
        } else {
            if (cache.Count >= maxSize) {
                var lru = accessOrder.First.Value;
                cache.Remove(lru);
                accessOrder.RemoveFirst();
            }
            cache[key] = value;
        }
        accessOrder.AddLast(key);
    }
    
    public bool TryGet(TKey key, out TValue value) {
        return cache.TryGetValue(key, out value);
    }
}
```

**Key insights:** Dictionary for fast lookup, LinkedList for LRU tracking, proper capacity management

---

## Q2: Performance comparison - what's the bottleneck?

```csharp
// Find common elements between two lists - O(n²) SLOW
List<int> list1 = new List<int> { 1, 2, 3, 4, 5 };
List<int> list2 = new List<int> { 4, 5, 6, 7, 8 };

var common = new List<int>();
foreach (var item in list1) {
    if (list2.Contains(item))  // O(n) for each item
        common.Add(item);
}

// Better - O(n) FAST
var set2 = new HashSet<int>(list2);
var common = list1.Where(x => set2.Contains(x)).ToList();
```

**Key concept:** Choose data structure based on access patterns

---

## Q3: LINQ query optimization

```csharp
// Problem - Multiple iterations
var filtered = people.Where(p => p.Age > 30);
int count = filtered.Count();           // Iterate
int firstAge = filtered.First().Age;    // Iterate again

// Solution - Materialize once
var filtered = people.Where(p => p.Age > 30).ToList();
int count = filtered.Count;
int firstAge = filtered.First().Age;

// Or use LINQ for aggregates
var count = people.Count(p => p.Age > 30);
var first = people.FirstOrDefault(p => p.Age > 30);
```

---

## Q4: Complex collection manipulation

```csharp
// Group by department, get average salary
var result = employees
    .GroupBy(e => e.Department)
    .Select(g => new {
        Department = g.Key,
        AverageSalary = g.Average(e => e.Salary),
        Count = g.Count()
    })
    .OrderByDescending(x => x.AverageSalary)
    .ToList();
```

---

## Q5: When to use IEnumerable vs IList vs ICollection

```csharp
// IEnumerable - Read-only, lazy
public IEnumerable<User> GetUsers() {
    yield return user1;
    yield return user2;
}

// IList - Indexed access needed
public IList<User> GetUsersList() {
    return users;
}

// IReadOnlyList - Immutable from caller perspective
public IReadOnlyList<User> GetUsersReadOnly() {
    return users.AsReadOnly();
}

// Each has specific purpose and performance characteristics
```

---

## Q6: Flatten nested collections

```csharp
List<List<int>> matrix = new List<List<int>> {
    new List<int> { 1, 2, 3 },
    new List<int> { 4, 5, 6 },
    new List<int> { 7, 8, 9 }
};

// SelectMany flattens
var flat = matrix.SelectMany(row => row).ToList();
// Result: {1, 2, 3, 4, 5, 6, 7, 8, 9}
```

---

## Q7: Difference between LINQ deferred and immediate execution

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Deferred - Not executed yet
var query = numbers.Where(x => x > 2);

// Executed here - Immediate
foreach (var item in query) { }

// Or force immediate with ToList
var list = query.ToList();

// Understanding: effects how many times iteration happens
```

---

## Q8: Collection equality and hashing

```csharp
// Custom class in Dictionary must override GetHashCode/Equals
public class Person {
    public int Id { get; set; }
    public string Name { get; set; }
    
    public override bool Equals(object obj) {
        return obj is Person p && p.Id == Id;
    }
    
    public override int GetHashCode() {
        return Id.GetHashCode();
    }
}

var dict = new Dictionary<Person, string>();
dict[new Person { Id = 1, Name = "Alice" }] = "data";
```

---

## Q9: Array.Resize vs List growth

```csharp
// Array.Resize - O(n) expensive operation
int[] arr = new int[5];
Array.Resize(ref arr, 10);

// List - O(1) amortized, handles growth internally
List<int> list = new List<int>();
list.Add(1);  // Grows automatically

// Key difference: List is preferred for unknown sizes
```

---

## Q10: LINQ performance - paging pattern

```csharp
public List<T> GetPage<T>(
    IEnumerable<T> source, 
    int pageNumber, 
    int pageSize) {
    
    return source
        .Skip((pageNumber - 1) * pageSize)  // Skip to page
        .Take(pageSize)                       // Take page items
        .ToList();                            // Materialize
}
```

---

## Summary of Medium Concepts

✓ Cache design with Dictionary + LinkedList
✓ Performance optimization with HashSet
✓ LINQ materialization and iterations
✓ Complex grouping and aggregation
✓ Interface selection (IEnumerable, IList)
✓ Flattening with SelectMany
✓ Deferred vs immediate execution
✓ Custom equality and hashing
✓ Array vs List for growth
✓ Pagination patterns

---

## Next Steps

1. Practice implementing these patterns
2. Understand performance tradeoffs
3. Move to Hard questions
