# String: The Reference Type for Text

## Overview

The `string` type represents a sequence of characters. It's a reference type that stores text on the heap.

### Characteristics
```csharp
string text = "Hello, World!";

// Reference type: stored on heap
// Immutable: cannot change after creation
// Unicode: supports all Unicode characters
// Default value: null
// Size: variable (depends on content)
```

## String Basics

### Creating Strings

#### String Literals
```csharp
// Regular string
string greeting = "Hello";

// Empty string
string empty = "";
string empty2 = string.Empty;

// String with escape sequences
string escaped = "Line 1\nLine 2";  // Newline
string path = "C:\\Users\\Documents\\file.txt";  // Backslashes

// String with special characters
string special = "Quote: \"Hello\"";  // Double quotes
string tab = "Column1\tColumn2";  // Tab
```

#### Verbatim Strings (@)
```csharp
// Treats backslashes literally
string path = @"C:\Users\Documents\file.txt";

// Multiline strings
string multiline = @"Line 1
Line 2
Line 3";

// Useful for regex patterns
string regex = @"^\d{3}-\d{3}-\d{4}$";  // Phone pattern
```

#### Raw Strings (C# 11+)
```csharp
// For strings with many special characters
string json = """
{
    "name": "John",
    "age": 30
}
""";

// Indentation-aware
string code = """
    public void Method() {
        Console.WriteLine("Hello");
    }
    """;
```

#### String Interpolation
```csharp
string name = "Alice";
int age = 30;

// String interpolation (most readable)
string message = $"My name is {name} and I'm {age} years old";

// With expressions
string result = $"Total: {10 + 20}";
string formatted = $"Price: {price:C2}";  // Currency format

// Null coalescing
string display = $"Name: {person?.Name ?? "Unknown"}";
```

### String Properties and Methods

#### Length
```csharp
string text = "Hello";
int length = text.Length;  // 5

// Empty check
if (text.Length == 0) { }
if (string.IsNullOrEmpty(text)) { }
if (string.IsNullOrWhiteSpace(text)) { }
```

#### Accessing Characters
```csharp
string text = "Hello";

// By index (0-based)
char first = text[0];       // 'H'
char last = text[4];        // 'o'
char lastAlt = text[^1];    // 'o' (from end)

// Enumerating
foreach (char ch in text) {
    Console.WriteLine(ch);
}
```

#### Substring Operations
```csharp
string text = "Hello, World!";

// Substring
string sub1 = text.Substring(0, 5);     // "Hello"
string sub2 = text.Substring(7);        // "World!"

// String slicing (C# 8+)
string slice1 = text[0..5];             // "Hello"
string slice2 = text[7..];              // "World!"
string slice3 = text[^6..];             // "World!"
```

#### Case Conversion
```csharp
string text = "Hello";

string upper = text.ToUpper();          // "HELLO"
string lower = text.ToLower();          // "hello"

// Cultural awareness
string turkish = text.ToUpperInvariant();  // Culture-independent
```

#### Searching
```csharp
string text = "Hello, World!";

// Contains
bool hasWorld = text.Contains("World");  // true
bool hasJohn = text.Contains("John");    // false

// StartsWith / EndsWith
bool startsH = text.StartsWith("Hello");  // true
bool endsMark = text.EndsWith("!");       // true

// IndexOf / LastIndexOf
int index = text.IndexOf("o");           // 4
int lastIndex = text.LastIndexOf("o");   // 7
int notFound = text.IndexOf("xyz");      // -1
```

#### Replacing
```csharp
string text = "Hello, World!";

// Replace
string replaced = text.Replace("World", "C#");  // "Hello, C#!"

// Replace first occurrence only (custom)
string replaceFirst = System.Text.RegularExpressions.Regex
    .Replace(text, "o", "0", count: 1);  // "Hell0, World!"
```

#### Trimming and Padding
```csharp
string text = "  Hello  ";

// Trim whitespace
string trimmed = text.Trim();            // "Hello"
string trimStart = text.TrimStart();     // "Hello  "
string trimEnd = text.TrimEnd();         // "  Hello"

// Padding
string padLeft = "5".PadLeft(3, '0');    // "005"
string padRight = "5".PadRight(3, '0');  // "500"
```

#### Splitting and Joining
```csharp
string csv = "John,Alice,Bob";

// Split
string[] names = csv.Split(',');         // ["John", "Alice", "Bob"]

// Join
string result = string.Join(", ", names);  // "John, Alice, Bob"

// Split with options
string text = "a  b  c";
string[] parts = text.Split(' ', 
    System.StringSplitOptions.RemoveEmptyEntries);
```

## String Immutability

### What It Means
```csharp
string text = "Hello";
// Cannot modify individual characters
// text[0] = 'J';  // Compiler error

// Operations return new strings
string upper = text.ToUpper();  // New string, text unchanged
```

### Performance Implications
```csharp
// BAD: Creates new string each iteration
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;  // Creates new string each time!
}

// GOOD: Use StringBuilder for many concatenations
System.Text.StringBuilder sb = new();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
string result = sb.ToString();  // Single final string
```

## String Concatenation

### Methods

#### String Concatenation Operator (+)
```csharp
string first = "Hello";
string second = "World";
string result = first + ", " + second;  // "Hello, World"

// With other types
string mixed = "Count: " + 5;  // "Count: 5"
```

#### String.Concat
```csharp
string result = string.Concat("Hello", " ", "World");
string result2 = string.Concat(new[] { "a", "b", "c" });
```

#### String Interpolation (Recommended)
```csharp
string name = "Alice";
string message = $"Hello, {name}!";  // Most readable
```

