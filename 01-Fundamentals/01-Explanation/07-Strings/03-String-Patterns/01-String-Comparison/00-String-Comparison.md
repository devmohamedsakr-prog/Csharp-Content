# String Comparison and Equality

## Overview
Compare strings correctly using appropriate methods and options. Understand case sensitivity, culture, and ordinal comparison.

---

## Equality Operators and Methods

### Equality Comparison

```csharp
string a = "Hello";
string b = "Hello";
string c = "hello";

// == operator (compares content by default)
bool same = a == b;  // true
bool different = a == c;  // false

// Equals method (same behavior)
bool same2 = a.Equals(b);  // true
bool different2 = a.Equals(c);  // false

// Not equal
bool notSame = a != c;  // true
```

### Reference vs Content Equality

```csharp
// Content equality (what == does for strings)
string a = new string(new[] { 'H', 'i' });
string b = new string(new[] { 'H', 'i' });

bool contentSame = a == b;  // true (content equal)
bool referenceEqual = ReferenceEquals(a, b);  // false (different objects)

// String interning (compiler optimization)
string s1 = "Hello";
string s2 = "Hello";
bool refSame = ReferenceEquals(s1, s2);  // Often true (interned)
```

---

## StringComparison Options

### Ordinal vs Culture

```csharp
string text1 = "Café";
string text2 = "Cafe";

// Ordinal - byte-by-byte comparison, culture-independent
bool ordinal = text1.Equals(text2, StringComparison.Ordinal);  // false

// Culture-aware - may consider accents as same
bool cultureSensitive = text1.Equals(text2, StringComparison.CurrentCulture);  // Depends on culture

// Case-sensitive
bool caseSensitive = "Hello".Equals("hello", StringComparison.Ordinal);  // false

// Case-insensitive
bool caseInsensitive = "Hello".Equals("hello", StringComparison.OrdinalIgnoreCase);  // true
```

### Available Options

```csharp
string a = "Hello";
string b = "HELLO";

// Ordinal (exact match, case-sensitive)
bool test1 = a.Equals(b, StringComparison.Ordinal);  // false

// OrdinalIgnoreCase (exact match, case-insensitive)
bool test2 = a.Equals(b, StringComparison.OrdinalIgnoreCase);  // true

// CurrentCulture (culture-aware, case-sensitive)
bool test3 = a.Equals(b, StringComparison.CurrentCulture);  // false

// CurrentCultureIgnoreCase (culture-aware, case-insensitive)
bool test4 = a.Equals(b, StringComparison.CurrentCultureIgnoreCase);  // true

// InvariantCulture (invariant culture, case-sensitive)
bool test5 = a.Equals(b, StringComparison.InvariantCulture);  // false

// InvariantCultureIgnoreCase (invariant, case-insensitive)
bool test6 = a.Equals(b, StringComparison.InvariantCultureIgnoreCase);  // true
```

---

## CompareTo Method

### Lexicographic Comparison

```csharp
string a = "Apple";
string b = "Banana";
string c = "Apple";

// CompareTo returns:
// 0 = equal
// negative = a < b
// positive = a > b

int result1 = a.CompareTo(b);  // Negative (Apple < Banana)
int result2 = b.CompareTo(a);  // Positive (Banana > Apple)
int result3 = a.CompareTo(c);  // 0 (Apple == Apple)

// Sorting with CompareTo
var words = new[] { "Zebra", "Apple", "Banana" };
System.Array.Sort(words);  // Uses CompareTo internally
// Result: ["Apple", "Banana", "Zebra"]
```

### Case-Insensitive CompareTo

```csharp
string a = "hello";
string b = "HELLO";

// Case-sensitive (default)
int result1 = a.CompareTo(b);  // Negative (lowercase > uppercase in ASCII)

// Case-insensitive using Compare
int result2 = string.Compare(a, b, ignoreCase: true);  // 0 (equal when ignoring case)

// With StringComparison
int result3 = string.Compare(a, b, StringComparison.OrdinalIgnoreCase);  // 0
```

### Sorting Examples

```csharp
var names = new[] { "Charlie", "alice", "Bob" };

// Case-sensitive sort
System.Array.Sort(names);
// ["Bob", "Charlie", "alice"]  (uppercase < lowercase)

// Case-insensitive sort
System.Array.Sort(names, (x, y) => 
    string.Compare(x, y, StringComparison.OrdinalIgnoreCase));
// ["alice", "Bob", "Charlie"]
```

---

## Null and Empty Checks

### IsNullOrEmpty

```csharp
string nullStr = null;
string emptyStr = "";
string validStr = "Hello";

// Check null or empty
bool isNull1 = string.IsNullOrEmpty(nullStr);  // true
bool isEmpty = string.IsNullOrEmpty(emptyStr);  // true
bool isValid = string.IsNullOrEmpty(validStr);  // false

// Practical use
string userInput = GetUserInput();
if (!string.IsNullOrEmpty(userInput)) {
    Process(userInput);
}
```

### IsNullOrWhiteSpace

```csharp
string nullStr = null;
string emptyStr = "";
string whitespaceStr = "   \t\n";
string validStr = "Hello";

// Check null, empty, or whitespace
bool test1 = string.IsNullOrWhiteSpace(nullStr);  // true
bool test2 = string.IsNullOrWhiteSpace(emptyStr);  // true
bool test3 = string.IsNullOrWhiteSpace(whitespaceStr);  // true
bool test4 = string.IsNullOrWhiteSpace(validStr);  // false

// Practical use
string userInput = GetUserInput();
if (!string.IsNullOrWhiteSpace(userInput)) {
    Process(userInput.Trim());
}
```

