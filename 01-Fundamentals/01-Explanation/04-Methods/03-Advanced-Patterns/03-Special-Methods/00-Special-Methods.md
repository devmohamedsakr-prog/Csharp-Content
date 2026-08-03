# Special Methods

## Overview

Special methods serve specific purposes in C#: constructors initialize objects, destructors clean up resources, and helper methods like TryParse follow established patterns.

## Constructors

Methods that run when an object is created:

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor - called when new Person() is created
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
        Console.WriteLine($"Person created: {name}");
    }
}

// Usage
Person person = new Person("Alice", 30);  // Constructor runs
```

## Default Constructor

If no constructor defined, default is created:

```csharp
public class SimpleClass
{
    // No constructor defined
    // Default constructor created automatically
}

// Usage
SimpleClass obj = new SimpleClass();  // Works with default constructor
```

## Multiple Constructors

Constructor overloading:

```csharp
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    
    // Constructor 1: Two parameters
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    // Constructor 2: One parameter (creates square point)
    public Point(int value)
    {
        X = value;
        Y = value;
    }
    
    // Constructor 3: No parameters (origin)
    public Point()
    {
        X = 0;
        Y = 0;
    }
}

// Usage
Point p1 = new Point(5, 10);     // Uses constructor 1
Point p2 = new Point(5);         // Uses constructor 2
Point p3 = new Point();          // Uses constructor 3
```

## Constructor Chaining

Calling one constructor from another:

```csharp
public class Rectangle
{
    public int Width { get; set; }
    public int Height { get; set; }
    
    // Primary constructor
    public Rectangle(int width, int height)
    {
        Width = width;
        Height = height;
    }
    
    // Simplified constructor calls primary
    public Rectangle(int size) : this(size, size)
    {
        // Calls Rectangle(int, int) first
    }
    
    // Default constructor calls primary
    public Rectangle() : this(0, 0)
    {
        // Calls Rectangle(int, int) first
    }
}

// Usage
Rectangle r1 = new Rectangle(10, 20);  // Uses primary
Rectangle r2 = new Rectangle(15);      // Calls Rectangle(15, 15)
Rectangle r3 = new Rectangle();        // Calls Rectangle(0, 0)
```

## Destructors

Methods that run when object is destroyed:

```csharp
public class FileHandle
{
    private string? filename;
    
    public FileHandle(string file)
    {
        filename = file;
        Console.WriteLine($"File {filename} opened");
    }
    
    // Destructor - called when object is garbage collected
    ~FileHandle()
    {
        Console.WriteLine($"File {filename} closed");
    }
}

// Usage
var handle = new FileHandle("data.txt");
// When handle goes out of scope and is garbage collected,
// destructor runs and prints "File data.txt closed"
```

## ref and out Parameters

Special parameter modifiers:

### ref - Pass by Reference

```csharp
public void Increment(ref int value)
{
    value++;  // Modifies original variable
}

// Usage
int x = 5;
Increment(ref x);
Console.WriteLine(x);  // 6 - changed
```

### out - Output Parameter

```csharp
public bool TryParseInt(string input, out int result)
{
    result = 0;  // Must assign before return
    
    if (int.TryParse(input, out int parsed))
    {
        result = parsed;
        return true;
    }
    return false;
}

// Usage
if (TryParseInt("42", out int value))
{
    Console.WriteLine(value);  // 42
}
```

### in - Pass by Reference (Read-Only)

```csharp
public void DisplayPoint(in Point p)
{
    // Can read p, but cannot modify
    Console.WriteLine($"({p.X}, {p.Y})");
    // p.X = 10;  // ERROR
}

// Usage
Point point = new Point(5, 10);
DisplayPoint(in point);
```

## TryParse Pattern

Standard pattern for safe parsing:

```csharp
// Usage of TryParse
string input = "42";
if (int.TryParse(input, out int number))
{
    Console.WriteLine($"Parsed: {number}");  // 42
}
else
{
    Console.WriteLine("Parse failed");
}

// Works with other types
if (double.TryParse("3.14", out double pi))
{
    Console.WriteLine(pi);  // 3.14
}

if (bool.TryParse("true", out bool flag))
{
    Console.WriteLine(flag);  // true
}

