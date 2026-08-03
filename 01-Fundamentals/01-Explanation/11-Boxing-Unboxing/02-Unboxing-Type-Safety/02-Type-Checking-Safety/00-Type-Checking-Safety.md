# Type Checking and Safety

## Overview

Type checking before unboxing is critical for preventing runtime exceptions. This section covers safe patterns for working with boxed values.

## The Problem: Type Safety

Unboxing without type checking can cause runtime exceptions:

```csharp
// Dangerous: Assumes type without checking
object mystery = GetSomeObject();
int value = (int)mystery;  // May throw if not int!

// Safe: Check type first
if (mystery is int intVal)
{
    int value = intVal;  // Safe to unbox
}
```

## Pattern Matching: 'is' Operator

### Basic Type Checking

```csharp
// Simple is check
object obj = 42;
if (obj is int)
{
    Console.WriteLine("It's an int");
}

// Combined check and cast (pattern matching)
if (obj is int intValue)
{
    Console.WriteLine($"Int value: {intValue}");  // intValue is int
}
```

### Multiple Type Checks

```csharp
void DisplayValue(object obj)
{
    // Pattern matching with multiple types
    if (obj is int intVal)
    {
        Console.WriteLine($"Int: {intVal}");
    }
    else if (obj is double doubleVal)
    {
        Console.WriteLine($"Double: {doubleVal}");
    }
    else if (obj is string strVal)
    {
        Console.WriteLine($"String: {strVal}");
    }
    else if (obj is bool boolVal)
    {
        Console.WriteLine($"Bool: {boolVal}");
    }
    else
    {
        Console.WriteLine("Unknown type");
    }
}

DisplayValue(42);        // "Int: 42"
DisplayValue(3.14);      // "Double: 3.14"
DisplayValue("hello");   // "String: hello"
DisplayValue(true);      // "Bool: true"
```

## GetType() Checking

### Exact Type Comparison

```csharp
// Check exact type
object obj = 42;

if (obj.GetType() == typeof(int))
{
    int value = (int)obj;  // Safe
    Console.WriteLine($"Exactly int: {value}");
}

// vs. 'is' (includes derived types)
if (obj is int intValue)
{
    Console.WriteLine($"Is or derived from int");
}

// For value types, both are equivalent
```

### GetType() with Multiple Checks

```csharp
void ProcessByType(object obj)
{
    Type type = obj.GetType();
    
    if (type == typeof(int))
    {
        int value = (int)obj;
        Console.WriteLine($"Int: {value}");
    }
    else if (type == typeof(double))
    {
        double value = (double)obj;
        Console.WriteLine($"Double: {value}");
    }
    else if (type == typeof(string))
    {
        string value = (string)obj;
        Console.WriteLine($"String: {value}");
    }
}

ProcessByType(42);     // "Int: 42"
ProcessByType(3.14);   // "Double: 3.14"
```

## Switch Expressions (C# 8+)

### Pattern Matching with Switch

```csharp
// Old style: if-else
void OldStyle(object obj)
{
    if (obj is int intVal)
        Console.WriteLine($"Int: {intVal}");
    else if (obj is double doubleVal)
        Console.WriteLine($"Double: {doubleVal}");
    else if (obj is string strVal)
        Console.WriteLine($"String: {strVal}");
}

// Modern style: switch expression
void ModernStyle(object obj) =>
    Console.WriteLine(obj switch
    {
        int intVal => $"Int: {intVal}",
        double doubleVal => $"Double: {doubleVal}",
        string strVal => $"String: {strVal}",
        _ => "Unknown"
    });

// Both work, switch expressions are more concise
```

### Switch with Complex Patterns

```csharp
object obj = 42;

string result = obj switch
{
    int i when i < 0 => "Negative int",
    int i when i == 0 => "Zero",
    int i => $"Positive int: {i}",
    double d when d > 100 => "Large double",
    double d => $"Double: {d}",
    string s => $"String: {s}",
    null => "Null",
    _ => "Unknown"
};

Console.WriteLine(result);  // "Positive int: 42"
```

## 'as' Operator: Safe Casting

