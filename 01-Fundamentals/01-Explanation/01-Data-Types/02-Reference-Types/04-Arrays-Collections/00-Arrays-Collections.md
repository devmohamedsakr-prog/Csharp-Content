# Arrays and Collections: Grouping Reference Types

## Overview

Arrays and collections group multiple elements together. Both are reference types that provide different trade-offs for storing and accessing data.

## Arrays

### Array Basics

```csharp
// Single-dimensional array
int[] numbers = new int[5];           // Size 5, all zeros
int[] initialized = { 1, 2, 3, 4, 5 };  // Initialize with values
string[] names = new string[3];       // Size 3, all null

// Array characteristics
// - Fixed size (cannot grow)
// - Zero-indexed (first element at index 0)
// - Type-safe (all elements same type)
// - Value types copied, reference types shared
```

### Creating Arrays

#### Empty Array
```csharp
// Explicit size
int[] arr = new int[10];  // 10 zeros
string[] strs = new string[5];  // 5 nulls

// Array initialization syntax
int[] arr2 = new int[] { 1, 2, 3 };

// Inferred size
int[] arr3 = { 1, 2, 3 };  // Size inferred as 3
```

#### Multi-Dimensional Arrays

```csharp
// 2D array (matrix)
int[,] matrix = new int[3, 3];  // 3x3 grid

// Initialize 2D array
int[,] grid = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Access element
int value = grid[1, 2];  // Row 1, Column 2 = 6

// 3D array (cube)
int[,,] cube = new int[3, 3, 3];
```

#### Jagged Arrays (Array of Arrays)

```csharp
// Array of arrays (different sizes)
int[][] jagged = new int[3][];
jagged[0] = new int[2];
jagged[1] = new int[4];
jagged[2] = new int[3];

// Initialize
int[][] jagged2 = {
    new int[] { 1, 2 },
    new int[] { 3, 4, 5 },
    new int[] { 6 }
};

// Access
int value = jagged2[1][2];  // Second array, third element = 5
```

### Array Operations

#### Indexing and Iteration

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// By index
int first = numbers[0];     // 1
int last = numbers[4];      // 5
int fromEnd = numbers[^1];  // 5 (from end)

// For loop
for (int i = 0; i < numbers.Length; i++) {
    Console.WriteLine(numbers[i]);
}

// Foreach loop
foreach (int num in numbers) {
    Console.WriteLine(num);
}
```

#### Length and Bounds

```csharp
int[] arr = { 1, 2, 3 };

int length = arr.Length;  // 3
bool isEmpty = arr.Length == 0;

// Array bounds
try {
    int value = arr[10];  // IndexOutOfRangeException
} catch (IndexOutOfRangeException) {
    Console.WriteLine("Index out of range");
}
```

#### Slicing

```csharp
int[] arr = { 1, 2, 3, 4, 5 };

// Slice (returns new array)
int[] slice1 = arr[1..4];      // { 2, 3, 4 }
int[] slice2 = arr[..3];       // { 1, 2, 3 }
int[] slice3 = arr[2..];       // { 3, 4, 5 }
int[] slice4 = arr[^3..^1];    // { 3, 4 }
```

#### Common Array Methods

```csharp
int[] arr = { 3, 1, 4, 1, 5, 9 };

// Sorting
Array.Sort(arr);  // { 1, 1, 3, 4, 5, 9 }

// Reverse
Array.Reverse(arr);  // { 9, 5, 4, 3, 1, 1 }

// Find
int found = Array.Find(arr, x => x > 5);  // 9
int notFound = Array.Find(arr, x => x > 100);  // 0 (default)

// Exists
bool hasValue = Array.Exists(arr, x => x == 5);  // true

// Copy
int[] copy = new int[arr.Length];
Array.Copy(arr, copy, arr.Length);

// Clear
Array.Clear(arr);  // All zeros

