# Static Members

## Overview

Static members belong to the class itself, not individual instances. All instances share the same static data. Use static for class-level data and operations that don't require instance state.

## Static vs Instance Members

| Aspect | Static | Instance |
|--------|--------|----------|
| Belongs to | Class | Object |
| Shared | All instances | One object |
| Accessed via | Class name | Object reference |
| Memory | Once per class | Once per instance |

```csharp
public class Counter
{
    public static int StaticCount;  // Shared by all instances
    public int InstanceCount;       // Unique per instance
}

// Usage
Counter.StaticCount = 10;    // Class access
var c1 = new Counter();
c1.InstanceCount = 5;        // Instance access
var c2 = new Counter();
c2.InstanceCount = 3;        // Separate from c1

Console.WriteLine(Counter.StaticCount);  // 10 - shared
Console.WriteLine(c1.InstanceCount);     // 5 - separate
Console.WriteLine(c2.InstanceCount);     // 3 - separate
```

## Static Fields

Class-level data shared by all instances:

```csharp
public class Logger
{
    private static int _totalLogs = 0;
    
    public static void Log(string message)
    {
        _totalLogs++;
        Console.WriteLine($"[{_totalLogs}] {message}");
    }
    
    public static int GetTotalLogs()
    {
        return _totalLogs;
    }
}

// Usage
Logger.Log("First");      // [1]
Logger.Log("Second");     // [2]
Console.WriteLine(Logger.GetTotalLogs());  // 2
```

## Static Methods

Methods that don't need instance data:

```csharp
public class Math Utilities
{
    public static double Sqrt(double x)
    {
        return System.Math.Sqrt(x);
    }
    
    public static int Max(int a, int b)
    {
        return a > b ? a : b;
    }
}

// Usage - call on class, not instance
int max = MathUtilities.Max(5, 10);
double sqrt = MathUtilities.Sqrt(16);
```

## Static Properties

Class-level properties:

```csharp
public class Configuration
{
    private static string _appName = "MyApp";
    private static int _version = 1;
    
    public static string AppName
    {
        get { return _appName; }
        set { _appName = value; }
    }
    
    public static int Version
    {
        get { return _version; }
    }
}

// Usage
Console.WriteLine(Configuration.AppName);  // MyApp
Configuration.AppName = "NewApp";
```

## Static vs Instance - When to Use

Use static for:
- Utility functions
- Constants or configuration
- Shared counters or state
- Factory methods

Use instance for:
- Data unique to object
- Behavior dependent on state
- Most business logic

```csharp
public class User
{
    // Static - shared, utility
    public static User CreateAdmin(string name)
    {
        return new User { Name = name, IsAdmin = true };
    }
    
    // Instance - unique per user
    public string Name { get; set; }
    public bool IsAdmin { get; set; }
}

// Usage
var admin = User.CreateAdmin("Alice");  // Static factory
var user = new User { Name = "Bob" };   // Instance creation
```

## Static Collections

Share collections across instances:

```csharp
public class Application
{
    private static List<User> _allUsers = new();
    
    public static void RegisterUser(User user)
    {
        _allUsers.Add(user);
    }
    
    public static int UserCount
    {
        get { return _allUsers.Count; }
    }
}

// Usage
Application.RegisterUser(new User { Name = "Alice" });
Application.RegisterUser(new User { Name = "Bob" });
Console.WriteLine(Application.UserCount);  // 2
```

## Cautions with Static

### Tight Coupling

```csharp
// Bad - tight coupling to static
public class RepositoryBad
{
    public void Save()
    {
        Logger.Log("Saving");  // Depends on static Logger
    }
}

// Good - inject dependency
public interface ILogger
{
    void Log(string message);
}

public class RepositoryGood
{
    private ILogger _logger;
    
    public RepositoryGood(ILogger logger)
    {
        _logger = logger;
    }
    
    public void Save()
    {
        _logger.Log("Saving");  // Flexible
    }
}
```

### Testing Challenges

```csharp
// Hard to test - static state persists
public class Counter
{
    public static int Count;
}

// Test 1
Counter.Count = 0;
Counter.Count++;
Assert.AreEqual(1, Counter.Count);

// Test 2 - affected by Test 1!
Counter.Count++;
Assert.AreEqual(1, Counter.Count);  // Fails!
```

## Common Static Patterns

### Singleton Pattern
```csharp
public class Database
{
    private static Database _instance;
    
    public static Database Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Database();
            return _instance;
        }
    }
    
    private Database() { }
}
```

### Utility Classes
```csharp
public static class StringHelper
{
    public static bool IsValidEmail(string email)
    {
        return email.Contains("@");
    }
    
    public static string Truncate(string text, int length)
    {
        return text.Length > length ? text.Substring(0, length) : text;
    }
}
```

## Best Practices

### Avoid Excessive Static

```csharp
// Too much static - tightly coupled
public class ControllerBad
{
    public void Process()
    {
        Logger.Log("Processing");
        Database.SaveData();
        EmailService.Send();
    }
}

// Better - inject dependencies
public class ControllerGood
{
    private ILogger _logger;
    private IDatabase _database;
    private IEmailService _email;
}
```

### Use Static Classes for Utilities

```csharp
public static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        return collection == null || collection.Count() == 0;
    }
}
```

## Summary

- **Static** - Belongs to class, shared by all instances
- **Instance** - Belongs to object, unique per instance
- **Static fields** - Class-level data
- **Static methods** - Utility functions
- **Static properties** - Class-level properties
- **When to use** - Utilities, factories, constants
- **Cautions** - Coupling, testing difficulty

## Next Steps

- Learn [Static-Classes](../06-Static-Classes/00-Static-Classes.md) for utility classes
- Study [Access-Modifiers](../04-Access-Modifiers/00-Access-Modifiers.md) for visibility
- Review [Encapsulation](../03-Encapsulation/00-Encapsulation.md) for hiding state
