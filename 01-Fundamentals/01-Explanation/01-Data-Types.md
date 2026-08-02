# Data Types in C#

## Overview
C# has two main categories of data types: **Value Types** and **Reference Types**.

---

## Value Types (Stack Memory)

Value types store data directly in memory. They are stored on the **stack**.

### Numeric Types

#### Integer Types
```csharp
byte     // 0 to 255 (1 byte)
short    // -32,768 to 32,767 (2 bytes)
int      // -2,147,483,648 to 2,147,483,647 (4 bytes) - default
long     // Large numbers (8 bytes)

// Usage
int age = 25;
long population = 8000000000;
byte percentage = 100;
```

#### Floating Point Types
```csharp
float    // 7 digits precision (4 bytes) - use 'f' suffix
double   // 15-16 digits precision (8 bytes) - default
decimal  // 28-29 digits precision (16 bytes) - for money, use 'm' suffix

// Usage
float price = 19.99f;
double pi = 3.14159265359;
decimal accountBalance = 1000.50m;
```

### Boolean Type
```csharp
bool isActive = true;
bool isComplete = false;

// Used in conditions
if (isActive) {
    Console.WriteLine("Active");
}
```

### Character Type
```csharp
char letter = 'A';
char digit = '5';

// Single character only
char c = 'Z';  // OK
// char x = 'AB';  // Error - too long
```

### Struct (User-Defined Value Type)
```csharp
public struct Point {
    public int X { get; set; }
    public int Y { get; set; }
}

Point p = new Point { X = 10, Y = 20 };
```

---

## Reference Types (Heap Memory)

Reference types store a **reference** to data on the **heap**. The reference itself is on the stack.

### String
```csharp
string name = "John";
string empty = "";
string multiline = @"Line 1
Line 2";

// Immutable - creates new string when modified
string text = "Hello";
text = text + " World";  // Creates new string
```

### Class (User-Defined)
```csharp
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
}

Person person = new Person { Name = "Alice", Age = 30 };
```

### Interface
```csharp
public interface IAnimal {
    void MakeSound();
}

public class Dog : IAnimal {
    public void MakeSound() {
        Console.WriteLine("Woof!");
    }
}
```

### Array
```csharp
int[] numbers = new int[5];
int[] arr = { 1, 2, 3, 4, 5 };
string[] names = new string[3];
```

### Collections
```csharp
List<int> numbers = new List<int> { 1, 2, 3 };
Dictionary<string, int> ages = new Dictionary<string, int>();
HashSet<string> uniqueNames = new HashSet<string>();
```

### Delegate
```csharp
public delegate void Notify(string message);

Notify notifier = Console.WriteLine;
notifier("Hello");
```

---

## Key Differences: Value vs Reference

| Aspect | Value Type | Reference Type |
|--------|-----------|-----------------|
| Storage | Stack | Heap |
| Copy Behavior | Copies data | Copies reference |
| Default Value | 0/false | null |
| Memory | Fixed size | Variable size |
| Garbage Collection | No | Yes |
| Speed | Faster | Slower (heap access) |
| Nullable | Requires `?` | Inherently nullable |

### Value Type Example
```csharp
int x = 5;
int y = x;  // Copies the value
y = 10;

Console.WriteLine(x);  // Output: 5 (unchanged)
Console.WriteLine(y);  // Output: 10
```

### Reference Type Example
```csharp
List<int> list1 = new List<int> { 1, 2, 3 };
List<int> list2 = list1;  // Copies the reference
list2.Add(4);

Console.WriteLine(list1.Count);  // Output: 4 (changed!)
Console.WriteLine(list2.Count);  // Output: 4
```

---

## Choosing Data Types

### Use Value Types When:
- ✓ Storing simple data (numbers, boolean)
- ✓ Need performance (stack allocation)
- ✓ Small, fixed-size data
- ✓ Performance-critical loops

### Use Reference Types When:
- ✓ Storing complex objects
- ✓ Need shared references
- ✓ Variable-sized data
- ✓ Collections of objects

---

## Best Practices

✓ Use `int` for most integers (not `long` unless necessary)
✓ Use `decimal` for money, not `float` or `double`
✓ Use `string` for text (it's optimized)
✓ Choose appropriate numeric type for range needed
✓ Understand boxing/unboxing cost
✓ Be aware of default values (0, false, null)

---

## Common Mistakes

❌ Using `float` for financial calculations
```csharp
float total = 0.1f + 0.2f;  // May not equal 0.3
```

✓ Use `decimal` instead
```csharp
decimal total = 0.1m + 0.2m;  // Exactly 0.3
```

❌ Forgetting nullable suffix for value types
```csharp
int? value = null;  // Need ? for nullable
```

✓ Use nullable types when needed
```csharp
int? age = null;  // OK
decimal? price = null;  // OK for money
```
