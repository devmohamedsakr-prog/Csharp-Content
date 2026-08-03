# Exception Properties

## Overview
Exception objects contain rich information about errors. Understanding and using these properties helps with debugging and error handling.

## Core Exception Properties

### Message
Human-readable description of the exception.

```csharp
try {
    int result = int.Parse("abc");
} catch (FormatException ex) {
    Console.WriteLine(ex.Message);
    // Output: Input string was not in a correct format.
}
```

### StackTrace
Shows where the exception occurred and the call chain.

```csharp
try {
    MethodA();
} catch (Exception ex) {
    Console.WriteLine(ex.StackTrace);
    // at Program.MethodC() in C:\Program.cs:line 42
    // at Program.MethodB() in C:\Program.cs:line 30
    // at Program.MethodA() in C:\Program.cs:line 15
}

static void MethodA() => MethodB();
static void MethodB() => MethodC();
static void MethodC() => throw new Exception("Error!");
```

### InnerException
The underlying exception that caused this exception.

```csharp
try {
    try {
        riskyOperation();
    } catch (SqlException ex) {
        throw new DataAccessException("Save failed", ex);
    }
} catch (DataAccessException ex) {
    Console.WriteLine($"Outer: {ex.Message}");
    if (ex.InnerException != null) {
        Console.WriteLine($"Inner: {ex.InnerException.Message}");
        Console.WriteLine($"Inner Type: {ex.InnerException.GetType().Name}");
    }
}
```

### Source
The name of the assembly/project where exception was thrown.

```csharp
try {
    throw new Exception("Error");
} catch (Exception ex) {
    Console.WriteLine(ex.Source);
    // Output: Assembly-CSharp or your assembly name
}
```

### TargetSite
Information about the method where exception was thrown.

```csharp
try {
    throw new Exception("Error");
} catch (Exception ex) {
    Console.WriteLine(ex.TargetSite.Name);
    // Output: MethodName
    
    Console.WriteLine(ex.TargetSite.DeclaringType);
    // Output: FullTypeName
}
```

### HResult
Error code associated with the exception.

```csharp
try {
    throw new Exception("Error");
} catch (Exception ex) {
    Console.WriteLine($"HResult: {ex.HResult}");
    // Output: HResult: -2146893055 (or similar)
}
```

## Examining Exception Information

### Complete Exception Details
```csharp
try {
    riskyOperation();
} catch (Exception ex) {
    Console.WriteLine($"Type: {ex.GetType().Name}");
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"StackTrace: {ex.StackTrace}");
    Console.WriteLine($"Source: {ex.Source}");
    Console.WriteLine($"Method: {ex.TargetSite.Name}");
    
    if (ex.InnerException != null) {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
}
```

### Exception Chain
```csharp
try {
    operation();
} catch (Exception ex) {
    var current = ex;
    int level = 0;
    
    while (current != null) {
        Console.WriteLine($"Level {level}: {current.Message}");
        current = current.InnerException;
        level++;
    }
}
```

## Logging Exception Details

### Basic Logging
```csharp
try {
    operation();
} catch (Exception ex) {
    logger.Error($"Error occurred: {ex.Message}");
}
```

### Comprehensive Logging
```csharp
try {
    operation();
} catch (Exception ex) {
    var logMessage = new StringBuilder();
    logMessage.AppendLine($"Exception Type: {ex.GetType().FullName}");
    logMessage.AppendLine($"Message: {ex.Message}");
    logMessage.AppendLine($"StackTrace: {ex.StackTrace}");
    
    if (ex.InnerException != null) {
        logMessage.AppendLine($"Inner Exception: {ex.InnerException.Message}");
        logMessage.AppendLine($"Inner StackTrace: {ex.InnerException.StackTrace}");
    }
    
    logger.Error(logMessage.ToString());
}
```

### Structured Logging
```csharp
try {
    operation();
} catch (Exception ex) {
    logger.Error(ex, "Operation failed", new {
        userId = userId,
        operationId = operationId,
        timestamp = DateTime.UtcNow,
        exceptionType = ex.GetType().Name
    });
}
```

## Specific Exception Properties

### ArgumentException
```csharp
try {
    SetAge(-5);
} catch (ArgumentException ex) {
    Console.WriteLine($"Parameter Name: {ex.ParamName}");
    Console.WriteLine($"Message: {ex.Message}");
    // ParamName tells which parameter was invalid
}

void SetAge(int age) {
    if (age < 0) {
        throw new ArgumentException("Age cannot be negative", nameof(age));
    }
}
```

### ArgumentOutOfRangeException
```csharp
try {
    SetIndex(-1);
} catch (ArgumentOutOfRangeException ex) {
    Console.WriteLine($"Parameter: {ex.ParamName}");
    Console.WriteLine($"Actual Value: {ex.ActualValue}");
    // ActualValue shows what was passed
}

void SetIndex(int index) {
    if (index < 0 || index >= Size) {
        throw new ArgumentOutOfRangeException(
            nameof(index),
            index,
            "Index out of range"
        );
    }
}
```

### FileNotFoundException
```csharp
try {
    File.ReadAllText("missing.txt");
} catch (FileNotFoundException ex) {
    Console.WriteLine($"File: {ex.FileName}");
    // Shows which file was not found
}
```

### InvalidOperationException
```csharp
try {
    var item = list.First();
} catch (InvalidOperationException ex) {
    Console.WriteLine($"Message: {ex.Message}");
    // "Sequence contains no elements"
}
```

