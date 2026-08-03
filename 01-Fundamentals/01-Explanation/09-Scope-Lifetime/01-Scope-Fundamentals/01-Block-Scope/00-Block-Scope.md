# Block Scope in C#

## Overview

Block scope defines the visibility and accessibility of variables within code blocks (enclosed by curly braces `{}`). Once a variable is declared within a block, it's only accessible within that block and any nested blocks.

## Understanding Block Scope

### Basic Block Scope Definition

```csharp
public class BlockScopeDemo
{
    public void Demonstrate()
    {
        int x = 5; // Method scope
        
        {
            // Begin new block
            int y = 10; // Block scope - only accessible within this block
            Console.WriteLine($"x = {x}, y = {y}"); // OK: Both accessible
        }
        // End block
        
        Console.WriteLine(x); // OK: x still in scope
        // Console.WriteLine(y); // COMPILE ERROR: y is out of scope
    }
}
```

### Block Scope in Control Flow

#### If Statements

```csharp
public class IfBlockScope
{
    public void CheckValue(int number)
    {
        if (number > 0)
        {
            int result = number * 2; // Block scope - only exists in if block
            Console.WriteLine($"Result: {result}");
        }
        
        // Console.WriteLine(result); // ERROR: result doesn't exist here
    }
    
    public void MultipleBlocks(int value)
    {
        if (value > 10)
        {
            int largeValue = value;
            Console.WriteLine($"Large: {largeValue}");
        }
        else if (value > 0)
        {
            int smallValue = value;
            Console.WriteLine($"Small: {smallValue}");
        }
        else
        {
            int negativeValue = value;
            Console.WriteLine($"Negative: {negativeValue}");
        }
        
        // None of these variables exist here
    }
}
```

#### For Loops

```csharp
public class ForLoopScope
{
    public void LoopExample()
    {
        // Traditional for loop
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(i); // i is block-scoped to the loop
        }
        
        // Console.WriteLine(i); // ERROR: i doesn't exist outside loop
        
        // Nested loops have separate block scopes
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.WriteLine($"({i}, {j})"); // Both accessible
            }
            // Console.WriteLine(j); // ERROR: j out of scope
        }
    }
    
    public void ForEachExample()
    {
        int[] numbers = { 1, 2, 3, 4, 5 };
        
        foreach (int num in numbers)
        {
            Console.WriteLine(num); // num block-scoped to foreach
        }
        
        // Console.WriteLine(num); // ERROR: num doesn't exist here
    }
}
```

#### While/Do-While Loops

```csharp
public class WhileLoopScope
{
    public void WhileExample()
    {
        int counter = 0; // Method scope
        
        while (counter < 5)
        {
            int doubledValue = counter * 2; // Block scope - each iteration
            Console.WriteLine(doubledValue);
            counter++;
        }
        
        // doubledValue doesn't exist here
    }
    
    public void DoWhileExample()
    {
        int value = 0; // Method scope
        
        do
        {
            int squared = value * value; // Block scope
            Console.WriteLine(squared);
            value++;
        } while (value < 5);
        
        // squared doesn't exist here
    }
}
```

#### Switch Statements

```csharp
public class SwitchBlockScope
{
    public void SwitchExample(int choice)
    {
        switch (choice)
        {
            case 1:
                int caseOneValue = 100; // Block scope within case
                Console.WriteLine(caseOneValue);
                break;
                
            case 2:
                int caseTwoValue = 200; // Separate block scope
                Console.WriteLine(caseTwoValue);
                break;
                
            default:
                int defaultValue = 0; // Separate block scope
                Console.WriteLine(defaultValue);
                break;
        }
        
        // None of these variables exist here
    }
    
    // Best Practice: Use explicit blocks in switch cases to avoid confusion
    public void SwitchBestPractice(string command)
    {
        switch (command)
        {
            case "start":
            {
                int startCode = 1; // Explicit block scope
                Console.WriteLine($"Starting with code: {startCode}");
                break;
            }
            case "stop":
            {
                int stopCode = 0; // Separate explicit block scope
                Console.WriteLine($"Stopping with code: {stopCode}");
                break;
            }
        }
    }
}
```

### Try-Catch-Finally Block Scope

```csharp
public class TryBlockScope
{
    public void TryExample()
    {
        try
        {
            int tryValue = 10; // Block scope within try
            int result = 100 / tryValue;
            Console.WriteLine(result);
        }
        catch (DivideByZeroException ex)
        {
            string errorMsg = ex.Message; // Block scope within catch
            Console.WriteLine(errorMsg);
        }
        finally
        {
            string cleanupMsg = "Cleanup"; // Block scope within finally
            Console.WriteLine(cleanupMsg);
        }
        
        // None of these variables exist here
    }
    
    public void MultipleBlocks()
    {
        try
        {
            int value1 = 5;
        }
        catch (Exception ex1)
        {
            string error1 = ex1.Message;
        }
        
        try
        {
            int value2 = 10;
        }
        catch (Exception ex2)
        {
            string error2 = ex2.Message;
        }
        
        // ex1, ex2, error1, error2, value1, value2 all out of scope
    }
}
```

