# Static Members - Shared Class Data

## Overview

Static members (fields, methods, properties) belong to the class itself, not to individual instances. All instances share the same static members.

## Static Fields

```csharp
public class Counter
{
    // Instance field - each object has its own
    public int InstanceCount { get; set; }
    
    // Static field - shared by all instances
    public static int TotalCount { get; set; }
    
    public Counter()
    {
        InstanceCount = 0;
        TotalCount++;  // Shared counter
    }
}

// Usage
var c1 = new Counter();  // TotalCount = 1
var c2 = new Counter();  // TotalCount = 2
var c3 = new Counter();  // TotalCount = 3

Console.WriteLine(Counter.TotalCount);  // 3
Console.WriteLine(c1.InstanceCount);   // 0 (instance-level)
```

## Static Methods

```csharp
public class MathUtility
{
    // Static method - no instance needed
    public static int Add(int a, int b)
    {
        return a + b;
    }
    
    public static double Sqrt(double value)
    {
        return Math.Sqrt(value);
    }
}

// Usage - call on class, not instance
int result = MathUtility.Add(5, 3);
double root = MathUtility.Sqrt(16);

// No need to create instance
// var util = new MathUtility();  // Unnecessary
```

## Static Properties

```csharp
public class ApplicationConfig
{
    private static string _appName = "MyApp";
    private static int _version = 1;
    
    // Static property
    public static string AppName
    {
        get { return _appName; }
        set { _appName = value; }
    }
    
    public static int Version
    {
        get { return _version; }
        private set { _version = value; }  // Private setter
    }
}

// Usage
Console.WriteLine(ApplicationConfig.AppName);
ApplicationConfig.AppName = "NewApp";
```

## Static Constructor

```csharp
public class Configuration
{
    public static string ConnectionString { get; private set; }
    public static bool IsInitialized { get; private set; }
    
    // Static constructor - runs once before first use
    static Configuration()
    {
        Console.WriteLine("Configuration initializing");
        ConnectionString = GetConnectionStringFromConfig();
        IsInitialized = true;
    }
    
    private static string GetConnectionStringFromConfig()
    {
        return "Server=localhost;Database=MyDB";
    }
}

// Usage
// Static constructor runs here on first access
Console.WriteLine(Configuration.ConnectionString);
```

## Static Classes

Cannot be instantiated, only contain static members:

```csharp
// Static class - cannot instantiate
public static class Logger
{
    private static string _logFile = "app.log";
    
    public static void Log(string message)
    {
        File.AppendAllText(_logFile, $"{DateTime.Now}: {message}\n");
    }
    
    public static void ClearLog()
    {
        File.Delete(_logFile);
    }
}

// Usage
Logger.Log("Application started");
Logger.ClearLog();

// var logger = new Logger();  // ERROR - cannot instantiate
```

## Static vs Instance

```csharp
public class Employee
{
    // Instance - each employee has own name
    public string Name { get; set; }
    
    // Static - all employees share this count
    public static int TotalEmployees { get; private set; }
    
    public Employee(string name)
    {
        Name = name;
        TotalEmployees++;
    }
}

// Usage
var emp1 = new Employee("Alice");
var emp2 = new Employee("Bob");

Console.WriteLine(emp1.Name);              // Alice (instance)
Console.WriteLine(Employee.TotalEmployees); // 2 (class-level)
```

## Common Patterns

### Pattern 1: Singleton

```csharp
public class Database
{
    private static Database _instance;
    
    // Private constructor - prevent instantiation
    private Database() { }
    
    // Static property - single instance
    public static Database Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Database();
            return _instance;
        }
    }
}

// Usage
var db = Database.Instance;
var db2 = Database.Instance;  // Same instance
Console.WriteLine(ReferenceEquals(db, db2));  // true
```

### Pattern 2: Factory

```csharp
public class Logger
{
    private static Dictionary<string, Logger> _loggers = new();
    
    private Logger(string name) { }
    
    public static Logger GetLogger(string name)
    {
        if (!_loggers.ContainsKey(name))
            _loggers[name] = new Logger(name);
        return _loggers[name];
    }
}

// Usage
var log1 = Logger.GetLogger("App");
var log2 = Logger.GetLogger("App");  // Same instance
```

### Pattern 3: Utility Functions

```csharp
public static class StringHelper
{
    public static bool IsValidEmail(string email)
    {
        return email.Contains("@");
    }
    
    public static string Truncate(string text, int length)
    {
        return text.Length > length ? text.Substring(0, length) + "..." : text;
    }
}

// Usage
if (StringHelper.IsValidEmail(email))
{
    string shortened = StringHelper.Truncate(email, 10);
}
```

## Best Practices

### 1. Use Static for Stateless Operations

```csharp
// Good - No state
public static class Validator
{
    public static bool IsValidEmail(string email) => email.Contains("@");
}

// Bad - Static should not hold instance state
public static class BadHelper
{
    public static string _lastValue;  // Problematic
}
```

### 2. Prefer Instance Methods When Modifying State

```csharp
// Good - Instance method modifies object
public class Account
{
    public decimal Balance { get; private set; }
    
    public void Deposit(decimal amount)
    {
        Balance += amount;
    }
}

// Bad - Static method with instance state
public static class BadAccount
{
    public static decimal Balance;
    
    public static void Deposit(decimal amount)
    {
        Balance += amount;
    }
}
```

### 3. Document Static Usage

```csharp
/// <summary>
/// Static logger for application-wide logging.
/// Thread-safe but expensive - consider instance logger for performance.
/// </summary>
public static class AppLogger
{
    public static void Log(string message) { }
}
```

## Common Issues

### Issue 1: Testing Static Methods

```csharp
// Hard to test - depends on static state
public static class Processor
{
    public static int Count { get; set; }
    
    public static void Process()
    {
        Count++;
    }
}

// Better - testable instance method
public class Processor
{
    public int Count { get; set; }
    
    public void Process()
    {
        Count++;
    }
}
```

### Issue 2: Thread Safety

```csharp
// Not thread-safe
public static class Counter
{
    public static int Count { get; set; }  // Race conditions
}

// Better - thread-safe
public static class SafeCounter
{
    private static object _lock = new();
    private static int _count;
    
    public static void Increment()
    {
        lock (_lock)
        {
            _count++;
        }
    }
}
```

## Summary

- **Static** - Belongs to class, not instance
- **Static field** - Shared by all instances
- **Static method** - Call on class
- **Static constructor** - Runs once
- **Static class** - Only static members
- **Singleton** - Single shared instance
- **Utility functions** - Stateless operations

## Next Steps

- Learn [Access-Modifiers](../05-Access-Modifiers/00-Access-Modifiers.md)
- Study [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
