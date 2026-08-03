# Unboxing and Type Safety

## Overview

This section covers unboxing (reverse of boxing) and type-safe techniques to prevent runtime errors when working with boxed values.

## Learning Path

### Beginner
1. **[Unboxing-Rules](01-Unboxing-Rules/00-Unboxing-Rules.md)** - Start here
   - What is unboxing?
   - Core unboxing rules
   - Type matching requirement
   - Null handling
   - Common errors

2. **[Type-Checking-Safety](02-Type-Checking-Safety/00-Type-Checking-Safety.md)** - Safe patterns
   - Pattern matching (is operator)
   - as operator for safe casting
   - GetType() checking
   - Switch expressions
   - Type dispatch patterns

3. **[Nullable-Unboxing](03-Nullable-Unboxing/00-Nullable-Unboxing.md)** - Nullable types
   - Nullable boxing behavior
   - Unboxing nullable types
   - Null handling
   - Collections with nullables

### Intermediate
- Master all three files
- Write type-safe code
- Handle edge cases
- Ready for Performance section

### Advanced
- Optimize type checking
- Handle complex scenarios
- Build robust systems

## Quick Reference

### Unboxing Rules

```csharp
// Rule 1: Type must match
int boxedInt = 42;
object boxed = boxedInt;
int unboxed = (int)boxed;  // OK - matches

// Rule 2: Cannot change type
// long value = (long)boxed;  // InvalidCastException!

// Rule 3: Unbox to nullable
int? nullable = (int?)boxed;  // OK - nullable
```

### Safe Unboxing Pattern

```csharp
// Safe: Check type first
if (obj is int intVal)
{
    int value = intVal;  // Guaranteed safe
}

// Safe: Use as operator
int? result = obj as int?;
if (result.HasValue)
{
    int value = result.Value;
}
```

## Topics Covered

### Unboxing Rules
- Unboxing mechanism
- Type matching requirement
- Null value handling
- InvalidCastException scenarios
- NullReferenceException scenarios

### Type Safety
- Pattern matching with 'is'
- Safe casting with 'as'
- Type checking with GetType()
- Switch expressions
- Type dispatch

### Nullable Handling
- Nullable boxing special behavior
- Null unboxing to nullable
- HasValue checking
- Default values
- Collections with nullables

## Code Examples

### Example 1: Safe Unboxing

```csharp
// Pattern matching
void Process(object obj)
{
    if (obj is int intVal)
        Console.WriteLine($"Int: {intVal}");
    else if (obj is double doubleVal)
        Console.WriteLine($"Double: {doubleVal}");
}
```

### Example 2: Null Handling

```csharp
int? nullable = null;
object boxedNull = nullable;  // Boxes as null

// Safe: Unbox to nullable
int? restored = (int?)boxedNull;  // null preserved

// Unsafe: Would throw
// int value = (int)boxedNull;  // NullReferenceException!
```

### Example 3: Type Checking

```csharp
// Loop safely through mixed types
foreach (object item in list)
{
    if (item is int i)
        HandleInt(i);
    else if (item is string s)
        HandleString(s);
}
```

## Key Concepts

1. **Unboxing** = object reference → value type
2. **Type must match** exactly
3. **Null handling** requires care
4. **Pattern matching** is safest
5. **Type checking first** prevents errors

## Practice Exercises

### Exercise 1: Unboxing Rules

```csharp
object boxedInt = 42;
// Which are valid?
int x = (int)boxedInt;              // Valid?
long y = (long)boxedInt;            // Valid?
int? z = (int?)boxedInt;            // Valid?
```

### Exercise 2: Safe Patterns

```csharp
object mystery = GetValue();
// Write safe unboxing code
// Handle all cases
```

### Exercise 3: Nullable Scenarios

```csharp
int? nullable = null;
object boxed = nullable;
// Can you unbox? How?
// What happens?
```

## Error Prevention

### Common Errors

| Error | Cause | Fix |
|-------|-------|-----|
| InvalidCastException | Type mismatch | Check type first |
| NullReferenceException | Unbox null | Use nullable |
| Index out of bounds | Wrong collection | Verify collection |

### Prevention Checklist

- [ ] Check type before unboxing?
- [ ] Handle null values?
- [ ] Use pattern matching?
- [ ] Test edge cases?
- [ ] Verify collection contents?

## Type Safety Patterns

### Pattern 1: Type-Safe Loop

```csharp
foreach (object item in list)
{
    if (item is int i)
        ProcessInt(i);
    else if (item is string s)
        ProcessString(s);
    else
        ProcessOther(item);
}
```

### Pattern 2: TryParse Pattern

```csharp
public bool TryUnbox<T>(object obj, out T result)
    where T : struct
{
    if (obj is T typedValue)
    {
        result = typedValue;
        return true;
    }
    result = default;
    return false;
}
```

### Pattern 3: Null-Safe Unboxing

```csharp
int? SafeUnbox(object obj)
{
    return obj as int?;
}
```

## Related Topics

- [Boxing-Fundamentals](../01-Boxing-Fundamentals/README.md) - Boxing basics
- [Performance-Memory](../03-Performance-Memory/README.md) - Performance details
- [Best-Practices-Interview](../04-Best-Practices-Interview/README.md) - Best practices

## Next Steps

1. **Read** Unboxing-Rules
2. **Study** Type-Checking-Safety
3. **Master** Nullable-Unboxing
4. **Practice** safe unboxing patterns
5. **Move to** Performance-Memory

## Summary

Unboxing and type safety teach you:
- How to safely reverse boxing
- Type checking patterns
- Null value handling
- Preventing runtime exceptions
- Building robust systems

**Key Takeaway:** Always check type before unboxing.

---

**Ready to learn?**

- **Rules:** Start with [Unboxing-Rules](01-Unboxing-Rules/00-Unboxing-Rules.md)
- **Safety:** Learn in [Type-Checking-Safety](02-Type-Checking-Safety/00-Type-Checking-Safety.md)
- **Nullable:** Study [Nullable-Unboxing](03-Nullable-Unboxing/00-Nullable-Unboxing.md)
