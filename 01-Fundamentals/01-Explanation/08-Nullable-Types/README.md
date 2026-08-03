# Nullable Types and Null Handling

## Overview

Master nullable types in C# - understanding null, safe access patterns, and professional null handling. This comprehensive guide covers fundamentals, checking techniques, patterns, best practices, and interview preparation.

**Total Learning Time:** 4-5 hours  
**Depth:** Beginner to Advanced  
**Files:** 15+ focused documents with code examples

---

## Quick Navigation

| Section | Time | Topics |
|---------|------|--------|
| **Nullable Fundamentals** | 45 min | What is null, nullable value types |
| **Null Checking** | 1 hour | HasValue, ??, ?. operators |
| **Null Patterns** | 45 min | Guard clauses, pattern matching, scenarios |
| **Best Practices & Interview** | 1.5 hours | Guidelines, mistakes, 15 interview questions |

---

## Section 1: Nullable Fundamentals 📚
**Understand null and nullable types**

- **What is Null** - Null concept, reference vs value types, contexts
- **Nullable Value Types** - Creating nullable types, operations, examples

**Key concepts:**
```csharp
// Null represents no value
string? nullStr = null;  // Reference type can be null

// Value types need ? to be nullable
int? age = null;        // Without ?, int cannot be null
int count = 5;          // Regular int cannot be null
```

[→ Go to Nullable Fundamentals](01-Nullable-Fundamentals/README.md)

---

## Section 2: Null Checking 🔍
**Master safe null checking techniques**

- **HasValue Property** - Checking values, safe extraction
- **Null-Coalescing Operator** - ?? operator, ??= assignment, chaining
- **Null-Conditional Operator** - ?. and ?[], safe access

**Key operators:**
```csharp
// HasValue check
if (age.HasValue) { int val = age.Value; }

// Null coalescing (??)
int val = age ?? 0;  // 0 if age is null

// Null coalescing assignment (??=)
age ??= 18;  // Only assign if null

// Null conditional (?.)
string? name = person?.Name;  // null if person is null
```

[→ Go to Null Checking](02-Null-Checking/README.md)

---

## Section 3: Null Patterns 🎯
**Professional patterns for null handling**

- **Guard Clauses** - Early validation, fail fast
- **Pattern Matching** - is null, is not null, switch
- **Real-World Scenarios** - Database, API, configuration

**Common patterns:**
```csharp
// Guard clause - fail fast
ArgumentNullException.ThrowIfNull(data);

// Pattern matching
if (value is not null) { Process(value); }

// Real-world - API response
string email = response?.Email ?? "default@example.com";
```

[→ Go to Null Patterns](03-Null-Patterns/README.md)

---

## Section 4: Best Practices & Interview 🎓
**Professional practices and interview preparation**

### Best Practices (10 Guidelines)
- Use nullable types explicitly
- Use ?? for defaults
- Guard clauses for validation
- Safe access with ?.
- Pattern matching for clarity
- And 5 more...

### Common Mistakes (10 Items)
- NullReferenceException
- Forgetting ?. operator
- Accessing .Value without check
- Not providing defaults
- And 6 more...

### Interview Questions (15 Total)
- **Easy (5 questions)** - Fundamentals, basic patterns
- **Medium (5 questions)** - Pattern matching, real-world
- **Hard (5 questions)** - Complex scenarios, design

[→ Go to Best Practices & Interview](04-Best-Practices-Interview/README.md)

---

## Quick Reference

### Null Checking Methods

| Method | Purpose | Example |
|--------|---------|---------|
| `.HasValue` | Check if has value | `if (x.HasValue)` |
| `??` | Default if null | `x ?? 0` |
| `??=` | Assign if null | `x ??= 0` |
| `?.` | Safe member access | `person?.Name` |
| `?[]` | Safe indexing | `arr?[0]` |
| `.GetValueOrDefault()` | Extract with default | `x.GetValueOrDefault(0)` |

### Common Null Checks

```csharp
// Null check
if (value == null) { }
if (value is null) { }

// Not null check
if (value != null) { }
if (value is not null) { }

// Safe defaults
int result = value ?? 0;
int result = value.GetValueOrDefault(0);
int result = value.HasValue ? value.Value : 0;
```

---

## Learning Paths

### Path 1: Beginner (2-3 hours)
1. Nullable Fundamentals
2. Null Checking - HasValue and ??
3. Basic patterns and examples

### Path 2: Intermediate (3-4 hours)
1. Complete Beginner Path
2. ?. operator mastery
3. Guard clauses
4. Pattern matching

### Path 3: Interview Preparation (4-5 hours)
1. Complete Intermediate Path
2. Best Practices and Common Mistakes
3. All 15 Interview Questions
4. Mock practice

---

## Key Takeaways

✓ **Null represents "no value"**  
✓ **Reference types nullable by default**  
✓ **Value types need ? to be nullable**  
✓ **Use ?? for safe defaults**  
✓ **Use ?. for safe member access**  
✓ **Guard clauses for validation**  
✓ **Pattern matching for clarity**  
✓ **Always handle null safely**  
✓ **Fail fast approach**  
✓ **Prevent NullReferenceException**  

---

## Common Mistakes to Avoid

❌ **NullReferenceException**
```csharp
// WRONG
int len = text.Length;  // Crashes if text is null!

// RIGHT
int? len = text?.Length;  // Safe
```

❌ **Forgetting ?. operator**
```csharp
// WRONG
string name = person.Name;  // Might crash

// RIGHT
string? name = person?.Name;  // Safe
```

❌ **Accessing .Value without check**
```csharp
// WRONG
int val = age.Value;  // InvalidOperationException if null!

// RIGHT
int val = age ?? 0;  // Safe
```

---

## Self-Assessment

Can you:
- [ ] Explain what null is?
- [ ] Create nullable value types?
- [ ] Use ??, ??=, ?. operators?
- [ ] Check for null safely?
- [ ] Apply guard clauses?
- [ ] Use pattern matching?
- [ ] Handle real-world scenarios?
- [ ] Answer interview questions?

---

## Next Steps

1. Start with Nullable Fundamentals
2. Master Null Checking techniques
3. Learn Null Patterns
4. Study Best Practices
5. Prepare for interviews

---

## Troubleshooting

**"I'm getting NullReferenceException"**
→ Use `?.` for safe access or check with `??` first

**"How do I provide a default?"**
→ Use `??` operator: `value ?? default`

**"Should I check HasValue or use ???"**
→ Use `??` for simplicity, `HasValue` for explicit checks

**"When should I use pattern matching?"**
→ Use for clarity and multiple case handling

---

**Master null handling to write safer, more robust C# code!** 🚀
