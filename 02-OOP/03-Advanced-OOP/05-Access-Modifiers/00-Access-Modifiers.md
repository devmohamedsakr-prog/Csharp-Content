# Access Modifiers - Visibility Control

## Overview

Access modifiers control who can access classes, methods, properties, and fields. They are essential for encapsulation and API design.

## Access Modifier Levels

| Modifier | Same Class | Derived Class | Same Assembly | External |
|----------|-----------|---------------|---------------|----------|
| public | ✓ | ✓ | ✓ | ✓ |
| protected | ✓ | ✓ | ✗ | ✗ |
| internal | ✓ | ✗ | ✓ | ✗ |
| protected internal | ✓ | ✓ | ✓ | ✗ |
| private | ✓ | ✗ | ✗ | ✗ |
| private protected | ✓ | ✓ | ✗ | ✗ |

## Public

Accessible everywhere:

```csharp
public class PublicClass
{
    public void PublicMethod() { }
    public string PublicProperty { get; set; }
}

// Accessible from anywhere
var obj = new PublicClass();
obj.PublicMethod();
obj.PublicProperty = "value";
```

## Private

Accessible only in the class:

```csharp
public class MyClass
{
    private string _privateField;
    
    private void PrivateMethod()
    {
        _privateField = "internal";
    }
    
    public void PublicMethod()
    {
        PrivateMethod();  // OK - within class
    }
}

// var obj = new MyClass();
// obj.PrivateMethod();  // ERROR - not accessible
```

## Protected

Accessible in same class and derived classes:

```csharp
public class Base
{
    protected string ProtectedField;
    
    protected void ProtectedMethod()
    {
        Console.WriteLine("Protected method");
    }
}

public class Derived : Base
{
    public void CallProtected()
    {
        ProtectedField = "value";  // OK - derived class
        ProtectedMethod();         // OK - derived class
    }
}

// var obj = new Derived();
// obj.ProtectedMethod();  // ERROR - not accessible from outside
```

## Internal

Accessible in same assembly:

```csharp
// Assembly 1 (MyLibrary.dll)
internal class InternalClass
{
    internal void Method() { }
}

// Assembly 2 (MyApp.exe) - different assembly
// var obj = new InternalClass();  // ERROR - not accessible
```

## Protected Internal

Accessible in derived classes or same assembly:

```csharp
// Assembly 1
public class Base
{
    protected internal string Data;
    
    protected internal void Method() { }
}

// Assembly 1 - can access (same assembly)
var obj = new Base();
obj.Data = "value";

// Derived in Assembly 2
public class Derived : Base
{
    public void Access()
    {
        Data = "value";  // OK - derived
        Method();        // OK - derived
    }
}
```

## Private Protected

Accessible in same class and derived classes in same assembly:

```csharp
public class Base
{
    private protected void Method() { }
}

// Same assembly - derived class
public class Derived : Base
{
    public void Call()
    {
        Method();  // OK
    }
}

// Different assembly - even if derived
// public class ExternalDerived : Base
// {
//     public void Call()
//     {
//         Method();  // ERROR
//     }
// }
```

## Method Overloading and Access

```csharp
public class MyClass
{
    // Public interface
    public void DoSomething(string input)
    {
        var validated = ValidateInput(input);
        Process(validated);
    }
    
    // Private helper
    private string ValidateInput(string input)
    {
        return input?.Trim() ?? "";
    }
    
    // Private implementation
    private void Process(string data)
    {
        Console.WriteLine($"Processing: {data}");
    }
}
```

## Property Access Modifiers

```csharp
public class DataContainer
{
    // Public get, public set
    public string PublicData { get; set; }
    
    // Public get, private set
    public string ReadOnlyFromOutside { get; private set; }
    
    // Public get, protected set
    public string ProtectedSet { get; protected set; }
    
    // Private get, public set (unusual)
    public string WriteOnlyFromOutside { private get; set; }
}

// Usage
var obj = new DataContainer();
obj.PublicData = "value";  // OK
obj.ReadOnlyFromOutside = "value";  // ERROR - private set
Console.WriteLine(obj.ReadOnlyFromOutside);  // OK - public get
```

## Class Access Modifiers

```csharp
// Only public or internal for top-level classes
public class PublicClass { }
internal class InternalClass { }

public class Outer
{
    // Nested classes can be private
    private class NestedPrivate { }
    public class NestedPublic { }
}
```

## Best Practices

### 1. Start Private, Expose What's Needed

```csharp
// Good - Minimal public API
public class Employee
{
    public string Name { get; set; }
    
    // Internal calculations - private
    private decimal CalculateBonus()
    {
        return 0;
    }
}

// Bad - Over-exposed
public class BadEmployee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Salary { get; set; }
    public decimal Tax { get; set; }  // Implementation detail
    public decimal CalculateBonus() { return 0; }
    public decimal CalculateTax() { return 0; }
}
```

### 2. Use Properties with Controlled Access

```csharp
// Good - Controlled access
public class Account
{
    public decimal Balance { get; private set; }
    
    public void Deposit(decimal amount)
    {
        Balance += amount;
    }
}

// Bad - Exposed field
public class BadAccount
{
    public decimal Balance;  // Can be set to invalid value
}
```

### 3. Document Protected Members

```csharp
/// <summary>
/// For internal use by derived classes only.
/// </summary>
protected virtual void OnDataChanged()
{
    // Subclasses can override
}
```

## Common Patterns

### Pattern 1: Sealed Classes

```csharp
// Cannot be inherited
public sealed class FinalClass
{
    public void Method() { }
}

// public class Derived : FinalClass { }  // ERROR
```

### Pattern 2: Interface Implementation

```csharp
public interface IService
{
    void Execute();
}

public class Service : IService
{
    // Explicit implementation - not directly callable
    void IService.Execute()
    {
        ExecuteInternal();
    }
    
    // Private helper
    private void ExecuteInternal()
    {
    }
}

// Usage
IService service = new Service();
service.Execute();  // OK - through interface

// Service s = new Service();
// s.Execute();  // ERROR - private
```

## Summary

- **public** - Accessible everywhere (API)
- **private** - Only in class (default for members)
- **protected** - Class + derived classes
- **internal** - Within assembly
- **protected internal** - Protected OR internal
- **private protected** - Protected AND internal (rare)
- **Best practice** - Start private, expose minimum

## Next Steps

- Review [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
- Study [Common-Mistakes](../../04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md)
