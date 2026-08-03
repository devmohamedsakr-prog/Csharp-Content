# Variable Shadowing in C#

## Overview

Variable shadowing occurs when a variable declared in an inner scope has the same name as a variable in an outer scope. The inner variable "hides" the outer variable within that scope. While sometimes unintentional, shadowing can cause bugs and confusion.

## Understanding Variable Shadowing

### Basic Shadowing

```csharp
public class BasicShadowing
{
    private int _value = 5; // Class scope
    
    public void Method()
    {
        int _value = 10; // Local scope - shadows class field
        
        Console.WriteLine(_value); // Prints 10 (local variable)
        Console.WriteLine(this._value); // Prints 5 (class field via explicit reference)
    }
    
    public void Demonstrate()
    {
        Console.WriteLine(_value); // Prints 5 (class field, no local shadowing here)
    }
}
```

### Block-Level Shadowing

```csharp
public class BlockShadowing
{
    public void Method()
    {
        int x = 5; // Outer scope
        
        {
            int x = 10; // Block scope - shadows outer x
            Console.WriteLine(x); // Prints 10
        }
        
        Console.WriteLine(x); // Prints 5
        
        // Can declare new x in another block
        {
            int x = 20; // Another block scope
            Console.WriteLine(x); // Prints 20
        }
    }
}
```

### Nested Block Shadowing

```csharp
public class NestedBlockShadowing
{
    public void Method()
    {
        int value = 1;
        Console.WriteLine(value); // 1
        
        {
            int value = 2; // Shadows outer
            Console.WriteLine(value); // 2
            
            {
                int value = 3; // Shadows middle
                Console.WriteLine(value); // 3
            }
            
            Console.WriteLine(value); // 2
        }
        
        Console.WriteLine(value); // 1
    }
}
```

## Shadowing in Control Structures

### If Statements

```csharp
public class IfShadowing
{
    public void Example()
    {
        string message = "Outer"; // Outer scope
        
        if (true)
        {
            string message = "Inner"; // Shadows outer
            Console.WriteLine(message); // "Inner"
        }
        
        Console.WriteLine(message); // "Outer"
    }
}
```

### Loop Shadowing

```csharp
public class LoopShadowing
{
    public void Example()
    {
        int count = 0; // Outer scope
        
        for (int i = 0; i < 3; i++)
        {
            int count = i + 1; // Shadows outer count
            Console.WriteLine(count); // 1, 2, 3
        }
        
        Console.WriteLine(count); // 0 (unchanged)
    }
    
    public void ForEachExample()
    {
        string item = "Original"; // Outer scope
        
        foreach (string item in new[] { "A", "B", "C" }) // ERROR: item declared twice
        {
            // Cannot shadow loop variable - this is a compile error
            // The loop variable is the item here
        }
    }
}
```

### Try-Catch Shadowing

```csharp
public class TryCatchShadowing
{
    public void Example()
    {
        int result = 0; // Outer scope
        
        try
        {
            int result = ProcessData(); // ERROR: Cannot shadow in same scope
        }
        catch
        {
            // Catch doesn't create new scope for existing variables
        }
    }
    
    public void CatchBlockExample()
    {
        string error = "none"; // Outer scope
        
        try
        {
            // Some code
        }
        catch (Exception ex)
        {
            string errorDetail = ex.Message; // Different name - no shadowing
            Console.WriteLine(errorDetail); // OK
        }
        
        // errorDetail not accessible here
    }
}
```

## Class Member Shadowing

### Field and Method Shadowing

```csharp
public class Parent
{
    public virtual void Method()
    {
        Console.WriteLine("Parent Method");
    }
    
    public void Helper()
    {
        Console.WriteLine("Parent Helper");
    }
}

public class Child : Parent
{
    public override void Method() // Override (polymorphism) - not shadowing
    {
        Console.WriteLine("Child Method");
    }
    
    public new void Helper() // NEW - shadows parent's Helper (shadowing!)
    {
        Console.WriteLine("Child Helper");
    }
}

public class Usage
{
    public void Demo()
    {
        Parent p = new Child();
        Child c = new Child();
        
        p.Method(); // "Child Method" - virtual dispatch
        p.Helper(); // "Parent Helper" - new doesn't participate in polymorphism
        
        c.Method(); // "Child Method"
        c.Helper(); // "Child Helper"
    }
}
```

### Field Shadowing

```csharp
public class Parent
{
    public int Value = 5;
}

public class Child : Parent
{
    public new int Value = 10; // Shadows parent's Value field
}

public class FieldShadowing
{
    public void Demo()
    {
        Parent p = new Child();
        Child c = new Child();
        
        Console.WriteLine(p.Value); // 5 (accesses Parent's Value)
        Console.WriteLine(c.Value); // 10 (accesses Child's Value)
    }
}
```

### Property Shadowing

```csharp
public class ParentClass
{
    public string Name { get; set; }
}

public class ChildClass : ParentClass
{
    public new string Name { get; set; } // Shadows parent property
}
```

## Parameter Shadowing

### Parameter vs Field Shadowing

