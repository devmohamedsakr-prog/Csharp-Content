# Scope and Lifetime Best Practices

## 1. Keep Variable Scope as Narrow as Possible

### Principle
Declare variables as close as possible to where they're used. Narrow scope reduces cognitive load and prevents unintended modifications.

```csharp
// BAD: Wide scope
public int CalculateTotal(int[] numbers)
{
    int total = 0; // Declared at top
    int count = 0;
    int average = 0;
    
    // Hundreds of lines of code...
    
    for (int i = 0; i < numbers.Length; i++)
    {
        total += numbers[i];
        count++;
    }
    
    average = total / count;
    return total;
}

// GOOD: Narrow scope
public int CalculateTotal(int[] numbers)
{
    int total = 0;
    
    for (int i = 0; i < numbers.Length; i++)
    {
        total += numbers[i];
    }
    
    return total;
}

// BETTER: Inline where possible
public int CalculateTotal(int[] numbers)
{
    return numbers.Sum();
}
```

## 2. Use Access Modifiers Appropriately

### Principle
Start with the most restrictive access level and only broaden when necessary. Default to private.

```csharp
// BAD: Over-exposing
public class UserManager
{
    public List<User> _users = new(); // Public field!
    public string _password; // Public password!
    public int _failedAttempts; // Public internal state!
}

// GOOD: Proper encapsulation
public class UserManager
{
    private List<User> _users = new();
    private string _passwordHash;
    private int _failedAttempts;
    
    public IReadOnlyList<User> Users => _users.AsReadOnly();
    
    public bool Authenticate(string password)
    {
        // Validate password
        return VerifyPassword(password);
    }
    
    private bool VerifyPassword(string password)
    {
        // Implementation
        return true;
    }
}
```

## 3. Distinguish Class Members from Local Variables

### Principle
Use clear naming conventions to distinguish scope: prefix class fields, use meaningful names for locals.

```csharp
// BAD: Confusing naming
public class Processor
{
    private int value = 0;
    
    public void Process()
    {
        int value = 10; // Shadows class field - confusing!
        Console.WriteLine(value);
    }
}

// GOOD: Clear naming
public class Processor
{
    private int _cachedValue = 0;
    
    public void Process()
    {
        int processingValue = 10; // Clear, no shadowing
        Console.WriteLine(processingValue);
    }
}
```

## 4. Avoid Variable Shadowing

### Principle
Don't reuse variable names in nested scopes. Shadowing is confusing and error-prone.

```csharp
// BAD: Multiple levels of shadowing
public void Process()
{
    int value = 1;
    
    {
        int value = 2; // Shadows outer
        
        {
            int value = 3; // Shadows middle
            Console.WriteLine(value);
        }
    }
}

// GOOD: Distinct variable names
public void Process()
{
    int outerValue = 1;
    
    {
        int blockValue = 2;
        
        {
            int innerValue = 3;
            Console.WriteLine(innerValue);
        }
    }
}
```

## 5. Manage Object Lifetime with IDisposable

### Principle
Implement IDisposable for classes managing resources. Use 'using' statements for disposal.

```csharp
// BAD: No resource management
public class FileProcessor
{
    public void ProcessFile(string path)
    {
        var reader = File.OpenText(path);
        string content = reader.ReadToEnd();
        // reader never disposed - potential resource leak!
    }
}

// GOOD: Proper disposal
public class FileProcessor
{
    public void ProcessFile(string path)
    {
        using var reader = File.OpenText(path);
        string content = reader.ReadToEnd();
        // reader automatically disposed
    }
}

// BETTER: IDisposable implementation
public class DatabaseConnection : IDisposable
{
    private SqlConnection _connection;
    
    public DatabaseConnection(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
    }
    
    public void Dispose()
    {
        _connection?.Dispose();
    }
}
```

## 6. Avoid Unintended Variable Capture in Closures

### Principle
Be explicit about which variables you're capturing. Avoid capturing loop variables directly.

```csharp
// BAD: Loop variable capture
public List<Action> CreateActions()
{
    var actions = new List<Action>();
    
    for (int i = 0; i < 3; i++)
    {
        actions.Add(() => Console.WriteLine(i)); // All print 3
    }
    
    return actions;
}

// GOOD: Capture a copy
public List<Action> CreateActions()
{
    var actions = new List<Action>();
    
    for (int i = 0; i < 3; i++)
    {
        int copy = i; // Create local copy
        actions.Add(() => Console.WriteLine(copy));
    }
    
    return actions;
}

// BETTER: Use LINQ
public List<Action> CreateActions()
{
    return Enumerable.Range(0, 3)
        .Select(i => (Action)(() => Console.WriteLine(i)))
        .ToList();
}
```

## 7. Understand Stack vs Heap Memory

### Principle
Know where data lives: stack for value types, heap for reference types. Affects performance and lifetime.

