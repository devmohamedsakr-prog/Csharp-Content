# Constructors and Destructors

This category covers object initialization and cleanup patterns in C#.

## Learning Path

### 1. [Instance Constructors](01-Instance-Constructors/00-Instance-Constructors.md)
Learn how to initialize objects:
- What constructors are
- Default and parameterized constructors
- Constructor overloading
- Object initializers

### 2. [Constructor Chaining](02-Constructor-Chaining/00-Constructor-Chaining.md)
Reduce code duplication:
- Using `this` to chain constructors
- Using `base` for parent constructors
- Avoiding repeated initialization code

### 3. [Static Constructors](03-Static-Constructors/00-Static-Constructors.md)
Initialize class-level data:
- Class initialization before first use
- One static constructor per class
- Setting up static members

### 4. [Primary Constructors](04-Primary-Constructors/00-Primary-Constructors.md)
C# 12+ simplified syntax:
- Simplified parameter definition
- Less boilerplate for simple classes
- Immutable types

### 5. [Destructors and IDisposable](05-Destructors-IDisposable/00-Destructors-IDisposable.md)
Clean up resources:
- Why destructors are unreliable
- IDisposable pattern (recommended)
- Using statements
- IAsyncDisposable for async cleanup

### 6. [Initialization Order](06-Initialization-Order/00-Initialization-Order.md)
Understand the sequence:
- Static vs instance initialization
- When each part runs
- Inheritance initialization order

## Quick Reference

| Topic | Best For |
|-------|----------|
| Instance Constructors | Creating objects with initial values |
| Constructor Chaining | DRY principle - avoiding duplicate code |
| Static Constructors | One-time class setup |
| Primary Constructors | Simple immutable data classes (C# 12+) |
| Destructors/IDisposable | Releasing files, database connections |
| Initialization Order | Understanding lifecycle |

## Common Patterns

### Basic Initialization
```csharp
public class User
{
    public User(string name, int age)
    {
        Name = name;
        Age = age;
    }
    
    public string Name { get; set; }
    public int Age { get; set; }
}
```

### Resource Management
```csharp
using var file = new FileStream("data.txt", FileMode.Open);
// Use file
// Disposed automatically
```

### Constructor Chaining
```csharp
public class Settings
{
    public Settings() : this("localhost", 5432) { }
    public Settings(string host) : this(host, 5432) { }
    public Settings(string host, int port)
    {
        Host = host;
        Port = port;
    }
}
```

## Next Steps

After mastering constructors and destructors:
1. Learn [Properties-Fields](../03-Properties-Fields/README.md) for data management
2. Study [Inheritance](../../02-Inheritance-Polymorphism/01-Inheritance/00-Inheritance.md) for constructor inheritance
3. Review [Access-Modifiers](../../03-Advanced-OOP/05-Access-Modifiers/00-Access-Modifiers.md) for visibility
