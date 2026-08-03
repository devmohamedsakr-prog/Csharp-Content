# Boxing Conversions

## Overview

This section covers how to box values, when boxing happens, and practical conversion patterns.

## Implicit Boxing

Most boxing is implicit - happens automatically without explicit cast:

### Simple Implicit Boxing

```csharp
// Implicit boxing to object
int number = 42;
object boxed = number;  // Automatically boxed

// Implicit boxing with interface
int value = 100;
IComparable comparable = value;  // Automatically boxed

// Implicit boxing in method call
void Display(object obj)
{
    Console.WriteLine(obj);
}

Display(42);  // Automatically boxed
```

### Boxing Different Types

```csharp
// Primitives
object boxedInt = 42;           // int boxed
object boxedDouble = 3.14;      // double boxed
object boxedBool = true;        // bool boxed
object boxedChar = 'A';         // char boxed
object boxedByte = 255;         // byte boxed
object boxedLong = 1000L;       // long boxed

// All work the same way internally
```

## Explicit Boxing

You can also use explicit cast (though implicit is more common):

```csharp
int x = 10;
object explicit1 = (object)x;  // Explicit cast
object implicit = x;            // Implicit (clearer)

// Both produce identical result
```

## Boxing with Type Inference

Modern C# can infer when boxing is needed:

```csharp
// Compiler infers boxing needed
var boxed = (object)42;  // Explicit boxing
var items = new object[] { 1, 2, 3 };  // Elements boxed
var mixed = new object[] { 1, "text", 3.14 };  // Mixed boxing
```

## Boxing Expressions

### Boxing Arithmetic Results

```csharp
// Boxing expression result
int a = 10;
int b = 20;
object sum = a + b;  // Result (30) boxed

// More complex
object result = (10 + 20) * 2;  // Result (60) boxed

// Boxing method call result
int GetValue() => 42;
object boxed = GetValue();  // Return value boxed
```

## Boxing Conversions in Collections

### Non-Generic Collection Boxing

```csharp
// ArrayList automatically boxes
ArrayList list = new ArrayList();
list.Add(1);      // int boxed
list.Add(2.5);    // double boxed
list.Add(true);   // bool boxed
list.Add('A');    // char boxed

// ArrayList contains object references
// [ ref to boxed 1, ref to boxed 2.5, ref to boxed true, ... ]
```

### Hashtable Conversion

```csharp
// Both keys and values boxed
Hashtable hash = new Hashtable();
hash[1] = 100;        // key 1 boxed, value 100 boxed
hash[2] = "two";      // key 2 boxed, value not boxed (already reference)
hash["three"] = 300;  // key not boxed, value 300 boxed

// Type: Hashtable
// Contents: object references
```

### Stack and Queue

```csharp
// Stack boxes value types
Stack stack = new Stack();
stack.Push(42);      // int boxed
stack.Push(3.14);    // double boxed

// Queue boxes value types
Queue queue = new Queue();
queue.Enqueue(10);   // int boxed
queue.Enqueue(20);   // int boxed
```

## Boxing with Interfaces

### Non-Generic Interface

```csharp
// IComparable causes boxing
int x = 42;
IComparable comparable = x;  // Boxing (interface reference)

// Method call on boxed value
int result = comparable.CompareTo(50);  // Already boxed

// Why? IComparable.CompareTo(object other)
// Method expects object parameter
```

### IEnumerable Example

```csharp
// IEnumerable causes boxing in loops
IEnumerable items = new ArrayList { 1, 2, 3 };
foreach (object item in items)
{
    int value = (int)item;  // Unboxing on each iteration
}

// Better: Use generic
IEnumerable<int> genericItems = new List<int> { 1, 2, 3 };
foreach (int item in genericItems)
{
    // No boxing/unboxing
}
```

## Boxing Nullable Types

### Nullable Boxing

```csharp
// Boxing nullable with value
int? nullable = 42;
object boxed = nullable;  // Boxes as int (not Nullable<int>)

// Can unbox back to nullable
int? restored = (int?)boxed;  // OK
int? restored2 = (int?)boxed;  // Also OK

// Or unbox to value
int value = (int)boxed;  // OK (value was 42)
```

### Nullable with Null

```csharp
// Boxing nullable null
int? nullableNull = null;
object boxedNull = nullableNull;  // Boxes as null

// Unbox null value
int? restoredNull = (int?)boxedNull;  // null (OK)
object retrieved = (int?)boxedNull;   // null (OK)

// Cannot unbox null to non-nullable
// int value = (int)boxedNull;  // NullReferenceException!
```

## Pattern: Convert and Box

### Boxing with Cast

