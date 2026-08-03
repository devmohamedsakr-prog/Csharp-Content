# Properties and Fields - Data Management

## Overview

Fields and properties are how objects store and manage data. Fields provide direct data storage, while properties provide controlled access through getters and setters. Properties are the preferred approach in modern C#.

## Fields - Direct Data Storage

### Basic Fields

Fields store data directly in objects:

```csharp
public class User
{
    // Public field - direct access (avoid)
    public string Name;
    
    // Private field - internal only
    private int _age;
    
    // Protected field - accessible in derived classes
    protected string _email;
    
    // Internal field - accessible within same assembly
    internal string _phone;
}

// Usage
var user = new User();
user.Name = "Alice";  // Direct access
Console.WriteLine(user.Name);
```

### Field Initialization

Initialize fields at declaration or in constructor:

```csharp
public class Config
{
    // Initialize at declaration
    private string _connectionString = "Server=localhost";
    
    // Readonly field - set once only
    private readonly DateTime _createdAt = DateTime.Now;
    
    // Can override in constructor
    public Config()
    {
        _createdAt = DateTime.Now;  // OK - readonly allows in constructor
    }
    
    public void Change()
    {
        // _createdAt = DateTime.Now;  // ERROR: readonly, can't change
    }
}
```

### Readonly Fields

Fields that can only be set once:

```csharp
public class Product
{
    // Readonly field - set at declaration or in constructor
    public readonly int Id;
    public readonly string Name;
    
    public Product(int id, string name)
    {
        Id = id;  // OK - setting readonly in constructor
        Name = name;
    }
    
    public void UpdateName(string newName)
    {
        // Name = newName;  // ERROR: readonly field cannot be modified
    }
}
```

## Why Avoid Public Fields?

Public fields have no protection:

```csharp
// BAD - Public field
public class BadClass
{
    public int Value;  // Can be any value
}

var obj = new BadClass();
obj.Value = -1000;  // No validation possible!
obj.Value = int.MaxValue;  // Dangerous values allowed
```

**Problems with public fields:**
- No validation possible
- No logging or side effects
- Breaking changes if you add validation later
- Violates encapsulation principle

## Properties - Controlled Data Access

### Auto-Properties (Recommended)

Compiler generates backing field automatically:

```csharp
public class Person
{
    // Auto-property: simple and clean
    public string Name { get; set; }
    public int Age { get; set; }
    
    // With initializer (C# 6.0+)
    public string Email { get; set; } = "unknown@example.com";
    
    // Read-only auto-property (getter only)
    public int Id { get; }
    
    // Required property (must be set during init - C# 11.0+)
    public required string Department { get; set; }
}

// Usage
var person = new Person 
{ 
    Name = "Bob", 
    Age = 30,
    Department = "Sales"
};
Console.WriteLine(person.Name);
```

### Properties with Backing Fields

Full control over getter and setter logic:

```csharp
public class BankAccount
{
    private decimal _balance;  // Backing field
    
    // Property with validation in setter
    public decimal Balance
    {
        get { return _balance; }
        set 
        { 
            if (value < 0)
                throw new ArgumentException("Balance cannot be negative");
            _balance = value;
        }
    }
    
    // Expression-bodied property (read-only)
    public decimal FormattedBalance => Math.Round(_balance, 2);
    
    // Private setter (read-only from outside)
    public DateTime LastUpdated { get; private set; } = DateTime.Now;
}

// Usage
var account = new BankAccount();
account.Balance = 1000;  // Uses setter with validation
account.Balance = -100;  // ERROR: validation throws
```

### Read-Only Properties

Properties with only a getter:

```csharp
public class Employee
{
    private string _name;
    
    // Read-only property
    public string Name
    {
        get { return _name; }
    }
    
    // Alternative: auto-property with no setter
    public int EmployeeId { get; }
    
    // Expression-bodied read-only
    public string FullInfo => $"{Name} (ID: {EmployeeId})";
    
    public Employee(string name, int id)
    {
        _name = name;
        EmployeeId = id;
    }
}

// Usage
var emp = new Employee("Alice", 123);
Console.WriteLine(emp.Name);  // OK
// emp.Name = "Bob";  // ERROR - no setter
```

### Init-Only Properties (C# 9.0+)

Set during initialization, then read-only:

```csharp
public class User
{
    // Init-only property - set during init, then read-only
    public string Name { get; init; }
    public int Age { get; init; }
    public string Email { get; init; }
}

// Usage - initialization
var user = new User 
{ 
    Name = "Alice", 
    Age = 30, 
    Email = "alice@example.com" 
};

// user.Name = "Bob";  // ERROR: init-only, can't modify after init
```

## Computed Properties

Calculate values on-the-fly:

```csharp
public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }
    
    // Computed property - calculated each time
    public double Area
    {
        get { return Width * Height; }
    }
    
    // Expression-bodied property (concise)
    public double Perimeter => 2 * (Width + Height);
    
    // With validation
    public double Aspect => Height > 0 ? Width / Height : 0;
}

// Usage
var rect = new Rectangle { Width = 10, Height = 5 };
Console.WriteLine(rect.Area);      // 50 (calculated)
Console.WriteLine(rect.Perimeter); // 30 (calculated)
```

## Lazy-Loaded Properties

Defer expensive operations until first access:

```csharp
public class User
{
    private List<Order> _orders;  // Backing field
    
    // Lazy-loaded property
    public List<Order> Orders
    {
        get
        {
            if (_orders == null)
            {
                _orders = LoadOrdersFromDatabase();  // Expensive operation
            }
            return _orders;
        }
    }
    
    private List<Order> LoadOrdersFromDatabase()
    {
        // Expensive database query
        return new List<Order> { /* ... */ };
    }
}

// Usage
var user = new User();
// Orders not loaded yet

var orders = user.Orders;  // Loaded here on first access
var more = user.Orders;    // Returns cached value, no reload
```

