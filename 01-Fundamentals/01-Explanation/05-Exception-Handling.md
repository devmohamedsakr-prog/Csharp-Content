# Exception Handling

## Overview
Exception handling allows gracefully dealing with errors in your code instead of crashing.

---

## What is an Exception?

An exception is an error that occurs during program execution.

```csharp
// Unhandled exception - program crashes
int result = int.Parse("not a number");  // FormatException
Console.WriteLine("This won't execute");

// Handled exception - program continues
try {
    int result = int.Parse("not a number");
} catch (FormatException) {
    Console.WriteLine("Please enter a valid number");
}
Console.WriteLine("Program continues");
```

---

## Try-Catch Block

Catch and handle exceptions.

```csharp
try {
    // Code that might throw exception
    int x = int.Parse("abc");
} catch (FormatException ex) {
    // Handle FormatException
    Console.WriteLine($"Format error: {ex.Message}");
} catch (OverflowException ex) {
    // Handle OverflowException
    Console.WriteLine($"Overflow error: {ex.Message}");
} catch (Exception ex) {
    // Catch all other exceptions
    Console.WriteLine($"Error: {ex.Message}");
}
```

**Rules**:
- Catch specific exceptions first
- Catch general exceptions last
- Exception must inherit from previous catches

---

## Exception Types

### Common Exceptions

```csharp
// FormatException - invalid format
int num = int.Parse("abc");

// OverflowException - value too large
int big = int.Parse("999999999999999999");

// ArgumentNullException - null argument
string text = null;
text.Length;  // NullReferenceException

// IndexOutOfRangeException - index not in array
int[] arr = new int[5];
int x = arr[10];  // Out of range

// DivideByZeroException - dividing by zero
int result = 10 / 0;

// InvalidOperationException - invalid operation
List<int> list = new List<int>();
int first = list.First();  // No items

// ArgumentException - invalid argument
public void SetAge(int age) {
    if (age < 0) {
        throw new ArgumentException("Age cannot be negative");
    }
}
```

---

## Try-Catch-Finally

Finally block always executes.

```csharp
StreamReader reader = null;

try {
    reader = new StreamReader("file.txt");
    string line = reader.ReadLine();
    Console.WriteLine(line);
} catch (FileNotFoundException ex) {
    Console.WriteLine($"File not found: {ex.Message}");
} catch (IOException ex) {
    Console.WriteLine($"IO error: {ex.Message}");
} finally {
    // Always executes - cleanup code
    reader?.Dispose();
    Console.WriteLine("Cleanup completed");
}

// Output:
// Cleanup completed (always runs)
```

**Use Finally For**:
- Closing file handles
- Releasing resources
- Cleanup code
- Code that must always run

---

## Throwing Exceptions

Create and throw custom exceptions.

```csharp
public class BankAccount {
    private decimal balance;
    
    public void Withdraw(decimal amount) {
        if (amount <= 0) {
            throw new ArgumentException("Amount must be positive");
        }
        
        if (amount > balance) {
            throw new InvalidOperationException("Insufficient funds");
        }
        
        balance -= amount;
    }
}

// Usage
BankAccount account = new BankAccount();
try {
    account.Withdraw(-50);  // Throws ArgumentException
} catch (ArgumentException ex) {
    Console.WriteLine($"Invalid: {ex.Message}");
}

try {
    account.Withdraw(1000);  // Throws InvalidOperationException
} catch (InvalidOperationException ex) {
    Console.WriteLine($"Operation error: {ex.Message}");
}
```

---

## Custom Exceptions

Create your own exception classes.

```csharp
// Custom exception
public class InsufficientFundsException : Exception {
    public decimal RequiredAmount { get; set; }
    public decimal AvailableAmount { get; set; }
    
    public InsufficientFundsException(string message, 
        decimal required, decimal available) 
        : base(message) {
        RequiredAmount = required;
        AvailableAmount = available;
    }
}

// Usage
public void Withdraw(decimal amount) {
    if (amount > balance) {
        throw new InsufficientFundsException(
            "Not enough funds",
            amount,
            balance
        );
    }
}

try {
    account.Withdraw(1000);
} catch (InsufficientFundsException ex) {
    Console.WriteLine($"Need: {ex.RequiredAmount}, Have: {ex.AvailableAmount}");
}
```

---

## Exception Properties

Access exception information.

```csharp
try {
    int result = int.Parse("abc");
} catch (FormatException ex) {
    // Message - human readable error description
    Console.WriteLine(ex.Message);
    // "Input string was not in a correct format."
    
    // StackTrace - where error occurred
    Console.WriteLine(ex.StackTrace);
    
    // InnerException - underlying exception
    if (ex.InnerException != null) {
        Console.WriteLine(ex.InnerException.Message);
    }
}
```

