# OOP Interview Questions

## Overview

15 progressive OOP interview questions organized by difficulty level. Each answers core concepts, patterns, and best practices.

---

## EASY QUESTIONS (5)

### 1. What is OOP and why is it useful?

**Answer:**
Object-Oriented Programming organizes code into objects that combine data and behavior. Benefits:
- Modularity - Easier to understand and maintain
- Reusability - Code can be reused through inheritance
- Maintainability - Changes localized to objects
- Scalability - Easier to extend with new classes

```csharp
public class Car
{
    // Data (state)
    public string Color { get; set; }
    
    // Behavior (methods)
    public void Drive() { }
}
```

### 2. What's the difference between a class and an object?

**Answer:**
- **Class** - Blueprint/template (like a cookie cutter)
- **Object** - Instance of a class (like a cookie made from that cutter)

```csharp
public class Car { }  // Class - template

var car1 = new Car();  // Object - instance
var car2 = new Car();  // Different object from same class
```

### 3. What are the four pillars of OOP?

**Answer:**
1. **Encapsulation** - Hiding internal details
2. **Inheritance** - Reusing code through hierarchies
3. **Polymorphism** - Objects responding differently to same message
4. **Abstraction** - Exposing essentials, hiding complexity

### 4. Explain inheritance with an example.

**Answer:**
Inheritance allows a derived class to inherit members from a base class:

```csharp
public class Animal
{
    public void Eat() { }
}

public class Dog : Animal
{
    // Inherits Eat() from Animal
    public void Bark() { }
}

var dog = new Dog();
dog.Eat();   // Inherited
dog.Bark();  // Own method
```

### 5. What is polymorphism?

**Answer:**
Ability of objects to respond differently to the same method call:

```csharp
public class Animal { public virtual void Sound() { } }
public class Dog : Animal { public override void Sound() => Console.WriteLine("Woof"); }
public class Cat : Animal { public override void Sound() => Console.WriteLine("Meow"); }

Animal dog = new Dog();
dog.Sound();  // "Woof"

Animal cat = new Cat();
cat.Sound();  // "Meow"
```

---

## MEDIUM QUESTIONS (5)

### 1. Explain the difference between abstract classes and interfaces.

**Answer:**