// Resize
Array.Resize(ref arr, 10);  // New size 10
```

### Array Performance

```csharp
// Stack allocation for value types
int[] numbers = new int[1000];  // Stack-like efficiency

// Efficient access
for (int i = 0; i < numbers.Length; i++) {
    numbers[i] = i;  // O(1) access
}

// LINQ is convenient but slower
var result = numbers.Where(x => x > 500).ToArray();
```

## Collections

### List<T> (Dynamic Array)

```csharp
// Create
List<int> numbers = new();
List<string> names = new() { "Alice", "Bob" };

// Add
numbers.Add(1);
numbers.AddRange(new[] { 2, 3, 4 });

// Access
int first = numbers[0];
int last = numbers[^1];

// Count
int count = numbers.Count;

// Contains
bool has3 = numbers.Contains(3);

// Find
int found = numbers.Find(x => x > 2);

// Index
int idx = numbers.IndexOf(3);

// Remove
numbers.Remove(3);
numbers.RemoveAt(0);
numbers.RemoveAll(x => x < 0);

// Clear
numbers.Clear();

// Convert to array
int[] arr = numbers.ToArray();
```

### Dictionary<TKey, TValue>

```csharp
// Create
Dictionary<string, int> ages = new();
Dictionary<int, string> names = new() {
    { 1, "Alice" },
    { 2, "Bob" }
};

// Add
ages["Alice"] = 30;
ages.Add("Bob", 25);

// Access
int aliceAge = ages["Alice"];

// Safe access
if (ages.TryGetValue("Alice", out int age)) {
    Console.WriteLine($"Alice is {age}");
}

// Contains
bool hasAlice = ages.ContainsKey("Alice");

// Keys and values
var keys = ages.Keys;
var values = ages.Values;

// Remove
ages.Remove("Alice");

// Count
int count = ages.Count;

// Iterate
foreach (var kvp in ages) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
```

### HashSet<T> (Unique Elements)

```csharp
// Create
HashSet<int> numbers = new() { 1, 2, 3, 2, 1 };  // { 1, 2, 3 }

// Add
numbers.Add(4);
numbers.Add(1);  // Duplicate, not added

// Contains
bool has2 = numbers.Contains(2);

// Count
int count = numbers.Count;  // 4

// Remove
numbers.Remove(2);

// Set operations
var set1 = new HashSet<int> { 1, 2, 3 };
var set2 = new HashSet<int> { 2, 3, 4 };

set1.UnionWith(set2);       // { 1, 2, 3, 4 }
set1.IntersectWith(set2);   // { 2, 3 }
set1.ExceptWith(set2);      // { 1 }
```

### Other Collections

#### Queue<T> (FIFO)
```csharp
Queue<int> queue = new();

queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);

int first = queue.Dequeue();  // 1
int peek = queue.Peek();      // 2
```

#### Stack<T> (LIFO)
```csharp
Stack<int> stack = new();

stack.Push(1);
stack.Push(2);
stack.Push(3);

int top = stack.Pop();        // 3
int peek = stack.Peek();      // 2
```

#### LinkedList<T> (Doubly Linked)
```csharp
LinkedList<int> list = new();

var node1 = list.AddLast(1);
var node2 = list.AddLast(2);
var node3 = list.AddLast(3);

// Insert before/after node
list.AddBefore(node2, 1.5);  // Between 1 and 2

// Remove
list.Remove(node2);

// Navigate
var first = list.First;
var last = list.Last;
```

#### SortedList<TKey, TValue>
```csharp
var sorted = new SortedList<string, int> {
    { "Charlie", 30 },
    { "Alice", 25 },
    { "Bob", 35 }
};

