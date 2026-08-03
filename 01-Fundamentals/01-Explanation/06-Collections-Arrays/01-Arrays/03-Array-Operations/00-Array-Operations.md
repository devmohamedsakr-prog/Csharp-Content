# Array Operations

## Overview
Arrays provide many static methods and properties for manipulation, sorting, searching, and transformation.

## Common Array Methods

### Array.Reverse()
Reverses the order of elements.

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
Array.Reverse(numbers);
// Result: [5, 4, 3, 2, 1]

// Reverse portion
int[] arr = { 1, 2, 3, 4, 5 };
Array.Reverse(arr, 1, 3);  // Reverse indices 1-3
// Result: [1, 4, 3, 2, 5]
```

### Array.Sort()
Sorts elements in ascending order.

```csharp
int[] numbers = { 5, 2, 8, 1, 9 };
Array.Sort(numbers);
// Result: [1, 2, 5, 8, 9]

// Sort strings
string[] names = { "Charlie", "Alice", "Bob" };
Array.Sort(names);
// Result: ["Alice", "Bob", "Charlie"]
```

### Array.Copy()
Copies array elements to another array.

```csharp
int[] source = { 1, 2, 3, 4, 5 };
int[] dest = new int[5];

Array.Copy(source, dest, 5);
// dest: [1, 2, 3, 4, 5]

// Copy portion
int[] partial = new int[3];
Array.Copy(source, 1, partial, 0, 3);
// partial: [2, 3, 4]
```

### Array.Find() and Array.FindAll()
Find elements matching a predicate.

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

// Find first matching
int first = Array.Find(numbers, x => x > 5);  // 6

// Find all matching
int[] evens = Array.FindAll(numbers, x => x % 2 == 0);
// Result: [2, 4, 6, 8]

// Find with index
int index = Array.FindIndex(numbers, x => x > 6);  // 6
```

### Array.IndexOf()
Find index of element.

```csharp
int[] numbers = { 10, 20, 30, 20, 40 };

int index = Array.IndexOf(numbers, 20);  // 1 (first)
int lastIndex = Array.LastIndexOf(numbers, 20);  // 3 (last)

// Returns -1 if not found
int notFound = Array.IndexOf(numbers, 99);  // -1
```

### Array.Exists()
Check if element exists.

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

bool hasEven = Array.Exists(numbers, x => x % 2 == 0);  // true
bool hasNegative = Array.Exists(numbers, x => x < 0);  // false

// Or use simple Contains
bool has5 = numbers.Contains(5);  // true
```

### Array.TrueForAll()
Check if all elements match condition.

```csharp
int[] numbers = { 2, 4, 6, 8 };

bool allEven = Array.TrueForAll(numbers, x => x % 2 == 0);  // true

int[] mixed = { 1, 2, 3, 4 };
bool allPositive = Array.TrueForAll(mixed, x => x > 0);  // true
```

### Array.Resize()
Change array size (creates new array).

```csharp
int[] arr = { 1, 2, 3 };
Array.Resize(ref arr, 5);  // Grows to size 5
// Result: [1, 2, 3, 0, 0]

// Shrink
Array.Resize(ref arr, 2);
// Result: [1, 2]
```

### Array.Clear()
Clear array elements (set to default).

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
Array.Clear(numbers, 0, 5);
// Result: [0, 0, 0, 0, 0]

// Clear portion
int[] arr = { 1, 2, 3, 4, 5 };
Array.Clear(arr, 1, 2);  // Clear indices 1-2
// Result: [1, 0, 0, 4, 5]
```

## LINQ Methods for Arrays

### Select - Transform elements

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// Square each number
int[] squared = numbers.Select(x => x * x).ToArray();
// Result: [1, 4, 9, 16, 25]

// Convert to strings
string[] strings = numbers.Select(x => x.ToString()).ToArray();
```

### Where - Filter elements

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

// Get only even numbers
int[] evens = numbers.Where(x => x % 2 == 0).ToArray();
// Result: [2, 4, 6, 8]
```

### OrderBy - Sort

