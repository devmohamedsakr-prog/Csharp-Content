# Exception Hierarchy in C#

## Overview
All exceptions in C# inherit from `System.Exception`. Understanding the hierarchy helps write better exception handling code.

## Exception Class Hierarchy

```
System.Object
    ↓
System.Exception (Base class for all exceptions)
    ├── System.SystemException
    │   ├── System.ArgumentException
    │   │   └── System.ArgumentNullException
    │   │   └── System.ArgumentOutOfRangeException
    │   │
    │   ├── System.ArithmeticException
    │   │   ├── System.DivideByZeroException
    │   │   ├── System.OverflowException
    │   │
    │   ├── System.InvalidOperationException
    │   ├── System.NullReferenceException
    │   ├── System.IndexOutOfRangeException
    │   ├── System.InvalidCastException
    │   ├── System.NotSupportedException
    │   ├── System.NotImplementedException
    │   ├── System.ObjectDisposedException
    │   │
    │   ├── System.IO.IOException
    │   │   ├── System.IO.FileNotFoundException
    │   │   ├── System.IO.DirectoryNotFoundException
    │   │   ├── System.IO.PathTooLongException
    │   │
    │   └── System.TimeoutException
    │
    └── System.ApplicationException
        └── Custom Exceptions (User-defined)
```

## Exception Base Class (System.Exception)

The base class for all exceptions with common properties:

```csharp
public class Exception : ISerializable {
    public string Message { get; }
    public string StackTrace { get; }
    public Exception InnerException { get; }
    public string Source { get; }
    public int HResult { get; }
    public MethodBase TargetSite { get; }
}
```

### Properties

**Message**: Human-readable description of the error

```csharp
try {
    int result = int.Parse("abc");
} catch (Exception ex) {
    Console.WriteLine(ex.Message);
    // "Input string was not in a correct format."
}
```

**StackTrace**: Shows where exception occurred

```csharp
try {
    throw new Exception("Error occurred");
} catch (Exception ex) {
    Console.WriteLine(ex.StackTrace);
    // at Program.MethodC() in C:\Program.cs:line 25
    // at Program.MethodB() in C:\Program.cs:line 20
    // at Program.MethodA() in C:\Program.cs:line 15
}
```

**InnerException**: Wrapping another exception

```csharp
try {
    try {
        riskyOperation();
    } catch (Exception ex) {
        throw new Exception("Outer error", ex);  // ex is InnerException
    }
} catch (Exception ex) {
    Console.WriteLine($"Outer: {ex.Message}");
    Console.WriteLine($"Inner: {ex.InnerException?.Message}");
}
```

## SystemException vs ApplicationException

### SystemException
Represents errors thrown by CLR (Common Language Runtime).

```csharp
// All built-in .NET exceptions inherit from SystemException
try {
    int result = int.Parse("abc");  // FormatException
} catch (SystemException ex) {
    // Catches all CLR exceptions
}
```

### ApplicationException
Base class for custom exceptions (not required but conventional).

```csharp
// Custom exception (optional inheritance)
public class BusinessException : Exception {
    public string ErrorCode { get; set; }
}

// Or inherit from ApplicationException (older pattern, still used)
public class BusinessException : ApplicationException {
    public string ErrorCode { get; set; }
}
```

## ArgumentException Family

### ArgumentException
Generic invalid argument.

```csharp
public void SetAge(int age) {
    if (age < 0 || age > 150) {
        throw new ArgumentException(
            "Age must be between 0 and 150",
            nameof(age)
        );
    }
}

try {
    SetAge(-5);
} catch (ArgumentException ex) {
    Console.WriteLine($"Parameter: {ex.ParamName}");
    Console.WriteLine($"Message: {ex.Message}");
}
```

### ArgumentNullException
Argument is null when it shouldn't be.

```csharp
public void ProcessData(string data) {
    if (data == null) {
        throw new ArgumentNullException(nameof(data));
    }
}

try {
    ProcessData(null);
} catch (ArgumentNullException ex) {
    Console.WriteLine($"Null parameter: {ex.ParamName}");
}
```

### ArgumentOutOfRangeException
Argument value is outside valid range.

```csharp
public void SetIndex(int index) {
    if (index < 0 || index >= Size) {
        throw new ArgumentOutOfRangeException(
            nameof(index),
            $"Index must be between 0 and {Size - 1}"
        );
    }
}

try {
    SetIndex(-1);
} catch (ArgumentOutOfRangeException ex) {
    Console.WriteLine($"Value: {ex.ActualValue}");
    Console.WriteLine($"Message: {ex.Message}");
}
```

## ArithmeticException Family

### DivideByZeroException
Division by zero.

```csharp
try {
    int result = 10 / 0;
} catch (DivideByZeroException ex) {
    Console.WriteLine("Cannot divide by zero");
}
```

### OverflowException
Value exceeds type limits.

```csharp
try {
    checked {
        int result = int.MaxValue + 1;
    }
} catch (OverflowException ex) {
    Console.WriteLine("Value overflow");
}
```

## Catching by Hierarchy

Catch specific exceptions before general ones:

