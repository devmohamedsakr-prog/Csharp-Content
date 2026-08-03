# Foreach Loop

## Overview

Foreach loops iterate through collections without manual indexing. Simplest way to process all items.

## Basic Foreach

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

foreach (int num in numbers) {
    Console.WriteLine(num);
}

// Output: 10, 20, 30, 40, 50
```

**Syntax**: `foreach (type variable in collection)`

---

## Collections

### Array
```csharp
string[] fruits = { "Apple", "Banana", "Orange" };

foreach (string fruit in fruits) {
    Console.WriteLine(fruit);
}
```

### List
```csharp
List<int> scores = new() { 85, 90, 78, 92 };

foreach (int score in scores) {
    Console.WriteLine(score);
}
```

### Dictionary
```csharp
Dictionary<string, int> ages = new() {
    { "Alice", 25 },
    { "Bob", 30 },
    { "Charlie", 35 }
};

foreach (var person in ages) {
    Console.WriteLine($"{person.Key}: {person.Value}");
}

// Keys only
foreach (string name in ages.Keys) {
    Console.WriteLine(name);
}

// Values only
foreach (int age in ages.Values) {
    Console.WriteLine(age);
}
```

### HashSet
```csharp
HashSet<string> tags = new() { "C#", "Programming", "Learning" };

foreach (string tag in tags) {
    Console.WriteLine(tag);
}
```

### LINQ Results
```csharp
var adults = people.Where(p => p.Age >= 18);

foreach (var person in adults) {
    Console.WriteLine(person.Name);
}
```

---

## With Index (C# 7.1+)

```csharp
string[] items = { "First", "Second", "Third" };

foreach ((int index, string item) in items.Index()) {
    Console.WriteLine($"{index}: {item}");
}

// Output:
// 0: First
// 1: Second
// 2: Third
```

---

## Best Practices

✓ Use foreach by default
```csharp
// Good: simple and clear
foreach (var item in items) {
    Console.WriteLine(item);
}

// Use for only when needed
for (int i = 0; i < items.Count; i++) {
    // Need index
    items[i] = i + 1;
}
```

✓ Use meaningful variable names
```csharp
// Good
foreach (var student in students) {
    Console.WriteLine(student.Name);
}

// Less clear
foreach (var s in list) {
    Console.WriteLine(s.Name);
}
```

✓ Use var for readability
```csharp
// Good: type is obvious from context
foreach (var item in items) {
    Process(item);
}

// Also okay: explicit type
foreach (Item item in items) {
    Process(item);
}
```

---

## Common Mistakes

❌ Modifying collection during iteration
```csharp
foreach (var item in list) {
    if (item > 100) {
        list.Remove(item);  // InvalidOperationException!
    }
}
```

✓ Create copy or use LINQ
```csharp
// Option 1: LINQ
var filtered = list.Where(x => x <= 100).ToList();

// Option 2: Iterate copy
foreach (var item in list.ToList()) {
    if (item > 100) {
        list.Remove(item);  // Safe
    }
}
```

---

❌ Trying to modify iteration variable
```csharp
foreach (int x in numbers) {
    x = x * 2;  // Doesn't affect original array
}
Console.WriteLine(numbers[0]);  // Still original value
```

✓ Use for loop if modification needed
```csharp
for (int i = 0; i < numbers.Length; i++) {
    numbers[i] = numbers[i] * 2;  // Modifies array
}
```

---

## Performance

Foreach is very efficient:
```csharp
// All similar performance
foreach (var item in items) { }
for (int i = 0; i < items.Count; i++) { }  // Similar speed

// Foreach uses enumerator internally
// No performance penalty vs manual loop
```

---

## Real-World Examples

### Processing Users
```csharp
List<User> users = GetUsers();

foreach (var user in users) {
    if (user.IsActive) {
        SendNotification(user);
    }
}
```

### Building String
```csharp
var items = new[] { "Item1", "Item2", "Item3" };
var result = string.Join(", ", items);

// Or manually with foreach
var sb = new StringBuilder();
foreach (var item in items) {
    sb.Append(item).Append(", ");
}
string result = sb.ToString().TrimEnd(',', ' ');
```

### Dictionary Processing
```csharp
var settings = new Dictionary<string, string> {
    { "Theme", "Dark" },
    { "Language", "English" }
};

foreach (var setting in settings) {
    Console.WriteLine($"{setting.Key} = {setting.Value}");
}
```

---

## Foreach vs Other Loops

| Loop | Use |
|------|-----|
| Foreach | Iterate all items |
| For | Need index or count |
| While | Condition-based |
| Do-While | At least one execution |

---

## Next Steps

- Study [For Loop](../01-For-Loop/00-For-Loop.md)
- Learn [While Loop](../02-While-Do-While/00-While-Do-While.md)
- Review [Control Keywords](../../03-Control-Keywords/README.md)