```csharp
public class ParameterShadowing
{
    private int _value = 5;
    
    public void Method(int _value) // Parameter shadows field
    {
        Console.WriteLine(_value); // Prints parameter (10)
        Console.WriteLine(this._value); // Prints field (5)
    }
    
    public void Demonstrate()
    {
        Method(10);
    }
}
```

### Constructor Parameter Shadowing

```csharp
public class Person
{
    private string _name;
    
    public Person(string _name) // Parameter shadows field
    {
        // Can use 'this' to distinguish
        this._name = _name; // Assigns parameter to field
    }
}
```

## Lambda and Closure Shadowing

### Variable Capture with Shadowing

```csharp
public class LambdaShadowing
{
    public void Example()
    {
        int x = 5; // Outer scope
        
        Func<int> func1 = () =>
        {
            // Cannot declare new x here - would shadow
            // int x = 10; // ERROR: x already declared
            return x * 2; // Uses captured x
        };
        
        // After the lambda
        {
            int x = 10; // New block scope - different x
            Console.WriteLine(func1()); // Still 10 (captured original)
        }
    }
}
```

## LINQ Shadowing

### LINQ Query Variables

```csharp
public class LinqShadowing
{
    public void Example()
    {
        var numbers = new[] { 1, 2, 3, 4, 5 };
        
        var result = from num in numbers // num is range variable
                     where num > 2
                     select num;
        
        // In another scope
        {
            var result = from num in numbers // Can't shadow result in same scope
                         select num * 2;
        }
    }
}
```

## When Shadowing is Detected

### Compiler Warning: CS0219

```csharp
public class CompilerWarning
{
    public void Method()
    {
        int x = 5; // Outer x
        
        {
            int x = 10; // Compiler may warn: shadowing
            // CS0219: Variable 'x' is assigned but never used
        }
    }
}
```

### Compiler Error: Duplicate Declaration

```csharp
public class CompilerError
{
    public void Method()
    {
        int x = 5;
        
        // int x = 10; // ERROR: local variable or parameter named 'x' already defined
    }
}
```

## Detecting and Avoiding Shadowing

### Using 'new' Keyword for Inheritance

```csharp
public class Parent
{
    public void Greet()
    {
        Console.WriteLine("Parent");
    }
}

public class Child : Parent
{
    public new void Greet() // Explicitly shadows parent method
    {
        Console.WriteLine("Child");
    }
}
```

### Naming Conventions to Prevent Shadowing

```csharp
public class GoodNaming
{
    private string _classField; // Prefix with _ for class fields
    private int _value;
    
    public void Method(string parameter) // Clear parameter name
    {
        string localVariable = parameter; // Clear local variable
        
        if (true)
        {
            string blockVariable = localVariable; // Clear block variable
            Console.WriteLine(blockVariable);
        }
    }
}
```

### Using IDE Warnings

```csharp
public class UsingIdleSuggestions
{
    // Most IDEs warn about shadowing:
    // Visual Studio: Suggests renaming to avoid confusion
    // ReSharper: Highlights shadowing issues
    // .editorconfig: Can enforce rules against shadowing
}
```

## Best Practices

1. **Avoid Shadowing**: Use different variable names in different scopes
2. **Use Naming Conventions**: Distinguish class fields, parameters, locals
3. **Explicit Intent**: If shadowing is intentional, use 'new' keyword explicitly
4. **IDE Configuration**: Enable shadowing warnings in your IDE
5. **Code Review**: Have peers review for unintended shadowing
6. **Use 'this'**: Explicitly reference class members when needed
7. **Consistent Naming**: Follow team conventions for scope indicators

## Common Mistakes

1. **Accidental Shadowing**: Using same name in different scope by accident
2. **Shadowing Parameters**: Declaring local variable with same name as parameter
3. **Inherited Member Shadowing**: Using 'new' without understanding impact
4. **Complex Shadowing Chains**: Multiple levels of shadowing causing confusion
5. **Forgetting Scope**: Not realizing variable is shadowed in nested block

## Anti-Patterns

```csharp
// ANTI-PATTERN: Confusing shadowing
public class AntiPattern
{
    private int value = 5;
    
    public void Process()
    {
        int value = 10; // CONFUSING - shadows field
        
        if (value > 8)
        {
            int value = 15; // VERY CONFUSING - multiple levels
            Console.WriteLine(value);
        }
    }
}

// BETTER: Clear naming
public class BetterPattern
{
    private int _instanceValue = 5;
    
    public void Process()
    {
        int localValue = 10; // CLEAR - different name
        
        if (localValue > 8)
        {
            int blockValue = 15; // CLEAR - distinct name
            Console.WriteLine(blockValue);
        }
    }
}
```

## Summary

Variable shadowing occurs when variables in inner scopes have the same name as outer scope variables. While C# allows shadowing in many cases, it's generally considered poor practice due to readability and maintenance issues. Understanding shadowing mechanisms helps prevent bugs and write clearer code. Using consistent naming conventions, keeping scopes narrow, and avoiding unnecessary name reuse are key strategies for preventing shadowing-related issues.