### Null Coalescing

```csharp
string? userInput = GetUserInput();  // May be null

// Use default if null
string value = userInput ?? "Default Value";

// Null conditional
string? text = someObject?.GetText();
int? length = text?.Length;  // null if text is null

// Chain null coalescing
string result = userInput ?? GetAlternate() ?? "Ultimate Default";
```

---

## Contains vs StartsWith vs EndsWith

### Pattern Matching

```csharp
string email = "user@example.com";

// Contains substring
bool hasAt = email.Contains("@");  // true
bool hasDomain = email.Contains("example");  // true

// Starts with pattern
bool isEmail = email.StartsWith("user");  // true
bool isInvalid = email.StartsWith("invalid");  // false

// Ends with pattern
bool isDotCom = email.EndsWith(".com");  // true
bool isDotOrg = email.EndsWith(".org");  // false

// Case-insensitive versions (C# 8+)
bool hasAtCI = email.Contains("@", StringComparison.OrdinalIgnoreCase);  // true
bool startsCI = email.StartsWith("USER", StringComparison.OrdinalIgnoreCase);  // true
bool endsCI = email.EndsWith(".COM", StringComparison.OrdinalIgnoreCase);  // true
```

---

## Prefix and Suffix Comparison

### File Type Checking

```csharp
string filename = "document.pdf";

// Check file type
bool isPdf = filename.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);  // true
bool isDoc = filename.EndsWith(".docx", StringComparison.OrdinalIgnoreCase);  // false

// Multiple file types
string[] imageExtensions = { ".jpg", ".png", ".gif", ".bmp" };
bool isImage = imageExtensions.Any(ext => 
    filename.EndsWith(ext, StringComparison.OrdinalIgnoreCase));  // false
```

### URL/Protocol Checking

```csharp
string url = "https://www.example.com";

// Protocol check
bool isHttps = url.StartsWith("https://");  // true
bool isHttp = url.StartsWith("http://");  // true

// Domain check
bool isExample = url.Contains("example.com");  // true
bool isWWW = url.Contains("www.");  // true
```

---

## Advanced Comparison Scenarios

### Case-Insensitive Collections

```csharp
// Dictionary with case-insensitive keys
var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
dict["Name"] = 1;
dict["AGE"] = 2;

// Access with any case
int value1 = dict["name"];  // 1 (found despite case difference)
int value2 = dict["age"];  // 2
```

### Sorting with Custom Comparison

```csharp
var items = new[] { "apple", "APPLE", "Banana", "banana" };

// Sort case-sensitive
System.Array.Sort(items);
// ["APPLE", "Banana", "apple", "banana"]

// Sort case-insensitive
System.Array.Sort(items, StringComparer.OrdinalIgnoreCase);
// ["apple", "APPLE", "banana", "Banana"]

// Sort by length then alphabetically
System.Array.Sort(items, (a, b) => {
    int lenCompare = a.Length.CompareTo(b.Length);
    return lenCompare != 0 ? lenCompare : 
        string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
});
```

---

## Performance Considerations

### Comparison Performance

```csharp
// Fast - Ordinal (no culture consideration)
bool same = text1.Equals(text2, StringComparison.Ordinal);

// Slower - Culture-aware
bool sameCulture = text1.Equals(text2, StringComparison.CurrentCulture);

// Fastest for repeated comparisons - use StringComparer
var comparer = StringComparer.OrdinalIgnoreCase;
bool result1 = comparer.Equals(text1, text2);
bool result2 = comparer.Equals(text3, text4);
```

### Avoid Repeated Conversions

```csharp
// INEFFICIENT
foreach (var item in items) {
    if (item.ToLower() == searchTerm.ToLower()) { }
}

// EFFICIENT
foreach (var item in items) {
    if (item.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) { }
}
```

---

## Common Mistakes

### ❌ Case Sensitivity Assumption

```csharp
// May fail if case differs
if (userRole == "admin") { }  // Case-sensitive!
```

✓ **Use case-insensitive comparison:**
```csharp
if (userRole.Equals("admin", StringComparison.OrdinalIgnoreCase)) { }
```

### ❌ Comparing Null Without Check

```csharp
string? input = GetInput();
bool same = input == "expected";  // May work, but not explicit
```

✓ **Be explicit:**
```csharp
bool same = input?.Equals("expected", StringComparison.OrdinalIgnoreCase) ?? false;
```

### ❌ Using ToString() for Comparison

```csharp
if (obj.ToString() == "expected") { }  // Fragile
```

✓ **Use type-specific comparison:**
```csharp
if (obj is StringData data && data.Value == "expected") { }
```

---

## Summary of Comparison Methods

| Method | Purpose | Case Sensitive |
|--------|---------|---|
| `==` | Equality operator | Sensitive |
| `.Equals()` | Equality method | Sensitive (or option) |
| `.CompareTo()` | Ordering | Sensitive |
| `.Contains()` | Substring search | Sensitive (or option) |
| `.StartsWith()` | Prefix check | Sensitive (or option) |
| `.EndsWith()` | Suffix check | Sensitive (or option) |
| `string.Compare()` | Comparison | Sensitive (or option) |

---

## Best Practices

✓ Use `StringComparison.OrdinalIgnoreCase` for case-insensitive
✓ Use `StringComparison.Ordinal` for culture-independent
✓ Always check for null before comparison
✓ Use `string.IsNullOrWhiteSpace()` for validation
✓ Use `StringComparer` for collections
✓ Avoid repeated case conversions

---

## Next Steps

1. Study String Formatting
2. Learn String Validation
3. Master StringBuilder Performance
