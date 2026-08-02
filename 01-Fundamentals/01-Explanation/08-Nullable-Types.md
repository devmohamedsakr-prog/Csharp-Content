# Nullable Types and Null Handling

## Overview
Nullable types allow value types to have null values. Reference types are nullable by default.

---

## What is Null?

Null represents "no value" or "undefined".

```csharp
// Reference types can be null
string name = null;
List<int> list = null;

// Value types cannot be null (without nullable wrapper)
int age = null;  // Error

// But with nullable syntax
int? age = null;  // OK
```

---

## Nullable Value Types

Use `?` to make value types nullable.

```csharp
// Nullable integer
int? number = null;
int? number2 = 42;

// Nullable double
double? price = null;
double? price2 = 19.99;

// Nullable boolean
bool? flag = null;
bool? flag2 = true;

// Nullable char
char? letter = null;
char? letter2 = 'A';

// Nullable DateTime
DateTime? birthDate = null;
DateTime? birthDate2 = new DateTime(2000, 1, 15);
```

---

## Checking for Null

### HasValue Property

```csharp
int? age = null;

if (age.HasValue) {
    Console.WriteLine($"Age: {age}");
} else {
    Console.WriteLine("Age not provided");
}

// Using Value property
int? score = 95;
if (score.HasValue) {
    int value = score.Value;  // 95
}
```

### Null Coalescing Operator (??)

Provides default value if null.

```csharp
int? age = null;
int defaultAge = age ?? 18;  // 18 (because age is null)

int? score = 95;
int defaultScore = score ?? 0;  // 95 (because score is not null)

string name = null;
string displayName = name ?? "Unknown";  // "Unknown"

// Chaining
string city = null;
string country = null;
string location = city ?? country ?? "Not specified";  // "Not specified"
```

### Null-Conditional Operator (?.)

Safely access members of potentially null object.

```csharp
string text = null;

// Without null-conditional - throws error
// int length = text.Length;  // NullReferenceException

// With null-conditional - returns null
int? length = text?.Length;  // null (no error)

// With object
Person person = null;
string name = person?.Name;  // null (no error)

// Chaining
string firstName = person?.Name?.Substring(0, 1);  // null

// With array
int[] arr = null;
int? firstElement = arr?[0];  // null
```

---

## Null-Coalescing Assignment (??=)

Assign only if null.

```csharp
int? x = null;
x ??= 10;  // x becomes 10
Console.WriteLine(x);  // 10

int? y = 5;
y ??= 20;  // y stays 5 (not null)
Console.WriteLine(y);  // 5

string name = null;
name ??= "Default";  // name becomes "Default"
Console.WriteLine(name);  // "Default"

string city = "New York";
city ??= "Unknown";  // city stays "New York"
Console.WriteLine(city);  // "New York"
```

---

## GetValueOrDefault

Get value or default without null check.

```csharp
int? age = null;
int result1 = age.GetValueOrDefault();  // 0 (default for int)
int result2 = age.GetValueOrDefault(18);  // 18 (custom default)

decimal? price = null;
decimal result3 = price.GetValueOrDefault();  // 0m
decimal result4 = price.GetValueOrDefault(9.99m);  // 9.99m

bool? flag = null;
bool result5 = flag.GetValueOrDefault();  // false
bool result6 = flag.GetValueOrDefault(true);  // true
```

---

## Pattern Matching with Null

```csharp
object obj = null;

// Check null
if (obj is null) {
    Console.WriteLine("Null");
}

if (obj is not null) {
    Console.WriteLine("Has value");
}

// Pattern matching
string result = obj switch {
    null => "Null value",
    "" => "Empty string",
    string s => $"String: {s}",
    int i => $"Integer: {i}",
    _ => "Unknown"
};
```

---

## Reference Types and Null

### Nullable Reference Types (C# 8+)

Enable with `#nullable enable`.

```csharp
#nullable enable

// Non-nullable reference type
string name = "Alice";  // Must have value
// string name = null;  // Error at compile time

// Nullable reference type
string? optionalName = null;  // OK

// Safe access
int length1 = name.Length;  // OK - name cannot be null
int? length2 = optionalName?.Length;  // OK - nullable
```

---

## Null Checks

### IsNullOrEmpty

```csharp
string text = null;

// Check null or empty
if (string.IsNullOrEmpty(text)) {
    Console.WriteLine("Text is null or empty");
}

// Check null, empty, or whitespace
if (string.IsNullOrWhiteSpace("   ")) {
    Console.WriteLine("Text is null, empty, or whitespace");
}
```

### Guard Clauses

```csharp
public void ProcessUser(User user) {
    if (user == null) {
        throw new ArgumentNullException(nameof(user));
    }
    
    if (user.Name == null) {
        throw new ArgumentException("Name cannot be null");
    }
    
    // Safe to use user
}

// ArgumentNullException.ThrowIfNull (C# 11+)
public void ProcessUser(User user) {
    ArgumentNullException.ThrowIfNull(user);
    // Safe to use user
}
```

---

## Common Null Scenarios

### Database Queries

```csharp
// Database might not have a value
int? productId = GetProductIdFromDatabase();

if (productId.HasValue) {
    Product product = GetProduct(productId.Value);
}

// or using ?? 
int id = productId ?? 0;  // Use 0 if null
```

### Optional Method Parameters

```csharp
public void CreateUser(string name, string email = null) {
    if (email == null) {
        email = "noemail@example.com";
    }
    // Continue processing
}

// Better
public void CreateUser(string name, string? email = null) {
    email ??= "noemail@example.com";
}
```

### API Responses

```csharp
public class ApiResponse {
    public string? Message { get; set; }
    public int? StatusCode { get; set; }
    public object? Data { get; set; }
}

var response = GetApiResponse();

// Safe access
string message = response?.Message ?? "No message";
int code = response?.StatusCode ?? 500;
```

---

## Best Practices

✓ **Use nullable types explicitly**
```csharp
// Good - clear intent
int? age = null;  // Can be null
int count = 5;  // Cannot be null

// Bad - unclear
int? age = 30;  // Could just be int
```

✓ **Use null coalescing for defaults**
```csharp
// Good
int value = input ?? 0;

// Bad
int value;
if (input == null) {
    value = 0;
} else {
    value = input.Value;
}
```

✓ **Validate inputs early**
```csharp
// Good
public void ProcessData(Data? data) {
    if (data == null) {
        return;  // Early return
    }
    // Process data
}

// Less ideal
public void ProcessData(Data? data) {
    if (data != null) {
        // Nested processing
    }
}
```

---

## Common Mistakes

❌ **NullReferenceException**
```csharp
string text = null;
int length = text.Length;  // Crashes!
```

✓ **Check before accessing**
```csharp
string text = null;
int? length = text?.Length;  // null, no crash
```

❌ **Forgetting null check**
```csharp
Person? person = GetPerson();
string name = person.Name;  // Might crash if person is null
```

✓ **Always check**
```csharp
Person? person = GetPerson();
string name = person?.Name ?? "Unknown";
```

❌ **Value without HasValue**
```csharp
int? age = null;
int value = age.Value;  // InvalidOperationException
```

✓ **Check HasValue first**
```csharp
int? age = null;
int value = age.HasValue ? age.Value : 0;
// or
int value = age ?? 0;
```

---

## Quick Summary

- Nullable types: `int?`, `bool?`, `string?`
- `??` operator provides default value
- `?.` safely accesses potentially null objects
- `??=` assigns only if null
- Check `HasValue` or use `GetValueOrDefault()`
- Use guard clauses to validate early
- Reference types are nullable by default (C# 8+)
