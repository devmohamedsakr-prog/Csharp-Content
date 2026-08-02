# LINQ to Entities (Database Queries)

## Overview
LINQ to Entities enables querying databases using Entity Framework, translating LINQ queries into SQL.

## DbContext and IQueryable

### Basic Setup
```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Post> Posts { get; set; }
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

// Usage
var context = new AppDbContext();

// Returns IQueryable<User> - not executed yet
IQueryable<User> query = context.Users.Where(u => u.Id == 1);

// Executed when materialized
var user = query.FirstOrDefault(); // SQL sent to database
```

## Query Execution

### Deferred Execution
```csharp
// Query not executed yet - still IQueryable
var query = context.Users.Where(u => u.IsActive);

// Add another filter
query = query.Where(u => u.Age > 18);

// Executed here - when you enumerate or materialize
var users = query.ToList(); // SQL translated and sent

// Check generated SQL (for debugging)
string sql = query.ToQueryString(); // Shows translated SQL
```

### Immediate Execution
```csharp
// These execute immediately:
var count = context.Users.Count(); // SELECT COUNT(*)
var first = context.Users.First(); // SELECT TOP 1
var exists = context.Users.Any(u => u.IsActive); // SELECT 1 WHERE EXISTS
var list = context.Users.ToList(); // SELECT * FROM Users
```

## Common Query Patterns

### Basic Queries
```csharp
// Get all users
var allUsers = context.Users.ToList();

// Get with filter
var activeUsers = context.Users.Where(u => u.IsActive).ToList();

// Get with ordering
var sorted = context.Users.OrderBy(u => u.Name).ToList();

// Get with pagination
var page1 = context.Users.OrderBy(u => u.Id)
    .Skip(0)
    .Take(10)
    .ToList();

// Select specific properties
var names = context.Users.Select(u => u.Name).ToList();
```

### Include (Eager Loading)
```csharp
// Load user with all posts
var user = context.Users
    .Include(u => u.Posts) // Load navigation property
    .FirstOrDefault(u => u.Id == 1);

// Access Posts without additional query
var posts = user.Posts; // Already loaded

// Multiple includes
var users = context.Users
    .Include(u => u.Posts)
    .ThenInclude(p => p.Comments) // Nested include
    .ToList();

// Conditional include
var users = context.Users
    .Include(u => u.Posts.Where(p => p.IsPublished)) // Filter included data
    .ToList();
```

### Join Operations
```csharp
// Inner join
var joined = context.Users
    .Join(context.Posts,
        u => u.Id,
        p => p.UserId,
        (u, p) => new { u.Name, p.Title })
    .ToList();

// Using Include (preferred for navigation properties)
var withPosts = context.Users
    .Include(u => u.Posts)
    .ToList();

// Query syntax join
var result = (from user in context.Users
              join post in context.Posts on user.Id equals post.UserId
              select new { user.Name, post.Title })
    .ToList();
```

### Grouping
```csharp
// Group in database
var grouped = context.Posts
    .GroupBy(p => p.UserId)
    .Select(g => new
    {
        UserId = g.Key,
        PostCount = g.Count(),
        Titles = g.Select(p => p.Title).ToList()
    })
    .ToList();

// With filtered aggregates
var stats = context.Posts
    .GroupBy(p => p.UserId)
    .Select(g => new
    {
        UserId = g.Key,
        TotalPosts = g.Count(),
        PublishedCount = g.Count(p => p.IsPublished)
    })
    .ToList();
```

## Performance Considerations

### N+1 Query Problem
```csharp
// Bad: Causes N+1 queries (1 for users + 1 per user)
var users = context.Users.ToList();
foreach (var user in users)
{
    var posts = context.Posts.Where(p => p.UserId == user.Id).ToList(); // Query per user!
    Console.WriteLine($"{user.Name}: {posts.Count} posts");
}

// Good: Single query with Include
var users = context.Users.Include(u => u.Posts).ToList();
foreach (var user in users)
{
    Console.WriteLine($"{user.Name}: {user.Posts.Count} posts"); // Already loaded
}
```

### Query Filtering Before Materialization
```csharp
// Bad: Loads all to memory first, then filters
var result = context.Users.ToList().Where(u => u.Age > 18).ToList();

// Good: Filters in database
var result = context.Users.Where(u => u.Age > 18).ToList();
```

