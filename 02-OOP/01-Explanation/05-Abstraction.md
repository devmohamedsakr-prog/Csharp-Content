# Abstraction

## Overview
Abstraction hides complex implementation details and shows only essential features.

---

## What is Abstraction?

Hiding complexity and exposing only what's necessary to the user.

```csharp
// Complex implementation hidden
public class Car {
    // User just presses button
    public void StartEngine() {
        InitializeEcu();
        CheckBattery();
        EngageStarterMotor();
        InjectFuel();
        IgniteSpark();
        RunIdleCheck();
    }
    
    // Implementation details hidden
    private void InitializeEcu() { }
    private void CheckBattery() { }
    private void EngageStarterMotor() { }
    private void InjectFuel() { }
    private void IgniteSpark() { }
    private void RunIdleCheck() { }
}

// User only sees simple interface
Car car = new Car();
car.StartEngine();  // Don't need to know complexity
```

---

## Abstract Classes

Base class that cannot be instantiated.

```csharp
// Cannot create instance of abstract class
public abstract class Animal {
    public string Name { get; set; }
    
    // Abstract method - must be implemented by derived classes
    public abstract void MakeSound();
    
    // Concrete method - implementation provided
    public void Eat() {
        Console.WriteLine("Eating...");
    }
}

// Cannot do this
// Animal animal = new Animal();  // Error

// Must create derived class
public class Dog : Animal {
    public override void MakeSound() {
        Console.WriteLine("Woof!");
    }
}

Dog dog = new Dog();
dog.MakeSound();  // "Woof!"
dog.Eat();        // "Eating..."
```

---

## Abstract Methods

Methods without implementation, must be overridden.

```csharp
public abstract class Shape {
    public string Name { get; set; }
    
    // Abstract method - no body
    public abstract double CalculateArea();
    
    public abstract double CalculatePerimeter();
    
    // Concrete method
    public virtual void Display() {
        Console.WriteLine($"Shape: {Name}");
    }
}

public class Circle : Shape {
    public double Radius { get; set; }
    
    // Must implement abstract methods
    public override double CalculateArea() {
        return Math.PI * Radius * Radius;
    }
    
    public override double CalculatePerimeter() {
        return 2 * Math.PI * Radius;
    }
}

public class Rectangle : Shape {
    public double Width { get; set; }
    public double Height { get; set; }
    
    public override double CalculateArea() {
        return Width * Height;
    }
    
    public override double CalculatePerimeter() {
        return 2 * (Width + Height);
    }
}

// Usage
Shape circle = new Circle { Name = "Circle", Radius = 5 };
Console.WriteLine(circle.CalculateArea());  // 78.54...

Shape rect = new Rectangle { Name = "Rectangle", Width = 4, Height = 5 };
Console.WriteLine(rect.CalculateArea());  // 20
```

---

## Interfaces

Contract that classes must implement.

```csharp
// Interface defines contract
public interface IPaymentMethod {
    bool ProcessPayment(decimal amount);
    void Refund(decimal amount);
}

// Concrete implementations
public class CreditCard : IPaymentMethod {
    public bool ProcessPayment(decimal amount) {
        Console.WriteLine($"Processing credit card: ${amount}");
        return true;
    }
    
    public void Refund(decimal amount) {
        Console.WriteLine($"Refunding credit card: ${amount}");
    }
}

public class PayPal : IPaymentMethod {
    public bool ProcessPayment(decimal amount) {
        Console.WriteLine($"Processing PayPal: ${amount}");
        return true;
    }
    
    public void Refund(decimal amount) {
        Console.WriteLine($"Refunding PayPal: ${amount}");
    }
}

// Usage
IPaymentMethod payment = new CreditCard();
payment.ProcessPayment(99.99);

payment = new PayPal();
payment.ProcessPayment(99.99);
```

---

## Abstract Class vs Interface

| Feature | Abstract Class | Interface |
|---------|----------------|-----------|
| Instantiation | No | No |
| Constructor | Yes | No |
| Fields | Yes | No |
| Implementation | Can have | Cannot have |
| Methods | Abstract + concrete | Abstract only |
| Access Modifiers | Any | All public |
| Inheritance | Single | Multiple |
| Purpose | Base behavior | Contract |

```csharp
// Abstract class - shared implementation
public abstract class Vehicle {
    public string Make { get; set; }
    
    public virtual void Start() {
        Console.WriteLine("Starting engine");
    }
    
    public abstract void Drive();
}

// Interface - contract only
public interface IElectric {
    void Charge();
    int GetBatteryLevel();
}

// Implementation
public class ElectricCar : Vehicle, IElectric {
    public override void Drive() {
        Console.WriteLine("Driving electrically");
    }
    
    public void Charge() {
        Console.WriteLine("Charging battery");
    }
    
    public int GetBatteryLevel() {
        return 100;
    }
}
```

