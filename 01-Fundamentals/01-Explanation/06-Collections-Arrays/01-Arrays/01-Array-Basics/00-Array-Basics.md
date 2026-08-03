# Array Basics

## Overview
Arrays are fixed-size collections that store multiple elements of the same type. They provide fast access by index but cannot grow or shrink dynamically.

## What is an Array?

An array is a collection of elements stored in contiguous memory locations, each accessed by an index starting at 0.

```csharp
// Visual representation:
// Index:  0    1    2    3    4
// Value: [10] [20] [30] [40] [50]

int[] numbers = { 10, 20, 30, 40, 50 };
```

## Declaring Arrays

### Basic Declaration

```csharp
// Declare and initialize with default values
int[] numbers = new int[5];  // All 0
string[] names = new string[3];  // All null
bool[] flags = new bool[4];  // All false

// Declare and initialize with values
int[] values = { 1, 2, 3, 4, 5 };

// Using new with initialization
int[] scores = new int[] { 95, 87, 92, 88 };

// Using var with initialization
var items = new int[] { 10, 20, 30 };
```

### Type-Specific Arrays

```csharp
// Integer array
int[] integers = new int[10];

// String array
string[] strings = new string[5];

// Boolean array
bool[] booleans = new bool[3];

// Double array
double[] decimals = new double[8];

// Custom type array
Person[] people = new Person[20];
```

### Implicit Typing

```csharp
// Compiler infers type from initialization
var numbers = new[] { 1, 2, 3, 4, 5 };  // int[]

var names = new[] { "Alice", "Bob", "Charlie" };  // string[]

var mixed = new[] { 1.5, 2.5, 3.5 };  // double[]
```

## Array Length and Capacity

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// Get length
int length = numbers.Length;  // 5

// Arrays have fixed length
// Cannot add or remove elements
// Cannot change size after creation
```

**Key Difference from Collections:**
```csharp
// Array - fixed size
int[] arr = new int[5];  // Always size 5
arr[10] = 99;  // IndexOutOfRangeException!

// List - dynamic size
List<int> list = new List<int>();  // Starts empty
list.Add(99);  // Grows automatically
```

## Accessing Array Elements

### Getting Elements

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

// Access by index (0-based)
int first = numbers[0];      // 10
int second = numbers[1];     // 20
int last = numbers[4];       // 50

// Last element
int lastElement = numbers[numbers.Length - 1];  // 50
```

### Setting Elements

```csharp
int[] scores = new int[5];

// Assign values
scores[0] = 95;
scores[1] = 87;
scores[2] = 92;

// Modify existing value
scores[0] = 100;  // Change first element

// All elements
int[] values = new int[3];
values[0] = 1;
values[1] = 2;
values[2] = 3;
```

### Index Out of Range

```csharp
int[] arr = new int[5];  // Indices 0-4 valid

int value = arr[5];  // IndexOutOfRangeException
arr[-1] = 10;        // IndexOutOfRangeException

// Safe access pattern
if (index >= 0 && index < arr.Length) {
    int value = arr[index];
}
```

## Array Initialization Patterns

### Pattern 1: Empty Array

```csharp
// Create empty array, fill later
int[] numbers = new int[5];  // All default (0)

for (int i = 0; i < numbers.Length; i++) {
    numbers[i] = i * 10;
}
// Result: [0, 10, 20, 30, 40]
```

### Pattern 2: Literal Array

```csharp
// Create with values
int[] scores = { 95, 87, 92, 88, 91 };

string[] names = { "Alice", "Bob", "Charlie" };

double[] prices = { 9.99, 19.99, 29.99 };
```

### Pattern 3: Default Values

```csharp
// Explicitly use new keyword
int[] zeros = new int[5];  // [0, 0, 0, 0, 0]

string[] nulls = new string[3];  // [null, null, null]

bool[] falses = new bool[4];  // [false, false, false, false]
```

## Array Iteration

### For Loop

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

for (int i = 0; i < numbers.Length; i++) {
    Console.WriteLine($"numbers[{i}] = {numbers[i]}");
}

// Output:
// numbers[0] = 1
// numbers[1] = 2
// ... etc
```

### Foreach Loop

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

foreach (int num in numbers) {
    Console.WriteLine(num);
}

// Output:
// 1
// 2
// 3
// 4
// 5
```

### While Loop

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
int i = 0;

while (i < numbers.Length) {
    Console.WriteLine(numbers[i]);
    i++;
}
```

## Array Methods and Properties

### Length Property

```csharp
int[] arr = { 10, 20, 30, 40, 50 };

