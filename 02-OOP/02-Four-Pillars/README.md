# The Four Pillars of OOP

The four fundamental principles that make up Object-Oriented Programming.

## 1️⃣ Encapsulation

**Definition:** Bundling data (properties) and behavior (methods) into a single unit (class), while hiding internal details.

**Key Benefits:**
- Data protection
- Control access to internal state
- Flexibility to change implementation without affecting users

**Example:**
```csharp
public class BankAccount
{
    private decimal _balance;  // Hidden from outside
    
    public decimal Balance
    {
        get { return _balance; }
        private set { _balance = value; }  // Can only be set internally
    }
    
    public void Deposit(decimal amount)
    {
        if (amount > 0)
            _balance += amount;
    }
    
    public bool Withdraw(decimal amount)
    {
        if (amount > 0 && amount <= _balance)
        {
            _balance -= amount;
            return true;
        }
        return false;
    }
}
```

---

## 2️⃣ Inheritance

**Definition:** Creating new classes based on existing classes, inheriting their properties and methods.

**Key Benefits:**
- Code reuse
- Establish relationships between classes
- Create class hierarchies

**Example:**
```csharp
public class Animal
{
    public string Name { get; set; }
    
    public virtual void MakeSound()
    {
        Console.WriteLine("Some sound");
    }
}

public class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }
}

public class Cat : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Meow!");
    }
}

// Usage
Dog dog = new Dog { Name = "Buddy" };
dog.MakeSound();  // Output: Woof!
```

---

## 3️⃣ Polymorphism

**Definition:** Objects can take multiple forms. Same method call can behave differently based on the object type.

**Types:**
- **Compile-time (Method Overloading)**
- **Runtime (Method Overriding)**

**Example:**
```csharp
public class Shape
{
    public virtual double GetArea()
    {
        return 0;
    }
}

public class Circle : Shape
{
    public double Radius { get; set; }
    
    public override double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    
    public override double GetArea()
    {
        return Width * Height;
    }
}

// Usage - Polymorphism in action
Shape shape1 = new Circle { Radius = 5 };
Shape shape2 = new Rectangle { Width = 4, Height = 6 };

Console.WriteLine(shape1.GetArea());  // π * 25 ≈ 78.54
Console.WriteLine(shape2.GetArea());  // 24
```

---

## 4️⃣ Abstraction

**Definition:** Hiding complex implementation details and showing only essential features.

**Key Benefits:**
- Simplify complex systems
- Reduce dependency on implementation details
- Create contracts for derived classes

**Example:**
```csharp
public abstract class DataProcessor
{
    // Abstract method - must be implemented by derived classes
    public abstract void Process(string data);
    
    // Concrete method - shared implementation
    public void LogProcessing(string message)
    {
        Console.WriteLine($"[{DateTime.Now}] {message}");
    }
}

public class JsonProcessor : DataProcessor
{
    public override void Process(string data)
    {
        Console.WriteLine("Processing JSON...");
        // Complex JSON parsing logic
    }
}

public class XmlProcessor : DataProcessor
{
    public override void Process(string data)
    {
        Console.WriteLine("Processing XML...");
        // Complex XML parsing logic
    }
}
```

---

## 📚 Files in This Section

- `01-Encapsulation.md` - Data hiding and access control
- `02-Inheritance.md` - Class hierarchies and code reuse
- `03-Polymorphism.md` - Runtime behavior flexibility
- `04-Abstraction.md` - Abstract classes and interfaces
- `05-Pillar-Interactions.md` - How pillars work together

---

## 🎯 Key Takeaway

The four pillars work together to create flexible, maintainable, and scalable code:
- **Encapsulation** protects data
- **Inheritance** promotes reuse
- **Polymorphism** enables flexibility
- **Abstraction** simplifies complexity

