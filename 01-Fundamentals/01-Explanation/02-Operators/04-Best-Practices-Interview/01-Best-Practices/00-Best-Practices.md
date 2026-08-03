# Operators: Best Practices

## Arithmetic Operators

✓ **Use appropriate types**
```csharp
// For money
decimal price = 19.99m;

// For general numbers
double result = 3.14;

// For integers
int count = 100;
```

✓ **Handle division by zero**
```csharp
if (divisor != 0) {
    int result = dividend / divisor;
}
```

✓ **Cast for precision**
```csharp
double average = (double)sum / count;
```

---

## Assignment Operators

✓ **Use compound operators for clarity**
```csharp
total += amount;  // Better than: total = total + amount;
value *= 2;       // Better than: value = value * 2;
```

✓ **Use null-coalescing assignment for defaults**
```csharp
cache ??= LoadCache();  // Only load if null
```

✓ **Avoid string concatenation in loops**
```csharp
// Bad
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;  // Creates 1000 strings!
}

// Good
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
string result = sb.ToString();
```

---

## Comparison Operators

✓ **Use explicit comparisons**
```csharp
// Good
if (age >= 18) { }

// Avoid implicit
if (age) { }  // Won't compile for int anyway
```

✓ **Be careful with string comparison**
```csharp
// Case-sensitive (default)
if (status == "Active") { }

// Case-insensitive
if (status.Equals("active", StringComparison.OrdinalIgnoreCase)) { }
```

✓ **Check for null before dereferencing**
```csharp
if (text != null && text.Length > 0) {
    // Safe - null check first
}
```

---

## Logical Operators

✓ **Order conditions by short-circuit efficiency**
```csharp
// Good: quick check first
if (list != null && list.Count > 0) { }

// Bad: can throw if list is null
if (list.Count > 0 && list != null) { }
```

✓ **Extract complex conditions**
```csharp
if (IsEligible(user, order)) {
    Process(order);
}

private bool IsEligible(User user, Order order) {
    return user.IsActive && 
           order.Total > 100 && 
           user.HasPermission("process");
}
```

✓ **Use parentheses for clarity**
```csharp
// Clear
if ((isAdmin || isMod) && !isDisabled) { }

// Ambiguous
if (isAdmin || isMod && !isDisabled) { }
```

---

## Bitwise Operators

✓ **Use for flags and permissions**
```csharp
[Flags]
public enum Permissions {
    Read = 1 << 0,
    Write = 1 << 1,
    Delete = 1 << 2
}

// Check permission
if ((userPerms & Permissions.Read) != 0) { }
```

✓ **Document bitwise intent**
```csharp
// Clear: set bit 3
flags |= (1 << 3);

// Or with enum
flags |= Permissions.Execute;
```

✓ **Avoid when unclear**
```csharp
// Unclear
int doubled = value << 1;

// Clear
int doubled = value * 2;
```

---

## Null Handling

✓ **Use null-conditional for safe navigation**
```csharp
string city = person?.Address?.City;
```

✓ **Use null-coalescing for defaults**
```csharp
string name = input ?? "Default";
```

✓ **Combine both**
```csharp
string email = user?.GetEmail() ?? "no-email";
```

✓ **Enable nullable reference types**
```csharp
#nullable enable

public class Person {
    public string Name { get; set; }        // Non-null
    public string? MiddleName { get; set; } // Nullable
}
```

---

## Ternary Operator

✓ **Use for simple conditions**
```csharp
string status = age >= 18 ? "Adult" : "Minor";
int discount = isVIP ? 20 : 10;
```

✓ **Avoid deep nesting**
```csharp
// Bad: hard to read
string x = a ? b ? c : d : e ? f : g;

// Good: use switch expression
string x = condition switch {
    case1 => value1,
    case2 => value2,
    _ => default
};
```

---

## Operator Precedence

✓ **Use parentheses for clarity**
```csharp
// Good
int result = (a + b) * c;

// Risky: depends on knowledge of precedence
int result = a + b * c;
```

✓ **Understand logical precedence**
```csharp
// AND before OR
if (a || b && c) { }  // Same as: a || (b && c)

// Make it explicit
if (a || (b && c)) { }
```

✓ **Use switch expressions for complex logic**
```csharp
string category = age switch {
    < 5 => "Toddler",
    < 18 => "Minor",
    < 65 => "Adult",
    _ => "Senior"
};
```

---

## Performance Considerations

✓ **Short-circuit evaluation is beneficial**
```csharp
// Expensive function only called if needed
if (QuickCheck() && ExpensiveCheck()) { }
```

✓ **Bitwise operations are fast**
```csharp
// Fast for powers of 2
int doubled = value << 1;
```

✓ **Be aware of type promotion**
```csharp
// Creates double, not int
int result = 10 / 3;  // 3
double result = 10.0 / 3;  // 3.333...
```

---

## Readability

✓ **Keep expressions simple**
```csharp
// Clear
int x = 5;
int y = x + 10;
int z = y * 2;

// Less clear
int z = (x + 10) * 2;
```

✓ **Use meaningful variable names**
```csharp
// Good
bool isEligible = age >= 18 && hasLicense;
if (isEligible) { }

// Less clear
if (age >= 18 && hasLicense) { }
```

✓ **Document operator intent**
```csharp
// Comment explains why
if (status != "Cancelled" &&  // Not cancelled
    total > 0 &&             // Has items
    !isPending) {            // Already processed
    Process();
}
```

---

## Summary Checklist

- [ ] Use appropriate numeric types (int, decimal, double)
- [ ] Handle edge cases (null, zero, overflow)
- [ ] Use compound operators (+=, -=, etc.)
- [ ] Avoid string concatenation in loops
- [ ] Check nulls before dereferencing
- [ ] Order logical conditions efficiently
- [ ] Use parentheses for clarity
- [ ] Enable nullable reference types
- [ ] Use null-conditional (?.) and null-coalescing (??)
- [ ] Keep ternary expressions simple
- [ ] Document complex operator usage
- [ ] Consider performance implications

---

## Next Steps

- Review [Common Mistakes](../02-Common-Mistakes/00-Common-Mistakes.md)
- Practice with [Interview Questions](../03-Interview-Questions/README.md)
