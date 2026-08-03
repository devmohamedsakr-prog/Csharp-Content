# Variable Capture and Closures in C#

## Overview

A closure is a function that captures and remembers variables from its enclosing scope. Variable capture allows lambda expressions and anonymous methods to access and use variables from their defining context, even after that context has exited.

## Understanding Closures

### Basic Variable Capture

```csharp
public class BasicClosure
{
    public Func<int> CreateCounter()
    {
        int count = 0; // Local variable to be captured
        
        // Lambda captures count variable
        return () => ++count;
    }
    
    public void Demo()
    {
        var counter = CreateCounter();
        
        Console.WriteLine(counter()); // 1
        Console.WriteLine(counter()); // 2
        Console.WriteLine(counter()); // 3
        
        // count persists across calls due to closure!
    }
}
```

### How Closures Work

```csharp
public class ClosureMechanism
{
    public Func<int> Demonstrate()
    {
        // The C# compiler creates a hidden class:
        // private class DisplayClass
        // {
        //     public int count;
        //     public int Lambda() => ++count;
        // }
        
        int count = 0;
        
        // The lambda becomes a method on DisplayClass
        return () => ++count;
    }
    
    public void Explanation()
    {
        // Conceptual transformation:
        // Original: return () => ++count;
        // Becomes: 
        // var display = new DisplayClass();
        // display.count = 0;
        // return display.Lambda;
        
        // The closure keeps DisplayClass alive, which keeps count alive
    }
}
```

## Captured Variables Behavior

### Shared State Among Multiple Closures

```csharp
public class SharedState
{
    public void Demo()
    {
        int x = 0;
        
        // Both lambdas capture same x variable
        Func<int> increment = () => ++x;
        Func<int> getX = () => x;
        Func<void> reset = () => x = 0;
        
        Console.WriteLine(increment()); // 1
        Console.WriteLine(increment()); // 2
        Console.WriteLine(getX()); // 2
        reset();
        Console.WriteLine(getX()); // 0
        
        // All three functions share the same captured x
    }
}
```

### Variables Are Captured, Not Values

```csharp
public class CaptureVariable
{
    public void Demo()
    {
        int value = 10;
        
        // Captures the variable, not the value
        Func<int> func = () => value * 2;
        
        Console.WriteLine(func()); // 20 (value = 10)
        
        value = 20; // Change the variable
        
        Console.WriteLine(func()); // 40 (value = 20)
        // The lambda sees the new value!
    }
}
```

## Multiple Variable Capture

### Capturing Multiple Variables

```csharp
public class MultipleCapture
{
    public Func<string> CreateMessage()
    {
        string greeting = "Hello";
        string name = "Alice";
        int count = 1;
        
        // Captures all three variables
        return () => $"{greeting} {name} (Call #{count++})";
    }
    
    public void Demo()
    {
        var message = CreateMessage();
        
        Console.WriteLine(message()); // Hello Alice (Call #1)
        Console.WriteLine(message()); // Hello Alice (Call #2)
        Console.WriteLine(message()); // Hello Alice (Call #3)
    }
}
```

### Capturing 'this' in Instance Methods

```csharp
public class CaptureThis
{
    private int _instanceValue = 10;
    private string _name = "Instance";
    
    public Action GetAction()
    {
        // Implicitly captures 'this'
        return () => Console.WriteLine($"{_name}: {_instanceValue}");
    }
    
    public void Demo()
    {
        var action = GetAction();
        action(); // Instance: 10
        
        _instanceValue = 20;
        action(); // Instance: 20 - sees updated field
    }
}
```

## Closures with Different Contexts

### Method-Level Closure

```csharp
public class MethodClosure
{
    public void Method()
    {
        int local = 5;
        
        Action action = () => Console.WriteLine(local);
        
        action(); // 5
    }
}
```

### Class-Level Closure

```csharp
public class ClassClosure
{
    private int _field = 10;
    
    public Action GetAction()
    {
        // Captures 'this'
        return () => Console.WriteLine(_field);
    }
}
```

### Nested Closure

```csharp
public class NestedClosure
{
    public Func<Func<int>> CreateNestedClosure()
    {
        int outer = 10;
        
        // Returns a function that returns a function
        return () =>
        {
            int middle = outer + 5;
            
            // Captures both outer and middle
            return () => outer + middle;
        };
    }
    
    public void Demo()
    {
        var outer = CreateNestedClosure();
        var middle = outer();
        int result = middle(); // 10 + 15 = 25
        
        Console.WriteLine(result);
    }
}
```

## Closure Memory Considerations

### Closure Keeps Variables Alive

```csharp
public class ClosureMemory
{
    public Func<int> CreateCounterWithLargeData()
    {
        int count = 0;
        int[] largeArray = new int[1_000_000]; // Large data
        
        // Both count and largeArray are captured
        return () =>
        {
            largeArray[0] = count; // Uses largeArray
            return ++count;
        };
    }
    
    public void Demo()
    {
        var counter = CreateCounterWithLargeData();
        
        // largeArray is kept alive as long as counter exists
        // Even if we never use largeArray in the lambda,
        // it's captured, so it's not garbage collected
        
        Console.WriteLine(counter()); // 1
        Console.WriteLine(counter()); // 2
    }
}
```

### Closure Lifecycle

```csharp
public class ClosureLifecycle
{
    private List<Action> _actions = new List<Action>();
    
    public void Example()
    {
        int x = 5;
        
        var action = () => Console.WriteLine(x);
        _actions.Add(action);
        
        // x is captured and kept alive because:
        // 1. action references the captured x
        // 2. _actions keeps action alive
        // So x never becomes eligible for GC
    }
}
```

