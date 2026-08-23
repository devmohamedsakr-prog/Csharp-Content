# Attribute Snippets

Custom attributes and common built-in attributes.

## Attribute Declaration

**Pattern:**
```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAttribute : Attribute
{
    public CustomAttribute(string name)
    {
        Name = name;
    }
    
    public string Name { get; set; }
    public string Description { get; set; }
}
```

---

## Obsolete - Mark as Deprecated

**Pattern:**
```csharp
[Obsolete("Use NewMethod instead")]
public void OldMethod()
{
    // Old implementation
}

[Obsolete("This method is deprecated", true)]  // true = error
public void DeprecatedMethod()
{
}
```

**Usage:**
```csharp
// Warning when used
OldMethod();

// Compiler error when used
// DeprecatedMethod();
```

---

## Serializable - Mark for Serialization

**Pattern:**
```csharp
[Serializable]
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
```

---

## NonSerialized - Exclude from Serialization

**Pattern:**
```csharp
[Serializable]
public class User
{
    public string Name { get; set; }
    
    [NonSerialized]
    public string Password;  // Won't be serialized
}
```

---

## DataContract & DataMember (WCF/Serialization)

**Pattern:**
```csharp
[DataContract]
public class Employee
{
    [DataMember]
    public int Id { get; set; }
    
    [DataMember]
    public string Name { get; set; }
    
    // Not serialized
    public string InternalId { get; set; }
}
```

---

## JsonPropertyName (System.Text.Json)

**Pattern:**
```csharp
public class ApiResponse
{
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }
    
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }
}
```

**Example:**
```json
{
  "user_id": 123,
  "user_name": "John"
}
```

---

## Required (C# 11+)

**Pattern:**
```csharp
public class Product
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    
    public string Description { get; set; }
}
```

---

## Range & StringLength (Validation)

**Pattern:**
```csharp
public class Product
{
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }
    
    [Range(0, 1000)]
    public decimal Price { get; set; }
    
    [Range(18, 120)]
    public int Age { get; set; }
}
```

---

## EmailAddress & Phone (Validation)

**Pattern:**
```csharp
public class Contact
{
    [EmailAddress]
    public string Email { get; set; }
    
    [Phone]
    public string PhoneNumber { get; set; }
}
```

---

## Table & Column (Entity Framework)

**Pattern:**
```csharp
[Table("tbl_Users")]
public class User
{
    [Key]
    [Column("user_id")]
    public int Id { get; set; }
    
    [Column("user_name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }
    
    [NotMapped]
    public string FullName { get; set; }
}
```

---

## DatabaseGenerated (EF)

**Pattern:**
```csharp
public class Order
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal Total { get; set; }
}
```

---

## ForeignKey & InverseProperty (EF)

**Pattern:**
```csharp
public class Order
{
    public int Id { get; set; }
    
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
    public int CustomerId { get; set; }
}

public class Comment
{
    public int Id { get; set; }
    
    [InverseProperty("Comments")]
    public Post Post { get; set; }
}
```

---

## Authorize (ASP.NET Core)

**Pattern:**
```csharp
[Authorize]
public class AdminController : Controller
{
    [AllowAnonymous]
    public IActionResult Login() => View();
    
    [Authorize(Roles = "Admin")]
    public IActionResult Dashboard() => View();
}
```

---

## Route (ASP.NET Core)

**Pattern:**
```csharp
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<Product> GetProduct(int id) { }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto) { }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id) { }
}
```

---

## Test Attributes

**XUnit:**
```csharp
[Fact]
public void TestMethod()
{
    Assert.True(true);
}

[Theory]
[InlineData(1, 2, 3)]
[InlineData(0, 0, 0)]
public void TestWithData(int a, int b, int expected)
{
}
```

**NUnit:**
```csharp
[Test]
public void TestMethod()
{
    Assert.IsTrue(true);
}

[TestCase(1, 2, 3)]
[TestCase(0, 0, 0)]
public void TestWithData(int a, int b, int expected)
{
}
```

---

## Conditional Compilation

**Pattern:**
```csharp
[Conditional("DEBUG")]
public void DebugOnly()
{
    Console.WriteLine("Debug message");
}

public void Main()
{
    DebugOnly();  // Only called in Debug mode
}
```

---

## Custom Attribute Usage

**Pattern:**
```csharp
// Define attribute
[AttributeUsage(AttributeTargets.Method)]
public class ValidationAttribute : Attribute
{
    public string Rule { get; set; }
}

// Apply attribute
public class DataProcessor
{
    [Validation(Rule = "required")]
    public void ProcessData(string data) { }
}

// Read attribute
var method = typeof(DataProcessor).GetMethod("ProcessData");
var attr = method.GetCustomAttribute<ValidationAttribute>();
if (attr != null)
{
    Console.WriteLine($"Rule: {attr.Rule}");
}
```

---

## ParamArray (Old Style, use params instead)

**Pattern:**
```csharp
public void PrintValues(params string[] values)
{
    foreach (var v in values)
        Console.WriteLine(v);
}
```

---

## Quick Reference

| Attribute | Purpose | Target |
|-----------|---------|--------|
| `Obsolete` | Mark deprecated | Method/Type |
| `Serializable` | Enable serialization | Class |
| `NonSerialized` | Exclude from serialization | Field |
| `Required` | Value required | Property |
| `Table` | Database table mapping | Class |
| `Column` | Database column mapping | Property |
| `ForeignKey` | Related entity | Property |
| `Key` | Primary key | Property |
| `Authorize` | Authentication required | Method/Class |
| `Route` | HTTP endpoint | Controller/Method |
| `HttpGet/Post` | HTTP verb | Method |
| `Fact` (XUnit) | Test method | Method |
| `Test` (NUnit) | Test method | Method |

---

## Best Practices

- Use built-in attributes when available
- Create custom attributes only when needed
- Use meaningful attribute names (suffix with "Attribute")
- Document attribute usage clearly
- Validate attribute parameters
- Consider performance of attribute reflection
- Use conditional compilation for debug-only code
- Apply attributes consistently across codebase

