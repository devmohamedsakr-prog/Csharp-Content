# Comparison and Logical Operators

## Overview

This section covers comparison operators for testing values and logical operators for combining conditions.

## Categories

### Comparison Operators
Test relationships between values, returning boolean results.

**Files**: `01-Comparison/00-Comparison-Operators.md`

**Topics Covered**:
- Equality (==) and inequality (!=)
- Relational operators (<, >, <=, >=)
- String comparison nuances
- Reference vs value equality
- Pattern matching

**Key Concepts**:
- Comparison returns true/false
- String comparison is case-sensitive
- Reference types compare by reference (usually)
- Pattern matching simplifies conditions

### Logical AND (&&)
Returns true only if both conditions are true.

**Files**: `02-Logical-AND/00-Logical-AND.md`

**Topics Covered**:
- AND operator behavior
- Short-circuit evaluation
- Common use cases
- Permission checking
- Range validation

**Key Concepts**:
- Both must be true
- First false stops evaluation
- Efficient null checking
- Order matters for performance

### Logical OR and NOT
OR returns true if any condition is true, NOT negates boolean.

**Files**: `03-Logical-OR-NOT/00-Logical-OR-NOT.md`

**Topics Covered**:
- OR operator behavior
- NOT operator behavior
- De Morgan's Laws
- Combining operators
- Complex logic simplification

**Key Concepts**:
- At least one true for OR
- NOT flips value
- De Morgan's simplifies logic
- Parentheses clarify intent

## Quick Reference

### Comparison

| Operator | Meaning | Example |
|----------|---------|---------|
| == | Equal | x == 5 |
| != | Not equal | x != 5 |
| > | Greater than | x > 5 |
| >= | Greater/equal | x >= 5 |
| < | Less than | x < 5 |
| <= | Less/equal | x <= 5 |

### Logical

| Operator | Meaning | Example |
|----------|---------|---------|
| && | AND (both) | a && b |
| \|\| | OR (any) | a \|\| b |
| ! | NOT (negate) | !a |

## Truth Tables

### AND (&&)

| A | B | A && B |
|---|---|--------|
| T | T | **T** |
| T | F | F |
| F | T | F |
| F | F | F |

### OR (||)

| A | B | A \|\| B |
|---|---|----------|
| T | T | **T** |
| T | F | **T** |
| F | T | **T** |
| F | F | F |

### NOT (!)

| A | !A |
|---|-----|
| T | F |
| F | **T** |

## Common Use Cases

### Permission Checking
```csharp
if (isAdmin || isModerator) {
    GrantAccess();
}
```

### Range Validation
```csharp
if (age >= 18 && age < 65) {
    Console.WriteLine("Working age");
}
```

### Null Safety
```csharp
if (list != null && list.Count > 0) {
    ProcessList(list);
}
```

### Complex Conditions
```csharp
if ((isAdmin || hasPermission) && !isDisabled && isApproved) {
    Process();
}
```

## Best Practices

✓ Order conditions efficiently
```csharp
// Good: null check first
if (list != null && list.Count > 0) { }

// Bad: can throw
if (list.Count > 0 && list != null) { }
```

✓ Use parentheses for clarity
```csharp
if ((a || b) && (c || d)) { }  // Clear
if (a || b && c || d) { }      // Ambiguous
```

✓ Extract complex logic to methods
```csharp
if (IsEligible(user, order)) {
    Process(order);
}
```

✓ Know operator precedence
```csharp
// AND before OR
if (a || b && c) { }  // Same as: a || (b && c)
```

## Common Mistakes to Avoid

❌ Confusing && with &
❌ Wrong order causing null reference
❌ Complex nested conditions
❌ Forgetting De Morgan's Laws
❌ Not using parentheses for clarity

## Learning Path

1. Start with **Comparison Operators** - test values
2. Learn **Logical AND** - combine with AND logic
3. Study **Logical OR/NOT** - complex conditions
4. Practice **Operator Precedence** - understand evaluation order

## Interview Preparation

Key points:
- Know short-circuit evaluation
- Order conditions for safety
- Understand precedence (AND before OR)
- De Morgan's Laws for simplifying
- Pattern matching alternatives

## Navigation

- **Parent**: [Operators](../README.md)
- **Comparison**: `01-Comparison/00-Comparison-Operators.md`
- **Logical AND**: `02-Logical-AND/00-Logical-AND.md`
- **Logical OR/NOT**: `03-Logical-OR-NOT/00-Logical-OR-NOT.md`
- **Arithmetic & Assignment**: `../01-Arithmetic-Assignment/README.md`
- **Best Practices**: `../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md`
