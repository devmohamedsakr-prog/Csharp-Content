# Null-Related Operators

## Overview

C# provides several operators to safely handle null values and reduce null-checking boilerplate.

---

## Null-Coalescing Operator (??)

Returns the left operand if it's not null; otherwise returns the right operand.

```csharp
string name = null;
string result = name ?? "Unknown";  // "Unknown"

string actual = "Alice";
string result2 = actual ?? "Unknown";  // "Alice"
```

### Practical Use Cases

```csharp
// Default values
public string GetDisplayName(User user) {
    return user?.Name ?? "Anonymous";
}

// Chain multiple nulls
string city = address?.City ?? region?.Name ?? "Unknown Location";

// With nullable numbers
int? count = null;
int actualCount = count ?? 0;

// With objects
Person person = dbPerson ?? new Person { Name = "Default" };
```

### Chaining

```csharp
string result = value1 ?? value2 ?? value3 ?? "default";
// Uses first non-null value
```

---

## Null-Conditional Operator (?.)

Safely accesses members of potentially null objects. Returns null if object is null.

```csharp
class Person {
    public string Name { get; set; }
}

Person person = null;

// Without null-conditional - throws exception
// string name = person.Name;  // NullReferenceException

// With null-conditional - safe
string name = person?.Name;  // null (no exception)

// With non-null object
Person alice = new Person { Name = "Alice" };
string aliceName = alice?.Name;  // "Alice"
```

### Method Invocation

```csharp
List<int> list = null;
int? count = list?.Count;  // null

List<int> items = new() { 1, 2, 3 };
int? count2 = items?.Count;  // 3

// Invoke method
string email = user?.GetEmail();  // null if user is null
```

### Array/Indexer Access

```csharp
int[] array = null;
int? first = array?[0];  // null

int[] numbers = { 1, 2, 3 };
int? second = numbers?[1];  // 2

Dictionary<string, int> dict = null;
int? value = dict?["key"];  // null
```

### Chaining

```csharp
class Address {
    public string City { get; set; }
}

class Person {
    public Address Address { get; set; }
}

Person person = null;

// Multiple null-conditionals
string city = person?.Address?.City;  // null (safe)

// Non-null chain
Person alice = new() { 
    Address = new() { City = "NYC" } 
};
string aliceCity = alice?.Address?.City;  // "NYC"
```

---

## Null-Conditional with Null-Coalescing

Combine for powerful null handling:

```csharp
Person person = null;

// Get address city, or default to "Unknown"
string city = person?.Address?.City ?? "Unknown";

// Get email, with fallback
string contact = user?.Email ?? user?.Phone ?? "No contact";

// With method calls
string message = logger?.GetLastError() ?? "No errors";
```

---

## Null-Coalescing Assignment (??=)

Assigns only if the current value is null.

```csharp
string name = null;
name ??= "Default";  // Assigns because null
// name = "Default"

name ??= "Another";  // Doesn't assign (already not null)
// name = "Default"

// With objects
User user = null;
user ??= new User { Id = 1 };

User existing = new User { Id = 2 };
existing ??= new User { Id = 3 };  // existing unchanged
```

### Lazy Initialization

```csharp
private List<int> _cache = null;

public List<int> Cache {
    get {
        _cache ??= LoadCache();
        return _cache;
    }
}

private List<int> LoadCache() {
    // Expensive operation only done once
    return database.GetCachedItems();
}
```

---

## Practical Examples

### Safe Property Access

```csharp
public class OrderService {
    public decimal GetTotalPrice(Order order) {
        // Safe navigation and default
        return order?.Items?.Sum(i => i.Price) ?? 0;
    }
}
```

### Safe Method Calling

```csharp
public void ProcessUser(User user) {
    // Safe call with fallback
    string name = user?.GetFullName() ?? "Anonymous";
    
    // Only log if logger exists
    logger?.Log($"Processing {name}");
}
```

### Nullable Value Types

