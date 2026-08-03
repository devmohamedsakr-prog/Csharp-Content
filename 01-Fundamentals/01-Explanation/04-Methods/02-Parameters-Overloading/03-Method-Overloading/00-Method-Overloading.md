# Method Overloading

## Overview

Method overloading allows multiple methods with the same name but different parameters.

## What is Overloading?

Same method name, different parameter lists:

```csharp
public class Calculator
{
    // Overload 1: Two integers
    public int Add(int a, int b)
    {
        return a + b;
    }
    
    // Overload 2: Three integers
    public int Add(int a, int b, int c)
    {
        return a + b + c;
    }
    
    // Overload 3: Two doubles
    public double Add(double a, double b)
    {
        return a + b;
    }
}

// Usage - compiler picks correct overload
Calculator calc = new Calculator();
calc.Add(5, 3);           // Calls overload 1 (int, int)
calc.Add(5, 3, 2);        // Calls overload 2 (int, int, int)
calc.Add(5.5, 3.2);       // Calls overload 3 (double, double)
```

## Overloading by Parameter Count

Different number of parameters:

```csharp
public class Logger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
    
    public void Log(string message, string level)
    {
        Console.WriteLine($"[{level}] {message}");
    }
    
    public void Log(string message, string level, DateTime timestamp)
    {
        Console.WriteLine($"[{timestamp}] [{level}] {message}");
    }
}

// Usage
Logger logger = new Logger();
logger.Log("Error occurred");
logger.Log("Warning detected", "WARN");
logger.Log("Info message", "INFO", DateTime.Now);
```

## Overloading by Parameter Type

Different parameter types:

```csharp
public class Printer
{
    public void Print(int value)
    {
        Console.WriteLine($"Integer: {value}");
    }
    
    public void Print(string value)
    {
        Console.WriteLine($"String: {value}");
    }
    
    public void Print(double value)
    {
        Console.WriteLine($"Double: {value}");
    }
    
    public void Print(bool value)
    {
        Console.WriteLine($"Boolean: {value}");
    }
}

// Usage
Printer printer = new Printer();
printer.Print(42);         // Calls int version
printer.Print("Hello");    // Calls string version
printer.Print(3.14);       // Calls double version
printer.Print(true);       // Calls bool version
```

## Overloading by Parameter Type Combination

Different parameter combinations:

```csharp
public class DataProcessor
{
    public void Process(int[] data) { }
    public void Process(string[] data) { }
    public void Process(List<int> data) { }
    public void Process(Dictionary<string, int> data) { }
    public void Process(int x, int y) { }
    public void Process(string x, string y) { }
}
```

## Complex Overloading Example

```csharp
public class UserManager
{
    // Overload 1: Just name
    public void CreateUser(string name)
    {
        CreateUser(name, "user@example.com");
    }
    
    // Overload 2: Name and email
    public void CreateUser(string name, string email)
    {
        CreateUser(name, email, 18);
    }
    
    // Overload 3: Name, email, and age
    public void CreateUser(string name, string email, int age)
    {
        Console.WriteLine($"Created: {name}, {email}, {age}");
    }
    
    // Overload 4: Different types
    public void CreateUser(int id, string name)
    {
        Console.WriteLine($"ID: {id}, Name: {name}");
    }
}

// Usage
UserManager manager = new UserManager();
manager.CreateUser("Alice");
manager.CreateUser("Bob", "bob@example.com");
manager.CreateUser("Charlie", "charlie@example.com", 30);
manager.CreateUser(1, "Diana");
```

## Overloading Rules

### Valid Overloads

Differ in parameter type or count:

```csharp
public class Valid
{
    public void Method(int x) { }
    public void Method(int x, int y) { }      // Different count
    public void Method(string x) { }          // Different type
    public void Method(int[] x) { }           // Different type
    public void Method(List<int> x) { }       // Different type
    public void Method(int x, string y) { }   // Different combination
}
```

### Invalid Overloads

Cannot differ only by return type:

