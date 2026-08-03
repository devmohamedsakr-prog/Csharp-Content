# Guard Clauses

## Overview
Guard clauses are conditional statements at the beginning of a method that validate preconditions and exit early if conditions aren't met. They make code more readable and prevent exception-prone operations on invalid data.

## What is a Guard Clause

A guard clause checks a condition and throws an exception or exits early if violated:

```csharp
// Guard clause checks precondition
public void ProcessUser(User user) {
    if (user == null) {
        throw new ArgumentNullException(nameof(user));
    }
    // Safe to use user here - conditions guaranteed
    ValidateUser(user);
    SaveUser(user);
}
```

## Benefits of Guard Clauses

### 1. Fail Fast
Errors caught at entry, not deep in logic:

```csharp
// BAD - Error deep in execution
public void ProcessOrder(Order order) {
    if (order != null) {
        if (order.Items != null) {
            if (order.Items.Count > 0) {
                if (order.Total > 0) {
                    // 4 levels deep
                    Process();
                }
            }
        }
    }
}

// GOOD - Fail immediately
public void ProcessOrder(Order order) {
    if (order == null) {
        throw new ArgumentNullException(nameof(order));
    }
    if (order.Items == null || order.Items.Count == 0) {
        throw new InvalidOperationException("Order has no items");
    }
    if (order.Total <= 0) {
        throw new InvalidOperationException("Order total must be positive");
    }
    // Safe to process
    Process();
}
```

### 2. Reduced Nesting
Flat code is easier to read:

```csharp
// BAD - Deep nesting
public void ValidateUser(User user) {
    if (user != null) {
        if (!string.IsNullOrEmpty(user.Name)) {
            if (user.Age >= 18) {
                if (!string.IsNullOrEmpty(user.Email)) {
                    // Finally can validate
                }
            }
        }
    }
}

// GOOD - Guard clauses flatten structure
public void ValidateUser(User user) {
    if (user == null) throw new ArgumentNullException(nameof(user));
    if (string.IsNullOrEmpty(user.Name)) throw new ArgumentException("Name required");
    if (user.Age < 18) throw new ArgumentException("Must be 18+");
    if (string.IsNullOrEmpty(user.Email)) throw new ArgumentException("Email required");
    
    // Method is flat, easy to read
}
```

### 3. Clear Intent
Guard clauses document preconditions:

```csharp
public void DeleteUser(int userId) {
    // Preconditions documented right at start
    if (userId <= 0) {
        throw new ArgumentException("Invalid user ID");
    }
    if (!UserExists(userId)) {
        throw new ArgumentException("User not found");
    }
    
    // Safe to delete
    database.DeleteUser(userId);
}
```

## Common Guard Clause Patterns

### Null Check
```csharp
public void ProcessData(string data) {
    if (data == null) {
        throw new ArgumentNullException(nameof(data));
    }
}

// C# pattern
public void ProcessData(string data) {
    if (data is null) {
        throw new ArgumentNullException(nameof(data));
    }
}
```

### Empty Check
```csharp
public void ProcessList(List<int> items) {
    if (items == null || items.Count == 0) {
        throw new ArgumentException("Items cannot be empty");
    }
}

// Or separate clauses
public void ProcessList(List<int> items) {
    if (items == null) {
        throw new ArgumentNullException(nameof(items));
    }
    if (items.Count == 0) {
        throw new ArgumentException("Items cannot be empty");
    }
}
```

### Range Check
```csharp
public void SetAge(int age) {
    if (age < 0 || age > 150) {
        throw new ArgumentOutOfRangeException(
            nameof(age),
            $"Age must be between 0 and 150"
        );
    }
}

// Or as multiple clauses
public void SetAge(int age) {
    if (age < 0) {
        throw new ArgumentException("Age cannot be negative", nameof(age));
    }
    if (age > 150) {
        throw new ArgumentException("Age too high", nameof(age));
    }
}
```

### State Check
```csharp
public void Save() {
    if (!database.IsConnected) {
        throw new InvalidOperationException("Database not connected");
    }
}

public void ReadStream() {
    if (stream.IsClosed) {
        throw new ObjectDisposedException("Stream");
    }
}
```

### Permission Check
```csharp
public void DeleteUser(int userId, User currentUser) {
    if (currentUser == null) {
        throw new UnauthorizedAccessException("User not authenticated");
    }
    if (!currentUser.IsAdmin) {
        throw new UnauthorizedAccessException("Admin access required");
    }
}
```

## Guard Clause vs Throw Expression

### Traditional Guard Clause
```csharp
public void SetName(string name) {
    if (string.IsNullOrEmpty(name)) {
        throw new ArgumentException("Name required");
    }
}
```

### Throw Expression (C# 7+)
```csharp
// Inline throw
public string Name {
    get => name;
    set => name = value ?? throw new ArgumentNullException(nameof(value));
}

// In ternary
public void SetAge(int age) =>
    age < 0 ? throw new ArgumentException("Invalid age") : ProcessAge(age);

// With null coalescing
public string GetUserName(User user) =>
    user?.Name ?? throw new ArgumentNullException(nameof(user));
```

## Real-World Examples

