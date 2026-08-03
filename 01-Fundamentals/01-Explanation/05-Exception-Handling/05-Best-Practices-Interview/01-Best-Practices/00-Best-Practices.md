# Exception Handling Best Practices

## 1. Catch Specific Exceptions Before General Ones

The order matters - catch derived classes before base classes.

```csharp
// GOOD - Specific to general
try {
    operation();
} catch (ArgumentNullException ex) {
    // Specific exception
} catch (ArgumentException ex) {
    // More general
} catch (SystemException ex) {
    // Even more general
} catch (Exception ex) {
    // Catch all
}

// BAD - Reversed order
try {
    operation();
} catch (Exception ex) {
    // Catches everything!
} catch (ArgumentNullException ex) {
    // Never reached
}
```

## 2. Fail Fast with Guard Clauses

Validate preconditions at method entry:

```csharp
// GOOD - Fail fast
public void ProcessUser(User user) {
    if (user == null) {
        throw new ArgumentNullException(nameof(user));
    }
    if (user.Age < 18) {
        throw new ArgumentException("Must be 18+", nameof(user));
    }
    // Safe to process
    ValidateUser(user);
    SaveUser(user);
}

// BAD - Error deep in code
public void ProcessUserBad(User user) {
    ValidateUser(user);  // Error might occur here
    SaveUser(user);
    // If null passed, error occurs later
}
```

## 3. Preserve Call Stack with 'throw;'

Use `throw;` not `throw ex;` to preserve stack trace:

```csharp
// GOOD - Preserves stack trace
try {
    operation();
} catch (Exception ex) {
    logger.Error("Operation failed", ex);
    throw;  // Original stack trace preserved
}

// BAD - Replaces stack trace
try {
    operation();
} catch (Exception ex) {
    logger.Error("Operation failed", ex);
    throw ex;  // Stack trace shows only this line
}
```

Stack trace comparison:
```
// With 'throw;'
at Program.MethodC() in C:\Program.cs:line 42
at Program.MethodB() in C:\Program.cs:line 30
at Program.MethodA() in C:\Program.cs:line 15

// With 'throw ex;'
at Program.MethodA() in C:\Program.cs:line 20
// Lost information about where it originated!
```

## 4. Never Swallow Exceptions Silently

Always log or handle exceptions, never ignore:

```csharp
// GOOD - Log the error
try {
    SaveData();
} catch (Exception ex) {
    logger.Error("Save failed", ex);
}

// GOOD - Handle appropriately
try {
    SaveData();
} catch (ArgumentException) {
    Console.WriteLine("Invalid input");
} catch (Exception ex) {
    logger.Error("Unexpected error", ex);
    throw;
}

// BAD - Silent failure
try {
    SaveData();
} catch {
    // What happened? No one knows!
}
```

## 5. Use try-Finally or Using for Resource Cleanup

Always ensure resources are cleaned up:

```csharp
// GOOD - Using statement (C# 8+)
using StreamReader reader = new StreamReader("file.txt");
string content = reader.ReadToEnd();

// GOOD - Using block
using (var reader = new StreamReader("file.txt")) {
    string content = reader.ReadToEnd();
}

// GOOD - Try-finally
StreamReader reader = null;
try {
    reader = new StreamReader("file.txt");
    string content = reader.ReadToEnd();
} finally {
    reader?.Dispose();
}

// BAD - Resource leaks
StreamReader reader = new StreamReader("file.txt");
string content = reader.ReadToEnd();
// If exception occurs, reader never closes!
```

## 6. Throw Specific Exception Types

Use the most specific exception type:

```csharp
// GOOD - Specific types
if (user == null) {
    throw new ArgumentNullException(nameof(user));
}

if (age < 0) {
    throw new ArgumentOutOfRangeException(nameof(age));
}

if (!database.IsConnected) {
    throw new InvalidOperationException("Database not connected");
}

// BAD - Generic exception
throw new Exception("Error");  // Not specific
```

## 7. Include Context in Exception Messages

Provide meaningful information for debugging:

```csharp
// GOOD - Context included
if (age < 0 || age > 150) {
    throw new ArgumentException(
        $"Age must be between 0 and 150, got {age}",
        nameof(age)
    );
}

// GOOD - Include IDs for tracing
throw new DataAccessException(
    $"Failed to save user {userId} to database"
);

// BAD - No context
throw new ArgumentException("Invalid");

// BAD - Generic message
throw new Exception("Error in operation");
```

## 8. Use TryParse for Expected Failures

Don't use exceptions for expected input validation failures:

```csharp
// GOOD - Expected failure (user input)
if (!int.TryParse(userInput, out int number)) {
    Console.WriteLine("Please enter a valid number");
}

// BAD - Exception for expected failure
try {
    int number = int.Parse(userInput);
} catch (FormatException) {
    Console.WriteLine("Please enter a valid number");
}

// GOOD - Exception for unexpected failure
try {
    database.Save();  // Unexpected failure
} catch (DatabaseException ex) {
    logger.Error("Database error", ex);
    throw;
}
```

## 9. Wrap External Exceptions with Domain Exceptions

