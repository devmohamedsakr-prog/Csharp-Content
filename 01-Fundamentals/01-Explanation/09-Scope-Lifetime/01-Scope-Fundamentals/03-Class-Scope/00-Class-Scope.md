# Class Scope and Access Modifiers in C#

## Overview

Class scope determines where class members (fields, properties, methods) can be accessed. Access modifiers control this visibility: public, private, protected, internal, and private protected.

## Access Modifiers

### Public Access

```csharp
public class PublicExample
{
    // Public members are accessible from anywhere
    public string PublicField = "Public";
    
    public void PublicMethod()
    {
        Console.WriteLine("This is public");
    }
    
    public int PublicProperty { get; set; }
}

// Accessible from anywhere
public class AnyClass
{
    public void AccessPublic()
    {
        var obj = new PublicExample();
        Console.WriteLine(obj.PublicField); // OK
        obj.PublicMethod(); // OK
        obj.PublicProperty = 5; // OK
    }
}
```

### Private Access

```csharp
public class PrivateExample
{
    // Private members are only accessible within this class
    private string _privateField = "Private";
    
    private void PrivateMethod()
    {
        Console.WriteLine("This is private");
    }
    
    private int PrivateProperty { get; set; }
    
    public void PublicMethod()
    {
        Console.WriteLine(_privateField); // OK - within same class
        PrivateMethod(); // OK - within same class
        PrivateProperty = 5; // OK - within same class
    }
}

public class OtherClass
{
    public void AccessPrivate()
    {
        var obj = new PrivateExample();
        // Console.WriteLine(obj._privateField); // ERROR: private
        // obj.PrivateMethod(); // ERROR: private
        // obj.PrivateProperty = 5; // ERROR: private
        
        obj.PublicMethod(); // OK - can only access public
    }
}
```

### Protected Access

```csharp
public class BaseClass
{
    // Protected members are accessible in this class and derived classes
    protected string ProtectedField = "Protected";
    
    protected void ProtectedMethod()
    {
        Console.WriteLine("Protected method");
    }
    
    protected int ProtectedProperty { get; set; }
}

public class DerivedClass : BaseClass
{
    public void AccessProtected()
    {
        Console.WriteLine(ProtectedField); // OK - accessed in derived class
        ProtectedMethod(); // OK
        ProtectedProperty = 10; // OK
    }
}

public class UnrelatedClass
{
    public void TryAccess()
    {
        var obj = new BaseClass();
        // Console.WriteLine(obj.ProtectedField); // ERROR: protected, not derived
        // obj.ProtectedMethod(); // ERROR: protected, not derived
    }
}
```

### Internal Access

```csharp
// MyAssembly.dll

public class InternalExample
{
    // Internal members are accessible within the same assembly
    internal string InternalField = "Internal";
    
    internal void InternalMethod()
    {
        Console.WriteLine("Internal method");
    }
    
    internal int InternalProperty { get; set; }
}

public class SameAssemblyClass
{
    public void Access()
    {
        var obj = new InternalExample();
        Console.WriteLine(obj.InternalField); // OK - same assembly
        obj.InternalMethod(); // OK
    }
}

// Different assembly would NOT have access to internal members
```

### Private Protected Access (C# 7.2+)

```csharp
public class PrivateProtectedExample
{
    // Private protected: accessible in this class and derived classes ONLY within same assembly
    private protected string PrivateProtectedField = "Private Protected";
    
    private protected void PrivateProtectedMethod()
    {
        Console.WriteLine("Private protected method");
    }
}

// Same assembly, derived class
public class DerivedInSameAssembly : PrivateProtectedExample
{
    public void Access()
    {
        Console.WriteLine(PrivateProtectedField); // OK
        PrivateProtectedMethod(); // OK
    }
}

// Different assembly, derived class
// public class DerivedInDifferentAssembly : PrivateProtectedExample
// {
//     public void Access()
//     {
//         Console.WriteLine(PrivateProtectedField); // ERROR: not accessible
//     }
// }
```

## Class Member Scope Combinations

### Fields with Different Access Levels

```csharp
public class MemberScope
{
    public string PublicField;
    private string _privateField;
    protected string ProtectedField;
    internal string InternalField;
    private protected string PrivateProtectedField;
    
    public void PublicMethod()
    {
        // All accessible within the class
        Console.WriteLine(PublicField);
        Console.WriteLine(_privateField);
        Console.WriteLine(ProtectedField);
        Console.WriteLine(InternalField);
        Console.WriteLine(PrivateProtectedField);
    }
}
```

### Default Access Modifiers

```csharp
public class DefaultModifiers
{
    // Default: private
    string _implicitlyPrivate = "Private";
    int ImplicitlyPrivateField;
    
    // To make it public, must explicitly specify
    public string PublicField;
    public int PublicProperty { get; set; }
    
    // Methods are private by default too
    void PrivateMethod() { } // No modifier = private
    
    public void PublicMethod() { } // Must specify public
}
```

## Property Scope

