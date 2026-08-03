# Assignment Operators

## Overview

Assignment operators assign values to variables. The basic assignment operator is `=`, with compound variations for convenience.

## Basic Assignment (=)

Assigns a value to a variable.

```csharp
int x = 10;              // Assign 10 to x
string name = "Alice";   // Assign string to name
double price = 19.99;    // Assign decimal to price

// Multiple assignment (right-to-left)
int a, b, c;
a = b = c = 5;  // All become 5
```

---

## Compound Assignment Operators

Combine an operation with assignment, shorthand for common patterns.

### Addition Assignment (+=)

```csharp
int x = 10;
x += 5;  // Equivalent to: x = x + 5;
// x is now 15

// Works with strings
string message = "Hello";
message += " World";  // "Hello World"

// Works with numbers
double total = 100.50;
total += 25.25;  // 125.75
```

**Use Cases**:
- Accumulating sums
- Building strings
- Growing counters

---

### Subtraction Assignment (-=)

```csharp
int x = 20;
x -= 5;  // Equivalent to: x = x - 5;
// x is now 15

// With floating point
double balance = 1000.0;
balance -= 50.0;  // 950.0

// Countdown
int countdown = 10;
countdown -= 1;  // 9
```

**Use Cases**:
- Decreasing counters
- Subtracting balances
- Reducing quantities

---

### Multiplication Assignment (*=)

```csharp
int x = 5;
x *= 3;  // Equivalent to: x = x * 3;
// x is now 15

// Scaling
double scale = 1.5;
scale *= 2;  // 3.0

// Doubling
int value = 10;
value *= 2;  // 20
```

**Use Cases**:
- Scaling values
- Multiplying accumulators
- Percentage calculations

---

### Division Assignment (/=)

```csharp
int x = 20;
x /= 4;  // Equivalent to: x = x / 4;
// x is now 5

// Splitting
double total = 100.0;
total /= 4;  // 25.0

// Halving
int value = 20;
value /= 2;  // 10
```

**Use Cases**:
- Splitting values
- Scaling down
- Averaging groups

---

### Modulo Assignment (%=)

```csharp
int x = 10;
x %= 3;  // Equivalent to: x = x % 3;
// x is now 1

// Cycling
int index = 15;
index %= 10;  // Wraps to 5
```

**Use Cases**:
- Wrapping indices
- Remainder calculations

---

## Practical Examples

### Accumulating Values
```csharp
int total = 0;
int[] numbers = { 5, 10, 15, 20 };

foreach (int num in numbers) {
    total += num;  // Add each number to total
}
// total = 50
```

### Building Strings
```csharp
string result = "";
foreach (string word in words) {
    result += word + " ";
}
// result = "word1 word2 word3 "
```

### Scaling Prices
```csharp
decimal price = 100m;
double taxRate = 0.08;

decimal tax = price * (decimal)taxRate;
price += tax;  // Apply tax
// price = 108m
```

### Processing Balances
```csharp
double balance = 1000.0;

balance += 500;      // Deposit
balance -= 200;      // Withdrawal
balance *= 1.02;     // Apply 2% interest
balance /= 2;        // Split between accounts
```

---

## Assignment in Expressions

Assignment operators return the assigned value, allowing chaining:

```csharp
int a, b, c;

// Assigns and returns value
a = (b = c = 5);  // All become 5

// In expressions
int x = 10;
int y = (x += 5);  // x = 15, y = 15

// Assignment in conditions
int value;
if ((value = GetValue()) > 0) {
    Console.WriteLine($"Positive: {value}");
}
```

---

## Type Coercion with Assignment

Assignment can change types through implicit conversion:

```csharp
// Implicit conversion (safe)
int x = 10;
long y;
y = x;  // int to long (always safe)

// Explicit conversion (may lose data)
double d = 10.5;
int i;
i = (int)d;  // 10 (decimal part lost)

// With compound operators
int total = 0;
double average = 5.5;
total += (int)average;  // Cast needed
```

---

## Null-Coalescing Assignment (??=)

Assigns only if current value is null.

```csharp
string name = null;
name ??= "Unknown";  // Assigns because null
// name = "Unknown"

name ??= "Default";  // Doesn't assign because not null
// name = "Unknown"

// With nullable types
int? count = null;
count ??= 0;  // Assigns 0
// count = 0

count ??= 10;  // Doesn't assign because not null
// count = 0
```

**Use Cases**:
- Default initialization
- Lazy defaults
- Optional value setup

---

## Compound Assignment Performance

Compound assignments are efficient and preferred:

```csharp
// Less efficient (read, calculate, write)
x = x + 5;

// More efficient (direct operation)
x += 5;

// Compiler optimizes both similarly, but compound is clearer
```

---

## Best Practices

✓ **Use compound assignment for clarity**
```csharp
// Good
total += amount;
value *= 2;
counter -= 1;

// Less clear
total = total + amount;
value = value * 2;
counter = counter - 1;
```

✓ **Be aware of null-coalescing assignment**
```csharp
// Safe initialization
result ??= CalculateDefault();
```

✓ **Consider StringBuilder for repeated string concatenation**
```csharp
// Risky in loops
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;  // Creates many strings
}

// Better
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
string result = sb.ToString();
```

---

## Common Mistakes

❌ **Using = in conditions**
```csharp
if (x = 5) { }  // Assigns instead of compares
```

✓ **Use == for comparison**
```csharp
if (x == 5) { }  // Correct comparison
```

---

❌ **Forgetting compound operators**
```csharp
total = total + amount;  // Works but verbose
```

✓ **Use compound**
```csharp
total += amount;  // Clearer intent
```

---

❌ **String concatenation in loops**
```csharp
for (int i = 0; i < 1000; i++) {
    result += i;  // O(n²) complexity!
}
```

✓ **Use StringBuilder**
```csharp
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);  // O(n) complexity
}
```

---

## Quick Reference

| Operator | Example | Equivalent |
|----------|---------|------------|
| = | x = 5 | Assign 5 |
| += | x += 5 | x = x + 5 |
| -= | x -= 5 | x = x - 5 |
| *= | x *= 5 | x = x * 5 |
| /= | x /= 5 | x = x / 5 |
| %= | x %= 5 | x = x % 5 |
| ??= | x ??= 5 | x = x ?? 5 |

---

## Next Steps

- Study [Increment/Decrement Operators](../03-Increment-Decrement/00-Increment-Decrement.md)
- Review [Comparison Operators](../../02-Comparison-Logical/01-Comparison/00-Comparison-Operators.md)
- Compare with [Arithmetic Operators](../01-Arithmetic/00-Arithmetic-Operators.md)
