# What is Null?

## Overview
Null represents "no value," "undefined," or "absence of data." Understanding null is critical for safe C# programming.

---

## Null Concept

### Definition
Null indicates that a variable has no value or refers to nothing. It's different from zero, empty string, or false.

```csharp
// Not null - has value 0
int zero = 0;

// Not null - has value empty string
string empty = "";

// Not null - has value false
bool flag = false;

// Null - has no value
string? nullRef = null;
int? nullValue = null;
```

### Reference Types vs Value Types

```csharp
// Reference types (nullable by default)
string name = null;  // Can be null
List<int> list = null;  // Can be null
object obj = null;  // Can be null

// Value types (cannot be null without ?)
int age = null;  // ERROR - compiler won't allow

// With nullable syntax
int? age = null;  // OK - can be null
double? price = null;  // OK - can be null
bool? flag = null;  // OK - can be null
```

---

## Null in Different Contexts

### Database NULL
Database NULL represents missing data in a column.

```csharp
// Result from database query might be null
int? productId = GetProductIdFromDatabase();

// Age column might not have value for all records
int? age = GetAgeFromDatabase();  // Could be null
```

### API/Web Responses
JSON APIs often have optional fields.

```csharp
// JSON: { "name": "Alice", "phone": null }
public class User {
    public string Name { get; set; }
    public string? Phone { get; set; }  // Optional field
}
```

### Optional Parameters
Method parameters might be optional.

```csharp
// Email is optional (can be null)
public void CreateUser(string name, string? email = null) {
    // Handle null email
}
```

---

## Why Null Matters

### The Billion-Dollar Mistake
Null references cause many runtime errors.

```csharp
// DANGEROUS - Common crash
string text = null;
int length = text.Length;  // NullReferenceException at runtime!

// The problem:
// - No compile-time warning
// - Crashes at runtime
// - Hard to debug in production
```

### Safe Approach
Modern C# helps prevent null reference errors.

```csharp
// Explicit null handling
string? text = GetText();

// Safe access - won't crash
int? length = text?.Length;  // null if text is null

// Or check before use
if (text != null) {
    int length = text.Length;  // Safe
}
```

---

## Null vs Default Values

### Null is Different
```csharp
// Different from defaults
int zero = 0;          // Default int value, NOT null
string empty = "";     // Empty string, NOT null
bool false_val = false; // Default bool, NOT null

// These are DIFFERENT from null
int? nullInt = null;   // Null, not 0
string? nullStr = null; // Null, not empty string
```

### Default Values for Types
```csharp
// Nullable type without value
int? age = null;

// Get default for type
int defaultAge = age.GetValueOrDefault();  // 0

// Custom default
int customDefault = age.GetValueOrDefault(18);  // 18
```

---

## When to Expect Null

### Common Scenarios
1. **Uninitialized reference types**
   ```csharp
   string name = null;  // Intentionally null
   ```

2. **Database missing values**
   ```csharp
   int? salary = db.Employee.Salary;  // Could be null
   ```

3. **API optional fields**
   ```csharp
   string? middleName = response?.MiddleName;  // Could be null
   ```

4. **Method returns null on error**
   ```csharp
   User? user = FindUser(id);  // Returns null if not found
   ```

5. **No value available**
   ```csharp
   DateTime? birthDate = null;  // Not provided
   ```

---

## Nullable Reference Types (C# 8+)

### Explicit Null Safety
```csharp
#nullable enable

// Non-nullable - must have value
string name = "Alice";

// Nullable - can be null
string? optionalName = null;

// Compiler warns about potential null issues
int length = optionalName.Length;  // WARNING: might be null!
```

### Nullable Context
```csharp
#nullable enable

// Non-nullable types (cannot be null)
class User {
    public string Name { get; set; }  // Cannot be null
    public int Age { get; set; }
}

// Nullable types (can be null)
class UserOptional {
    public string? MiddleName { get; set; }  // Can be null
    public string? Phone { get; set; }  // Can be null
}
```

---

## Null Checking Intro

### Basic Check
```csharp
string? name = GetName();

// Check if null
if (name == null) {
    Console.WriteLine("Name is null");
}

// Check if not null
if (name != null) {
    Console.WriteLine("Name is: " + name);
}
```

### Pattern Matching
```csharp
string? name = GetName();

// Modern C# syntax
if (name is null) {
    Console.WriteLine("Null");
}

if (name is not null) {
    Console.WriteLine("Has value: " + name);
}
```

---

## Summary

✓ Null represents "no value"
✓ Reference types nullable by default
✓ Value types need `?` to be nullable
✓ Null is different from default values
✓ NullReferenceException is common crash
✓ Modern C# helps prevent null issues
✓ Always consider null possibilities
✓ Handle null explicitly and safely

---

## Next Steps

1. Study Nullable Value Types
2. Learn Null Checking Methods
3. Master Safe Access Patterns
