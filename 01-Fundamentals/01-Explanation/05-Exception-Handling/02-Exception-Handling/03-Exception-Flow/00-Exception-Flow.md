# Exception Flow and Propagation

## Overview
Understanding how exceptions flow through your code helps you handle them at the right level and debug problems effectively.

## Basic Exception Flow

When an exception occurs, it travels through the call stack looking for a catch block:

```
Method C throws exception
    ↓
Method B (no catch) - exception propagates up
    ↓
Method A (has catch) - exception caught and handled
    ↓
Program continues
```

## Call Stack Propagation

### Example
```csharp
public class Program {
    static void Main() {
        Console.WriteLine("1. Main starts");
        MethodA();
        Console.WriteLine("5. Main continues");
    }
    
    static void MethodA() {
        Console.WriteLine("2. MethodA starts");
        try {
            MethodB();
            Console.WriteLine("4. MethodA - after MethodB");
        } catch (Exception ex) {
            Console.WriteLine($"3b. MethodA caught: {ex.Message}");
        }
    }
    
    static void MethodB() {
        Console.WriteLine("2b. MethodB starts");
        throw new InvalidOperationException("Error in B");
    }
}

// Output:
// 1. Main starts
// 2. MethodA starts
// 2b. MethodB starts
// 3b. MethodA caught: Error in B
// 5. Main continues
```

## No Catch - Exception Propagates Up

When no catch block exists, exception bubbles up:

```csharp
public class Program {
    static void Main() {
        try {
            MethodA();
        } catch (Exception) {
            Console.WriteLine("Caught in Main");
        }
    }
    
    static void MethodA() {
        MethodB();  // No try-catch - exception propagates
    }
    
    static void MethodB() {
        MethodC();  // No try-catch - exception propagates
    }
    
    static void MethodC() {
        throw new Exception("Error!");  // Exception thrown
    }
}

// Propagation chain:
// MethodC throws
// MethodB doesn't catch - passes to MethodA
// MethodA doesn't catch - passes to Main
// Main catches
```

## Multiple Catch Levels

Catch at appropriate level:

```csharp
public class DataProcessor {
    public void ProcessFile(string filename) {
        try {
            ValidateFile(filename);
            ReadData(filename);
            SaveData();
        } catch (FileNotFoundException ex) {
            Console.WriteLine("File not found");
        } catch (InvalidDataException ex) {
            Console.WriteLine("Invalid data in file");
        } catch (Exception ex) {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
    
    private void ValidateFile(string filename) {
        if (!File.Exists(filename)) {
            throw new FileNotFoundException($"File not found: {filename}");
        }
    }
    
    private void ReadData(string filename) {
        var content = File.ReadAllText(filename);
        if (string.IsNullOrEmpty(content)) {
            throw new InvalidDataException("File is empty");
        }
    }
    
    private void SaveData() {
        // Save logic
    }
}
```

## Nested Try-Catch

Inner and outer handlers:

```csharp
public void OuterMethod() {
    try {
        try {
            Console.WriteLine("1. Inner try");
            throw new FormatException("Inner error");
        } catch (ArgumentException ex) {
            // Doesn't catch FormatException
            Console.WriteLine("Inner catch");
        }
        // FormatException propagates here
    } catch (FormatException ex) {
        Console.WriteLine("2. Outer catch");
    }
}

// Output:
// 1. Inner try
// 2. Outer catch
```

## Re-throwing Exceptions

### Re-throw Original
```csharp
try {
    riskyOperation();
} catch (Exception ex) {
    logger.Error($"Error: {ex.Message}");
    throw;  // Re-throw preserves stack trace
}
```

### Throw New Exception
```csharp
try {
    database.Save();
} catch (SqlException ex) {
    // Wrap with context-specific exception
    throw new DataAccessException("Failed to save", ex);
}
```

**Stack trace with re-throw**:
```csharp
// Using 'throw;'
at Program.MethodC() in C:\Program.cs:line 42
at Program.MethodB() in C:\Program.cs:line 30
at Program.MethodA() in C:\Program.cs:line 15
at Program.Main() in C:\Program.cs:line 5

// Using 'throw ex;' - stack trace loses inner calls
at Program.MethodA() in C:\Program.cs:line 20
```

**Always use `throw;` not `throw ex;`**:

```csharp
// BAD - Loses stack trace
catch (Exception ex) {
    throw ex;  // Stack trace shows only this level
}

// GOOD - Preserves stack trace
catch (Exception ex) {
    throw;  // Original stack trace preserved
}
```

## Exception Flow Visualization

