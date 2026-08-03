# Polymorphism - Runtime Behavior

## Overview

Polymorphism allows objects of different types to be treated through the same interface, with each responding uniquely to the same method call.

## Types of Polymorphism

### Compile-Time (Static) Polymorphism

```csharp
public class Calculator
{
    // Same method name, different parameters
    public int Add(int a, int b) => a + b;
    public double Add(double a, double b) => a + b;
    public int Add(int a, int b, int c) => a + b + c;
}

// Usage - compiler determines which version
Calculator calc = new Calculator();
int result1 = calc.Add(5, 3);            // Calls int version
double result2 = calc.Add(5.5, 3.2);     // Calls double version
int result3 = calc.Add(5, 3, 2);         // Calls 3-param version
```

### Runtime (Dynamic) Polymorphism

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

// Usage - actual method determined at runtime
Animal animal1 = new Dog();
animal1.MakeSound();  // "Woof!"

Animal animal2 = new Cat();
animal2.MakeSound();  // "Meow!"

// Polymorphic collection
List<Animal> animals = new List<Animal>
{
    new Dog(),
    new Cat(),
    new Dog()
};

foreach (var animal in animals)
{
    animal.MakeSound();  // Each calls its own version
}
```

## Interface Polymorphism

```csharp
public interface IShape
{
    double GetArea();
    void Display();
}

public class Circle : IShape
{
    public double Radius { get; set; }
    
    public double GetArea() => Math.PI * Radius * Radius;
    public void Display() => Console.WriteLine("Circle");
}

public class Rectangle : IShape
{
    public double Width { get; set; }
    public double Height { get; set; }
    
    public double GetArea() => Width * Height;
    public void Display() => Console.WriteLine("Rectangle");
}

// Usage
List<IShape> shapes = new List<IShape>
{
    new Circle { Radius = 5 },
    new Rectangle { Width = 4, Height = 6 }
};

foreach (var shape in shapes)
{
    shape.Display();
    Console.WriteLine($"Area: {shape.GetArea()}");
}
```

## Polymorphic Collections

```csharp
public class PaymentProcessor
{
    public void ProcessPayments(List<IPaymentMethod> methods)
    {
        foreach (var method in methods)
        {
            method.Process(100m);
        }
    }
}

public interface IPaymentMethod
{
    void Process(decimal amount);
}

public class CreditCard : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} via Credit Card");
    }
}

public class PayPal : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} via PayPal");
    }
}

public class Bitcoin : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} via Bitcoin");
    }
}

// Usage
var processor = new PaymentProcessor();
var methods = new List<IPaymentMethod>
{
    new CreditCard(),
    new PayPal(),
    new Bitcoin()
};
processor.ProcessPayments(methods);
```

## Method Overriding Patterns

### Pattern 1: Base Implementation

```csharp
public class Logger
{
    public virtual void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now}] {message}");
    }
}

public class FileLogger : Logger
{
    public override void Log(string message)
    {
        base.Log(message);  // Call base first
        File.AppendAllText("log.txt", message + "\n");
    }
}
```

### Pattern 2: Completely Replace Implementation

```csharp
public class Animal
{
    public virtual void Sleep()
    {
        Console.WriteLine("Animal sleeping");
    }
}

public class Fish : Animal
{
    public override void Sleep()
    {
        // Completely replace - no base.Sleep()
        Console.WriteLine("Fish never sleeps");
    }
}
```

### Pattern 3: Conditional Behavior

```csharp
public class Shape
{
    protected double _size;
    
    public virtual double GetArea()
    {
        return _size * _size;
    }
}

public class SpecialShape : Shape
{
    public override double GetArea()
    {
        if (_size < 0)
            return base.GetArea();  // Use base for negative
        
        return _size * _size * 2;   // Modified behavior for positive
    }
}
```

## Polymorphism Benefits

### 1. Extensibility

```csharp
// Add new types without changing existing code
public class NewPaymentMethod : IPaymentMethod
{
    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} via new method");
    }
}

// Works with existing processor
var processor = new PaymentProcessor();
var methods = new List<IPaymentMethod> { new NewPaymentMethod() };
processor.ProcessPayments(methods);
```

### 2. Code Reusability

```csharp
public class DataProcessor
{
    public void Process(IEnumerable<IDataSource> sources)
    {
        foreach (var source in sources)
        {
            var data = source.GetData();
            // Same processing for all sources
            Console.WriteLine($"Processing: {data}");
        }
    }
}

public interface IDataSource
{
    string GetData();
}

public class FileSource : IDataSource
{
    public string GetData() => File.ReadAllText("data.txt");
}

public class WebSource : IDataSource
{
    public string GetData() => new WebClient().DownloadString("http://example.com");
}

public class DatabaseSource : IDataSource
{
    public string GetData() => database.Query("SELECT * FROM data");
}

// Works with any source
var processor = new DataProcessor();
processor.Process(new List<IDataSource>
{
    new FileSource(),
    new WebSource(),
    new DatabaseSource()
});
```

### 3. Dependency Injection

```csharp
public class OrderService
{
    private readonly IPaymentProcessor _processor;
    
    // Depends on interface, not concrete class
    public OrderService(IPaymentProcessor processor)
    {
        _processor = processor;
    }
    
    public void PlaceOrder(Order order)
    {
        _processor.Process(order.Amount);
    }
}

public interface IPaymentProcessor
{
    void Process(decimal amount);
}

// Can inject any implementation
var processor = new StripeProcessor();
var service = new OrderService(processor);
```

## Common Patterns

### Template Method Pattern

```csharp
public abstract class ReportGenerator
{
    public void Generate()
    {
        LoadData();
        ProcessData();
        FormatOutput();
        SaveReport();
    }
    
    protected abstract void LoadData();
    protected abstract void ProcessData();
    protected abstract void FormatOutput();
    protected abstract void SaveReport();
}

public class PdfReport : ReportGenerator
{
    protected override void LoadData() { }
    protected override void ProcessData() { }
    protected override void FormatOutput() { }
    protected override void SaveReport() { }
}
```

## Best Practices

### 1. Program to Interfaces, Not Implementations

```csharp
// Good - Depends on interface
public class Handler
{
    private readonly ILogger _logger;
    
    public Handler(ILogger logger)
    {
        _logger = logger;
    }
}

// Bad - Depends on concrete class
public class BadHandler
{
    private readonly FileLogger _logger;
    
    public BadHandler()
    {
        _logger = new FileLogger();
    }
}
```

### 2. Don't Overuse Inheritance

```csharp
// Good - Composition
public class Engine { }
public class Car
{
    public Engine Engine { get; set; }
}

// Bad - Unnecessary inheritance
public class BadCar : Engine { }
```

## Summary

- **Compile-time** - Method overloading
- **Runtime** - Virtual methods and interfaces
- **Polymorphic collections** - Treat different types uniformly
- **Benefits** - Extensibility, reusability, maintainability
- **Patterns** - Template method, dependency injection
- **Best practice** - Program to interfaces

## Next Steps

- Learn [Virtual-Methods](../03-Virtual-Methods/00-Virtual-Methods.md) for advanced patterns
- Study [Interfaces](../../03-Advanced-OOP/01-Interfaces/00-Interfaces.md) as design tool
- Review [Abstract-Classes](../../03-Advanced-OOP/02-Abstract-Classes/00-Abstract-Classes.md)
