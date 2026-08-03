# Nullable Types - Easy Questions

## Q1: What is null and why does it matter?
Null represents "no value." It's different from default values. Causes NullReferenceException if not handled. Critical for safe C# programming.

## Q2: How do you create nullable value types?
Use `?` syntax: `int?`, `double?`, `bool?`, `DateTime?`. Cannot be null without `?`.

```csharp
int? age = null;
int? score = 95;
```

## Q3: What does the ?? operator do?
Provides default value if left operand is null.

```csharp
int value = age ?? 18;  // 18 if age null
```

## Q4: Explain the ?. operator
Safely accesses members. Returns null if object is null, preventing NullReferenceException.

```csharp
string? name = person?.Name;  // null if person null
```

## Q5: How do you check if nullable has value?
Use `.HasValue` property or check with `==` null.

```csharp
if (age.HasValue) { }
if (age != null) { }
if (age is not null) { }
```
