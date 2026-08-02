# LINQ Projection Operations

## Overview
Projection transforms elements from a source collection into a different form using Select and SelectMany operators.

## Select Operator

### Basic Transformation
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Transform to strings
var strings = numbers.Select(n => n.ToString()); // ["1", "2", "3", "4", "5"]

// Double each number
var doubled = numbers.Select(n => n * 2); // [2, 4, 6, 8, 10]

// Square each number
var squared = numbers.Select(n => n * n); // [1, 4, 9, 16, 25]
```

### Selecting Properties
```csharp
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

var people = new List<Person>
{
    new Person { Id = 1, Name = "Alice", Age = 30, Email = "alice@example.com" },
    new Person { Id = 2, Name = "Bob", Age = 25, Email = "bob@example.com" }
};

// Project to strings
var names = people.Select(p => p.Name); // ["Alice", "Bob"]

// Project to specific properties
var ids = people.Select(p => p.Id); // [1, 2]
```

### Selecting Anonymous Types
```csharp
// Create anonymous type with selected properties
var personInfo = people.Select(p => new
{
    p.Name,
    p.Age,
    Email = p.Email.ToLower()
});

// Result: [{ Name = "Alice", Age = 30, Email = "alice@example.com" }, ...]
```

### Select with Index
```csharp
var names = new List<string> { "Alice", "Bob", "Charlie" };

// Project with index
var withIndex = names.Select((name, index) => $"{index + 1}. {name}");
// ["1. Alice", "2. Bob", "3. Charlie"]
```

## SelectMany Operator

### Flattening Nested Collections
```csharp
public class Order
{
    public int OrderId { get; set; }
    public List<string> Items { get; set; }
}

var orders = new List<Order>
{
    new Order { OrderId = 1, Items = new List<string> { "Item1", "Item2" } },
    new Order { OrderId = 2, Items = new List<string> { "Item3", "Item4", "Item5" } }
};

// SelectMany flattens to single collection
var allItems = orders.SelectMany(o => o.Items);
// ["Item1", "Item2", "Item3", "Item4", "Item5"]
```

### SelectMany with Complex Types
```csharp
public class Order
{
    public int OrderId { get; set; }
    public List<OrderLine> Lines { get; set; }
}

public class OrderLine
{
    public string Product { get; set; }
    public int Quantity { get; set; }
}

var orders = new List<Order> { /* ... */ };

// Get all products from all orders
var products = orders.SelectMany(o => o.Lines.Select(l => l.Product));

// Get all quantities
var quantities = orders.SelectMany(o => o.Lines.Select(l => l.Quantity));
```

### SelectMany with Index
```csharp
var result = orders.SelectMany(
    (order, orderIndex) => order.Items.Select(
        (item, itemIndex) => new
        {
            OrderIndex = orderIndex,
            ItemIndex = itemIndex,
            Item = item
        }
    )
);
```

## Cast Operator

### Type Conversion
```csharp
var objects = new List<object> { 1, 2, 3, 4, 5 };

// Cast to int
var integers = objects.Cast<int>(); // [1, 2, 3, 4, 5]

// Will throw if types are incompatible
// var strings = objects.Cast<string>(); // InvalidCastException
```

## OfType vs Cast

### Key Differences
```csharp
var objects = new List<object> { "Hello", 42, 3.14, "World", 100 };

// OfType: Skips incompatible types
var strings = objects.OfType<string>(); // ["Hello", "World"]

// Cast: Throws on incompatible types
var integers = objects.Cast<int>(); // Throws InvalidCastException (3.14 can't cast to int)
```

## Complex Projections

### Multiple Properties
```csharp
var people = new List<Person> { /* ... */ };

var result = people.Select(p => new
{
    FullInfo = $"{p.Name} ({p.Age})",
    p.Id,
    p.Email,
    IsAdult = p.Age >= 18,
    Domain = p.Email.Split('@')[1]
});
```

### Conditional Projection
```csharp
var people = new List<Person> { /* ... */ };

var categorized = people.Select(p => new
{
    p.Name,
    Category = p.Age < 25 ? "Young" : p.Age < 65 ? "Adult" : "Senior"
});
```

### Nested Projection with SelectMany
```csharp
var orders = new List<Order> { /* ... */ };

var orderDetails = orders.SelectMany(o => 
    o.Lines.Select(line => new
    {
        OrderId = o.OrderId,
        line.Product,
        line.Quantity
    })
);
```

## Best Practices

1. **Project Early to Reduce Data Transfer**
```csharp
// Bad: Selecting entire entity from database
var results = dbContext.Users.ToList().Select(u => u.Name);

// Good: Project on server
var results = dbContext.Users.Select(u => u.Name).ToList();
```

2. **Use Anonymous Types for Single-Use Projections**
```csharp
// Good for one-time use
var summary = people.Select(p => new { p.Name, p.Age });

// Better for reuse - create dedicated class
public class PersonSummary
{
    public string Name { get; set; }
    public int Age { get; set; }
}
var summary = people.Select(p => new PersonSummary { Name = p.Name, Age = p.Age });
```

3. **Chain Projections Carefully**
```csharp
// Acceptable: Clear and readable
var result = people
    .Where(p => p.Age > 18)
    .Select(p => new { p.Name, p.Email })
    .ToList();

// Complex: Use intermediate variables
var adults = people.Where(p => p.Age > 18);
var contacts = adults.Select(p => new { p.Name, p.Email });
var result = contacts.ToList();
```

## Common Mistakes

1. **Over-Projecting Complex Objects**
```csharp
// Bad: Materializing unnecessary data
var result = dbContext.Users.Select(u => new
{
    u.Id,
    u.Name,
    u.Orders, // Entire collection!
    u.Address // Complex object
}).ToList();

// Good: Project only needed fields
var result = dbContext.Users.Select(u => new
{
    u.Id,
    u.Name,
    OrderCount = u.Orders.Count()
}).ToList();
```

2. **Forgetting to Materialize After Final Projection**
```csharp
// Bad: Returns IEnumerable, not materialized
IEnumerable<string> names = people.Select(p => p.Name);

// Good: Materialize to List
List<string> names = people.Select(p => p.Name).ToList();
```

3. **Using Select Inside Database Queries Incorrectly**
```csharp
// Bad: Can cause N+1 query problem
var users = dbContext.Users.Select(u => new
{
    u.Name,
    Posts = dbContext.Posts.Where(p => p.UserId == u.Id).ToList()
}).ToList();

// Good: Use Include or Join
var users = dbContext.Users.Include(u => u.Posts).ToList();
```

## Quick Summary
- Select transforms each element
- SelectMany flattens nested collections
- Cast converts all elements or throws
- OfType safely filters by type
- Project close to data source for performance
- Use anonymous types for one-time projections
- Understand deferred execution implications

## Resources
- Projection Operations (LINQ)
- Select and SelectMany documentation
- IEnumerable and IQueryable performance
