# LINQ with Generics

## Overview
Combining LINQ and generics creates type-safe, powerful query capabilities with compile-time checking.

## Generic Collections with LINQ

### Type-Safe Queries
```csharp
// Generic collection
public class Repository<T> where T : class
{
    private List<T> _items = new();
    
    // LINQ method with generic
    public IEnumerable<T> GetAll()
    {
        return _items.AsEnumerable();
    }
    
    // Query with type safety
    public T GetById(int id) where T : IEntity
    {
        return _items.OfType<IEntity>()
                    .FirstOrDefault(e => e.Id == id) as T;
    }
}

// Usage: full type safety
var userRepo = new Repository<User>();
var allUsers = userRepo.GetAll(); // IEnumerable<User>

var adminUsers = allUsers.Where(u => u.IsAdmin).ToList(); // List<User>
```

## Generic LINQ Operators

### Extension Methods with Constraints
```csharp
// Custom generic LINQ operator
public static class LinqExtensions
{
    // Filter by type and condition
    public static IEnumerable<T> WhereOfType<T>(
        this IEnumerable<object> source,
        Func<T, bool> predicate) where T : class
    {
        return source.OfType<T>().Where(predicate);
    }
    
    // Project with type constraint
    public static IEnumerable<TResult> SelectWhere<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TResult?> selector) where TResult : class
    {
        return source.Select(selector).Where(x => x != null);
    }
    
    // Group by property value
    public static IEnumerable<IGrouping<TKey, TSource>> GroupByProperty<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector) where TKey : notnull
    {
        return source.GroupBy(keySelector);
    }
}

// Usage
var items = new List<object> { new User { Name = "Alice" }, "string", new User { Name = "Bob" } };
var users = items.WhereOfType<User>(u => u.Name.Length > 3);
```

## Filtering with Generic Constraints

### Constraint-Based Queries
```csharp
// Interface constraint
public interface IEntity
{
    int Id { get; }
    DateTime CreatedAt { get; }
}

// Query with constraint
public static class EntityQueries
{
    public static IEnumerable<T> GetRecent<T>(
        this IEnumerable<T> entities,
        TimeSpan period) where T : IEntity
    {
        var cutoff = DateTime.UtcNow - period;
        return entities.Where(e => e.CreatedAt > cutoff);
    }
    
    public static Dictionary<int, T> ToDictionary<T>(
        this IEnumerable<T> entities) where T : IEntity
    {
        return entities.ToDictionary(e => e.Id);
    }
}

// Usage
List<User> users = GetUsers();
var recentUsers = users.GetRecent(TimeSpan.FromDays(7));
var userDict = users.ToDictionary();
```

## Generic LINQ Queries

### Complex Generic Queries
```csharp
// Generic repository with LINQ
public interface IRepository<T> where T : class
{
    IQueryable<T> Query();
    Task<T> GetByIdAsync(int id);
}

public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    
    public IQueryable<T> Query() => _context.Set<T>();
    
    public async Task<T> GetByIdAsync(int id)
    {
        return await Query().FirstOrDefaultAsync(e => (e as IEntity).Id == id);
    }
    
    // Generic filter method
    public async Task<IEnumerable<T>> FindAsync(
        Func<IQueryable<T>, IQueryable<T>> filter)
    {
        return await filter(Query()).ToListAsync();
    }
}

// Usage
var userRepo = new GenericRepository<User>();

// Reusable filters
var adults = await userRepo.FindAsync(q => q.Where(u => u.Age >= 18));
var activeUsers = await userRepo.FindAsync(q => q.Where(u => u.IsActive).OrderBy(u => u.Name));
```

## Generic Join and Select

### Type-Safe Joins
```csharp
public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
}

public class OrderService
{
    public IEnumerable<OrderSummary> GetOrderSummaries(
        IEnumerable<User> users,
        IEnumerable<Order> orders)
    {
        // Generic join with strong typing
        return users.Join(
            orders,
            u => u.Id,
            o => o.UserId,
            (u, o) => new OrderSummary
            {
                UserName = u.Name,
                OrderId = o.Id,
                OrderDate = o.OrderDate
            }
        );
    }
    
    // With SelectMany (cross product)
    public IEnumerable<string> GetAllUserOrderDescriptions(
        IEnumerable<User> users,
        Dictionary<int, List<Order>> ordersByUser)
    {
        return users.SelectMany(
            u => ordersByUser[u.Id],
            (u, o) => $"{u.Name} placed order {o.Id}"
        );
    }
}
```

## Generic Grouping and Aggregation

