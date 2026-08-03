# String Creation and Initialization

## Overview
Strings are immutable sequences of characters. Understand different ways to create strings and the implications of each approach.

---

## String Literals

### Basic String Literals

```csharp
// Simple string
string greeting = "Hello, World!";

// Empty string
string empty = "";
string emptyAlt = string.Empty;  // Preferred

// With escape sequences
string withNewline = "Line 1\nLine 2";
string withTab = "Column1\tColumn2";
string withQuote = "He said \"Hello\"";
string withBackslash = "Path: C:\\Users\\Name";
```

### Verbatim Strings (@)

```csharp
// Backslashes not escaped
string path = @"C:\Users\Name\Documents";

// Newlines preserved (multi-line)
string multiline = @"First line
Second line
Third line";

// Quotes doubled
string withQuote = @"He said ""Hello""";

// Common use: file paths, regex patterns
string regex = @"^\d{3}-\d{2}-\d{4}$";  // SSN pattern
```

### Raw Strings (C# 11+)

```csharp
// Raw string - special characters NOT escaped
string raw = """
This is a "raw" string
with special characters: \n \t \r
and they are literal!
""";

// Multi-line raw strings
string json = """
{
    "name": "Alice",
    "age": 30,
    "email": "alice@example.com"
}
""";

// Useful for: JSON, XML, regex, code snippets
```

### Unicode Strings

```csharp
// Unicode escape sequences
string unicode = "Hello \u0057orld";  // \u0057 = 'W'
string emoji = "Hi \U0001F44B";  // 👋 waving hand

// Direct unicode characters
string chinese = "你好";
string russian = "Привет";
string arabic = "مرحبا";
```

---

## String Constructors

### From Character Arrays

```csharp
// From char array
char[] chars = { 'H', 'e', 'l', 'l', 'o' };
string text = new string(chars);  // "Hello"

// From char array with offset and length
char[] allChars = { 'H', 'e', 'l', 'l', 'o', ' ', 'W', 'o', 'r', 'l', 'd' };
string part = new string(allChars, 0, 5);  // "Hello"
string part2 = new string(allChars, 6, 5);  // "World"

// Repeating character
string repeated = new string('*', 10);  // "**********"
string stars = new string('-', 20);  // "--------------------"
```

### From Bytes

```csharp
// From byte array (encoding required)
byte[] bytes = System.Text.Encoding.UTF8.GetBytes("Hello");
string fromBytes = System.Text.Encoding.UTF8.GetString(bytes);

// Different encodings
byte[] asciiBytes = System.Text.Encoding.ASCII.GetBytes("Hello");
string fromAscii = System.Text.Encoding.ASCII.GetString(asciiBytes);

// UTF-16 (default .NET encoding)
byte[] utf16Bytes = System.Text.Encoding.Unicode.GetBytes("Hello");
string fromUtf16 = System.Text.Encoding.Unicode.GetString(utf16Bytes);
```

---

## String Interpolation

### Basic Interpolation (C# 6+)

```csharp
string name = "Alice";
int age = 30;

// Simple interpolation
string message = $"My name is {name}";  // "My name is Alice"

// Multiple expressions
string info = $"{name} is {age} years old";  // "Alice is 30 years old"

// Expressions in braces
string math = $"2 + 2 = {2 + 2}";  // "2 + 2 = 4"
string comparison = $"Is 5 > 3? {5 > 3}";  // "Is 5 > 3? True"
```

### Advanced Interpolation

```csharp
// Conditional expressions
int score = 85;
string result = $"Score: {score} - {(score >= 90 ? "A" : score >= 80 ? "B" : "C")}";

// Method calls
string text = "hello";
string upper = $"Uppercase: {text.ToUpper()}";  // "Uppercase: HELLO"

// Property access
DateTime now = DateTime.Now;
string dateMsg = $"Today is {now.DayOfWeek}, {now:yyyy-MM-dd}";

// Complex expressions
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
string stats = $"Sum: {numbers.Sum()}, Count: {numbers.Count}, Avg: {numbers.Average():F2}";
```

### Format Specifiers

```csharp
// Decimal places
decimal price = 19.99m;
string formatted = $"Price: {price:F2}";  // "Price: 19.99"

// Currency
string currency = $"Cost: {price:C}";  // "Cost: $19.99"

// Percentage
double percent = 0.85;
string pct = $"Completion: {percent:P}";  // "Completion: 85.00%"

// Numbers with leading zeros
int id = 42;
string padded = $"ID: {id:D5}";  // "ID: 00042"

// Hexadecimal
int hex = 255;
string hexStr = $"Hex: {hex:X}";  // "Hex: FF"
```

