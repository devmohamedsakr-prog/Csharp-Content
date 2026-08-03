# Switch Statements

## Overview

Switch statements select one execution path from many based on a value. Cleaner than long if-else chains.

## Basic Switch

```csharp
string day = "Monday";

switch (day) {
    case "Monday":
        Console.WriteLine("Start of week");
        break;
    case "Friday":
        Console.WriteLine("Almost weekend");
        break;
    case "Saturday":
    case "Sunday":
        Console.WriteLine("Weekend");
        break;
    default:
        Console.WriteLine("Midweek");
        break;
}
```

**Key Points**:
- `break` exits the switch
- `default` catches unmatched values
- Multiple cases can share code (fallthrough)

---

## Switch Expression (C# 8+)

Modern, concise alternative to switch statement.

```csharp
string day = "Monday";

string message = day switch {
    "Monday" => "Start of week",
    "Friday" => "Almost weekend",
    "Saturday" or "Sunday" => "Weekend",
    _ => "Midweek"
};
```

**Benefits**:
- Expression, not statement (returns value)
- No break needed
- More concise
- Pattern matching support

---

## Pattern Matching in Switch

Match by type, property, or pattern.

```csharp
object obj = "Hello";

string result = obj switch {
    string s => $"String: {s}",
    int i => $"Integer: {i}",
    bool b => $"Boolean: {b}",
    null => "Null value",
    _ => "Unknown type"
};
```

**Use When**: Different types need different handling

---

## Real-World Examples

### Order Status Handler
```csharp
public string GetStatusMessage(OrderStatus status) => status switch {
    OrderStatus.Pending => "Your order is pending",
    OrderStatus.Processing => "We're preparing your order",
    OrderStatus.Shipped => "Your order is on the way",
    OrderStatus.Delivered => "Your order has arrived",
    OrderStatus.Cancelled => "Your order was cancelled",
    _ => "Unknown status"
};
```

### Discount Calculator
```csharp
public decimal CalculateDiscount(string customerType) => customerType switch {
    "Premium" => 0.20m,    // 20% off
    "Gold" => 0.15m,       // 15% off
    "Silver" => 0.10m,     // 10% off
    "Standard" => 0.05m,   // 5% off
    _ => 0.00m             // No discount
};
```

---

## Switch vs If-Else

**Switch better for**:
- Checking single value against many options
- Cleaner syntax for many options
- Performance (switch table optimization)

**If-Else better for**:
- Complex conditions
- Boolean logic (&&, ||)
- Range checking

---

## Common Mistakes

❌ Forgetting break (falls through)
```csharp
switch (value) {
    case 1:
        DoSomething();
        // Falls through to case 2!
    case 2:
        DoSomethingElse();
        break;
}
```

✓ Use break
```csharp
switch (value) {
    case 1:
        DoSomething();
        break;  // Correct
    case 2:
        DoSomethingElse();
        break;
}
```

---

## Best Practices

✓ Use switch expressions for simple returns
```csharp
string result = day switch {
    "Monday" => "Work",
    "Saturday" or "Sunday" => "Rest",
    _ => "Day off"
};
```

✓ Use switch statement for complex actions
```csharp
switch (command) {
    case "quit":
        SaveData();
        Cleanup();
        Environment.Exit(0);
        break;
    case "save":
        SaveData();
        break;
}
```

✓ Group related cases
```csharp
switch (value) {
    case 1:
    case 2:
    case 3:
        Console.WriteLine("1, 2, or 3");
        break;
    default:
        Console.WriteLine("Other");
        break;
}
```

---

## Quick Reference

| Feature | Switch Statement | Switch Expression |
|---------|------------------|-------------------|
| **Return value** | No (unless assignment) | Yes |
| **Syntax** | Verbose | Concise |
| **Pattern matching** | Yes (C# 7+) | Yes (C# 8+) |
| **Best for** | Complex logic | Simple returns |

---

## Next Steps

- Learn [Pattern Matching](../03-Pattern-Matching/00-Pattern-Matching.md)
- Study [Loops](../../02-Loop-Statements/README.md)
- Review [Best Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
