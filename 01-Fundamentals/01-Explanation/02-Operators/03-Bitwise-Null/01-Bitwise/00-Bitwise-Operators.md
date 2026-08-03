# Bitwise Operators

## Overview

Bitwise operators work directly on binary representations of integers. They're useful for performance-critical code, flags, and bit manipulation.

## Binary Review

```csharp
// Decimal to binary
5 = 0101
3 = 0011
```

---

## Bitwise AND (&)

Performs AND on each bit position.

```csharp
int a = 5;    // 0101
int b = 3;    // 0011
int result = a & b;  // 0001 = 1
```

**Truth Table (per bit)**:
| Bit1 | Bit2 | Result |
|------|------|--------|
| 1 | 1 | 1 |
| 1 | 0 | 0 |
| 0 | 1 | 0 |
| 0 | 0 | 0 |

**Use Cases**:
```csharp
// Check if bit is set
int flags = 0b1010;  // Binary literal
if ((flags & 0b0010) != 0) {
    Console.WriteLine("Bit 1 is set");
}

// Mask lower bits
int value = 0b11110101;
int lowerNibble = value & 0b00001111;  // 0b0101 = 5

// Check if multiple bits set
if ((permissions & 0b11000000) == 0b11000000) {
    Console.WriteLine("Has both bits");
}
```

---

## Bitwise OR (|)

Performs OR on each bit position.

```csharp
int a = 5;    // 0101
int b = 3;    // 0011
int result = a | b;  // 0111 = 7
```

**Truth Table (per bit)**:
| Bit1 | Bit2 | Result |
|------|------|--------|
| 1 | 1 | 1 |
| 1 | 0 | 1 |
| 0 | 1 | 1 |
| 0 | 0 | 0 |

**Use Cases**:
```csharp
// Set specific bits
int flags = 0b0000;
flags |= 0b0001;  // Set bit 0
flags |= 0b1000;  // Set bit 3
// flags = 0b1001

// Combine flags
FileAttributes combined = FileAttributes.ReadOnly | FileAttributes.Hidden;

// Enable specific permission
permissions |= PermissionFlags.Write;
```

---

## Bitwise XOR (^)

Exclusive OR: true only if bits differ.

```csharp
int a = 5;    // 0101
int b = 3;    // 0011
int result = a ^ b;  // 0110 = 6
```

**Truth Table (per bit)**:
| Bit1 | Bit2 | Result |
|------|------|--------|
| 1 | 1 | 0 |
| 1 | 0 | 1 |
| 0 | 1 | 1 |
| 0 | 0 | 0 |

**Use Cases**:
```csharp
// Toggle specific bits
int flags = 0b1010;
flags ^= 0b0100;  // Toggle bit 2
// flags = 0b1110

// Swap without temp variable (old trick)
int x = 5, y = 10;
x ^= y;
y ^= x;
x ^= y;
// Now: x = 10, y = 5 (not recommended today)

// Check if values different
int a = 5;
int b = 5;
if ((a ^ b) != 0) {
    Console.WriteLine("Different");
}
```

---

## Bitwise NOT (~)

