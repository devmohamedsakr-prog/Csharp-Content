# Method Structure and Design

## Overview

Method structure defines how methods are organized and designed. Proper structure leads to readable, maintainable code.

## Complete Method Structure

```csharp
public int Calculate(int x, int y)
{
    int result = x + y;
    return result;
}

// Components:
// public - Access Modifier (visibility)
// int - Return Type (output type)
// Calculate - Method Name (identifier)
// (int x, int y) - Parameter List (inputs)
// { } - Method Body (implementation)
```

## Access Modifiers

Controls who can call the method:

### Public

Accessible from anywhere:

```csharp
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;  // Anyone can call
    }
}

// Usage (from another class)
Calculator calc = new Calculator();
calc.Add(5, 3);  // OK - public method
```

### Private

Accessible only within the class:

```csharp
public class User
{
    private string password;  // Private field
    
    private void ValidatePassword()  // Private method
    {
        // Only this class can call
    }
    
    public void SetPassword(string newPassword)  // Public method
    {
        ValidatePassword();  // Can call private method
    }
}

// Usage
User user = new User();
user.SetPassword("secure");     // OK - public method
// user.ValidatePassword();    // ERROR - private method
```

### Protected

Accessible within class and derived classes:

```csharp
public class Animal
{
    protected void MakeSound()  // Protected - accessible to derived classes
    {
        Console.WriteLine("Some sound");
    }
}

public class Dog : Animal
{
    public void Bark()
    {
        MakeSound();  // OK - derived class can call
    }
}
```

### Internal

Accessible within same assembly:

```csharp
internal class InternalClass  // Accessible only in this assembly
{
    internal void InternalMethod()  // Accessible only in this assembly
    {
    }
}
```

## Method Naming Conventions

Use PascalCase and descriptive names:

```csharp
// GOOD - Clear intent
public void PrintReport() { }
public bool IsValidEmail(string email) { }
public string GetFullName(string first, string last) { }
public void ProcessPayment() { }
public int CalculateTotalPrice() { }

// BAD - Unclear
public void Print() { }
public bool Check() { }
public string Get() { }
public void Process() { }
public int Calculate() { }
```

## Parameter Organization

Organize parameters logically:

```csharp
// GOOD - Related parameters together
public void CreateUser(string name, string email, string phone)
{
}

// GOOD - More complex? Use object
public class UserOptions
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
}

public void CreateUser(UserOptions options)
{
}
```

## Method Body Best Practices

### Keep It Simple

```csharp
// GOOD - Simple, focused
public int GetDiscount(decimal price, int percentage)
{
    return (int)(price * percentage / 100);
}

// BAD - Too complex
public void ProcessOrder(Order order)
{
    // Validate order
    // Check inventory
    // Calculate discount
    // Apply tax
    // Process payment
    // Update database
    // Send email
    // Too many responsibilities!
}
```

### Single Responsibility

Each method should do one thing:

```csharp
// BAD - Multiple responsibilities
public void SaveAndEmail(User user, string email)
{
    // Save user
    user.Save();
    
    // Send email
    SendEmail(user, email);
}

// GOOD - Separate concerns
public void SaveUser(User user)
{
    user.Save();
}

public void SendWelcomeEmail(User user)
{
    SendEmail(user, user.Email);
}
```

### Method Length

Shorter methods are better:

```csharp
// BAD - Too long
public void GenerateReport()
{
    // 50+ lines of code
    // Multiple responsibilities
    // Hard to test
    // Hard to understand
}

// GOOD - Smaller methods
public void GenerateReport()
{
    var data = FetchData();
    var formatted = FormatData(data);
    ExportReport(formatted);
}

private Data FetchData() { }
private string FormatData(Data data) { }
private void ExportReport(string formatted) { }
```

## Method Signature

The signature is the method declaration (without body):

```csharp
// Signatures:
public void Print()
public int Add(int a, int b)
public string GetName(int id)
public bool IsValid(string email)
public void Process(int id, string data, bool force)
```

### Signature Rules

- No two methods in same class can have identical signatures
- Return type is NOT part of signature (for overloading purposes)
- Parameter types and count ARE part of signature

