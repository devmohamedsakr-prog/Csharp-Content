# Access Modifiers

## Overview

Access modifiers control where members and types can be accessed. C# provides several levels: public (anywhere), private (this class only), protected (derived classes), internal (assembly only), and combinations thereof.

## Access Levels

| Modifier | This Class | Derived Class | Same Assembly | Other Assembly |
|----------|-----------|--------------|---------------|----------------|
| public | ✓ | ✓ | ✓ | ✓ |
| protected | ✓ | ✓ | ✗ | ✗ |
| internal | ✓ | ✗ | ✓ | ✗ |
| private | ✓ | ✗ | ✗ | ✗ |
| protected internal | ✓ | ✓ | ✓ | ✗ |
| private protected | ✓ | ✓ | ✗ | ✗ |

## Public

Accessible everywhere:

```csharp
public class PublicClass
{
    public int PublicField = 10;
    
    public void PublicMethod()
    {
        Console.WriteLine("Public method");
    }
}

// Accessible from anywhere
var obj = new PublicClass();
obj.PublicMethod();
Console.WriteLine(obj.PublicField);
```

## Private

Accessible only in this class:

```csharp
public class MyClass
{
    private int _privateField;
    
    private void PrivateMethod()
    {
        Console.WriteLine("Private");
    }
    
    public void PublicMethod()
    {
        PrivateMethod();  // OK - same class
        _privateField = 10;  // OK
    }
}

// Usage
var obj = new MyClass();
obj.PublicMethod();      // OK
// obj.PrivateMethod();  // ERROR - private
```

## Protected

Accessible in this class and derived classes:

```csharp
public class Base
{
    protected string _protected = "Protected";
    
    protected void ProtectedMethod()
    {
        Console.WriteLine("Protected method");
    }
}

public class Derived : Base
{
    public void Test()
    {
        Console.WriteLine(_protected);  // OK - derived class
        ProtectedMethod();  // OK
    }
}

// Usage
var obj = new Derived();
obj.Test();  // OK
// obj.ProtectedMethod();  // ERROR - not accessible outside
```

## Internal

Accessible within same assembly only:

```csharp
// MyAssembly.dll
internal class InternalClass  // Only accessible within MyAssembly
{
    internal void InternalMethod()
    {
        Console.WriteLine("Internal");
    }
}

// OtherAssembly.dll
// var obj = new InternalClass();  // ERROR - different assembly
```

## Protected Internal

Accessible in derived classes OR same assembly:

```csharp
public class Base
{
    protected internal string Data = "Protected Internal";
}

// Within same assembly OR derived class
public class Usage
{
    public void Test()
    {
        var base_obj = new Base();
        Console.WriteLine(base_obj.Data);  // OK - same assembly
    }
}

public class DerivedClass : Base
{
    public void Test()
    {
        Console.WriteLine(Data);  // OK - derived class
    }
}
```

## Private Protected

Accessible only in derived classes within same assembly:

```csharp
public class Base
{
    private protected string Data = "Private Protected";
}

public class DerivedSameAssembly : Base
{
    public void Test()
    {
        Console.WriteLine(Data);  // OK - derived + same assembly
    }
}

// In different assembly:
// public class DerivedOtherAssembly : Base
// {
//     public void Test()
//     {
//         Console.WriteLine(Data);  // ERROR - different assembly
//     }
// }
```

## Default Access Levels

When no modifier specified:

```csharp
// Class - defaults to internal
class MyClass { }       // internal
public class Public { } // public

// Member - defaults to private
public class Data
{
    int field;          // private
    void Method() { }   // private
    
    public int Public_field;  // public
}
```

## Class Access Modifiers

Classes can be public or internal:

```csharp
// Public class - can inherit outside assembly
public class PublicClass
{
}

// Internal class - can only inherit within assembly
internal class InternalClass
{
}
```

## Property Access Modifiers

Properties can have different access for get and set:

```csharp
public class Account
{
    private decimal _balance;
    
    // Different access levels for getter and setter
    public decimal Balance
    {
        get { return _balance; }  // Public getter
        private set { _balance = value; }  // Private setter
    }
    
    public void Withdraw(decimal amount)
    {
        Balance = _balance - amount;  // Can set via private setter
    }
}

// Usage
var account = new Account();
Console.WriteLine(account.Balance);  // OK - public getter
// account.Balance = 1000;  // ERROR - private setter
```

## Method Overriding and Access

Override cannot reduce visibility:

```csharp
public class Base
{
    public virtual void PublicMethod() { }
    protected virtual void ProtectedMethod() { }
}

public class Derived : Base
{
    // OK - same or more public
    public override void PublicMethod() { }
    
    // ERROR - cannot reduce visibility
    // private override void ProtectedMethod() { }
    
    // OK - same or more public
    public override void ProtectedMethod() { }
}
```

## Best Practices

### Principle of Least Privilege

```csharp
// Good - only expose what's needed
public class Service
{
    public void PublicOperation() { }
    private void InternalHelper() { }
    protected void ForDerivedClasses() { }
}

// Bad - expose everything
public class ServiceBad
{
    public int x;
    public string y;
    public void DoEverything() { }
}
```

### Use Private for Implementation Details

```csharp
public class DataValidator
{
    public bool Validate(string input)
    {
        return CheckFormat(input) && CheckLength(input);
    }
    
    // Private - implementation detail
    private bool CheckFormat(string input)
    {
        return !string.IsNullOrEmpty(input);
    }
    
    private bool CheckLength(string input)
    {
        return input.Length > 3;
    }
}
```

### Use Protected for Extension Points

```csharp
public abstract class Report
{
    public void Generate()
    {
        Load();
        Process();
        Save();
    }
    
    // Protected - meant to be overridden
    protected abstract void Load();
    protected abstract void Process();
    protected virtual void Save() { }
}
```

## Summary

- **public** - Accessible everywhere
- **private** - Only this class
- **protected** - This class + derived
- **internal** - Same assembly only
- **protected internal** - Derived or same assembly
- **private protected** - Derived in same assembly
- **Default** - Private for members, internal for types
- **Principle** - Use least privilege

## Next Steps

- Learn [Encapsulation](../03-Encapsulation/00-Encapsulation.md) for hiding implementation
- Study [Interfaces-Basics](../01-Interfaces-Basics/00-Interfaces-Basics.md) for contracts
- Review [Abstract-Classes](../02-Abstract-Classes/00-Abstract-Classes.md) for inheritance
