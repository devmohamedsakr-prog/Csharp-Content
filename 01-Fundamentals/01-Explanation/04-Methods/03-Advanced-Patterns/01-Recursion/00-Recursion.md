# Recursion

## Overview

Recursion is when a method calls itself. It's a powerful technique for solving problems that have a recursive structure.

## What is Recursion?

A method that calls itself:

```csharp
public void CountDown(int n)
{
    if (n <= 0)
        return;  // Base case - stop recursion
    
    Console.WriteLine(n);
    CountDown(n - 1);  // Recursive call - method calls itself
}

// Usage
CountDown(5);
// Output: 5, 4, 3, 2, 1
```

## Recursive Structure

Every recursive method needs:

1. **Base case**: When to stop
2. **Recursive case**: Call itself with different parameters

```csharp
public int Factorial(int n)
{
    // Base case - stop recursion
    if (n <= 1)
        return 1;
    
    // Recursive case - call itself
    return n * Factorial(n - 1);
}

Factorial(5);  // 5 * 4 * 3 * 2 * 1 = 120
```

## Simple Recursion Examples

### Example 1: Countdown

```csharp
public void PrintNumbers(int n)
{
    // Base case
    if (n == 0)
        return;
    
    Console.WriteLine(n);
    
    // Recursive call
    PrintNumbers(n - 1);
}

// Usage
PrintNumbers(5);
// Output: 5, 4, 3, 2, 1
```

### Example 2: Sum of Numbers

```csharp
public int Sum(int n)
{
    // Base case
    if (n <= 0)
        return 0;
    
    // Recursive case
    return n + Sum(n - 1);
}

// Usage
Sum(5);  // 5 + 4 + 3 + 2 + 1 = 15
```

### Example 3: Power Function

```csharp
public int Power(int baseNum, int exponent)
{
    // Base case
    if (exponent == 0)
        return 1;
    
    // Recursive case
    return baseNum * Power(baseNum, exponent - 1);
}

// Usage
Power(2, 5);  // 2^5 = 32
```

## Factorial

Classic recursion example:

```csharp
public int Factorial(int n)
{
    // Base case
    if (n <= 1)
        return 1;
    
    // Recursive case: n! = n * (n-1)!
    return n * Factorial(n - 1);
}

// Trace for Factorial(4):
// Factorial(4) = 4 * Factorial(3)
// Factorial(3) = 3 * Factorial(2)
// Factorial(2) = 2 * Factorial(1)
// Factorial(1) = 1 (base case)
// Unwind: 2*1 = 2, 3*2 = 6, 4*6 = 24

Console.WriteLine(Factorial(5));  // 120
```

## Fibonacci

Another classic example:

```csharp
public int Fibonacci(int n)
{
    // Base cases
    if (n <= 0)
        return 0;
    if (n == 1)
        return 1;
    
    // Recursive case: Fib(n) = Fib(n-1) + Fib(n-2)
    return Fibonacci(n - 1) + Fibonacci(n - 2);
}

// Usage
Fibonacci(6);  // 0, 1, 1, 2, 3, 5, 8
// Fibonacci(6) = 8
```

## Array Recursion

Recursion on arrays:

```csharp
public int SumArray(int[] array, int index = 0)
{
    // Base case - reached end
    if (index >= array.Length)
        return 0;
    
    // Recursive case
    return array[index] + SumArray(array, index + 1);
}

// Usage
int[] numbers = { 1, 2, 3, 4, 5 };
SumArray(numbers);  // 15
```

## Search in Array

```csharp
public int FindInArray(int[] array, int target, int index = 0)
{
    // Base case - not found
    if (index >= array.Length)
        return -1;
    
    // Base case - found
    if (array[index] == target)
        return index;
    
    // Recursive case
    return FindInArray(array, target, index + 1);
}

// Usage
int[] numbers = { 5, 2, 8, 1, 9 };
FindInArray(numbers, 8);  // 2
```

## Tree Traversal

Recursion on hierarchical structures:

```csharp
public class TreeNode
{
    public int Value { get; set; }
    public TreeNode? Left { get; set; }
    public TreeNode? Right { get; set; }
}

public void PrintTree(TreeNode? node)
{
    // Base case - null node
    if (node == null)
        return;
    
    Console.WriteLine(node.Value);
    
    // Recursive calls
    PrintTree(node.Left);   // Left subtree
    PrintTree(node.Right);  // Right subtree
}

// Usage
var root = new TreeNode
{
    Value = 1,
    Left = new TreeNode { Value = 2 },
    Right = new TreeNode { Value = 3 }
};
PrintTree(root);  // Prints: 1, 2, 3
```

