# Encapsulation - Data Protection

## Overview

Encapsulation hides internal implementation details and controls access to object data through public interfaces.

## Principle of Encapsulation

```csharp
// Bad - Direct data access
public class BankAccountBad
{
    public decimal Balance;  // Public field - no protection
}

var account = new BankAccountBad();
account.Balance = -1000;  // Invalid!

// Good - Controlled access
public class BankAccount
{
    private decimal _balance;
    
    public decimal Balance
    {
        get { return _balance; }
        set { _balance = value >= 0 ? value : 0; }  // Validation
    }
}

var account = new BankAccount();
account.Balance = -1000;  // Sets to 0 instead
```

## Access Levels

```csharp
public class EncapsulationExample
{
    // Public - accessible everywhere
    public string PublicData { get; set; }
    
    // Private - accessible only in this class
    private string _privateData;
    
    // Protected - accessible in derived classes
    protected string ProtectedData;
    
    // Internal - accessible in same assembly
    internal string InternalData;
    
    // Protected internal - protected or internal
    protected internal string ProtectedInternalData;
}
```

## Hiding Implementation

```csharp
public class EmailService
{
    // Public interface
    public void SendEmail(string to, string message)
    {
        ValidateEmail(to);
        BuildMessage(message);
        SendViaSmtp();
    }
    
    // Private implementation - hidden
    private void ValidateEmail(string email)
    {
        // Validation logic
    }
    
    private void BuildMessage(string message)
    {
        // Message building
    }
    
    private void SendViaSmtp()
    {
        // SMTP logic
    }
}

// Usage - only see public interface
var service = new EmailService();
service.SendEmail("user@example.com", "Hello");
// Implementation details hidden
```

## Property Encapsulation

```csharp
public class Employee
{
    private int _age;
    
    // Encapsulated property with validation
    public int Age
    {
        get { return _age; }
        set
        {
            if (value < 0 || value > 150)
                throw new ArgumentException("Invalid age");
            _age = value;
        }
    }
    
    // Read-only property
    public int YearsUntilRetirement
    {
        get { return Math.Max(0, 65 - _age); }
    }
    
    // Write-only property (rare)
    private string _password;
    public string Password
    {
        set { _password = value; }  // Only set, cannot read
    }
}

// Usage
var emp = new Employee();
emp.Age = 30;
Console.WriteLine(emp.YearsUntilRetirement);  // 35
emp.Password = "secret";  // Can set
// Console.WriteLine(emp.Password);  // ERROR - cannot read
```

## Method Visibility

```csharp
public class DataProcessor
{
    // Public - exposed interface
    public void ProcessData(string input)
    {
        string validated = ValidateInput(input);
        string transformed = TransformData(validated);
        SaveData(transformed);
    }
    
    // Private - internal implementation
    private string ValidateInput(string input)
    {
        return string.IsNullOrEmpty(input) ? "" : input.Trim();
    }
    
    private string TransformData(string data)
    {
        return data.ToUpper();
    }
    
    private void SaveData(string data)
    {
        // Save implementation
    }
}
```

## Lazy Loading Encapsulation

```csharp
public class User
{
    private List<Order> _orders;
    private bool _ordersLoaded = false;
    
    // Encapsulate expensive operation
    public IEnumerable<Order> Orders
    {
        get
        {
            if (!_ordersLoaded)
            {
                _orders = LoadOrdersFromDatabase();
                _ordersLoaded = true;
            }
            return _orders;
        }
    }
    
    private List<Order> LoadOrdersFromDatabase()
    {
        // Expensive database query
        return new List<Order>();
    }
}

// Usage - details hidden
var user = new User();
var orders = user.Orders;  // Loads if needed
```

## Benefits of Encapsulation

### 1. Protection

```csharp
public class Account
{
    private decimal _balance;
    
    // Prevent invalid states
    public decimal Balance
    {
        get { return _balance; }
        set { _balance = value >= 0 ? value : _balance; }
    }
}
```

### 2. Flexibility

```csharp
public class Logger
{
    // Can change implementation later
    private ILogWriter _writer;
    
    public void Log(string message)
    {
        _writer.Write(message);
    }
}
```

### 3. Maintainability

```csharp
// Private methods can be refactored without affecting users
public class DataService
{
    public void LoadData()
    {
        var data = FetchFromDatabase();
        ProcessData(data);
    }
    
    private void ProcessData(IEnumerable<T> data) { }
}
```

## Summary

- **Encapsulation** - Hide implementation, expose interface
- **Access modifiers** - Control visibility
- **Properties** - Controlled data access
- **Validation** - Protect data integrity
- **Private** - Internal implementation
- **Public** - External interface
- **Benefits** - Protection, flexibility, maintainability

## Next Steps

- Learn [Static-Members](../04-Static-Members/00-Static-Members.md)
- Study [Access-Modifiers](../05-Access-Modifiers/00-Access-Modifiers.md)
