# Strings - Easy Interview Questions

## Q1: What is string immutability? Why does it matter?

**Answer:**
Strings are immutable - once created, they cannot be changed. Any modification creates a new string object.

```csharp
string text = "Hello";
string upper = text.ToUpper();  // New object, original unchanged

// Performance implication: Every operation allocates new memory
string result = "";
for (int i = 0; i < 100; i++) {
    result += i;  // Creates 100 intermediate strings!
}
```

**Why it matters:**
- Safety: Cannot accidentally modify shared strings
- Performance: Need StringBuilder for multiple operations
- Caching: Strings can be safely cached/reused

---

## Q2: Explain the difference between Length and Count()

**Answer:**
```csharp
string text = "Hello";

// Length - property, O(1)
int len = text.Length;  // 5 - instant

// Count() - LINQ method, O(n)
int count = text.Count(c => true);  // Iterates each character

// Use Length for simple count
// Use Count() with conditions: text.Count(c => c > 'a')
```

---

## Q3: What methods can find a substring?

**Answer:**
```csharp
string text = "Hello, World!";

// Contains - returns bool
bool has = text.Contains("World");  // true

// IndexOf - returns position
int pos = text.IndexOf("World");  // 7
if (pos >= 0) { /* found */ }

// StartsWith/EndsWith
bool starts = text.StartsWith("Hello");  // true
bool ends = text.EndsWith("!");  // true

// Use Contains for existence
// Use IndexOf when you need position
```

---

## Q4: How do you safely access a character at an index?

**Answer:**
```csharp
// WRONG - May throw IndexOutOfRangeException
char c = text[100];

// RIGHT - Check bounds
if (index >= 0 && index < text.Length) {
    char c = text[index];
}

// RIGHT - Using range (C# 8+)
char? last = text.ElementAtOrDefault(text.Length - 1);

// RIGHT - Using safe access
char first = text.FirstOrDefault();  // Default char if empty
```

---

## Q5: What's the best way to join strings?

**Answer:**
```csharp
var items = new[] { "apple", "banana", "orange" };

// BEST - Efficient and clean
string result = string.Join(", ", items);  // "apple, banana, orange"

// AVOID - Inefficient
string result = "";
foreach (var item in items) {
    if (result != "") result += ", ";
    result += item;  // O(n²)
}

// Use string.Join for collections
// Use StringBuilder only if special formatting needed
```

---

## Q6: How do you handle null strings safely?

**Answer:**
```csharp
string? input = GetInput();

// Check for null
if (input != null) {
    Process(input);
}

// Using null conditional
int? length = input?.Length;

// Using null coalescing
string value = input ?? "default";

// Check null and whitespace
if (!string.IsNullOrWhiteSpace(input)) {
    Process(input.Trim());
}
```

---

## Q7: Explain case-insensitive comparison

**Answer:**
```csharp
string a = "Hello";
string b = "hello";

// WRONG - Case-sensitive
if (a == b) { }  // false

// RIGHT - Case-insensitive
if (a.Equals(b, StringComparison.OrdinalIgnoreCase)) { }  // true

// With ToLower (inefficient)
if (a.ToLower() == b.ToLower()) { }

// Use StringComparison parameter for efficiency
```

---

## Q8: What does Substring do?

**Answer:**
```csharp
string text = "Hello, World!";

// From index to end
string sub1 = text.Substring(7);  // "World!"

// From index with length
string sub2 = text.Substring(0, 5);  // "Hello"

// Modern approach using ranges
string sub3 = text[0..5];  // "Hello"
string sub4 = text[7..];  // "World!"

// Always check bounds before Substring
// Ranges handle bounds gracefully
```

---

## Q9: How do you split a string?

**Answer:**
```csharp
string csv = "apple,banana,orange";

// Split by delimiter
string[] items = csv.Split(',');
// ["apple", "banana", "orange"]

// Remove empty entries
string text = "a,,b,,c";
string[] cleaned = text.Split(',', StringSplitOptions.RemoveEmptyEntries);
// ["a", "b", "c"]

// Split with multiple delimiters
string data = "apple;banana|orange,grape";
char[] delims = { ';', '|', ',' };
string[] result = data.Split(delims);
```

---

## Q10: What are format specifiers?

**Answer:**
```csharp
// Currency
decimal price = 19.99m;
string c = $"{price:C}";  // "$19.99"

// Decimal places
string d = $"{price:F2}";  // "19.99"

// Percentage
double pct = 0.85;
string p = $"{pct:P}";  // "85.00%"

// Numbers with zeros
int id = 42;
string padded = $"{id:D5}";  // "00042"

// Hexadecimal
string hex = $"{255:X}";  // "FF"

// Date/Time
DateTime now = DateTime.Now;
string date = $"{now:yyyy-MM-dd}";  // "2024-08-03"
```

---

## Summary of Easy Concepts

✓ Strings are immutable
✓ Length property O(1)
✓ Contains, IndexOf for finding
✓ Safe character access requires bounds check
✓ String.Join for efficient joining
✓ Handle null safely
✓ Case-insensitive comparison with StringComparison
✓ Substring and ranges for extraction
✓ Split with delimiters
✓ Format specifiers for display

---

## Next Steps

1. Practice writing code
2. Move to Medium questions
