# Parameter Types

## Overview

Parameters are inputs to methods. Understanding different parameter types allows you to pass data to methods effectively.

## No Parameters

Methods can have no parameters:

```csharp
public class Example
{
    public void SayHello()
    {
        Console.WriteLine("Hello!");
    }
    
    public int GetAnswer()
    {
        return 42;
    }
    
    public DateTime GetCurrentTime()
    {
        return DateTime.Now;
    }
}

// Usage
Example ex = new Example();
ex.SayHello();
int answer = ex.GetAnswer();
DateTime now = ex.GetCurrentTime();
```

## Single Parameter

Methods can take one input:

```csharp
public class Greeter
{
    public void Greet(string name)
    {
        Console.WriteLine($"Hello, {name}!");
    }
    
    public bool IsPositive(int number)
    {
        return number > 0;
    }
    
    public double DoubleIt(double value)
    {
        return value * 2;
    }
}

// Usage
Greeter greeter = new Greeter();
greeter.Greet("Alice");       // Hello, Alice!
bool positive = IsPositive(5);  // true
double result = DoubleIt(3.5);  // 7.0
```

## Multiple Parameters

Methods can take several inputs:

```csharp
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
    
    public void PrintInfo(string name, int age, string city)
    {
        Console.WriteLine($"{name}, {age} years old, from {city}");
    }
    
    public bool IsInRange(int value, int min, int max)
    {
        return value >= min && value <= max;
    }
}

// Usage
Calculator calc = new Calculator();
int sum = calc.Add(5, 3);                          // 8
calc.PrintInfo("Alice", 30, "New York");
bool inRange = calc.IsInRange(15, 10, 20);       // true
```

## Parameter Declaration

Each parameter must have a type and name:

```csharp
// GOOD - Explicit types
public void PrintInfo(string name, int age, string city)
{
}

// BAD - Missing types
public void PrintInfo(name, age, city)
{
}

// GOOD - Mixed types OK
public void ProcessData(int id, string name, double salary, bool active)
{
}
```

## Primitive Type Parameters

Methods can take any primitive type:

```csharp
public class Demo
{
    public void ProcessInt(int value) { }
    public void ProcessDouble(double value) { }
    public void ProcessBool(bool value) { }
    public void ProcessChar(char value) { }
    public void ProcessString(string value) { }
    public void ProcessLong(long value) { }
    public void ProcessFloat(float value) { }
}
```

## Reference Type Parameters

Methods can take objects and collections:

```csharp
public class DataHandler
{
    public void ProcessArray(int[] numbers)
    {
        foreach (int num in numbers)
        {
            Console.WriteLine(num);
        }
    }
    
    public void ProcessList(List<string> items)
    {
        foreach (string item in items)
        {
            Console.WriteLine(item);
        }
    }
    
    public void ProcessObject(Person person)
    {
        Console.WriteLine($"{person.Name} is {person.Age}");
    }
}

// Usage
int[] numbers = { 1, 2, 3 };
dataHandler.ProcessArray(numbers);

List<string> items = new List<string> { "A", "B", "C" };
dataHandler.ProcessList(items);

Person person = new Person { Name = "Alice", Age = 30 };
dataHandler.ProcessObject(person);
```

## Optional (Default) Parameters

Parameters can have default values:

```csharp
public class Printer
{
    public void Print(string message, int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            Console.WriteLine(message);
        }
    }
    
    public void PrintWithStyle(string message, string style = "NORMAL")
    {
        if (style == "BOLD")
            Console.WriteLine($"**{message}**");
        else if (style == "ITALIC")
            Console.WriteLine($"_{message}_");
        else
            Console.WriteLine(message);
    }
}

// Usage
Printer printer = new Printer();
printer.Print("Hello");           // Prints once (default)
printer.Print("Hello", 3);        // Prints 3 times
printer.PrintWithStyle("Alert");  // Normal style (default)
printer.PrintWithStyle("Alert", "BOLD");  // Bold style
```

### Default Parameter Rules

```csharp
// GOOD - Defaults at end
public void Method(int required, string optional = "default") { }

// BAD - Required after optional
// public void Method(string optional = "default", int required) { }

// GOOD - All defaults at end
public void Method(int a, string b = "B", int c = 3) { }
```

## Named Parameters

Call methods using parameter names:

```csharp
public class User
{
    public void Create(string name, int age, string email)
    {
        Console.WriteLine($"{name}, {age}, {email}");
    }
}

// Usage - Positional
User user = new User();
user.Create("Alice", 30, "alice@example.com");

// Usage - Named (order doesn't matter)
user.Create(name: "Bob", age: 25, email: "bob@example.com");
user.Create(age: 35, name: "Charlie", email: "charlie@example.com");
user.Create(email: "diana@example.com", name: "Diana", age: 28);

// Usage - Mix positional and named
user.Create("Eve", email: "eve@example.com", age: 32);
```

## Params Keyword (Variable Arguments)

Accept variable number of arguments:

