# If-Else Statements

## Overview

If-Else statements execute code based on boolean conditions. They're the foundation of decision-making in programs.

## Simple If

Execute code only if condition is true.

```csharp
int age = 18;

if (age >= 18) {
    Console.WriteLine("You are an adult");
}
```

**Use When**: Single condition with optional action

---

## If-Else

Execute one path if true, another if false.

```csharp
int age = 15;

if (age >= 18) {
    Console.WriteLine("You are an adult");
} else {
    Console.WriteLine("You are a minor");
}
```

**Use When**: Two mutually exclusive paths

---

## If-Else If-Else Chain

Multiple conditions checked in order.

```csharp
int score = 75;

if (score >= 90) {
    Console.WriteLine("A");
} else if (score >= 80) {
    Console.WriteLine("B");
} else if (score >= 70) {
    Console.WriteLine("C");  // This executes
} else if (score >= 60) {
    Console.WriteLine("D");
} else {
    Console.WriteLine("F");
}
```

**Important**: First matching condition executes, rest skipped

**Use When**: Multiple conditions with priority order

---

## Nested If

If statements inside if statements.

```csharp
int age = 25;
bool hasLicense = true;

if (age >= 18) {
    if (hasLicense) {
        Console.WriteLine("Can drive");
    } else {
        Console.WriteLine("Need a license");
    }
} else {
    Console.WriteLine("Too young");
}
```

**Use When**: Multiple conditions must all be true (use && instead)

---

## Best Practices

✓ Use && instead of nested if
```csharp
// Good
if (age >= 18 && hasLicense) {
    Console.WriteLine("Can drive");
}

// Avoid nesting
if (age >= 18) {
    if (hasLicense) {
        Console.WriteLine("Can drive");
    }
}
```

✓ Keep conditions simple
```csharp
// Good
bool isAdult = age >= 18;
if (isAdult) { }

// Less clear
if (age >= 18 && age < 150 && !isStudent || hasPermission) { }
```

✓ Early return to reduce nesting
```csharp
public void Process(User user) {
    if (user == null) return;
    if (!user.IsActive) return;
    // Process user
}
```

---

## Common Mistakes

❌ Using = instead of ==
```csharp
if (x = 5) { }  // Assigns instead of compares
```

✓ Use ==
```csharp
if (x == 5) { }
```

---

❌ Forgetting braces (risky)
```csharp
if (condition)
    Statement1();
    Statement2();  // Always executes!
```

✓ Use braces
```csharp
if (condition) {
    Statement1();
    Statement2();
}
```

---

## Real-World Example

```csharp
public string ValidatePassword(string password) {
    if (string.IsNullOrWhiteSpace(password)) {
        return "Password cannot be empty";
    }
    
    if (password.Length < 8) {
        return "Password must be at least 8 characters";
    }
    
    if (!password.Any(char.IsUpper)) {
        return "Password must contain uppercase letter";
    }
    
    if (!password.Any(char.IsDigit)) {
        return "Password must contain digit";
    }
    
    return "Password is valid";
}
```

---

## Quick Reference

| Pattern | Use |
|---------|-----|
| `if (condition) { }` | Single condition |
| `if (condition) { } else { }` | Two paths |
| `if (c1) { } else if (c2) { } else { }` | Multiple paths |
| `if (c1 && c2) { }` | Both must be true |
| `if (c1 \|\| c2) { }` | Either can be true |

---

## Next Steps

- Study [Switch Statements](../02-Switch-Statements/00-Switch-Statements.md)
- Learn [Pattern Matching](../03-Pattern-Matching/00-Pattern-Matching.md)
- Review [Best Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
