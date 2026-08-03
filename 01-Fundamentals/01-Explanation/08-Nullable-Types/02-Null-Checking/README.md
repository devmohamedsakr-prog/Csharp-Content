# Null Checking

## Overview
Master safe null checking techniques: HasValue, ??, and ?. operators.

## Files

1. **00-HasValue-Property.md** - Checking values, safe extraction, GetValueOrDefault
2. **00-Null-Coalescing-Operator.md** - ?? operator, ??= assignment, chaining
3. **00-Null-Conditional-Operator.md** - ?. and ?[], chaining, combining

## Quick Reference

```csharp
// Check for value
if (age.HasValue) { }

// Null coalescing
int val = age ?? 0;

// Null conditional
int? len = text?.Length;

// Assignment
age ??= 18;
```

## Key Operators

✓ `.HasValue` - Check if has value
✓ `??` - Default if null
✓ `??=` - Assign if null
✓ `?.` - Safe member access
✓ `?[]` - Safe indexing

---

[← Back to Main](../README.md) | [Next: Null Patterns →](../03-Null-Patterns/README.md)
