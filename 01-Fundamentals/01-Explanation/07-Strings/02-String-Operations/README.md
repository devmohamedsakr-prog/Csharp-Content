# String Operations

## Overview
Master string methods: case conversion, searching, and manipulation techniques.

## Learning Path

### 1. Case Methods
- ToUpper and ToLower
- Culture-specific casing
- TextInfo for title case
- Case-insensitive comparison
- Invariant vs culture-aware

**Time:** 15-20 minutes

### 2. Search Methods
- Contains for existence
- IndexOf and LastIndexOf
- StartsWith and EndsWith
- Character and substring search
- Regex patterns

**Time:** 20-25 minutes

### 3. Manipulation Methods
- Substring and ranges
- Split by delimiter
- Replace and trim
- Remove and insert
- Padding methods
- StringBuilder for efficiency

**Time:** 25-30 minutes

## Files in This Section

1. **00-Case-Methods.md** - ToUpper/ToLower, culture, comparison
2. **00-Search-Methods.md** - Contains, IndexOf, StartsWith, EndsWith
3. **00-Manipulation-Methods.md** - Substring, Split, Replace, Trim, StringBuilder

## Quick Reference

```csharp
// Case
string upper = text.ToUpper();
string lower = text.ToLower();
bool sameIgnoreCase = text.Equals(other, StringComparison.OrdinalIgnoreCase);

// Search
bool has = text.Contains("search");
int pos = text.IndexOf("search");
bool starts = text.StartsWith("pre");

// Manipulation
string sub = text.Substring(0, 5);
string[] parts = text.Split(',');
string replaced = text.Replace("old", "new");
string trimmed = text.Trim();

// StringBuilder
var sb = new StringBuilder();
sb.Append("text");
string result = sb.ToString();
```

## Operations by Category

### Case Conversion
- `ToUpper()` - Uppercase conversion
- `ToLower()` - Lowercase conversion
- `ToUpperInvariant()` - Culture-independent uppercase
- `ToLowerInvariant()` - Culture-independent lowercase
- `TextInfo.ToTitleCase()` - Proper casing

### Search Operations
- `Contains(str)` - Check if contains substring
- `IndexOf(str)` - Find first position
- `LastIndexOf(str)` - Find last position
- `StartsWith(str)` - Check prefix
- `EndsWith(str)` - Check suffix
- `IndexOfAny(chars)` - Find any character

### Manipulation Operations
- `Substring(start, length)` - Extract part
- `[start..end]` - Range extraction
- `Split(delimiter)` - Break into parts
- `Replace(old, new)` - Replace text
- `Trim()` - Remove whitespace
- `Remove(index, length)` - Remove characters
- `Insert(index, str)` - Insert text
- `PadLeft(width)` - Pad left
- `PadRight(width)` - Pad right

## Performance Considerations

| Operation | Complexity | Notes |
|-----------|-----------|-------|
| `Contains` | O(n) | Linear search |
| `IndexOf` | O(n) | Linear search |
| `Substring` | O(n) | Creates new string |
| `Split` | O(n) | Creates array |
| `Replace` | O(n) | Creates new string |
| `ToUpper/ToLower` | O(n) | Creates new string |

## When to Use Each

### Contains/IndexOf
```csharp
// Existence check
if (text.Contains("word")) { }

// Position needed
int pos = text.IndexOf("word");
```

### StartsWith/EndsWith
```csharp
// Prefix/suffix check
if (text.StartsWith("http://")) { }
if (file.EndsWith(".txt")) { }
```

### Split/Join
```csharp
// Break into parts
string[] parts = csv.Split(',');

// Join parts
string joined = string.Join(", ", parts);
```

### Replace
```csharp
// Simple replacement
string result = text.Replace("old", "new");

// Case-insensitive
string resultCI = Regex.Replace(text, "OLD", "new", RegexOptions.IgnoreCase);
```

### Substring vs Range
```csharp
// Substring (verbose)
string sub = text.Substring(0, 5);

// Range (modern, C# 8+)
string sub = text[0..5];
```

### StringBuilder
```csharp
// For loops/many operations
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
```

## Best Practices

✓ Use `StringBuilder` for loops with concatenation
✓ Use `Contains` for existence checks
✓ Use `string.Join` for collection joining
✓ Use `StringComparison.OrdinalIgnoreCase` for case-insensitive
✓ Use invariant culture for keys
✓ Pre-allocate StringBuilder capacity if known
✓ Use ranges for extraction (C# 8+)

## Common Mistakes

❌ Using `+` in loops - Use StringBuilder
❌ Case sensitivity in comparison - Use StringComparison
❌ Not checking IndexOf for -1 - Check >= 0
❌ Multiple LINQ iterations - Materialize with ToList()
❌ Forgetting ToString() on StringBuilder

## Self-Assessment

Can you:
- [ ] Use case methods correctly?
- [ ] Search for substrings efficiently?
- [ ] Extract and manipulate text?
- [ ] Use StringBuilder for loops?
- [ ] Understand performance implications?
- [ ] Handle culture considerations?

---

## Related Topics

- **String Basics** - Creation and properties
- **String Patterns** - Comparison, formatting, validation
- **Collections** - Working with string collections
- **Performance** - Optimization strategies

## Next Steps

1. ✓ Study Case Methods
2. ✓ Master Search Methods
3. ✓ Learn Manipulation Methods
4. → Move to String Patterns
5. → Review Best Practices
