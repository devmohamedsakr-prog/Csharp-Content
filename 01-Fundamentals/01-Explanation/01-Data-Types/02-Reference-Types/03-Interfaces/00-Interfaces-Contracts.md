# Interfaces: Contracts for Types

## Overview

An `interface` defines a contract that classes and structs must implement. It specifies what methods and properties a type must have, but not how they work.

### Characteristics
```csharp
public interface IAnimal {
    void MakeSound();
    string Name { get; }
}

// Reference type (interface itself)
// Cannot be instantiated directly
// No implementation provided
// Supports multiple inheritance
// Defines obligations for implementing types
```

## Interface Basics

### Defining Interfaces

#### Basic Interface

```csharp
public interface IVehicle {
    // Property
    string Make { get; }
    
    // Method
    void Start();
    void Stop();
}
```

#### Interface with Events and Indexers

```csharp
public interface IDataStore {
    // Property
    int Count { get; }
    
    // Method
    void Save(object data);
    object Load(int id);
    
    // Event
    event EventHandler OnDataChanged;
    
    // Indexer
    object this[int index] { get; set; }
}
```

### Implementing Interfaces

#### Basic Implementation

```csharp
public interface IShape {
    double CalculateArea();
    double CalculatePerimeter();
}

public class Circle : IShape {
    public double Radius { get; set; }
    
    public double CalculateArea() {
        return Math.PI * Radius * Radius;
    }
    
    public double CalculatePerimeter() {
        return 2 * Math.PI * Radius;
    }
}
```

#### Multiple Interface Implementation

```csharp
public interface IMovable {
    void Move();
}

public interface IResizable {
    void Resize(double scale);
}

public class Rectangle : IMovable, IResizable {
    public double Width { get; set; }
    public double Height { get; set; }
    
    public void Move() {
        Console.WriteLine("Moving rectangle");
    }
    
    public void Resize(double scale) {
        Width *= scale;
        Height *= scale;
    }
}
```

#### Explicit Interface Implementation

```csharp
public interface ILogger {
    void Log(string message);
}

public interface IDebugger {
    void Log(string message);  // Different meaning
}

public class DebugLogger : ILogger, IDebugger {
    // Explicit implementation for ILogger
    void ILogger.Log(string message) {
        Console.WriteLine($"LOG: {message}");
    }
    
    // Explicit implementation for IDebugger
    void IDebugger.Log(string message) {
        Console.WriteLine($"DEBUG: {message}");
    }
}

// Usage
DebugLogger logger = new();
((ILogger)logger).Log("Error");  // "LOG: Error"
((IDebugger)logger).Log("Error");  // "DEBUG: Error"
```

## Interface Polymorphism

### Working with Interfaces

```csharp
public interface IAnimal {
    void MakeSound();
}

public class Dog : IAnimal {
    public void MakeSound() => Console.WriteLine("Woof!");
}

public class Cat : IAnimal {
    public void MakeSound() => Console.WriteLine("Meow!");
}

// Polymorphic behavior
IAnimal animal1 = new Dog();
IAnimal animal2 = new Cat();

animal1.MakeSound();  // "Woof!"
animal2.MakeSound();  // "Meow!"

// Collection of interfaces
IAnimal[] animals = {
    new Dog(),
    new Cat(),
    new Dog()
};

foreach (IAnimal animal in animals) {
    animal.MakeSound();  // Each makes its own sound
}
```

### Dependency Injection

```csharp
public interface IRepository {
    object Get(int id);
    void Save(object item);
}

public class SqlRepository : IRepository {
    public object Get(int id) => /* SQL query */;
    public void Save(object item) => /* SQL insert */;
}

public class Service {
    private readonly IRepository _repository;
    
    // Inject dependency
    public Service(IRepository repository) {
        _repository = repository;
    }
    
    public void ProcessData(int id) {
        object data = _repository.Get(id);
        // Process...
        _repository.Save(data);
    }
}

// Usage - can swap implementations
IRepository repo = new SqlRepository();
var service = new Service(repo);
```

## Default Interface Members (C# 8+)

### Methods with Implementation

```csharp
public interface ILogger {
    void Log(string message);
    
    // Default implementation
    void LogError(string message) {
        Console.WriteLine($"ERROR: {message}");
    }
    
    // Static method
    static void LogStatic(string message) {
        Console.WriteLine($"[STATIC] {message}");
    }
}

public class ConsoleLogger : ILogger {
    public void Log(string message) {
        Console.WriteLine(message);
    }
    
    // Can use inherited default implementation
    // Or override it
}

// Usage
ConsoleLogger logger = new();
logger.Log("Info");           // Uses implementation
logger.LogError("Error");     // Uses default implementation
ILogger.LogStatic("Static");  // Call static method
```

## Interface Design Patterns

### Segregation Principle

```csharp
// Bad - fat interface
public interface IWorker {
    void Work();
    void Eat();
    void Sleep();
}

// Good - segregated interfaces
public interface IWorkable {
    void Work();
}

public interface IEatable {
    void Eat();
}

public interface ISleepable {
    void Sleep();
}

public class Human : IWorkable, IEatable, ISleepable {
    public void Work() { }
    public void Eat() { }
    public void Sleep() { }
}

public class Robot : IWorkable {
    public void Work() { }
    // Not forced to implement Eat/Sleep
}
```

### Strategy Pattern

