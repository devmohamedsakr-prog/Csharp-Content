# LINQ Performance Tuning

## Overview
Performance optimization for LINQ queries focusing on database queries, in-memory operations, and common pitfalls.

## Database Query Optimization

### Filter on Server First
```csharp
// Bad: Load all users, filter in memory
var results = dbContext.Users.ToList().Where(u => u.IsActive);
// SQL: SELECT * FROM Users (entire table!)

// Good: Filter on server
var results = dbContext.Users.Where(u => u.IsActive).ToList();
// SQL: SELECT * FROM Users WHERE IsActive = 1
```

### Project Early to Reduce Data Transfer
```csharp
// Bad: Transfer entire entity
var users = dbContext.Users
    .ToList()
    .Select(u => new { u.Id, u.Name });
// Transfers: Id, Name, Email, Password, Address, Phone, ... (all columns)

// Good: Project on server
var users = dbContext.Users
    .Select(u => new { u.Id, u.Name })
    .ToList();
// SQL: SELECT Id, Name FROM Users (only needed columns)
```

### Use Include for Related Data
```csharp
// Bad: N+1 problem - one query per user
var users = dbContext.Users.ToList();
foreach (var user in users)
{
    var posts = dbContext.Posts.Where(p => p.UserId == user.Id).ToList(); // N queries!
}

// Good: Single query with Include
var users = dbContext.Users
    .Include(u => u.Posts)
    .ToList(); // 1 query, posts pre-loaded

// With filtering on included data
var users = dbContext.Users
    .Include(u => u.Posts.Where(p => p.IsPublished))
    .ToList();
```

### Use Join Instead of Navigation Properties
```csharp
// Bad: May cause N+1
var result = users.Select(u => new 
{ 
    u.Name, 
    PostCount = u.Posts.Count() // Lazy load for each user!
});

// Good: Single join query
var result = dbContext.Users
    .GroupJoin(dbContext.Posts,
        u => u.Id,
        p => p.UserId,
        (u, posts) => new { u.Name, PostCount = posts.Count() })
    .ToList();
```

### Avoid Client-Side Code in Queries
```csharp
// Bad: Method call can't translate to SQL
var result = dbContext.Users.Where(u => IsValidEmail(u.Email)).ToList();
// Likely throws or loads all data

// Good: Use translatable predicates
var result = dbContext.Users.Where(u => u.Email.Contains("@")).ToList();
// SQL: SELECT * FROM Users WHERE Email LIKE '%@%'
```

## In-Memory Query Optimization

### Materialize Appropriately
```csharp
// Bad: Materializing unnecessarily
var list = items.ToList();
var filtered = list.Where(x => x > 10).ToList();
var doubled = filtered.Select(x => x * 2).ToList();

// Good: Chain before materializing
var result = items
    .Where(x => x > 10)
    .Select(x => x * 2)
    .ToList(); // Single materialization
```

### Avoid Multiple Enumerations
```csharp
// Bad: Enumerates multiple times
var query = items.Where(x => x > 5);
var first = query.First();      // First enumeration
var count = query.Count();      // Second enumeration
var list = query.ToList();      // Third enumeration

// Good: Materialize once
var list = items.Where(x => x > 5).ToList();
var first = list.First();
var count = list.Count;
```

### Use Appropriate Collection Types
```csharp
// Bad: List for large collection with lookup
var items = new List<int> { /* 1 million items */ };
if (items.Contains(target)) // O(n) - linear search!

// Good: HashSet for membership testing
var itemSet = items.ToHashSet();
if (itemSet.Contains(target)) // O(1) - constant time

// Good: Dictionary for key-value lookups
var dict = items.ToDictionary(x => x.Id);
var item = dict[5]; // O(1) instead of First(x => x.Id == 5)
```

## Query Composition Optimization

### Use Intermediary Variables
```csharp
// Bad: Complex nested chain
var result = dbContext.Users
    .Where(u => u.IsActive)
    .Select(u => new { u.Id, u.Name })
    .Where(u => u.Name.Length > 3)
    .OrderBy(u => u.Name)
    .Skip(10)
    .Take(10)
    .ToList();

// Good: Name intermediate steps for clarity
var activeUsers = dbContext.Users.Where(u => u.IsActive);
var projectedUsers = activeUsers.Select(u => new { u.Id, u.Name });
var filteredUsers = projectedUsers.Where(u => u.Name.Length > 3);
var sortedUsers = filteredUsers.OrderBy(u => u.Name);
var pagedUsers = sortedUsers.Skip(10).Take(10).ToList();
```

### Order Query for SQL Translation
```csharp
// Bad: Complex logic after projection (can't translate)
var result = dbContext.Users
    .Select(u => new { u.Id, u.Name })
    .Where(u => /* Complex logic on projected data */)
    .ToList();

// Good: Filter before projection
var result = dbContext.Users
    .Where(u => /* Filter on original entity */)
    .Select(u => new { u.Id, u.Name })
    .ToList();
```

## GroupBy and Join Optimization

