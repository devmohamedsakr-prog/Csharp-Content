# Classes and Objects

## Overview

A **class** is a blueprint that defines the structure and behavior of objects. An **object** is a specific instance of that class created with actual data.

## What is a Class?

A class is a template defining:
- Structure (what data it holds)
- Behavior (what it can do)

```csharp
public class Car
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
}
```

## What is an Object?

An object is a specific instance created from a class:

```csharp
Car car1 = new Car();  // Object 1
car1.Make = "Toyota";
car1.Model = "Camry";

Car car2 = new Car();  // Object 2  
car2.Make = "Honda";
car2.Model = "Civic";

// Each object has separate data
```

## Class vs Object

| Aspect | Class | Object |
|--------|-------|--------|
| What | Blueprint | Instance |
| Created | In code | At runtime with `new` |
| Count | One | Many from one class |
| Memory | Not allocated | Allocated per instance |
| Example | "Car" blueprint | Specific car I own |

## Creating Objects

```csharp
// Simple creation
Car car = new Car();

// With object initializer
Car car = new Car { Make = "Toyota", Year = 2023 };
```

## Object Identity

Each object is unique:

```csharp
Car car1 = new Car { Make = "Toyota" };
Car car2 = new Car { Make = "Toyota" };
Car car3 = car1;

Console.WriteLine(car1 == car2);  // false - different objects
Console.WriteLine(car1 == car3);  // true - same object
```

## Summary

- **Class** - Blueprint/template
- **Object** - Instance with data
- **new keyword** - Creates objects
- **Each object** - Independent copy of data
- **Identity** - Different objects are unique

## Next Steps

- Learn [Constructors-Destructors](../02-Constructors-Destructors/00-Constructors-Destructors.md) for object initialization
- Study [Properties-Fields](../03-Properties-Fields/00-Properties-Fields.md) for data management
- Review [Inheritance](../../02-Inheritance-Polymorphism/01-Inheritance/00-Inheritance.md) for reusing code
