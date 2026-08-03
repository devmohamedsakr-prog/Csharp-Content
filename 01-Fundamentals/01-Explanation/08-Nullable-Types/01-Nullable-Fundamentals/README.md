# Nullable Fundamentals

## Overview
Understand what null is and how to work with nullable types in C#.

## Files

1. **00-What-is-Null.md** - Null concept, reference vs value types, null in different contexts
2. **00-Nullable-Value-Types.md** - Creating nullable types, operations, practical examples

## Quick Reference

```csharp
// Null represents no value
string? nullStr = null;

// Value types need ? to be nullable
int? age = null;
int? score = 95;

// Default operations
int val = age.GetValueOrDefault(0);
int val2 = age ?? 0;
```

## Key Concepts

✓ Null represents "no value"
✓ Reference types nullable by default
✓ Value types need `?` syntax
✓ Null different from default values
✓ Operations propagate null

---

[← Back to Main](../README.md) | [Next: Null Checking →](../02-Null-Checking/README.md)
