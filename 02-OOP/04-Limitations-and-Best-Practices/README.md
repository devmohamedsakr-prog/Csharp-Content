# Limitations and Best Practices

Understanding OOP constraints and avoiding common pitfalls.

## ⚠️ Limitations of OOP

### 1. Learning Curve
OOP concepts can be overwhelming for beginners.

**Challenge:**
```csharp
// Difficult for beginners to understand
public abstract class BaseEntity<T> where T : class
{
    // Complex generic constraints and inheritance
}
```

**Best Practice:**
- Start with simple classes
- Build understanding gradually
- Use concrete examples

### 2. Performance Overhead
Object creation and method calls have performance costs.

**Problem:**
```csharp
// Creating thousands of objects can be slow
for (int i = 0; i < 1_000_000; i++)
{
    var obj = new Point { X = i, Y = i * 2 };  // Overhead per creation
}
```

**Solution:**
```csharp
// Use value types for high-frequency objects
public struct Point  // Struct instead of class
{
    public int X { get; set; }
    public int Y { get; set; }
}

// Use object pooling for expensive objects
public class ObjectPool<T> where T : class, new()
{
    private Stack<T> _available = new();
    
    public T Rent()
    {
        return _available.Count > 0 ? _available.Pop() : new T();
    }
    
    public void Return(T obj)
    {
        _available.Push(obj);
    }
}
```

### 3. Over-Engineering
Not all problems need complex OOP solutions.

**Over-Complicated:**
```csharp
// Overkill for a simple calculation
public abstract class Calculator
{
    public abstract int Calculate(int a, int b);
}

public class AdderCalculator : Calculator
{
    public override int Calculate(int a, int b) => a + b;
}
```

**Simpler Alternative:**
```csharp
// Just use a method
public static int Add(int a, int b) => a + b;
```

### 4. Tight Coupling
Poor OOP design can create interdependent classes.

**Bad - Tightly Coupled:**
```csharp
public class OrderService
{
    public void ProcessOrder(Order order)
    {
        // Directly creates dependency - hard to test
        var paymentProcessor = new CreditCardProcessor();
        paymentProcessor.Process(order.Total);
    }
}
```

**Good - Loose Coupling:**
```csharp
public class OrderService
{
    private readonly IPaymentProcessor _paymentProcessor;
    
    public OrderService(IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;  // Injected dependency
    }
    
    public void ProcessOrder(Order order)
    {
        _paymentProcessor.Process(order.Total);
    }
}
```

### 5. Fragile Base Class Problem
Changes to base classes can break derived classes.

**Problem:**
```csharp
public class BaseEmployee
{
    public virtual decimal CalculateSalary()
    {
        return 5000;
    }
}

public class Manager : BaseEmployee
{
    public override decimal CalculateSalary()
    {
        return base.CalculateSalary() * 1.5m;  // Depends on base implementation
    }
}

// If base class changes, derived class may break
```

**Solution:**
```csharp
// Use composition over inheritance
public class Manager
{
    private readonly SalaryCalculator _calculator;
    
    public decimal CalculateSalary()
    {
        return _calculator.Calculate(EmployeeType.Manager);
    }
}
```

---

## ✨ Best Practices

### 1. Single Responsibility Principle (SRP)
A class should have only one reason to change.

```csharp
// ❌ Bad - Multiple responsibilities
public class User
{
    public void Register() { }
    public void SendEmail() { }
    public void SaveToDatabase() { }
    public void LogActivity() { }
}

// ✅ Good - Single responsibility
public class User
{
    public string Email { get; set; }
    public void Register() { }
}

public class EmailService
{
    public void SendEmail(User user) { }
}

public class UserRepository
{
    public void Save(User user) { }
}

public class ActivityLogger
{
    public void Log(string activity) { }
}
```

### 2. Open/Closed Principle
Classes should be open for extension, closed for modification.

```csharp
// ✅ Good - Easy to extend without modifying
public interface INotification
{
    void Send(string message);
}

public class EmailNotification : INotification
{
    public void Send(string message) { }
}

public class SMSNotification : INotification
{
    public void Send(string message) { }
}

// Add new notification type without changing existing code
public class SlackNotification : INotification
{
    public void Send(string message) { }
}
```

### 3. Liskov Substitution Principle (LSP)
Derived classes should be substitutable for base classes.

```csharp
// ✅ Good - Proper inheritance
public abstract class Bird
{
    public abstract void Move();
}

public class Eagle : Bird
{
    public override void Move() => Console.WriteLine("Flying");
}

public class Penguin : Bird
{
    public override void Move() => Console.WriteLine("Swimming");
}

// Can use any bird type interchangeably
Bird bird = new Eagle();
bird.Move();  // Works correctly
```

### 4. Interface Segregation Principle
Many specific interfaces are better than one general interface.

```csharp
// ❌ Bad - Too many unrelated methods
public interface IWorker
{
    void Work();
    void Manage();
    void Report();
}

// ✅ Good - Segregated interfaces
public interface IWorker
{
    void Work();
}

public interface IManager
{
    void Manage();
}

public interface IReporter
{
    void Report();
}

public class Developer : IWorker
{
    public void Work() { }
}
```

### 5. Dependency Inversion Principle
Depend on abstractions, not concrete implementations.

```csharp
// ✅ Good - Depends on abstraction
public class ShoppingCart
{
    private readonly IPaymentProcessor _processor;
    
    public ShoppingCart(IPaymentProcessor processor)
    {
        _processor = processor;  // Depends on interface
    }
}
```

### 6. Composition Over Inheritance
Prefer has-a relationships over is-a relationships.

```csharp
// ❌ Bad - Deep inheritance chain
public class Person { }
public class Employee : Person { }
public class Manager : Employee { }

// ✅ Good - Composition
public class Person
{
    public string Name { get; set; }
}

public class Employee
{
    public Person Person { get; set; }
    public Role Role { get; set; }
}
```

---

## 📚 Files in This Section

- `01-Common-Limitations.md` - Understanding constraints
- `02-Performance-Considerations.md` - Optimization strategies
- `03-SOLID-Principles.md` - Design principles
- `04-Design-Patterns.md` - Proven solutions
- `05-Anti-Patterns.md` - What to avoid

---

## 🎯 Key Takeaway

OOP is powerful but requires discipline:
- Understand its limitations
- Follow SOLID principles
- Avoid over-engineering
- Use patterns appropriately
- Measure and optimize where needed