### Projection Over Full Objects
```csharp
// Bad: Loads entire objects, transfers all columns
var users = context.Users.ToList(); // Get all columns
var names = users.Select(u => u.Name).ToList();

// Good: Project in database, transfers only needed data
var names = context.Users.Select(u => u.Name).ToList();

// Transfer less data
var summary = context.Users.Select(u => new
{
    u.Id,
    u.Name,
    u.Email
}).ToList();
```

### Use SelectMany Carefully
```csharp
// Bad: Multiple queries (SelectMany + Include issue)
var allPosts = context.Users
    .SelectMany(u => u.Posts) // Can trigger lazy loading
    .ToList();

// Good: Use Include and SelectMany in projection
var allPosts = context.Users
    .Include(u => u.Posts)
    .SelectMany(u => u.Posts)
    .ToList();

// Or direct query
var allPosts = context.Posts.ToList();
```

## Advanced Patterns

### Batch Operations
```csharp
// Update batch
context.Users
    .Where(u => u.IsInactive)
    .ExecuteUpdate(s => s.SetProperty(u => u.IsActive, false));

// Delete batch
context.Posts
    .Where(p => p.CreatedDate < DateTime.Now.AddYears(-5))
    .ExecuteDelete();
```

### Raw SQL Queries
```csharp
// When LINQ can't express query
var users = context.Users
    .FromSql($"SELECT * FROM Users WHERE Age > {minAge}")
    .ToList();

// With parameters (SQL injection safe)
var name = "John%";
var results = context.Users
    .FromSql($"SELECT * FROM Users WHERE Name LIKE {name}")
    .ToList();
```

### Change Tracking Control
```csharp
// Disable tracking for read-only queries
var users = context.Users.AsNoTracking().ToList();

// No change tracking = better performance for reads

// Re-enable for specific query
var editUsers = context.Users
    .AsNoTracking()
    .Where(u => u.IsAdmin)
    .AsTracking()
    .ToList();
```

## Best Practices

1. **Always Use Include for Navigation Properties**
```csharp
// Bad: Lazy loading causes extra queries
var user = context.Users.FirstOrDefault(u => u.Id == 1);
var postCount = user.Posts.Count; // Additional query!

// Good: Include upfront
var user = context.Users
    .Include(u => u.Posts)
    .FirstOrDefault(u => u.Id == 1);
var postCount = user.Posts.Count; // Already loaded
```

2. **Use AsNoTracking for Read-Only Queries**
```csharp
// Good: Reports don't need tracking
var reportData = context.Users
    .AsNoTracking()
    .Select(u => new { u.Name, u.Email })
    .ToList();
```

3. **Project Early, Materialize Late**
```csharp
// Good: Filter and project before ToList()
var result = context.Users
    .Where(u => u.IsActive)
    .Select(u => new { u.Name, u.Email })
    .ToList();
```

## Common Mistakes

1. **Forgetting to Materialize**
```csharp
// Bad: Returns IQueryable, not executed
IQueryable<User> users = context.Users.Where(u => u.IsActive);
// Query never runs!

// Good: Materialize explicitly
List<User> users = context.Users.Where(u => u.IsActive).ToList();
```

2. **Using Client-Side Code in WHERE**
```csharp
// Bad: Method call can't be translated to SQL
var threshold = GetThreshold(); // Client method
var result = context.Users.Where(u => MyCheck(u.Name, threshold)).ToList();
// This causes exception or loads all data

// Good: Call methods before query
var threshold = GetThreshold();
var result = context.Users.Where(u => u.Score > threshold).ToList();
```

3. **Not Handling No-Track Queries Correctly**
```csharp
// Bad: Trying to save no-track entities
var users = context.Users.AsNoTracking().ToList();
users[0].Name = "New Name";
context.SaveChanges(); // Changes not saved!

// Good: Only use AsNoTracking for reads
var readOnlyUsers = context.Users.AsNoTracking().ToList();

// Track modified entities
var editUser = context.Users.FirstOrDefault(u => u.Id == 1);
editUser.Name = "New Name";
context.SaveChanges(); // Works!
```

## Quick Summary
- LINQ to Entities translates to SQL
- Include prevents N+1 query problem
- Materialize at end of query chain
- Project early to reduce data transfer
- Use AsNoTracking for read-only queries
- Avoid client-side code in WHERE
- Filter before materialization
- Watch for deferred execution gotchas

## Resources
- Entity Framework Core LINQ
- Query Performance documentation
- SQL Translation in EF Core