### Safe Type Conversion

```csharp
// 'as' returns null if type doesn't match (doesn't throw)
object obj = 42;

int? intResult = obj as int?;      // Returns 42
double? doubleResult = obj as double?;  // Returns null
string strResult = obj as string;       // Returns null

// Check result
if (intResult.HasValue)
{
    Console.WriteLine($"Int: {intResult}");
}
if (doubleResult.HasValue)
{
    Console.WriteLine($"Double: {doubleResult}");
}
else
{
    Console.WriteLine("Not a double");
}
```

### 'as' vs Direct Cast

```csharp
object obj = "hello";

// Direct cast: throws InvalidCastException if wrong type
try
{
    int directValue = (int)obj;  // Throws!
}
catch (InvalidCastException)
{
    Console.WriteLine("Cannot cast");
}

// 'as' operator: returns null if wrong type
int? safeValue = obj as int?;  // Returns null, doesn't throw
if (safeValue.HasValue)
{
    Console.WriteLine($"Value: {safeValue}");
}
else
{
    Console.WriteLine("Not an int");
}
```

## Type Checking Patterns

### Pattern 1: Safe Unboxing Function

```csharp
// Generic type-safe unboxing
public T SafeUnbox<T>(object obj) where T : struct
{
    if (obj is T typedValue)
    {
        return typedValue;
    }
    throw new InvalidOperationException(
        $"Cannot unbox {obj?.GetType().Name} to {typeof(T).Name}");
}

// Usage
object boxedInt = 42;
int unboxed = SafeUnbox<int>(boxedInt);  // OK
// SafeUnbox<double>(boxedInt);  // Throws
```

### Pattern 2: TryUnbox Pattern

```csharp
// Try pattern similar to TryParse
public bool TryUnbox<T>(object obj, out T result) where T : struct
{
    if (obj is T typedValue)
    {
        result = typedValue;
        return true;
    }
    result = default;
    return false;
}

// Usage
object source = 42;
if (TryUnbox<int>(source, out int intValue))
{
    Console.WriteLine($"Got int: {intValue}");
}
else
{
    Console.WriteLine("Not an int");
}

if (TryUnbox<double>(source, out double doubleValue))
{
    Console.WriteLine($"Got double: {doubleValue}");
}
else
{
    Console.WriteLine("Not a double");
}
```

### Pattern 3: Type Dispatch

```csharp
// Process based on actual type
public void ProcessByType(object obj)
{
    switch (obj)
    {
        case int intVal:
            Console.WriteLine($"Processing int: {intVal}");
            break;
        case double doubleVal:
            Console.WriteLine($"Processing double: {doubleVal}");
            break;
        case string strVal:
            Console.WriteLine($"Processing string: {strVal}");
            break;
        case null:
            Console.WriteLine("Null object");
            break;
        default:
            Console.WriteLine($"Unknown type: {obj.GetType().Name}");
            break;
    }
}

ProcessByType(42);        // "Processing int: 42"
ProcessByType(3.14);      // "Processing double: 3.14"
ProcessByType("hello");   // "Processing string: hello"
ProcessByType(null);      // "Null object"
```

## Collections and Type Checking

### Iterating ArrayList Safely

```csharp
// Unsafe iteration
ArrayList list = new ArrayList { 1, 2, "three", 4 };
foreach (object item in list)
{
    int value = (int)item;  // Throws on "three"!
}

// Safe iteration with type checking
ArrayList list = new ArrayList { 1, 2, "three", 4 };
foreach (object item in list)
{
    if (item is int intVal)
    {
        Console.WriteLine($"Int: {intVal}");
    }
    else if (item is string strVal)
    {
        Console.WriteLine($"String: {strVal}");
    }
}

// Even better: Generic collection
List<int> genericList = new List<int> { 1, 2, 3, 4 };
foreach (int item in genericList)
{
    Console.WriteLine($"Int: {item}");  // No type checking needed
}
```

### LINQ with Type Checking

