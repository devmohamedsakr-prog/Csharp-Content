# Numeric Data Types in C#

## Overview

Numeric types store numerical values. C# provides two categories: **Integer Types** and **Floating-Point Types**.

## Integer Types

### Purpose
Store whole numbers (no decimal part).

### Available Types

#### Byte
```csharp
byte value = 255;
// Range: 0 to 255
// Size: 1 byte
// Use: Small positive integers, flags, raw data
```

#### Short
```csharp
short value = 32767;
// Range: -32,768 to 32,767
// Size: 2 bytes
// Use: Moderate range integers, rarely used
```

#### Int (Default Integer)
```csharp
int value = 2147483647;
// Range: -2,147,483,648 to 2,147,483,647
// Size: 4 bytes
// Use: DEFAULT choice for integers, most common
```

#### Long
```csharp
long value = 9223372036854775807;
// Range: -9,223,223,372,036,854,775,808 to 9,223,372,036,854,775,807
// Size: 8 bytes
// Use: Very large numbers, timestamps, IDs
```

#### Unsigned Variants
```csharp
// Unsigned (positive only, double range)
uint uintValue = 4294967295;      // 0 to 4,294,967,295
ushort ushortValue = 65535;       // 0 to 65,535
ulong ulongValue = 18446744073709551615;  // 0 to huge number
```

### Integer Type Comparison

| Type | Size | Min Value | Max Value | Use Case |
|------|------|-----------|-----------|----------|
| byte | 1 | 0 | 255 | Flags, small data |
| sbyte | 1 | -128 | 127 | Signed small data |
| short | 2 | -32,768 | 32,767 | Small range |
| ushort | 2 | 0 | 65,535 | Unsigned range |
| **int** | 4 | -2.1B | 2.1B | **DEFAULT** |
| uint | 4 | 0 | 4.3B | Positive only |
| long | 8 | -9.2E18 | 9.2E18 | Very large |
| ulong | 8 | 0 | 1.8E19 | Unsigned large |

### Integer Usage Examples

```csharp
// Basic usage
int age = 25;
long population = 8000000000;
byte percentage = 100;

// Literals with suffixes
int x = 100;           // No suffix needed for int
long y = 100L;         // L suffix for long
uint z = 100U;         // U suffix for uint

// Overflow behavior
int max = int.MaxValue;
int overflow = max + 1;  // Wraps around to negative

// Checked arithmetic (throws exception)
checked {
    int result = int.MaxValue + 1;  // OverflowException
}

// Unchecked arithmetic (wraps silently)
unchecked {
    int result = int.MaxValue + 1;  // Wraps, no error
}
```

## Floating-Point Types

### Purpose
Store numbers with decimal precision.

### Available Types

#### Float
```csharp
float value = 3.14f;
// Precision: 6-7 significant digits
// Size: 4 bytes
// Use: Graphics, general floating-point, graphics calculations
// Suffix: f or F
```

#### Double (Default Floating-Point)
```csharp
double value = 3.14159265359;
// Precision: 15-16 significant digits
// Size: 8 bytes
// Use: DEFAULT for floating-point, scientific calculations
// No suffix needed (default)
```

#### Decimal
```csharp
decimal value = 99.99m;
// Precision: 28-29 significant digits
// Size: 16 bytes
// Use: MONEY/FINANCIAL calculations, high precision
// Suffix: m or M
```

### Floating-Point Comparison

| Type | Size | Precision | Use Case |
|------|------|-----------|----------|
| float | 4 | 6-7 digits | Graphics, performance-critical |
| **double** | 8 | 15-16 digits | **DEFAULT**, scientific |
| decimal | 16 | 28-29 digits | **FINANCIAL**, money |

### Floating-Point Usage Examples

```csharp
// Float
float price = 19.99f;
float temperature = -5.5f;
float pi_approx = 3.14f;

// Double (default)
double pi = 3.14159265359;
double radius = 5.5;
double result = pi * radius * radius;

// Decimal (for money)
decimal accountBalance = 1000.50m;
decimal tax = 100.25m;
decimal total = accountBalance + tax;

// Special values
double infinity = double.PositiveInfinity;
double negInfinity = double.NegativeInfinity;
double notANumber = double.NaN;

// Checking for special values
if (double.IsNaN(value)) { }
if (double.IsInfinity(value)) { }

// Precision comparison
float f = 0.1f + 0.2f;  // May not equal 0.3
double d = 0.1 + 0.2;   // May not equal 0.3 exactly
decimal dec = 0.1m + 0.2m;  // Exactly 0.3
```

