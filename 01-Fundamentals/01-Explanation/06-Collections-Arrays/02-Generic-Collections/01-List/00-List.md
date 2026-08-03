# List<T> - Dynamic Collections

## Overview
List<T> is a dynamic collection that grows as needed. It's the most commonly used collection for storing ordered sequences of elements.

## Creating Lists

### Basic Creation

```csharp
// Empty list
List<int> numbers = new List<int>();

// With capacity hint
List<string> names = new List<string>(10);

// With initialization
List<int> values = new List<int> { 1, 2, 3, 4, 5 };

// Generic type inference
var items = new List<string> { "apple", "banana", "cherry" };
```

## Adding Elements

### Add Single Element

```csharp
List<string> fruits = new List<string>();
fruits.Add("apple");
fruits.Add("banana");
fruits.Add("cherry");
// List: ["apple", "banana", "cherry"]
```

### Add Range

```csharp
List<int> numbers = new List<int> { 1, 2, 3 };
numbers.AddRange(new[] { 4, 5, 6 });
// List: [1, 2, 3, 4, 5, 6]

numbers.AddRange(new List<int> { 7, 8, 9 });
```

### Insert at Index

```csharp
List<string> items = new List<string> { "a", "b", "d" };
items.Insert(2, "c");  // Insert "c" at index 2
// List: ["a", "b", "c", "d"]
```

## Removing Elements

### Remove by Value

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 2, 4 };
numbers.Remove(2);  // Removes first occurrence
// List: [1, 3, 2, 4]
```

### Remove at Index

```csharp
List<string> items = new List<string> { "a", "b", "c", "d" };
items.RemoveAt(1);  // Remove element at index 1
// List: ["a", "c", "d"]
```

### Remove Matching Elements

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
numbers.RemoveAll(x => x > 3);  // Remove all elements > 3
// List: [1, 2, 3]
```

### Clear List

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
numbers.Clear();
// List is now empty
```

## Accessing Elements

### By Index

```csharp
List<string> names = new List<string> { "Alice", "Bob", "Charlie" };

string first = names[0];      // "Alice"
string last = names[^1];      // "Charlie" (index from end)
string middle = names[1];     // "Bob"

// Modify by index
names[0] = "Amy";
```

### Count

```csharp
List<int> numbers = new List<int> { 1, 2, 3 };
int count = numbers.Count;  // 3

// Check if empty
if (numbers.Count > 0) {
    int first = numbers[0];
}
```

### Contains

```csharp
List<string> fruits = new List<string> { "apple", "banana", "cherry" };
bool hasApple = fruits.Contains("apple");  // true
bool hasOrange = fruits.Contains("orange");  // false
```

### Find Element

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Find first matching
int first = numbers.FirstOrDefault(x => x > 3);  // 4

// Find index
int index = numbers.IndexOf(3);  // 2
int lastIndex = numbers.LastIndexOf(3);  // 2
```

## Iterating Lists

### Foreach Loop

```csharp
List<string> fruits = new List<string> { "apple", "banana", "cherry" };

foreach (string fruit in fruits) {
    Console.WriteLine(fruit);
}
```

### For Loop

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

for (int i = 0; i < numbers.Count; i++) {
    Console.WriteLine(numbers[i]);
}
```

### Safe Removal During Iteration

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// WRONG - modifying while iterating
foreach (int num in numbers) {
    if (num > 3) numbers.Remove(num);  // Problem!
}

// RIGHT - iterate copy
foreach (int num in numbers.ToList()) {
    if (num > 3) numbers.Remove(num);
}

// OR - use RemoveAll
numbers.RemoveAll(x => x > 3);
```

## List Methods

### Sort

```csharp
List<int> numbers = new List<int> { 5, 2, 8, 1, 9 };
numbers.Sort();
// Result: [1, 2, 5, 8, 9]

// Sort descending
numbers.Sort((a, b) => b.CompareTo(a));
// Result: [9, 8, 5, 2, 1]
```

