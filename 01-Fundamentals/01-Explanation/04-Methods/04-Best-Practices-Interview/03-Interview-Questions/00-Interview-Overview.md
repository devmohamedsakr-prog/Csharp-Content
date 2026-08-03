# Method Interview Questions

## Overview

Common interview questions about methods, organized by difficulty level.

## How to Use This Guide

- **Easy (5 questions)**: Fundamentals and definitions, ~5-10 minutes per answer
- **Medium (5 questions)**: Problem-solving and application, ~15-20 minutes per answer
- **Hard (5 questions)**: Design patterns and edge cases, ~25-35 minutes per answer

## EASY QUESTIONS

### 1. What is a method? Explain with an example.

**Answer:**
A method is a reusable block of code that performs a specific task. Methods help organize code, reduce repetition, and make programs more maintainable.

```csharp
// Definition
public int Add(int a, int b)
{
    return a + b;
}

// Usage
int result = Add(5, 3);  // 8
```

**Key Points:**
- Methods encapsulate logic
- Can be reused multiple times
- Have parameters and return values
- Improve code organization

### 2. What's the difference between a method parameter and a return value?

**Answer:**

**Parameters** are inputs to a method:
```csharp
public void Greet(string name)  // 'name' is a parameter
{
    Console.WriteLine($"Hello, {name}");
}
```

**Return values** are outputs from a method:
```csharp
public int Add(int a, int b)
{
    return a + b;  // returns sum
}
```

**Key Differences:**
- Parameters: data going IN
- Return values: data coming OUT
- A method can have multiple parameters but only one return type

### 3. What does "void" mean in a method signature?

**Answer:**
`void` means the method doesn't return anything:

```csharp
public void PrintMessage(string message)
{
    Console.WriteLine(message);  // No return statement
}

// Usage
PrintMessage("Hello");  // Can't assign return value
```

**Key Points:**
- `void` = method performs action but returns nothing
- No `return` statement needed (or `return;` to exit early)
- Call method only for its side effects

### 4. What is method overloading?

**Answer:**
Method overloading allows multiple methods with the same name but different parameters:

```csharp
public class Calculator
{
    // Overload 1: Two integers
    public int Add(int a, int b)
    {
        return a + b;
    }
    
    // Overload 2: Two doubles
    public double Add(double a, double b)
    {
        return a + b;
    }
    
    // Overload 3: Three parameters
    public int Add(int a, int b, int c)
    {
        return a + b + c;
    }
}

// Usage
calc.Add(5, 3);           // Calls first overload
calc.Add(5.5, 3.2);       // Calls second overload
calc.Add(1, 2, 3);        // Calls third overload
```

**Key Points:**
- Same method name, different signatures
- Determined by parameter type or count
- Improves readability

### 5. What is the difference between ref and out parameters?

**Answer:**

| Feature | ref | out |
|---------|-----|-----|
| Initialization | Must initialize before calling | Not required |
| Assignment | Method may or may not assign | Method must assign |
| Use Case | Modify existing value | Return multiple values |

```csharp
// ref - parameter must be initialized
public void Increment(ref int value)
{
    value++;
}
int x = 5;
Increment(ref x);  // Must pass initialized variable
// x is now 6

// out - parameter must be assigned in method
public bool TryParse(string input, out int result)
{
    result = 0;  // Must assign
    return int.TryParse(input, out result);
}
if (TryParse("42", out int number))
{
    Console.WriteLine(number);  // 42
}
```

---

## MEDIUM QUESTIONS

### 1. How would you refactor a method that does too many things?

**Answer:**
Apply Single Responsibility Principle - break into smaller methods:

**Before (Bad):**
```csharp
public void ProcessOrder(Order order)
{
    // Validate
    if (order == null || order.Items.Count == 0)
        throw new Exception("Invalid order");
    
    // Calculate
    decimal total = 0;
    foreach (var item in order.Items)
        total += item.Price * item.Quantity;
    
    // Apply discount
    if (total > 100)
        total *= 0.9m;
    
    // Save
    _database.SaveOrder(order);
    order.Total = total;
    _database.SaveOrderTotal(order);
    
    // Notify
    SendOrderConfirmation(order);
}
```

