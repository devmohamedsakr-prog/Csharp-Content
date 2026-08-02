# Loops and Iteration

## Overview
Loops allow repeating code blocks. C# provides multiple loop types for different iteration scenarios.

## For Loop

### Basic For Loop
```csharp
// Standard for loop
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(i); // 0, 1, 2, 3, 4
}

// With custom increment
for (int i = 0; i < 10; i += 2)
{
    Console.WriteLine(i); // 0, 2, 4, 6, 8
}

// Decrement
for (int i = 5; i > 0; i--)
{
    Console.WriteLine(i); // 5, 4, 3, 2, 1
}

// Multiple counters
for (int i = 0, j = 10; i < 5; i++, j--)
{
    Console.WriteLine($"{i}, {j}"); // 0,10 / 1,9 / 2,8 / 3,7 / 4,6
}

// Infinite loop
for (; ; )
{
    break; // Must have break to exit
}
```

### Nested For Loops
```csharp
// Multiplication table
for (int i = 1; i <= 3; i++)
{
    for (int j = 1; j <= 3; j++)
    {
        Console.Write($"{i * j,3} ");
    }
    Console.WriteLine();
}

// 2D array iteration
int[,] matrix = new int[3, 3];
for (int row = 0; row < 3; row++)
{
    for (int col = 0; col < 3; col++)
    {
        matrix[row, col] = row * 3 + col;
    }
}
```

## While Loop

### Basic While Loop
```csharp
int count = 0;
while (count < 5)
{
    Console.WriteLine(count);
    count++;
}

// While with condition
string input = "";
while (input != "exit")
{
    input = Console.ReadLine();
    if (input != "exit")
    {
        Console.WriteLine($"You entered: {input}");
    }
}
```

### Do-While Loop

### Execute At Least Once
```csharp
// Do-while: always executes at least once
int number;
do
{
    Console.Write("Enter a number (1-10): ");
    number = int.Parse(Console.ReadLine());
}
while (number < 1 || number > 10);

// Menu loop
int choice;
do
{
    Console.WriteLine("1. Add\n2. Delete\n3. Exit");
    choice = int.Parse(Console.ReadLine());
    
    switch (choice)
    {
        case 1: Console.WriteLine("Adding..."); break;
        case 2: Console.WriteLine("Deleting..."); break;
        case 3: Console.WriteLine("Exiting..."); break;
    }
}
while (choice != 3);
```

## Foreach Loop

### Iterating Collections
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Foreach on List
foreach (int num in numbers)
{
    Console.WriteLine(num);
}

// Foreach on array
string[] names = { "Alice", "Bob", "Charlie" };
foreach (string name in names)
{
    Console.WriteLine(name);
}

// Foreach on dictionary
var dict = new Dictionary<string, int>
{
    { "Alice", 30 },
    { "Bob", 25 }
};

foreach (var kvp in dict)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

// KeyValuePair deconstruction
foreach (var (name, age) in dict)
{
    Console.WriteLine($"{name}: {age}");
}
```

## Break and Continue

### Loop Control
```csharp
// Break: exit loop
for (int i = 0; i < 10; i++)
{
    if (i == 5)
        break; // Exits loop
    Console.WriteLine(i); // 0, 1, 2, 3, 4
}

// Continue: skip iteration
for (int i = 0; i < 5; i++)
{
    if (i == 2)
        continue; // Skip this iteration
    Console.WriteLine(i); // 0, 1, 3, 4
}

// Breaking nested loops
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        if (i == 1 && j == 1)
            goto Exit; // Jump to Exit label
        Console.WriteLine($"{i},{j}");
    }
}
Exit:
Console.WriteLine("Done");
```

## Foreach with Index (LINQ)

### Enumerating with Index
```csharp
var items = new[] { "apple", "banana", "cherry" };

// Using Select with index
foreach (var (item, index) in items.Select((x, i) => (x, i)))
{
    Console.WriteLine($"{index}: {item}");
}

// Using Index type (C# 8.0+)
foreach (var item in items.WithIndex())
{
    Console.WriteLine($"{item.Index}: {item.Value}");
}

