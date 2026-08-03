# Operators in C#

## Overview
Operators are symbols that perform operations on variables and values.

---

## Arithmetic Operators

Perform mathematical calculations on numeric values.

```csharp
int a = 10;
int b = 3;

// Addition
int sum = a + b;           // 13

// Subtraction
int diff = a - b;          // 7

// Multiplication
int product = a * b;       // 30

// Division
int quotient = a / b;      // 3 (integer division)
double precise = (double)a / b;  // 3.333...

// Modulo (remainder)
int remainder = a % b;     // 1

// Increment/Decrement
int x = 5;
x++;  // x becomes 6 (post-increment)
++x;  // x becomes 7 (pre-increment)

// String concatenation
string full = "Hello" + " " + "World";  // "Hello World"
```

---

## Comparison Operators

Compare two values and return true or false.

```csharp
int a = 10;
int b = 5;

// Equal
bool equal = a == b;       // false

// Not equal
bool notEqual = a != b;    // true

// Greater than
bool greater = a > b;      // true

// Less than
bool less = a < b;         // false

// Greater or equal
bool greaterEq = a >= 10;  // true

// Less or equal
bool lessEq = b <= 5;      // true

// String comparison
string name1 = "Alice";
string name2 = "Bob";
bool isSame = name1 == name2;  // false
```

---

## Logical Operators

Combine boolean conditions.

### AND Operator (&&)
Both conditions must be true.

```csharp
int age = 25;
bool hasLicense = true;

if (age >= 18 && hasLicense) {
    Console.WriteLine("Can drive");
}

// Short-circuit: if first is false, second not evaluated
bool result = false && ExpensiveFunction();  // Function not called
```

### OR Operator (||)
At least one condition must be true.

```csharp
string userRole = "Admin";
bool isManager = false;

if (userRole == "Admin" || isManager) {
    Console.WriteLine("Has access");
}

// Short-circuit: if first is true, second not evaluated
bool result = true || ExpensiveFunction();  // Function not called
```

### NOT Operator (!)
Negates a boolean value.

```csharp
bool isActive = true;

if (!isActive) {
    Console.WriteLine("Not active");
}

// Combined
bool canAccess = !(userRole == "Guest");  // true if NOT guest
```

---

## Assignment Operators

Assign values to variables.

```csharp
int x = 10;

// Basic assignment
x = 20;

// Compound assignment
x += 5;   // x = x + 5;   → 25
x -= 3;   // x = x - 3;   → 22
x *= 2;   // x = x * 2;   → 44
x /= 2;   // x = x / 2;   → 22
x %= 5;   // x = x % 5;   → 2

// String assignment
string message = "Hello";
message += " World";  // "Hello World"
```

---

## Ternary Operator

Conditional expression (short if-else).

```csharp
int age = 20;

// Format: condition ? valueIfTrue : valueIfFalse
string status = age >= 18 ? "Adult" : "Minor";
// status = "Adult"

// Nested ternary
string grade = score >= 90 ? "A" : 
               score >= 80 ? "B" : 
               score >= 70 ? "C" : "F";

// With assignment
int discount = isVIP ? 20 : 10;
```

---

## Null-Related Operators

Handle null values safely.

### Null-Coalescing Operator (??)

Returns left value if not null, otherwise right value.

```csharp
string name = null;
string result = name ?? "Unknown";
// result = "Unknown"

int? age = null;
int actualAge = age ?? 0;
// actualAge = 0
```

### Null-Conditional Operator (?.)

Safely access members of potentially null object.

```csharp
class Person {
    public string Name { get; set; }
}

Person person = null;

// Without null-conditional - throws error
// string name = person.Name;

// With null-conditional - returns null safely
string name = person?.Name;  // null (no error)

// Chaining
string firstName = person?.Name?.Substring(0, 1);  // null
```

### Null-Coalescing Assignment (??=)

Assigns only if null.

```csharp
string name = null;
name ??= "Default";  // Assigns because null
// name = "Default"

name ??= "Another";  // Doesn't assign because not null
// name = "Default"
```

---

## Bitwise Operators

Operate on individual bits.

```csharp
int a = 5;   // Binary: 0101
int b = 3;   // Binary: 0011

// AND
int and = a & b;  // 0001 = 1

// OR
int or = a | b;   // 0111 = 7

// XOR (exclusive OR)
int xor = a ^ b;  // 0110 = 6

// NOT
int not = ~a;     // Inverts all bits

// Left shift
int leftShift = a << 1;  // 1010 = 10

// Right shift
int rightShift = a >> 1;  // 0010 = 2
```

---

## Operator Precedence

Order in which operators are evaluated (highest to lowest).

```csharp
// Parentheses have highest precedence
int result = (5 + 3) * 2;  // 16

// Without parentheses: multiplication before addition
int result2 = 5 + 3 * 2;   // 11 (not 16)

// Full precedence order
int x = 5 + 3 * 2 - 1;  // Multiply first: 5 + 6 - 1 = 10
```

**Precedence Order** (simplified):
1. Parentheses `()`
2. Multiplication, Division, Modulo: `*`, `/`, `%`
3. Addition, Subtraction: `+`, `-`
4. Comparison: `==`, `!=`, `<`, `>`, `<=`, `>=`
5. AND: `&&`
6. OR: `||`
7. Assignment: `=`, `+=`, etc.

---

## Best Practices

✓ Use explicit parentheses for clarity
```csharp
// Clear intent
int result = (a + b) * (c - d);
```

✓ Use appropriate operators
```csharp
// Good: null-coalescing
string name = input ?? "Default";

// Bad: too many ternary operators
string status = age > 18 ? "Adult" : age > 13 ? "Teen" : "Child";
```

✓ Remember short-circuit evaluation
```csharp
// Second condition not evaluated if first is false
if (user != null && user.IsActive) { }
```

---

## Common Mistakes

❌ Confusing `=` (assignment) with `==` (comparison)
```csharp
if (x = 5) { }  // Error: assigns instead of compares
```

✓ Use comparison
```csharp
if (x == 5) { }  // Correct
```

❌ Forgetting operator precedence
```csharp
int result = 5 + 3 * 2;  // 11, not 16
```

✓ Use parentheses
```csharp
int result = (5 + 3) * 2;  // 16
```
