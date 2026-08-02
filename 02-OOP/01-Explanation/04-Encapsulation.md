# Encapsulation

## Overview
Encapsulation is bundling data (fields) and methods together while hiding internal implementation details.

---

## What is Encapsulation?

Bundling data and behavior into a single unit (class) and controlling access through visibility.

```csharp
// Without encapsulation - exposed data
public class BadAccount {
    public decimal balance;  // Direct access
}

// With encapsulation - controlled access
public class GoodAccount {
    private decimal balance;  // Hidden
    
    public decimal GetBalance() {
        return balance;
    }
    
    public void Deposit(decimal amount) {
        if (amount > 0) {
            balance += amount;
        }
    }
}
```

---

## Access Modifiers

Control who can access members.

### Public
Accessible everywhere.

```csharp
public class Car {
    public string Model { get; set; }  // Anyone can access
    
    public void Start() {
        Console.WriteLine("Starting");
    }
}

Car car = new Car();
car.Model = "Tesla";  // OK
car.Start();  // OK
```

### Private
Only accessible within the class.

```csharp
public class BankAccount {
    private decimal balance;  // Only this class can access
    
    public void Deposit(decimal amount) {
        balance += amount;  // OK - within class
    }
}

BankAccount account = new BankAccount();
// account.balance = 100;  // Error - private
account.Deposit(100);  // OK - public method
```

### Protected
Accessible in class and derived classes.

```csharp
public class Employee {
    protected decimal salary;  // Derived classes can access
}

public class Manager : Employee {
    public void RaiseSalary(decimal amount) {
        salary += amount;  // OK - protected
    }
}
```

### Internal
Accessible within same assembly only.

```csharp
// File1.cs
public class PublicClass { }
internal class InternalClass { }  // Only this assembly

// File2.cs - different assembly
PublicClass obj1 = new PublicClass();  // OK
// InternalClass obj2 = new InternalClass();  // Error
```

### Access Modifier Summary

| Modifier | Same Class | Derived | Assembly | Outside |
|----------|-----------|---------|----------|---------|
| public | ✓ | ✓ | ✓ | ✓ |
| protected | ✓ | ✓ | ✗ | ✗ |
| internal | ✓ | ✗ | ✓ | ✗ |
| private | ✓ | ✗ | ✗ | ✗ |
| protected internal | ✓ | ✓ | ✓ | ✗ |

---

## Properties vs Fields

### Fields (Not Recommended)
Direct access to data.

```csharp
// Bad - no validation
public class Person {
    public int age;  // Direct access
}

Person p = new Person();
p.age = -50;  // Invalid! No validation
```

### Properties (Recommended)
Controlled access with getters/setters.

```csharp
// Good - with validation
public class Person {
    private int _age;
    
    public int Age {
        get { return _age; }
        set {
            if (value >= 0 && value <= 150) {
                _age = value;
            }
        }
    }
}

Person p = new Person();
p.Age = 30;  // Valid
p.Age = -50;  // Rejected by setter
```

### Auto-Properties
Simplified property syntax.

```csharp
public class Product {
    // Auto-property - no backing field needed
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    // Read-only property
    public string Id { get; } = Guid.NewGuid().ToString();
    
    // Property with initialization
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
```

---

## Property Validation

Validate data in setters.

```csharp
public class Employee {
    private string _email;
    
    public string Email {
        get { return _email; }
        set {
            if (IsValidEmail(value)) {
                _email = value;
            } else {
                throw new ArgumentException("Invalid email");
            }
        }
    }
    
    private bool IsValidEmail(string email) {
        return email.Contains("@") && email.Contains(".");
    }
}

var emp = new Employee();
emp.Email = "john@example.com";  // OK
emp.Email = "invalid";  // Throws exception
```

---

## Read-Only and Write-Only Properties

