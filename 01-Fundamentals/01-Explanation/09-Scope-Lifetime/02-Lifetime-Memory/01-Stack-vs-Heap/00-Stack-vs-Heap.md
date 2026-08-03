# Stack vs Heap: Understanding Variable Lifetime and Memory

## Overview

Stack and Heap are two different memory regions where variables and objects are stored. Understanding where data is stored determines variable lifetime and is crucial for proper memory management.

## The Stack

### What is the Stack?

The stack is a region of memory that stores value type variables and method references. It operates in a LIFO (Last-In-First-Out) manner - variables are automatically freed when they go out of scope.

### Stack Characteristics

```csharp
public class StackDemo
{
    public void StackVariables()
    {
        // All of these are on the STACK
        int x = 5; // Stack: 4 bytes
        double y = 3.14; // Stack: 8 bytes
        bool flag = true; // Stack: 1 byte
        decimal money = 100.50m; // Stack: 16 bytes
        
        // When method returns, all stack variables are freed automatically
        // x, y, flag, money are deleted from stack
    }
}
```

### Stack Allocation and Deallocation

```csharp
public class StackLifetime
{
    public void MethodA()
    {
        int a = 1; // Stack pointer moves
        int b = 2; // Stack pointer moves
        Console.WriteLine(a + b);
    } // Stack pointer returns - a, b automatically freed
    
    public void MethodB()
    {
        string name = "Alice"; // Stack: reference (8 bytes)
        int age = 30; // Stack: value (4 bytes)
        
        MethodA(); // Nested call - MethodA has its own stack frame
        
        Console.WriteLine(name); // Still accessible - a, b from MethodA are gone
    } // name and age freed
}
```

### Stack Overflow

```csharp
public class StackOverflow
{
    // BAD: Infinite recursion - stack overflow
    public void InfiniteRecursion()
    {
        int localVar = 1; // Each call uses stack space
        InfiniteRecursion(); // Calls itself forever
        // Stack keeps growing until it crashes: StackOverflowException
    }
    
    // GOOD: Controlled recursion with base case
    public int Factorial(int n)
    {
        if (n <= 1) return 1; // Base case - stops recursion
        int result = n * Factorial(n - 1); // Each call gets stack frame
        return result;
    }
}
```

## The Heap

### What is the Heap?

The heap is a region of memory that stores reference type objects (classes, arrays, strings). Objects on the heap persist until no references point to them, then the garbage collector cleans them up.

### Heap Characteristics

```csharp
public class HeapDemo
{
    public void HeapVariables()
    {
        // Objects are created on the HEAP
        
        string text = "Hello"; // Reference on stack, string object on heap
        int[] numbers = { 1, 2, 3 }; // Reference on stack, array on heap
        Person person = new Person(); // Reference on stack, object on heap
        List<int> list = new List<int>(); // Reference on stack, list on heap
        
        // When method returns:
        // - Stack variables (references) are freed
        // - Heap objects are marked for garbage collection
        // - GC eventually cleans them up
    }
}

public class Person
{
    public string Name { get; set; }
}
```

### Heap Allocation

```csharp
public class HeapAllocation
{
    public void AllocateObjects()
    {
        // Each 'new' keyword allocates memory on the heap
        
        var person1 = new Person { Name = "Alice" }; // Heap allocation
        var person2 = new Person { Name = "Bob" }; // Separate heap allocation
        var person3 = person1; // Same reference - person3 points to same object as person1
        
        // In memory:
        // Stack: person1 -> [Heap: Person object 1 (Alice)]
        // Stack: person2 -> [Heap: Person object 2 (Bob)]
        // Stack: person3 -> [Heap: Person object 1 (Alice)] // Same as person1
    }
}
```

## Value Types vs Reference Types

### Value Types (Stack)

```csharp
public class ValueTypeDemo
{
    public void StackTypes()
    {
        // VALUE TYPES - stored on STACK
        int age = 30; // Stack
        double height = 5.9; // Stack
        bool isActive = true; // Stack
        decimal salary = 50000.00m; // Stack
        DateTime birthDate = new DateTime(1994, 1, 15); // Stack
        
        // When copied, entire value is copied
        int age1 = 30;
        int age2 = age1; // age2 gets a copy of the value
        age2 = 31; // Changing age2 doesn't affect age1
        Console.WriteLine(age1); // Still 30
    }
}

// Custom value type
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class CustomValueDemo
{
    public void Demo()
    {
        Point p1 = new Point { X = 10, Y = 20 }; // Stack
        Point p2 = p1; // Entire struct copied to stack
        p2.X = 100; // p1 unaffected
        
        Console.WriteLine(p1.X); // 10
        Console.WriteLine(p2.X); // 100
    }
}
```

### Reference Types (Heap)

```csharp
public class ReferenceTypeDemo
{
    public void HeapTypes()
    {
        // REFERENCE TYPES - stored on HEAP
        string name = "Alice"; // Reference on stack, string on heap
        Person person = new Person { Name = "Bob" }; // Reference on stack, object on heap
        int[] numbers = { 1, 2, 3 }; // Reference on stack, array on heap
        List<string> list = new List<string>(); // Reference on stack, list on heap
        
        // When copied, only reference is copied
        Person p1 = new Person { Name = "Alice" };
        Person p2 = p1; // p2 gets copy of reference (points to same object)
        p2.Name = "Bob"; // Changes the object both point to
        Console.WriteLine(p1.Name); // "Bob" - p1 also changed!
    }
}
```

### Key Difference Illustrated

