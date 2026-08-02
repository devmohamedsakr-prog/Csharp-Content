# LINQ Aggregation Operations

## Overview
Aggregation operations compute single values from collections of data.

## Count and LongCount

### Counting Elements
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Count all elements
int total = numbers.Count(); // 10

// Count with condition
int evens = numbers.Count(n => n % 2 == 0); // 5

// Count objects
var people = new List<Person> { /* ... */ };
int adults = people.Count(p => p.Age >= 18);

// LongCount - for large collections (returns long)
long largeCount = numbers.LongCount();
```

## Sum

### Summing Values
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Sum all elements
int total = numbers.Sum(); // 15

// Sum with projection
int doubled = numbers.Sum(n => n * 2); // 30

// Sum with objects
var people = new List<Person>
{
    new Person { Name = "Alice", Salary = 50000 },
    new Person { Name = "Bob", Salary = 60000 },
    new Person { Name = "Charlie", Salary = 55000 }
};

decimal totalSalary = people.Sum(p => p.Salary); // 165000
```

## Average

### Computing Averages
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Average of all elements
double avg = numbers.Average(); // 3.0

// Average with condition
double evenAvg = numbers.Where(n => n % 2 == 0).Average(); // 4.0 (2,4,6,8,10)

// Average of salaries
double avgSalary = people.Average(p => p.Salary); // 55000

// Safe average handling
var empty = new List<int>();
double safeAvg = empty.Any() ? empty.Average() : 0; // 0 instead of exception
```

## Min and Max

### Finding Extremes
```csharp
var numbers = new List<int> { 5, 2, 8, 1, 9, 3 };

// Minimum value
int min = numbers.Min(); // 1

// Maximum value
int max = numbers.Max(); // 9

// Min/Max with objects
var people = new List<Person> { /* ... */ };

// Youngest person's age
int youngestAge = people.Min(p => p.Age);

// Highest salary
decimal maxSalary = people.Max(p => p.Salary);

// Find person with highest salary
var topPerson = people.Where(p => p.Salary == people.Max(x => x.Salary)).First();
```

### MinBy and MaxBy (C# 8.0+)
```csharp
// Get person with highest salary (not just the salary value)
var topEarner = people.MaxBy(p => p.Salary); // Returns Person object

// Get person with lowest age
var youngest = people.MinBy(p => p.Age);

// Multiple criteria
var bestStudent = students.MaxBy(s => s.GPA);
```

## Aggregate

### Custom Aggregation
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Aggregate: combine all elements into single value
// Seed value, accumulator function
int product = numbers.Aggregate(1, (acc, n) => acc * n); // 120

// Without seed (uses first element)
int sum = numbers.Aggregate((acc, n) => acc + n); // 15

// String concatenation
var words = new List<string> { "Hello", "World", "!" };
string sentence = words.Aggregate((acc, w) => acc + " " + w); // "Hello World !"
```

### Advanced Aggregate
```csharp
// Aggregate with result selector
var result = numbers.Aggregate(
    0, // seed
    (acc, n) => acc + n, // accumulator
    acc => acc * 2 // result selector
); // 30 (sum=15, multiply by 2)

// Aggregate collecting items
var items = new List<int> { 1, 2, 3, 4, 5 };
List<int> evens = items.Aggregate(
    new List<int>(),
    (list, item) => 
    {
        if (item % 2 == 0) list.Add(item);
        return list;
    }
); // [2, 4]
```

## First, Last, and Single

### Getting Specific Elements
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// First element
int first = numbers.First(); // 1

// First with condition
int firstEven = numbers.First(n => n % 2 == 0); // 2

// First or default (no exception if empty or not found)
int firstOrDefault = numbers.FirstOrDefault(); // 1
int notFound = numbers.FirstOrDefault(n => n > 100); // 0

// Last element
int last = numbers.Last(); // 5

// Last with condition
int lastEven = numbers.Last(n => n % 2 == 0); // 4

// Last or default
int lastOrDefault = numbers.LastOrDefault(); // 5

// Single element (throws if 0 or 2+ elements)
var singleList = new List<int> { 42 };
int single = singleList.Single(); // 42

// Single with condition
int singleEven = numbers.Single(n => n == 2); // Throws if 0 or 2+ matches

// Single or default
int singleOrDefault = singleList.SingleOrDefault(); // 42
int notFoundSingle = numbers.SingleOrDefault(n => n > 100); // 0
```

## Any and All

### Existence and Universal Checks
```csharp
var numbers = new List<int> { 2, 4, 6, 8, 10 };

// Any: at least one element exists
bool hasElements = numbers.Any(); // true