## String Recursion

```csharp
public void PrintStringReversed(string str, int index)
{
    // Base case - reached beginning
    if (index < 0)
        return;
    
    Console.Write(str[index]);
    
    // Recursive call
    PrintStringReversed(str, index - 1);
}

// Usage
PrintStringReversed("Hello", 4);  // Output: olleH
```

## Call Stack

Understanding how recursion works in memory:

```csharp
public int Calculate(int n)
{
    if (n <= 0)
        return 0;
    
    return n + Calculate(n - 1);
}

// Calculate(3) call stack:
// Calculate(3) -> return 3 + Calculate(2)
//   Calculate(2) -> return 2 + Calculate(1)
//     Calculate(1) -> return 1 + Calculate(0)
//       Calculate(0) -> return 0 (base case)
//     Calculate(1) -> return 1 + 0 = 1
//   Calculate(2) -> return 2 + 1 = 3
// Calculate(3) -> return 3 + 3 = 6

Console.WriteLine(Calculate(3));  // 6
```

## Recursion vs Iteration

Same logic, different approaches:

### Factorial - Recursive

```csharp
public int FactorialRecursive(int n)
{
    if (n <= 1)
        return 1;
    return n * FactorialRecursive(n - 1);
}
```

### Factorial - Iterative

```csharp
public int FactorialIterative(int n)
{
    int result = 1;
    for (int i = 2; i <= n; i++)
        result *= i;
    return result;
}
```

Both work, but iteration is often more efficient.

## Performance Considerations

### Problem: Exponential Complexity

```csharp
// This is SLOW - calculates same values multiple times
public int FibonacciSlow(int n)
{
    if (n <= 1)
        return n;
    return FibonacciSlow(n - 1) + FibonacciSlow(n - 2);
}

// Fib(5) calls: Fib(4) + Fib(3)
// Fib(4) calls: Fib(3) + Fib(2) <- Fib(3) called again!
```

### Solution: Memoization

```csharp
public int FibonacciMemo(int n, Dictionary<int, int>? memo = null)
{
    memo ??= new Dictionary<int, int>();
    
    if (n <= 1)
        return n;
    
    if (memo.ContainsKey(n))
        return memo[n];
    
    int result = FibonacciMemo(n - 1, memo) + FibonacciMemo(n - 2, memo);
    memo[n] = result;
    return result;
}

// Much faster - avoids recalculating
```

## When to Use Recursion

**Good for:**
- Tree/graph traversal
- Divide and conquer algorithms
- Backtracking problems
- Natural recursive structures

**Avoid for:**
- Simple loops
- Performance-critical code
- Very deep recursion (stack overflow risk)

## Stack Overflow Risk

```csharp
public void DangerousRecursion(int n)
{
    // This will cause StackOverflowException for large n
    DangerousRecursion(n + 1);  // No base case!
}

// Even with base case, very deep recursion can overflow:
public int DeepRecursion(int n)
{
    if (n <= 0)
        return 0;
    return 1 + DeepRecursion(n - 1);
}

DeepRecursion(100000);  // May cause stack overflow
```

## Common Patterns

### Pattern 1: Linear Recursion

```csharp
public int LinearSum(int n)
{
    if (n <= 0)
        return 0;
    return n + LinearSum(n - 1);
}
```

### Pattern 2: Tree Recursion

```csharp
public int TreeSum(int n)
{
    if (n <= 0)
        return 0;
    return n + TreeSum(n - 1) + TreeSum(n - 2);
}
```

### Pattern 3: Mutual Recursion

```csharp
public bool IsEven(int n)
{
    if (n == 0)
        return true;
    return IsOdd(n - 1);
}

public bool IsOdd(int n)
{
    if (n == 0)
        return false;
    return IsEven(n - 1);
}
```

## Summary

- **Recursion**: Method calls itself
- **Base case**: Stop condition
- **Recursive case**: Call with different parameters
- **Classic examples**: Factorial, Fibonacci, tree traversal
- **Performance**: Use memoization for repeated calculations
- **Stack**: Each call uses stack memory
- **When to use**: Natural recursive structures
- **When to avoid**: Simple loops, deep recursion

## Next Steps

- Learn [Method-Scope](../02-Method-Scope/00-Method-Scope.md) for method interaction patterns
- Study [Special-Methods](../03-Special-Methods/00-Special-Methods.md) for advanced method types
- Review [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) for recursion guidelines
