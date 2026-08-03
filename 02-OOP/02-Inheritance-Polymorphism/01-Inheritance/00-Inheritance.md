# Inheritance - Code Reuse and Hierarchy

## Overview

Inheritance allows a derived class (child) to inherit members from a base class (parent), enabling code reuse and creating class hierarchies.

## Base and Derived Classes

```csharp
// Base class (parent)
public class Animal
{
    public string Name { get; set; }
    
    public void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }
    
    public virtual void MakeSound()
    {
        Console.WriteLine("Generic animal sound");
    }
}

// Derived class (child)
public class Dog : Animal
{
    public string Breed { get; set; }
    
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says: Woof!");
    }
}

// Usage
Animal animal = new Animal();
animal.Name = "Unknown";
animal.Eat();          // Unknown is eating
animal.MakeSound();    // Generic animal sound

Dog dog = new Dog();
dog.Name = "Buddy";
dog.Breed = "Golden";
dog.Eat();             // Buddy is eating
dog.MakeSound();       // Buddy says: Woof!
```

## Virtual and Override

### Virtual Keyword

Allows base class method to be overridden:

```csharp
public class Vehicle
{
    // Virtual - can be overridden in derived classes
    public virtual void Start()
    {
        Console.WriteLine("Vehicle starting");
    }
    
    public virtual void Stop()
    {
        Console.WriteLine("Vehicle stopping");
    }
}
```

### Override Keyword

Derived class provides new implementation:

```csharp
public class Car : Vehicle
{
    // Override - replaces base implementation
    public override void Start()
    {
        Console.WriteLine("Car engine starting");
    }
    
    public override void Stop()
    {
        Console.WriteLine("Car engine stopping");
    }
}

public class Bicycle : Vehicle
{
    public override void Start()
    {
        Console.WriteLine("Bicycle is ready to go");
    }
    
    public override void Stop()
    {
        Console.WriteLine("Brakes applied");
    }
}

// Usage - polymorphism in action
Vehicle vehicle1 = new Car();
vehicle1.Start();   // Car engine starting
vehicle1.Stop();    // Car engine stopping

Vehicle vehicle2 = new Bicycle();
vehicle2.Start();   // Bicycle is ready to go
vehicle2.Stop();    // Brakes applied
```

## Calling Base Class Methods

Use `base` keyword to access parent implementation:

```csharp
public class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }
    
    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Employee: {Name}, Salary: ${Salary}");
    }
}

public class Manager : Employee
{
    public List<Employee> DirectReports { get; set; }
    
    public override void DisplayInfo()
    {
        base.DisplayInfo();  // Call parent implementation
        Console.WriteLine($"Direct Reports: {DirectReports.Count}");
    }
}

// Usage
var manager = new Manager
{
    Name = "Alice",
    Salary = 100000,
    DirectReports = new List<Employee> { }
};
manager.DisplayInfo();
// Output:
// Employee: Alice, Salary: $100000
// Direct Reports: 0
```

## Constructor Inheritance

Derived class must call base constructor:

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
        Console.WriteLine("Person created");
    }
}

public class Student : Person
{
    public string StudentId { get; set; }
    
    // Call base constructor first
    public Student(string name, int age, string studentId) 
        : base(name, age)
    {
        StudentId = studentId;
        Console.WriteLine("Student created");
    }
}

// Usage
var student = new Student("Bob", 20, "STU001");
// Output:
// Person created
// Student created
Console.WriteLine($"{student.Name}, Age {student.Age}, ID: {student.StudentId}");
```

## Inheritance Hierarchy

Multiple levels of inheritance:

```csharp
// Level 1: Base class
public class Animal
{
    public virtual void Eat()
    {
        Console.WriteLine("Eating");
    }
}

// Level 2: Derived from Animal
public class Mammal : Animal
{
    public virtual void Nurse()
    {
        Console.WriteLine("Nursing offspring");
    }
}

// Level 3: Derived from Mammal
public class Dog : Mammal
{
    public override void Eat()
    {
        Console.WriteLine("Dog eating kibble");
    }
    
    public override void Nurse()
    {
        Console.WriteLine("Dog nursing puppies");
    }
}

// Usage
Animal animal = new Dog();
animal.Eat();    // Dog eating kibble

Mammal mammal = new Dog();
mammal.Eat();    // Dog eating kibble
mammal.Nurse();  // Dog nursing puppies
```

## Type Casting and Polymorphism

### Upcasting (Safe)

Derived to base:

```csharp
Dog dog = new Dog();
Animal animal = dog;  // Upcasting - always safe
```

### Downcasting (Check Required)

Base to derived:

```csharp
Animal animal = new Dog();

