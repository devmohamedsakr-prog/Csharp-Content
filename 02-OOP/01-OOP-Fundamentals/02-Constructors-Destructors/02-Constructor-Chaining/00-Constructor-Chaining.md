# Constructor Chaining

## Overview

Constructor chaining allows constructors to call other constructors to avoid code duplication. Use `this` to call another constructor in the same class, or `base` to call a parent constructor.

## Chaining with `this`

Call another constructor in the same class:

```csharp
public class User
{
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    
    // Constructor 1: Minimal
    public User() : this("Unknown", "unknown@example.com")
    {
    }
    
    // Constructor 2: Username only
    public User(string username) : this(username, $"{username}@example.com")
    {
    }
    
    // Constructor 3: Full constructor (does the work)
    public User(string username, string email)
    {
        Username = username;
        Email = email;
        IsActive = true;
    }
}

// Usage - each delegates to the full constructor
var user1 = new User();                           // Default: "Unknown"
var user2 = new User("alice");                    // alice@example.com
var user3 = new User("bob", "bob@custom.com");    // Custom email
```

## Benefits of Constructor Chaining

### Avoid Code Duplication

```csharp
// Bad - Duplicate initialization
public class SettingsBad
{
    public string Host { get; set; }
    public int Port { get; set; }
    
    public SettingsBad()
    {
        Host = "localhost";
        Port = 5432;
    }
    
    public SettingsBad(string host)
    {
        Host = host;
        Port = 5432;  // Duplicated
    }
    
    public SettingsBad(string host, int port)
    {
        Host = host;
        Port = port;
    }
}

// Good - Chain to single source
public class SettingsGood
{
    public string Host { get; set; }
    public int Port { get; set; }
    
    public SettingsGood() : this("localhost", 5432) { }
    public SettingsGood(string host) : this(host, 5432) { }
    public SettingsGood(string host, int port)
    {
        Host = host;
        Port = port;
    }
}
```

## Base Constructor Chaining with `base`

Call parent class constructor:

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
    
    // Call base (parent) constructor first
    public Dog(string name, string breed) : base(name)
    {
        Breed = breed;
        Console.WriteLine($"Dog created: {name}, {breed}");
    }
}

// Usage
var dog = new Dog("Buddy", "Golden Retriever");
// Output:
// Animal created: Buddy
// Dog created: Buddy, Golden Retriever
```

## Initialization Order with `base`

Base constructor always runs first:

```csharp
public class Employee
{
    public string Name { get; set; }
    
    protected Employee(string name)
    {
        Name = name;
        Console.WriteLine("1. Employee initialized");
    }
}

public class Manager : Employee
{
    public List<Employee> DirectReports { get; set; }
    
    public Manager(string name) : base(name)
    {
        DirectReports = new List<Employee>();
        Console.WriteLine("2. Manager initialized");
    }
}

// Usage
var manager = new Manager("Alice");
// Output:
// 1. Employee initialized
// 2. Manager initialized
```

## Complex Chaining Patterns

Multiple levels of inheritance:

```csharp
public class Vehicle
{
    public string Make { get; set; }
    
    public Vehicle(string make)
    {
        Make = make;
    }
}

public class Car : Vehicle
{
    public int Doors { get; set; }
    
    public Car(string make) : base(make)
    {
        Doors = 4;
    }
    
    public Car(string make, int doors) : base(make)
    {
        Doors = doors;
    }
}

public class SportsCar : Car
{
    public int TopSpeed { get; set; }
    
    public SportsCar(string make) : base(make)
    {
        TopSpeed = 200;
    }
    
    public SportsCar(string make, int topSpeed) : base(make, 2)
    {
        TopSpeed = topSpeed;
    }
}

// Usage
var sports = new SportsCar("Ferrari", 240);
// Base constructors called in order: Vehicle → Car → SportsCar
```

## When to Chain

Chain constructors when:
- Multiple constructors perform similar initialization
- You want to avoid duplication
- You need flexibility in construction options

```csharp
// Good - Multiple ways to construct
public class Logger
{
    public string Name { get; set; }
    public string FilePath { get; set; }
    
    public Logger() : this("App", "./logs.txt") { }
    public Logger(string name) : this(name, "./logs.txt") { }
    public Logger(string name, string filePath)
    {
        Name = name;
        FilePath = filePath;
    }
}
```

## Summary

- **this** - Chain to another constructor in same class
- **base** - Chain to parent constructor
- **Avoid duplication** - Single source of truth
- **Order** - Base constructor runs first
- **Flexibility** - Multiple construction options
- **Clarity** - One primary constructor does real work

## Next Steps

- Learn [Static-Constructors](../03-Static-Constructors/00-Static-Constructors.md) for class initialization
- Review [Inheritance](../../02-Inheritance-Polymorphism/01-Inheritance/00-Inheritance.md) for inheritance concepts
- Study [Destructors-IDisposable](../05-Destructors-IDisposable/00-Destructors-IDisposable.md) for cleanup
