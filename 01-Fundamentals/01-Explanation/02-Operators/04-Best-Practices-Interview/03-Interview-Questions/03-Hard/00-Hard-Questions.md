# Operators Interview - Hard Level

## Q1: Explain operator overloading and when to use it

**Answer**:
Custom behavior for operators on your types

```csharp
public class Money {
    public decimal Amount { get; }
    
    public static Money operator+(Money a, Money b) {
        return new Money(a.Amount + b.Amount);
    }
    
    public static bool operator==(Money a, Money b) {
        return a.Amount == b.Amount;
    }
    
    public static bool operator<(Money a, Money b) {
        return a.Amount < b.Amount;
    }
}

Money m1 = new Money(10);
Money m2 = new Money(20);
Money sum = m1 + m2;  // Uses overloaded +
```

When: For domain types that naturally support operations

---

## Q2: What are expression-bodied members and when use?

**Answer**:
Shorthand property syntax using =>

```csharp
public class Rectangle {
    public double Width { get; set; }
    public double Height { get; set; }
    
    // Expression-bodied property
    public double Area => Width * Height;
    
    // Expression-bodied method
    public bool IsSquare => Width == Height;
    
    public double Perimeter => 2 * (Width + Height);
}

var rect = new Rectangle { Width = 5, Height = 5 };
Console.WriteLine(rect.Area);      // Calculated via =>
```

When: Simple read-only properties or methods

---

## Q3: Optimize this: Check if number is power of 2

**Answer**:
```csharp
// Naive: O(log n)
bool IsPowerOfTwo(int n) {
    return n > 0 && (n & (n - 1)) == 0;
}

// Why it works:
// n = 8 = 1000
// n-1 = 7 = 0111
// n & (n-1) = 0000 = 0 ✓

// n = 6 = 0110
// n-1 = 5 = 0101
// n & (n-1) = 0100 ≠ 0 ✗
```

Bitwise trick: Power of 2 has exactly one bit set

---

## Q4: Design expression tree for parsing

**Answer**:
```csharp
public abstract class Expr {
    public abstract T Accept<T>(IExprVisitor<T> visitor);
}

public class BinaryOp : Expr {
    public Expr Left { get; }
    public string Operator { get; }
    public Expr Right { get; }
    
    public override T Accept<T>(IExprVisitor<T> visitor) {
        return visitor.VisitBinary(this);
    }
}

public interface IExprVisitor<T> {
    T VisitBinary(BinaryOp expr);
    T VisitLiteral(Literal expr);
}

// Usage: Parse and evaluate expressions with custom operators
```

Advanced: Visitor pattern for expression evaluation

---

## Q5: Implement checked vs unchecked arithmetic

**Answer**:
```csharp
// Unchecked (default) - wraps silently
int max = int.MaxValue;
int result = max + 1;  // -2147483648 (wraps)

// Checked - throws on overflow
try {
    int result = checked(max + 1);  // OverflowException
} catch (OverflowException) {
    // Handle overflow
}

// Checked context
checked {
    int x = max + 1;  // Throws
}

// Unchecked context
unchecked {
    int x = max + 1;  // Wraps silently
}
```

When: Financial calculations, critical operations

---

## Q6: Performance: When to use bitwise vs arithmetic

**Answer**:
```csharp
// Today's compilers often optimize similarly
int x = value * 2;   // Compiler may optimize to << 1
int y = value << 1;  // Explicit shift

// Modern guidance:
// - Use arithmetic for clarity (*, /, %)
// - Use bitwise for flags and bit manipulation
// - Profile before micro-optimizing

// When bitwise still matters:
// - Bit flags/permissions
// - Embedded systems
// - Graphics/game engines
// - Low-level algorithms
```

Modern compilers optimize * and / by 2^n to shifts

---

## Summary

- Operator overloading for custom types
- Expression-bodied members for conciseness
- Bitwise tricks (power of 2, bit counting)
- Checked/unchecked for overflow
- Expression trees for custom evaluation
- Know when micro-optimization helps

---

**Complete**: All operator interview questions covered
