# Attributes

## Overview
Attributes add metadata to code elements that can be inspected at runtime.

---

## Predefined Attributes

```csharp
// Obsolete - marks as deprecated
[Obsolete("Use NewMethod instead")]
public void OldMethod() { }

// Serializable - can be serialized
[Serializable]
public class Data { }

// NonSerialized - exclude from serialization
[NonSerialized]
private string password;

// Conditional - call only if DEBUG
[Conditional("DEBUG")]
public void DebugLog(string message) { }
```

---

## Creating Custom Attributes

```csharp
// Define attribute
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorAttribute : Attribute {
    public string Name { get; set; }
    public DateTime Date { get; set; }
    
    public AuthorAttribute(string name) {
        Name = name;
        Date = DateTime.Now;
    }
}

// Apply attribute
[Author("John Doe")]
public class MyClass {
    [Author("Jane Smith")]
    public void MyMethod() { }
}
```

---

## AttributeUsage

Controls where attribute can be applied.

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class ClassOnlyAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class MethodOnlyAttribute : Attribute { }

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method,
    AllowMultiple = true,
    Inherited = true
)]
public class MultipleAttribute : Attribute { }
```

---

## Reading Attributes

```csharp
[Author("Alice")]
public class Person { }

Type type = typeof(Person);

// Get attribute
AuthorAttribute attr = (AuthorAttribute)Attribute.GetCustomAttribute(
    type,
    typeof(AuthorAttribute)
);

if (attr != null) {
    Console.WriteLine($"Author: {attr.Name}");
}

// Get all attributes
object[] attributes = type.GetCustomAttributes();
foreach (var attribute in attributes) {
    Console.WriteLine($"Attribute: {attribute.GetType().Name}");
}

// Check if has attribute
bool hasAuthor = Attribute.IsDefined(type, typeof(AuthorAttribute));
```

---

## Validation Attributes

```csharp
// Custom validation attribute
[AttributeUsage(AttributeTargets.Property)]
public class RangeAttribute : Attribute {
    public int Min { get; set; }
    public int Max { get; set; }
    
    public RangeAttribute(int min, int max) {
        Min = min;
        Max = max;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute { }

// Use in class
public class User {
    [Required]
    public string Name { get; set; }
    
    [Range(0, 150)]
    public int Age { get; set; }
}

// Validator
public class Validator {
    public static bool Validate(object obj) {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();
        
        foreach (PropertyInfo prop in properties) {
            // Check Required
            if (Attribute.IsDefined(prop, typeof(RequiredAttribute))) {
                object value = prop.GetValue(obj);
                if (value == null || (value is string && string.IsNullOrEmpty((string)value))) {
                    return false;
                }
            }
            
            // Check Range
            RangeAttribute range = (RangeAttribute)Attribute.GetCustomAttribute(
                prop,
                typeof(RangeAttribute)
            );
            
            if (range != null && prop.PropertyType == typeof(int)) {
                int value = (int)prop.GetValue(obj);
                if (value < range.Min || value > range.Max) {
                    return false;
                }
            }
        }
        
        return true;
    }
}

// Usage
User user = new User { Name = "Alice", Age = 30 };
bool valid = Validator.Validate(user);  // true
```

---

## Framework Integration

Most frameworks use attributes extensively:

```csharp
// Entity Framework
[Table("Users")]
public class User {
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [Column("CreatedDate")]
    public DateTime Created { get; set; }
}

// ASP.NET Core
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id) { }
    
    [HttpPost]
    public ActionResult CreateUser([FromBody] User user) { }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id) { }
}
```

---

## Quick Summary

- Attributes add metadata to code
- Custom attributes for your own metadata
- AttributeUsage controls where attribute can be applied
- Inspected using reflection
- Used by frameworks for configuration
- Powerful for validation and decoration
