# Iteration Patterns

## Overview
Different ways to iterate collections, each suited for specific scenarios.

## For Loop Pattern

Use for indexed access and modification:

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Forward iteration
for (int i = 0; i < numbers.Count; i++) {
    Console.WriteLine(numbers[i]);
}

// Backward iteration
for (int i = numbers.Count - 1; i >= 0; i--) {
    Console.WriteLine(numbers[i]);
}

// With index access
for (int i = 0; i < numbers.Count; i++) {
    numbers[i] = numbers[i] * 2;  // Modify elements
}
```

## Foreach Loop Pattern

Use for simple iteration without index:

```csharp
List<string> names = new List<string> { "Alice", "Bob", "Charlie" };

// Simple iteration
foreach (string name in names) {
    Console.WriteLine(name);
}

// Can iterate any IEnumerable
foreach (var item in collection) {
    // Process item
}
```

## While Loop Pattern

Use for conditional iteration:

```csharp
Queue<int> queue = new Queue<int> { 1, 2, 3 };

// Process until empty
while (queue.Count > 0) {
    int item = queue.Dequeue();
    Console.WriteLine(item);
}

// With condition
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
int i = 0;
while (i < numbers.Count && numbers[i] < 4) {
    Console.WriteLine(numbers[i]);
    i++;
}
```

## LINQ Iteration

Use for transforming and filtering:

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Filter then iterate
numbers
    .Where(x => x > 2)
    .ForEach(Console.WriteLine);
// Outputs: 3, 4, 5

// Transform then iterate
numbers
    .Select(x => x * 2)
    .ForEach(x => Console.WriteLine($"Double: {x}"));
```

## Safe Removal During Iteration

### Pattern 1: Iterate Copy

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Iterate copy, modify original
foreach (int num in numbers.ToList()) {
    if (num > 3) {
        numbers.Remove(num);
    }
}
```

### Pattern 2: Use RemoveAll

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Remove all matching
numbers.RemoveAll(x => x > 3);
// Result: [1, 2, 3]
```

### Pattern 3: Backward For Loop

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Iterate backward (safe for removal)
for (int i = numbers.Count - 1; i >= 0; i--) {
    if (numbers[i] > 3) {
        numbers.RemoveAt(i);
    }
}
```

## Enumerate with Index

### Using Select with Index

```csharp
List<string> fruits = new List<string> { "apple", "banana", "cherry" };

// Get index and value
fruits
    .Select((value, index) => new { index, value })
    .ForEach(x => Console.WriteLine($"{x.index}: {x.value}"));
```

### Using Traditional For

```csharp
List<string> fruits = new List<string> { "apple", "banana", "cherry" };

// Index available in for loop
for (int i = 0; i < fruits.Count; i++) {
    Console.WriteLine($"{i}: {fruits[i]}");
}
```

## Multiple Collection Iteration

### Parallel Iteration

```csharp
List<string> names = new List<string> { "Alice", "Bob", "Charlie" };
List<int> ages = new List<int> { 30, 25, 28 };

// Iterate together
for (int i = 0; i < names.Count; i++) {
    Console.WriteLine($"{names[i]}: {ages[i]}");
}

// Using Zip (LINQ)
names.Zip(ages, (name, age) => $"{name}: {age}")
    .ForEach(Console.WriteLine);
```

### Nested Iteration

```csharp
List<List<int>> matrix = new List<List<int>> {
    new List<int> { 1, 2, 3 },
    new List<int> { 4, 5, 6 },
    new List<int> { 7, 8, 9 }
};

// Nested foreach
foreach (var row in matrix) {
    foreach (int value in row) {
        Console.WriteLine(value);
    }
}

// Nested for
for (int r = 0; r < matrix.Count; r++) {
    for (int c = 0; c < matrix[r].Count; c++) {
        Console.WriteLine(matrix[r][c]);
    }
}
```

## Dictionary Iteration

### Iterate KeyValuePairs

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

foreach (var kvp in ages) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
```

### Iterate Keys or Values Only

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

// Just keys
foreach (string name in ages.Keys) {
    Console.WriteLine(name);
}

// Just values
foreach (int age in ages.Values) {
    Console.WriteLine(age);
}
```

## Queue and Stack Iteration

### Queue Iteration (Doesn't Remove)

```csharp
Queue<int> queue = new Queue<int> { 1, 2, 3 };

foreach (int item in queue) {
    Console.WriteLine(item);
}
// Queue unchanged after iteration
```

### Stack Iteration (Doesn't Remove)

```csharp
Stack<int> stack = new Stack<int> { 1, 2, 3 };

foreach (int item in stack) {
    Console.WriteLine(item);  // Top to bottom
}
// Stack unchanged after iteration
```

### Dequeue/Pop Until Empty

```csharp
Queue<int> queue = new Queue<int> { 1, 2, 3 };

// Process and remove
while (queue.Count > 0) {
    int item = queue.Dequeue();
    Console.WriteLine(item);
}
// Queue is now empty
```

## Performance Considerations

### Avoid in Foreach

```csharp
// BAD - O(n) operation in loop
foreach (var item in collection) {
    // Avoid collection modifications
    collection.Add(item);
    collection.Remove(item);
}
```

### Prefer Direct Access

```csharp
// Fast O(1)
int value = list[index];

// Slower O(n)
int value = list.FirstOrDefault(x => x == target);
```

## Best Practices

✓ **Use appropriate iteration method**
```csharp
// Simple traversal
foreach (var item in collection) { }

// Need index
for (int i = 0; i < count; i++) { }

// Need transformation
collection.Select(x => transform(x))
```

✓ **Don't modify collection while iterating foreach**
```csharp
// WRONG
foreach (var item in list) {
    if (condition) list.Remove(item);
}

// RIGHT
foreach (var item in list.ToList()) {
    if (condition) list.Remove(item);
}
```

✓ **Use LINQ for complex iterations**
```csharp
var result = collection
    .Where(x => x > 5)
    .Select(x => x * 2)
    .OrderBy(x => x)
    .ToList();
```

## Summary

- **For** - When need index or modification
- **Foreach** - Simple iteration without index
- **While** - Conditional iteration
- **LINQ** - Transform and filter
- **Safe removal** - Iterate copy or use RemoveAll
- **Don't modify** during foreach iteration

---

## Next Steps

1. Learn LINQ with Collections
2. Study Best Practices
3. Review Common Mistakes
