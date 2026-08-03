# Logical AND Operator (&&)

## Overview

The logical AND operator (`&&`) combines two boolean conditions. The result is true only if **both** conditions are true.

## Basic Syntax

```csharp
bool result = condition1 && condition2;
```

| Condition1 | Condition2 | Result |
|-----------|-----------|--------|
| true | true | **true** |
| true | false | false |
| false | true | false |
| false | false | false |

---

## Truth Table

```csharp
bool a = true;
bool b = true;
Console.WriteLine(a && b);    // true

bool c = true;
bool d = false;
Console.WriteLine(c && d);    // false

bool e = false;
bool f = true;
Console.WriteLine(e && f);    // false

bool g = false;
bool h = false;
Console.WriteLine(g && h);    // false
```

---

## Short-Circuit Evaluation

AND uses short-circuit evaluation: if the first condition is false, the second is never evaluated.

```csharp
int age = 15;
bool hasLicense = true;

// Second condition NOT evaluated
if (age >= 18 && hasLicense) {
    Console.WriteLine("Can drive");
}
// Output: (nothing - age condition is false)

// Another example
bool result = false && ExpensiveFunction();  
// ExpensiveFunction() is NOT called (saves time)

// Contrast:
bool result2 = true && ExpensiveFunction();
// ExpensiveFunction() IS called (both needed)
```

**Performance Benefit**:
```csharp
if (list != null && list.Count > 0) {
    // Second condition only checked if list is not null
    // Prevents NullReferenceException
}
```

---

## Common Use Cases

### Permission Checks
```csharp
bool isAdmin = user.Role == "Admin";
bool isActive = user.IsActive;

if (isAdmin && isActive) {
    Console.WriteLine("Has admin access");
}
```

### Range Validation
```csharp
int age = 25;
if (age >= 18 && age <= 65) {
    Console.WriteLine("Working age");
}

// Equivalent to
if (age >= 18 && age <= 65) {
    Console.WriteLine("Between 18 and 65");
}
```

### Null Checking
```csharp
string text = GetText();

// Safe: null check first
if (text != null && text.Length > 0) {
    Console.WriteLine(text);
}

// Risky: second condition might throw
if (text.Length > 0 && text != null) {
    // NullReferenceException if text is null!
}
```

### Multiple Conditions
```csharp
int score = 85;
string level = "Premium";
bool dayOfWeek = DateTime.Now.DayOfWeek == DayOfWeek.Friday;

if (score >= 80 && level == "Premium" && dayOfWeek) {
    Console.WriteLine("Bonus multiplier applied");
}
```

### Status Validation
```csharp
if (order.Status == "Ready" && order.PaymentConfirmed && !order.Shipped) {
    Console.WriteLine("Ready to ship");
}
```

---

## Practical Examples

### Login Validation
```csharp
public bool ValidateLogin(string username, string password) {
    bool isValidUsername = !string.IsNullOrEmpty(username);
    bool isValidPassword = password.Length >= 8;
    
    if (isValidUsername && isValidPassword) {
        return true;
    }
    return false;
}
```

### Product Eligibility
```csharp
public bool IsEligibleForDiscount(Order order, Customer customer) {
    bool isNewCustomer = customer.JoinDate > DateTime.Now.AddYears(-1);
    bool hasMinimumOrder = order.Total >= 50m;
    bool noActiveReturns = customer.ActiveReturns == 0;
    
    if (isNewCustomer && hasMinimumOrder && noActiveReturns) {
        return true;
    }
    return false;
}
```

### Game Logic
```csharp
public bool CanAttack(Player attacker, Player target) {
    bool inRange = Distance(attacker, target) <= 10;
    bool hasAmmo = attacker.Ammo > 0;
    bool targetAlive = target.Health > 0;
    
    return inRange && hasAmmo && targetAlive;
}
```

### Data Processing
```csharp
foreach (var item in items) {
    bool isActive = item.Status == "Active";
    bool isRecent = item.DateCreated > DateTime.Now.AddDays(-30);
    bool isValidated = item.ValidationStatus == "Passed";
    
    if (isActive && isRecent && isValidated) {
        ProcessItem(item);
    }
}
```

