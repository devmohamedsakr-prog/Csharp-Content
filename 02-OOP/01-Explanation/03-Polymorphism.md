# Polymorphism

## Overview
Polymorphism means "many forms" - same method, different behaviors based on object type.

---

## Method Overriding (Runtime Polymorphism)

Different derived classes override same method differently.

```csharp
public class Shape {
    public virtual void Draw() {
        Console.WriteLine("Drawing shape");
    }
}

public class Circle : Shape {
    public override void Draw() {
        Console.WriteLine("Drawing circle");
    }
}

public class Rectangle : Shape {
    public override void Draw() {
        Console.WriteLine("Drawing rectangle");
    }
}

public class Triangle : Shape {
    public override void Draw() {
        Console.WriteLine("Drawing triangle");
    }
}

// Polymorphism in action
List<Shape> shapes = new List<Shape> {
    new Circle(),
    new Rectangle(),
    new Triangle()
};

foreach (Shape shape in shapes) {
    shape.Draw();  // Different behavior for each
}

// Output:
// Drawing circle
// Drawing rectangle
// Drawing triangle
```

---

## Method Overloading (Compile-Time Polymorphism)

Same method name with different parameters.

```csharp
public class Calculator {
    // Method 1: two integers
    public int Add(int a, int b) {
        return a + b;
    }
    
    // Method 2: three integers
    public int Add(int a, int b, int c) {
        return a + b + c;
    }
    
    // Method 3: two doubles
    public double Add(double a, double b) {
        return a + b;
    }
    
    // Method 4: different parameter name pattern
    public void Add(int[] numbers) {
        int sum = 0;
        foreach (int num in numbers) {
            sum += num;
        }
    }
}

// Usage
Calculator calc = new Calculator();
calc.Add(5, 3);              // 8 (int, int)
calc.Add(5, 3, 2);           // 10 (int, int, int)
calc.Add(5.5, 3.2);          // 8.7 (double, double)
calc.Add(new int[] { 1, 2, 3 });  // (array)
```

---

## Interface-Based Polymorphism

Use interfaces for loose coupling.

```csharp
// Interface defines contract
public interface INotifier {
    void Send(string message);
}

// Different implementations
public class EmailNotifier : INotifier {
    public void Send(string message) {
        Console.WriteLine($"Sending email: {message}");
    }
}

public class SmsNotifier : INotifier {
    public void Send(string message) {
        Console.WriteLine($"Sending SMS: {message}");
    }
}

public class SlackNotifier : INotifier {
    public void Send(string message) {
        Console.WriteLine($"Sending Slack message: {message}");
    }
}

// Polymorphic usage
public class Alerter {
    private readonly INotifier _notifier;
    
    public Alerter(INotifier notifier) {
        _notifier = notifier;
    }
    
    public void Alert(string message) {
        _notifier.Send(message);
    }
}

// Usage
INotifier emailNotifier = new EmailNotifier();
INotifier smsNotifier = new SmsNotifier();
INotifier slackNotifier = new SlackNotifier();

var alerter1 = new Alerter(emailNotifier);
alerter1.Alert("Alert via email");

var alerter2 = new Alerter(smsNotifier);
alerter2.Alert("Alert via SMS");

var alerter3 = new Alerter(slackNotifier);
alerter3.Alert("Alert via Slack");
```

---

## Abstract Classes for Polymorphism

Define abstract methods that derived classes must implement.

```csharp
// Abstract base class
public abstract class PaymentProcessor {
    // Abstract method - must be implemented by derived classes
    public abstract void Process(decimal amount);
    
    // Concrete method - same for all
    public void LogTransaction(decimal amount) {
        Console.WriteLine($"Processing ${amount}");
    }
}

// Concrete implementations
public class CreditCardProcessor : PaymentProcessor {
    public override void Process(decimal amount) {
        Console.WriteLine($"Processing credit card payment: ${amount}");
    }
}

public class PayPalProcessor : PaymentProcessor {
    public override void Process(decimal amount) {
        Console.WriteLine($"Processing PayPal payment: ${amount}");
    }
}

public class BitcoinProcessor : PaymentProcessor {
    public override void Process(decimal amount) {
        Console.WriteLine($"Processing Bitcoin payment: ${amount}");
    }
}

// Polymorphic usage
List<PaymentProcessor> processors = new List<PaymentProcessor> {
    new CreditCardProcessor(),
    new PayPalProcessor(),
    new BitcoinProcessor()
};

foreach (PaymentProcessor processor in processors) {
    processor.LogTransaction(99.99);
    processor.Process(99.99);
    Console.WriteLine();
}
```

