# Exception Handling Interview Questions - Easy Level

## Question 1: What is an Exception and Why Do We Need Them?

### Question
Explain what an exception is in C# and describe why exception handling is important in a program.

### Answer

An exception is an object that represents an error or exceptional condition that occurs during program execution. It interrupts the normal flow of the program.

**Why We Need Exceptions:**

1. **Graceful Error Handling** - Programs don't crash abruptly
2. **Separation of Concerns** - Error handling separate from normal logic
3. **Clear Error Information** - Tells you what went wrong and where
4. **Resource Cleanup** - Finally blocks ensure resources are freed
5. **Debugging** - Stack trace shows execution path

**Example:**
```csharp
// Without exception handling - program crashes
int result = int.Parse("abc");  // Unhandled exception

// With exception handling - continues running
try {
    int result = int.Parse("abc");
} catch (FormatException) {
    Console.WriteLine("Invalid number");
}
Console.WriteLine("Program continues");  // Prints this
```

### Follow-up
- What's the difference between an exception and an error?
- Can you give examples of when exceptions would be thrown?

---

## Question 2: Explain the Try-Catch-Finally Structure

### Question
Write code showing the structure of try-catch-finally and explain when each block executes.

### Answer

```csharp
try {
    // Code that might throw exception
    risky Operation();
} catch (SpecificException ex) {
    // Handles SpecificException
} finally {
    // ALWAYS executes - cleanup code
    CloseResources();
}
```

**Block Execution:**

1. **Try Block** - Executes first, attempts risky operation
2. **Catch Block** - Executes if exception matches type
3. **Finally Block** - ALWAYS executes, regardless of what happened

**Execution Scenarios:**
```csharp
// No exception
try { Console.WriteLine("1"); }      // Prints: 1
catch { Console.WriteLine("2"); }    // Skipped
finally { Console.WriteLine("3"); }  // Prints: 3
// Output: 1, 3

// Exception occurs
try { throw new Exception(); }       // Throws
catch { Console.WriteLine("2"); }    // Prints: 2
finally { Console.WriteLine("3"); }  // Prints: 3
// Output: 2, 3

// With return in catch
try { throw new Exception(); }
catch { Console.WriteLine("2"); return; }  // Prints: 2, then returns
finally { Console.WriteLine("3"); }        // Prints: 3 before return!
// Output: 2, 3 - Finally ALWAYS runs!
```

### Follow-up
- What if an exception occurs in the catch block?
- What if there's no matching catch block?

---

## Question 3: What Are Common Exception Types in C#?

### Question
List and describe 5 common exception types you encounter in C#.

### Answer

| Exception | Cause | Example |
|-----------|-------|---------|
| **FormatException** | Invalid format during parsing | `int.Parse("abc")` |
| **ArgumentNullException** | Null argument passed | `SetName(null)` |
| **IndexOutOfRangeException** | Array index out of bounds | `arr[100]` when arr.Length=5 |
| **DivideByZeroException** | Integer division by zero | `10 / 0` |
| **InvalidOperationException** | Invalid state for operation | `list.First()` on empty list |

**Code Examples:**
```csharp
// FormatException
try {
    int num = int.Parse("invalid");
} catch (FormatException) {
    Console.WriteLine("Number format invalid");
}

// ArgumentNullException
if (user == null) {
    throw new ArgumentNullException(nameof(user));
}

// IndexOutOfRangeException
int[] arr = new int[5];
int value = arr[10];  // Throws

// DivideByZeroException
int result = 10 / 0;  // Throws

// InvalidOperationException
var first = emptyList.First();  // Throws
```

### Follow-up
- How would you prevent FormatException?
- What's the difference between FormatException and ArgumentException?

---

## Question 4: What is the Purpose of the Finally Block?

### Question
Explain what the finally block does and give examples of when you'd use it.

### Answer

**Purpose**: The finally block is guaranteed to execute whether an exception occurs or not. Used for cleanup operations.

**Guaranteed Execution:**
```csharp
try {
    return 42;  // Even returns don't skip finally!
} finally {
    Console.WriteLine("Finally still runs");
}
```

**Common Uses:**

1. **Closing Files**
```csharp
StreamReader reader = null;
try {
    reader = new StreamReader("file.txt");
    string content = reader.ReadToEnd();
} finally {
    reader?.Dispose();  // Always runs
}
```

