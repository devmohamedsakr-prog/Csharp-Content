# Database Access - Interview Questions & Answers

## 1. What is Entity Framework Core and what are its advantages?

**Answer:**

Entity Framework Core (EF Core) is an Object-Relational Mapper (ORM) that maps database tables to C# classes.

```csharp
// Without EF Core - raw SQL
using (SqlConnection conn = new SqlConnection(connectionString)) {
    conn.Open();
    SqlCommand cmd = new SqlCommand("SELECT * FROM Students WHERE Id = @id", conn);
    cmd.Parameters.AddWithValue("@id", 1);
    using (SqlDataReader reader = cmd.ExecuteReader()) {
        if (reader.Read()) {
            var student = new Student {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"]
            };
        }
    }
}

// With EF Core - LINQ
var student = await dbContext.Students.FirstOrDefaultAsync(s => s.Id == 1);
```

**Advantages**:
- Write less code
- Type-safe queries
- Automatic SQL generation
- Built-in change tracking
- Lazy loading and eager loading
- Migration management

---

## 2. What is DbContext and its responsibilities?

**Answer:**

DbContext is the main class for database communication.

```csharp
public class MyDbContext : DbContext {
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // Configure entities
        modelBuilder.Entity<Student>()
            .Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        // Configure relationships
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);
    }
}

// Register in Startup
services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
```

**Responsibilities**:
- Manages database connection
- Tracks entity changes
- Generates SQL queries
- Manages relationships
- Handles transactions

---

## 3. What is a migration and how do you use them?

**Answer:**

Migrations track database schema changes over time.

```csharp
// Create initial migration
// dotnet ef migrations add InitialCreate

// Generated migration file
public partial class InitialCreate : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.CreateTable(
            name: "Students",
            columns: table => new {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table => {
                table.PrimaryKey("PK_Students", x => x.Id);
            });
    }
    
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(name: "Students");
    }
}

// Apply migration to database
// dotnet ef database update

// Commands
// dotnet ef migrations add AddPhoneToStudent
// dotnet ef database update
// dotnet ef migrations list
// dotnet ef database drop
```

---

## 4. What are relationships in Entity Framework?

**Answer:**

**One-to-Many**:
```csharp
public class Student {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Enrollment> Enrollments { get; set; }  // Collection
}

public class Enrollment {
    public int Id { get; set; }
    public int StudentId { get; set; }  // Foreign key
    public Student Student { get; set; }  // Navigation
}

// Configuration
modelBuilder.Entity<Enrollment>()
    .HasOne(e => e.Student)
    .WithMany(s => s.Enrollments)
    .HasForeignKey(e => e.StudentId);
```

**Many-to-Many**:
```csharp
public class Student {
    public int Id { get; set; }
    public List<Course> Courses { get; set; }
}

public class Course {
    public int Id { get; set; }
    public List<Student> Students { get; set; }
}

// Configuration
modelBuilder.Entity<Student>()
    .HasMany(s => s.Courses)
    .WithMany(c => c.Students)
    .UsingEntity(j => j.ToTable("StudentCourses"));
```

**One-to-One**:
```csharp
public class Student {
    public int Id { get; set; }
    public StudentDetails Details { get; set; }
}

public class StudentDetails {
    public int StudentId { get; set; }
    public Student Student { get; set; }
}
```

---

## 5. What is lazy loading, eager loading, and explicit loading?

**Answer:**

**Lazy Loading** - Load related data when accessed:
```csharp
var student = await dbContext.Students.FirstAsync();
var courses = student.Courses;  // Loaded on access (separate query)
```

**Eager Loading** - Load related data upfront:
```csharp
var student = await dbContext.Students
    .Include(s => s.Courses)
    .FirstAsync();
var courses = student.Courses;  // Already loaded
```

**Explicit Loading** - Load on demand:
```csharp
var student = await dbContext.Students.FirstAsync();
await dbContext.Entry(student)
    .Collection(s => s.Courses)
    .LoadAsync();
var courses = student.Courses;  // Loaded explicitly
```

**Performance Impact**:
```csharp
// ❌ N+1 problem (lazy loading in loop)
var students = dbContext.Students.ToList();
foreach (var student in students) {
    var count = student.Courses.Count;  // Extra query per student
}

// ✓ Eager loading (one query)
var students = dbContext.Students
    .Include(s => s.Courses)
    .ToList();
foreach (var student in students) {
    var count = student.Courses.Count;  // No extra queries
}
```

---

## 6. What is LINQ to Entities vs LINQ to Objects?

**Answer:**

**LINQ to Entities** - Translated to SQL:
```csharp
// Executed on database
var students = dbContext.Students
    .Where(s => s.GPA > 3.5)
    .OrderBy(s => s.Name)
    .ToList();  // SQL generated and executed
```

**LINQ to Objects** - Executed in memory:
```csharp
// Executed in C#
List<Student> students = new List<Student> { ... };
var filtered = students
    .Where(s => s.GPA > 3.5)
    .OrderBy(s => s.Name)
    .ToList();  // Filtered in memory
```

**Key Difference**:
```csharp
// ❌ Bad - loads all then filters
var allStudents = dbContext.Students.ToList();  // All rows!
var filtered = allStudents.Where(s => s.GPA > 3.5).ToList();

// ✓ Good - filters on database
var filtered = dbContext.Students
    .Where(s => s.GPA > 3.5)
    .ToList();  // Only matching rows
```

---

## 7. What is the difference between Find and FirstOrDefault?

**Answer:**

