# Strings and String Manipulation

## Overview

Master strings - the fundamental data structure for text in C#. This comprehensive guide covers string creation, methods, patterns, best practices, and real-world design scenarios.

**Total Learning Time:** 6-8 hours  
**Depth:** Beginner to Advanced  
**Files:** 20+ focused documents with code examples

---

## Quick Navigation

### 📚 Learning Sections

| Section | Time | Level | Topics |
|---------|------|-------|--------|
| **String Basics** | 1 hour | Beginner | Creation, properties, access |
| **String Operations** | 1.5 hours | Beginner | Case, search, manipulation |
| **String Patterns** | 1.5 hours | Intermediate | Comparison, formatting, validation |
| **Best Practices & Interview** | 2-3 hours | Intermediate-Advanced | Patterns, mistakes, 18 questions |

---

## Section 1: String Basics 📝
**Master string fundamentals and safe access patterns**

- **String Creation** - Literals, interpolation, concatenation, constructors
- **String Properties** - Length, indexing, ranges, character access
- **README** - Full learning guide

**Key concepts:**
- Immutable: Once created, strings never change
- 0-indexed: First character at index 0
- Unicode: Full Unicode support including emoji
- Escape sequences: \n, \t, \\, etc.

```csharp
// Creation
string msg = "Hello";
string msg2 = $"Hello {name}";

// Access
int len = msg.Length;
char first = msg[0];
string sub = msg[0..5];
```

[→ Go to String Basics](01-String-Basics/README.md)

---

## Section 2: String Operations 🔧
**Practical methods for string transformation and search**

- **Case Methods** - ToUpper, ToLower, culture-aware casing
- **Search Methods** - Contains, IndexOf, StartsWith, EndsWith, regex
- **Manipulation Methods** - Substring, Split, Replace, Trim, StringBuilder
- **README** - Operation reference and patterns

**Common operations:**
```csharp
// Case
string upper = text.ToUpper();
bool sameIgnoreCase = text.Equals(other, StringComparison.OrdinalIgnoreCase);

// Search
bool has = text.Contains("search");
int pos = text.IndexOf("search");

// Manipulation
string[] parts = text.Split(',');
string replaced = text.Replace("old", "new");
string trimmed = text.Trim();
```

**When to use each:**
- **Contains** - Check if substring exists
- **IndexOf** - Need position of substring
- **Split** - Break into parts by delimiter
- **Replace** - Find and replace text
- **Substring/Ranges** - Extract specific part

[→ Go to String Operations](02-String-Operations/README.md)

---

## Section 3: String Patterns 🎯
**Advanced patterns for real-world string usage**

- **String Comparison** - Equals, CompareTo, StringComparison options, null-safe
- **String Formatting** - Interpolation, format specifiers, date/time, custom
- **String Validation** - Email, phone, URL, password, regex patterns
- **StringBuilder & Performance** - Optimization, memory management
- **README** - Pattern reference and examples

**Common patterns:**
```csharp
// Comparison
if (!string.IsNullOrWhiteSpace(input)) { }

// Formatting
string currency = $"{price:C}";
string date = $"{now:yyyy-MM-dd}";

// Validation
bool isEmail = Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");

// Performance
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
```

[→ Go to String Patterns](03-String-Patterns/README.md)

---

## Section 4: Best Practices & Interview 🎓
**Professional practices and interview preparation**

### Best Practices (15 Guidelines)
- Use string interpolation
- Use StringBuilder for loops
- Validate input safely
- Check bounds before access
- Use StringComparison for case-insensitive
- Handle null safely
- Format appropriately
- Cache regex patterns
- And 7 more...

### Common Mistakes (20 Items)
- Index out of bounds
- NullReferenceException
- Inefficient concatenation
- Case sensitivity issues
- Missing bounds checks
- Forgetting immutability
- And 14 more...

### Interview Questions (18 Total)
- **Easy (10 questions)** - 8-10 min each
  - String creation, properties, basic methods
