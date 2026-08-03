# Easy OOP Interview Questions

## 1. What is Object-Oriented Programming (OOP)?

**Answer:**
Object-Oriented Programming is a programming paradigm that uses "objects" and "classes" to structure code. Objects represent entities with state (data) and behavior (methods). OOP emphasizes modularity, reusability, and maintainability.

---

## 2. What is the difference between a class and an object?

**Answer:**
- **Class**: A blueprint or template that defines structure and behavior
- **Object**: A concrete instance of a class with actual data values

```csharp
// Class - blueprint
public class Car {
    public string Make { get; set; }
}

// Objects - instances
var myCar = new Car { Make = "Toyota" };
var yourCar = new Car { Make = "Honda" };
```

---

## 3. What are the four pillars of OOP?

**Answer:**
1. **Encapsulation** - Hide internal details, expose only what's needed
2. **Abstraction** - Show essential features, hide complexity
3. **Inheritance** - Reuse code through class hierarchies
4. **Polymorphism** - Objects behave differently based on type

---

## 4. What is a constructor?

**Answer:**
A special method that runs automatically when an object is created. It initializes the object's state (fields and properties).

```csharp
public class Person {
    public string Name { get; set; }
    
    // Constructor
    public Person(string name) {
        Name = name;
    }
}

// Constructor runs here
var person = new Person("Alice");
```

---

## 5. What is encapsulation?

**Answer:**
Encapsulation hides internal implementation details and exposes only necessary interfaces. It protects data integrity and allows safe modifications.

```csharp
public class Account {
    private decimal _balance;  // Hidden
    
    public decimal Balance {
        get { return _balance; }
        set {
            if (value >= 0)
                _balance = value;
        }
    }
}
```

---

## 6. What is inheritance?

**Answer:**
Inheritance allows a derived class (child) to inherit members from a base class (parent), enabling code reuse.

```csharp
public class Animal {
    public void Eat() { }
}

public class Dog : Animal {
    // Inherits Eat() from Animal
}
```

---

## 7. What is polymorphism?

**Answer:**
Polymorphism means "many forms". It allows objects to be treated as instances of their parent class, and methods to behave differently based on the actual object type.

```csharp
public class Shape {
    public virtual double GetArea() { }
}

public class Circle : Shape {
    public override double GetArea() { }  // Different behavior
}
```

---

## 8. What is the difference between public and private?

**Answer:**
- **public**: Accessible from anywhere
- **private**: Accessible only within the same class

```csharp
public class MyClass {
    public int Public { get; set; }     // Accessible everywhere
    private int Private { get; set; }   // Only in this class
}
```

---

## 9. What is an interface?

**Answer:**
An interface defines a contract/specification of what methods and properties a class must implement. It provides polymorphism without implementation.

```csharp
public interface IAnimal {
    void Eat();
}

public class Dog : IAnimal {
    public void Eat() { }  // Must implement
}
```

---

## 10. What is the `new` keyword used for?

**Answer:**
The `new` keyword creates an instance (object) of a class and calls its constructor.

```csharp
Car myCar = new Car();  // Creates new Car object
```

---

## Summary of Key Terms

| Term | Meaning |
|------|---------|
| Class | Blueprint for objects |
| Object | Instance of a class |
| Constructor | Initializes objects |
| Encapsulation | Hide implementation |
| Inheritance | Reuse code |
| Polymorphism | Multiple forms/behaviors |
| Interface | Contract specification |
| Virtual | Can be overridden |
| Abstract | Must be overridden |

## Next Steps

- Review [Medium Questions](../02-Medium/00-Medium-Questions.md) for deeper understanding
- Study actual implementation in code examples
- Practice creating simple classes and objects
