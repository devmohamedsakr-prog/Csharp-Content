# Static Constructors

## Overview

A static constructor initializes static members of a class. It runs automatically once, before any instances are created or static members are accessed, and is used to set up class-level data.

## What is a Static Constructor?

A static constructor:
- Runs once per class (first time only)
- Has no parameters or modifiers (always private)
- Cannot be called directly
- Initializes static fields and properties
- Runs before first use (instance creation or static member access)

```csharp
public class Logger
{
    private static string _logFilePath;
    
    // Static constructor - runs once automatically
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
Logger.Log("Second message");  // Static constructor already ran
// Output:
// Logger initialized
// (log entries written)
```

## Static vs Instance Constructors

| Aspect | Static Constructor | Instance Constructor |
|--------|-------------------|----------------------|
| Called | Once per class | Each time `new` is called |
| Parameters | None allowed | Can have parameters |
| Modifiers | None (implicit private) | public/private/protected |
| Triggers | First use of class | `new` keyword |
| Purpose | Class initialization | Instance initialization |

```csharp
public class Example
{
    private static int _staticCount;
    private int _instanceCount;
    
    // Static constructor - runs once
    static Example()
    {
        _staticCount = 0;
        Console.WriteLine("Static constructor");
    }
    
    // Instance constructor - runs each time
    public Example()
    {
        _instanceCount = 0;
        Console.WriteLine("Instance constructor");
    }
}

// Usage
var obj1 = new Example();
// Output:
// Static constructor (first time)
// Instance constructor

var obj2 = new Example();
// Output:
// Instance constructor (static doesn't run again)
```

## Common Use Cases

### Initialize Configuration

```csharp
public class AppSettings
{
    public static string AppName { get; private set; }
    public static string Version { get; private set; }
    public static string Environment { get; private set; }
    
    static AppSettings()
    {
        AppName = "MyApp";
        Version = "1.0.0";
        Environment = System.Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Development";
        Console.WriteLine($"App {AppName} v{Version} started in {Environment}");
    }
}

// Usage
var name = AppSettings.AppName;  // Static constructor runs here
```

### Initialize Static Collections

```csharp
public class ConstantValues
{
    public static Dictionary<string, string> ColorCodes { get; private set; }
    
    static ConstantValues()
    {
        ColorCodes = new Dictionary<string, string>
        {
            { "Red", "#FF0000" },
            { "Green", "#00FF00" },
            { "Blue", "#0000FF" }
        };
        Console.WriteLine("Color codes initialized");
    }
}

// Usage
var red = ConstantValues.ColorCodes["Red"];  // Static constructor runs
```

### Set Up Logging or Monitoring

```csharp
public class MonitoringService
{
    private static Timer _timer;
    
    static MonitoringService()
    {
        _timer = new Timer(OnTimerElapsed, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
        Console.WriteLine("Monitoring started");
    }
    
    private static void OnTimerElapsed(object state)
    {
        Console.WriteLine("Health check...");
    }
    
    public static void Stop()
    {
        _timer?.Dispose();
    }
}

// Usage
var check = MonitoringService.Stop;  // Static constructor runs
```

## Initialization Order

When you create the first instance:

```csharp
public class InitOrder
{
    // 1. Static field initializer
    private static string StaticField = GetStaticValue();
    
    private static string GetStaticValue()
    {
        Console.WriteLine("1. Static field initializer");
        return "static";
    }
    
    // 2. Static constructor
    static InitOrder()
    {
        Console.WriteLine("2. Static constructor");
    }
    
    // 3. Instance field initializer
    private string InstanceField = GetInstanceValue();
    
    private string GetInstanceValue()
    {
        Console.WriteLine("3. Instance field initializer");
        return "instance";
    }
    
    // 4. Instance constructor
    public InitOrder()
    {
        Console.WriteLine("4. Instance constructor");
    }
}

// Usage
var obj = new InitOrder();
// Output:
// 1. Static field initializer
// 2. Static constructor
// 3. Instance field initializer
// 4. Instance constructor
```

## Best Practices

### Initialize Only Static Members

```csharp
// Good - Only static work
public class GoodStatic
{
    private static int _counter;
    
    static GoodStatic()
    {
        _counter = 0;
    }
}

// Bad - Instance work in static constructor
public class BadStatic
{
    private int _value;
    
    static BadStatic()
    {
        _value = 0;  // Can't initialize instance member in static constructor
    }
}
```

### Keep it Lightweight

```csharp
// Good - Quick initialization
public class LightWeight
{
    private static string _name;
    
    static LightWeight()
    {
        _name = "App";  // Simple assignment
    }
}

// Bad - Heavy I/O in static constructor
public class Heavy
{
    private static string _data;
    
    static Heavy()
    {
        _data = ReadLargeFileFromDisk();  // Slow!
    }
}
```

## Summary

- **Static constructor** - Runs once per class
- **No parameters** - Cannot be defined
- **Implicit private** - Cannot be called directly
- **Class initialization** - Setup static data
- **Before first use** - Runs automatically
- **One per class** - No overloading allowed

## Next Steps

- Learn [Instance-Constructors](../01-Instance-Constructors/00-Instance-Constructors.md) for object initialization
- Study [Constructor-Chaining](../02-Constructor-Chaining/00-Constructor-Chaining.md) with base classes
- Review [Destructors-IDisposable](../05-Destructors-IDisposable/00-Destructors-IDisposable.md) for cleanup
