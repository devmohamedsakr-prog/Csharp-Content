# LINQ Conversion Operations

## Overview
Conversion operations transform LINQ queries into specific collection types or values.

## ToList and ToArray

### Converting to Collections
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Deferred execution query
IEnumerable<int> query = numbers.Where(n => n > 2);

// Materialize to List
List<int> list = query.ToList(); // [3, 4, 5]

// Materialize to Array
int[] array = query.ToArray(); // [3, 4, 5]

// Direct conversion
var strings = new[] { "a", "b", "c" };
List<string> stringList = strings.ToList();
```

### When to Use
```csharp
// Use ToList() when:
// - Need to iterate multiple times
// - Query results change based on collection state
// - Passing to method expecting List<T>
var result = dataContext.Users.Where(u => u.IsActive).ToList();

// Use ToArray() when:
// - Need fixed-size collection
// - Memory efficiency important
// - Passing to method expecting T[]
var items = items.Where(i => i.Price > 100).ToArray();
```

## ToDictionary

### Creating Dictionaries
```csharp
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

var people = new List<Person>
{
    new Person { Id = 1, Name = "Alice", Email = "alice@example.com" },
    new Person { Id = 2, Name = "Bob", Email = "bob@example.com" }
};

// Create dictionary with Id as key
Dictionary<int, Person> byId = people.ToDictionary(p => p.Id);
// Access: byId[1] -> Alice

// Create dictionary with Id as key, Name as value
Dictionary<int, string> nameById = people.ToDictionary(p => p.Id, p => p.Name);
// nameById[1] -> "Alice"

// Create dictionary with Email as key
Dictionary<string, Person> byEmail = people.ToDictionary(p => p.Email);
// byEmail["alice@example.com"] -> Alice Person object
```

### Handling Duplicate Keys
```csharp
// Bad: Throws if duplicate keys
var duplicates = new List<int> { 1, 2, 2, 3, 3, 3 };
var dict = duplicates.ToDictionary(x => x); // ArgumentException: An item with the same key already exists

// Good: Use first/last or handle duplicates
var dict = duplicates.Distinct().ToDictionary(x => x);

// Or use ToLookup instead
var lookup = duplicates.ToLookup(x => x);
// lookup[2] -> [2, 2]
```

## ToLookup

### Creating Multi-Value Dictionaries
```csharp
// ToLookup allows multiple values per key
var lookup = people.ToLookup(p => p.Department);

// Access all people in department
var itStaff = lookup["IT"]; // IEnumerable<Person>

// Safe access - no exception for missing key
var nonExistent = lookup["NonExistent"]; // Empty collection
```

## ToHashSet

### Creating Hash Sets
```csharp
var numbers = new List<int> { 1, 2, 2, 3, 3, 3, 4, 4, 5 };

// Convert to HashSet (removes duplicates)
HashSet<int> unique = numbers.ToHashSet(); // {1, 2, 3, 4, 5}

// With comparer
var strings = new List<string> { "Hello", "hello", "HELLO" };
var caseInsensitive = strings.ToHashSet(StringComparer.OrdinalIgnoreCase);
// {"Hello"} - only one due to case-insensitive comparison
```

## AsEnumerable and AsQueryable

### Provider Conversion
```csharp
// AsEnumerable: LINQ to Objects
List<int> list = new List<int> { 1, 2, 3, 4, 5 };
IEnumerable<int> enumerable = list.AsEnumerable();

// AsQueryable: LINQ to Entities/SQL
var dbContext = new MyDbContext();
IQueryable<User> queryable = dbContext.Users.AsQueryable();

// Practical: Force client-side evaluation
var result = dbContext.Users
    .Where(u => u.IsActive) // Server-side
    .AsEnumerable() // Switch to LINQ to Objects
    .Where(u => ComplexLogic(u)) // Client-side
    .ToList();
```

## Cast

### Type Casting
```csharp
// Cast all elements to type
var objects = new List<object> { 1, 2, 3, 4, 5 };
IEnumerable<int> integers = objects.Cast<int>(); // [1, 2, 3, 4, 5]

// Throws if incompatible types
var mixed = new List<object> { "Hello", 42, 3.14 };
var allInts = mixed.Cast<int>(); // InvalidCastException on "Hello"

// Query syntax with Cast
var result = from int num in numbers select num;
```

## OfType vs Cast

### Key Differences
```csharp
var objects = new List<object> { "Hello", 42, 3.14, "World", 100, true };