### Simple Case
```
Program (Main)
    ↓
Method1 (no handler)
    ↓
Method2 (no handler)
    ↓
Method3 (throws exception)
    ↓
Exception propagates back up: 3→2→1→Main
    ↓
Main catches - handled
```

### Complex Case
```
Program (Main) → tries to catch
    ↓
Method1 (tries to catch FormatException)
    ↓
Method2 (no handler)
    ↓
Method3 (throws NullReferenceException) ← Wrong type!
    ↓
Propagates back: 3→2→1
    ↓
Method1 doesn't catch (wrong type) → propagates
    ↓
Main catches (generic Exception)
```

## Real-World Example: API Call

```csharp
public class UserService {
    private readonly IUserRepository repository;
    private readonly IEmailService emailService;
    private readonly ILogger logger;
    
    public void RegisterUser(User user) {
        try {
            ValidateUser(user);
            SaveUser(user);
            SendConfirmationEmail(user.Email);
        } catch (ValidationException ex) {
            // User validation failed - expected
            logger.Info($"Registration validation failed: {ex.Message}");
            throw;  // Let caller know
        } catch (DataAccessException ex) {
            // Database error - unexpected
            logger.Error($"Database error during registration: {ex.Message}");
            throw;  // Let caller know
        } catch (EmailException ex) {
            // Email failed but user saved - warning
            logger.Warn($"Email failed for {user.Email}: {ex.Message}");
            // Don't throw - user was saved successfully
        } catch (Exception ex) {
            // Unexpected error
            logger.Error($"Unexpected error: {ex.Message}", ex);
            throw;
        }
    }
    
    private void ValidateUser(User user) {
        if (user == null) {
            throw new ValidationException("User cannot be null");
        }
    }
    
    private void SaveUser(User user) {
        try {
            repository.Save(user);
        } catch (SqlException ex) {
            throw new DataAccessException("Failed to save user", ex);
        }
    }
    
    private void SendConfirmationEmail(string email) {
        try {
            emailService.Send(email, "Confirm registration");
        } catch (SmtpException ex) {
            throw new EmailException("Failed to send email", ex);
        }
    }
}
```

## Exception vs Normal Flow

### Exception-Based
```csharp
try {
    int result = int.Parse(userInput);
    ProcessResult(result);
} catch (FormatException) {
    Console.WriteLine("Invalid input");
}
```

### Check-Based (Preferred for Expected Failures)
```csharp
if (int.TryParse(userInput, out int result)) {
    ProcessResult(result);
} else {
    Console.WriteLine("Invalid input");
}
```

## Unhandled Exception Handler

For exceptions that reach the top:

```csharp
public class Program {
    static void Main() {
        // Set global exception handler
        AppDomain.CurrentDomain.UnhandledException += 
            (sender, e) => {
                logger.Fatal($"Unhandled exception: {e.ExceptionObject}");
                Environment.Exit(1);
            };
        
        try {
            Run();
        } catch (Exception ex) {
            Console.WriteLine($"Top-level error: {ex.Message}");
        }
    }
}
```

## Best Practices

✓ Catch at appropriate level
```csharp
// Catch at the level where you can handle it
try {
    operation();
} catch (ValidationException) {
    // Handle validation error here
}
```

✓ Preserve stack trace
```csharp
catch (Exception ex) {
    throw;  // Use 'throw;' not 'throw ex;'
}
```

✓ Log before re-throwing
```csharp
catch (Exception ex) {
    logger.Error("Operation failed", ex);
    throw;
}
```

✓ Use inner exceptions for context
```csharp
catch (SqlException ex) {
    throw new DataAccessException("Save failed", ex);
    // ex becomes InnerException
}
```

## Anti-Patterns

❌ Swallow exceptions
```csharp
try {
    operation();
} catch (Exception) {
    // Silent failure
}
```

❌ Lose stack trace
```csharp
catch (Exception ex) {
    throw ex;  // Use 'throw;' instead
}
```

❌ Re-throw without value
```csharp
catch (Exception ex) {
    throw;  // If not logging or handling, this is ok
}
```

❌ Catch everything
```csharp
try {
    // 100 lines of code
} catch (Exception) {
    // Which operation failed?
}
```

## Summary

- Exceptions propagate up call stack
- Catch at appropriate level
- Use `throw;` to preserve stack trace
- Log before re-throwing
- Wrap exceptions with context
- Never swallow exceptions silently

---

## Next Steps

1. Learn Custom Exceptions
2. Master Exception Properties
3. Study Resource Management
4. Learn Best Practices
