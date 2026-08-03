# String Patterns

## Overview
Learn practical patterns: comparison, formatting, validation, and performance optimization.

## Learning Path

### 1. String Comparison
- Equality operators and methods
- StringComparison options (Ordinal, Culture)
- CompareTo for ordering
- Null and empty checks
- Safe comparison patterns

**Time:** 20-25 minutes

### 2. String Formatting
- String interpolation with specifiers
- Format methods and composites
- Number, currency, date/time formats
- Custom formatting
- Table output and alignment

**Time:** 20-25 minutes

### 3. String Validation
- Null and empty checks
- Length validation
- Character type checks
- Email, phone, URL validation
- Password strength validation
- Regular expressions

**Time:** 25-30 minutes

### 4. StringBuilder & Performance
- String immutability implications
- When to use StringBuilder
- Capacity management
- Performance comparison
- Optimization strategies

**Time:** 20-25 minutes

## Files in This Section

1. **00-String-Comparison.md** - Equals, CompareTo, StringComparison options
2. **00-String-Formatting.md** - Interpolation, format specifiers, custom formats
3. **00-String-Validation.md** - Input validation, regex patterns, security
4. **00-StringBuilder-Performance.md** - Optimization, pooling, memory management

## Quick Reference

```csharp
// Comparison
bool same = text1.Equals(text2, StringComparison.OrdinalIgnoreCase);
if (!string.IsNullOrWhiteSpace(input)) { }

// Formatting
string currency = $"{price:C}";
string date = $"{now:yyyy-MM-dd}";
string padded = $"{id:D5}";

// Validation
bool isEmail = Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
bool isStrong = password.Length >= 8 && password.Any(char.IsUpper);

// Performance
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
string result = sb.ToString();
```

## Comparison Patterns

### Safe Comparison
```csharp
// Null-safe
bool same = a?.Equals(b) ?? false;

// Case-insensitive
bool sameIgnoreCase = a.Equals(b, StringComparison.OrdinalIgnoreCase);

// Null and empty check
if (!string.IsNullOrWhiteSpace(input)) { }
```

### Performance
- `Ordinal` - Fast, culture-independent
- `OrdinalIgnoreCase` - Fast, case-insensitive
- `CurrentCulture` - Slow, culture-aware

## Formatting Patterns

### Format Specifiers
```csharp
$"{value:D5}"      // Decimal with zeros: "00042"
$"{price:C}"       // Currency: "$19.99"
$"{percent:P}"     // Percentage: "85.00%"
$"{date:yyyy-MM-dd}"  // Date: "2024-08-03"
```

### Table Output
```csharp
Console.WriteLine("{0,-15} {1,5}", "Name", "Age");
Console.WriteLine("{0,-15} {1,5}", "Alice", 30);
```

## Validation Patterns

### Basic Checks
```csharp
// Empty/null
if (string.IsNullOrWhiteSpace(input)) { }

// Length
if (input.Length >= 8) { }

// Character types
if (input.All(char.IsDigit)) { }
if (input.Any(char.IsUpper)) { }
```

### Regex Patterns
```csharp
// Email
@"^[^@]+@[^@]+\.[^@]+$"

// Phone
@"^\d{3}-\d{3}-\d{4}$"

// URL
@"^https?://"

// Numbers only
@"^\d+$"
```

## Performance Optimization

### String Concatenation
```csharp
// SLOW - O(n²)
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;
}

// FAST - O(n)
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
```

### Regex Caching
```csharp
// SLOW - Recompiled each use
for (int i = 0; i < 1000; i++) {
    if (Regex.IsMatch(item, pattern)) { }
}

// FAST - Compiled once
static readonly Regex cached = new Regex(pattern, RegexOptions.Compiled);
for (int i = 0; i < 1000; i++) {
    if (cached.IsMatch(item)) { }
}
```

## Real-World Scenarios

### CSV Processing
```csharp
// Parsing
string[] fields = line.Split(',');

// Building
var sb = new StringBuilder();
sb.AppendLine("Name,Age,City");
foreach (var item in items) {
    sb.AppendLine($"{item.Name},{item.Age},{item.City}");
}
```

### User Input Validation
```csharp
// Trim and validate
string email = input.Trim();
if (!Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$")) {
    return false;
}
```

### Text Search
```csharp
// Case-insensitive
bool found = text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
```

## Decision Tree

**Need to compare strings?**
- Simple equality: `==` or `.Equals()`
- Case-insensitive: `.Equals(..., OrdinalIgnoreCase)`
- Ordering: `.CompareTo()`
- Null-safe: `?.Equals(...) ?? false`

**Need to validate?**
- Simple check: `string.IsNullOrWhiteSpace()`
- Pattern: Use regex
- Security: Use constant-time comparison

**Need to build strings?**
- Few operations: String interpolation
- Many operations: StringBuilder
- Joining collection: `string.Join()`

**Performance critical?**
- Compile regex patterns
- Use StringBuilder for loops
- Cache results
- Avoid repeated conversions

## Best Practices

✓ Use StringComparison for explicit comparison
✓ Validate input with IsNullOrWhiteSpace
✓ Use StringBuilder for loops
✓ Compile regex patterns for reuse
✓ Format with appropriate specifiers
✓ Test validation with edge cases
✓ Use invariant culture for keys
✓ Document format string requirements

## Common Mistakes

❌ Culture-dependent comparisons
❌ Missing null checks
❌ String concatenation in loops
❌ Regex recompilation each iteration
❌ Ignoring whitespace in validation
❌ Wrong comparison operator

## Self-Assessment

Can you:
- [ ] Use StringComparison options?
- [ ] Format strings with specifiers?
- [ ] Validate common formats?
- [ ] Use regex for patterns?
- [ ] Optimize string operations?
- [ ] Handle internationalization?

---

## Related Topics

- **String Operations** - Methods and manipulation
- **String Basics** - Creation and properties
- **Collections** - Working with string collections
- **Regular Expressions** - Advanced pattern matching

## Next Steps

1. ✓ Study String Comparison
2. ✓ Master String Formatting
3. ✓ Learn Validation Techniques
4. ✓ Optimize with StringBuilder
5. → Review Best Practices
6. → Study Interview Questions