// Any with condition: at least one matches
bool hasOdd = numbers.Any(n => n % 2 != 0); // false

// All: all elements match condition
bool allEven = numbers.All(n => n % 2 == 0); // true

// Practical uses
var people = new List<Person> { /* ... */ };

// Check if any adult
bool hasAdults = people.Any(p => p.Age >= 18);

// Check if all have valid email
bool allValid = people.All(p => !string.IsNullOrEmpty(p.Email));

// Check if collection is empty
bool isEmpty = !numbers.Any(); // Same as numbers.Count() == 0
```

## ElementAt

### Index-Based Access
```csharp
var numbers = new List<int> { 10, 20, 30, 40, 50 };

// Get element at index
int third = numbers.ElementAt(2); // 30

// ElementAtOrDefault
int notFound = numbers.ElementAtOrDefault(10); // 0 (default)

// Useful for query results
var result = numbers.Where(n => n > 15).ElementAt(0); // 20
```

## Complex Aggregation Examples

### Multiple Aggregations
```csharp
var sales = new List<decimal> { 100, 250, 150, 300, 200 };

var stats = new
{
    Total = sales.Sum(),
    Count = sales.Count(),
    Average = sales.Average(),
    Min = sales.Min(),
    Max = sales.Max(),
    Variance = CalculateVariance(sales)
};

private static decimal CalculateVariance(List<decimal> values)
{
    decimal avg = values.Average();
    return values.Average(v => (v - avg) * (v - avg));
}
```

### Group Statistics
```csharp
var students = new List<Student> { /* ... */ };

var deptStats = students.GroupBy(s => s.Department)
    .Select(g => new
    {
        Department = g.Key,
        StudentCount = g.Count(),
        AverageGPA = g.Average(s => s.GPA),
        HighestGPA = g.Max(s => s.GPA),
        LowestGPA = g.Min(s => s.GPA),
        TopStudent = g.MaxBy(s => s.GPA)
    });
```

## Best Practices

1. **Use Appropriate Aggregation for Performance**
```csharp
// Bad: Multiple separate queries
int count = items.Count();
decimal sum = items.Sum();
decimal avg = items.Average();

// Better: Single pass (if database)
var stats = items.Select(i => new { i.Sum, i.Count })
    .FirstOrDefault();

// Good: Combine in one query if possible
var data = items.GroupBy(x => 1).Select(g => new
{
    Count = g.Count(),
    Sum = g.Sum(x => x.Value),
    Avg = g.Average(x => x.Value)
});
```

2. **Handle Empty Collections**
```csharp
// Bad: Throws if empty
var avg = items.Average(); // InvalidOperationException

// Good: Safe handling
var avg = items.Any() ? items.Average() : 0;

// Or use FirstOrDefault pattern
var result = items.GroupBy(x => 1)
    .Select(g => g.Average())
    .FirstOrDefault();
```

3. **Use Appropriate Null Handling**
```csharp
// Bad: Null values cause errors
var max = people.Max(p => p.Salary);

// Good: Handle nulls
var max = people
    .Where(p => p.Salary.HasValue)
    .Max(p => p.Salary.Value);

// Or use coalesce
var max = people.Max(p => p.Salary ?? 0);
```

## Common Mistakes

1. **Multiple Passes Over Data**
```csharp
// Bad: Enumerates collection multiple times
var count = items.Count();
var sum = items.Sum();
var avg = items.Average();

// Better: Cache results if used multiple times
var list = items.ToList();
var stats = new { count = list.Count, sum = list.Sum(), avg = list.Average() };
```

2. **Aggregate on Empty Collection**
```csharp
// Bad: Throws if no elements match
var first = items.First(x => x.IsActive); // NoElementException

// Good: Use FirstOrDefault
var first = items.FirstOrDefault(x => x.IsActive);
```

3. **Single with Multiple Matches**
```csharp
// Bad: Throws if 2+ matches (defeats purpose)
var item = items.Single(x => x.Status == "Active");

// Good: Use FirstOrDefault for multiple matches
var item = items.FirstOrDefault(x => x.Status == "Active");

// Or explicit count check
var active = items.Where(x => x.Status == "Active").ToList();
var count = active.Count; // Manual count before using Single
```

## Quick Summary
- Count, Sum, Average, Min, Max for numeric aggregations
- First, Last, Single for element retrieval
- Any, All for condition checks
- Aggregate for custom aggregation
- Handle empty collections gracefully
- Use appropriate null handling
- Combine aggregations in single pass when possible
- Single is strict; use FirstOrDefault for flexibility

## Resources
- Aggregation Operations (LINQ)
- IEnumerable aggregate methods
- LINQ performance considerations