```csharp
int? score = GetScore();  // Could be null

// Use with null-coalescing
int finalScore = score ?? 0;

// Chain multiple sources
int? localScore = GetLocalScore();
int? apiScore = GetApiScore();
int result = localScore ?? apiScore ?? 0;
```

### Defensive Initialization

```csharp
public class Configuration {
    private string _connectionString = null;
    
    public string ConnectionString {
        get {
            // Initialize on first access
            _connectionString ??= ReadFromConfig();
            return _connectionString;
        }
    }
    
    private string ReadFromConfig() {
        // Expensive operation
        return configReader.GetConnectionString();
    }
}
```

### API Response Handling

```csharp
public async Task<UserDto> GetUser(int id) {
    var response = await apiClient.GetUser(id);
    
    return new UserDto {
        Id = response?.Id ?? 0,
        Name = response?.Name ?? "Unknown",
        Email = response?.Contact?.Email ?? "No email",
        Phone = response?.Contact?.Phone ??= "No phone"
    };
}
```

---

## Pattern Matching with Null

Modern C# (8.0+) supports pattern matching:

```csharp
// Traditional
if (person != null && person.Address != null) {
    Console.WriteLine(person.Address.City);
}

// Pattern matching (C# 8+)
if (person is { Address: { City: not null } }) {
    Console.WriteLine(person.Address.City);
}

// Not null pattern (C# 9+)
if (person is not null) {
    Console.WriteLine(person.Name);
}
```

---

## Nullable Reference Types

C# 8.0+ allows enabling nullable reference types:

```csharp
#nullable enable

public class Person {
    // Non-nullable: must have value
    public string Name { get; set; }
    
    // Nullable: can be null
    public string? MiddleName { get; set; }
    
    public string GetFullName() {
        // MiddleName might be null - need to check
        if (MiddleName != null) {
            return $"{Name} {MiddleName}";
        }
        return Name;
    }
}
```

---

## Best Practices

✓ **Use null-conditional for safe navigation**
```csharp
string city = person?.Address?.City;  // Safe
```

✓ **Use null-coalescing for defaults**
```csharp
string name = input ?? "Default";
```

✓ **Combine for powerful null handling**
```csharp
string result = user?.GetEmail() ?? "no-email";
```

✓ **Use ??= for lazy initialization**
```csharp
cache ??= LoadCache();
```

✓ **Enable nullable reference types**
```csharp
#nullable enable
public string Name { get; set; }  // Non-null
public string? Middle { get; set; }  // Nullable
```

---

## Common Mistakes

❌ **Using ?. when . would work**
```csharp
Person person = new Person();
string name = person?.Name;  // Unnecessary, person is not null
```

✓ **Only use ?. for potentially null**
```csharp
Person person = null;
string name = person?.Name;  // Correct
```

---

❌ **Forgetting null-coalescing**
```csharp
string result = GetValue();  // Might be null!
int length = result.Length;  // NullReferenceException
```

✓ **Handle nulls**
```csharp
string result = GetValue() ?? "default";
int length = result.Length;  // Safe
```

---

❌ **Complex null checks**
```csharp
if (user != null && user.Profile != null && 
    user.Profile.Address != null && 
    user.Profile.Address.City != null) {
    string city = user.Profile.Address.City;
}
```

✓ **Use null-conditional**
```csharp
string city = user?.Profile?.Address?.City;
```

---

## Quick Reference

| Operator | Syntax | Purpose |
|----------|--------|---------|
| ?? | x ?? y | Return x if not null, else y |
| ?. | x?.y | Access y if x not null |
| ?[ ] | x?[i] | Access index if x not null |
| ??= | x ??= y | Assign y only if x is null |

---

## Next Steps

- Study [Ternary Operator and Precedence](../../03-Ternary-Precedence/00-Ternary-Precedence.md)
- Review [Best Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
- Practice with [Interview Questions](../../04-Best-Practices-Interview/03-Interview-Questions/README.md)