### Using Statements - Block Scope

```csharp
public class UsingBlockScope
{
    public void FileOperationOld()
    {
        using (var file = File.OpenRead("data.txt"))
        {
            // file is accessible here
            byte[] buffer = new byte[1024];
            file.Read(buffer, 0, buffer.Length);
        }
        
        // file is disposed and out of scope here
    }
    
    public void FileOperationNew()
    {
        using var file = File.OpenRead("data.txt"); // C# 8.0+
        // file is accessible here
        byte[] buffer = new byte[1024];
        file.Read(buffer, 0, buffer.Length);
        // file is disposed at end of method
    }
}
```

## Nested Block Scope

### Scope Hierarchy

```csharp
public class NestedBlockScope
{
    private int _classLevel = 1; // Class scope
    
    public void Method()
    {
        int methodLevel = 2; // Method scope
        
        {
            int block1Level = 3; // Outer block scope
            
            {
                int block2Level = 4; // Inner block scope
                
                // All accessible: _classLevel, methodLevel, block1Level, block2Level
                Console.WriteLine($"{_classLevel}, {methodLevel}, {block1Level}, {block2Level}");
            }
            
            // block2Level not accessible here
            // Console.WriteLine(block2Level); // ERROR
        }
        
        // block1Level not accessible here
        // Console.WriteLine(block1Level); // ERROR
    }
}
```

### Complex Nested Structures

```csharp
public class ComplexNesting
{
    public void ProcessData(int[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            int loopIndex = i; // Loop block scope
            
            if (data[i] > 0)
            {
                int positiveValue = data[i]; // If block scope
                
                for (int j = 0; j < 3; j++)
                {
                    int multiplier = j + 1; // Inner loop block scope
                    int result = positiveValue * multiplier;
                    Console.WriteLine(result);
                    // All accessible: loopIndex, positiveValue, multiplier
                }
                // multiplier out of scope
            }
            // positiveValue out of scope
        }
        // loopIndex out of scope
    }
}
```

## Block Scope Rules and Behaviors

### Rule 1: Variables Are Accessible in Their Block and Nested Blocks

```csharp
public void Rule1()
{
    int outer = 1;
    {
        int inner = 2;
        Console.WriteLine(outer); // OK - outer accessible
        Console.WriteLine(inner);  // OK - inner accessible
    }
    // Console.WriteLine(inner); // ERROR - out of scope
}
```

### Rule 2: Cannot Redeclare Variables in Same Scope

```csharp
public void Rule2()
{
    int x = 5;
    // int x = 10; // ERROR: x already declared in this scope
    
    {
        int x = 10; // OK: Different block scope (shadowing - see next concept)
    }
}
```

### Rule 3: Inner Scopes Can Shadow Outer Scopes

```csharp
public void Rule3()
{
    int value = 5; // Outer scope
    
    {
        int value = 10; // Inner scope - shadows outer (name collision)
        Console.WriteLine(value); // Prints 10
    }
    
    Console.WriteLine(value); // Prints 5
}
```

### Rule 4: Scope Extends to End of Block

```csharp
public void Rule4()
{
    for (int i = 0; i < 3; i++)
    {
        int temp = i * 2;
        Console.WriteLine(temp);
    } // i and temp go out of scope here
    
    // for (int i = 0; i < 3; i++); // Would declare new i (previous i is gone)
}
```

## Best Practices

1. **Keep Scope as Narrow as Possible**: Declare variables close to where they're used
2. **Use Explicit Blocks for Clarity**: Especially in switch statements and complex logic
3. **Avoid Shadowing**: Don't reuse names in nested scopes
4. **Prefer Expression Scoping**: Use inline expressions when possible (LINQ, conditional expressions)
5. **Document Complex Scope**: Add comments for deeply nested or unclear scope structures

## Common Pitfalls

- **Accessing Out-of-Scope Variables**: Results in compile errors
- **Unintended Shadowing**: Using same variable name in nested blocks
- **Scope Width Confusion**: Thinking variables are accessible outside their blocks
- **Loop Variable Persistence**: Mistakenly thinking loop variables persist after the loop

## Summary

Block scope is fundamental to C# code organization. Understanding block scope prevents errors, improves code clarity, and enables proper variable lifetime management. Variables are accessible only within their declared block and nested blocks, creating a hierarchical scope structure that protects against name collisions and maintains clean code organization.
