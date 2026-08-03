# Comparison Operators

## Overview

Comparison operators compare two values and return a boolean (true or false). These are essential for decision-making in programs.

## Equality Operators

### Equal (==)
Returns true if values are equal.

```csharp
int a = 5;
int b = 5;
bool result = a == b;  // true

// Different types
double x = 5.0;
int y = 5;
bool same = x == y;  // true (values are equal)

// String comparison
string name1 = "Alice";
string name2 = "Alice";
bool isSame = name1 == name2;  // true (content comparison)

// Object reference comparison
class Person { }
Person p1 = new Person();
Person p2 = p1;
bool sameObject = p1 == p2;  // true (same object)
```

**Important**: `==` compares values, not references (for most types)

---

### Not Equal (!=)
Returns true if values are different.

```csharp
int a = 5;
int b = 10;
bool result = a != b;  // true

// Strings
string status = "inactive";
if (status != "active") {
    Console.WriteLine("Not active");
}

// Negation
if (value != null) {
    Process(value);
}
```

---

## Relational Operators

### Greater Than (>)
```csharp
int age = 25;
bool isAdult = age > 18;  // true

double score = 85.5;
if (score > 90) {
    Console.WriteLine("Excellent");
}
```

---

### Greater Than or Equal (>=)
```csharp
int minScore = 60;
int studentScore = 60;
bool passed = studentScore >= minScore;  // true

double price = 99.99;
if (price >= 100) {
    Console.WriteLine("Over 100");
}
```

---

### Less Than (<)
```csharp
int temperature = 10;
bool isCold = temperature < 0;  // false

if (value < limit) {
    Console.WriteLine("Within limit");
}
```

---

### Less Than or Equal (<=)
```csharp
int capacity = 100;
int current = 100;
bool withinCapacity = current <= capacity;  // true

if (usage <= maxUsage) {
    Console.WriteLine("Safe");
}
```

---

## Comparison Order

```csharp
// Most useful for numbers
int x = 10;
int y = 20;

if (x < y && x > 0) {
    Console.WriteLine("Between 0 and 20");
}

// Range checking
int age = 25;
if (age >= 18 && age < 65) {
    Console.WriteLine("Working age");
}
```

---

## String Comparison

### Reference vs Value Equality

```csharp
// Value equality
string s1 = "Hello";
string s2 = "Hello";
bool equal = s1 == s2;  // true (content)

// Reference equality
object obj1 = new object();
object obj2 = obj1;
bool sameRef = obj1 == obj2;  // true (same object)

object obj3 = new object();
bool different = obj1 == obj3;  // false (different objects)
```

---

### Case-Sensitive Comparison

```csharp
string status = "Active";

// Default: case-sensitive
bool match1 = status == "Active";   // true
bool match2 = status == "active";   // false

// Case-insensitive
bool match3 = status.Equals("active", StringComparison.OrdinalIgnoreCase);  // true
bool match4 = status.ToLower() == "active";  // true
```

---

### String Ordering

```csharp
string a = "Alice";
string b = "Bob";

if (a < b) {
    Console.WriteLine("Alice comes before Bob");  // Alphabetical
}

// Lexicographic (dictionary) order
string x = "apple";
string y = "apricot";
bool result = x < y;  // true (p < r)
```

---

## Comparison with Nullable Types

```csharp
int? x = 5;
int? y = null;

// Comparison with value
bool result1 = x == 5;       // true

// Comparison with null
bool result2 = y == null;    // true

// Nullable to nullable
int? a = 5;
int? b = 5;
bool same = a == b;          // true
```

---

## Practical Examples

### Input Validation
```csharp
int age = int.Parse(userInput);
if (age >= 0 && age <= 150) {
    Console.WriteLine("Valid age");
} else {
    Console.WriteLine("Invalid age");
}
```

### Range Checking
```csharp
int score = 85;
string grade;

if (score >= 90) {
    grade = "A";
} else if (score >= 80) {
    grade = "B";  // This executes
} else if (score >= 70) {
    grade = "C";
} else {
    grade = "F";
}
```

### Equality Tests
```csharp
string password = "secret123";
string confirmation = userInput;

if (password == confirmation) {
    Console.WriteLine("Passwords match");
} else {
    Console.WriteLine("Passwords don't match");
}
```

### Boundary Conditions
```csharp
int min = 1;
int max = 100;
int value = 50;

if (value >= min && value <= max) {
    Console.WriteLine("In range");
}

if (value < min || value > max) {
    Console.WriteLine("Out of range");
}
```

---

## Chaining Comparisons

```csharp
// NOT valid: can't chain like Python
// if (1 < x < 10) { }  // Error!

// Valid: combine with logical operators
if (1 < x && x < 10) {
    Console.WriteLine("Between 1 and 10");
}

// Range pattern (C# 7+)
int value = 5;
if (value is >= 1 and <= 10) {
    Console.WriteLine("In range");
}

// Pattern matching (C# 8+)
if (value is > 0 and < 100) {
    Console.WriteLine("Valid range");
}
```

---

## Best Practices

✓ **Use appropriate comparison**
```csharp
// For numeric ranges
if (value >= min && value <= max) { }

// For equality
if (status == "active") { }

// For null checks
if (value != null) { }
```

✓ **Be explicit with types**
```csharp
// Clear
int age = 25;
if (age > 18) { }

// Risky
var age = "25";
if (age > "18") { }  // String comparison!
```

✓ **Use StringComparison for strings**
```csharp
// Good
string input = "Alice";
bool match = input.Equals("alice", StringComparison.OrdinalIgnoreCase);

// Risky
bool match2 = input == "alice";  // false (case-sensitive)
```

---

## Common Mistakes

❌ **Confusing = with ==**
```csharp
if (x = 5) { }  // Assigns instead of compares
```

✓ **Use == for comparison**
```csharp
if (x == 5) { }  // Correct
```

---

❌ **Case sensitivity in strings**
```csharp
if (status == "active") {  // Won't match "Active"
    Console.WriteLine("Active");
}
```

✓ **Use case-insensitive comparison**
```csharp
if (status.Equals("active", StringComparison.OrdinalIgnoreCase)) {
    Console.WriteLine("Active");
}
```

---

❌ **Comparing references instead of values**
```csharp
string a = new string(new[] { 'H', 'i' });
string b = new string(new[] { 'H', 'i' });
bool same = a == b;  // true (strings use value equality)
```

---

❌ **Assuming null comparisons fail**
```csharp
string text = null;
if (text == null) {  // true, this is correct
    Console.WriteLine("Is null");
}
```

---

## Quick Reference

| Operator | Meaning | Example |
|----------|---------|---------|
| == | Equal | x == 5 |
| != | Not equal | x != 5 |
| > | Greater than | x > 5 |
| >= | Greater/equal | x >= 5 |
| < | Less than | x < 5 |
| <= | Less/equal | x <= 5 |

---

## Next Steps

- Study [Logical Operators (AND/OR/NOT)](../../02-Logical-AND/00-Logical-AND.md)
- Review [Assignment Operators](../../01-Arithmetic-Assignment/02-Assignment/00-Assignment-Operators.md)
- Practice with [Interview Questions](../../04-Best-Practices-Interview/03-Interview-Questions/README.md)
