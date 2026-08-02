# Tuples and Records

## Overview
Tuples group multiple values. Records are immutable reference types optimized for data.

---

## Tuples

### Value Tuples (C# 7+)

```csharp
// Create tuple
(int, string) tuple1 = (1, "Alice");

// Named tuple
(int Id, string Name) tuple2 = (1, "Bob");

// Access by position
int id = tuple2.Item1;
string name = tuple2.Item2;

// Access by name
int id2 = tuple2.Id;
string name2 = tuple2.Name;

// Deconstruction
(int id3, string name3) = (1, "Charlie");

// With type inference
var tuple3 = (Id: 1, Name: "David");
```

### Tuple Methods

```csharp
public (bool success, string message) ProcessData(string input) {
    if (string.IsNullOrEmpty(input)) {
        return (false, "Input is empty");
    }
    return (true, "Success");
}

// Usage
(bool success, string msg) = ProcessData("data");
if (success) {
    Console.WriteLine(msg);
}

// Discard unused values
(bool _, string message) = ProcessData("test");
Console.WriteLine(message);
```

### Tuple Comparison

```csharp
(int, string) tuple1 = (1, "A");
(int, string) tuple2 = (1, "A");

// Tuples support equality
bool equal = tuple1 == tuple2;  // true

// Tuples support comparison
(int, int) a = (1, 2);
(int, int) b = (1, 3);
bool less = a < b;  // true
```

---

## Records (C# 9+)

Immutable reference types optimized for data.

### Record Declaration

```csharp
// Simple record
public record Person(string Name, int Age);

// With properties
public record Person {
    public string Name { get; init; }
    public int Age { get; init; }
}

// Mix positional and properties
public record Employee(string Name) {
    public int Id { get; init; }
    public decimal Salary { get; init; }
}
```

### Record Usage

```csharp
// Create with positional syntax
Person person1 = new Person("Alice", 30);

// Create with named properties
Person person2 = new Person { Name = "Bob", Age = 25 };

// Access properties
string name = person1.Name;
int age = person1.Age;

// Records are immutable
// person1.Name = "Charlie";  // Error - immutable

// With keyword for immutable copy
Person person3 = person1 with { Age = 31 };
// Creates new instance with Age changed, Name unchanged
```

### Record Equality

```csharp
Person p1 = new Person("Alice", 30);
Person p2 = new Person("Alice", 30);
Person p3 = new Person("Bob", 25);

// Value-based equality
bool same = p1 == p2;  // true (same values)
bool different = p1 == p3;  // false (different values)

// Structural equality - compares values, not reference
// Classes use reference equality by default
```

### Deconstruction

```csharp
Person person = new Person("Alice", 30);

// Deconstruct record
(string name, int age) = person;

// Pattern matching
if (person is Person { Name: "Alice", Age: > 25 }) {
    Console.WriteLine("Match!");
}
```

### Inheritance with Records

```csharp
public record Person(string Name, int Age);

public record Employee(string Name, int Age, string Department) 
    : Person(Name, Age);

// Usage
Employee emp = new Employee("Alice", 30, "Engineering");
Person person = emp;  // Covariance

// With keyword works with inheritance
Employee emp2 = emp with { Department = "Sales" };
```

---

## Tuples vs Records

| Feature | Tuple | Record |
|---------|-------|--------|
| Type | Value type | Reference type |
| Mutability | Immutable | Immutable (init) |
| Equality | Value-based | Value-based |
| Inheritance | No | Yes |
| Named members | Yes (C# 7+) | Yes |
| Purpose | Ad-hoc grouping | Data type |
| Performance | Lightweight | Slightly heavier |

---

## Real-World Examples

### Tuple for Return Values

```csharp
public (bool success, string message, int? result) TryCalculate(string input) {
    if (!int.TryParse(input, out int value)) {
        return (false, "Invalid input", null);
    }
    
    if (value < 0) {
        return (false, "Negative not allowed", null);
    }
    
    return (true, "Success", value * 2);
}

// Usage
(bool success, string msg, int? result) = TryCalculate("5");
if (success) {
    Console.WriteLine($"Result: {result}");
}
```

### Record for Data Transfer Object (DTO)

```csharp
public record PersonDto(
    int Id,
    string Name,
    string Email,
    DateTime CreatedDate
);

public record CreatePersonRequest(string Name, string Email);

// Usage
PersonDto dto = new PersonDto(1, "Alice", "alice@example.com", DateTime.Now);
CreatePersonRequest request = new("Bob", "bob@example.com");

// With keyword for modifications
PersonDto updated = dto with { Name = "Alice Updated" };
```

### Pattern Matching with Records

```csharp
public record Shape;
public record Circle(double Radius) : Shape;
public record Rectangle(double Width, double Height) : Shape;

public static double GetArea(Shape shape) {
    return shape switch {
        Circle c => Math.PI * c.Radius * c.Radius,
        Rectangle r => r.Width * r.Height,
        _ => 0
    };
}
```

---

## Best Practices

✓ **Use tuples for temporary grouping**
```csharp
(int count, double average) = CalculateStats(data);
```

✓ **Use records for data types**
```csharp
public record User(int Id, string Name, string Email);
```

✓ **Use with keyword for immutable updates**
```csharp
Person updated = person with { Age = 31 };
```

✓ **Leverage pattern matching**
```csharp
if (result is (true, _, int value)) {
    // Success
}
```

---

## Quick Summary

- Tuples group multiple values temporarily
- Records are immutable data types
- Value-based equality for both
- Records support inheritance
- with keyword for immutable copies
- Great for DTOs and pattern matching
- Improved readability and safety
