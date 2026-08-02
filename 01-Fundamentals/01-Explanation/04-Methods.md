# Methods (Functions) in C#

## Overview
A method is a block of code that performs a specific task. Methods allow you to organize code into reusable chunks.

---

## Method Structure

```csharp
// Basic structure
public void MethodName() {
    // Method body
}

// With parameters and return type
public int Add(int a, int b) {
    return a + b;
}
```

**Components**:
- **Access Modifier**: public, private, protected, internal
- **Return Type**: void, int, string, etc.
- **Method Name**: PascalCase convention
- **Parameters**: (type name, type name, ...)
- **Body**: { code }

---

## Method Types

### Void Methods (No Return)
Execute code but return nothing.

```csharp
public void Greet(string name) {
    Console.WriteLine($"Hello, {name}!");
}

public void PrintNumbers(int count) {
    for (int i = 1; i <= count; i++) {
        Console.WriteLine(i);
    }
}

// Usage
Greet("Alice");
PrintNumbers(5);
```

### Return Type Methods
Return a value to the caller.

```csharp
public int Add(int a, int b) {
    return a + b;
}

public string GetFullName(string first, string last) {
    return $"{first} {last}";
}

public double CalculateAverage(int[] numbers) {
    int sum = 0;
    foreach (int num in numbers) {
        sum += num;
    }
    return (double)sum / numbers.Length;
}

// Usage
int result = Add(5, 3);  // 8
string name = GetFullName("John", "Doe");  // "John Doe"
double avg = CalculateAverage(new int[] { 10, 20, 30 });  // 20
```

---

## Parameters

### No Parameters
```csharp
public string GetCurrentDate() {
    return DateTime.Now.ToString("yyyy-MM-dd");
}

GetCurrentDate();
```

### Single Parameter
```csharp
public bool IsPositive(int number) {
    return number > 0;
}

IsPositive(5);  // true
```

### Multiple Parameters
```csharp
public void PrintPersonInfo(string name, int age, string city) {
    Console.WriteLine($"{name}, {age}, {city}");
}

PrintPersonInfo("Alice", 30, "New York");
```

### Default Parameters
```csharp
public void PrintMessage(string message, int times = 1) {
    for (int i = 0; i < times; i++) {
        Console.WriteLine(message);
    }
}

PrintMessage("Hello");  // Prints once
PrintMessage("Hello", 3);  // Prints 3 times
```

### Named Parameters
```csharp
public void CreateUser(string name, int age, string email) {
    Console.WriteLine($"{name}, {age}, {email}");
}

CreateUser(name: "Bob", age: 25, email: "bob@example.com");
CreateUser(email: "alice@example.com", name: "Alice", age: 30);
```

### Params Keyword (Variable Arguments)
```csharp
public int Sum(params int[] numbers) {
    int total = 0;
    foreach (int num in numbers) {
        total += num;
    }
    return total;
}

Sum(1, 2, 3);  // 6
Sum(1, 2, 3, 4, 5);  // 15
Sum();  // 0
```

---

## Method Overloading

Same method name with different parameters.

```csharp
public class Calculator {
    // Overload 1: two integers
    public int Add(int a, int b) {
        return a + b;
    }
    
    // Overload 2: three integers
    public int Add(int a, int b, int c) {
        return a + b + c;
    }
    
    // Overload 3: two doubles
    public double Add(double a, double b) {
        return a + b;
    }
    
    // Overload 4: array of integers
    public int Add(int[] numbers) {
        int sum = 0;
        foreach (int num in numbers) {
            sum += num;
        }
        return sum;
    }
}

Calculator calc = new Calculator();
calc.Add(5, 3);  // 8 (Overload 1)
calc.Add(5, 3, 2);  // 10 (Overload 2)
calc.Add(5.5, 3.2);  // 8.7 (Overload 3)
calc.Add(new int[] { 1, 2, 3 });  // 6 (Overload 4)
```

---

## Return Statement

Exit method and return value.

```csharp
public int GetFirstPositive(int[] numbers) {
    foreach (int num in numbers) {
        if (num > 0) {
            return num;  // Exit and return
        }
    }
    return 0;  // Default if none found
}

public void PrintUntilNegative(int[] numbers) {
    foreach (int num in numbers) {
        if (num < 0) {
            return;  // Exit (void method)
        }
        Console.WriteLine(num);
    }
}
```

---

## Local Variables

Variables declared inside a method are local to that method.

