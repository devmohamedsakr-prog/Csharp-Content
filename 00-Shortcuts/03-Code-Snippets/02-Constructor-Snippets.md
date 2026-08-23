# Constructor Snippets

Generate constructors quickly with built-in snippets.

## ctor - Constructor

**Shortcut:** `ctor` + Tab

**Generates:**
```csharp
public ClassName()
{
}
```

**Placeholders:**
- Class name is auto-filled

**Usage:**
```csharp
public class Person
{
    ctor → Tab
    // Creates constructor for Person class
}
```

**Examples:**
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public User()
    {
    }
}

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    public Product()
    {
        Name = "Unknown";
        Price = 0;
    }
}
```

---

## ctor with Parameters

**Manual:** Create constructor with parameters

**Pattern:**
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}
```

**Usage:**
```csharp
var person = new Person("John", 30);
```

---

## ctorbase - Constructor with Base Call

**Shortcut:** `ctorbase` + Tab

**Generates:**
```csharp
public ClassName(params) : base(params)
{
}
```

**Usage:**
```csharp
public class Employee : Person
{
    ctorbase → Tab
    // Creates constructor that calls base class constructor
}
```

**Example:**
```csharp
public class Animal
{
    public string Name { get; set; }
    
    public Animal(string name)
    {
        Name = name;
    }
}

public class Dog : Animal
{
    public string Breed { get; set; }
    
    public Dog(string name, string breed) : base(name)
    {
        Breed = breed;
    }
}

// Usage
var dog = new Dog("Buddy", "Golden Retriever");
```

---

## Parameterized Constructor

**Pattern:**
```csharp
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public User(int id, string email, string password)
    {
        Id = id;
        Email = email;
        Password = password;
    }
}
```

**Usage:**
```csharp
var user = new User(1, "john@example.com", "password");
```

---

## Multiple Constructors

**Pattern:**
```csharp
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    // Default constructor
    public Product()
    {
    }
    
    // Constructor with name only
    public Product(string name)
    {
        Name = name;
    }
    
    // Constructor with all properties
    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

// Usage
var p1 = new Product();
var p2 = new Product("Laptop");
var p3 = new Product("Laptop", 999.99m);
```

---

## Constructor Chaining

**Pattern:**
```csharp
public class Rectangle
{
    public int Width { get; set; }
    public int Height { get; set; }
    
    public Rectangle() : this(0, 0)
    {
    }
    
    public Rectangle(int size) : this(size, size)
    {
    }
    
    public Rectangle(int width, int height)
    {
        Width = width;
        Height = height;
    }
}

// Usage
var r1 = new Rectangle();           // 0x0
var r2 = new Rectangle(5);          // 5x5
var r3 = new Rectangle(5, 10);      // 5x10
```

---

## Tips

- **Auto-complete:** VS shows constructor templates as you type
- **Constructor overloading:** Multiple constructors with different parameters
- **Constructor chaining:** Use `:this()` or `:base()` to call other constructors
- **Initialization:** Set properties in constructor
- **Validation:** Add logic to validate constructor parameters

