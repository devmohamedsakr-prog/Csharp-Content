# Condition Snippets

Generate conditional statements with built-in snippets.

## if - If Statement

**Shortcut:** `if` + Tab

**Generates:**
```csharp
if (true)
{
}
```

**Placeholders:**
- true: Replace with condition

**Usage:**
```csharp
if → Tab
// Replace condition
```

**Examples:**
```csharp
if (age >= 18)
{
    Console.WriteLine("Adult");
}

if (string.IsNullOrEmpty(name))
{
    Console.WriteLine("Name is required");
}

if (count > 0)
{
    ProcessItems();
}
```

---

## else - If-Else Statement

**Pattern:** After `if` statement, manually add `else`

**Code:**
```csharp
if (condition)
{
    // True branch
}
else
{
    // False branch
}
```

**Examples:**
```csharp
if (age >= 18)
{
    Console.WriteLine("You can vote");
}
else
{
    Console.WriteLine("Too young to vote");
}

if (score >= 60)
{
    Console.WriteLine("Pass");
}
else
{
    Console.WriteLine("Fail");
}
```

---

## if-else if-else Chain

**Pattern:**
```csharp
if (grade == 'A')
{
    Console.WriteLine("Excellent");
}
else if (grade == 'B')
{
    Console.WriteLine("Good");
}
else if (grade == 'C')
{
    Console.WriteLine("Average");
}
else
{
    Console.WriteLine("Below Average");
}
```

**Example:**
```csharp
int score = 75;

if (score >= 90)
{
    Console.WriteLine("A");
}
else if (score >= 80)
{
    Console.WriteLine("B");
}
else if (score >= 70)
{
    Console.WriteLine("C");
}
else if (score >= 60)
{
    Console.WriteLine("D");
}
else
{
    Console.WriteLine("F");
}
```

---

## switch - Switch Statement

**Shortcut:** `switch` + Tab

**Generates:**
```csharp
switch (expression)
{
    case value:
        break;
    default:
        break;
}
```

**Usage:**
```csharp
switch → Tab
```

**Examples:**
```csharp
// Switch on integer
int day = 3;
switch (day)
{
    case 1:
        Console.WriteLine("Monday");
        break;
    case 2:
        Console.WriteLine("Tuesday");
        break;
    case 3:
        Console.WriteLine("Wednesday");
        break;
    default:
        Console.WriteLine("Other day");
        break;
}

// Switch on string
string command = "start";
switch (command)
{
    case "start":
        StartProcess();
        break;
    case "stop":
        StopProcess();
        break;
    case "restart":
        RestartProcess();
        break;
    default:
        Console.WriteLine("Unknown command");
        break;
}
```

---

## switch with case - Case Block

**Shortcut:** `case` + Tab

**Generates:**
```csharp
case value:
    break;
```

**Usage:**
```csharp
// Inside switch block
case → Tab
```

**Example:**
```csharp
switch (status)
{
    case "active":
        Console.WriteLine("Running");
        break;
    case "inactive":
        Console.WriteLine("Stopped");
        break;
    case "error":
        Console.WriteLine("Error state");
        break;
}
```

---

## Ternary Operator - Inline Condition

**Syntax:** `condition ? trueValue : falseValue`

**Examples:**
```csharp
// Simple condition
string message = age >= 18 ? "Adult" : "Minor";

// Nested ternary
string grade = score >= 90 ? "A" : score >= 80 ? "B" : score >= 70 ? "C" : "F";

// With method calls
string result = string.IsNullOrEmpty(name) ? "Anonymous" : name;

// With expressions
int maximum = a > b ? a : b;
```

---

## Null Coalescing ??

**Syntax:** `value ?? defaultValue`

**Examples:**
```csharp
string name = userInput ?? "Guest";

int? age = null;
int displayAge = age ?? 0;

string email = GetEmail() ?? "not@provided.com";
```

---

## Null Conditional ?.

**Syntax:** `obj?.property` or `obj?.Method()`

**Examples:**
```csharp
// Safe property access
string name = person?.Name ?? "Unknown";

// Safe method call
int? length = text?.Length;

// Safe array/list access
int? firstItem = items?[0];
```

---

## Pattern Matching (C# 7+)

**is Pattern:**
```csharp
if (obj is string str)
{
    Console.WriteLine($"String: {str}");
}
else if (obj is int num)
{
    Console.WriteLine($"Number: {num}");
}
```

**switch Expression (C# 8+):**
```csharp
string message = status switch
{
    "active" => "Running",
    "inactive" => "Stopped",
    "error" => "Error state",
    _ => "Unknown"
};
```

---

## Quick Reference

| Type | Shortcut | Purpose |
|------|----------|---------|
| if | `if` | Single condition |
| else if | Manual | Multiple conditions |
| switch | `switch` | Many cases |
| case | `case` | Case block |
| ?: | Manual | Inline condition |
| ?? | Manual | Null coalescing |
| ?. | Manual | Null conditional |

---

## Best Practices

- Use `if` for simple conditions
- Use `switch` for many distinct values
- Use `?:` for simple value selection
- Use `??` for null defaults
- Use `?.` to prevent null reference exceptions