// Extension method
public static class EnumerableExtensions
{
    public static IEnumerable<(T value, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        int index = 0;
        foreach (var item in source)
        {
            yield return (item, index++);
        }
    }
}
```

## Yield and Iterators

### Generator Functions
```csharp
// Yield return: lazy evaluation
public IEnumerable<int> GenerateNumbers(int count)
{
    for (int i = 0; i < count; i++)
    {
        yield return i; // Returns one at a time
    }
}

// Usage: items generated on demand
foreach (int num in GenerateNumbers(5))
{
    Console.WriteLine(num); // 0, 1, 2, 3, 4
}

// Infinite generator
public IEnumerable<int> Fibonacci()
{
    int a = 0, b = 1;
    while (true)
    {
        yield return a;
        int temp = a;
        a = b;
        b = temp + b;
    }
}

// Take first 10 Fibonacci numbers
foreach (var num in Fibonacci().Take(10))
{
    Console.WriteLine(num);
}

// Yield break: exit generator
public IEnumerable<int> YieldUntil(Predicate<int> predicate)
{
    for (int i = 0; i < 100; i++)
    {
        if (predicate(i))
            yield break; // Stop iteration
        yield return i;
    }
}
```

## Parallel Iteration

### Parallel.ForEach
```csharp
var items = Enumerable.Range(0, 1000).ToList();

// Sequential
foreach (var item in items)
{
    Process(item);
}

// Parallel
Parallel.ForEach(items, item =>
{
    Process(item); // Multiple threads
});

// With configuration
var options = new ParallelOptions
{
    MaxDegreeOfParallelism = Environment.ProcessorCount
};

Parallel.ForEach(items, options, item =>
{
    Process(item);
});

private void Process(int item)
{
    Thread.Sleep(10); // Simulate work
}
```

## Best Practices

1. **Choose Correct Loop Type**
```csharp
// Use for: when you need index
for (int i = 0; i < items.Count; i++)
{
    Console.WriteLine($"{i}: {items[i]}");
}

// Use foreach: simple iteration
foreach (var item in items)
{
    Console.WriteLine(item);
}

// Use while: condition-based
while (condition)
{
    // Process
}

// Use do-while: at least one execution
do
{
    // Process
}
while (condition);
```

2. **Avoid Modifying Collection During Iteration**
```csharp
// Bad: causes InvalidOperationException
var list = new List<int> { 1, 2, 3, 4, 5 };
foreach (var item in list)
{
    if (item == 3)
        list.Remove(item); // Modifying during iteration!
}

// Good: iterate over copy
foreach (var item in list.ToList())
{
    if (item == 3)
        list.Remove(item);
}
```

3. **Use LINQ for Complex Scenarios**
```csharp
// Better: LINQ is clearer
var even = numbers.Where(n => n % 2 == 0);
var doubled = numbers.Select(n => n * 2);
var sum = numbers.Sum();

// Instead of manual loops
int sum = 0;
foreach (var n in numbers)
{
    sum += n;
}
```

## Common Mistakes

1. **Off-by-One Errors**
```csharp
// Bad: iterates 0-4 instead of 0-5
for (int i = 0; i < 5; i++) { } // 5 iterations

// Correct: be clear about boundaries
for (int i = 0; i <= 5; i++) { } // 6 iterations
```

2. **Infinite Loops**
```csharp
// Bad: infinite loop
for (int i = 0; i < 5; i--) { } // i decrements, never < 5

// Good
for (int i = 0; i < 5; i++) { }
```

3. **Forgetting Break in Loop**
```csharp
// Bad: processes after finding match
for (int i = 0; i < items.Count; i++)
{
    if (items[i] == target)
    {
        Process(items[i]); // Found it
        // But continues looping!
    }
}

// Good
for (int i = 0; i < items.Count; i++)
{
    if (items[i] == target)
    {
        Process(items[i]);
        break;
    }
}
```

## Quick Summary
- for: counted iterations with index
- while: condition-based, pre-check
- do-while: condition-based, post-check
- foreach: iterate collection elements
- break: exit loop
- continue: skip to next iteration
- yield: generator functions, lazy evaluation
- Parallel.ForEach: multi-threaded iteration
- LINQ alternatives for complex scenarios
- Avoid modifying collections during iteration

## Resources
- Iteration Statements (C# documentation)
- For Loop Best Practices
- LINQ vs Loops
- Parallel Patterns Library (PPL)