- **Medium (10 questions)** - 10-15 min each
  - StringBuilder, validation, performance
- **Hard (10 questions)** - 15-20 min each
  - Thread safety, optimization, design

[→ Go to Best Practices & Interview](04-Best-Practices-Interview/README.md)

---

## Learning Paths

### Path 1: Beginner (4-5 hours)
Perfect for learning the fundamentals

1. **String Basics** (1 hour)
   - String Creation
   - String Properties

2. **String Operations** (1.5 hours)
   - Case Methods
   - Search Methods

3. **String Patterns Intro** (1.5 hours)
   - String Comparison
   - String Formatting

4. **Best Practices Overview** (30 min)
   - Read guidelines
   - Review common mistakes

### Path 2: Intermediate (6-7 hours)
Deepening understanding with patterns

1. **Complete Beginner Path** (4-5 hours)

2. **Advanced String Operations** (30 min)
   - StringBuilder optimization
   - Manipulation patterns

3. **String Patterns Deep Dive** (1.5 hours)
   - Validation techniques
   - Complex formatting

4. **Performance Optimization** (1 hour)
   - StringBuilder mastery
   - Regex caching
   - String pooling

### Path 3: Interview Preparation (8+ hours)
Comprehensive interview readiness

1. **Complete Intermediate Path** (6-7 hours)

2. **Interview Questions - Easy** (1.5-2 hours)
   - Answer all 10 questions
   - Explain each thoroughly

3. **Interview Questions - Medium** (1.5-2 hours)
   - Design scenarios
   - Performance analysis

4. **Interview Questions - Hard** (2-2.5 hours)
   - Complex solutions
   - Production-ready code

5. **Mock Interviews** (1-2 hours)
   - Time yourself
   - Discuss solutions

---

## Quick Reference

### String Creation

```csharp
// Literal
string text = "Hello";

// Interpolation
string msg = $"Hello {name}";

// Concatenation
string full = first + " " + last;

// Constructor
string repeated = new string('*', 10);

// Multi-line
string multiline = @"Line 1
Line 2";
```

### Common String Methods

| Method | Purpose | Example |
|--------|---------|---------|
| `Length` | Get string length | `text.Length` |
| `ToUpper()` | Convert to uppercase | `text.ToUpper()` |
| `ToLower()` | Convert to lowercase | `text.ToLower()` |
| `Contains()` | Check if contains | `text.Contains("hi")` |
| `IndexOf()` | Find position | `text.IndexOf("hi")` |
| `Substring()` | Extract part | `text.Substring(0, 5)` |
| `Split()` | Break into parts | `text.Split(',')` |
| `Replace()` | Find and replace | `text.Replace("a", "b")` |
| `Trim()` | Remove whitespace | `text.Trim()` |
| `Equals()` | Compare strings | `text.Equals(other)` |

### Performance Cheat Sheet

| Operation | Efficiency | Use Case |
|-----------|-----------|----------|
| `+` operator | Slow for loops | Single concat |
| `string.Format` | Reasonable | Formatting |
| `string.Join` | Fast | Joining arrays |
| `StringBuilder` | Very Fast | Loops with many appends |
| String interpolation | Good | General use |

### Format Specifiers

```csharp
// Currency
$"{price:C}"        // "$19.99"

// Decimal places
$"{value:F2}"       // "19.99"

// Percentage
$"{percent:P}"      // "85.00%"

// Numbers with zeros
$"{id:D5}"          // "00042"

// Hexadecimal
$"{255:X}"          // "FF"

// Date/Time
$"{date:yyyy-MM-dd}"  // "2024-08-03"
```

---

## Key Takeaways

