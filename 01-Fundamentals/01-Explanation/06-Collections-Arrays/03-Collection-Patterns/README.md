# Collection Patterns

## Overview
Master practical patterns for working with collections: choosing the right type, iterating efficiently, and using LINQ effectively.

## Learning Path

### 1. Choosing Collections - Decision Guide
- Selection matrix for each collection type
- Performance characteristics
- Common scenarios
- Trade-off analysis

**Time:** 15-20 minutes

### 2. Iteration Patterns - Multiple Approaches
- For loops with indexing
- Foreach for simple traversal
- While loops for conditional iteration
- LINQ for transformations

**Time:** 20-25 minutes

### 3. LINQ with Collections - Query Patterns
- Filtering with Where
- Transforming with Select
- Grouping and aggregation
- Sorting and ordering

**Time:** 25-30 minutes

## Files in This Section

1. **00-Choosing-Collections.md** - Selection decision guide
2. **00-Iteration-Patterns.md** - For, foreach, while, LINQ
3. **00-LINQ-Collections.md** - Query and transformation

## Quick Decision Guide

```csharp
// Need fast lookup by key? → Dictionary
Dictionary<string, User> users = new Dictionary<string, User>();

// Need unique values only? → HashSet
HashSet<int> uniqueIds = new HashSet<int>(ids);

// Need FIFO processing? → Queue
Queue<Task> tasks = new Queue<Task>();

// Need LIFO processing? → Stack
Stack<State> history = new Stack<State>();

// Need indexed access? → List
List<Item> items = new List<Item>();

// Need array-like fixed size? → Array
int[] numbers = new int[100];
```

## Iteration Patterns

```csharp
// Simple iteration - use foreach
foreach (var item in collection) { }

// Need index - use for loop
for (int i = 0; i < list.Count; i++) { }

// Conditional iteration - use while
while (queue.Count > 0) { }

// Transform/filter - use LINQ
var result = list.Where(x => x > 5).Select(x => x * 2);
```

## LINQ Query Patterns

```csharp
// Filter
var adults = people.Where(p => p.Age >= 18).ToList();

// Transform
var names = people.Select(p => p.Name).ToList();

// Sort
var sorted = people.OrderBy(p => p.Age).ToList();

// Group
var byAge = people.GroupBy(p => p.Age).ToList();

// Aggregate
int total = numbers.Sum();
double average = numbers.Average();

// Combine
var result = people
    .Where(p => p.Age >= 18)
    .OrderBy(p => p.Name)
    .Select(p => p.Name)
    .ToList();
```

## Collection Selection Matrix

| Need | Use | Why |
|------|-----|-----|
| Indexed access | List | O(1) by index |
| Key lookup | Dictionary | O(1) by key |
| Unique values | HashSet | Automatic dedup |
| FIFO processing | Queue | Optimized for it |
| LIFO processing | Stack | Optimized for it |
| Fixed size | Array | Memory efficient |
| Immutable | ImmutableList | Thread-safe |

## Best Practices

✓ Choose collection based on access pattern
✓ Use LINQ for clarity
✓ Materialize LINQ when needed multiple times
✓ Check bounds/keys before access
✓ Don't modify while iterating

## Performance Considerations

```csharp
// O(1) operations - very fast
list[index]              // List access by index
dict[key]                // Dictionary lookup
set.Contains(item)       // HashSet contains
queue.Enqueue/Dequeue    // Queue ops
stack.Push/Pop           // Stack ops

// O(n) operations - slower at scale
list.Contains(item)      // Linear search
list.Remove(item)        // Remove from middle
list.IndexOf(item)       // Find by value

// O(n log n) operations - sorting
list.OrderBy(x => x)     // LINQ sort
Array.Sort(arr)          // Array sort
```

## Common Patterns

### Caching
```csharp
Dictionary<int, User> cache = new Dictionary<int, User>();
if (!cache.TryGetValue(userId, out var user)) {
    user = LoadUser(userId);
    cache[userId] = user;
}
```

### Deduplication
```csharp
var uniqueIds = new HashSet<int>(allIds);
var uniqueList = uniqueIds.ToList();
```

### Frequency Counting
```csharp
var frequency = new Dictionary<string, int>();
foreach (var word in words) {
    if (frequency.TryGetValue(word, out int count)) {
        frequency[word] = count + 1;
    } else {
        frequency[word] = 1;
    }
}
```

### Top-N Selection
```csharp
var topN = items
    .OrderByDescending(x => x.Score)
    .Take(10)
    .ToList();
```

### Pagination
```csharp
int pageSize = 20;
var page = items
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();
```

## Self-Assessment

Can you:
- [ ] Choose right collection for scenario?
- [ ] Use appropriate iteration method?
- [ ] Write efficient LINQ queries?
- [ ] Understand performance implications?
- [ ] Apply common patterns?

---

## Related Topics

- **Arrays** - Fixed collections
- **Generic Collections** - List, Dictionary, HashSet, Queue, Stack
- **Best Practices** - Performance and safety
- **Interview Questions** - Real-world scenarios

## Next Steps

1. ✓ Study Choosing Collections
2. ✓ Master Iteration Patterns
3. ✓ Learn LINQ with Collections
4. → Review Best Practices
5. → Study Interview Questions
