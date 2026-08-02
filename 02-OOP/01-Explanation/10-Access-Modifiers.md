# Access Modifiers Deep Dive

## Overview
Access modifiers control visibility and accessibility of types and members. Understanding them is critical for proper encapsulation.

## Public

### Accessible Everywhere
```csharp
public class PublicClass
{
    public string PublicField = "Accessible anywhere";
    
    public void PublicMethod()
    {
        Console.WriteLine("Public method");
    }
}

// Usage anywhere - same namespace or different
var obj = new PublicClass();
obj.PublicMethod();
Console.WriteLine(obj.PublicField);
```

### Use Case
```csharp
// API surface - intentionally exposed
public class UserService
{
    public async Task<User> GetUserAsync(int id)
    {
        // Public API
    }
}

// Library classes exposed to consumers
public interface IRepository<T>
{
    Task<T> GetAsync(int id);
}
```

## Private

### Accessible Only Within Same Class
```csharp
public class PrivateExample
{
    private string _secret = "Only in this class";
    
    private void PrivateMethod()
    {
        Console.WriteLine(_secret);
    }
    
    public void PublicMethod()
    {
        PrivateMethod(); // OK - same class
    }
}

// Usage outside class
var obj = new PrivateExample();
// obj.PrivateMethod(); // ERROR: cannot access
// obj._secret = "test"; // ERROR: cannot access
```

### Nested Classes
```csharp
public class Outer
{
    private string _outerSecret = "Secret";
    
    private class Inner // Private nested class
    {
        public void AccessOuter()
        {
            var outer = new Outer();
            // Can access private members of outer
            Console.WriteLine(outer._outerSecret);
        }
    }
}

// Usage
var outer = new Outer();
// var inner = new Outer.Inner(); // ERROR: Inner is private
```

## Protected

### Accessible in Same Class and Derived Classes
```csharp
public class Base
{
    protected string ProtectedField = "Accessible in derived";
    
    protected void ProtectedMethod()
    {
        Console.WriteLine("Protected method");
    }
}

public class Derived : Base
{
    public void CallProtected()
    {
        ProtectedMethod(); // OK - derived class
        Console.WriteLine(ProtectedField); // OK
    }
}

// Usage outside hierarchy
var derived = new Derived();
derived.CallProtected(); // OK
// derived.ProtectedMethod(); // ERROR: protected, not public
```

### Protected with Different Access in Derived
```csharp
public class Base
{
    protected virtual void DoSomething()
    {
        Console.WriteLine("Base");
    }
}

public class Derived : Base
{
    public override void DoSomething() // Public in derived
    {
        base.DoSomething();
        Console.WriteLine("Derived");
    }
}

// Usage
Base b = new Derived();
b.DoSomething(); // Public in derived, so OK
```

## Internal

### Accessible Within Same Assembly
```csharp
// In Assembly A
internal class InternalClass
{
    internal void InternalMethod()
    {
    }
}

public class PublicClass
{
    public void UseInternal()
    {
        var internal = new InternalClass(); // OK - same assembly
        internal.InternalMethod();
    }
}

// In Assembly B (different)
// var obj = new InternalClass(); // ERROR: internal, different assembly
// var pub = new PublicClass().UseInternal(); // OK, but InternalClass not accessible
```

### Use Case
```csharp
// Internal helper classes not exposed to consumers
internal class CacheImplementation
{
    // Implementation detail
}

// Public API
public class CacheService
{
    private readonly CacheImplementation _cache = new();
    
    public void Set(string key, object value)
    {
        _cache.Store(key, value);
    }
}
```

## Private Protected (C# 7.2+)

### Accessible in Derived Classes Within Same Assembly
```csharp
public class Base
{
    private protected string Secret = "Derived in same assembly only";
    
    private protected void Method()
    {
    }
}

public class Derived : Base
{
    public void Access()
    {
        Console.WriteLine(Secret); // OK - derived, same assembly
        Method(); // OK
    }
}

// In different assembly
public class DerivedOther : Base
{
    // public void Access()
    // {
    //     Console.WriteLine(Secret); // ERROR: private protected, different assembly
    // }
}

// Outside hierarchy, same assembly
// var obj = new Base();
// obj.Method(); // ERROR: private protected
```

## Protected Internal (C# 7.2+)

### Accessible in Derived Classes OR Same Assembly
```csharp
public class Base
{
    protected internal string Data = "Derived anywhere or same assembly";
    
    protected internal void Method()
    {
    }
}

// Same assembly, not derived - OK
var obj = new Base();
obj.Method(); // OK - same assembly

// Different assembly, derived - OK
public class Derived : Base
{
    public void Access()
    {
        Console.WriteLine(Data); // OK - derived
        Method(); // OK
    }
}

// Different assembly, not derived - ERROR
// var other = new Base();
// other.Method(); // ERROR: protected internal, different assembly
```

