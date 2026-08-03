# Collections and Arrays - Hard Interview Questions

## Q1: Design an efficient autocomplete system

**Answer:**
```csharp
public class AutocompleteSystem {
    private TrieNode root = new TrieNode();
    private Dictionary<string, int> frequency = 
        new Dictionary<string, int>();
    
    public void AddWord(string word) {
        frequency[word] = frequency.TryGetValue(word, out int count) 
            ? count + 1 
            : 1;
    }
    
    public List<string> GetSuggestions(string prefix) {
        // Find all words with prefix
        return frequency
            .Where(kvp => kvp.Key.StartsWith(prefix))
            .OrderByDescending(kvp => kvp.Value)  // By frequency
            .ThenBy(kvp => kvp.Key)               // Alphabetical
            .Take(5)
            .Select(kvp => kvp.Key)
            .ToList();
    }
}
```

**Key concepts:** Dictionary for frequency counting, LINQ for sorting and filtering, top-N pattern

---

## Q2: Implement LRU Cache

```csharp
public class LRUCache<TKey, TValue> {
    private Dictionary<TKey, LinkedListNode<(TKey, TValue)>> map;
    private LinkedList<(TKey, TValue)> lruList;
    private int capacity;
    
    public LRUCache(int cap) {
        capacity = cap;
        map = new Dictionary<TKey, LinkedListNode<(TKey, TValue)>>();
        lruList = new LinkedList<(TKey, TValue)>();
    }
    
    public TValue Get(TKey key) {
        if (!map.TryGetValue(key, out var node))
            throw new KeyNotFoundException();
        
        lruList.Remove(node);
        lruList.AddLast(node);
        return node.Value.Item2;
    }
    
    public void Put(TKey key, TValue value) {
        if (map.ContainsKey(key)) {
            lruList.Remove(map[key]);
        } else if (map.Count >= capacity) {
            var first = lruList.First;
            lruList.RemoveFirst();
            map.Remove(first.Value.Item1);
        }
        
        var node = lruList.AddLast((key, value));
        map[key] = node;
    }
}
```

**Design decisions:** Dictionary for O(1) access, LinkedList for O(1) eviction, combined for LRU behavior

---

## Q3: Analyze collection performance for large datasets

```csharp
// Scenario: Find all duplicates in 1M integers

// O(n²) - AVOID
var duplicates = new List<int>();
for (int i = 0; i < numbers.Count; i++) {
    for (int j = i + 1; j < numbers.Count; j++) {
        if (numbers[i] == numbers[j]) {
            duplicates.Add(numbers[i]);
        }
    }
}

// O(n) - GOOD
var seen = new HashSet<int>();
var duplicates = new HashSet<int>();
foreach (var num in numbers) {
    if (!seen.Add(num))  // Add returns false if exists
        duplicates.Add(num);
}

// Memory: HashSet uses more memory but provides O(1) lookup
// Time: O(n) vs O(n²) massive difference at scale
```

---

## Q4: Design thread-safe collection wrapper

```csharp
public class ThreadSafeList<T> {
    private List<T> innerList = new List<T>();
    private readonly object lockObj = new object();
    
    public void Add(T item) {
        lock (lockObj) {
            innerList.Add(item);
        }
    }
    
    public List<T> GetSnapshot() {
        lock (lockObj) {
            return new List<T>(innerList);  // Return copy
        }
    }
    
    public bool TryGetAt(int index, out T item) {
        lock (lockObj) {
            if (index >= 0 && index < innerList.Count) {
                item = innerList[index];
                return true;
            }
            item = default;
            return false;
        }
    }
}

// Thread safety is crucial in production systems
```

---

## Q5: Optimize group-by-count pattern

```csharp
// Find top 10 most frequent elements - millions of items

List<int> data = /* 10M integers */;

// Inefficient - Groups everything
var topNGrouping = data
    .GroupBy(x => x)
    .OrderByDescending(g => g.Count())
    .Take(10)
    .ToList();

// Better - Use Dictionary for O(1) counting
var frequency = new Dictionary<int, int>();
foreach (var item in data) {
    if (frequency.TryGetValue(item, out int count)) {
        frequency[item] = count + 1;
    } else {
        frequency[item] = 1;
    }
}

var top10 = frequency
    .OrderByDescending(kvp => kvp.Value)
    .Take(10)
    .Select(kvp => kvp.Key)
    .ToList();

// LINQ is less efficient for this pattern
// Direct Dictionary iteration is faster at scale
```

