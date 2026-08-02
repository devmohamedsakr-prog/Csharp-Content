# Strings and String Manipulation

## Overview
Strings are immutable sequences of characters. Each modification creates a new string.

---

## String Basics

### Creating Strings

```csharp
// String literal
string message = "Hello, World!";

// Empty string
string empty = "";
string empty2 = string.Empty;

// String from character array
char[] chars = { 'H', 'e', 'l', 'l', 'o' };
string fromChars = new string(chars);  // "Hello"

// Multi-line string (C# 6+)
string multiline = @"Line 1
Line 2
Line 3";

// Raw string literal (C# 11+)
string raw = """
This is a "raw" string
with special characters: \n \t
""";
```

### String Interpolation

```csharp
string name = "Alice";
int age = 30;

// String interpolation (C# 6+)
string message = $"My name is {name} and I'm {age} years old";

// Expressions in interpolation
string result = $"2 + 2 = {2 + 2}";

// Formatting
decimal price = 19.99m;
string formatted = $"Price: {price:C}";  // "Price: $19.99"

// Conditional
string status = $"Age: {(age >= 18 ? "Adult" : "Minor")}";
```

### String Concatenation

```csharp
string first = "Hello";
string second = "World";

// Using +
string result1 = first + " " + second;  // "Hello World"

// Using string.Concat
string result2 = string.Concat(first, " ", second);

// Using StringBuilder (for many concatenations)
StringBuilder sb = new StringBuilder();
sb.Append("Hello");
sb.Append(" ");
sb.Append("World");
string result3 = sb.ToString();  // "Hello World"
```

---

## String Properties and Methods

### Properties

```csharp
string text = "Hello";

// Length
int length = text.Length;  // 5

// Accessing characters
char first = text[0];  // 'H'
char last = text[text.Length - 1];  // 'o'
```

### Case Methods

```csharp
string text = "Hello World";

// To uppercase
string upper = text.ToUpper();  // "HELLO WORLD"

// To lowercase
string lower = text.ToLower();  // "hello world"

// First letter uppercase
string capitalized = char.ToUpper(text[0]) + text.Substring(1);
```

### Searching Methods

```csharp
string text = "Hello World";

// Contains
bool hasWorld = text.Contains("World");  // true

// IndexOf - find position
int index = text.IndexOf("World");  // 6
int notFound = text.IndexOf("xyz");  // -1

// StartsWith
bool startsHello = text.StartsWith("Hello");  // true

// EndsWith
bool endsWorld = text.EndsWith("World");  // true
```

### Substring Methods

```csharp
string text = "Hello World";

// Substring - extract part
string sub1 = text.Substring(0, 5);  // "Hello"
string sub2 = text.Substring(6);  // "World"

// Split - break into parts
string[] words = text.Split(' ');  // ["Hello", "World"]

string csv = "apple,banana,orange";
string[] fruits = csv.Split(',');  // ["apple", "banana", "orange"]
```

### Replacement and Trimming

```csharp
string text = "Hello World";

// Replace
string replaced = text.Replace("World", "C#");  // "Hello C#"

// Trimming
string padded = "  Hello  ";
string trimmed = padded.Trim();  // "Hello"
string trimStart = padded.TrimStart();  // "Hello  "
string trimEnd = padded.TrimEnd();  // "  Hello"

// Remove
string removed = "Hello123".Remove(5);  // "Hello"

// Insert
string inserted = "Hello World".Insert(5, " C#");  // "Hello C# World"
```

---

## String Comparison

```csharp
string str1 = "Hello";
string str2 = "Hello";
string str3 = "HELLO";

// Equals - case sensitive
bool same1 = str1 == str2;  // true
bool same2 = str1.Equals(str2);  // true

// Equals - case insensitive
bool sameIgnoreCase = str1.Equals(str3, StringComparison.OrdinalIgnoreCase);  // true

// CompareTo
int comparison = str1.CompareTo(str3);
// 0 = equal, negative = str1 < str3, positive = str1 > str3

// Null check
string nullStr = null;
bool isEmpty = string.IsNullOrEmpty(nullStr);  // true
bool isWhiteSpace = string.IsNullOrWhiteSpace("   ");  // true
```

