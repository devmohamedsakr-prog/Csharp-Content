# Terms and Concepts

Essential terminology for understanding Object-Oriented Programming in C#.

## 📌 Core Terminology

### Class
A blueprint or template for creating objects. Defines properties, methods, and behaviors.

**Example:**
```csharp
public class Car
{
    public string Brand { get; set; }
    public string Model { get; set; }
    
    public void Drive()
    {
        Console.WriteLine("Car is driving");
    }
}
```

### Object & Instance
An instance of a class - a concrete entity created from the class blueprint.

```csharp
Car myCar = new Car(); // myCar is an object/instance of the Car class
myCar.Brand = "Toyota";
```

### Property (Member Variable)
Data associated with an object. Represents state or characteristics.

```csharp
public class Person
{
    public string Name { get; set; }  // Property
    public int Age { get; set; }      // Property
}
```

### Method
An action or behavior that an object can perform.

```csharp
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}
```

### Access Modifiers
Control visibility and accessibility of class members:
- `public` - Accessible everywhere
- `private` - Accessible only within the class
- `protected` - Accessible within the class and derived classes
- `internal` - Accessible within the same assembly

```csharp
public class Account
{
    public string AccountNumber { get; set; }     // Public
    private decimal _balance;                      // Private
    protected string AccountType { get; set; }    // Protected
}
```

### Constructor
Special method that initializes an object when it's created.

```csharp
public class User
{
    public string Username { get; set; }
    
    public User(string username)
    {
        Username = username;
    }
}

var user = new User("john_doe"); // Constructor called here
```

### Method Overloading
Multiple methods with the same name but different parameters.

```csharp
public class Printer
{
    public void Print(string text)
    {
        Console.WriteLine(text);
    }
    
    public void Print(int number)
    {
        Console.WriteLine(number);
    }
    
    public void Print(double value)
    {
        Console.WriteLine(value);
    }
}

printer.Print("Hello");     // Calls first method
printer.Print(42);          // Calls second method
printer.Print(3.14);        // Calls third method
```

### Static Members
Belong to the class itself, not to instances.

```csharp
public class Counter
{
    public static int TotalCount { get; set; }
    
    public static void IncrementCount()
    {
        TotalCount++;
    }
}

Counter.IncrementCount(); // Called on class, not instance
```

---

## 📚 Files in This Section

- `01-Class-and-Object.md` - Deep dive into classes and objects
- `02-Properties-and-Fields.md` - Understanding properties vs fields
- `03-Methods-and-Parameters.md` - Methods and function signatures
- `04-Access-Modifiers.md` - Visibility and encapsulation
- `05-Constructors.md` - Object initialization
- `06-Static-Members.md` - Class-level vs instance-level members

