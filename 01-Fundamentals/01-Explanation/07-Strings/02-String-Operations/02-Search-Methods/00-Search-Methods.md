# String Search and Lookup Methods

## Overview
Find substrings, check patterns, and locate content within strings using various search methods.

---

## Contains Method

### Basic Contains

```csharp
string text = "Hello, World!";

// Check if contains substring
bool hasWorld = text.Contains("World");  // true
bool hasHello = text.Contains("Hello");  // true
bool hasXYZ = text.Contains("XYZ");  // false

// Case-sensitive by default
bool hasHELLO = text.Contains("HELLO");  // false
```

### Case-Insensitive Contains

```csharp
string text = "Hello, World!";

// Using StringComparison (C# 8+)
bool found = text.Contains("HELLO", StringComparison.OrdinalIgnoreCase);  // true

// Manual with ToLower
bool foundManual = text.ToLower().Contains("hello");  // true

// Check for any of multiple substrings
string[] searchTerms = { "World", "WORLD", "world" };
bool hasAny = searchTerms.Any(term => text.Contains(term, StringComparison.OrdinalIgnoreCase));
// true
```

### Contains for Validation

```csharp
string email = "user@example.com";

// Basic email validation
bool isEmail = email.Contains("@") && email.Contains(".");  // true

// Check for forbidden characters
string[] forbidden = { "<", ">", "!", "#", "$" };
bool hasForbidden = forbidden.Any(c => email.Contains(c));  // false
```

---

## IndexOf Method

### Find Substring Position

```csharp
string text = "Hello, World! Hello!";

// Find first occurrence
int index1 = text.IndexOf("Hello");  // 0
int index2 = text.IndexOf("World");  // 7
int notFound = text.IndexOf("XYZ");  // -1

// Find from specific position
int index3 = text.IndexOf("Hello", 1);  // 14 (skips first)
int index4 = text.IndexOf("Hello", 1, 10);  // -1 (search limit)
```

### Case-Insensitive IndexOf

```csharp
string text = "Hello World";

// Case-insensitive search
int index = text.IndexOf("hello", StringComparison.OrdinalIgnoreCase);  // 0
int index2 = text.IndexOf("WORLD", StringComparison.OrdinalIgnoreCase);  // 6
```

### LastIndexOf (Search from End)

```csharp
string text = "Hello, World! Hello!";

// Find last occurrence
int lastIndex = text.LastIndexOf("Hello");  // 14

// From right, search backwards
int lastComma = text.LastIndexOf(",");  // 5

// Not found
int notFound = text.LastIndexOf("XYZ");  // -1

// Case-insensitive
int last = text.LastIndexOf("HELLO", StringComparison.OrdinalIgnoreCase);  // 14
```

### Practical IndexOf Usage

```csharp
// Find character index
string text = "user@example.com";
int atIndex = text.IndexOf('@');  // 4

// Extract domain
if (atIndex != -1) {
    string domain = text.Substring(atIndex + 1);  // "example.com"
}

// Find file extension
string filename = "document.pdf";
int dotIndex = filename.LastIndexOf('.');
if (dotIndex != -1) {
    string extension = filename.Substring(dotIndex);  // ".pdf"
}
```

---

## StartsWith and EndsWith

### Check Beginning and End

```csharp
string url = "https://www.example.com";

// Check start
bool isHttps = url.StartsWith("https");  // true
bool isHttp = url.StartsWith("http");  // true
bool isWWW = url.StartsWith("www");  // false

// Check end
bool isDotCom = url.EndsWith(".com");  // true
bool isDotOrg = url.EndsWith(".org");  // false
```

### Case-Insensitive Prefix/Suffix

```csharp
string filename = "Document.PDF";

// Case-insensitive
bool isPdf = filename.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);  // true
bool isDoc = filename.StartsWith("doc", StringComparison.OrdinalIgnoreCase);  // true

// Multiple file types
string[] imageExtensions = { ".jpg", ".png", ".gif", ".bmp" };
bool isImage = imageExtensions.Any(ext => 
    filename.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
// false
```

### Practical Examples

```csharp
// API endpoint validation
string endpoint = "api/users/123";
if (endpoint.StartsWith("api/")) {
    // Process API request
}

// Protocol check
string link = "https://example.com";
if (link.StartsWith("https://") || link.StartsWith("http://")) {
    // Valid web URL
}

// File type validation
string uploadedFile = "image.jpg";
if (uploadedFile.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
    uploadedFile.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) {
    // Process image
}
```

---

## Character and Substring Searching

### IndexOf Character

```csharp
string text = "Hello, World!";

// Find character
int commaIndex = text.IndexOf(',');  // 5
int spaceIndex = text.IndexOf(' ');  // 6
int notFound = text.IndexOf('?');  // -1

// Find any of several characters
char[] separators = { ' ', ',', '!' };
int sepIndex = text.IndexOfAny(separators);  // 5 (comma)

// Find last character
int lastCommaIndex = text.LastIndexOf(',');  // 5
```

