# LINQ Introduction

## Overview
LINQ (Language Integrated Query) is unified query syntax for diverse data sources including collections, databases, and XML.

## Core Concepts

### What is LINQ?
```csharp
// Traditional approach: loops and conditions
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

var evens = new List<int>();
foreach (var n in numbers)
{
    if (n % 2 == 0)
        evens.Add(n);
}
// Result: [2, 4, 6, 8, 10]

// LINQ approach: declarative
var linqEvens = numbers.Where(n => n % 2 == 0).ToList();
// Same result, clearer intent
```

## Query Syntax vs Method Syntax

### Query Syntax (SQL-like)
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Query syntax
var result = from n in numbers
             where n % 2 == 0
             select n * 2;

// More complex
var people = new List<Person> { /* ... */ };

var query = from person in people
            where person.Age > 25
            orderby person.Name
            select new { person.Name, person.Age };
```

### Method Syntax (Fluent)
```csharp
// Method syntax - more flexible
var result = numbers
    .Where(n => n % 2 == 0)
    .Select(n => n * 2)
    .ToList();

// Same complex query
var query = people
    .Where(p => p.Age > 25)
    .OrderBy(p => p.Name)
    .Select(p => new { p.Name, p.Age })
    .ToList();
```

## IEnumerable and IQueryable

### Collections (IEnumerable)
```csharp
// In-memory: LINQ to Objects
IEnumerable<int> enumerable = new List<int> { 1, 2, 3, 4, 5 };

var result = enumerable
    .Where(n => n > 2)
    .Select(n => n * 2)
    .ToList();

// Executed in-memory immediately upon .ToList()
```

### Database Queries (IQueryable)
```csharp
// LINQ to Entities: translates to SQL
IQueryable<User> queryable = dbContext.Users;

var result = queryable
    .Where(u => u.IsActive)
    .Select(u => new { u.Id, u.Name })
    .ToList(); // Translated to SQL, executed on server

// SQL: SELECT Id, Name FROM Users WHERE IsActive = 1
```

## Deferred vs Immediate Execution

### Deferred Execution
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Query not executed yet
IEnumerable<int> query = numbers
    .Where(n =>
    {
        Console.WriteLine($"Checking {n}"); // Not printed yet
        return n > 2;
    });

// Query executed here (when enumerated)
var result = query.ToList();
// Output: Checking 3, Checking 4, Checking 5

// Can be enumerated multiple times
foreach (var item in query)
{
    Console.WriteLine(item); // Executed again!
}
```

### Immediate Execution
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Immediate execution methods
int count = numbers.Count(); // Executes immediately
int firstOrDefault = numbers.FirstOrDefault(); // Executes
var list = numbers.ToList(); // Executes

// Query executed, result cached
var cachedResult = numbers.Where(n => n > 2).ToList();

// Further operations on cached result
var doubled = cachedResult.Select(n => n * 2); // No re-execution
```

## Basic Operators

### Where - Filtering
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Single condition
var evens = numbers.Where(n => n % 2 == 0);

// Multiple conditions
var filtered = numbers.Where(n => n > 3 && n < 8);

// Complex objects
var people = new List<Person>
{
    new Person { Name = "Alice", Age = 30 },
    new Person { Name = "Bob", Age = 25 },
    new Person { Name = "Charlie", Age = 35 }
};

var adults = people.Where(p => p.Age >= 30);
```

### Select - Projection
```csharp
// Transform elements
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var doubled = numbers.Select(n => n * 2); // [2, 4, 6, 8, 10]

// Extract properties
var names = people.Select(p => p.Name); // ["Alice", "Bob", "Charlie"]

// Create new objects
var summary = people.Select(p => new
{
    p.Name,
    IsAdult = p.Age >= 18
});
```

### OrderBy - Sorting
```csharp
// Ascending order
var sorted = people.OrderBy(p => p.Age);

// Descending order
var descending = people.OrderByDescending(p => p.Age);

// Multiple criteria
var multiSort = people
    .OrderBy(p => p.Age)
    .ThenBy(p => p.Name);
```

