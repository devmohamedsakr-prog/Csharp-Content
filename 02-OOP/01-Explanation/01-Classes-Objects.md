# Classes and Objects

## Overview
A **class** is a blueprint for creating objects. An **object** is an instance of a class with actual data.

---

## What is a Class?

A class defines:
- **Properties** (data/state)
- **Methods** (behavior/actions)
- **Constructors** (initialization)

```csharp
// Blueprint
public class Car {
    // Properties (state)
    public string Color { get; set; }
    public string Model { get; set; }
    public int Speed { get; set; }
    
    // Method (behavior)
    public void Accelerate() {
        Speed += 10;
    }
    
    public void Brake() {
        Speed -= 10;
    }
}
```

---

## Creating Objects

Create instances from a class using the `new` keyword.

```csharp
// Create objects from Car class
Car myCar = new Car();
myCar.Color = "Red";
myCar.Model = "Tesla";

Car yourCar = new Car();
yourCar.Color = "Blue";
yourCar.Model = "BMW";

// Each object has separate data
Console.WriteLine(myCar.Color);  // Red
Console.WriteLine(yourCar.Color); // Blue
```

---

## Class Members

### Fields
Store data directly (not recommended, use properties instead).

```csharp
public class Person {
    // Field (avoid this pattern)
    public string name;  // Direct access to data
    
    // Better: use property
    public string Name { get; set; }
}
```

### Properties
Controlled access to data with getters/setters.

```csharp
public class Person {
    // Auto-property (modern, recommended)
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Property with backing field (traditional)
    private string _email;
    public string Email {
        get { return _email; }
        set { _email = value; }
    }
    
    // Read-only property
    public string Id { get; } = Guid.NewGuid().ToString();
    
    // Property with validation
    private int _age;
    public int Age {
        get { return _age; }
        set {
            if (value >= 0 && value <= 150) {
                _age = value;
            }
        }
    }
}

// Usage
Person person = new Person();
person.Name = "Alice";  // Uses setter
person.Age = 30;
Console.WriteLine(person.Name);  // Uses getter
```

### Methods
Functions that perform actions.

```csharp
public class Calculator {
    // Method with parameters and return
    public int Add(int a, int b) {
        return a + b;
    }
    
    // Method with no return
    public void PrintSum(int a, int b) {
        Console.WriteLine(a + b);
    }
    
    // Method with no parameters
    public int GetRandomNumber() {
        return new Random().Next(1, 101);
    }
}

// Usage
Calculator calc = new Calculator();
int result = calc.Add(5, 3);  // 8
calc.PrintSum(5, 3);  // Prints 8
```

---

## Constructors

Special method that runs when object is created.

### Default Constructor
Automatically created if you don't define one.

```csharp
public class Car {
    // Default constructor (implicit)
}

Car car = new Car();  // Calls default constructor
```

### Custom Constructor
Explicitly defined constructor.

```csharp
public class Car {
    public string Make { get; set; }
    public string Model { get; set; }
    
    // Constructor with parameters
    public Car(string make, string model) {
        Make = make;
        Model = model;
    }
}

// Usage
Car car = new Car("Toyota", "Camry");
Console.WriteLine($"{car.Make} {car.Model}");  // Toyota Camry
```

### Multiple Constructors
Use constructor overloading.

```csharp
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor 1: no parameters
    public Person() {
        Name = "Unknown";
        Age = 0;
    }
    
    // Constructor 2: name only
    public Person(string name) {
        Name = name;
        Age = 0;
    }
    
    // Constructor 3: both parameters
    public Person(string name, int age) {
        Name = name;
        Age = age;
    }
}

// Usage - different constructors
Person p1 = new Person();
Person p2 = new Person("Alice");
Person p3 = new Person("Bob", 30);
```

### Constructor Chaining
Call another constructor using `this`.

```csharp
public class Car {
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    
    public Car() : this("Unknown", "Unknown", 0) { }
    
    public Car(string make, string model) 
        : this(make, model, DateTime.Now.Year) { }
    
    public Car(string make, string model, int year) {
        Make = make;
        Model = model;
        Year = year;
    }
}

Car car1 = new Car();  // Uses all defaults
Car car2 = new Car("Toyota", "Camry");  // Current year
Car car3 = new Car("Honda", "Civic", 2020);  // All specified
```

---

## Destructors

Special method that runs when object is destroyed (rarely used in C#).

```csharp
public class FileHandler {
    ~FileHandler() {
        // Cleanup code
        Console.WriteLine("Destructor called");
    }
}

// Generally avoid destructors - use IDisposable pattern instead
public class FileHandler : IDisposable {
    public void Dispose() {
        // Cleanup code
        Console.WriteLine("Cleaned up");
    }
}

using (var handler = new FileHandler()) {
    // Use handler
}  // Dispose called automatically
```

---

## Access Modifiers

Control visibility of class members.

```csharp
public class BankAccount {
    // Private - only accessible within this class
    private decimal _balance = 1000;
    
    // Public - accessible from anywhere
    public string AccountNumber { get; set; }
    
    // Public method to access private data safely
    public decimal GetBalance() {
        return _balance;
    }
    
    // Public method with validation
    public void Deposit(decimal amount) {
        if (amount > 0) {
            _balance += amount;
        }
    }
}

var account = new BankAccount();
account.AccountNumber = "123456";  // OK - public
Console.WriteLine(account.GetBalance());  // OK - public method
// Console.WriteLine(account._balance);  // Error - private
```

---

## Static Members

Belong to the class, not to instances.

```csharp
public class Counter {
    // Static field - shared by all instances
    public static int TotalCount = 0;
    
    // Instance field - unique per object
    public int InstanceId { get; set; }
    
    public Counter() {
        TotalCount++;
        InstanceId = TotalCount;
    }
}

Counter c1 = new Counter();  // TotalCount = 1, InstanceId = 1
Counter c2 = new Counter();  // TotalCount = 2, InstanceId = 2
Counter c3 = new Counter();  // TotalCount = 3, InstanceId = 3

Console.WriteLine(Counter.TotalCount);  // 3 (class level)
Console.WriteLine(c1.InstanceId);  // 1 (instance level)
```

---

## Class vs Object - Key Differences

| Class | Object |
|-------|--------|
| Blueprint | Instance |
| Defined once | Created multiple times |
| Type | Actual data |
| In source code | In memory |
| Logical | Physical |

---

## Best Practices

✓ Use properties instead of public fields
```csharp
// Good
public class Person {
    public string Name { get; set; }
}

// Avoid
public class Person {
    public string Name;  // Direct field access
}
```

✓ Initialize fields in constructor
```csharp
// Good
public class Car {
    public Car(string make) {
        Make = make;
    }
}

// Less ideal
public class Car {
    public string Make;  // Uninitialized
}
```

✓ Keep methods focused and single-responsibility
```csharp
// Good
public class User {
    public void ValidateEmail() { }
    public void SendEmail() { }
}

// Avoid - too many responsibilities
public class User {
    public void HandleEverything() {
        // Validation, sending, logging, etc.
    }
}
```

---

## Common Mistakes

❌ Forgetting `new` keyword
```csharp
// Error - not instantiated
Person person;
person.Name = "Alice";  // NullReferenceException
```

✓ Always instantiate
```csharp
// Correct
Person person = new Person();
person.Name = "Alice";  // OK
```

❌ Confusing static with instance
```csharp
public class Math {
    public static int GetRandom() => new Random().Next();
}

Math math = new Math();
int value = math.GetRandom();  // Works but confusing

// Better
int value = Math.GetRandom();  // Clearly static
```