```csharp
public class PropertyScope
{
    // Public property with private backing field
    private int _privateValue;
    
    public int Value
    {
        get { return _privateValue; }
        set { _privateValue = value; }
    }
    
    // Auto-property (backing field created automatically)
    public string Name { get; set; }
    
    // Read-only property
    public DateTime CreatedDate { get; } = DateTime.Now;
    
    // Init-only property (C# 9.0+)
    public int Age { get; init; }
    
    // With different access levels
    public string Description { get; private set; }
    
    public PropertyScope(int age)
    {
        Age = age; // Can set init-only in constructor
    }
}

public class PropertyAccess
{
    public void DemoProperties()
    {
        var obj = new PropertyScope(25);
        
        obj.Value = 10; // OK - public property
        Console.WriteLine(obj.Value); // OK
        
        obj.Name = "Alice"; // OK - auto-property
        Console.WriteLine(obj.Name); // OK
        
        Console.WriteLine(obj.CreatedDate); // OK - read-only property
        
        Console.WriteLine(obj.Age); // OK - can read
        // obj.Age = 30; // ERROR - init-only
        
        obj.Description = "Test"; // OK - public setter
        // Actually ERROR - setter is private!
        Console.WriteLine(obj.Description); // OK - getter is public
    }
}
```

## Nested Class Scope

```csharp
public class OuterClass
{
    private string _outerPrivate = "Outer Private";
    public string OuterPublic = "Outer Public";
    
    // Public nested class
    public class PublicNested
    {
        public void Access()
        {
            // Cannot access outer private members
            // var outer = _outerPrivate; // ERROR
            // But can if using outer instance
        }
    }
    
    // Private nested class
    private class PrivateNested
    {
        public void Access(OuterClass outer)
        {
            // Can access outer private members via instance
            Console.WriteLine(outer._outerPrivate); // OK
            Console.WriteLine(outer.OuterPublic); // OK
        }
    }
}
```

## Static vs Instance Scope

```csharp
public class StaticVsInstance
{
    // Static member - class scope, shared across all instances
    public static int StaticCounter = 0;
    private static string _staticField;
    
    // Instance member - object scope, unique per instance
    public int InstanceValue = 0;
    private string _instanceField;
    
    public void Demonstrate()
    {
        // Static members are accessed via class name
        StaticVsInstance.StaticCounter++;
        
        // Instance members are accessed via this
        this.InstanceValue++;
        
        // Both can be accessed in instance methods
        Console.WriteLine(StaticCounter);
        Console.WriteLine(InstanceValue);
    }
}

public class StaticAccess
{
    public void Demo()
    {
        // Access static directly
        StaticVsInstance.StaticCounter++;
        
        // Need instance for instance members
        var obj = new StaticVsInstance();
        obj.InstanceValue++;
        
        // Cannot access InstanceValue without instance
        // StaticVsInstance.InstanceValue++; // ERROR
    }
}
```

## Inheritance and Scope

```csharp
public class Parent
{
    public string PublicMember = "Public";
    protected string ProtectedMember = "Protected";
    private string PrivateMember = "Private";
}

public class Child : Parent
{
    public void Access()
    {
        Console.WriteLine(PublicMember); // OK - public
        Console.WriteLine(ProtectedMember); // OK - protected in derived
        // Console.WriteLine(PrivateMember); // ERROR - private not inherited
    }
}

public class Unrelated
{
    public void Access()
    {
        var parent = new Parent();
        Console.WriteLine(parent.PublicMember); // OK - public
        // Console.WriteLine(parent.ProtectedMember); // ERROR - not in derived
        // Console.WriteLine(parent.PrivateMember); // ERROR - private
    }
}
```

## Scope Access Summary Table

```csharp
/*
┌─────────────────────┬──────┬────────┬──────────┬──────────┬─────────────────┐
│ Modifier            │Class │Package │Derived   │Same Asm  │Different Asm    │
├─────────────────────┼──────┼────────┼──────────┼──────────┼─────────────────┤
│ public              │ ✓    │ ✓      │ ✓        │ ✓        │ ✓               │
│ protected           │ ✓    │ ✓      │ ✓        │ ✓        │ ✗               │
│ internal            │ ✓    │ ✓      │ ✓        │ ✓        │ ✗               │
│ private protected   │ ✓    │ ✓      │ ✓        │ ✗        │ ✗               │
│ protected internal  │ ✓    │ ✓      │ ✓        │ ✓        │ ✓               │
│ private             │ ✓    │ ✗      │ ✗        │ ✗        │ ✗               │
│ (default/private)   │ ✓    │ ✗      │ ✗        │ ✗        │ ✗               │
└─────────────────────┴──────┴────────┴──────────┴──────────┴─────────────────┘
(Note: ✓ = accessible, ✗ = not accessible)
*/
```

## Best Practices

1. **Use Most Restrictive Access**: Start with private, broaden if needed
2. **Expose via Public Interface**: Use properties instead of public fields
3. **Document API Boundaries**: Clear public vs internal contracts
4. **Prefer Composition Over Inheritance**: When possible, to avoid protected scope issues
5. **Use Private for Implementation**: Hide internal details
6. **Mark Sealed When Appropriate**: Prevent unexpected inheritance

## Common Mistakes

1. **Public Fields Instead of Properties**: Loses encapsulation
2. **Over-Exposing Protected Members**: Consider if you really need inheritance
3. **Confusing Internal and Protected**: Different scope boundaries
4. **Forgetting Default is Private**: Accidentally exposing implementation
5. **Not Using private protected**: When assembly-scoped inheritance matters

## Summary

Class scope and access modifiers form the foundation of encapsulation in C#. They control visibility and accessibility of class members across different contexts (same class, derived classes, same assembly, different assembly). Proper use of access modifiers creates maintainable, secure, and clear APIs while hiding implementation details and protecting against unintended modifications.
