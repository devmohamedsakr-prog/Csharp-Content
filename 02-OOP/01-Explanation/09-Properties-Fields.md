# Properties vs Fields

## Overview
Fields are direct data members. Properties provide controlled access through getters and setters, enabling validation and computed values.

## Fields

### Basic Fields
```csharp
public class User
{
    // Public field - direct access
    public string Name;
    
    // Private field - encapsulation
    private int _age;
    
    // Protected field - accessible in derived classes
    protected string _email;
    
    // Internal field - accessible within assembly
    internal string _phone;
}

// Usage
var user = new User();
user.Name = "Alice"; // Direct access
Console.WriteLine(user.Name);
```

### Field Initialization
```csharp
public class Config
{
    // Field initializer
    private string _connectionString = "Server=localhost";
    
    // Readonly field - set once only
    private readonly DateTime _createdAt = DateTime.Now;
    
    // Can initialize in constructor
    public Config()
    {
        _createdAt = DateTime.Now; // OK
    }
    
    public void Change()
    {
        // _createdAt = DateTime.Now; // ERROR: readonly
    }
}
```

## Properties

### Auto-Properties
```csharp
public class Person
{
    // Auto-property: compiler generates backing field
    public string Name { get; set; }
    public int Age { get; set; }
    
    // With initializer (C# 6.0+)
    public string Email { get; set; } = "unknown@example.com";
    
    // Read-only auto-property
    public int Id { get; } // Only getter, no setter
    
    // Init-only property (C# 9.0+)
    public string Address { get; init; } // Can only set during initialization
}

// Usage
var person = new Person { Name = "Bob", Age = 30 };
Console.WriteLine(person.Name);
// person.Id = 5; // ERROR: no setter
```

### Properties with Backing Fields
```csharp
public class User
{
    private string _name; // Backing field
    
    // Property with validation
    public string Name
    {
        get { return _name; }
        set 
        { 
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Name cannot be empty");
            _name = value;
        }
    }
    
    // Expression-bodied property
    public string DisplayName => $"User: {_name}";
    
    // Different access levels
    public int Age { get; private set; } // Public get, private set
}

// Usage
var user = new User();
user.Name = "Alice"; // Validation occurs
// user.Age = 30; // ERROR: private setter
```

### Property Validation
```csharp
public class BankAccount
{
    private decimal _balance;
    
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
    
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit must be positive");
        Balance += amount; // Uses property setter for validation
    }
    
    public bool Withdraw(decimal amount)
    {
        if (amount > Balance)
            return false; // Insufficient funds
        Balance -= amount;
        return true;
    }
}
```

## Computed Properties

### Calculated Values
```csharp
public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    // Computed property
    public string FullName
    {
        get { return $"{FirstName} {LastName}"; }
    }
    
    // Expression-bodied
    public string Initials => $"{FirstName?[0]}.{LastName?[0]}.";
}

public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }
    
    // Computed properties
    public double Area => Width * Height;
    public double Perimeter => 2 * (Width + Height);
}

// Usage
var rect = new Rectangle { Width = 10, Height = 5 };
Console.WriteLine(rect.Area); // 50
Console.WriteLine(rect.Perimeter); // 30
```

## Lazy-Loaded Properties

### Deferred Initialization
```csharp
public class User
{
    private List<Order> _orders; // Backing field
    
    public List<Order> Orders
    {
        get
        {
            if (_orders == null)
            {
                _orders = LoadOrdersFromDatabase(); // Load on first access
            }
            return _orders;
        }
    }
    
    private List<Order> LoadOrdersFromDatabase()
    {
        // Expensive database operation
        return new List<Order> { /* ... */ };
    }
}

// Usage
var user = new User();
// Orders not loaded yet

var orders = user.Orders; // Loaded here on first access
var more = user.Orders; // Already loaded, returns cached value
```

## Init-Only Properties (C# 9.0+)

### Immutable Properties
```csharp
// Immutable record (modern approach)
public record PersonRecord(string Name, int Age);

// Class with init-only properties
public class PersonClass
{
    public string Name { get; init; }
    public int Age { get; init; }
}

// Usage
var person = new PersonClass { Name = "Alice", Age = 30 };
// person.Name = "Bob"; // ERROR: init-only, can't set after initialization

// With initializer method
var person2 = new PersonClass { Name = "Bob", Age = 25 };
var modified = person2 with { Age = 26 }; // Record feature
```

## Property Indexers

### Array-Like Access
```csharp
public class Dictionary
{
    private Dictionary<string, object> _values = new();
    
    // Property indexer
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
Console.WriteLine(dict["Name"]); // Alice

var grid = new Grid();
grid[0, 0] = 42;
Console.WriteLine(grid[0, 0]); // 42
```

## Best Practices

1. **Use Properties for Data Access**
```csharp
// Bad: Public fields
public class BadUser
{
    public string Name; // Direct access, no validation
    public int Age; // Can be set to negative
}

// Good: Properties with validation
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

2. **Use Auto-Properties When No Logic Needed**
```csharp
// Good: Clean, simple
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

// Overcomplicated: Unnecessary backing field
public class BadPoint
{
    private int _x;
    public int X
    {
        get { return _x; }
        set { _x = value; }
    }
}
```

3. **Use Expression-Bodied for Computed Properties**
```csharp
// Good: Clear and concise
public double Area => Width * Height;

// Unnecessary verbosity
public double Area
{
    get { return Width * Height; }
}
```

4. **Use Init-Only for Immutability**
```csharp
// Good: Cannot modify after creation
public record User
{
    public string Name { get; init; }
    public int Id { get; init; }
}

// Bad: Can be modified unexpectedly
public class MutableUser
{
    public string Name { get; set; }
    public int Id { get; set; }
}
```

## Common Mistakes

1. **Public Fields Instead of Properties**
```csharp
// Bad: Can't add validation later
public class BadClass
{
    public int Value; // Public field, can be anything
}

// Good: Can add validation
public class GoodClass
{
    private int _value;
    public int Value
    {
        get { return _value; }
        set { _value = Math.Max(0, value); } // Can validate
    }
}
```

2. **Forgetting Backing Field**
```csharp
// Bad: Infinite recursion
public class BadProperty
{
    public int Value
    {
        get { return Value; } // Calls itself!
        set { Value = value; } // Calls itself!
    }
}

// Good: Use backing field
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

3. **Heavy Computation in Property Getter**
```csharp
// Bad: Performance issue if accessed repeatedly
public int ExpensiveCalculation
{
    get { return PerformSlowOperation(); }
}

// Good: Cache or reconsider design
private int _cachedValue;
private bool _cached = false;

public int CachedCalculation
{
    get
    {
        if (!_cached)
        {
            _cachedValue = PerformSlowOperation();
            _cached = true;
        }
        return _cachedValue;
    }
}
```

## Quick Summary
- Fields: Direct data members, simple storage
- Properties: Controlled access with validation
- Auto-properties: Compiler-generated backing field
- Computed properties: Calculated on access
- Indexers: Array-like access syntax
- Init-only: Immutable after initialization
- Use properties for encapsulation
- Validate in property setters
- Use expression bodies for computed properties
- Avoid public fields for encapsulation

## Resources
- Properties (C# documentation)
- Backing Fields and Auto-Properties
- Init-Only Properties (C# 9.0)
- Indexers
