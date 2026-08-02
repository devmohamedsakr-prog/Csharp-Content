# Static Members

## Overview
Static members belong to the class itself, not to individual instances.

---

## Static vs Instance Members

```csharp
public class Counter {
    // Instance member - unique per object
    public int InstanceCount { get; set; }
    
    // Static member - shared by all objects
    public static int TotalCount = 0;
}

Counter c1 = new Counter();
Counter c2 = new Counter();

c1.InstanceCount = 10;
c2.InstanceCount = 20;

c1.InstanceCount != c2.InstanceCount;  // Different values

Counter.TotalCount = 100;  // Shared
// Both c1 and c2 see TotalCount as 100
```

---

## Static Fields

Shared across all instances.

```csharp
public class User {
    private static int nextId = 1;  // Shared by all users
    
    public int Id { get; private set; }
    public string Name { get; set; }
    
    public User(string name) {
        Name = name;
        Id = nextId++;  // Use shared field
    }
}

User user1 = new User("Alice");  // Id = 1
User user2 = new User("Bob");    // Id = 2
User user3 = new User("Charlie");// Id = 3

Console.WriteLine(user1.Id);  // 1
Console.WriteLine(user2.Id);  // 2
Console.WriteLine(user3.Id);  // 3
Console.WriteLine(User.nextId);  // 4
```

---

## Static Methods

Methods that operate on class, not instances.

```csharp
public class Math2 {
    // Static method
    public static int Add(int a, int b) {
        return a + b;
    }
    
    public static int Multiply(int a, int b) {
        return a * b;
    }
    
    // Instance method
    public int Square(int x) {
        return x * x;
    }
}

// Call static methods on class
int sum = Math2.Add(5, 3);  // 8
int product = Math2.Multiply(5, 3);  // 15

// Cannot call static method on instance
// Math2 math = new Math2();
// int x = math.Add(5, 3);  // Error - static method

// Can call instance method on instance
Math2 math = new Math2();
int squared = math.Square(5);  // 25
```

---

## Static Properties

```csharp
public class Configuration {
    private static string _connectionString;
    
    public static string ConnectionString {
        get { return _connectionString; }
        set { _connectionString = value; }
    }
}

Configuration.ConnectionString = "Server=localhost;Database=MyDb";
string conn = Configuration.ConnectionString;
```

---

## Static Constructors

Run once when class is first used.

```csharp
public class Database {
    private static string connectionString;
    private static bool isInitialized;
    
    // Static constructor - runs once
    static Database() {
        Console.WriteLine("Database class initialized");
        connectionString = "Server=localhost";
        isInitialized = true;
    }
    
    // Instance constructor
    public Database() {
        Console.WriteLine("Database instance created");
    }
}

// First usage
Database db1 = new Database();
// Output:
// Database class initialized
// Database instance created

// Subsequent usage
Database db2 = new Database();
// Output:
// Database instance created
// (static constructor NOT called again)
```

---

## Static Classes

Class with only static members.

```csharp
// Static class - cannot be instantiated
public static class MathUtilities {
    public static double Pi = 3.14159;
    
    public static double CircleArea(double radius) {
        return Pi * radius * radius;
    }
    
    public static double CircleCircumference(double radius) {
        return 2 * Pi * radius;
    }
}

// Cannot create instance
// MathUtilities math = new MathUtilities();  // Error

// Use directly from class
double area = MathUtilities.CircleArea(5);
double circumference = MathUtilities.CircleCircumference(5);
```

---

## Real-World Examples

### Application Settings

```csharp
public class AppSettings {
    public static string DatabaseConnection { get; set; }
    public static string ApiKey { get; set; }
    public static int Timeout { get; set; }
    
    static AppSettings() {
        DatabaseConnection = "Server=localhost;Database=MyDb";
        ApiKey = "secret-key-123";
        Timeout = 30;
    }
}

// Usage
string connStr = AppSettings.DatabaseConnection;
string apiKey = AppSettings.ApiKey;
```

### Logger

```csharp
public static class Logger {
    private static List<string> logs = new List<string>();
    
    public static void Log(string message) {
        logs.Add($"[{DateTime.Now}] {message}");
        Console.WriteLine(message);
    }
    
    public static void ShowAllLogs() {
        foreach (var log in logs) {
            Console.WriteLine(log);
        }
    }
}

Logger.Log("Application started");
Logger.Log("User logged in");
Logger.ShowAllLogs();
```

### Singleton Pattern

```csharp
public class Database {
    private static Database instance;
    private static readonly object lockObject = new object();
    
    private string connectionString;
    
    // Private constructor - cannot instantiate from outside
    private Database() {
        connectionString = "Server=localhost;Database=MyDb";
    }
    
    // Static property for single instance
    public static Database Instance {
        get {
            if (instance == null) {
                lock (lockObject) {
                    if (instance == null) {
                        instance = new Database();
                    }
                }
            }
            return instance;
        }
    }
    
    public void Connect() {
        Console.WriteLine($"Connecting to {connectionString}");
    }
}

// Usage
Database.Instance.Connect();
Database.Instance.Connect();  // Same instance
```

---

## Static Inheritance

Static members are not inherited.

```csharp
public class Parent {
    public static void StaticMethod() {
        Console.WriteLine("Parent static");
    }
}

public class Child : Parent {
    // Does NOT override parent's static method
}

Parent.StaticMethod();  // "Parent static"
Child.StaticMethod();  // "Parent static" - inherited from parent

// Each class has its own static version
public class ChildWithStatic : Parent {
    public new static void StaticMethod() {
        Console.WriteLine("Child static");
    }
}

ChildWithStatic.StaticMethod();  // "Child static"
```

---

## Best Practices

✓ **Use static for utility methods**
```csharp
// Good - static utility class
public static class StringUtilities {
    public static bool IsValidEmail(string email) {
        return email.Contains("@");
    }
}

bool valid = StringUtilities.IsValidEmail("user@example.com");
```

✓ **Use static for shared configuration**
```csharp
// Good - shared application settings
public static class AppConfig {
    public static string Environment { get; set; }
    public static int MaxConnections { get; set; }
}
```

✓ **Be careful with mutable static state**
```csharp
// Risky - mutable static state
public static class Counter {
    public static int Count = 0;  // Can be modified from anywhere
}

// Better - encapsulated static
public class Counter {
    private static int count = 0;
    
    public static void Increment() {
        count++;
    }
    
    public static int GetCount() {
        return count;
    }
}
```

---

## Common Mistakes

❌ **Forgetting static keyword when needed**
```csharp
public class Math {
    public int Add(int a, int b) {  // Instance method - need new Math()
        return a + b;
    }
}

int result = Math.Add(5, 3);  // Error - need instance
```

✓ **Mark as static**
```csharp
public class Math {
    public static int Add(int a, int b) {  // Static method
        return a + b;
    }
}

int result = Math.Add(5, 3);  // OK
```

❌ **Mutable shared state**
```csharp
public static class GlobalState {
    public static string CurrentUser = "";  // Problematic
    public static List<Data> Cache = new List<Data>();  // Thread issues
}
```

✓ **Immutable or synchronized**
```csharp
public static class AppSettings {
    public static string Environment { get; } = "Production";  // Immutable
    
    private static readonly object lockObj = new object();
    public static void UpdateCache(Data data) {
        lock (lockObj) {  // Thread-safe
            cache.Add(data);
        }
    }
}
```

---

## Quick Summary

- Static members belong to class, not instances
- Shared across all instances
- Access via ClassName.Member
- Static methods useful for utilities
- Static classes cannot be instantiated
- Static constructors run once
- Careful with mutable static state
- Thread safety important with static
