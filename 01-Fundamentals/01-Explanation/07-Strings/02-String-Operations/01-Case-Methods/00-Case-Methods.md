# String Case Methods

## Overview
Convert strings between uppercase, lowercase, and title case. Understand cultural considerations and proper implementation.

---

## ToUpper and ToLower

### Basic Conversion

```csharp
string text = "Hello World";

// To uppercase
string upper = text.ToUpper();  // "HELLO WORLD"

// To lowercase
string lower = text.ToLower();  // "hello world"

// Verify conversion
Console.WriteLine($"Original: {text}");
Console.WriteLine($"Upper: {upper}");
Console.WriteLine($"Lower: {lower}");
```

### With Different String Types

```csharp
// Numeric characters unchanged
string withNumbers = "Hello123World456";
string upper = withNumbers.ToUpper();  // "HELLO123WORLD456"
string lower = withNumbers.ToLower();  // "hello123world456"

// Special characters unchanged
string special = "Hello-World_2024!";
string upperSpecial = special.ToUpper();  // "HELLO-WORLD_2024!"

// Punctuation unchanged
string punctuated = "Hello. World? Yes!";
string upperPunct = punctuated.ToUpper();  // "HELLO. WORLD? YES!"
```

### Empty and Null Strings

```csharp
// Empty string
string empty = "";
string emptyUpper = empty.ToUpper();  // ""
string emptyLower = empty.ToLower();  // ""

// Null string - careful!
string? nullStr = null;
// nullStr.ToUpper();  // NullReferenceException!

// Safe conversion
string? input = GetUserInput();
string upper = input?.ToUpper() ?? "";  // Safe null coalescing
```

---

## Invariant vs Culture-Specific

### Culture-Invariant (Recommended)

```csharp
// Invariant - always same result
string text = "hello";
string upper = text.ToUpperInvariant();  // "HELLO"
string lower = "HELLO".ToLowerInvariant();  // "hello"

// Use for: identifiers, keys, comparisons
string language = "english";
string normalized = language.ToLowerInvariant();  // Always "english"
```

### Culture-Specific

```csharp
// Current culture
string text = "hello";
string upper = text.ToUpper();  // Depends on system culture

// Specific culture
CultureInfo tr = CultureInfo.GetCultureInfo("tr-TR");
string turkishText = "i";
string turkishUpper = turkishText.ToUpper(tr);  // "İ" (capital I with dot)

// English culture
CultureInfo en = CultureInfo.GetCultureInfo("en-US");
string englishUpper = turkishText.ToUpper(en);  // "I" (regular I)
```

### Why Culture Matters

```csharp
// Turkish has two types of I
char i = 'i';
char I = 'I';
char iDotted = 'ı';
char IDotted = 'İ';

// Turkish: i <-> İ, ı <-> I
// English: i <-> I, ı <-> I (or treated differently)

// Example: Turkish uppercase
string sample = "istanbul";
CultureInfo tr = CultureInfo.GetCultureInfo("tr-TR");
string turkishUpper = sample.ToUpper(tr);  // "İSTANBUL" (with dotted I)

// English uppercase
CultureInfo en = CultureInfo.GetCultureInfo("en-US");
string englishUpper = sample.ToUpper(en);  // "ISTANBUL" (without dot)
```

---

## Character-by-Character Conversion

### First Letter Uppercase

```csharp
// Manual approach
string text = "hello world";
if (text.Length > 0) {
    string capitalized = char.ToUpper(text[0]) + text.Substring(1);
    // "Hello world"
}

// Helper method
static string Capitalize(string input) {
    if (string.IsNullOrEmpty(input)) return input;
    return char.ToUpper(input[0]) + input.Substring(1);
}

string result = Capitalize("hello");  // "Hello"
```

### Title Case (Pascal Case)

```csharp
// Manual - capitalize first letter of each word
static string ToTitleCase(string input) {
    if (string.IsNullOrEmpty(input)) return input;
    
    var words = input.Split(' ');
    for (int i = 0; i < words.Length; i++) {
        if (words[i].Length > 0) {
            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
        }
    }
    return string.Join(" ", words);
}

string result = ToTitleCase("hello world example");  // "Hello World Example"
```

### Using TextInfo

```csharp
using System.Globalization;

string text = "hello world";

// TextInfo for proper casing
TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
string titleCase = textInfo.ToTitleCase(text);  // "Hello World"

// Invariant
TextInfo invariantInfo = CultureInfo.InvariantCulture.TextInfo;
string invariantTitle = invariantInfo.ToTitleCase(text);  // "Hello World"
```

---

## Case-Insensitive Comparison

### Compare Without Case

