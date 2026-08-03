# Advanced OOP Concepts

This category covers advanced object-oriented programming patterns and principles.

## Learning Path

### 1. [Interfaces - Basics](01-Interfaces-Basics/00-Interfaces-Basics.md)
Learn about contracts and multiple implementation:
- Interface definition and members
- Multiple interface implementation
- Interface segregation principle
- Interface inheritance
- Polymorphism with interfaces

### 2. [Abstract Classes](02-Abstract-Classes/00-Abstract-Classes.md)
Combine inheritance with abstract members:
- Abstract class definition
- Abstract methods and properties
- Abstract vs Virtual
- Abstract vs Interface
- Template method pattern

### 3. [Encapsulation](03-Encapsulation/00-Encapsulation.md)
Hide implementation and protect data:
- Private, protected, public access
- Data validation
- Computed properties
- Read-only properties
- Protecting collections

### 4. [Access Modifiers](04-Access-Modifiers/00-Access-Modifiers.md)
Control visibility and scope:
- Public, private, protected
- Internal (assembly level)
- Protected internal and private protected
- Property-level modifiers
- Class access levels

### 5. [Static Members](05-Static-Members/00-Static-Members.md)
Class-level data and methods:
- Static fields and methods
- Static properties
- Static vs Instance
- Shared state
- When to use static

### 6. [Static Classes](06-Static-Classes/00-Static-Classes.md)
Utility classes with only static members:
- Static class definition
- Utility functions
- Extension methods
- Factory methods
- Configuration and constants

## Quick Reference

| Topic | Best For |
|-------|----------|
| Interfaces | Contracts, multiple types |
| Abstract | Shared implementation, enforcement |
| Encapsulation | Data protection, hiding |
| Access Modifiers | Visibility and scope control |
| Static Members | Class-level data and utilities |
| Static Classes | Utility and extension methods |

## Common Patterns

### Interface Contract
```csharp
public interface IRepository
{
    void Save();
    object GetById(int id);
}

public class UserRepository : IRepository
{
    public void Save() { }
    public object GetById(int id) { }
}
```

### Abstract Template Method
```csharp
public abstract class ReportGenerator
{
    public void Generate()
    {
        Load();
        ProcessData();  // Abstract
        Export();
    }
    
    protected abstract void ProcessData();
}
```

### Encapsulated Property
```csharp
public class Account
{
    private decimal _balance;
    
    public decimal Balance
    {
        get { return _balance; }
        set
        {
            if (value < 0) throw new Exception();
            _balance = value;
        }
    }
}
```

### Extension Method
```csharp
public static class StringExtensions
{
    public static string Reverse(this string text)
    {
        var chars = text.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}
```

## SOLID Principles

These topics support SOLID principles:

1. **S** - Single Responsibility (Interfaces, Static Classes)
2. **O** - Open/Closed (Inheritance, Abstract, Interfaces)
3. **L** - Liskov Substitution (Inheritance, Interfaces)
4. **I** - Interface Segregation (Interfaces)
5. **D** - Dependency Inversion (Interfaces, Access Modifiers)

## Relationship to Other Topics

**Prerequisites:**
- [Inheritance Basics](../02-Inheritance-Polymorphism/01-Inheritance-Basics/00-Inheritance-Basics.md)
- [Virtual and Override](../02-Inheritance-Polymorphism/03-Virtual-Override/00-Virtual-Override.md)

**Related:**
- [Classes and Objects](../01-OOP-Fundamentals/01-Classes-Objects/00-Classes-Objects.md)
- [Constructors](../01-OOP-Fundamentals/02-Constructors-Destructors/01-Instance-Constructors/00-Instance-Constructors.md)

## Design Principles

### Favor Composition Over Inheritance
```csharp
// Good - composition
public class Car
{
    public Engine Engine { get; set; }
}

// Less flexible - inheritance
public class CarInherit : Vehicle { }
```

### Program to Interfaces, Not Implementations
```csharp
// Good - depend on interface
public class Service
{
    private IRepository _repo;
}

// Bad - depend on concrete class
public class ServiceBad
{
    private UserRepository _repo;
}
```

### Keep It Simple
```csharp
// Good - clear, focused
public static string Truncate(string text, int length) { }

// Too complex - too many responsibilities
public static string ProcessAllTextOperations(string text) { }
```

## Next Steps

After mastering advanced OOP:
1. Study [Best Practices](../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
2. Review design patterns
3. Learn dependency injection
4. Explore SOLID principles in depth
