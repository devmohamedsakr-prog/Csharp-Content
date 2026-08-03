# String Properties and Basic Access

## Overview
Learn how to access string properties and characters, understand indexing, and work with string length.

---

## String Length Property

### Getting Length

```csharp
string text = "Hello, World!";
int length = text.Length;  // 13

// Length includes all characters
string withSpaces = "Hello World";
int len1 = withSpaces.Length;  // 11 (includes space)

string withNumbers = "ABC123";
int len2 = withNumbers.Length;  // 6

// Empty string has length 0
string empty = "";
int emptyLen = empty.Length;  // 0

// Length never negative
// Length is always >= 0
```

### Length with Special Characters

```csharp
// Escape sequences count as single characters
string escaped = "Line1\nLine2";
int len = escaped.Length;  // 12 (not 13)

// Unicode characters
string unicode = "Hello👋";
int lenUnicode = unicode.Length;  // 7 (👋 is 2 UTF-16 characters)

// Tab counts as single character
string withTab = "Col1\tCol2";
int lenTab = withTab.Length;  // 10
```

---

## Character Indexing

### Accessing Individual Characters

```csharp
string text = "Hello";

// Zero-based indexing
char first = text[0];   // 'H'
char second = text[1];  // 'e'
char last = text[4];    // 'o'

// Using Length to get last character
char lastChar = text[text.Length - 1];  // 'o'
char secondLast = text[text.Length - 2];  // 'l'
```

### Safe Character Access

```csharp
string text = "Hello";

// WRONG - May throw IndexOutOfRangeException
char c = text[10];  // Exception!

// RIGHT - Check bounds first
if (text.Length > 0) {
    char first = text[0];
}

// RIGHT - Use conditional access
int index = 2;
if (index >= 0 && index < text.Length) {
    char c = text[index];
}

// RIGHT - Use LINQ ElementAtOrDefault
char? c2 = text.ElementAtOrDefault(10);  // null if out of range
```

### Iterating Characters

```csharp
string text = "Hello";

// Method 1: foreach
foreach (char c in text) {
    Console.WriteLine(c);  // H, e, l, l, o
}

// Method 2: for loop with index
for (int i = 0; i < text.Length; i++) {
    Console.WriteLine($"[{i}] = {text[i]}");
}

// Method 3: LINQ
text.ForEach(c => Console.WriteLine(c));

// Method 4: Select
var chars = text.Select((c, i) => new { Index = i, Char = c });
foreach (var item in chars) {
    Console.WriteLine($"[{item.Index}] = {item.Char}");
}
```

---

## Character Arrays

### Converting String to Char Array

```csharp
string text = "Hello";

// ToCharArray() method
char[] chars = text.ToCharArray();
// Result: ['H', 'e', 'l', 'l', 'o']

// Iterate
foreach (char c in chars) {
    Console.WriteLine(c);
}

// Modify array then create new string
chars[0] = 'J';
string modified = new string(chars);  // "Jello"
```

### Working with Char Arrays

```csharp
string text = "Hello World";

// Convert to array
char[] chars = text.ToCharArray();

// Reverse
Array.Reverse(chars);
string reversed = new string(chars);  // "dlroW olleH"

// Sort
Array.Sort(chars);
string sorted = new string(chars);  // "  HWdellloor"

// Get specific chars
var vowels = text.Where(c => "aeiouAEIOU".Contains(c)).ToArray();
// ['e', 'o', 'o']
```

### Char Operations

```csharp
// Character type checking
char c1 = 'A';
bool isUpper = char.IsUpper(c1);  // true
bool isLower = char.IsLower(c1);  // false

char c2 = '5';
bool isDigit = char.IsDigit(c2);  // true
bool isLetter = char.IsLetter(c2);  // false

char c3 = ' ';
bool isWhiteSpace = char.IsWhiteSpace(c3);  // true

// Character conversion
char upper = char.ToUpper('a');  // 'A'
char lower = char.ToLower('Z');  // 'z'

// Character to code point
int code = (int)'A';  // 65
char fromCode = (char)65;  // 'A'
```