---

## Multiple Interface Implementation

A class can implement multiple interfaces.

```csharp
public interface IDrawable {
    void Draw();
}

public interface IResizable {
    void Resize(double scale);
}

public interface IMoveable {
    void Move(int x, int y);
}

// Implement multiple interfaces
public class Rectangle : IDrawable, IResizable, IMoveable {
    public void Draw() {
        Console.WriteLine("Drawing rectangle");
    }
    
    public void Resize(double scale) {
        Console.WriteLine($"Resizing by {scale}");
    }
    
    public void Move(int x, int y) {
        Console.WriteLine($"Moving to ({x}, {y})");
    }
}

Rectangle rect = new Rectangle();
rect.Draw();
rect.Resize(1.5);
rect.Move(10, 20);
```

---

## Real-World Example: Logger Abstraction

```csharp
// Abstract interface
public interface ILogger {
    void Log(string message);
    void LogError(string error);
    void LogWarning(string warning);
}

// Console implementation
public class ConsoleLogger : ILogger {
    public void Log(string message) {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
    }
    
    public void LogError(string error) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {error}");
    }
    
    public void LogWarning(string warning) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"WARNING: {warning}");
    }
}

// File implementation
public class FileLogger : ILogger {
    private string filePath = "log.txt";
    
    public void Log(string message) {
        File.AppendAllText(filePath, $"[{DateTime.Now}] {message}\n");
    }
    
    public void LogError(string error) {
        File.AppendAllText(filePath, $"[{DateTime.Now}] ERROR: {error}\n");
    }
    
    public void LogWarning(string warning) {
        File.AppendAllText(filePath, $"[{DateTime.Now}] WARNING: {warning}\n");
    }
}

// Usage - doesn't care about implementation
public class Application {
    private ILogger logger;
    
    public Application(ILogger logger) {
        this.logger = logger;
    }
    
    public void Run() {
        logger.Log("Application started");
        // ... do work
        logger.LogError("An error occurred");
    }
}

// Can use either implementation
Application app1 = new Application(new ConsoleLogger());
app1.Run();

Application app2 = new Application(new FileLogger());
app2.Run();
```

---

## Benefits of Abstraction

✓ **Simplicity**
Complex systems presented simply.

✓ **Flexibility**
Can change implementation without affecting callers.

✓ **Reusability**
Common interface allows interchangeable implementations.

✓ **Maintainability**
Implementation details isolated from users.

---

## Best Practices

✓ **Design to interface, not implementation**
```csharp
// Good
public class Service {
    private IRepository repository;
    
    public Service(IRepository repo) {
        repository = repo;
    }
}

// Bad - tied to concrete class
public class Service {
    private SqlRepository repository = new SqlRepository();
}
```

✓ **Use abstraction for variants**
```csharp
// Good - abstract payment differences
public interface IPaymentProcessor {
    bool Process(decimal amount);
}

// Bad - concrete implementations everywhere
if (paymentType == "credit") { }
else if (paymentType == "paypal") { }
else if (paymentType == "bitcoin") { }
```

✓ **Keep abstractions focused**
```csharp
// Good - single responsibility
public interface ILogger {
    void Log(string message);
}

// Bad - too many methods
public interface IService {
    void Log();
    void Save();
    void Delete();
    void Validate();
}
```

---

## Common Mistakes

❌ **Over-abstraction**
```csharp
// Too many interfaces for simple problem
public interface IRepository { }
public interface IService { }
public interface IValidator { }
```

✓ **Abstract when needed**
```csharp
// Abstract only if multiple implementations
public interface IRepository { }  // Multiple DB types
```

❌ **Violating interface contract**
```csharp
public interface IPaymentProcessor {
    bool Process(decimal amount);
}

public class BadProcessor : IPaymentProcessor {
    public bool Process(decimal amount) {
        // Ignoring amount parameter
        Process(100);
        return true;
    }
}
```

✓ **Follow contract**
```csharp
public class GoodProcessor : IPaymentProcessor {
    public bool Process(decimal amount) {
        // Actually process the amount
        return ProcessPayment(amount);
    }
}
```

---

## Quick Summary

- Abstraction hides complexity, shows only essentials
- Abstract classes provide base implementation
- Interfaces define contracts
- Multiple interfaces possible, single class inheritance
- Design to interface for flexibility
- Change implementation without affecting callers
