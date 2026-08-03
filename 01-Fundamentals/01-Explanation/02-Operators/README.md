# C# Operators: Complete Guide

## Overview

Comprehensive guide covering all C# operators: arithmetic, assignment, comparison, logical, bitwise, null-related, and ternary operators. Each operator is explained with practical examples, use cases, and best practices.

## Folder Structure

```
02-Operators/
├── 01-Arithmetic-Assignment/           [Basic math and variable assignment]
│   ├── 01-Arithmetic/                  [+, -, *, /, %]
│   ├── 02-Assignment/                  [=, +=, -=, *=, /=, %=, ??=]
│   ├── 03-Increment-Decrement/         [++, --]
│   └── README.md
│
├── 02-Comparison-Logical/              [Testing and combining conditions]
│   ├── 01-Comparison/                  [==, !=, <, >, <=, >=]
│   ├── 02-Logical-AND/                 [&&]
│   ├── 03-Logical-OR-NOT/              [||, !]
│   └── README.md
│
├── 03-Bitwise-Null/                    [Advanced operators]
│   ├── 01-Bitwise/                     [&, |, ^, ~, <<, >>]
│   ├── 02-Null-Related/                [??, ?., ?[], ??=]
│   ├── 03-Ternary-Precedence/          [? :, precedence]
│   └── README.md
│
├── 04-Best-Practices-Interview/        [Guidance and preparation]
│   ├── 01-Best-Practices/              [12 guidelines]
│   ├── 02-Common-Mistakes/             [15 mistakes + solutions]
│   ├── 03-Interview-Questions/         [21 questions across 3 levels]
│   └── README.md
│
└── README.md                           [This file]
```

---

## Quick Navigation

### 🔢 Arithmetic and Assignment (Basic Operations)

For mathematical calculations and variable updates.

#### Arithmetic Operators
- **File**: `01-Arithmetic-Assignment/01-Arithmetic/00-Arithmetic-Operators.md`
- **Covers**: +, -, *, /, %
- **Key**: Integer division truncates, modulo for remainder
- **When**: Math calculations, type conversions

#### Assignment Operators
- **File**: `01-Arithmetic-Assignment/02-Assignment/00-Assignment-Operators.md`
- **Covers**: =, +=, -=, *=, /=, %=, ??=
- **Key**: Compound operators for brevity, avoid string loops
- **When**: Variable modification, building values

#### Increment/Decrement
- **File**: `01-Arithmetic-Assignment/03-Increment-Decrement/00-Increment-Decrement.md`
- **Covers**: ++x, x++, --x, x--
- **Key**: Prefix vs postfix return values differ
- **When**: Loop counters, incrementing

---

### 🔴 Comparison and Logical (Testing Conditions)

For decision-making and combining conditions.

#### Comparison Operators
- **File**: `02-Comparison-Logical/01-Comparison/00-Comparison-Operators.md`
- **Covers**: ==, !=, <, >, <=, >=
- **Key**: Case-sensitive strings, null checking first
- **When**: Testing values, ranges

#### Logical AND
- **File**: `02-Comparison-Logical/02-Logical-AND/00-Logical-AND.md`
- **Covers**: &&
- **Key**: Short-circuit evaluation, both must be true
- **When**: Multiple required conditions

#### Logical OR and NOT
- **File**: `02-Comparison-Logical/03-Logical-OR-NOT/00-Logical-OR-NOT.md`
- **Covers**: ||, !
- **Key**: At least one true for OR, NOT flips value
- **When**: Alternatives, negation

---

### ⚙️ Bitwise, Null, and Ternary (Advanced)

For bit manipulation, null safety, and conditional expressions.

#### Bitwise Operators
- **File**: `03-Bitwise-Null/01-Bitwise/00-Bitwise-Operators.md`
- **Covers**: &, |, ^, ~, <<, >>
- **Key**: Operate on binary, fast for flags
- **When**: Permissions, performance-critical code

#### Null-Related Operators
- **File**: `03-Bitwise-Null/02-Null-Related/00-Null-Related.md`
- **Covers**: ??, ?., ?[], ??=
- **Key**: Prevent NullReferenceException, provide defaults
- **When**: Safe navigation, default values

#### Ternary and Precedence
- **File**: `03-Bitwise-Null/03-Ternary-Precedence/00-Ternary-Precedence.md`
- **Covers**: ? :, operator order
- **Key**: Shorthand if-else, know precedence
- **When**: Simple conditions, complex expressions

---

### 📚 Best Practices and Interview Prep

Guidelines, common mistakes, and interview questions.

#### Best Practices
- **File**: `04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md`
- **Count**: 12 key guidelines
- **Topics**: Safe coding, performance, readability

#### Common Mistakes
- **File**: `04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md`
- **Count**: 15 mistakes with solutions
- **Topics**: Errors to avoid, how to fix them

#### Interview Questions
- **Files**: `04-Best-Practices-Interview/03-Interview-Questions/`
- **Count**: 21 questions (8 Easy, 7 Medium, 6 Hard)
- **Topics**: Real interview scenarios

---

## Complete Operator Reference

### Arithmetic Operators

