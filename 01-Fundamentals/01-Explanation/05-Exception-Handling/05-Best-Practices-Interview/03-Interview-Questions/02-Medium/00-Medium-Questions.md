# Exception Handling Interview Questions - Medium Level

## Question 1: Explain Exception Hierarchy and Catch Order

### Question
What is the exception hierarchy and why does the order of catch blocks matter?

### Answer

**Exception Hierarchy:**
```
Exception (Base)
├── SystemException
│   ├── ArgumentException
│   │   ├── ArgumentNullException
│   │   └── ArgumentOutOfRangeException
│   ├── FormatException
│   ├── DivideByZeroException
│   ├── InvalidOperationException
│   └── IOException
│       ├── FileNotFoundException
│       └── DirectoryNotFoundException
└── ApplicationException (Custom exceptions)
```

**Why Order Matters:**
```csharp
// WRONG - ArgumentNullException never caught
try {
    SetAge(null);
} catch (Exception ex) {
    // Catches ArgumentNullException too
} catch (ArgumentNullException ex) {
    // Never reached!
}

// CORRECT - Specific before general
try {
    SetAge(null);
} catch (ArgumentNullException ex) {
    // Specific exception first
} catch (ArgumentException ex) {
    // More general
} catch (Exception ex) {
    // Catch all
}
```

**Why It Works This Way:**
- `ArgumentNullException` IS-A `ArgumentException`
- `ArgumentException` IS-A `Exception`
- First matching catch block executes
- Derived classes must come before base classes

**Real-World Example:**
```csharp
try {
    ProcessFile(filename);
} catch (FileNotFoundException ex) {
    Console.WriteLine("File not found - ask user to check path");
} catch (DirectoryNotFoundException ex) {
    Console.WriteLine("Directory not found");
} catch (IOException ex) {
    Console.WriteLine("General I/O error");
} catch (Exception ex) {
    logger.Error("Unexpected error", ex);
}
```

### Follow-up
- What if you catch the base class first?
- Can you catch multiple exceptions in one block?

---

## Question 2: How Do You Create a Proper Custom Exception?

### Question
Design a custom exception with properties and proper constructors.

### Answer

**Complete Custom Exception:**
```csharp
public class ValidationException : Exception {
    public string FieldName { get; set; }
    public object InvalidValue { get; set; }
    
    // Constructor with message only
    public ValidationException(string message)
        : base(message) { }
    
    // Constructor with message and inner exception
    public ValidationException(string message, Exception innerException)
        : base(message, innerException) { }
    
    // Constructor with properties
    public ValidationException(
        string fieldName,
        object value,
        string message)
        : base(message) {
        FieldName = fieldName;
        InvalidValue = value;
    }
}

// Usage
public void ValidateAge(int age) {
    if (age < 0 || age > 150) {
        throw new ValidationException(
            "Age",
            age,
            $"Age must be between 0 and 150"
        );
    }
}

try {
    ValidateAge(-5);
} catch (ValidationException ex) {
    Console.WriteLine($"Field: {ex.FieldName}");
    Console.WriteLine($"Value: {ex.InvalidValue}");
    Console.WriteLine($"Error: {ex.Message}");
}
```

**Exception Hierarchy:**
```csharp
// Base domain exception
public abstract class DomainException : Exception {
    protected DomainException(string message) : base(message) { }
}

// Specific domain exceptions
public class UserValidationException : DomainException {
    public UserValidationException(string message) : base(message) { }
}

public class BusinessRuleException : DomainException {
    public string RuleName { get; set; }
    
    public BusinessRuleException(string ruleName, string message)
        : base(message) {
        RuleName = ruleName;
    }
}

// Usage
try {
    ValidateUser(user);
    ApplyBusinessRules(user);
} catch (UserValidationException ex) {
    logger.Warn($"User validation failed: {ex.Message}");
} catch (BusinessRuleException ex) {
    logger.Error($"Business rule violation: {ex.RuleName}");
} catch (DomainException ex) {
    logger.Error($"Domain error: {ex.Message}");
}
```

### Follow-up
- When should you add properties to exceptions?
- How many custom exceptions should you create?
- Should you make exceptions serializable?

---

## Question 3: When Would You Use Guard Clauses vs Exceptions?

### Question
Compare guard clauses and exceptions - when would you use each?

### Answer

**Guard Clauses** - Validate preconditions upfront:
```csharp
public void ProcessData(Data data) {
    // Guard clauses - fail fast
    if (data == null) {
        throw new ArgumentNullException(nameof(data));
    }
    
    if (data.IsEmpty) {
        throw new ArgumentException("Data cannot be empty");
    }
    
    if (!data.IsValid) {
        throw new ArgumentException("Data is invalid");
    }
    
    // Safe to proceed - all conditions checked
    DoWork(data);
}
```

