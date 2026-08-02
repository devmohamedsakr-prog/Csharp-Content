# Interfaces

## Overview
Interfaces define contracts specifying what methods and properties a class must implement without providing implementation details.

## Interface Basics

### Declaration and Implementation
```csharp
// Define interface
public interface IAnimal
{
    void Eat();
    void Sleep();
    string Name { get; set; }
}

// Implement interface
public class Dog : IAnimal
{
    public string Name { get; set; }
    
    public void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }
    
    public void Sleep()
    {
        Console.WriteLine($"{Name} is sleeping");
    }
}

public class Cat : IAnimal
{
    public string Name { get; set; }
    
    public void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }
    
    public void Sleep()
    {
        Console.WriteLine($"{Name} is sleeping");
    }
}

// Usage
IAnimal dog = new Dog { Name = "Buddy" };
dog.Eat();   // Buddy is eating
dog.Sleep(); // Buddy is sleeping
```

## Multiple Interface Implementation

### Implementing Multiple Interfaces
```csharp
public interface IMovable
{
    void Move();
}

public interface ISwimmable
{
    void Swim();
}

public interface IFlying
{
    void Fly();
}

// Class implements multiple interfaces
public class Duck : IMovable, ISwimmable, IFlying
{
    public void Move() => Console.WriteLine("Duck walks");
    public void Swim() => Console.WriteLine("Duck swims");
    public void Fly() => Console.WriteLine("Duck flies");
}

// Usage
var duck = new Duck();
duck.Move();  // Duck walks
duck.Swim();  // Duck swims
duck.Fly();   // Duck flies

// Can be treated as any interface
IMovable movable = duck;
movable.Move();

ISwimmable swimmer = duck;
swimmer.Swim();
```

## Interface Inheritance

### Interfaces Inheriting from Interfaces
```csharp
public interface IAnimal
{
    void Eat();
}

public interface IPet : IAnimal
{
    void Play();
}

public interface IServiceDog : IPet
{
    void PerformDuty();
}

// Implementing derived interface
public class ServiceDog : IServiceDog
{
    public void Eat() => Console.WriteLine("Eating");
    public void Play() => Console.WriteLine("Playing");
    public void PerformDuty() => Console.WriteLine("Performing duty");
}

// Must implement all inherited members
var dog = new ServiceDog();
dog.Eat();
dog.Play();
dog.PerformDuty();
```

## Interface Members (C# 8.0+)

### Default Implementation
```csharp
public interface ILogger
{
    void Log(string message);
    
    // Default implementation (C# 8.0+)
    void LogInfo(string message)
    {
        Console.WriteLine($"[INFO] {message}");
    }
    
    void LogError(string message)
    {
        Console.WriteLine($"[ERROR] {message}");
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
    
    // Can override default implementation
    public void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {message}");
        Console.ResetColor();
    }
}

// Usage
ILogger logger = new ConsoleLogger();
logger.Log("Hello");
logger.LogInfo("Information");
logger.LogError("Error occurred");
```

### Static Members in Interface (C# 11.0+)
```csharp
public interface IConfiguration
{
    // Static properties
    static string DefaultPath => "config.json";
    
    // Static methods
    static IConfiguration Load(string path)
    {
        return new FileConfiguration(path);
    }
}

public class FileConfiguration : IConfiguration
{
    // Static members inherited from interface
}

// Usage - accessed via interface or implementation
string path = IConfiguration.DefaultPath;
var config = IConfiguration.Load("custom.json");
```

## Explicit Interface Implementation

### Resolving Method Name Conflicts
```csharp
public interface IAnimal
{
    void Move();
}

public interface IVehicle
{
    void Move();
}

// Explicit implementation for conflicting methods
public class Car : IAnimal, IVehicle
{
    // Explicit implementation for IAnimal
    void IAnimal.Move()
    {
        Console.WriteLine("Animal moves");
    }
    
    // Explicit implementation for IVehicle
    void IVehicle.Move()
    {
        Console.WriteLine("Vehicle moves");
    }
    
    // Own implementation
    public void Move()
    {
        Console.WriteLine("Car moves");
    }
}

// Usage
var car = new Car();
car.Move();              // Car moves

IAnimal animal = car;
animal.Move();           // Animal moves

IVehicle vehicle = car;
vehicle.Move();          // Vehicle moves
```

## Generic Interfaces

