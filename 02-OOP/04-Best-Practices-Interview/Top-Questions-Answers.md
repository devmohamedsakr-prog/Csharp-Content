# OOP - Interview Questions & Answers

## 1. What are the four pillars of OOP?

**Answer:**

### Encapsulation
Bundling data and methods together, hiding internal details from outside access.
```csharp
class BankAccount {
    private decimal balance;  // Encapsulated
    
    public void Deposit(decimal amount) {
        if (amount > 0) balance += amount;
    }
}
```

### Abstraction
Hiding complex implementation details, showing only essential features.
```csharp
abstract class Vehicle {
    public abstract void Start();  // What to do, not how
}
```

### Inheritance
Child classes inherit properties and methods from parent classes.
```csharp
class Car : Vehicle {
    // Inherits Vehicle properties
}
```

### Polymorphism
Objects can take multiple forms or methods can behave differently based on context.
```csharp
public virtual void Drive() { }  // Base
public override void Drive() { }  // Child
```

---

## 2. What is the difference between a class and an object?

**Answer:**

**Class**: A blueprint or template defining structure and behavior
**Object**: An instance of a class with actual data

```csharp
// Class - blueprint
class Dog {
    public string Name { get; set; }
    public void Bark() { }
}

// Objects - instances
Dog myDog = new Dog { Name = "Buddy" };
Dog yourDog = new Dog { Name = "Max" };
```

Think: Class = Cookie cutter, Objects = Individual cookies

---

## 3. What are access modifiers and their visibility?

**Answer:**

| Modifier | Class | Assembly | Derived | Outside |
|----------|-------|----------|---------|---------|
| public | ✓ | ✓ | ✓ | ✓ |
| protected | ✓ | ✗ | ✓ | ✗ |
| internal | ✓ | ✓ | ✗ | ✗ |
| private | ✓ | ✗ | ✗ | ✗ |
| protected internal | ✓ | ✓ | ✓ | ✗ |

```csharp
class MyClass {
    public int x = 1;        // Accessible everywhere
    protected int y = 2;     // Accessible in derived classes
    internal int z = 3;      // Accessible in same assembly
    private int w = 4;       // Accessible only here
}
```

---

## 4. What is the difference between inheritance and composition?

**Answer:**

**Inheritance (IS-A)**: Child class inherits from parent
```csharp
class Animal { }
class Dog : Animal { }  // Dog IS-A Animal
```

**Composition (HAS-A)**: Class contains instance of another class
```csharp
class Engine { }
class Car {
    private Engine engine;  // Car HAS-A Engine
}
```

**When to Use**:
- **Inheritance**: True hierarchical relationship, shared behavior
- **Composition**: Flexible relationships, avoids brittle hierarchies

**Real Example**:
```csharp
// Bad inheritance
class Duck : Bird {
    public virtual void Fly() { }
}
class Penguin : Bird {
    // Penguin can't fly! Bad design
}

// Better composition
class Duck {
    private IFlyBehavior flyer;
}
class Penguin {
    private ISwimBehavior swimmer;
}
```

---

## 5. What is a virtual method and why use override?

**Answer:**

Virtual methods allow derived classes to provide their own implementation.

```csharp
class Animal {
    public virtual void MakeSound() {
        Console.WriteLine("Generic sound");
    }
}

class Dog : Animal {
    public override void MakeSound() {
        Console.WriteLine("Woof!");
    }
}

// Usage
Animal myDog = new Dog();
myDog.MakeSound();  // Outputs: "Woof!"
```

**Key Points**:
- Base class marks method as `virtual`
- Derived class uses `override`
- Enables polymorphism - method called based on actual type
- Without virtual, base class method is called

---

## 6. What is an abstract class and how does it differ from an interface?

**Answer:**

