# Guard Clauses and Defensive Programming

## Overview
Guard clauses validate input early and exit quickly if conditions aren't met. Prevents NullReferenceException and improves code clarity.

---

## Basic Guard Clauses

### Early Return Pattern
```csharp
public void ProcessUser(User? user) {
    // Guard clause - exit early if null
    if (user == null) {
        return;  // Early exit
    }
    
    // Safe to use user
    ProcessUserData(user);
}

// vs nested approach (less clear)
public void ProcessUserNested(User? user) {
    if (user != null) {
        ProcessUserData(user);
    }
}
```

### Validation Guards
```csharp
public void CreateAccount(string? email, int age) {
    // Check null
    if (string.IsNullOrWhiteSpace(email)) {
        throw new ArgumentException("Email required");
    }
    
    // Check range
    if (age < 18) {
        throw new ArgumentException("Must be 18+");
    }
    
    // Safe to proceed
    SaveAccount(email, age);
}
```

---

## Null Checks

### Traditional Null Check
```csharp
public string GetUserName(User? user) {
    if (user == null) {
        return "Unknown";
    }
    
    return user.Name;
}

// Guard throws instead
public string GetUserNameStrict(User user) {
    ArgumentNullException.ThrowIfNull(user);
    return user.Name;
}
```

### Multiple Null Checks
```csharp
public void ProcessOrder(Order? order, Customer? customer) {
    if (order == null) {
        throw new ArgumentNullException(nameof(order));
    }
    
    if (customer == null) {
        throw new ArgumentNullException(nameof(customer));
    }
    
    // Safe to use both
    order.Customer = customer;
    SaveOrder(order);
}
```

---

## ArgumentNullException (C# 11+)

### New Helper Method
```csharp
public void ProcessData(Data? data) {
    ArgumentNullException.ThrowIfNull(data);
    // Safe to use data
}

// Equivalent to
public void ProcessDataOld(Data? data) {
    if (data == null) {
        throw new ArgumentNullException(nameof(data));
    }
}
```

### With Custom Messages
```csharp
ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
```

---

## Pattern Matching Guards

### Modern Syntax
```csharp
public void Process(object? obj) {
    // Guard with pattern matching
    if (obj is null) {
        return;
    }
    
    // Or
    if (obj is not null) {
        HandleObject(obj);
    }
}

// Switch expression guard
string result = obj switch {
    null => "No value",
    _ => "Has value"
};
```

---

## Summary

✓ Use guard clauses for early returns
✓ Check null before accessing members
✓ Throw exceptions for invalid states
✓ Use `ArgumentNullException` in C# 11+
✓ Pattern matching for clarity
✓ Fail fast approach

---

## Next Steps

1. Study Pattern Matching
2. Learn Real-World Scenarios
