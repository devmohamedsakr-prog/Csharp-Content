# Structs: User-Defined Value Types

## Overview

A `struct` is a value type that allows you to create custom types. Unlike classes (reference types), structs are stored on the stack and copied by value.

### Characteristics
```csharp
public struct Point {
    public int X { get; set; }
    public int Y { get; set; }
}

// Value type: stored on stack
// Copied on assignment: each copy is independent
// No garbage collection needed
// Best for small, immutable data
```

## Defining Structs

### Basic Structure

```csharp
public struct Person {
    // Fields (old style, not recommended)
    public string Name;
    public int Age;
    
    // Properties (recommended)
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    // Constructor
    public Person(string name, int age) {
        Name = name;
        Age = age;
        Email = "";
        DateOfBirth = DateTime.Now;
    }
    
    // Method
    public void PrintInfo() {
        Console.WriteLine($"{Name}, {Age} years old");
    }
}
```

### Modern Struct (Record Struct - C# 10+)

```csharp
public record struct Point(int X, int Y);

// Usage
Point p1 = new Point(10, 20);
Point p2 = p1;  // Copy
Console.WriteLine(p1 == p2);  // true (value equality)
```

## Struct vs Class

### Key Differences

| Aspect | Struct | Class |
|--------|--------|-------|
| Type | Value type | Reference type |
| Storage | Stack | Heap |
| Copy Behavior | Copies entire value | Copies reference |
| Default | Default values (0, false) | null |
| Inheritance | No (except interfaces) | Yes (inheritance) |
| Garbage Collection | Not needed | Required |
| Null Value | Requires `?` suffix | Inherently nullable |
| Performance | Faster (stack) | Slower (heap access) |

### Struct Example

```csharp
public struct Color {
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
    
    public Color(byte r, byte g, byte b) {
        Red = r;
        Green = g;
        Blue = b;
    }
}

// Usage
Color red = new Color(255, 0, 0);
Color redCopy = red;  // Copies entire value
redCopy.Green = 128;

Console.WriteLine(red.Green);      // 0 (unchanged)
Console.WriteLine(redCopy.Green);  // 128 (changed)
```

### Class Example (For Comparison)

```csharp
public class ColorClass {
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
    
    public ColorClass(byte r, byte g, byte b) {
        Red = r;
        Green = g;
        Blue = b;
    }
}

// Usage
ColorClass red = new ColorClass(255, 0, 0);
ColorClass redCopy = red;  // Copies reference (same object!)
redCopy.Green = 128;

Console.WriteLine(red.Green);      // 128 (changed!)
Console.WriteLine(redCopy.Green);  // 128 (same object)
```

## When to Use Structs

### ✓ Good Use Cases

#### 1. Small Data Containers
```csharp
// Good - small struct
public struct Point2D {
    public double X { get; set; }
    public double Y { get; set; }
}

public struct Rectangle {
    public Point2D TopLeft { get; set; }
    public Point2D BottomRight { get; set; }
}
```

#### 2. Immutable Data (No Changes After Creation)
```csharp
// Good - immutable struct
public readonly struct ImmutablePoint {
    public readonly double X;
    public readonly double Y;
    
    public ImmutablePoint(double x, double y) {
        X = x;
        Y = y;
    }
}

// Can't be modified after creation
ImmutablePoint p = new(10, 20);
// p.X = 15;  // Compiler error
```

#### 3. High-Performance Code
```csharp
// Stack allocation is faster
// Process thousands of points efficiently
public struct Vector3 {
    public float X, Y, Z;
}

Vector3[] vertices = new Vector3[1000000];  // Stack-like efficiency
```

#### 4. Graphics/Games
```csharp
public struct Color {
    public byte R, G, B, A;
}

public struct Vertex {
    public Vector3 Position;
    public Color Color;
    public Vector2 TextureCoord;
}
```

### ❌ Bad Use Cases

#### 1. Large Data (> 16 bytes)
```csharp
// Bad - too large for struct
public struct LargeData {
    public byte[] data;  // Might be huge
    public string description;  // Long string
}

// Copying large structs is expensive
LargeData copy = original;  // Entire content copied
```

#### 2. Mutable Complex Objects
```csharp
// Bad - mutable struct with collections
public struct DataHolder {
    public List<int> items;  // Mutable collection
}

DataHolder holder = new();
holder.items = new List<int> { 1, 2, 3 };

DataHolder copy = holder;
copy.items.Add(4);
// holder.items also changed! (reference copied, not value)
```

#### 3. Need Inheritance
```csharp
// Bad - structs can't inherit from structs
// Can only implement interfaces
public struct Base {
}

// This is NOT valid:
// public struct Derived : Base { }  // Compiler error
```

## Struct Constructors

### Default Constructor
```csharp
public struct Point {
    public int X { get; set; }
    public int Y { get; set; }
}

Point p1 = new Point();  // All properties set to default
Console.WriteLine($"X={p1.X}, Y={p1.Y}");  // X=0, Y=0

Point p2 = default;  // Same as new Point()
```

### Custom Constructors

```csharp
public struct Rectangle {
    public int Width { get; set; }
    public int Height { get; set; }
    
    // Custom constructor
    public Rectangle(int width, int height) {
        Width = width;
        Height = height;
    }
    
    // Method
    public int Area => Width * Height;
}

Rectangle r1 = new Rectangle(10, 20);
Rectangle r2 = default;  // Width=0, Height=0
```

### Constructor Requirements