## Property Validation

Ensure data integrity:

```csharp
public class Employee
{
    private string _name;
    private int _age;
    private decimal _salary;
    
    public string Name
    {
        get { return _name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty");
            _name = value.Trim();
        }
    }
    
    public int Age
    {
        get { return _age; }
        set
        {
            if (value < 0 || value > 150)
                throw new ArgumentException("Age must be 0-150");
            _age = value;
        }
    }
    
    public decimal Salary
    {
        get { return _salary; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Salary cannot be negative");
            _salary = value;
        }
    }
}

// Usage
var emp = new Employee();
emp.Name = "Alice";      // OK
emp.Name = "";           // ERROR: validation
emp.Age = 30;            // OK
emp.Age = 200;           // ERROR: validation
emp.Salary = 50000;      // OK
emp.Salary = -1000;      // ERROR: validation
```

## Indexed Properties (Indexers)

Access objects like arrays:

```csharp
public class Dictionary
{
    private Dictionary<string, object> _values = new();
    
    // Indexer - access like dict["key"]
    public object this[string key]
    {
        get { return _values.ContainsKey(key) ? _values[key] : null; }
        set { _values[key] = value; }
    }
}

public class Grid
{
    private int[,] _data = new int[10, 10];
    
    // Multi-dimensional indexer
    public int this[int row, int col]
    {
        get { return _data[row, col]; }
        set { _data[row, col] = value; }
    }
}

// Usage
var dict = new Dictionary();
dict["Name"] = "Alice";
Console.WriteLine(dict["Name"]);  // Alice

var grid = new Grid();
grid[0, 0] = 42;
Console.WriteLine(grid[0, 0]);    // 42
```

## Best Practices

### 1. Use Properties Instead of Public Fields

```csharp
// BAD - Public field
public class BadUser
{
    public string Name;  // No validation
    public int Age;      // Can be negative!
}

// GOOD - Properties
public class GoodUser
{
    private int _age;
    
    public string Name { get; set; }
    
    public int Age
    {
        get { return _age; }
        set 
        { 
            if (value < 0)
                throw new ArgumentException("Age cannot be negative");
            _age = value;
        }
    }
}
```

### 2. Use Auto-Properties When No Logic Needed

```csharp
// GOOD - Auto-property, clean and simple
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

// BAD - Unnecessary backing field
public class BadPoint
{
    private int _x;
    public int X
    {
        get { return _x; }
        set { _x = value; }  // No logic, use auto-property
    }
}
```

### 3. Use Expression-Bodied for Computed Properties

```csharp
// GOOD - Concise and clear
public class Circle
{
    public double Radius { get; set; }
    public double Area => Math.PI * Radius * Radius;
}

// Less ideal - verbose
public class BadCircle
{
    public double Radius { get; set; }
    public double Area
    {
        get { return Math.PI * Radius * Radius; }
    }
}
```

### 4. Use Readonly or Init-Only for Immutability

```csharp
// GOOD - Immutable after creation
public class UserId
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; }
}

// LESS GOOD - Can be modified
public class MutableId
{
    public int Id { get; set; }  // Can change
    public DateTime CreatedAt { get; set; }  // Can change
}
```

## Common Mistakes

### Mistake 1: Infinite Recursion in Properties

```csharp
// BAD - Infinite recursion
public class BadProperty
{
    public int Value
    {
        get { return Value; }  // Calls itself!
        set { Value = value; } // Calls itself!
    }
}

// GOOD - Use backing field
public class GoodProperty
{
    private int _value;
    
    public int Value
    {
        get { return _value; }
        set { _value = value; }
    }
}
```

### Mistake 2: Heavy Computation in Getter

```csharp
// BAD - Expensive operation every access
public class BadPerformance
{
    public List<User> AllUsers
    {
        get { return LoadUsersFromDatabase(); }  // Slow!
    }
}

// GOOD - Cache the result
public class GoodPerformance
{
    private List<User> _cachedUsers;
    private bool _cached = false;
    
    public List<User> AllUsers
    {
        get
        {
            if (!_cached)
            {
                _cachedUsers = LoadUsersFromDatabase();
                _cached = true;
            }
            return _cachedUsers;
        }
    }
}
```

### Mistake 3: Public Mutable Properties

```csharp
// BAD - Can be modified unexpectedly
public class BadDesign
{
    public List<string> Items { get; set; } = new();
}

var obj = new BadDesign();
obj.Items.Add("item");
obj.Items = null;  // Can set to null!

// BETTER - Read-only collection
public class BetterDesign
{
    public IReadOnlyList<string> Items { get; }
    
    private List<string> _items = new();
    
    public BetterDesign()
    {
        Items = _items.AsReadOnly();
    }
}
```

## Summary

- **Fields** - Direct storage (generally discouraged)
- **Properties** - Controlled access (preferred)
- **Auto-properties** - Compiler-generated backing field
- **Backing field** - Manual storage for complex logic
- **Read-only** - No setter, immutable after creation
- **Init-only** - Set during initialization, then read-only
- **Computed** - Calculated on access
- **Lazy-loaded** - Deferred initialization
- **Validation** - Ensure data integrity
- **Indexers** - Array-like access

## Next Steps

- Learn [Inheritance](../../02-Inheritance-Polymorphism/01-Inheritance/00-Inheritance.md) for property inheritance
- Study [Encapsulation](../../03-Advanced-OOP/03-Encapsulation/00-Encapsulation.md) for data protection
- Review [Access-Modifiers](../../03-Advanced-OOP/05-Access-Modifiers/00-Access-Modifiers.md) for visibility control
