# Interfaces - Contracts and Abstraction

## Overview

An interface is a contract that defines what methods and properties a class must implement. Interfaces enable loose coupling and polymorphism without inheritance.

## Interface Definition

```csharp
// Define a contract
public interface IShape
{
    // Method contract
    double GetArea();
    void Display();
    
    // Property contract
    string Name { get; set; }
}

// Implement the contract
public class Circle : IShape
{
    public double Radius { get; set; }
    public string Name { get; set; }
    
    public double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
    
    public void Display()
    {
        Console.WriteLine($"Circle: {Name}");
    }
}

// Usage
IShape shape = new Circle { Radius = 5, Name = "MyCircle" };
shape.Display();
Console.WriteLine(shape.GetArea());
```

## Interface Members

```csharp
public interface IDataService
{
    // Method
    void SaveData(string data);
    
    // Property
    string ConnectionString { get; set; }
    
    // Read-only property
    bool IsConnected { get; }
    
    // Event (C# feature)
    event EventHandler DataSaved;
    
    // Indexer
    string this[int index] { get; set; }
}
```

## Multiple Interface Implementation

```csharp
public interface IComparable
{
    int CompareTo(object other);
}

public interface ICloneable
{
    object Clone();
}

public class Product : IComparable, ICloneable
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    public int CompareTo(object other)
    {
        if (other is Product p)
            return Price.CompareTo(p.Price);
        return 0;
    }
    
    public object Clone()
    {
        return new Product { Name = Name, Price = Price };
    }
}

// Usage
var product = new Product { Name = "Laptop", Price = 999 };
var cloned = (Product)product.Clone();
```

## Interface Segregation

```csharp
// Bad - Fat interface
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}

// Good - Segregated interfaces
public interface IWorker
{
    void Work();
}

public interface ILiving
{
    void Eat();
    void Sleep();
}

public class Robot : IWorker
{
    public void Work() { }
    // Doesn't need Eat/Sleep
}

public class Human : IWorker, ILiving
{
    public void Work() { }
    public void Eat() { }
    public void Sleep() { }
}
```

## Interface Inheritance

```csharp
public interface IAnimal
{
    void MakeSound();
}

public interface IDog : IAnimal
{
    void Fetch();
}

public class Dog : IDog
{
    public void MakeSound()
    {
        Console.WriteLine("Woof!");
    }
    
    public void Fetch()
    {
        Console.WriteLine("Fetching...");
    }
}
```

## Default Interface Members (C# 8.0+)

```csharp
public interface ILogger
{
    void Log(string message);
    
    // Default implementation
    void LogError(string message)
    {
        Console.WriteLine($"ERROR: {message}");
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
    // LogError inherited with default implementation
}
```

## Generic Interfaces

```csharp
public interface IRepository<T>
{
    void Add(T item);
    T GetById(int id);
    IEnumerable<T> GetAll();
}

public class UserRepository : IRepository<User>
{
    public void Add(User user) { }
    public User GetById(int id) { return null; }
    public IEnumerable<User> GetAll() { return new List<User>(); }
}
```

## Interface vs Abstract Class

| Feature | Interface | Abstract Class |
|---------|-----------|----------------|
| Inheritance | Multiple | Single |
| Members | Contracts | Implementation |
| Constructors | No | Yes |
| Fields | No | Yes |
| Access modifiers | Limited | Full |
| State | No | Yes |

## Best Practices

### 1. Program to Interfaces

```csharp
// Good
public class OrderService
{
    private readonly IPaymentProcessor _processor;
    
    public OrderService(IPaymentProcessor processor)
    {
        _processor = processor;
    }
}

// Bad - Depends on concrete class
public class BadOrderService
{
    private readonly StripeProcessor _processor = new();
}
```

### 2. Segregate Interfaces

```csharp
// Good - Small, focused interfaces
public interface IRepository { void Save(); }
public interface INotifier { void Notify(string message); }

// Bad - Large interface
public interface IService
{
    void Save();
    void Notify(string message);
}
```

### 3. Use Common Interfaces

```csharp
// Good - Use existing interfaces
public class MyCollection : IEnumerable<T>, IComparable { }

// Bad - Reinvent
public interface IMyIterable { void Iterate(); }
```

## Summary

- **Interface** - Contract/specification
- **Implementation** - Class fulfills contract
- **Multiple** - Implement multiple interfaces
- **Segregation** - Small, focused interfaces
- **Inheritance** - Interfaces can inherit
- **Default members** - C# 8+ adds implementations
- **Generic** - Parameterized interfaces

## Next Steps

- Study [Abstract-Classes](../02-Abstract-Classes/00-Abstract-Classes.md)
- Learn [Encapsulation](../03-Encapsulation/00-Encapsulation.md)
