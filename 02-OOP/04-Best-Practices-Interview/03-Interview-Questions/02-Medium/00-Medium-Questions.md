# Medium OOP Interview Questions

## 1. Explain the difference between `virtual` and `abstract`

**Answer:**
- **virtual**: Method has a default implementation, can be overridden optionally
- **abstract**: Method has no implementation, MUST be overridden by derived classes

```csharp
// Virtual - optional override
public class Vehicle {
    public virtual void Start() {
        Console.WriteLine("Starting...");
    }
}

// Abstract - required override
public abstract class Shape {
    public abstract double GetArea();  // No implementation
}
```

---

## 2. What is the difference between an interface and an abstract class?

**Answer:**

| Aspect | Interface | Abstract Class |
|--------|-----------|----------------|
| Implementation | None | Partial |
| State (fields) | No | Yes |
| Inheritance | Multiple | Single |
| Access modifiers | Public only | Any |

```csharp
// Interface - contract only
public interface ILogger {
    void Log(string message);
}

// Abstract class - partial implementation
public abstract class DataProcessor {
    protected string _data;  // Can have state
    public abstract void Process();
}
```

---

## 3. What is constructor chaining?

**Answer:**
Constructor chaining calls one constructor from another to avoid code duplication.

```csharp
public class User {
    public string Name { get; set; }
    public int Age { get; set; }
    
    public User() : this("Unknown", 0) { }
    public User(string name) : this(name, 0) { }
    public User(string name, int age) {
        Name = name;
        Age = age;
    }
}
```

---

## 4. Explain the `base` keyword

**Answer:**
The `base` keyword accesses members from the parent class. Used to call parent methods or access parent properties.

```csharp
public class Animal {
    public virtual void Speak() {
        Console.WriteLine("Generic sound");
    }
}

public class Dog : Animal {
    public override void Speak() {
        base.Speak();  // Calls parent implementation
        Console.WriteLine("Woof!");
    }
}
```

---

## 5. What is polymorphism? Give a real-world example

**Answer:**
Polymorphism allows different objects to respond to the same message in different ways.

Real-world example: A "Payment" operation works differently for each payment method:

```csharp
public abstract class PaymentMethod {
    public abstract void Process(decimal amount);
}

public class CreditCard : PaymentMethod {
    public override void Process(decimal amount) {
        Console.WriteLine($"Processing ${amount} via Credit Card");
    }
}

public class PayPal : PaymentMethod {
    public override void Process(decimal amount) {
        Console.WriteLine($"Processing ${amount} via PayPal");
    }
}

// Usage - polymorphic
PaymentMethod payment = GetPaymentMethod();
payment.Process(99.99m);  // Correct method called based on type
```

---

## 6. What is the difference between `==` and `.Equals()` for objects?

**Answer:**
- `==`: Checks reference equality by default (same object in memory)
- `.Equals()`: Can be overridden to check value equality

```csharp
public class Person {
    public string Name { get; set; }
    
    public override bool Equals(object obj) {
        if (obj is Person other)
            return this.Name == other.Name;
        return false;
    }
}

Person p1 = new Person { Name = "Alice" };
Person p2 = new Person { Name = "Alice" };

Console.WriteLine(p1 == p2);        // false (different objects)
Console.WriteLine(p1.Equals(p2));   // true (same name)
```

---

## 7. Explain the Liskov Substitution Principle

**Answer:**
Objects of derived classes should be substitutable for objects of base classes without breaking the program.

```csharp
// Good - respects LSP
public class Vehicle {
    public virtual int GetMaxSpeed() => 100;
}

public class Car : Vehicle {
    public override int GetMaxSpeed() => 200;  // Valid override
}

// Can use Car wherever Vehicle is expected
Vehicle vehicle = new Car();
int speed = vehicle.GetMaxSpeed();  // Works correctly
```

---

## 8. What is method overloading?

**Answer:**
Method overloading allows multiple methods with the same name but different parameters in the same class.

```csharp
public class Calculator {
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
```

---

## 9. What is method overriding?

**Answer:**
Method overriding allows a derived class to provide a new implementation for a method from the base class.

```csharp
public class Animal {
    public virtual void Speak() {
        Console.WriteLine("Animal sound");
    }
}

public class Dog : Animal {
    public override void Speak() {  // Override
        Console.WriteLine("Woof!");
    }
}
```

---

## 10. Explain the Single Responsibility Principle (SRP)

**Answer:**
Each class should have only one reason to change (one responsibility). It should do one thing and do it well.

```csharp
// Bad - Multiple responsibilities
public class UserManager {
    public void CreateUser() { }
    public void SendEmail() { }
    public void SaveToDatabase() { }
}

// Good - Single responsibility each
public class UserService {
    public void CreateUser() { }
}

public class EmailService {
    public void SendEmail() { }
}

public class DataRepository {
    public void SaveToDatabase() { }
}
```

---

## Summary

- **virtual/abstract**: Control overriding behavior
- **interface/abstract class**: Choose based on inheritance needs
- **Constructor chaining**: Reduce duplication
- **base keyword**: Access parent members
- **Polymorphism**: Different behavior, same interface
- **Overloading**: Same method name, different parameters
- **Overriding**: Replace parent implementation
- **SOLID principles**: Write maintainable code

## Next Steps

- Review [Hard Questions](../03-Hard/00-Hard-Questions.md) for advanced concepts
- Practice implementing design patterns
- Study real-world code examples
