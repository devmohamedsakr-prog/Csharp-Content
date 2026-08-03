# Classes and Objects - Complete Guide

## Overview

Classes are the fundamental building blocks of object-oriented programming in C#. A **class** is a blueprint or template that defines the structure and behavior of objects. An **object** is a concrete instance of a class that contains actual data and can perform actions defined by the class.

## What is a Class?

A class defines:
- **Properties/Fields** (state/data) - What the object knows
- **Methods** (behavior/actions) - What the object does
- **Constructors** (initialization) - How the object is created
- **Access modifiers** (visibility rules) - Who can access what

```csharp
// Class blueprint
public class Car
{
    // Properties (state)
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int Speed { get; private set; }
    
    // Method (behavior)
    public void Accelerate()
    {
        Speed += 10;
    }
    
    public void Brake()
    {
        Speed = Math.Max(0, Speed - 10);
    }
    
    public void PrintInfo()
    {
        Console.WriteLine($"{Year} {Make} {Model} - Speed: {Speed}");
    }
}
```

## What is an Object?

An object is an instance of a class created with actual values using the `new` keyword. Each object has its own copy of the class's data members but shares the behavior defined in the class.

```csharp
// Creating objects (instances)
Car car1 = new Car();
car1.Make = "Toyota";
car1.Model = "Camry";
car1.Year = 2023;

Car car2 = new Car();
car2.Make = "Honda";
car2.Model = "Civic";
car2.Year = 2022;

// Each object has separate state
car1.Accelerate();  // car1 speed = 10
car2.Accelerate();  // car2 speed = 10
car1.Accelerate();  // car1 speed = 20, car2 still 10
```

## Class vs Object - Key Differences

| Aspect | Class | Object |
|--------|-------|--------|
| Definition | Blueprint/template | Concrete instance |
| When created | Written in code | Created at runtime with `new` |
| How many | One class definition | Many objects from one class |
| Memory | No memory allocated | Memory allocated when created |
| Unique | One definition | Each object is unique |
| Example | "Car" concept | My specific car "myCar" |

## Class Members Explained

### Fields - Direct Data Storage

Fields store data directly (generally discouraged in favor of properties):

```csharp
public class Person
{
    // Public field (avoid - no control)
    public string name;
    
    // Private field (better)
    private int age;
    
    // Read-only field (set once only)
    public readonly string Id;
    
    // Constant (shared by all)
    public const int MaxAge = 150;
}

// Usage
var person = new Person();
person.name = "Alice";  // Direct access
// person.age = 30;  // ERROR - private
```

**Why avoid public fields?**
- No validation possible
- Can't add logic later without breaking compatibility
- Properties are the preferred C# pattern

### Properties - Controlled Data Access

Properties provide controlled access with getters and setters:

```csharp
public class BankAccount
{
    // Auto-property (simplest form)
    public string AccountNumber { get; set; }
    
    // Property with private setter
    public decimal Balance { get; private set; }
    
    // Property with custom getter/setter
    private decimal _balance;
    public decimal SafeBalance
    {
        get { return _balance; }
        set { _balance = value >= 0 ? value : 0; }  // Validates
    }
    
    // Read-only property (only getter)
    public string Owner { get; }
    
    // Expression-bodied property
    public decimal FormattedBalance => Math.Round(_balance, 2);
    
    // Init-only property (set during init, then read-only)
    public DateTime CreatedDate { get; init; }
}

// Usage
var account = new BankAccount();
account.AccountNumber = "123456";
account.SafeBalance = 1000;
account.SafeBalance = -100;  // Actually sets to 0
Console.WriteLine(account.SafeBalance);  // 0
```

**Benefits of properties over fields:**
- Encapsulation - control how data is accessed
- Validation - ensure data integrity
- Computed values - calculate on-the-fly
- Future changes - add logic without breaking interface

### Methods - Define Behavior

Methods are functions that define what objects can do:

```csharp
public class Calculator
{
    // Method with parameters and return value
    public int Add(int a, int b)
    {
        return a + b;
    }
    
    // Void method (no return value)
    public void PrintResult(int result)
    {
        Console.WriteLine($"Result: {result}");
    }
    
    // Method with optional parameters
    public int Multiply(int a, int b = 2)
    {
        return a * b;
    }
    
    // Method with variable parameters
    public int SumAll(params int[] numbers)
    {
        int sum = 0;
        foreach (int num in numbers)
            sum += num;
        return sum;
    }
}

// Usage
var calc = new Calculator();
int result = calc.Add(5, 3);           // 8
calc.PrintResult(result);              // Prints: 8
int doubled = calc.Multiply(5);        // 10 (default b=2)
int summed = calc.SumAll(1, 2, 3, 4); // 10
```

## Creating Objects

### Default Construction

```csharp
public class Dog
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Create with default constructor
Dog dog = new Dog();
dog.Name = "Buddy";
dog.Age = 5;
```

### Object Initializer Syntax

```csharp
Dog dog = new Dog
{
    Name = "Buddy",
    Age = 5
};
```

### Constructor with Parameters

```csharp
public class Dog
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor
    public Dog(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

Dog dog = new Dog("Buddy", 5);
```

### Combined Constructor and Initializer

```csharp
public class Dog
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Color { get; set; }
    
    public Dog(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

// Constructor + additional initialization
Dog dog = new Dog("Buddy", 5) { Color = "Brown" };
```

## Instance vs Class State

### Instance State (Non-Static Members)

Each object has its own copy of instance members:

```csharp
public class BankAccount
{
    // Instance member - each object has its own
    public decimal Balance { get; set; }
}

var account1 = new BankAccount { Balance = 1000 };
var account2 = new BankAccount { Balance = 2000 };

account1.Balance = 1500;

Console.WriteLine(account1.Balance);  // 1500
Console.WriteLine(account2.Balance);  // 2000 (unchanged)
```

