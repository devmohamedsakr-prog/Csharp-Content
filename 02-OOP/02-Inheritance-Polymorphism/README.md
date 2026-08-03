# Inheritance and Polymorphism

This category covers inheritance hierarchies and polymorphic behavior in C#.

## Learning Path

### 1. [Inheritance Basics](01-Inheritance-Basics/00-Inheritance-Basics.md)
Learn how classes inherit from parent classes:
- Single inheritance model
- What gets inherited (public, protected, private)
- Class hierarchies
- Constructor inheritance
- IS-A relationships

### 2. [Base Class Members](02-Base-Class-Members/00-Base-Class-Members.md)
Access and extend parent functionality:
- Using `base` keyword
- Calling parent methods
- Extending functionality
- Base constructor calls
- Initialization patterns

### 3. [Virtual and Override](03-Virtual-Override/00-Virtual-Override.md)
Enable polymorphic behavior:
- Virtual methods
- Override implementations
- Virtual properties
- Method resolution at runtime
- Sealed overrides

### 4. [Type Casting](04-Type-Casting/00-Type-Casting.md)
Convert between derived and base types:
- Upcasting (safe)
- Downcasting (requires checking)
- `is` keyword type checking
- `as` safe casting
- Pattern matching (C# 7+)

### 5. [Polymorphism Patterns](05-Polymorphism-Patterns/00-Polymorphism-Patterns.md)
Design patterns using polymorphism:
- Runtime dispatch
- Polymorphic collections
- Strategy pattern
- Template method
- Open/Closed principle
- Liskov substitution

## Quick Reference

| Topic | Best For |
|-------|----------|
| Inheritance Basics | Understanding class hierarchies |
| Base Class Members | Extending parent behavior |
| Virtual/Override | Enabling polymorphic behavior |
| Type Casting | Safe type conversions |
| Polymorphism Patterns | Design patterns and SOLID principles |

## Common Patterns

### Basic Inheritance
```csharp
public class Animal { }
public class Dog : Animal { }  // Dog IS-A Animal
```

### Virtual Override
```csharp
public class Vehicle
{
    public virtual void Start() { }
}

public class Car : Vehicle
{
    public override void Start() { }
}
```

### Safe Downcasting
```csharp
Animal animal = GetAnimal();
if (animal is Dog dog)
{
    dog.Bark();
}
```

### Polymorphic Collections
```csharp
List<Shape> shapes = new List<Shape>
{
    new Circle(),
    new Rectangle()
};

foreach (var shape in shapes)
{
    Console.WriteLine(shape.GetArea());  // Correct override
}
```

## Relationship to Other Topics

**Prerequisites:**
- [Classes and Objects](../01-OOP-Fundamentals/01-Classes-Objects/00-Classes-Objects.md)
- [Constructors](../01-OOP-Fundamentals/02-Constructors-Destructors/01-Instance-Constructors/00-Instance-Constructors.md)

**Related:**
- [Interfaces](../03-Advanced-OOP/01-Interfaces/00-Interfaces.md) - Alternative to inheritance
- [Abstract Classes](../03-Advanced-OOP/02-Abstract-Classes/00-Abstract-Classes.md) - Required overrides
- [Encapsulation](../03-Advanced-OOP/03-Encapsulation/00-Encapsulation.md) - Hide implementation

## Key Principles

1. **IS-A Relationship** - Use inheritance when "is a" applies
2. **Liskov Substitution** - Derived classes should substitute for base
3. **Open/Closed** - Open for extension, closed for modification
4. **Composition over Inheritance** - Prefer composition for "has a"

## Next Steps

After mastering inheritance and polymorphism:
1. Learn [Interfaces](../03-Advanced-OOP/01-Interfaces/00-Interfaces.md) for contracts
2. Study [Abstract-Classes](../03-Advanced-OOP/02-Abstract-Classes/00-Abstract-Classes.md) for required overrides
3. Review [Best-Practices](../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) for design guidance
