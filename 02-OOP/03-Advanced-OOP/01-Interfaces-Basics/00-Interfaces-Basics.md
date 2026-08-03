# Interfaces - Basics

## Overview

An interface defines a contract specifying what methods and properties a class must implement. Interfaces enable polymorphism without inheritance and support multiple implementation (a class can implement multiple interfaces).

## What is an Interface?

An interface:
- Defines a contract/specification
- Contains method and property signatures (no implementation)
- Cannot be instantiated
- Can be inherited by classes and other interfaces
- Member access is implicitly public

```csharp
// Interface definition
public interface IAnimal
{
    string Name { get; set; }
    void Eat();
    void Sleep();
}

// Class implementing interface
public class Dog : IAnimal
{
    public string Name { get; set; }
    
    public void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }
    
    public void Sleep()
    {
        Console.WriteLine($"{Name} is sleeping");
    }
}

// Usage
IAnimal dog = new Dog { Name = "Buddy" };
dog.Eat();    // Buddy is eating
dog.Sleep();  // Buddy is sleeping
```

## Interface Members

Interfaces can contain:
- Methods
- Properties
- Events
- Indexers

```csharp
public interface IRepository
{
    // Property
    int Count { get; }
    
    // Methods
    void Add(object item);
    object GetById(int id);
    void Remove(int id);
    
    // Indexer
    object this[int index] { get; }
}
```

## Multiple Implementation

A class can implement multiple interfaces:

```csharp
public interface IWorker
{
    void Work();
}

public interface IRunner
{
    void Run();
}

public class Person : IWorker, IRunner
{
    public void Work()
    {
        Console.WriteLine("Working");
    }
    
    public void Run()
    {
        Console.WriteLine("Running");
    }
}

// Usage
var person = new Person();
person.Work();  // Working
person.Run();   // Running
```

## Interface Segregation Principle

Keep interfaces focused with single responsibility:

```csharp
// Bad - Too many methods in one interface
public interface IDataServiceBad
{
    void Create(object item);
    void Read();
    void Update(object item);
    void Delete();
    void Export();
    void Import();
    void Backup();
    void Restore();
}

// Good - Segregated interfaces
public interface IRepository
{
    void Create(object item);
    void Read();
    void Update(object item);
    void Delete();
}

public interface IBackupService
{
    void Backup();
    void Restore();
}

public class DataService : IRepository, IBackupService
{
    // Implement only what's needed
}
```

## Interface Inheritance

Interfaces can inherit from other interfaces:

```csharp
public interface IEntity
{
    int Id { get; }
}

public interface IRepository : IEntity
{
    void Save();
}

public class User : IRepository
{
    public int Id { get; set; }
    
    public void Save()
    {
        Console.WriteLine("Saving user");
    }
}
```

## Polymorphism with Interfaces

Use interfaces for polymorphic behavior:

```csharp
List<IWorker> workers = new List<IWorker>
{
    new Engineer(),
    new Manager(),
    new Developer()
};

foreach (var worker in workers)
{
    worker.Work();  // Calls correct implementation
}
```

## Interface vs Class

| Aspect | Interface | Class |
|--------|-----------|-------|
| Instantiate | No | Yes |
| Implementation | No (members are signatures) | Yes |
| Inheritance | Multiple possible | Single |
| Purpose | Define contract | Provide implementation |

## Common Patterns

### Dependency Injection
```csharp
public class Service
{
    private IRepository _repository;
    
    public Service(IRepository repository)
    {
        _repository = repository;
    }
}
```

### Strategy Pattern
```csharp
public interface IPaymentStrategy
{
    void Process(decimal amount);
}

public class PaymentProcessor
{
    private IPaymentStrategy _strategy;
    
    public void ProcessPayment(decimal amount)
    {
        _strategy.Process(amount);
    }
}
```

## Best Practices

### Name with "I" Prefix
```csharp
public interface ILogger { }  // Good
public interface Logger { }   // Bad - looks like class
```

### Keep Interfaces Focused
```csharp
// Good - single responsibility
public interface IPersistent { void Save(); }

// Bad - mixed concerns
public interface IEverything
{
    void Save();
    void Delete();
    void Email();
    void Log();
}
```

### Use Interfaces for Abstraction
```csharp
// Good - depend on interface
public class Service
{
    public Service(IRepository repo) { }
}

// Bad - depend on concrete class
public class ServiceBad
{
    public ServiceBad(UserRepository repo) { }
}
```

## Summary

- **Interface** - Contract defining what to implement
- **Multiple implementation** - Classes can implement multiple interfaces
- **Polymorphism** - Call correct implementation at runtime
- **Segregation** - Keep interfaces focused
- **Dependency injection** - Pass interfaces, not concrete classes
- **Contract** - Specifies behavior without implementation

## Next Steps

- Learn [Abstract-Classes](../02-Abstract-Classes/00-Abstract-Classes.md) for partial implementation
- Study [Access-Modifiers](../04-Access-Modifiers/00-Access-Modifiers.md) for visibility control
- Review [Encapsulation](../03-Encapsulation/00-Encapsulation.md) for hiding implementation