**Exceptions** - Catch unexpected failures:
```csharp
public void SaveData(Data data) {
    ProcessData(data);  // Checked by guard clauses
    
    try {
        database.Save(data);  // Unexpected failure possible
    } catch (DatabaseException ex) {
        logger.Error("Save failed", ex);
        throw;
    }
}
```

**Comparison:**

| Aspect | Guard Clauses | Exceptions |
|--------|--------------|-----------|
| When | Expected failures (input validation) | Unexpected failures |
| Performance | Very fast | Slower if exception thrown |
| Readability | Clear conditions | Exception indicates abnormal |
| Prevention | Prevents invalid calls | Handles unexpected errors |

**Decision Tree:**
```
Is the error condition expected?
├─ Yes (user input, file not found) → Guard clause or TryParse
└─ No (database error, calculation error) → Try-catch exception
```

**Real-World Example:**
```csharp
public class OrderService {
    public void PlaceOrder(OrderRequest request, User customer) {
        // Guard clauses for preconditions
        if (request == null) {
            throw new ArgumentNullException(nameof(request));
        }
        if (customer == null) {
            throw new ArgumentNullException(nameof(customer));
        }
        if (!customer.IsVerified) {
            throw new InvalidOperationException("Customer not verified");
        }
        
        // Exception for unexpected failure
        try {
            ValidateOrder(request);
            ReserveInventory(request);
            ProcessPayment(customer, request.Total);
            CreateOrder(request, customer);
        } catch (InventoryException ex) {
            logger.Error("Inventory error", ex);
            throw new OrderException("Cannot place order", ex);
        } catch (PaymentException ex) {
            logger.Error("Payment error", ex);
            throw new OrderException("Payment failed", ex);
        }
    }
}
```

### Follow-up
- What's the performance difference?
- Can you use both in the same method?
- When should you use guard clauses over if statements?

---

## Question 4: How Do You Properly Re-throw Exceptions?

### Question
Show different ways to re-throw exceptions and explain the differences.

### Answer

**Scenario: Log Error and Re-throw**

**Option 1: Using `throw;` (BEST)**
```csharp
try {
    MethodC();
} catch (Exception ex) {
    logger.Error("Error in MethodC", ex);
    throw;  // Original exception, original stack trace
}

// Stack trace shows:
// at MethodC() ...
// at MethodB() ...
// at MethodA() ...
```

**Option 2: Using `throw ex;` (AVOID)**
```csharp
try {
    MethodC();
} catch (Exception ex) {
    logger.Error("Error in MethodC", ex);
    throw ex;  // Loses original stack trace!
}

// Stack trace shows:
// at MethodA() ...  ← Lost MethodC and MethodB!
```

**Option 3: Wrapping Exception (Preserve Inner)**
```csharp
try {
    database.Save();
} catch (SqlException ex) {
    // Wrap with domain exception, preserve original
    throw new DataAccessException("Failed to save", ex);
}

// Usage - can access both
try {
    service.Save();
} catch (DataAccessException ex) {
    Console.WriteLine($"Outer: {ex.Message}");
    Console.WriteLine($"Inner: {ex.InnerException?.Message}");
}
```

**Option 4: Selective Re-throwing**
```csharp
try {
    operation();
} catch (ValidationException ex) {
    // Log and handle validation errors
    logger.Info("Validation failed", ex);
} catch (Exception ex) {
    // Log unexpected errors and re-throw
    logger.Error("Unexpected error", ex);
    throw;  // Only re-throw unexpected
}
```

**Complete Example:**
```csharp
public class DataRepository {
    public User GetUser(int userId) {
        try {
            return database.GetUser(userId);
        } catch (ArgumentException ex) {
            // Input validation error - don't re-throw
            logger.Info("Invalid user ID", ex);
            return null;
        } catch (SqlException ex) {
            // Unexpected database error - re-throw as domain exception
            logger.Error("Database error", ex);
            throw new DataAccessException("Failed to get user", ex);
        }
    }
}
```

### Follow-up
- When would you throw a new exception vs re-throw?
- How does `throw;` preserve stack trace?
- What information do you get from InnerException?

---

## Question 5: Explain Exception Filtering with When Clauses

### Question
Show how to use when clauses to filter exceptions and give a practical example.

### Answer

