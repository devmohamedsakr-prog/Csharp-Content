# Comparison, Best Practices & Common Mistakes

## Overview

This section provides practical guidance on choosing between value and reference types, best practices for different data types, and common pitfalls to avoid.

## Categories in This Section

### 1. Value vs Reference Comparison
Detailed comparison of value types and reference types covering storage, copying, performance, and when to use each.

**Files**: `01-Value-vs-Reference/00-Value-Reference-Comparison.md`

### 2. Data Type Best Practices
Guidelines for selecting and using data types effectively in real applications.

**Files**: `02-Best-Practices/00-DataType-BestPractices.md`

### 3. Common Mistakes
Real-world mistakes developers make with data types and how to avoid them.

**Files**: `03-Common-Mistakes/00-Common-Mistakes.md`

## Quick Decision Guide

### Choosing Between Value and Reference Types

```
Are you storing simple, small data?
├─ Yes → Consider value type (struct, int, bool)
└─ No → Use reference type (class, List, etc)

Does your data need inheritance?
├─ Yes → Must use class (reference type)
└─ No → Could be value type or reference type

Will you modify this data frequently?
├─ Yes → Use mutable reference type (class)
└─ No → Could use immutable value type (struct)

Is performance critical?
├─ Yes → Measure; value types might be faster
└─ No → Use reference types for flexibility
```

### Choosing Collections

```
Need fast lookup by key?
├─ Yes → Use Dictionary<K,V> (O(1))

Need to maintain insertion order?
├─ Yes → Use List<T>

Need to track unique items?
├─ Yes → Use HashSet<T> (O(1) lookups)

Need sorted order?
├─ Yes → Use SortedDictionary or SortedList

Default case?
└─ Use List<T> (most flexible and common)
```

### Choosing String Handling

```
Building strings in a loop?
├─ Moderate iterations (< 10) → String concatenation OK
├─ Many iterations (> 100) → Use StringBuilder
├─ Very high volume → Consider stream/buffer

Comparing strings?
├─ Case-insensitive → Use StringComparison.OrdinalIgnoreCase
├─ Culture-aware → Use StringComparison.CurrentCulture
└─ Fast & safe → Use StringComparison.Ordinal

Need string formatting?
├─ Simple interpolation → Use $"..." syntax
├─ Complex formatting → Use string.Format()
└─ High volume → Use StringBuilder.AppendFormat()
```

## Core Principles

### 1. Type Safety
```csharp
// Explicit types
int count = 5;
string name = "Alice";
List<User> users = new();

// Avoid object for type safety
object count2 = 5;
int unwrapped = (int)count2;  // Runtime type checking
```

### 2. Null Safety
```csharp
#nullable enable

// Indicate nullability explicitly
public string? MiddleName { get; set; }
public string FirstName { get; set; }  // Cannot be null

// Check before use
if (person.MiddleName != null) {
    Console.WriteLine(person.MiddleName.Length);
}
```

### 3. Immutability
```csharp
// Prefer immutable when possible
public readonly struct Point {
    public int X { get; }
    public int Y { get; }
}

// Immutable collections
IReadOnlyList<string> items = new List<string>().AsReadOnly();
```

### 4. Performance Awareness
```csharp
// Be aware of boxing
int value = 42;
object boxed = value;  // Boxing - allocation + copy
int unboxed = (int)boxed;  // Unboxing - copy

// Avoid in performance-critical code
List<int> list = new();  // No boxing
ArrayList array = new();  // Boxing on each add!
```

## Common Data Type Mistakes

### ❌ Mistake 1: Using float for Money
```csharp
float total = 0.1f + 0.2f;
if (total == 0.3f) { }  // FALSE - precision error!
```

### ✓ Correct: Use decimal
```csharp
decimal total = 0.1m + 0.2m;
if (total == 0.3m) { }  // TRUE - exact
```

### ❌ Mistake 2: String Concatenation in Loops
```csharp
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;  // Creates 1000 strings - very slow
}
```

### ✓ Correct: Use StringBuilder
```csharp
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);  // Efficient - one allocation
}
string result = sb.ToString();
```

### ❌ Mistake 3: Not Checking Null
```csharp
public void ProcessUser(User user) {
    string name = user.Name;  // Could crash if null
}
```

### ✓ Correct: Validate Inputs
```csharp
public void ProcessUser(User user) {
    if (user == null) throw new ArgumentNullException(nameof(user));
    if (user.Name == null) throw new ArgumentNullException(nameof(user.Name));
    
    string name = user.Name;  // Safe
}
```

### ❌ Mistake 4: Modifying Collection During Iteration
```csharp
foreach (var item in list) {
    if (item.Value > 100) {
        list.Remove(item);  // InvalidOperationException!
    }
}
```

### ✓ Correct: Use LINQ or Copy
```csharp
// Option 1: LINQ
var filtered = list.Where(x => x.Value <= 100).ToList();

// Option 2: Iterate copy
foreach (var item in list.ToList()) {
    if (item.Value > 100) {
        list.Remove(item);  // Works - iterating copy
    }
}
```

### ❌ Mistake 5: Using Wrong Collection Type
```csharp
List<int> approved = new() { 1, 2, 3, 4, 5 };
// Inside loop 1000000 times:
if (approved.Contains(i)) { }  // O(n) - very slow!
```

### ✓ Correct: Choose Right Collection
```csharp
HashSet<int> approved = new() { 1, 2, 3, 4, 5 };
// Inside loop 1000000 times:
if (approved.Contains(i)) { }  // O(1) - much faster!
```

## Best Practices Checklist

### General
- [ ] Use specific types instead of `object`
- [ ] Enable nullable reference types (`#nullable enable`)
- [ ] Check for null before using reference types
- [ ] Choose appropriate collections for your access patterns
- [ ] Avoid boxing/unboxing when possible

### Value Types
- [ ] Keep structs small (typically < 16 bytes)
- [ ] Make structs immutable (readonly)
- [ ] Don't make mutable structs (hard to reason about)
- [ ] Use well-known value types (int, decimal, bool)

### Reference Types
- [ ] Return `IReadOnlyList<T>` instead of `List<T>` when possible
- [ ] Use `using` statements for `IDisposable` types
- [ ] Implement `Equals` and `GetHashCode` when needed
- [ ] Be careful with circular references

### Strings
- [ ] Use `decimal StringComparison` for comparisons
- [ ] Use `StringBuilder` for loop-based concatenation
- [ ] Use `$"..."` string interpolation for readability
- [ ] Remember strings are immutable

### Collections
- [ ] Default to `List<T>` for ordered collections
- [ ] Use `HashSet<T>` for unique items and fast lookups
- [ ] Use `Dictionary<K,V>` for key-value pairs
- [ ] Consider `IReadOnlyList<T>` for immutable contracts

## Navigation

- **Parent**: [Data Types](../README.md)
- **Value vs Reference**: `01-Value-vs-Reference/00-Value-Reference-Comparison.md`
- **Best Practices**: `02-Best-Practices/00-DataType-BestPractices.md`
- **Common Mistakes**: `03-Common-Mistakes/00-Common-Mistakes.md`
- **Interview Questions**: `../04-Interview-Questions/README.md`
- **Value Types**: `../01-Value-Types/README.md`
- **Reference Types**: `../02-Reference-Types/README.md`
