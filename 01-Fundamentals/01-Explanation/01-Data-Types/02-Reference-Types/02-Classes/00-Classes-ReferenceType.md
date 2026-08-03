# Classes: User-Defined Reference Types

## Overview

A `class` is a reference type that represents objects with data (fields/properties) and behavior (methods). Classes are stored on the heap.

### Characteristics
```csharp
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
}

// Reference type: stored on heap
// Mutable: can change after creation
// Can inherit from other classes
// Default value: null
// Requires garbage collection
```

## Class Structure

### Basic Class Definition

```csharp
public class Person {
    // Fields (old style)
    private string _name;
    private int _age;
    
    // Properties (recommended)
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor
    public Person(string name, int age) {
        Name = name;
        Age = age;
    }
    
    // Methods
    public void PrintInfo() {
        Console.WriteLine($"{Name}, age {Age}");
    }
    
    public void HaveBirthday() {
        Age++;
    }
}
```

### Class Members

#### Fields (Data Storage)
```csharp
public class Product {
    // Public field (not recommended)
    public string name;
    
    // Private field (recommended)
    private decimal _price;
    
    // Readonly field (cannot change after initialization)
    private readonly int _productId;
    
    public Product(int id) {
        _productId = id;
    }
}
```

#### Properties (Controlled Access)
```csharp
public class Person {
    // Auto-property (simple)
    public string Name { get; set; }
    
    // Property with backing field
    private int _age;
    public int Age {
        get { return _age; }
        set { _age = value < 0 ? 0 : value; }  // Validation
    }
    
    // Read-only property
    public int BirthYear { get; private set; }
    
    // Expression-bodied property
    public string Summary => $"{Name}, {Age} years old";
    
    // Init-only property (C# 9+)
    public string Email { get; init; }
}
```

#### Methods (Behavior)
```csharp
public class Calculator {
    // Basic method
    public int Add(int a, int b) {
        return a + b;
    }
    
    // Method with no return
    public void PrintResult(int result) {
        Console.WriteLine($"Result: {result}");
    }
    
    // Method with optional parameters
    public int Multiply(int a, int b = 1) {
        return a * b;
    }
    
    // Method with params
    public int Sum(params int[] numbers) {
        int total = 0;
        foreach (int n in numbers) {
            total += n;
        }
        return total;
    }
}
```

## Constructors

### Purpose
Initialize object data when creating instance.

### Types

#### Default Constructor
```csharp
public class Person {
    public string Name { get; set; }
    
    // If not defined, compiler provides parameterless constructor
    public Person() {
        // Initialize defaults
    }
}

Person p = new Person();
```

#### Parameterized Constructor
```csharp
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Person(string name, int age) {
        Name = name;
        Age = age;
    }
}

Person p = new Person("Alice", 30);
```

#### Constructor Chaining
```csharp
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    
    // Call another constructor
    public Person() : this("Unknown", 0) { }
    
    public Person(string name, int age) 
        : this(name, age, "") { }
    
    public Person(string name, int age, string email) {
        Name = name;
        Age = age;
        Email = email;
    }
}
```

#### Static Constructor
```csharp
public class Config {
    public static string ConnectionString { get; private set; }
    
    // Runs once when class first used
    static Config() {
        ConnectionString = Environment.GetEnvironmentVariable("CONN_STR");
    }
}
```

## Object Creation and References

### Creating Objects

```csharp
// Create new instance on heap
Person p1 = new Person("Alice", 30);
Person p2 = new Person("Bob", 25);

// Reference assignment (same object)
Person p3 = p1;  // p3 points to same object as p1

// Modification via different reference
p3.Age = 31;
Console.WriteLine(p1.Age);  // 31 (changed!)
```

### Reference Behavior

```csharp
public class Box {
    public int Value { get; set; }
}

Box b1 = new Box { Value = 10 };
Box b2 = new Box { Value = 10 };
Box b3 = b1;

// Reference equality
Console.WriteLine(b1 == b2);  // false (different objects)
Console.WriteLine(b1 == b3);  // true (same object)
Console.WriteLine(ReferenceEquals(b1, b2));  // false
Console.WriteLine(ReferenceEquals(b1, b3));  // true
```

## Inheritance

### Basic Inheritance

```csharp
// Base class
public class Animal {
    public string Name { get; set; }
    
    public virtual void MakeSound() {
        Console.WriteLine("Some sound");
    }
}

// Derived class
public class Dog : Animal {
    public override void MakeSound() {
        Console.WriteLine("Woof!");
    }
}

// Usage
Animal a = new Dog();
a.MakeSound();  // "Woof!" (virtual method call)
```

### Virtual and Override

```csharp
public class Vehicle {
    // Virtual - can be overridden
    public virtual void Start() {
        Console.WriteLine("Vehicle starting");
    }
}

public class Car : Vehicle {
    // Override - provide new implementation
    public override void Start() {
        Console.WriteLine("Car engine starting");
    }
}

// Polymorphism
Vehicle v = new Car();
v.Start();  // "Car engine starting"
```

### Base Class Access

```csharp
public class Person {
    public virtual string GetInfo() {
        return $"Person";
    }
}

public class Employee : Person {
    public string Department { get; set; }
    
    public override string GetInfo() {
        // Call base implementation
        string baseInfo = base.GetInfo();
        return $"{baseInfo}, Department: {Department}";
    }
}
```

## Access Modifiers

### Visibility Levels

| Modifier | Visible In | Use Case |
|----------|-----------|----------|
| `public` | Everywhere | API surface |
| `protected` | Derived classes | Inheritance |
| `internal` | Same assembly | Internal implementation |
| `private` | Same class | Implementation details |
| `protected internal` | Derived + same assembly | Inheritance + internal |

