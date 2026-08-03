# Calling Base Class Members

## Overview

The `base` keyword allows derived classes to access and call members from the parent class. Use it to access base implementation, call parent methods, or extend parent behavior without replacing it entirely.

## Base Keyword

Access parent class members using `base`:

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
        base.DisplayInfo();  // Call parent method
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

## Calling Parent Methods

Extend parent behavior without replacing it:

```csharp
public class Vehicle
{
    public string Make { get; set; }
    
    public virtual void Start()
    {
        Console.WriteLine("Vehicle starting");
    }
}

public class Car : Vehicle
{
    public override void Start()
    {
        base.Start();  // Call parent
        Console.WriteLine("Car engine warming up");
    }
}

// Usage
var car = new Car { Make = "Toyota" };
car.Start();
// Output:
// Vehicle starting
// Car engine warming up
```

## Extending Functionality

Preserve parent logic while adding new behavior:

```csharp
public class DataValidator
{
    public virtual bool Validate(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            Console.WriteLine("Data cannot be empty");
            return false;
        }
        return true;
    }
}

public class StrictValidator : DataValidator
{
    public override bool Validate(string data)
    {
        // Call parent validation first
        if (!base.Validate(data))
            return false;
        
        // Add additional strict validation
        if (data.Length < 5)
        {
            Console.WriteLine("Data must be at least 5 characters");
            return false;
        }
        
        return true;
    }
}

// Usage
var validator = new StrictValidator();
validator.Validate("ab");      // Fails parent check + strict check
validator.Validate("hello");   // Passes both
```

## Base Constructor Calls

Call parent constructor to initialize base members:

```csharp
public class Animal
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Animal(string name, int age)
    {
        Name = name;
        Age = age;
        Console.WriteLine("Animal initialized");
    }
}

public class Dog : Animal
{
    public string Breed { get; set; }
    
    // Call base constructor
    public Dog(string name, int age, string breed) 
        : base(name, age)  // Initialize parent first
    {
        Breed = breed;
        Console.WriteLine("Dog initialized");
    }
}

// Usage
var dog = new Dog("Buddy", 5, "Golden");
// Output:
// Animal initialized
// Dog initialized
```

## Base Property Access

Access parent properties:

```csharp
public class Shape
{
    public string Color { get; set; }
}

public class Circle : Shape
{
    public double Radius { get; set; }
    
    public void Describe()
    {
        // Access base property
        Console.WriteLine($"Circle with color {base.Color} and radius {Radius}");
    }
}

// Usage
var circle = new Circle { Color = "Red", Radius = 5 };
circle.Describe();  // Circle with color Red and radius 5
```

## Initialization Pattern

Common pattern for derived classes:

```csharp
public class DataRepository
{
    protected string _connectionString;
    
    protected DataRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public virtual void Connect()
    {
        Console.WriteLine($"Connecting to {_connectionString}");
    }
}

public class UserRepository : DataRepository
{
    public UserRepository(string connectionString) 
        : base(connectionString)
    {
    }
    
    public override void Connect()
    {
        base.Connect();  // Call parent
        Console.WriteLine("User repository ready");
    }
}

// Usage
var repo = new UserRepository("Server=localhost");
repo.Connect();
// Output:
// Connecting to Server=localhost
// User repository ready
```

## When to Use Base

Use `base` to:
- Call parent implementation (don't replace entirely)
- Extend functionality
- Initialize parent state
- Maintain parent behavior while adding new features

## When NOT to Use Base

- If you're completely replacing functionality - omit `base` call
- If parent method is not relevant to your override

```csharp
// Good - Extending
public override void Process()
{
    base.Process();  // Do parent work
    DoExtraWork();   // Add new work
}

// Also OK - Replacing completely
public override void Process()
{
    // No base call - complete replacement
    DoDifferentWork();
}
```

## Summary

- **base** - Access parent class members
- **Extend behavior** - Call parent, then add more
- **Constructor calls** - Initialize parent first
- **Maintain functionality** - Don't lose parent logic
- **Clean inheritance** - Proper use of base improves code

## Next Steps

- Learn [Virtual-Override](../03-Virtual-Override/00-Virtual-Override.md) for polymorphic behavior
- Study [Type-Casting](../04-Type-Casting/00-Type-Casting.md) for conversions
- Review [Polymorphism-Patterns](../05-Polymorphism-Patterns/00-Polymorphism-Patterns.md) for design patterns