```csharp
public class Calculator
{
    public int Sum(params int[] numbers)
    {
        int total = 0;
        foreach (int num in numbers)
        {
            total += num;
        }
        return total;
    }
    
    public void PrintAll(params string[] items)
    {
        foreach (string item in items)
        {
            Console.WriteLine(item);
        }
    }
}

// Usage
Calculator calc = new Calculator();
int sum1 = calc.Sum(1, 2, 3);           // 6
int sum2 = calc.Sum(1, 2, 3, 4, 5);     // 15
int sum3 = calc.Sum();                   // 0 (empty)

calc.PrintAll("A", "B", "C");
calc.PrintAll("X");
calc.PrintAll();  // No items
```

### Params Rules

```csharp
// GOOD - Params at end
public void Method(int required, params int[] numbers) { }

// BAD - Params not at end
// public void Method(params int[] numbers, int required) { }

// GOOD - Only one params allowed
public void Method(string text, params int[] numbers) { }

// BAD - Multiple params
// public void Method(params int[] numbers, params string[] texts) { }
```

## Parameter Passing Mechanisms

### Pass by Value (Default)

Method gets a copy:

```csharp
public void Increment(int number)
{
    number++;  // Modifies local copy only
}

int x = 5;
Increment(x);
Console.WriteLine(x);  // Still 5 (unchanged)
```

### Pass by Reference (ref)

Method can modify original:

```csharp
public void Increment(ref int number)
{
    number++;  // Modifies original
}

int x = 5;
Increment(ref x);
Console.WriteLine(x);  // 6 (changed!)
```

### Output Parameters (out)

Method must assign value:

```csharp
public bool TryParse(string input, out int result)
{
    result = 0;  // Must initialize
    
    if (int.TryParse(input, out int number))
    {
        result = number;
        return true;
    }
    return false;
}

// Usage
if (TryParse("42", out int value))
{
    Console.WriteLine($"Parsed: {value}");
}
```

## Common Parameter Patterns

### Pattern 1: Filter Parameters

```csharp
public List<int> FilterNumbers(int[] numbers, int minValue, int maxValue)
{
    var filtered = new List<int>();
    foreach (int num in numbers)
    {
        if (num >= minValue && num <= maxValue)
        {
            filtered.Add(num);
        }
    }
    return filtered;
}

// Usage
int[] data = { 1, 5, 10, 15, 20 };
var result = FilterNumbers(data, 5, 15);  // [5, 10, 15]
```

### Pattern 2: Conditional Parameters

```csharp
public string GetStatus(int code, bool verbose = false)
{
    if (code == 200)
        return verbose ? "Request succeeded successfully" : "OK";
    if (code == 404)
        return verbose ? "Resource not found" : "NOT FOUND";
    return "UNKNOWN";
}

// Usage
GetStatus(200);          // "OK"
GetStatus(200, true);    // "Request succeeded successfully"
```

### Pattern 3: Configuration Parameters

```csharp
public void GenerateReport(string title, int pageSize = 10, 
    string format = "PDF", bool includeCharts = true)
{
    // Generate report with specified options
}

// Usage
GenerateReport("Sales Report");
GenerateReport("Sales Report", pageSize: 20);
GenerateReport("Sales Report", format: "Excel");
GenerateReport("Sales Report", includeCharts: false);
```

## Type Safety

Parameters enforce type checking:

```csharp
public void PrintInteger(int value)
{
    Console.WriteLine(value);
}

// Usage
PrintInteger(42);        // OK
// PrintInteger("text");  // ERROR - wrong type
// PrintInteger(3.14);    // ERROR - wrong type (needs cast)
```

## Nullable Parameters

Parameters can accept null:

```csharp
public void ProcessName(string? name)
{
    if (name != null)
        Console.WriteLine($"Name: {name}");
    else
        Console.WriteLine("No name provided");
}

public void ProcessAge(int? age)
{
    if (age.HasValue)
        Console.WriteLine($"Age: {age.Value}");
    else
        Console.WriteLine("No age provided");
}

// Usage
ProcessName("Alice");
ProcessName(null);
ProcessAge(30);
ProcessAge(null);
```

## Parameter Order

Organize parameters logically:

```csharp
// GOOD - Related parameters together
public void CreateUser(string name, string email, string phone,
    int age, string address, string city)
{
}

// BETTER - Use object for many parameters
public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
}

public void CreateUser(CreateUserRequest request)
{
}
```

## Summary

- **No parameters**: Methods can have no inputs
- **Single/multiple**: Accept one or more parameters
- **Default values**: Optional parameters with defaults
- **Named parameters**: Call using parameter names
- **Params keyword**: Variable number of arguments
- **Passing mechanisms**: Value (copy), ref (reference), out (output)
- **Type safety**: Strong typing of parameters
- **Organization**: Logical parameter ordering

## Next Steps

- Learn [Advanced-Parameters](../02-Advanced-Parameters/00-Advanced-Parameters.md) for special parameter techniques
- Study [Method-Overloading](../03-Method-Overloading/00-Method-Overloading.md) for multiple parameter combinations
- Review [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) for parameter design guidelines