**Basic Exception Filtering:**
```csharp
try {
    file.Open();
} catch (IOException ex) when (IsFileLocked(ex)) {
    RetryOperation();  // Specific handling for locked files
} catch (IOException ex) when (IsAccessDenied(ex)) {
    ReportAccessDenied();  // Different handling for access denied
} catch (IOException ex) {
    throw;  // Other IO errors propagate
}

private bool IsFileLocked(IOException ex) {
    // Check error code
    return ex.HResult == -2147024891;  // File in use error
}

private bool IsAccessDenied(IOException ex) {
    return ex.HResult == -2147024897;  // Access denied error
}
```

**Parameter-Based Filtering:**
```csharp
try {
    ProcessUser(user);
} catch (ArgumentException ex) when (ex.ParamName == "id") {
    Console.WriteLine("Invalid ID");
} catch (ArgumentException ex) when (ex.ParamName == "name") {
    Console.WriteLine("Invalid name");
} catch (ArgumentException) {
    Console.WriteLine("Invalid argument");
}
```

**Value-Based Filtering:**
```csharp
try {
    Withdraw(amount);
} catch (InvalidOperationException ex) when (IsInsufficientFunds(ex)) {
    Console.WriteLine("Not enough money");
} catch (InvalidOperationException ex) when (IsAccountLocked(ex)) {
    Console.WriteLine("Account locked");
} catch (InvalidOperationException) {
    throw;
}

private bool IsInsufficientFunds(InvalidOperationException ex) {
    return ex.Message.Contains("Insufficient");
}

private bool IsAccountLocked(InvalidOperationException ex) {
    return ex.Message.Contains("Locked");
}
```

**Real-World API Example:**
```csharp
public async Task<ApiResponse> CallExternalApi(string url) {
    try {
        return await client.GetAsync(url);
    } catch (HttpRequestException ex) when (ex.StatusCode == 404) {
        return new ApiResponse { Found = false };
    } catch (HttpRequestException ex) when (ex.StatusCode == 503) {
        logger.Warn("Service unavailable - retry later");
        throw;
    } catch (TimeoutException ex) when (ex.InnerException is OperationCanceledException) {
        logger.Warn("Request timeout");
        throw;
    } catch (Exception ex) {
        logger.Error("Unexpected API error", ex);
        throw;
    }
}
```

**Advantages:**
- Cleaner than nested if-else
- Specific handling per condition
- Declarative code style
- Better readability

### Follow-up
- Can you use multiple conditions in when clause?
- What happens if when expression throws?
- Should you use when or nested if-catch?

---

## Question 6: How Do You Implement IDisposable Correctly?

### Question
Show the complete IDisposable pattern implementation with explanation.

### Answer

**Full IDisposable Implementation:**
```csharp
public class ResourceManager : IDisposable {
    private IntPtr unmanagedResource;  // Unmanaged resource
    private StreamWriter managedResource;  // Managed resource
    private bool disposed = false;
    
    // Constructor
    public ResourceManager() {
        unmanagedResource = AllocateNativeMemory();
        managedResource = new StreamWriter("file.txt");
    }
    
    // Public Dispose
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);  // Tell GC not to call finalizer
    }
    
    // Protected Dispose (for inheritance)
    protected virtual void Dispose(bool disposing) {
        if (!disposed) {
            if (disposing) {
                // Clean up managed resources
                managedResource?.Dispose();
            }
            
            // Clean up unmanaged resources
            if (unmanagedResource != IntPtr.Zero) {
                FreeNativeMemory(unmanagedResource);
                unmanagedResource = IntPtr.Zero;
            }
            
            disposed = true;
        }
    }
    
    // Finalizer - safety net
    ~ResourceManager() {
        Dispose(false);
    }
    
    // Check if disposed
    public void UseResource() {
        if (disposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
        // Use resource safely
    }
}

// Usage
using (var manager = new ResourceManager()) {
    manager.UseResource();
}  // Dispose called automatically
```

**Key Components:**

1. **disposed flag** - Prevent double dispose
2. **Dispose(bool)** - Separate managed/unmanaged cleanup
3. **Dispose()** - Call Dispose(true) and GC.SuppressFinalize
4. **Finalizer** - Call Dispose(false) as safety net
5. **ObjectDisposedException** - Check before use

**Checklist:**
- [ ] Implements IDisposable
- [ ] Has private `disposed` field
- [ ] Checks disposed state in Dispose(bool)
- [ ] Cleans up managed resources
- [ ] Cleans up unmanaged resources
- [ ] Has virtual Dispose(bool) for inheritance
- [ ] Public Dispose calls Dispose(true) and GC.SuppressFinalize
- [ ] Has finalizer calling Dispose(false)
- [ ] Public methods check disposed state

