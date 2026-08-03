# Logical OR and NOT Operators

## Logical OR Operator (||)

### Overview

The logical OR operator (`||`) combines two boolean conditions. The result is true if **at least one** condition is true.

### Truth Table

| Condition1 | Condition2 | Result |
|-----------|-----------|--------|
| true | true | **true** |
| true | false | **true** |
| false | true | **true** |
| false | false | false |

### Basic Syntax

```csharp
bool result = condition1 || condition2;
```

### Examples

```csharp
int age = 15;
bool isStudent = true;

// Result is true if EITHER age >= 18 OR is student
if (age >= 18 || isStudent) {
    Console.WriteLine("Can enter");  // This prints
}

// String checking
string status = "pending";
if (status == "active" || status == "pending") {
    Console.WriteLine("Process request");
}

// Multiple conditions
bool hasAdmin = user.Role == "Admin";
bool hasMod = user.Role == "Moderator";
bool hasOwner = user.Role == "Owner";

if (hasAdmin || hasMod || hasOwner) {
    Console.WriteLine("Has elevated access");
}
```

### Short-Circuit Evaluation

OR uses short-circuit evaluation: if the first condition is true, the second is never evaluated.

```csharp
bool result = true || ExpensiveFunction();
// ExpensiveFunction() is NOT called (result already true)

bool result2 = false || ExpensiveFunction();
// ExpensiveFunction() IS called (needed to determine result)

// Practical example
if (isAdmin || GetPermissions().Contains("Edit")) {
    // GetPermissions() only called if not admin
}
```

**Performance Benefit**:
```csharp
// Quick exit if first condition succeeds
if (useCache || FetchFromDatabase()) {
    return result;
}
```

---

## Logical NOT Operator (!)

### Overview

The logical NOT operator (`!`) negates a boolean value. It flips true to false and false to true.

### Truth Table

| Value | Result |
|-------|--------|
| true | false |
| false | **true** |

### Basic Syntax

```csharp
bool result = !condition;
```

### Examples

```csharp
bool isActive = true;
bool isInactive = !isActive;  // false

// Negating conditions
if (!isAdmin) {
    Console.WriteLine("Not an admin");
}

// Double negation (avoid)
bool isValid = true;
bool notInvalid = !!isValid;  // Just use isValid
```

---

## Combining AND, OR, and NOT

### Complex Logic

```csharp
bool isAdmin = user.Role == "Admin";
bool isActive = user.Status == "Active";
bool isBlocked = user.IsBlocked;

// Admin OR (Active AND not blocked)
if (isAdmin || (isActive && !isBlocked)) {
    Console.WriteLine("Can proceed");
}
```

### De Morgan's Laws

Useful for simplifying complex conditions:

```csharp
// NOT (A AND B) = (NOT A) OR (NOT B)
if (!(x > 0 && y > 0)) { }
// Equivalent to:
if (x <= 0 || y <= 0) { }

// NOT (A OR B) = (NOT A) AND (NOT B)
if (!(isAdmin || isMod)) { }
// Equivalent to:
if (!isAdmin && !isMod) { }
```

---

## Practical Examples

### User Access Control

```csharp
public bool CanAccessResource(User user, Resource resource) {
    bool isOwner = user.Id == resource.OwnerId;
    bool isAdmin = user.Role == "Admin";
    bool isPublic = resource.IsPublic;
    
    // Can access if: owner OR admin OR public
    return isOwner || isAdmin || isPublic;
}
```

### Game State Check

```csharp
public bool CanStartGame(Game game) {
    bool hasPlayers = game.Players.Count >= 2;
    bool isNotRunning = game.Status != "Running";
    bool notAlreadyEnded = game.Status != "Ended";
    
    return hasPlayers && isNotRunning && notAlreadyEnded;
}
```

### Data Validation

```csharp
public bool IsValidEmail(string email) {
    bool isNotEmpty = !string.IsNullOrWhiteSpace(email);
    bool hasAtSign = email.Contains("@");
    bool hasDot = email.Contains(".");
    
    return isNotEmpty && hasAtSign && hasDot;
}
```

### Conditional Processing

```csharp
foreach (var item in items) {
    // Process if: not processed AND (approved OR expired)
    if (!item.IsProcessed && (item.IsApproved || DateTime.Now > item.ExpiryDate)) {
        ProcessItem(item);
    }
}
```

### Feature Flags