### Class State (Static Members)

All objects share the same value:

```csharp
public class Counter
{
    // Instance member - each object has its own
    public int Count { get; set; }
    
    // Static member - all objects share this
    public static int TotalCount { get; set; }
    
    public void Increment()
    {
        Count++;
        TotalCount++;
    }
}

var c1 = new Counter();
var c2 = new Counter();

c1.Increment();  // c1.Count = 1, Counter.TotalCount = 1
c2.Increment();  // c2.Count = 1, Counter.TotalCount = 2

Console.WriteLine(c1.Count);          // 1
Console.WriteLine(c2.Count);          // 1
Console.WriteLine(Counter.TotalCount); // 2 (shared)
```

## Object Identity and Equality

### Object Identity (Reference Equality)

Each object is a unique entity in memory:

```csharp
public class Book
{
    public string Title { get; set; }
}

Book book1 = new Book { Title = "C# Guide" };
Book book2 = new Book { Title = "C# Guide" };
Book book3 = book1;

// Identity - are they the same object?
Console.WriteLine(book1 == book2);  // false (different objects)
Console.WriteLine(book1 == book3);  // true (same object reference)
Console.WriteLine(ReferenceEquals(book1, book2));  // false
Console.WriteLine(ReferenceEquals(book1, book3));  // true
```

### Object Equality (Value Equality)

Same content, possibly different objects:

```csharp
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    
    // Override Equals for value comparison
    public override bool Equals(object obj)
    {
        if (obj is Book other)
            return Title == other.Title && Author == other.Author;
        return false;
    }
    
    // Override GetHashCode when overriding Equals
    public override int GetHashCode()
    {
        return HashCode.Combine(Title, Author);
    }
}

Book book1 = new Book { Title = "C#", Author = "John" };
Book book2 = new Book { Title = "C#", Author = "John" };

Console.WriteLine(book1 == book2);        // false (different objects)
Console.WriteLine(book1.Equals(book2));   // true (same content)
```

## The `this` Keyword

Reference to the current object instance:

```csharp
public class Person
{
    private string name;
    private int age;
    
    public Person(string name, int age)
    {
        // `this` refers to current object instance
        this.name = name;
        this.age = age;
    }
    
    public void PrintInfo()
    {
        Console.WriteLine($"Name: {this.name}, Age: {this.age}");
    }
    
    // Calling another method
    public void PrintFullInfo()
    {
        this.PrintInfo();
        Console.WriteLine($"Birth Year: {DateTime.Now.Year - this.age}");
    }
    
    // Returning this object
    public Person Clone()
    {
        return new Person(this.name, this.age);
    }
}
```

## Object Lifetime

### Creation
```csharp
Car car = new Car();  // Object created, memory allocated
```

### Usage
```csharp
car.Accelerate();      // Object is accessible
Console.WriteLine(car.Speed);
```

### Destruction
```csharp
car = null;  // Reference removed
// Object eligible for garbage collection
// Memory reclaimed when GC runs
```

## Common Class Patterns

### Pattern 1: Simple Data Class

Holds data with minimal logic:

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

var product = new Product 
{ 
    Id = 1, 
    Name = "Laptop", 
    Price = 999.99m, 
    Quantity = 10 
};
```

### Pattern 2: Service Class

Provides operations/functionality:

```csharp
public class OrderService
{
    private readonly Database _db;
    
    public OrderService(Database db)
    {
        _db = db;
    }
    
    public void PlaceOrder(Order order)
    {
        ValidateOrder(order);
        _db.SaveOrder(order);
        NotifyCustomer(order);
    }
    
    private void ValidateOrder(Order order) { }
    private void NotifyCustomer(Order order) { }
}
```

### Pattern 3: Domain Object

Models a business concept with behavior:

```csharp
public class BankAccount
{
    public string AccountNumber { get; }
    public decimal Balance { get; private set; }
    
    public BankAccount(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }
    
    public void Deposit(decimal amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Amount must be positive");
        Balance += amount;
    }
    
    public bool Withdraw(decimal amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Amount must be positive");
        if (amount > Balance) 
            return false;
        
        Balance -= amount;
        return true;
    }
}
```

## Copying Objects

### Shallow Copy (Reference Copy)

```csharp
Car car1 = new Car { Make = "Toyota", Model = "Camry" };
Car car2 = car1;  // car2 points to SAME object

car2.Make = "Honda";
Console.WriteLine(car1.Make);  // Honda (both changed!)
```

### Deep Copy

```csharp
Car car1 = new Car { Make = "Toyota", Model = "Camry" };
Car car2 = new Car { Make = car1.Make, Model = car1.Model };

car2.Make = "Honda";
Console.WriteLine(car1.Make);  // Toyota (unchanged)
```

## Summary

- **Class** - Blueprint/template that defines structure and behavior
- **Object** - Instance of a class with specific data
- **Properties** - Encapsulated data access with validation
- **Methods** - Define behavior and actions
- **Instance members** - Each object has its own copy
- **Static members** - All objects share same value
- **Identity vs Equality** - Important distinction
- **this** - Reference to current object
- **Lifetime** - Creation, usage, garbage collection

## Next Steps

- Learn [Constructors-Destructors](../02-Constructors-Destructors/00-Constructors-Destructors.md) for initialization
- Study [Properties-Fields](../03-Properties-Fields/00-Properties-Fields.md) for data management
- Review [Inheritance](../../02-Inheritance-Polymorphism/01-Inheritance/00-Inheritance.md) for code reuse
- Explore [Encapsulation](../../03-Advanced-OOP/03-Encapsulation/00-Encapsulation.md) for data protection
