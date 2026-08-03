# Inheritance - Basics

## Overview

Inheritance allows a derived class (child) to inherit members from a base class (parent), enabling code reuse and creating class hierarchies. A derived class automatically gets all public and protected members from its base.

## Single Inheritance Model

C# supports single inheritance - one direct base class:

```csharp
// Base class (parent)
public class Animal
{
    public string Name { get; set; }
    
    public void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }
}

// Derived class (child)
public class Dog : Animal
{
    public string Breed { get; set; }
}

// Usage
var dog = new Dog();
dog.Name = "Buddy";           // From Animal
dog.Breed = "Golden";         // From Dog
dog.Eat();                    // From Animal
```

## What Gets Inherited?

Accessibility affects what is inherited:

| Member | Public | Protected | Private |
|--------|--------|-----------|---------|
| Inherited | Yes | Yes | No |
| Accessible | Yes | Derived only | No |

```csharp
public class Base
{
    public string PublicMember { get; set; }      // Inherited, accessible
    protected string ProtectedMember { get; set; } // Inherited, derived only
    private string PrivateMember { get; set; }    // NOT inherited
}

public class Derived : Base
{
    public void Test()
    {
        PublicMember = "ok";        // OK - public
        ProtectedMember = "ok";     // OK - protected in derived
        // PrivateMember = "ok";    // ERROR - private not inherited
    }
}
```

## Class Hierarchies

Multiple levels of inheritance:

```csharp
// Level 1: Base
public class Animal
{
    public void Eat()
    {
        Console.WriteLine("Eating");
    }
}

// Level 2: Derived from Animal
public class Mammal : Animal
{
    public void Nurse()
    {
        Console.WriteLine("Nursing offspring");
    }
}

// Level 3: Derived from Mammal
public class Dog : Mammal
{
    public void Bark()
    {
        Console.WriteLine("Woof!");
    }
}

// Usage
var dog = new Dog();
dog.Eat();      // From Animal
dog.Nurse();    // From Mammal
dog.Bark();     // From Dog
```

## Constructor Inheritance

Derived class must call base constructor:

```csharp
public class Animal
{
    public string Name { get; set; }
    
    public Animal(string name)
    {
        Name = name;
        Console.WriteLine($"Animal created: {name}");
    }
}

public class Dog : Animal
{
    public string Breed { get; set; }
    
    // Call base constructor with : base(...)
    public Dog(string name, string breed) : base(name)
    {
        Breed = breed;
        Console.WriteLine($"Dog created: {breed}");
    }
}

// Usage
var dog = new Dog("Buddy", "Golden");
// Output:
// Animal created: Buddy
// Dog created: Golden
```

## IS-A Relationship

Inheritance models "IS-A" relationships:

```csharp
// Good - Dog IS-A Animal
public class Dog : Animal { }

// Bad - Dog HAS-A Tail (wrong inheritance)
public class BadDog : Tail { }  // Wrong!

// Correct way - composition
public class GoodDog
{
    public Tail Tail { get; set; }  // HAS-A
}
```

## Sealed Classes

Prevent a class from being inherited:

```csharp
// Cannot be inherited
public sealed class FinalClass
{
    public void Method()
    {
        Console.WriteLine("Final class");
    }
}

// ERROR: Cannot derive from sealed class
// public class DerivedClass : FinalClass { }
```

## Single Inheritance Limitation

C# only allows one base class (multiple inheritance not supported):

```csharp
// Good - Single inheritance
public class Dog : Animal { }

// NOT allowed - Multiple inheritance
// public class Dog : Animal, Pet { }  // ERROR

// Alternative - Use interfaces
public class Dog : Animal, IPet { }  // OK - can implement multiple interfaces
```

## Best Practices

### Use Inheritance for True IS-A Relationships

```csharp
// Good
public class Manager : Employee { }  // Manager IS-A Employee

// Bad - Using inheritance for "HAS-A"
public class PersonBad : Address { }  // Wrong - Person HAS-A Address

// Good - Use composition
public class PersonGood
{
    public Address Address { get; set; }  // HAS-A
}
```

### Prefer Composition Over Inheritance

```csharp
// When uncertain, composition is often safer
public class Car
{
    public Engine Engine { get; set; }  // HAS-A engine
    public Transmission Transmission { get; set; }  // HAS-A transmission
}
```

## Summary

- **Inheritance** - Derived class inherits from base class
- **IS-A relationship** - Models "is a" concept
- **Public/Protected** - Inherited and accessible
- **Private** - NOT inherited
- **Single inheritance** - Only one base class in C#
- **Constructor chaining** - Call base constructor
- **Sealed** - Prevent inheritance
- **Composition** - Alternative to inheritance

## Next Steps

- Learn [Base-Class-Members](../02-Base-Class-Members/00-Base-Class-Members.md) for calling parent code
- Study [Virtual-Override](../03-Virtual-Override/00-Virtual-Override.md) for polymorphic behavior
- Review [Type-Casting](../04-Type-Casting/00-Type-Casting.md) for conversions