---

## Combining AND with Other Operators

### AND with OR
```csharp
int age = 25;
bool hasLicense = true;
bool hasPlatinum = false;

// Can drive if: (licensed AND 18+) OR (platinum member)
if ((age >= 18 && hasLicense) || hasPlatinum) {
    Console.WriteLine("Can drive");
}
```

### AND with NOT
```csharp
bool isAdmin = false;
bool isActive = true;

if (isAdmin && !isActive) {
    Console.WriteLine("Inactive admin");
}
```

### Multiple ANDs
```csharp
if (condition1 && condition2 && condition3 && condition4) {
    Console.WriteLine("All conditions met");
}
```

---

## Performance Optimization

Short-circuit evaluation saves CPU:

```csharp
// Bad: checks all conditions
if (CheckA() && CheckB() && CheckC()) {
    // Even if CheckA() is false, CheckB() and CheckC() run
}

// Good: most likely false first
if (QuickCheck() && ExpensiveCheck()) {
    // ExpensiveCheck() only runs if QuickCheck() is true
}

// Best: order by likelihood
if (LeastLikelyTrue() && MoreLikelyTrue() && MostLikelyTrue()) {
    // Short-circuits early if first fails
}
```

---

## Pattern Matching with AND

C# 9+ supports pattern matching:

```csharp
int age = 25;

// Old way
if (age > 18 && age < 65) {
    Console.WriteLine("Working age");
}

// Pattern matching (C# 9+)
if (age is > 18 and < 65) {
    Console.WriteLine("Working age");
}
```

---

## Best Practices

✓ **Order conditions by short-circuit efficiency**
```csharp
// Good: quick check first
if (list != null && list.Count > 0) {
    // ...
}

// Bad: slower operation first
if (list.Count > 0 && list != null) {
    // NullReferenceException if list is null!
}
```

✓ **Use for complex validation**
```csharp
if (IsValidUser() && HasPermission() && DataExists()) {
    ProcessData();
}
```

✓ **Combine with other operators clearly**
```csharp
if ((age >= 18 && hasLicense) || isInstructor) {
    Console.WriteLine("Can drive");
}
```

✓ **Extract complex conditions to methods**
```csharp
if (IsEligibleForDiscount(customer)) {
    ApplyDiscount();
}

private bool IsEligibleForDiscount(Customer customer) {
    return customer.IsActive && 
           customer.PurchaseTotal > 100 && 
           customer.MembershipYears > 1;
}
```

---

## Common Mistakes

❌ **Using & (bitwise) instead of && (logical)**
```csharp
if (x = 5 & y = 10) {  // Bitwise AND, slower
    Console.WriteLine("Both conditions");
}
```

✓ **Use logical AND**
```csharp
if (x == 5 && y == 10) {  // Logical AND
    Console.WriteLine("Both conditions");
}
```

---

❌ **Wrong order causing errors**
```csharp
if (text.Length > 0 && text != null) {
    // NullReferenceException if text is null!
}
```

✓ **Null check first**
```csharp
if (text != null && text.Length > 0) {
    // Safe: text is guaranteed not null
}
```

---

❌ **Expecting different short-circuit behavior**
```csharp
if (IsValid() && Process()) {
    // Process() WILL be called if IsValid() is true
}
```

✓ **Understand short-circuit**
```csharp
// Second condition only evaluated if first is true
if (RequiredCheck() && OptionalCheck()) {
    // ...
}
```

---

## Quick Reference

| Expression | Result |
|-----------|--------|
| true && true | true |
| true && false | false |
| false && true | false |
| false && false | false |
| true && ExpensiveFunc() | Calls ExpensiveFunc() |
| false && ExpensiveFunc() | Doesn't call ExpensiveFunc() |

---

## Next Steps

- Study [Logical OR Operator](../03-Logical-OR-NOT/00-Logical-OR-NOT.md)
- Review [Comparison Operators](../01-Comparison/00-Comparison-Operators.md)
- Learn about [Best Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
