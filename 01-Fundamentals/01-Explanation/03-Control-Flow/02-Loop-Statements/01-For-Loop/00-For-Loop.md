# For Loop

## Overview

For loops execute code a specific number of times. Perfect when you know the iteration count.

## Standard For Loop

```csharp
for (int i = 0; i < 5; i++) {
    Console.WriteLine($"Iteration {i}");
}

// Output: 0, 1, 2, 3, 4
```

**Syntax**: `for (initialize; condition; increment)`
- **Initialize**: Run once before loop starts
- **Condition**: Checked before each iteration
- **Increment**: Run after each iteration

---

## Loop Anatomy

```csharp
for (int i = 0;      // Initialize: set starting value
     i < 5;          // Condition: continue while true
     i++)            // Increment: update after each iteration
{
    Console.WriteLine(i);  // Loop body
}
```

---

## Common Patterns

### Loop Forward
```csharp
for (int i = 0; i < 10; i++) {
    Console.WriteLine(i);  // 0 to 9
}
```

### Loop Backward
```csharp
for (int i = 10; i >= 0; i--) {
    Console.WriteLine(i);  // 10 to 0
}
```

### Loop by 2
```csharp
for (int i = 0; i < 10; i += 2) {
    Console.WriteLine(i);  // 0, 2, 4, 6, 8
}
```

### Nested Loops
```csharp
// Multiplication table
for (int i = 1; i <= 3; i++) {
    for (int j = 1; j <= 3; j++) {
        Console.WriteLine($"{i} * {j} = {i * j}");
    }
}
```

---

## Advanced Patterns

### Multiple Initializers
```csharp
for (int i = 0, j = 10; i < j; i++, j--) {
    Console.WriteLine($"i={i}, j={j}");
}
```

### Infinite Loop (use break to exit)
```csharp
for (;;) {
    Console.WriteLine("Running...");
    break;  // Exit
}
```

### Complex Condition
```csharp
for (int i = 0; i < 10 && !shouldStop; i++) {
    // Continue while i < 10 AND shouldStop is false
}
```

---

## Array Iteration

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

for (int i = 0; i < numbers.Length; i++) {
    Console.WriteLine(numbers[i]);
}
```

---

## Best Practices

✓ Use foreach for simple iteration
```csharp
// Good
foreach (var item in items) {
    Console.WriteLine(item);
}

// Less ideal (when foreach works)
for (int i = 0; i < items.Count; i++) {
    Console.WriteLine(items[i]);
}
```

✓ Use meaningful variable names
```csharp
// Good
for (int row = 0; row < grid.Height; row++) {
    for (int col = 0; col < grid.Width; col++) {
        // Process grid
    }
}

// Less clear
for (int i = 0; i < 10; i++) {
    for (int j = 0; j < 10; j++) {
        // Process
    }
}
```

✓ Avoid deep nesting
```csharp
// Extract to method
for (int i = 0; i < 10; i++) {
    ProcessRow(i);
}

void ProcessRow(int row) {
    for (int col = 0; col < 10; col++) {
        // Process
    }
}
```

---

## Common Mistakes

❌ Off-by-one error
```csharp
for (int i = 0; i <= 5; i++) {  // Includes 5 (0-5 = 6 items)
    Console.WriteLine(i);
}
```

✓ Correct range
```csharp
for (int i = 0; i < 5; i++) {   // Excludes 5 (0-4 = 5 items)
    Console.WriteLine(i);
}
```

---

❌ Modifying collection during iteration
```csharp
for (int i = 0; i < list.Count; i++) {
    if (list[i] > 100) {
        list.RemoveAt(i);  // Don't do this!
    }
}
```

✓ Iterate backward or copy
```csharp
for (int i = list.Count - 1; i >= 0; i--) {
    if (list[i] > 100) {
        list.RemoveAt(i);  // Safe when going backward
    }
}
```

---

## For vs Other Loops

| Loop | When to Use |
|------|------------|
| For | Know exact iteration count |
| While | Unknown iteration count |
| Do-While | Need at least one execution |
| Foreach | Iterate collection |

---

## Performance

For loops are very fast:
```csharp
// All similar performance
for (int i = 0; i < 1000; i++) { }
for (int i = 0; i < 1000; ++i) { }  // No real difference
```

---

## Next Steps

- Study [While and Do-While](../02-While-Do-While/00-While-Do-While.md)
- Learn [Foreach Loop](../03-ForEach-Loop/00-ForEach-Loop.md)
- Review [Control Keywords](../../03-Control-Keywords/README.md)