**After (Good):**
```csharp
public void ProcessOrder(Order order)
{
    ValidateOrder(order);
    CalculateOrderTotal(order);
    SaveOrder(order);
    NotifyCustomer(order);
}

private void ValidateOrder(Order order)
{
    if (order == null || order.Items.Count == 0)
        throw new ArgumentException("Invalid order");
}

private void CalculateOrderTotal(Order order)
{
    decimal total = order.Items.Sum(i => i.Price * i.Quantity);
    if (total > 100)
        total *= 0.9m;
    order.Total = total;
}

private void SaveOrder(Order order)
{
    _database.SaveOrder(order);
}

private void NotifyCustomer(Order order)
{
    SendOrderConfirmation(order);
}
```

**Key Benefits:**
- Each method has single responsibility
- Easier to test
- More readable and maintainable
- Functions can be reused

### 2. Explain recursion and when to use it.

**Answer:**
Recursion is when a method calls itself. Used for problems with recursive structure:

```csharp
// Example: Factorial
public int Factorial(int n)
{
    // Base case - stop recursion
    if (n <= 1)
        return 1;
    
    // Recursive case
    return n * Factorial(n - 1);
}

Factorial(5);  // 5 * 4 * 3 * 2 * 1 = 120
```

**When to Use:**
- Tree/graph traversal
- Divide and conquer
- Natural recursive structures (file systems, hierarchies)

**When to Avoid:**
- Simple loops (use iteration)
- Deep recursion (stack overflow risk)
- Performance-critical code

**Performance Optimization (Memoization):**
```csharp
public int FibonacciMemo(int n, Dictionary<int, int> memo)
{
    if (n <= 1)
        return n;
    
    if (memo.ContainsKey(n))
        return memo[n];
    
    int result = FibonacciMemo(n - 1, memo) + FibonacciMemo(n - 2, memo);
    memo[n] = result;
    return result;
}
```

### 3. How do you handle null references safely in methods?

**Answer:**
Multiple strategies:

**Strategy 1: Null Coalescing**
```csharp
public string GetUserName(User? user)
{
    return user?.Name ?? "Unknown";
}
```

**Strategy 2: Guard Clauses**
```csharp
public string GetUserName(User? user)
{
    if (user == null)
        return "Unknown";
    
    return user.Name;
}
```

**Strategy 3: Throw Exception**
```csharp
public string GetUserName(User user)
{
    ArgumentNullException.ThrowIfNull(user);
    return user.Name;
}
```

**Strategy 4: Return null**
```csharp
public string? GetUserName(User? user)
{
    return user?.Name;
}
```

**Best Practice:**
- Use nullable types (`?`) in signature
- Validate early (fail fast)
- Be explicit about null handling
- Document null possibilities

### 4. How would you design a method to safely parse user input?

**Answer:**

```csharp
// TryParse Pattern
public bool TryParseAge(string input, out int age)
{
    age = 0;
    
    // Validate input
    if (string.IsNullOrWhiteSpace(input))
        return false;
    
    // Try parse
    if (!int.TryParse(input, out int parsedAge))
        return false;
    
    // Validate range
    if (parsedAge < 0 || parsedAge > 150)
        return false;
    
    age = parsedAge;
    return true;
}

// Usage
if (TryParseAge(userInput, out int age))
{
    // Valid age
    ProcessAge(age);
}
else
{
    // Invalid - show error
    ShowError("Please enter valid age (0-150)");
}
```

**Key Principles:**
- Never trust user input
- Validate format, range, and constraints
- Use TryParse pattern over exceptions
- Provide clear error feedback

### 5. Explain the difference between static and instance methods.

**Answer:**

**Static Methods:**
- Belong to class, not instances
- Called on class name
- Can't access instance fields
- Used for utility operations

```csharp
public class Calculator
{
    public static int Add(int a, int b)
    {
        return a + b;
    }
}

// Usage
int result = Calculator.Add(5, 3);  // Called on class
```

**Instance Methods:**
- Belong to instances
- Called on object
- Can access instance fields
- Used for object operations

```csharp
public class BankAccount
{
    private decimal balance;
    
    public void Deposit(decimal amount)
    {
        balance += amount;  // Accesses instance field
    }
}

// Usage
var account = new BankAccount();
account.Deposit(100);  // Called on instance
```

**Key Differences:**

| Feature | Static | Instance |
|---------|--------|----------|
| Called on | Class | Object |
| Access instance data | No | Yes |
| Needs new | No | Yes |
| Use case | Utility functions | Object behavior |

---

## HARD QUESTIONS

### 1. Design a method that safely processes a file with proper resource management.

**Answer:**