### Type-Safe Aggregations
```csharp
public static class AggregationExtensions
{
    // Generic grouping aggregation
    public static Dictionary<TKey, TAggregate> Aggregate<TSource, TKey, TAggregate>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> groupKey,
        Func<IGrouping<TKey, TSource>, TAggregate> aggregator) where TKey : notnull
    {
        return source.GroupBy(groupKey)
                    .ToDictionary(g => g.Key, aggregator);
    }
    
    // Generic statistics
    public static (T Min, T Max, double Average) GetStats<T>(
        this IEnumerable<T> values) where T : struct, IComparable<T>
    {
        var min = values.Min();
        var max = values.Max();
        var avg = values.Cast<double>().Average();
        
        return (min, max, avg);
    }
}

// Usage
var users = GetUsers();

var statsByDept = users.Aggregate(
    u => u.Department,
    g => new
    {
        Count = g.Count(),
        AvgAge = g.Average(u => u.Age),
        Names = g.Select(u => u.Name).ToList()
    }
);

var salaries = new List<decimal> { 50000, 60000, 55000, 70000 };
var (min, max, avg) = salaries.GetStats();
```

## Generic Sorting and Comparison

### Dynamic Sorting
```csharp
public static class SortingExtensions
{
    // Generic sort with expression
    public static IOrderedEnumerable<T> OrderByProperty<T>(
        this IEnumerable<T> source,
        string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var lambda = Expression.Lambda<Func<T, object>>(property, parameter);
        
        return source.OrderBy(lambda.Compile());
    }
    
    // Generic multi-property sort
    public static IEnumerable<T> SortBy<T>(
        this IEnumerable<T> source,
        params (string Property, bool Descending)[] sorts) where T : class
    {
        var query = (IOrderedEnumerable<T>)source;
        
        foreach (var (prop, desc) in sorts)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, prop);
            var lambda = Expression.Lambda<Func<T, object>>(property, parameter);
            var compiled = lambda.Compile();
            
            query = desc 
                ? query.ThenByDescending(compiled)
                : query.ThenBy(compiled);
        }
        
        return query;
    }
}

// Usage
var users = GetUsers();
var sorted = users.OrderByProperty(nameof(User.Name));
var multiSort = users.SortBy(
    (nameof(User.Department), false),
    (nameof(User.Name), false)
);
```

## Best Practices

1. **Use Generic Constraints for Safety**
```csharp
// Good: Constraints ensure type safety
public IEnumerable<T> GetFiltered<T>(
    IEnumerable<T> items,
    Func<T, bool> filter) where T : class
{
    return items.Where(filter);
}

// Bad: No constraints, less type information
public IEnumerable<object> GetFiltered(
    IEnumerable<object> items,
    Func<object, bool> filter)
{
    return items.Where(filter);
}
```

2. **Leverage IQueryable for Databases**
```csharp
// Good: Deferred to database
public IQueryable<User> GetActiveUsers()
{
    return dbContext.Users.Where(u => u.IsActive);
}

// Less efficient: Materialized too early
public IEnumerable<User> GetActiveUsersWrong()
{
    return dbContext.Users.ToList().Where(u => u.IsActive);
}
```

3. **Use Extension Methods for Reusable Queries**
```csharp
// Good: Reusable across repositories
public static IQueryable<T> Active<T>(this IQueryable<T> query)
    where T : class, IActivatable
{
    return query.Where(x => x.IsActive);
}

public static IQueryable<T> Recent<T>(this IQueryable<T> query, int days)
    where T : class, IAuditable
{
    var cutoff = DateTime.UtcNow.AddDays(-days);
    return query.Where(x => x.CreatedAt > cutoff);
}

// Usage
var recentActiveUsers = dbContext.Users.Recent(30).Active();
```

## Common Mistakes

1. **Not Using Generic Constraints**
```csharp
// Bad: Casting needed
public object GetFirst(IEnumerable<object> items)
{
    return items.FirstOrDefault();
}

// Good: Generic with constraint
public T GetFirst<T>(IEnumerable<T> items) where T : class
{
    return items.FirstOrDefault();
}
```

2. **Materializing Too Early with Generics**
```csharp
// Bad: Loads all to memory
public IEnumerable<T> Filter<T>(IEnumerable<T> items)
{
    return items.ToList().Where(x => /* filter */);
}

// Good: Filter first
public IEnumerable<T> Filter<T>(IEnumerable<T> items)
{
    return items.Where(x => /* filter */);
}
```

3. **Not Using IQueryable with Databases**
```csharp
// Bad: Filters in memory
public List<User> GetFiltered()
{
    return dbContext.Users.ToList().Where(u => u.IsActive).ToList();
}

// Good: Filters on server
public IQueryable<User> GetFiltered()
{
    return dbContext.Users.Where(u => u.IsActive);
}
```

## Quick Summary
- Generics + LINQ = type-safe queries
- Constraints enable compile-time checking
- IQueryable for deferred database queries
- IEnumerable for in-memory LINQ to Objects
- Extension methods for reusable query logic
- Expression trees enable dynamic queries
- Join/SelectMany for combining generic collections
- Filter/project/aggregate all with type safety
- Avoid boxing with proper generic constraints
- Use IQueryable to filter on server first

## Resources
- Generic Collections
- IQueryable vs IEnumerable
- LINQ Expression Trees
- Entity Framework Core
