# Method Best Practices

## Overview

Best practices for writing effective, maintainable, and efficient methods in C#.

## 1. Single Responsibility Principle

Each method should do one thing well:

```csharp
// BAD - Method does too many things
public void ProcessUserData(string name, int age, string email)
{
    // Validate
    if (string.IsNullOrEmpty(name))
        throw new ArgumentException("Name required");
    if (age < 0)
        throw new ArgumentException("Age invalid");
    
    // Transform
    name = name.ToUpper();
    email = email.ToLower();
    
    // Save to database
    using (var db = new Database())
    {
        db.Users.Add(new User { Name = name, Age = age, Email = email });
        db.SaveChanges();
    }
    
    // Send email
    SendWelcomeEmail(email);
    
    // Log
    Logger.Log($"User {name} processed");
}

// GOOD - Each method has single responsibility
public void RegisterUser(string name, int age, string email)
{
    ValidateUserData(name, age, email);
    User user = CreateUser(name, age, email);
    SaveUser(user);
    NotifyUser(email);
}

private void ValidateUserData(string name, int age, string email)
{
    if (string.IsNullOrEmpty(name))
        throw new ArgumentException("Name required");
    if (age < 0)
        throw new ArgumentException("Age invalid");
    if (string.IsNullOrEmpty(email))
        throw new ArgumentException("Email required");
}

private User CreateUser(string name, int age, string email)
{
    return new User 
    { 
        Name = name.ToUpper(), 
        Age = age, 
        Email = email.ToLower() 
    };
}

private void SaveUser(User user)
{
    using (var db = new Database())
    {
        db.Users.Add(user);
        db.SaveChanges();
    }
}

private void NotifyUser(string email)
{
    SendWelcomeEmail(email);
}
```

## 2. Keep Methods Short

Shorter methods are easier to understand and test:

```csharp
// BAD - 50 line method
public decimal CalculateTotal(Order order)
{
    decimal subtotal = 0;
    foreach (var item in order.Items)
    {
        subtotal += item.Price * item.Quantity;
    }
    
    decimal tax = subtotal * 0.1m;
    decimal discount = 0;
    
    if (subtotal > 100)
        discount = subtotal * 0.1m;
    else if (subtotal > 50)
        discount = subtotal * 0.05m;
    
    // ... more code ...
}

// GOOD - Each method is focused
public decimal CalculateTotal(Order order)
{
    decimal subtotal = CalculateSubtotal(order);
    decimal tax = CalculateTax(subtotal);
    decimal discount = CalculateDiscount(subtotal);
    return subtotal + tax - discount;
}

private decimal CalculateSubtotal(Order order)
{
    return order.Items.Sum(i => i.Price * i.Quantity);
}

private decimal CalculateTax(decimal amount)
{
    return amount * 0.1m;
}

private decimal CalculateDiscount(decimal subtotal)
{
    if (subtotal > 100) return subtotal * 0.1m;
    if (subtotal > 50) return subtotal * 0.05m;
    return 0;
}
```

## 3. Meaningful Names

Method names should clearly indicate what they do:

```csharp
// BAD - Unclear names
public void Process(User u) { }
public int Do(int x, int y) { }
public bool Check(string s) { }

// GOOD - Clear names
public void ValidateAndRegisterUser(User user) { }
public int CalculateSum(int a, int b) { }
public bool IsValidEmail(string email) { }
```

## 4. Consistent Parameter Order

Keep parameter order consistent:

```csharp
// BAD - Inconsistent order
public void CreateRecord(string id, string name) { }
public void UpdateRecord(string name, string id) { }
public void DeleteRecord(string name, string id) { }

// GOOD - Consistent order
public void CreateRecord(string id, string name) { }
public void UpdateRecord(string id, string name) { }
public void DeleteRecord(string id, string name) { }
```

## 5. Fail Fast

Validate inputs at the beginning:

```csharp
// BAD - Validation scattered throughout
public void ProcessPayment(decimal amount, string accountId)
{
    // ... some code ...
    
    if (amount <= 0)
        throw new ArgumentException("Amount invalid");
    
    // ... more code ...
    
    if (string.IsNullOrEmpty(accountId))
        throw new ArgumentException("Account required");
    
    // ... even more code ...
}

// GOOD - All validation at start
public void ProcessPayment(decimal amount, string accountId)
{
    ValidateAmount(amount);
    ValidateAccountId(accountId);
    
    // Process payment with confidence inputs are valid
}

private void ValidateAmount(decimal amount)
{
    if (amount <= 0)
        throw new ArgumentException("Amount must be positive", nameof(amount));
}

private void ValidateAccountId(string accountId)
{
    if (string.IsNullOrEmpty(accountId))
        throw new ArgumentException("Account ID required", nameof(accountId));
}
```

## 6. Use Guard Clauses

Return early with guard clauses:

```csharp
// BAD - Deep nesting
public string GetUserStatus(User user)
{
    if (user != null)
    {
        if (user.IsActive)
        {
            if (user.HasPermission("view"))
            {
                if (user.IsVerified)
                {
                    return "Verified Active User";
                }
                else
                {
                    return "Unverified Active User";
                }
            }
            else
            {
                return "Active User - No Permission";
            }
        }
        else
        {
            return "Inactive User";
        }
    }
    else
    {
        return "Unknown User";
    }
}

// GOOD - Guard clauses
public string GetUserStatus(User? user)
{
    if (user == null)
        return "Unknown User";
    
    if (!user.IsActive)
        return "Inactive User";
    
    if (!user.HasPermission("view"))
        return "Active User - No Permission";
    
    if (!user.IsVerified)
        return "Unverified Active User";
    
    return "Verified Active User";
}
```

