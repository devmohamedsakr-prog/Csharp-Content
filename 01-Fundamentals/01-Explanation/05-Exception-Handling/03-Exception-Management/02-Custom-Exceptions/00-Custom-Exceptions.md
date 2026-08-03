# Custom Exceptions

## Overview
Custom exceptions allow you to create domain-specific exception types that represent errors in your application. They provide better error handling and clearer intent.

## Creating a Simple Custom Exception

### Basic Custom Exception
```csharp
public class InvalidUserException : Exception {
    public InvalidUserException(string message) : base(message) { }
}

// Usage
public void ValidateUser(User user) {
    if (user == null) {
        throw new InvalidUserException("User cannot be null");
    }
}

try {
    ValidateUser(null);
} catch (InvalidUserException ex) {
    Console.WriteLine(ex.Message);
}
```

### With Constructor Chaining
```csharp
public class DatabaseException : Exception {
    public DatabaseException(string message) : base(message) { }
    
    public DatabaseException(string message, Exception innerException)
        : base(message, innerException) { }
}

// Usage - preserve inner exception
try {
    database.Connect();
} catch (SqlException ex) {
    throw new DatabaseException("Failed to connect to database", ex);
}
```

## Custom Properties

Add domain-specific properties:

```csharp
public class InsufficientFundsException : Exception {
    public decimal RequiredAmount { get; }
    public decimal AvailableAmount { get; }
    
    public InsufficientFundsException(
        string message,
        decimal required,
        decimal available)
        : base(message) {
        RequiredAmount = required;
        AvailableAmount = available;
    }
}

// Usage
public void Withdraw(decimal amount) {
    if (amount > balance) {
        throw new InsufficientFundsException(
            "Not enough funds in account",
            amount,
            balance
        );
    }
}

try {
    account.Withdraw(1000);
} catch (InsufficientFundsException ex) {
    Console.WriteLine($"Need: {ex.RequiredAmount}");
    Console.WriteLine($"Have: {ex.AvailableAmount}");
}
```

## Exception Hierarchy

Create domain-specific hierarchy:

```csharp
// Base exception for your application
public class MyApplicationException : Exception {
    public MyApplicationException(string message) : base(message) { }
    
    public MyApplicationException(string message, Exception innerException)
        : base(message, innerException) { }
}

// Domain-specific exceptions
public class ValidationException : MyApplicationException {
    public string FieldName { get; }
    
    public ValidationException(string fieldName, string message)
        : base(message) {
        FieldName = fieldName;
    }
}

public class DataAccessException : MyApplicationException {
    public DataAccessException(string message, Exception innerException)
        : base(message, innerException) { }
}

public class BusinessRuleException : MyApplicationException {
    public string RuleName { get; }
    
    public BusinessRuleException(string ruleName, string message)
        : base(message) {
        RuleName = ruleName;
    }
}

// Usage
try {
    ValidateUser(user);
    SaveToDatabase(user);
    ApplyBusinessRules(user);
} catch (ValidationException ex) {
    Console.WriteLine($"Field {ex.FieldName} is invalid");
} catch (DataAccessException ex) {
    Console.WriteLine($"Database error: {ex.Message}");
} catch (BusinessRuleException ex) {
    Console.WriteLine($"Rule violation - {ex.RuleName}");
}
```

## Rich Exception Information

Include detailed information:

```csharp
public class OrderException : Exception {
    public string OrderId { get; }
    public DateTime OccurredAt { get; }
    public string ErrorCode { get; }
    public Dictionary<string, object> Context { get; }
    
    public OrderException(
        string orderId,
        string errorCode,
        string message,
        Dictionary<string, object> context = null)
        : base(message) {
        OrderId = orderId;
        ErrorCode = errorCode;
        Context = context ?? new Dictionary<string, object>();
        OccurredAt = DateTime.UtcNow;
    }
}

// Usage
try {
    ProcessOrder(order);
} catch (OrderException ex) {
    logger.Error(
        $"Order {ex.OrderId} failed with code {ex.ErrorCode}",
        ex
    );
}
```

