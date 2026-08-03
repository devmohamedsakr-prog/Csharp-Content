# Virtual Methods and Dynamic Dispatch

## Overview

Virtual methods enable runtime polymorphism by allowing derived classes to override base class implementations that are called dynamically.

## Virtual Methods

```csharp
public class Shape
{
    public virtual double GetArea()
    {
        return 0;
    }
    
    public virtual void Display()
    {
        Console.WriteLine("Shape");
    }
}

public class Circle : Shape
{
    public double Radius { get; set; }
    
    public override double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
    
    public override void Display()
    {
        Console.WriteLine($"Circle (Area: {GetArea():F2})");
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
    
    public override void Display()
    {
        Console.WriteLine($"Rectangle (Area: {GetArea():F2})");
    }
}

// Usage - virtual method dispatch
List<Shape> shapes = new List<Shape>
{
    new Circle { Radius = 5 },
    new Rectangle { Width = 4, Height = 6 }
};

foreach (var shape in shapes)
{
    shape.Display();  // Correct version called at runtime
}
```

## Virtual Properties

```csharp
public class Person
{
    public virtual string Name { get; set; }
    public virtual decimal Salary { get; set; }
}

public class Employee : Person
{
    private decimal _salary;
    
    public override decimal Salary
    {
        get { return _salary; }
        set { _salary = value >= 0 ? value : 0; }  // Validation
    }
}

public class Manager : Person
{
    private decimal _salary;
    private List<Employee> _team;
    
    public override decimal Salary
    {
        get { return _salary; }
        set { _salary = value * 1.5m; }  // 50% bonus
    }
}

// Usage
Person person = new Employee { Salary = 50000 };
Console.WriteLine(person.Salary);  // 50000

person = new Manager { Salary = 50000 };
Console.WriteLine(person.Salary);  // 75000
```

## Abstract Virtual Methods

```csharp
public abstract class Animal
{
    // Abstract virtual - must be overridden
    public abstract void MakeSound();
    
    // Virtual - optional override
    public virtual void Sleep()
    {
        Console.WriteLine("Sleeping");
    }
}

public class Dog : Animal
{
    // Must override abstract
    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }
    
    // Optional - can override virtual
    public override void Sleep()
    {
        Console.WriteLine("Dog sleeping");
    }
}

// Usage
// Animal animal = new Animal();  // ERROR: cannot instantiate abstract
Animal animal = new Dog();
animal.MakeSound();  // Woof!
animal.Sleep();      // Dog sleeping
```

## Sealed Override

Prevent further overriding:

```csharp
public class Base
{
    public virtual void Method()
    {
        Console.WriteLine("Base");
    }
}

public class Derived : Base
{
    public sealed override void Method()
    {
        Console.WriteLine("Derived - final version");
    }
}

// Cannot override sealed method
// public class MoreDerived : Derived
// {
//     public override void Method() { }  // ERROR
// }
```

## Virtual Method Lookup

Methods are resolved at runtime based on actual type:

```csharp
public class Base
{
    public virtual void VirtualMethod()
    {
        Console.WriteLine("Base Virtual");
    }
    
    public void NonVirtualMethod()
    {
        Console.WriteLine("Base Non-Virtual");
    }
}

public class Derived : Base
{
    public override void VirtualMethod()
    {
        Console.WriteLine("Derived Virtual");
    }
    
    public new void NonVirtualMethod()
    {
        Console.WriteLine("Derived Non-Virtual");
    }
}

// Usage
Base obj = new Derived();

obj.VirtualMethod();       // Derived Virtual (runtime lookup)
obj.NonVirtualMethod();    // Base Non-Virtual (compile-time)

Derived derived = new Derived();
derived.NonVirtualMethod(); // Derived Non-Virtual
```

## Performance Considerations

Virtual methods have slight performance cost due to runtime lookup:

```csharp
// Non-virtual - direct call, faster
public class FastClass
{
    public void Method() { }
}

// Virtual - lookup overhead
public class SlowClass
{
    public virtual void Method() { }
}

// Generally negligible unless called millions of times
for (int i = 0; i < 1000000; i++)
{
    obj.Method();  // Tiny difference
}
```

