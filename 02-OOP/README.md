# Object-Oriented Programming (OOP)

Comprehensive guide to OOP concepts in C#, organized by topic with clear focus on single concepts per file.

## Structure

This module is organized into 4 main categories:

### [01-OOP-Fundamentals](01-OOP-Fundamentals/README.md)
Foundation concepts every developer needs to know:
- **Classes and Objects** - Blueprint and instances
- **Constructors-Destructors** (6 focused files)
  - Instance Constructors
  - Constructor Chaining
  - Static Constructors
  - Primary Constructors (C# 12+)
  - Destructors and IDisposable
  - Initialization Order
- **Properties-Fields** - Data management
- See [01-OOP-Fundamentals/README.md](01-OOP-Fundamentals/README.md) for details

### [02-Inheritance-Polymorphism](02-Inheritance-Polymorphism/README.md)
Code reuse and dynamic behavior (5 focused files):
- **Inheritance Basics** - Class hierarchies
- **Base Class Members** - Using base keyword
- **Virtual and Override** - Polymorphic behavior
- **Type Casting** - Safe conversions
- **Polymorphism Patterns** - Design patterns and SOLID
- See [02-Inheritance-Polymorphism/README.md](02-Inheritance-Polymorphism/README.md) for details

### [03-Advanced-OOP](03-Advanced-OOP/README.md)
Advanced patterns and principles (6 focused files):
- **Interfaces Basics** - Contracts and multiple implementation
- **Abstract Classes** - Partial implementation and enforcement
- **Encapsulation** - Data protection
- **Access Modifiers** - Visibility control
- **Static Members** - Class-level data
- **Static Classes** - Utility classes
- See [03-Advanced-OOP/README.md](03-Advanced-OOP/README.md) for details

### [04-Best-Practices-Interview](04-Best-Practices-Interview/README.md)
Professional practices and interview preparation:
- **Best Practices** - SOLID, design patterns, class/method design
- **Common Mistakes** - What to avoid
- **Interview Questions** - Preparation and key concepts
- See [04-Best-Practices-Interview/README.md](04-Best-Practices-Interview/README.md) for details

## Learning Paths

### Beginner Path (Start Here)
1. [Classes and Objects](01-OOP-Fundamentals/01-Classes-Objects/00-Classes-Objects.md)
2. [Instance Constructors](01-OOP-Fundamentals/02-Constructors-Destructors/01-Instance-Constructors/00-Instance-Constructors.md)
3. [Properties and Fields](01-OOP-Fundamentals/03-Properties-Fields/00-Properties-Fields.md)
4. [Inheritance Basics](02-Inheritance-Polymorphism/01-Inheritance-Basics/00-Inheritance-Basics.md)
5. [Interfaces Basics](03-Advanced-OOP/01-Interfaces-Basics/00-Interfaces-Basics.md)

### Intermediate Path
1. Complete Beginner Path
2. [Constructor Chaining](01-OOP-Fundamentals/02-Constructors-Destructors/02-Constructor-Chaining/00-Constructor-Chaining.md)
3. [Base Class Members](02-Inheritance-Polymorphism/02-Base-Class-Members/00-Base-Class-Members.md)
4. [Virtual and Override](02-Inheritance-Polymorphism/03-Virtual-Override/00-Virtual-Override.md)
5. [Encapsulation](03-Advanced-OOP/03-Encapsulation/00-Encapsulation.md)
6. [Access Modifiers](03-Advanced-OOP/04-Access-Modifiers/00-Access-Modifiers.md)

### Advanced Path
1. Complete Intermediate Path
2. [Abstract Classes](03-Advanced-OOP/02-Abstract-Classes/00-Abstract-Classes.md)
3. [Type Casting](02-Inheritance-Polymorphism/04-Type-Casting/00-Type-Casting.md)
4. [Polymorphism Patterns](02-Inheritance-Polymorphism/05-Polymorphism-Patterns/00-Polymorphism-Patterns.md)
5. [Static Members](03-Advanced-OOP/05-Static-Members/00-Static-Members.md)
6. [Static Classes](03-Advanced-OOP/06-Static-Classes/00-Static-Classes.md)
7. [Best Practices](04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)

## Key Concepts at a Glance

| Concept | Purpose | Example |
|---------|---------|---------|
| **Class** | Blueprint for objects | `public class Dog { }` |
| **Object** | Instance of class | `var dog = new Dog();` |
| **Inheritance** | Code reuse | `public class Dog : Animal { }` |
| **Polymorphism** | Different behavior, same interface | `virtual` + `override` |
| **Encapsulation** | Hide implementation | `private` fields, `public` properties |
| **Abstraction** | Hide complexity | `abstract`, `interface` |
| **Interface** | Contract/specification | `public interface IAnimal { }` |
| **Abstract Class** | Partial implementation | `public abstract class Shape { }` |

## SOLID Principles

**S** - Single Responsibility: One class = one reason to change
**O** - Open/Closed: Open for extension, closed for modification
**L** - Liskov Substitution: Derived can replace base
**I** - Interface Segregation: Smaller, focused interfaces
**D** - Dependency Inversion: Depend on abstractions

## File Organization Philosophy

Each file has **ONE clear focus**:
- No mixing multiple topics in one file
- Focused content (150-300 lines typical)
- Clear learning progression
- Easy to find what you need
- Examples for each concept

## Total Structure

- **4 Categories** (main sections)
- **19 Subcategories** (focused topic areas)
- **30+ focused files** (one concept each)
- **30+ README guides** (navigation and learning paths)
- ~80,000+ words of focused content

## Quick Navigation

### By Topic

**Object Creation**
- [Classes and Objects](01-OOP-Fundamentals/01-Classes-Objects/00-Classes-Objects.md)
- [Instance Constructors](01-OOP-Fundamentals/02-Constructors-Destructors/01-Instance-Constructors/00-Instance-Constructors.md)

**Reusability**
- [Inheritance Basics](02-Inheritance-Polymorphism/01-Inheritance-Basics/00-Inheritance-Basics.md)
- [Static Classes](03-Advanced-OOP/06-Static-Classes/00-Static-Classes.md)

**Behavior**
- [Virtual and Override](02-Inheritance-Polymorphism/03-Virtual-Override/00-Virtual-Override.md)
- [Polymorphism Patterns](02-Inheritance-Polymorphism/05-Polymorphism-Patterns/00-Polymorphism-Patterns.md)

**Data Management**
- [Properties and Fields](01-OOP-Fundamentals/03-Properties-Fields/00-Properties-Fields.md)
- [Encapsulation](03-Advanced-OOP/03-Encapsulation/00-Encapsulation.md)

**Access Control**
- [Access Modifiers](03-Advanced-OOP/04-Access-Modifiers/00-Access-Modifiers.md)
- [Interfaces Basics](03-Advanced-OOP/01-Interfaces-Basics/00-Interfaces-Basics.md)

## How to Use This Content

1. **Choose your level**: Beginner, Intermediate, or Advanced path
2. **Follow the learning path**: Sequential content builds knowledge
3. **Read category READMEs**: Understand topic connections
4. **Focus on ONE file at a time**: Deep understanding over breadth
5. **Do the examples**: Copy and run code snippets
6. **Review related topics**: Use "Next Steps" links

## Study Tips

- **Don't skip files**: Each builds on previous concepts
- **Run the code**: Understanding comes from practice
- **Modify examples**: Change code and see what breaks
- **Apply to projects**: Use concepts in real code
- **Review regularly**: OOP concepts layer and build
- **Read multiple times**: Different things click each time

## Related Learning

After mastering OOP:
- Design Patterns
- SOLID Principles in depth
- Dependency Injection
- Architecture patterns (MVC, MVVM, etc.)
- Domain-Driven Design
- Microservices design

## Contributing & Feedback

This content is organized to maximize learning. Each file:
- Has a clear, single topic
- Includes practical examples
- Shows both good and bad patterns
- Links to related concepts
- Avoids mixing unrelated ideas

## Summary

Object-oriented programming is fundamental to modern C# development. This comprehensive guide organizes OOP concepts into focused, digestible pieces. Start with the fundamentals, progress through inheritance and polymorphism, explore advanced patterns, and master best practices.

**Total Learning Time**: 10-15 hours for thorough study
**Beginner Path**: 3-5 hours
**Practice Time**: 5-10 hours on your own projects

Happy learning!
