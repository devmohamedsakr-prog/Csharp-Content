# Sorting & Filtering

Quick LINQ snippets for sorting and filtering.

## Sort

```csharp
var numbers = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6 };

// Ascending
var ascending = numbers.OrderBy(x => x).ToList();

// Descending
var descending = numbers.OrderByDescending(x => x).ToList();

// Multiple criteria
var students = new List<Student>();
var sorted = students
    .OrderBy(s => s.Grade)
    .ThenBy(s => s.Name)
    .ToList();

// Sort in-place
numbers.Sort();
```

## Filter

```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Where condition
var evens = numbers.Where(x => x % 2 == 0).ToList();

// Multiple conditions
var filtered = numbers
    .Where(x => x > 3 && x < 8)
    .ToList();

// Filter by type
var objects = new List<object> { 1, "text", 2.5, "more" };
var strings = objects.OfType<string>().ToList();
```

## Distinct & Unique

```csharp
var numbers = new List<int> { 1, 2, 2, 3, 3, 3, 4 };

// Remove duplicates
var unique = numbers.Distinct().ToList();

// Distinct by property
var students = new List<Student>();
var uniqueGrades = students
    .DistinctBy(s => s.Grade)
    .ToList();

// First N unique
var firstUnique = numbers.Distinct().Take(3).ToList();
```

## Take & Skip

```csharp
var numbers = Enumerable.Range(1, 10).ToList();

// First N items
var first3 = numbers.Take(3).ToList();  // [1, 2, 3]

// Skip N items
var skipFirst3 = numbers.Skip(3).ToList();  // [4, 5, ..., 10]

// Pagination
int pageSize = 5;
int pageNumber = 2;
var page = numbers
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();

// Until condition
var until5 = numbers.TakeWhile(x => x < 5).ToList();
```

## Search

```csharp
var students = new List<Student>();

// Any - exists
bool anyGradeA = students.Any(s => s.Grade == 'A');

// All - all match
bool allPassed = students.All(s => s.Score >= 60);

// First - with default
var student = students.FirstOrDefault(s => s.Id == 5);

// Single - must be exactly one
var unique = students.Single(s => s.Email == "unique@email.com");

// Last
var last = students.Last();
```

