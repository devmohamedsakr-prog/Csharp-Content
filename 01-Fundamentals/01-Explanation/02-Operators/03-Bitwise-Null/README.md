# Bitwise, Null, and Ternary Operators

## Overview

Advanced operators for bit manipulation, null handling, and conditional expressions.

## Categories

### Bitwise Operators
Operate directly on binary representations.

**Files**: `01-Bitwise/00-Bitwise-Operators.md`

**Topics Covered**:
- AND (&), OR (|), XOR (^), NOT (~)
- Left shift (<<), right shift (>>)
- Bit counting and manipulation
- Flag systems with [Flags]
- Performance considerations

**Key Concepts**:
- Operate on individual bits
- Fast for powers of 2
- Essential for flags/permissions
- Low-level bit manipulation

### Null-Related Operators
Safe null handling and default values.

**Files**: `02-Null-Related/00-Null-Related.md`

**Topics Covered**:
- Null-coalescing (??)
- Null-conditional (?.)
- Null-conditional with indexers (?[])
- Null-coalescing assignment (??=)
- Pattern matching with null

**Key Concepts**:
- Prevent NullReferenceException
- Provide defaults elegantly
- Lazy initialization
- Modern C# 8+ patterns

### Ternary and Precedence
Conditional expressions and operator evaluation order.

**Files**: `03-Ternary-Precedence/00-Ternary-Precedence.md`

**Topics Covered**:
- Ternary operator (? :)
- Nested ternary
- Operator precedence complete list
- Precedence examples
- Switch expressions alternative

**Key Concepts**:
- Shorthand if-else
- Avoid complex nesting
- Know precedence rules
- Use parentheses for clarity

## Quick Reference

### Bitwise

| Operator | Name | Example |
|----------|------|---------|
| & | AND | 5 & 3 = 1 |
| \| | OR | 5 \| 3 = 7 |
| ^ | XOR | 5 ^ 3 = 6 |
| ~ | NOT | ~5 = -6 |
| << | Left Shift | 5 << 1 = 10 |
| >> | Right Shift | 20 >> 1 = 10 |

### Null-Related

| Operator | Purpose | Example |
|----------|---------|---------|
| ?? | Default | x ?? "default" |
| ?. | Safe access | person?.Name |
| ?[ ] | Safe index | array?[0] |
| ??= | Assign if null | x ??= value |

### Ternary

| Operator | Purpose | Example |
|----------|---------|---------|
| ?: | Conditional | condition ? true : false |

## Common Use Cases

### Flag System
```csharp
[Flags]
public enum Permissions {
    Read = 1 << 0,
    Write = 1 << 1,
    Delete = 1 << 2
}

if ((userPerms & Permissions.Read) != 0) {
    // Has read permission
}
```

### Safe Navigation
```csharp
string city = person?.Address?.City ?? "Unknown";
```

### Lazy Initialization
```csharp
cache ??= LoadCache();
```

### Conditional Expression
```csharp
string status = age >= 18 ? "Adult" : "Minor";
```

## Precedence Order

Highest to lowest:
1. Parentheses ()
2. Unary (!, ~, ++, --)
3. Multiplicative (*, /, %)
4. Additive (+, -)
5. Shift (<<, >>)
6. Relational (<, >, <=, >=)
7. Equality (==, !=)
8. AND (&)
9. XOR (^)
10. OR (|)
11. Logical AND (&&)
12. Logical OR (||)
13. Null-coalescing (??, ??=)
14. Ternary (?:)
15. Assignment (=, +=, -=, etc.)

## Best Practices

✓ Use bitwise for flags
```csharp
[Flags]
enum Permissions { Read = 1, Write = 2 }
```

✓ Combine null operators
```csharp
string result = user?.GetEmail() ?? "no-email";
```

✓ Simple ternary only
```csharp
// Good
string status = isActive ? "On" : "Off";

// Bad: too nested
string x = a ? b ? c : d : e ? f : g;
```

✓ Use parentheses
```csharp
int result = (a + b) * c;  // Clear
```

✓ Use switch expressions for complex logic
```csharp
string category = age switch {
    < 18 => "Minor",
    < 65 => "Adult",
    _ => "Senior"
};
```

## Common Mistakes to Avoid

❌ Complex nested ternary
❌ Bitwise (&) instead of logical (&&)
❌ Wrong precedence assumptions
❌ Not using null-conditional
❌ Forgetting parentheses

## Learning Path

1. Start with **Bitwise Operators** - bit manipulation
2. Learn **Null-Related** - safe navigation
3. Study **Ternary and Precedence** - conditional expressions
4. Practice **Best Practices** - real-world patterns

## Interview Preparation

Key points:
- Flags enum with [Flags]
- Null-coalescing vs null-conditional
- Operator precedence complete order
- Power of 2 bitwise trick
- When to use switch expressions

## Navigation

- **Parent**: [Operators](../README.md)
- **Bitwise**: `01-Bitwise/00-Bitwise-Operators.md`
- **Null-Related**: `02-Null-Related/00-Null-Related.md`
- **Ternary & Precedence**: `03-Ternary-Precedence/00-Ternary-Precedence.md`
- **Comparison & Logical**: `../02-Comparison-Logical/README.md`
- **Best Practices**: `../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md`