```csharp
int[] numbers = { 5, 2, 8, 1, 9 };

// Ascending
int[] ascending = numbers.OrderBy(x => x).ToArray();
// Result: [1, 2, 5, 8, 9]

// Descending
int[] descending = numbers.OrderByDescending(x => x).ToArray();
// Result: [9, 8, 5, 2, 1]
```

### Aggregate - Combine elements

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// Sum
int sum = numbers.Aggregate((acc, x) => acc + x);  // 15

// String concatenation
string concat = numbers.Aggregate("", (acc, x) => acc + x);
// Result: "12345"
```

### Any and All

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// Any element matches
bool hasEven = numbers.Any(x => x % 2 == 0);  // true

// All elements match
bool allPositive = numbers.All(x => x > 0);  // true
bool allEven = numbers.All(x => x % 2 == 0);  // false
```

## Practical Patterns

### Pattern 1: Sum and Average

```csharp
int[] scores = { 85, 90, 92, 88, 91 };

int sum = scores.Sum();
double average = scores.Average();
int max = scores.Max();
int min = scores.Min();

Console.WriteLine($"Sum: {sum}");
Console.WriteLine($"Average: {average}");
Console.WriteLine($"Max: {max}");
Console.WriteLine($"Min: {min}");
```

### Pattern 2: Filter and Transform

```csharp
// Get squares of even numbers
int[] numbers = { 1, 2, 3, 4, 5, 6 };

int[] result = numbers
    .Where(x => x % 2 == 0)  // [2, 4, 6]
    .Select(x => x * x)      // [4, 16, 36]
    .ToArray();
```

### Pattern 3: Group and Count

```csharp
int[] numbers = { 1, 2, 2, 3, 3, 3, 4, 4, 4, 4 };

var grouped = numbers
    .GroupBy(x => x)
    .Select(g => new { Value = g.Key, Count = g.Count() })
    .ToArray();

// Result: {Value:1, Count:1}, {Value:2, Count:2}, ...
```

### Pattern 4: Distinct Elements

```csharp
int[] numbers = { 1, 2, 2, 3, 3, 3, 4, 5, 5 };

int[] unique = numbers.Distinct().ToArray();
// Result: [1, 2, 3, 4, 5]
```

## Array Cloning

### Shallow Copy

```csharp
int[] original = { 1, 2, 3 };

// Create shallow copy
int[] copy = (int[])original.Clone();
// or
int[] copy2 = original.ToArray();

copy[0] = 999;
// original[0] still 1 (separate arrays)
```

## Performance Considerations

### Direct Access (Fast)

```csharp
int[] arr = { 1, 2, 3, 4, 5 };
int value = arr[2];  // O(1) - very fast
```

### Linear Search (Slow)

```csharp
int[] arr = { 1, 2, 3, ..., 1000000 };
int index = Array.IndexOf(arr, 500000);  // O(n) - slower
```

### Recommendation

```csharp
// For known index - direct access
int value = arr[index];

// For searching - use LINQ with early exit
int first = arr.FirstOrDefault(x => x > 100);

// For frequent searches - use Dictionary
var dict = arr.ToDictionary(x => x);
```

## Best Practices

✓ **Use LINQ for clarity**
```csharp
var result = numbers
    .Where(x => x > 5)
    .OrderBy(x => x)
    .ToArray();
```

✓ **Check for null before operations**
```csharp
if (arr != null && arr.Length > 0) {
    int first = arr[0];
}
```

✓ **Use Resize sparingly**
```csharp
// Resize is expensive - creates new array
Array.Resize(ref arr, newSize);

// Better - use List if growing frequently
List<int> list = new List<int>();
```

## Summary

- Array.Sort, Reverse, Copy for basic operations
- Array.Find, Exists for searching
- LINQ methods (Where, Select, OrderBy) for transformations
- Performance matters - direct access vs searching
- Prefer LINQ for clarity when performance acceptable

---

## Next Steps

1. Learn Generic Collections (List, Dictionary)
2. Study Iteration Patterns
3. Review Best Practices
