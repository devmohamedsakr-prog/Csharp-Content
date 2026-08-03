# Arithmetic Operators

## Overview

Arithmetic operators perform mathematical calculations on numeric values. These are fundamental operations used in virtually every program.

## Basic Arithmetic Operators

### Addition (+)
Adds two numbers together.

```csharp
int a = 10;
int b = 3;
int sum = a + b;  // 13

// Works with different numeric types
double x = 5.5;
double y = 2.3;
double result = x + y;  // 7.8

// String concatenation (also uses +)
string greeting = "Hello" + " " + "World";  // "Hello World"
```

**Use Cases**:
- Adding quantities
- Summing values
- Combining strings

---

### Subtraction (-)
Subtracts second number from first.

```csharp
int a = 10;
int b = 3;
int diff = a - b;  // 7

// Negative result
int result = 3 - 10;  // -7

// With floating point
double x = 5.5;
double y = 2.3;
double difference = x - y;  // 3.2
```

**Use Cases**:
- Calculating differences
- Decreasing values
- Finding elapsed time

---

### Multiplication (*)
Multiplies two numbers.

```csharp
int a = 10;
int b = 3;
int product = a * b;  // 30

// Negative result
int result = -5 * 3;  // -15

// Floating point
double x = 2.5;
double y = 4.0;
double result = x * y;  // 10.0
```

**Use Cases**:
- Scaling values
- Calculating area/volume
- Unit conversions

---

### Division (/)
Divides first number by second.

```csharp
// Integer division (truncates)
int a = 10;
int b = 3;
int quotient = a / b;  // 3 (not 3.333...)

// Floating point division (preserves decimals)
double x = 10.0;
double y = 3.0;
double result = x / y;  // 3.333...

// Casting for precision
double precise = (double)a / b;  // 3.333...
int integer = a / b;  // 3
```

**Integer Division Important Notes**:
```csharp
// Division truncates toward zero
int result1 = 10 / 3;   // 3
int result2 = -10 / 3;  // -3 (not -4)

// Division by zero throws exception
int x = 10;
int y = 0;
int bad = x / y;  // DivideByZeroException
```

**Use Cases**:
- Calculating averages
- Converting units
- Splitting into groups

---

### Modulo (%)
Returns remainder after division.

```csharp
int a = 10;
int b = 3;
int remainder = a % b;  // 1

// Check if even/odd
int num = 5;
if (num % 2 == 0) {
    Console.WriteLine("Even");
} else {
    Console.WriteLine("Odd");  // This prints
}

// Cycling through values (0 to 9)
for (int i = 0; i < 100; i++) {
    int index = i % 10;  // Always 0-9
}
```

**With Negative Numbers**:
```csharp
int result1 = 10 % 3;   // 1
int result2 = -10 % 3;  // -1 (sign follows dividend)
int result3 = 10 % -3;  // 1 (sign follows dividend)
```

**Use Cases**:
- Checking divisibility
- Even/odd determination
- Cycling through ranges
- Time calculations (hours, minutes)

---

## Operator Precedence

Arithmetic operators follow mathematical precedence:

```csharp
// Multiplication and division before addition and subtraction
int result = 5 + 3 * 2;  // 11 (not 16)
// Evaluated as: 5 + (3 * 2)

// Use parentheses for clarity
int clear = (5 + 3) * 2;  // 16
```

**Order (highest to lowest)**:
1. Parentheses `()`
2. Multiplication, Division, Modulo: `*`, `/`, `%`
3. Addition, Subtraction: `+`, `-`

---

## Type Promotion

When operators combine different types, smaller types promote to larger:

```csharp
int x = 10;
double y = 3.5;
double result = x + y;  // 13.5 (int promoted to double)

byte a = 10;
byte b = 20;
int sum = a + b;  // 30 (bytes promoted to int)
```

**Promotion Order**:
`byte` → `short` → `int` → `long` → `float` → `double`

---

## Common Numeric Types and Ranges

| Type | Bits | Range | Use |
|------|------|-------|-----|
| byte | 8 | 0 to 255 | Small counts |
| sbyte | 8 | -128 to 127 | Small numbers |
| short | 16 | -32,768 to 32,767 | Medium numbers |
| ushort | 16 | 0 to 65,535 | Medium positive |
| int | 32 | -2.1B to 2.1B | Default integer |
| uint | 32 | 0 to 4.3B | Large positive |
| long | 64 | -9.2E18 to 9.2E18 | Very large |
| ulong | 64 | 0 to 18.4E18 | Very large positive |
| float | 32 | ±1.5E-45 to ±3.4E38 | Fast, approximate |
| double | 64 | ±5E-324 to ±1.7E308 | Precise, default |
| decimal | 128 | ±7.9E-28 to ±7.9E28 | Money, precise |

