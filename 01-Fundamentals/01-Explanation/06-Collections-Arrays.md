# Collections and Arrays

## Overview
Arrays and collections store multiple values. Arrays have fixed size; collections are dynamic.

---

## Arrays

Fixed-size collection of elements of the same type.

### Declaring Arrays

```csharp
// Array of integers
int[] numbers = new int[5];

// Array with initialization
int[] arr = { 1, 2, 3, 4, 5 };

// String array
string[] names = new string[3];
names[0] = "Alice";
names[1] = "Bob";

// Array with new and initialization
int[] values = new int[] { 10, 20, 30 };

// Multi-dimensional array
int[,] matrix = new int[3, 3];

// Jagged array (array of arrays)
int[][] jagged = new int[3][];
jagged[0] = new int[5];
jagged[1] = new int[3];
```

### Array Access

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

// Access element
int first = numbers[0];  // 10
int second = numbers[1];  // 20

// Modify element
numbers[2] = 99;  // Changes 30 to 99

// Array length
int length = numbers.Length;  // 5

// Last element
int last = numbers[numbers.Length - 1];  // 50
```

### Array Iteration

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// For loop
for (int i = 0; i < numbers.Length; i++) {
    Console.WriteLine(numbers[i]);
}

// Foreach loop
foreach (int num in numbers) {
    Console.WriteLine(num);
}
```

### Multi-Dimensional Arrays

```csharp
// 2D array (matrix)
int[,] matrix = new int[3, 3] {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Access element
int value = matrix[0, 0];  // 1
int value2 = matrix[2, 2];  // 9

// Iterate 2D array
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 3; j++) {
        Console.WriteLine(matrix[i, j]);
    }
}
```

---

## List<T>

Dynamic collection that grows as needed.

### Creating Lists

```csharp
// Empty list
List<int> numbers = new List<int>();

// List with capacity
List<string> names = new List<string>(10);

// List with initialization
List<int> values = new List<int> { 1, 2, 3, 4, 5 };

// List of objects
List<Person> people = new List<Person> {
    new Person { Name = "Alice" },
    new Person { Name = "Bob" }
};
```

### Adding Elements

```csharp
List<string> items = new List<string>();

// Add single item
items.Add("apple");
items.Add("banana");

// Add range
items.AddRange(new[] { "orange", "grape" });

// Insert at index
items.Insert(1, "mango");  // Insert at position 1
```

### Removing Elements

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Remove specific value
numbers.Remove(3);  // Removes first occurrence of 3

// Remove at index
numbers.RemoveAt(0);  // Removes element at index 0

// Remove all matching
numbers.RemoveAll(n => n > 3);  // Removes 4 and 5

// Clear list
numbers.Clear();  // Removes all
```

### Accessing List Elements

```csharp
List<string> names = new List<string> { "Alice", "Bob", "Charlie" };

// Index access
string first = names[0];  // "Alice"
string last = names[names.Count - 1];  // "Charlie"

// Count
int count = names.Count;  // 3

// Check if contains
bool hasAlice = names.Contains("Alice");  // true

// Find index
int index = names.IndexOf("Bob");  // 1

// Find first matching
Person found = people.FirstOrDefault(p => p.Age > 30);
```

### Iterating Lists

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Foreach
foreach (int num in numbers) {
    Console.WriteLine(num);
}

// For loop
for (int i = 0; i < numbers.Count; i++) {
    Console.WriteLine(numbers[i]);
}

// Foreach with index (LINQ)
foreach (var item in numbers.Select((value, index) => new { value, index })) {
    Console.WriteLine($"{item.index}: {item.value}");
}
```

---

## Dictionary<K, V>

Key-value pairs collection.

### Creating Dictionaries

```csharp
// Empty dictionary
Dictionary<string, int> ages = new Dictionary<string, int>();

// Dictionary with initialization
Dictionary<string, int> scores = new Dictionary<string, int> {
    { "Alice", 95 },
    { "Bob", 87 },
    { "Charlie", 92 }
};

// Modern syntax
var scores2 = new Dictionary<string, int> {
    ["Alice"] = 95,
    ["Bob"] = 87
};
```

### Adding and Removing

