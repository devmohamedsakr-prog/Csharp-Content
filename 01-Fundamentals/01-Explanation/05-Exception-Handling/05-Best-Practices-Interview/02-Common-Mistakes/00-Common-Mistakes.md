# Common Exception Handling Mistakes

## 1. Swallowing Exceptions Silently

One of the worst mistakes - catching and ignoring exceptions.

```csharp
// BAD - Silent failure
try {
    SaveData();
} catch (Exception) {
    // Exception ignored - no logging, no handling
}

// GOOD - Log the error
try {
    SaveData();
} catch (Exception ex) {
    logger.Error("Save failed", ex);
}

// GOOD - Handle or re-throw
try {
    SaveData();
} catch (Exception ex) {
    if (IsRecoverable(ex)) {
        HandleError(ex);
    } else {
        throw;  // Re-throw if not recoverable
    }
}
```

**Impact**: Bugs hidden, hard to debug, system fails silently

## 2. Catching Exception Too Broadly

Catching exceptions in wrong scope, obscuring which operation failed.

```csharp
// BAD - Can't tell which operation failed
try {
    ValidateInput(input);
    ComplexCalculation(input);
    SaveToDatabase(input);
} catch (Exception ex) {
    logger.Error("Error occurred");  // Which operation?
}

// GOOD - Narrow scope
try {
    ValidateInput(input);
} catch (ValidationException ex) {
    logger.Error("Validation failed", ex);
}

try {
    var result = ComplexCalculation(input);
} catch (CalculationException ex) {
    logger.Error("Calculation failed", ex);
}

try {
    SaveToDatabase(result);
} catch (DataAccessException ex) {
    logger.Error("Save failed", ex);
}
```

**Impact**: Harder to debug, vague error handling

## 3. Losing Stack Trace

Using `throw ex;` instead of `throw;` loses debugging information.

```csharp
// BAD - Loses stack trace
try {
    MethodC();  // Exception thrown here
} catch (Exception ex) {
    throw ex;   // Stack trace starts here!
}

// GOOD - Preserves original stack trace
try {
    MethodC();  // Exception thrown here
} catch (Exception ex) {
    throw;      // Stack trace preserved
}

// Result comparison:
// With 'throw;': Shows full chain from MethodC → MethodB → MethodA
// With 'throw ex;': Shows only MethodA (loss of origin information)
```

**Impact**: Nearly impossible to debug production issues

## 4. Wrong Exception Order

Catching general exceptions before specific ones.

```csharp
// BAD - ArgumentNullException never caught
try {
    operation();
} catch (Exception ex) {
    // Catches everything, including ArgumentNullException
} catch (ArgumentNullException ex) {
    // Never reached
}

// GOOD - Specific before general
try {
    operation();
} catch (ArgumentNullException ex) {
    // Catch specific first
} catch (ArgumentException ex) {
    // More general
} catch (Exception ex) {
    // Most general
}
```

**Impact**: Wrong exception handlers execute, incorrect error recovery

## 5. Not Disposing Resources

Forgetting to dispose of unmanaged resources.

```csharp
// BAD - Resource leak
StreamReader reader = new StreamReader("file.txt");
string content = reader.ReadToEnd();
// reader never disposed!

// BAD - Still leaks if exception occurs
StreamReader reader = new StreamReader("file.txt");
string content = reader.ReadToEnd();
reader.Dispose();

// GOOD - Guaranteed disposal
using (StreamReader reader = new StreamReader("file.txt")) {
    string content = reader.ReadToEnd();
}

// GOOD - C# 8+ simpler syntax
using StreamReader reader = new StreamReader("file.txt");
string content = reader.ReadToEnd();
```

**Impact**: File handles/memory leaks, application runs out of resources

## 6. Throwing Bare Exception

Throwing generic Exception without specific type.

```csharp
// BAD - Too generic
throw new Exception("Error");
throw new Exception("Invalid input");

// GOOD - Specific exception type
throw new ArgumentException("Age must be positive", nameof(age));
throw new ArgumentNullException(nameof(user));
throw new InvalidOperationException("Database not connected");

// GOOD - Custom domain exceptions
throw new ValidationException("User validation failed");
throw new BusinessRuleException("Cannot process incomplete order");
```

**Impact**: Caller can't handle specific cases, generic handling only

## 7. Not Checking for Null

Not validating null before operations.

```csharp
// BAD - No null check
public void ProcessUser(User user) {
    ValidateUser(user);  // Crashes if user is null
}

// GOOD - Guard clause
public void ProcessUser(User user) {
    if (user == null) {
        throw new ArgumentNullException(nameof(user));
    }
    ValidateUser(user);
}

// GOOD - Null-safe navigation
public void ProcessUser(User user) {
    var name = user?.Name ?? "Unknown";  // Safe
}
```

