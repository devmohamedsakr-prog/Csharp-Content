# Constructors and Destructors

## Overview

Constructors are special methods that initialize object instances when created. Destructors (rarely used) perform cleanup when objects are destroyed. In modern C#, the IDisposable pattern is preferred for resource management.

## Constructors

### What is a Constructor?

A constructor is a special method that:
- Runs automatically when an object is created
- Initializes fields and properties
- Has same name as the class
- Has no return type

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor - runs when Person is created
    public Person()
    {
        Console.WriteLine("Person created");
        Name = "Unknown";
        Age = 0;
    }
}

// Usage
var person = new Person();  // Constructor runs here
// Output: Person created
```

### Default Constructor

If you don't define a constructor, C# creates a default one:

```csharp
public class Dog
{
    // No explicit constructor
    // C# creates: public Dog() { }
}

Dog dog = new Dog();  // Works with default constructor
```

### Custom Constructor with Parameters

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor with parameters
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
        Console.WriteLine($"Person created: {name}, {age}");
    }
}

// Usage
var person = new Person("Alice", 30);
// Output: Person created: Alice, 30
```

### Constructor Overloading

Multiple constructors with different signatures:

```csharp
public class BankAccount
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    
    // Constructor 1: No parameters
    public BankAccount()
    {
        AccountNumber = Guid.NewGuid().ToString();
        Balance = 0;
    }
    
    // Constructor 2: Account number only
    public BankAccount(string accountNumber)
    {
        AccountNumber = accountNumber;
        Balance = 0;
    }
    
    // Constructor 3: Both parameters
    public BankAccount(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }
}

// Usage
var account1 = new BankAccount();                      // Uses constructor 1
var account2 = new BankAccount("ACC123");              // Uses constructor 2
var account3 = new BankAccount("ACC456", 1000);        // Uses constructor 3
```

### Constructor Chaining with `this`

Call another constructor to avoid code duplication:

```csharp
public class User
{
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    
    // Constructor 1: Minimal - delegates to full constructor
    public User() : this("Unknown", "unknown@example.com")
    {
    }
    
    // Constructor 2: Username only - delegates to full constructor
    public User(string username) : this(username, $"{username}@example.com")
    {
    }
    
    // Constructor 3: Full constructor - does the work
    public User(string username, string email)
    {
        Username = username;
        Email = email;
        IsActive = true;
    }
}

// Usage
var user1 = new User();                           // Default: "Unknown"
var user2 = new User("alice");                    // alice@example.com
var user3 = new User("bob", "bob@custom.com");    // Custom email
```

### Base Constructor Chaining with `base`

Call parent class constructor:

```csharp
public class Animal
{
    public string Name { get; set; }
    
    public Animal(string name)
    {
        Name = name;
        Console.WriteLine($"Animal created: {name}");
    }
}

public class Dog : Animal
{
    public string Breed { get; set; }
    
    // Call base (parent) constructor first
    public Dog(string name, string breed) : base(name)
    {
        Breed = breed;
        Console.WriteLine($"Dog created: {name}, {breed}");
    }
}

// Usage
var dog = new Dog("Buddy", "Golden Retriever");
// Output:
// Animal created: Buddy
// Dog created: Buddy, Golden Retriever
```

### Static Constructor

Runs once per class before first instance is created:

```csharp
public class Logger
{
    private static string _logFilePath;
    
    // Static constructor - runs once per class
    static Logger()
    {
        _logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
            "app.log");
        Console.WriteLine("Logger initialized");
    }
    
    public static void Log(string message)
    {
        File.AppendAllText(_logFilePath, $"{DateTime.Now}: {message}\n");
    }
}

// Usage
Logger.Log("First message");   // Static constructor runs here
Logger.Log("Second message");  // Static constructor doesn't run again
```

### Primary Constructor (C# 12.0+)

Simplified syntax for constructors:

```csharp
// Traditional way
public class PersonTraditional
{
    public string Name { get; }
    public int Age { get; }
    
    public PersonTraditional(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

// Primary constructor (C# 12.0+)
public class PersonNew(string name, int age)
{
    public string Name => name;
    public int Age => age;
}

// Usage
var person = new PersonNew("Alice", 30);
```

## Destructors (Finalizers)

### Basic Destructor

Destructor (~ClassName) runs when object is garbage collected:

```csharp
public class FileHandler
{
    private FileStream _file;
    
    public FileHandler(string path)
    {
        _file = new FileStream(path, FileMode.Open);
        Console.WriteLine("File opened");
    }
    
    // Destructor - runs when GC collects the object
    ~FileHandler()
    {
        Console.WriteLine("Destructor called");
        _file?.Close();  // Cleanup
    }
}

// Usage
var handler = new FileHandler("file.txt");
// When handler goes out of scope and GC runs, destructor is called
```

**Important:** Destructors run at unpredictable times. Use IDisposable for deterministic cleanup.

## IDisposable Pattern - Proper Resource Management

Use IDisposable for controlled cleanup:

```csharp
public class DatabaseConnection : IDisposable
{
    private SqlConnection _connection;
    private bool _disposed = false;
    
    public DatabaseConnection(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        _connection.Open();
    }
    
    public void ExecuteQuery(string query)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(DatabaseConnection));
        
        using (SqlCommand cmd = new SqlCommand(query, _connection))
        {
            cmd.ExecuteNonQuery();
        }
    }
    
    // Public Dispose - called by user or using statement
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);  // Tell GC to skip finalizer
    }
    
    // Protected virtual Dispose - allows subclasses to override
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Managed resources
                _connection?.Close();
                _connection?.Dispose();
            }
            // Unmanaged resources here (if any)
            _disposed = true;
        }
    }
    
    // Finalizer as safety net (rarely runs)
    ~DatabaseConnection()
    {
        Dispose(false);
    }
}

// Usage with using statement
using (var connection = new DatabaseConnection("connection-string"))
{
    connection.ExecuteQuery("SELECT * FROM Users");
}  // Dispose called automatically
```