```csharp
public class User {
    // Read-only property
    public string Id { get; } = Guid.NewGuid().ToString();
    
    // Write-only property (rare)
    private string password;
    public string Password {
        set { password = HashPassword(value); }
    }
    
    // Read-only after initialization
    public string Email { get; init; }
}

User user = new User { Email = "user@example.com" };
// user.Email = "other@example.com";  // Error - can't change
```

---

## Real-World Example: Bank Account

```csharp
public class BankAccount {
    private decimal balance;
    private List<string> transactions;
    
    public string AccountNumber { get; private set; }
    public string Owner { get; set; }
    public decimal Balance {
        get { return balance; }
        private set { balance = value; }  // Only settable internally
    }
    
    public BankAccount(string accountNumber, string owner) {
        AccountNumber = accountNumber;
        Owner = owner;
        balance = 0;
        transactions = new List<string>();
    }
    
    public void Deposit(decimal amount) {
        if (amount <= 0) {
            throw new ArgumentException("Amount must be positive");
        }
        balance += amount;
        transactions.Add($"Deposit: +{amount}");
    }
    
    public void Withdraw(decimal amount) {
        if (amount <= 0) {
            throw new ArgumentException("Amount must be positive");
        }
        if (amount > balance) {
            throw new InvalidOperationException("Insufficient funds");
        }
        balance -= amount;
        transactions.Add($"Withdraw: -{amount}");
    }
    
    public List<string> GetTransactionHistory() {
        return transactions.ToList();  // Return copy
    }
}

// Usage
BankAccount account = new BankAccount("12345", "John");
account.Deposit(1000);
account.Withdraw(100);
Console.WriteLine(account.Balance);  // 900

// Cannot directly modify
// account.balance = 5000;  // Error - private
// account.Balance = 5000;  // Error - no public setter
```

---

## Benefits of Encapsulation

✓ **Data Protection**
Internal data cannot be directly modified.

✓ **Validation**
All changes go through setter validation.

✓ **Flexibility**
Can change internal implementation without affecting callers.

✓ **Maintainability**
Clear interface between public and private.

```csharp
// Can change internal implementation
public class Calculator {
    private double result;  // Could change to 'decimal' later
    
    public double Result {
        get { return result; }
    }
}

// Callers don't care about internal type
Calculator calc = new Calculator();
var output = calc.Result;  // Still works if we change internal type
```

---

## Best Practices

✓ **Default to private**
Make everything private, expose only what's needed.

```csharp
// Good - minimal public surface
public class User {
    private string email;
    private string password;
    
    public string Email { get; set; }  // Only Email is public
}

// Bad - too much exposed
public class User {
    public string email;
    public string password;
    public string firstName;
    public string lastName;
}
```

✓ **Use properties with logic**
Use get/set for data with behavior.

```csharp
// Good - property with logic
public decimal Price {
    get { return price; }
    set { price = value > 0 ? value : 0; }
}

// Bad - direct field
public decimal price;
```

✓ **Return copies of mutable data**
Prevent external modification.

```csharp
public class Team {
    private List<Player> players;
    
    // Good - return copy
    public List<Player> GetPlayers() {
        return new List<Player>(players);
    }
    
    // Bad - return reference
    public List<Player> Players {
        get { return players; }  // Can be modified externally
    }
}
```

---

## Common Mistakes

❌ **Public fields**
```csharp
public class Person {
    public int age;  // Anyone can set invalid value
}
```

✓ **Use properties**
```csharp
public class Person {
    private int _age;
    public int Age {
        get { return _age; }
        set { _age = value > 0 ? value : 0; }
    }
}
```

❌ **No validation in setters**
```csharp
public string Email { get; set; }  // No validation
```

✓ **Add validation**
```csharp
private string _email;
public string Email {
    get { return _email; }
    set {
        if (IsValidEmail(value)) {
            _email = value;
        }
    }
}
```

---

## Quick Summary

- Encapsulation hides internal details
- Use private for internal data
- Expose public interface for interaction
- Use properties with validation
- Default to private, expose what's needed
- Protect mutable data from external modification
