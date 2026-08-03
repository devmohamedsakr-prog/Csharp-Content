# Pattern Matching with Null

## Overview
Modern pattern matching provides clean, expressive ways to handle null values.

---

## is null / is not null

### Basic Pattern
```csharp
string? text = GetText();

if (text is null) {
    Console.WriteLine("Null value");
} else if (text is not null) {
    Console.WriteLine($"Value: {text}");
}
```

---

## Switch Expressions

### Null Handling
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

## Summary

✓ Use `is null` / `is not null` for clarity
✓ Switch expressions for multiple cases
✓ Clean and readable code
✓ Compiler validates completeness