### IAsyncDisposable (Async Cleanup)

For asynchronous resource cleanup:

```csharp
public class AsyncResource : IAsyncDisposable
{
    private HttpClient _client;
    
    public AsyncResource()
    {
        _client = new HttpClient();
    }
    
    public async Task<string> FetchAsync(string url)
    {
        return await _client.GetStringAsync(url);
    }
    
    public async ValueTask DisposeAsync()
    {
        _client?.Dispose();
        await Task.CompletedTask;
    }
}

// Usage with await using (C# 8.0+)
await using var resource = new AsyncResource();
string data = await resource.FetchAsync("https://api.example.com");
// DisposeAsync called automatically
```

## Initialization Order

Understanding the order things are initialized:

```csharp
public class InitializationDemo
{
    // 1. Static field initializer (first time only)
    private static string StaticValue = GetStaticValue();
    
    private static string GetStaticValue()
    {
        Console.WriteLine("1. Static field initializer");
        return "static";
    }
    
    // 2. Instance field initializer
    private string InstanceValue = GetInstanceValue();
    
    private string GetInstanceValue()
    {
        Console.WriteLine("2. Instance field initializer");
        return "instance";
    }
    
    // 3. Constructor
    public InitializationDemo()
    {
        Console.WriteLine("3. Constructor");
    }
}

// Usage
var obj = new InitializationDemo();
// Output:
// 1. Static field initializer (only first time)
// 2. Instance field initializer
// 3. Constructor
```

## Best Practices

### 1. Always Initialize Resources

```csharp
// Bad: Fields not initialized
public class Config
{
    public string ConnectionString { get; set; }
    public int Timeout { get; set; }
}

// Good: Initialize in constructor
public class Config
{
    public string ConnectionString { get; set; }
    public int Timeout { get; set; }
    
    public Config()
    {
        ConnectionString = "Default";
        Timeout = 30;
    }
}
```

### 2. Use Constructor Chaining

```csharp
// Good: Single source of truth
public class Settings
{
    public string Host { get; set; }
    public int Port { get; set; }
    
    public Settings() : this("localhost", 5432) { }
    public Settings(string host) : this(host, 5432) { }
    public Settings(string host, int port)
    {
        Host = host;
        Port = port;
    }
}
```

### 3. Avoid Heavy Logic in Constructors

```csharp
// Bad: Heavy I/O in constructor
public class UserService
{
    public UserService()
    {
        var users = FetchUsersFromDatabase();  // Slow!
    }
}

// Good: Use factory method for complex initialization
public class UserService
{
    public UserService() { }
    
    public static async Task<UserService> CreateAsync()
    {
        var service = new UserService();
        await service.InitializeAsync();
        return service;
    }
}
```

### 4. Implement IDisposable for Resources

```csharp
// Good: Proper resource management
public class FileReader : IDisposable
{
    private FileStream _file;
    private bool _disposed = false;
    
    public FileReader(string path)
    {
        _file = new FileStream(path, FileMode.Open);
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _file?.Dispose();
            }
            _disposed = true;
        }
    }
}
```

## Common Mistakes

### Mistake 1: Not Disposing Resources

```csharp
// Bad: Resource leak
public class FileReader
{
    private FileStream _file;
    
    public FileReader(string path)
    {
        _file = new FileStream(path, FileMode.Open);
    }
    // No Dispose - FileStream not properly closed!
}

// Good: Implement IDisposable
public class FileReader : IDisposable
{
    private FileStream _file;
    
    public void Dispose()
    {
        _file?.Dispose();
        GC.SuppressFinalize(this);
    }
}
```

### Mistake 2: Relying on Finalizer

```csharp
// Bad: Unpredictable cleanup
~Resource()
{
    _connection?.Close();  // May not run for a long time
}

// Good: Use IDisposable for deterministic cleanup
public void Dispose()
{
    _connection?.Close();
    GC.SuppressFinalize(this);
}
```

### Mistake 3: Forgetting GC.SuppressFinalize

```csharp
// Bad: Finalizer still runs after Dispose
public void Dispose()
{
    _resource?.Close();
    // Forgot GC.SuppressFinalize(this);
}

// Good: Suppress finalizer
public void Dispose()
{
    _resource?.Close();
    GC.SuppressFinalize(this);
}
```

## Summary

- **Constructor** - Initializes objects
- **Constructor overloading** - Multiple constructors
- **Constructor chaining** - Avoid duplication
- **Base constructor** - Call parent initialization
- **Static constructor** - Initialize class-level data
- **Destructor** - Cleanup (avoid, use IDisposable)
- **IDisposable** - Deterministic resource cleanup
- **Initialization order** - Static → Instance → Constructor
- **Best practice** - Always dispose resources

## Next Steps

- Study [Properties-Fields](../03-Properties-Fields/00-Properties-Fields.md) for data management
- Review [Inheritance](../../02-Inheritance-Polymorphism/01-Inheritance/00-Inheritance.md) for constructor inheritance
- Learn about [Access-Modifiers](../../03-Advanced-OOP/05-Access-Modifiers/00-Access-Modifiers.md)
