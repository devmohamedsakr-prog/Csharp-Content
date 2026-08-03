# Boxing Basics

## Overview

Boxing is the process of converting a value type (int, double, struct, etc.) to a reference type (object). Understanding boxing is crucial for performance and preventing subtle bugs.

## What is Boxing?

Boxing converts a value type into an object reference. The value is wrapped in a new object allocated on the heap.

### Simple Boxing Example

```csharp
// Boxing: value type → reference type
int number = 42;              // Value type on stack
object boxed = number;        // Boxes to heap

// What happens internally:
// 1. Create new object on heap
// 2. Copy value into object
// 3. Return reference to object
// Stack now contains reference to heap object
```

### Memory Layout

```
Without Boxing:
┌─────────────────────┐
│ Stack               │
│ number = 42         │ (direct value)
└─────────────────────┘

With Boxing:
┌─────────────────────┐
│ Stack               │
│ boxed = [ref]───────┼──┐
└─────────────────────┘  │
                         │
┌─────────────────────┐  │
│ Heap                │  │
│ [Object wrapper]    │←─┘
│ value = 42          │
│ type info           │
└─────────────────────┘
```

## Implicit Boxing

Boxing can happen implicitly (automatically) when you assign a value type to an object reference:

```csharp
// Implicit boxing - automatic
int x = 10;
object obj = x;  // Boxes automatically

// Implicit boxing with interface
int value = 100;
IComparable comparable = value;  // Boxes automatically

// Implicit boxing in collections
var list = new ArrayList();
list.Add(5);      // int boxed automatically
list.Add(3.14);   // double boxed automatically
```

## Explicit Boxing

Boxing can also be explicit with a cast:

```csharp
int number = 42;
object boxed = (object)number;  // Explicit cast (same result)

// Usually implicit is clearer
object implicit = number;  // Preferred
```

## Boxing Different Value Types

### Primitives

```csharp
// All primitive value types can be boxed
int intVal = 42;
object boxedInt = intVal;

double doubleVal = 3.14;
object boxedDouble = doubleVal;

bool boolVal = true;
object boxedBool = boolVal;

char charVal = 'A';
object boxedChar = charVal;

byte byteVal = 255;
object boxedByte = byteVal;
```

### Structs

```csharp
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

// Structs can be boxed
Point p = new Point { X = 10, Y = 20 };
object boxedPoint = p;  // Struct boxed to object
```

### Nullable Types

```csharp
// Nullable boxing
int? nullable = 42;
object boxed = nullable;  // Boxes as int, not Nullable<int>

// Nullable with null
int? nullableNull = null;
object boxedNull = nullableNull;  // Boxes as null (special case)
```

## Boxing in Collections

### Non-Generic Collections (Old Style)

```csharp
// ArrayList boxes every value type
ArrayList list = new ArrayList();
list.Add(1);      // int boxed
list.Add(2.5);    // double boxed
list.Add("text"); // string already reference type
list.Add(true);   // bool boxed

// Retrieving requires unboxing
foreach (object item in list)
{
    if (item is int intVal)
        Console.WriteLine($"Int: {intVal}");
    else if (item is double doubleVal)
        Console.WriteLine($"Double: {doubleVal}");
}
```

### Hashtable Example

```csharp
// Hashtable boxes keys and values
Hashtable hash = new Hashtable();
hash[1] = "One";           // int key boxed
hash[2] = 42;              // int value boxed
hash["key"] = "value";     // strings already reference

foreach (DictionaryEntry entry in hash)
{
    // Keys and values are objects
    object key = entry.Key;       // May be boxed int
    object value = entry.Value;   // May be boxed int
}
```

## Boxing with Interfaces

### IComparable Example

```csharp
// When value type implements interface
int value = 42;
IComparable comparable = value;  // Boxing occurs

// The value type is boxed to satisfy the interface reference
// Why? IComparable parameter expects object reference
int result = comparable.CompareTo(100);
```

### Generic vs Non-Generic

```csharp
// Non-generic version boxes
IComparable nonGeneric = 42;  // Boxing!

// Generic version avoids boxing
IComparable<int> generic = 42;  // No boxing!
```

## When Boxing Happens

Boxing occurs automatically when:

1. **Assigning to object reference**
```csharp
object obj = 42;  // Boxing
```

2. **Passing to object parameter**
```csharp
void PrintObject(object obj) { }
PrintObject(42);  // Boxing
```

3. **Adding to non-generic collection**
```csharp
ArrayList list = new ArrayList();
list.Add(42);  // Boxing
```

4. **Using with interface (non-generic)**
```csharp
IComparable comp = 42;  // Boxing
```

5. **String concatenation**
```csharp
string s = "Value: " + 42;  // Boxing for conversion
```

## Boxing Performance

Boxing has performance cost due to:

1. **Memory allocation** - Object created on heap
2. **GC pressure** - Garbage collector must clean up
3. **CPU cycles** - Copy value to heap
4. **Reference indirection** - Need to dereference

### Simple Benchmark

```csharp
using System.Diagnostics;

int iterations = 1_000_000;

// Without boxing
var sw = Stopwatch.StartNew();
long sum = 0;
for (int i = 0; i < iterations; i++)
{
    sum += i;  // No boxing
}
sw.Stop();
Console.WriteLine($"No boxing: {sw.ElapsedMilliseconds}ms");

// With boxing
sw.Restart();
object objSum = 0;
for (int i = 0; i < iterations; i++)
{
    objSum = (int)objSum + i;  // Box, unbox, box
}
sw.Stop();
Console.WriteLine($"With boxing: {sw.ElapsedMilliseconds}ms");
// With boxing is typically 10-100x slower!
```

## Key Points

- **Boxing** = value type → object reference
- **Occurs implicitly** when assigning to object
- **Allocates memory** on heap
- **Performance cost** in loops and collections
- **Can cause issues** with non-generic collections
- **Generics avoid boxing** entirely

## Boxing vs Assignment

### Value Type Assignment (No Boxing)

```csharp
int x = 42;
int y = x;  // Copy value, no boxing
```

### Boxing (Reference Type Assignment)

```csharp
int x = 42;
object obj = x;  // Box value to object reference
```

## Practical Examples

### Example 1: Collection Scenario

```csharp
// Scenario: Storing mixed types
var list = new ArrayList();
list.Add(42);        // Boxing
list.Add(3.14);      // Boxing
list.Add("text");    // No boxing
list.Add(new object()); // No boxing

// Each access requires type checking and potential unboxing
```

### Example 2: Method Parameter

```csharp
public void ProcessValue(object value)
{
    // value might be boxed
    Console.WriteLine(value);
}

ProcessValue(42);     // Boxes int
ProcessValue("text"); // No boxing
ProcessValue(3.14);   // Boxes double
```

### Example 3: Generic vs Non-Generic

```csharp
// Non-generic (boxing)
ArrayList arrayList = new ArrayList();
arrayList.Add(42);  // Boxing

// Generic (no boxing)
List<int> list = new List<int>();
list.Add(42);  // No boxing
```

## Summary

- Boxing converts value types to objects
- Happens automatically and implicitly
- Has performance costs in loops
- Avoided by using generics
- Important for understanding old .NET code
- Modern .NET strongly prefers generics

## Next Steps

- Study [Value-Reference-Types](../02-Value-Reference-Types/00-Value-Reference-Types.md) for type system
- Learn [Boxing-Conversions](../03-Boxing-Conversions/00-Boxing-Conversions.md) for practical patterns
- Explore [Boxing-Collections](../04-Boxing-Collections/00-Boxing-Collections.md) for collection patterns