// Option 1: Direct cast (risky)
Dog dog = (Dog)animal;  // Works if animal is actually a Dog

// Option 2: Safe cast with as
Dog dog2 = animal as Dog;
if (dog2 != null)
{
    dog2.MakeSound();
}

// Option 3: Type check with is
if (animal is Dog)
{
    Dog dog3 = (Dog)animal;
    dog3.MakeSound();
}

// Option 4: Pattern matching (C# 7+)
if (animal is Dog dog4)
{
    dog4.MakeSound();
}
```

## Method Hiding vs Overriding

### Override - Replaces Implementation

```csharp
public class BaseClass
{
    public virtual void Method()
    {
        Console.WriteLine("Base version");
    }
}

public class DerivedClass : BaseClass
{
    public override void Method()
    {
        Console.WriteLine("Derived version");
    }
}

// Usage
BaseClass obj = new DerivedClass();
obj.Method();  // "Derived version" - polymorphic
```

### Hiding - Shadows Base Implementation

```csharp
public class BaseClass
{
    public void Method()
    {
        Console.WriteLine("Base version");
    }
}

public class DerivedClass : BaseClass
{
    // new hides (shadows) base method - not polymorphic
    public new void Method()
    {
        Console.WriteLine("Derived version");
    }
}

// Usage
BaseClass obj = new DerivedClass();
obj.Method();  // "Base version" - not polymorphic!

DerivedClass derived = new DerivedClass();
derived.Method();  // "Derived version"
```

## Sealed Keyword

Prevent class from being inherited:

```csharp
// Cannot be inherited
public sealed class FinalClass
{
    public virtual void Method()
    {
        Console.WriteLine("Can override");
    }
}

// ERROR: Cannot derive from sealed class
// public class DerivedClass : FinalClass { }

// Seal a method to prevent further overriding
public class BaseClass
{
    public virtual void Method()
    {
        Console.WriteLine("Base");
    }
}

public class DerivedClass : BaseClass
{
    // Seal this method - no further overriding allowed
    public sealed override void Method()
    {
        Console.WriteLine("Sealed");
    }
}

// ERROR: Cannot override sealed method
// public class MoreDerived : DerivedClass
// {
//     public override void Method() { }
// }
```

## Best Practices

### 1. Use Inheritance for IS-A Relationships

```csharp
// Good - Dog IS-A Animal
public class Dog : Animal { }

// Bad - Dog HAS-A Tail (not inheritance)
public class DogBad
{
    public Tail Tail { get; set; }  // Composition instead
}
```

### 2. Favor Composition Over Inheritance

```csharp
// Good - Composition
public class Employee
{
    public Address Address { get; set; }  // HAS-A
}

// Less good - Inheritance
public class EmployeeInherit : Address  // Doesn't make sense
{
}
```

### 3. Don't Override Unless Necessary

```csharp
// Good - Inherit behavior as-is
public class Dog : Animal
{
    // Inherit Eat() from Animal
}

// Less good - Unnecessary override
public class DogBad : Animal
{
    public override void Eat()
    {
        base.Eat();  // Just calls base - unnecessary
    }
}
```

## Common Mistakes

### Mistake 1: Confusing Override with Hiding

```csharp
// Bad - Not overriding
public class Bad : BaseClass
{
    public void Method()  // Hides, doesn't override
    {
    }
}

// Good - Explicitly override
public class Good : BaseClass
{
    public override void Method()
    {
    }
}
```

### Mistake 2: Inheritance for Code Reuse Only

```csharp
// Bad - Utility class as base
public class MathBase
{
    public static int Add(int a, int b) => a + b;
}

public class MyClass : MathBase  // Wrong reason to inherit
{
}

// Good - Static utility class
public static class Math
{
    public static int Add(int a, int b) => a + b;
}
```

## Summary

- **Inheritance** - Derived class inherits from base
- **Virtual/Override** - Enable polymorphic behavior
- **base** - Access parent implementation
- **Upcasting** - Safe, always works
- **Downcasting** - Requires type checking
- **Sealed** - Prevent inheritance
- **Composition** - Often better than inheritance
- **IS-A** - Inheritance relationship test

## Next Steps

- Learn [Polymorphism](../02-Polymorphism/00-Polymorphism.md) for dynamic behavior
- Study [Virtual-Methods](../03-Virtual-Methods/00-Virtual-Methods.md) for runtime behavior
- Review [Interfaces](../../03-Advanced-OOP/01-Interfaces/00-Interfaces.md) as alternative to inheritance