2. **Releasing Database Connections**
```csharp
try {
    connection.Open();
    ExecuteQuery();
} finally {
    connection?.Close();
}
```

3. **Unlocking Resources**
```csharp
mutex.WaitOne();
try {
    CriticalSection();
} finally {
    mutex.ReleaseMutex();  // Always releases lock
}
```

**Why Not Just Dispose in Catch?**
```csharp
// BAD - Reader never closed if no exception
try {
    reader = new StreamReader("file.txt");
    string content = reader.ReadToEnd();
} catch {
    reader?.Dispose();
}  // Reader leaks!

// GOOD - Always closed
finally {
    reader?.Dispose();
}
```

### Follow-up
- What if an exception occurs in finally?
- Should you throw exceptions in finally blocks?

---

## Question 5: What is a Guard Clause?

### Question
Explain guard clauses and show an example of how to use them.

### Answer

A guard clause is an early return/exit when preconditions aren't met. It validates input and fails fast.

**Purpose**: Prevent operations on invalid data by checking conditions upfront.

**Example:**
```csharp
// Without guard clause - nested
public void ProcessUser(User user) {
    if (user != null) {
        if (user.IsActive) {
            if (user.Age >= 18) {
                // 3 levels deep - process
            }
        }
    }
}

// With guard clause - flat
public void ProcessUser(User user) {
    // Validate preconditions first
    if (user == null) {
        throw new ArgumentNullException(nameof(user));
    }
    
    if (!user.IsActive) {
        throw new InvalidOperationException("User not active");
    }
    
    if (user.Age < 18) {
        throw new ArgumentException("Must be 18+");
    }
    
    // Safe to process - all conditions met
    ValidateUser(user);
    SaveUser(user);
}
```

**Benefits:**
- Flat, readable code
- Early detection of errors
- Clear preconditions documented
- No deep nesting

### Follow-up
- How do guard clauses relate to exceptions?
- When should you use guard clauses vs if statements?

---

## Question 6: What Does throw; vs throw ex; Do?

### Question
Explain the difference between `throw;` and `throw ex;` in a catch block.

### Answer

**The Difference:**

```csharp
// Using 'throw;' - GOOD
try {
    MethodC();  // Exception thrown here
} catch (Exception ex) {
    logger.Error("Error", ex);
    throw;      // Re-throws original exception
}

// Using 'throw ex;' - BAD
try {
    MethodC();  // Exception thrown here
} catch (Exception ex) {
    logger.Error("Error", ex);
    throw ex;   // Throws at THIS line
}
```

**Stack Trace Comparison:**

With `throw;`:
```
at MethodC() in C:\Program.cs:line 42
at MethodB() in C:\Program.cs:line 30
at MethodA() in C:\Program.cs:line 15
```

With `throw ex;`:
```
at MethodA() in C:\Program.cs:line 20
```
*Lost information about where it came from!*

**Why It Matters:**
- `throw;` preserves original stack trace
- `throw ex;` loses debugging information
- Production bugs become unsolvable

**Always Use `throw;`:**
```csharp
try {
    operation();
} catch (Exception ex) {
    logger.Error("Failed", ex);
    throw;  // Preserves where error actually occurred
}
```

### Follow-up
- When would you use throw vs throw ex?
- How do you wrap an exception with context?

---

## Question 7: How Do You Create a Basic Custom Exception?

### Question
Show how to create a simple custom exception class.

### Answer

**Basic Custom Exception:**
```csharp
public class InvalidUserException : Exception {
    public InvalidUserException(string message) 
        : base(message) { }
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

**With Inner Exception Support:**
```csharp
public class DataAccessException : Exception {
    public DataAccessException(string message) 
        : base(message) { }
    
    public DataAccessException(string message, Exception innerException)
        : base(message, innerException) { }
}

// Usage - preserving original exception
try {
    database.Save();
} catch (SqlException ex) {
    throw new DataAccessException("Failed to save", ex);
}
```

**Naming Convention:**
- Always end with "Exception"
- Describe the error condition
- Examples: `ValidationException`, `PaymentException`, `InvalidOrderException`

### Follow-up
- Should you add properties to custom exceptions?
- How many custom exceptions should you create?

---

## Question 8: When Should You Use TryParse Instead of Try-Catch?

### Question
Explain when to use TryParse instead of catching FormatException.

### Answer

**TryParse for Expected Failures:**
```csharp
// User input - expected failure
string userInput = Console.ReadLine();

