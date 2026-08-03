# OOP Best Practices

## Overview

Best practices for writing maintainable, extensible, and professional object-oriented code in C#.

## 1. SOLID Principles

### Single Responsibility Principle

```csharp
// Bad - Multiple responsibilities
public class UserManager
{
    public void CreateUser(string name) { }
    public void SendEmail(string email) { }
    public void LogToDatabase(string message) { }
}

// Good - Single responsibility
public class UserService
{
    public void CreateUser(string name) { }
}

public class EmailService
{
    public void SendEmail(string email) { }
}

public class Logger
{
    public void Log(string message) { }
}
```

### Open/Closed Principle

```csharp
// Open for extension, closed for modification
public abstract class ReportGenerator
{
    public abstract void GenerateReport();
}

public class PdfReport : ReportGenerator
{
    public override void GenerateReport() { }
}

// New report types don't modify existing code
public class ExcelReport : ReportGenerator
{
    public override void GenerateReport() { }
}
```

### Liskov Substitution Principle

```csharp
// Derived classes can substitute base
public abstract class Shape
{
    public abstract double GetArea();
}

public class Circle : Shape
{
    public override double GetArea() => Math.PI * Radius * Radius;
}

public class Rectangle : Shape
{
    public override double GetArea() => Width * Height;
}

// Works with any Shape
public void ProcessShape(Shape shape)
{
    Console.WriteLine(shape.GetArea());
}
```

### Interface Segregation Principle

```csharp
// Bad - Fat interface
public interface IWorker
{
    void Work();
    void Manage();
}

// Good - Segregated interfaces
public interface IWorker
{
    void Work();
}

public interface IManager
{
    void Manage();
}

public class Employee : IWorker { }
public class Manager : IWorker, IManager { }
```

### Dependency Inversion Principle

```csharp
// Bad - Depends on concrete class
public class OrderService
{
    private PaymentProcessor _processor = new();
}

// Good - Depends on abstraction
public class OrderService
{
    private readonly IPaymentProcessor _processor;
    
    public OrderService(IPaymentProcessor processor)
    {
        _processor = processor;
    }
}
```

## 2. Design Patterns

### Singleton Pattern

```csharp
public class Database
{
    private static Database _instance;
    
    private Database() { }
    
    public static Database Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Database();
            return _instance;
        }
    }
}

// Modern: C# 8+ with default method
public sealed class AppSettings
{
    private static readonly Lazy<AppSettings> _instance = 
        new(() => new AppSettings());
    
    public static AppSettings Instance => _instance.Value;
}
```

### Factory Pattern

```csharp
public interface IDataProvider
{
    void Connect();
}

public class SqlProvider : IDataProvider { }
public class MongoProvider : IDataProvider { }

public class DataProviderFactory
{
    public static IDataProvider CreateProvider(string type)
    {
        return type switch
        {
            "sql" => new SqlProvider(),
            "mongo" => new MongoProvider(),
            _ => throw new ArgumentException()
        };
    }
}
```

### Observer Pattern

```csharp
public class Subject
{
    private List<IObserver> _observers = new();
    
    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }
    
    public void Notify(string message)
    {
        foreach (var observer in _observers)
            observer.Update(message);
    }
}

public interface IObserver
{
    void Update(string message);
}
```

## 3. Class Design

### Use Composition Over Inheritance

```csharp
// Good - Composition
public class Car
{
    public Engine Engine { get; set; }
    public Wheels Wheels { get; set; }
}

// Bad - Inheritance
public class CarInherit : Engine { }
```

### Immutable Classes

```csharp
// Good - Immutable
public class Point
{
    public int X { get; }
    public int Y { get; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

// C# 9+ - Records
public record Point(int X, int Y);
```

### Encapsulation

```csharp
// Good - Controlled access
public class BankAccount
{
    private decimal _balance;
    
    public decimal Balance
    {
        get { return _balance; }
        private set { _balance = value; }
    }
    
    public void Deposit(decimal amount)
    {
        if (amount > 0)
            _balance += amount;
    }
}
```

## 4. Method Design

### Meaningful Names

```csharp
// Good
public void ProcessUserRegistration(User user) { }
public bool IsEmailValid(string email) { }
public decimal CalculateTotalPrice(Order order) { }

// Bad
public void Handle(object obj) { }
public bool Check(string s) { }
public decimal Calculate() { }
```

### Single Responsibility

```csharp
// Good - One method, one job
public void ValidateUser(User user) { }
public void SaveUser(User user) { }
public void SendConfirmationEmail(User user) { }

// Bad - Too many responsibilities
public void ProcessUser(User user)
{
    // Validate, save, send email - too much
}
```

### Keep Methods Short

```csharp
// Good - Focused, readable
public void ProcessOrder(Order order)
{
    ValidateOrder(order);
    CalculateTotal(order);
    SaveOrder(order);
}

// Bad - Too long, hard to follow
public void ProcessOrder(Order order)
{
    if (order == null) throw new Exception();
    // 50 lines of logic
}
```

## 5. Testing Considerations

### Testable Design

```csharp
// Good - Dependencies injectable
public class OrderService
{
    private readonly IRepository _repository;
    
    public OrderService(IRepository repository)
    {
        _repository = repository;
    }
}

// Bad - Hard to test
public class BadOrderService
{
    private readonly Database _db = new();  // Cannot mock
}
```

### Clear Contracts

```csharp
// Good - Clear preconditions
public decimal Calculate(int quantity, decimal price)
{
    if (quantity < 0) throw new ArgumentException();
    if (price < 0) throw new ArgumentException();
    return quantity * price;
}
```

## 6. Documentation

### XML Comments

```csharp
/// <summary>
/// Calculates the total price including tax.
/// </summary>
/// <param name="subtotal">The price before tax</param>
/// <param name="taxRate">The tax rate (0-1)</param>
/// <returns>Total price with tax</returns>
public decimal CalculateTotal(decimal subtotal, decimal taxRate)
{
    return subtotal * (1 + taxRate);
}
```

## 7. Error Handling

```csharp
// Good - Specific exceptions
public void Deposit(decimal amount)
{
    if (amount <= 0)
        throw new ArgumentException("Amount must be positive");
}

// Bad - Generic exceptions
public void Deposit(decimal amount)
{
    if (amount <= 0)
        throw new Exception("Error");
}
```

## Summary

- **SOLID** - Design principles for maintainable code
- **Patterns** - Proven solutions to common problems
- **Composition** - Often better than inheritance
- **Encapsulation** - Hide implementation details
- **Testing** - Design with testability in mind
- **Documentation** - Clear, helpful comments
- **Error handling** - Meaningful exceptions

## Next Steps

- Review [Common-Mistakes](../02-Common-Mistakes/00-Common-Mistakes.md)
- Study [Interview-Questions](../03-Interview-Questions/00-Interview-Overview.md)
