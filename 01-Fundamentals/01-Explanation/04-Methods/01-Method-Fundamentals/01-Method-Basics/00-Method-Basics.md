# Method Basics

## Overview

Methods are blocks of code that perform specific tasks. They're fundamental building blocks for organizing code into reusable, maintainable units.

## What is a Method?

A method is a collection of statements that performs a specific task and can be reused throughout a program.

```csharp
// Simple method
public void Greet()
{
    Console.WriteLine("Hello, World!");
}

// Method with parameters
public void Greet(string name)
{
    Console.WriteLine($"Hello, {name}!");
}

// Method with return value
public int Add(int a, int b)
{
    return a + b;
}
```

## Method Structure

Every method has specific components:

```csharp
public int Add(int a, int b)
{
    return a + b;
}

// Components:
// public - Access modifier (visibility)
// int - Return type (what the method returns)
// Add - Method name (what you call it)
// (int a, int b) - Parameters (what it takes in)
// { return a + b; } - Body (what it does)
```

### Access Modifiers

Controls visibility of the method:

```csharp
public int PublicMethod()      // Accessible everywhere
{
    return 42;
}

private int PrivateMethod()    // Accessible only within class
{
    return 42;
}

protected int ProtectedMethod() // Accessible in class and derived classes
{
    return 42;
}

internal int InternalMethod()  // Accessible within same assembly
{
    return 42;
}
```

### Return Types

Specifies what the method returns:

```csharp
public void NoReturn()         // Returns nothing
{
    Console.WriteLine("Just prints");
}

public int ReturnInt()         // Returns integer
{
    return 42;
}

public string ReturnString()   // Returns string
{
    return "Hello";
}

public bool ReturnBool()       // Returns boolean
{
    return true;
}

public double ReturnDouble()   // Returns double
{
    return 3.14;
}

public object ReturnObject()   // Returns any object
{
    return new object();
}
```

### Method Names

Convention: PascalCase (first letter capitalized, each word capitalized)

```csharp
// GOOD: Clear, descriptive names
public void PrintMessage() { }
public bool IsPositive(int number) { }
public string GetFullName(string first, string last) { }
public void CalculateTotalPrice() { }

// BAD: Unclear, abbreviated
public void Print() { }
public bool Check(int n) { }
public string GetName(string f, string l) { }
public void Calc() { }
```

## Simple Method Examples

### Example 1: Void Method (No Return)

```csharp
public class Printer
{
    public void PrintHello()
    {
        Console.WriteLine("Hello!");
    }
    
    public void PrintLine(string message)
    {
        Console.WriteLine(message);
    }
}

// Usage
Printer printer = new Printer();
printer.PrintHello();           // Output: Hello!
printer.PrintLine("Test");      // Output: Test
```

### Example 2: Method with Return Value

```csharp
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
    
    public double Average(int[] numbers)
    {
        int sum = 0;
        foreach (int num in numbers)
        {
            sum += num;
        }
        return (double)sum / numbers.Length;
    }
}

// Usage
Calculator calc = new Calculator();
int result = calc.Add(5, 3);    // result = 8
double avg = calc.Average(new int[] { 10, 20, 30 }); // avg = 20
```

### Example 3: Method with Multiple Parameters

```csharp
public class Person
{
    public void DisplayInfo(string name, int age, string city)
    {
        Console.WriteLine($"{name}, {age} years old, from {city}");
    }
    
    public bool IsAdult(int age)
    {
        return age >= 18;
    }
}

// Usage
Person person = new Person();
person.DisplayInfo("Alice", 30, "New York");  // Output: Alice, 30 years old, from New York
bool adult = person.IsAdult(25);              // adult = true
```

## Method Invocation

Calling a method:

```csharp
public class Example
{
    public void SimpleMethod()
    {
        Console.WriteLine("This runs");
    }
    
    public int Calculate(int x)
    {
        return x * 2;
    }
}

// Call method
Example example = new Example();
example.SimpleMethod();         // Invoke void method
int result = example.Calculate(5); // Invoke method with return value
```

## Return Statement

Exit method and return a value (or exit void method):

```csharp
public class Validator
{
    public bool IsPositive(int number)
    {
        if (number > 0)
        {
            return true;  // Exit here
        }
        return false;     // Exit here (default)
    }
    
    public int GetFirstPositive(int[] numbers)
    {
        foreach (int num in numbers)
        {
            if (num > 0)
            {
                return num;  // Exit with value
            }
        }
        return 0;  // Default if none found
    }
    
    public void PrintUntilNegative(int[] numbers)
    {
        foreach (int num in numbers)
        {
            if (num < 0)
            {
                return;  // Exit void method
            }
            Console.WriteLine(num);
        }
    }
}
```

## Local Variables

Variables declared inside a method exist only within that method:

```csharp
public class Example
{
    public int classVariable = 10;  // Class scope
    
    public void ShowScope()
    {
        int localVariable = 5;      // Method scope
        string message = "Hello";   // Method scope
        
        Console.WriteLine(localVariable);  // OK - 5
        Console.WriteLine(message);        // OK - Hello
        Console.WriteLine(classVariable);  // OK - 10
    }
}

Example ex = new Example();
ex.ShowScope();
// Console.WriteLine(ex.localVariable);  // ERROR - not accessible!
```

## Key Concepts

1. **Methods organize code** into reusable blocks
2. **Each method has one purpose** (Single Responsibility Principle)
3. **Methods have inputs** (parameters) and **outputs** (return values)
4. **Local variables** are scoped to the method
5. **Return statement** exits method immediately

## Naming Conventions

Follow PascalCase for method names:

```csharp
// GOOD
public void PrintReport()
public bool IsValidEmail()
public string GetUsername()
public int CalculateSum()
public void SendNotification()

// BAD
public void print_report()
public void printReport()
public bool isvalidemail()
public void sendnotification()
```

## Method vs Function

In C#:
- **Methods** are functions inside a class
- All functions in C# are technically methods
- Sometimes "function" and "method" used interchangeably

```csharp
public class MyClass
{
    // This is called a method (in a class)
    public void DoSomething()
    {
        Console.WriteLine("Doing something");
    }
}

// Usage
MyClass obj = new MyClass();
obj.DoSomething();  // Call the method
```

## Common Method Patterns

### Pattern 1: Simple Calculator

```csharp
public class SimpleCalc
{
    public int Add(int a, int b) => a + b;
    public int Subtract(int a, int b) => a - b;
    public int Multiply(int a, int b) => a * b;
    public int Divide(int a, int b) => a / b;
}
```

### Pattern 2: Validator

```csharp
public class Validator
{
    public bool IsEmpty(string text)
    {
        return string.IsNullOrEmpty(text);
    }
    
    public bool IsValidEmail(string email)
    {
        return email.Contains("@");
    }
}
```

### Pattern 3: Getter/Setter

```csharp
public class User
{
    private string name;
    
    public void SetName(string newName)
    {
        name = newName;
    }
    
    public string GetName()
    {
        return name;
    }
}
```

## Summary

- **Methods** organize code into reusable units
- **Structure**: access modifier + return type + name + parameters + body
- **Return types** can be void or any data type
- **Parameters** provide input to methods
- **Local variables** are scoped to the method
- **Return statement** exits method with optional value
- **Follow naming conventions** (PascalCase)

## Next Steps

- Learn [Return-Types](../02-Return-Types/00-Return-Types.md) for detailed return type handling
- Study [Method-Structure](../03-Method-Structure/00-Method-Structure.md) for method design
- Move to [Parameters-Overloading](../../02-Parameters-Overloading/README.md) for advanced parameter handling
