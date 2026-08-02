# LINQ Grouping Operations

## Overview
Grouping collects elements into groups based on a key, similar to SQL GROUP BY.

## GroupBy Operator

### Basic Grouping
```csharp
public class Student
{
    public string Name { get; set; }
    public string Department { get; set; }
    public int Age { get; set; }
    public decimal GPA { get; set; }
}

var students = new List<Student>
{
    new Student { Name = "Alice", Department = "CS", Age = 20, GPA = 3.8m },
    new Student { Name = "Bob", Department = "IT", Age = 21, GPA = 3.6m },
    new Student { Name = "Charlie", Department = "CS", Age = 20, GPA = 3.9m },
    new Student { Name = "Diana", Department = "IT", Age = 22, GPA = 3.7m }
};

// Group by department
var byDept = students.GroupBy(s => s.Department);

foreach (var group in byDept)
{
    Console.WriteLine($"Department: {group.Key}");
    foreach (var student in group)
    {
        Console.WriteLine($"  {student.Name}: {student.GPA}");
    }
}

// Output:
// Department: CS
//   Alice: 3.8
//   Charlie: 3.9
// Department: IT
//   Bob: 3.6
//   Diana: 3.7
```

### Query Syntax Grouping
```csharp
var result = from student in students
             group student by student.Department;
```

### Projecting Grouped Results
```csharp
var result = students.GroupBy(
    s => s.Department,
    (key, group) => new
    {
        Department = key,
        Count = group.Count(),
        AvgGPA = group.Average(s => s.GPA),
        Students = group.Select(s => s.Name).ToList()
    }
);

// Result:
// { Department = "CS", Count = 2, AvgGPA = 3.85, Students = ["Alice", "Charlie"] }
// { Department = "IT", Count = 2, AvgGPA = 3.65, Students = ["Bob", "Diana"] }
```

## Multiple Key Grouping

### Grouping by Multiple Properties
```csharp
// Group by department and age
var byDeptAndAge = students.GroupBy(
    s => new { s.Department, s.Age }
);

foreach (var group in byDeptAndAge)
{
    Console.WriteLine($"Department: {group.Key.Department}, Age: {group.Key.Age}");
    foreach (var student in group)
    {
        Console.WriteLine($"  {student.Name}");
    }
}
```

### Using Tuples
```csharp
// Group by multiple keys using tuple
var result = students.GroupBy(
    s => (s.Department, s.Age),
    (key, group) => new
    {
        Department = key.Department,
        Age = key.Age,
        StudentCount = group.Count(),
        Names = group.Select(s => s.Name).ToList()
    }
);
```

## Aggregating Grouped Data

### Aggregation Methods
```csharp
var departmentStats = students.GroupBy(
    s => s.Department,
    (dept, group) => new
    {
        Department = dept,
        Count = group.Count(),
        AvgGPA = group.Average(s => s.GPA),
        MaxGPA = group.Max(s => s.GPA),
        MinGPA = group.Min(s => s.GPA),
        TotalAge = group.Sum(s => s.Age),
        StudentNames = string.Join(", ", group.Select(s => s.Name))
    }
);

// Result:
// { Department = "CS", Count = 2, AvgGPA = 3.85, MaxGPA = 3.9, MinGPA = 3.8, ... }
```

### Query Syntax with Aggregation
```csharp
var result = from student in students
             group student by student.Department into deptGroup
             select new
             {
                 Department = deptGroup.Key,
                 Count = deptGroup.Count(),
                 AvgGPA = deptGroup.Average(s => s.GPA)
             };
```

## Advanced Grouping

### Grouped Data with Filtering
```csharp
// Groups with specific aggregate conditions
var goodGroups = students.GroupBy(
    s => s.Department,
    (dept, group) => new
    {
        Department = dept,
        Students = group.Where(s => s.GPA >= 3.7).ToList(),
        AverageGPA = group.Average(s => s.GPA)
    }
)
.Where(g => g.AverageGPA >= 3.75);
```

### Nested Grouping
```csharp
// Group by department, then by age
var nestedGroups = students.GroupBy(s => s.Department)
    .Select(deptGroup => new
    {
        Department = deptGroup.Key,
        AgeGroups = deptGroup.GroupBy(s => s.Age)
            .Select(ageGroup => new
            {
                Age = ageGroup.Key,
                Students = ageGroup.Select(s => s.Name).ToList()
            })
            .ToList()
    });
```

