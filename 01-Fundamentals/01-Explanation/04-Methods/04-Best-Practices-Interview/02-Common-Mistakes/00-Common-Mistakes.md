# Common Method Mistakes

## Overview

Common mistakes when writing methods and how to fix them.

## 1. Forgetting to Handle null

### Mistake

```csharp
// BAD - NullReferenceException risk
public string GetUserName(User user)
{
    return user.Name.ToUpper();  // Crashes if user is null
}

// Usage
GetUserName(null);  // NullReferenceException
```

### Fix

```csharp
// GOOD - Handle null
public string GetUserName(User? user)
{
    if (user == null)
        return "Unknown";
    
    return user.Name.ToUpper();
}

// Or use null coalescing
public string GetUserName(User? user)
{
    return user?.Name?.ToUpper() ?? "Unknown";
}
```

## 2. Modifying Parameters Unexpectedly

### Mistake

```csharp
// BAD - Caller doesn't expect parameter to change
public void ProcessData(List<string> items)
{
    items.Clear();  // Modifies caller's list!
    // ... process items
}

// Usage
var data = new List<string> { "a", "b", "c" };
ProcessData(data);
Console.WriteLine(data.Count);  // 0 - caller's list is empty!
```

### Fix

```csharp
// GOOD - Work with copy or document behavior
public void ProcessData(List<string> items)
{
    var localItems = new List<string>(items);  // Make copy
    localItems.Clear();
    // ... process local copy
}

// Or explicitly use ref if modification intended
public void ClearData(ref List<string> items)
{
    items = new List<string>();
}
```

## 3. Not Validating Method Arguments

### Mistake

```csharp
// BAD - No validation
public int Divide(int a, int b)
{
    return a / b;  // Crashes on b=0
}

// Usage
Divide(10, 0);  // DivideByZeroException
```

### Fix

```csharp
// GOOD - Validate arguments
public int Divide(int a, int b)
{
    if (b == 0)
        throw new ArgumentException("Divisor cannot be zero", nameof(b));
    
    return a / b;
}

// Or use method contract
public int Divide(int a, int b)
{
    ArgumentOutOfRangeException.ThrowIfZero(b);
    return a / b;
}
```

## 4. Inconsistent Return Types

### Mistake

```csharp
// BAD - Sometimes returns null, sometimes empty
public List<User> GetActiveUsers()
{
    var users = FindUsers();
    
    if (users.Count == 0)
        return null;  // Returns null sometimes
    
    return users;
}

// Usage - Caller must check for null or empty
var users = GetActiveUsers();
if (users != null && users.Count > 0)  // Defensive check
{
    // Process
}
```

### Fix

```csharp
// GOOD - Consistent return
public List<User> GetActiveUsers()
{
    return FindUsers().Where(u => u.IsActive).ToList();
}

// Always returns non-null list (possibly empty)
var users = GetActiveUsers();
foreach (var user in users)  // Safe - never null
{
    // Process
}

// Or use IEnumerable for efficiency
public IEnumerable<User> GetActiveUsers()
{
    return FindUsers().Where(u => u.IsActive);
}
```

## 5. Too Many Parameters

### Mistake

```csharp
// BAD - Too many parameters
public void CreateReport(string title, string author, string department, 
    DateTime startDate, DateTime endDate, bool includeDetails, 
    bool sortByDate, string format, bool emailResult, string emailTo)
{
    // Hard to remember order
    // Hard to test
    // Easy to mix up boolean parameters
}

// Usage - Easy to make mistakes
CreateReport("Sales", "John", "Finance", 
    DateTime.Now.AddMonths(-1), DateTime.Now, true, true, 
    "PDF", false, "reports@company.com");  // Which boolean is which?
```

### Fix

```csharp
// GOOD - Use parameter object
public class ReportOptions
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Department { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IncludeDetails { get; set; }
    public bool SortByDate { get; set; }
    public string Format { get; set; }
    public bool EmailResult { get; set; }
    public string EmailTo { get; set; }
}

public void CreateReport(ReportOptions options)
{
    // Clear and maintainable
}

// Usage - Clear intent
var options = new ReportOptions
{
    Title = "Sales",
    Author = "John",
    Department = "Finance",
    StartDate = DateTime.Now.AddMonths(-1),
    EndDate = DateTime.Now,
    IncludeDetails = true,
    Format = "PDF"
};
CreateReport(options);
```

## 6. Not Using Out Parameters Correctly

### Mistake

```csharp
// BAD - out parameter not always assigned
public bool TryGetUser(string id, out User user)
{
    user = null;
    
    if (id == "123")
    {
        user = new User { Id = id };
        return true;
    }
    
    // Forgot to ensure user is assigned when returning false
    // Compiler allows this - potential bugs
    return false;
}
```

### Fix

```csharp
// GOOD - Always assign out parameter
public bool TryGetUser(string id, out User? user)
{
    user = null;  // Always initialize
    
    if (string.IsNullOrEmpty(id))
        return false;
    
    var found = _database.Find(id);
    if (found != null)
    {
        user = found;
        return true;
    }
    
    return false;
}

// Better - Return object instead
public User? GetUserOrDefault(string id)
{
    if (string.IsNullOrEmpty(id))
        return null;
    
    return _database.Find(id);
}
```

## 7. Swallowing Exceptions

### Mistake

```csharp
// BAD - Exception hidden
public void LoadConfiguration()
{
    try
    {
        var config = File.ReadAllText("config.json");
        ParseConfig(config);
    }
    catch (Exception ex)
    {
        // Silent failure - problem hidden from caller
        Console.WriteLine("Error occurred");
    }
}

// Caller has no idea what went wrong
LoadConfiguration();  // May silently fail
```

