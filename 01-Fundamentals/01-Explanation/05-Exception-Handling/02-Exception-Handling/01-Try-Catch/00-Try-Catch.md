# Try-Catch Block

## Overview
Try-catch blocks are the fundamental mechanism for handling exceptions in C#. They allow you to attempt risky operations and handle any exceptions that occur.

## Basic Structure

```csharp
try {
    // Code that might throw exception
} catch (ExceptionType ex) {
    // Handle exception
}
```

## Simple Try-Catch

Catch one exception type:

```csharp
try {
    int result = int.Parse("abc");
} catch (FormatException ex) {
    Console.WriteLine("Invalid format");
}
```

## Multiple Catch Blocks

Handle different exception types:

```csharp
try {
    int result = int.Parse(userInput);
    int value = Calculate(result);
} catch (FormatException ex) {
    Console.WriteLine("Invalid number format");
} catch (ArgumentException ex) {
    Console.WriteLine("Invalid argument");
} catch (DivideByZeroException ex) {
    Console.WriteLine("Division by zero");
}
```

## Catch Order Matters

Specific exceptions must come before general ones:

```csharp
// CORRECT - Specific first
try {
    ProcessData();
} catch (ArgumentNullException ex) {
    // Catches null arguments (specific)
} catch (ArgumentException ex) {
    // Catches all ArgumentException subclasses (general)
} catch (Exception ex) {
    // Catches everything else (most general)
}

// WRONG - General first
try {
    ProcessData();
} catch (Exception ex) {
    // Catches ALL exceptions
} catch (ArgumentNullException ex) {
    // NEVER REACHED!
}
```

## Accessing Exception Information

```csharp
try {
    int result = int.Parse("abc");
} catch (FormatException ex) {
    // Message - description of error
    Console.WriteLine($"Message: {ex.Message}");
    // Output: Input string was not in a correct format.
    
    // StackTrace - where error occurred
    Console.WriteLine($"StackTrace: {ex.StackTrace}");
    
    // Source - assembly where thrown
    Console.WriteLine($"Source: {ex.Source}");
    
    // InnerException - underlying exception
    if (ex.InnerException != null) {
        Console.WriteLine($"Inner: {ex.InnerException.Message}");
    }
}
```

## Filtering Exceptions with 'when'

Use when clause to filter specific exceptions:

```csharp
try {
    ProcessData(user);
} catch (ArgumentException ex) when (ex.ParamName == "user") {
    Console.WriteLine("Invalid user parameter");
} catch (ArgumentException ex) {
    Console.WriteLine("Invalid parameter");
}
```

More complex filtering:

```csharp
try {
    accessFile();
} catch (IOException ex) when (IsFileLockedError(ex)) {
    RetryLater();
} catch (IOException ex) when (IsAccessDeniedError(ex)) {
    Console.WriteLine("Access denied");
}

private bool IsFileLockedError(IOException ex) {
    return ex.HResult == -2147024891;  // File locked error code
}

private bool IsAccessDeniedError(IOException ex) {
    return ex.HResult == -2147024891;  // Access denied error code
}
```

## Catch with No Type (Legacy)

Catches any exception without declaring variable:

```csharp
try {
    riskyOperation();
} catch {
    Console.WriteLine("An error occurred");
}
```

**Not recommended** - you can't access exception information.

## Catch Generic Exception Type

Catch any exception:

```csharp
try {
    operation();
} catch (Exception ex) {
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Stack: {ex.StackTrace}");
}
```

## Nested Try-Catch

Try-catch inside try-catch:

```csharp
try {
    try {
        risky1();
    } catch (SpecificException ex) {
        Console.WriteLine("Handled inner exception");
        // Can rethrow, throw new, or continue
    }
    
    risky2();
} catch (Exception ex) {
    Console.WriteLine("Outer handler");
}
```

## Re-throwing Exceptions

### Re-throw Same Exception
```csharp
try {
    operation();
} catch (ArgumentException ex) {
    Console.WriteLine("Logging error...");
    throw;  // Re-throw original exception
}
```

### Throw New Exception
```csharp
try {
    database.Save();
} catch (SqlException ex) {
    throw new DataAccessException("Failed to save data", ex);
}
```

### Conditional Re-throw
```csharp
try {
    operation();
} catch (ArgumentException ex) {
    if (ex.ParamName == "critical") {
        throw;  // Re-throw critical errors
    }
    Console.WriteLine("Non-critical error, continuing...");
}
```

