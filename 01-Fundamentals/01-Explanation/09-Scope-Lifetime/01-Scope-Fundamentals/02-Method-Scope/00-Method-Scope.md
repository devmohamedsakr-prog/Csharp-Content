# Method Scope in C#

## Overview

Method scope defines the visibility of local variables and parameters within a method. Variables declared in a method are only accessible within that method and are destroyed when the method returns.

## Understanding Method Scope

### Method Local Variables

```csharp
public class MethodScopeDemo
{
    public void MethodA()
    {
        int localVar = 10; // Method-local scope
        Console.WriteLine(localVar); // OK
    }
    
    public void MethodB()
    {
        // Console.WriteLine(localVar); // ERROR: localVar not accessible here
    }
    
    public void MethodC()
    {
        int localVar = 20; // Different localVar - separate method scope
        Console.WriteLine(localVar); // OK - prints 20
    }
}
```

### Method Parameters

```csharp
public class MethodParameterScope
{
    public void ProcessData(int value, string name)
    {
        // value and name are method-local parameters
        Console.WriteLine($"Value: {value}, Name: {name}");
        
        // Can be reassigned
        value = value * 2;
        name = name.ToUpper();
        
        Console.WriteLine($"Value: {value}, Name: {name}");
    }
    
    public void CallMethod()
    {
        int myValue = 5;
        string myName = "alice";
        
        ProcessData(myValue, myName);
        
        // myValue and myName are unchanged (value types passed by value)
        Console.WriteLine($"Original - Value: {myValue}, Name: {myName}");
    }
}
```

### Local Variables vs Parameters

```csharp
public class LocalVsParameters
{
    public void Calculate(int inputValue)
    {
        // inputValue is a parameter (method scope)
        
        int result = inputValue * 2; // Local variable (method scope)
        string message = "Result: "; // Local variable (method scope)
        
        Console.WriteLine(message + result);
        
        // All three (inputValue, result, message) are method-scoped
        // All are destroyed when method returns
    }
}
```

## Method Scope Lifetime

### Variables Are Created on Method Call

```csharp
public class MethodLifetime
{
    private int _callCount = 0;
    
    public void CountCalls()
    {
        _callCount++; // Class-scoped variable - persists
        
        int localCount = _callCount; // Method-scoped - created each call
        
        Console.WriteLine($"Call #{_callCount}, Local: {localCount}");
    }
    
    public void Demonstrate()
    {
        CountCalls(); // _callCount = 1, localCount created/destroyed
        CountCalls(); // _callCount = 2, NEW localCount created/destroyed
        CountCalls(); // _callCount = 3, NEW localCount created/destroyed
        
        // Each call creates a fresh localCount variable
    }
}
```

### Recursive Method Scope

```csharp
public class RecursiveScope
{
    public int Factorial(int n)
    {
        int result; // Each recursive call has its own result variable
        
        if (n <= 1)
        {
            result = 1;
        }
        else
        {
            result = n * Factorial(n - 1); // Each call has separate scope
        }
        
        return result;
    }
    
    public void Demonstrate()
    {
        // Factorial(3) call stack:
        // Factorial(3): result variable #1
        //   Factorial(2): result variable #2
        //     Factorial(1): result variable #3
        //       returns 1
        //     result #2 = 2 * 1 = 2, returns
        //   result #1 = 3 * 2 = 6, returns
        
        int answer = Factorial(3);
        Console.WriteLine(answer); // 6
    }
}
```

## Scope Differences Between Method Types

### Instance Methods

```csharp
public class InstanceMethodScope
{
    private string _instanceField = "Instance";
    
    public void InstanceMethod()
    {
        int localVar = 1; // Method scope
        
        Console.WriteLine(_instanceField); // Can access instance field
        Console.WriteLine(localVar); // Can access local variable
    }
    
    public void AnotherInstanceMethod()
    {
        // Cannot access localVar from InstanceMethod - different method scope
        Console.WriteLine(_instanceField); // OK - instance field accessible
    }
}
```

### Static Methods

```csharp
public class StaticMethodScope
{
    private static string _staticField = "Static";
    
    public static void StaticMethod()
    {
        int localVar = 1; // Method scope
        
        Console.WriteLine(_staticField); // Can access static field
        Console.WriteLine(localVar); // Can access local variable
        
        // Cannot access instance fields or call instance methods
    }
    
    public static void Demonstrate()
    {
        StaticMethod(); // Can call from static context
    }
}
```

### Constructor Scope

```csharp
public class ConstructorScope
{
    private string _name;
    private int _age;
    
    public ConstructorScope(string name, int age)
    {
        // name and age are parameters (constructor scope)
        _name = name; // Assign to instance field
        _age = age;
        
        int tempValue = age * 2; // Local variable (constructor scope)
        Console.WriteLine($"Initializing: {_name}, temp: {tempValue}");
        // tempValue destroyed after constructor completes
    }
    
    public void DisplayInfo()
    {
        // _name and _age are accessible (instance fields)
        // name, age, tempValue are NOT accessible (constructor scope)
        Console.WriteLine($"Name: {_name}, Age: {_age}");
    }
}
```

