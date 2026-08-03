# Throwing Exceptions

## Overview
Throwing exceptions is how you signal error conditions to the caller. Understanding when and how to throw exceptions is crucial for building robust code.

## Basic Throw

```csharp
throw new ExceptionType("error message");
```

## When to Throw Exceptions

### 1. Invalid Input/State
```csharp
public void SetAge(int age) {
    if (age < 0) {
        throw new ArgumentException("Age cannot be negative");
    }
}
```

### 2. Precondition Violation
```csharp
public void ProcessData(string data) {
    if (data == null) {
        throw new ArgumentNullException(nameof(data));
    }
}
```

### 3. Operation Failure
```csharp
public void Save() {
    if (!database.IsConnected) {
        throw new InvalidOperationException("Database not connected");
    }
}
```

### 4. Business Rule Violation
```csharp
public void Withdraw(decimal amount) {
    if (amount > balance) {
        throw new InvalidOperationException("Insufficient funds");
    }
}
```

## Throw with Message

Always provide descriptive message:

```csharp
// BAD - No message
throw new Exception();

// GOOD - Descriptive message
throw new ArgumentException("User ID must be positive", nameof(userId));

// GOOD - Include context
throw new InvalidOperationException($"Cannot process {status} request");
```

## Built-in Exception Types

### ArgumentException (Invalid Argument)
```csharp
public void SetPercentage(int value) {
    if (value < 0 || value > 100) {
        throw new ArgumentException(
            "Percentage must be between 0 and 100",
            nameof(value)
        );
    }
}

try {
    SetPercentage(150);
} catch (ArgumentException ex) {
    Console.WriteLine($"Parameter: {ex.ParamName}");
}
```

### ArgumentNullException (Null Argument)
```csharp
public void ProcessUser(User user) {
    if (user == null) {
        throw new ArgumentNullException(nameof(user), "User cannot be null");
    }
}

try {
    ProcessUser(null);
} catch (ArgumentNullException ex) {
    Console.WriteLine($"Null parameter: {ex.ParamName}");
}
```

### ArgumentOutOfRangeException (Value Out of Range)
```csharp
public void SetIndex(int index) {
    if (index < 0 || index >= Size) {
        throw new ArgumentOutOfRangeException(
            nameof(index),
            index,
            $"Index must be between 0 and {Size - 1}"
        );
    }
}

try {
    SetIndex(-5);
} catch (ArgumentOutOfRangeException ex) {
    Console.WriteLine($"Invalid value: {ex.ActualValue}");
}
```

### InvalidOperationException (Invalid State)
```csharp
public void ReadFile() {
    if (stream == null) {
        throw new InvalidOperationException("Stream not initialized");
    }
}
```

### NotImplementedException (Feature Not Implemented)
```csharp
public virtual void VirtualMethod() {
    throw new NotImplementedException("Subclass must implement");
}
```

### NotSupportedException (Feature Not Supported)
```csharp
public void SaveToFormat(string format) {
    if (format != "json") {
        throw new NotSupportedException($"Format {format} not supported");
    }
}
```

## Guard Clauses

Check preconditions early and throw immediately:

```csharp
// Good - fail fast with guard clauses
public void ProcessOrder(Order order) {
    if (order == null) {
        throw new ArgumentNullException(nameof(order));
    }
    
    if (order.Items.Count == 0) {
        throw new InvalidOperationException("Order has no items");
    }
    
    if (order.Total < 0) {
        throw new InvalidOperationException("Order total cannot be negative");
    }
    
    // Safe to process order
    ProcessPayment(order);
    ShipOrder(order);
}

// Avoid - nested conditions make it hard to read
public void ProcessOrderBad(Order order) {
    if (order != null) {
        if (order.Items.Count > 0) {
            if (order.Total >= 0) {
                ProcessPayment(order);
                ShipOrder(order);
            }
        }
    }
}
```

## Throwing Custom Exceptions

```csharp
public class InsufficientFundsException : Exception {
    public decimal Amount { get; set; }
    public decimal Available { get; set; }
    
    public InsufficientFundsException(string message, 
        decimal amount, decimal available) : base(message) {
        Amount = amount;
        Available = available;
    }
}

public class BankAccount {
    private decimal balance;
    
    public void Withdraw(decimal amount) {
        if (amount > balance) {
            throw new InsufficientFundsException(
                "Not enough funds",
                amount,
                balance
            );
        }
        balance -= amount;
    }
}

try {
    account.Withdraw(1000);
} catch (InsufficientFundsException ex) {
    Console.WriteLine($"Need: {ex.Amount}, Have: {ex.Available}");
}
```

## Throwing with Inner Exception

Preserve original exception as InnerException:

```csharp
try {
    database.Save();
} catch (SqlException ex) {
    // Throw new exception with original as inner
    throw new DataAccessException("Failed to save user", ex);
}

// Usage
try {
    service.SaveUser(user);
} catch (DataAccessException ex) {
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Cause: {ex.InnerException?.Message}");
}
```