// Always sorted by key
foreach (var kvp in sorted) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    // Output: Alice: 25, Bob: 35, Charlie: 30
}
```

## Collection Comparison

| Type | Use Case | Access | Add/Remove | Order |
|------|----------|--------|-----------|-------|
| Array | Fixed size, performance | O(1) | N/A | Maintains |
| List<T> | Dynamic array | O(1) | O(n) | Maintains |
| Dictionary<K,V> | Key-value lookup | O(1) | O(1) | Unordered |
| HashSet<T> | Unique items, fast lookup | O(1) | O(1) | Unordered |
| Queue<T> | FIFO processing | O(1) | O(1) | FIFO |
| Stack<T> | LIFO processing | O(1) | O(1) | LIFO |
| LinkedList<T> | Insert/remove middle | O(n) | O(1) | Maintains |
| SortedList | Sorted key-value | O(1) | O(n) | Sorted |

## LINQ Operations

```csharp
List<int> numbers = new() { 1, 2, 3, 4, 5 };

// Filtering
var evens = numbers.Where(x => x % 2 == 0);  // { 2, 4 }

// Projection
var doubled = numbers.Select(x => x * 2);    // { 2, 4, 6, 8, 10 }

// Aggregation
int sum = numbers.Sum();                      // 15
int max = numbers.Max();                      // 5
double avg = numbers.Average();               // 3.0

// Ordering
var sorted = numbers.OrderBy(x => x);        // 1, 2, 3, 4, 5
var descending = numbers.OrderByDescending(x => x);  // 5, 4, 3, 2, 1

// Grouping
var grouped = numbers.GroupBy(x => x % 2);
foreach (var group in grouped) {
    Console.WriteLine($"Key {group.Key}: {string.Join(", ", group)}");
}

// Chaining
var result = numbers
    .Where(x => x > 2)
    .Select(x => x * 2)
    .OrderByDescending(x => x)
    .ToList();
```

## Performance Considerations

```csharp
// Array - fastest for indexed access
int[] arr = new int[1000000];
for (int i = 0; i < arr.Length; i++) {
    arr[i] = i;  // O(1) access
}

// List<T> - nearly as fast as array
List<int> list = new(arr);
for (int i = 0; i < list.Count; i++) {
    list[i] = i;  // O(1) access
}

// Dictionary - O(1) lookup by key
var dict = new Dictionary<int, int>();
for (int i = 0; i < 1000; i++) {
    dict[i] = i;  // O(1) insert/lookup
}

// HashSet - O(1) membership test
var set = new HashSet<int>(arr);
bool contains = set.Contains(500);  // Fast

// LINQ - convenient but slower
var filtered = arr.Where(x => x % 2 == 0).ToArray();
```

## Common Collection Mistakes

❌ **Using Count in LINQ chain**
```csharp
// Inefficient - evaluates LINQ again
if (collection.Where(x => x > 0).Count() > 5) {
    foreach (var item in collection.Where(x => x > 0)) { }
}
```

✓ **Materialize before using multiple times**
```csharp
var filtered = collection.Where(x => x > 0).ToList();
if (filtered.Count > 5) {
    foreach (var item in filtered) { }
}
```

❌ **Dictionary with null key**
```csharp
var dict = new Dictionary<string, int>();
dict[null] = 5;  // Compiles but is dangerous
```

✓ **Always check for null**
```csharp
if (key != null) {
    dict[key] = value;
}
```

❌ **Modifying collection during iteration**
```csharp
foreach (int item in list) {
    if (item > 5) {
        list.Remove(item);  // InvalidOperationException!
    }
}
```

✓ **Create new collection or use iterator version**
```csharp
var toRemove = list.Where(x => x > 5).ToList();
foreach (var item in toRemove) {
    list.Remove(item);
}
```

## Summary

**Arrays**:
- Fixed size, fast indexed access
- Zero-indexed
- Type-safe
- Best for performance-critical code

**Collections**:
- Dynamic sizing (List<T>)
- Specialized purposes (Queue, Stack, HashSet)
- Flexible APIs
- Best for most scenarios

**Key Takeaway**: Use arrays for performance-critical, fixed-size data. Use List<T> for dynamic data. Choose specialized collections (Dictionary, HashSet, Queue) based on access patterns.