## Exception Methods

### GetBaseException()
Get the root cause exception:

```csharp
try {
    try {
        operation1();
    } catch (Exception ex) {
        throw new Exception("Level 2", ex);
    }
} catch (Exception ex) {
    var rootException = ex.GetBaseException();
    Console.WriteLine($"Root cause: {rootException.Message}");
}
```

### ToString()
Get complete exception information:

```csharp
try {
    riskyOperation();
} catch (Exception ex) {
    Console.WriteLine(ex.ToString());
    // Includes: Type, Message, StackTrace, InnerException
}
```

## Custom Exception Properties

### Example 1: Domain Properties
```csharp
public class PaymentException : Exception {
    public string TransactionId { get; }
    public decimal Amount { get; }
    public DateTime OccurredAt { get; }
    
    public PaymentException(
        string transactionId,
        decimal amount,
        string message)
        : base(message) {
        TransactionId = transactionId;
        Amount = amount;
        OccurredAt = DateTime.UtcNow;
    }
}

// Usage
try {
    ProcessPayment(order);
} catch (PaymentException ex) {
    logger.Error(
        $"Payment failed for transaction {ex.TransactionId}: {ex.Amount} - {ex.Message}"
    );
}
```

### Example 2: Error Code Properties
```csharp
public class ApiException : Exception {
    public int StatusCode { get; }
    public string ErrorCode { get; }
    public Dictionary<string, object> ErrorDetails { get; }
    
    public ApiException(
        int statusCode,
        string errorCode,
        string message,
        Dictionary<string, object> details = null)
        : base(message) {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        ErrorDetails = details ?? new Dictionary<string, object>();
    }
}

// Usage
try {
    callApi();
} catch (ApiException ex) {
    return StatusCode(ex.StatusCode, new {
        code = ex.ErrorCode,
        message = ex.Message,
        details = ex.ErrorDetails
    });
}
```

## Accessing Exception Context

### User-Friendly Message
```csharp
try {
    operation();
} catch (Exception ex) {
    string userMessage = GetUserFriendlyMessage(ex);
    Console.WriteLine(userMessage);
}

private string GetUserFriendlyMessage(Exception ex) {
    return ex switch {
        ValidationException => $"Invalid data: {ex.Message}",
        FileNotFoundException => "File not found. Please check the path.",
        TimeoutException => "Operation timed out. Please try again.",
        _ => "An error occurred. Please try again later."
    };
}
```

### Debugging Information
```csharp
try {
    operation();
} catch (Exception ex) {
    var debugInfo = new {
        ExceptionType = ex.GetType().FullName,
        Message = ex.Message,
        StackTrace = ex.StackTrace,
        InnerException = ex.InnerException?.Message,
        Source = ex.Source,
        Method = ex.TargetSite?.Name
    };
    
    logger.Debug(JsonConvert.SerializeObject(debugInfo));
}
```

## Best Practices

✓ Always log full exception information
```csharp
catch (Exception ex) {
    logger.Error(ex.ToString());
}
```

✓ Use InnerException for context
```csharp
catch (SqlException ex) {
    throw new DataAccessException("Save failed", ex);
}
```

✓ Extract specific properties for custom exceptions
```csharp
catch (ArgumentOutOfRangeException ex) {
    Console.WriteLine($"Invalid value: {ex.ActualValue}");
}
```

✓ Create structured logging
```csharp
logger.Error(ex, "Operation failed", new {
    operationId = id,
    timestamp = DateTime.UtcNow
});
```

## Anti-Patterns

❌ Ignoring exception information
```csharp
catch (Exception) { }  // Lost all context
```

❌ Only logging message
```csharp
logger.Error(ex.Message);  // Lost StackTrace
```

❌ Losing InnerException
```csharp
throw new Exception("Error");  // Should wrap inner exception
```

❌ Not preserving stack trace
```csharp
catch (Exception ex) {
    throw ex;  // Use 'throw;' instead
}
```

## Exception Inspection Utility

```csharp
public static class ExceptionHelper {
    public static string GetDetailedMessage(Exception ex) {
        var sb = new StringBuilder();
        sb.AppendLine($"Exception Type: {ex.GetType().Name}");
        sb.AppendLine($"Message: {ex.Message}");
        
        if (ex.InnerException != null) {
            sb.AppendLine($"Inner Exception: {ex.InnerException.Message}");
        }
        
        sb.AppendLine($"Stack Trace: {ex.StackTrace}");
        
        if (ex.TargetSite != null) {
            sb.AppendLine($"Target Method: {ex.TargetSite.Name}");
        }
        
        return sb.ToString();
    }
    
    public static void LogFullException(Exception ex, ILogger logger) {
        logger.Error(GetDetailedMessage(ex));
        
        if (ex.InnerException != null) {
            logger.Error("=== INNER EXCEPTION ===");
            LogFullException(ex.InnerException, logger);
        }
    }
}

// Usage
try {
    operation();
} catch (Exception ex) {
    ExceptionHelper.LogFullException(ex, logger);
}
```

## Summary

- Message: Human-readable description
- StackTrace: Where exception occurred
- InnerException: Underlying exception
- Source: Assembly name
- TargetSite: Method information
- Custom properties: Domain-specific data
- Always log complete exception information
- Use InnerException for context
- Extract specific properties when available

---

## Next Steps

1. Learn Using Statement
2. Master Guard Clauses
3. Study IDisposable Pattern
4. Learn Best Practices
