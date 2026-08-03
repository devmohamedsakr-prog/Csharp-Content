# Nullable Types and Unboxing

## Overview

Nullable types (T?) have special boxing and unboxing behavior that differs from regular value types. Understanding this prevents subtle bugs and runtime errors.

## How Nullable Boxing Works

### Nullable with Value

When you box a nullable type that contains a value, it boxes the underlying type, NOT the Nullable<T> wrapper:

```csharp
// Nullable with value
int? nullable = 42;
object boxed = nullable;

// What gets boxed?
// NOT: Nullable<int> { value: 42 }
// BUT: int { value: 42 }

// This means:
object boxedInt = 42;
object boxedNullable = (int?)42;
// boxedInt and boxedNullable are equivalent!
// Both contain wrapped int 42 on heap
```

### Nullable with Null

Boxing a nullable with null produces a special null object reference:

```csharp
// Nullable with null
int? nullableNull = null;
object boxedNull = nullableNull;  // Boxes as null

// The resulting object reference is null
// Not a boxed Nullable<int>
// Not a boxed 0

object isNull = boxedNull;  // isNull is null
if (isNull == null)
    Console.WriteLine("It's null");  // This prints
```

## Unboxing Nullable Types

### Unboxing to Nullable

You can unbox any boxed value type to its nullable equivalent:

```csharp
// Boxing int
int value = 42;
object boxed = value;

// Unboxing to nullable (always works)
int? nullable = (int?)boxed;  // OK - unboxes to nullable

// Nullable contains the value
if (nullable.HasValue)
    Console.WriteLine(nullable.Value);  // 42
```

### Unboxing Null to Nullable

Boxing null unboxes to nullable null:

```csharp
// Boxing null
int? nullableNull = null;
object boxedNull = nullableNull;  // Boxes as null

// Unboxing null to nullable
int? restoredNull = (int?)boxedNull;  // OK - null
if (!restoredNull.HasValue)
    Console.WriteLine("Null preserved");
```

### Unboxing Null to Non-Nullable (Error)

You cannot unbox null to a non-nullable value type:

```csharp
// Boxing null
int? nullableNull = null;
object boxedNull = nullableNull;

// ✗ ERROR: Cannot unbox null to non-nullable int
try
{
    int value = (int)boxedNull;  // NullReferenceException!
}
catch (NullReferenceException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

// ✓ CORRECT: Unbox to nullable first
int? nullable = (int?)boxedNull;  // OK - null
```

## Key Nullable Unboxing Rules

### Rule 1: Nullable with Value

```csharp
int? withValue = 42;
object boxed = withValue;  // Boxes as int (value 42)

// Can unbox to both
int unboxedInt = (int)boxed;      // OK - 42
int? unboxedNullable = (int?)boxed;  // OK - 42
```

### Rule 2: Nullable with Null

```csharp
int? withNull = null;
object boxed = withNull;  // Boxes as null

// Cannot unbox null to non-nullable
try
{
    int value = (int)boxed;  // NullReferenceException!
}
catch (NullReferenceException)
{
    Console.WriteLine("Null cannot unbox to non-nullable");
}

// Must unbox to nullable
int? nullable = (int?)boxed;  // OK - null
```

### Rule 3: Unbox to Correct Underlying Type

```csharp
// Even with nullable, type must match
int? boxedAsInt = 42;
object boxed = boxedAsInt;

// Must unbox to original type
int? correct = (int?)boxed;      // OK
long? wrong = (long?)boxed;       // InvalidCastException!
double? alsoWrong = (double?)boxed;  // InvalidCastException!
```

## Common Nullable Unboxing Patterns

### Pattern 1: Safe Null Handling

```csharp
// Scenario: Working with boxed nullables
object source = null;  // Could be null boxed int

// Safe approach: unbox to nullable
int? result = source as int?;
if (result.HasValue)
{
    int value = result.Value;
    Console.WriteLine($"Value: {value}");
}
else
{
    Console.WriteLine("No value");
}
```

### Pattern 2: Default Values

```csharp
// Get value with default
object source = null;
int? nullable = source as int?;
int finalValue = nullable ?? 0;  // 0 if null
Console.WriteLine(finalValue);   // 0
```

### Pattern 3: Converting Collections

```csharp
// ArrayList with mixed nullables and values
ArrayList list = new ArrayList();
list.Add(null);
list.Add((int?)42);
list.Add((int?)100);
list.Add(null);

// Safe iteration
List<int> values = new List<int>();
foreach (object item in list)
{
    int? nullable = item as int?;
    if (nullable.HasValue)
    {
        values.Add(nullable.Value);
    }
}
// values: [42, 100]
```

## Nullable vs Non-Nullable Boxing Comparison

### Value Type: Different Results

```csharp
// Non-nullable boxing
int nonNullable = 42;
object boxedNonNull = nonNullable;  // Boxes int

// Nullable boxing (with value)
int? nullable = 42;
object boxedNullable = nullable;  // Boxes int (same!)

// They're equivalent
// Both unwrap to int 42
int fromNonNull = (int)boxedNonNull;      // 42
int fromNullable = (int)boxedNullable;    // 42 (same)
```

### Null Type: Different Results

