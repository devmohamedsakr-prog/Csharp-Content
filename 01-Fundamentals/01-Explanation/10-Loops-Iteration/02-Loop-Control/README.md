# Loop Control

## Overview

This section covers controlling loop flow with break/continue and handling nested loops. Learn to write efficient, correct loop logic.

## Learning Path

### Beginner
1. **[Break & Continue](01-Break-Continue/00-Break-Continue.md)** - Start here
   - Break statement (exit loop)
   - Continue statement (skip iteration)
   - When to use each
   - Common patterns

2. **[Nested Loops](02-Nested-Loops/00-Nested-Loops.md)** - Multiple levels
   - Basic nested loops
   - 2D arrays and matrices
   - Break/continue in nested loops
   - Performance considerations

### Intermediate
- Combine break/continue with different loop types
- Identify nested loop patterns
- Optimize performance

### Advanced
- Complex control flow
- Performance analysis (O(n²) issues)
- Move to Advanced-Iteration section

## Quick Reference

### Break Statement

Immediately exits the loop (skips remaining iterations):

```csharp
// Search for value
for (int i = 0; i < array.Length; i++)
{
    if (array[i] == target)
    {
        Console.WriteLine($"Found at index {i}");
        break; // Exit loop immediately
    }
}
```

### Continue Statement

Skips current iteration, continues with next:

```csharp
// Process valid items
foreach (var item in items)
{
    if (!item.IsValid)
        continue; // Skip to next iteration
    
    ProcessItem(item);
}
```

### Nested Loops

Multiple levels of iteration:

```csharp
// Print multiplication table
for (int i = 1; i <= 3; i++)
{
    for (int j = 1; j <= 3; j++)
    {
        Console.Write($"{i * j:D2} ");
    }
    Console.WriteLine();
}
```

## Topics Covered

### Break & Continue
- Break statement syntax and behavior
- Continue statement syntax and behavior
- When to use each
- Multiple break statements
- Break vs return
- Performance implications
- Common patterns
- Mistakes to avoid

### Nested Loops
- Basic nested loops
- 2D arrays and matrices
- Triple nested loops
- Break/continue in nested context
- Early exit strategies
- Performance analysis
- Optimization techniques

## Performance Patterns

### O(n) - Linear
```csharp
// Single loop: O(n)
foreach (var item in items)
    Process(item);
```

### O(n²) - Quadratic
```csharp
// Nested loops: O(n²) - Watch out!
for (int i = 0; i < items.Count; i++)
    for (int j = 0; j < items.Count; j++)
        Compare(items[i], items[j]);
```

### O(n²) Optimization
```csharp
// Use HashSet: O(n)
var set = new HashSet<Item>(items);
foreach (var item in items)
    if (set.Contains(item))
        Process(item);
```

## Code Examples

### Example 1: Search with Break

```csharp
// LINEAR SEARCH - O(n)
public int FindIndex(int[] array, int target)
{
    for (int i = 0; i < array.Length; i++)
    {
        if (array[i] == target)
            return i; // Found! Exit early
    }
    return -1; // Not found
}

// Usage
int index = FindIndex(new[] { 1, 2, 3, 4, 5 }, 3);
Console.WriteLine(index); // 2
```

### Example 2: Filter with Continue

```csharp
// FILTER ITEMS
public void ProcessActiveItems(List<Item> items)
{
    foreach (var item in items)
    {
        if (!item.IsActive)
            continue; // Skip inactive
        
        Console.WriteLine($"Processing: {item.Name}");
    }
}
```

### Example 3: Nested Loops

```csharp
// MULTIPLICATION TABLE
public void PrintMultiplicationTable(int size)
{
    for (int i = 1; i <= size; i++)
    {
        for (int j = 1; j <= size; j++)
        {
            Console.Write($"{i * j:D3} ");
        }
        Console.WriteLine();
    }
}

// Output:
//   1   2   3
//   2   4   6
//   3   6   9
```

### Example 4: Break in Nested Loops

```csharp
// Only breaks inner loop
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        if (j == 1)
            break; // Only exits inner loop
    }
    Console.WriteLine($"Outer {i}"); // Still prints 3 times
}

// To exit both: use method return or flag
public bool Find2D(int[,] matrix, int target)
{
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            if (matrix[i, j] == target)
                return true; // Exits both loops
        }
    }
    return false;
}
```