## Method Scope Interactions

### Calling Other Methods

```csharp
public class MethodChaining
{
    private int _sharedValue = 0;
    
    public void MethodA()
    {
        int localA = 5; // Method A's scope
        _sharedValue = localA;
        
        MethodB(); // Call MethodB
        // MethodB's local variables are not accessible here
    }
    
    public void MethodB()
    {
        int localB = _sharedValue; // Can access class field set by MethodA
        // Cannot access localA - different method scope
        
        localB = localB * 2;
        MethodC(localB); // Pass value to MethodC
    }
    
    public void MethodC(int parameter)
    {
        // parameter is MethodC's scope
        Console.WriteLine(parameter);
        // Cannot access localA or localB - different method scopes
    }
}
```

### Passing by Reference

```csharp
public class PassByReferenceScope
{
    public void ModifyArray(int[] array)
    {
        // array parameter refers to original array
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = array[i] * 2; // Modifies original
        }
        
        // array variable scope ends here, but array object persists
    }
    
    public void Demonstrate()
    {
        int[] myArray = { 1, 2, 3 };
        ModifyArray(myArray);
        
        foreach (int num in myArray)
        {
            Console.WriteLine(num); // Prints 2, 4, 6 (modified by ModifyArray)
        }
    }
}
```

### Return Values and Scope

```csharp
public class ReturnValueScope
{
    public int CalculateValue(int input)
    {
        int result = input * 2; // Local variable
        int intermediate = input + 5; // Local variable
        
        return result; // intermediate is destroyed, result value is returned
    }
    
    public object ComplexReturn()
    {
        var localList = new List<int> { 1, 2, 3 }; // Local reference
        return localList; // Returns reference (object persists on heap)
    }
    
    public void Demonstrate()
    {
        int value = CalculateValue(5);
        // result and intermediate are destroyed
        // value contains the returned value (10)
        
        object obj = ComplexReturn();
        // localList reference is destroyed, but list object persists
    }
}
```

## Method Scope with Lambda and Anonymous Functions

```csharp
public class MethodScopeWithLambda
{
    public void DemoLambda()
    {
        int methodLocal = 10; // Method scope
        
        // Lambda captures methodLocal (closure)
        Func<int, int> multiply = x => x * methodLocal;
        
        Console.WriteLine(multiply(5)); // 50 (methodLocal is 10)
        
        methodLocal = 20;
        Console.WriteLine(multiply(5)); // 100 (methodLocal is now 20)
    }
}
```

## Method Scope Rules and Behaviors

### Rule 1: Method-Local Variables Don't Persist Between Calls

```csharp
public class PersistenceRule
{
    public void Counter()
    {
        int count = 0; // Each call starts with count = 0
        count++;
        Console.WriteLine(count); // Always prints 1
    }
    
    public void Demonstrate()
    {
        Counter(); // Prints 1
        Counter(); // Prints 1 (count doesn't persist)
        Counter(); // Prints 1
    }
}
```

### Rule 2: Each Method Call Has Separate Stack Frame

```csharp
public void Method()
{
    int value = 5;
} // value destroyed, stack frame popped

public void Method()
{
    int value = 10; // Different value, different stack frame
} // value destroyed, stack frame popped
```

### Rule 3: Method Parameters Are in Method Scope

```csharp
public void ProcessData(int value, string name)
{
    // value and name are in method scope
    // Can be modified but changes don't affect original for value types
    value = value * 2;
    name = name.ToUpper();
}
```

## Best Practices

1. **Keep Variables Local**: Declare variables in methods, not as class fields unnecessarily
2. **Use Parameters Over Class Fields**: When values are only needed in one method
3. **Clear Variable Names**: Make scope intent obvious through naming
4. **Return Values Explicitly**: Rather than relying on side effects and class fields
5. **Document Method Dependencies**: Show what a method depends on (parameters vs class state)

## Common Issues

- **Accessing Variables After Method Returns**: Variables destroyed when method ends
- **Forgetting About Stack Frames**: Each call creates fresh variables
- **Unintended Variable Sharing**: Using class fields when method-local variables would be appropriate
- **Parameter Confusion**: Forgetting parameters are method-scoped
- **Return Value Scope**: Returning references to local objects (works but can be confusing)

## Summary

Method scope defines the lifetime of local variables and parameters within a method. Each method call creates a new stack frame with its own set of local variables that persist for the duration of the method execution. Understanding method scope is crucial for proper variable management and prevents unintended state sharing between method calls.
