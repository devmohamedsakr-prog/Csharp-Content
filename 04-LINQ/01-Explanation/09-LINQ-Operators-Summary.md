# LINQ Operators Summary - Quick Reference

## Overview
Quick reference guide for all standard LINQ operators organized by category.

## Filtering Operators

### Where
```csharp
// Filter elements by condition
var result = numbers.Where(n => n > 5);
// Equivalent: from n in numbers where n > 5 select n
```

### OfType
```csharp
// Filter by type
var strings = objects.OfType<string>();
```

### Distinct
```csharp
// Remove duplicates
var unique = numbers.Distinct();
```

### Except, Intersect, Union
```csharp
var except = list1.Except(list2);      // In list1, not in list2
var intersect = list1.Intersect(list2); // In both lists
var union = list1.Union(list2);         // In either list
```

### Skip, Take
```csharp
var skip3 = numbers.Skip(3);           // Skip first 3
var first5 = numbers.Take(5);          // Take first 5
var pagination = numbers.Skip(10).Take(5); // Items 10-15
```

### SkipWhile, TakeWhile
```csharp
var skipSmall = numbers.SkipWhile(n => n < 5);  // Skip while < 5
var takeSmall = numbers.TakeWhile(n => n < 5);  // Take while < 5
```

## Projection Operators

### Select
```csharp
// Transform each element
var doubled = numbers.Select(n => n * 2);
var names = people.Select(p => p.Name);
```

### SelectMany
```csharp
// Flatten nested collections
var allItems = orders.SelectMany(o => o.Items);
// Equivalent: from order in orders from item in order.Items select item
```

### Cast
```csharp
// Cast all to type (throws if incompatible)
var ints = objects.Cast<int>();
```

## Ordering Operators

### OrderBy, OrderByDescending
```csharp
var ascending = people.OrderBy(p => p.Age);
var descending = people.OrderByDescending(p => p.Age);
```

### ThenBy, ThenByDescending
```csharp
var sorted = people.OrderBy(p => p.Department)
                   .ThenBy(p => p.Name);
```

### Reverse
```csharp
var reversed = numbers.Reverse();
```

## Grouping Operators

### GroupBy
```csharp
// Group by key
var groups = people.GroupBy(p => p.Department);
// Equivalent: from p in people group p by p.Department
```

### ToLookup
```csharp
// Optimized for lookups
var lookup = people.ToLookup(p => p.Department);
var dept = lookup["IT"]; // Fast lookup
```

## Join Operators

### Join
```csharp
// Inner join
var joined = authors.Join(
    books,
    a => a.Id,
    b => b.AuthorId,
    (a, b) => new { a.Name, b.Title }
);
```

### GroupJoin
```csharp
// Left join behavior
var grouped = authors.GroupJoin(
    books,
    a => a.Id,
    b => b.AuthorId,
    (a, books) => new { a.Name, books }
);
```

## Aggregation Operators

### Count, LongCount
```csharp
int count = numbers.Count();
int evenCount = numbers.Count(n => n % 2 == 0);
```

### Sum, Average, Min, Max
```csharp
int sum = numbers.Sum();
int sum10 = numbers.Sum(n => n * 10);
double avg = numbers.Average();
int min = numbers.Min();
int max = numbers.Max();
```

### MinBy, MaxBy (C# 10+)
```csharp
var youngest = people.MinBy(p => p.Age);
var oldest = people.MaxBy(p => p.Age);
```

### Aggregate
```csharp
// Custom aggregation
int product = numbers.Aggregate(1, (acc, n) => acc * n);
var concatenated = words.Aggregate((acc, w) => acc + " " + w);
```

## Quantifier Operators

### Any
```csharp
bool hasAny = numbers.Any();
bool hasEven = numbers.Any(n => n % 2 == 0);
```

### All
```csharp
bool allPositive = numbers.All(n => n > 0);
```

### Contains
```csharp
bool contains5 = numbers.Contains(5);
```

## Element Operators

### First, FirstOrDefault
```csharp
int first = numbers.First();
int firstEven = numbers.First(n => n % 2 == 0);
int firstOrDefault = numbers.FirstOrDefault();
int firstOrDefault0 = numbers.FirstOrDefault(n => n > 100, 0); // C# 9+
```

### Last, LastOrDefault
```csharp
int last = numbers.Last();
int lastOrDefault = numbers.LastOrDefault();
```

### Single, SingleOrDefault
```csharp
int single = singleList.Single();
int singleOrDefault = items.SingleOrDefault();
```

### ElementAt, ElementAtOrDefault
```csharp
int fifth = numbers.ElementAt(4);
int orDefault = numbers.ElementAtOrDefault(100);
```

## Set Operators

### Distinct
```csharp
var unique = numbers.Distinct();
```

### Union, Intersect, Except
```csharp
var union = list1.Union(list2);
var intersect = list1.Intersect(list2);
var except = list1.Except(list2);
```

## Conversion Operators