```csharp
// Using IDisposable and using statement
public class FileProcessor
{
    // Method 1: Using pattern
    public string ProcessFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path required", nameof(filePath));
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");
        
        try
        {
            // File handle automatically disposed
            using (var reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException(
                $"Access denied to file: {filePath}");
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException(
                $"Error reading file: {filePath}", ex);
        }
    }
    
    // Method 2: Async with cancellation support
    public async Task<string> ProcessFileAsync(
        string filePath, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path required", nameof(filePath));
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");
        
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                return await reader.ReadToEndAsync(cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            Logger.Warn($"File processing cancelled: {filePath}");
            throw;
        }
        catch (Exception ex)
        {
            Logger.Error($"Error processing file: {filePath}", ex);
            throw;
        }
    }
}
```

**Key Features:**
- Input validation
- Specific exception handling
- Resource cleanup (using statement)
- Async support
- Logging
- Cancellation support

### 2. Create a method that implements the builder pattern for complex object creation.

**Answer:**

```csharp
// Product class
public class Report
{
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IncludeTableOfContents { get; set; }
    public List<string> Sections { get; set; } = new();
    public ReportFormat Format { get; set; }
    public int FontSize { get; set; }
    public bool IncludeHeader { get; set; }
}

public enum ReportFormat { PDF, HTML, Word }

// Builder class
public class ReportBuilder
{
    private readonly Report _report = new();
    
    public ReportBuilder WithTitle(string title)
    {
        _report.Title = title;
        return this;
    }
    
    public ReportBuilder WithAuthor(string author)
    {
        _report.Author = author;
        return this;
    }
    
    public ReportBuilder WithFormat(ReportFormat format)
    {
        _report.Format = format;
        return this;
    }
    
    public ReportBuilder IncludeTableOfContents(bool include = true)
    {
        _report.IncludeTableOfContents = include;
        return this;
    }
    
    public ReportBuilder AddSection(string section)
    {
        _report.Sections.Add(section);
        return this;
    }
    
    public ReportBuilder WithFontSize(int size)
    {
        if (size < 8 || size > 72)
            throw new ArgumentException("Font size must be 8-72");
        _report.FontSize = size;
        return this;
    }
    
    public Report Build()
    {
        ValidateReport();
        _report.CreatedDate = DateTime.Now;
        return _report;
    }
    
    private void ValidateReport()
    {
        if (string.IsNullOrEmpty(_report.Title))
            throw new InvalidOperationException("Title required");
        if (_report.Sections.Count == 0)
            throw new InvalidOperationException("At least one section required");
    }
}

// Usage
var report = new ReportBuilder()
    .WithTitle("Sales Report Q1")
    .WithAuthor("John Smith")
    .WithFormat(ReportFormat.PDF)
    .IncludeTableOfContents()
    .AddSection("Executive Summary")
    .AddSection("Financial Data")
    .AddSection("Conclusions")
    .Build();
```

**Benefits:**
- Fluent API (method chaining)
- Clear intent
- Validation
- Flexibility
- Immutable result

### 3. How would you implement method caching to avoid recalculation?

**Answer:**

```csharp
public class CachedCalculator
{
    private readonly Dictionary<string, (DateTime, object)> _cache = new();
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);
    
    // Method 1: Manual caching
    public int GetFactorial(int n)
    {
        string key = $"Factorial_{n}";
        
        // Check cache
        if (_cache.TryGetValue(key, out var cached))
        {
            var (timestamp, value) = cached;
            if (DateTime.Now - timestamp < _cacheDuration)
            {
                Console.WriteLine($"Cache hit for {key}");
                return (int)value;
            }
        }
        
        // Calculate if not cached or expired
        Console.WriteLine($"Calculating {key}");
        int result = CalculateFactorial(n);
        
        // Store in cache
        _cache[key] = (DateTime.Now, result);
        
        return result;
    }
    
    private int CalculateFactorial(int n)
    {
        if (n <= 1) return 1;
        return n * CalculateFactorial(n - 1);
    }
    
    // Method 2: Using ConcurrentDictionary for thread safety
    private readonly ConcurrentDictionary<string, (DateTime, object)> 
        _threadSafeCache = new();
    
    public int GetFactorialThreadSafe(int n)
    {
        string key = $"Factorial_{n}";
        
        if (_threadSafeCache.TryGetValue(key, out var cached))
        {
            if (DateTime.Now - cached.Item1 < _cacheDuration)
                return (int)cached.Item2;
        }
        
        int result = CalculateFactorial(n);
        _threadSafeCache[key] = (DateTime.Now, result);
        return result;
    }
}

// Usage
var calc = new CachedCalculator();
calc.GetFactorial(5);   // Calculates
calc.GetFactorial(5);   // Cache hit
```

