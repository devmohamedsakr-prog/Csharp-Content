# String Best Practices

## 1. Use String Interpolation

```csharp
// GOOD - Clear and readable
string msg = $"Hello {name}, age {age}";

// AVOID - Verbose concatenation
string msg2 = "Hello " + name + ", age " + age;
```

## 2. Use StringBuilder for Loops

```csharp
// GOOD - Efficient O(n)
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
string result = sb.ToString();

// AVOID - Inefficient O(n²)
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;
}
```

## 3. Validate Input Safely

```csharp
// GOOD - Explicit null check
string input = GetUserInput();
if (!string.IsNullOrWhiteSpace(input)) {
    Process(input.Trim());
}

// AVOID - Assumes non-null
Process(input.ToUpper());  // Can throw!
```

## 4. Use Appropriate Comparison

```csharp
// GOOD - Case-insensitive when needed
if (role.Equals("admin", StringComparison.OrdinalIgnoreCase)) { }

// AVOID - Case-sensitive assumption
if (role == "Admin") { }  // Fails if "admin"
```

## 5. Check Bounds Before Indexing

```csharp
// GOOD - Safe access
if (index >= 0 && index < text.Length) {
    char c = text[index];
}

// AVOID - May throw
char c = text[unknownIndex];
```

## 6. Use Range Operators (C# 8+)

```csharp
// GOOD - Clean and safe
string first5 = text[0..5];
string last = text[^1..];

// AVOID - More verbose
string first5 = text.Substring(0, 5);
```

## 7. Use String.Join for Collections

```csharp
// GOOD - Fast and clean
string joined = string.Join(", ", items);

// AVOID - Manual loop
string result = "";
foreach (var item in items) {
    if (result != "") result += ", ";
    result += item;
}
```

## 8. Use IsNullOrWhiteSpace for Validation

```csharp
// GOOD - Checks all conditions
if (string.IsNullOrWhiteSpace(input)) {
    return false;
}

// PARTIAL - Doesn't check whitespace
if (string.IsNullOrEmpty(input)) { }
```

## 9. Cache Regex Patterns

```csharp
// GOOD - Compile once
static readonly Regex EmailRegex = 
    new Regex(@"^[^@]+@[^@]+\.[^@]+$", RegexOptions.Compiled);

bool isEmail = EmailRegex.IsMatch(email);

// AVOID - Recompiled each time
bool isEmail = Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
```

## 10. Use Invariant Culture for Keys

```csharp
// GOOD - Consistent across cultures
string key = userInput.ToLowerInvariant();

// AVOID - Culture-dependent
string key = userInput.ToLower();  // May differ by culture
```

## 11. Don't Modify While Iterating

```csharp
// GOOD - Work with copy
foreach (var c in text.ToCharArray()) {
    Process(c);
}

// AVOID - String immutable anyway, but be clear
foreach (var c in text) {
    Process(c);
}
```

## 12. Use Null Coalescing

```csharp
// GOOD - Default value if null
string value = userInput ?? "default";

// GOOD - Null conditional
int? length = text?.Length;

// AVOID - Verbose null check
string value = userInput != null ? userInput : "default";
```

## 13. Format Strings Appropriately

```csharp
// GOOD - Use format specifiers
string currency = $"{price:C}";  // "$19.99"
string padded = $"{id:D5}";  // "00042"

// AVOID - Manual formatting
string currency = "$" + price.ToString();
```

## 14. Use Consistent Casing for Comparison

```csharp
// GOOD - Consistent approach
bool sameIgnoreCase = text1.Equals(text2, StringComparison.OrdinalIgnoreCase);

// AVOID - Inefficient multiple conversions
if (text1.ToLower() == text2.ToLower()) { }
```

## 15. Document String Format Constraints

```csharp
/// <summary>
/// Formats the date as yyyy-MM-dd
/// </summary>
public string FormatDate(DateTime date) => date.ToString("yyyy-MM-dd");

// Without documentation, developers might not know expected format
public string FormatDate(DateTime date) => date.ToString("d");
```

## Summary of Best Practices

✓ Use string interpolation
✓ Use StringBuilder for loops
✓ Validate input safely
✓ Use appropriate comparison methods
✓ Check bounds before indexing
✓ Use ranges for extraction
✓ Use String.Join for collections
✓ Check null and whitespace
✓ Cache compiled regex
✓ Use invariant culture for keys
✓ Use null coalescing
✓ Format appropriately
✓ Document constraints

---

## Next Steps

1. Study Common Mistakes
2. Review Interview Questions
3. Practice Coding