## Serializable Custom Exception

For distributed scenarios:

```csharp
[Serializable]
public class RemoteException : Exception {
    public string ServiceName { get; }
    
    public RemoteException(string serviceName, string message)
        : base(message) {
        ServiceName = serviceName;
    }
    
    protected RemoteException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        : base(info, context) {
        ServiceName = info.GetString("ServiceName");
    }
    
    public override void GetObjectData(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) {
        base.GetObjectData(info, context);
        info.AddValue("ServiceName", ServiceName);
    }
}
```

## Validation Exception with Multiple Errors

```csharp
public class ValidationException : Exception {
    public List<ValidationError> Errors { get; }
    
    public ValidationException(List<ValidationError> errors)
        : base(FormatMessage(errors)) {
        Errors = errors;
    }
    
    private static string FormatMessage(List<ValidationError> errors) {
        var messages = string.Join("; ", 
            errors.Select(e => $"{e.Field}: {e.Message}"));
        return $"Validation failed: {messages}";
    }
}

public class ValidationError {
    public string Field { get; set; }
    public string Message { get; set; }
}

// Usage
public void ValidateUser(User user) {
    var errors = new List<ValidationError>();
    
    if (string.IsNullOrEmpty(user.Name)) {
        errors.Add(new ValidationError { Field = "Name", Message = "Required" });
    }
    
    if (user.Age < 18) {
        errors.Add(new ValidationError { Field = "Age", Message = "Must be 18+" });
    }
    
    if (errors.Count > 0) {
        throw new ValidationException(errors);
    }
}

try {
    ValidateUser(user);
} catch (ValidationException ex) {
    foreach (var error in ex.Errors) {
        Console.WriteLine($"{error.Field}: {error.Message}");
    }
}
```

## Generic Custom Exception

For reusable custom exceptions:

```csharp
public class ResourceNotFoundException<T> : Exception {
    public object ResourceId { get; }
    public ResourceNotFoundException(object id)
        : base($"{typeof(T).Name} with id {id} not found") {
        ResourceId = id;
    }
}

// Usage
try {
    var user = repository.GetById(userId);
    if (user == null) {
        throw new ResourceNotFoundException<User>(userId);
    }
} catch (ResourceNotFoundException<User> ex) {
    Console.WriteLine($"User not found: {ex.ResourceId}");
}
```

## Exception with Recovery Information

```csharp
public class RecoverableException : Exception {
    public bool IsRecoverable { get; }
    public Action RecoveryAction { get; }
    
    public RecoverableException(
        string message,
        bool isRecoverable = true,
        Action recoveryAction = null)
        : base(message) {
        IsRecoverable = isRecoverable;
        RecoveryAction = recoveryAction;
    }
}

// Usage
try {
    operation();
} catch (RecoverableException ex) {
    if (ex.IsRecoverable) {
        ex.RecoveryAction?.Invoke();
    } else {
        throw;
    }
}
```

## Documentation for Custom Exceptions

Always document what exceptions methods throw:

```csharp
/// <summary>
/// Validates and saves the user to the database.
/// </summary>
/// <param name="user">The user to save.</param>
/// <exception cref="ArgumentNullException">user is null</exception>
/// <exception cref="ValidationException">User data is invalid</exception>
/// <exception cref="DataAccessException">Database operation failed</exception>
public void SaveUser(User user) {
    if (user == null) {
        throw new ArgumentNullException(nameof(user));
    }
    
    ValidateUser(user);
    
    try {
        database.Save(user);
    } catch (SqlException ex) {
        throw new DataAccessException("Failed to save user", ex);
    }
}
```

## Best Practices for Custom Exceptions

✓ Inherit from Exception (or specific subclass)
```csharp
public class MyException : Exception { }
```