---

## Q6: Handle circular buffer pattern efficiently

```csharp
public class CircularBuffer<T> {
    private T[] buffer;
    private int head;
    private int count;
    
    public CircularBuffer(int capacity) {
        buffer = new T[capacity];
    }
    
    public void Add(T item) {
        buffer[head] = item;
        head = (head + 1) % buffer.Length;
        if (count < buffer.Length) count++;
    }
    
    public T[] GetItems() {
        var result = new T[count];
        for (int i = 0; i < count; i++) {
            int index = (head - count + i + buffer.Length) % buffer.Length;
            result[i] = buffer[index];
        }
        return result;
    }
}

// Array-based for efficiency, modulo arithmetic for wrapping
```

---

## Q7: Multi-collection aggregation

```csharp
// Join data from 3 collections, calculate aggregate

var users = new List<User> { /* ... */ };
var orders = new List<Order> { /* ... */ };
var items = new List<Item> { /* ... */ };

// Efficient approach using HashSet/Dictionary
var userIds = new HashSet<int>(users.Select(u => u.Id));
var userOrders = orders
    .Where(o => userIds.Contains(o.UserId))
    .GroupBy(o => o.UserId)
    .ToDictionary(g => g.Key, g => g.ToList());

var result = users
    .Where(u => userOrders.ContainsKey(u.Id))
    .Select(u => new {
        User = u,
        OrderCount = userOrders[u.Id].Count,
        TotalValue = userOrders[u.Id].Sum(o => o.Value)
    })
    .ToList();
```

---

## Q8: Immutable vs mutable collection trade-offs

```csharp
// Mutable - Fast, shared state
List<int> mutableList = new List<int> { 1, 2, 3 };
mutableList.Add(4);  // O(1)

// Immutable - Slower but safe in concurrent scenarios
ImmutableList<int> immList = ImmutableList.Create(1, 2, 3);
immList = immList.Add(4);  // Creates new instance

// Trade-off: Performance vs thread safety and predictability
// Use immutable in: shared state, async code, functional programming
// Use mutable in: single-threaded, performance-critical code
```

---

## Q9: Benchmark collection operations

```csharp
// Compare different approaches for finding element

var sw = Stopwatch.StartNew();

// Approach 1: List with Contains - O(n)
for (int i = 0; i < iterations; i++) {
    list.Contains(target);
}
sw.Stop();
Console.WriteLine($"List.Contains: {sw.ElapsedMilliseconds}ms");

// Approach 2: HashSet - O(1)
sw.Restart();
for (int i = 0; i < iterations; i++) {
    hashSet.Contains(target);
}
sw.Stop();
Console.WriteLine($"HashSet.Contains: {sw.ElapsedMilliseconds}ms");

// HashSet dramatically faster at scale
// This demonstrates importance of collection choice
```

---

## Q10: Design high-performance filtering pipeline

```csharp
public class FilterPipeline<T> {
    private List<Func<T, bool>> filters = new List<Func<T, bool>>();
    
    public void AddFilter(Func<T, bool> filter) {
        filters.Add(filter);
    }
    
    public IEnumerable<T> Apply(IEnumerable<T> source) {
        // Combine filters - return early if any fails
        return source.Where(item => 
            filters.All(f => f(item))  // All filters must pass
        );
    }
    
    // More efficient: Compose filters
    public Func<T, bool> ComposeFilters() {
        return item => filters.All(f => f(item));
    }
}

// LINQ allows lazy evaluation - items filtered on-demand
// Composition is more efficient than multiple Where calls
```

---

## Summary of Hard Concepts

✓ LRU Cache with Dictionary + LinkedList
✓ Performance optimization at scale (O(n) vs O(n²))
✓ Thread safety in collections
✓ Circular buffers with modulo arithmetic
✓ Multi-collection joins and aggregation
✓ Immutable vs mutable trade-offs
✓ Benchmarking and profiling
✓ Lazy evaluation with LINQ
✓ Complex real-world patterns
✓ Architecture decisions for scalability

---

## Next Steps

1. Study production systems design
2. Practice implementing these patterns
3. Understand trade-offs deeply
4. Review performance characteristics
