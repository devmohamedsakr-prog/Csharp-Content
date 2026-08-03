# Static Classes

## Overview

A static class is a class with only static members and cannot be instantiated. It's a container for utility functions, extension methods, and helper operations that don't require instance state.

## Static Class Definition

Mark class as static to make all members static:

```csharp
// Static class - cannot instantiate
public static class MathHelper
{
    // All members must be static
    public static double SquareRoot(double x)
    {
        return System.Math.Sqrt(x);
    }
    
    public static int Max(int a, int b)
    {
        return a > b ? a : b;
    }
}

// ERROR: Cannot instantiate static class
// var helper = new MathHelper();

// OK: Call static methods
int max = MathHelper.Max(5, 10);
```

## Static Class Requirements

Static classes:
- Cannot contain instance members
- Cannot be instantiated
- Cannot be inherited
- All members are implicitly static

```csharp
public static class Utilities
{
    // OK - static field
    public static int Counter = 0;
    
    // OK - static method
    public static void Log(string message)
    {
        Console.WriteLine(message);
    }
    
    // ERROR - instance members not allowed
    // public int InstanceField;
    // public void InstanceMethod() { }
}
```

## Common Use Cases

### String Utilities

```csharp
public static class StringHelper
{
    public static bool IsNullOrWhiteSpace(string text)
    {
        return string.IsNullOrWhiteSpace(text);
    }
    
    public static string Truncate(string text, int length)
    {
        if (text == null) return null;
        return text.Length > length ? text.Substring(0, length) + "..." : text;
    }
    
    public static string Reverse(string text)
    {
        var chars = text.ToCharArray();
        System.Array.Reverse(chars);
        return new string(chars);
    }
}

// Usage
string result = StringHelper.Truncate("Hello World", 5);  // "Hello..."
```

### Math Operations

```csharp
public static class MathOperations
{
    public static double Square(double x) => x * x;
    
    public static double Cube(double x) => x * x * x;
    
    public static bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (int i = 2; i < number / 2; i++)
            if (number % i == 0) return false;
        return true;
    }
}
```

### Validation

```csharp
public static class Validator
{
    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    
    public static bool IsValidPhone(string phone)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(
            phone, @"^\d{3}-\d{3}-\d{4}$"
        );
    }
}
```

### Extension Methods

Static classes enable extension methods:

```csharp
public static class StringExtensions
{
    // Extension method - extends string
    public static string CapitalizeWords(this string text)
    {
        return System.Globalization.CultureInfo.CurrentCulture
            .TextInfo.ToTitleCase(text.ToLower());
    }
    
    public static int WordCount(this string text)
    {
        return text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

// Usage - looks like string method
string text = "hello world".CapitalizeWords();  // "Hello World"
int count = text.WordCount();  // 2
```

### Factory Methods

```csharp
public static class DataFactory
{
    public static User CreateUser(string name)
    {
        return new User { Name = name, CreatedAt = DateTime.Now };
    }
    
    public static User CreateAdmin(string name)
    {
        return new User { Name = name, IsAdmin = true, CreatedAt = DateTime.Now };
    }
    
    public static List<User> CreateUsers(params string[] names)
    {
        return names.Select(n => CreateUser(n)).ToList();
    }
}

// Usage
var user = DataFactory.CreateUser("Alice");
var admin = DataFactory.CreateAdmin("Bob");
```

## Constants and Configuration

```csharp
public static class AppConstants
{
    public const string AppVersion = "1.0.0";
    public const int MaxRetries = 3;
    public const string DatabaseConnection = "Server=localhost;Database=MyApp";
    
    public static readonly DateTime StartupTime = DateTime.Now;
    public static readonly string[] SupportedFormats = { "pdf", "doc", "docx" };
}

// Usage
Console.WriteLine(AppConstants.AppVersion);  // "1.0.0"
if (AppConstants.SupportedFormats.Contains(extension))
    Process(file);
```

## Sealed vs Static

| Aspect | Static Class | Sealed Class |
|--------|-------------|-------------|
| Instantiate | No | Yes |
| Inherit | No | No |
| Instance members | No | Yes |
| Use case | Utilities | Prevent inheritance |

```csharp
// Static - no instances
public static class Utilities
{
    public static void DoWork() { }
}

// Sealed - can't inherit but can instantiate
public sealed class FinalClass
{
    public void DoWork() { }
}

var util = new Utilities();  // ERROR - can't instantiate
var final = new FinalClass(); // OK - can instantiate
```

## Best Practices

### Organize by Purpose

```csharp
// Good - organized by function
public static class StringHelpers { }
public static class DateHelpers { }
public static class ValidationHelpers { }

// Bad - mixed purposes
public static class HelpersBad { }  // What helpers?
```

### Make Methods Pure

```csharp
// Good - no side effects
public static class Math
{
    public static int Add(int a, int b) => a + b;
}

// Bad - side effects
public static class MathBad
{
    public static int AddAndLog(int a, int b)
    {
        Console.WriteLine("Adding");  // Side effect
        return a + b;
    }
}
```

### Use for Extension Methods

```csharp
// Perfect use - extends existing types
public static class CollectionExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> items)
    {
        return !items.Any();
    }
}

// Usage
List<int> numbers = new();
if (numbers.IsEmpty())
    Console.WriteLine("No numbers");
```

## Common Mistakes

### Putting State in Static Class

```csharp
// Bad - static state is shared
public static class CounterBad
{
    public static int Count = 0;
    
    public static void Increment() => Count++;
}

// Test 1: Counter.Count = 0; Counter.Increment(); // Count = 1
// Test 2: Counter.Increment(); // Count = 2 (affected by Test 1!)
```

### Too Many Responsibilities

```csharp
// Bad - too many unrelated utilities
public static class HelpersBad
{
    public static void ValidateEmail() { }
    public static void SendEmail() { }
    public static void CalculateDiscount() { }
    public static void ParseJson() { }
}

// Good - focused, organized
public static class EmailValidator { }
public static class EmailSender { }
public static class DiscountCalculator { }
```

## Summary

- **Static class** - Container for utility members
- **Cannot instantiate** - No `new` keyword
- **All members static** - No instance members
- **Common uses** - Utilities, extensions, factories
- **Extension methods** - Add methods to existing types
- **Constants** - Store configuration
- **Best for** - Stateless operations

## Next Steps

- Learn [Static-Members](../05-Static-Members/00-Static-Members.md) for class-level data
- Study [Access-Modifiers](../04-Access-Modifiers/00-Access-Modifiers.md) for visibility
- Review [Encapsulation](../03-Encapsulation/00-Encapsulation.md) for data protection