### Reverse

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
numbers.Reverse();
// Result: [5, 4, 3, 2, 1]
```

### GetRange

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
List<int> subset = numbers.GetRange(2, 3);  // 3 elements starting at index 2
// Result: [3, 4, 5]
```

### ToArray

```csharp
List<string> fruits = new List<string> { "apple", "banana", "cherry" };
string[] array = fruits.ToArray();
// Can now use as array
```

## LINQ with Lists

### Select - Transform

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
List<int> doubled = numbers.Select(x => x * 2).ToList();
// Result: [2, 4, 6, 8, 10]
```

### Where - Filter

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
List<int> evens = numbers.Where(x => x % 2 == 0).ToList();
// Result: [2, 4, 6, 8]
```

### OrderBy

```csharp
List<string> names = new List<string> { "Charlie", "Alice", "Bob" };
List<string> sorted = names.OrderBy(x => x).ToList();
// Result: ["Alice", "Bob", "Charlie"]
```

### GroupBy

```csharp
List<int> numbers = new List<int> { 1, 2, 2, 3, 3, 3 };
var grouped = numbers.GroupBy(x => x).ToList();

foreach (var group in grouped) {
    Console.WriteLine($"{group.Key}: {group.Count()} times");
}
```

## Common Patterns

### Pattern 1: Add if Not Exists

```csharp
List<int> numbers = new List<int> { 1, 2, 3 };

if (!numbers.Contains(4)) {
    numbers.Add(4);
}
```

### Pattern 2: Distinct Elements

```csharp
List<int> numbers = new List<int> { 1, 2, 2, 3, 3, 3 };
List<int> unique = numbers.Distinct().ToList();
// Result: [1, 2, 3]
```

### Pattern 3: Flatten List of Lists

```csharp
List<List<int>> matrix = new List<List<int>> {
    new List<int> { 1, 2, 3 },
    new List<int> { 4, 5, 6 },
    new List<int> { 7, 8, 9 }
};

List<int> flat = matrix.SelectMany(x => x).ToList();
// Result: [1, 2, 3, 4, 5, 6, 7, 8, 9]
```

## Performance Considerations

### Adding Elements

```csharp
// Adding is fast O(1) amortized
List<int> list = new List<int>();
list.Add(1);  // Fast

// But Resize is expensive O(n)
// List grows by factor of 2 when needed
```

### Removing Elements

```csharp
// Remove by value - O(n) (searches and shifts)
list.Remove(item);

// Remove at index - O(n) (shifts elements)
list.RemoveAt(0);  // Expensive if removing from front

// RemoveAll - O(n)
list.RemoveAll(x => x > 5);
```

### Iteration

```csharp
// Foreach - O(n)
foreach (var item in list) { }

// For loop - O(n)
for (int i = 0; i < list.Count; i++) { }

// LINQ queries - depends on operation
```

## Best Practices

✓ **Use List for dynamic collections**
```csharp
List<int> items = new List<int>();  // Grows as needed
```

✓ **Iterate safely when modifying**
```csharp
// Safe - iterate copy
foreach (var item in list.ToList()) {
    if (condition) list.Remove(item);
}
```

✓ **Use LINQ for filtering/transforming**
```csharp
var result = list
    .Where(x => x > 5)
    .Select(x => x * 2)
    .ToList();
```

## Anti-Patterns

❌ **Removing from front repeatedly**
```csharp
while (list.Count > 0) {
    process(list[0]);
    list.RemoveAt(0);  // Expensive!
}

// Better - use Queue
```

❌ **Modifying while iterating with foreach**
```csharp
foreach (var item in list) {
    if (item > 5) list.Remove(item);  // Problem!
}
```

## Summary

- List<T> is dynamic, grows as needed
- Fast random access by index
- Add/Remove have performance implications
- Use LINQ for transformations
- Prefer List for unknown size collections
- Consider Queue/Stack for specific patterns

---

## Next Steps

1. Learn Dictionary
2. Study HashSet, Queue, Stack
3. Review Collection Patterns
4. Study Best Practices
