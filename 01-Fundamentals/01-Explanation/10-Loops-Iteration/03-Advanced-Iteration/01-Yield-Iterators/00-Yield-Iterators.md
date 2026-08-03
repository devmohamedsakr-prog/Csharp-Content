# Yield and Iterators in C#

## Overview

Yield statements enable lazy evaluation and generator functions. They allow you to create iterators that return values one at a time without loading entire collections into memory.

## Yield Return

`yield return` produces values one at a time in a generator function.

### Basic Yield Return

```csharp
public IEnumerable<int> GenerateNumbers(int count)
{
    for (int i = 0; i < count; i++)
    {
        yield return i; // Return value one at a time
    }
}

// Usage
foreach (int num in GenerateNumbers(5))
{
    Console.WriteLine(num); // 0, 1, 2, 3, 4
}
```

### How Yield Works

```csharp
public IEnumerable<int> CountToThree()
{
    Console.WriteLine("Starting");
    yield return 1; // First call - returns here
    Console.WriteLine("Between 1 and 2");
    yield return 2; // Second call - resumes here
    Console.WriteLine("Between 2 and 3");
    yield return 3; // Third call - resumes here
    Console.WriteLine("Done");
}

// Usage demonstrates lazy execution
var enumerator = CountToThree().GetEnumerator();
Console.WriteLine("Created enumerator");
enumerator.MoveNext(); // Output: "Starting", returns 1
enumerator.MoveNext(); // Output: "Between 1 and 2", returns 2
enumerator.MoveNext(); // Output: "Between 2 and 3", returns 3
enumerator.MoveNext(); // Output: "Done", returns false
```

## Lazy Evaluation

### Deferred Execution

```csharp
public IEnumerable<int> ExpensiveCalculation()
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine($"Calculating {i}");
        yield return i * i;
    }
}

// No calculation yet!
var results = ExpensiveCalculation();
Console.WriteLine("Created sequence");

// Calculations start here
foreach (var result in results)
{
    Console.WriteLine($"Result: {result}");
}
```

### On-Demand Processing

```csharp
public IEnumerable<int> Fibonacci()
{
    int a = 0, b = 1;
    while (true) // Infinite sequence
    {
        yield return a;
        int temp = a;
        a = b;
        b = temp + b;
    }
}

// Get first 10 Fibonacci numbers
foreach (var num in Fibonacci().Take(10))
{
    Console.WriteLine(num);
}

// Get first 20
foreach (var num in Fibonacci().Take(20))
{
    Console.WriteLine(num);
}
```

## Yield Break

`yield break` exits a generator function.

### Yield Break Example

```csharp
public IEnumerable<int> NumbersUntilNegative(int[] numbers)
{
    foreach (var num in numbers)
    {
        if (num < 0)
            yield break; // Stop iteration
        
        yield return num;
    }
}

// Usage
var nums = new[] { 1, 2, 3, -1, 4, 5 };
foreach (var num in NumbersUntilNegative(nums))
{
    Console.WriteLine(num); // 1, 2, 3
}
```

## Generator Patterns

### Pattern 1: Fibonacci Sequence

```csharp
public IEnumerable<int> FibonacciSequence(int count)
{
    int a = 0, b = 1;
    
    for (int i = 0; i < count; i++)
    {
        yield return a;
        int temp = a;
        a = b;
        b = temp + b;
    }
}

// Usage
foreach (var fib in FibonacciSequence(8))
{
    Console.WriteLine(fib); // 0, 1, 1, 2, 3, 5, 8, 13
}
```

### Pattern 2: Range Generator

```csharp
public IEnumerable<int> Range(int start, int end, int step = 1)
{
    for (int i = start; i < end; i += step)
    {
        yield return i;
    }
}

// Usage
foreach (var i in Range(0, 10, 2))
{
    Console.WriteLine(i); // 0, 2, 4, 6, 8
}
```

### Pattern 3: File Line Reader

```csharp
public IEnumerable<string> ReadLines(string filePath)
{
    using (var reader = new StreamReader(filePath))
    {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line; // Lazy reading
        }
    }
}

// Process large file line by line
foreach (var line in ReadLines("large_file.txt"))
{
    ProcessLine(line); // Memory efficient
}
```

### Pattern 4: Filtering with Yield

```csharp
public IEnumerable<int> FilterEven(IEnumerable<int> numbers)
{
    foreach (var num in numbers)
    {
        if (num % 2 == 0)
            yield return num;
    }
}

// Usage
var nums = new[] { 1, 2, 3, 4, 5, 6 };
foreach (var even in FilterEven(nums))
{
    Console.WriteLine(even); // 2, 4, 6
}
```

### Pattern 5: Transformation

```csharp
public IEnumerable<string> ToUpperCase(IEnumerable<string> items)
{
    foreach (var item in items)
    {
        yield return item.ToUpper();
    }
}

// Usage
var words = new[] { "hello", "world" };
foreach (var word in ToUpperCase(words))
{
    Console.WriteLine(word); // HELLO, WORLD
}
```