```csharp
public class Invalid
{
    public int Method(int x) { return x; }
    
    // ERROR - same parameters, different return type
    // public string Method(int x) { return x.ToString(); }
    
    // ERROR - return type not considered for overloading
    // public double Method(int x) { return x; }
}
```

## Implicit Overloading

Sometimes overloading happens implicitly:

```csharp
public class ImplicitOverload
{
    // These are different methods due to inheritance
    public void Print(object obj) { }
    public void Print(string str) { }
    
    // When you call Print with string, it uses the string version (more specific)
}

ImplicitOverload obj = new ImplicitOverload();
obj.Print("text");           // Calls Print(string) - more specific
obj.Print((object)"text");   // Calls Print(object) - explicit cast
```

## Overloading with Generics

```csharp
public class GenericExample
{
    public void Process<T>(T value) { }
    public void Process(int value) { }
    public void Process<T>(T[] values) { }
}

// Usage
GenericExample example = new GenericExample();
example.Process("text");           // Generic version
example.Process(42);               // int version (more specific)
example.Process(new[] { 1, 2, 3 }); // Array version
```

## Overloading with Defaults

Combine overloading and defaults:

```csharp
public class MixedApproach
{
    // Method 1
    public void Report(string title)
    {
        Report(title, 10, "PDF");
    }
    
    // Method 2 - overload with defaults
    public void Report(string title, int pages = 10, string format = "PDF")
    {
        Console.WriteLine($"{title}, {pages} pages, {format}");
    }
}

MixedApproach ap = new MixedApproach();
ap.Report("Sales");
ap.Report("Sales", 20);
ap.Report("Sales", 20, "Excel");
```

## Best Practices

### Do Overload When

```csharp
// Good - related operations
public int Calculate(int a, int b) => a + b;
public double Calculate(double a, double b) => a + b;
public string Calculate(string a, string b) => a + b;
```

### Don't Overload When

```csharp
// Bad - different behavior
public void Save(string filename) => /* Save as file */
public void Save(Stream stream) => /* Upload to server */
// These have different purposes - use different names
```

### Use Meaningful Overloads

```csharp
// Good - clear variants
public void Log(string message)
public void Log(string message, LogLevel level)
public void Log(Exception ex)

// Bad - confusing
public void Process(int x)
public void Process(int x, int y)
public void Process(int x, int y, int z)
// Use arrays or collections instead for many similar parameters
```

## Common Overloading Patterns

### Pattern 1: Progressive Methods

```csharp
public class Report
{
    public void Generate()
    {
        Generate(10, "PDF");
    }
    
    public void Generate(int pageSize)
    {
        Generate(pageSize, "PDF");
    }
    
    public void Generate(int pageSize, string format)
    {
        // Implementation
    }
}
```

### Pattern 2: Type Variants

```csharp
public class Converter
{
    public string ToString(int value) => value.ToString();
    public string ToString(double value) => value.ToString();
    public string ToString(bool value) => value.ToString();
}
```

### Pattern 3: Collection Variants

```csharp
public class Processor
{
    public void Process(int item) { }
    public void Process(int[] items) { }
    public void Process(List<int> items) { }
    public void Process(IEnumerable<int> items) { }
}
```

## Overload Resolution

Compiler chooses most specific overload:

```csharp
public class Resolution
{
    public void Method(object obj) { }
    public void Method(string str) { }
    public void Method(int num) { }
}

Resolution r = new Resolution();
r.Method("text");    // Calls Method(string) - most specific
r.Method(42);        // Calls Method(int) - most specific
r.Method(3.14);      // Calls Method(object) - widening conversion
```

## Summary

- **Same name, different parameters**
- **Differ by**: parameter count, type, or combination
- **Cannot differ by**: return type alone
- **Compiler picks**: most specific overload
- **Best practice**: overload for related operations
- **Common patterns**: progressive methods, type variants, collection variants

## Next Steps

- Review [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) for overloading guidelines
- Study [Common-Mistakes](../../04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md) for common pitfalls
