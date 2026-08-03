# Value Types

## Overview

Value types in C# are data types that store their actual values directly in memory (typically on the stack). When you assign a value type to another variable or pass it as a parameter, the entire value is copied.

## Key Characteristics

- **Storage**: Stack memory (typically)
- **Copy Behavior**: Entire value is copied
- **Garbage Collection**: Not needed (automatic cleanup when scope ends)
- **Default Value**: 0, false, or equivalent
- **Inheritance**: Cannot inherit from other value types
- **Performance**: Generally faster for small data

## Categories in This Section

### 1. Numeric Types
Integer types (byte, short, int, long), floating-point types (float, double), and decimal type. Each has different ranges and precision characteristics.

**Files**: `01-Numeric/00-Numeric-Types-Overview.md`

### 2. Boolean and Character Types
The `bool` type for true/false values and `char` type for Unicode characters.

**Files**: `02-Boolean-Char/00-Boolean-Char-Types.md`

### 3. Structs
User-defined value types that can contain fields, properties, and methods.

**Files**: `03-Structs/00-Structs-ValueTypes.md`

## Quick Reference

| Type | Bits | Range |
|------|------|-------|
| byte | 8 | 0 to 255 |
| sbyte | 8 | -128 to 127 |
| short | 16 | -32,768 to 32,767 |
| ushort | 16 | 0 to 65,535 |
| int | 32 | -2.1B to 2.1B |
| uint | 32 | 0 to 4.3B |
| long | 64 | -9.2E18 to 9.2E18 |
| ulong | 64 | 0 to 18.4E18 |
| float | 32 | ±1.5E-45 to ±3.4E38 |
| double | 64 | ±5E-324 to ±1.7E308 |
| decimal | 128 | ±7.9E-28 to ±7.9E28 |
| bool | 8 | true or false |
| char | 16 | Unicode character |

## Value Type Behavior

### Stack Allocation
```csharp
int x = 10;  // Stored on stack
int y = x;   // Copy entire value to stack
y = 20;
Console.WriteLine(x);  // 10 (unchanged)
```

### Default Values
```csharp
int defaultInt = default;        // 0
bool defaultBool = default;      // false
char defaultChar = default;      // '\0'
decimal defaultDecimal = default; // 0.0m
```

## When to Use Value Types

✓ Small immutable data
✓ Performance-critical code
✓ Primitive values (numbers, booleans)
✓ Custom struct for related small values

## Common Pitfalls

❌ Mutable structs (hard to reason about)
❌ Large structs (inefficient copying)
❌ Boxing/unboxing in loops (performance hit)

## Learning Path

1. Start with **Numeric Types** - understand integer and floating-point values
2. Learn **Boolean and Character Types** - basic logic and text
3. Explore **Structs** - creating your own value types
4. Read **Comparison** section to understand value vs reference differences

## Navigation

- **Parent**: [Data Types](../README.md)
- **Numeric Types**: `01-Numeric/00-Numeric-Types-Overview.md`
- **Boolean & Character**: `02-Boolean-Char/00-Boolean-Char-Types.md`
- **Structs**: `03-Structs/00-Structs-ValueTypes.md`
- **Comparison & Practices**: `../03-Comparison-Practices/README.md`
