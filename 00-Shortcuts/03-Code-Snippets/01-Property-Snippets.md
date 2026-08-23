# Property Snippets

Generate properties quickly with built-in snippets.

## prop - Auto-Property

**Shortcut:** `prop` + Tab

**Generates:**
```csharp
public int MyProperty { get; set; }
```

**Placeholders:**
- Type: `int` (change this)
- Name: `MyProperty` (change this)

**Usage:**
```csharp
public class Person
{
    prop → Tab
    // Now you have: public int MyProperty { get; set; }
    // Edit the type and name as needed
}
```

**Examples:**
```csharp
public string Name { get; set; }
public int Age { get; set; }
public DateTime CreatedDate { get; set; }
public bool IsActive { get; set; }
```

---

## propfull - Full Property with Backing Field

**Shortcut:** `propfull` + Tab

**Generates:**
```csharp
private int myVar;

public int MyProperty
{
    get { return myVar; }
    set { myVar = value; }
}
```

**Placeholders:**
- Type: `int` (change this)
- Backing field: `myVar` (change this)
- Property name: `MyProperty` (change this)

**Usage:**
```csharp
propfull → Tab
// Creates property with private backing field
// Great for properties with validation or logic
```

**Examples:**
```csharp
private string _email;

public string Email
{
    get { return _email; }
    set { _email = value?.ToLower() ?? string.Empty; }
}

private int _age;

public int Age
{
    get { return _age; }
    set { _age = value > 0 ? value : 0; }
}
```

---

## propg - Get-Only Property

**Shortcut:** `propg` + Tab

**Generates:**
```csharp
public int MyProperty { get; }
```

**Usage:**
```csharp
propg → Tab
// Read-only property
// Can only be set in constructor or initializer
```

**Example:**
```csharp
public string Id { get; } = Guid.NewGuid().ToString();
public DateTime CreatedAt { get; } = DateTime.Now;
public string Description { get; }

public MyClass(string description)
{
    Description = description; // Can set in constructor
}
```

---

## Quick Reference

| Snippet | Usage | Access Level |
|---------|-------|--------------|
| `prop` | Auto-property | public |
| `propfull` | With backing field | public with private field |
| `propg` | Read-only | public get only |

---

## Tips

- **After Tab:** Use **Tab** to move between placeholders
- **After Shift+Tab:** Move backwards between placeholders
- **Escape:** Exit snippet mode
- **Shortcut again:** Type another snippet on same line

## Custom Properties with Validation

While snippets generate basic properties, you can manually extend them:

```csharp
private string _email;

public string Email
{
    get { return _email; }
    set { _email = ValidateEmail(value) ? value : throw new ArgumentException("Invalid email"); }
}

private string ValidateEmail(string email) 
    => !string.IsNullOrWhiteSpace(email) && email.Contains("@");
```