---

## Real-World Example: Employee Payroll

```csharp
// Abstract base
public abstract class Employee {
    public string Name { get; set; }
    
    public abstract decimal CalculatePay();
}

// Derived classes
public class HourlyEmployee : Employee {
    public decimal HourlyRate { get; set; }
    public int HoursWorked { get; set; }
    
    public override decimal CalculatePay() {
        decimal regularPay = HoursWorked <= 40 
            ? HourlyRate * HoursWorked 
            : HourlyRate * 40;
        
        decimal overtimePay = HoursWorked > 40 
            ? (HoursWorked - 40) * HourlyRate * 1.5m 
            : 0;
        
        return regularPay + overtimePay;
    }
}

public class SalariedEmployee : Employee {
    public decimal AnnualSalary { get; set; }
    
    public override decimal CalculatePay() {
        return AnnualSalary / 12;  // Monthly
    }
}

public class CommissionEmployee : Employee {
    public decimal Sales { get; set; }
    public decimal CommissionRate { get; set; }
    
    public override decimal CalculatePay() {
        return Sales * CommissionRate;
    }
}

// Polymorphic processing
public class PayrollSystem {
    public void ProcessPayroll(List<Employee> employees) {
        foreach (Employee employee in employees) {
            decimal pay = employee.CalculatePay();
            Console.WriteLine($"{employee.Name}: ${pay:F2}");
        }
    }
}

// Usage
var employees = new List<Employee> {
    new HourlyEmployee { Name = "John", HourlyRate = 15, HoursWorked = 45 },
    new SalariedEmployee { Name = "Alice", AnnualSalary = 60000 },
    new CommissionEmployee { Name = "Bob", Sales = 10000, CommissionRate = 0.05m }
};

var payroll = new PayrollSystem();
payroll.ProcessPayroll(employees);

// Output:
// John: $712.50
// Alice: $5000.00
// Bob: $500.00
```

---

## Dynamic Polymorphism with `dynamic`

Resolve type at runtime (use sparingly).

```csharp
dynamic obj = new Circle();
obj.Draw();  // Works - Circle has Draw()

obj = new Rectangle();
obj.Draw();  // Works - Rectangle has Draw()

obj = "Hello";
// obj.Draw();  // Runtime error - string has no Draw()
```

---

## Benefits of Polymorphism

✓ **Code Reusability** - Write once, use for many types

```csharp
// Works for ANY type that implements IAnimal
public void MakeAnimalSound(IAnimal animal) {
    animal.MakeSound();
}

MakeAnimalSound(new Dog());
MakeAnimalSound(new Cat());
MakeAnimalSound(new Bird());
```

✓ **Flexibility** - Easy to add new types

```csharp
// Add new animal without changing existing code
public class Lion : IAnimal {
    public void MakeSound() {
        Console.WriteLine("Roar!");
    }
}

MakeAnimalSound(new Lion());  // Works!
```

✓ **Maintainability** - Changes in one place

```csharp
// Change payment logic once, works for all processors
```

---

## Best Practices

✓ Use polymorphism to reduce code duplication
```csharp
// Instead of
if (obj is Circle) ((Circle)obj).Draw();
if (obj is Rectangle) ((Rectangle)obj).Draw();

// Use polymorphism
shape.Draw();  // Works for any shape
```

✓ Design for interfaces, not implementations
```csharp
// Good
public void Process(IPaymentProcessor processor) { }

// Less flexible
public void Process(CreditCardProcessor processor) { }
```

✓ Use abstract classes for shared implementation
```csharp
// Good - shared code + enforced contract
public abstract class Employee { }

// Less ideal - duplicate code across classes
public class HourlyEmployee { }
public class SalariedEmployee { }
```

---

## Common Mistakes

❌ Not using `virtual` keyword
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
```

❌ Casting when polymorphism would work
```csharp
// Unnecessarily casting
Shape shape = new Circle();
((Circle)shape).Draw();

// Better - use polymorphism
shape.Draw();
```