// Good - TryParse
if (int.TryParse(userInput, out int number)) {
    ProcessNumber(number);
} else {
    Console.WriteLine("Invalid number");
}

// Bad - Exception for expected failure
try {
    int number = int.Parse(userInput);
    ProcessNumber(number);
} catch (FormatException) {
    Console.WriteLine("Invalid number");
}
```

**Why TryParse is Better:**
1. **Performance** - No exception overhead
2. **Readability** - Clear intent
3. **Efficiency** - Designed for this pattern
4. **Semantics** - This is normal flow, not error

**When to Use Try-Catch:**
```csharp
// Unexpected failure - use exception
try {
    database.Save();  // Should always work
} catch (DatabaseException ex) {
    logger.Error("Database error", ex);
    throw;
}
```

**Rule of Thumb:**
- **Expected failures** (user input, file not found) → TryParse/TryGetValue
- **Unexpected failures** (database errors) → Try-catch

### Follow-up
- Is exception handling slower than TryParse?
- Are there other Try* methods besides TryParse?

---

## Question 9: What Does ObjectDisposedException Mean?

### Question
Explain what ObjectDisposedException is and when it occurs.

### Answer

**Definition**: ObjectDisposedException occurs when you try to use a disposed object.

**Example:**
```csharp
StreamReader reader = new StreamReader("file.txt");
reader.Dispose();  // Dispose the reader

string line = reader.ReadLine();  
// ObjectDisposedException - reader is already disposed!
```

**Why It Occurs:**
```csharp
using (var reader = new StreamReader("file.txt")) {
    // reader is valid here
}  // Dispose called automatically

// reader is disposed here
string line = reader.ReadLine();  
// ObjectDisposedException
```

**Prevention:**
```csharp
public class FileReader : IDisposable {
    private bool disposed = false;
    
    public string ReadLine() {
        if (disposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
        // Safe to read
        return reader.ReadLine();
    }
    
    public void Dispose() {
        disposed = true;
    }
}
```

**Best Practice:**
```csharp
// Use 'using' to avoid
using (var reader = new StreamReader("file.txt")) {
    string line = reader.ReadLine();
}  // Can't access reader here
```

### Follow-up
- How do you prevent ObjectDisposedException?
- Should you check disposed state in all methods?

---

## Question 10: Explain the Purpose of the Using Statement

### Question
What does the using statement do and how is it different from try-finally?

### Answer

**Purpose**: The using statement automatically calls Dispose() on resources.

**Traditional Try-Finally:**
```csharp
StreamReader reader = null;
try {
    reader = new StreamReader("file.txt");
    string content = reader.ReadToEnd();
} finally {
    reader?.Dispose();  // Must remember to dispose
}
```

**Using Statement (Cleaner):**
```csharp
using (StreamReader reader = new StreamReader("file.txt")) {
    string content = reader.ReadToEnd();
}  // Dispose called automatically
```

**C# 8+ Even Simpler:**
```csharp
using StreamReader reader = new StreamReader("file.txt");
string content = reader.ReadToEnd();
// Dispose at end of scope
```

**Key Benefits:**
- More concise code
- Can't forget to dispose
- Automatic even with exceptions
- Works with any IDisposable type

**Equivalent Functionality:**
```csharp
// Using statement
using (var conn = new SqlConnection()) {
    conn.Open();
}

// Is equivalent to
var conn = new SqlConnection();
try {
    conn.Open();
} finally {
    conn?.Dispose();
}
```

### Follow-up
- What types of objects use the using statement?
- Can you nest using statements?
- What happens if Dispose throws an exception?

---

## Summary

**Easy Level Key Points:**
- Exceptions handle errors gracefully
- Try-catch-finally structure ensures cleanup
- Common exceptions help debugging
- Guard clauses validate preconditions
- Use `throw;` to preserve stack traces
- Create custom exceptions for domain logic
- TryParse for expected failures
- Using statements guarantee cleanup
- ObjectDisposedException = using disposed object

**Next Steps:**
1. Practice writing exception-safe code
2. Understand when to use each pattern
3. Study code examples in production
4. Move to Medium-level questions

---

## Additional Practice Questions

1. What happens if you don't catch an exception?
2. Can a catch block have multiple exception types?
3. How do you log exception information?
4. What's the difference between finally and using?
5. When should you create a custom exception?