```csharp
// Find - searches by primary key, checks identity map first
var student = dbContext.Students.Find(5);  // Fast, checks cache first

// FirstOrDefault - queries with predicate
var student = dbContext.Students.FirstOrDefault(s => s.Id == 5);

// For primary key lookups, Find is more efficient
var student1 = dbContext.Students.Find(5);  // From identity map
var student2 = dbContext.Students.Find(5);  // Same object, no query

// FirstOrDefault always queries (unless query is cached by EF)
var student1 = dbContext.Students.FirstOrDefault(s => s.Id == 5);
var student2 = dbContext.Students.FirstOrDefault(s => s.Id == 5);  // Queries again
```

---

## 8. What is change tracking and how does it work?

**Answer:**

EF Core tracks changes to entities for automatic updates.

```csharp
var student = await dbContext.Students.FindAsync(1);
Console.WriteLine(student.Name);  // "John"

// Make changes
student.Name = "Jane";

// SaveChanges detects the change and generates UPDATE
await dbContext.SaveChangesAsync();  // Generates: UPDATE Students SET Name = 'Jane' WHERE Id = 1

// Add new entity
var newStudent = new Student { Name = "Bob" };
dbContext.Students.Add(newStudent);
await dbContext.SaveChangesAsync();  // Generates: INSERT

// Delete entity
dbContext.Students.Remove(student);
await dbContext.SaveChangesAsync();  // Generates: DELETE

// Entity states
EntityState.Added       // New entity
EntityState.Modified    // Changed entity
EntityState.Deleted     // Marked for deletion
EntityState.Unchanged   // No changes
EntityState.Detached    // Not tracked
```

---

## 9. What is AsNoTracking and when to use it?

**Answer:**

AsNoTracking improves performance for read-only queries.

```csharp
// With tracking (default) - slower
var students = dbContext.Students.Where(s => s.GPA > 3.5).ToList();
// EF tracks all entities

// Without tracking - faster
var students = dbContext.Students
    .AsNoTracking()
    .Where(s => s.GPA > 3.5)
    .ToList();
// EF doesn't track entities

// Use cases:
// ✓ Read-only queries (reporting, displaying lists)
// ✗ Don't use if you plan to modify and save
```

**Performance Example**:
```csharp
// ❌ Tracking overhead for 10,000 records
var allStudents = dbContext.Students.ToList();  // Memory intensive

// ✓ No tracking for reporting
var summary = dbContext.Students
    .AsNoTracking()
    .GroupBy(s => s.Department)
    .Select(g => new { Dept = g.Key, Count = g.Count() })
    .ToList();
```

---

## 10. What is a shadow property?

**Answer:**

Properties that don't exist in C# class but exist in the database.

```csharp
public class Student {
    public int Id { get; set; }
    public string Name { get; set; }
    // CreatedDate not in class
}

// Configure shadow property
protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Student>()
        .Property<DateTime>("CreatedDate");
}

// Use shadow property
var student = new Student { Name = "John" };
dbContext.Entry(student).Property("CreatedDate").CurrentValue = DateTime.Now;
dbContext.Students.Add(student);
await dbContext.SaveChangesAsync();

// Query with shadow property
var recentStudents = dbContext.Students
    .Where(s => EF.Property<DateTime>(s, "CreatedDate") > DateTime.Now.AddMonths(-1))
    .ToList();
```

---

## 11. What is the difference between Add and Attach?

**Answer:**

```csharp
// Add - marks as new (INSERT)
var student = new Student { Name = "John" };
dbContext.Students.Add(student);
await dbContext.SaveChangesAsync();  // INSERT

// Attach - marks as existing (no change)
var detachedStudent = new Student { Id = 5, Name = "Jane" };
dbContext.Students.Attach(detachedStudent);
detachedStudent.Name = "Janet";
await dbContext.SaveChangesAsync();  // UPDATE

// Update - similar to Attach but marks as modified
var student = new Student { Id = 5, Name = "Jane" };
dbContext.Students.Update(student);
await dbContext.SaveChangesAsync();  // UPDATE (all properties)

// Scenario: API receives data from client
public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student input) {
    // Detached entity (came from client)
    var student = new Student { Id = id, Name = input.Name };
    
    dbContext.Students.Update(student);  // Mark for update
    await dbContext.SaveChangesAsync();  // UPDATE
}
```

---

## 12. What is query optimization in EF Core?

**Answer:**

```csharp
// ❌ Bad - loads unnecessary data
var students = dbContext.Students
    .Include(s => s.Courses)
    .Include(s => s.Enrollments)
    .ToList();

// ✓ Better - select only needed data
var students = dbContext.Students
    .Select(s => new {
        s.Id,
        s.Name,
        CourseCount = s.Courses.Count
    })
    .ToList();

// ❌ N+1 problem
var students = dbContext.Students.ToList();
foreach (var s in students) {
    var count = s.Courses.Count;  // Query per student
}

// ✓ Eager load
var students = dbContext.Students
    .Include(s => s.Courses)
    .ToList();

// ❌ Multiple queries
var active = dbContext.Students.Where(s => s.IsActive).ToList();
var topStudents = dbContext.Students.Where(s => s.GPA > 3.8).ToList();

// ✓ Single query with projection
var data = dbContext.Students
    .Select(s => new {
        s.Id,
        s.Name,
        IsActive = s.IsActive,
        IsTopStudent = s.GPA > 3.8
    })
    .ToList();

// Use .ToListAsync() for async
var students = await dbContext.Students.ToListAsync();
```

---

## Quick Tips for Interview

✓ Know EF Core vs raw SQL advantages
✓ Understand DbContext lifecycle
✓ Know migrations for schema management
✓ Explain one-to-many, many-to-many relationships
✓ Know lazy vs eager vs explicit loading
✓ Understand N+1 problem and solutions
✓ Know Find vs FirstOrDefault
✓ Understand change tracking
✓ Know AsNoTracking for read-only queries
✓ Comfortable with Add vs Attach vs Update
✓ Know query optimization techniques