### Follow-up
- What's the difference between Dispose(true) and Dispose(false)?
- Why call GC.SuppressFinalize?
- What happens if you don't implement the pattern correctly?

---

## Question 7: Design Exception Handling for a Web API

### Question
Design exception handling for a REST API that returns appropriate HTTP status codes.

### Answer

**Exception Hierarchy:**
```csharp
public abstract class ApiException : Exception {
    public abstract int StatusCode { get; }
    public string ErrorCode { get; protected set; }
    
    protected ApiException(string message) : base(message) { }
}

public class ValidationException : ApiException {
    public override int StatusCode => 400;  // Bad Request
    public List<string> Errors { get; set; }
    
    public ValidationException(List<string> errors)
        : base("Validation failed") {
        ErrorCode = "VALIDATION_ERROR";
        Errors = errors;
    }
}

public class NotFoundException : ApiException {
    public override int StatusCode => 404;  // Not Found
    
    public NotFoundException(string resource)
        : base($"{resource} not found") {
        ErrorCode = "NOT_FOUND";
    }
}

public class ConflictException : ApiException {
    public override int StatusCode => 409;  // Conflict
    
    public ConflictException(string message)
        : base(message) {
        ErrorCode = "CONFLICT";
    }
}

public class InternalException : ApiException {
    public override int StatusCode => 500;  // Internal Server Error
    
    public InternalException(Exception inner)
        : base("Internal server error", inner) {
        ErrorCode = "INTERNAL_ERROR";
    }
}
```

**API Controller with Handling:**
```csharp
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase {
    [HttpGet("{id}")]
    public IActionResult GetUser(int id) {
        try {
            if (id <= 0) {
                throw new ValidationException(
                    new List<string> { "User ID must be positive" }
                );
            }
            
            var user = repository.GetUser(id);
            if (user == null) {
                throw new NotFoundException("User");
            }
            
            return Ok(user);
        } catch (ApiException ex) {
            return StatusCode(
                ex.StatusCode,
                new {
                    code = ex.ErrorCode,
                    message = ex.Message,
                    errors = (ex as ValidationException)?.Errors
                }
            );
        } catch (Exception ex) {
            logger.Error("Unexpected error", ex);
            return StatusCode(500, new {
                code = "INTERNAL_ERROR",
                message = "An unexpected error occurred"
            });
        }
    }
}
```

**Exception Middleware:**
```csharp
public class ExceptionMiddleware {
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionMiddleware> logger;
    
    public async Task InvokeAsync(HttpContext context) {
        try {
            await next(context);
        } catch (Exception ex) {
            logger.Error("Unhandled exception", ex);
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception) {
        context.Response.ContentType = "application/json";
        
        var response = exception switch {
            ValidationException ve => (ve.StatusCode, ve.ErrorCode),
            NotFoundException nfe => (nfe.StatusCode, nfe.ErrorCode),
            ConflictException ce => (ce.StatusCode, ce.ErrorCode),
            _ => (500, "INTERNAL_ERROR")
        };
        
        context.Response.StatusCode = response.Item1;
        return context.Response.WriteAsJsonAsync(new {
            code = response.Item2,
            message = exception.Message
        });
    }
}
```

### Follow-up
- Should you catch exceptions at controller level or globally?
- How do you handle logging across the API?
- What HTTP status codes should each exception return?

---

## Question 8: Explain Exception Propagation in Call Stack

### Question
Trace exception propagation and explain how it works across method calls.

### Answer

**Propagation Example:**
```csharp
public void Main() {
    MethodA();  // Calling A
}

public void MethodA() {
    try {
        MethodB();  // Calling B
    } catch (Exception ex) {
        Console.WriteLine($"A caught: {ex.Message}");
    }
}

public void MethodB() {
    MethodC();  // Calling C, no try-catch
}

public void MethodC() {
    throw new InvalidOperationException("Error!");  // Throws here
}
```

**Propagation Flow:**
```
Main
  └─ MethodA (has try-catch)
      └─ MethodB (no handler)
          └─ MethodC (throws)

Execution:
1. MethodC throws InvalidOperationException
2. MethodB has no catch → exception propagates up
3. MethodA has catch → exception caught
4. Execution continues after try-catch
```

**Stack Trace Analysis:**
```
at MethodC() in C:\Program.cs:line 42
at MethodB() in C:\Program.cs:line 30
at MethodA() in C:\Program.cs:line 15
at Main() in C:\Program.cs:line 5
```