// String that can't parse
if (int.TryParse("abc", out int invalid))
{
    Console.WriteLine(invalid);
}
else
{
    Console.WriteLine("Failed to parse 'abc'");  // Prints this
}
```

## Creating TryParse Pattern

Implementing your own TryParse:

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // TryParse pattern
    public static bool TryParse(string data, out Person? result)
    {
        result = null;
        
        if (string.IsNullOrEmpty(data))
            return false;
        
        string[] parts = data.Split(',');
        if (parts.Length != 2)
            return false;
        
        if (!int.TryParse(parts[1], out int age))
            return false;
        
        result = new Person { Name = parts[0], Age = age };
        return true;
    }
}

// Usage
if (Person.TryParse("Alice,30", out Person? person))
{
    Console.WriteLine($"{person?.Name} is {person?.Age}");
}
```

## Operator Overloading

Special methods for operators:

```csharp
public class Vector
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Vector(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    // Overload + operator
    public static Vector operator +(Vector a, Vector b)
    {
        return new Vector(a.X + b.X, a.Y + b.Y);
    }
    
    // Overload - operator
    public static Vector operator -(Vector a, Vector b)
    {
        return new Vector(a.X - b.X, a.Y - b.Y);
    }
    
    // Overload == operator
    public static bool operator ==(Vector a, Vector b)
    {
        return a.X == b.X && a.Y == b.Y;
    }
    
    public static bool operator !=(Vector a, Vector b)
    {
        return !(a == b);
    }
}

// Usage
Vector v1 = new Vector(3, 4);
Vector v2 = new Vector(1, 2);
Vector sum = v1 + v2;        // (4, 6)
Vector diff = v1 - v2;       // (2, 2)
bool equal = v1 == v2;       // false
```

## Main Method

Program entry point:

```csharp
public class Program
{
    // Main method - where program starts
    public static void Main(string[] args)
    {
        Console.WriteLine("Program started");
        Console.WriteLine($"Arguments: {string.Join(", ", args)}");
    }
}

// Command line: dotnet run arg1 arg2
// Output: Arguments: arg1, arg2
```

## ToString Override

Special override method:

```csharp
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    // Override ToString to customize string representation
    public override string ToString()
    {
        return $"{Name}: ${Price:F2}";
    }
}

// Usage
Product p = new Product { Name = "Widget", Price = 9.99m };
Console.WriteLine(p);  // Output: Widget: $9.99
```

## GetHashCode and Equals

Special methods for object comparison:

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Override Equals for value comparison
    public override bool Equals(object? obj)
    {
        if (obj is Student other)
            return Id == other.Id && Name == other.Name;
        return false;
    }
    
    // Override GetHashCode for collections
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }
}

// Usage
Student s1 = new Student { Id = 1, Name = "Alice" };
Student s2 = new Student { Id = 1, Name = "Alice" };
Console.WriteLine(s1 == s2);        // false (different objects)
Console.WriteLine(s1.Equals(s2));   // true (same values)
```

## params Keyword

Variable number of parameters:

```csharp
public int Sum(params int[] numbers)
{
    int total = 0;
    foreach (int num in numbers)
        total += num;
    return total;
}

// Usage
Sum(1);               // 1
Sum(1, 2);            // 3
Sum(1, 2, 3, 4, 5);   // 15
```

## Extension Methods

Special static methods that look like instance methods:

```csharp
public static class StringExtensions
{
    // Extension method on string
    public static bool IsNumeric(this string str)
    {
        return !string.IsNullOrEmpty(str) && 
               str.All(c => char.IsDigit(c));
    }
    
    // Extension method that transforms
    public static string Repeat(this string str, int count)
    {
        return string.Concat(Enumerable.Repeat(str, count));
    }
}

// Usage - looks like instance method
string text = "12345";
bool isNum = text.IsNumeric();        // true
string repeated = "ab".Repeat(3);     // "ababab"
```

## Async Methods

Methods that return tasks:

```csharp
public async Task<string> FetchDataAsync()
{
    // Simulate async operation
    await Task.Delay(1000);
    return "Data loaded";
}

// Usage
var result = await FetchDataAsync();
Console.WriteLine(result);  // Prints after 1 second
```

## Summary

- **Constructors**: Initialize objects
- **Destructors**: Clean up resources
- **ref/out/in**: Parameter modifiers for references
- **TryParse**: Safe parsing pattern
- **Operators**: Overload +, -, ==, etc.
- **ToString**: Customize string representation
- **Equals/GetHashCode**: Object comparison
- **params**: Variable arguments
- **Extension methods**: Add methods to types
- **Async methods**: Return Task for async operations

## Next Steps

- Review [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) for special method guidelines
- Study interview questions on special methods