### Type Parameters
```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

public class UserRepository : IRepository<User>
{
    private readonly DbContext _context;
    
    public async Task<User> GetAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
    
    public async Task AddAsync(User entity)
    {
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var user = await GetAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}

// Usage
IRepository<User> userRepo = new UserRepository();
var user = await userRepo.GetAsync(1);
```

## Covariance and Contravariance

### Output (Covariance)
```csharp
public interface IProducer<out T>
{
    T Produce();
}

public class AnimalProducer : IProducer<Animal>
{
    public Animal Produce() => new Animal();
}

// Covariance: IProducer<Dog> can be assigned to IProducer<Animal>
public class DogProducer : IProducer<Dog>
{
    public Dog Produce() => new Dog();
}

IProducer<Animal> producer = new DogProducer(); // Allowed via covariance
Animal animal = producer.Produce();
```

### Input (Contravariance)
```csharp
public interface IConsumer<in T>
{
    void Consume(T item);
}

public class AnimalConsumer : IConsumer<Animal>
{
    public void Consume(Animal item) => Console.WriteLine("Consuming animal");
}

// Contravariance: IConsumer<Animal> can be assigned to IConsumer<Dog>
IConsumer<Dog> consumer = new AnimalConsumer(); // Allowed via contravariance
consumer.Consume(new Dog());
```

## Best Practices

1. **Use Interfaces for Abstraction**
```csharp
// Good: Depend on interface, not implementation
public class UserService
{
    private readonly IUserRepository _repository;
    
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }
}

// Bad: Depend on concrete class
public class UserService
{
    private readonly SqlUserRepository _repository;
}
```

2. **Keep Interfaces Focused (ISP)**
```csharp
// Bad: Too many responsibilities
public interface IRepository
{
    void Add(object entity);
    void Update(object entity);
    void Delete(object entity);
    void SaveToFile(string path);
    void PrintReport();
}

// Good: Single responsibility
public interface IRepository
{
    void Add(object entity);
    void Update(object entity);
    void Delete(object entity);
}

public interface IFileRepository : IRepository
{
    void SaveToFile(string path);
}

public interface IReportGenerator
{
    void PrintReport();
}
```

3. **Leverage Default Implementations**
```csharp
// Good: Provides default behavior, reducing boilerplate
public interface IEntity
{
    int Id { get; set; }
    
    // Default: Mark as deleted instead of actually deleting
    void Delete()
    {
        IsDeleted = true;
    }
    
    bool IsDeleted { get; set; }
}
```

## Common Mistakes

1. **Forgetting All Members Must Be Implemented**
```csharp
// Bad: Incomplete implementation
public class Incomplete : IAnimal
{
    public void Eat() { } // Missing Sleep()
}

// Compiler error: 'Incomplete' does not implement interface member 'IAnimal.Sleep()'
```

2. **Violating Liskov Substitution Principle**
```csharp
// Bad: Implementation violates interface contract
public interface IRepository
{
    Task<T> GetAsync(int id);
}

public class BrokenRepository : IRepository
{
    public Task<T> GetAsync(int id)
    {
        throw new NotImplementedException(); // Violates contract!
    }
}

// Good: Always fulfill contract
public class GoodRepository : IRepository
{
    public async Task<T> GetAsync(int id)
    {
        // Implementation returns what interface promises
        return await _context.Set<T>().FindAsync(id);
    }
}
```

3. **Making Interfaces Too Coarse or Fine**
```csharp
// Bad: Too coarse - forces unnecessary implementations
public interface IEverything
{
    void Create();
    void Read();
    void Update();
    void Delete();
    void Report();
    void Export();
    void Import();
}

// Bad: Too fine - fragmentation
public interface ICreatable { void Create(); }
public interface IReadable { void Read(); }
public interface IUpdatable { void Update(); }
// ... excessive fragmentation

// Good: Balanced, related members
public interface ICrudRepository<T>
{
    Task<T> GetAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

## Quick Summary
- Interfaces define contracts without implementation
- Multiple interface implementation supported
- Explicit implementation resolves conflicts
- Generic interfaces for type-safe contracts
- Default implementation (C# 8.0+) reduces boilerplate
- Covariance and contravariance for flexibility
- Depend on interfaces, not implementations (DIP)
- Single responsibility per interface (ISP)

## Resources
- Interfaces documentation
- Interface design guidelines
- SOLID Principles