---

## String Slicing and Ranges

### Substring (Classic Approach)

```csharp
string text = "Hello, World!";

// Substring from index
string sub1 = text.Substring(0, 5);  // "Hello"
string sub2 = text.Substring(7);  // "World!"

// Safe substring
int startIndex = 3;
int length = 5;
if (startIndex >= 0 && startIndex + length <= text.Length) {
    string sub = text.Substring(startIndex, length);  // "lo, W"
}
```

### Range Operator (C# 8+)

```csharp
string text = "Hello, World!";

// Range from start to index (exclusive)
string first5 = text[0..5];  // "Hello"

// Range from index to end
string fromIndex = text[7..];  // "World!"

// Range from end
string lastChar = text[^1..];  // "!"
string last5 = text[^5..];  // "orld!"

// Range middle
string middle = text[3..8];  // "lo, W"

// Using variables
int start = 0;
int end = 5;
string range = text[start..end];  // "Hello"
```

### Advanced Range Examples

```csharp
string text = "Hello, World!";

// Every other character
var everyOther = text[0..^0..2].ToList();  // Won't compile - needs loop

// Custom slicing
string[] parts = text.Split(", ");  // ["Hello", "World!"]

// Skip and Take
string skip3 = text[3..];  // "lo, World!"
string take5 = text[..5];  // "Hello"
string skip3Take5 = text[3..8];  // "lo, W"
```

---

## Empty String Properties

### Checking Empty Strings

```csharp
string empty = "";

// Length
int len = empty.Length;  // 0

// IsEmpty (implicit check)
if (empty == string.Empty) { }

// IsNullOrEmpty
bool isNullOrEmpty = string.IsNullOrEmpty(empty);  // true

// Operations on empty strings
string upper = empty.ToUpper();  // "" (no error)
string concat = empty + "test";  // "test"
```

---

## String Equality and Comparison

### Direct Comparison

```csharp
string a = "Hello";
string b = "Hello";
string c = "hello";

// Equality comparison
bool same = a == b;  // true
bool diff = a == c;  // false (case-sensitive)

// Not equal
bool notSame = a != c;  // true
```

### Lexicographic Comparison

```csharp
string a = "Apple";
string b = "Banana";
string c = "apple";

// CompareTo - returns 0 (equal), <0 (less), >0 (greater)
int result1 = a.CompareTo(b);  // Negative (A < B)
int result2 = b.CompareTo(a);  // Positive (B > A)
int result3 = a.CompareTo(a);  // 0 (equal)

// Case-insensitive
int result4 = a.CompareTo(c);  // Negative (case-sensitive)
int result5 = string.Compare(a, c, ignoreCase: true);  // 0
```

---

## Common Properties Summary

| Property/Method | Purpose | Example |
|----------------|---------|---------|
| `Length` | Get string length | `text.Length` |
| `[index]` | Get character at index | `text[0]` |
| `[range]` | Get substring using range | `text[0..5]` |
| `ToCharArray()` | Convert to char array | `text.ToCharArray()` |
| `Substring()` | Extract substring | `text.Substring(0, 5)` |
| `CompareTo()` | Compare strings | `a.CompareTo(b)` |
| `Equals()` | Check equality | `a.Equals(b)` |

---

## Best Practices

✓ **Always check bounds** before accessing by index
✓ **Use Length property** for validation
✓ **Use string.IsNullOrEmpty** before operations
✓ **Use ranges** for cleaner substring extraction
✓ **Validate index ranges** in loops

## Common Mistakes

❌ **Index out of bounds**
```csharp
string text = "Hi";
char c = text[5];  // Exception!
```

✓ **Check bounds first**
```csharp
if (index < text.Length) {
    char c = text[index];
}
```

---

## Next Steps

1. Study Case Methods
2. Learn Search Methods
3. Master Manipulation Methods
4. Explore Patterns
