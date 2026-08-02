# Entity Framework Core

## Overview
Entity Framework Core is a modern ORM (Object-Relational Mapper) for accessing databases using .NET objects.

## DbContext Setup

### Configuration
```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("connection-string");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        
        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts);
    }
}

// Dependency injection
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
```

## CRUD Operations

### Create
```csharp
var user = new User { Name = "Alice", Email = "alice@example.com" };
context.Users.Add(user);
await context.SaveChangesAsync();
// User now has Id set by database
```

### Read
```csharp
// Get single
var user = await context.Users.FirstOrDefaultAsync(u => u.Id == 1);

// Get multiple
var users = await context.Users.Where(u => u.IsActive).ToListAsync();

// Include related data
var user = await context.Users
    .Include(u => u.Posts)
    .FirstOrDefaultAsync(u => u.Id == 1);
```

### Update
```csharp
var user = await context.Users.FindAsync(1);
user.Name = "Updated Name";
await context.SaveChangesAsync();

// Batch update
await context.Users
    .Where(u => u.IsActive)
    .ExecuteUpdateAsync(s => s.SetProperty(u => u.Name, "Updated"));
```

### Delete
```csharp
var user = await context.Users.FindAsync(1);
context.Users.Remove(user);
await context.SaveChangesAsync();

// Batch delete
await context.Users
    .Where(u => u.IsInactive)
    .ExecuteDeleteAsync();
```

## Relationships

### One-to-Many
```csharp
public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Book> Books { get; set; } = new();
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
}

// Usage
var author = await context.Authors
    .Include(a => a.Books)
    .FirstOrDefaultAsync(a => a.Id == 1);
var bookCount = author.Books.Count;
```

### Many-to-Many
```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Course> Courses { get; set; } = new();
}

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<Student> Students { get; set; } = new();
}

// Configuration
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Student>()
        .HasMany(s => s.Courses)
        .WithMany(c => c.Students)
        .UsingEntity(j => j.ToTable("StudentCourses"));
}

// Usage
var student = await context.Students
    .Include(s => s.Courses)
    .FirstOrDefaultAsync(s => s.Id == 1);
```

## Migrations

### Create and Apply
```csharp
// Add-Migration Initial
// Update-Database

// In code
using (var context = new AppDbContext())
{
    // Pending migrations applied automatically
    context.Database.Migrate();
}
```

## Best Practices

1. **Use Async Operations**
```csharp
// Good
var user = await context.Users.FindAsync(id);

// Bad: Blocks
var user = context.Users.Find(id);
```

2. **Use AsNoTracking for Read-Only**
```csharp
var report = await context.Users
    .AsNoTracking()
    .Select(u => new { u.Name, u.Email })
    .ToListAsync();
```

3. **Filter Before Materializing**
```csharp
// Good: Filtered on server
var users = await context.Users.Where(u => u.IsActive).ToListAsync();

// Bad: All loaded to memory
var users = await context.Users.ToListAsync();
var active = users.Where(u => u.IsActive).ToList();
```

## Common Mistakes

1. **Lazy Loading Without Include**
```csharp
// Bad: Extra queries (if lazy loading enabled)
var user = context.Users.First();
var postCount = user.Posts.Count(); // Extra query!

// Good: Include upfront
var user = context.Users.Include(u => u.Posts).First();
var postCount = user.Posts.Count;
```

2. **Not Disposing DbContext**
```csharp
// Bad: Resource leak
var context = new AppDbContext();
context.Users.ToList();

// Good: Use using
using var context = new AppDbContext();
var users = await context.Users.ToListAsync();
```

## Quick Summary
- DbContext represents session with database
- Add, Update, Delete modify entities
- SaveChangesAsync persists changes
- Include loads related data
- Migrations track schema changes
- Use AsNoTracking for reads
- Filter on server before materializing
- Always use async operations

## Resources
- Entity Framework Core documentation
- DbContext configuration
- Relationships configuration