## Choosing the Right Integer Type

### Decision Tree

```
Need Integer?
├─ Yes
│  ├─ Range -2.1B to 2.1B? → int (DEFAULT)
│  ├─ Larger? → long
│  ├─ Smaller? → short or byte
│  ├─ Only positive? → uint, ushort, byte
│  └─ Very specific? → Consider enum or custom type
└─ No → Use floating-point or decimal
```

### Best Practices

✓ **Use `int` by default** - it's optimized
```csharp
int count = 100;  // Good
long count = 100;  // Unnecessary
```

✓ **Use `long` only when needed**
```csharp
long timestamp = DateTime.Now.Ticks;  // Necessary
long id = userId;  // OK if IDs are large
```

✓ **Use `byte` for flags or raw data**
```csharp
byte[] imageData = new byte[1024];
byte flags = 0xFF;
```

## Choosing the Right Floating-Point Type

### Decision Tree

```
Need Decimal?
├─ Money/Financial? → decimal (MUST)
├─ Scientific/High precision? → double (DEFAULT)
└─ Performance/Graphics? → float
```

### Best Practices

✓ **Use `decimal` for money**
```csharp
decimal price = 99.99m;  // Correct
double price = 99.99;    // Wrong - precision issues
```

✓ **Use `double` for general calculations**
```csharp
double average = (score1 + score2) / 2.0;
```

✓ **Use `float` for performance-critical graphics**
```csharp
float[] vertices = new float[1000];  // Graphics
```

## Common Numeric Operations

```csharp
// Arithmetic
int sum = 10 + 5;
int diff = 10 - 5;
int product = 10 * 5;
int quotient = 10 / 3;      // Integer division = 3
int remainder = 10 % 3;     // Modulo = 1
int power = (int)Math.Pow(2, 3);  // 8

// Comparisons
bool isEqual = (10 == 10);
bool isGreater = (10 > 5);
bool isLess = (10 < 5);

// Min/Max
int minValue = Math.Min(10, 5);  // 5
int maxValue = Math.Max(10, 5);  // 10

// Absolute value
int absolute = Math.Abs(-10);    // 10

// Rounding
double rounded = Math.Round(3.7);  // 4
double ceiling = Math.Ceiling(3.2);  // 4
double floor = Math.Floor(3.9);  // 3
```

## Performance Considerations

```csharp
// Stack allocation - fast
int x = 10;  // Stack

// Boxing - slow (value type to reference)
object boxed = 10;  // Copies to heap
int unboxed = (int)boxed;  // Copies back from heap

// Avoid boxing in loops
for (int i = 0; i < 1000000; i++) {
    ArrayList list = new ArrayList();
    list.Add(i);  // Boxing occurs! Slow
}

// Use generic collections instead
for (int i = 0; i < 1000000; i++) {
    List<int> list = new List<int>();
    list.Add(i);  // No boxing - fast
}
```

## Common Mistakes

❌ **Using float for money**
```csharp
float total = 0.1f + 0.2f;  // May not equal 0.3!
```

✓ **Use decimal for money**
```csharp
decimal total = 0.1m + 0.2m;  // Exactly 0.3
```

❌ **Integer division when expecting decimal**
```csharp
int x = 10;
int y = 3;
int result = x / y;  // Result is 3, not 3.33...
```

✓ **Cast to decimal for decimal division**
```csharp
decimal result = (decimal)10 / 3;  // Result is 3.333...
```

❌ **Boxing value types**
```csharp
object boxed = 10;  // Boxing - performance hit
```

✓ **Use generics**
```csharp
List<int> list = new List<int>();  // No boxing
```

## Summary

| Need | Type | Example |
|------|------|---------|
| Small whole number | byte/short | byte age = 25; |
| General whole number | int | int count = 100; |
| Large whole number | long | long population = 8000000000; |
| Decimal number | double | double pi = 3.14159; |
| Money/Financial | decimal | decimal price = 99.99m; |
| High precision | decimal | decimal precise = 0.123456789m; |

---

**Key Takeaway**: Use `int` for integers and `decimal` for money. Use `double` for general calculations and `float` only for performance-critical graphics code.