```csharp
// Non-nullable int cannot be null
// int nullValue = null;  // Compile error!

// Nullable int can be null
int? nullableNull = null;
object boxedNull = nullableNull;  // Boxes as null object

// The boxed value is null
object result = boxedNull;
if (result == null)
    Console.WriteLine("It's null");  // This prints

// Can only unbox to nullable
int? unboxed = (int?)boxedNull;  // OK - null
// int value = (int)boxedNull;   // NullReferenceException!
```

## Working with Collections of Nullables

### Filtering Nullables

```csharp
// Collection with mixed nullables
ArrayList list = new ArrayList();
list.Add(null);
list.Add((int?)10);
list.Add((int?)20);
list.Add(null);
list.Add((int?)30);

// Filter non-null values
List<int> nonNullValues = new List<int>();
foreach (object item in list)
{
    if (item is int? nullable && nullable.HasValue)
    {
        nonNullValues.Add(nullable.Value);
    }
}
// Result: [10, 20, 30]

// Alternative: Use OfType (loses null info)
var typed = list.OfType<int>();  // Gets boxed ints, not nullables
```

### Preserving Nullables

```csharp
// When you need to preserve null information
List<int?> nullables = new List<int?>();
foreach (object item in list)
{
    if (item is int? nullable)
    {
        nullables.Add(nullable);  // Preserves null
    }
}
// Result: [null, 10, 20, null, 30]
```

## Nullable Unboxing Errors

### Error 1: Wrong Type

```csharp
// Boxing long as nullable
long? boxedLong = 42L;
object boxed = boxedLong;

// ✗ ERROR: Cannot unbox long to int?
try
{
    int? intNullable = (int?)boxed;  // InvalidCastException!
}
catch (InvalidCastException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

// ✓ CORRECT: Match types
long? longNullable = (long?)boxed;  // OK
```

### Error 2: Non-Null Boxing

```csharp
// Non-nullable boxing
int nonNullable = 42;
object boxed = nonNullable;  // Boxes int (not int?)

// Unboxing works but preserves no-null semantics
int? unboxed = (int?)boxed;  // 42, HasValue = true
int direct = (int)boxed;     // 42
```

### Error 3: Generic Type Confusion

```csharp
// Generic method with nullable
public T? UnboxNullable<T>(object obj) where T : struct
{
    return (T?)obj;  // May throw if type mismatch
}

// Using it
object boxedInt = 42;
int? result = UnboxNullable<int>(boxedInt);  // OK

object boxedLong = 42L;
int? wrongType = UnboxNullable<int>(boxedLong);  // InvalidCastException!
```

## Nullable Unboxing Best Practices

### Practice 1: Use 'as' Operator

```csharp
// Safe approach
object source = GetValue();
int? result = source as int?;  // Returns null if not int

if (result.HasValue)
{
    int value = result.Value;
}
```

### Practice 2: Check HasValue

```csharp
// Always check HasValue
int? nullable = (int?)source;
if (nullable.HasValue)
{
    // Safe to use nullable.Value
    int value = nullable.Value;
}
else
{
    // Handle null case
}
```

### Practice 3: Use Null Coalescing

```csharp
// Default value for null
int? result = (int?)source;
int finalValue = result ?? 0;  // 0 if null
```

### Practice 4: Pattern Matching

```csharp
// Modern pattern matching
void Process(object obj)
{
    if (obj is int? nullable && nullable.HasValue)
    {
        Console.WriteLine($"Value: {nullable.Value}");
    }
    else if (obj is int? && obj == null)
    {
        Console.WriteLine("Null int");
    }
    else
    {
        Console.WriteLine("Not a nullable int");
    }
}
```

## Performance Considerations

### Nullable vs Non-Nullable Performance

```csharp
using System.Diagnostics;

// Non-nullable unboxing
object boxedInt = 42;
var sw = Stopwatch.StartNew();
for (int i = 0; i < 1_000_000; i++)
{
    int val = (int)boxedInt;
}
sw.Stop();
Console.WriteLine($"Non-nullable: {sw.ElapsedMilliseconds}ms");

// Nullable unboxing
sw.Restart();
for (int i = 0; i < 1_000_000; i++)
{
    int? val = (int?)boxedInt;
}
sw.Stop();
Console.WriteLine($"Nullable: {sw.ElapsedMilliseconds}ms");

// Performance is similar
```

## Nullable Unboxing Summary Table

| Scenario | Boxed | Unbox to int | Unbox to int? | Result |
|----------|-------|-------------|---------------|--------|
| int (42) | int | OK | OK | 42 / 42 |
| int? (42) | int | OK | OK | 42 / 42 |
| int? (null) | null | ✗ Error | OK | - / null |
| long (42) | long | ✗ Error | ✗ Error | - / - |

## Summary

- **Nullable boxing** boxes the underlying type, not the wrapper
- **Null boxing** results in null object reference
- **Unboxing null** requires unboxing to nullable (int?)
- **Type must match** even with nullable
- **Use 'as' operator** for safe unboxing
- **Check HasValue** before using nullable value

## Next Steps

- Study performance in [Boxing-Overhead](../../03-Performance-Memory/01-Boxing-Overhead/00-Boxing-Overhead.md)
- Learn best practices in [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
- Review common mistakes in [Common-Mistakes](../../04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md)