### IndexOfAny (Multiple Characters)

```csharp
string text = "user@example.com";

// Find first special character
char[] specials = { '@', '.', '-', '_' };
int firstSpecial = text.IndexOfAny(specials);  // 4 (@)

// Find first digit
char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
string data = "abc123def";
int firstDigit = data.IndexOfAny(digits);  // 3
```

---

## Contains with Char

### Check for Specific Characters

```csharp
string text = "Hello World";

// C# 9+: Contains char
bool hasH = text.Contains('H');  // true
bool has_space = text.Contains(' ');  // true
bool hasZ = text.Contains('Z');  // false

// Older versions: use IndexOf
bool hasHOld = text.IndexOf('H') >= 0;  // true
```

### Check for Vowels/Consonants

```csharp
string text = "Hello";
string vowels = "aeiouAEIOU";

// Any vowel?
bool hasVowel = text.Any(c => vowels.Contains(c));  // true

// All consonants?
bool allConsonants = text.All(c => !vowels.Contains(c));  // false

// Count vowels
int vowelCount = text.Count(c => vowels.Contains(c));  // 2
```

---

## Pattern Matching

### Using Regex (Regular Expressions)

```csharp
using System.Text.RegularExpressions;

string email = "user@example.com";

// Check pattern
bool isEmail = Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");  // true

// Find matches
MatchCollection matches = Regex.Matches("abc123def456", @"\d+");
foreach (Match match in matches) {
    Console.WriteLine(match.Value);  // 123, 456
}

// Replace with pattern
string phoneFormatted = Regex.Replace("1234567890", @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
// (123) 456-7890
```

### Common Regex Patterns

```csharp
// Email
Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");

// Phone
Regex.IsMatch(phone, @"^\d{3}-\d{3}-\d{4}$");

// URL
Regex.IsMatch(url, @"^https?://");

// Numbers only
Regex.IsMatch(input, @"^\d+$");

// Alphanumeric only
Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
```

---

## Performance Considerations

### IndexOf vs Contains

```csharp
string text = "Long text content...";

// Contains - cleaner for boolean checks
if (text.Contains("content")) { }

// IndexOf - needed if you want position
int position = text.IndexOf("content");
if (position >= 0) { }
```

### Case-Insensitive Performance

```csharp
// INEFFICIENT - Multiple conversions
string searchTerm = "HELLO";
foreach (var item in items) {
    if (item.ToLower() == searchTerm.ToLower()) { }
}

// EFFICIENT - Use StringComparison
foreach (var item in items) {
    if (item.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) { }
}

// BEST - If search term is fixed, convert once
string searchTermLower = searchTerm.ToLower();
foreach (var item in items) {
    if (item.ToLower() == searchTermLower) { }
}
```

### Compiled Regex for Reuse

```csharp
// Create once, reuse many times
static readonly Regex EmailRegex = 
    new Regex(@"^[^@]+@[^@]+\.[^@]+$", RegexOptions.Compiled);

// Use efficiently
bool isEmail = EmailRegex.IsMatch(email);
bool isEmail2 = EmailRegex.IsMatch(anotherEmail);
```

---

## Common Mistakes

### ❌ Assuming IndexOf Returns True/False

```csharp
int index = text.IndexOf("search");
if (index) { }  // Error! index is int, not bool
```

✓ **Correct:**
```csharp
if (index >= 0) { }  // or
if (text.Contains("search")) { }
```

### ❌ Not Handling -1

```csharp
string text = "Hello";
int index = text.IndexOf("XYZ");  // -1
char c = text[index];  // IndexOutOfRangeException!
```

✓ **Check first:**
```csharp
if (index >= 0) {
    char c = text[index];
}
```

### ❌ Case Sensitivity Issues

```csharp
string name = "Alice";
if (name.IndexOf("alice") >= 0) { }  // Fails! Case-sensitive
```

✓ **Use OrdinalIgnoreCase:**
```csharp
if (name.IndexOf("alice", StringComparison.OrdinalIgnoreCase) >= 0) { }
```

---

## Summary of Search Methods

| Method | Purpose | Returns |
|--------|---------|---------|
| `Contains()` | Check if contains | bool |
| `IndexOf()` | Find first position | int (-1 if not found) |
| `LastIndexOf()` | Find last position | int |
| `StartsWith()` | Check beginning | bool |
| `EndsWith()` | Check ending | bool |
| `IndexOfAny()` | Find any character | int |

---

## Best Practices

✓ Use `Contains()` for simple existence checks
✓ Use `IndexOf()` when you need the position
✓ Always check for -1 return from IndexOf
✓ Use `StringComparison.OrdinalIgnoreCase` for case-insensitive
✓ Cache compiled Regex for repeated use
✓ Use regex for complex patterns

---

## Next Steps

1. Study Manipulation Methods
2. Learn String Patterns
3. Explore Best Practices
