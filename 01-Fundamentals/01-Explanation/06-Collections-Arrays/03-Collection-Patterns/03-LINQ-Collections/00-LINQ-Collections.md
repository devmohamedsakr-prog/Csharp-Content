# LINQ with Collections

## Overview
LINQ (Language Integrated Query) provides a powerful way to query, filter, transform, and aggregate collections using expressive syntax.

## Common LINQ Operations

### Where - Filtering

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

// Get even numbers
var evens = numbers.Where(x => x % 2 == 0);
// Result: {2, 4, 6, 8}

// Multiple conditions
var filtered = numbers.Where(x => x > 3 && x < 8);
// Result: {4, 5, 6, 7}
```

### Select - Transformation

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Square each number
var squared = numbers.Select(x => x * x);
// Result: {1, 4, 9, 16, 25}

// Transform to strings
var strings = numbers.Select(x => $"Value: {x}");
// Result: {"Value: 1", "Value: 2", ...}
```

### OrderBy/OrderByDescending - Sorting

```csharp
List<int> numbers = new List<int> { 5, 2, 8, 1, 9 };

// Ascending
var ascending = numbers.OrderBy(x => x);
// Result: {1, 2, 5, 8, 9}

// Descending
var descending = numbers.OrderByDescending(x => x);
// Result: {9, 8, 5, 2, 1}
```

### GroupBy - Grouping

```csharp
List<int> numbers = new List<int> { 1, 2, 2, 3, 3, 3 };

// Group by value
var grouped = numbers.GroupBy(x => x);
foreach (var group in grouped) {
    Console.WriteLine($"{group.Key}: {group.Count()} items");
}
// Output:
// 1: 1 items
// 2: 2 items
// 3: 3 items
```

### Distinct - Unique Elements

```csharp
List<int> numbers = new List<int> { 1, 2, 2, 3, 3, 3, 4 };

var unique = numbers.Distinct();
// Result: {1, 2, 3, 4}

// Convert to list to materialize
var uniqueList = numbers.Distinct().ToList();
```

### Take and Skip

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// First 3 elements
var first3 = numbers.Take(3);
// Result: {1, 2, 3}

// Skip first 3, take next 3
var middle = numbers.Skip(3).Take(3);
// Result: {4, 5, 6}

// Last 3 elements
var last3 = numbers.TakeLast(3);
// Result: {8, 9, 10}
```

### First, FirstOrDefault, Last

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// First element
int first = numbers.First();  // 1
// Throws if empty

// First or default
int firstOrDef = numbers.FirstOrDefault();  // 1
int firstOrDef2 = (new List<int>()).FirstOrDefault();  // 0 (default)

// Last element
int last = numbers.Last();  // 5
```

### Any and All

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Check if any element matches
bool hasEven = numbers.Any(x => x % 2 == 0);  // true
bool hasNegative = numbers.Any(x => x < 0);   // false

// Check if all match
bool allPositive = numbers.All(x => x > 0);   // true
bool allEven = numbers.All(x => x % 2 == 0);  // false
```

### Count and Sum

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Count
int count = numbers.Count();  // 5
int evenCount = numbers.Count(x => x % 2 == 0);  // 2

// Sum
int sum = numbers.Sum();  // 15
int evenSum = numbers.Where(x => x % 2 == 0).Sum();  // 6
```

### Average, Min, Max

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Average
double avg = numbers.Average();  // 3.0

// Min and Max
int min = numbers.Min();  // 1
int max = numbers.Max();  // 5
```

## Chaining Operations

### Query Composition

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Chain multiple operations
var result = numbers
    .Where(x => x > 3)           // Filter: {4,5,6,7,8,9,10}
    .Select(x => x * 2)          // Transform: {8,10,12,14,16,18,20}
    .Where(x => x < 18)          // Filter again: {8,10,12,14,16}
    .OrderBy(x => x)             // Sort: {8,10,12,14,16}
    .Take(3)                     // First 3: {8,10,12}
    .ToList();                   // Materialize to list
```

## Working with Complex Objects

### Select Properties

