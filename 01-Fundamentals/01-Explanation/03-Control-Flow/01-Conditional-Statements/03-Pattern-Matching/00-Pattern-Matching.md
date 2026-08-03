# Pattern Matching

## Overview

Pattern matching (C# 7+) provides powerful ways to test values and extract information in switch expressions and if statements.

## Type Patterns

Match by type.

```csharp
object obj = "Hello";

if (obj is string s) {
    Console.WriteLine($"String: {s}");
}

// In switch
string result = obj switch {
    string => "It's a string",
    int => "It's an int",
    bool => "It's a bool",
    _ => "Unknown type"
};
```

---

## Property Patterns (C# 8+)

Match by object properties.

```csharp
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
}

Person person = new() { Name = "Alice", Age = 25 };

if (person is { Name: "Alice", Age: > 18 }) {
    Console.WriteLine("Adult Alice");
}

// In switch
string category = person switch {
    { Age: < 18 } => "Minor",
    { Age: >= 18 and < 65 } => "Adult",
    { Age: >= 65 } => "Senior",
    _ => "Unknown"
};
```

---

## List Patterns (C# 9+)

Match array or list contents.

```csharp
int[] numbers = { 1, 2, 3 };

if (numbers is [1, 2, ..]) {
    Console.WriteLine("Starts with 1, 2");
}

// Switch example
string result = numbers switch {
    [1, 2, 3] => "Exact match",
    [1, ..] => "Starts with 1",
    [.., 3] => "Ends with 3",
    [] => "Empty array",
    _ => "Other"
};
```

---

## Relational Patterns

Compare values in patterns.

```csharp
int age = 25;

if (age is > 18 and < 65) {
    Console.WriteLine("Working age");
}

// Multiple alternatives
if (age is < 5 or > 80) {
    Console.WriteLine("Special category");
}
```

---

## Practical Examples

### Validation
```csharp
public bool IsValidUser(User user) => user switch {
    null => false,
    { Name: null or "" } => false,
    { Age: < 0 or > 150 } => false,
    { Email: not null } => true,
    _ => false
};
```

### Discount Logic
```csharp
public decimal GetDiscount(Customer customer) => customer switch {
    { IsVIP: true, Age: > 60 } => 0.30m,
    { IsVIP: true } => 0.25m,
    { Age: > 60 } => 0.15m,
    { PurchaseTotal: > 1000 } => 0.20m,
    _ => 0.05m
};
```

---

## Best Practices

✓ Use pattern matching for type checking
```csharp
if (obj is string s) {
    // s is string here
}
```

✓ Combine patterns for clarity
```csharp
if (person is { Age: > 18 and < 65, IsActive: true }) {
    Process();
}
```

✓ Use when guard clauses
```csharp
string category = person switch {
    _ when person.Age < 18 => "Minor",
    _ when person.Age < 65 => "Adult",
    _ => "Senior"
};
```

---

## Quick Reference

| Pattern | Matches | Example |
|---------|---------|---------|
| Type | Specific type | `is string` |
| Property | Object properties | `is { Age: > 18 }` |
| List | Array/list contents | `is [1, 2, ..]` |
| Relational | Comparison | `is > 18 and < 65` |
| Null | Null value | `is null` |
| Not null | Non-null | `is not null` |

---

## Next Steps

- Study [Loops](../../02-Loop-Statements/README.md)
- Review [Control Keywords](../../03-Control-Keywords/README.md)
- Check [Best Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