---

## String Concatenation

### Using + Operator

```csharp
string first = "Hello";
string second = "World";
string result = first + " " + second;  // "Hello World"

// Multiple concatenations
string msg = "Name: " + name + ", Age: " + age + ", City: " + city;

// With mixed types (implicit ToString)
int num = 42;
string result = "The answer is " + num;  // "The answer is 42"
```

### Using string.Concat

```csharp
// Concatenate multiple strings
string result = string.Concat("Hello", " ", "World");  // "Hello World"

// With array
string[] parts = { "Apple", "Banana", "Orange" };
string fruits = string.Concat(parts);  // "AppleBananaOrange"

// With enumerable
List<string> items = new List<string> { "Red", "Green", "Blue" };
string colors = string.Concat(items);  // "RedGreenBlue"
```

### Using string.Join

```csharp
// Join with separator
string[] words = { "apple", "banana", "orange" };
string joined = string.Join(", ", words);  // "apple, banana, orange"

// With different separators
string paths = string.Join(" | ", words);  // "apple | banana | orange"
string csv = string.Join(",", words);  // "apple,banana,orange"

// With numbers
int[] nums = { 1, 2, 3, 4, 5 };
string numStr = string.Join("-", nums);  // "1-2-3-4-5"
```

---

## String Null and Empty

### Null vs Empty

```csharp
// Null string
string nullStr = null;
bool isNull = nullStr == null;  // true

// Empty string
string empty = "";
string emptyAlt = string.Empty;
bool isEmpty = empty == "";  // true
bool isEmpty2 = empty.Length == 0;  // true

// Null vs empty
string? nullOrEmpty = null;
// nullOrEmpty.Length throws NullReferenceException

string? emptyString = "";
// emptyString.Length == 0 (no error)
```

### Safe Null/Empty Checking

```csharp
// Check both null and empty
if (string.IsNullOrEmpty(input)) {
    Console.WriteLine("Input is null or empty");
}

// Check null, empty, or whitespace
if (string.IsNullOrWhiteSpace(input)) {
    Console.WriteLine("Input is null, empty, or whitespace");
}

// Null coalescing
string value = userInput ?? "default";  // Use default if null

// Null conditional (C# 6+)
int? length = userInput?.Length;  // null if userInput is null
```

---

## Type Conversions

### Converting to String

```csharp
// ToString() on any object
int num = 42;
string numStr = num.ToString();  // "42"

bool flag = true;
string flagStr = flag.ToString();  // "True"

DateTime date = DateTime.Now;
string dateStr = date.ToString();  // Current date/time in default format

// Explicit conversion with format
int val = 123;
string formatted = val.ToString("X");  // "7B" (hexadecimal)
```

### Converting from String

```csharp
// Parse (throws if invalid)
int number = int.Parse("42");  // 42
double price = double.Parse("19.99");  // 19.99
bool flag = bool.Parse("true");  // true

// TryParse (safer)
if (int.TryParse("42", out int result)) {
    Console.WriteLine($"Parsed: {result}");
} else {
    Console.WriteLine("Parse failed");
}

// Convert class
int num2 = Convert.ToInt32("42");  // 42
string numStr = Convert.ToString(42);  // "42"
```

---

## Performance Considerations

### String Creation Overhead

```csharp
// Each creates a new string object
string s1 = "Hello";
string s2 = "Hello";  // Different object (usually)
bool same = s1 == s2;  // true (content comparison)
bool sameRef = ReferenceEquals(s1, s2);  // May be true (interning)

// String interning optimizes memory
string a = "Test";
string b = string.Intern("Test");
bool refSame = ReferenceEquals(a, b);  // true
```

### Interpolation vs Concatenation

```csharp
// Interpolation - cleaner
string msg = $"Hello {name}, age {age}";

// Concatenation - verbose
string msg2 = "Hello " + name + ", age " + age;

// Performance is similar - both compile to similar IL
// Use interpolation for readability
```

---

## Summary of String Creation

✓ **String literals** - Use for simple strings
✓ **String interpolation** - Use for combining data
✓ **Verbatim strings** - Use for paths and multi-line
✓ **Raw strings** - Use for JSON, regex (C# 11+)
✓ **Constructors** - Use for char arrays, repetition
✓ **null/Empty checks** - Always validate input

---

## Next Steps

1. Learn String Properties
2. Study String Methods
3. Master String Operations
4. Explore Patterns
