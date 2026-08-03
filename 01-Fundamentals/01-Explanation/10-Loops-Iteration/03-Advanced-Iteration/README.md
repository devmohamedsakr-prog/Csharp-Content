# Advanced Iteration

## Overview

This section covers advanced iteration patterns: generators with yield, LINQ-based iteration, and parallel processing. Explore beyond basic loops.

## Learning Path

### Beginner
- Finish Loop-Fundamentals first
- Finish Loop-Control first
- Then start here

### Intermediate
1. **[Yield & Iterators](01-Yield-Iterators/00-Yield-Iterators.md)** - Start here
   - yield return (lazy evaluation)
   - Custom iterators
   - Generator functions
   - Memory efficiency

2. **[LINQ Iteration](02-LINQ-Iteration/00-LINQ-Iteration.md)** - Declarative style
   - Select, Where, filtering
   - Method chains
   - Query syntax
   - Performance vs loops

3. **[Parallel Iteration](03-Parallel-Iteration/00-Parallel-Iteration.md)** - Multi-threaded
   - Parallel.ForEach
   - Thread safety
   - Performance considerations
   - When to use parallel

### Advanced
- Combine techniques
- Optimize complex scenarios
- Real-world applications

## Quick Reference

### Yield (Lazy Evaluation)

```csharp
// Generate on-demand
public IEnumerable<int> GetNumbers(int count)
{
    for (int i = 0; i < count; i++)
        yield return i; // Lazy - returns one at a time
}

// Usage
foreach (var num in GetNumbers(1000000))
    Console.WriteLine(num); // Memory efficient!
```

### LINQ (Declarative)

```csharp
// Express intent clearly
var result = items
    .Where(x => x.IsActive)
    .Select(x => x.Name)
    .OrderBy(x => x)
    .ToList();
```

### Parallel (Multi-threaded)

```csharp
// Process multiple items concurrently
Parallel.ForEach(largeDataset, item =>
{
    ProcessItem(item);
});
```

## Topics Covered

### Yield & Iterators
- yield return syntax
- yield break
- Lazy vs eager evaluation
- Custom iterators
- Generator functions (Fibonacci, Range, etc.)
- Memory efficiency
- When to use yield

### LINQ Iteration
- Query operators
- Method syntax
- Query syntax
- Deferred execution
- Immediate execution
- Performance considerations
- LINQ vs for/foreach

### Parallel Iteration
- Parallel.ForEach
- Thread safety
- Lock-free patterns
- Partitioning
- Performance tuning
- When NOT to use parallel

## Code Examples

### Example 1: Generator Function

```csharp
// FIBONACCI GENERATOR - Memory efficient
public IEnumerable<int> Fibonacci(int count)
{
    int a = 0, b = 1;
    for (int i = 0; i < count; i++)
    {
        yield return a;
        int temp = a + b;
        a = b;
        b = temp;
    }
}

// Usage - generates on-demand
foreach (var num in Fibonacci(10))
    Console.Write($"{num} ");
// Output: 0 1 1 2 3 5 8 13 21 34
```

### Example 2: LINQ Query

```csharp
// FIND ACTIVE USERS - Clear intent
var activeUsers = users
    .Where(u => u.IsActive)
    .OrderBy(u => u.Name)
    .Select(u => new { u.Id, u.Name })
    .ToList();

// Equivalent loop - less clear
var activeUsers2 = new List<dynamic>();
foreach (var user in users)
{
    if (user.IsActive)
    {
        activeUsers2.Add(new { user.Id, user.Name });
    }
}
// Would need separate sort step
```

### Example 3: Custom Iterator

```csharp
// RANGE ITERATOR
public IEnumerable<int> Range(int start, int end)
{
    for (int i = start; i <= end; i++)
    {
        yield return i;
    }
}

// Usage
foreach (var num in Range(1, 5))
    Console.WriteLine(num);
// Output: 1 2 3 4 5
```

### Example 4: Parallel Processing

```csharp
// PARALLEL IMAGE PROCESSING
public void ProcessImages(string[] imagePaths)
{
    Parallel.ForEach(imagePaths, new ParallelOptions 
    { 
        MaxDegreeOfParallelism = Environment.ProcessorCount 
    },
    imagePath =>
    {
        var image = Image.Load(imagePath);
        image.ApplyFilter();
        image.Save(imagePath);
    });
}
```

### Example 5: Comparison

