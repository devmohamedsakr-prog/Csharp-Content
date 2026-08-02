# Scope and Lifetime

## Overview
Scope determines where a variable can be accessed. Lifetime is how long a variable exists in memory.

## Variable Scope

### Block Scope
```csharp
public class ScopeDemo
{
    public void Method()
    {
        int x = 5; // Method scope
        
        if (x > 0)
        {
            int y = 10; // Block scope
            Console.WriteLine(y); // OK
        }
        
        // Console.WriteLine(y); // ERROR: y not in scope
        Console.WriteLine(x); // OK
    }
}
```

### Method Scope
```csharp
public class MethodScope
{
    private string _classField = "Class"; // Class/instance scope
    
    public void Method()
    {
        string methodLocal = "Method"; // Method scope - exists only in this method
        Console.WriteLine(methodLocal);
    }
    
    public void AnotherMethod()
    {
        // Console.WriteLine(methodLocal); // ERROR: methodLocal not accessible here
        Console.WriteLine(_classField); // OK: Class field accessible
    }
}
```

### Loop Scope (C# Scoping Rules)
```csharp
// Bad: i is accessible outside loop (confusing)
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}
// Console.WriteLine(i); // ERROR in modern C# (i out of scope)

// Good: Clear block scope
{
    int j = 5;
    Console.WriteLine(j);
}
// Console.WriteLine(j); // ERROR
```

## Class Member Scope

### Access Modifiers
```csharp
public class AccessScopes
{
    // Public - accessible everywhere
    public string PublicField = "Public";
    
    // Private - accessible only within this class
    private string _privateField = "Private";
    
    // Protected - accessible in this class and derived classes
    protected string ProtectedField = "Protected";
    
    // Internal - accessible within same assembly
    internal string InternalField = "Internal";
    
    // Private protected - accessible in this class and derived classes in same assembly
    private protected string PrivateProtectedField = "PrivateProtected";
}

public class Derived : AccessScopes
{
    public void Access()
    {
        Console.WriteLine(PublicField); // OK
        // Console.WriteLine(_privateField); // ERROR
        Console.WriteLine(ProtectedField); // OK
    }
}
```

## Namespace Scope

### Organizing Code
```csharp
namespace MyApp.Domain
{
    public class User { }
    
    namespace Repositories
    {
        public class UserRepository { }
    }
}

namespace MyApp.Services
{
    using MyApp.Domain; // Import namespace
    
    public class UserService
    {
        private User _user; // Accessible via using
        private Repositories.UserRepository _repo; // Nested namespace
    }
}
```

## Object Lifetime

### Stack vs Heap
```csharp
public class LifetimeDemo
{
    public void Demonstrate()
    {
        // Value types on STACK - automatically freed when out of scope
        int x = 5; // Stack memory
        
        // Reference types on HEAP - garbage collected
        var person = new Person { Name = "Alice" }; // Reference on stack, object on heap
        
        MyMethod();
        
        // After this method ends:
        // - x is automatically freed (stack)
        // - person reference goes out of scope, object eligible for GC (heap)
    }
    
    private void MyMethod()
    {
        string s = "hello"; // Stack: reference, Heap: string object
        // When method returns, s goes out of scope
        // String object eligible for garbage collection
    }
}

public class Person
{
    public string Name { get; set; }
}
```

## Variable Shadowing

### Name Conflicts
```csharp
public class Shadowing
{
    private int _value = 5; // Class scope
    
    public void Method()
    {
        int _value = 10; // Local scope - shadows class field
        Console.WriteLine(_value); // Prints 10 (local takes precedence)
        Console.WriteLine(this._value); // Prints 5 (explicit class reference)
    }
}

// Bad: Easy to confuse
public class BadShadowing
{
    public string Name = "Class";
    
    public void Process()
    {
        string Name = "Local"; // Shadows class field - confusing!
        Console.WriteLine(Name);
    }
}

// Good: Use clear naming
public class GoodNaming
{
    private string _classNameField = "Class";
    
    public void Process()
    {
        string localName = "Local"; // No shadowing, clear intent
        Console.WriteLine(localName);
    }
}
```

## Closure and Captured Variables

