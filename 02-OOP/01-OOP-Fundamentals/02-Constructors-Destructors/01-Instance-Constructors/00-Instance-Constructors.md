# Instance Constructors

## Overview

Constructors are special methods that initialize object instances when created with the `new` keyword. Each constructor automatically runs initialization code.

## What is a Constructor?

A constructor is a special method that:
- Runs automatically when an object is created
- Initializes fields and properties
- Has the same name as the class
- Has no return type (not even void)

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor - runs when Person is created
    public Person()
    {
        Console.WriteLine("Person created");
        Name = "Unknown";
        Age = 0;
    }
}

// Usage
var person = new Person();  // Constructor runs automatically
// Output: Person created
```

## Default Constructor

If you don't define a constructor, C# creates an implicit default one:

```csharp
public class Dog
{
    // No explicit constructor
    // C# automatically creates: public Dog() { }
}

Dog dog = new Dog();  // Works with default constructor
```

## Parameterized Constructor

Constructor with parameters for initialization:

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor with parameters
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
        Console.WriteLine($"Person created: {name}, {age}");
    }
}

// Usage
var person = new Person("Alice", 30);
// Output: Person created: Alice, 30
```

## Constructor Overloading

Multiple constructors with different signatures provide flexibility:

```csharp
public class BankAccount
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    
    // Constructor 1: No parameters
    public BankAccount()
    {
        AccountNumber = Guid.NewGuid().ToString();
        Balance = 0;
    }
    
    // Constructor 2: Account number only
    public BankAccount(string accountNumber)
    {
        AccountNumber = accountNumber;
        Balance = 0;
    }
    
    // Constructor 3: Both parameters
    public BankAccount(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }
}

// Usage - choose which constructor to call
var account1 = new BankAccount();                      // Constructor 1
var account2 = new BankAccount("ACC123");              // Constructor 2
var account3 = new BankAccount("ACC456", 1000);        // Constructor 3
```

## Constructor with Initialization

Combine constructor and object initializer:

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    public Product(int id)
    {
        Id = id;
    }
}

// Usage - constructor + initializer
var product = new Product(1)
{
    Name = "Laptop",
    Price = 999.99m
};
```

## Initialization Best Practices

### Always Initialize Members

```csharp
// Bad - members not initialized
public class ConfigBad
{
    public string ConnectionString { get; set; }
    public int Timeout { get; set; }
}

// Good - initialize in constructor
public class ConfigGood
{
    public string ConnectionString { get; set; }
    public int Timeout { get; set; }
    
    public ConfigGood()
    {
        ConnectionString = "Default";
        Timeout = 30;
    }
}
```

### Use Appropriate Defaults

```csharp
public class User
{
    public string Username { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public User()
    {
        Username = "Guest";
        IsActive = true;
        CreatedAt = DateTime.Now;
    }
    
    public User(string username)
    {
        Username = username;
        IsActive = true;
        CreatedAt = DateTime.Now;
    }
}
```

## Summary

- **Constructor** - Automatic initialization method
- **Default constructor** - Created by C# if not defined
- **Parameterized** - Constructor with parameters
- **Overloading** - Multiple constructors allowed
- **Object initializer** - Set properties after construction
- **new keyword** - Triggers constructor call

## Next Steps

- Learn [Constructor-Chaining](../02-Constructor-Chaining/00-Constructor-Chaining.md) to reduce duplication
- Study [Static-Constructors](../03-Static-Constructors/00-Static-Constructors.md) for class initialization
- Review [Destructors-IDisposable](../05-Destructors-IDisposable/00-Destructors-IDisposable.md) for cleanup
