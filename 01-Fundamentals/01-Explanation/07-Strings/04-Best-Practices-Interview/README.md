# Best Practices & Interview Questions

## Overview
Learn professional practices for working with strings and prepare for technical interviews with 18 comprehensive questions across difficulty levels.

## Section 1: Best Practices

### 15 Essential Guidelines
- Use string interpolation
- Use StringBuilder for loops
- Validate input safely
- Use appropriate comparison
- Check bounds before access
- Use ranges for extraction
- Use String.Join for collections
- Check null and whitespace
- Cache regex patterns
- Use invariant culture for keys
- Handle null safely
- Format appropriately
- Document constraints
- Avoid repeated conversions
- Optimize early operations

**Time:** 20-30 minutes

**File:** 01-Best-Practices/00-Best-Practices.md

---

## Section 2: Common Mistakes

### 20 Critical Mistakes to Avoid
- Index out of bounds
- NullReferenceException
- Inefficient concatenation
- Case sensitivity issues
- Null assumption
- Missing ToString()
- IndexOf confusion
- Culture-dependent operations
- Whitespace handling
- String immutability confusion
- Case-sensitive collections
- Multiple iterations
- Regex issues
- Missing trim
- + operator in loops
- Split without checks
- Substring bounds
- Empty vs space confusion
- String modification attempts
- Invalid regex patterns

**Time:** 20-30 minutes

**File:** 02-Common-Mistakes/00-Common-Mistakes.md

---

## Section 3: Interview Questions

### 18 Questions Across 3 Difficulty Levels

#### Easy (10 questions) - 8-10 min each
Topics: Creation, immutability, properties, basic methods, safe access

**File:** 03-Interview-Questions/01-Easy/00-Easy-Questions.md

#### Medium (10 questions) - 10-15 min each
Topics: StringBuilder, validation patterns, formatting, performance analysis

**File:** 03-Interview-Questions/02-Medium/00-Medium-Questions.md

#### Hard (10 questions) - 15-20 min each
Topics: Thread safety, optimization, complex validation, architecture

**File:** 03-Interview-Questions/03-Hard/00-Hard-Questions.md

---

## Quick Reference: Key Concepts

### Best Practices Checklist

✓ **String Interpolation**
```csharp
string msg = $"Hello {name}";
```

✓ **StringBuilder for Loops**
```csharp
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
```

✓ **Safe Input Validation**
```csharp
if (!string.IsNullOrWhiteSpace(input)) {
    Process(input.Trim());
}
```

✓ **Appropriate Comparison**
```csharp
bool same = text.Equals(other, StringComparison.OrdinalIgnoreCase);
```

✓ **Bounds Checking**
```csharp
if (index >= 0 && index < text.Length) {
    char c = text[index];
}
```

### Common Mistakes to Avoid

❌ **String Concatenation in Loop**
```csharp
// WRONG - O(n²)
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;  // Very slow!
}

// RIGHT - O(n)
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
```

❌ **Null String Access**
```csharp
// WRONG
string? input = GetInput();
int len = input.Length;  // May throw!

// RIGHT
int len = input?.Length ?? 0;
```

❌ **Case Sensitivity**
```csharp
// WRONG
if (role == "Admin") { }  // Fails if "ADMIN"

// RIGHT
if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase)) { }
```

---

## Interview Preparation Guide

### Difficulty Levels

**Easy Questions**
- Basic concepts
- String methods
- Common operations
- Simple patterns

✓ **Success:** Know definitions, common methods, basic use cases
✓ **Topics:** Creation, properties, basic search/manipulation

### Medium Questions
- Design scenarios
- Performance analysis
- Complex validation
- Pattern implementation

✓ **Success:** Explain trade-offs, write working code, discuss optimization
✓ **Topics:** StringBuilder, validation patterns, formatting, performance

### Hard Questions
- Complex optimization
- Real-world design
- Architecture patterns
- Advanced scenarios

✓ **Success:** Deep understanding, production-ready code, implications
✓ **Topics:** Thread safety, text indexing, localization, security

---

## Interview Tips & Strategies

### Before the Interview

1. **Understand fundamentals**
   - String immutability
   - Performance characteristics
   - Method options and trade-offs

2. **Study patterns**
   - Common validation scenarios
   - Real-world examples
   - Performance optimization

3. **Practice coding**
   - Write solution code
   - Test edge cases
   - Discuss performance

### During the Interview

1. **Clarify the question**
   - Ask for requirements
   - Confirm constraints
   - Ask about scale

2. **Think aloud**
   - Explain approach
   - Discuss trade-offs
   - Ask clarifying questions

3. **Write quality code**
   - Use proper naming
   - Add comments
   - Handle edge cases

4. **Discuss performance**
   - Time complexity
   - Space complexity
   - Optimization opportunities

### Common Interview Patterns

```csharp
// Pattern 1: Input validation
if (string.IsNullOrWhiteSpace(input)) {
    return false;
}

// Pattern 2: Efficient string building
var sb = new StringBuilder();
foreach (var item in items) {
    sb.Append(item);
}

// Pattern 3: Case-insensitive search
bool found = text.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;

// Pattern 4: Format for output
string formatted = $"{value:C}";  // Currency

// Pattern 5: Validate format
bool valid = Regex.IsMatch(input, pattern);
```

---

## Study Schedule

### Day 1: Best Practices (2-3 hours)
- Read all 15 guidelines
- Study code examples
- Understand trade-offs

### Day 2: Common Mistakes (2-3 hours)
- Review each mistake
- Study solutions
- Practice avoiding them

### Day 3-4: Interview Questions (4-6 hours)
- **Day 3:** Easy questions (1-1.5 hours)
- **Day 3:** Medium questions (1.5-2 hours)
- **Day 4:** Hard questions (2-2.5 hours)

### Day 5: Review & Practice (1-2 hours)
- Re-read key concepts
- Write code for scenarios
- Mock interview practice

---

## Self-Assessment

Can you:
- [ ] List best practices for strings?
- [ ] Identify and fix common mistakes?
- [ ] Answer Easy questions confidently?
- [ ] Explain Medium question solutions?
- [ ] Solve Hard questions with production code?
- [ ] Discuss performance implications?
- [ ] Design real-world string solutions?

---

## Quick Links

- [Best Practices](01-Best-Practices/00-Best-Practices.md)
- [Common Mistakes](02-Common-Mistakes/00-Common-Mistakes.md)
- [Interview Overview](03-Interview-Questions/00-Interview-Overview.md)
- [Easy Questions](03-Interview-Questions/01-Easy/00-Easy-Questions.md)
- [Medium Questions](03-Interview-Questions/02-Medium/00-Medium-Questions.md)
- [Hard Questions](03-Interview-Questions/03-Hard/00-Hard-Questions.md)

---

## Next Steps

1. ✓ Study Best Practices thoroughly
2. ✓ Review Common Mistakes
3. ✓ Answer Easy questions confidently
4. ✓ Master Medium questions
5. ✓ Solve Hard questions
6. → Practice on real problems
7. → Conduct mock interviews
8. → Review edge cases
