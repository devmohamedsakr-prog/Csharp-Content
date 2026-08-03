# Nested Loops

## Overview

Nested loops are loops within loops. They allow you to iterate through multi-dimensional data structures or perform operations on combinations of items.

## Basic Nested Loop

```csharp
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        Console.WriteLine($"({i}, {j})");
    }
}

// Output:
// (0, 0) (0, 1) (0, 2)
// (1, 0) (1, 1) (1, 2)
// (2, 0) (2, 1) (2, 2)
```

## Common Nested Loop Patterns

### Multiplication Table

```csharp
public void PrintMultiplicationTable(int size)
{
    for (int i = 1; i <= size; i++)
    {
        for (int j = 1; j <= size; j++)
        {
            Console.Write($"{i * j,4}");
        }
        Console.WriteLine();
    }
}

// Output:
//    1   2   3   4   5
//    2   4   6   8  10
//    3   6   9  12  15
//    4   8  12  16  20
//    5  10  15  20  25
```

### 2D Array Processing

```csharp
int[,] matrix = new int[3, 3]
{
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Print all elements
for (int row = 0; row < matrix.GetLength(0); row++)
{
    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        Console.Write($"{matrix[row, col]} ");
    }
    Console.WriteLine();
}

// Sum all elements
int sum = 0;
for (int row = 0; row < matrix.GetLength(0); row++)
{
    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        sum += matrix[row, col];
    }
}
```

### Jagged Array

```csharp
// Jagged array: array of arrays
int[][] jagged = new int[3][];
jagged[0] = new int[2];
jagged[1] = new int[3];
jagged[2] = new int[1];

// Different inner array sizes
for (int i = 0; i < jagged.Length; i++)
{
    for (int j = 0; j < jagged[i].Length; j++)
    {
        jagged[i][j] = i * 10 + j;
    }
}

// Print
for (int i = 0; i < jagged.Length; i++)
{
    for (int j = 0; j < jagged[i].Length; j++)
    {
        Console.Write($"{jagged[i][j]} ");
    }
    Console.WriteLine();
}
```

### Nested Collections

```csharp
var departments = new Dictionary<string, List<Employee>>
{
    { "Sales", new List<Employee> { emp1, emp2 } },
    { "IT", new List<Employee> { emp3, emp4, emp5 } }
};

foreach (var dept in departments)
{
    Console.WriteLine($"Department: {dept.Key}");
    
    foreach (var employee in dept.Value)
    {
        Console.WriteLine($"  - {employee.Name}");
    }
}
```

## Performance with Nested Loops

### Time Complexity

```csharp
// O(n) - Single loop
for (int i = 0; i < n; i++)
{
    DoWork(); // Executed n times
}

// O(n²) - Two nested loops
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        DoWork(); // Executed n * n times
    }
}

// O(n³) - Three nested loops
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        for (int k = 0; k < n; k++)
        {
            DoWork(); // Executed n * n * n times
        }
    }
}
```

### Performance Example

```csharp
int size = 1000;
var stopwatch = System.Diagnostics.Stopwatch.StartNew();

for (int i = 0; i < size; i++)
{
    for (int j = 0; j < size; j++)
    {
        int dummy = i * j; // 1 million iterations
    }
}

stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms"); // ~50ms
```

## Early Exit from Nested Loops

### Using Break (Inner Loop Only)

```csharp
bool found = false;
for (int i = 0; i < 10 && !found; i++)
{
    for (int j = 0; j < 10; j++)
    {
        if (array[i, j] == target)
        {
            Console.WriteLine($"Found at ({i}, {j})");
            found = true;
            break; // Exits inner loop only
        }
    }
}
```

### Using Method Return

```csharp
public (int, int) FindTarget(int[,] array, int target)
{
    for (int i = 0; i < array.GetLength(0); i++)
    {
        for (int j = 0; j < array.GetLength(1); j++)
        {
            if (array[i, j] == target)
                return (i, j); // Exit both loops
        }
    }
    return (-1, -1); // Not found
}
```

### Using Exception (Avoid)

```csharp
// NOT RECOMMENDED - Too heavy-weight
try
{
    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 10; j++)
        {
            if (SomeCondition())
                throw new BreakException();
        }
    }
}
catch (BreakException)
{
    // Loop exited
}
```

## Summary

- **Nested loops multiply iterations**: O(n²) or worse
- **Use for multi-dimensional data**: Arrays, matrices
- **Break only exits innermost**: Use flags for outer exit
- **Watch performance**: Nested loops can be slow
- **Consider LINQ alternatives**: Often cleaner
