# HasValue Property and Value Access

## Overview
Use `.HasValue` to safely check if a nullable type contains a value before accessing it.

---

## HasValue Property

### Basic Usage
```csharp
int? age = null;

if (age.HasValue) {
    Console.WriteLine($"Age: {age}");
} else {
    Console.WriteLine("Age not provided");
}

// Another example
int? score = 95;
if (score.HasValue) {
    Console.WriteLine($"Score: {score}");  // Prints "Score: 95"
}
```

### HasValue Returns Boolean
```csharp
int? value = 42;
bool hasValue = value.HasValue;  // true

int? empty = null;
bool isEmpty = empty.HasValue;  // false
```

---

## Value Property (Unsafe Access)

### Correct Usage (With HasValue Check)
```csharp
int? score = 95;

if (score.HasValue) {
    int value = score.Value;  // Safe - we know it has value
    Console.WriteLine(value);  // 95
}
```

### Dangerous Usage (Without Check)
```csharp
// WRONG - Will throw exception
int? score = null;
int value = score.Value;  // InvalidOperationException!

// The error: "Nullable object must have a value"
```

---

## Safe Extraction Methods

### Using GetValueOrDefault()
```csharp
int? age = null;

// Default for type (0 for int)
int result1 = age.GetValueOrDefault();  // 0

// Custom default
int result2 = age.GetValueOrDefault(18);  // 18

// With value
int? score = 95;
int result3 = score.GetValueOrDefault();  // 95
int result4 = score.GetValueOrDefault(0);  // 95
```

### Using Null Coalescing (??)
```csharp
int? age = null;

// Simpler syntax
int result1 = age ?? 18;  // 18

// With value
int? score = 95;
int result2 = score ?? 0;  // 95

// Chaining
int? first = null;
int? second = null;
int? third = 25;
int result = first ?? second ?? third ?? 0;  // 25
```

---

## Pattern Matching for Null Checking

### is null / is not null
```csharp
int? value = GetValue();

// Modern syntax
if (value is not null) {
    int num = value.Value;  // Safe here
}

// is null
if (value is null) {
    Console.WriteLine("No value");
}
```

### Switch Expressions
```csharp
int? value = GetValue();

string result = value switch {
    null => "No value",
    0 => "Zero",
    > 0 => "Positive",
    < 0 => "Negative"
};
```

---

## Practical Examples

### Safe Database Access
```csharp
// Get optional value from database
int? salary = GetEmployeeSalary(id);

if (salary.HasValue) {
    ProcessSalary(salary.Value);
} else {
    ApplyDefaultSalary();
}

// Or simpler
ProcessSalary(salary ?? 40000);
```

### Optional User Input
```csharp
// User may enter age or skip
int? userAge = ParseUserInput();

// Safe handling
if (userAge.HasValue && userAge.Value >= 18) {
    AllowAccess();
} else {
    DenyAccess();
}

// Simpler
int ageToCheck = userAge ?? 0;
if (ageToCheck >= 18) AllowAccess();
```

### Nullable DateTime
```csharp
DateTime? startDate = GetStartDate();

if (startDate.HasValue) {
    TimeSpan elapsed = DateTime.Now - startDate.Value;
    Console.WriteLine($"Elapsed: {elapsed}");
} else {
    Console.WriteLine("Not started");
}

// Or
var date = startDate ?? DateTime.Now;
```

---

## Common Patterns

### Check and Extract
```csharp
int? value = source;

if (value.HasValue) {
    int actualValue = value.Value;
    // Use actualValue
}
```

### Default Fallback
```csharp
int? primary = GetPrimary();
int? secondary = GetSecondary();

int toUse = primary ?? secondary ?? 100;
```

### Conditional Processing
```csharp
bool? flag = GetFlag();

if (flag is true) {
    DoSomething();
} else if (flag is false) {
    DoOtherThing();
} else {
    // flag is null
    DoDefault();
}
```

---

## Comparison: Different Approaches

| Approach | Code | Pros/Cons |
|----------|------|----------|
| HasValue check | `if (x.HasValue) value = x.Value;` | Explicit, clear |
| ?? operator | `value = x ?? default;` | Concise, readable |
| GetValueOrDefault | `value = x.GetValueOrDefault();` | Flexible defaults |
| is not null | `if (x is not null)` | Modern, pattern-based |

---

## Summary

✓ Use `.HasValue` to check for value
✓ Only access `.Value` if HasValue is true
✓ Use `??` for simple defaults
✓ Use `GetValueOrDefault()` for custom defaults
✓ Use pattern matching for modern code
✓ Choose readable approach for your codebase

---

## Next Steps

1. Study Null-Coalescing Operator
2. Learn Null-Conditional Operator
3. Master Safe Access Patterns
