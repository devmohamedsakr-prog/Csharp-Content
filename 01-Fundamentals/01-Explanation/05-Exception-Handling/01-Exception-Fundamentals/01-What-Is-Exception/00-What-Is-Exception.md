# What is an Exception?

## Definition

An exception is an event that occurs during program execution that disrupts the normal flow of the program. Exceptions provide a structured way to handle errors instead of letting programs crash.

## Exception vs Error

### Error
- Represents a serious abnormal condition
- Cannot be recovered from
- Examples: OutOfMemoryError, StackOverflowError
- System terminates the program

### Exception
- Represents unexpected conditions that can be handled
- Can be caught and handled gracefully
- Examples: FormatException, FileNotFoundException
- Program can continue

## How Exceptions Work

When an exception occurs, the runtime creates an exception object and "throws" it up the call stack until caught by a try-catch block.

```csharp
// Unhandled exception - program crashes
int result = int.Parse("not a number");
// FormatException thrown
// No catch block → crash
Console.WriteLine("Won't reach here");
```

Output:
```
Unhandled exception: System.FormatException: 
Input string was not in a correct format.
```

## Exception Handling Flow

```
Program executes normally
    ↓
Exception thrown
    ↓
Search for catch block (current method)
    ↓
Not found → move to calling method
    ↓
Found catch → handle exception
    ↓
Program continues after catch block
```

## Basic Try-Catch

Simplest form to handle exceptions:

```csharp
try {
    // Code that might throw exception
    int value = int.Parse("abc");
    Console.WriteLine(value);
} catch (FormatException) {
    // Handle the exception
    Console.WriteLine("Please enter a valid number");
}

Console.WriteLine("Program continues");
```

Output:
```
Please enter a valid number
Program continues
```

**Flow**:
1. Try block executes
2. Exception thrown during Parse
3. Control jumps to catch block
4. Catch block executes
5. Program continues after try-catch

## Exception vs No Exception

### Without Exception Handling (Crashes)
```csharp
int result = int.Parse("abc");  // FormatException
Console.WriteLine("Won't run");
// Program crashes, won't print
```

### With Exception Handling (Continues)
```csharp
try {
    int result = int.Parse("abc");  // FormatException
} catch (FormatException) {
    Console.WriteLine("Invalid input");  // Prints this
}
Console.WriteLine("Program continues");  // And this
```

Output:
```
Invalid input
Program continues
```

## Why Use Exceptions?

### 1. Graceful Degradation
Instead of crashing, handle errors gracefully:

```csharp
// Without exception handling
var number = int.Parse(userInput);  // Crashes if invalid

// With exception handling
if (!int.TryParse(userInput, out var number)) {
    Console.WriteLine("Invalid input");
    number = 0;
}
```

### 2. Centralized Error Handling
Pass exceptions up to central handler:

```csharp
public decimal GetAccountBalance(int accountId) {
    try {
        return database.GetBalance(accountId);
    } catch (DatabaseException ex) {
        logger.Error($"Database error: {ex.Message}");
        throw;  // Re-throw to caller
    }
}
```

### 3. Resource Cleanup
Ensure cleanup happens even on error:

```csharp
try {
    using (var file = new StreamReader("data.txt")) {
        ProcessFile(file);
    }
} finally {
    // Cleanup happens regardless
}
```

### 4. Separation of Concerns
Keep happy path separate from error handling:

```csharp
// Good: Happy path is clear
try {
    ValidateInput(input);
    ProcessData(input);
    SaveResults(results);
} catch (ValidationException ex) {
    Console.WriteLine($"Invalid input: {ex.Message}");
} catch (ProcessingException ex) {
    Console.WriteLine($"Processing failed: {ex.Message}");
}
```

## Exception vs Logging

### Exceptions
- For unexpected, recoverable errors
- Interrupt normal flow
- Handled at specific points

### Logging
- For informational events
- Track what happens
- Usually doesn't interrupt flow

```csharp
// Both approaches
try {
    logger.Info("Processing started");
    ProcessData();
    logger.Info("Processing completed");
} catch (Exception ex) {
    logger.Error($"Processing failed: {ex.Message}");
}
```

## Exception Propagation

Exceptions bubble up the call stack:

```csharp
public void Method1() {
    try {
        Method2();
    } catch (Exception ex) {
        Console.WriteLine("Caught in Method1");
    }
}

public void Method2() {
    Method3();  // Exception propagates here
}

public void Method3() {
    throw new Exception("Error!");  // Thrown here
}

// Execution:
// Method3 throws
// Method2 doesn't handle → propagates
// Method1 catches
// Output: "Caught in Method1"
```

## Exception Context

When an exception occurs, it captures context:

```csharp
try {
    var x = 10 / int.Parse(userInput);
} catch (DivideByZeroException) {
    // Knows it was division by zero
} catch (FormatException) {
    // Knows it was parse error
}
```

## Best Practices

✓ Handle exceptions at appropriate level
✓ Catch specific exceptions before general ones
✓ Use exceptions for exceptional conditions
✓ Provide meaningful error messages
✓ Log exceptions for debugging

## Anti-Patterns

❌ Catch and ignore
```csharp
try {
    riskyOperation();
} catch {
    // Silent failure - bad!
}
```

❌ Throw bare Exception
```csharp
throw new Exception("Error");  // Not specific
```

❌ Catch everything
```csharp
try {
    // Large block of code
    method1();
    method2();
    method3();
} catch (Exception) {
    // Which one failed?
}
```

## Exception Types Overview

```csharp
// Input validation
FormatException  // Invalid format (int.Parse)

// Array/Collection
IndexOutOfRangeException  // Index out of bounds
ArgumentException  // Invalid argument

// Null reference
NullReferenceException  // Accessing null

// Math
DivideByZeroException  // Division by zero
OverflowException  // Value overflow

// File I/O
FileNotFoundException  // File not found
IOException  // I/O error
```

## Summary

- Exceptions are error objects thrown during execution
- Try-catch blocks handle exceptions gracefully
- Exceptions propagate up the call stack
- Specific exceptions before general ones
- Use finally for guaranteed cleanup
- Log or handle exceptions, never ignore silently

---

## Next Steps

1. Learn Common Exceptions
2. Understand Exception Hierarchy
3. Master Try-Catch patterns
4. Learn Custom Exceptions
5. Study Best Practices
