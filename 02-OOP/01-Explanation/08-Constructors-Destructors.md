# Constructors and Destructors

## Overview
Constructors initialize object instances, while destructors (finalizers) clean up resources before garbage collection.

## Constructors

### Basic Constructor
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor
    public Person()
    {
        Console.WriteLine("Default constructor called");
    }
    
    // Constructor with parameters
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
        Console.WriteLine($"Constructor called: {name}, {age}");
    }
}

// Usage
var person1 = new Person(); // Calls default constructor
var person2 = new Person("Alice", 30); // Calls parameterized constructor
```

### Constructor Overloading
```csharp
public class BankAccount
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    
    public BankAccount()
    {
        AccountNumber = Guid.NewGuid().ToString();
        Balance = 0;
    }
    
    public BankAccount(string accountNumber)
    {
        AccountNumber = accountNumber;
        Balance = 0;
    }
    
    public BankAccount(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }
}

// Usage
var account1 = new BankAccount();
var account2 = new BankAccount("ACC123");
var account3 = new BankAccount("ACC456", 1000);
```

### Constructor Chaining with 'this'
```csharp
public class User
{
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    
    // Default constructor
    public User() : this("Unknown", "unknown@example.com")
    {
    }
    
    // Constructor with username
    public User(string username) : this(username, $"{username}@example.com")
    {
    }
    
    // Main constructor with all parameters
    public User(string username, string email)
    {
        Username = username;
        Email = email;
        IsActive = true;
    }
}

// Usage
var user1 = new User();
var user2 = new User("alice");
var user3 = new User("bob", "bob@example.com");
```

### Base Constructor Chaining with 'base'
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
    
    // Call base constructor first
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
```csharp
public class Logger
{
    private static string _logFilePath;
    
    // Static constructor - runs once before first instance created
    static Logger()
    {
        _logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "app.log");
        Console.WriteLine("Logger initialized");
    }
    
    public static void Log(string message)
    {
        File.AppendAllText(_logFilePath, $"{DateTime.Now}: {message}\n");
    }
}

// Usage
Logger.Log("First message"); // Static constructor runs here
Logger.Log("Second message"); // Static constructor doesn't run again
```

## Destructors (Finalizers)

### Basic Destructor
```csharp
public class FileHandler
{
    private FileStream _file;
    
    public FileHandler(string path)
    {
        _file = new FileStream(path, FileMode.Open);
    }
    
    // Destructor
    ~FileHandler()
    {
        Console.WriteLine("Destructor called");
        _file?.Close(); // Cleanup
    }
}

// Usage
var handler = new FileHandler("file.txt");
// When handler goes out of scope and GC runs, destructor is called
```

## IDisposable Pattern

### Proper Resource Cleanup
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
    
    // Implement IDisposable
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Tell GC to skip finalizer
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _connection?.Close();
                _connection?.Dispose();
            }
            _disposed = true;
        }
    }
    
    // Finalizer as safety net
    ~DatabaseConnection()
    {
        Dispose(false);
    }
}

// Usage
using (var connection = new DatabaseConnection("connection-string"))
{
    connection.ExecuteQuery("SELECT * FROM Users");
} // Dispose called automatically
```

### IAsyncDisposable
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

// Usage
await using var resource = new AsyncResource();
string data = await resource.FetchAsync("https://api.example.com");
// DisposeAsync called automatically
```

## Initialization Order

### Object Creation Sequence
```csharp
public class InitializationDemo
{
    // 1. Static initializer
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
// 1. Static field initializer (first time only)
// 2. Instance field initializer
// 3. Constructor
```

## Primary Constructors (C# 12.0+)

### Simplified Syntax
```csharp
// Traditional way
public class Person
{
    public string Name { get; }
    public int Age { get; }
    
    public Person(string name, int age)
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

### Primary Constructor with Body
```csharp
public class Logger(string filePath)
{
    private readonly string _path = filePath;
    
    public Logger(string filePath, bool append) : this(filePath)
    {
        if (append)
            Console.WriteLine("Appending to existing log");
    }
    
    public void Log(string message)
    {
        File.AppendAllText(_path, message + "\n");
    }
}
```

## Best Practices

1. **Always Implement IDisposable for Unmanaged Resources**
```csharp
// Good: Proper resource management
public class SqlConnection : IDisposable
{
    private IntPtr _handle;
    private bool _disposed = false;
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Managed resources
            }
            // Unmanaged resources
            if (_handle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_handle);
            }
            _disposed = true;
        }
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
```

2. **Use Constructor Chaining to Avoid Duplication**
```csharp
// Good: Single source of truth
public class Config
{
    public string Host { get; set; }
    public int Port { get; set; }
    
    public Config() : this("localhost", 5432) { }
    public Config(string host) : this(host, 5432) { }
    public Config(string host, int port)
    {
        Host = host;
        Port = port;
    }
}
```

3. **Avoid Heavy Logic in Constructors**
```csharp
// Bad: Heavy I/O in constructor
public class UserService
{
    public UserService()
    {
        var users = FetchUsersFromDatabase(); // Slow!
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

## Common Mistakes

1. **Not Disposing Managed Resources**
```csharp
// Bad: Resource leak
public class FileReader
{
    private FileStream _file;
    
    public FileReader(string path)
    {
        _file = new FileStream(path, FileMode.Open);
    }
    // No Dispose - FileStream not closed!
}

// Good: Implement IDisposable
public class FileReader : IDisposable
{
    private FileStream _file;
    
    public FileReader(string path)
    {
        _file = new FileStream(path, FileMode.Open);
    }
    
    public void Dispose()
    {
        _file?.Dispose();
        GC.SuppressFinalize(this);
    }
}
```

2. **Relying on Destructor for Cleanup**
```csharp
// Bad: Destructors run at unpredictable time
~Resource()
{
    _connection?.Close(); // May run very late
}

// Good: Use IDisposable for deterministic cleanup
public void Dispose()
{
    _connection?.Close();
}
```

3. **Forgetting GC.SuppressFinalize**
```csharp
// Bad: Destructor still runs after Dispose
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

## Quick Summary
- Constructors initialize instances
- Constructor overloading for flexibility
- Constructor chaining with this and base
- Static constructor runs once per type
- Destructors are unpredictable - avoid relying
- IDisposable for deterministic cleanup
- Proper disposal pattern with Dispose pattern
- Avoid heavy logic in constructors
- Use factory methods for complex initialization
- Primary constructors (C# 12+) simplify code

## Resources
- Constructors documentation
- IDisposable Pattern
- Finalizers and Destructors
- Object initialization order