**Complex Example: Multiple Exception Types:**
```csharp
public void Process() {
    try {
        LoadData();  // May throw FormatException
        ValidateData();  // May throw ValidationException
        SaveData();  // May throw IOException
    } catch (FormatException ex) {
        logger.Error("Format error", ex);
    } catch (ValidationException ex) {
        logger.Error("Validation error", ex);
    } catch (IOException ex) {
        logger.Error("IO error", ex);
    }
}

private void LoadData() {
    throw new FormatException("Invalid format");  // Type 1
}

private void ValidateData() {
    throw new ValidationException("Invalid data");  // Type 2
}

private void SaveData() {
    throw new IOException("Save failed");  // Type 3
}
```

**Partial Handling:**
```csharp
public void MethodA() {
    try {
        MethodB();
    } catch (SpecificException ex) {
        // Handle specific type
        Console.WriteLine("Handled specific");
    }
    // If different exception, propagates to caller
}

public void MethodB() {
    throw new OtherException();  // Not caught by MethodA
}

// OtherException propagates to Main/caller
```

### Follow-up
- What happens if exception isn't caught anywhere?
- Can you catch the same exception at multiple levels?
- How do you trace exceptions in production?

---

## Question 9: When Should You Use Try-Catch vs Using Statement?

### Question
Compare try-catch-finally with using statement and explain when to use each.

### Answer

**Try-Catch-Finally:**
```csharp
// Manual resource management
StreamReader reader = null;
try {
    reader = new StreamReader("file.txt");
    string content = reader.ReadToEnd();
    return content;
} catch (FileNotFoundException) {
    Console.WriteLine("File not found");
} finally {
    reader?.Dispose();  // Must remember
}
```

**Using Statement:**
```csharp
// Automatic resource management
using (StreamReader reader = new StreamReader("file.txt")) {
    string content = reader.ReadToEnd();
    return content;
}  // Dispose automatic

// C# 8+ even simpler
using StreamReader reader = new StreamReader("file.txt");
string content = reader.ReadToEnd();
return content;
```

**Comparison:**

| Aspect | Try-Catch-Finally | Using |
|--------|-------------------|-------|
| Cleanup | Manual | Automatic |
| Conciseness | Verbose | Concise |
| Error prone | Easy to forget | Safe |
| Multiple resources | Nested blocks | Stack handles |
| Exception handling | Explicit | Can combine |

**When to Use Using:**
```csharp
// 1. Simple resource cleanup
using (var conn = new SqlConnection(connStr)) {
    conn.Open();
    ExecuteQuery();
}

// 2. Multiple resources
using var file1 = new StreamReader("file1.txt");
using var file2 = new StreamReader("file2.txt");
ProcessBoth(file1, file2);

// 3. Nested resources
using (var outer = new Resource1()) {
    using (var inner = new Resource2()) {
        UseResources();
    }
}
```

**When to Use Try-Catch-Finally:**
```csharp
// 1. Non-IDisposable resource with special cleanup
lock lockObj;
try {
    AcquireLock(lockObj);
    CriticalSection();
} finally {
    ReleaseLock(lockObj);
}

// 2. Complex exception handling
StreamReader reader = null;
try {
    reader = new StreamReader("file.txt");
    string content = reader.ReadToEnd();
} catch (FileNotFoundException) {
    return "default";
} catch (IOException ex) {
    logger.Error("Read error", ex);
    throw;
} finally {
    reader?.Dispose();
}

// 3. Additional cleanup beyond Dispose
try {
    database.Execute();
} finally {
    database.Close();
    logger.Info("Database closed");
}
```

**Combined Pattern:**
```csharp
try {
    using (var conn = new SqlConnection(connStr)) {
        conn.Open();
        var command = new SqlCommand(sql, conn);
        command.ExecuteReader();
    }
} catch (SqlException ex) {
    logger.Error("Database error", ex);
    throw;
} finally {
    logger.Info("Database operation completed");
}
```

### Follow-up
- What's the difference in IL generated?
- Can you use both together?
- Which is better for performance?

---

## Summary

**Medium Level Key Points:**
- Exception hierarchy determines catch order
- Create custom exceptions with properties
- Guard clauses for preconditions, exceptions for failures
- Use `throw;` to preserve stack traces
- When clauses filter exceptions effectively
- IDisposable pattern manages resources correctly
- Design exception handling for specific scenarios
- Understand exception propagation
- Using statement preferred over try-finally
- Combine patterns appropriately

**Next Steps:**
1. Practice designing exception hierarchies
2. Implement IDisposable pattern correctly
3. Build error handling for real scenarios
4. Move to Hard-level questions