### Example 5: Performance Comparison

```csharp
// SLOW: O(n²)
public bool ContainsAll(List<int> list1, List<int> list2)
{
    foreach (var item in list1)
    {
        bool found = false;
        foreach (var item2 in list2) // O(n²) - nested!
        {
            if (item == item2)
            {
                found = true;
                break;
            }
        }
        if (!found) return false;
    }
    return true;
}

// FAST: O(n)
public bool ContainsAllFast(List<int> list1, List<int> list2)
{
    var set = new HashSet<int>(list2);
    foreach (var item in list1)
        if (!set.Contains(item))
            return false;
    return true;
}
```

## Decision Tree

```
Does loop need to exit early?
├─ YES → Use break
└─ NO → Continue to next check

Does iteration need to skip?
├─ YES → Use continue
└─ NO → Normal flow

Are you nesting loops?
├─ YES → Check performance (O(n²)?)
│   ├─ Too slow → Optimize
│   └─ Acceptable → OK
└─ NO → Single loop
```

## Practice Exercises

### Exercise 1: Search
```csharp
// TODO: Find first even number
int[] numbers = { 1, 3, 5, 4, 7, 9 };
// Use break when found
```

### Exercise 2: Filter
```csharp
// TODO: Print only positive numbers
int[] nums = { -1, 2, -3, 4, -5, 6 };
// Use continue for negative
```

### Exercise 3: Matrix Sum
```csharp
// TODO: Sum all elements in 2D array
int[,] matrix = { { 1, 2 }, { 3, 4 } };
// Use nested loops
```

### Exercise 4: Find Pair
```csharp
// TODO: Find two numbers that sum to 10
int[] nums = { 2, 3, 5, 7, 8 };
// Use nested loop or HashSet?
```

## Common Mistakes

### Mistake 1: Break in Wrong Context
```csharp
// WRONG: Break only exits inner loop
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        if (condition)
            break; // Only exits j loop!
    }
}
```

### Mistake 2: O(n²) Performance
```csharp
// SLOW: O(n²)
for (int i = 0; i < 1000; i++)
    for (int j = 0; j < 1000; j++)
        CheckMatch(i, j); // 1,000,000 iterations!
```

### Mistake 3: Continue Logic
```csharp
// Unclear what continues
foreach (var item in items)
{
    if (item.Check1) continue;
    if (item.Check2) continue;
    if (item.Check3) continue;
    Process(item); // Very nested logic
}
```

## Troubleshooting

### "Break Not Working"
- Check if in nested loop (only exits current loop)
- Use return to exit multiple levels
- Use flag variable for outer loop control

### "Infinite Loop"
- Verify break condition can be true
- Check counter updates
- Add debug output

### "Wrong Iterations Skipped"
- Continue skips ONE iteration only
- Check condition logic
- May need multiple continues

## Optimization Checklist

- [ ] Nested loops O(n²)?
- [ ] Could use HashSet instead?
- [ ] Early exit with break?
- [ ] Skip unnecessary work?
- [ ] Cache values outside loop?
- [ ] Performance acceptable?

## Next Steps

1. **Read** Break-Continue details
2. **Read** Nested-Loops patterns
3. **Practice** exercises above
4. **Optimize** nested loop examples
5. **Move to** [Advanced-Iteration](../03-Advanced-Iteration/README.md)

## Performance Summary

| Pattern | Complexity | When to Use |
|---------|-----------|-----------|
| Single loop | O(n) | Default choice |
| Nested loops | O(n²) | Only if necessary |
| With break | O(n) avg | Early exit |
| HashSet lookup | O(1) | Duplicate search |
| LINQ | O(n) | Complex operations |

## Links

- **Previous**: [Loop-Fundamentals](../01-Loop-Fundamentals/README.md)
- **Next**: [Advanced-Iteration](../03-Advanced-Iteration/README.md)
- **All Topics**: [Loops Overview](../README.md)

---

**Pro Tip**: If you find yourself writing nested loops, pause and think: "Could I use a HashSet or LINQ instead?" Usually you can.