---

## Overflow and Underflow

### Unchecked (Default)
```csharp
int max = int.MaxValue;  // 2,147,483,647
int overflow = max + 1;  // Wraps to -2,147,483,648 (no exception)
```

### Checked
```csharp
try {
    int max = int.MaxValue;
    int result = checked(max + 1);  // Throws OverflowException
} catch (OverflowException) {
    Console.WriteLine("Overflow detected");
}
```

---

## Practical Examples

### Calculating Average
```csharp
int[] scores = { 85, 90, 78, 92 };
int sum = 0;
foreach (int score in scores) {
    sum = sum + score;  // OR: sum += score
}
double average = (double)sum / scores.Length;  // 86.25
```

### Time Calculations
```csharp
int totalSeconds = 3665;
int hours = totalSeconds / 3600;      // 1
int minutes = (totalSeconds % 3600) / 60;  // 1
int seconds = totalSeconds % 60;      // 5
Console.WriteLine($"{hours}:{minutes:D2}:{seconds:D2}");  // 1:01:05
```

### Unit Conversion
```csharp
double kilometers = 5.5;
double miles = kilometers * 0.621371;  // 3.42
double feet = miles * 5280;  // 18,062.88
```

### Divisibility Check
```csharp
int number = 15;
if (number % 3 == 0) {
    Console.WriteLine("Divisible by 3");
}
if (number % 5 == 0) {
    Console.WriteLine("Divisible by 5");  // Also prints
}
```

---

## Best Practices

✓ **Be clear about intent**
```csharp
// Good: explicit types
double average = (double)sum / count;

// Risky: implicit promotion
var result = x / y;  // Type depends on x and y
```

✓ **Use appropriate types**
```csharp
// For money
decimal price = 19.99m;

// For general math
double result = 3.14;

// For integers
int count = 100;
```

✓ **Handle division by zero**
```csharp
if (divisor != 0) {
    int result = dividend / divisor;
} else {
    Console.WriteLine("Cannot divide by zero");
}
```

✓ **Consider overflow in calculations**
```csharp
// Risky
int result = int.MaxValue + 1;

// Better: use long or checked
long result = (long)int.MaxValue + 1;
```

---

## Common Mistakes

❌ **Integer division when decimal needed**
```csharp
int total = 10;
int count = 3;
int average = total / count;  // 3 (not 3.333...)
```

✓ **Cast to double**
```csharp
double average = (double)total / count;  // 3.333...
```

---

❌ **Division by zero**
```csharp
int result = 10 / 0;  // DivideByZeroException
```

✓ **Validate before dividing**
```csharp
if (divisor != 0) {
    int result = dividend / divisor;
}
```

---

❌ **Wrong operator precedence**
```csharp
int result = 5 + 3 * 2;  // 11, not 16
```

✓ **Use parentheses**
```csharp
int result = (5 + 3) * 2;  // 16
```

---

## Performance Considerations

**Speed**: `+`, `-`, `*` are very fast
**Slightly slower**: `/` (depends on CPU)
**Avoid in loops**: Division repeated many times

```csharp
// Fast
for (int i = 0; i < 1000000; i++) {
    int sum = a + b;
}

// Slower - use before loop
double ratio = (double)total / count;
for (int i = 0; i < 1000000; i++) {
    double scaled = value * ratio;
}
```

---

## Quick Reference

| Operator | Name | Example | Result |
|----------|------|---------|--------|
| + | Addition | 10 + 3 | 13 |
| - | Subtraction | 10 - 3 | 7 |
| * | Multiplication | 10 * 3 | 30 |
| / | Division | 10 / 3 | 3 (int) or 3.33 (double) |
| % | Modulo | 10 % 3 | 1 |

---

## Next Steps

- Study [Assignment Operators](../02-Assignment/00-Assignment-Operators.md)
- Learn [Increment/Decrement](../03-Increment-Decrement/00-Increment-Decrement.md)
- Compare with [Comparison Operators](../../02-Comparison-Logical/01-Comparison/00-Comparison-Operators.md)
