# Virtual and Override

## Overview

The `virtual` keyword marks a method as overridable. The `override` keyword allows derived classes to provide new implementations. Together they enable polymorphism - calling the correct method based on object type at runtime.

## Virtual Methods

Mark methods as overridable:

```csharp
public class Vehicle
{
    public string Make { get; set; }
    
    // Virtual - can be overridden in derived classes
    public virtual void Start()
    {
        Console.WriteLine("Vehicle starting");
    }
    
    public virtual void Stop()
    {
        Console.WriteLine("Vehicle stopping");
    }
}
```

## Override Keyword

Derived class provides new implementation:

```csharp
public class Car : Vehicle
{
    // Override - replace base implementation
    public override void Start()
    {
        Console.WriteLine("Car engine starting");
    }
    
    public override void Stop()
    {
        Console.WriteLine("Car engine stopping");
    }
}

public class Bicycle : Vehicle
{
    public override void Start()
    {
        Console.WriteLine("Bicycle is ready to go");
    }
    
    public override void Stop()
    {
        Console.WriteLine("Brakes applied");
    }
}

// Usage
Vehicle vehicle1 = new Car();
vehicle1.Start();   // Car engine starting

Vehicle vehicle2 = new Bicycle();
vehicle2.Start();   // Bicycle is ready to go
```

## Virtual Properties

Properties can also be virtual:

```csharp
public class Account
{
    private decimal _balance;
    
    public virtual decimal Balance
    {
        get { return _balance; }
        set { _balance = value; }
    }
}

public class PremiumAccount : Account
{
    private decimal _bonus;
    
    public override decimal Balance
    {
        get { return base.Balance + _bonus; }
        set { base.Balance = value; }
    }
}

// Usage
Account account = new PremiumAccount();
account.Balance = 1000;  // Uses PremiumAccount's override
```

## Method Resolution

The runtime determines which method to call:

```csharp
public class Animal
{
    public virtual void MakeSound()
    {
        Console.WriteLine("Generic sound");
    }
}

public class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }
}

public class Cat : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Meow!");
    }
}

// Usage - runtime decides
Animal[] animals = new Animal[]
{
    new Dog(),
    new Cat(),
    new Animal()
};

foreach (var animal in animals)
{
    animal.MakeSound();  // Calls the right override
}
// Output:
// Woof!
// Meow!
// Generic sound
```

## Sealed Override

Prevent further overriding in derived classes:

```csharp
public class Vehicle
{
    public virtual void Start()
    {
        Console.WriteLine("Starting");
    }
}

public class Car : Vehicle
{
    // Seal this override - no further overriding allowed
    public sealed override void Start()
    {
        Console.WriteLine("Car starting");
    }
}

// ERROR - Cannot override sealed method
// public class SportsCar : Car
// {
//     public override void Start() { }  // Not allowed!
// }
```

## Virtual vs Non-Virtual

Only virtual methods are overridable:

```csharp
public class Base
{
    public virtual void VirtualMethod()
    {
        Console.WriteLine("Virtual");
    }
    
    public void NonVirtualMethod()
    {
        Console.WriteLine("Non-virtual");
    }
}

public class Derived : Base
{
    // Can override
    public override void VirtualMethod()
    {
        Console.WriteLine("Override virtual");
    }
    
    // ERROR - Cannot override non-virtual
    // public override void NonVirtualMethod() { }
}
```

## Performance Consideration

Virtual method calls are slightly slower due to runtime lookup:

```csharp
public class OptimizedClass
{
    // Only mark virtual if you intend to override
    public virtual void ExpectedOverride()
    {
        // Method likely to be overridden
    }
    
    public void NonVirtualHelper()
    {
        // Small helper - not meant to be overridden
        // Slightly faster
    }
}
```

## Abstract Virtual (Next Step)

For methods that MUST be overridden, use `abstract`:

```csharp
public abstract class Shape
{
    // Abstract - MUST be overridden
    public abstract double GetArea();
}

public class Circle : Shape
{
    public double Radius { get; set; }
    
    public override double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
}
```

## Summary

- **virtual** - Mark method as overridable
- **override** - Provide new implementation
- **Runtime dispatch** - Correct method called at runtime
- **sealed override** - Prevent further overriding
- **Abstract virtual** - Required override (covered separately)
- **Performance** - Slight overhead vs non-virtual
- **Use when** - Different behavior needed per derived class

## Next Steps

- Learn [Type-Casting](../04-Type-Casting/00-Type-Casting.md) for conversions
- Study [Polymorphism-Patterns](../05-Polymorphism-Patterns/00-Polymorphism-Patterns.md) for design patterns
- Review [Abstract-Classes](../../03-Advanced-OOP/02-Abstract-Classes/00-Abstract-Classes.md) for required overrides