### ToList, ToArray, ToHashSet
```csharp
List<int> list = query.ToList();
int[] array = query.ToArray();
HashSet<int> set = query.ToHashSet();
```

### ToDictionary
```csharp
Dictionary<int, Person> byId = people.ToDictionary(p => p.Id);
```

### ToLookup
```csharp
ILookup<string, Person> byDept = people.ToLookup(p => p.Department);
```

### AsEnumerable, AsQueryable
```csharp
IEnumerable<int> e = query.AsEnumerable();
IQueryable<int> q = query.AsQueryable();
```

## Generation Operators

### Range, Repeat, Empty
```csharp
var range = Enumerable.Range(1, 5);        // 1,2,3,4,5
var repeated = Enumerable.Repeat("x", 3);  // "x", "x", "x"
var empty = Enumerable.Empty<int>();
```

## Partitioning Operators

### Skip, Take
```csharp
var middle = items.Skip(5).Take(10);
```

### TakeWhile, SkipWhile
```csharp
var taken = items.TakeWhile(x => x < 100);
var skipped = items.SkipWhile(x => x < 100);
```

### Chunk (C# 11+)
```csharp
var chunks = items.Chunk(10); // Groups of 10
```

## Operators Reference Table

| Category | Operators |
|----------|-----------|
| Filtering | Where, OfType, Distinct, Except, Intersect, Union, Skip, Take, SkipWhile, TakeWhile |
| Projection | Select, SelectMany, Cast |
| Ordering | OrderBy, OrderByDescending, ThenBy, ThenByDescending, Reverse |
| Grouping | GroupBy, ToLookup |
| Joining | Join, GroupJoin |
| Aggregation | Count, Sum, Average, Min, Max, Aggregate, MinBy, MaxBy |
| Quantifiers | Any, All, Contains |
| Elements | First, Last, Single, ElementAt, FirstOrDefault, LastOrDefault, SingleOrDefault, ElementAtOrDefault |
| Sets | Distinct, Union, Intersect, Except |
| Conversion | ToList, ToArray, ToDictionary, ToLookup, ToHashSet, AsEnumerable, AsQueryable, Cast, OfType |
| Generation | Range, Repeat, Empty |
| Partitioning | Skip, Take, SkipWhile, TakeWhile, Chunk |

## Common Combinations

### Filter, Project, Order
```csharp
var result = items
    .Where(x => x.IsActive)
    .Select(x => x.Name)
    .OrderBy(x => x)
    .ToList();
```

### Group and Aggregate
```csharp
var stats = items
    .GroupBy(x => x.Category)
    .Select(g => new
    {
        Category = g.Key,
        Count = g.Count(),
        Average = g.Average(x => x.Value)
    });
```

### Join and Select
```csharp
var result = authors.Join(
    books,
    a => a.Id,
    b => b.AuthorId,
    (a, b) => new { a.Name, b.Title }
).OrderBy(x => x.Name);
```

## Performance Notes

| Operator | Performance |
|----------|-------------|
| Where | O(n) - single pass |
| Select | O(n) - single pass |
| GroupBy | O(n) - hash lookup |
| OrderBy | O(n log n) - sorting |
| Join | O(n + m) - hash lookup |
| Distinct | O(n) - with hash |
| Contains | O(1) with HashSet, O(n) with List |
| First | O(1) - short circuits |
| Any | O(1) - short circuits |

## Deferred vs Immediate

| Operator | Deferred | Immediate |
|----------|----------|-----------|
| Where, Select | ✓ | |
| OrderBy, GroupBy | ✓ | |
| ToList, ToArray | | ✓ |
| Count, First | | ✓ |
| Any, All | | ✓ |

## Best Practices

1. **Know Which Operators Short-Circuit**
```csharp
// Short-circuit: stops at first match
var found = items.FirstOrDefault(x => x.Id == 5);
var hasAny = items.Any(x => x.IsActive);

// Non-short-circuit: checks all
var count = items.Count(x => x.IsActive);
```

2. **Use Appropriate Operators for Intent**
```csharp
// Get first 5
var first5 = items.Take(5);

// Skip first, take 5
var next5 = items.Skip(5).Take(5);

// Remove duplicates
var unique = items.Distinct();
```

3. **Chain Operators Efficiently**
```csharp
// Good: Filter before projection
var result = items.Where(x => x.IsActive).Select(x => x.Name);

// Bad: Project everything
var result = items.Select(x => x.Name).Where(x => x != null);
```

## Quick Summary
- 50+ LINQ operators available
- Filtering: Where, OfType, Distinct
- Projection: Select, SelectMany
- Ordering: OrderBy, ThenBy
- Grouping: GroupBy, ToLookup
- Joining: Join, GroupJoin
- Aggregation: Count, Sum, Average
- Quantifiers: Any, All, Contains
- Elements: First, Last, Single
- Know which short-circuit
- Filter before projecting
- Chain operators efficiently

## Resources
- Standard LINQ Query Operators
- LINQ Method Syntax vs Query Syntax
- Query Performance Tuning
- Deferred Execution in LINQ
