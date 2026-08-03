# Nullable Types - Medium Questions

## Q1: Design a function to safely extract nullable values
```csharp
public static T GetValueOrDefault<T>(T? value, T defaultValue) where T : struct {
    return value.HasValue ? value.Value : defaultValue;
}

// Better yet, use built-in GetValueOrDefault()
public static T GetSafe<T>(T? value, T defaultValue) where T : struct {
    return value?.GetValueOrDefault(defaultValue) ?? defaultValue;
}
```

## Q2: Explain null coalescing operator chaining
```csharp
string location = 
    user?.PrimaryPhone ?? 
    user?.SecondaryPhone ?? 
    organization?.Phone ?? 
    "No contact";
```
Tries each option until finding non-null value.

## Q3: When to use guard clauses?
Early validation and return. Prevents nested code and improves readability.

```csharp
public void Process(Data data) {
    ArgumentNullException.ThrowIfNull(data);
    // Safe to use
}
```

## Q4: Real-world scenario - API response handling
```csharp
var response = await GetUserAsync(id);
var email = response?.User?.Email ?? "no-email@example.com";
int age = response?.User?.Age ?? 0;
```

## Q5: Pattern matching for null handling
```csharp
string result = value switch {
    null => "No value",
    0 => "Zero",
    > 0 => "Positive",
    _ => "Other"
};
```
