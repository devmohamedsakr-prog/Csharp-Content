# Class Declaration

Quick class structure patterns.

## Basic Class

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public void Greet()
    {
        Console.WriteLine($"Hello, I'm {Name}");
    }
}
```

## With Constructor

```csharp
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; private set; }
    
    public User(int id, string email, string password)
    {
        Id = id;
        Email = email;
        Password = HashPassword(password);
    }
    
    private string HashPassword(string pwd) => BCrypt.HashPassword(pwd);
}
```

## With Validation

```csharp
public class Product
{
    private decimal _price;
    
    public string Name { get; set; }
    public decimal Price
    {
        get => _price;
        set => _price = value > 0 ? value : throw new ArgumentException("Price must be positive");
    }
}
```

## Auto Properties

```csharp
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Salary { get; set; }
    
    // Read-only
    public DateTime HireDate { get; } = DateTime.Now;
    
    // Init-only (set during initialization)
    public string Department { get; init; }
}

// Usage
var emp = new Employee { Id = 1, Department = "IT" };
```

## Static Class

```csharp
public static class MathHelper
{
    public static double Square(double number) => number * number;
    
    public static double Cube(double number) => number * number * number;
    
    public const double PI = 3.14159;
}

// Usage
double area = MathHelper.Square(5);
```

## Abstract Class

```csharp
public abstract class Animal
{
    public string Name { get; set; }
    
    public abstract void MakeSound();
    
    public void Sleep()
    {
        Console.WriteLine("Sleeping...");
    }
}

public class Dog : Animal
{
    public override void MakeSound() => Console.WriteLine("Woof!");
}
```

## Sealed Class

```csharp
public sealed class FinalClass
{
    // Cannot be inherited
    public string Name { get; set; }
}
```

