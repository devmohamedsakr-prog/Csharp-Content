# HashSet<T> - Unique Values

## Overview
HashSet stores unique values only. Adding duplicates has no effect. Perfect for membership testing and removing duplicates.

## Creating HashSets

### Basic Creation

```csharp
// Empty HashSet
HashSet<int> numbers = new HashSet<int>();

// With initialization
HashSet<string> fruits = new HashSet<string> {
    "apple", "banana", "cherry"
};

// From another collection
int[] arr = { 1, 2, 2, 3, 3, 3 };
HashSet<int> unique = new HashSet<int>(arr);
// Result: {1, 2, 3}
```

## Adding and Removing

### Add Elements

```csharp
HashSet<int> numbers = new HashSet<int>();

// Add returns whether item was added
bool added1 = numbers.Add(1);   // true (new)
bool added2 = numbers.Add(2);   // true (new)
bool added3 = numbers.Add(1);   // false (duplicate)

// HashSet: {1, 2}
```

### Remove Elements

```csharp
HashSet<int> numbers = new HashSet<int> { 1, 2, 3, 4, 5 };

// Remove returns whether item was found
bool removed = numbers.Remove(3);      // true
bool notFound = numbers.Remove(99);    // false

// HashSet now: {1, 2, 4, 5}
```

### Clear

```csharp
HashSet<int> numbers = new HashSet<int> { 1, 2, 3 };
numbers.Clear();
// HashSet is now empty
```

## Checking Membership

```csharp
HashSet<string> fruits = new HashSet<string> {
    "apple", "banana", "cherry"
};

// Check if contains
bool hasApple = fruits.Contains("apple");      // true
bool hasOrange = fruits.Contains("orange");    // false

// Fast O(1) operation
```

## Set Operations

HashSet supports mathematical set operations.

### Union

```csharp
HashSet<int> set1 = new HashSet<int> { 1, 2, 3 };
HashSet<int> set2 = new HashSet<int> { 3, 4, 5 };

// Union (modifies set1)
set1.UnionWith(set2);
// Result: {1, 2, 3, 4, 5}
```

### Intersection

```csharp
HashSet<int> set1 = new HashSet<int> { 1, 2, 3, 4 };
HashSet<int> set2 = new HashSet<int> { 3, 4, 5, 6 };

// Intersection (modifies set1)
set1.IntersectWith(set2);
// Result: {3, 4}
```

### Difference

```csharp
HashSet<int> set1 = new HashSet<int> { 1, 2, 3, 4 };
HashSet<int> set2 = new HashSet<int> { 3, 4, 5, 6 };

// Difference (modifies set1)
set1.ExceptWith(set2);
// Result: {1, 2}
```

### Symmetric Difference

```csharp
HashSet<int> set1 = new HashSet<int> { 1, 2, 3, 4 };
HashSet<int> set2 = new HashSet<int> { 3, 4, 5, 6 };

// Symmetric difference (modifies set1)
set1.SymmetricExceptWith(set2);
// Result: {1, 2, 5, 6}
```

## Subset and Superset

```csharp
HashSet<int> set1 = new HashSet<int> { 1, 2 };
HashSet<int> set2 = new HashSet<int> { 1, 2, 3 };

// Subset
bool isSubset = set1.IsSubsetOf(set2);          // true

// Superset
bool isSuperset = set2.IsSupersetOf(set1);      // true

// Proper subset
bool isProperSubset = set1.IsProperSubsetOf(set2);  // true

// No overlap
bool disjoint = set1.SetEquals(set2);           // false
```

## Iterating HashSet

```csharp
HashSet<int> numbers = new HashSet<int> { 1, 2, 3, 4, 5 };

foreach (int num in numbers) {
    Console.WriteLine(num);
}

// Note: Order is undefined (hash-based)
```

## Common Patterns

### Pattern 1: Remove Duplicates

```csharp
int[] numbers = { 1, 2, 2, 3, 3, 3, 4, 5, 5 };

// Remove duplicates
HashSet<int> unique = new HashSet<int>(numbers);

// Convert back to array if needed
int[] uniqueArray = unique.ToArray();
// Result: {1, 2, 3, 4, 5} (order not guaranteed)
```

### Pattern 2: Fast Lookup

```csharp
string[] allowedWords = { "apple", "banana", "cherry" };
HashSet<string> allowed = new HashSet<string>(allowedWords);

string userInput = "apple";
if (allowed.Contains(userInput)) {
    Console.WriteLine("Valid word");
} else {
    Console.WriteLine("Not in allowed list");
}
```

### Pattern 3: Intersection of Lists

```csharp
List<int> list1 = new List<int> { 1, 2, 3, 4, 5 };
List<int> list2 = new List<int> { 3, 4, 5, 6, 7 };

HashSet<int> set1 = new HashSet<int>(list1);
set1.IntersectWith(list2);

// Result: Elements in both lists: {3, 4, 5}
```

## Performance

```csharp
// Contains is O(1) average
bool has = hashSet.Contains(item);

// Add is O(1) average
hashSet.Add(item);

// Much faster than List.Contains which is O(n)
```

## Best Practices

✓ **Use for duplicate removal**
```csharp
var unique = new HashSet<T>(collection);
```

✓ **Use for fast membership testing**
```csharp
if (set.Contains(item)) { }
```

✓ **Use for set operations**
```csharp
set1.UnionWith(set2);
set1.IntersectWith(set2);
```

## Anti-Patterns

❌ **For ordered collections**
```csharp
// Don't use if order matters
var set = new HashSet<int> { 5, 3, 1, 4, 2 };
// Order is undefined
```

❌ **For duplicate preservation**
```csharp
// Don't use if you need duplicates
var data = new HashSet<int> { 1, 2, 2, 3 };
// Second 2 is lost
```

## Summary

- HashSet stores unique values only
- O(1) average for Contains, Add, Remove
- Perfect for membership testing
- Supports mathematical set operations
- Order is undefined
- Use for removing duplicates efficiently

---

## Next Steps

1. Learn Queue and Stack
2. Study Collection Patterns
3. Review Best Practices
