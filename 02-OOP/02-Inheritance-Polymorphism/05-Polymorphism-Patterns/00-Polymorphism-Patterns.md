# Polymorphism Patterns

## Overview

Polymorphism ("many forms") allows treating different derived types through a common base type. The correct method runs at runtime based on actual object type, not declared type.

## Runtime Dispatch

Method called depends on actual object type:

```csharp
public class Shape
{
    public virtual double GetArea()
    {
        return 0;
    }
}

public class Circle : Shape
{
    public double Radius { get; set; }
    
    public override double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    
    public override double GetArea()
    {
        return Width * Height;
    }
}

// Usage - runtime dispatch
Shape shape1 = new Circle { Radius = 5 };
Shape shape2 = new Rectangle { Width = 4, Height = 6 };

Console.WriteLine(shape1.GetArea());  // Circle's override
Console.WriteLine(shape2.GetArea());  // Rectangle's override
```

## Polymorphic Collections

Process different types uniformly:

```csharp
List<Shape> shapes = new List<Shape>
{
    new Circle { Radius = 5 },
    new Rectangle { Width = 4, Height = 6 },
    new Circle { Radius = 3 }
};

double totalArea = 0;
foreach (var shape in shapes)
{
    totalArea += shape.GetArea();  // Calls correct override
}

Console.WriteLine($"Total area: {totalArea}");
```

## Strategy Pattern

Different behaviors without if/else:

```csharp
public abstract class PaymentProcessor
{
    public abstract void Process(decimal amount);
}

public class CreditCardProcessor : PaymentProcessor
{
    public override void Process(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} with credit card");
    }
}

public class PayPalProcessor : PaymentProcessor
{
    public override void Process(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} with PayPal");
    }
}

// Usage - polymorphic behavior
PaymentProcessor processor = GetProcessor(paymentMethod);
processor.Process(99.99m);  // Correct implementation called
```

## Template Method Pattern

Base class defines structure, derived classes fill in details:

```csharp
public abstract class ReportGenerator
{
    // Template method - defines the algorithm
    public void Generate()
    {
        OpenConnection();
        FetchData();
        FormatData();
        WriteOutput();
        CloseConnection();
    }
    
    protected abstract void FetchData();
    protected abstract void FormatData();
    
    protected void OpenConnection()
    {
        Console.WriteLine("Connecting to database...");
    }
    
    protected void CloseConnection()
    {
        Console.WriteLine("Closing connection...");
    }
    
    protected void WriteOutput()
    {
        Console.WriteLine("Writing output...");
    }
}

public class SalesReport : ReportGenerator
{
    protected override void FetchData()
    {
        Console.WriteLine("Fetching sales data");
    }
    
    protected override void FormatData()
    {
        Console.WriteLine("Formatting as sales report");
    }
}

// Usage
ReportGenerator report = new SalesReport();
report.Generate();  // Runs template with SalesReport implementations
```

## Open/Closed Principle

Open for extension, closed for modification:

```csharp
// Bad - add new type = modify existing code
public class AnimalSound
{
    public void MakeSound(string type)
    {
        if (type == "Dog")
            Console.WriteLine("Woof!");
        else if (type == "Cat")
            Console.WriteLine("Meow!");
        // Add new type = change this class!
    }
}

// Good - polymorphism allows extension
public abstract class Animal
{
    public abstract void MakeSound();
}

public class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }
}

// Add new type = create new class, no changes needed
public class Bird : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Tweet!");
    }
}
```

## Liskov Substitution Principle

Derived classes should be substitutable for base:

```csharp
// Good - proper substitution
public class Vehicle
{
    public virtual int GetMaxSpeed() => 100;
}

public class Car : Vehicle
{
    public override int GetMaxSpeed() => 200;  // OK - valid override
}

// Everywhere Vehicle is used, Car works too
Vehicle vehicle = new Car();
int speed = vehicle.GetMaxSpeed();  // Works correctly
```

## Multiple Implementations

One interface, many implementations:

```csharp
public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}

public class FileLogger : ILogger
{
    public void Log(string message)
    {
        File.AppendAllText("log.txt", message + Environment.NewLine);
    }
}

// Usage - swap implementations
ILogger logger = new ConsoleLogger();  // Or FileLogger
logger.Log("Something happened");
```

## Factory Pattern with Polymorphism

Create objects without knowing concrete type:

```csharp
public abstract class DatabaseFactory
{
    public abstract IDatabase CreateDatabase();
}

public class MySQLFactory : DatabaseFactory
{
    public override IDatabase CreateDatabase()
    {
        return new MySQLDatabase();
    }
}

public class SqlServerFactory : DatabaseFactory
{
    public override IDatabase CreateDatabase()
    {
        return new SqlServerDatabase();
    }
}

// Usage
DatabaseFactory factory = new MySQLFactory();  // Or SqlServerFactory
IDatabase db = factory.CreateDatabase();
db.Connect();
```

## Benefits of Polymorphism

1. **Code Reuse** - Write once, use for all derived types
2. **Flexibility** - Easy to add new types
3. **Maintainability** - Changes localized to specific classes
4. **Extensibility** - Extend without modifying existing code
5. **Loose Coupling** - Depend on abstractions, not concrete types

## Summary

- **Polymorphism** - Same interface, different behavior
- **Runtime dispatch** - Correct method called based on type
- **Strategy pattern** - Swap behaviors polymorphically
- **Template method** - Base defines structure, derived fills details
- **Open/Closed** - Extend without modifying
- **Liskov substitution** - Derived classes substitute for base
- **Benefits** - Flexible, extensible, maintainable code

## Next Steps

- Learn [Interfaces](../../03-Advanced-OOP/01-Interfaces/00-Interfaces.md) for contracts
- Study [Abstract-Classes](../../03-Advanced-OOP/02-Abstract-Classes/00-Abstract-Classes.md) for required overrides
- Review [Encapsulation](../../03-Advanced-OOP/03-Encapsulation/00-Encapsulation.md) for hiding details
