# Initialization Order

## Overview

Understanding the initialization order in C# is crucial for writing predictable code. When an object is created, C# follows a specific sequence: static fields, static constructor, instance fields, then instance constructor.

## Complete Initialization Sequence

The order of initialization is:

1. Static field initializers
2. Static constructor
3. Instance field initializers
4. Instance constructor

```csharp
public class InitializationDemo
{
    // 1. Static field initializer (first time only)
    private static string StaticField = GetStaticValue();
    
    private static string GetStaticValue()
    {
        Console.WriteLine("1. Static field initializer");
        return "static";
    }
    
    // 2. Static constructor (first time only)
    static InitializationDemo()
    {
        Console.WriteLine("2. Static constructor");
    }
    
    // 3. Instance field initializer (every time)
    private string InstanceField = GetInstanceValue();
    
    private string GetInstanceValue()
    {
        Console.WriteLine("3. Instance field initializer");
        return "instance";
    }
    
    // 4. Instance constructor (every time)
    public InitializationDemo()
    {
        Console.WriteLine("4. Instance constructor");
    }
}

// Usage
Console.WriteLine("Creating first instance:");
var obj1 = new InitializationDemo();
// Output:
// 1. Static field initializer (only this time)
// 2. Static constructor (only this time)
// 3. Instance field initializer
// 4. Instance constructor

Console.WriteLine("\nCreating second instance:");
var obj2 = new InitializationDemo();
// Output:
// 3. Instance field initializer (static parts skip)
// 4. Instance constructor
```

## Static Initialization (First Time Only)

Static initialization happens once:

```csharp
public class Database
{
    private static bool _connected = false;
    
    static Database()
    {
        Console.WriteLine("Connecting to database...");
        _connected = true;
    }
    
    public static void Query(string sql)
    {
        Console.WriteLine($"Executing: {sql}");
    }
}

// Usage
Database.Query("SELECT *");    // Static constructor runs here
Database.Query("SELECT COUNT"); // Static constructor doesn't run again
```

## Instance Initialization (Every Time)

Instance initialization happens for each object:

```csharp
public class Counter
{
    private int _count = 0;  // Instance field initializer
    
    public Counter()
    {
        Console.WriteLine($"Counter created with value: {_count}");
    }
}

// Usage
var c1 = new Counter();  // _count = 0, then constructor
var c2 = new Counter();  // _count = 0, then constructor (separate)
```

## Inheritance Initialization Order

With inheritance, the order includes base classes:

```csharp
public class Animal
{
    private string _name = InitializeName();
    
    private static string InitializeName()
    {
        Console.WriteLine("1. Animal field initializer");
        return "Animal";
    }
    
    public Animal()
    {
        Console.WriteLine("2. Animal constructor");
    }
}

public class Dog : Animal
{
    private string _breed = InitializeBreed();
    
    private static string InitializeBreed()
    {
        Console.WriteLine("3. Dog field initializer");
        return "Breed";
    }
    
    public Dog() : base()
    {
        Console.WriteLine("4. Dog constructor");
    }
}

// Usage
var dog = new Dog();
// Output:
// 1. Animal field initializer
// 2. Animal constructor
// 3. Dog field initializer
// 4. Dog constructor
```

## Practical Example: Settings Class

```csharp
public class AppSettings
{
    // Static fields - initialized once
    private static string _configPath = GetConfigPath();
    private static Dictionary<string, string> _settings;
    
    private static string GetConfigPath()
    {
        Console.WriteLine("Loading config path...");
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "app.config");
    }
    
    // Static constructor - runs once
    static AppSettings()
    {
        Console.WriteLine("Initializing app settings...");
        _settings = new Dictionary<string, string>
        {
            { "AppName", "MyApp" },
            { "Version", "1.0" }
        };
    }
    
    // Instance fields - initialized each time
    private DateTime _createdAt = DateTime.Now;
    
    // Instance constructor - runs each time
    public AppSettings()
    {
        Console.WriteLine($"Settings instance created at {_createdAt}");
    }
    
    public static string GetSetting(string key)
    {
        return _settings.ContainsKey(key) ? _settings[key] : null;
    }
    
    public DateTime CreatedAt => _createdAt;
}

// Usage
var settings1 = new AppSettings();  // Static init, then instance init
var settings2 = new AppSettings();  // Only instance init
```

## Common Mistakes

### Assuming Static Initialization Every Time

```csharp
// Bad - Assumes static constructor runs each time
public class Logger
{
    private static int _callCount = 0;
    
    static Logger()
    {
        _callCount = 0;  // Only resets first time!
    }
    
    public static void Log(string msg)
    {
        _callCount++;
        Console.WriteLine($"Call {_callCount}: {msg}");
    }
}

// Usage
Logger.Log("First");   // _callCount = 1, static constructor runs
Logger.Log("Second");  // _callCount = 2, static constructor doesn't run
```

### Initializing Instance Fields with Static Data

```csharp
// Bad - Shared reference!
public class ConfigBad
{
    private static List<string> _items = new();
    
    public List<string> Items = _items;  // All instances share same list!
}

var obj1 = new ConfigBad();
var obj2 = new ConfigBad();
obj1.Items.Add("A");
Console.WriteLine(obj2.Items.Count);  // 1! (should be 0)

// Good - Separate lists
public class ConfigGood
{
    private static List<string> _templates = new();
    
    public List<string> Items { get; } = new();  // Each instance has own list
}

var obj1 = new ConfigGood();
var obj2 = new ConfigGood();
obj1.Items.Add("A");
Console.WriteLine(obj2.Items.Count);  // 0 (correct)
```

## Best Practices

### Initialize Static Data in Static Constructor

```csharp
// Good - Clear initialization
public class Settings
{
    private static string _appName;
    
    static Settings()
    {
        _appName = "MyApp";
    }
}

// Less clear - Field initializer
public class SettingsLess
{
    private static string _appName = "MyApp";
}
```

### Keep Initialization Lightweight

```csharp
// Good - Quick initialization
static Database()
{
    _connectionString = "server=localhost";
}

// Bad - Heavy work in static constructor
static DatabaseBad()
{
    var data = LoadMillionRecordsFromDisk();  // Slow!
}
```

## Summary

- **Static fields** → Static constructor → Instance fields → Instance constructor
- **Static init** - Happens once per class
- **Instance init** - Happens each time `new` is called
- **Base classes** - Initialized before derived
- **Field initializers** - Run before constructors
- **Static data** - Shared across all instances
- **Instance data** - Separate for each instance

## Next Steps

- Learn [Instance-Constructors](../01-Instance-Constructors/00-Instance-Constructors.md) for object creation
- Study [Static-Constructors](../03-Static-Constructors/00-Static-Constructors.md) for class initialization
- Review [Destructors-IDisposable](../05-Destructors-IDisposable/00-Destructors-IDisposable.md) for cleanup