| Operator | Name | Use |
|----------|------|-----|
| + | Addition | Add numbers, concatenate strings |
| - | Subtraction | Subtract numbers |
| * | Multiplication | Multiply numbers |
| / | Division | Divide numbers (truncates if integer) |
| % | Modulo | Get remainder after division |

### Assignment Operators

| Operator | Use |
|----------|-----|
| = | Assign value |
| += | Add and assign |
| -= | Subtract and assign |
| *= | Multiply and assign |
| /= | Divide and assign |
| %= | Modulo and assign |
| ??= | Assign if null |

### Comparison Operators

| Operator | Meaning |
|----------|---------|
| == | Equal |
| != | Not equal |
| < | Less than |
| > | Greater than |
| <= | Less or equal |
| >= | Greater or equal |

### Logical Operators

| Operator | Meaning |
|----------|---------|
| && | AND (both true) |
| \|\| | OR (any true) |
| ! | NOT (negate) |

### Bitwise Operators

| Operator | Name |
|----------|------|
| & | AND |
| \| | OR |
| ^ | XOR |
| ~ | NOT |
| << | Left shift |
| >> | Right shift |

### Null-Related Operators

| Operator | Purpose |
|----------|---------|
| ?? | Default if null |
| ?. | Safe member access |
| ?[] | Safe indexer access |
| ??= | Assign if null |

---

## Learning Paths

### 🚀 Path 1: Beginner (1 week)
Focus on fundamentals, everyday operators.

**Week 1**:
- Arithmetic operators
- Assignment operators
- Comparison operators
- Logical AND/OR/NOT
- Basic best practices

### 🎯 Path 2: Intermediate (3 days)
Deeper understanding, edge cases, patterns.

**Day 1**: Review all basic operators
**Day 2**: Bitwise, null-related, ternary
**Day 3**: Best practices, common mistakes

### 🔬 Path 3: Advanced/Interview (2 days)
Master all concepts, interview preparation.

**Day 1**: Study all common mistakes, medium questions
**Day 2**: Practice hard questions, mock interviews

---

## Key Concepts to Master

### Short-Circuit Evaluation
```csharp
// && stops if first is false
if (list != null && list.Count > 0) { }

// || stops if first is true
if (isVIP || CheckPermissions()) { }
```

### Operator Precedence
Multiplication/division before addition/subtraction, AND before OR

```csharp
int x = 5 + 3 * 2;  // 11 (multiply first)
bool y = true || false && false;  // true (AND first)
```

### Null Safety
Use null-coalescing and null-conditional
```csharp
string city = person?.Address?.City ?? "Unknown";
```

### Type Conversion
Be aware of promotion and truncation
```csharp
double avg = (double)sum / count;  // Cast for precision
int div = 10 / 3;                  // 3 (truncates)
```

---

## Common Mistakes Summary

**Top 5 Mistakes**:
1. Confusing = with ==
2. Integer division truncation
3. String concatenation in loops
4. Missing null checks
5. Wrong operator precedence

**How to Avoid**:
- Use linters and analyzers
- Enable nullable reference types
- Write tests
- Code review
- Know the 15 mistakes

---

## Interview Quick Reference

**Must Know**:
- = vs ==
- Short-circuit evaluation
- Operator precedence
- Null-coalescing vs null-conditional
- Common mistakes list

**Nice to Know**:
- Bitwise optimization
- Expression trees
- Operator overloading
- Advanced patterns

---

## Real-World Applications

### Web Application
```csharp
// Safe navigation with defaults
string userEmail = currentUser?.Email ?? "guest@example.com";

// Permission checking
if ((userPermissions & Permissions.Admin) != 0) {
    ShowAdminPanel();
}
```

### Data Processing
```csharp
// Efficient string building
var sb = new StringBuilder();
foreach (var item in items) {
    sb.Append(item).Append(",");
}

// Safe collection checking
if (items != null && items.Count > 0) {
    ProcessItems(items);
}
```

### Game Development
```csharp
// Bit flags for game state
if ((gameState & GameState.Running) != 0 && 
    (player & CollisionMask.Solid) != 0) {
    HandleCollision();
}
```

---

## Performance Tips

✓ Use bitwise for flags (fast)
✓ Use << and >> for powers of 2 (very fast)
✓ Use StringBuilder for string loops (essential)
✓ Order logical conditions efficiently (quick fail first)
✓ Avoid boxing/unboxing

---

## Next Steps

1. **Start Here**: Choose a learning path above
2. **Study**: Read each section carefully
3. **Practice**: Try code examples
4. **Verify**: Check against best practices
5. **Interview**: Practice interview questions
6. **Apply**: Use in real projects

---

## File Statistics

**Total Files**: 19
**Total Content**: ~20,000 words
**Code Examples**: 200+
**Interview Questions**: 21
**Best Practices**: 12
**Common Mistakes**: 15

---

**Last Updated**: August 2026
**Status**: Complete - All operator categories covered with comprehensive examples and interview preparation

---

## Navigation

- **01-Arithmetic-Assignment**: `01-Arithmetic-Assignment/README.md`
- **02-Comparison-Logical**: `02-Comparison-Logical/README.md`
- **03-Bitwise-Null**: `03-Bitwise-Null/README.md`
- **04-Best-Practices-Interview**: `04-Best-Practices-Interview/README.md`
- **Parent**: [01-Fundamentals Explanation](../README.md)
