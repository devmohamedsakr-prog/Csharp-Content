# Abstract Classes

## Overview

An abstract class is a partially implemented class that cannot be instantiated directly. It provides a base implementation and defines abstract members that derived classes must override, combining inheritance with enforced method implementation.

## Abstract Class Definition

Mark a class as abstract to prevent direct instantiation:

```csharp
// Abstract class - cannot instantiate
public abstract class Vehicle
{
    public string Make { get; set; }
    
    // Regular method
    public void DisplayMake()
    {
        Console.WriteLine($"Make: {Make}");
    }
    
    // Abstract method - must be overridden
    public abstract void Start();
}

// ERROR: Cannot instantiate abstract class
// var vehicle = new Vehicle();

// Derived class must implement abstract member
public class Car : Vehicle
{
    public override void Start()
    {
        Console.WriteLine("Car engine starting");
    }
}

// OK - Can instantiate concrete class
var car = new Car();
car.Start();  // Car engine starting
```

## Abstract Members

Abstract methods and properties have no implementation:

```csharp
public abstract class Employee
{
    // Abstract method
    public abstract decimal CalculateBonus();
    
    // Abstract property
    public abstract string JobTitle { get; set; }
    
    // Regular method (optional to override)
    public void DisplayInfo()
    {
        Console.WriteLine($"{JobTitle}");
    }
}

public class Manager : Employee
{
    public override string JobTitle { get; set; }
    
    public override decimal CalculateBonus()
    {
        return 5000;  // Must implement
    }
}
```

## Abstract vs Virtual

| Aspect | Abstract | Virtual |
|--------|----------|---------|
| Implementation | None required | Has default |
| Override | Must | Optional |
| Instantiate | Cannot | Can |
| Purpose | Enforce contract | Provide default |

```csharp
// Abstract - MUST override
public abstract class Shape
{
    public abstract double GetArea();
}

// Virtual - CAN override
public class Collection
{
    public virtual int Count() { return 0; }
}
```

## Multiple Levels of Abstraction

Hierarchy of abstract classes:

```csharp
// Level 1 - Abstract base
public abstract class Animal
{
    public abstract void MakeSound();
}

// Level 2 - Partial implementation
public abstract class Mammal : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Generic mammal sound");
    }
    
    public abstract void Nurse();
}

// Level 3 - Concrete implementation
public class Dog : Mammal
{
    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }
    
    public override void Nurse()
    {
        Console.WriteLine("Nursing pups");
    }
}
```

## Abstract vs Interface

| Aspect | Abstract | Interface |
|--------|----------|-----------|
| Implementation | Partial | None |
| State (fields) | Yes | No |
| Access modifiers | Any | Public only |
| Inheritance | Single | Multiple |
| Purpose | Shared base | Contract |

```csharp
// Abstract - provides shared behavior
public abstract class Vehicle
{
    protected string _make;  // Shared field
    public abstract void Start();
}

// Interface - just contract
public interface IVehicle
{
    void Start();
}
```

## Template Method Pattern

Abstract class defines structure, derived classes fill details:

```csharp
public abstract class DataProcessor
{
    // Template method - defines algorithm
    public void Process()
    {
        LoadData();
        ValidateData();
        TransformData();
        SaveData();
    }
    
    protected virtual void LoadData()
    {
        Console.WriteLine("Loading data...");
    }
    
    protected abstract void ValidateData();
    protected abstract void TransformData();
    
    protected virtual void SaveData()
    {
        Console.WriteLine("Saving data...");
    }
}

public class CsvProcessor : DataProcessor
{
    protected override void ValidateData()
    {
        Console.WriteLine("Validating CSV");
    }
    
    protected override void TransformData()
    {
        Console.WriteLine("Transforming CSV");
    }
}

// Usage
DataProcessor processor = new CsvProcessor();
processor.Process();  // Runs template with overrides
```

## Best Practices

### Use Abstract for Shared Implementation

```csharp
// Good - share common behavior
public abstract class Repository
{
    public virtual void Connect()
    {
        Console.WriteLine("Connecting...");
    }
    
    public abstract void Save();
}

// Bad - no shared code, use interface instead
public abstract class IAltBad
{
    public abstract void Method1();
    public abstract void Method2();
}
```

### Keep Abstract Members Simple

```csharp
// Good - single responsibility
public abstract void Process();

// Bad - too many abstract methods
public abstract void ProcessAndValidateAndSaveAndEmail();
```

### Document Expected Implementation

```csharp
public abstract class Service
{
    /// <summary>
    /// Validates the input data.
    /// Must check for null and empty values.
    /// </summary>
    public abstract bool Validate(object data);
}
```

## Common Mistakes

### Forgetting `override` Keyword

```csharp
// Bad - compiler error
public class DerivedBad : Abstract
{
    public void AbstractMethod()  // Missing override!
    {
    }
}

// Good
public class DerivedGood : Abstract
{
    public override void AbstractMethod()
    {
    }
}
```

### Abstract Class vs Interface Confusion

```csharp
// Use abstract when you have shared code
public abstract class BaseRepository
{
    protected string _connectionString;  // Shared
    public abstract void Save();
}

// Use interface when just defining contract
public interface ILogger
{
    void Log(string message);
}
```

## Summary

- **Abstract class** - Partially implemented, cannot instantiate
- **Abstract members** - Must be overridden by derived class
- **Template method** - Base defines structure, derived fill details
- **vs Virtual** - Abstract forces override, virtual is optional
- **vs Interface** - Abstract allows state, interface is contract
- **Inheritance** - Single inheritance, but can implement interfaces

## Next Steps

- Learn [Interfaces-Basics](../01-Interfaces-Basics/00-Interfaces-Basics.md) for contracts
- Study [Access-Modifiers](../04-Access-Modifiers/00-Access-Modifiers.md) for visibility
- Review [Encapsulation](../03-Encapsulation/00-Encapsulation.md) for hiding details
