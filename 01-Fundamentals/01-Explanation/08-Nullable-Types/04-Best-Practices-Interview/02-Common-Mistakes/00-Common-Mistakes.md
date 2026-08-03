# Nullable Types Common Mistakes

## 1. NullReferenceException
```csharp
// WRONG
string? text = GetText();
int len = text.Length;  // Crashes if null!

// RIGHT
int? len = text?.Length;
```

## 2. Forgetting the ?
```csharp
// WRONG
string name = person.Name;  // Might crash!

// RIGHT
string? name = person?.Name;
```

## 3. Accessing Value Without Check
```csharp
// WRONG
int? age = null;
int value = age.Value;  // InvalidOperationException!

// RIGHT
int value = age ?? 0;
```

## 4. Forgetting to Chain ?.
```csharp
// WRONG
string city = person.Address.City;  // Can crash

// RIGHT
string? city = person?.Address?.City;
```

## 5. Wrong Null Check
```csharp
// LESS IDEAL
if (text != null) {
    // Process
}

// BETTER
if (text is not null) {
    // Process
}
```

## 6. Not Providing Defaults
```csharp
// INCOMPLETE
int? timeout = config.Timeout;  // What if null?

// COMPLETE
int timeout = config.Timeout ?? 5000;
```

## 7. Using Ternary Instead of ??
```csharp
// VERBOSE
int value = age != null ? age.Value : 0;

// CLEAN
int value = age ?? 0;
```

## 8. Assuming Database Values
```csharp
// WRONG
int salary = GetSalary();  // Might be null!

// RIGHT
int? salary = GetSalary();
int actual = salary ?? 0;
```

## 9. Forgetting ArgumentNullException
```csharp
// INCOMPLETE
public void Process(Data data) {
    // What if data is null?
}

// COMPLETE
public void Process(Data data) {
    ArgumentNullException.ThrowIfNull(data);
}
```

## 10. Nested Null Checks
```csharp
// HARD TO READ
if (person != null) {
    if (person.Address != null) {
        if (person.Address.City != null) {
            // Process
        }
    }
}

// CLEAN
string? city = person?.Address?.City;
if (city is not null) {
    // Process
}
```

## Summary

❌ **Common mistakes to avoid:**
- NullReferenceException crashes
- Forgetting ?. operator
- Accessing .Value without check
- Not chaining ?.
- Wrong null checks
- Not providing defaults
- Missing validation guards
- Wrong assumptions about nullability

---

## Next Steps

1. Study Interview Questions
2. Practice Safe Null Handling