```csharp
public class Calculator {
    public int result = 0;  // Class member (instance variable)
    
    public void Calculate() {
        int localVar = 5;  // Local variable
        string message = "Hello";  // Local variable
        
        Console.WriteLine(localVar);  // OK
        Console.WriteLine(message);  // OK
    }
}

Calculator calc = new Calculator();
calc.Calculate();
// Console.WriteLine(calc.localVar);  // Error - not accessible
```

---

## Method Scope

Methods can call other methods.

```csharp
public class Printer {
    public void PrintNumbers(int count) {
        for (int i = 1; i <= count; i++) {
            PrintNumber(i);  // Call another method
        }
    }
    
    public void PrintNumber(int num) {
        Console.WriteLine(num);
    }
}

Printer printer = new Printer();
printer.PrintNumbers(5);
```

---

## Recursive Methods

Method that calls itself.

```csharp
// Calculate factorial
public int Factorial(int n) {
    if (n <= 1) {
        return 1;  // Base case
    }
    return n * Factorial(n - 1);  // Recursive call
}

Factorial(5);  // 120

// Calculate Fibonacci
public int Fibonacci(int n) {
    if (n <= 1) {
        return n;  // Base case
    }
    return Fibonacci(n - 1) + Fibonacci(n - 2);  // Recursive call
}

Fibonacci(6);  // 8
```

---

## Method with Out Parameter

Modify parameter and return value.

```csharp
public bool TryParseNumber(string input, out int result) {
    result = 0;  // Must assign before return
    
    if (int.TryParse(input, out int number)) {
        result = number;
        return true;
    }
    return false;
}

if (TryParseNumber("42", out int value)) {
    Console.WriteLine($"Parsed: {value}");  // "Parsed: 42"
}
```

---

## Method with Ref Parameter

Pass parameter by reference (can modify original).

```csharp
public void Increment(ref int number) {
    number++;  // Modifies original variable
}

int x = 5;
Increment(ref x);
Console.WriteLine(x);  // 6 (changed!)

// vs

public void IncrementCopy(int number) {
    number++;  // Only modifies copy
}

int y = 5;
IncrementCopy(y);
Console.WriteLine(y);  // 5 (unchanged!)
```

---

## Best Practices

✓ **Single Responsibility** - One method, one purpose
```csharp
// Good
public void ValidateEmail(string email) { }
public void SendEmail(string email) { }

// Bad - too many responsibilities
public void HandleEmail(string email) {
    // Validate AND send
}
```

✓ **Meaningful Names** - Describe what method does
```csharp
// Good
public bool IsValidEmail(string email) { }
public List<User> GetActiveUsers() { }

// Bad - unclear
public bool Check(string email) { }
public List<User> Get() { }
```

✓ **Keep Methods Short** - Easier to understand and test
```csharp
// Good - focused
public int CalculateDiscount(decimal price, int percentage) {
    return (int)(price * percentage / 100);
}

// Too long
public void ProcessOrder(Order order) {
    // Validate order
    // Calculate discount
    // Apply tax
    // Process payment
    // Send email
    // Update database
}
```

✓ **Limited Parameters** - Easier to call and understand
```csharp
// Good - few parameters
public User CreateUser(string name, string email) { }

// Less ideal - many parameters
public User CreateUser(string name, string email, string phone, 
    string address, int age, string city, string country) { }
```

✓ **Use Default Parameters** - Simpler API
```csharp
// Good
public void PrintReport(string title, int pageSize = 10) { }

PrintReport("Sales");  // Uses default
PrintReport("Sales", 20);  // Custom value
```

---

## Common Mistakes

❌ **Forgetting Return Type**
```csharp
public Add(int a, int b) {  // Error - no return type
    return a + b;
}
```

✓ **Specify Return Type**
```csharp
public int Add(int a, int b) {  // Correct
    return a + b;
}
```

❌ **Missing Return Statement**
```csharp
public int GetNumber() {
    Console.WriteLine("5");
    // Error - missing return
}
```

✓ **Include Return**
```csharp
public int GetNumber() {
    return 5;
}
```

❌ **Too Many Parameters**
```csharp
public void CreateReport(string title, int width, int height, 
    string format, bool landscape, int fontSize, string fontFamily) { }
```

✓ **Use Object Parameter**
```csharp
public class ReportOptions {
    public string Title { get; set; }
    public int Width { get; set; }
    // ... other properties
}

public void CreateReport(ReportOptions options) { }
```

---

## Quick Summary

- Methods organize code into reusable blocks
- Can have parameters and return values
- Void methods don't return anything
- Support overloading (same name, different parameters)
- Local variables are scoped to the method
- Follow naming conventions and keep focused