## LINQ and Closures

### Closure in LINQ Queries

```csharp
public class LinqClosure
{
    public void Demo()
    {
        var numbers = new[] { 1, 2, 3, 4, 5 };
        int threshold = 3;
        
        // threshold is captured
        var result = numbers.Where(n => n > threshold).ToList();
        
        Console.WriteLine(string.Join(", ", result)); // 4, 5
    }
    
    public void DynamicFiltering()
    {
        var numbers = new[] { 1, 2, 3, 4, 5 };
        int threshold = 2;
        
        // Capture threshold in LINQ
        var filter = numbers.Where(n => n > threshold);
        
        threshold = 4;
        // filter sees updated threshold - deferred execution!
        var result = filter.ToList();
        
        Console.WriteLine(string.Join(", ", result)); // 5 - uses threshold=4
    }
}
```

### Deferred Execution with Closures

```csharp
public class DeferredExecution
{
    public void Demo()
    {
        var numbers = new[] { 1, 2, 3, 4, 5 };
        int factor = 2;
        
        // Deferred execution - closure captures factor
        IEnumerable<int> result = numbers.Select(n => n * factor);
        
        factor = 10;
        
        // factor is now 10 when enumeration happens
        foreach (int num in result)
        {
            Console.WriteLine(num); // 10, 20, 30, 40, 50
        }
    }
}
```

## Thread Safety with Closures

### Potential Thread Issues

```csharp
public class ThreadSafety
{
    private List<Action> _actions = new List<Action>();
    
    public void UnsafeExample()
    {
        int x = 0;
        
        // Multiple threads access captured x
        for (int i = 0; i < 10; i++)
        {
            var action = () => x++; // Captures x
            _actions.Add(action);
        }
        
        // If actions run on different threads:
        // Race condition on x!
        Parallel.ForEach(_actions, action => action());
        
        // x might not be 10 due to race condition
    }
    
    public void SafeExample()
    {
        var actions = new List<Action>();
        
        for (int i = 0; i < 10; i++)
        {
            int localCopy = i; // Copy for each iteration
            actions.Add(() => Console.WriteLine(localCopy));
        }
        
        // Each action has its own localCopy
        Parallel.ForEach(actions, action => action());
    }
}
```

## Event Handlers and Closures

### Closure in Event Handlers

```csharp
public class EventClosure
{
    private Button _button;
    
    public void SetupEventHandlers()
    {
        string context = "Button clicked";
        
        // Closure captures context
        _button.Click += (sender, e) =>
        {
            Console.WriteLine(context);
        };
    }
    
    public void MemoryLeak()
    {
        string largeString = new string('x', 1_000_000);
        
        // Closure captures largeString
        _button.Click += (sender, e) =>
        {
            Console.WriteLine(largeString.Length);
        };
        
        // largeString kept alive by event handler closure!
        // Even if no other references exist
    }
}

public class Button
{
    public event EventHandler Click;
}
```

## Best Practices with Closures

### Clear Capture Intent

```csharp
public class ClearCapture
{
    public Action CreateAction()
    {
        int value = 10;
        
        // Clear intent: capture value
        return () => 
        {
            // Explicit reference to captured variable
            return value * 2;
        };
    }
}
```

### Avoiding Unintended Captures

```csharp
public class AvoirdUnintended
{
    public void Demo()
    {
        int[] data = { 1, 2, 3 };
        
        // DON'T: Capture entire array if not needed
        var action = () =>
        {
            // Captures data array
            Console.WriteLine(data[0]);
        };
        
        // DO: Capture only what you need
        int firstItem = data[0];
        var action2 = () =>
        {
            // Only captures firstItem
            Console.WriteLine(firstItem);
        };
    }
}
```

### Document Captured Variables

```csharp
public class DocumentedCapture
{
    /// <summary>
    /// Creates a counter that captures the initial count value.
    /// Captures: count (local variable)
    /// </summary>
    public Func<int> CreateCounter(int initialCount = 0)
    {
        int count = initialCount;
        return () => ++count; // Captures count
    }
}
```

## Common Closure Patterns

### Factory Pattern with Closure

```csharp
public class ClosureFactory
{
    public Func<int, int> CreateMultiplier(int factor)
    {
        // factor is captured
        return x => x * factor;
    }
    
    public void Demo()
    {
        var multiplyBy2 = CreateMultiplier(2);
        var multiplyBy5 = CreateMultiplier(5);
        
        Console.WriteLine(multiplyBy2(10)); // 20
        Console.WriteLine(multiplyBy5(10)); // 50
    }
}
```

### Memoization with Closure

```csharp
public class Memoization
{
    public Func<int, int> CreateMemoizedFibonacci()
    {
        var cache = new Dictionary<int, int>(); // Captured
        
        Func<int, int> fib = null;
        fib = n =>
        {
            if (cache.TryGetValue(n, out int result))
                return result;
                
            if (n <= 1) result = n;
            else result = fib(n - 1) + fib(n - 2);
            
            cache[n] = result;
            return result;
        };
        
        return fib;
    }
    
    public void Demo()
    {
        var fib = CreateMemoizedFibonacci();
        Console.WriteLine(fib(10)); // Fast - uses cache
        Console.WriteLine(fib(11)); // Faster - reuses cache
    }
}
```

## Summary

Closures enable functions to capture and maintain state from their defining scope. The C# compiler transforms closures into hidden classes that keep captured variables alive. Understanding closures is essential for working with lambdas, LINQ, and event handlers. While powerful, closures require care to avoid unintended variable capture, memory leaks, and thread safety issues. Proper documentation and careful variable management make closures a valuable tool for functional programming patterns in C#.
