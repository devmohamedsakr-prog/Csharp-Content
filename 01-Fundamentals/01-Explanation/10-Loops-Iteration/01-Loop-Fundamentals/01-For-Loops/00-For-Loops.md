# For Loops in C#

## Overview

The `for` loop is the most versatile iteration construct in C#. It's ideal when you know exactly how many times you need to iterate or when you need access to the iteration counter/index.

## Basic For Loop Syntax

```csharp
for (initialization; condition; increment)
{
    // Loop body executed while condition is true
}
```

**Components:**
- **Initialization**: Executed once before the loop starts (typically declares loop variable)
- **Condition**: Checked before each iteration; loop continues while true
- **Increment**: Executed after each iteration (typically updates loop variable)
- **Body**: Code executed in each iteration

## Simple For Loop Examples

### Basic Counting

```csharp
// Count from 0 to 4
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(i); // Output: 0, 1, 2, 3, 4
}

// Count from 1 to 5
for (int i = 1; i <= 5; i++)
{
    Console.WriteLine(i); // Output: 1, 2, 3, 4, 5
}

// Count backward
for (int i = 5; i > 0; i--)
{
    Console.WriteLine(i); // Output: 5, 4, 3, 2, 1
}
```

### Different Increments

```csharp
// Increment by 2
for (int i = 0; i < 10; i += 2)
{
    Console.WriteLine(i); // 0, 2, 4, 6, 8
}

// Increment by 5
for (int i = 0; i < 50; i += 5)
{
    Console.WriteLine(i); // 0, 5, 10, 15, 20, 25, 30, 35, 40, 45
}

// Multiply each iteration
for (int i = 1; i < 100; i *= 2)
{
    Console.WriteLine(i); // 1, 2, 4, 8, 16, 32, 64
}
```

## Loop Variations

### Omitting Components

```csharp
// Omit initialization (declare before loop)
int i = 0;
for (; i < 5; i++)
{
    Console.WriteLine(i);
}

// Omit condition (infinite loop - needs break)
for (int i = 0; ; i++)
{
    if (i == 5) break;
    Console.WriteLine(i);
}

// Omit increment (must be in body)
for (int i = 0; i < 5; )
{
    Console.WriteLine(i);
    i++;
}

// Omit all (infinite loop - needs break)
for (; ; )
{
    Console.WriteLine("Infinite");
    break;
}
```

### Multiple Variables

```csharp
// Multiple initialization
for (int i = 0, j = 10; i < 5; i++, j--)
{
    Console.WriteLine($"i={i}, j={j}");
    // Output: i=0, j=10 / i=1, j=9 / i=2, j=8 / i=3, j=7 / i=4, j=6
}

// Multiple updates
for (int i = 1, j = 1; i <= 3; i++, j *= 2)
{
    Console.WriteLine($"{i}: {j}");
    // Output: 1: 1 / 2: 2 / 3: 4
}
```

## Working with Collections

### Array Iteration

```csharp
string[] fruits = { "Apple", "Banana", "Cherry" };

// Access by index
for (int i = 0; i < fruits.Length; i++)
{
    Console.WriteLine($"{i}: {fruits[i]}");
    // Output: 0: Apple / 1: Banana / 2: Cherry
}

// Reverse iteration
for (int i = fruits.Length - 1; i >= 0; i--)
{
    Console.WriteLine(fruits[i]);
    // Output: Cherry, Banana, Apple
}
```

### List Iteration

```csharp
var numbers = new List<int> { 10, 20, 30, 40, 50 };

// Forward iteration with index
for (int i = 0; i < numbers.Count; i++)
{
    Console.WriteLine($"Index {i}: {numbers[i]}");
}

// Reverse iteration
for (int i = numbers.Count - 1; i >= 0; i--)
{
    Console.WriteLine(numbers[i]);
}

// Every other element
for (int i = 0; i < numbers.Count; i += 2)
{
    Console.WriteLine(numbers[i]); // 10, 30, 50
}
```

### 2D Array Iteration

```csharp
int[,] matrix = new int[3, 3]
{
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Iterate all elements
for (int row = 0; row < matrix.GetLength(0); row++)
{
    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        Console.Write($"{matrix[row, col]} ");
    }
    Console.WriteLine();
}

// Output:
// 1 2 3
// 4 5 6
// 7 8 9
```

## Advanced For Loop Patterns

### Pattern 1: Fibonacci Sequence

```csharp
public void PrintFibonacci(int count)
{
    int prev = 0, curr = 1;
    
    for (int i = 0; i < count; i++)
    {
        Console.WriteLine(prev);
        int next = prev + curr;
        prev = curr;
        curr = next;
    }
}

// Usage
PrintFibonacci(7); // 0, 1, 1, 2, 3, 5, 8
```

### Pattern 2: Validation Loop

