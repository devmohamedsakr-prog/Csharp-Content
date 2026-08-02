# Lambda Expressions

## Overview
Lambda expressions are inline anonymous methods with concise syntax.

---

## Basic Syntax

```csharp
// Traditional anonymous method
Func<int, int> square = delegate(int x) {
    return x * x;
};

// Lambda expression - much simpler
Func<int, int> square = x => x * x;

// With multiple parameters
Func<int, int, int> add = (a, b) => a + b;

// With no parameters
Action greet = () => Console.WriteLine("Hello");

// With multiple statements
Func<int, string> describe = x => {
    if (x > 0) return "Positive";
    if (x < 0) return "Negative";
    return "Zero";
};
```

---

## With Collections

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// Filter with Where
var evens = numbers.Where(n => n % 2 == 0);  // [2, 4]

// Transform with Select
var doubled = numbers.Select(n => n * 2);  // [2, 4, 6, 8, 10]

// Find with FirstOrDefault
int first = numbers.FirstOrDefault(n => n > 3);  // 4

// All matching condition
bool allPositive = numbers.All(n => n > 0);  // true

// Any matching condition
bool hasEven = numbers.Any(n => n % 2 == 0);  // true

// Aggregate
int sum = numbers.Aggregate(0, (acc, n) => acc + n);  // 15
```

---

## Type Inference

```csharp
// Compiler infers types
var add = (int a, int b) => a + b;

// Or with Func
Func<int, int, int> add = (a, b) => a + b;

// String operations
Func<string, string> upper = s => s.ToUpper();

// Predicates
Predicate<int> isPositive = n => n > 0;
```

---

## Complex Lambdas

```csharp
// Multiple statements
Func<int, int, int> max = (a, b) => {
    if (a > b) return a;
    return b;
};

// With objects
Func<Person, string> getFullName = p => $"{p.FirstName} {p.LastName}";

// Nested lambdas
Func<int, Func<int, int>> multiplier = x => y => x * y;
var times2 = multiplier(2);
int result = times2(5);  // 10
```

---

## LINQ with Lambdas

```csharp
List<Person> people = new List<Person> {
    new Person { Name = "Alice", Age = 30 },
    new Person { Name = "Bob", Age = 25 },
    new Person { Name = "Charlie", Age = 35 }
};

// Filter
var adults = people.Where(p => p.Age >= 18);

// Select (transform)
var names = people.Select(p => p.Name);

// OrderBy
var sorted = people.OrderBy(p => p.Age);

// GroupBy
var grouped = people.GroupBy(p => p.Age / 10);

// FirstOrDefault
Person oldest = people.FirstOrDefault(p => p.Age == people.Max(x => x.Age));

// All / Any
bool allAdults = people.All(p => p.Age >= 18);
bool hasOldPerson = people.Any(p => p.Age > 60);

// Aggregate
string allNames = people.Aggregate("", (acc, p) => acc + ", " + p.Name).TrimStart(',', ' ');
```

---

## Closure

Lambda captures variables from outer scope.

```csharp
int multiplier = 2;

Func<int, int> multiply = n => n * multiplier;

Console.WriteLine(multiply(5));  // 10

multiplier = 3;
Console.WriteLine(multiply(5));  // 15 - captured variable changed

// Avoiding closure issues
List<Func<int>> functions = new List<Func<int>>();

for (int i = 0; i < 3; i++) {
    int temp = i;  // Create new variable each iteration
    functions.Add(() => temp);
}

foreach (var func in functions) {
    Console.WriteLine(func());  // 0, 1, 2
}
```

---

## Expression Bodied Members

Lambda-like syntax for members.

```csharp
public class Person {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    // Expression bodied property
    public string FullName => $"{FirstName} {LastName}";
    
    // Expression bodied method
    public string Greet() => $"Hello, I'm {FullName}";
    
    // Expression bodied operator
    public static Person operator +(Person p1, Person p2) =>
        new Person { FirstName = p1.FirstName + p2.FirstName };
}
```

---

## Quick Summary

- Lambda expressions are anonymous methods
- Use => for concise syntax
- Commonly used with LINQ
- Can capture outer variables (closure)
- Improves code readability
- Alternative to traditional delegates