```csharp
string text1 = "Hello";
string text2 = "hello";

// Case-insensitive equality
bool sameIgnoreCase = text1.Equals(text2, StringComparison.OrdinalIgnoreCase);
// true

// Case-sensitive equality
bool sameCaseSensitive = text1.Equals(text2, StringComparison.Ordinal);
// false

// Using ToLower for comparison
bool same = text1.ToLower() == text2.ToLower();  // true
```

### StringComparison Options

```csharp
string a = "Hello";
string b = "HELLO";

// Ordinal - culture-independent, case-sensitive
bool caseSensitive = a.Equals(b, StringComparison.Ordinal);  // false

// OrdinalIgnoreCase - culture-independent, case-insensitive
bool caseInsensitive = a.Equals(b, StringComparison.OrdinalIgnoreCase);  // true

// CurrentCulture - culture-dependent, case-sensitive
bool cultureSensitive = a.Equals(b, StringComparison.CurrentCulture);  // false

// CurrentCultureIgnoreCase - culture-dependent, case-insensitive
bool cultureIgnoreCase = a.Equals(b, StringComparison.CurrentCultureIgnoreCase);  // true
```

---

## Practical Examples

### Case-Insensitive Search

```csharp
// Search with case insensitivity
string text = "Hello World Hello";
string search = "hello";

// Using Contains with culture
bool found = text.Contains(search, StringComparison.OrdinalIgnoreCase);  // true

// Manual approach
bool foundManual = text.ToLower().Contains(search.ToLower());  // true

// IndexOf with culture
int index = text.IndexOf(search, StringComparison.OrdinalIgnoreCase);  // 0
```

### Normalizing Input

```csharp
// Normalize user input for comparison/storage
string userInput = "  Hello World  ";

// Clean and normalize
string normalized = userInput.Trim().ToLowerInvariant();
// "hello world"

// Check against list
var validInputs = new[] { "hello world", "goodbye world", "test" };
bool valid = validInputs.Contains(normalized);  // true
```

### Email Case-Insensitive Storage

```csharp
// Store emails in lowercase for consistency
string userEmail = "Alice@Example.COM";
string normalized = userEmail.ToLowerInvariant();
// Store in database: "alice@example.com"

// When searching
string searchEmail = "ALICE@EXAMPLE.COM";
bool found = normalized == searchEmail.ToLowerInvariant();  // true
```

---

## Performance Considerations

### Repeated Conversions

```csharp
// INEFFICIENT - Multiple conversions in loop
foreach (var item in items) {
    if (item.ToLower() == "test") {  // Converts each time
        Process(item);
    }
}

// EFFICIENT - Convert once
string searchTerm = "test".ToLower();
foreach (var item in items) {
    if (item.ToLower() == searchTerm) {  // Or use OrdinalIgnoreCase
        Process(item);
    }
}

// BEST - Use comparison option
foreach (var item in items) {
    if (item.Equals("test", StringComparison.OrdinalIgnoreCase)) {
        Process(item);
    }
}
```

### String Allocation

```csharp
// Each ToUpper/ToLower creates new string
string original = "Hello";
string upper1 = original.ToUpper();  // New object
string upper2 = original.ToUpper();  // Different new object

// If needed multiple times, cache it
string upper = original.ToUpper();
// Use 'upper' multiple times
```

---

## Common Mistakes

### ❌ Assuming ToUpper/ToLower on Null

```csharp
string? input = GetUserInput();
string upper = input.ToUpper();  // Throws if null!
```

✓ **Safe approach:**
```csharp
string upper = input?.ToUpper() ?? "";
```

### ❌ Culture Issues

```csharp
// Might be culture-dependent
string upper = text.ToUpper();

// Use invariant for consistency
string upper = text.ToUpperInvariant();
```

### ❌ Case for Comparison Instead of StringComparison

```csharp
// Inefficient and allocates strings
if (text.ToLower() == searchTerm.ToLower()) { }

// Better
if (text.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)) { }
```

---

## Summary of Case Methods

| Method | Purpose | Use Case |
|--------|---------|----------|
| `ToUpper()` | Uppercase string | Display, culture-specific |
| `ToLower()` | Lowercase string | Display, culture-specific |
| `ToUpperInvariant()` | Uppercase invariant | Keys, identifiers |
| `ToLowerInvariant()` | Lowercase invariant | Normalization |
| `Equals(..., IgnoreCase)` | Compare ignoring case | Validation, search |

---

## Best Practices

✓ Use invariant methods for identifiers/keys
✓ Use StringComparison for case-insensitive comparison
✓ Always check for null before case conversion
✓ Cache converted strings if used multiple times
✓ Consider culture implications
✓ Use TextInfo for proper title casing

---

## Next Steps

1. Study Search Methods
2. Learn Manipulation Methods
3. Explore String Patterns
