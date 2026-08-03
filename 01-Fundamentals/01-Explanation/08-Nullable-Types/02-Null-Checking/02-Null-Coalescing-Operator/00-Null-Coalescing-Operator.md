# Null-Coalescing Operator (??)

## Overview
The `??` operator provides a default value when the left operand is null. Concise and readable.

---

## Basic Syntax

### Simple Default
```csharp
int? age = null;
int result = age ?? 18;  // 18 (age is null, use default)

int? score = 95;
int result2 = score ?? 0;  // 95 (score has value, use it)

string name = null;
string display = name ?? "Unknown";  // "Unknown"
```

### With Operators
```csharp
// Assignment
int? value = null;
int final = value ?? 100;

// In method calls
Print(name ?? "Guest");

// In expressions
int total = (first ?? 0) + (second ?? 0);
```

---

## Chaining Null-Coalescing

### Multiple Fallbacks
```csharp
int? first = null;
int? second = null;
int? third = 25;

// Use first non-null value
int result = first ?? second ?? third ?? 0;  // 25

// Real example
string primary = GetPrimary();
string secondary = GetSecondary();
string display = primary ?? secondary ?? "N/A";
```

### Complex Chains
```csharp
string? email = user?.Email;
string? phone = user?.Phone;
string? backup = contact?.Email;

string contact = email ?? phone ?? backup ?? "No contact";
```

---

## Null-Coalescing Assignment (??=)

### Assign Only If Null
```csharp
int? x = null;
x ??= 10;  // x becomes 10
Console.WriteLine(x);  // 10

int? y = 5;
y ??= 20;  // y stays 5 (not null)
Console.WriteLine(y);  // 5
```

### With References
```csharp
string name = null;
name ??= "Default";  // name becomes "Default"

string city = "NYC";
city ??= "Unknown";  // city stays "NYC"
```

### Dictionary/Collection Pattern
```csharp
Dictionary<string, int> cache = new();

// Initialize if not present
cache["count"] ??= 0;  // Add if missing
cache["count"]++;

// Or safely
if (!cache.ContainsKey("count")) {
    cache["count"] = 0;
}
```

---

## Use Cases

### Database Defaults
```csharp
// Get value or use application default
int? dbTimeout = GetDatabaseTimeout();
int timeout = dbTimeout ?? 5000;  // 5 seconds default
```

### Optional Parameters
```csharp
public void CreateUser(string name, string? email = null) {
    email ??= $"{name}@example.com";  // Default email
}
```

### Fallback Values
```csharp
// Try multiple sources
string displayName = 
    user?.FullName ?? 
    user?.Email ?? 
    $"User_{user?.Id}" ?? 
    "Unknown";
```

### Configuration
```csharp
public class Config {
    public int? MaxConnections { get; set; }
    public int? Timeout { get; set; }
}

var config = LoadConfig();
int maxConn = config.MaxConnections ?? 10;
int timeout = config.Timeout ?? 5000;
```

---

## Comparison with Other Approaches

### vs Ternary Operator
```csharp
int? value = null;

// Null-coalescing (clear)
int result1 = value ?? 0;

// Ternary (more verbose)
int result2 = value != null ? value.Value : 0;

// Null-coalescing is more readable for this case
```

### vs HasValue Check
```csharp
int? value = null;

// Null-coalescing (concise)
int result1 = value ?? 0;

// HasValue check (explicit)
int result2 = value.HasValue ? value.Value : 0;

// Both work, ?? is simpler for defaults
```

### vs GetValueOrDefault
```csharp
int? value = null;

// Null-coalescing
int result1 = value ?? 0;

// GetValueOrDefault
int result2 = value.GetValueOrDefault(0);

// Both equivalent, ?? is more common
```

---

## Common Patterns

### Chained Defaults
```csharp
int? priority = userInput ?? systemDefault ?? configDefault ?? 1;
```

### Fallback Chain
```csharp
string location = 
    user?.WorkCity ?? 
    user?.HomeCity ?? 
    organization?.City ?? 
    "Unknown";
```

### Safe Arithmetic
```csharp
int? x = GetX();
int? y = GetY();

int sum = (x ?? 0) + (y ?? 0);
```

### Conditional Initialization
```csharp
int? threshold = config?.Threshold;
threshold ??= 100;  // Default to 100 if not configured
```

---

## Performance Note

### No Performance Cost
```csharp
// Compiled efficiently
int result = value ?? default;

// Similar performance to manual checks
// Compiler optimizes appropriately
```

---

## Summary

✓ `??` provides default if left is null
✓ `??=` assigns only if null
✓ Chain multiple ?? for fallbacks
✓ More readable than ternary for this case
✓ Works with any type
✓ Clean and idiomatic C#

---

## Next Steps

1. Study Null-Conditional Operator
2. Learn Pattern Matching
3. Master Real-World Scenarios