## Type-Level Modifiers

### Class, Interface, Struct
```csharp
// Public - accessible anywhere
public class PublicClass { }

// Internal - accessible only in assembly (default for classes)
internal class InternalClass { }

// No protected/private at type level (only for members)

// Public interface
public interface IService { }

// Internal struct
internal struct Point { }
```

### Nested Types
```csharp
public class Outer
{
    // Public nested class
    public class NestedPublic { }
    
    // Private nested class
    private class NestedPrivate { }
    
    // Protected nested class
    protected class NestedProtected { }
    
    // Internal nested class
    internal class NestedInternal { }
}
```

## Modifier Combinations

### Member Modifiers
```csharp
public class Example
{
    // Valid combinations
    public void PublicMethod() { }
    private void PrivateMethod() { }
    protected void ProtectedMethod() { }
    internal void InternalMethod() { }
    protected internal void ProtectedInternalMethod() { }
    private protected void PrivateProtectedMethod() { }
    
    // Valid with static
    public static void StaticPublic() { }
    private static void StaticPrivate() { }
    
    // Valid with virtual (only protected+ for virtual)
    public virtual void VirtualPublic() { }
    protected virtual void VirtualProtected() { }
    
    // Invalid combinations
    // public private void Invalid() { } // ERROR
    // protected private void Invalid() { } // ERROR
}
```

## Best Practices

1. **Principle of Least Privilege**
```csharp
// Bad: Everything public
public class BadClass
{
    public string _internalState;
    public void _internalMethod() { }
}

// Good: Only expose what's needed
public class GoodClass
{
    private string _internalState;
    private void _internalMethod() { }
    
    public void PublicMethod()
    {
        _internalMethod();
    }
}
```

2. **Use Internal for Assembly Implementation**
```csharp
// Good: Hide implementation, expose interface
public interface ICache { }

internal class MemoryCache : ICache { }
internal class RedisCache : ICache { }

public class CacheFactory
{
    public static ICache CreateCache() // Returns interface, not internal implementation
    {
        return new MemoryCache();
    }
}
```

3. **Protected for Extensible Classes**
```csharp
// Good: Allow extension by derived classes
public abstract class BaseService
{
    protected virtual void ValidateInput(object input)
    {
        // Default validation
    }
    
    protected abstract void ExecuteCore();
    
    public void Execute(object input)
    {
        ValidateInput(input);
        ExecuteCore();
    }
}
```

4. **Seal When Not Meant for Extension**
```csharp
// Good: Prevent unwanted inheritance
public sealed class FinalService
{
    // Cannot be derived
}

// Or private nested class
public class Factory
{
    private class HiddenImplementation { }
    
    public object Create() => new HiddenImplementation();
}
```

## Common Mistakes

1. **Public Fields (Encapsulation Violation)**
```csharp
// Bad: Direct field access, no validation
public class BadClass
{
    public int Value; // Anyone can set to any value
}

// Good: Property with validation
public class GoodClass
{
    private int _value;
    public int Value
    {
        get { return _value; }
        set { _value = Math.Max(0, value); }
    }
}
```

2. **Protected When Private Suffices**
```csharp
// Bad: Exposes too much
public class BadBase
{
    protected void HelperMethod() { } // Derived classes might depend on this
}

// Good: Private unless needed by derived
public class GoodBase
{
    private void HelperMethod() { }
    
    protected virtual void ExtensionPoint() { }
}
```

3. **Inconsistent Access Levels**
```csharp
// Bad: Getter public, setter inconsistent
public class BadClass
{
    public int Id { get; set; } // Both public
}

// Good: Intentional asymmetry
public class GoodClass
{
    public int Id { get; private set; } // Public get, private set
}
```

4. **Confusing Internal with Hidden**
```csharp
// Mistake: Internal is still visible in same assembly
internal class Helper { }

// Consumers in same assembly can still see it:
using ReflectionHelpers;

var type = typeof(Helper); // Accessible via reflection!

// Use private for true hiding
private class TrulyPrivate { }
```

## Quick Summary
- public: Accessible everywhere
- private: Accessible only in same class (default for members)
- protected: Accessible in derived classes
- internal: Accessible in same assembly (default for types)
- protected internal: Accessible in derived or same assembly
- private protected: Accessible in derived classes in same assembly
- Use least privilege principle
- Prefer private, promote to protected/public only when needed
- Hide implementation, expose interfaces
- Seal classes when not meant for inheritance
- Properties over public fields

## Resources
- Access Modifiers (C# documentation)
- Encapsulation Principles
- SOLID Principles (especially I and D)
- Library Design Guidelines
