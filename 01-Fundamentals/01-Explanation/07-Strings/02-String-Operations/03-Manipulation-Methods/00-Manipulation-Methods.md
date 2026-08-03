# String Manipulation Methods

## Overview
Extract, replace, trim, and modify strings. Master essential string manipulation techniques.

---

## Substring Extraction

### Substring Method

```csharp
string text = "Hello, World!";

// Substring from index to end
string sub1 = text.Substring(7);  // "World!"

// Substring with length
string sub2 = text.Substring(0, 5);  // "Hello"
string sub3 = text.Substring(7, 5);  // "World"

// Safe substring with bounds check
int startIndex = 5;
int length = 10;
if (startIndex >= 0 && startIndex + length <= text.Length) {
    string sub = text.Substring(startIndex, length);  // ", World!"
}
```

### Range Operator (C# 8+)

```csharp
string text = "Hello, World!";

// Range from start
string first5 = text[0..5];  // "Hello"

// Range to end
string fromIndex = text[7..];  // "World!"

// Range middle
string middle = text[1..6];  // "ello,"

// From end (negative indexing)
string last = text[^1..];  // "!"
string lastN = text[^6..];  // "World!"

// Combining
string excerpt = text[7..^1];  // "World"
```

### Extract Parts by Delimiter

```csharp
string csv = "apple,banana,orange";

// Split by delimiter
string[] fruits = csv.Split(',');
// ["apple", "banana", "orange"]

// Get first part
string first = fruits[0];  // "apple"

// Get last part
string last = fruits[fruits.Length - 1];  // "orange"
```

---

## Split Method

### Basic Split

```csharp
string text = "apple,banana,orange";

// Split by single character
string[] items = text.Split(',');
// ["apple", "banana", "orange"]

// Split by string
string text2 = "one::two::three";
string[] parts = text2.Split("::");
// ["one", "two", "three"]

// Split by multiple characters (or any)
string csv2 = "apple;banana,orange|grape";
char[] delimiters = { ';', ',', '|' };
string[] items2 = csv2.Split(delimiters);
// ["apple", "banana", "orange", "grape"]
```

### Split with Options

```csharp
string text = "apple, , banana,  orange";

// Include empty entries (default)
string[] all = text.Split(',');
// ["apple", " ", " banana", "  orange"]

// Remove empty entries
string[] noEmpty = text.Split(',', StringSplitOptions.RemoveEmptyEntries);
// ["apple", "banana", "orange"]

// Trim whitespace
string[] trimmed = text.Split(',', StringSplitOptions.RemoveEmptyEntries)
    .Select(s => s.Trim())
    .ToArray();
// ["apple", "banana", "orange"]
```

### Split by Lines

```csharp
string multiline = @"Line 1
Line 2
Line 3";

// Split by newline
string[] lines = multiline.Split(new[] { "\r\n", "\r", "\n" }, 
    StringSplitOptions.None);
// ["Line 1", "Line 2", "Line 3"]

// Using Environment.NewLine
string[] lines2 = multiline.Split(Environment.NewLine);
```

---

## Replace Method

### Simple Replace

```csharp
string text = "Hello World";

// Replace all occurrences
string replaced = text.Replace("World", "C#");  // "Hello C#"
string replaced2 = text.Replace("o", "0");  // "Hell0 W0rld"

// Replace multiple times
string text2 = "aaa";
string result = text2.Replace("a", "b");  // "bbb"
```

### Case-Insensitive Replace

```csharp
string text = "Hello HELLO hello";

// Case-sensitive (default)
string replaced = text.Replace("hello", "hi");  // "Hello HELLO hi"

// Case-insensitive (using Regex)
using System.Text.RegularExpressions;
string replacedCI = Regex.Replace(text, "hello", "hi", RegexOptions.IgnoreCase);
// "hi hi hi"
```

### Replace with Regex

```csharp
using System.Text.RegularExpressions;

string text = "The price is $19.99";

// Replace pattern
string result = Regex.Replace(text, @"\$\d+\.\d{2}", "$25.00");
// "The price is $25.00"

// Replace all numbers with X
string masked = Regex.Replace("123-456-7890", @"\d", "X");
// "XXX-XXX-XXXX"

// Capture groups
string phone = "123-456-7890";
string formatted = Regex.Replace(phone, @"(\d{3})-(\d{3})-(\d{4})", "($1) $2-$3");
// "(123) 456-7890"
```

---

## Trimming Methods

### Trim Whitespace

```csharp
string text = "  Hello World  ";

// Trim both sides
string trimmed = text.Trim();  // "Hello World"

// Trim start
string trimStart = text.TrimStart();  // "Hello World  "

// Trim end
string trimEnd = text.TrimEnd();  // "  Hello World"

// With spaces/tabs/newlines
string withWhitespace = "\n\t  Hello  \t\n";
string cleaned = withWhitespace.Trim();  // "Hello"
```

### Trim Specific Characters

```csharp
string text = "###Hello###";

// Trim specific character
string trimmed = text.Trim('#');  // "Hello"
string trimStart = text.TrimStart('#');  // "Hello###"
string trimEnd = text.TrimEnd('#');  // "###Hello"

// Trim specific characters (any of them)
string text2 = "<<< Hello >>>";
char[] brackets = { '<', '>' };
string trimmed2 = text2.Trim(brackets);  // "Hello"
```

---

## Remove and Insert

### Remove Characters