```csharp
public interface ISortStrategy {
    void Sort(int[] array);
}

public class BubbleSortStrategy : ISortStrategy {
    public void Sort(int[] array) {
        // Bubble sort implementation
    }
}

public class QuickSortStrategy : ISortStrategy {
    public void Sort(int[] array) {
        // Quick sort implementation
    }
}

public class DataProcessor {
    private ISortStrategy _sortStrategy;
    
    public DataProcessor(ISortStrategy strategy) {
        _sortStrategy = strategy;
    }
    
    public void ProcessData(int[] data) {
        _sortStrategy.Sort(data);
    }
}

// Usage
DataProcessor processor = new(new QuickSortStrategy());
processor.ProcessData(new[] { 3, 1, 4, 1, 5 });
```

### Factory Pattern

```csharp
public interface IShape {
    double GetArea();
}

public class Circle : IShape {
    public double Radius { get; set; }
    public double GetArea() => Math.PI * Radius * Radius;
}

public class Square : IShape {
    public double Side { get; set; }
    public double GetArea() => Side * Side;
}

public static class ShapeFactory {
    public static IShape CreateShape(string type) {
        return type.ToLower() switch {
            "circle" => new Circle { Radius = 5 },
            "square" => new Square { Side = 5 },
            _ => throw new ArgumentException("Unknown shape")
        };
    }
}

// Usage
IShape shape = ShapeFactory.CreateShape("circle");
Console.WriteLine(shape.GetArea());
```

## Common Interfaces in .NET

### IEnumerable / IEnumerator

```csharp
public class CustomCollection : IEnumerable {
    private int[] _items = { 1, 2, 3, 4, 5 };
    
    public IEnumerator GetEnumerator() {
        return _items.GetEnumerator();
    }
}

// Usage
var collection = new CustomCollection();
foreach (int item in collection) {
    Console.WriteLine(item);
}
```

### IComparable

```csharp
public class Person : IComparable {
    public string Name { get; set; }
    public int Age { get; set; }
    
    public int CompareTo(object obj) {
        if (obj is not Person person) return 1;
        return Age.CompareTo(person.Age);
    }
}

Person[] people = {
    new Person { Name = "Alice", Age = 30 },
    new Person { Name = "Bob", Age = 25 }
};

Array.Sort(people);  // Sorts by age
```

### IDisposable

```csharp
public class FileHandler : IDisposable {
    private FileStream _file;
    
    public FileHandler(string path) {
        _file = File.OpenRead(path);
    }
    
    public void Dispose() {
        _file?.Dispose();
        GC.SuppressFinalize(this);
    }
    
    // Destructor as fallback
    ~FileHandler() {
        Dispose();
    }
}

// Usage with using statement
using (var handler = new FileHandler("data.txt")) {
    // Use handler
}  // Automatically disposed
```

## Generic Interfaces

```csharp
// Generic repository pattern
public interface IRepository<T> {
    T Get(int id);
    void Save(T item);
    void Delete(int id);
    IEnumerable<T> GetAll();
}

public class UserRepository : IRepository<User> {
    public User Get(int id) { /* ... */ }
    public void Save(User item) { /* ... */ }
    public void Delete(int id) { /* ... */ }
    public IEnumerable<User> GetAll() { /* ... */ }
}

// Usage
IRepository<User> userRepo = new UserRepository();
User user = userRepo.Get(1);
```

## Interface vs Abstract Class

### Key Differences

| Aspect | Interface | Abstract Class |
|--------|-----------|-----------------|
| Inheritance | Multiple | Single |
| State | No fields (usually) | Can have fields |
| Access Modifiers | Usually public | Can be various |
| Constructor | No | Yes |
| Implementation | No (except default members) | Yes |
| Use Case | Contracts, capabilities | Shared base behavior |

### When to Use

#### Use Interface When:
```csharp
// Multiple types share behavior
public interface IPrintable {
    void Print();
}

public class Document : IPrintable { }
public class Report : IPrintable { }
public class Email : IPrintable { }
```

#### Use Abstract Class When:
```csharp
// Shared implementation needed
public abstract class Vehicle {
    public int Speed { get; set; }  // Shared property
    
    public virtual void Drive() {
        Console.WriteLine($"Driving at {Speed} mph");
    }
}

public class Car : Vehicle { }
```

## Common Interface Mistakes

❌ **Interface with implementation details**
```csharp
public interface IDataFetcher {
    // Should not expose internal details
    private List<string> _cache;
    void FetchData();
}
```

✓ **Clean interface contract**
```csharp
public interface IDataFetcher {
    // Just the contract
    void FetchData();
    T GetData<T>();
}
```

❌ **Fat interface (violates segregation)**
```csharp
public interface IPaymentProcessor {
    void ProcessPayment();
    void RefundPayment();
    void GenerateReport();
    void SendEmail();
}
```

✓ **Segregated interfaces**
```csharp
public interface IPaymentProcessor {
    void ProcessPayment();
    void RefundPayment();
}

public interface IReportGenerator {
    void GenerateReport();
}
```

## Summary

**Interface Characteristics**:
- Defines a contract
- Multiple inheritance allowed
- Cannot be instantiated directly
- Reference type
- Enables polymorphism and dependency injection

**Best Practices**:
- Keep interfaces focused (Single Responsibility)
- Use dependency injection with interfaces
- Name interfaces with "I" prefix
- Prefer interfaces for contracts
- Use abstract classes for shared implementation

---

**Key Takeaway**: Interfaces define what a type can do, enabling polymorphism and loose coupling. Use them for contracts and dependency injection. Keep them small and focused.