## 7. Avoid Output Parameters When Possible

Prefer return values over out parameters:

```csharp
// BAD - Using out parameter
public bool GetUserAge(string userId, out int age)
{
    age = 0;
    var user = _database.FindUser(userId);
    if (user != null)
    {
        age = user.Age;
        return true;
    }
    return false;
}

// GOOD - Return object
public User? GetUser(string userId)
{
    return _database.FindUser(userId);
}

// Usage
var user = GetUser("123");
if (user != null)
{
    int age = user.Age;
}

// Or with TryParse pattern
public bool TryGetUserAge(string userId, out int age)
{
    var user = GetUser(userId);
    if (user != null)
    {
        age = user.Age;
        return true;
    }
    age = 0;
    return false;
}
```

## 8. Don't Repeat Yourself

Extract repeated code to helper methods:

```csharp
// BAD - Code repetition
public void SaveToFile(string data, string filename)
{
    try
    {
        File.WriteAllText(filename, data);
        Logger.Log($"Saved {filename}");
    }
    catch (Exception ex)
    {
        Logger.Error($"Error saving {filename}: {ex.Message}");
    }
}

public void SaveToBackup(string data, string filename)
{
    try
    {
        File.WriteAllText(filename, data);
        Logger.Log($"Backed up {filename}");
    }
    catch (Exception ex)
    {
        Logger.Error($"Error backing up {filename}: {ex.Message}");
    }
}

// GOOD - Extract common logic
private void SaveFile(string data, string filename, string operation)
{
    try
    {
        File.WriteAllText(filename, data);
        Logger.Log($"{operation}: {filename}");
    }
    catch (Exception ex)
    {
        Logger.Error($"Error {operation} {filename}: {ex.Message}");
    }
}

public void SaveToFile(string data, string filename)
    => SaveFile(data, filename, "Saved");

public void SaveToBackup(string data, string filename)
    => SaveFile(data, filename, "Backed up");
```

## 9. Document Complex Logic

Use comments and documentation for non-obvious code:

```csharp
// BAD - No documentation
public int CalculateHash(string input)
{
    int hash = 5381;
    foreach (char c in input)
    {
        hash = ((hash << 5) + hash) + c;
    }
    return hash;
}

// GOOD - Documented
/// <summary>
/// Calculates a hash using the DJB2 algorithm.
/// This algorithm is fast and provides good distribution for strings.
/// </summary>
/// <param name="input">The string to hash</param>
/// <returns>A 32-bit hash value</returns>
public int CalculateHash(string input)
{
    const int initialHash = 5381;
    int hash = initialHash;
    
    foreach (char c in input)
    {
        // DJB2 algorithm: hash = hash * 33 + c
        hash = ((hash << 5) + hash) + c;
    }
    
    return hash;
}
```

## 10. Use Appropriate Access Modifiers

Make methods private unless they need to be public:

```csharp
// BAD - Everything public
public class PaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        ConnectToBank();
        AuthorizeTransaction(amount);
        RecordTransaction(amount);
        CloseConnection();
    }
    
    public void ConnectToBank() { }
    public void AuthorizeTransaction(decimal amount) { }
    public void RecordTransaction(decimal amount) { }
    public void CloseConnection() { }
}

// GOOD - Only public API exposed
public class PaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        ConnectToBank();
        AuthorizeTransaction(amount);
        RecordTransaction(amount);
        CloseConnection();
    }
    
    private void ConnectToBank() { }
    private void AuthorizeTransaction(decimal amount) { }
    private void RecordTransaction(decimal amount) { }
    private void CloseConnection() { }
}
```

## 11. Handle Exceptions Appropriately

Catch specific exceptions and handle meaningfully:

```csharp
// BAD - Catching everything
public void LoadFile(string filename)
{
    try
    {
        File.ReadAllText(filename);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error");
    }
}

// GOOD - Specific handling
public void LoadFile(string filename)
{
    try
    {
        File.ReadAllText(filename);
    }
    catch (FileNotFoundException)
    {
        Logger.Warn($"File not found: {filename}");
    }
    catch (UnauthorizedAccessException)
    {
        Logger.Error($"Access denied: {filename}");
    }
    catch (IOException ex)
    {
        Logger.Error($"IO error: {filename} - {ex.Message}");
        throw;
    }
}
```

## 12. Prefer Composition Over Inheritance

Use helper methods rather than inheritance:

```csharp
// BAD - Unnecessary inheritance
public class Logger : FileWriter
{
    public void Log(string message)
    {
        Write($"[{DateTime.Now}] {message}");
    }
}

// GOOD - Composition
public class Logger
{
    private readonly FileWriter _writer;
    
    public Logger(FileWriter writer)
    {
        _writer = writer;
    }
    
    public void Log(string message)
    {
        _writer.Write($"[{DateTime.Now}] {message}");
    }
}
```

## Summary

**12 Best Practices:**
1. Single Responsibility - One thing per method
2. Keep Short - Easier to understand
3. Meaningful Names - Clear purpose
4. Consistent Parameters - Order matters
5. Fail Fast - Validate inputs early
6. Guard Clauses - Return early
7. Avoid Output Parameters - Use returns
8. Don't Repeat - Extract helpers
9. Document Complex Logic - Comment why
10. Use Access Modifiers - Encapsulation
11. Handle Exceptions - Catch specific types
12. Composition Over Inheritance - Flexibility

## Next Steps

- Review [Common-Mistakes](../02-Common-Mistakes/00-Common-Mistakes.md) to learn what to avoid
- Study [Interview-Questions](../03-Interview-Questions/00-Interview-Overview.md) for interview preparation
