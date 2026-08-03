# Arithmetic and Assignment Operators

## Overview

This section covers basic arithmetic operations and variable assignment, including compound assignment operators for convenience.

## Categories

### Arithmetic Operators
Basic mathematical operations on numeric values.

**Files**: `01-Arithmetic/00-Arithmetic-Operators.md`

**Topics Covered**:
- Addition (+)
- Subtraction (-)
- Multiplication (*)
- Division (/)
- Modulo (%)
- Operator precedence
- Type promotion
- Overflow handling

**Key Concepts**:
- Integer division truncates
- Modulo for remainder/cycling
- Type promotion rules
- Overflow/underflow behavior

### Assignment Operators
Assign and modify variable values efficiently.

**Files**: `02-Assignment/00-Assignment-Operators.md`

**Topics Covered**:
- Basic assignment (=)
- Compound operators (+=, -=, *=, /=, %=)
- Null-coalescing assignment (??=)
- Multiple assignment
- Assignment in expressions

**Key Concepts**:
- Compound operators for brevity
- Null-coalescing assignment
- String concatenation with +=
- StringBuilder for performance

### Increment/Decrement Operators
Increase or decrease values by 1 with prefix/postfix forms.

**Files**: `03-Increment-Decrement/00-Increment-Decrement.md`

**Topics Covered**:
- Pre-increment (++x)
- Post-increment (x++)
- Pre-decrement (--x)
- Post-decrement (x--)
- Return value differences
- Performance implications
- Usage patterns

**Key Concepts**:
- Prefix returns new value
- Postfix returns old value
- Short-circuit evaluation
- Loop counter usage

## Quick Reference

### Arithmetic

| Operator | Name | Example | Result |
|----------|------|---------|--------|
| + | Addition | 10 + 3 | 13 |
| - | Subtraction | 10 - 3 | 7 |
| * | Multiplication | 10 * 3 | 30 |
| / | Division | 10 / 3 | 3 (integer) |
| % | Modulo | 10 % 3 | 1 |

### Assignment

| Operator | Example | Equivalent |
|----------|---------|------------|
| = | x = 5 | Assign 5 |
| += | x += 5 | x = x + 5 |
| -= | x -= 5 | x = x - 5 |
| *= | x *= 5 | x = x * 5 |
| /= | x /= 5 | x = x / 5 |
| %= | x %= 5 | x = x % 5 |
| ??= | x ??= 5 | x = x ?? 5 |

### Increment/Decrement

| Operator | Name | Effect |
|----------|------|--------|
| ++x | Pre-increment | Increment, return new |
| x++ | Post-increment | Return old, increment |
| --x | Pre-decrement | Decrement, return new |
| x-- | Post-decrement | Return old, decrement |

## Common Use Cases

### Accumulating Values
```csharp
int total = 0;
total += 10;  // 10
total += 5;   // 15
```

### Building Strings
```csharp
string result = "";
result += "Hello";
result += " World";  // Use StringBuilder for loops!
```

### Loop Counters
```csharp
for (int i = 0; i < 10; i++) {
    Console.WriteLine(i);
}
```

### Dividing and Remainders
```csharp
int hours = minutes / 60;
int remainder = minutes % 60;
```

## Best Practices

✓ Use compound operators for clarity
```csharp
total += amount;  // Better than: total = total + amount;
```

✓ Use StringBuilder for string loops
```csharp
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
```

✓ Cast for precision in division
```csharp
double average = (double)sum / count;
```

✓ Handle division by zero
```csharp
if (divisor != 0) {
    int result = dividend / divisor;
}
```

## Common Mistakes to Avoid

❌ String concatenation in loops (use StringBuilder)
❌ Integer division without casting (precision loss)
❌ Forgetting division by zero check
❌ Using postfix when prefix would be clearer

## Learning Path

1. Start with **Arithmetic Operators** - understand basic math
2. Learn **Assignment Operators** - how to modify variables
3. Practice **Increment/Decrement** - loop and counter patterns
4. Study **Operator Precedence** - how operations combine

## Interview Preparation

Key points to remember:
- Integer division truncates, not rounds
- Modulo for remainder operations
- String concatenation with += is slow in loops
- Compound operators are preferred
- Understanding pre vs post increment

## Navigation

- **Parent**: [Operators](../README.md)
- **Arithmetic**: `01-Arithmetic/00-Arithmetic-Operators.md`
- **Assignment**: `02-Assignment/00-Assignment-Operators.md`
- **Increment/Decrement**: `03-Increment-Decrement/00-Increment-Decrement.md`
- **Comparison & Logical**: `../02-Comparison-Logical/README.md`
- **Best Practices**: `../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md`