```csharp
Dictionary<string, int> ages = new Dictionary<string, int>();

// Add
ages["Alice"] = 30;
ages.Add("Bob", 25);  // Throws if key exists

// Update
ages["Alice"] = 31;  // Changes value

// Remove
ages.Remove("Bob");  // Returns true if found

// Clear
ages.Clear();
```

### Accessing Dictionary

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

// Access by key
int aliceAge = ages["Alice"];  // 30

// Safe access with TryGetValue
if (ages.TryGetValue("Charlie", out int charlieAge)) {
    Console.WriteLine($"Charlie's age: {charlieAge}");
} else {
    Console.WriteLine("Charlie not found");
}

// Keys and Values
var keys = ages.Keys;  // ["Alice", "Bob"]
var values = ages.Values;  // [30, 25]

// Check if key exists
bool hasAlice = ages.ContainsKey("Alice");  // true
bool hasValue30 = ages.ContainsValue(30);  // true
```

### Iterating Dictionary

```csharp
Dictionary<string, int> scores = new Dictionary<string, int> {
    { "Alice", 95 },
    { "Bob", 87 }
};

// Foreach KeyValuePair
foreach (var kvp in scores) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

// Just keys
foreach (string name in scores.Keys) {
    Console.WriteLine(name);
}

// Just values
foreach (int score in scores.Values) {
    Console.WriteLine(score);
}
```

---

## Other Collections

### HashSet<T>
Unique values only, no duplicates.

```csharp
HashSet<int> numbers = new HashSet<int> { 1, 2, 3 };

numbers.Add(2);  // Not added (duplicate)
numbers.Add(4);  // Added

// Set operations
HashSet<int> set1 = new HashSet<int> { 1, 2, 3 };
HashSet<int> set2 = new HashSet<int> { 2, 3, 4 };

set1.UnionWith(set2);  // {1, 2, 3, 4}
set1.IntersectWith(set2);  // {2, 3}
set1.ExceptWith(set2);  // {1}
```

### Queue<T>
First-in, first-out (FIFO).

```csharp
Queue<string> queue = new Queue<string>();

queue.Enqueue("first");
queue.Enqueue("second");
queue.Enqueue("third");

string front = queue.Dequeue();  // "first"

string peek = queue.Peek();  // "second" (without removing)
```

### Stack<T>
Last-in, first-out (LIFO).

```csharp
Stack<int> stack = new Stack<int>();

stack.Push(1);
stack.Push(2);
stack.Push(3);

int top = stack.Pop();  // 3

int peek = stack.Peek();  // 2 (without removing)
```

---

## Choosing the Right Collection

| Collection | Use Case | Access |
|-----------|----------|--------|
| Array | Fixed size, known length | Fast by index |
| List | Dynamic, frequently added/removed | By index |
| Dictionary | Key-value pairs | By key |
| HashSet | Unique values only | By value |
| Queue | FIFO operations | From front |
| Stack | LIFO operations | From top |

---

## Best Practices

✓ **Use appropriate collection type**
```csharp
// Good - List for dynamic
List<string> items = new List<string>();

// Good - Dictionary for key-value
Dictionary<string, int> ages = new Dictionary<string, int>();

// Avoid - Array when size changes frequently
int[] arr = new int[100];  // Resizing is expensive
```

✓ **Iterate safely when modifying**
```csharp
// Bad - modifying while iterating
foreach (int item in list) {
    if (item > 5) list.Remove(item);  // Causes issues
}

// Good - iterate copy or use RemoveAll
foreach (int item in list.ToList()) {
    if (item > 5) list.Remove(item);
}
```

---

## Common Mistakes

❌ **Array index out of range**
```csharp
int[] arr = new int[5];
int value = arr[10];  // IndexOutOfRangeException
```

✓ **Check bounds**
```csharp
if (index >= 0 && index < arr.Length) {
    int value = arr[index];
}
```

❌ **Dictionary key null exception**
```csharp
Dictionary<string, int> dict = new Dictionary<string, int>();
int value = dict["missing"];  // KeyNotFoundException
```

✓ **Use TryGetValue**
```csharp
if (dict.TryGetValue("key", out int value)) {
    // Use value
}
```
