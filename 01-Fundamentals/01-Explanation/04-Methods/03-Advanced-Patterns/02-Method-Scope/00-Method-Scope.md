# Method Scope and Interaction

## Overview

Method scope defines where methods can be called and how they interact with each other and class members.

## Method Visibility

Access modifiers control method visibility:

```csharp
public class DataManager
{
    public void PublicMethod()
    {
        Console.WriteLine("Accessible everywhere");
    }
    
    private void PrivateMethod()
    {
        Console.WriteLine("Accessible only in this class");
    }
    
    protected void ProtectedMethod()
    {
        Console.WriteLine("Accessible in derived classes");
    }
    
    internal void InternalMethod()
    {
        Console.WriteLine("Accessible in same assembly");
    }
}

// Usage
DataManager dm = new DataManager();
dm.PublicMethod();        // OK
// dm.PrivateMethod();    // ERROR - private
// dm.ProtectedMethod();  // ERROR - protected (not in derived class)
```

## Local Variables vs Class Members

Scope of variables in methods:

```csharp
public class Example
{
    private int classVariable = 10;  // Class scope
    
    public void MethodOne()
    {
        int localVar = 5;            // Method scope
        
        Console.WriteLine(localVar);      // OK - 5
        Console.WriteLine(classVariable); // OK - 10
    }
    
    public void MethodTwo()
    {
        Console.WriteLine(classVariable); // OK - 10
        // Console.WriteLine(localVar);  // ERROR - local var from MethodOne not accessible
    }
}
```

## Methods Calling Methods

Methods can call other methods:

```csharp
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
    
    public int Multiply(int a, int b)
    {
        return a * b;
    }
    
    public int AddAndMultiply(int a, int b, int c)
    {
        int sum = Add(a, b);              // Call Add method
        int product = Multiply(sum, c);   // Call Multiply method
        return product;
    }
}

// Usage
Calculator calc = new Calculator();
calc.AddAndMultiply(2, 3, 4);  // (2+3)*4 = 20
```

## Method Chain Calls

Calling multiple methods in sequence:

```csharp
public class DataProcessor
{
    public string? rawData;
    
    public void LoadData(string filename)
    {
        Console.WriteLine($"Loading {filename}");
        rawData = "file contents";
    }
    
    public void ValidateData()
    {
        if (rawData != null && rawData.Length > 0)
            Console.WriteLine("Data valid");
        else
            Console.WriteLine("Data invalid");
    }
    
    public void ProcessData()
    {
        Console.WriteLine("Processing...");
    }
}

// Usage
DataProcessor dp = new DataProcessor();
dp.LoadData("data.txt");
dp.ValidateData();
dp.ProcessData();
```

## Method Dependency

Methods depending on state:

```csharp
public class UserSession
{
    private string? username;
    private bool isLoggedIn = false;
    
    public void Login(string user, string password)
    {
        if (ValidateCredentials(user, password))
        {
            username = user;
            isLoggedIn = true;
            Console.WriteLine($"Logged in as {username}");
        }
    }
    
    private bool ValidateCredentials(string user, string password)
    {
        // Simple validation
        return !string.IsNullOrEmpty(user) && password.Length >= 8;
    }
    
    public void GetUserData()
    {
        if (!isLoggedIn)
        {
            Console.WriteLine("Not logged in");
            return;
        }
        Console.WriteLine($"User data for {username}");
    }
}

// Usage
UserSession session = new UserSession();
session.Login("alice", "password123");
session.GetUserData();
```

## Private Helper Methods

Using private methods for internal logic:

```csharp
public class FileProcessor
{
    public void ProcessFile(string filename)
    {
        string content = ReadFile(filename);
        string processed = Transform(content);
        SaveFile(filename, processed);
    }
    
    // Private helper methods
    private string ReadFile(string filename)
    {
        return File.ReadAllText(filename);
    }
    
    private string Transform(string content)
    {
        return content.ToUpper();
    }
    
    private void SaveFile(string filename, string content)
    {
        File.WriteAllText(filename, content);
    }
}
```

## Method Scope Rules

### Within Same Class

All methods can call each other (respecting access):

```csharp
public class Example
{
    public void PublicMethod()
    {
        PrivateMethod();  // Can call private from public
        ProtectedMethod();
    }
    
    private void PrivateMethod()
    {
        PublicMethod();   // Can call public from private
    }
    
    protected void ProtectedMethod()
    {
        PublicMethod();   // Can call public from protected
    }
}
```