## Exception Propagation

Exception bubbles up until caught:

```csharp
public void MethodA() {
    try {
        MethodB();
    } catch (Exception ex) {
        Console.WriteLine($"Caught in A: {ex.Message}");
    }
}

public void MethodB() {
    MethodC();  // Exception propagates here
}

public void MethodC() {
    throw new Exception("Error in C");
}

// Execution:
// MethodC throws
// MethodB doesn't catch → propagates
// MethodA catches → prints "Caught in A: Error in C"
```

## Exception Swallowing (Anti-Pattern)

**Never ignore exceptions**:

```csharp
// BAD - Silent failure
try {
    ImportantOperation();
} catch (Exception) {
    // Exception ignored, bug hidden!
}
```

**Better - Log or handle**:

```csharp
// GOOD - Log the error
try {
    ImportantOperation();
} catch (Exception ex) {
    logger.Error($"Operation failed: {ex.Message}");
}

// GOOD - Handle or re-throw
try {
    ImportantOperation();
} catch (Exception ex) {
    if (IsRecoverable(ex)) {
        HandleError(ex);
    } else {
        throw;
    }
}
```

## Try-Catch with Assignment

```csharp
int result = 0;
try {
    result = int.Parse(userInput);
} catch (FormatException) {
    Console.WriteLine("Invalid number");
    result = 0;  // Default value
}

ProcessResult(result);
```

## Complex Try-Catch Scenario

```csharp
try {
    // Step 1: Validate
    ValidateInput(user);
    
    // Step 2: Database operation
    database.SaveUser(user);
    
    // Step 3: Send notification
    emailService.SendConfirmation(user.Email);
} catch (ValidationException ex) {
    // Validation failed
    Console.WriteLine($"Validation error: {ex.Message}");
    return false;
} catch (SqlException ex) {
    // Database error
    logger.Error($"Database error: {ex.Message}");
    throw new DataAccessException("Failed to save user", ex);
} catch (EmailException ex) {
    // Email failed - but user was already saved
    logger.Warn($"Email notification failed: {ex.Message}");
    // Continue - user saved even though email failed
} catch (Exception ex) {
    // Unexpected error
    logger.Error($"Unexpected error: {ex.Message}");
    throw;
}
```

## Best Practices

✓ Catch specific exceptions

```csharp
try {
    int value = int.Parse(input);
} catch (FormatException) {  // Specific
    Console.WriteLine("Invalid format");
}
```

✓ Preserve call stack

```csharp
try {
    operation();
} catch (Exception ex) {
    throw;  // Preserves original stack trace
}
```

✓ Log meaningful information

```csharp
try {
    operation();
} catch (Exception ex) {
    logger.Error($"Operation failed. User: {userId}, Time: {DateTime.Now}, Error: {ex.Message}");
}
```

✓ Use when clauses for filtering

```csharp
try {
    operation();
} catch (IOException ex) when (IsRecoverable(ex)) {
    Retry();
}
```

## Anti-Patterns

❌ Too broad
```csharp
catch (Exception) { }  // Catches everything including programming errors
```

❌ Wrong order
```csharp
catch (Exception) { }
catch (FormatException) { }  // Never reached
```

❌ Silently fail
```csharp
catch (Exception) { }  // No logging or handling
```

❌ Lose context
```csharp
catch (Exception ex) {
    throw new Exception("Error");  // Lost original exception
}
```

## Performance Considerations

Try-catch has minimal performance impact when no exception occurs:

```csharp
// Very fast - exception not thrown
try {
    if (int.TryParse(input, out int value)) {
        ProcessValue(value);
    }
} catch (Exception) { }

// Expensive - exception thrown
try {
    int value = int.Parse(input);  // May throw
    ProcessValue(value);
} catch (Exception) { }
```

**Recommendation**: Use TryParse/TryGetValue for expected failures.

## Summary

- Try-catch catches and handles exceptions
- Catch specific exceptions before general ones
- Use 'when' clauses for filtering
- Re-throw with 'throw;' to preserve stack trace
- Never swallow exceptions silently
- Log or handle all caught exceptions

---

## Next Steps

1. Learn Try-Catch-Finally
2. Master Exception Flow
3. Create Custom Exceptions
4. Study Best Practices