```csharp
// ✓ All fields/properties initialized in constructor
public struct Good {
    public int X { get; set; }
    public int Y { get; set; }
    
    public Good(int x, int y) {
        X = x;
        Y = y;
    }
}

// ✗ Constructor doesn't initialize all properties
// (Compiler warning in modern C#)
public struct Problem {
    public int X { get; set; }
    public int Y { get; set; }
    
    public Problem(int x) {
        X = x;
        // Y not initialized
    }
}
```

## Mutable vs Immutable Structs

### Mutable Struct (Can Be Changed)

```csharp
public struct MutablePoint {
    public int X { get; set; }
    public int Y { get; set; }
    
    public MutablePoint(int x, int y) {
        X = x;
        Y = y;
    }
}

MutablePoint p = new(10, 20);
p.X = 15;  // Can modify
```

### Immutable Struct (Cannot Be Changed)

```csharp
public readonly struct ImmutablePoint {
    public int X { get; }
    public int Y { get; }
    
    public ImmutablePoint(int x, int y) {
        X = x;
        Y = y;
    }
}

ImmutablePoint p = new(10, 20);
// p.X = 15;  // Compiler error - readonly
```

### Benefits of Immutable Structs

```csharp
// Predictable behavior
public readonly struct Money {
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money(decimal amount, string currency) {
        Amount = amount;
        Currency = currency;
    }
    
    // Can override Equals for value equality
    public override bool Equals(object obj) {
        return obj is Money m && 
               m.Amount == Amount && 
               m.Currency == Currency;
    }
}

Money m1 = new(100, "USD");
Money m2 = m1;  // Copy
// Can't accidentally modify m1 via m2
```

## Struct Equality

### Value Equality
```csharp
public struct Point {
    public int X { get; set; }
    public int Y { get; set; }
}

Point p1 = new Point { X = 10, Y = 20 };
Point p2 = new Point { X = 10, Y = 20 };

Console.WriteLine(p1 == p2);  // true (same values)
Console.WriteLine(ReferenceEquals(p1, p2));  // false (different copies)
```

### Reference Equality (Classes)
```csharp
public class PointClass {
    public int X { get; set; }
    public int Y { get; set; }
}

PointClass p1 = new PointClass { X = 10, Y = 20 };
PointClass p2 = new PointClass { X = 10, Y = 20 };

Console.WriteLine(p1 == p2);  // false (different objects)
Console.WriteLine(ReferenceEquals(p1, p2));  // false (different objects)

PointClass p3 = p1;
Console.WriteLine(p1 == p3);  // true (same reference)
Console.WriteLine(ReferenceEquals(p1, p3));  // true (same object)
```

## Nullable Structs

```csharp
public struct Point {
    public int X { get; set; }
    public int Y { get; set; }
}

// Regular struct - cannot be null
Point p1 = new Point();  // Default values

// Nullable struct - can be null
Point? p2 = null;  // Null allowed with ?

Point? p3 = new Point { X = 10, Y = 20 };  // Has value

// Checking for null
if (p2.HasValue) {
    Console.WriteLine($"X={p2.Value.X}");
} else {
    Console.WriteLine("Point is null");
}

// Using null-coalescing
Point p4 = p2 ?? new Point { X = 0, Y = 0 };
```

## Performance Considerations

### Stack vs Heap Allocation

```csharp
// Struct - stack allocation (fast)
public struct FastPoint {
    public int X, Y;
}

// Class - heap allocation (slower)
public class SlowPoint {
    public int X, Y;
}

// Struct array - cache-friendly
FastPoint[] points = new FastPoint[1000];  // Contiguous memory

// Class array - scattered in memory
SlowPoint[] classPoints = new SlowPoint[1000];  // References scattered
```

### Boxing/Unboxing

```csharp
public struct Value {
    public int Data;
}

Value v = new Value { Data = 10 };

// Boxing - converts to reference type (slow!)
object boxed = v;  // Copy to heap

// Unboxing - converts back (slow!)
Value unboxed = (Value)boxed;  // Copy from heap
```

## Common Struct Mistakes

❌ **Large Mutable Structs**
```csharp
public struct LargeMutable {
    public int[] array;
    public string text;
    public DateTime timestamp;
}

// Expensive to copy
LargeMutable copy = original;  // Large copy operation
```

✓ **Small Immutable Structs**
```csharp
public readonly struct Small {
    public int X { get; }
    public int Y { get; }
}

// Cheap to copy
Small copy = original;  // Quick copy
```

❌ **Mutable Structs with References**
```csharp
public struct Problem {
    public List<int> items;  // Reference type inside struct
    
    public Problem() {
        items = new List<int>();
    }
}

Problem p1 = new();
p1.items.Add(1);

Problem p2 = p1;
p2.items.Add(2);
// p1.items also modified! (reference copied)
```

✓ **Avoid Reference Types in Structs**
```csharp
public readonly struct Good {
    public int X { get; }
    public int Y { get; }
    // No collections or references
}
```

## Summary

**Use Structs For**:
- Small value types (< 16 bytes)
- Immutable data
- High-performance code
- Value semantics (copy on assignment)

**Use Classes For**:
- Complex objects
- Mutable data
- Need inheritance
- Reference semantics (reference on assignment)

**Remember**:
- Structs are value types (stack)
- Classes are reference types (heap)
- Copy a struct = copy entire value
- Assign a class = copy reference
- Keep structs small and immutable

---

**Key Takeaway**: Structs are powerful for performance-critical code with small, immutable data. For larger or mutable objects, use classes.