```csharp
// Use OfType<T> to filter and unbox
ArrayList mixed = new ArrayList { 1, 2, "three", 3.14, 4 };

// Get only integers
var integers = mixed.OfType<int>();
foreach (int item in integers)
{
    Console.WriteLine($"Int: {item}");
}
// Output: Int: 1, Int: 2, Int: 4

// Get only strings
var strings = mixed.OfType<string>();
foreach (string item in strings)
{
    Console.WriteLine($"String: {item}");
}
// Output: String: three
```

## Type Safety Best Practices

### Practice 1: Always Check Before Unboxing

```csharp
// BAD: Assume type
int value = (int)unknownObject;

// GOOD: Check first
if (unknownObject is int intValue)
{
    int value = intValue;
}

// GOOD: Use pattern matching
void Process(object obj)
{
    if (obj is int i)
        DoIntThing(i);
    else if (obj is string s)
        DoStringThing(s);
}
```

### Practice 2: Use OfType for Collections

```csharp
// BAD: Assumes all items are ints
ArrayList list = new ArrayList { 1, 2, 3 };
foreach (object item in list)
    ProcessInt((int)item);

// GOOD: Use OfType to filter and unbox
foreach (int item in list.OfType<int>())
    ProcessInt(item);
```

### Practice 3: Prefer Generics

```csharp
// BAD: Need type checking
ArrayList list = new ArrayList();
list.Add(42);
foreach (object item in list)
    if (item is int i) ProcessInt(i);

// GOOD: Generic - no type checking
List<int> list = new List<int>();
list.Add(42);
foreach (int item in list)
    ProcessInt(item);
```

## Comparison: Type Checking Methods

| Method | Syntax | Throws | Returns | Use Case |
|--------|--------|--------|---------|----------|
| **Direct Cast** | `(int)obj` | Yes | Value | Known type |
| **is check** | `obj is int` | No | bool | Type check |
| **is pattern** | `obj is int i` | No | bool + var | Check + use |
| **as operator** | `obj as int?` | No | nullable/null | Safe conversion |
| **GetType()** | `obj.GetType() == typeof(int)` | No | bool | Exact type |
| **OfType<T>** | `list.OfType<int>()` | No | IEnumerable | Filter collection |

## Performance Considerations

### Type Checking Performance

```csharp
using System.Diagnostics;

object source = 42;
int iterations = 1_000_000;

// Pattern matching (recommended)
var sw = Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    if (source is int intVal)
        _ = intVal;
}
sw.Stop();
Console.WriteLine($"Pattern matching: {sw.ElapsedMilliseconds}ms");

// GetType() comparison
sw.Restart();
for (int i = 0; i < iterations; i++)
{
    if (source.GetType() == typeof(int))
        _ = (int)source;
}
sw.Stop();
Console.WriteLine($"GetType(): {sw.ElapsedMilliseconds}ms");

// Direct cast (baseline)
sw.Restart();
for (int i = 0; i < iterations; i++)
{
    _ = (int)source;
}
sw.Stop();
Console.WriteLine($"Direct: {sw.ElapsedMilliseconds}ms");
```

## Error Handling Pattern

```csharp
public void SafeProcess(object obj)
{
    try
    {
        // Pattern 1: Type check
        if (obj is int intVal)
        {
            ProcessInt(intVal);
            return;
        }

        // Pattern 2: Multiple checks
        if (obj is double doubleVal)
        {
            ProcessDouble(doubleVal);
            return;
        }

        // Pattern 3: Null check
        if (obj == null)
        {
            ProcessNull();
            return;
        }

        // Pattern 4: Fallback
        throw new ArgumentException($"Unexpected type: {obj.GetType().Name}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
```

## Summary

- **Always check type** before unboxing
- **Use 'is' pattern** for modern type checking
- **Use 'as' operator** for safe conversions
- **Use OfType<T>** for collection filtering
- **Prefer generics** to avoid type checking entirely
- **Handle null** explicitly

## Next Steps

- Study nullable unboxing in [Nullable-Unboxing](../03-Nullable-Unboxing/00-Nullable-Unboxing.md)
- Learn performance in [Boxing-Overhead](../../03-Performance-Memory/01-Boxing-Overhead/00-Boxing-Overhead.md)
- Review best practices in [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