---

## String Formatting

### Format Method

```csharp
// Basic formatting
string formatted = string.Format("Hello {0}, you are {1} years old", "Alice", 30);
// "Hello Alice, you are 30 years old"

// Multiple placeholders
string msg = string.Format("{0} {1} {0}", "A", "B");  // "A B A"
```

### Format Specifiers

```csharp
// Numbers
int number = 42;
Console.WriteLine($"{number:D5}");  // "00042" (padded)
Console.WriteLine($"{number:X}");   // "2A" (hexadecimal)

// Currency
decimal price = 19.99m;
Console.WriteLine($"{price:C}");    // "$19.99"

// Percentage
double percent = 0.85;
Console.WriteLine($"{percent:P}");  // "85.00%"

// Date/Time
DateTime now = DateTime.Now;
Console.WriteLine($"{now:yyyy-MM-dd}");  // "2024-08-02"
Console.WriteLine($"{now:hh:mm:ss}");    // "14:30:45"
```

---

## String Validation

```csharp
string email = "user@example.com";

// Check if empty or null
if (string.IsNullOrEmpty(email)) { }

// Check if only whitespace
if (string.IsNullOrWhiteSpace(email)) { }

// Using regex (requires System.Text.RegularExpressions)
using System.Text.RegularExpressions;

bool isEmail = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

bool isNumeric = Regex.IsMatch("12345", @"^\d+$");

bool isAlphabetic = Regex.IsMatch("ABC", @"^[a-zA-Z]+$");
```

---

## StringBuilder (Efficient String Building)

For many string operations, use StringBuilder instead of string concatenation.

```csharp
// Bad - creates many intermediate strings
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i.ToString();  // Inefficient
}

// Good - StringBuilder
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
string result = sb.ToString();

// Methods
sb.Append("text");
sb.AppendLine("line");  // Adds newline
sb.Insert(0, "prefix");
sb.Replace("old", "new");
sb.Remove(0, 5);
sb.Clear();
```

---

## Common String Methods Summary

| Method | Purpose |
|--------|---------|
| `Length` | Get string length |
| `ToUpper()` | Convert to uppercase |
| `ToLower()` | Convert to lowercase |
| `Contains()` | Check if contains substring |
| `IndexOf()` | Find position of substring |
| `Substring()` | Extract part of string |
| `Split()` | Break into parts |
| `Replace()` | Replace text |
| `Trim()` | Remove whitespace |
| `StartsWith()` | Check beginning |
| `EndsWith()` | Check ending |
| `Equals()` | Compare strings |

---

## Best Practices

✓ **Use string interpolation**
```csharp
// Good
string msg = $"Hello {name}, age {age}";

// Less ideal
string msg = "Hello " + name + ", age " + age;
```

✓ **Use StringBuilder for loops**
```csharp
// Good
StringBuilder sb = new StringBuilder();
foreach (var item in items) {
    sb.Append(item);
}

// Bad
string result = "";
foreach (var item in items) {
    result += item;
}
```

✓ **Check null/empty before using**
```csharp
// Good
if (!string.IsNullOrEmpty(input)) {
    Process(input);
}

// Bad - can crash
int length = input.Length;  // NullReferenceException if null
```

---

## Common Mistakes

❌ **Index out of range**
```csharp
string text = "Hello";
char c = text[10];  // IndexOutOfRangeException
```

✓ **Check bounds**
```csharp
if (index >= 0 && index < text.Length) {
    char c = text[index];
}
```

❌ **Inefficient concatenation**
```csharp
string result = "";
for (int i = 0; i < 10000; i++) {
    result += i;  // Very slow
}
```

✓ **Use StringBuilder**
```csharp
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 10000; i++) {
    sb.Append(i);
}
string result = sb.ToString();
```