### Example 1: User Registration
```csharp
public void RegisterUser(UserRegisterRequest request) {
    // Guard clauses - all preconditions checked upfront
    if (request == null) {
        throw new ArgumentNullException(nameof(request));
    }
    
    if (string.IsNullOrWhiteSpace(request.Email)) {
        throw new ArgumentException("Email is required", nameof(request.Email));
    }
    
    if (!IsValidEmail(request.Email)) {
        throw new ArgumentException("Invalid email format", nameof(request.Email));
    }
    
    if (string.IsNullOrWhiteSpace(request.Password)) {
        throw new ArgumentException("Password is required", nameof(request.Password));
    }
    
    if (request.Password.Length < 8) {
        throw new ArgumentException("Password too short", nameof(request.Password));
    }
    
    if (UserExists(request.Email)) {
        throw new InvalidOperationException("User already exists");
    }
    
    // Safe to process registration
    var user = new User {
        Email = request.Email,
        Password = HashPassword(request.Password)
    };
    
    database.SaveUser(user);
}
```

### Example 2: Order Processing
```csharp
public void PlaceOrder(OrderRequest request, User customer) {
    // Guard clauses establish all preconditions
    if (request == null) {
        throw new ArgumentNullException(nameof(request));
    }
    
    if (customer == null) {
        throw new ArgumentNullException(nameof(customer));
    }
    
    if (!customer.IsVerified) {
        throw new InvalidOperationException("Customer not verified");
    }
    
    if (request.Items == null || request.Items.Count == 0) {
        throw new InvalidOperationException("Order must contain items");
    }
    
    if (request.Total <= 0) {
        throw new InvalidOperationException("Order total must be positive");
    }
    
    if (!HasInventory(request.Items)) {
        throw new InvalidOperationException("Insufficient inventory");
    }
    
    if (!customer.HasValidPayment()) {
        throw new InvalidOperationException("No valid payment method");
    }
    
    // Safe to process order - all conditions met
    var order = CreateOrder(request, customer);
    ReserveInventory(request.Items);
    ProcessPayment(order);
    SendConfirmation(customer.Email, order);
}
```

### Example 3: API Endpoint
```csharp
public IActionResult GetUser(int userId) {
    // Guard clause for invalid input
    if (userId <= 0) {
        return BadRequest("Invalid user ID");
    }
    
    // Guard clause for missing resource
    var user = repository.GetUser(userId);
    if (user == null) {
        return NotFound();
    }
    
    // Guard clause for authorization
    if (!CurrentUser.CanView(user)) {
        return Forbid();
    }
    
    // Safe to return user
    return Ok(user);
}
```

## Guard Clause Helper Methods

Reusable guard clauses:

```csharp
public static class Guard {
    public static void NotNull<T>(T value, string paramName) where T : class {
        if (value == null) {
            throw new ArgumentNullException(paramName);
        }
    }
    
    public static void NotNullOrEmpty(string value, string paramName) {
        if (string.IsNullOrEmpty(value)) {
            throw new ArgumentException("Value cannot be null or empty", paramName);
        }
    }
    
    public static void InRange(int value, int min, int max, string paramName) {
        if (value < min || value > max) {
            throw new ArgumentOutOfRangeException(
                paramName,
                $"Value must be between {min} and {max}"
            );
        }
    }
    
    public static void True(bool condition, string message) {
        if (!condition) {
            throw new InvalidOperationException(message);
        }
    }
}

// Usage
public void SetAge(int age) {
    Guard.InRange(age, 0, 150, nameof(age));
}

public void ProcessUser(User user) {
    Guard.NotNull(user, nameof(user));
}

public void SetName(string name) {
    Guard.NotNullOrEmpty(name, nameof(name));
}
```

## Order of Guard Clauses

Put checks in logical order:

```csharp
public void ProcessUser(User user, List<Role> roles) {
    // 1. Null checks first
    if (user == null) throw new ArgumentNullException(nameof(user));
    if (roles == null) throw new ArgumentNullException(nameof(roles));
    
    // 2. Empty checks
    if (roles.Count == 0) throw new ArgumentException("At least one role required");
    
    // 3. Range/format checks
    if (user.Age < 18) throw new ArgumentException("User must be 18+");
    
    // 4. Logic checks
    if (!database.IsConnected) throw new InvalidOperationException("Not connected");
    
    // 5. Permission checks
    if (!CurrentUser.CanModify(user)) throw new UnauthorizedAccessException();
    
    // Safe to proceed
    Process();
}
```

## Best Practices

✓ Use guard clauses for all preconditions
```csharp
if (value == null) throw new ArgumentNullException(nameof(value));
```

✓ Group related guard clauses
```csharp
// Null checks
if (user == null) throw new ArgumentNullException(nameof(user));
if (data == null) throw new ArgumentNullException(nameof(data));

// Range checks
if (age < 0) throw new ArgumentException("Invalid age");
```

✓ Create reusable helper methods
```csharp
Guard.NotNull(user, nameof(user));
```

✓ Include parameter names in exceptions
```csharp
throw new ArgumentException("Invalid value", nameof(value));
```

## Anti-Patterns

❌ Guard clause deep in method
```csharp
public void Process(User user) {
    DoStuff();
    if (user == null) throw new ArgumentNullException(nameof(user));  // Should be first!
}
```

❌ Silently returning instead of throwing
```csharp
if (user == null) {
    return;  // Silent failure - should throw
}
```

❌ Nested guard clauses
```csharp
if (value != null) {
    if (value.Count > 0) {
        // Should use single check
    }
}
```

## Summary

- Guard clauses validate preconditions upfront
- Fail fast with early exits
- Reduce nesting and improve readability
- Check null, empty, range, state, permissions
- Use in correct order: null → empty → range → logic
- Create reusable helper methods
- Always throw specific exceptions with parameter names

---

## Next Steps

1. Learn IDisposable Pattern
2. Study Best Practices
3. Learn Common Mistakes
4. Master Interview Questions
