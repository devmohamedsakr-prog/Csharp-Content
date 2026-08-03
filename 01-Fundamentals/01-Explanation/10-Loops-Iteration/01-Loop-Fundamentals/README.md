# Loop Fundamentals

## Overview

This section covers the foundational loop types in C#. Master these core loops before moving to advanced iteration patterns.

## Learning Path

### Beginner
1. **[For Loops](01-For-Loops/00-For-Loops.md)** - Start here
   - Basic for loop syntax
   - Iteration patterns
   - Working with collections
   - Performance characteristics

2. **[While Loops](02-While-Loops/00-While-Loops.md)** - Condition-based iteration
   - While vs do-while
   - Common patterns (menu, validation, retry)
   - Infinite loop prevention

3. **[Foreach Loops](03-Foreach-Loops/00-Foreach-Loops.md)** - Collection iteration
   - Simple, readable iteration
   - Collections and IEnumerable
   - Index access patterns

### Intermediate
- Review all three loop types
- Practice choosing the right loop
- Combine with Loop-Control section

### Advanced
- Study performance characteristics
- Compare with LINQ alternatives
- Move to Advanced-Iteration section

## Quick Reference

### Loop Type Comparison

| Feature | For | While | Do-While | Foreach |
|---------|-----|-------|----------|---------|
| **Index access** | ✓ | ✗ | ✗ | ✗ |
| **Count known** | ✓ | ✗ | ✗ | ✓ |
| **Condition-based** | ✓ | ✓ | ✓ | ✗ |
| **Always executes** | ✗ | ✗ | ✓ | ✗ |
| **Collections** | ✓ | Limited | Limited | ✓ |
| **Readability** | Good | Good | Fair | Best |

### When to Use Each

**For Loop**
```csharp
// When you need: index, count known, or control
for (int i = 0; i < 10; i++)
{
    Process(items[i]);
}
```

**While Loop**
```csharp
// When: condition-based, no count known
while (reader.HasData())
{
    ProcessData(reader);
}
```

**Do-While Loop**
```csharp
// When: must execute at least once
do
{
    response = GetInput();
} while (!IsValid(response));
```

**Foreach Loop**
```csharp
// When: simple collection iteration, no index
foreach (var item in items)
{
    Process(item);
}
```

## Topics Covered

### For Loops
- Basic syntax and structure
- Loop variations (increment/decrement)
- Reverse iteration
- Working with arrays and lists
- Common for loop patterns
- Performance characteristics

### While Loops
- Basic while syntax
- Do-while loops (post-test)
- Menu-driven loops
- Input validation loops
- Retry patterns
- Infinite loop detection

### Foreach Loops
- Basic foreach syntax
- IEnumerable collections
- Working with lists, arrays, strings
- Index access with foreach
- Performance vs for loops
- Common foreach patterns

## Code Examples

### Example 1: Choose the Right Loop

```csharp
// TASK: Print numbers 1-5

// FOR: Good choice (count known)
for (int i = 1; i <= 5; i++)
    Console.WriteLine(i);

// WHILE: Works but less clear
int num = 1;
while (num <= 5)
{
    Console.WriteLine(num);
    num++;
}

// FOREACH: Not applicable (no collection)
```

### Example 2: Iterating Collections

```csharp
var names = new List<string> { "Alice", "Bob", "Carol" };

// FOR: Need index
for (int i = 0; i < names.Count; i++)
    Console.WriteLine($"{i}: {names[i]}");

// FOREACH: Simple iteration
foreach (var name in names)
    Console.WriteLine(name);
```

### Example 3: Condition-Based

```csharp
// WHILE: Good for conditions
string input = "";
while (input != "quit")
{
    input = Console.ReadLine();
}

// FOR: Awkward (count unknown)
// Not recommended
```

## Practice Exercises

### Exercise 1: Print 1-100
```csharp
// TODO: Print numbers 1 to 100
// Choose: for, while, or foreach?
```

### Exercise 2: Sum Array
```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
// TODO: Calculate sum using a loop
// Try all three loop types
```

### Exercise 3: Search List
```csharp
var items = new List<string> { "apple", "banana", "cherry" };
// TODO: Find index of "banana"
// Which loop type works best?
```

## Troubleshooting

### "Loop Not Executing"
- Check initial condition
- Verify loop counter initialization
- Ensure condition can be true

### "Infinite Loop"
- Check loop counter updates
- Verify condition can become false
- Add debug output

### "Off-by-One Error"
- Check loop boundaries
- Use < not <=
- Test edge cases

## Next Steps

1. **Read** each loop type (For, While, Foreach)
2. **Try** the practice exercises
3. **Compare** loop characteristics
4. **Move to** [Loop-Control](../02-Loop-Control/README.md) for break/continue

## Summary Table

| Loop | Use When | Example |
|------|----------|---------|
| For | Index needed | `for (int i = 0; i < n; i++)` |
| While | Condition-based | `while (condition)` |
| Do-While | Must run once | `do { } while (condition)` |
| Foreach | Simple iteration | `foreach (var item in items)` |

## Links

- **Previous**: [Scope & Lifetime](../09-Scope-Lifetime/README.md)
- **Next**: [Loop-Control](../02-Loop-Control/README.md)
- **All Topics**: [Loops Overview](README.md)

---

**Pro Tip**: Most real code uses foreach for collections and for when you need the index. Master these two first.