### Examples

```csharp
public class BankAccount {
    // Public - anyone can access
    public string AccountNumber { get; set; }
    
    // Private - only this class
    private decimal _balance;
    
    // Protected - derived classes can access
    protected virtual void UpdateBalance(decimal amount) {
        _balance += amount;
    }
    
    // Internal - within assembly
    internal void AuditLog() {
        // Log transaction
    }
}
```

## Static Members

### Static Fields and Properties

```csharp
public class Counter {
    // Shared across all instances
    private static int _totalCount = 0;
    public static int TotalCount => _totalCount;
    
    private int _instanceValue;
    
    public void Increment() {
        _instanceValue++;
        _totalCount++;  // Increment shared counter
    }
}

Counter c1 = new Counter();
Counter c2 = new Counter();

c1.Increment();
c1.Increment();
c2.Increment();

Console.WriteLine(Counter.TotalCount);  // 3
```

### Static Methods

```csharp
public class Math {
    // Called on class, not instance
    public static int Add(int a, int b) {
        return a + b;
    }
}

int result = Math.Add(5, 3);  // Call static method directly
```

## Null and Type Checking

### Null Check

```csharp
Person p = null;

// Explicit null check
if (p != null) {
    Console.WriteLine(p.Name);
}

// Null-conditional operator
Console.WriteLine(p?.Name);  // Null if p is null, otherwise p.Name

// Null coalescing
string display = p?.Name ?? "Unknown";
```

### Type Checking

```csharp
object obj = new Person { Name = "Alice" };

// Is operator
if (obj is Person) {
    Person p = (Person)obj;
}

// Pattern matching (modern)
if (obj is Person person) {
    Console.WriteLine(person.Name);
}

// As operator
Person p2 = obj as Person;
if (p2 != null) {
    Console.WriteLine(p2.Name);
}
```

## Common Class Patterns

### Immutable Class

```csharp
public class Point {
    // Read-only properties
    public int X { get; }
    public int Y { get; }
    
    // Set via constructor
    public Point(int x, int y) {
        X = x;
        Y = y;
    }
    
    // Cannot modify after creation
    // Point p = new Point(10, 20);
    // p.X = 15;  // Compiler error
}
```

### Singleton Pattern

```csharp
public class Logger {
    // Static instance
    private static Logger _instance;
    
    // Private constructor
    private Logger() { }
    
    // Global access point
    public static Logger Instance {
        get {
            _instance ??= new Logger();
            return _instance;
        }
    }
    
    public void Log(string message) {
        Console.WriteLine(message);
    }
}

// Usage
Logger.Instance.Log("Error occurred");
```

### Builder Pattern

```csharp
public class PersonBuilder {
    private string _name = "";
    private int _age = 0;
    private string _email = "";
    
    public PersonBuilder WithName(string name) {
        _name = name;
        return this;
    }
    
    public PersonBuilder WithAge(int age) {
        _age = age;
        return this;
    }
    
    public PersonBuilder WithEmail(string email) {
        _email = email;
        return this;
    }
    
    public Person Build() {
        return new Person(_name, _age, _email);
    }
}

// Fluent interface
Person p = new PersonBuilder()
    .WithName("Alice")
    .WithAge(30)
    .WithEmail("alice@example.com")
    .Build();
```

## Equality and Identity

### Object.Equals

```csharp
public class Person {
    public string Name { get; set; }
    
    // Override Equals for value comparison
    public override bool Equals(object obj) {
        if (obj is Person person) {
            return Name == person.Name;
        }
        return false;
    }
    
    // Override GetHashCode when overriding Equals
    public override int GetHashCode() {
        return Name.GetHashCode();
    }
}

Person p1 = new Person { Name = "Alice" };
Person p2 = new Person { Name = "Alice" };

Console.WriteLine(p1 == p2);  // false (reference equality)
Console.WriteLine(p1.Equals(p2));  // true (value equality)
```

## Memory and Garbage Collection

### Reference Semantics

```csharp
public class Resource {
    public string Data { get; set; }
}

// Object created on heap
Resource r1 = new Resource { Data = "Info" };

// References the same object
Resource r2 = r1;

// Set to null
r1 = null;
r2.Data;  // Still accessible!

// When all references are null/out of scope
// Object eligible for garbage collection
```

## Common Class Mistakes

❌ **Mutable class accessed from multiple threads**
```csharp
public class UnsafeCounter {
    public int Count { get; set; }  // Race condition possible
}
```

✓ **Use synchronization for thread safety**
```csharp
public class SafeCounter {
    private int _count;
    private readonly object _lock = new();
    
    public int Count {
        get {
            lock (_lock) { return _count; }
        }
    }
}
```

❌ **Leaking internal collections**
```csharp
public class Team {
    public List<string> Members { get; set; }  // Can be modified externally!
}

Team t = new Team { Members = new List<string> { "Alice" } };
t.Members.Add("Bob");  // Unexpected modification
```

✓ **Return copies or read-only collections**
```csharp
public class Team {
    private readonly List<string> _members = new();
    
    public IReadOnlyList<string> Members => _members.AsReadOnly();
}
```

## Summary

**Class Characteristics**:
- Reference type (stored on heap)
- Mutable (can change after creation)
- Can inherit from other classes
- Default value is `null`
- Requires garbage collection

**Best Practices**:
- Use properties instead of public fields
- Implement virtual methods for inheritance
- Override `Equals()` and `GetHashCode()` appropriately
- Use access modifiers to encapsulate implementation
- Consider immutability for thread safety

---

**Key Takeaway**: Classes are the foundation of object-oriented programming. Use them for complex objects that need mutable state and inheritance, keep implementation details private, and use properties for controlled access to data.