```csharp
string text = "Hello, World!";

// Remove from index (to end)
string removed1 = text.Remove(5);  // "Hello"

// Remove from index with length
string removed2 = text.Remove(5, 2);  // "HelloWorld!"
string removed3 = text.Remove(12, 1);  // "Hello, World"

// Remove specific characters
string text2 = "a1b2c3d4";
string noDigits = string.Concat(text2.Where(c => !char.IsDigit(c)));
// "abcd"
```

### Insert Text

```csharp
string text = "Hello World";

// Insert at index
string inserted = text.Insert(5, " Beautiful");  // "Hello Beautiful World"
string inserted2 = text.Insert(0, "Say: ");  // "Say: Hello World"
string inserted3 = text.Insert(text.Length, "!");  // "Hello World!"

// Insert with condition
if (text.IndexOf("World") != -1) {
    int index = text.IndexOf("World");
    string withPrefix = text.Insert(index, "Beautiful ");
    // "Hello Beautiful World"
}
```

---

## Padding Methods

### PadLeft and PadRight

```csharp
string text = "42";

// Pad left (numbers right-aligned)
string padLeft = text.PadLeft(5);  // "   42"
string padLeftZero = text.PadLeft(5, '0');  // "00042"

// Pad right
string padRight = text.PadRight(5);  // "42   "
string padRightDot = text.PadRight(5, '.');  // "42..."

// Common uses
string id = "123";
string formatted = id.PadLeft(6, '0');  // "000123"

// Table formatting
var rows = new[] { "ID", "Name", "Value" };
foreach (var row in rows) {
    Console.WriteLine(row.PadRight(15) + "Value");
}
```

---

## Concatenation and Joining

### String.Concat

```csharp
// Concatenate multiple strings
string result = string.Concat("Hello", " ", "World");  // "Hello World"

// With array
string[] words = { "Apple", "Banana", "Orange" };
string result2 = string.Concat(words);  // "AppleBananaOrange"

// With IEnumerable
List<string> items = new() { "a", "b", "c" };
string result3 = string.Concat(items);  // "abc"
```

### String.Join

```csharp
// Join with separator
string[] words = { "apple", "banana", "orange" };
string joined = string.Join(", ", words);  // "apple, banana, orange"

// With different separators
string csv = string.Join(",", words);  // "apple,banana,orange"
string pipe = string.Join(" | ", words);  // "apple | banana | orange"

// With numbers
int[] nums = { 1, 2, 3, 4, 5 };
string numStr = string.Join("-", nums);  // "1-2-3-4-5"

// With LINQ
var doubled = nums.Select(n => n * 2);
string result = string.Join(", ", doubled);  // "2, 4, 6, 8, 10"
```

---

## StringBuilder for Efficiency

### When to Use StringBuilder

```csharp
// INEFFICIENT - creates many strings
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i.ToString();  // O(n²) complexity
}

// EFFICIENT - StringBuilder
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);  // O(n)
}
string result = sb.ToString();
```

### StringBuilder Methods

```csharp
StringBuilder sb = new StringBuilder();

// Append
sb.Append("Hello");
sb.Append(" ");
sb.Append("World");

// AppendLine (adds newline)
sb.AppendLine("Line 1");
sb.AppendLine("Line 2");

// Insert
sb.Insert(0, "Start: ");

// Replace
sb.Replace("World", "C#");

// Remove
sb.Remove(0, 7);  // Remove "Start: "

// ToString
string result = sb.ToString();

// Clear and reuse
sb.Clear();
sb.Append("New content");
```

---

## Common Patterns

### Remove Whitespace

```csharp
string text = "H e l l o";

// Remove all spaces
string noSpaces = text.Replace(" ", "");  // "Hello"

// Using Regex
string noWhitespace = Regex.Replace(text, @"\s+", "");  // "Hello"
```

### Remove Duplicates

```csharp
string text = "aabbccdd";

// Remove consecutive duplicates
string unique = Regex.Replace(text, @"(.)\1+", "$1");  // "abcd"
```

### Format Phone Number

```csharp
string phone = "1234567890";

// Format: (123) 456-7890
string formatted = Regex.Replace(phone, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
```

---

## Common Mistakes

### ❌ String Modification in Loop

```csharp
string result = "";
for (int i = 0; i < 10000; i++) {
    result += i;  // Very slow! O(n²)
}
```

✓ **Use StringBuilder:**
```csharp
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 10000; i++) {
    sb.Append(i);  // Fast! O(n)
}
string result = sb.ToString();
```

### ❌ Replace Without Checking Existence

```csharp
string result = text.Replace("old", "new");  // If "old" doesn't exist, no error but no change
```

✓ **Check first if critical:**
```csharp
if (text.Contains("old")) {
    string result = text.Replace("old", "new");
}
```

---

## Performance Summary

| Operation | Efficient | Notes |
|-----------|-----------|-------|
| Single replace | string.Replace | Fine for single operations |
| Many replaces | StringBuilder | Use for loop operations |
| String split | string.Split | Built-in, fast |
| Complex patterns | Regex | Use for pattern matching |
| Concatenation | string.Join | Better than multiple + |

---

## Best Practices

✓ Use `string.Join()` for concatenation
✓ Use `StringBuilder` for loops with modifications
✓ Use `string.Split()` with `RemoveEmptyEntries` for clean data
✓ Use ranges `[..]` for C# 8+ substring extraction
✓ Use `Trim()` to clean user input
✓ Use `Regex` for complex pattern manipulation

---

## Next Steps

1. Study String Patterns
2. Learn String Comparison
3. Explore Validation