```csharp
// IMPERATIVE - Manual iteration
var total = 0;
foreach (var item in items)
{
    if (item.Value > 0)
        total += item.Value;
}

// DECLARATIVE - LINQ
var total = items
    .Where(i => i.Value > 0)
    .Sum(i => i.Value);

// Both do same thing, LINQ is clearer
```

## Technique Comparison

| Technique | Use When | Pros | Cons |
|-----------|----------|------|------|
| **Yield** | Lazy, large sequences | Memory efficient | Slight overhead |
| **LINQ** | Complex operations | Readable, chainable | Learning curve |
| **Parallel** | CPU-intensive | Faster (multi-core) | Complexity, overhead |
| **For/Foreach** | Simple iteration | Fast, familiar | Verbose |

## Decision Tree

```
What are you iterating?
├─ Simple collection?
│  ├─ With filtering? → LINQ
│  └─ Just read? → Foreach
│
├─ Large/infinite sequence?
│  └─ → Yield (lazy generation)
│
└─ Heavy computation?
   ├─ CPU-bound? → Parallel
   └─ I/O-bound? → Async
```

## Performance Patterns

### Memory Efficient with Yield
```csharp
// Doesn't load all 1M items at once
var million = GetMillionItems(); // Lazy!
foreach (var item in million)
    Process(item); // Processes one at a time
```

### LINQ Deferred Execution
```csharp
var query = items.Where(x => x.Value > 0); // Not executed yet!
var count = query.Count(); // NOW it executes
```

### Parallel Speedup
```csharp
// Single-threaded: 10 seconds
var result = items.Select(Process).ToList();

// Multi-threaded: ~3 seconds (4 cores)
var result = items.AsParallel().Select(Process).ToList();
```

## Practice Exercises

### Exercise 1: Generator
```csharp
// TODO: Create infinite sequence generator
// Hint: Yield infinite numbers (use break elsewhere)
public IEnumerable<int> InfiniteNumbers()
{
    // Start here
}
```

### Exercise 2: LINQ Chain
```csharp
// TODO: Find top 5 active users by name
var users = GetUsers();
// Use Where, OrderBy, Take
```

### Exercise 3: Parallel Processing
```csharp
// TODO: Process 1000 items in parallel
var items = GetItems();
// Use Parallel.ForEach
```

### Exercise 4: Hybrid Approach
```csharp
// TODO: Combine yield + LINQ
// Create a generator that yields filtered results
public IEnumerable<int> FilteredRange(int start, int end, 
    Func<int, bool> predicate)
{
    // Start here
}
```

## Common Patterns

### Pattern 1: Paging
```csharp
public IEnumerable<T> GetPage<T>(IEnumerable<T> items, 
    int pageNumber, int pageSize)
{
    return items
        .Skip(pageNumber * pageSize)
        .Take(pageSize);
}
```

### Pattern 2: Filtered Iterator
```csharp
public IEnumerable<int> EvensOnly(IEnumerable<int> numbers)
{
    foreach (var num in numbers)
        if (num % 2 == 0)
            yield return num;
}
```

### Pattern 3: Transform Chain
```csharp
var result = data
    .Select(x => x.ToUpper())
    .Where(x => x.Length > 2)
    .Distinct()
    .OrderBy(x => x);
```

## When NOT to Use

### When NOT to use Yield
- Need immediate validation
- Full collection required upfront
- Performance critical (slight overhead)

### When NOT to use LINQ
- Simple loops (over-engineering)
- Ultra-performance critical
- Multiple passes needed (materialize once)

### When NOT to use Parallel
- Small datasets (overhead > benefit)
- I/O-bound operations
- Shared state complexity
- Thread-unsafe operations

## Troubleshooting

### "Yield Not Executing"
- Yield is lazy - need to enumerate
- Wrap in foreach or .ToList()

### "LINQ Too Slow"
- Check for multiple enumerations
- Materialize with .ToList()
- Profile before optimizing

### "Parallel Slower Than Sequential"
- Overhead may exceed benefit
- Measure actual performance
- Check for lock contention

## Next Steps

1. **Read** Yield-Iterators details
2. **Read** LINQ-Iteration patterns
3. **Read** Parallel-Iteration cautions
4. **Practice** exercises
5. **Move to** [Best-Practices-Interview](../04-Best-Practices-Interview/README.md)

## Links

- **Previous**: [Loop-Control](../02-Loop-Control/README.md)
- **Next**: [Best-Practices-Interview](../04-Best-Practices-Interview/README.md)
- **All Topics**: [Loops Overview](../README.md)

---

**Pro Tip**: Start with LINQ for readability. Only use Parallel if you've measured and confirmed it's faster. Use Yield when generating large sequences.