// OfType: Skips incompatible types
var strings = objects.OfType<string>(); // ["Hello", "World"]
var ints = objects.OfType<int>(); // [42, 100]

// Cast: Throws on incompatible type
var allInt = objects.Cast<int>(); // InvalidCastException on "Hello"

// When to use:
// Use OfType when: Collection has mixed types, want compatible types
// Use Cast when: Expect uniform types, want error if mismatch
```

## Enumerable.Range and Enumerable.Repeat

### Generating Collections
```csharp
// Range: Generate sequence of integers
IEnumerable<int> range = Enumerable.Range(1, 5); // [1, 2, 3, 4, 5]

// Range starting at 0
var indices = Enumerable.Range(0, 10); // [0, 1, 2, ..., 9]

// Repeat: Generate repeated sequence
IEnumerable<string> repeated = Enumerable.Repeat("Hello", 3);
// ["Hello", "Hello", "Hello"]

// Practical: Generate test data
var numbers = Enumerable.Range(1, 100).ToList();
var dates = Enumerable.Range(0, 30).Select(d => DateTime.Now.AddDays(d));
```

## Complex Conversion Examples

### Chaining Conversions
```csharp
var source = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

var result = source
    .Where(n => n > 3)
    .Select(n => n * 2)
    .OrderByDescending(n => n)
    .Take(4)
    .ToList(); // [18, 16, 14, 12]
```

### Multiple Collection Conversions
```csharp
var people = new List<Person> { /* ... */ };

// Convert to different formats for different purposes
var list = people.ToList();
var dict = people.ToDictionary(p => p.Id);
var array = people.ToArray();
var lookup = people.ToLookup(p => p.Department);
var hashSet = people.Select(p => p.Id).ToHashSet();
```

## Best Practices

1. **Materialize at Appropriate Time**
```csharp
// Bad: Materializing too early
var allUsers = dbContext.Users.ToList();
var activeUsers = allUsers.Where(u => u.IsActive).ToList();

// Good: Filter first, then materialize
var activeUsers = dbContext.Users.Where(u => u.IsActive).ToList();
```

2. **Choose Right Collection Type**
```csharp
// Bad: Always using List
var items = query.ToList();
var lookup = items.Where(i => i.Category == "A"); // Linear search

// Good: Use appropriate type
var dict = items.ToDictionary(i => i.Id); // O(1) lookup
var item = dict[5];

var categoryLookup = items.ToLookup(i => i.Category); // Multiple values per key
var categoryA = categoryLookup["A"];
```

3. **Handle Type Mismatches Safely**
```csharp
// Bad: Using Cast with uncertain types
var converted = objects.Cast<int>();

// Good: Use OfType for mixed collections
var integers = objects.OfType<int>();
```

## Common Mistakes

1. **Duplicate Keys in ToDictionary**
```csharp
// Bad: Throws on duplicates
var dict = people.ToDictionary(p => p.Department);

// Good: Handle duplicates
var dict = people.GroupBy(p => p.Department)
    .ToDictionary(g => g.Key, g => g.FirstOrDefault());
```

2. **Not Materializing Query Results**
```csharp
// Bad: Multiple enumerations of same query
IEnumerable<int> query = source.Where(x => x > 5);
var first = query.First(); // First enumeration
var list = query.ToList(); // Second enumeration

// Good: Materialize once
var list = source.Where(x => x > 5).ToList();
var first = list.First();
```

3. **Using Cast with Incompatible Types**
```csharp
// Bad: Throws if any type incompatible
var numbers = objects.Cast<int>();

// Good: Use OfType for uncertain collections
var numbers = objects.OfType<int>();
```

4. **Performance: Too Early or Too Late Materialization**
```csharp
// Bad: Materialize database query unnecessarily early
var allUsers = dbContext.Users.ToList(); // Loads all
var result = allUsers.Where(u => u.Age > 30).ToList();

// Good: Materialize after filtering
var result = dbContext.Users.Where(u => u.Age > 30).ToList();
```

## Quick Summary
- ToList/ToArray materialize queries
- ToDictionary creates key-value collection
- ToLookup supports multiple values per key
- ToHashSet removes duplicates
- OfType safely filters by type
- Cast converts all or throws
- AsEnumerable switches to LINQ to Objects
- Materialize at appropriate time for performance
- Choose collection type based on usage pattern

## Resources
- Conversion Operations (LINQ)
- Collection Types comparison
- LINQ Performance considerations