**Impact**: NullReferenceException at runtime, potential data corruption

## 8. Throwing in Finalizer

Throwing exceptions in finalizer causes crash.

```csharp
// BAD - Never throw in finalizer
~Resource() {
    throw new Exception("Cleanup failed");  // Crashes app!
}

// GOOD - Suppress exceptions
~Resource() {
    try {
        CloseHandle();
    } catch {
        // Log error but don't throw
    }
}
```

**Impact**: Application crash, uncontrolled termination

## 9. Not Preserving Inner Exception

Losing original exception context when wrapping.

```csharp
// BAD - Lost original exception
try {
    database.Save();
} catch (SqlException ex) {
    throw new DataAccessException("Save failed");  // ex is lost!
}

// GOOD - Preserve inner exception
try {
    database.Save();
} catch (SqlException ex) {
    throw new DataAccessException("Save failed", ex);
}

// Usage
try {
    service.Save();
} catch (DataAccessException ex) {
    logger.Error($"Error: {ex.Message}");
    logger.Error($"Cause: {ex.InnerException?.Message}");  // Can trace root cause
}
```

**Impact**: Can't diagnose root cause, hard to debug

## 10. Using Exceptions for Normal Flow

Using exceptions for expected conditions.

```csharp
// BAD - Exception for expected failure
try {
    int value = int.Parse(userInput);
} catch (FormatException) {
    Console.WriteLine("Invalid input");
}

// GOOD - TryParse for expected failure
if (int.TryParse(userInput, out int value)) {
    Console.WriteLine($"Valid: {value}");
} else {
    Console.WriteLine("Invalid input");
}

// BAD - Exception for expected condition
try {
    int first = list.First();
} catch (InvalidOperationException) {
    Console.WriteLine("List is empty");
}

// GOOD - Check first
if (list.Count > 0) {
    int first = list[0];
} else {
    Console.WriteLine("List is empty");
}
```

**Impact**: Performance hit, less readable code

## 11. Catching All Without Re-throwing

Catching and modifying exception inappropriately.

```csharp
// BAD - Loses context
try {
    operation();
} catch {
    throw new Exception("Something went wrong");  // Which operation?
}

// GOOD - Include context and preserve original
try {
    operation();
} catch (Exception ex) {
    throw new ApplicationException(
        $"Operation failed in {MethodBase.GetCurrentMethod().Name}",
        ex
    );
}
```

**Impact**: Debugging becomes harder, context lost

## 12. Not Handling Different Exception Types

Treating all exceptions the same.

```csharp
// BAD - Same handling for all
try {
    ValidateInput(input);
    AccessDatabase();
} catch (Exception ex) {
    Console.WriteLine("Error occurred");
}

// GOOD - Different handling
try {
    ValidateInput(input);
} catch (ValidationException ex) {
    Console.WriteLine("Invalid input");  // User error
}

try {
    AccessDatabase();
} catch (DatabaseException ex) {
    logger.Error("Database error", ex);  // System error
    throw;
}
```

**Impact**: Inappropriate error recovery, poor user experience

## 13. Not Logging Exception Details

Logging only message, losing stack trace and context.

```csharp
// BAD - Minimal logging
catch (Exception ex) {
    logger.Error(ex.Message);  // Lost StackTrace
}

// GOOD - Complete logging
catch (Exception ex) {
    logger.Error(ex.ToString());  // Includes everything
}

// GOOD - Structured logging
catch (Exception ex) {
    logger.Error(ex, "Operation failed", new {
        userId = userId,
        operationId = operationId,
        timestamp = DateTime.UtcNow
    });
}
```

**Impact**: Production debugging nearly impossible

## 14. Null Conditional But No Null Coalescing

Not providing default value.

```csharp
// BAD - Returns null if object is null
int? length = text?.Length;

// GOOD - Provides default
int length = text?.Length ?? 0;

// GOOD - Explicitly handle null
string name = user?.Name;
if (name == null) {
    name = "Unknown";
}
```

**Impact**: Null reference exceptions later, unexpected behavior

## 15. Finally Block Throwing Exceptions

Throwing in finally replaces original exception.

```csharp
// BAD - Replaces original exception
try {
    throw new FormatException("Original");
} finally {
    throw new Exception("Finally");  // This exception is thrown instead!
}

// GOOD - Handle exceptions in finally
try {
    throw new FormatException("Original");
} finally {
    try {
        Cleanup();
    } catch {
        logger.Error("Cleanup error");
    }
}
```

