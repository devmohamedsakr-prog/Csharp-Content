# LINQ - Interview Questions & Answers

## 1. What is LINQ and what problem does it solve?

**Answer:**

LINQ (Language Integrated Query) provides a unified way to query different data sources using the same syntax.

```csharp
// Without LINQ (verbose)
List<int> evens = new List<int>();
foreach (int num in numbers) {
    if (num % 2 == 0) {
        evens.Add(num);
    }
}

// With LINQ (concise)
var evens = numbers.Where(n => n % 2 == 0).ToList();
```

**Benefits**:
- Unified query syntax for collections, databases, XML
- Strongly typed and IntelliSense support
- More readable and maintainable code
- Compile-time error checking

---

## 2. What is the difference between query syntax and method syntax?

**Answer:**

**Query Syntax**: SQL-like syntax
```csharp
var evens = from n in numbers
            where n % 2 == 0
            select n;
```

**Method Syntax**: Fluent API with extension methods
```csharp
var evens = numbers.Where(n => n % 2 == 0);
```

**Under the hood**: Query syntax is translated to method syntax by the compiler.

**When to Use**:
- **Query Syntax**: Complex queries with joins, groups
- **Method Syntax**: Simple queries, more flexible

```csharp
// Query syntax - cleaner for complex scenarios
var result = from student in students
             join course in courses on student.CourseId equals course.Id
             where student.Score > 80
             group student by course.Name into g
             select new { Course = g.Key, Count = g.Count() };

// Method syntax - simpler operations
var result = students.Where(s => s.Score > 80).ToList();
```

---

## 3. What are standard query operators (LINQ methods)?

**Answer:**

Common LINQ methods:

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Filtering
var evens = numbers.Where(n => n % 2 == 0);        // [2, 4]
var unique = numbers.Distinct();                    // Remove duplicates

// Projection
var doubled = numbers.Select(n => n * 2);          // [2, 4, 6, 8, 10]
var flattened = lists.SelectMany(l => l);          // Flatten nested

// Ordering
var sorted = numbers.OrderBy(n => n);              // Ascending
var reverse = numbers.OrderByDescending(n => n);   // Descending
var thenBy = students.OrderBy(s => s.Grade)
                     .ThenBy(s => s.Name);         // Multi-level

// Aggregation
int sum = numbers.Sum();                           // 15
int count = numbers.Count();                       // 5
int max = numbers.Max();                           // 5
int min = numbers.Min();                           // 1
double avg = numbers.Average();                    // 3

// Partitioning
var first3 = numbers.Take(3);                      // [1, 2, 3]
var skip2 = numbers.Skip(2);                       // [3, 4, 5]
var takeWhile = numbers.TakeWhile(n => n < 4);     // [1, 2, 3]
```

---

## 4. What is deferred execution in LINQ?

**Answer:**

LINQ queries are not executed immediately - they execute when data is accessed.

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Query defined but not executed
var query = numbers.Where(n => {
    Console.WriteLine($"Processing {n}");
    return n > 2;
});

// Nothing printed yet!

// Execution happens here
var result = query.ToList();  // Now prints: Processing 3, 4, 5
// result: [3, 4, 5]

// Or when iterating
foreach (var item in query) {
    // Executes here
}
```

**Consequences**:
```csharp
List<int> numbers = new List<int> { 1, 2, 3 };
var query = numbers.Where(n => n > 1);

numbers.Add(4);  // Modify source

// Query includes new item because execution is deferred!
var result = query.ToList();  // [2, 3, 4]
```

**Force Immediate Execution**:
```csharp
var result = query.ToList();        // IList<T>
var array = query.ToArray();        // T[]
var dict = query.ToDictionary(...); // Dictionary<K, V>
```

---

## 5. What is the difference between IEnumerable and IQueryable?

**Answer:**

| Feature | IEnumerable | IQueryable |
|---------|------------|-----------|
| Assembly | System.Collections | System.Linq |
| LINQ Provider | LINQ to Objects | Any provider |
| Execution | In-memory | Remote (DB, web) |
| Filtering | In-memory filtering | Translated to query |
| Performance | Load all, then filter | Filter at source |

```csharp
// IEnumerable - in-memory
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
IEnumerable<int> evens = numbers.Where(n => n > 2);  // LINQ to Objects

// IQueryable - queryable source
var dbContext = new MyDbContext();
IQueryable<Student> students = dbContext.Students.Where(s => s.Age > 18);  // LINQ to SQL

// Important difference
var inMemory = new List<int> { 1, 2, 3 };
IEnumerable<int> q1 = inMemory.Where(n => n > 1);  // Filters in memory

var database = dbContext.Students;
IQueryable<Student> q2 = database.Where(s => s.Age > 18);  // Translated to SQL
```

---

## 6. What is a join in LINQ?

**Answer:**

Combining data from multiple sequences based on a key.

```csharp
// Inner Join
var result = from student in students
             join course in courses on student.CourseId equals course.Id
             select new { student.Name, course.Title };

// Method syntax
var result = students.Join(courses,
    s => s.CourseId,        // outer key
    c => c.Id,              // inner key
    (s, c) => new { s.Name, c.Title }  // result selector
);

// Left Outer Join
var result = from student in students
             join course in courses on student.CourseId equals course.Id
             into courseGroup
             from course in courseGroup.DefaultIfEmpty()
             select new { student.Name, CourseName = course?.Title };
```