```csharp
List<Person> people = new List<Person> {
    new Person { Name = "Alice", Age = 30 },
    new Person { Name = "Bob", Age = 25 },
    new Person { Name = "Charlie", Age = 35 }
};

// Get just names
var names = people.Select(p => p.Name);
// Result: {"Alice", "Bob", "Charlie"}

// Get adults
var adults = people.Where(p => p.Age >= 18).Select(p => p.Name);
```

### Select Many (Flatten)

```csharp
List<List<int>> matrix = new List<List<int>> {
    new List<int> { 1, 2, 3 },
    new List<int> { 4, 5, 6 },
    new List<int> { 7, 8, 9 }
};

// Flatten to single list
var flat = matrix.SelectMany(row => row);
// Result: {1,2,3,4,5,6,7,8,9}
```

## Grouping and Aggregation

### Group with Aggregation

```csharp
List<(string, int)> scores = new List<(string, int)> {
    ("Alice", 90),
    ("Bob", 85),
    ("Alice", 95),
    ("Bob", 88)
};

// Group and get average
var averages = scores
    .GroupBy(x => x.Item1)
    .Select(g => (name: g.Key, avg: g.Average(x => x.Item2)));

foreach (var item in averages) {
    Console.WriteLine($"{item.name}: {item.avg}");
}
```

## Practical LINQ Patterns

### Pattern 1: Top N Items

```csharp
List<int> scores = new List<int> { 95, 87, 92, 88, 91, 85 };

// Top 3 scores
var top3 = scores
    .OrderByDescending(x => x)
    .Take(3)
    .ToList();
// Result: {95, 92, 91}
```

### Pattern 2: Filter and Transform

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6 };

// Get squares of even numbers
var result = numbers
    .Where(x => x % 2 == 0)
    .Select(x => x * x)
    .ToList();
// Result: {4, 16, 36}
```

### Pattern 3: Distinct and Count

```csharp
List<string> words = new List<string> 
    { "apple", "banana", "apple", "cherry", "banana" };

// Count unique words
int uniqueCount = words.Distinct().Count();
// Result: 3
```

### Pattern 4: Dictionary from List

```csharp
List<Person> people = new List<Person> {
    new Person { Id = 1, Name = "Alice" },
    new Person { Id = 2, Name = "Bob" }
};

// Create dictionary
var byId = people.ToDictionary(p => p.Id, p => p.Name);
// Result: {1: "Alice", 2: "Bob"}
```

## Performance Considerations

### Deferred Execution

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Query not executed yet (deferred)
var query = numbers.Where(x => x > 2);

// Query executed here (materialization)
foreach (var item in query) {
    Console.WriteLine(item);
}

// Or materialize explicitly
var list = query.ToList();
```

### Optimization

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// BAD - Multiple enumeration
var evens = numbers.Where(x => x % 2 == 0);
int count1 = evens.Count();
int count2 = evens.Count();  // Iterates again!

// GOOD - Materialize once
var evensList = numbers.Where(x => x % 2 == 0).ToList();
int count1 = evensList.Count;
int count2 = evensList.Count;  // No re-iteration
```

## Best Practices

✓ **Use LINQ for clarity**
```csharp
var result = list
    .Where(x => condition)
    .Select(x => transform)
    .ToList();
```

✓ **Materialize when needed multiple times**
```csharp
var filtered = list.Where(x => x > 5).ToList();
// Can iterate multiple times efficiently
```

✓ **Use method syntax for complex queries**
```csharp
var result = numbers
    .OrderBy(x => x)
    .ThenBy(x => x)  // Secondary sort
    .ToList();
```

## Summary

- **Where** - Filter elements
- **Select** - Transform elements
- **OrderBy/OrderByDescending** - Sort
- **GroupBy** - Group by key
- **Distinct** - Unique elements
- **Count, Sum, Average** - Aggregate
- **Any, All** - Conditions
- Powerful for data manipulation
- Deferred execution (unless materialized)

---

## Next Steps

1. Study Best Practices
2. Learn Common Mistakes
3. Practice Interview Questions
