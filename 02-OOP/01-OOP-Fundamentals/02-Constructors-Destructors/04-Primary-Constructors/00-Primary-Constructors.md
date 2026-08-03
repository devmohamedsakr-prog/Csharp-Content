# Primary Constructors (C# 12.0+)

## Overview

Primary constructors provide a simplified syntax for classes with constructor parameters that are assigned to properties. Available in C# 12.0+, they reduce boilerplate code for simple classes.

## What is a Primary Constructor?

A primary constructor:
- Defined in the class declaration
- Parameters become accessible throughout the class
- Automatically creates read-only properties
- Reduces boilerplate for data classes

```csharp
// Traditional way (pre C# 12)
public class PersonTraditional
{
    public string Name { get; }
    public int Age { get; }
    
    public PersonTraditional(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

// Primary constructor (C# 12+)
public class PersonNew(string name, int age)
{
    public string Name => name;
    public int Age => age;
}

// Usage - same result
var person = new PersonNew("Alice", 30);
Console.WriteLine($"{person.Name}, {person.Age}");  // Alice, 30
```

## Parameters as Class Members

Primary constructor parameters are accessible in the entire class:

```csharp
public class DataLogger(string filePath, string appName)
{
    // Parameters accessible as fields
    public void Log(string message)
    {
        Console.WriteLine($"[{appName}] {message}");
        WriteToFile(filePath, message);
    }
    
    private void WriteToFile(string path, string message)
    {
        // Use filePath and appName parameters
        File.AppendAllText(path, $"{DateTime.Now}: {message}\n");
    }
}

// Usage
var logger = new DataLogger("app.log", "MyApp");
logger.Log("Hello");  // [MyApp] Hello
```

## With Properties and Methods

Combine primary constructor with properties and methods:

```csharp
public class Product(int id, string name, decimal price)
{
    // Properties from primary constructor parameters
    public int Id => id;
    public string Name => name;
    public decimal Price => price;
    
    // Additional property
    public decimal TaxedPrice => price * 1.1m;
    
    // Method using parameters
    public string GetInfo()
    {
        return $"{Name} (ID: {id}) - ${price}";
    }
}

// Usage
var product = new Product(1, "Laptop", 999.99m);
Console.WriteLine(product.GetInfo());  // Laptop (ID: 1) - $999.99
```

## Validation in Primary Constructors

Add constructor bodies for validation:

```csharp
public class BankAccount(string accountNumber, decimal initialBalance)
{
    // Validate parameters in constructor body
    {
        if (string.IsNullOrEmpty(accountNumber))
            throw new ArgumentException("Account number cannot be empty");
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative");
    }
    
    public string AccountNumber => accountNumber;
    public decimal Balance { get; private set; } = initialBalance;
    
    public void Withdraw(decimal amount)
    {
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds");
        Balance -= amount;
    }
}

// Usage
try
{
    var account = new BankAccount("", 1000);  // Throws ArgumentException
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message);
}
```

## Inheritance with Primary Constructors

Derived classes must call base constructor:

```csharp
public class Animal(string name)
{
    public string Name => name;
}

public class Dog(string name, string breed) : Animal(name)
{
    public string Breed => breed;
    
    public void Bark()
    {
        Console.WriteLine($"{Name} says: Woof!");
    }
}

// Usage
var dog = new Dog("Buddy", "Golden");
dog.Bark();  // Buddy says: Woof!
```

## Comparison: Traditional vs Primary

### Simple Data Class

**Traditional:**
```csharp
public class Point
{
    public int X { get; }
    public int Y { get; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}
```

**Primary Constructor:**
```csharp
public class Point(int x, int y)
{
    public int X => x;
    public int Y => y;
}
```

### With Additional Logic

**Traditional:**
```csharp
public class User
{
    public string Username { get; }
    public string Email { get; }
    public DateTime CreatedAt { get; }
    
    public User(string username, string email)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException("Username required");
        
        Username = username;
        Email = email;
        CreatedAt = DateTime.Now;
    }
}
```

**Primary Constructor:**
```csharp
public class User(string username, string email)
{
    if (string.IsNullOrEmpty(username))
        throw new ArgumentException("Username required");
    
    public string Username => username;
    public string Email => email;
    public DateTime CreatedAt { get; } = DateTime.Now;
}
```

## When to Use Primary Constructors

Use primary constructors for:
- Simple data-holding classes
- Immutable types
- Classes that primarily store parameters as properties
- Reducing boilerplate code

Don't use if:
- You need multiple constructors (overloading)
- Complex initialization logic needed
- Parameter validation is extensive

```csharp
// Good use - simple data class
public class Address(string street, string city, string zip)
{
    public string Street => street;
    public string City => city;
    public string Zip => zip;
}

// Not ideal - would need multiple constructors
public class ConfigBad(string appName, string version)
{
    // Would want: ConfigBad(string appName) and ConfigBad() too
    public string AppName => appName;
    public string Version => version;
}
```

## Summary

- **Primary constructor** - Parameters in class declaration (C# 12+)
- **Parameters** - Available throughout the class
- **Simplified syntax** - Less boilerplate
- **Read-only access** - Parameters become read-only
- **Inheritance** - Must call base constructor
- **Best for** - Simple data classes
- **Validation** - Add in constructor body

## Next Steps

- Learn [Instance-Constructors](../01-Instance-Constructors/00-Instance-Constructors.md) for traditional constructors
- Study [Constructor-Chaining](../02-Constructor-Chaining/00-Constructor-Chaining.md) with this/base
- Review [Destructors-IDisposable](../05-Destructors-IDisposable/00-Destructors-IDisposable.md) for cleanup