| Feature | Abstract Class | Interface |
|---------|---|---|
| Inheritance | Single | Multiple |
| Methods | Can have implementation | Usually contracts (C# 8+: default) |
| State | Can have fields | Cannot have fields |
| Constructors | Yes | No |
| Access modifiers | All types | Limited |

Use abstract classes for "IS-A" (Mammal IS-A Animal), interfaces for contracts (IPayable).

### 2. What is encapsulation and how do you achieve it?

**Answer:**
Encapsulation hides internal implementation and controls access to data:

```csharp
public class BankAccount
{
    private decimal _balance;  // Hidden
    
    public decimal Balance
    {
        get { return _balance; }
        set { _balance = value >= 0 ? value : 0; }  // Validation
    }
}
```

Benefits: Protection, flexibility, maintainability.

### 3. When would you use composition over inheritance?

**Answer:**
Use composition when objects have relationships rather than hierarchies:

```csharp
// Bad - Inheritance doesn't fit
public class Dog : Tail { }

// Good - Composition
public class Dog
{
    public Tail Tail { get; set; }  // HAS-A Tail
}

// Rule of thumb: "HAS-A" → Composition, "IS-A" → Inheritance
```

### 4. Explain SOLID principles (any two).

**Answer:**
- **S**ingle Responsibility: One class, one reason to change
- **O**pen/Closed: Open for extension, closed for modification
- **L**iskov Substitution: Derived classes can substitute base
- **I**nterface Segregation: Small, focused interfaces
- **D**ependency Inversion: Depend on abstractions, not concretions

Example:

```csharp
// Good - Open/Closed
public abstract class ReportGenerator { }
public class PdfReport : ReportGenerator { }  // Can add without modifying
public class ExcelReport : ReportGenerator { }  // existing code
```

### 5. What is dependency injection and why is it useful?

**Answer:**
Passing dependencies to a class instead of creating them:

```csharp
// Bad - Hard to test
public class OrderService
{
    private PaymentProcessor _processor = new();
}

// Good - Testable
public class OrderService
{
    private readonly IPaymentProcessor _processor;
    
    public OrderService(IPaymentProcessor processor)
    {
        _processor = processor;
    }
}

// Test with mock
var mockProcessor = new MockPaymentProcessor();
var service = new OrderService(mockProcessor);
```

Benefits: Testability, flexibility, loose coupling.

---

## HARD QUESTIONS (5)

### 1. Design a payment processing system using OOP principles.

**Answer:**
```csharp
// Abstraction
public interface IPaymentMethod
{
    void Process(decimal amount);
}

// Implementations
public class CreditCard : IPaymentMethod
{
    public void Process(decimal amount) { }
}

public class PayPal : IPaymentMethod
{
    public void Process(decimal amount) { }
}

// Service - depends on interface
public class PaymentService
{
    private readonly IPaymentMethod _method;
    
    public PaymentService(IPaymentMethod method)
    {
        _method = method;
    }
    
    public void ProcessPayment(decimal amount)
    {
        ValidateAmount(amount);
        _method.Process(amount);
        LogTransaction(amount);
    }
}

// Extensible - add new payment types without changing PaymentService
```

Principles applied: DIP, SRP, Interface segregation, OCP.

### 2. Explain template method pattern and when to use it.

**Answer:**
Template method defines algorithm structure, letting subclasses override specific steps:

```csharp
public abstract class DataProcessor
{
    public void Process()  // Template method
    {
        Load();
        Transform();
        Save();
    }
    
    protected abstract void Load();
    protected abstract void Transform();
    protected abstract void Save();
}

public class CsvProcessor : DataProcessor
{
    protected override void Load() { }
    protected override void Transform() { }
    protected override void Save() { }
}

// Use when: Common algorithm, specific implementations vary
```

### 3. How would you implement a singleton pattern safely?

**Answer:**
```csharp
// Thread-safe singleton using Lazy<T>
public sealed class Database
{
    private static readonly Lazy<Database> _instance =
        new(() => new Database());
    
    private Database() { }
    
    public static Database Instance => _instance.Value;
}

// Or C# 6 simpler version
public sealed class Config
{
    public static Config Instance { get; } = new();
    
    private Config() { }
}

// Use when: Exactly one instance needed globally
// Caution: Hard to test, couples components
```

### 4. Design an extensible plugin system using OOP.

**Answer:**
```csharp
public interface IPlugin
{
    string Name { get; }
    void Initialize();
    void Execute();
}

public class PluginManager
{
    private readonly Dictionary<string, IPlugin> _plugins = new();
    
    public void Register(IPlugin plugin)
    {
        _plugins[plugin.Name] = plugin;
    }
    
    public void Execute(string pluginName)
    {
        if (_plugins.TryGetValue(pluginName, out var plugin))
        {
            plugin.Execute();
        }
    }
}

// New plugins just implement IPlugin - system is open for extension
```

### 5. Compare inheritance and composition for code reuse. When use each?

**Answer:**
```csharp
// Inheritance - IS-A (class hierarchy)
public class Employee : Person
{
    // IS-A: Employee IS-A Person
}

// Composition - HAS-A (object composition)
public class Employee
{
    public Address Address { get; set; }  // HAS-A Address
    public Department Department { get; set; }  // HAS-A Department
}

// Rule of thumb:
// - Inheritance: Natural hierarchies (Animal → Dog → Puppy)
// - Composition: Flexible relationships (Car HAS-A Engine)
// - Composition is more flexible, easier to change
// - Inheritance is sometimes more natural for true IS-A relationships

// Prefer composition when:
// - Relationships are dynamic
// - Need multiple components
// - Inheritance hierarchy gets deep
```

---

## Interview Tips

### Before Interview
- Review SOLID principles and design patterns
- Practice writing code on paper
- Prepare real-world examples
- Understand trade-offs

### During Interview
1. Clarify the problem
2. Think before coding
3. Explain your approach
4. Consider edge cases
5. Write clean, testable code

### Common Follow-ups
- "How would you test this?"
- "What about performance?"
- "What if requirements change?"
- "How would you handle errors?"
- "Can you improve this design?"

---

## Quick Study Checklist

- [ ] Know four pillars of OOP
- [ ] Understand IS-A vs HAS-A
- [ ] Know SOLID principles
- [ ] Understand design patterns (Factory, Singleton, Observer, Template Method)
- [ ] Composition vs inheritance
- [ ] Abstract classes vs interfaces
- [ ] Encapsulation benefits
- [ ] Dependency injection
- [ ] Can write testable code
- [ ] Recognize anti-patterns

---

## Related Resources

- [Best-Practices](../01-Best-Practices/00-Best-Practices.md)
- [Common-Mistakes](../02-Common-Mistakes/00-Common-Mistakes.md)
- [Classes-Objects](../../01-OOP-Fundamentals/01-Classes-Objects/00-Classes-Objects.md)
- [Interfaces](../../03-Advanced-OOP/01-Interfaces/00-Interfaces.md)

---

## Summary

**15 Questions** covering:
- Fundamentals (5 easy)
- Application (5 medium)
- Design (5 hard)

**Key Concepts:**
- Classes and objects
- Inheritance and polymorphism
- Encapsulation and abstraction
- SOLID principles
- Design patterns
- Testing and design

**Interview Success:** Understand principles, write clean code, explain reasoning, ask clarifying questions.