✓ Provide constructors with message and inner exception
```csharp
public MyException(string message) : base(message) { }
public MyException(string message, Exception inner) 
    : base(message, inner) { }
```

✓ Add domain-specific properties
```csharp
public string UserId { get; }
public string OrderId { get; }
```

✓ Create exception hierarchy
```csharp
public class DomainException : Exception { }
public class ValidationException : DomainException { }
```

✓ Document exceptions thrown
```csharp
/// <exception cref="ValidationException">Invalid input</exception>
```

## Anti-Patterns

❌ Generic Exception
```csharp
throw new Exception("Error");  // Not specific enough
```

❌ Catching and wrapping unnecessarily
```csharp
try {
    operation();
} catch (Exception ex) {
    throw new MyException(ex.Message, ex);  // Just throw directly
}
```

❌ No message
```csharp
throw new MyException();  // Always provide message
```

❌ Losing inner exception
```csharp
throw new MyException("Error");  // If wrapping, include inner exception
```

❌ Too many exception types
```csharp
// Having 50 custom exceptions is worse than generic ones
```

## Exception Naming Conventions

Always end with "Exception":

```csharp
// Good
public class PaymentException : Exception { }
public class ValidationException : Exception { }
public class DatabaseException : Exception { }

// Bad
public class PaymentError : Exception { }
public class InvalidData : Exception { }
```

## Real-World Example

```csharp
public abstract class DomainException : Exception {
    public string ErrorCode { get; protected set; }
    public DateTime OccurredAt { get; protected set; }
    
    protected DomainException(string message, string errorCode)
        : base(message) {
        ErrorCode = errorCode;
        OccurredAt = DateTime.UtcNow;
    }
}

public class UserValidationException : DomainException {
    public string Field { get; }
    
    public UserValidationException(string field, string message)
        : base(message, $"USER_VALIDATION_{field.ToUpper()}") {
        Field = field;
    }
}

public class PaymentProcessingException : DomainException {
    public string TransactionId { get; }
    public decimal Amount { get; }
    
    public PaymentProcessingException(
        string transactionId,
        decimal amount,
        string message)
        : base(message, "PAYMENT_PROCESSING_FAILED") {
        TransactionId = transactionId;
        Amount = amount;
    }
}

public class InsufficientInventoryException : DomainException {
    public string ItemId { get; }
    public int RequestedQuantity { get; }
    public int AvailableQuantity { get; }
    
    public InsufficientInventoryException(
        string itemId,
        int requested,
        int available)
        : base(
            $"Insufficient inventory for item {itemId}",
            "INSUFFICIENT_INVENTORY") {
        ItemId = itemId;
        RequestedQuantity = requested;
        AvailableQuantity = available;
    }
}

// Usage
try {
    ValidateUser(user);
    ProcessPayment(order);
    ReserveInventory(items);
} catch (UserValidationException ex) {
    logger.Error($"User validation failed: {ex.Field} - {ex.Message}");
    return BadRequest(ex.Message);
} catch (PaymentProcessingException ex) {
    logger.Error($"Payment failed: {ex.TransactionId} - {ex.Amount}");
    return StatusCode(402, ex.Message);
} catch (InsufficientInventoryException ex) {
    logger.Warn($"Inventory low: {ex.ItemId} - {ex.AvailableQuantity} available");
    return StatusCode(409, ex.Message);
} catch (DomainException ex) {
    logger.Error($"Domain error ({ex.ErrorCode}): {ex.Message}");
    return StatusCode(500, "An error occurred");
}
```

## Summary

- Create custom exceptions for domain-specific errors
- Inherit from Exception or specific subclass
- Include meaningful message and optional inner exception
- Add domain-specific properties for context
- Create exception hierarchies for related errors
- Document exceptions thrown by methods
- Use proper naming convention (end with "Exception")

---

## Next Steps

1. Learn Exception Properties
2. Master Resource Management
3. Study Guard Clauses
4. Learn Best Practices
