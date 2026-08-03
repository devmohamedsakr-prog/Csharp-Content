# Ternary Operator and Operator Precedence

## Ternary (Conditional) Operator (?:)

### Overview

The ternary operator is a shorthand for if-else expressions. It evaluates a condition and returns one of two values.

### Syntax

```csharp
condition ? valueIfTrue : valueIfFalse
```

### Basic Examples

```csharp
int age = 20;
string status = age >= 18 ? "Adult" : "Minor";
// status = "Adult"

bool isRaining = true;
string activity = isRaining ? "Stay inside" : "Go outside";
// activity = "Stay inside"

int score = 85;
string result = score >= 80 ? "Pass" : "Fail";
// result = "Pass"
```

### With Different Types

```csharp
// Numeric
double temperature = 15.5;
double clothing = temperature < 0 ? 5 : 1;

// String
string userStatus = isAdmin ? "Administrator" : "User";

// Object
Person priority = isVIP ? vipPerson : regularPerson;

// Boolean
bool canVote = age >= 18 ? true : false;
// Better: bool canVote = age >= 18;
```

---

## Nested Ternary

Multiple conditions can be nested:

```csharp
int score = 85;

// Grading system
string grade = score >= 90 ? "A" :
               score >= 80 ? "B" :
               score >= 70 ? "C" :
               score >= 60 ? "D" : "F";
// grade = "B"

// Temperature description
string weather = temperature < 0 ? "Freezing" :
                 temperature < 10 ? "Cold" :
                 temperature < 20 ? "Cool" :
                 temperature < 30 ? "Warm" : "Hot";
```

---

## Ternary vs If-Else

**Ternary** - Use for simple expressions:
```csharp
string message = isSuccess ? "Done!" : "Failed";
int value = isPositive ? Math.Abs(x) : -Math.Abs(x);
```

**If-Else** - Use for complex logic:
```csharp
if (isSuccess) {
    SendEmail();
    LogSuccess();
    UpdateUI();
} else {
    LogError();
    ShowRetry();
}
```

---

## Operator Precedence

Precedence determines evaluation order (highest to lowest):

### Complete Precedence Order

1. **Primary** - `()`, `[]`, `?.`, `??`
2. **Unary** - `!`, `~`, `++`, `--`, `+x`, `-x`
3. **Multiplicative** - `*`, `/`, `%`
4. **Additive** - `+`, `-`
5. **Shift** - `<<`, `>>`
6. **Relational** - `<`, `>`, `<=`, `>=`, `is`, `as`
7. **Equality** - `==`, `!=`
8. **Logical AND** - `&`
9. **Logical XOR** - `^`
10. **Logical OR** - `|`
11. **Conditional AND** - `&&`
12. **Conditional OR** - `||`
13. **Null-coalescing** - `??`, `??=`
14. **Ternary** - `?:`
15. **Assignment** - `=`, `+=`, `-=`, etc.

### Examples

```csharp
// Multiplication before addition
int result = 5 + 3 * 2;  // 11 (not 16)
// Evaluated as: 5 + (3 * 2)

// Logical AND before OR
bool logic = true || false && false;  // true
// Evaluated as: true || (false && false) = true || false = true

// Logical NOT has high precedence
bool neg = !true && false;  // false
// Evaluated as: (!true) && false = false && false = false
```

---

## Practical Precedence Examples

### Mixed Operators

```csharp
// What's the result?
int x = 10;
int y = 5;
int z = 2;

int result = x + y * z;  // 20 (multiply first)
// Equivalent: x + (y * z) = 10 + 10 = 20

int result2 = x * y + z;  // 52
// Equivalent: (x * y) + z = 50 + 2 = 52

int result3 = (x + y) * z;  // 30 (parentheses override)
// Equivalent: (10 + 5) * 2 = 15 * 2 = 30
```

### Logical Precedence

```csharp
bool a = true;
bool b = false;
bool c = true;

// AND before OR
bool result1 = a || b && c;  // true
// Equivalent: a || (b && c) = true || false = true

bool result2 = (a || b) && c;  // true
// Equivalent: (true || false) && true = true && true = true

// NOT has highest precedence
bool result3 = !a || b && c;  // false
// Equivalent: (!a) || (b && c) = false || false = false
```

### Comparison Precedence

```csharp
int x = 5;
int y = 10;

// Arithmetic before comparison
bool result = x + 5 > y;  // false
// Equivalent: (x + 5) > y = 10 > 10 = false

bool result2 = x < y - 2;  // false
// Equivalent: x < (y - 2) = 5 < 8 = true
```

---

## Using Parentheses

Always use parentheses for clarity, even when not required:

```csharp
// Works but unclear
if (a && b || c && d) { }

// Clear
if ((a && b) || (c && d)) { }

// Even clearer with variables
bool condition1 = a && b;
bool condition2 = c && d;
if (condition1 || condition2) { }
```

