# Unboxing Rules

## Overview

Unboxing is the reverse of boxing - converting an object reference back to a value type. Unboxing has strict rules and can throw exceptions if done incorrectly.

## What is Unboxing?

Unboxing copies the boxed value from the heap back to a value type variable on the stack.

### Simple Unboxing Example

```csharp
// Boxing first
int original = 42;
object boxed = original;  // Boxes to heap

// Unboxing: object → value type
int unboxed = (int)boxed;  // Unboxes from heap
Console.WriteLine(unboxed); // 42

// Memory:
// Before unbox: Stack=[ref], Heap=[42 wrapped]
// After unbox:  Stack=[42], Heap=[42 wrapped] (still there until GC)
```

## Core Unboxing Rules

### Rule 1: Must Unbox to Original Type

The most important rule: **you must unbox to the exact type that was boxed**.

```csharp
// ✓ CORRECT: Unbox to original type
int boxedInt = 42;
object boxed = boxedInt;
int unboxed = (int)boxed;  // OK - matches original type

// ✗ WRONG: Unbox to different type
// short shortVal = (short)boxed;  // InvalidCastException!
// long longVal = (long)boxed;     // InvalidCastException!
// byte byteVal = (byte)boxed;     // InvalidCastException!

// Even though short, long, byte can hold the value,
// unboxing requires exact type match
```

### Rule 2: Type Mismatch Throws InvalidCastException

```csharp
// Type mismatch errors
object boxedInt = 42;
object boxedDouble = 3.14;

// Correct unboxing
int i = (int)boxedInt;        // OK
double d = (double)boxedDouble; // OK

// Incorrect unboxing - throws exception
try
{
    // double d = (double)boxedInt;  // InvalidCastException
    // int i = (int)boxedDouble;     // InvalidCastException
}
catch (InvalidCastException ex)
{
    Console.WriteLine($"Cannot unbox {ex.Message}");
}
```

### Rule 3: Can Unbox to Nullable Type

You can unbox a boxed value type to a nullable version:

```csharp
// Unbox to nullable
int value = 42;
object boxed = value;
int? nullable = (int?)boxed;  // OK - unboxes to nullable

// This is useful for type-safe operations
int? result = boxed as int?;  // Alternative approach
```

### Rule 4: Nullable Boxing Special Behavior

When you box a nullable type with a value, it boxes the underlying type (not the Nullable wrapper):

```csharp
// Boxing nullable with value
int? nullable = 42;
object boxed = nullable;  // Boxes as int (not Nullable<int>)

// Unbox back
int unboxed = (int)boxed;     // OK - unboxes as int
int? nullable2 = (int?)boxed; // OK - unboxes as nullable
```

## Null Handling

### Unboxing Null Values

Nullable types handle null specially during unboxing:

```csharp
// Boxing null
int? nullableNull = null;
object boxedNull = nullableNull;  // Boxes as null (not as int)

// Unbox null to nullable
int? restoredNull = (int?)boxedNull;  // OK - null preserved
// restoredNull is null (no exception)

// ✗ WRONG: Unbox null to non-nullable
try
{
    // int value = (int)boxedNull;  // NullReferenceException!
}
catch (NullReferenceException)
{
    Console.WriteLine("Cannot unbox null to non-nullable");
}
```

### Safe Null Unboxing Pattern

```csharp
object source = null;  // Could be null boxed value

// Safe pattern 1: Use nullable
int? result1 = (int?)source;  // null if source is null
if (result1.HasValue)
{
    int value = result1.Value;
}

// Safe pattern 2: Check for null first
if (source != null)
{
    int value = (int)source;  // Safe - not null
}

// Safe pattern 3: Use 'as' operator
int? result3 = source as int?;  // null if cannot unbox
```

## Unboxing Different Types

### Unboxing Primitives

```csharp
// Unbox each primitive type correctly
object boxedInt = 42;
int intVal = (int)boxedInt;

object boxedDouble = 3.14;
double doubleVal = (double)boxedDouble;

object boxedBool = true;
bool boolVal = (bool)boxedBool;

object boxedChar = 'A';
char charVal = (char)boxedChar;

// Each requires exact type match
```

### Unboxing Structs

```csharp
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

Point p = new Point { X = 10, Y = 20 };
object boxed = p;  // Box struct

// Unbox struct - must use exact type
Point unboxed = (Point)boxed;  // OK
Console.WriteLine($"({unboxed.X}, {unboxed.Y})");  // (10, 20)

// Unbox to wrong type fails
try
{
    // int x = (int)boxed;  // InvalidCastException - not an int!
}
catch (InvalidCastException)
{
    Console.WriteLine("Cannot unbox struct as int");
}
```

### Unboxing Enums

```csharp
public enum Status { Active = 1, Inactive = 2 }

Status status = Status.Active;
object boxed = status;  // Boxes as Status

// Unbox to enum type
Status unboxed = (Status)boxed;  // OK
Console.WriteLine(unboxed);  // Active

// Unbox to underlying type (int)
int underlying = (int)boxed;  // OK - enum is backed by int
Console.WriteLine(underlying);  // 1
```

## Common Unboxing Patterns

### Pattern 1: Type Checking Before Unboxing

```csharp
// Safe pattern: Check type first
void ProcessObject(object obj)
{
    if (obj is int intVal)
    {
        Console.WriteLine($"Int: {intVal}");  // Unboxes safely
    }
    else if (obj is double doubleVal)
    {
        Console.WriteLine($"Double: {doubleVal}");
    }
    else if (obj is string str)
    {
        Console.WriteLine($"String: {str}");
    }
}

ProcessObject(42);      // "Int: 42"
ProcessObject(3.14);    // "Double: 3.14"
ProcessObject("text");  // "String: text"
```

