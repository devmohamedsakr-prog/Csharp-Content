# String Basics

## Overview
Master string fundamentals: creation, properties, indexing, and basic access patterns.

## Learning Path

### 1. String Creation (Start here)
- String literals and escape sequences
- String interpolation (C# 6+)
- Concatenation methods
- Verbatim and raw strings (C# 11+)
- String constructors
- Type conversions

**Time:** 20-25 minutes

### 2. String Properties and Access
- Length property
- Character indexing (0-indexed)
- Character ranges (C# 8+)
- Character arrays
- Safe access patterns
- String comparison basics

**Time:** 20-25 minutes

## Files in This Section

1. **00-String-Creation.md** - 7 creation methods, interpolation, concatenation
2. **00-String-Properties.md** - Length, indexing, ranges, character access

## Quick Reference

```csharp
// Creation
string msg = "Hello";  // Literal
string msg2 = $"Hello {name}";  // Interpolation
string msg3 = "Line 1\nLine 2";  // Escape sequences

// Properties
int len = msg.Length;  // 5
char first = msg[0];  // 'H'
char last = msg[^1];  // 'o' (last)

// Safe access
if (index < msg.Length) {
    char c = msg[index];
}
```

## Key Concepts

- **Immutable** - Strings never change, operations create new strings
- **0-indexed** - First character is at index 0
- **Unicode** - Full Unicode support including emoji
- **Interning** - Compiler optimizes duplicate literals
- **Escape sequences** - \n, \t, \\, \", \'
- **Ranges** - [start..end] syntax for extraction

## Common Patterns

✓ **String interpolation**
```csharp
string msg = $"Value: {x:F2}";
```

✓ **Safe character access**
```csharp
if (index >= 0 && index < text.Length) {
    char c = text[index];
}
```

✓ **Multi-line strings**
```csharp
string multiline = @"Line 1
Line 2";
```

✓ **Range extraction**
```csharp
string sub = text[0..5];
string last = text[^5..];
```

## When to Use Strings

✓ Text data storage
✓ User input
✓ Display messages
✓ Identifiers and keys
✓ Configuration data

## Best Practices

✓ Use interpolation for formatting
✓ Validate null/empty before use
✓ Use ranges for extraction (C# 8+)
✓ Use verbatim strings for paths
✓ Check bounds before indexing
✓ Use string.Empty instead of ""

## Common Mistakes

❌ Index out of bounds - Check bounds first
❌ Null reference - Validate before use
❌ Case sensitivity - Use appropriate comparison
❌ Forgetting immutability - Assign result
❌ Assuming non-empty - Check length

## Self-Assessment

Can you:
- [ ] Create strings using different methods?
- [ ] Use string interpolation effectively?
- [ ] Access characters safely?
- [ ] Use ranges for extraction?
- [ ] Understand immutability?
- [ ] Validate input safely?

---

## Related Topics

- **String Operations** - Methods and manipulation
- **String Patterns** - Comparison, formatting, validation
- **Collections** - Working with string collections
- **LINQ** - Query strings with LINQ

## Next Steps

1. ✓ Learn String Creation
2. ✓ Study String Properties
3. → Move to String Operations
4. → Study String Patterns
5. → Review Best Practices
