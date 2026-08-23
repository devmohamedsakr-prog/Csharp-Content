# LINQ Snippets

Language Integrated Query patterns and methods.

## from - Query Expression

**Pattern:**
```csharp
var query = from item in items
            where item > 5
            select item;
```

**Usage:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
var result = from n in numbers
             where n > 5
             select n;
```

---

## from - Multiple Collections

**Pattern:**
```csharp
var query = from customer in customers
            from order in customer.Orders
            where order.Total > 100
            select new { customer.Name, order.OrderId };
```

---

## where - Filter

**Query Expression:**
```csharp
var result = from n in numbers
             where n % 2 == 0
             select n;
```

**Method Syntax:**
```csharp
var result = numbers.Where(n => n % 2 == 0);
```

**Complex Condition:**
```csharp
var students = new List<Student>();
var result = students
    .Where(s => s.Age >= 18 && s.Score >= 60)
    .ToList();
```

---

## select - Transform

**Query Expression:**
```csharp
var names = from student in students
            select student.Name;
```

**Method Syntax:**
```csharp
var names = students.Select(s => s.Name);
```

**Complex Selection:**
```csharp
var result = students
    .Select(s => new 
    { 
        s.Name, 
        s.Score,
        Grade = s.Score >= 90 ? "A" : "B"
    })
    .ToList();
```

---

## OrderBy - Ascending Sort

**Query Expression:**
```csharp
var result = from student in students
             orderby student.Name
             select student;
```

**Method Syntax:**
```csharp
var result = students.OrderBy(s => s.Name).ToList();
```

---

## OrderByDescending - Descending Sort

**Query Expression:**
```csharp
var result = from student in students
             orderby student.Score descending
             select student;
```

**Method Syntax:**
```csharp
var result = students.OrderByDescending(s => s.Score).ToList();
```

---

## ThenBy - Multi-Level Sort

**Query Expression:**
```csharp
var result = from student in students
             orderby student.Grade, student.Name
             select student;
```

**Method Syntax:**
```csharp
var result = students
    .OrderBy(s => s.Grade)
    .ThenBy(s => s.Name)
    .ToList();
```

---

## join - Join Collections

**Query Expression:**
```csharp
var result = from student in students
             join course in courses on student.CourseId equals course.Id
             select new { student.Name, course.Title };
```

**Method Syntax:**
```csharp
var result = students
    .Join(courses,
        s => s.CourseId,
        c => c.Id,
        (s, c) => new { s.Name, c.Title })
    .ToList();
```

---

## GroupBy - Group Items

**Query Expression:**
```csharp
var result = from student in students
             group student by student.Grade into g
             select new { Grade = g.Key, Count = g.Count() };
```

**Method Syntax:**
```csharp
var result = students
    .GroupBy(s => s.Grade)
    .Select(g => new { Grade = g.Key, Count = g.Count() })
    .ToList();
```

---

## Distinct - Remove Duplicates

**Pattern:**
```csharp
var numbers = new List<int> { 1, 2, 2, 3, 3, 3, 4 };
var unique = numbers.Distinct().ToList();  // [1, 2, 3, 4]

var names = students.Select(s => s.Grade).Distinct();

// Custom equality
var uniqueStudents = students.DistinctBy(s => s.Email);
```

---

## First, FirstOrDefault, Last, LastOrDefault

**Pattern:**
```csharp
var first = students.First();  // Throws if empty
var firstOrNull = students.FirstOrDefault();  // null if empty

var withCondition = students.First(s => s.Score > 90);
var withConditionOrNull = students.FirstOrDefault(s => s.Score > 90);

var last = students.Last();
var lastOrNull = students.LastOrDefault();
```

---

## Single, SingleOrDefault

**Pattern:**
```csharp
// Exactly one item
var unique = students.Single(s => s.Id == 5);

// Unique or null
var uniqueOrNull = students.SingleOrDefault(s => s.Email == "unique@email.com");
```

---

## Any, All

**Pattern:**
```csharp
// Check if any item matches
bool hasHighScorer = students.Any(s => s.Score > 90);

// Check if all items match
bool allPassed = students.All(s => s.Score >= 60);
```

---

## Count, Sum, Average, Min, Max

**Pattern:**
```csharp
int count = students.Count();
int countHigh = students.Count(s => s.Score > 90);

decimal total = orders.Sum(o => o.Amount);
decimal average = students.Average(s => s.Score);

int minimum = numbers.Min();
int maximum = numbers.Max();

var best = students.MaxBy(s => s.Score);
var worst = students.MinBy(s => s.Score);
```

---

## Take, Skip

**Pattern:**
```csharp
var first5 = students.Take(5).ToList();
var skip5 = students.Skip(5).ToList();

// Pagination
int pageSize = 10;
int page = 2;
var pageData = students
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToList();
```

---

## Select Many - Flatten

**Query Expression:**
```csharp
var allOrders = from customer in customers
                from order in customer.Orders
                select order;
```

**Method Syntax:**
```csharp
var allOrders = customers.SelectMany(c => c.Orders);
```

---

## Distinct + Custom Comparer

**Pattern:**
```csharp
public class StudentEqualityComparer : IEqualityComparer<Student>
{
    public bool Equals(Student x, Student y) => x.Id == y.Id;
    public int GetHashCode(Student s) => s.Id.GetHashCode();
}

var unique = students.Distinct(new StudentEqualityComparer());
```

---

## Except, Intersect, Union

**Pattern:**
```csharp
var list1 = new List<int> { 1, 2, 3, 4, 5 };
var list2 = new List<int> { 3, 4, 5, 6, 7 };

var except = list1.Except(list2);     // [1, 2]
var intersect = list1.Intersect(list2);  // [3, 4, 5]
var union = list1.Union(list2);       // [1, 2, 3, 4, 5, 6, 7]
```

---

## Aggregate - Reduce

**Pattern:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
int sum = numbers.Aggregate((acc, n) => acc + n);  // 15

string result = words.Aggregate((acc, w) => $"{acc} {w}");
```

---

## Let - Intermediate Variable

**Query Expression:**
```csharp
var result = from student in students
             let grade = student.Score >= 90 ? "A" : "B"
             where grade == "A"
             select new { student.Name, grade };
```

---

## Quick Reference

| Method | Purpose |
|--------|---------|
| `Where` | Filter items |
| `Select` | Transform items |
| `OrderBy/ThenBy` | Sort ascending |
| `OrderByDescending` | Sort descending |
| `GroupBy` | Group items |
| `Join` | Join collections |
| `Take/Skip` | Get subset |
| `Distinct` | Remove duplicates |
| `First/Last` | Get specific item |
| `Any/All` | Check conditions |
| `Count/Sum/Avg` | Aggregate |
| `SelectMany` | Flatten |

---

## Deferred Execution

```csharp
var query = numbers.Where(n => n > 5);  // Not executed yet
foreach (var n in query)  // Executed here
{
    Console.WriteLine(n);
}

var list = query.ToList();  // Force execution
```

---

## Best Practices

- Use method syntax when more readable
- Use query syntax for complex operations
- Call `.ToList()` when you need to force evaluation
- Be aware of deferred execution
- Use `.AsEnumerable()` to switch to LINQ-to-Objects
- Avoid N+1 queries with `.Include()` in EF
- Use `.Distinct()` to remove duplicates
- Chain methods for readable pipelines

