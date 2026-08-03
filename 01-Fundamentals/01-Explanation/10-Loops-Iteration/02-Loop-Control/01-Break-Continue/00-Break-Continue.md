# Break and Continue Statements

## Overview

`break` and `continue` statements provide fine-grained control over loop execution. They allow you to exit loops early or skip iterations conditionally.

## Break Statement

The `break` statement immediately exits the loop, regardless of the loop condition.

### Basic Break Syntax

```csharp
for (int i = 0; i < 10; i++)
{
    if (condition)
        break; // Exit loop immediately
}
// Code continues here after loop
```

### Break Examples

#### Simple Break

```csharp
// Exit loop when value found
for (int i = 0; i < 10; i++)
{
    if (i == 5)
    {
        Console.WriteLine("Found 5!");
        break;
    }
    Console.WriteLine(i); // 0, 1, 2, 3, 4
}
Console.WriteLine("Loop ended");
```

#### Search Pattern

```csharp
public int FindIndex(int[] array, int target)
{
    for (int i = 0; i < array.Length; i++)
    {
        if (array[i] == target)
        {
            return i; // Exit early - found
        }
    }
    return -1; // Not found
}

// Usage
int index = FindIndex(new[] { 10, 20, 30, 40 }, 30); // Returns 2
```

#### Menu Loop

```csharp
public void RunMenu()
{
    while (true)
    {
        Console.WriteLine("1. Add");
        Console.WriteLine("2. Delete");
        Console.WriteLine("3. Exit");
        int choice = int.Parse(Console.ReadLine());
        
        switch (choice)
        {
            case 1:
                Add();
                break; // Break from switch, not loop
            case 2:
                Delete();
                break; // Break from switch
            case 3:
                Console.WriteLine("Exiting...");
                break; // This breaks while loop
        }
        
        if (choice == 3)
            break; // Or explicitly break the while loop
    }
}
```

### Break in Switch

```csharp
// Break in switch exits switch, not surrounding loop
for (int i = 0; i < 3; i++)
{
    switch (i)
    {
        case 0:
            Console.WriteLine("Zero");
            break; // Exits switch only
            
        case 1:
            Console.WriteLine("One");
            break; // Exits switch only
            
        case 2:
            Console.WriteLine("Two");
            break; // Exits switch only
    }
}
// Loop continues normally
```

## Continue Statement

The `continue` statement skips the rest of the current iteration and jumps to the next iteration.

### Basic Continue Syntax

```csharp
for (int i = 0; i < 10; i++)
{
    if (condition)
        continue; // Skip to next iteration
}
```

### Continue Examples

#### Skip Specific Values

```csharp
// Skip even numbers
for (int i = 1; i <= 10; i++)
{
    if (i % 2 == 0)
        continue; // Skip even
    
    Console.WriteLine(i); // 1, 3, 5, 7, 9
}
```

#### Filter Pattern

```csharp
public void ProcessValidItems(List<Item> items)
{
    foreach (var item in items)
    {
        // Skip invalid items
        if (!item.IsValid)
            continue;
        
        // Process only valid items
        ProcessItem(item);
    }
}
```

#### Skip Condition

```csharp
for (int i = 0; i < 100; i++)
{
    if (i < 10 || i > 90)
        continue; // Skip first 10 and last 10
    
    Console.WriteLine(i); // 10-90
}
```

## Nested Loops and Break/Continue

### Continue in Nested Loop

```csharp
// Continue only affects inner loop
for (int i = 0; i < 3; i++)
{
    Console.WriteLine($"Outer: {i}");
    
    for (int j = 0; j < 3; j++)
    {
        if (j == 1)
            continue; // Skip inner iteration, not outer
        
        Console.WriteLine($"  Inner: {j}");
    }
}

// Output:
// Outer: 0
//   Inner: 0
//   Inner: 2
// Outer: 1
//   Inner: 0
//   Inner: 2
// Outer: 2
//   Inner: 0
//   Inner: 2
```

### Break in Nested Loop

```csharp
// Break only exits innermost loop
bool found = false;

for (int i = 0; i < 3 && !found; i++)
{
    for (int j = 0; j < 3; j++)
    {
        if (someCondition)
        {
            found = true;
            break; // Exits inner loop only
        }
    }
}

// Outer loop continues checking
```

### Breaking Multiple Levels (using goto)

```csharp
// Using goto to break multiple levels (use sparingly!)
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        if (foundTarget)
            goto ExitLoops; // Jump to label
    }
}

ExitLoops:
Console.WriteLine("Done");
```

### Better: Using Method Return

```csharp
public bool SearchMatrix(int[,] matrix, int target)
{
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            if (matrix[i, j] == target)
                return true; // Exit both loops
        }
    }
    return false;
}
```

## Patterns with Break and Continue

### Pattern 1: Skip and Process

```csharp
public void ProcessPrimes(int max)
{
    for (int i = 2; i <= max; i++)
    {
        // Skip even numbers (except 2)
        if (i > 2 && i % 2 == 0)
            continue;
        
        Console.WriteLine(i);
    }
}
```

### Pattern 2: Find and Exit