### Variable Capture
```csharp
public class Closures
{
    public Func<int> CreateCounter()
    {
        int count = 0; // Captured variable
        
        return () => ++count; // Closure captures count
    }
}

public class ClosureDemo
{
    public static void Main()
    {
        var closures = new Closures();
        
        var counter = closures.CreateCounter();
        
        Console.WriteLine(counter()); // 1
        Console.WriteLine(counter()); // 2
        Console.WriteLine(counter()); // 3
        // count persists across calls!
    }
}

// Common mistake: Loop variable closure
public class LoopClosureProblem
{
    public List<Action> CreateActions()
    {
        var actions = new List<Action>();
        
        // Bad: All closures capture same variable
        for (int i = 0; i < 3; i++)
        {
            actions.Add(() => Console.WriteLine(i)); // All capture same 'i'
        }
        
        return actions;
    }
    
    public static void Main()
    {
        var closures = new LoopClosureProblem();
        var actions = closures.CreateActions();
        
        actions[0](); // Prints 3 (not 0!)
        actions[1](); // Prints 3 (not 1!)
        actions[2](); // Prints 3 (not 2!)
    }
}

// Solution: Create new variable in each iteration
public class LoopClosureFix
{
    public List<Action> CreateActions()
    {
        var actions = new List<Action>();
        
        for (int i = 0; i < 3; i++)
        {
            int temp = i; // New variable each iteration
            actions.Add(() => Console.WriteLine(temp));
        }
        
        return actions;
    }
    
    public static void Main()
    {
        var closures = new LoopClosureFix();
        var actions = closures.CreateActions();
        
        actions[0](); // Prints 0 ✓
        actions[1](); // Prints 1 ✓
        actions[2](); // Prints 2 ✓
    }
}
```

## Using Declaration (C# 8.0+)

### Automatic Resource Management
```csharp
// Old way: Try-finally
public void OldWay()
{
    var file = File.OpenRead("data.txt");
    try
    {
        // Use file
    }
    finally
    {
        file?.Dispose();
    }
}

// Old using statement
public void UsingStatement()
{
    using (var file = File.OpenRead("data.txt"))
    {
        // Use file
        // Disposed automatically at end of block
    }
}

// New using declaration (C# 8.0+)
public void UsingDeclaration()
{
    using var file = File.OpenRead("data.txt");
    // Use file
    // Disposed automatically at end of method
}

// Multiple resources
public void MultipleResources()
{
    using var file1 = File.OpenRead("file1.txt");
    using var file2 = File.OpenRead("file2.txt");
    using var file3 = File.OpenRead("file3.txt");
    
    // Use all files
    // All disposed in reverse order at method end
}
```

## Best Practices

1. **Keep Scope as Small as Possible**
```csharp
// Bad: Wide scope
public int CalculateTotal(int[] numbers)
{
    int result = 0; // Declared at top
    int count = 0;
    
    // Hundreds of lines...
    
    for (int i = 0; i < numbers.Length; i++)
    {
        result += numbers[i];
        count++;
    }
    return result;
}

// Good: Declare close to use
public int CalculateTotal(int[] numbers)
{
    int result = 0;
    for (int i = 0; i < numbers.Length; i++)
    {
        result += numbers[i];
    }
    return result;
}
```

2. **Use Meaningful Names to Avoid Shadowing**
```csharp
// Bad: Confusing shadowing
private int value;
public void Process() { int value; }

// Good: Clear, distinct names
private int _classValue;
public void Process() { int localValue; }
```

3. **Avoid Variable Capture in Loops**
```csharp
// Bad: Unexpected behavior
var actions = Enumerable.Range(0, 3)
    .Select(i => (Action)(() => Console.WriteLine(i)))
    .ToList();

// Good: Capture explicitly
var actions = Enumerable.Range(0, 3)
    .Select(i => (Action)(() => Console.WriteLine(i.ToString()))) // or copy in closure
    .ToList();
```

## Common Mistakes

1. **Using Private Fields Outside Class**
```csharp
// Bad: Compile error
public class MyClass
{
    private string _secret;
}

public class Other
{
    public void Access(MyClass obj)
    {
        // obj._secret; // ERROR
    }
}
```

2. **Accessing Variables Outside Their Scope**
```csharp
// Bad: Variable out of scope
public int Problem()
{
    if (true)
    {
        int local = 5;
    }
    // return local; // ERROR
}
```

3. **Loop Variable Closure (Classic Mistake)**
```csharp
// Bad: All actions do the same thing
var actions = new List<Action>();
for (int i = 0; i < 3; i++)
{
    actions.Add(() => Console.WriteLine(i));
}
// All print 3!

// Good: Capture value
for (int i = 0; i < 3; i++)
{
    int temp = i;
    actions.Add(() => Console.WriteLine(temp));
}
```

## Quick Summary
- Block scope limits visibility
- Access modifiers control member accessibility
- Stack: value types, local variables
- Heap: reference types, objects
- Closures capture variables
- Loop variables: beware of capture
- Using declarations simplify resource management
- Keep scope minimal for clarity
- Shadow variables carefully
- Garbage collection handles heap cleanup

## Resources
- Variable Scope (C# documentation)
- Lifetime and Scope
- Closures and Captured Variables
- Memory Management (.NET)