### When Parentheses Matter

```csharp
// Wrong: doesn't do what you might expect
int result = 5 + 3 * 2;  // 11, not 16

// Correct: explicit precedence
int result = (5 + 3) * 2;  // 16

// Logical precedence issue
if (age > 18 && employed || retired) { }
// Means: (age > 18 && employed) || retired

if ((age > 18 && employed) || retired) { }  // Clearer intent
```

---

## Chained Operators

### Ternary Chaining

```csharp
// Complex nested ternary - hard to read
string category = age < 5 ? "Toddler" :
                  age < 12 ? "Child" :
                  age < 18 ? "Teen" :
                  age < 65 ? "Adult" : "Senior";

// Better: use switch or if-else
string category;
if (age < 5) category = "Toddler";
else if (age < 12) category = "Child";
else if (age < 18) category = "Teen";
else if (age < 65) category = "Adult";
else category = "Senior";

// Best: use switch expression (C# 8+)
string category = age switch {
    < 5 => "Toddler",
    < 12 => "Child",
    < 18 => "Teen",
    < 65 => "Adult",
    _ => "Senior"
};
```

### Null-Coalescing Chain

```csharp
// Multiple ?? chained
string result = value1 ?? value2 ?? value3 ?? "default";
// Uses first non-null value
```

---

## Real-World Examples

### Data Validation

```csharp
int age = GetAge();

// With precedence understanding
bool canAccess = age >= 18 && (isStudent || isTeacher) ? true : false;
// Better written as:
bool canAccess = age >= 18 && (isStudent || isTeacher);
```

### Pricing Calculation

```csharp
decimal basePrice = 100;
double discount = 0;
bool isVIP = false;
bool isMember = true;

// Calculate with mixed operators
decimal finalPrice = basePrice * 
                     (1 - (discount / 100)) *
                     (isVIP ? 0.9m : (isMember ? 0.95m : 1.0m));
```

### Decision Logic

```csharp
public bool CanProcess(Order order, User user) {
    // Complex logic with clear precedence
    return !order.IsCancelled &&
           order.Total > 0 &&
           (user.IsAdmin || user.IsManager || order.OwnerId == user.Id) &&
           (order.Status == "Pending" || order.Status == "Review");
}
```

---

## Best Practices

✓ **Use ternary for simple expressions**
```csharp
string status = isActive ? "On" : "Off";
```

✓ **Use explicit parentheses**
```csharp
if ((a && b) || (c && d)) { }  // Clear
```

✓ **Avoid complex nested ternaries**
```csharp
// Avoid
string x = a ? b ? c : d : e ? f : g;

// Use switch expression instead
string x = condition switch {
    true => value1,
    false => value2,
    _ => value3
};
```

✓ **Know operator precedence**
```csharp
// Understand this without testing
int x = 2 + 3 * 4;  // 14

// Use parentheses when unclear
int y = (2 + 3) * 4;  // 20
```

✓ **Extract complex logic**
```csharp
if (IsEligible(order, user)) {
    Process(order);
}

private bool IsEligible(Order order, User user) {
    return !order.IsCancelled && 
           order.Total > 0 && 
           user.HasPermission("process");
}
```

---

## Common Mistakes

❌ **Forgetting operator precedence**
```csharp
int x = 5 + 3 * 2;  // 11, not 16
```

✓ **Use parentheses**
```csharp
int x = (5 + 3) * 2;  // 16
```

---

❌ **Complex nested ternaries**
```csharp
// Hard to read
string x = a ? b ? c : d : e ? f : g;
```

✓ **Use switch or if-else**
```csharp
string x = condition switch {
    case1 => value1,
    case2 => value2,
    _ => default
};
```

---

❌ **Unclear precedence in conditions**
```csharp
if (a && b || c && d) { }  // Ambiguous
```

✓ **Use parentheses**
```csharp
if ((a && b) || (c && d)) { }  // Clear
```

---

## Quick Reference: Precedence

| Level | Operators |
|-------|-----------|
| 1 (Highest) | `()`, `[]`, `?.`, `??` |
| 2 | `!`, `~`, `++`, `--` |
| 3 | `*`, `/`, `%` |
| 4 | `+`, `-` |
| 5 | `<<`, `>>` |
| 6 | `<`, `>`, `<=`, `>=` |
| 7 | `==`, `!=` |
| 8 | `&` |
| 9 | `^` |
| 10 | `\|` |
| 11 | `&&` |
| 12 | `\|\|` |
| 13 | `??` |
| 14 | `?:` |
| 15 (Lowest) | `=`, `+=`, `-=` |

---

## Next Steps

- Review [Best Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
- Study [Common Mistakes](../../04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md)
- Practice with [Interview Questions](../../04-Best-Practices-Interview/03-Interview-Questions/README.md)