### Efficient Grouping
```csharp
// Bad: Multiple passes
var groups = items.GroupBy(x => x.Category);
var result = groups.Select(g => new
{
    Category = g.Key,
    Count = g.Count(),          // Enumeration 1
    Sum = g.Sum(x => x.Value),  // Enumeration 2
    Avg = g.Average(x => x.Value) // Enumeration 3
});

// Good: Single enumeration
var result = items
    .GroupBy(x => x.Category)
    .Select(g => new
    {
        Category = g.Key,
        Count = g.Count(),
        Sum = g.Sum(x => x.Value),
        Avg = g.Average(x => x.Value)
    })
    .ToList();
```

### Efficient Joins
```csharp
// Bad: Multiple lookups
var result = items1.Join(items2,
    i1 => i1.Id,
    i2 => i2.Id,
    (i1, i2) => new { i1, i2 })
.Select(x => Calculate(x.i1, x.i2));

// Good: Index before multiple uses
var lookup = items2.ToLookup(x => x.Id);
var result = items1
    .Select(i1 => new { i1, i2 = lookup[i1.Id].FirstOrDefault() })
    .Where(x => x.i2 != null)
    .Select(x => Calculate(x.i1, x.i2));
```

## Measurement and Profiling

### Analyzing Query Performance
```csharp
var sw = Stopwatch.StartNew();

// Query to test
var result = dbContext.Users
    .Where(u => u.IsActive)
    .Select(u => u.Name)
    .ToList();

sw.Stop();
Console.WriteLine($"Query time: {sw.ElapsedMilliseconds}ms");
Console.WriteLine($"Result count: {result.Count}");

// Check generated SQL
var sql = dbContext.Users
    .Where(u => u.IsActive)
    .AsQueryable()
    .ToQueryString(); // .NET 5+
```

### Entity Framework Logging
```csharp
// Enable EF Core logging
var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>()
    .LogTo(Console.WriteLine)
    .EnableSensitiveDataLogging();

// Shows generated SQL
using var context = new MyDbContext(optionsBuilder.Options);
var users = context.Users.Where(u => u.IsActive).ToList();
```

## Common Pitfalls

### 1. N+1 Query Problem
```csharp
// Bad: N+1 queries
var users = dbContext.Users.ToList();
foreach (var user in users)
{
    Console.WriteLine($"{user.Name}: {user.Posts.Count}"); // Query per user
}

// Good: Single query
var users = dbContext.Users
    .Include(u => u.Posts)
    .Select(u => new { u.Name, PostCount = u.Posts.Count() })
    .ToList();
```

### 2. Materializing Before Grouping
```csharp
// Bad: Group in memory
var groups = dbContext.Users
    .ToList()
    .GroupBy(u => u.Department);

// Good: Group on server
var groups = dbContext.Users
    .GroupBy(u => u.Department)
    .ToList();
```

### 3. ToList() Inside Loop
```csharp
// Bad: Multiple materializations
foreach (var category in categories)
{
    var items = dbContext.Items
        .Where(i => i.CategoryId == category.Id)
        .ToList(); // Query for each category
}

// Good: Single query
var itemsByCategory = dbContext.Items
    .GroupBy(i => i.CategoryId)
    .ToDictionary(g => g.Key, g => g.ToList());

foreach (var category in categories)
{
    var items = itemsByCategory[category.Id];
}
```

### 4. Complex Where Clauses
```csharp
// Bad: Might not translate to SQL
var result = dbContext.Users.Where(u =>
    ComplexLogic(u.Name) && 
    AnotherMethod(u.Age)
).ToList();

// Good: Simple, translatable predicates
var result = dbContext.Users
    .Where(u => u.Age > 18)
    .AsEnumerable() // Switch to LINQ to Objects
    .Where(u => ComplexLogic(u.Name))
    .ToList();
```

## Performance Checklist

- [ ] Filter on server before materializing
- [ ] Project early to reduce data transfer
- [ ] Use Include() to avoid N+1
- [ ] Use appropriate collection types (HashSet, Dictionary)
- [ ] Avoid materializing unnecessarily
- [ ] Don't enumerate multiple times
- [ ] Use AsNoTracking() for read-only queries
- [ ] Index frequently joined columns
- [ ] Monitor generated SQL
- [ ] Measure before and after optimization

## Best Practices Summary

1. **Translate operations to server when possible**
2. **Project only required columns**
3. **Use Include for related data**
4. **Materialize appropriately (not too early, not too late)**
5. **Cache results if used multiple times**
6. **Use AsNoTracking for read-only queries**
7. **Group and aggregate on server**
8. **Monitor generated SQL**
9. **Index your database columns**
10. **Profile before optimizing**

## Quick Summary
- Filter and project on server first
- Use Include to prevent N+1
- Materialize at right time
- Cache for multiple uses
- Monitor generated SQL
- Use AsNoTracking for reads
- Avoid client-side predicates
- Index foreign keys
- Profile real queries
- Optimize based on measurements

## Resources
- Entity Framework Performance
- LINQ Query Optimization
- SQL Query Analysis
- Database Indexing Strategies