### Fix

```csharp
// GOOD - Rethrow or handle meaningfully
public void LoadConfiguration()
{
    try
    {
        var config = File.ReadAllText("config.json");
        ParseConfig(config);
    }
    catch (FileNotFoundException ex)
    {
        Logger.Error($"Config file not found: {ex.Message}");
        throw;  // Let caller know
    }
    catch (JsonException ex)
    {
        Logger.Error($"Invalid config format: {ex.Message}");
        throw;
    }
}

// Or return result
public bool TryLoadConfiguration(out string? error)
{
    error = null;
    try
    {
        var config = File.ReadAllText("config.json");
        ParseConfig(config);
        return true;
    }
    catch (Exception ex)
    {
        error = ex.Message;
        return false;
    }
}
```

## 8. Method Names Don't Match Behavior

### Mistake

```csharp
// BAD - Name misleading
public void GetUserData(User user)
{
    // Method is called "Get" but actually modifies the user
    user.LastAccessTime = DateTime.Now;
    user.AccessCount++;
    // Saves to database
    _database.Save(user);
}

// Caller expects just reading
var user = new User { Id = "123" };
// Caller doesn't expect this to modify user
GetUserData(user);
```

### Fix

```csharp
// GOOD - Name matches behavior
public void LoadUserData(User user)
{
    user.LastAccessTime = DateTime.Now;
    user.AccessCount++;
    _database.Save(user);
}

// Or separate concerns
public User? FetchUserData(string userId)
{
    return _database.Find(userId);
}

public void UpdateUserActivity(User user)
{
    user.LastAccessTime = DateTime.Now;
    user.AccessCount++;
    _database.Save(user);
}
```

## 9. Hardcoding Values

### Mistake

```csharp
// BAD - Magic numbers and strings
public decimal CalculateDiscount(decimal amount)
{
    if (amount > 1000)
        return amount * 0.15m;  // What is 15%? Why 1000?
    
    if (amount > 500)
        return amount * 0.10m;
    
    return amount * 0.05m;
}
```

### Fix

```csharp
// GOOD - Use named constants
public class DiscountTiers
{
    private const decimal PremiumThreshold = 1000m;
    private const decimal PremiumRate = 0.15m;
    
    private const decimal GoldThreshold = 500m;
    private const decimal GoldRate = 0.10m;
    
    private const decimal StandardRate = 0.05m;
}

public decimal CalculateDiscount(decimal amount)
{
    if (amount > DiscountTiers.PremiumThreshold)
        return amount * DiscountTiers.PremiumRate;
    
    if (amount > DiscountTiers.GoldThreshold)
        return amount * DiscountTiers.GoldRate;
    
    return amount * DiscountTiers.StandardRate;
}
```

## 10. Not Testing Edge Cases

### Mistake

```csharp
// BAD - Only tested with normal data
public string FormatName(string firstName, string lastName)
{
    return $"{firstName} {lastName}";
}

// Works for: "John Smith"
// Breaks for: "", null, "    ", single names
```

### Fix

```csharp
// GOOD - Handle edge cases
public string FormatName(string? firstName, string? lastName)
{
    firstName = firstName?.Trim();
    lastName = lastName?.Trim();
    
    if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
        return "Unknown";
    
    if (string.IsNullOrEmpty(firstName))
        return lastName!;
    
    if (string.IsNullOrEmpty(lastName))
        return firstName;
    
    return $"{firstName} {lastName}";
}

// Test cases:
// "" -> "Unknown"
// null -> "Unknown"
// "John" -> "John"
// "John Smith" -> "John Smith"
```

## 11. Unclear Method Purpose

### Mistake

```csharp
// BAD - Unclear what method does
public void Execute(User u)
{
    // Is it creating, updating, deleting, or processing?
    // Code doesn't clarify intent
}

// Usage - Confusing
Execute(user);  // What happens?
```

### Fix

```csharp
// GOOD - Clear intent
public void RegisterNewUser(User user) { }
public void UpdateUserProfile(User user) { }
public void RemoveUser(User user) { }
public void ValidateUser(User user) { }

// Usage - Clear what happens
RegisterNewUser(user);
UpdateUserProfile(user);
```

## 12. Returning Objects for Side Effects Only

### Mistake

```csharp
// BAD - Return value ignored, method does side effects
public User SendNotification(string email, string message)
{
    // Primary purpose is side effect (sending email)
    // Why return User?
    EmailService.Send(email, message);
    return new User { Email = email };
}

// Usage - Caller ignores return value
SendNotification("user@example.com", "Welcome!");
```

### Fix

```csharp
// GOOD - Clear purpose
public void SendNotification(string email, string message)
{
    // Clear: method sends notification
    EmailService.Send(email, message);
}

// Or if returning data:
public async Task<NotificationResult> SendNotificationAsync(string email, string message)
{
    var result = await EmailService.SendAsync(email, message);
    return result;
}

// Usage - Intent is clear
await SendNotificationAsync("user@example.com", "Welcome!");
```

## Summary

**12 Common Mistakes:**
1. Forgetting null handling
2. Unexpectedly modifying parameters
3. Not validating arguments
4. Inconsistent return types
5. Too many parameters
6. Incorrect out parameter usage
7. Swallowing exceptions
8. Misleading method names
9. Hardcoding values
10. Not testing edge cases
11. Unclear purpose
12. Returning for side effects

## Next Steps

- Study [Interview-Questions](../03-Interview-Questions/00-Interview-Overview.md) for interview preparation
- Review [Best-Practices](../01-Best-Practices/00-Best-Practices.md) for positive patterns
