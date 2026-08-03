# Null-Conditional Operator (?. and ?[])

## Overview
The `?.` operator safely accesses members of potentially null objects. Returns null if the object is null, preventing NullReferenceException.

---

## Basic Syntax

### Member Access
```csharp
string? text = null;

// Without null-conditional - crashes!
// int length = text.Length;  // NullReferenceException

// With null-conditional - safe
int? length = text?.Length;  // null (no error)

// With value
string? text2 = "Hello";
int? length2 = text2?.Length;  // 5
```

### Method Calls
```csharp
string? text = null;

// Without - crashes
// string upper = text.ToUpper();  // NullReferenceException

// With - safe
string? upper = text?.ToUpper();  // null (no error)

// With value
string? text2 = "hello";
string? upper2 = text2?.ToUpper();  // "HELLO"
```

---

## Chaining Null-Conditional

### Property Chains
```csharp
// Safe navigation through chain
string? firstName = person?.Address?.City;  // null if any step is null

// Without null-conditional - dangerous
// string city = person.Address.City;  // Crashes if person or Address is null

// Each ?. checks for null:
// If person is null -> result is null
// If Address is null -> result is null
// If City is null -> result is null
// Otherwise -> result is city value
```

### Method and Property Mix
```csharp
string? initial = person?.GetName()?.Substring(0, 1);  // Safe chain

Order? order = customer?.GetOrder()?.Items?.FirstOrDefault()?.Name;
```

---

## Array/Indexer Access

### Array with Null-Conditional
```csharp
int[]? arr = GetArray();

// Safe array access
int? firstElement = arr?[0];  // null if arr is null

// With value
int[]? arr2 = new int[] { 1, 2, 3 };
int? first = arr2?[0];  // 1
```

### Collection Access
```csharp
List<string>? items = GetItems();

// Safe collection access
string? first = items?[0];  // null if items is null

// Safe with conditional operator
items?[0] = "new value";  // Only sets if items is not null
```

---

## Combining with Null-Coalescing

### Fallback Pattern
```csharp
// Get value or use default
string? email = user?.Email ?? "no-email@example.com";

// Chain with fallback
int? age = person?.Age ?? 0;

// Multiple fallbacks
string contact = 
    user?.PrimaryPhone ?? 
    user?.SecondaryPhone ?? 
    "No contact";
```

---

## Practical Examples

### Safe Property Access
```csharp
class Address {
    public string? Street { get; set; }
    public string? City { get; set; }
}

class Person {
    public string Name { get; set; }
    public Address? Address { get; set; }
}

Person? person = GetPerson();

// Safe access through chain
string? city = person?.Address?.City ?? "Unknown";
```

### Safe Method Results
```csharp
// GetUser might return null
User? user = GetUser(id);

// Safe access to method results
int? length = user?.GetFullName()?.Length;

// Use with default
string name = user?.GetFullName() ?? "Unknown";
```

### Safe Collection Access
```csharp
List<Order>? orders = customer?.Orders;

// Safe indexing
Order? firstOrder = orders?[0];

// Safe LINQ
int? count = orders?.Count;
```

### Safe Event Invocation
```csharp
public delegate void OnChanged(string value);
public event OnChanged? Changed;

// Safe event raising
Changed?.Invoke("New value");  // Only raises if Changed is not null
```

---

## When Null-Conditional Returns Null

```csharp
string? text = null;

// All of these return null
int? len = text?.Length;           // null
string? upper = text?.ToUpper();   // null
char? first = text?[0];             // null

// No exceptions thrown, no errors logged
```

---

## Null-Conditional in Conditions

### Direct Conditions
```csharp
string? text = GetText();

// Safe property access in condition
if (text?.Length > 5) {
    // Only executes if text is not null AND length > 5
}

// Comparing with null
if (text?.Length is null) {
    Console.WriteLine("Text is null");
}
```

### With Pattern Matching
```csharp
Person? person = GetPerson();

// Pattern match with null-conditional
string result = person?.GetAge() switch {
    null => "Unknown age",
    0 => "Invalid age",
    < 18 => "Minor",
    _ => "Adult"
};
```

---

## Common Mistakes

### ❌ Forgetting the ?
```csharp
string? text = null;
int length = text.Length;  // NullReferenceException!
```

✓ **Use ?. for safety:**
```csharp
int? length = text?.Length;  // null, no error
```

### ❌ Assuming Not Null
```csharp
Person? person = GetPerson();
string name = person.Name;  // Might crash!
```

✓ **Use safe access:**
```csharp
string? name = person?.Name;
string display = name ?? "Unknown";
```

---

## Summary

✓ `?.` safely accesses members of potentially null objects
✓ Returns null if object is null (no NullReferenceException)
✓ Chain multiple ?. for deep navigation
✓ Use `?[index]` for safe array/collection access
✓ Combine with `??` for defaults
✓ Works in conditions and expressions
✓ Common pattern in modern C#

---

## Next Steps

1. Study Pattern Matching with Null
2. Learn Real-World Scenarios
3. Master Null Handling Patterns
