# StringBuilder and String Performance

## Overview
Understand when and how to use StringBuilder for efficient string operations. Learn about immutability implications.

---

## String Immutability

### Strings Are Immutable

```csharp
string text = "Hello";

// Each operation creates a new string
string upper = text.ToUpper();  // New object
string lower = text.ToLower();  // Another new object
string sub = text.Substring(0, 3);  // Another new object

// Original text unchanged
Console.WriteLine(text);  // Still "Hello"

// Every modification = new string
string result = text + " World";  // New string created
string modified = result.Replace("World", "C#");  // Another new string
```

### Performance Impact

```csharp
// Each concatenation creates new string (O(n) each)
// Multiple concatenations = O(n²) complexity

string result = "";
for (int i = 0; i < 1000; i++) {
    result += i.ToString();  // Creates new string each iteration
}
// Total: ~500,000 strings created (n²/2)
```

---

## StringBuilder Introduction

### When to Use StringBuilder

```csharp
// INEFFICIENT - String concatenation in loop
string inefficient = "";
for (int i = 0; i < 10000; i++) {
    inefficient += i.ToString();  // O(n²) - Very slow
}

// EFFICIENT - StringBuilder
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 10000; i++) {
    sb.Append(i.ToString());  // O(n) - Fast
}
string efficient = sb.ToString();
```

### StringBuilder Basics

```csharp
using System.Text;

// Create StringBuilder
StringBuilder sb = new StringBuilder();

// Append text
sb.Append("Hello");
sb.Append(" ");
sb.Append("World");

// Get result (single allocation)
string result = sb.ToString();  // "Hello World"

// StringBuilder is not a string
// Must convert with ToString()
```

---

## StringBuilder Methods

### Append Operations

```csharp
StringBuilder sb = new StringBuilder();

// Append string
sb.Append("Hello");

// Append line (adds newline)
sb.AppendLine("Line 1");
sb.AppendLine("Line 2");

// Append format (like string.Format)
sb.AppendFormat("Value: {0}", 42);

// Append character
sb.Append('!');

// Append multiple chars
char[] chars = { 'H', 'i' };
sb.Append(chars);
```

### Insert and Replace

```csharp
StringBuilder sb = new StringBuilder("Hello World");

// Insert at position
sb.Insert(5, " Beautiful");  // "Hello Beautiful World"

// Replace text
sb.Replace("Beautiful", "Wonderful");  // "Hello Wonderful World"

// Remove characters
sb.Remove(0, 6);  // "Wonderful World"
```

### Clear and Reuse

```csharp
StringBuilder sb = new StringBuilder("Hello");

// Clear contents (reuse StringBuilder)
sb.Clear();  // Now empty

// Reuse for different content
sb.Append("New content");
string result = sb.ToString();  // "New content"

// More efficient than creating new StringBuilder
```

---

## Performance Comparison

### String vs StringBuilder

```csharp
// BENCHMARK: Build 10,000 strings

// String concatenation (SLOW)
Stopwatch sw = Stopwatch.StartNew();
string result = "";
for (int i = 0; i < 10000; i++) {
    result += i.ToString();
}
sw.Stop();
Console.WriteLine($"String: {sw.ElapsedMilliseconds}ms");  // ~1000-2000ms

// StringBuilder (FAST)
sw.Restart();
var sb = new StringBuilder();
for (int i = 0; i < 10000; i++) {
    sb.Append(i.ToString());
}
string result2 = sb.ToString();
sw.Stop();
Console.WriteLine($"StringBuilder: {sw.ElapsedMilliseconds}ms");  // ~5-10ms

// StringBuilder is 100-200x faster!
```

### Memory Usage

```csharp
// String allocation per concatenation
// After 10,000 iterations:
// String: 10,000 intermediate strings in memory
// StringBuilder: 1 final string + internal buffer

// StringBuilder is much more memory-efficient
```

---

## StringBuilder Capacity

### Managing Capacity

```csharp
// Default capacity is 16
StringBuilder sb = new StringBuilder();
Console.WriteLine(sb.Capacity);  // 16

// Grows as needed (doubles when full)
sb.Append("This is a longer string");
Console.WriteLine(sb.Capacity);  // 32 or more

// Pre-allocate if size known (avoids resizing)
StringBuilder sb2 = new StringBuilder(1000);
// Better performance if you know approximate size

// Get current length
Console.WriteLine(sb.Length);  // Actual content length
```

### Optimization Tip

```csharp
// If you know approximate final size, allocate upfront
int estimatedSize = 50000;
StringBuilder sb = new StringBuilder(estimatedSize);

// Much faster than growing dynamically
for (int i = 0; i < 10000; i++) {
    sb.Append(i.ToString());
}
```

---

## Practical Examples

### Building CSV