```csharp
try {
    ProcessUserInput(userInput);
} catch (ArgumentNullException ex) {
    // Most specific
    Console.WriteLine("Null argument");
} catch (ArgumentException ex) {
    // More general (catches any ArgumentException subclass)
    Console.WriteLine("Invalid argument");
} catch (SystemException ex) {
    // Even more general
    Console.WriteLine("System error");
} catch (Exception ex) {
    // Catch all
    Console.WriteLine("Unknown error");
}
```

**Order matters**:
```csharp
// WRONG - ArgumentNullException never caught
try {
    ProcessData(data);
} catch (ArgumentException ex) {
    // Catches both ArgumentException and ArgumentNullException
} catch (ArgumentNullException ex) {
    // Never reached!
}

// CORRECT - Specific first
try {
    ProcessData(data);
} catch (ArgumentNullException ex) {
    // Specific exception
} catch (ArgumentException ex) {
    // More general
}
```

## IO Exception Hierarchy

```
IOException (Base for all I/O)
├── FileNotFoundException
├── DirectoryNotFoundException
├── PathTooLongException
├── EndOfStreamException
└── etc.
```

### FileNotFoundException
File doesn't exist.

```csharp
try {
    var reader = new StreamReader("missing.txt");
} catch (FileNotFoundException ex) {
    Console.WriteLine($"File not found: {ex.FileName}");
}
```

### DirectoryNotFoundException
Directory doesn't exist.

```csharp
try {
    var files = Directory.GetFiles("C:\\missing");
} catch (DirectoryNotFoundException ex) {
    Console.WriteLine("Directory not found");
}
```

### IOException (Base)
General I/O error.

```csharp
try {
    File.Delete("file.txt");
} catch (FileNotFoundException) {
    // Specific IO error
} catch (IOException) {
    // General IO error
}
```

## Creating Custom Exception Hierarchies

### Simple Custom Exception
```csharp
public class BusinessException : Exception {
    public BusinessException(string message) : base(message) { }
}

try {
    throw new BusinessException("Invalid business rule");
} catch (BusinessException ex) {
    Console.WriteLine(ex.Message);
}
```

### Exception Hierarchy for Your Domain
```csharp
// Base exception for your application
public class MyAppException : Exception {
    public MyAppException(string message) : base(message) { }
}

// Domain-specific exceptions
public class ValidationException : MyAppException {
    public ValidationException(string message) : base(message) { }
}

public class DataAccessException : MyAppException {
    public DataAccessException(string message, Exception inner) 
        : base(message, inner) { }
}

public class BusinessRuleException : MyAppException {
    public BusinessRuleException(string message) : base(message) { }
}

// Usage
try {
    ValidateUser(user);
    AccessDatabase();
    ApplyBusinessRules();
} catch (ValidationException ex) {
    Console.WriteLine($"Validation error: {ex.Message}");
} catch (DataAccessException ex) {
    Console.WriteLine($"Database error: {ex.Message}");
} catch (BusinessRuleException ex) {
    Console.WriteLine($"Business rule violation: {ex.Message}");
} catch (MyAppException ex) {
    Console.WriteLine($"Application error: {ex.Message}");
}
```

## Catching Multiple Exceptions (C# 6+)

### Multiple Exceptions Same Handler
```csharp
try {
    ProcessFile(filename);
} catch (FileNotFoundException ex) when (IsRecoverable(ex)) {
    RetryWithDefault();
} catch (IOException) {
    LogError();
    throw;
}
```

### When Expressions (Filtering)
```csharp
try {
    ProcessData(data);
} catch (ArgumentException ex) when (ex.ParamName == "data") {
    Console.WriteLine("Invalid data parameter");
} catch (ArgumentException ex) {
    Console.WriteLine("Invalid argument");
}
```

## Best Practices for Exception Hierarchy

✓ Create domain-specific exceptions

```csharp
public class PaymentException : Exception { }
public class InvalidPaymentMethodException : PaymentException { }
public class InsufficientFundsException : PaymentException { }
```

✓ Catch specific exceptions before general ones

```csharp
try {
    operation();
} catch (SpecificException) { }
catch (GeneralException) { }
catch (Exception) { }
```

✓ Preserve inner exceptions

```csharp
try {
    database.Save();
} catch (SqlException ex) {
    throw new DataAccessException("Failed to save", ex);
}
```

✓ Document what exceptions are thrown

```csharp
/// <summary>
/// Processes the data.
/// </summary>
/// <exception cref="ArgumentNullException">data is null</exception>
/// <exception cref="ValidationException">data is invalid</exception>
public void ProcessData(string data) { }
```

## Anti-Patterns

❌ Catching too broad
```csharp
catch (Exception ex) { }  // Catches everything!
```

❌ Swallowing exceptions
```csharp
catch (Exception) { }  // Silent failure
```

❌ Wrong order
```csharp
catch (Exception) { }      // Catches all
catch (FileNotFound) { }   // Never reached
```

## Summary

- Exception hierarchy allows selective catching
- Catch specific exceptions before general ones
- SystemException = built-in CLR exceptions
- ApplicationException = custom exceptions
- Create domain-specific exception hierarchies
- Preserve inner exceptions for debugging

---

## Next Steps

1. Learn Try-Catch Patterns
2. Master Exception Flow
3. Create Custom Exceptions
4. Study Best Practices
