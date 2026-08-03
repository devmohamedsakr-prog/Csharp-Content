# Nullable Value Types

## Overview
Value types (int, double, bool, etc.) normally cannot be null. Use `?` syntax to make them nullable.

---

## Creating Nullable Value Types

### The `?` Syntax
```csharp
// Nullable integer
int? age = null;
int? score = 95;

// Nullable double
double? price = null;
double? amount = 19.99;

// Nullable boolean
bool? flag = null;
bool? isActive = true;

// Nullable character
char? letter = null;
char? grade = 'A';

// Nullable DateTime
DateTime? birthDate = null;
DateTime? today = DateTime.Now;

// Nullable enum
Status? status = null;
Status? current = Status.Active;
```

### Declaration vs Initialization
```csharp
// Declared as nullable, not initialized (defaults to null)
int? uninitialized;  // null

// Declared and initialized to null
int? declared = null;  // null

// Declared and initialized to value
int? withValue = 42;  // 42
```

---

## Assigning Values

### From Value to Nullable
```csharp
// Direct assignment (implicit conversion)
int? nullable = 10;  // int converts to int?

// Assignment from another nullable
int? first = 5;
int? second = first;  // null or value transfers

// Assignment of null
int? reset = null;  // Clear the value
```

### From Nullable to Value (Requires Caution)
```csharp
int? source = 42;

// WRONG - Cannot assign null to int
// int value = null;  // Compiler error!

// RIGHT - Must check or provide default
int value1 = source ?? 0;  // Use default if null
int value2 = source.HasValue ? source.Value : 0;

// Using GetValueOrDefault
int value3 = source.GetValueOrDefault();  // 0 if null
int value4 = source.GetValueOrDefault(99);  // 99 if null
```

---

## Common Nullable Types

### Numeric Types
```csharp
int? intValue = null;
long? longValue = null;
short? shortValue = null;
byte? byteValue = null;
float? floatValue = null;
double? doubleValue = null;
decimal? decimalValue = null;
```

### Boolean and Character
```csharp
bool? isValid = null;
bool? isEnabled = true;

char? letter = null;
char? space = ' ';
```

### DateTime
```csharp
DateTime? date = null;
DateTime? specificDate = new DateTime(2024, 8, 3);
DateTime? now = DateTime.Now;

// TimeSpan
TimeSpan? duration = null;
TimeSpan? elapsed = TimeSpan.FromSeconds(30);
```

### Structs (Custom)
```csharp
struct Point {
    public int X { get; set; }
    public int Y { get; set; }
}

// Nullable struct
Point? location = null;
Point? current = new Point { X = 10, Y = 20 };
```

---

## Checking for Values

### HasValue Property
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

### Value Property
```csharp
int? score = 95;

// Only access .Value if you know it has value
if (score.HasValue) {
    int value = score.Value;  // 95
    Console.WriteLine(value);
}

// WRONG - Will throw if null
// int? nullScore = null;
// int val = nullScore.Value;  // InvalidOperationException!
```

---

## Operations on Nullable Types

### Comparisons
```csharp
int? x = 10;
int? y = null;
int? z = 10;

bool test1 = x == z;  // true - both are 10
bool test2 = x == y;  // false - one is null
bool test3 = y == null;  // true - y is null
```

### Arithmetic (Propagates Null)
```csharp
int? a = 5;
int? b = 3;
int? c = null;

int? result1 = a + b;  // 8 (5 + 3)
int? result2 = a + c;  // null (null propagates)

// To do arithmetic, must get non-null value
int result3 = (a ?? 0) + (b ?? 0);  // 8
int result4 = (a ?? 0) + (c ?? 0);  // 5
```

### Logical Operators (Three-Valued Logic)
```csharp
bool? x = true;
bool? y = null;
bool? z = false;

bool? and1 = x & y;  // null
bool? and2 = x & z;  // false
bool? and3 = y & z;  // false

bool? or1 = x | y;  // true
bool? or2 = z | y;  // null
bool? or3 = z | z;  // false
```

---

## Default and Implicit Values

### Default Initialization
```csharp
int? unset;  // default: null

// Explicitly
int? nullValue = default;  // null

// Type default
int? defaultInt = int.MinValue;  // Explicit value
```

### Implicit Conversions
```csharp
int number = 10;
int? nullable = number;  // Implicit: int -> int?

// Reverse not implicit
// int back = nullable;  // Error - requires explicit conversion
```

---

## Practical Examples

### Handling Optional User Input
```csharp
// User may not provide age
int? userAge = GetUserInputAge();

// Safe handling
if (userAge.HasValue) {
    ValidateAge(userAge.Value);
} else {
    UseDefaultAge();
}

// Or simpler
int ageToUse = userAge ?? 18;  // Default to 18
```

### Database NULL Values
```csharp
// Database columns might be nullable
int? salary = GetEmployeeSalary(id);
decimal? bonus = GetEmployeeBonus(id);

// Calculate total compensation
decimal compensation = (salary ?? 0) + (bonus ?? 0);
```

### Optional Configuration
```csharp
public class AppConfig {
    public int? MaxConnections { get; set; }
    public int? TimeoutMs { get; set; }
    public bool? EnableCaching { get; set; }
}

var config = LoadConfig();
int maxConn = config.MaxConnections ?? 10;  // Default 10
int timeout = config.TimeoutMs ?? 5000;     // Default 5 seconds
bool cache = config.EnableCaching ?? true;  // Default true
```

---

## Summary

✓ Use `?` to make value types nullable
✓ Nullable types default to null
✓ Check `.HasValue` before accessing `.Value`
✓ Use `??` operator for defaults
✓ Use `GetValueOrDefault()` for safe extraction
✓ Operations propagate null
✓ Three-valued logic for bool?

---

## Next Steps

1. Learn Null Checking Methods
2. Study Safe Access Patterns
3. Master Null Handling