Inverts all bits (one's complement).

```csharp
int a = 5;    // 0101 (simplified to 4 bits)
int result = ~a;  // 1010 (in full: inverts all 32 bits)
```

**Use Cases**:
```csharp
// Clear specific bits
int flags = 0b1111;
flags &= ~0b0100;  // Clear bit 2
// flags = 0b1011

// Invert all bits
int value = 0;
int inverted = ~value;  // All bits set

// Logical NOT in bitwise context
// Note: use ! for boolean, ~ for bitwise
bool condition = true;
bool result = !condition;  // false (logical)

int bits = 0xFF;
int inverted = ~bits;  // Bitwise inversion
```

---

## Left Shift (<<)

Shifts bits left, fills right with 0.

```csharp
int a = 5;          // 0101
int result = a << 1;  // 1010 = 10 (multiplies by 2)
int result2 = a << 2; // 10100 = 20 (multiplies by 4)
```

**Formula**: `a << n` = `a * 2^n`

**Use Cases**:
```csharp
// Multiply by power of 2 (fast)
int x = 5;
int times2 = x << 1;   // 10
int times4 = x << 2;   // 20
int times8 = x << 3;   // 40

// Set specific bit
int flags = 0;
flags |= (1 << 0);  // Set bit 0
flags |= (1 << 3);  // Set bit 3

// Combine bytes
byte r = 255, g = 128, b = 64;
int color = (r << 16) | (g << 8) | b;  // RGB color

// Create masks
int mask = 0xFF << 8;  // 0xFF00
```

---

## Right Shift (>>)

Shifts bits right, fills left based on sign (arithmetic).

```csharp
int a = 20;         // 10100
int result = a >> 1;  // 01010 = 10 (divides by 2)
int result2 = a >> 2; // 00101 = 5 (divides by 4)
```

**Formula**: `a >> n` = `a / 2^n`

**Use Cases**:
```csharp
// Divide by power of 2 (fast)
int x = 20;
int div2 = x >> 1;   // 10
int div4 = x >> 2;   // 5

// Extract specific bit
int value = 0b1010;
int bit2 = (value >> 2) & 1;  // 1

// Extract bytes from int
int color = 0xFF8040;
int red = (color >> 16) & 0xFF;    // 255
int green = (color >> 8) & 0xFF;   // 128
int blue = color & 0xFF;           // 64

// Sign extension with negative
int negative = -5;
int result = negative >> 1;  // -3 (sign bit preserved)
```

---

## Unsigned Right Shift (>>)

C# doesn't have a separate operator; >> is arithmetic. Use unsigned types:

```csharp
uint a = 0xFF00;
uint result = a >> 8;  // Logical shift (zeros fill left)

int signed = -1;
int result2 = signed >> 1;  // -1 (sign bit replicated)
```

---

## Practical Examples

### Flag Enumeration

```csharp
[Flags]
public enum Permissions {
    Read = 1 << 0,      // 0001
    Write = 1 << 1,     // 0010
    Delete = 1 << 2,    // 0100
    Execute = 1 << 3    // 1000
}

// Check permission
if ((userPermissions & Permissions.Read) != 0) {
    Console.WriteLine("Has read");
}

// Grant permission
userPermissions |= Permissions.Write;

// Revoke permission
userPermissions &= ~Permissions.Delete;

// Toggle permission
userPermissions ^= Permissions.Execute;
```

### Bit Counting

```csharp
public int CountSetBits(int value) {
    int count = 0;
    while (value != 0) {
        count += value & 1;  // Check lowest bit
        value >>= 1;         // Shift right
    }
    return count;
}

// Usage
int bits = CountSetBits(0b1010101);  // 4 bits set
```

### Color Manipulation

```csharp
public class Color {
    public int ToInt32() {
        // Combine R, G, B
        return (R << 16) | (G << 8) | B;
    }
    
    public static Color FromInt32(int color) {
        int r = (color >> 16) & 0xFF;
        int g = (color >> 8) & 0xFF;
        int b = color & 0xFF;
        return new Color(r, g, b);
    }
}
```

### Bit Manipulation

```csharp
// Check if power of 2
bool IsPowerOfTwo(int n) {
    return n > 0 && (n & (n - 1)) == 0;
}

// Get highest set bit
int HighestSetBit(int n) {
    n |= n >> 1;
    n |= n >> 2;
    n |= n >> 4;
    n |= n >> 8;
    n |= n >> 16;
    return n - (n >> 1);
}
```

---

## Performance Considerations

**Fast Operations**:
```csharp
// Fast - avoid multiplication/division
int doubled = x << 1;       // Instead of x * 2
int halved = x >> 1;        // Instead of x / 2
```

**Modern Optimization**:
```csharp
// Modern compilers optimize these similarly
int x = value * 4;
int y = value << 2;  // Compiler may optimize to same code
```

---

## Best Practices

✓ **Use for flags and permissions**
```csharp
[Flags]
public enum Features {
    Basic = 1 << 0,
    Premium = 1 << 1,
    Advanced = 1 << 2
}
```

✓ **Use for bit manipulation**
```csharp
bool IsBitSet(int value, int position) {
    return (value & (1 << position)) != 0;
}
```

✓ **Document intent**
```csharp
// Set bit 3
flags |= (1 << 3);

// vs clearer with enum
flags |= Permissions.Execute;
```

✓ **Use >> and << only when clear**
```csharp
// Clear: multiply by 2
int doubled = value * 2;

// Or explicitly
int doubled = value << 1;  // Shift left = multiply by 2
```

---

## Common Mistakes

❌ **Confusing & with &&**
```csharp
if (a & b) { }        // Bitwise AND (may not be boolean!)
```

✓ **Use correct operator**
```csharp
if ((a & b) != 0) { }  // Bitwise AND, then check result
if (a && b) { }        // Logical AND
```

---

❌ **Using bitwise on negative numbers without care**
```csharp
int result = -5 >> 1;  // -3 (sign bit replicated)
```

✓ **Understand sign extension**
```csharp
// Use unsigned for logical shift
uint result = 0xFF00 >> 8;  // Zeros fill left
```

---

## Quick Reference

| Operator | Name | Example | Result |
|----------|------|---------|--------|
| & | AND | 5 & 3 | 1 |
| \| | OR | 5 \| 3 | 7 |
| ^ | XOR | 5 ^ 3 | 6 |
| ~ | NOT | ~5 | -6 |
| << | Left Shift | 5 << 1 | 10 |
| >> | Right Shift | 20 >> 1 | 10 |

---

## Next Steps

- Study [Null-Related Operators](../../02-Null-Related/00-Null-Related.md)
- Review [Ternary and Precedence](../../03-Ternary-Precedence/00-Ternary-Precedence.md)
- Learn about [Best Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
