# LINQ Basics

## Overview
LINQ (Language Integrated Query) is a powerful feature that provides a unified, consistent way to query different data sources using C#'s built-in language constructs.

## Core Concepts

### What is LINQ?
- Provides query syntax integrated into C#
- Works with IEnumerable, IQueryable, and other data sources
- Supports both method syntax and query syntax

### LINQ Providers
```csharp
// LINQ to Objects (in-memory collections)
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var evens = numbers.Where(n => n % 2 == 0).ToList();

// LINQ to Entities (databases)
var users = dbContext.Users.Where(u => u.IsActive).ToList();

// LINQ to XML
var xml = XDocument.Load("file.xml");
var elements = xml.Descendants("item").ToList();
```

## Query Syntax vs Method Syntax

### Query Syntax (SQL-like)
```csharp
var result = from number in numbers
             where number > 5
             select number * 2;
```

### Method Syntax (Fluent)
```csharp
var result = numbers.Where(n => n > 5).Select(n => n * 2);
```

## Deferred vs Immediate Execution

### Deferred Execution
```csharp
IEnumerable<int> query = numbers.Where(n => n > 5);
// Query is NOT executed yet - only when enumerated
foreach (var item in query)
{
    Console.WriteLine(item); // Execution happens here
}
```

### Immediate Execution
```csharp
// .ToList(), .ToArray(), .First(), .Count() force execution
var list = numbers.Where(n => n > 5).ToList();
var first = numbers.Where(n => n > 5).First();
var count = numbers.Count(n => n > 5);
```

## Common Operators

### Filtering
```csharp
// Where - filters based on condition
var adults = people.Where(p => p.Age >= 18);

// OfType - filters by type
var strings = objects.OfType<string>();
```

### Projection
```csharp
// Select - transforms each element
var names = people.Select(p => p.Name);

// SelectMany - flattens nested collections
var allItems = orders.SelectMany(o => o.Items);
```

### Ordering
```csharp
// OrderBy, ThenBy
var sorted = people.OrderBy(p => p.LastName)
                   .ThenBy(p => p.FirstName);

// OrderByDescending, ThenByDescending
var descending = people.OrderByDescending(p => p.Age);
```

### Grouping
```csharp
// GroupBy - groups elements
var byDept = employees.GroupBy(e => e.Department);

foreach (var group in byDept)
{
    Console.WriteLine($"Department: {group.Key}");
    foreach (var emp in group)
        Console.WriteLine($"  {emp.Name}");
}
```

## Best Practices

1. **Use Method Syntax for Simple Queries**: More concise and easier to read
2. **Use Query Syntax for Complex Joins**: More readable for complex operations
3. **Apply Filtering Early**: Filter first, project later for better performance
4. **Avoid Deferred Execution Pitfalls**: Be aware of when queries execute
5. **Use .ToList() Strategically**: Don't materialize unnecessarily
6. **Avoid Multiple Enumerations**: Cache results if enumerating multiple times

```csharp
// Bad: Multiple enumerations
var query = numbers.Where(n => n > 5);
var first = query.FirstOrDefault();
var count = query.Count();  // Enumerates again!

// Good: Cache results
var result = numbers.Where(n => n > 5).ToList();
var first = result.FirstOrDefault();
var count = result.Count();
```

## Common Mistakes

1. **Forgetting ToList() and Getting Stale Data**
```csharp
// Bad: numbers might change before use
var query = numbers.Where(n => n > 5);
numbers.Add(100);
var result = query.ToList(); // Includes 100!

// Good: Materialize immediately
var result = numbers.Where(n => n > 5).ToList();
```

2. **Complex Queries Remaining in Database**
```csharp
// Bad: Loading all data to filter in memory
var results = dbContext.Users.ToList().Where(u => ComplicatedLogic(u));

// Good: Filter before materialization when possible
var results = dbContext.Users.Where(u => u.IsActive).ToList();
```

3. **N+1 Query Problem**
```csharp
// Bad: One query per user
var users = dbContext.Users.ToList();
foreach (var user in users)
{
    var posts = dbContext.Posts.Where(p => p.UserId == user.Id).ToList();
}

// Good: Single query with include
var users = dbContext.Users.Include(u => u.Posts).ToList();
```

## Quick Summary
- LINQ provides query capabilities for various data sources
- Query syntax resembles SQL; method syntax is more flexible
- Be aware of deferred vs immediate execution
- Filter early, project later for performance
- Use ToList() strategically to avoid repeated enumerations

## Resources
- Microsoft Docs: LINQ (Language Integrated Query)
- LINQ Query Syntax vs Method Syntax comparison
- IEnumerable vs IQueryable understanding