```csharp
public bool IsFeatureEnabled(string featureName) {
    bool isEnabled = Features.IsEnabled(featureName);
    bool betaUser = CurrentUser.IsBetaTester;
    bool notDisabled = !Features.IsBlacklisted(CurrentUser.Id);
    
    // Enabled if: feature is on OR (beta user AND not blacklisted)
    return isEnabled || (betaUser && notDisabled);
}
```

---

## Operator Precedence with NOT

NOT has higher precedence than AND and OR:

```csharp
bool a = true;
bool b = false;
bool c = true;

// NOT evaluated first
bool result = !a || b && c;
// Equivalent to: (!a) || (b && c)
// true || false = true

// NOT on specific part
bool result2 = !(a || b) && c;
// Different: (!(true || false)) && true = false && true = false
```

---

## Common Usage Patterns

### Checking for Invalid Conditions

```csharp
// Bad
if (value != null && value.Length > 0) {
    Console.WriteLine("Valid");
}

// Also valid with NOT
if (!(value == null || value.Length == 0)) {
    Console.WriteLine("Valid");
}
```

### Exit Early Pattern

```csharp
public void ProcessUser(User user) {
    // Exit early if invalid
    if (!IsValidUser(user) || user.IsDisabled) {
        return;
    }
    
    // Continue processing...
}
```

### Multiple Alternatives

```csharp
string status = request.Status;

if (status == "Pending" || status == "Waiting" || status == "Queued") {
    AddToQueue(request);
}

// Alternative with NOT
if (status != "Completed" && status != "Failed" && status != "Cancelled") {
    AddToQueue(request);
}
```

### Null Coalescing with Conditions

```csharp
string name = user?.Name ?? "Unknown";

// With OR
if (user?.IsActive == true || isSpecialCase) {
    ProcessUser();
}
```

---

## Best Practices

✓ **Use OR for alternatives**
```csharp
if (role == "Admin" || role == "Moderator" || role == "Manager") {
    GrantAccess();
}
```

✓ **Use NOT for negation**
```csharp
if (!isDeleted && !isArchived) {
    Console.WriteLine("Active");
}
```

✓ **Combine logically**
```csharp
if ((isAdmin || hasMod) && !isDisabled) {
    AllowAccess();
}
```

✓ **Extract complex conditions**
```csharp
bool CanProcess(Order order) {
    return !order.IsCancelled && 
           (order.IsPaid || order.IsApproved) && 
           order.Items.Count > 0;
}

if (CanProcess(order)) {
    ProcessOrder();
}
```

✓ **Use De Morgan's Laws for clarity**
```csharp
// Less clear
if (!(x > 0 && y > 0)) { }

// Clearer
if (x <= 0 || y <= 0) { }
```

---

## Common Mistakes

❌ **Using | (bitwise) instead of || (logical)**
```csharp
if (x == 5 | y == 10) {  // Bitwise OR, evaluates both
    // Both conditions always evaluated
}
```

✓ **Use logical OR**
```csharp
if (x == 5 || y == 10) {  // Logical OR, short-circuits
    // Second only evaluated if first is false
}
```

---

❌ **Double negation confusion**
```csharp
bool notNotValid = !!isValid;  // Confusing!
```

✓ **Just use the original**
```csharp
bool isValid = true;  // Clear
```

---

❌ **Wrong precedence with NOT**
```csharp
bool result = !a && b;  // Means (!a) && b
// Not a && b, but !(a && b)
```

✓ **Use parentheses for clarity**
```csharp
bool result = !(a && b);  // Clear intent
```

---

❌ **Complex nested conditions**
```csharp
if ((a || b) && (c || d) && !(e && f)) {
    // Hard to read and understand
}
```

✓ **Extract to method**
```csharp
if (MeetsRequirements(a, b, c, d, e, f)) {
    // Much clearer
}

private bool MeetsRequirements(bool a, bool b, bool c, bool d, bool e, bool f) {
    return (a || b) && (c || d) && !(e && f);
}
```

---

## Quick Reference

### OR Operator (||)

| Condition1 | Condition2 | Result |
|-----------|-----------|--------|
| true | true | true |
| true | false | true |
| false | true | true |
| false | false | false |

### NOT Operator (!)

| Value | Result |
|-------|--------|
| true | false |
| false | true |

---

## Next Steps

- Study [Bitwise Operators](../../03-Bitwise-Null/01-Bitwise/00-Bitwise-Operators.md)
- Review [Logical AND Operator](../02-Logical-AND/00-Logical-AND.md)
- Learn about [Best Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