```csharp
public class ValueVsReference
{
    public class MyClass // Reference type
    {
        public int Value { get; set; }
    }
    
    public struct MyStruct // Value type
    {
        public int Value { get; set; }
    }
    
    public void Demonstrate()
    {
        // CLASS (Reference Type)
        MyClass c1 = new MyClass { Value = 10 };
        MyClass c2 = c1; // c2 points to same object
        c2.Value = 20;
        Console.WriteLine(c1.Value); // 20 - both changed
        
        // STRUCT (Value Type)
        MyStruct s1 = new MyStruct { Value = 10 };
        MyStruct s2 = s1; // s2 gets copy of struct
        s2.Value = 20;
        Console.WriteLine(s1.Value); // 10 - s1 unchanged
    }
}
```

## Method Stack Frames

### Stack Frame for Each Method Call

```csharp
public class StackFrames
{
    public int Calculate(int x)
    {
        int step1 = x + 5;
        int step2 = step1 * 2;
        return step2;
    }
    
    public void Demonstrate()
    {
        // Call stack visualization:
        
        // Before Calculate() call
        // [Demonstrate Frame: ...]
        
        int result = Calculate(10);
        
        // During Calculate(10) execution
        // [Demonstrate Frame: result=?, ...]
        // [Calculate Frame: x=10, step1=15, step2=30]
        
        // After Calculate() returns
        // [Demonstrate Frame: result=30, ...]
        
        Console.WriteLine(result); // 30
    }
}
```

### Recursive Stack Frames

```csharp
public class RecursiveStackFrames
{
    public int Factorial(int n)
    {
        if (n <= 1) return 1;
        return n * Factorial(n - 1);
    }
    
    public void Demo()
    {
        // Factorial(3) call stack:
        
        // Factorial(3)
        //   return 3 * Factorial(2)
        //     Factorial(2)
        //       return 2 * Factorial(1)
        //         Factorial(1)
        //           return 1
        //         [returns 1]
        //       [returns 2 * 1 = 2]
        //     [returns 2]
        //   [returns 3 * 2 = 6]
        
        int result = Factorial(3);
        Console.WriteLine(result); // 6
    }
}
```

## Passing Variables Between Methods

### Passing Value Types

```csharp
public class PassingValueTypes
{
    public void ModifyValue(int value)
    {
        value = value * 2; // Modifies local copy
    }
    
    public void Demo()
    {
        int x = 10;
        ModifyValue(x); // x is copied to ModifyValue's stack frame
        Console.WriteLine(x); // 10 - unchanged (different stack location)
    }
}
```

### Passing Reference Types

```csharp
public class PassingReferenceTypes
{
    public void ModifyObject(Person person)
    {
        person.Name = "Modified"; // Modifies object via reference
    }
    
    public void Demo()
    {
        var p = new Person { Name = "Original" };
        ModifyObject(p); // Reference copied, points to same object
        Console.WriteLine(p.Name); // "Modified" - object was changed
    }
}
```

### Pass by Reference with ref

```csharp
public class PassByReference
{
    public void ModifyValue(ref int value)
    {
        value = value * 2; // Modifies original variable
    }
    
    public void Demo()
    {
        int x = 10;
        ModifyValue(ref x); // Pass reference to x
        Console.WriteLine(x); // 20 - original changed
    }
}
```

## Memory Diagram Examples

### Simple Variable Allocation

```csharp
public class MemoryDiagram
{
    public void Demo()
    {
        // STACK:
        // int x = 5;           -> [x: 5]
        // string name = "Bob"; -> [name: 0x1000]
        
        // HEAP:
        //                      -> [0x1000: "Bob" string object]
        
        int x = 5;
        string name = "Bob";
        
        // After method:
        // All stack variables freed, heap object eligible for GC
    }
}
```

### Reference Type Sharing

```csharp
public class ReferenceSharing
{
    public void Demo()
    {
        // STACK:
        // List<int> list1 = new List<int>(); -> [list1: 0x2000]
        // List<int> list2 = list1;            -> [list2: 0x2000]
        
        // HEAP:
        //                                      -> [0x2000: List object]
        
        // Both list1 and list2 point to same list on heap
        var list1 = new List<int> { 1, 2, 3 };
        var list2 = list1;
        
        list2.Add(4); // Modifies list both point to
        Console.WriteLine(list1.Count); // 4 (both see the change)
    }
}
```

## Null References

### Stack vs Heap - Null

```csharp
public class NullHandling
{
    public void Demo()
    {
        // Null reference
        Person person = null; // Stack: person points to nothing
        
        if (person == null)
        {
            person = new Person { Name = "Alice" }; // Now points to heap object
        }
        
        // Null value types (nullable)
        int? age = null; // Stack: age = null (special nullable<int>)
        age = 30; // Stack: age = 30
    }
}
```

## Best Practices

1. **Know Your Types**: Understand whether you're using value or reference types
2. **Watch Stack Size**: Deep recursion can cause stack overflow
3. **Avoid Large Structs**: Can impact performance when copied
4. **Prefer Immutable Value Types**: Especially for values passed around
5. **Consider Memory Lifetime**: Objects on heap exist until unreferenced
6. **Use ref/out Carefully**: Explicitly showing intent to modify
7. **Profile Memory Usage**: Especially for long-running applications

## Common Issues

1. **Unintended Reference Sharing**: Modifying objects affects all references
2. **Stack Overflow**: From infinite recursion or excessive nesting
3. **Memory Leaks**: Holding references to unreachable objects (rare in C#)
4. **Large Struct Copying**: Performance penalty for large value types
5. **Null Reference Exceptions**: Accessing null references

## Summary

The stack stores value types and method references with automatic cleanup when variables go out of scope. The heap stores reference type objects that persist until the garbage collector reclaims them. Understanding this distinction is fundamental to writing efficient C# code - value types are faster but stack-limited, reference types are flexible but require garbage collection. Proper understanding of stack frames, passing mechanisms, and memory allocation prevents bugs and enables optimized code.