## Common Patterns

### Pattern 1: Template Method

```csharp
public abstract class DataProcessor
{
    // Template method - defines algorithm structure
    public void Process(string input)
    {
        string data = Load(input);
        data = Transform(data);
        Save(data);
    }
    
    // Override these methods
    protected abstract string Load(string input);
    protected abstract string Transform(string data);
    protected abstract void Save(string data);
}

public class JsonProcessor : DataProcessor
{
    protected override string Load(string input)
    {
        return JsonConvert.DeserializeObject(input);
    }
    
    protected override string Transform(string data)
    {
        // JSON-specific transformation
        return data.ToUpper();
    }
    
    protected override void Save(string data)
    {
        File.WriteAllText("output.json", data);
    }
}
```

### Pattern 2: Strategy Pattern

```csharp
public interface ISortStrategy
{
    void Sort(int[] array);
}

public class QuickSort : ISortStrategy
{
    public void Sort(int[] array)
    {
        // QuickSort implementation
    }
}

public class MergeSort : ISortStrategy
{
    public void Sort(int[] array)
    {
        // MergeSort implementation
    }
}

public class Sorter
{
    private ISortStrategy _strategy;
    
    public void SetStrategy(ISortStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public void Sort(int[] array)
    {
        _strategy.Sort(array);  // Virtual dispatch
    }
}

// Usage
var sorter = new Sorter();
sorter.SetStrategy(new QuickSort());
sorter.Sort(array);
```

### Pattern 3: Factory Pattern

```csharp
public abstract class DataProvider
{
    public abstract IEnumerable<T> GetData<T>();
}

public class SqlDataProvider : DataProvider
{
    public override IEnumerable<T> GetData<T>()
    {
        // SQL implementation
        yield break;
    }
}

public class MongoDataProvider : DataProvider
{
    public override IEnumerable<T> GetData<T>()
    {
        // MongoDB implementation
        yield break;
    }
}

public class DataProviderFactory
{
    public static DataProvider CreateProvider(string type)
    {
        return type switch
        {
            "sql" => new SqlDataProvider(),
            "mongo" => new MongoDataProvider(),
            _ => throw new ArgumentException("Unknown type")
        };
    }
}

// Usage
var provider = DataProviderFactory.CreateProvider("sql");
var data = provider.GetData<User>();
```

## Best Practices

### 1. Use Virtual Judiciously

```csharp
// Good - Intentional override point
public class Logger
{
    public virtual void Log(string message)
    {
        Console.WriteLine(message);
    }
}

// Bad - Unnecessary virtual
public class Utility
{
    public virtual string FormatString(string input)
    {
        return input.Trim();
    }
}
```

### 2. Document Virtual Methods

```csharp
/// <summary>
/// Logs a message. Derived classes can override to customize behavior.
/// </summary>
/// <remarks>
/// Always call base.Log() unless you want to suppress logging.
/// </remarks>
public virtual void Log(string message)
{
    Console.WriteLine(message);
}
```

### 3. Consider Abstract Classes for Virtual Methods

```csharp
// Good - Abstract class with virtual methods
public abstract class BaseService
{
    public abstract void Process();
    public virtual void Log(string message) { }
}

// Also good - Interface with implementations (C# 8+)
public interface IBaseService
{
    void Process();
    void Log(string message) => Console.WriteLine(message);
}
```

## Summary

- **Virtual methods** - Enables runtime polymorphism
- **Override** - Provide new implementation
- **Abstract virtual** - Must be overridden
- **Sealed override** - Prevent further overriding
- **Dynamic dispatch** - Runtime method resolution
- **Templates** - Define algorithm structure
- **Strategies** - Swap implementations dynamically

## Next Steps

- Learn [Abstract-Classes](../../03-Advanced-OOP/02-Abstract-Classes/00-Abstract-Classes.md)
- Study [Interfaces](../../03-Advanced-OOP/01-Interfaces/00-Interfaces.md)
- Review [Design-Patterns](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