| Feature | Abstract Class | Interface |
|---------|----------------|-----------|
| Instantiation | No | No |
| Constructor | Yes | No |
| State | Can have fields | No (properties only) |
| Access Modifiers | Any | All public |
| Methods | Abstract + concrete | Abstract only |
| Inheritance | Single | Multiple |
| Purpose | Base behavior | Contract |

```csharp
// Abstract Class
abstract class Shape {
    protected string color;
    
    public Shape(string c) { color = c; }
    
    public abstract double GetArea();
    
    public void Display() { }  // Concrete method
}

// Interface
interface IShape {
    double GetArea();
    void Display();
}
```

**When to Use**:
- **Abstract Class**: Share common code and state
- **Interface**: Define contract without implementation

---

## 7. What is method overloading?

**Answer:**

Multiple methods with same name but different parameters.

```csharp
class Calculator {
    public int Add(int a, int b) {
        return a + b;
    }
    
    public double Add(double a, double b) {
        return a + b;
    }
    
    public int Add(int a, int b, int c) {
        return a + b + c;
    }
}

// Compiler determines which method to call
Calculator calc = new Calculator();
calc.Add(5, 10);           // First method
calc.Add(5.5, 10.5);       // Second method
calc.Add(5, 10, 15);       // Third method
```

---

## 8. What is method overriding?

**Answer:**

Child class provides specific implementation for method inherited from parent.

```csharp
class Animal {
    public virtual void Speak() {
        Console.WriteLine("Animal sound");
    }
}

class Cat : Animal {
    public override void Speak() {
        Console.WriteLine("Meow!");
    }
}

// Runtime polymorphism
Animal animal = new Cat();
animal.Speak();  // Outputs: "Meow!" (based on actual type)
```

---

## 9. What is a constructor and destructor?

**Answer:**

**Constructor**: Initializes object when created
```csharp
class Person {
    public string Name { get; set; }
    
    // Constructor
    public Person(string name) {
        Name = name;
        Console.WriteLine($"{name} created");
    }
}

Person p = new Person("John");  // Calls constructor
```

**Destructor**: Cleans up resources when object is destroyed (rarely used in C#)
```csharp
class Resource {
    ~Resource() {  // Destructor
        Console.WriteLine("Cleanup");
    }
}
```

**Note**: Use `IDisposable` and `using` statement instead of destructors.

---

## 10. What is a property and auto-property?

**Answer:**

**Property**: Controlled access to fields via getter/setter

```csharp
// Traditional property
class Person {
    private string name;
    
    public string Name {
        get { return name; }
        set { name = value; }
    }
}

// Auto-property (shorthand)
class Person {
    public string Name { get; set; }
}

// With initialization
class Person {
    public string Name { get; set; } = "Unknown";
}

// Read-only property
class Person {
    public string Id { get; } = Guid.NewGuid().ToString();
}
```

---

## 11. What is the difference between this and base keyword?

**Answer:**

**this**: Refers to current class instance
**base**: Refers to parent class

```csharp
class Animal {
    public virtual void Display() {
        Console.WriteLine("Animal");
    }
}

class Dog : Animal {
    public override void Display() {
        base.Display();        // Calls parent method
        Console.WriteLine("Dog");
        
        // this refers to current object
        var current = this;
    }
}
```

---

## 12. What is the difference between static and instance members?

**Answer:**

**Static**: Shared across all instances
**Instance**: Unique to each object

```csharp
class Counter {
    public static int Total = 0;     // Shared
    public int Id { get; set; }      // Per instance
    
    public Counter() {
        Total++;
    }
}

Counter c1 = new Counter();
Counter c2 = new Counter();

Console.WriteLine(Counter.Total);  // 2 (shared)
Console.WriteLine(c1.Id);         // Unique
```

---

## Quick Tips for Interview

✓ Know the four pillars of OOP
✓ Understand when to use inheritance vs composition
✓ Know difference between abstract class and interface
✓ Explain virtual methods and polymorphism
✓ Be ready to code examples of overloading vs overriding
✓ Understand static vs instance members
✓ Know purpose of constructors and destructors