## Conditional Throwing

```csharp
public void Validate(string name) {
    if (string.IsNullOrEmpty(name)) {
        throw new ArgumentException("Name cannot be empty");
    }
    
    if (name.Length > 100) {
        throw new ArgumentException("Name too long");
    }
}
```

Throw Helper Method:

```csharp
public void Validate(string name) {
    ThrowIfNull(name, nameof(name));
    ThrowIf(name.Length > 100, "Name too long");
}

private void ThrowIfNull<T>(T value, string paramName) where T : class {
    if (value == null) {
        throw new ArgumentNullException(paramName);
    }
}

private void ThrowIf(bool condition, string message) {
    if (condition) {
        throw new ArgumentException(message);
    }
}
```

## Throw Expression (C# 7.0+)

Throw in expressions using `throw`:

```csharp
// Traditional
public string GetUserName(User user) {
    if (user == null) {
        throw new ArgumentNullException(nameof(user));
    }
    return user.Name;
}

// Using throw expression
public string GetUserName(User user) => 
    user ?? throw new ArgumentNullException(nameof(user));

// In ternary
public string GetStatus(int age) => 
    age >= 18 ? "Adult" : throw new ArgumentException("Too young");

// In method body
public void Configure(string setting) =>
    setting ?? throw new ArgumentNullException(nameof(setting));
```

## Throwing Based on Condition

```csharp
public void Process(int value) {
    var result = value switch {
        < 0 => throw new ArgumentException("Value cannot be negative"),
        > 100 => throw new ArgumentException("Value too large"),
        _ => ProcessValid(value)
    };
}
```

## Exception Message Best Practices

✓ Descriptive and specific

```csharp
// BAD
throw new Exception("Error");

// GOOD
throw new ArgumentException(
    $"User age must be between 18 and 120, got {age}",
    nameof(age)
);
```

✓ Include context

```csharp
// BAD
throw new InvalidOperationException("Cannot process");

// GOOD
throw new InvalidOperationException(
    $"Cannot process order {order.Id} in {order.Status} status"
);
```

✓ Include parameter names

```csharp
throw new ArgumentNullException(nameof(user));
throw new ArgumentOutOfRangeException(nameof(age), age, "Invalid age");
```

## Throwing Multiple Exceptions

Sequential validation with multiple potential exceptions:

```csharp
public void ValidateUser(User user) {
    var errors = new List<string>();
    
    if (user == null) {
        errors.Add("User is null");
    } else {
        if (string.IsNullOrEmpty(user.Name)) {
            errors.Add("Name is required");
        }
        if (user.Age < 0) {
            errors.Add("Age cannot be negative");
        }
        if (string.IsNullOrEmpty(user.Email)) {
            errors.Add("Email is required");
        }
    }
    
    if (errors.Count > 0) {
        throw new ValidationException(
            string.Join("; ", errors)
        );
    }
}
```

## Custom Exception for Domain

```csharp
public abstract class DomainException : Exception {
    protected DomainException(string message) : base(message) { }
}

public class PaymentException : DomainException {
    public string PaymentId { get; set; }
    
    public PaymentException(string paymentId, string message) 
        : base(message) {
        PaymentId = paymentId;
    }
}

public class OrderException : DomainException {
    public string OrderId { get; set; }
    
    public OrderException(string orderId, string message)
        : base(message) {
        OrderId = orderId;
    }
}
```

## Best Practices

✓ Throw as specific exception as possible
```csharp
throw new ArgumentNullException(nameof(user));  // Specific
```

✓ Include parameter names
```csharp
throw new ArgumentException("Invalid value", nameof(value));
```

✓ Preserve inner exceptions
```csharp
throw new DataAccessException("Save failed", ex);
```

✓ Fail fast with guard clauses
```csharp
if (value == null) throw new ArgumentNullException(...);
// Safe to use value here
```

✓ Use throw expressions
```csharp
public string Name => name ?? throw new InvalidOperationException();
```

## Anti-Patterns

❌ Generic exceptions
```csharp
throw new Exception("Error");  // Too generic
```

❌ Swallowing and re-throwing without value
```csharp
catch { throw; }  // Don't catch just to re-throw
```

❌ Throwing Exception base
```csharp
throw new Exception();  // Always be specific
```

❌ Losing inner exception
```csharp
throw new Exception("Outer");  // Lost inner exception
```

## Summary

- Throw specific exception types
- Include descriptive messages
- Use throw expressions (C# 7+)
- Fail fast with guard clauses
- Preserve inner exceptions
- Document what exceptions methods throw

---

## Next Steps

1. Learn Custom Exceptions
2. Master Exception Properties
3. Study Resource Management
4. Learn Best Practices