int length = arr.Length;  // 5

// Common pattern: validate before access
if (arr.Length > 0) {
    int first = arr[0];
}
```

### Array.Reverse()

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

Array.Reverse(numbers);
// Result: [5, 4, 3, 2, 1]
```

### Array.Sort()

```csharp
int[] numbers = { 5, 2, 8, 1, 9 };

Array.Sort(numbers);
// Result: [1, 2, 5, 8, 9]

// With descending order
Array.Sort(numbers, new ReverseComparer());
```

### Array.Copy()

```csharp
int[] source = { 1, 2, 3, 4, 5 };
int[] destination = new int[5];

Array.Copy(source, destination, 5);
// destination: [1, 2, 3, 4, 5]
```

### Array.Find()

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// Find first matching
int found = Array.Find(numbers, x => x > 3);  // 4

// Find all matching
int[] matches = Array.FindAll(numbers, x => x > 2);
// Result: [3, 4, 5]
```

### Array.IndexOf()

```csharp
int[] numbers = { 10, 20, 30, 20, 40 };

int index = Array.IndexOf(numbers, 20);  // 1 (first occurrence)

// Find all indices
var indices = numbers
    .Select((value, index) => (value, index))
    .Where(x => x.value == 20)
    .Select(x => x.index)
    .ToArray();
// Result: [1, 3]
```

## Common Array Patterns

### Pattern 1: Sum All Elements

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
int sum = 0;

foreach (int num in numbers) {
    sum += num;
}
// sum = 15
```

### Pattern 2: Find Maximum

```csharp
int[] scores = { 85, 92, 78, 95, 88 };
int max = scores[0];

for (int i = 1; i < scores.Length; i++) {
    if (scores[i] > max) {
        max = scores[i];
    }
}
// max = 95
```

### Pattern 3: Count Matching Elements

```csharp
int[] numbers = { 1, 5, 2, 7, 3, 9, 4 };
int count = 0;

foreach (int num in numbers) {
    if (num > 5) {
        count++;
    }
}
// count = 2 (7 and 9)
```

### Pattern 4: Filter to New Array

```csharp
int[] original = { 1, 2, 3, 4, 5, 6 };

// Get even numbers
var evens = original.Where(x => x % 2 == 0).ToArray();
// Result: [2, 4, 6]
```

## Array vs List Comparison

```csharp
// Array - fixed size
int[] arr = new int[5];  // Cannot grow
arr[10] = 99;  // Error!

// List - dynamic size
List<int> list = new List<int>();  // Can grow
list.Add(99);  // OK, grows automatically
```

## Default Values by Type

```csharp
// When creating array, elements have default values:

int[] ints = new int[5];           // [0, 0, 0, 0, 0]
double[] doubles = new double[3];  // [0.0, 0.0, 0.0]
bool[] bools = new bool[4];        // [false, false, false, false]
string[] strings = new string[2];  // [null, null]
object[] objects = new object[3];  // [null, null, null]
```

## Best Practices

✓ **Check bounds before access**
```csharp
if (index >= 0 && index < array.Length) {
    value = array[index];
}
```

✓ **Use foreach for simple iteration**
```csharp
foreach (int num in numbers) {
    Console.WriteLine(num);
}
```

✓ **Use appropriate loop type**
```csharp
// Need index? Use for loop
for (int i = 0; i < arr.Length; i++) { }

// Don't need index? Use foreach
foreach (var item in arr) { }
```

✓ **Prefer List if size changes**
```csharp
// Wrong - array size is fixed
int[] arr = new int[100];  // Wasteful if small

// Right - list grows as needed
List<int> list = new List<int>();  // Efficient
```

## Anti-Patterns

❌ **Accessing without bounds check**
```csharp
int value = array[unknownIndex];  // May crash!
```

❌ **Assuming array is not empty**
```csharp
int first = array[0];  // What if empty?
```

❌ **Modifying array while iterating (with for loop)**
```csharp
for (int i = 0; i < array.Length; i++) {
    if (array[i] > 5) {
        // If you modify array here, things break
    }
}
```

## Summary

- Arrays are fixed-size collections
- 0-indexed access by position
- Fast element access
- Cannot grow or shrink
- Default values depend on type
- Use when size is known and fixed
- Use List if size changes

---

## Next Steps

1. Learn Multi-Dimensional Arrays
2. Study Array Operations
3. Master Generic Collections
4. Learn Iteration Patterns
5. Review Best Practices