**Considerations:**
- Cache invalidation strategy
- Thread safety
- Memory usage
- Cache key design

### 4. Design a method for complex business logic with multiple validation levels.

**Answer:**

```csharp
public class PaymentProcessor
{
    public async Task<PaymentResult> ProcessPaymentAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Level 1: Input validation
            ValidatePaymentRequest(request);
            
            // Level 2: Business rule validation
            await ValidateBusinessRulesAsync(request, cancellationToken);
            
            // Level 3: Authorization
            var authResult = await AuthorizePaymentAsync(request, cancellationToken);
            if (!authResult.IsSuccess)
                return PaymentResult.Failed($"Authorization failed: {authResult.Message}");
            
            // Level 4: Process payment
            var processResult = await ProcessWithProviderAsync(request, cancellationToken);
            if (!processResult.IsSuccess)
                return PaymentResult.Failed($"Payment processing failed: {processResult.Message}");
            
            // Level 5: Record transaction
            await RecordTransactionAsync(request, processResult, cancellationToken);
            
            // Level 6: Notify
            await NotifySuccessAsync(request, cancellationToken);
            
            return PaymentResult.Success(processResult.TransactionId);
        }
        catch (ValidationException ex)
        {
            Logger.Warn($"Validation failed: {ex.Message}");
            return PaymentResult.Failed(ex.Message);
        }
        catch (OperationCanceledException)
        {
            Logger.Info("Payment processing cancelled");
            return PaymentResult.Cancelled();
        }
        catch (Exception ex)
        {
            Logger.Error("Unexpected error during payment processing", ex);
            return PaymentResult.Failed("An unexpected error occurred");
        }
    }
    
    private void ValidatePaymentRequest(PaymentRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        if (request.Amount <= 0)
            throw new ValidationException("Amount must be positive");
        
        if (string.IsNullOrWhiteSpace(request.CardNumber))
            throw new ValidationException("Card number required");
        
        if (request.ExpiryDate < DateTime.Now)
            throw new ValidationException("Card expired");
    }
    
    private async Task ValidateBusinessRulesAsync(
        PaymentRequest request, 
        CancellationToken cancellationToken)
    {
        // Check customer limits
        var customer = await GetCustomerAsync(request.CustomerId, cancellationToken);
        if (customer.DailyLimitRemaining < request.Amount)
            throw new ValidationException("Daily limit exceeded");
        
        // Check fraud
        if (await IsFraudulentAsync(request, cancellationToken))
            throw new ValidationException("Suspicious activity detected");
    }
    
    private async Task<AuthResult> AuthorizePaymentAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        return await _paymentGateway.AuthorizeAsync(request, cancellationToken);
    }
    
    private async Task<ProcessResult> ProcessWithProviderAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        return await _paymentGateway.ProcessAsync(request, cancellationToken);
    }
    
    private async Task RecordTransactionAsync(
        PaymentRequest request,
        ProcessResult result,
        CancellationToken cancellationToken)
    {
        var transaction = new Transaction
        {
            TransactionId = result.TransactionId,
            Amount = request.Amount,
            Timestamp = DateTime.UtcNow,
            Status = "Completed"
        };
        
        await _database.SaveTransactionAsync(transaction, cancellationToken);
    }
    
    private async Task NotifySuccessAsync(
        PaymentRequest request,
        CancellationToken cancellationToken)
    {
        await _notificationService.SendPaymentConfirmationAsync(
            request.Email, 
            cancellationToken);
    }
    
    // Helper methods...
    private async Task<Customer> GetCustomerAsync(string customerId, CancellationToken ct) { }
    private async Task<bool> IsFraudulentAsync(PaymentRequest request, CancellationToken ct) { }
}

public class PaymentResult
{
    public bool IsSuccess { get; private set; }
    public bool IsCancelled { get; private set; }
    public string? Message { get; private set; }
    public string? TransactionId { get; private set; }
    
    public static PaymentResult Success(string transactionId) =>
        new() { IsSuccess = true, TransactionId = transactionId };
    
    public static PaymentResult Failed(string message) =>
        new() { IsSuccess = false, Message = message };
    
    public static PaymentResult Cancelled() =>
        new() { IsCancelled = true };
}
```

