# Dictionary<K, V> - Key-Value Pairs

## Overview
Dictionary stores key-value pairs for fast lookup by key. Each key is unique.

## Creating Dictionaries

### Basic Creation

```csharp
// Empty dictionary
Dictionary<string, int> ages = new Dictionary<string, int>();

// With initialization
Dictionary<string, int> scores = new Dictionary<string, int> {
    { "Alice", 95 },
    { "Bob", 87 },
    { "Charlie", 92 }
};

// Modern syntax (C# 6+)
var scores2 = new Dictionary<string, int> {
    ["Alice"] = 95,
    ["Bob"] = 87
};
```

## Adding and Updating

### Add Elements

```csharp
Dictionary<string, int> ages = new Dictionary<string, int>();

// Using Add (throws if key exists)
ages.Add("Alice", 30);
ages.Add("Bob", 25);

// Using index (overwrites if exists)
ages["Charlie"] = 28;

// Add multiple
ages["David"] = 35;
```

### Update Values

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 }
};

// Update existing
ages["Alice"] = 31;

// Or use ContainsKey
if (ages.ContainsKey("Alice")) {
    ages["Alice"] = 31;
}
```

## Accessing Elements

### By Key

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

// Direct access
int aliceAge = ages["Alice"];  // 30

// Throws KeyNotFoundException if not found
int unknown = ages["Unknown"];  // Exception!
```

### Safe Access with TryGetValue

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

// Safe - no exception
if (ages.TryGetValue("Charlie", out int age)) {
    Console.WriteLine($"Age: {age}");
} else {
    Console.WriteLine("Not found");
}
```

### ContainsKey and ContainsValue

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

// Check key exists
bool hasAlice = ages.ContainsKey("Alice");  // true

// Check value exists
bool has30 = ages.ContainsValue(30);  // true
```

### Keys and Values

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 },
    { "Charlie", 28 }
};

// Get all keys
Dictionary<string, int>.KeyCollection keys = ages.Keys;
// {"Alice", "Bob", "Charlie"}

// Get all values
Dictionary<string, int>.ValueCollection values = ages.Values;
// {30, 25, 28}

// Convert to list
List<string> keyList = ages.Keys.ToList();
```

## Removing Elements

### Remove by Key

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

// Remove returns whether key was found
bool removed = ages.Remove("Alice");  // true
bool notFound = ages.Remove("Unknown");  // false
```

### Remove Matching (LINQ)

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 },
    { "Charlie", 35 }
};

// Remove ages > 30
var toRemove = ages.Where(x => x.Value > 30).Select(x => x.Key).ToList();
foreach (var key in toRemove) {
    ages.Remove(key);
}
```

### Clear Dictionary

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

ages.Clear();
// Dictionary is now empty
```

## Iterating Dictionaries

### Foreach with KeyValuePair

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

foreach (var kvp in ages) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
```

### Iterate Keys Only

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

foreach (string name in ages.Keys) {
    Console.WriteLine(name);
}
```

### Iterate Values Only

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};

foreach (int age in ages.Values) {
    Console.WriteLine(age);
}
```

## Common Patterns

### Pattern 1: Count Occurrences

```csharp
string text = "hello world";
Dictionary<char, int> charCount = new Dictionary<char, int>();

foreach (char c in text) {
    if (charCount.ContainsKey(c)) {
        charCount[c]++;
    } else {
        charCount[c] = 1;
    }
}

// Result: {'h': 1, 'e': 1, 'l': 3, 'o': 2, ...}
```

### Pattern 2: Cache Computed Values

```csharp
Dictionary<int, int> factorials = new Dictionary<int, int>();

int GetFactorial(int n) {
    if (factorials.ContainsKey(n)) {
        return factorials[n];  // Return cached
    }
    
    int result = ComputeFactorial(n);
    factorials[n] = result;
    return result;
}
```

### Pattern 3: Group Items

```csharp
string[] words = { "apple", "apricot", "banana", "blueberry" };
Dictionary<char, List<string>> byFirst = 
    new Dictionary<char, List<string>>();

foreach (var word in words) {
    char first = word[0];
    if (!byFirst.ContainsKey(first)) {
        byFirst[first] = new List<string>();
    }
    byFirst[first].Add(word);
}

// Result: {'a': ["apple", "apricot"], 'b': ["banana", "blueberry"]}
```

### Pattern 4: Merge Dictionaries

```csharp
Dictionary<string, int> dict1 = new Dictionary<string, int> {
    { "A", 1 },
    { "B", 2 }
};

Dictionary<string, int> dict2 = new Dictionary<string, int> {
    { "C", 3 },
    { "D", 4 }
};

// Merge into new dictionary
var merged = new Dictionary<string, int>(dict1);
foreach (var kvp in dict2) {
    merged[kvp.Key] = kvp.Value;
}
```

## LINQ with Dictionaries

### Select Keys/Values

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 },
    { "Charlie", 28 }
};

// Get names of people over 25
List<string> over25 = ages
    .Where(x => x.Value > 25)
    .Select(x => x.Key)
    .ToList();
// Result: ["Alice", "Charlie"]
```

### Order By Value

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 },
    { "Charlie", 28 }
};

// Sort by age (value)
var sorted = ages
    .OrderBy(x => x.Value)
    .ToDictionary(x => x.Key, x => x.Value);
```

## Special Considerations

### Key Types

```csharp
// String keys (most common)
Dictionary<string, int> dict1 = new Dictionary<string, int>();

// Integer keys
Dictionary<int, string> dict2 = new Dictionary<int, string>();

// Custom class as key (implement GetHashCode and Equals)
Dictionary<Person, int> dict3 = new Dictionary<Person, int>();

// Tuple as key
Dictionary<(int, string), double> dict4 = 
    new Dictionary<(int, string), double>();
```

### Performance

```csharp
// Lookup is O(1) average
int value = dict["key"];  // Fast

// Keys must be hashable
// Bad keys: unhashable objects, mutable keys
// Good keys: strings, numbers, immutable objects
```

## Best Practices

✓ **Use TryGetValue for safe access**
```csharp
if (dict.TryGetValue(key, out var value)) {
    Console.WriteLine(value);
}
```

✓ **Check key exists before access**
```csharp
if (dict.ContainsKey(key)) {
    var value = dict[key];
}
```

✓ **Use for fast lookups**
```csharp
// Fast O(1) average
var value = dict[key];

// Not for maintaining order
// Use SortedDictionary if needed
```

## Anti-Patterns

❌ **Direct access without checking**
```csharp
int age = ages["unknown"];  // May throw!
```

❌ **Mutable keys**
```csharp
var list = new List<int> { 1, 2, 3 };
var dict = new Dictionary<List<int>, string>();
dict[list] = "value";  // Dangerous!
```

## Summary

- Dictionary for key-value fast lookup O(1)
- TryGetValue for safe access
- Keys must be unique
- Perfect for caching and grouping
- Not ordered (use SortedDictionary if needed)

---

## Next Steps

1. Learn HashSet, Queue, Stack
2. Study Collection Patterns
3. Review Best Practices
