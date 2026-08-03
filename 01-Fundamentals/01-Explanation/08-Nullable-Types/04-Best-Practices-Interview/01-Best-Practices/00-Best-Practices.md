# Nullable Types Best Practices

## 1. Use Nullable Types Explicitly
```csharp
// GOOD - Clear intent
int? age = null;  // Can be null
int count = 5;  // Cannot be null

// AVOID - Unclear
int? age = 30;  // Could just be int
```

## 2. Enable Nullable Reference Types
```csharp
#nullable enable
string name = "Alice";  // Cannot be null
string? optional = null;  // Can be null
```

## 3. Use ?? for Defaults
```csharp
// GOOD - Concise
int value = input ?? 0;

// AVOID - Verbose
int value = input != null ? input.Value : 0;
```

## 4. Validate Early with Guard Clauses
```csharp
// GOOD
public void Process(Data? data) {
    ArgumentNullException.ThrowIfNull(data);
    // Safe to use
}

// AVOID - Nested checks
public void ProcessNested(Data? data) {
    if (data != null) {
        // nested code
    }
}
```

## 5. Use ?. for Safe Access
```csharp
// GOOD
string? city = person?.Address?.City;

// AVOID - Crashes if person is null
string city = person.Address.City;
```

## 6. Check HasValue or Use ??
```csharp
// GOOD
int value = age ?? 18;

// GOOD - Explicit
int value = age.HasValue ? age.Value : 18;

// AVOID - Will crash if null
int value = age.Value;
```

## 7. Use Null-Coalescing Assignment
```csharp
// GOOD
config.Timeout ??= 5000;

// AVOID - More verbose
if (config.Timeout == null) {
    config.Timeout = 5000;
}
```

## 8. Pattern Match for Clarity
```csharp
// GOOD
if (value is not null) {
    Process(value);
}

// AVOID - Less clear
if (value != null) {
    Process(value);
}
```

## 9. Document Nullable Properties
```csharp
/// <summary>
/// Gets the user's middle name.
/// </summary>
/// <remarks>
/// Can be null if not provided.
/// </remarks>
public string? MiddleName { get; set; }
```

## 10. Fail Fast with Validation
```csharp
// GOOD - Fail immediately
ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

// AVOID - Silent failures
if (email == null) return;
```

## Summary

✓ Explicit nullable declarations
✓ Use ?? for defaults
✓ Guard clauses for validation
✓ Safe access with ?.
✓ Pattern matching for clarity
✓ Fail fast approach

---

## Next Steps

1. Study Common Mistakes
2. Review Interview Questions