```csharp
// Understanding allocation
public void MemoryAllocation()
{
    // Stack allocation (fast, automatic cleanup)
    int age = 30;
    DateTime date = DateTime.Now;
    
    // Heap allocation (slower allocation, GC cleanup)
    var person = new Person { Name = "Alice" };
    var items = new List<int>();
    
    // Local large allocations should be done carefully
    // Large structs copied on assignment - performance issue
    var largeStruct = new LargeStruct(); // Stack if local
}

// Avoid large value types
public struct LargeStruct // BAD if copying frequently
{
    public byte[] Data = new byte[1_000_000];
}

// Better: use class for large data
public class LargeData
{
    public byte[] Data = new byte[1_000_000];
}
```

## 8. Use Namespaces Effectively

### Principle
Organize code with clear namespace hierarchy. Avoid deep nesting. Match folder structure.

```csharp
// GOOD: Clear organization
namespace MyApp.Features.Users
{
    public class User { }
}

namespace MyApp.Features.Products
{
    public class Product { }
}

namespace MyApp.Infrastructure.Data
{
    public class Repository { }
}

// Avoid: Confusing or flat structure
namespace MyApp.U { } // Too short
namespace MyApp.A.B.C.D.E { } // Too deep
namespace MyApp { } // Too broad
```

## 9. Prevent Memory Leaks in Event Handlers

### Principle
Unsubscribe from events when done. Event handlers create closures that keep objects alive.

```csharp
// BAD: Event leak
public class EventSubscriber
{
    public void Subscribe(Publisher publisher)
    {
        publisher.OnData += (s, e) =>
        {
            Console.WriteLine(e.Data);
        };
        // Handler keeps this object alive!
    }
}

// GOOD: Unsubscribe
public class EventSubscriber : IDisposable
{
    private Publisher _publisher;
    
    public void Subscribe(Publisher publisher)
    {
        _publisher = publisher;
        _publisher.OnData += HandleData;
    }
    
    private void HandleData(object sender, EventArgs e)
    {
        Console.WriteLine("Data received");
    }
    
    public void Dispose()
    {
        _publisher.OnData -= HandleData; // Unsubscribe
    }
}
```

## 10. Use Modern C# Features

### Principle
Leverage newer C# features that handle scope and lifetime better.

```csharp
// GOOD: Using declaration (C# 8.0+)
public void ProcessFile()
{
    using var reader = File.OpenText("data.txt");
    string content = reader.ReadToEnd();
    // Disposed at method end
}

// GOOD: File-scoped namespace (C# 10.0+)
namespace MyApp.Features;

public class User { }
// Cleaner, no indentation needed

// GOOD: Global usings (C# 10.0+)
global using System;
global using System.Collections.Generic;
// Reduces repetition across files

// GOOD: Record types (C# 9.0+)
public record Person(string Name, int Age);
// Immutable by default, cleaner syntax
```

## Key Principles Summary

| Principle | Benefit |
|-----------|---------|
| Narrow scope | Easier to understand, less mental load |
| Restrictive access | Better encapsulation, fewer dependencies |
| Clear naming | Reduces confusion, easier to maintain |
| Avoid shadowing | Prevents bugs from variable conflicts |
| Proper disposal | Prevents resource leaks |
| Explicit capture | Prevents closure bugs |
| Understanding memory | Better performance and reliability |
| Organized namespaces | Easier navigation, clearer structure |
| Event unsubscribe | Prevents memory leaks |
| Modern features | Safer, cleaner code |

## Practical Example: Putting it All Together

```csharp
namespace MyApp.Features.DataProcessing;

using System;
using System.Collections.Generic;
using System.IO;

// Clear naming, proper access
public class DataProcessor : IDisposable
{
    private readonly List<string> _cache = new();
    private StreamWriter _logWriter;
    private bool _disposed = false;
    
    public DataProcessor(string logPath)
    {
        _logWriter = new StreamWriter(logPath, append: true);
    }
    
    // Narrow scope for local variables
    public void ProcessData(string[] data)
    {
        foreach (string item in data)
        {
            int processedValue = int.Parse(item);
            _cache.Add(processedValue.ToString());
        }
    }
    
    // IDisposable implementation
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (disposing)
        {
            _logWriter?.Dispose();
        }
        
        _disposed = true;
    }
    
    // Usage
    public static void Main()
    {
        using var processor = new DataProcessor("log.txt");
        processor.ProcessData(new[] { "1", "2", "3" });
        // Automatic disposal
    }
}
```

## Summary

Following these best practices leads to:
- **Clearer code**: Easier to understand and maintain
- **Fewer bugs**: Prevents scope-related errors
- **Better performance**: Proper memory management
- **More reliable**: Proper resource cleanup
- **Professional quality**: Industry-standard patterns

The key is consistency: apply these practices systematically across your codebase to build habits that become second nature.
