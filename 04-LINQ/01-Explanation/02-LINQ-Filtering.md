# LINQ Filtering Operations

## Overview
Filtering is one of the most common LINQ operations, allowing you to select only elements that meet specific criteria.

## Where Operator

### Basic Usage
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Single condition
var evens = numbers.Where(n => n % 2 == 0);

// Complex condition
var filtered = numbers.Where(n => n > 3 && n < 8);

// Query syntax
var result = from n in numbers
             where n > 5
             select n;
```

### With Objects
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Department { get; set; }
}

var people = new List<Person>
{
    new Person { Name = "Alice", Age = 30, Department = "HR" },
    new Person { Name = "Bob", Age = 25, Department = "IT" },
    new Person { Name = "Charlie", Age = 35, Department = "IT" }
};

// Filter people in IT department
var itStaff = people.Where(p => p.Department == "IT");

// Multiple conditions
var juniorIT = people.Where(p => p.Department == "IT" && p.Age < 30);

// Complex logic
var filtered = people.Where(p => 
    p.Department == "IT" && 
    p.Age >= 25 && 
    p.Name.StartsWith("B"));
```

## OfType Operator

### Type Filtering
```csharp
var objects = new List<object>
{
    "Hello",
    42,
    3.14,
    "World",
    100
};

// Filter only strings
var strings = objects.OfType<string>(); // ["Hello", "World"]

// Filter only integers
var integers = objects.OfType<int>(); // [42, 100]
```

## Distinct Operator

### Removing Duplicates
```csharp
var numbers = new List<int> { 1, 2, 2, 3, 3, 3, 4, 5, 5 };
var distinct = numbers.Distinct(); // [1, 2, 3, 4, 5]

// With custom objects
var people = new List<string> { "Alice", "Bob", "Alice", "Charlie", "Bob" };
var uniquePeople = people.Distinct();

// By property value
var uniqueByDept = people.DistinctBy(p => p.Department);
```

## Except and Intersect

### Set Operations
```csharp
var list1 = new List<int> { 1, 2, 3, 4, 5 };
var list2 = new List<int> { 3, 4, 5, 6, 7 };

// Elements in list1 but not in list2
var except = list1.Except(list2); // [1, 2]

// Elements in both lists
var intersect = list1.Intersect(list2); // [3, 4, 5]

// Elements in either list
var union = list1.Union(list2); // [1, 2, 3, 4, 5, 6, 7]
```

## Skip and Take

### Pagination
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Skip first 3 elements
var after3 = numbers.Skip(3); // [4, 5, 6, 7, 8, 9, 10]

// Take only 5 elements
var first5 = numbers.Take(5); // [1, 2, 3, 4, 5]

// Pagination: page 2 with 3 items per page
var page = 2;
var pageSize = 3;
var pageResult = numbers.Skip((page - 1) * pageSize).Take(pageSize); // [4, 5, 6]
```

## SkipWhile and TakeWhile

### Conditional Skip/Take
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Skip while condition is true
var skipSmall = numbers.SkipWhile(n => n < 5); // [5, 6, 7, 8, 9, 10]

// Take while condition is true
var takeSmall = numbers.TakeWhile(n => n < 5); // [1, 2, 3, 4]
```

## All and Any

### Existence Checks
```csharp
var numbers = new List<int> { 2, 4, 6, 8, 10 };

// Check if all meet condition
bool allEven = numbers.All(n => n % 2 == 0); // true

// Check if any meet condition
bool hasOdd = numbers.Any(n => n % 2 != 0); // false

// Without condition - any elements exist?
var empty = new List<int>();
bool hasAny = empty.Any(); // false
```

## Contains

### Membership Testing
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
bool contains3 = numbers.Contains(3); // true

var people = new List<string> { "Alice", "Bob", "Charlie" };
bool hasAlice = people.Contains("Alice"); // true
```

## Best Practices

1. **Chain Filters Efficiently**: Order filters from most to least selective
```csharp
// Bad: Less selective first
var result = people.Where(p => p.Age > 20).Where(p => p.Department == "IT");

// Good: More selective first
var result = people.Where(p => p.Department == "IT").Where(p => p.Age > 20);
```

2. **Filter Early for Database Queries**: Keep query on server
```csharp
// Bad: Filters in memory
var results = dbContext.Users.ToList().Where(u => u.IsActive);

// Good: Filter on server
var results = dbContext.Users.Where(u => u.IsActive).ToList();
```

3. **Use Appropriate Operators**: Choose the right tool for the job
```csharp
// Bad: Using Where for existence check
var exists = items.Where(i => i.Id == 5).Any();

// Good: Use FirstOrDefault or Contains
var exists = items.Any(i => i.Id == 5);
```

## Common Mistakes

1. **Forgetting Deferred Execution**
```csharp
// Bad: Collection changes after query definition
var query = numbers.Where(n => n > 5);
numbers.Add(100);
var result = query.ToList(); // Includes 100!

// Good: Materialize immediately
var result = numbers.Where(n => n > 5).ToList();
```

2. **Null Reference in Where**
```csharp
// Bad: Can throw NullReferenceException
var filtered = people.Where(p => p.Name.StartsWith("A"));

// Good: Check for null
var filtered = people.Where(p => p.Name != null && p.Name.StartsWith("A"));
```

3. **Performance Issue with Complex Predicates**
```csharp
// Bad: Calling method multiple times
var filtered = people.Where(p => ExpensiveCheck(p));

// Good: Cache results if needed multiple times
var filtered = people.Where(p => ExpensiveCheck(p)).ToList();
```

## Quick Summary
- Where is the primary filtering operator
- OfType filters by type
- Distinct removes duplicates
- Skip/Take enable pagination
- All/Any check collection conditions
- Filter close to data source for performance
- Be aware of deferred execution implications

## Resources
- Standard LINQ Query Operators
- Filtering Data (LINQ)
- IEnumerable and IQueryable comparison