### In Derived Class

Can access public and protected:

```csharp
public class BaseClass
{
    public void PublicMethod() { }
    private void PrivateMethod() { }
    protected void ProtectedMethod() { }
}

public class DerivedClass : BaseClass
{
    public void TestAccess()
    {
        PublicMethod();      // OK - public
        ProtectedMethod();   // OK - protected
        // PrivateMethod();  // ERROR - private not accessible
    }
}
```

### From Outside Class

Only public methods accessible:

```csharp
public class MyClass
{
    public void PublicMethod() { }
    private void PrivateMethod() { }
    protected void ProtectedMethod() { }
}

// Outside the class
MyClass obj = new MyClass();
obj.PublicMethod();       // OK - public
// obj.PrivateMethod();   // ERROR - private
// obj.ProtectedMethod(); // ERROR - protected
```

## Static Methods

Can call without instance:

```csharp
public class StaticExample
{
    public static void StaticMethod()
    {
        Console.WriteLine("Static method");
    }
    
    public void InstanceMethod()
    {
        StaticMethod();  // Can call static from instance
    }
}

// Usage
StaticExample.StaticMethod();     // No instance needed
StaticExample obj = new StaticExample();
obj.StaticMethod();               // Also works with instance
```

## Parameter Scope

Parameters are accessible within method:

```csharp
public class Example
{
    public void Process(int value, string name)
    {
        // Parameters accessible throughout method
        Console.WriteLine(value);
        Console.WriteLine(name);
        
        // Modify parameters (local copy)
        value = 100;
        name = "changed";
        
        // Changes don't affect caller (unless ref/out)
    }
}
```

## Block Scope

Variables scoped to blocks:

```csharp
public void DemonstrateBlockScope()
{
    if (true)
    {
        int blockVar = 5;
        Console.WriteLine(blockVar);  // OK
    }
    // Console.WriteLine(blockVar);  // ERROR - out of scope
    
    for (int i = 0; i < 3; i++)
    {
        Console.WriteLine(i);         // OK in loop
    }
    // Console.WriteLine(i);          // ERROR - out of scope
}
```

## Variable Shadowing

Inner scope shadows outer scope:

```csharp
public class ShadowingExample
{
    public int x = 10;  // Class member
    
    public void Method()
    {
        int x = 5;      // Local variable shadows class member
        
        Console.WriteLine(x);        // 5 (local)
        Console.WriteLine(this.x);   // 10 (class member)
        
        if (true)
        {
            int x = 1;  // New scope, shadows method-level x
            Console.WriteLine(x);    // 1 (block-level)
        }
    }
}
```

## Method Scope in Lambda

Lambdas can access enclosing scope:

```csharp
public void DemonstrateLambdaScope()
{
    int outerVar = 10;
    
    Action lambda = () =>
    {
        Console.WriteLine(outerVar);  // Can access outer scope
    };
    
    lambda();  // Output: 10
}
```

## Common Patterns

### Pattern 1: Encapsulation

```csharp
public class BankAccount
{
    private decimal balance;
    
    public void Deposit(decimal amount)
    {
        if (ValidateAmount(amount))
        {
            balance += amount;
            LogTransaction("Deposit", amount);
        }
    }
    
    private bool ValidateAmount(decimal amount)
    {
        return amount > 0;
    }
    
    private void LogTransaction(string type, decimal amount)
    {
        Console.WriteLine($"{type}: {amount}");
    }
}
```

### Pattern 2: Template Method

```csharp
public class ReportGenerator
{
    public void GenerateReport()
    {
        LoadData();
        ProcessData();
        FormatOutput();
        SaveReport();
    }
    
    private void LoadData() { }
    private void ProcessData() { }
    private void FormatOutput() { }
    private void SaveReport() { }
}
```

## Summary

- **Visibility**: Public, private, protected, internal
- **Local variables**: Scoped to method/block
- **Method calls**: Can call other methods
- **Private helpers**: Internal implementation methods
- **Static methods**: Call without instance
- **Block scope**: Variables scoped to blocks
- **Shadowing**: Inner scope hides outer
- **Encapsulation**: Hide implementation details

## Next Steps

- Learn [Special-Methods](../03-Special-Methods/00-Special-Methods.md) for constructors and special method types
- Review [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) for scope guidelines