```csharp
var data = new[] { "Alice", "30", "NYC" };

// INEFFICIENT
string csv = string.Join(",", data) + "\n";  // OK for single line

// EFFICIENT - Multiple lines
StringBuilder sb = new StringBuilder();
sb.AppendLine(string.Join(",", new[] { "Name", "Age", "City" }));
sb.AppendLine(string.Join(",", data));
string result = sb.ToString();
```

### Building JSON

```csharp
var items = new[] {
    (Name: "Alice", Age: 30),
    (Name: "Bob", Age: 25),
    (Name: "Charlie", Age: 35)
};

// Using StringBuilder
StringBuilder sb = new StringBuilder();
sb.AppendLine("[");

for (int i = 0; i < items.Length; i++) {
    sb.Append($"  {{ \"name\": \"{items[i].Name}\", \"age\": {items[i].Age} }}");
    if (i < items.Length - 1) {
        sb.AppendLine(",");
    } else {
        sb.AppendLine();
    }
}

sb.AppendLine("]");
string json = sb.ToString();
```

### Building SQL

```csharp
var ids = new[] { 1, 2, 3, 4, 5 };

// Using StringBuilder
StringBuilder sb = new StringBuilder();
sb.Append("SELECT * FROM Users WHERE ID IN (");

for (int i = 0; i < ids.Length; i++) {
    sb.Append(ids[i]);
    if (i < ids.Length - 1) {
        sb.Append(", ");
    }
}

sb.Append(")");
string sql = sb.ToString();  // "SELECT * FROM Users WHERE ID IN (1, 2, 3, 4, 5)"
```

---

## String Joining Techniques

### String.Join (Preferred)

```csharp
var items = new[] { "apple", "banana", "orange" };

// Most efficient for this pattern
string result = string.Join(", ", items);  // "apple, banana, orange"

// With LINQ
var numbers = new[] { 1, 2, 3, 4, 5 };
string numStr = string.Join("-", numbers.Select(n => n * 2));
// "2-4-6-8-10"
```

### StringBuilder for Complex Joining

```csharp
var items = new[] { "apple", "banana", "orange" };

// When you need special formatting
StringBuilder sb = new StringBuilder();
for (int i = 0; i < items.Length; i++) {
    if (i > 0) sb.Append(", ");
    sb.Append(items[i].ToUpper());
}
string result = sb.ToString();  // "APPLE, BANANA, ORANGE"
```

---

## Decision Guide

### Use String When

```csharp
// Single operation
string msg = $"Hello {name}";

// Few concatenations
string full = first + " " + last;

// One-time formatting
string formatted = string.Format("Value: {0}", value);

// Using string.Join
string joined = string.Join(",", items);
```

### Use StringBuilder When

```csharp
// Loop with concatenation
StringBuilder sb = new StringBuilder();
foreach (var item in items) {
    sb.Append(item);
}

// Building large output
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 10000; i++) {
    sb.AppendLine($"Line {i}");
}

// Multiple string operations
var sb = new StringBuilder("Start");
sb.Append(" Middle");
sb.Append(" End");
```

---

## Common Mistakes

### ❌ Using String in Loop

```csharp
// WRONG - O(n²) complexity
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i.ToString();  // Very slow!
}
```

✓ **Use StringBuilder:**
```csharp
// RIGHT - O(n) complexity
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i.ToString());  // Fast!
}
string result = sb.ToString();
```

### ❌ Forgetting ToString()

```csharp
StringBuilder sb = new StringBuilder("Hello");
string msg = sb;  // Error! Cannot implicitly convert
```

✓ **Explicitly convert:**
```csharp
string msg = sb.ToString();
```

### ❌ Not Pre-allocating for Large Strings

```csharp
// Inefficient - many reallocations
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 100000; i++) {
    sb.Append(LongString);  // Grows and reallocates many times
}
```

✓ **Pre-allocate:**
```csharp
StringBuilder sb = new StringBuilder(1000000);  // Allocate once
for (int i = 0; i < 100000; i++) {
    sb.Append(LongString);  // No reallocations
}
```

---

## Performance Summary

| Operation | Efficiency | Use Case |
|-----------|-----------|----------|
| `+` operator | Slow for loops | Single concat |
| `string.Format` | Reasonable | Formatting |
| `string.Join` | Fast | Joining arrays |
| `StringBuilder` | Very Fast | Loops with many appends |
| String interpolation | Good | General use |

---

## Best Practices

✓ Use `StringBuilder` for loops with string concatenation
✓ Pre-allocate capacity if size is known
✓ Use `string.Join()` for joining collections
✓ Use string interpolation for simple formatting
✓ Don't use `+` for many concatenations
✓ Always call `ToString()` when done with StringBuilder
✓ Measure performance when in doubt

---

## Next Steps

1. Review Best Practices
2. Study Common Mistakes
3. Prepare Interview Questions