```csharp
// Convert then box
byte b = 42;
object boxedByte = b;  // Boxes as byte

short s = 1000;
object boxedShort = s;  // Boxes as short

// Each maintains original type
```

### Boxing Enum

```csharp
enum Status { Active, Inactive }

Status status = Status.Active;
object boxed = status;  // Boxes enum value

// Unbox back to enum
Status restored = (Status)boxed;  // Must specify type
```

## Boxing in LINQ

### LINQ and Boxing

```csharp
// Non-generic source causes boxing
ArrayList source = new ArrayList { 1, 2, 3 };
var query = source.Cast<int>();  // Unboxes during iteration
var result = query.ToList();

// Better: Generic source, no boxing
List<int> genericSource = new List<int> { 1, 2, 3 };
var genericQuery = genericSource.Where(x => x > 1);  // No boxing
var genericResult = genericQuery.ToList();
```

### Filtering with Boxing

```csharp
// Boxing in filter
ArrayList list = new ArrayList { 1, 2, 3, 4, 5 };
var evenNumbers = list.OfType<int>()  // Unboxes on iteration
    .Where(x => x % 2 == 0);

// Generic equivalent (no boxing)
List<int> genericList = new List<int> { 1, 2, 3, 4, 5 };
var genericEven = genericList.Where(x => x % 2 == 0);
```

## String Concatenation and Boxing

### String Conversion

```csharp
// Boxing occurs during string conversion
int number = 42;
string result = "Value: " + number;  // Boxes int for string conversion

// Better: Use string interpolation
string better = $"Value: {number}";  // More efficient

// Or ToString (no boxing)
string direct = "Value: " + number.ToString();
```

### StringBuilder

```csharp
var sb = new StringBuilder();
for (int i = 0; i < 100; i++)
{
    sb.Append("Value: ");
    sb.Append(i);  // Append directly, no boxing
}
string result = sb.ToString();
```

## Boxing Performance Impact

### Benchmark Example

```csharp
using System.Diagnostics;

// Scenario 1: No boxing (direct operations)
var sw = Stopwatch.StartNew();
long sum = 0;
for (int i = 0; i < 1_000_000; i++)
{
    sum += i;
}
sw.Stop();
Console.WriteLine($"Direct: {sw.ElapsedMilliseconds}ms");

// Scenario 2: Boxing in loop
sw.Restart();
object objSum = 0;
for (int i = 0; i < 1_000_000; i++)
{
    objSum = (int)objSum + i;  // Box, unbox, box each iteration
}
sw.Stop();
Console.WriteLine($"Boxed: {sw.ElapsedMilliseconds}ms");
// Boxed is typically 10-50x slower!
```

## Practical Patterns

### Pattern 1: Generic Alternative

```csharp
// BAD: Boxing with ArrayList
ArrayList nonGeneric = new ArrayList();
for (int i = 0; i < 100; i++)
    nonGeneric.Add(i);  // Boxing each iteration

// GOOD: Generic List, no boxing
List<int> generic = new List<int>();
for (int i = 0; i < 100; i++)
    generic.Add(i);  // No boxing
```

### Pattern 2: Type-Safe Conversion

```csharp
// Boxing with type safety
public object ConvertToObject<T>(T value) where T : struct
{
    return (object)value;  // Explicit boxing
}

object boxedInt = ConvertToObject(42);
object boxedDouble = ConvertToObject(3.14);
```

### Pattern 3: Conditional Boxing

```csharp
// Box only when needed
int value = 42;
object boxed = value switch
{
    > 100 => value,      // Box if > 100
    _ => value.ToString() // Otherwise convert to string
};
```

## Key Points

- **Implicit boxing** is most common
- **Happens automatically** when assigning to object
- **Different types** box to their respective object wrappers
- **Nullable boxing** boxes value, not Nullable<T>
- **Null boxing** results in null object reference
- **Performance cost** in loops and collections
- **Generics eliminate** boxing in modern code

## Summary

| Conversion Type | Result | Example |
|-----------------|--------|---------|
| int → object | Boxed int | `object o = 42` |
| double → object | Boxed double | `object o = 3.14` |
| int? → object | Boxed int or null | `object o = nullable` |
| enum → object | Boxed enum | `object o = Status.Active` |
| struct → object | Boxed struct | `object o = point` |

## Next Steps

- Learn [Boxing-Collections](../04-Boxing-Collections/00-Boxing-Collections.md) for collection patterns
- Study unboxing in [Unboxing-Rules](../../02-Unboxing-Type-Safety/01-Unboxing-Rules/00-Unboxing-Rules.md)
- Review performance in [Boxing-Overhead](../../03-Performance-Memory/01-Boxing-Overhead/00-Boxing-Overhead.md)
