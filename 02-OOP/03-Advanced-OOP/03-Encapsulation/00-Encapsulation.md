# Encapsulation

## Overview

Encapsulation hides internal implementation details and exposes only a controlled public interface. It protects data integrity, allows implementation changes without breaking code, and is a core OOP principle.

## What is Encapsulation?

Encapsulation:
- Hides internal state
- Exposes controlled access through public methods/properties
- Prevents direct data modification
- Allows validation logic

```csharp
// Bad - Direct access, no protection
public class AccountBad
{
    public decimal Balance;  // Anyone can modify
}

// Good - Encapsulated
public class Account
{
    private decimal _balance;  // Hidden
    
    public decimal Balance
    {
        get { return _balance; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Balance cannot be negative");
            _balance = value;
        }
    }
}

// Usage
var account = new Account();
account.Balance = 1000;  // Validated
// account.Balance = -100;  // Throws exception
```

## Private vs Public

Control what is visible:

```csharp
public class BankAccount
{
    // Private - hidden from outside
    private string _pin;
    private List<Transaction> _transactions;
    
    // Public - accessible from outside
    public string AccountNumber { get; private set; }
    public decimal Balance { get; private set; }
    
    // Public method for controlled access
    public void Withdraw(decimal amount)
    {
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds");
        
        Balance -= amount;
        _transactions.Add(new Transaction(amount, "Withdraw"));
    }
}

// Usage
var account = new BankAccount();
account.Withdraw(100);      // OK - public method
// account._transactions;   // ERROR - private
```

## Protected Members (Inheritance)

Accessible in derived classes but not outside:

```csharp
public class Employee
{
    // Private - only this class
    private decimal _salary;
    
    // Protected - this class + derived classes
    protected string _department;
    
    // Public - everyone
    public string Name { get; set; }
}

public class Manager : Employee
{
    public void SetDepartment(string dept)
    {
        _department = dept;  // OK - protected accessible
        // _salary = 50000;  // ERROR - private not accessible
    }
}
```

## Data Validation

Encapsulation enables validation:

```csharp
public class Person
{
    private int _age;
    
    public int Age
    {
        get { return _age; }
        set
        {
            if (value < 0 || value > 150)
                throw new ArgumentException("Age must be 0-150");
            _age = value;
        }
    }
    
    private string _email;
    
    public string Email
    {
        get { return _email; }
        set
        {
            if (!value.Contains("@"))
                throw new ArgumentException("Invalid email");
            _email = value;
        }
    }
}

// Usage
var person = new Person();
person.Age = 30;          // OK
person.Email = "test@example.com";  // OK
// person.Age = 200;      // Throws
// person.Email = "bad";  // Throws
```

## Computed Properties

Calculate values on access without exposing calculation logic:

```csharp
public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }
    
    // Area computed from width and height
    public double Area
    {
        get { return Width * Height; }
    }
    
    // Perimeter computed
    public double Perimeter
    {
        get { return 2 * (Width + Height); }
    }
}

// Usage - Area is calculated, not stored
var rect = new Rectangle { Width = 10, Height = 5 };
Console.WriteLine(rect.Area);  // 50 (calculated)
```

## Read-Only Properties

Expose data that cannot be changed:

```csharp
public class Product
{
    public int Id { get; }  // Read-only, set in constructor
    public string Name { get; set; }
    
    public Product(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

// Usage
var product = new Product(1, "Laptop");
// product.Id = 2;  // ERROR - read-only
product.Name = "Desktop";  // OK
```

## Access Through Methods

Old-style encapsulation (before properties):

```csharp
public class Account
{
    private decimal _balance;
    
    // Getter method
    public decimal GetBalance()
    {
        return _balance;
    }
    
    // Setter method with validation
    public void SetBalance(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Balance cannot be negative");
        _balance = amount;
    }
}

// Usage - more verbose than properties
var account = new Account();
account.SetBalance(1000);
decimal bal = account.GetBalance();
```

## Protecting Collections

Expose collections safely:

```csharp
// Bad - external code can modify collection
public class TeamBad
{
    public List<string> Members { get; set; }  // Direct access
}

var team = new TeamBad();
team.Members = null;  // Breaks class!

// Good - expose as read-only
public class TeamGood
{
    private List<string> _members = new();
    
    public IReadOnlyList<string> Members
    {
        get { return _members.AsReadOnly(); }
    }
    
    public void AddMember(string name)
    {
        _members.Add(name);
    }
}

var team = new TeamGood();
team.AddMember("Alice");
// team.Members = null;  // ERROR - property is read-only
```

## Benefits of Encapsulation

1. **Data Integrity** - Prevent invalid states
2. **Implementation Hiding** - Change internals safely
3. **Validation** - Enforce rules automatically
4. **Flexibility** - Add logic without changing interface
5. **Maintainability** - Easier to understand and modify

```csharp
// Can change internal implementation without breaking code
public class Temperature
{
    private double _celsius;
    
    public double Celsius
    {
        get { return _celsius; }
        set { _celsius = value; }
    }
    
    public double Fahrenheit
    {
        get { return (_celsius * 9 / 5) + 32; }
    }
}

// Later: change how Celsius is stored
// External code doesn't care, interface unchanged
```

## Best Practices

### Make Fields Private

```csharp
// Bad - expose internals
public class ConfigBad
{
    public List<string> _settings;
}

// Good - hide internals
public class ConfigGood
{
    private List<string> _settings;
    
    public IReadOnlyList<string> Settings
    {
        get { return _settings.AsReadOnly(); }
    }
}
```

### Use Properties for Access

```csharp
// Good - use properties
public class Data
{
    private int _value;
    public int Value { get; set; }
}

// Bad - direct field access
public class DataBad
{
    public int Value;
}
```

## Summary

- **Encapsulation** - Hide internal details, expose interface
- **Private** - Hidden from outside
- **Protected** - Hidden except in derived classes
- **Public** - Accessible everywhere
- **Validation** - Protect data integrity
- **Computed** - Calculate on access
- **Read-only** - Prevent modification
- **Collections** - Expose as IReadOnly

## Next Steps

- Learn [Access-Modifiers](../04-Access-Modifiers/00-Access-Modifiers.md) for visibility control
- Study [Interfaces-Basics](../01-Interfaces-Basics/00-Interfaces-Basics.md) for contracts
- Review [Abstract-Classes](../02-Abstract-Classes/00-Abstract-Classes.md) for inheritance