Translate exceptions into domain language:

```csharp
public class PaymentService {
    public void ProcessPayment(Order order) {
        try {
            paymentGateway.Charge(order.Total);
        } catch (PaymentGatewayException ex) {
            throw new PaymentException(
                $"Failed to process payment for order {order.Id}",
                ex
            );
        } catch (TimeoutException ex) {
            throw new PaymentException(
                "Payment service timeout",
                ex
            );
        }
    }
}

// Usage
try {
    paymentService.ProcessPayment(order);
} catch (PaymentException ex) {
    // Handle domain exception
}
```

## 10. Document Exceptions Thrown

Use XML documentation to document exceptions:

```csharp
/// <summary>
/// Withdraws money from the account.
/// </summary>
/// <param name="amount">Amount to withdraw.</param>
/// <exception cref="ArgumentException">Amount must be positive.</exception>
/// <exception cref="InvalidOperationException">Insufficient funds.</exception>
public void Withdraw(decimal amount) {
    if (amount <= 0) {
        throw new ArgumentException("Amount must be positive");
    }
    
    if (amount > balance) {
        throw new InvalidOperationException("Insufficient funds");
    }
    
    balance -= amount;
}
```

## 11. Check for Disposed Objects

Always check if object is disposed before use:

```csharp
public class Resource : IDisposable {
    private bool disposed = false;
    
    public void DoWork() {
        if (disposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
        // Safe to use
    }
    
    public void Dispose() {
        disposed = true;
    }
}
```

## 12. Use When Clauses for Exception Filtering

Filter exceptions with conditions:

```csharp
try {
    file.Open();
} catch (IOException ex) when (IsFileLocked(ex)) {
    RetryLater();
} catch (IOException ex) when (IsAccessDenied(ex)) {
    LogAccessDenied();
} catch (IOException ex) {
    throw;  // Other IO errors
}

private bool IsFileLocked(IOException ex) {
    return ex.HResult == -2147024891;
}

private bool IsAccessDenied(IOException ex) {
    return ex.HResult == -2147024897;
}
```

## 13. Create Exception Hierarchies

Organize custom exceptions hierarchically:

```csharp
// Base exception
public class DomainException : Exception {
    public DomainException(string message) : base(message) { }
}

// Domain-specific exceptions
public class ValidationException : DomainException {
    public ValidationException(string message) : base(message) { }
}

public class BusinessRuleException : DomainException {
    public BusinessRuleException(string message) : base(message) { }
}

public class DataAccessException : DomainException {
    public DataAccessException(string message, Exception inner)
        : base(message, inner) { }
}

// Usage
try {
    ValidateUser(user);
    SaveUser(user);
} catch (ValidationException) {
    // Handle validation
} catch (BusinessRuleException) {
    // Handle business rule
} catch (DataAccessException) {
    // Handle data access
} catch (DomainException) {
    // Handle other domain exceptions
}
```

## 14. Log with Proper Severity Levels

Log exceptions at appropriate levels:

```csharp
try {
    operation();
} catch (ValidationException ex) {
    // User error - info level
    logger.Info($"Validation failed: {ex.Message}");
} catch (InvalidOperationException ex) {
    // System state error - warning level
    logger.Warn($"Invalid state: {ex.Message}");
} catch (DatabaseException ex) {
    // System error - error level
    logger.Error("Database error", ex);
} catch (Exception ex) {
    // Unexpected - critical level
    logger.Critical("Unexpected error", ex);
}
```

## 15. Use Cancellation Tokens for Long Operations

Allow cancellation of long operations gracefully:

```csharp
public async Task ProcessLargeDataSet(CancellationToken cancellationToken) {
    try {
        for (int i = 0; i < items.Count; i++) {
            cancellationToken.ThrowIfCancellationRequested();
            await ProcessItem(items[i]);
        }
    } catch (OperationCanceledException) {
        logger.Info("Operation cancelled by user");
        // Clean up
    }
}
```

## Summary of Best Practices

| Practice | Why | How |
|----------|-----|-----|
| Specific exceptions first | Correct handling | catch derived before base |
| Guard clauses | Early validation | Check preconditions first |
| Preserve stack trace | Debugging | Use `throw;` not `throw ex;` |
| Never swallow exceptions | Error visibility | Log or handle all exceptions |
| Use finally/using | Resource cleanup | Always clean up resources |
| Specific exception types | Clear intent | Don't use generic Exception |
| Include context | Debugging | Add relevant info to messages |
| Use TryParse | Performance | For expected failures |
| Wrap exceptions | Abstraction | Domain-specific exceptions |
| Document exceptions | API clarity | XML documentation |
| Check disposed state | Correctness | Throw ObjectDisposedException |
| Use when clauses | Filtering | Conditional exception handling |
| Exception hierarchies | Organization | Clear exception structure |
| Proper logging levels | Log analysis | Match severity to error |
| Cancellation tokens | Responsiveness | Allow operation cancellation |

---

## Next Steps

1. Learn Common Mistakes
2. Study Interview Questions
3. Practice Patterns
4. Review Real-World Examples