## ToLookup vs GroupBy

### Key Differences
```csharp
// GroupBy - IEnumerable<IGrouping<TKey, TElement>>
var groups = students.GroupBy(s => s.Department);

// ToLookup - ILookup<TKey, TElement>> (optimized for lookups)
var lookup = students.ToLookup(s => s.Department);

// Lookup supports direct key access
var csStudents = lookup["CS"]; // Fast lookup

// With GroupBy, must search
var csGroup = groups.FirstOrDefault(g => g.Key == "CS");

// Lookup supports empty groups gracefully
var noStudents = lookup["NonExistent"]; // Empty collection, no error

// GroupBy must check for null
var noGroup = groups.FirstOrDefault(g => g.Key == "NonExistent"); // null
```

## Partition and Chunk

### Grouping by Position (LINQ newer versions)
```csharp
// Chunk - divides into groups of specified size
var chunks = students.Chunk(2);
// [[Alice, Bob], [Charlie, Diana]]

// Create numbered groups
var numbered = students.Select((s, i) => (index: i / 2, student: s))
    .GroupBy(x => x.index, x => x.student);
```

## Best Practices

1. **Use Lookup for Repeated Key Access**
```csharp
// Bad: Multiple searches
var allCS = groups.Where(g => g.Key == "CS").SelectMany(g => g);
var firstCS = groups.Where(g => g.Key == "CS").FirstOrDefault();

// Good: Use lookup
var lookup = students.ToLookup(s => s.Department);
var allCS = lookup["CS"];
var firstCS = lookup["CS"].FirstOrDefault();
```

2. **Materialize Groups Before Using Multiple Times**
```csharp
// Bad: Group might be evaluated multiple times
var result = students.GroupBy(s => s.Department)
    .Where(g => g.Count() > 1) // First enumeration
    .Select(g => new { 
        Count = g.Count(), // Second enumeration
        Avg = g.Average(s => s.GPA) // Third enumeration
    });

// Good: Materialize once
var result = students.GroupBy(s => s.Department)
    .Select(g => new { 
        Key = g.Key,
        Count = g.Count(),
        Avg = g.Average(s => s.GPA)
    })
    .Where(g => g.Count > 1);
```

3. **Filter Before Grouping for Large Collections**
```csharp
// Bad: Groups all before filtering
var result = students.GroupBy(s => s.Department)
    .Where(g => g.Count() > 1);

// Good: Filter smaller subset first
var result = students.Where(s => s.GPA >= 3.5)
    .GroupBy(s => s.Department)
    .Where(g => g.Count() > 1);
```

## Common Mistakes

1. **Forgetting to Handle Empty Groups**
```csharp
// Bad: Assumes groups exist
var firstDept = groups.First().Key;

// Good: Check for empty
var firstDept = groups.FirstOrDefault()?.Key;
```

2. **Using GroupBy Instead of ToLookup for Lookups**
```csharp
// Bad: Inefficient for lookups
var groups = students.GroupBy(s => s.Department);
var csStudents = groups.FirstOrDefault(g => g.Key == "CS");

// Good: Use ToLookup
var lookup = students.ToLookup(s => s.Department);
var csStudents = lookup["CS"];
```

3. **Multiple Enumerations of Same Group**
```csharp
// Bad: Group enumerated multiple times
var group = byDept.FirstOrDefault();
var count = group.Count(); // First pass
var names = group.Select(s => s.Name).ToList(); // Second pass

// Good: Materialize once
var groupList = group.ToList();
var count = groupList.Count;
var names = groupList.Select(s => s.Name).ToList();
```

## Quick Summary
- GroupBy groups elements by key
- Multiple keys use anonymous types or tuples
- Aggregation methods count, sum, average, min, max
- ToLookup optimizes for repeated key lookups
- Materialize groups before multiple enumerations
- Filter before grouping for performance
- Query syntax: `group ... by ... into`

## Resources
- Grouping Data (LINQ)
- GroupBy vs ToLookup comparison
- IGrouping and ILookup interfaces