#### StringBuilder (For Performance)
```csharp
var sb = new System.Text.StringBuilder();
sb.Append("Hello");
sb.Append(" ");
sb.Append("World");
string result = sb.ToString();  // "Hello World"
```

## String Formatting

### Format Specifiers
```csharp
int number = 42;
double price = 19.99;
DateTime date = DateTime.Now;

// Currency
string currency = price.ToString("C2");         // $19.99

// Numbers
string padded = number.ToString("D5");          // "00042"
string hex = number.ToString("X");              // "2A"

// Dates
string dateStr = date.ToString("yyyy-MM-dd");   // 2024-01-15
string time = date.ToString("HH:mm:ss");        // 14:30:45

// Percentage
double percent = 0.95;
string pct = percent.ToString("P");             // "95.00%"

// Interpolation with format
string formatted = $"Price: {price:C2}, Date: {date:yyyy-MM-dd}";
```

## String Comparison

### Equality
```csharp
string a = "Hello";
string b = "Hello";
string c = "hello";

// Case-sensitive comparison
Console.WriteLine(a == b);           // true
Console.WriteLine(a == c);           // false
Console.WriteLine(a.Equals(c));      // false

// Case-insensitive comparison
Console.WriteLine(a.Equals(c, 
    StringComparison.OrdinalIgnoreCase));  // true
```

### Ordering
```csharp
string[] names = { "Charlie", "Alice", "Bob" };

// Sort
System.Array.Sort(names);
// Result: ["Alice", "Bob", "Charlie"]

// Compare
int cmp = "Alice".CompareTo("Bob");  // -1 (Alice comes first)
```

## Null and Empty

### Checking
```csharp
string text = null;

// Null check
if (text == null) { }
if (text is null) { }  // Pattern matching

// Empty check
if (text == "") { }
if (string.IsNullOrEmpty(text)) { }
if (string.IsNullOrWhiteSpace(text)) { }

// Null-coalescing
string display = text ?? "Default";
```

### Assignment
```csharp
string text = null;       // Null reference
string empty = "";        // Empty string (length 0)
string blank = " ";       // Whitespace (length 1)

// Not equal!
Console.WriteLine(text == empty);      // false
Console.WriteLine(empty == blank);     // false
```

## Common String Operations

```csharp
string email = "john.doe@example.com";

// Extract username (before @)
int atIndex = email.IndexOf('@');
string username = email.Substring(0, atIndex);

// Extract domain
string domain = email.Substring(atIndex + 1);

// Validate
bool isValid = email.Contains("@") && email.Contains(".");

// Format display
string display = email.ToLower().Trim();
```

## String Performance Tips

### ✓ Do
```csharp
// Use StringBuilder for many operations
var sb = new StringBuilder();
for (int i = 0; i < 10000; i++) {
    sb.AppendLine($"Item {i}");
}
string result = sb.ToString();

// Use string interpolation
string message = $"Hello, {name}!";

// Use string.Join for collections
string csv = string.Join(",", items);

// Use appropriate comparison
bool match = str.Equals(other, StringComparison.OrdinalIgnoreCase);
```

### ❌ Don't
```csharp
// Avoid string concatenation in loops
string result = "";
for (int i = 0; i < 10000; i++) {
    result += $"Item {i}\n";  // Creates 10000 new strings!
}

// Avoid unnecessary ToUpper/ToLower
string compare = text.ToUpper() == other.ToUpper();  // Two conversions

// Avoid Contains for multiple checks
if (text.Contains("a") || text.Contains("b") || text.Contains("c")) { }
```

## String Escape Sequences

| Sequence | Meaning | Example |
|----------|---------|---------|
| `\"` | Double quote | `"He said \"Hi\""` |
| `\'` | Single quote | `'It\'s'` |
| `\\` | Backslash | `"C:\\Users\\"` |
| `\n` | Newline | `"Line1\nLine2"` |
| `\r` | Carriage return | `"Windows\r\nNewline"` |
| `\t` | Tab | `"Col1\tCol2"` |
| `\0` | Null character | `"\0"` |
| `\b` | Backspace | `"\b"` |
| `\f` | Form feed | `"\f"` |
| `\v` | Vertical tab | `"\v"` |

## Common String Mistakes

❌ **String concatenation in loop**
```csharp
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;  // Very slow!
}
```

✓ **Use StringBuilder**
```csharp
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
string result = sb.ToString();
```

❌ **Comparing null strings**
```csharp
string text = null;
if (text.Length > 0) { }  // NullReferenceException!
```

✓ **Check null first**
```csharp
if (!string.IsNullOrEmpty(text) && text.Length > 0) { }
```

❌ **Case-sensitive comparison**
```csharp
string input = GetUserInput();
if (input == "quit") { }  // User might type "QUIT" or "Quit"
```

✓ **Case-insensitive comparison**
```csharp
if (input.Equals("quit", StringComparison.OrdinalIgnoreCase)) { }
```

## Summary

**String Characteristics**:
- Reference type (stored on heap)
- Immutable (cannot change after creation)
- Unicode text (supports all characters)
- Default value is `null`

**Performance**:
- Use `StringBuilder` for many concatenations
- String interpolation is efficient
- Avoid repeated ToUpper/ToLower

**Best Practices**:
- Use `string.IsNullOrEmpty()` to check
- Use `StringComparison.OrdinalIgnoreCase` for case-insensitive comparisons
- Use verbatim strings (@) for paths and regex
- Use raw strings for multi-line content
- Prefer string interpolation over concatenation

---

**Key Takeaway**: Strings are immutable, so use `StringBuilder` for performance when building large strings. Use string interpolation for readability and `string.IsNullOrEmpty()` for safety.