```csharp
public class Example
{
    // Valid - different parameter count
    public void Print() { }
    public void Print(string message) { }
    
    // Valid - different parameter types
    public void Print(int number) { }
    public void Print(string text) { }
    
    // INVALID - same signature (different return type ignored)
    // public int Print() { }  // ERROR - same as first method
}
```

## Method Documentation

Use XML comments to document methods:

```csharp
/// <summary>
/// Calculates the sum of two numbers.
/// </summary>
/// <param name="a">The first number</param>
/// <param name="b">The second number</param>
/// <returns>The sum of a and b</returns>
public int Add(int a, int b)
{
    return a + b;
}

/// <summary>
/// Validates if an email address is correct.
/// </summary>
/// <param name="email">Email to validate</param>
/// <returns>True if valid, false otherwise</returns>
public bool IsValidEmail(string email)
{
    return email.Contains("@");
}

/// <summary>
/// Prints a message to the console.
/// </summary>
/// <remarks>
/// This is a simple helper method for output.
/// For complex logging, use a logging framework.
/// </remarks>
/// <param name="message">Message to print</param>
public void PrintMessage(string message)
{
    Console.WriteLine(message);
}
```

## Structure Patterns

### Simple Calculator Pattern

```csharp
public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Subtract(int a, int b) => a - b;
    public int Multiply(int a, int b) => a * b;
    public int Divide(int a, int b) => b != 0 ? a / b : 0;
}
```

### Validator Pattern

```csharp
public class Validator
{
    public bool IsValidEmail(string email)
    {
        return !string.IsNullOrEmpty(email) && email.Contains("@");
    }
    
    public bool IsValidAge(int age)
    {
        return age >= 0 && age <= 150;
    }
    
    public bool IsValidPassword(string password)
    {
        return password.Length >= 8;
    }
}
```

### Converter Pattern

```csharp
public class Converter
{
    public string ToUpperCase(string input)
    {
        return input?.ToUpper();
    }
    
    public int ParseInteger(string input)
    {
        return int.Parse(input);
    }
    
    public string FormatCurrency(decimal amount)
    {
        return amount.ToString("C");
    }
}
```

### Builder Pattern

```csharp
public class SqlQueryBuilder
{
    private string query = "";
    
    public SqlQueryBuilder Select(string columns)
    {
        query = $"SELECT {columns}";
        return this;
    }
    
    public SqlQueryBuilder From(string table)
    {
        query += $" FROM {table}";
        return this;
    }
    
    public SqlQueryBuilder Where(string condition)
    {
        query += $" WHERE {condition}";
        return this;
    }
    
    public string Build()
    {
        return query;
    }
}

// Usage
var sql = new SqlQueryBuilder()
    .Select("*")
    .From("Users")
    .Where("Age > 18")
    .Build();
```

## Consistency

Keep method structure consistent across your codebase:

```csharp
// CONSISTENT
public class User
{
    public string GetName() { }
    public int GetAge() { }
    public string GetEmail() { }
    
    public void SetName(string name) { }
    public void SetAge(int age) { }
    public void SetEmail(string email) { }
}

// INCONSISTENT - Avoid mixing patterns
public class User
{
    public string GetName() { }  // Getter style
    public void UpdateAge(int age) { }  // Different prefix
    public string Email { get; set; }  // Property style
}
```

## Method Ordering

Organize methods logically:

```csharp
public class User
{
    // Fields
    private string name;
    private int age;
    
    // Constructors
    public User() { }
    public User(string name, int age) { }
    
    // Public methods
    public void Display() { }
    public bool Validate() { }
    
    // Properties
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Private methods
    private void LogAction(string action) { }
    private bool IsValidAge() { }
}
```

## Summary

- **Structure**: Access modifier + return type + name + parameters + body
- **Naming**: Use PascalCase, descriptive names
- **Body**: Keep simple, single responsibility
- **Length**: Shorter is better
- **Documentation**: Use XML comments
- **Consistency**: Follow same patterns throughout
- **Organization**: Logical grouping of methods

## Next Steps

- Learn [Parameters](../../02-Parameters-Overloading/01-Parameter-Types/00-Parameter-Types.md) for method inputs
- Study [Method-Overloading](../../02-Parameters-Overloading/03-Method-Overloading/00-Method-Overloading.md) for multiple methods with same name
- Review [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) for production guidelines