```csharp
public Customer FindByEmail(List<Customer> customers, string email)
{
    Customer found = null;
    
    foreach (var customer in customers)
    {
        if (customer.Email == email)
        {
            found = customer;
            break; // Exit once found
        }
    }
    
    return found;
}
```

### Pattern 3: Validation Loop

```csharp
public bool ValidateAll(List<Item> items)
{
    foreach (var item in items)
    {
        if (!item.IsValid)
            return false; // Failed validation
    }
    
    return true; // All valid
}
```

### Pattern 4: Early Termination

```csharp
public void ProcessUntilError(List<Task> tasks)
{
    foreach (var task in tasks)
    {
        try
        {
            task.Execute();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            break; // Stop on first error
        }
    }
}
```

### Pattern 5: Cleanup on Break

```csharp
public void ProcessWithCleanup()
{
    var resources = AcquireResources();
    
    try
    {
        for (int i = 0; i < 100; i++)
        {
            if (ShouldStop())
            {
                break; // Will still execute finally
            }
            
            Process(i);
        }
    }
    finally
    {
        resources.Dispose(); // Always executed
    }
}
```

## Break and Continue vs LINQ

### Imperative (with break/continue)

```csharp
var result = new List<int>();

for (int i = 0; i < numbers.Count; i++)
{
    if (numbers[i] < 0)
        continue; // Skip negative
    
    if (numbers[i] > 100)
        break; // Stop at 100
    
    result.Add(numbers[i] * 2);
}
```

### Functional (LINQ)

```csharp
var result = numbers
    .Where(n => n >= 0)           // Filter negative
    .TakeWhile(n => n <= 100)     // Stop at 100
    .Select(n => n * 2)           // Transform
    .ToList();
```

## Performance Impact

### Break Benefit: Early Exit

```csharp
// EFFICIENT: Exits when found
for (int i = 0; i < 1_000_000; i++)
{
    if (array[i] == target)
    {
        result = i;
        break; // Don't check remaining 999,999
    }
}

// INEFFICIENT: Checks all elements
var result = array.IndexOf(target);
```

### Continue Overhead

```csharp
// FAST: Simple loop
for (int i = 0; i < 1_000_000; i++)
{
    sum += i;
}

// SLIGHTLY SLOWER: With continue check
for (int i = 0; i < 1_000_000; i++)
{
    if (i % 2 == 1)
        continue;
    sum += i;
}
```

## Common Mistakes

### Mistake 1: Break Doesn't Exit Nested Loop

```csharp
// WRONG: Assumes break exits both loops
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        if (someCondition)
            break; // Only exits inner loop!
    }
    // Outer loop continues
}

// RIGHT: Use flag or method return
bool found = false;
for (int i = 0; i < 3 && !found; i++)
{
    for (int j = 0; j < 3; j++)
    {
        if (someCondition)
        {
            found = true;
            break;
        }
    }
}
```

### Mistake 2: Continue in Wrong Context

```csharp
// WRONG: Continue after other statements
for (int i = 0; i < 10; i++)
{
    DoWork(i);
    
    if (shouldSkip)
        continue;
    
    // This code is unreachable if continue executes
    MoreWork(i); // Never reached for skipped items
}

// RIGHT: Continue skips remaining code
for (int i = 0; i < 10; i++)
{
    if (shouldSkip)
        continue; // Skips MoreWork
    
    MoreWork(i);
}
```

### Mistake 3: Forgetting Break in Switch

```csharp
// WRONG: Fall-through case
switch (value)
{
    case 1:
        DoSomething();
        // Missing break - falls through!
    case 2:
        DoAnother();
        break;
}

// RIGHT: Break after each case
switch (value)
{
    case 1:
        DoSomething();
        break;
    case 2:
        DoAnother();
        break;
}
```

## Best Practices

1. **Use Break for Early Exit**
   ```csharp
   foreach (var item in items)
   {
       if (item == target)
       {
           result = item;
           break;
       }
   }
   ```

2. **Use Continue for Skipping**
   ```csharp
   foreach (var item in items)
   {
       if (!item.IsValid)
           continue; // Skip invalid
       
       Process(item);
   }
   ```

3. **Consider LINQ for Complex Logic**
   ```csharp
   var result = items
       .Where(x => x.IsValid)
       .TakeWhile(x => x.Value > 0)
       .ToList();
   ```

4. **Use Flags for Multi-Level Break**
   ```csharp
   bool found = false;
   for (int i = 0; i < 3 && !found; i++)
   {
       for (int j = 0; j < 3; j++)
       {
           if (someCondition)
           {
               found = true;
               break;
           }
       }
   }
   ```

5. **Prefer Methods for Complex Exit Logic**
   ```csharp
   public bool Find()
   {
       for (int i = 0; i < n; i++)
       {
           if (found)
               return true; // Clear exit
       }
       return false;
   }
   ```

## Summary

- **Break**: Exit loop immediately
- **Continue**: Skip to next iteration
- **Break only exits innermost loop**: Use flags for nested breaks
- **Continue skips rest of iteration**: All remaining code skipped
- **Use sparingly**: Clear, simple loops are easier to understand
- **Consider LINQ**: For complex filtering/transformation