---

## 7. What is GroupBy and how is it used?

**Answer:**

Groups elements by a key.

```csharp
List<Student> students = new List<Student> {
    new Student { Name = "John", Grade = "A" },
    new Student { Name = "Jane", Grade = "A" },
    new Student { Name = "Bob", Grade = "B" }
};

// Query syntax
var grouped = from s in students
              group s by s.Grade into gradeGroup
              select new {
                  Grade = gradeGroup.Key,
                  Count = gradeGroup.Count(),
                  Students = gradeGroup.ToList()
              };

// Method syntax
var grouped = students
    .GroupBy(s => s.Grade)
    .Select(g => new {
        Grade = g.Key,
        Count = g.Count(),
        Students = g.ToList()
    });

// Result:
// Grade: "A", Count: 2, Students: [John, Jane]
// Grade: "B", Count: 1, Students: [Bob]
```

---

## 8. What are Set Operations in LINQ?

**Answer:**

Operations on sets of data:

```csharp
List<int> list1 = new List<int> { 1, 2, 3, 4 };
List<int> list2 = new List<int> { 3, 4, 5, 6 };

// Union - combine, remove duplicates
var union = list1.Union(list2);  // [1, 2, 3, 4, 5, 6]

// Intersect - common elements
var common = list1.Intersect(list2);  // [3, 4]

// Except - in first but not in second
var unique = list1.Except(list2);  // [1, 2]

// Distinct - remove duplicates
List<int> numbers = new List<int> { 1, 2, 2, 3, 3, 3 };
var unique = numbers.Distinct();  // [1, 2, 3]
```

---

## 9. What is the difference between Any and All?

**Answer:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Any - returns true if ANY element matches condition
bool hasEven = numbers.Any(n => n % 2 == 0);  // true
bool hasNegative = numbers.Any(n => n < 0);  // false
bool isEmpty = numbers.Any();  // true (has any elements)

// All - returns true if ALL elements match condition
bool allPositive = numbers.All(n => n > 0);  // true
bool allEven = numbers.All(n => n % 2 == 0);  // false
```

---

## 10. What is Select vs SelectMany?

**Answer:**

```csharp
// Select - one-to-one mapping
List<int> numbers = new List<int> { 1, 2, 3 };
var doubled = numbers.Select(n => n * 2);  // [2, 4, 6]

// SelectMany - one-to-many, then flattens
List<List<int>> lists = new List<List<int>> {
    new List<int> { 1, 2 },
    new List<int> { 3, 4 },
    new List<int> { 5, 6 }
};

var flattened = lists.SelectMany(l => l);  // [1, 2, 3, 4, 5, 6]

// Real example
List<Student> students = ...;
// Select - returns List<List<Course>>
var courseLists = students.Select(s => s.Courses);

// SelectMany - flattens to List<Course>
var allCourses = students.SelectMany(s => s.Courses);
```

---

## 11. What is First, FirstOrDefault, Single, and SingleOrDefault?

**Answer:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// First - throws if not found
int first = numbers.First();  // 1
int firstEven = numbers.First(n => n % 2 == 0);  // 2

// FirstOrDefault - returns default if not found
int? first = numbers.FirstOrDefault();  // 1
int? notFound = new List<int>().FirstOrDefault();  // 0

// Single - throws if not exactly one
int single = numbers.Where(n => n == 3).Single();  // 3
// numbers.Where(n => n > 2).Single();  // throws (multiple matches)

// SingleOrDefault - null if not found or multiple
int? single = numbers.Where(n => n == 3).SingleOrDefault();  // 3
```

**When to Use**:
- **First**: You expect at least one element
- **FirstOrDefault**: Zero or more elements expected
- **Single**: Exactly one element expected
- **SingleOrDefault**: Zero or one element expected

---

## 12. What are some common LINQ performance pitfalls?

**Answer:**

```csharp
// ❌ Bad: Multiple iterations
var result = students
    .Where(s => s.Score > 80)  // Iterates
    .OrderBy(s => s.Name)      // Iterates again
    .Take(10);

// ✓ Better: Combined query
var result = students
    .Where(s => s.Score > 80)
    .OrderBy(s => s.Name)
    .Take(10)
    .ToList();  // Single iteration with ToList()

// ❌ Bad: Loading all data then filtering
var students = dbContext.Students.ToList().Where(s => s.Age > 18);

// ✓ Better: Filter at source
var students = dbContext.Students.Where(s => s.Age > 18).ToList();

// ❌ Bad: Complex LINQ in tight loop
for (int i = 0; i < 1000; i++) {
    var result = items.Where(x => x.Value > i).ToList();
}

// ✓ Better: Pre-compute
var grouped = items.GroupBy(x => x.Value);
```

---

## Quick Tips for Interview

✓ Understand deferred execution
✓ Know difference between IEnumerable and IQueryable
✓ Comfortable with common operators: Where, Select, GroupBy, Join
✓ Understand query syntax vs method syntax
✓ Know First/FirstOrDefault/Single/SingleOrDefault
✓ Explain Any vs All
✓ Understand performance implications
✓ Know SelectMany flattens nested collections
