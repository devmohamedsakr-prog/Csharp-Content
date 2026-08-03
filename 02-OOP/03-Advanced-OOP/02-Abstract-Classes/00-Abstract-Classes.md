# Abstract Classes - Base Blueprints

## Overview

Abstract classes provide a base blueprint for derived classes. They cannot be instantiated directly and often contain abstract members that must be implemented by derived classes.

## Abstract Class Definition

```csharp
// Cannot instantiate - must derive
public abstract class Vehicle
{
    // Concrete member - has implementation
    public void Honk()
    {
        Console.WriteLine("Honk!");
    }
    
    // Abstract member - no implementation
    public abstract void Start();
    public abstract void Stop();
}

// Derived class must implement abstract members
public class Car : Vehicle
{
    public override void Start()
    {
        Console.WriteLine("Car engine starting");
    }
    
    public override void Stop()
    {
        Console.WriteLine("Car engine stopping");
    }
}

// Usage
// Vehicle vehicle = new Vehicle();  // ERROR - abstract
Vehicle car = new Car();  // OK
car.Start();  // Car engine starting
```

## Abstract Members

```csharp
public abstract class Animal
{
    // Abstract method - must override
    public abstract void MakeSound();
    
    // Abstract property - must override
    public abstract string Species { get; }
    
    // Concrete method - optional override
    public virtual void Sleep()
    {
        Console.WriteLine("Sleeping");
    }
    
    // Regular method - cannot override
    public void Eat()
    {
        Console.WriteLine("Eating");
    }
}

public class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }
    
    public override string Species
    {
        get { return "Canine"; }
    }
}
```

## Abstract vs Virtual

```csharp
// Abstract - MUST override
public abstract class Base
{
    public abstract void MustOverride();  // Required
}

// Virtual - CAN override
public abstract class Base2
{
    public virtual void CanOverride()     // Optional
    {
        Console.WriteLine("Default");
    }
}

public class Derived : Base
{
    public override void MustOverride()   // Required
    {
    }
}

public class Derived2 : Base2
{
    // Can skip override, gets default
}
```

## Abstract vs Interface

```csharp
// Abstract - Shared implementation
public abstract class DataAccess
{
    // Concrete - shared by all derived
    protected string GetConnectionString()
    {
        return "connection";
    }
    
    // Abstract - must override
    public abstract void Save();
}

// Interface - Contract only
public interface IDataAccess
{
    void Save();
}
```

## Common Patterns

### Pattern 1: Template Method

```csharp
public abstract class ReportGenerator
{
    // Template - defines algorithm
    public void Generate()
    {
        LoadData();
        ProcessData();
        FormatOutput();
    }
    
    // Override these
    protected abstract void LoadData();
    protected abstract void ProcessData();
    protected abstract void FormatOutput();
}

public class SalesReport : ReportGenerator
{
    protected override void LoadData()
    {
        // Load sales data
    }
    
    protected override void ProcessData()
    {
        // Process sales
    }
    
    protected override void FormatOutput()
    {
        // Format for display
    }
}
```

### Pattern 2: Framework Classes

```csharp
public abstract class DbContext
{
    public abstract DbSet<T> Set<T>() where T : class;
    public abstract int SaveChanges();
    
    // Template method
    public void CreateDatabase()
    {
        ConfigureMappings();
        CreateTables();
        SeedData();
    }
    
    protected abstract void ConfigureMappings();
    protected abstract void CreateTables();
    protected abstract void SeedData();
}

public class MyDbContext : DbContext
{
    public override DbSet<T> Set<T>() where T : class
    {
        // Implementation
        return null;
    }
    
    public override int SaveChanges()
    {
        // Implementation
        return 0;
    }
    
    protected override void ConfigureMappings() { }
    protected override void CreateTables() { }
    protected override void SeedData() { }
}
```

## Best Practices

### 1. Use Abstract for Shared Base

```csharp
// Good - Shared implementation
public abstract class DataService
{
    protected string ConnectionString { get; set; }
    
    protected void Connect()
    {
        // Shared connection logic
    }
}

// Bad - No shared implementation
public abstract class Something
{
    public abstract void DoSomething();
    public abstract void DoSomethingElse();
}
```

### 2. Provide Defaults When Possible

```csharp
// Good - Sensible defaults
public abstract class Logger
{
    public virtual void Log(string message)
    {
        Console.WriteLine(message);  // Default
    }
}

// Bad - Everything abstract
public abstract class BadLogger
{
    public abstract void Log(string message);
    public abstract void Error(string message);
    public abstract void Warning(string message);
}
```

## Summary

- **Abstract class** - Cannot instantiate
- **Abstract member** - Must override
- **Virtual member** - Optional override
- **Concrete member** - Shared implementation
- **Template method** - Algorithm structure
- **Inheritance** - Single parent
- **Use case** - Shared base behavior

## Next Steps

- Learn [Encapsulation](../03-Encapsulation/00-Encapsulation.md)
- Study [Static-Members](../04-Static-Members/00-Static-Members.md)
