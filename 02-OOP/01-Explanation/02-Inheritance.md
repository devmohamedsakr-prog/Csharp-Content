# Inheritance

## Overview
Inheritance allows a **derived class** to inherit members from a **base class**.

---

## Base Classes and Derived Classes

```csharp
// Base class (parent)
public class Animal {
    public string Name { get; set; }
    
    public virtual void MakeSound() {
        Console.WriteLine("Generic animal sound");
    }
}

// Derived class (child) inherits from Animal
public class Dog : Animal {
    public override void MakeSound() {
        Console.WriteLine("Woof!");
    }
}

// Usage
Animal animal = new Animal();
animal.MakeSound();  // "Generic animal sound"

Dog dog = new Dog();
dog.MakeSound();  // "Woof!"
```

---

## The `virtual` and `override` Keywords

**virtual**: Base class allows override
**override**: Derived class provides new implementation

```csharp
public class Vehicle {
    // Virtual - can be overridden
    public virtual void Start() {
        Console.WriteLine("Vehicle starting");
    }
}

public class Car : Vehicle {
    // Override - replace implementation
    public override void Start() {
        Console.WriteLine("Car engine starting");
    }
}

public class Bicycle : Vehicle {
    // Override - different implementation
    public override void Start() {
        Console.WriteLine("Bicycle is ready");
    }
}

// Usage - polymorphism
Vehicle v1 = new Car();
v1.Start();  // "Car engine starting"

Vehicle v2 = new Bicycle();
v2.Start();  // "Bicycle is ready"
```

---

## Calling Base Class Methods

Use `base` keyword to access parent class implementation.

```csharp
public class Employee {
    public string Name { get; set; }
    
    public virtual void DisplayInfo() {
        Console.WriteLine($"Name: {Name}");
    }
}

public class Manager : Employee {
    public List<Employee> Team { get; set; }
    
    public override void DisplayInfo() {
        base.DisplayInfo();  // Call parent implementation
        Console.WriteLine($"Team Size: {Team.Count}");
    }
}

// Usage
Manager manager = new Manager { 
    Name = "Alice",
    Team = new List<Employee> { }
};
manager.DisplayInfo();
// Output:
// Name: Alice
// Team Size: 0
```

---

## Constructor Inheritance

Call base class constructor with `base()`.

```csharp
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Person(string name, int age) {
        Name = name;
        Age = age;
    }
}

public class Student : Person {
    public string StudentId { get; set; }
    
    // Pass parameters to base constructor
    public Student(string name, int age, string studentId) 
        : base(name, age) {
        StudentId = studentId;
    }
}

// Usage
Student student = new Student("Bob", 20, "STU001");
Console.WriteLine($"{student.Name}, {student.Age}, {student.StudentId}");
```

---

## Inheritance Hierarchy

Multiple levels of inheritance.

```csharp
// Level 1: Base
public class Animal {
    public virtual void Eat() {
        Console.WriteLine("Eating");
    }
}

// Level 2: Derived from Animal
public class Mammal : Animal {
    public virtual void Nurse() {
        Console.WriteLine("Nursing offspring");
    }
}

// Level 3: Derived from Mammal
public class Dog : Mammal {
    public override void Eat() {
        Console.WriteLine("Dog eating");
    }
    
    public override void Nurse() {
        Console.WriteLine("Dog nursing puppies");
    }
}

// Usage
Dog dog = new Dog();
dog.Eat();    // "Dog eating"
dog.Nurse();  // "Dog nursing puppies"
```

---

## Protected Members

Accessible in derived classes but not outside.

```csharp
public class BankAccount {
    // Private - only in this class
    private decimal balance;
    
    // Protected - in this class and derived classes
    protected string AccountNumber { get; set; }
    
    // Public - anywhere
    public string AccountHolder { get; set; }
    
    // Protected method
    protected void UpdateBalance(decimal amount) {
        balance += amount;
    }
}

public class SavingsAccount : BankAccount {
    public void AddInterest(decimal rate) {
        decimal interest = 100 * rate;  // Simplified
        UpdateBalance(interest);  // OK - protected method
        Console.WriteLine(AccountNumber);  // OK - protected property
    }
}

var account = new SavingsAccount();
account.AccountHolder = "Alice";  // OK - public
account.AddInterest(0.05);
// account.UpdateBalance(100);  // Error - protected, not accessible here
```

---

## Sealed Classes

Prevent inheritance.

```csharp
// Can inherit from this
public class Vehicle {
    public virtual void Start() { }
}

// Cannot inherit from sealed class
public sealed class Car : Vehicle {
    public override void Start() {
        Console.WriteLine("Car starting");
    }
}

// Error - cannot inherit from sealed class
// public class ElectricCar : Car { }
```

---

## Method Hiding vs Overriding

**Overriding**: Replace method (base keyword required)
**Hiding**: Create new method without `override`

```csharp
public class Parent {
    public virtual void Display() {
        Console.WriteLine("Parent");
    }
}

public class Child1 : Parent {
    // Overriding - replaces parent method
    public override void Display() {
        Console.WriteLine("Child1 - Overridden");
    }
}

public class Child2 : Parent {
    // Hiding - hides parent method (not recommended)
    public new void Display() {
        Console.WriteLine("Child2 - Hidden");
    }
}

// Usage
Parent p1 = new Child1();
p1.Display();  // "Child1 - Overridden"

Parent p2 = new Child2();
p2.Display();  // "Parent" - uses parent implementation!

Child2 child2 = new Child2();
child2.Display();  // "Child2 - Hidden"
```

---

## Single Inheritance Limitation

C# allows only single class inheritance (but multiple interface implementation).

```csharp
public class A { }
public class B { }

// Error - cannot inherit from two classes
// public class C : A, B { }

// Solution: use interfaces
public interface IA { }
public interface IB { }

public class C : A, IA, IB { }  // Class + multiple interfaces OK
```

---

## Inheritance vs Composition

**Inheritance (IS-A)**: "Dog IS-A Animal"
**Composition (HAS-A)**: "Car HAS-A Engine"

```csharp
// Inheritance - IS-A relationship
public class Dog : Animal {
    public void Bark() { }
}

// Composition - HAS-A relationship
public class Car {
    public Engine engine;  // Car HAS-A Engine
}

// When to use each:
// Inheritance: true hierarchical relationship, shared behavior
// Composition: more flexible, easier to change
```

---

## Best Practices

✓ Use inheritance for true IS-A relationships
```csharp
// Good - true inheritance
public class Manager : Employee { }

// Bad - forced inheritance
public class Logger : BaseClass { }  // Just for code reuse
```

✓ Prefer composition over inheritance
```csharp
// Better composition
public class Car {
    private Engine engine;
    private Transmission transmission;
}

// vs inheritance (less flexible)
public class Car : Engine, Transmission { }
```

✓ Don't create deep inheritance hierarchies
```csharp
// Limit depth
Animal → Mammal → Dog (3 levels OK)

// Avoid excessive depth
Animal → Mammal → Carnivore → Feline → Wildcat → Lion (too deep)
```

---

## Common Mistakes

❌ Forgetting `virtual` on base method
```csharp
public class Parent {
    public void Display() { }  // Not virtual
}

public class Child : Parent {
    public override void Display() { }  // Error!
}
```

✓ Mark as virtual
```csharp
public class Parent {
    public virtual void Display() { }
}

public class Child : Parent {
    public override void Display() { }  // OK
}
```

❌ Deep inheritance hierarchies
```csharp
// Hard to maintain and understand
A → B → C → D → E → F
```

✓ Keep it shallow
```csharp
// Easier to understand
A → B
A → C
```