### Pattern 6: Batching

```csharp
public IEnumerable<List<T>> Batch<T>(IEnumerable<T> items, int batchSize)
{
    var batch = new List<T>();
    
    foreach (var item in items)
    {
        batch.Add(item);
        
        if (batch.Count == batchSize)
        {
            yield return batch;
            batch = new List<T>();
        }
    }
    
    if (batch.Count > 0)
        yield return batch;
}

// Usage
var numbers = Enumerable.Range(1, 10);
foreach (var batch in Batch(numbers, 3))
{
    Console.WriteLine(string.Join(", ", batch)); // Groups of 3
}
```

## Yield vs Traditional Collections

### Traditional Approach

```csharp
public List<int> GetNumbers(int count)
{
    var result = new List<int>();
    
    for (int i = 0; i < count; i++)
    {
        result.Add(i);
    }
    
    return result; // Entire list in memory
}
```

### Yield Approach

```csharp
public IEnumerable<int> GetNumbers(int count)
{
    for (int i = 0; i < count; i++)
    {
        yield return i; // Lazy evaluation
    }
}
```

### Performance Comparison

```csharp
// Traditional: Allocates 1 million integers
var list = GetNumbersList(1_000_000);
Console.WriteLine($"List memory: ~{1_000_000 * 4 / 1024 / 1024}MB");

// Yield: Only allocates what's used
var enumerable = GetNumbersYield(1_000_000);
var first10 = enumerable.Take(10);
Console.WriteLine("Only 10 items in memory");
```

## Custom Iterators with IEnumerable

### Simple Iterator

```csharp
public class CountUpTo : IEnumerable<int>
{
    private int _max;
    
    public CountUpTo(int max)
    {
        _max = max;
    }
    
    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < _max; i++)
        {
            yield return i;
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

// Usage
var counter = new CountUpTo(5);
foreach (var num in counter)
{
    Console.WriteLine(num);
}
```

## Yield with Recursion

### Tree Traversal

```csharp
public class TreeNode
{
    public int Value { get; set; }
    public List<TreeNode> Children { get; set; }
}

public IEnumerable<int> TraverseTree(TreeNode node)
{
    yield return node.Value;
    
    foreach (var child in node.Children)
    {
        foreach (var value in TraverseTree(child)) // Recursive
        {
            yield return value;
        }
    }
}

// Or simpler with yield from (if available)
public IEnumerable<int> TraverseTreeSimple(TreeNode node)
{
    yield return node.Value;
    
    foreach (var child in node.Children)
    {
        // In C# 7.0+, yield from would be: yield from TraverseTree(child);
        foreach (var value in TraverseTree(child))
        {
            yield return value;
        }
    }
}
```

## Best Practices

1. **Use Yield for Large or Infinite Sequences**
   ```csharp
   // Good: Lazy evaluation
   public IEnumerable<int> LargeSequence()
   {
       for (int i = 0; i < 1_000_000; i++)
           yield return i;
   }
   ```

2. **Combine with LINQ for Power**
   ```csharp
   public IEnumerable<int> GetNumbers()
   {
       for (int i = 0; i < 100; i++)
           yield return i;
   }
   
   // Easy to compose
   var result = GetNumbers()
       .Where(n => n % 2 == 0)
       .Select(n => n * 2)
       .Take(10);
   ```

3. **Document Generator Behavior**
   ```csharp
   /// <summary>
   /// Generates Fibonacci numbers lazily.
   /// Note: Returns infinite sequence.
   /// </summary>
   public IEnumerable<int> Fibonacci()
   {
       // ...
   }
   ```

4. **Handle Exceptions Properly**
   ```csharp
   public IEnumerable<string> ReadLines(string file)
   {
       using (var reader = new StreamReader(file))
       {
           string line;
           while ((line = reader.ReadLine()) != null)
           {
               yield return line; // Dispose handled properly
           }
       }
   }
   ```

## Performance Benefits

### Memory Efficiency

```csharp
// Traditional: Loads entire file into memory
var allLines = File.ReadAllLines("huge_file.txt");
foreach (var line in allLines)
{
    ProcessLine(line);
}

// Yield: Reads line by line
foreach (var line in File.ReadLines("huge_file.txt"))
{
    ProcessLine(line);
}
```

### Responsiveness

```csharp
// Can start processing while still generating
var data = GenerateSlowly().Take(10); // Gets only what's needed
foreach (var item in data)
{
    Process(item); // Starts immediately
}
```

## Summary

- **Yield return**: Return values one at a time (lazy evaluation)
- **Yield break**: Stop iteration early
- **Efficient**: Don't load entire collections into memory
- **Composable**: Works perfectly with LINQ
- **Infinite sequences**: Enable generators for endless data
- **Memory friendly**: Process large data efficiently