## Common Patterns

### Filtering and Projecting
```csharp
var adults = people
    .Where(p => p.Age >= 18)
    .Select(p => p.Name)
    .ToList();
```

### Grouping and Aggregating
```csharp
// Group by department, count employees
var departments = people
    .GroupBy(p => p.Department)
    .Select(g => new
    {
        Department = g.Key,
        Count = g.Count(),
        AvgAge = g.Average(p => p.Age)
    })
    .ToList();
```

### Joining Collections
```csharp
var authors = new List<Author> { /* ... */ };
var books = new List<Book> { /* ... */ };

// Join
var authorBooks = authors
    .Join(books,
        author => author.Id,
        book => book.AuthorId,
        (author, book) => new { author.Name, book.Title })
    .ToList();
```

## Performance Considerations

### Materialization Point
```csharp
// Query not executed
var query = dbContext.Users
    .Where(u => u.IsActive);

// Execute on server, transfer results
var results = query.ToList(); // Materialization

// Wrong: materializing too early
var allUsers = dbContext.Users.ToList(); // Load all
var filtered = allUsers.Where(u => u.IsActive); // Filter in memory
```

### Lazy vs Eager Loading
```csharp
// Lazy: only load when accessed
var query = people.Where(p => p.Age > 25);

// Eager: load immediately
var list = people.Where(p => p.Age > 25).ToList();

// Include related data
var usersWithPosts = dbContext.Users
    .Include(u => u.Posts) // Load related Posts
    .Where(u => u.IsActive)
    .ToList();
```

## Best Practices

1. **Choose Query Style Consistently**
```csharp
// Query syntax for complex joins
var result = from author in authors
             join book in books on author.Id equals book.AuthorId
             select new { author.Name, book.Title };

// Method syntax for simple queries
var filtered = items.Where(x => x.IsActive).ToList();
```

2. **Materialize Appropriately**
```csharp
// Good: Filter on server before materializing
var results = dbContext.Users
    .Where(u => u.IsActive)
    .ToList();

// Bad: Load all, filter in memory
var results = dbContext.Users.ToList().Where(u => u.IsActive);
```

3. **Avoid Multiple Enumerations**
```csharp
// Bad: Enumerates twice
var query = collection.Where(x => x > 5);
var count = query.Count();
var list = query.ToList();

// Good: Materialize once
var list = collection.Where(x => x > 5).ToList();
var count = list.Count;
```

## Common Mistakes

1. **Multiple Enumerations**
```csharp
// Bad
var query = numbers.Where(n => n > 5);
var first = query.First();
var count = query.Count(); // Enumerates again!

// Good
var list = numbers.Where(n => n > 5).ToList();
var first = list.First();
var count = list.Count;
```

2. **Filtering in Wrong Place**
```csharp
// Bad: Loads all users to memory
var allUsers = dbContext.Users.ToList();
var active = allUsers.Where(u => u.IsActive);

// Good: Filter on server
var active = dbContext.Users.Where(u => u.IsActive).ToList();
```

3. **Complex Objects Without Projection**
```csharp
// Bad: Transferring entire objects
var users = dbContext.Users.ToList(); // All columns, all records

// Good: Project only needed data
var userSummary = dbContext.Users
    .Select(u => new { u.Id, u.Name })
    .ToList();
```

## Quick Summary
- LINQ provides unified query syntax
- Query syntax: SQL-like, more readable for complex scenarios
- Method syntax: Flexible, chainable, more common
- IEnumerable: In-memory collections (LINQ to Objects)
- IQueryable: Database queries (LINQ to Entities)
- Deferred execution: Query runs when enumerated
- Materialize appropriately with .ToList(), .ToArray(), etc.
- Filter on server before transferring data
- Avoid multiple enumerations of same query
- Project early to reduce data transfer

## Resources
- LINQ documentation
- Query Syntax vs Method Syntax
- IEnumerable vs IQueryable
- Standard Query Operators (MSDN)
