# String Common Mistakes

## 1. Index Out of Bounds

```csharp
// WRONG
string text = "Hello";
char c = text[10];  // IndexOutOfRangeException!

// RIGHT
if (index >= 0 && index < text.Length) {
    char c = text[index];
}
```

## 2. NullReferenceException on Null String

```csharp
// WRONG
string? input = GetInput();
int len = input.Length;  // May throw if null!

// RIGHT
int len = input?.Length ?? 0;
```

## 3. Inefficient String Concatenation in Loop

```csharp
// WRONG - O(n²) complexity
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i.ToString();  // Very slow!
}

// RIGHT - O(n)
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i.ToString());
}
string result = sb.ToString();
```

## 4. Case Sensitivity in Comparison

```csharp
// WRONG - Case-sensitive fails
if (userRole == "Admin") { }  // Fails if "ADMIN"

// RIGHT - Case-insensitive
if (userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase)) { }
```

## 5. Assuming Non-Null Input

```csharp
// WRONG
string input = GetUserInput();
string upper = input.ToUpper();  // Crashes if null!

// RIGHT
string upper = input?.ToUpper() ?? "";
```

## 6. Forgetting ToString() on StringBuilder

```csharp
// WRONG
StringBuilder sb = new StringBuilder("Hello");
string result = sb;  // Compiler error!

// RIGHT
string result = sb.ToString();
```

## 7. IndexOf Returns -1, Not False

```csharp
// WRONG
if (text.IndexOf("search")) { }  // Won't compile or wrong logic

// RIGHT
if (text.IndexOf("search") >= 0) { }

// BETTER
if (text.Contains("search")) { }
```

## 8. Culture-Dependent Case Conversion

```csharp
// WRONG - May differ by culture
string key = userInput.ToLower();  // Unsafe for keys

// RIGHT - Consistent
string key = userInput.ToLowerInvariant();
```

## 9. Ignoring Whitespace in Validation

```csharp
// WRONG
if (string.IsNullOrEmpty(input)) { }  // Accepts whitespace

// RIGHT
if (string.IsNullOrWhiteSpace(input)) { }
```

## 10. Modifying String (Forgot It's Immutable)

```csharp
// WRONG - Thinking string was modified
string text = "Hello";
text.ToUpper();  // Returns new string, original unchanged
Console.WriteLine(text);  // Still "Hello"!

// RIGHT - Assign result
string text = "Hello";
string upper = text.ToUpper();
Console.WriteLine(upper);  // "HELLO"
```

## 11. Case-Sensitive Dictionary Keys

```csharp
// WRONG - Case-sensitive by default
var dict = new Dictionary<string, int>();
dict["Name"] = 1;
int value = dict["name"];  // KeyNotFoundException!

// RIGHT - Case-insensitive
var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
dict["Name"] = 1;
int value = dict["name"];  // Works! Returns 1
```

## 12. Multiple LINQ Enumerations

```csharp
// WRONG - Multiple iterations
var filtered = text.Where(c => !char.IsDigit(c));
int count = filtered.Count();  // Iterates
var first = filtered.First();  // Iterates again

// RIGHT - Materialize once
var filtered = text.Where(c => !char.IsDigit(c)).ToList();
int count = filtered.Count;
var first = filtered.First();
```

## 13. Regex Not Escaping Special Characters

```csharp
// WRONG - Dot matches any character
Regex.IsMatch(email, @"^[^@]+@[^@]+.[^@]+$");

// RIGHT - Escape the dot
Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
```

## 14. Forgetting to Trim User Input

```csharp
// WRONG
string email = GetUserEmail();  // May have spaces
bool valid = Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");  // Fails!

// RIGHT
string email = GetUserEmail().Trim();
bool valid = Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
```

## 15. Using + for String Join

```csharp
// WRONG - Inefficient
string result = "";
foreach (var item in items) {
    if (result != "") result += ", ";
    result += item;
}

// RIGHT - Efficient
string result = string.Join(", ", items);
```

## 16. Not Checking Split Results

```csharp
// WRONG
string[] parts = text.Split('.');
string extension = parts[1];  // IndexOutOfRangeException if no dot!

// RIGHT
string[] parts = text.Split('.');
if (parts.Length > 1) {
    string extension = parts[1];
}
```

## 17. Substring Without Bounds Check

```csharp
// WRONG
string sub = text.Substring(5, 20);  // May throw if string too short

// RIGHT
int startIndex = 5;
int length = Math.Min(20, text.Length - startIndex);
if (length > 0) {
    string sub = text.Substring(startIndex, length);
}
```

## 18. Assuming String Equality After Modification

```csharp
// WRONG
string a = "Hello";
string b = a.ToUpper();
// a still equals "Hello", not "HELLO"

// RIGHT - Know that methods return new strings
string upper = a.ToUpper();
// a unchanged, upper = "HELLO"
```

## 19. Empty String vs Space

```csharp
// WRONG - Confusing empty with space
string empty = "";
string space = " ";
bool isEmpty = empty == space;  // false

// RIGHT - Distinguish them
if (string.IsNullOrEmpty(input)) { }
if (string.IsNullOrWhiteSpace(input)) { }
```

## 20. Not Validating Regex Pattern

```csharp
// WRONG - Invalid regex
try {
    Regex.IsMatch(input, pattern);  // May throw RegexParseException
}
catch (RegexParseException ex) {
    Console.WriteLine($"Invalid regex: {ex.Message}");
}

// RIGHT - Validate before use
try {
    var regex = new Regex(pattern);
    bool matches = regex.IsMatch(input);
}
catch (ArgumentException ex) {
    Console.WriteLine($"Invalid pattern: {ex.Message}");
}
```

## Summary of Common Mistakes

| Mistake | Problem | Solution |
|---------|---------|----------|
| Index out of bounds | IndexOutOfRangeException | Check bounds |
| Null string access | NullReferenceException | Check for null |
| String concat in loop | O(n²) performance | Use StringBuilder |
| Case sensitivity | Wrong comparison | Use OrdinalIgnoreCase |
| Forgetting immutability | String unchanged | Assign result |
| Multiple iterations | Inefficiency | Use ToList() |
| Invalid regex | RegexParseException | Escape special chars |
| Whitespace not handled | Validation fails | Use IsNullOrWhiteSpace |
| No bounds check | IndexOutOfRangeException | Validate access |
| Culture-dependent key | Inconsistent lookup | Use Invariant |

---

## Next Steps

1. Study Interview Questions
2. Practice Coding
3. Review Best Practices