**Impact**: Original error hidden, wrong exception propagates

## 16. Not Using Guard Clauses

Deep nesting from missing guard clauses.

```csharp
// BAD - Deep nesting
public void Process(User user) {
    if (user != null) {
        if (!string.IsNullOrEmpty(user.Name)) {
            if (user.Age >= 18) {
                // Finally process
            }
        }
    }
}

// GOOD - Guard clauses
public void Process(User user) {
    if (user == null) throw new ArgumentNullException(nameof(user));
    if (string.IsNullOrEmpty(user.Name)) throw new ArgumentException("Name required");
    if (user.Age < 18) throw new ArgumentException("Must be 18+");
    
    // Flat, readable code
}
```

**Impact**: Hard to read code, harder to maintain

## 17. Empty Catch Blocks

Catching with no action - worse than swallowing.

```csharp
// BAD - Empty catch
try {
    operation();
} catch (Exception) {
    // What happened here?
}

// GOOD - At least comment why
try {
    operation();
} catch (ExpectedException) {
    // Expected - can safely ignore
}

// GOOD - Log if needed
try {
    operation();
} catch (NonCriticalException ex) {
    logger.Debug("Non-critical error", ex);
}
```

**Impact**: Impossible to troubleshoot issues

## 18. Not Documenting Exceptions

Not documenting what exceptions methods throw.

```csharp
// BAD - No documentation
public void SaveUser(User user) {
    if (user == null) throw new ArgumentNullException(nameof(user));
    // Caller doesn't know about this exception
}

// GOOD - Document exceptions
/// <exception cref="ArgumentNullException">user is null</exception>
/// <exception cref="ValidationException">User data invalid</exception>
/// <exception cref="DataAccessException">Database save failed</exception>
public void SaveUser(User user) {
    if (user == null) throw new ArgumentNullException(nameof(user));
    ValidateUser(user);
    database.Save(user);
}
```

**Impact**: Caller unprepared for exceptions, API unclear

## 19. Catching SystemException Specifically

Catching SystemException instead of specific types.

```csharp
// BAD - Too broad
try {
    operation();
} catch (SystemException ex) {
    // Catches all CLR exceptions including programming errors
}

// GOOD - Specific types
try {
    operation();
} catch (ArgumentException ex) {
    // Only argument errors
} catch (FormatException ex) {
    // Only format errors
}
```

**Impact**: Catches programming errors that should not be caught

## 20. Assuming Exception Won't Happen

Not handling likely exceptions.

```csharp
// BAD - Assumes no exception
public string GetConfigValue(string key) {
    return config[key];  // KeyNotFoundException if not found
}

// GOOD - Handle likely exceptions
public string GetConfigValue(string key) {
    if (!config.ContainsKey(key)) {
        return "default";
    }
    return config[key];
}

// GOOD - Or use TryGetValue
public string GetConfigValue(string key) {
    return config.TryGetValue(key, out var value) 
        ? value 
        : "default";
}
```

**Impact**: Unexpected exceptions at runtime

## Summary of Common Mistakes

| Mistake | Problem | Solution |
|---------|---------|----------|
| Swallow exceptions | Silent failures | Log or handle always |
| Catch too broadly | Can't identify error | Narrow exception scope |
| Lose stack trace | Can't debug | Use `throw;` not `throw ex;` |
| Wrong order | Wrong handler | Specific exceptions first |
| Don't dispose | Resource leak | Use `using` statement |
| Generic Exception | Can't handle specifically | Use specific exception types |
| No null check | NullReferenceException | Guard clauses |
| Throw in finalizer | App crash | Suppress exceptions |
| Lose inner exception | Can't diagnose | Preserve inner exception |
| Exceptions for flow | Performance hit | Use TryParse/TryGetValue |
| Catch all loosely | Can't debug | Handle or re-throw |
| Ignore exception types | Poor recovery | Handle each type |
| Minimal logging | Can't debug | Log full exception details |
| No defaults | Unexpected nulls | Use null coalescing |
| Throw in finally | Wrong exception | Handle exceptions in finally |
| No guard clauses | Deep nesting | Add precondition checks |
| Empty catch | Complete mystery | Always document why |
| Don't document | API unclear | Document exceptions thrown |
| Catch SystemException | Catch too much | Catch specific types |
| Assume no error | Unexpected crash | Handle likely exceptions |

---

## Next Steps

1. Study Interview Questions
2. Practice Exception Handling
3. Review Real-World Patterns
4. Build Exception Handler Utility
