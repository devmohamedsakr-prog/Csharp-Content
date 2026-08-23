# Fundamentals Examples

This folder contains practical, runnable examples for all C# fundamentals concepts. Each file demonstrates real-world scenarios and best practices.

## Contents

### 01-Data-Types-Examples.cs
Comprehensive examples of C# data types including:
- Value types (byte, sbyte, short, ushort, int, uint, long, ulong)
- Floating-point types (float, double, decimal)
- Boolean and character types
- Reference types (string, object, dynamic)
- Type inference with `var`
- Type conversion (implicit, explicit, Parse, TryParse, Convert)

**Key Concepts:**
- Stack vs Heap allocation
- Type safety
- Boxing and unboxing
- Safe type conversion patterns

### 02-Operators-Examples.cs
Demonstration of all operator types:
- Arithmetic operators (+, -, *, /, %)
- Increment/Decrement (++, --)
- Comparison operators (==, !=, <, >, <=, >=)
- Logical operators (&&, ||, !)
- Short-circuit evaluation
- Assignment operators (+=, -=, *=, /=, %=)
- Bitwise operators (&, |, ^, ~, <<, >>)
- Ternary operator (? :)

**Key Concepts:**
- Operator precedence
- Boolean logic
- Bit manipulation
- Safe string comparison

### 03-Control-Flow-Examples.cs
Complete control flow structures:
- If-else statements
- If-else if-else chains
- Complex conditions (AND, OR, NOT)
- Switch statements with multiple cases
- Switch fall-through
- Nested control structures
- Nested if-switch combinations

**Key Concepts:**
- Decision making
- Complex conditions
- Code readability and structure
- DRY principle in control flow

### 04-Methods-Examples.cs
Method declaration and usage:
- Methods with no parameters and no return
- Methods with return types
- Multiple parameters
- Optional parameters with default values
- Named parameters
- Reference parameters (ref)
- Output parameters (out)
- Parameter arrays (params)
- Method overloading
- Recursive methods (factorial, Fibonacci)

**Key Concepts:**
- Method signatures
- Parameter passing mechanisms
- Overloading resolution
- Recursion and performance

### 05-Collections-Arrays-Examples.cs
Collections and data structures:
- Single-dimensional arrays
- Multi-dimensional arrays (2D)
- Jagged arrays
- Array methods (Sort, Reverse, IndexOf)
- Lists and List operations
- LINQ queries on collections
- Dictionaries and key-value pairs
- HashSets
- Queues (FIFO)
- Stacks (LIFO)
- Tuples

**Key Concepts:**
- When to use each collection type
- Performance characteristics
- LINQ integration
- Iteration patterns

### 06-Exception-Handling-Examples.cs
Exception handling and custom exceptions:
- Basic try-catch
- Multiple catch blocks
- Catch-all exception handler
- Try-catch-finally
- Finally block execution guarantees
- Throwing exceptions
- Rethrowing exceptions
- Custom exception classes
- Exception properties and data

**Key Concepts:**
- Exception hierarchy
- Specific vs general exception handling
- Resource cleanup with finally
- Custom exceptions for domain-specific errors

## How to Use These Examples

1. **Copy and Paste**: Each file is a complete, self-contained example that can be copied to a Visual Studio project

2. **Learn and Modify**: Study the examples, then modify them to experiment with different approaches

3. **Reference**: Use these as quick reference guides when implementing similar functionality

4. **Best Practices**: Each example follows C# coding standards and best practices

## Running the Examples

To compile and run any of these examples:

```bash
# In Visual Studio
1. Create a new Console Application project
2. Copy the contents of any .cs file
3. Replace the Program.cs content
4. Press F5 to run

# Or from command line
csc 01-Data-Types-Examples.cs
.\01-Data-Types-Examples.exe
```

## Related Concepts

These examples cover the foundation for:
- Object-oriented programming (see `02-OOP` folder)
- LINQ queries (see `04-LINQ` folder)
- Async programming (see `05-Async-Programming` folder)
- Data access (see `07-Database-Access` folder)

## Common Patterns and Anti-Patterns

### ✓ Good Patterns
- Use `TryParse` for user input validation
- Use `foreach` for collection iteration
- Use `out` parameter for multiple return values
- Use specific exception types in catch blocks
- Use `params` for variable argument counts

### ✗ Anti-Patterns to Avoid
- Using exception handling for control flow
- Catching generic `Exception` without reason
- Not using `finally` for cleanup
- Empty catch blocks
- Ignoring method return values

## Performance Tips

1. **Arrays vs Lists**: Use arrays for fixed-size collections, Lists for dynamic
2. **String concatenation**: Use `StringBuilder` for multiple concatenations
3. **LINQ**: Great for readability but be mindful of performance with large datasets
4. **Boxing**: Minimize boxing of value types
5. **Recursion**: Be careful with deep recursion (stack overflow risk)

## Additional Resources

- [Microsoft C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [C# Fundamentals on Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/)
- [Exception Handling Best Practices](https://docs.microsoft.com/en-us/archive/msdn-magazine/2008/january/best-practices-for-c-exception-handling)

## Next Steps

After mastering these fundamentals:
1. Study OOP concepts (classes, inheritance, polymorphism)
2. Learn async/await patterns
3. Explore LINQ for data manipulation
4. Understand dependency injection and design patterns