✓ **Strings are immutable** - Operations create new strings  
✓ **Use interpolation** - For clarity and readability  
✓ **Use StringBuilder** - For loops with many concatenations  
✓ **Validate input** - Always check for null/whitespace  
✓ **StringComparison** - Use for case-insensitive comparisons  
✓ **Understand performance** - O(n) vs O(n²) matters  
✓ **Format appropriately** - Use format specifiers  
✓ **Handle edge cases** - Empty strings, null, bounds  
✓ **Cache regex** - Compile patterns for reuse  
✓ **Choose right method** - Contains, IndexOf, Split, etc.

---

## Self-Assessment Checklist

### String Basics
- [ ] Create strings using different methods
- [ ] Use string interpolation
- [ ] Access characters safely
- [ ] Use ranges for extraction
- [ ] Understand immutability

### String Operations
- [ ] Convert case properly
- [ ] Search for substrings
- [ ] Extract and manipulate text
- [ ] Use StringBuilder efficiently
- [ ] Understand performance implications

### String Patterns
- [ ] Compare strings correctly
- [ ] Format strings appropriately
- [ ] Validate common formats
- [ ] Use regex for patterns
- [ ] Optimize performance

### Interview Ready
- [ ] Answer Easy questions confidently
- [ ] Solve Medium questions
- [ ] Tackle Hard questions
- [ ] Discuss performance
- [ ] Design real-world solutions

---

## Common Interview Questions

**Easy Level**
- What is string immutability?
- When should you use StringBuilder?
- How do you safely access a character?

**Medium Level**
- Design email validation
- Analyze string performance
- When to use each search method?

**Hard Level**
- Build a string pool
- Optimize text processing
- Design localization system

---

## Resources & Files

```
07-Strings/
├── 01-String-Basics/
│   ├── 00-String-Creation.md
│   ├── 00-String-Properties.md
│   └── README.md
├── 02-String-Operations/
│   ├── 00-Case-Methods.md
│   ├── 00-Search-Methods.md
│   ├── 00-Manipulation-Methods.md
│   └── README.md
├── 03-String-Patterns/
│   ├── 00-String-Comparison.md
│   ├── 00-String-Formatting.md
│   ├── 00-String-Validation.md
│   ├── 00-StringBuilder-Performance.md
│   └── README.md
├── 04-Best-Practices-Interview/
│   ├── 01-Best-Practices/00-Best-Practices.md
│   ├── 02-Common-Mistakes/00-Common-Mistakes.md
│   ├── 03-Interview-Questions/
│   │   ├── 00-Interview-Overview.md
│   │   ├── 01-Easy/00-Easy-Questions.md
│   │   ├── 02-Medium/00-Medium-Questions.md
│   │   ├── 03-Hard/00-Hard-Questions.md
│   └── README.md
└── README.md (this file)
```

---

## Next Steps

1. **Choose your learning path** above
2. **Start with String Basics** if new to C#
3. **Move to String Operations** for practical methods
4. **Study String Patterns** for real-world usage
5. **Prepare for interviews** with Q&A sections

---

## Tips for Success

✓ **Practice coding** - Write examples for each concept  
✓ **Understand why** - Don't just memorize  
✓ **Performance matters** - Know time complexity  
✓ **Real scenarios** - Think about use cases  
✓ **Review often** - Revisit complex topics  
✓ **Mock interviews** - Practice explaining solutions  

---

## Related Topics

- **Collections** - Working with string collections
- **LINQ** - Query strings with LINQ
- **Exception Handling** - Handle string errors
- **OOP** - String in class design
- **File I/O** - Reading/writing text

---

## Troubleshooting

**"My string doesn't change when I call a method"**
→ Strings are immutable. Assign the result: `text = text.ToUpper();`

**"Index out of bounds error"**
→ Check bounds first: `if (index < text.Length) { ... }`

**"String concatenation is slow"**
→ Use StringBuilder for loops: `var sb = new StringBuilder();`

**"Case-sensitive comparison failing"**
→ Use StringComparison: `.Equals(other, StringComparison.OrdinalIgnoreCase)`

**"My regex isn't matching"**
→ Escape special characters: `@"\."` for literal dot

---

**Happy learning! Master strings to write better C# code.** 🚀