```csharp
public int GetValidNumber()
{
    int number = -1;
    
    for (int attempts = 0; attempts < 3; attempts++)
    {
        Console.Write("Enter a number (0-100): ");
        if (int.TryParse(Console.ReadLine(), out number) && number >= 0 && number <= 100)
        {
            return number;
        }
        Console.WriteLine($"Invalid. {3 - attempts - 1} attempts remaining.");
    }
    
    return -1; // Failed after 3 attempts
}
```

### Pattern 3: Exponential Backoff

```csharp
public void RetryWithBackoff(int maxAttempts)
{
    for (int attempt = 0; attempt < maxAttempts; attempt++)
    {
        try
        {
            PerformRiskyOperation();
            return; // Success
        }
        catch (Exception ex)
        {
            int delayMs = (int)Math.Pow(2, attempt) * 100; // 100ms, 200ms, 400ms...
            Console.WriteLine($"Attempt {attempt + 1} failed. Retrying in {delayMs}ms");
            Thread.Sleep(delayMs);
        }
    }
}
```

### Pattern 4: Timing Loop

```csharp
public void MeasurePerformance()
{
    int iterations = 1_000_000;
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    
    for (int i = 0; i < iterations; i++)
    {
        // Perform operation
        Math.Sqrt(i);
    }
    
    stopwatch.Stop();
    Console.WriteLine($"{iterations} iterations took {stopwatch.ElapsedMilliseconds}ms");
}
```

## When to Use For Loops

### Use For When:
- You need access to the index
- You know the iteration count in advance
- You need to skip elements (i += 2)
- You need to iterate in reverse
- You need fine-grained loop control

### Example: Use For

```csharp
// Accessing index is primary purpose
for (int i = 0; i < items.Count; i++)
{
    Console.WriteLine($"Item {i}: {items[i]}");
}
```

### Example: Don't Use For (use foreach instead)

```csharp
// Simply iterating, no index needed
var numbers = GetNumbers();
foreach (var number in numbers)
{
    ProcessNumber(number);
}
```

## Common For Loop Patterns

### Pattern: Sum Elements

```csharp
public int SumArray(int[] numbers)
{
    int sum = 0;
    for (int i = 0; i < numbers.Length; i++)
    {
        sum += numbers[i];
    }
    return sum;
}
```

### Pattern: Find Element

```csharp
public int FindIndex(int[] array, int target)
{
    for (int i = 0; i < array.Length; i++)
    {
        if (array[i] == target)
            return i;
    }
    return -1; // Not found
}
```

### Pattern: Modify Elements

```csharp
public void DoubleValues(int[] numbers)
{
    for (int i = 0; i < numbers.Length; i++)
    {
        numbers[i] *= 2;
    }
}
```

### Pattern: Conditional Iteration

```csharp
public void ProcessEvenIndices(List<string> items)
{
    for (int i = 0; i < items.Count; i += 2)
    {
        Console.WriteLine(items[i]);
    }
}
```

## Performance Considerations

### Loop Efficiency

```csharp
// INEFFICIENT: Calls .Count every iteration
for (int i = 0; i < list.Count; i++)
{
    // ...
}

// EFFICIENT: Cache the count
int count = list.Count;
for (int i = 0; i < count; i++)
{
    // ...
}

// OR: Use foreach (handles this internally)
foreach (var item in list)
{
    // ...
}
```

### Loop Complexity

```csharp
// O(n) - Single pass
for (int i = 0; i < n; i++)
{
    DoWork();
}

// O(n²) - Nested loops
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        DoWork();
    }
}

// O(n log n) - Logarithmic inner loop
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < Math.Log(n); j++)
    {
        DoWork();
    }
}
```

## Best Practices

1. **Use Meaningful Variable Names**
   ```csharp
   // BAD
   for (int i = 0; i < items.Count; i++) { }
   
   // GOOD
   for (int itemIndex = 0; itemIndex < items.Count; itemIndex++) { }
   ```

2. **Cache Collection Length**
   ```csharp
   // More efficient
   int count = collection.Count;
   for (int i = 0; i < count; i++) { }
   ```

3. **Choose Appropriate Increment**
   ```csharp
   // Clear intent
   for (int i = 0; i < 10; i++)    // Every element
   for (int i = 0; i < 10; i += 2) // Every other
   ```

4. **Avoid Complex Expressions**
   ```csharp
   // BAD: Hard to understand
   for (int i = 0, j = items.Count - 1; i < j; i++, j--) { }
   
   // GOOD: Clear logic
   int left = 0;
   int right = items.Count - 1;
   while (left < right)
   {
       Swap(items, left, right);
       left++;
       right--;
   }
   ```

## Summary

The for loop is essential for:
- Counting iterations
- Array/collection indexing
- Controlled iteration
- Complex iteration patterns
- Performance-critical code

Master for loops and you'll handle most iteration scenarios effectively.