---

## Exception Handling Patterns

### Pattern 1: Try-Parse (Preferred)
```csharp
// Good - no exception thrown
if (int.TryParse(input, out int number)) {
    Console.WriteLine($"Parsed: {number}");
} else {
    Console.WriteLine("Invalid number");
}
```

### Pattern 2: Try-Catch (When Necessary)
```csharp
// When TryParse not available
try {
    int number = int.Parse(input);
    Console.WriteLine($"Parsed: {number}");
} catch (FormatException) {
    Console.WriteLine("Invalid number");
}
```

### Pattern 3: Guard Clauses
```csharp
// Check conditions before operation
public void ProcessOrder(Order order) {
    if (order == null) {
        throw new ArgumentNullException(nameof(order));
    }
    
    if (order.Items.Count == 0) {
        throw new InvalidOperationException("Order has no items");
    }
    
    if (order.Total < 0) {
        throw new InvalidOperationException("Invalid total");
    }
    
    // Process order
}
```

---

## Using Statement (Automatic Cleanup)

Automatically calls Dispose().

```csharp
// Without using - manual cleanup
StreamReader reader = null;
try {
    reader = new StreamReader("file.txt");
    string line = reader.ReadLine();
} finally {
    reader?.Dispose();
}

// With using - automatic cleanup
using (StreamReader reader = new StreamReader("file.txt")) {
    string line = reader.ReadLine();
}  // Dispose called automatically

// C# 8+ - using declaration
using StreamReader reader = new StreamReader("file.txt");
string line = reader.ReadLine();
// Dispose called automatically at end of scope
```

---

## Best Practices

✓ **Catch Specific Exceptions**
```csharp
// Good - specific
try {
    int num = int.Parse(input);
} catch (FormatException ex) {
    Console.WriteLine("Invalid format");
}

// Bad - too general
try {
    int num = int.Parse(input);
} catch (Exception ex) {
    Console.WriteLine("Error");
}
```

✓ **Use Guard Clauses**
```csharp
// Good - validate early
public void SetAge(int age) {
    if (age < 0) throw new ArgumentException("Invalid age");
    // Safe to use age
}

// Less ideal - rely on exceptions
public void SetAge(int age) {
    try {
        if (age < 0) throw new ArgumentException("Invalid age");
    } catch {
        // Handle
    }
}
```

✓ **Don't Swallow Exceptions**
```csharp
// Bad - exception ignored
try {
    risky Operation();
} catch {
    // Silent failure
}

// Good - log or rethrow
try {
    riskyOperation();
} catch (Exception ex) {
    logger.Error($"Operation failed: {ex.Message}");
    throw;  // Re-throw for caller
}
```

✓ **Use Finally for Cleanup**
```csharp
// Good - ensures cleanup
try {
    using (var file = new StreamReader("data.txt")) {
        // Read file
    }
} finally {
    // Additional cleanup if needed
}
```

---

## Exception Flow

```csharp
public void MethodA() {
    try {
        MethodB();
    } catch (Exception ex) {
        Console.WriteLine("Caught in A");
    }
}

public void MethodB() {
    try {
        MethodC();
    } catch (FormatException ex) {
        Console.WriteLine("Caught in B");
        throw;  // Re-throw to A
    }
}

public void MethodC() {
    throw new FormatException("Error in C");
}

// Execution:
// 1. MethodC throws FormatException
// 2. MethodB catches and re-throws
// 3. MethodA catches final exception
// Output: "Caught in A"
```

---

## Common Mistakes

❌ **Empty Catch Block**
```csharp
try {
    riskyOperation();
} catch (Exception) {
    // Silent failure - terrible!
}
```

✓ **Handle or Log**
```csharp
try {
    riskyOperation();
} catch (Exception ex) {
    logger.Error($"Operation failed: {ex.Message}");
}
```

❌ **Too Broad Exception Handling**
```csharp
try {
    int num = int.Parse(input);
    ComplexCalculation(num);
    SaveToDatabase(num);
} catch (Exception ex) {
    // Which operation failed?
    Console.WriteLine("Error");
}
```

✓ **Specific Handling**
```csharp
try {
    int num = int.Parse(input);
} catch (FormatException) {
    Console.WriteLine("Invalid input format");
}

try {
    ComplexCalculation(num);
} catch (ArgumentException ex) {
    Console.WriteLine($"Calculation error: {ex.Message}");
}
```

---

## Quick Summary

- Try-catch handles exceptions gracefully
- Catch specific exceptions before general ones
- Finally always executes
- Throw custom exceptions for validation
- Use guard clauses for prevention
- Use using for automatic resource cleanup
- Log errors instead of silently failing