### Pattern 2: Try-Cast with 'as' Operator

```csharp
// Safe pattern: Use 'as' operator
void ProcessBoxed(object obj)
{
    int? intVal = obj as int?;
    if (intVal.HasValue)
    {
        Console.WriteLine($"Int: {intVal}");
    }
    else
    {
        Console.WriteLine("Not an int");
    }
}

ProcessBoxed(42);       // "Int: 42"
ProcessBoxed("text");   // "Not an int"
ProcessBoxed(null);     // "Not an int"
```

### Pattern 3: Try-Catch for Unboxing

```csharp
// Handle unboxing exceptions
void SafeUnbox(object obj)
{
    try
    {
        int unboxed = (int)obj;
        Console.WriteLine($"Unboxed: {unboxed}");
    }
    catch (InvalidCastException)
    {
        Console.WriteLine("Object is not a boxed int");
    }
    catch (NullReferenceException)
    {
        Console.WriteLine("Object is null");
    }
}

SafeUnbox(42);        // "Unboxed: 42"
SafeUnbox("text");    // "Object is not a boxed int"
SafeUnbox(null);      // "Object is null" (if unboxing to non-nullable)
```

## Unboxing in Collections

### ArrayList Unboxing

```csharp
// Add mixed types
ArrayList list = new ArrayList();
list.Add(42);
list.Add(3.14);
list.Add("text");

// Unbox on retrieval
foreach (object item in list)
{
    if (item is int intVal)
    {
        Console.WriteLine($"Int: {intVal}");  // Unboxes
    }
    else if (item is double doubleVal)
    {
        Console.WriteLine($"Double: {doubleVal}");
    }
    else if (item is string str)
    {
        Console.WriteLine($"String: {str}");
    }
}
```

### Generic Collection (No Unboxing)

```csharp
// Generic collection - no unboxing needed
List<int> list = new List<int>();
list.Add(42);
list.Add(100);

// Direct access - no unboxing
foreach (int item in list)
{
    Console.WriteLine(item);  // No unboxing
}
```

## Common Unboxing Errors

### Error 1: Type Mismatch

```csharp
// ✗ ERROR: Boxing as int, unboxing as long
object boxed = 42;  // Boxes as int
try
{
    long value = (long)boxed;  // InvalidCastException!
}
catch (InvalidCastException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

// ✓ FIX: Unbox to original type first
int intVal = (int)boxed;
long longVal = (long)intVal;  // Now convert
```

### Error 2: Unboxing Null to Non-Nullable

```csharp
// ✗ ERROR: Null cannot unbox to non-nullable
object nullBox = null;
try
{
    int value = (int)nullBox;  // NullReferenceException!
}
catch (NullReferenceException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

// ✓ FIX: Unbox to nullable
int? nullableValue = (int?)nullBox;  // OK - null
```

### Error 3: Wrong Struct Type

```csharp
public struct Point { public int X; }
public struct Vector { public int X; }

Point p = new Point { X = 10 };
object boxed = p;

try
{
    Vector v = (Vector)boxed;  // InvalidCastException!
}
catch (InvalidCastException)
{
    Console.WriteLine("Cannot unbox Point as Vector");
}

// ✓ FIX: Unbox to correct type
Point unboxed = (Point)boxed;  // OK
```

## Unboxing Performance

### Benchmark: Unboxing Cost

```csharp
using System.Diagnostics;

// Setup
object boxedInt = 42;
int iterations = 1_000_000;

// Unboxing performance
var sw = Stopwatch.StartNew();
int sum = 0;
for (int i = 0; i < iterations; i++)
{
    sum += (int)boxedInt;  // Unbox each time
}
sw.Stop();
Console.WriteLine($"Unboxing: {sw.ElapsedMilliseconds}ms");

// Compare with non-unboxing
int directInt = 42;
sw.Restart();
sum = 0;
for (int i = 0; i < iterations; i++)
{
    sum += directInt;  // No unboxing
}
sw.Stop();
Console.WriteLine($"Direct: {sw.ElapsedMilliseconds}ms");
// Unboxing slightly faster than boxing, but slower than direct access
```

## Unboxing Best Practices

1. **Check type before unboxing**
```csharp
if (obj is int intVal)
    int value = intVal;  // Safe unboxing
```

2. **Handle null carefully**
```csharp
int? nullable = obj as int?;  // Returns null if not int
if (nullable.HasValue)
    int value = nullable.Value;
```

3. **Prefer generics to avoid unboxing**
```csharp
// Bad: Unboxing in loop
foreach (object item in nonGeneric)
    int value = (int)item;  // Unboxes

// Good: Generic collection
foreach (int value in generic)
    // No unboxing
```

4. **Use pattern matching**
```csharp
void Process(object obj)
{
    if (obj is int intVal)
        Console.WriteLine($"Int: {intVal}");
    else if (obj is double doubleVal)
        Console.WriteLine($"Double: {doubleVal}");
}
```

## Summary

| Rule | Description | Example |
|------|-------------|---------|
| Type Match | Must unbox to original type | `int x = (int)boxedInt;` |
| No Type Change | Cannot change type | `(long)(int)` fails |
| Null Safe | Unbox null to nullable | `int? x = (int?)null;` |
| Error Handling | Use is/as patterns | `if (obj is int i)` |
| Performance | Faster than boxing | Still slower than direct |

## Next Steps

- Learn type safety in [Type-Checking-Safety](../02-Type-Checking-Safety/00-Type-Checking-Safety.md)
- Study nullable unboxing in [Nullable-Unboxing](../03-Nullable-Unboxing/00-Nullable-Unboxing.md)
- Review best practices in [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