**Key Features:**
- Multi-level validation
- Async operations
- Proper error handling
- Result objects
- Cancellation support
- Logging

### 5. How would you implement a decorator pattern for method enhancement?

**Answer:**

```csharp
// Interface for payment processor
public interface IPaymentProcessor
{
    Task<bool> ProcessAsync(Payment payment);
}

// Base implementation
public class BasePaymentProcessor : IPaymentProcessor
{
    public async Task<bool> ProcessAsync(Payment payment)
    {
        // Actual payment processing logic
        Console.WriteLine($"Processing ${payment.Amount}");
        await Task.Delay(100);  // Simulate work
        return true;
    }
}

// Decorator: Add logging
public class LoggingPaymentProcessor : IPaymentProcessor
{
    private readonly IPaymentProcessor _inner;
    private readonly ILogger _logger;
    
    public LoggingPaymentProcessor(IPaymentProcessor inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }
    
    public async Task<bool> ProcessAsync(Payment payment)
    {
        _logger.Info($"Starting payment processing: ${payment.Amount}");
        try
        {
            var result = await _inner.ProcessAsync(payment);
            _logger.Info($"Payment processed successfully: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Payment processing failed: {ex.Message}");
            throw;
        }
    }
}

// Decorator: Add caching
public class CachingPaymentProcessor : IPaymentProcessor
{
    private readonly IPaymentProcessor _inner;
    private readonly Dictionary<string, bool> _cache = new();
    
    public CachingPaymentProcessor(IPaymentProcessor inner)
    {
        _inner = inner;
    }
    
    public async Task<bool> ProcessAsync(Payment payment)
    {
        string key = payment.Id;
        if (_cache.TryGetValue(key, out var cached))
            return cached;
        
        var result = await _inner.ProcessAsync(payment);
        _cache[key] = result;
        return result;
    }
}

// Decorator: Add retry logic
public class RetryPaymentProcessor : IPaymentProcessor
{
    private readonly IPaymentProcessor _inner;
    private readonly int _maxRetries = 3;
    
    public RetryPaymentProcessor(IPaymentProcessor inner)
    {
        _inner = inner;
    }
    
    public async Task<bool> ProcessAsync(Payment payment)
    {
        for (int attempt = 0; attempt < _maxRetries; attempt++)
        {
            try
            {
                return await _inner.ProcessAsync(payment);
            }
            catch (Exception ex) when (attempt < _maxRetries - 1)
            {
                Console.WriteLine($"Attempt {attempt + 1} failed, retrying...");
                await Task.Delay(1000 * (attempt + 1));  // Exponential backoff
            }
        }
        
        throw new PaymentException("Payment processing failed after retries");
    }
}

// Usage
var processor = new BasePaymentProcessor();
var withLogging = new LoggingPaymentProcessor(processor, logger);
var withCaching = new CachingPaymentProcessor(withLogging);
var withRetry = new RetryPaymentProcessor(withCaching);

// Pipeline: Retry -> Cache -> Logging -> Base
await withRetry.ProcessAsync(payment);
```

**Benefits:**
- Separates concerns
- Flexible composition
- Easy to add/remove features
- Open/Closed Principle

---

## Interview Tips

### Preparation
1. Practice writing code by hand
2. Explain your reasoning out loud
3. Ask clarifying questions
4. Test edge cases

### During Interview
1. Clarify requirements
2. Think before coding
3. Explain your approach
4. Consider performance
5. Handle errors gracefully
6. Ask about assumptions

### Common Follow-ups
- "How would you optimize this?"
- "What about thread safety?"
- "How would you test this?"
- "What about edge cases?"
- "Can you refactor this?"

## Summary

**Question Difficulty Levels:**
- **Easy (5)**: Definitions, basic concepts
- **Medium (5)**: Problem-solving, refactoring
- **Hard (5)**: Design patterns, complex scenarios

**Interview Strategy:**
- Know fundamentals deeply
- Practice coding under pressure
- Explain your thinking
- Consider edge cases
- Discuss trade-offs

## Next Steps

- Review [Best-Practices](../01-Best-Practices/00-Best-Practices.md) for solid foundations
- Study [Common-Mistakes](../02-Common-Mistakes/00-Common-Mistakes.md) to avoid pitfalls
