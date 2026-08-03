# Interview Questions: Data Types

## Overview

This section contains comprehensive interview questions about C# data types, organized by difficulty level. These are real questions asked in interviews at companies of various sizes.

## Purpose

- **Test Understanding**: Know not just what but why
- **Build Confidence**: Practice before your interview
- **Deepen Knowledge**: Learn advanced concepts
- **Speed Practice**: Get quick at answering

## Structure

### Easy Level (5-10 minutes per question)
Warm-up questions testing basic understanding.

**Topics**:
- Type definitions
- Simple comparisons
- Basic conversions
- Common usage patterns

**Count**: 12 questions
**Files**: `01-Easy/00-Easy-Questions.md`

### Medium Level (15-20 minutes per question)
Core knowledge questions requiring practical application.

**Topics**:
- Type selection decisions
- Performance implications
- Memory behavior
- Collections selection

**Count**: 9 questions
**Files**: `02-Medium/00-Medium-Questions.md`

### Hard Level (20-30 minutes per question)
Expert-level questions about complex scenarios and trade-offs.

**Topics**:
- System design with types
- Advanced concepts
- Edge cases and gotchas
- Trade-off analysis

**Count**: 6 questions
**Files**: `03-Hard/00-Hard-Questions.md`

## How to Use This Section

### For Quick Review (30 minutes)
```
1. Read section overview above
2. Skim all Easy questions
3. Skim Medium questions if time
```

### For Thorough Study (2-3 hours)
```
1. Read all Easy questions
2. For each question:
   - Read the question
   - Think about answer (without looking)
   - Review provided answer
   - Understand the reasoning
3. Repeat for Medium questions
```

### For Mock Interview (1 hour)
```
1. Pick 3-4 questions from each level
2. Answer without looking at solutions
3. Time yourself (5 min easy, 15 min medium, 25 min hard)
4. Compare with provided answer
5. Ask yourself: Why is this the best answer?
```

### For Interview Prep (1 day)
```
- Morning: Study all Easy questions thoroughly
- Midday: Study Medium questions with code examples
- Afternoon: Practice Hard questions with detailed answers
- Evening: Mock interview with random questions
```

## Key Concepts to Master

Before answering questions, ensure you understand:

### Value Types
- Stored on stack
- Copied entirely
- No garbage collection
- Examples: int, bool, struct

### Reference Types
- Stored on heap (reference on stack)
- Copied by reference
- Garbage collected
- Examples: string, class, List<T>

### Collections
- List<T> vs Dictionary<K,V> vs HashSet<T>
- Performance implications of each
- When to choose each

### Strings
- Immutability
- StringBuilder usage
- Performance considerations

### Classes vs Structs
- When to use each
- Inheritance possibilities
- Performance trade-offs

## Interview Tips

### 1. Understand, Don't Memorize
- Know WHY you choose a type
- Not just WHAT to choose
- Understand trade-offs

### 2. Show Your Reasoning
```
"I would choose [X] because:
- [Benefit 1]
- [Benefit 2]
- However, [Drawback]
- So in this case, [X] is better because..."
```

### 3. Provide Code Examples
- Write quick, clear examples
- Explain what the code does
- Point out why it matters

### 4. Discuss Trade-offs
- Every choice has trade-offs
- Show you think critically
- Compare alternatives

### 5. Ask Clarifying Questions
```
"Before I answer, let me clarify:
- What's the size of the data?
- How often is this accessed?
- What's the performance requirement?"
```

## Common Interview Traps

### ❌ Trap 1: Using float for Money
```csharp
float total = 0.1f + 0.2f;  // Not exactly 0.3!
```
**Solution**: Use `decimal` for financial calculations

### ❌ Trap 2: String Concatenation in Loops
```csharp
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;  // Creates 1000 strings!
}
```
**Solution**: Use `StringBuilder`

### ❌ Trap 3: Not Checking Null
```csharp
string text = GetText();
int length = text.Length;  // Could crash!
```
**Solution**: Check null before use

### ❌ Trap 4: Wrong Collection for Use Case
```csharp
List<int> list = new();
// Later in loop: if (list.Contains(value)) { }  // O(n) - slow!
```
**Solution**: Use `HashSet<T>` for fast lookups

### ❌ Trap 5: Modifying Collection During Iteration
```csharp
foreach (var item in list) {
    list.Remove(item);  // InvalidOperationException!
}
```
**Solution**: Iterate a copy or use LINQ

## Success Criteria

You've mastered this section when you can:

✓ Answer all Easy questions correctly (5 min each)
✓ Answer Medium questions with clear reasoning (15 min each)
✓ Articulate trade-offs in Hard questions (25 min each)
✓ Provide working code examples
✓ Explain WHY, not just WHAT
✓ Discuss performance implications
✓ Handle edge cases and error scenarios

## Quick Reference: When to Use What

### Numeric Types
```
int      → Default for integers
long     → When you need larger range
float    → Graphics, scientific (with caution)
decimal  → MONEY - always use this!
double   → Scientific calculations
```

### Collections
```
List<T>           → Default choice
Dictionary<K,V>  → Key-value lookups
HashSet<T>       → Unique items, fast lookups
Queue<T>         → FIFO processing
Stack<T>         → LIFO processing
SortedDictionary → Need sorted by key
```

### String Operations
```
string + string          → Few concatenations
StringBuilder            → Loop concatenations
string.Intern()          → Not needed (usually)
StringBuilder.AppendLine → Multi-line building
```

### Type Selection
```
struct  → Small (< 16 bytes) immutable data
class   → Complex objects, inheritance
record  → Immutable data with value equality
readonly struct → High-performance immutable
```

## Navigation

- **Easy Questions**: `01-Easy/00-Easy-Questions.md`
- **Medium Questions**: `02-Medium/00-Medium-Questions.md`
- **Hard Questions**: `03-Hard/00-Hard-Questions.md`
- **Value Types**: `../01-Value-Types/README.md`
- **Reference Types**: `../02-Reference-Types/README.md`
- **Comparison & Practices**: `../03-Comparison-Practices/README.md`

## Frequently Asked Follow-ups

### "Why is X better than Y?"
Always explain:
- Performance characteristics
- Readability and maintainability
- Scalability implications
- Real-world trade-offs

### "What if we had a different requirement?"
Show flexibility:
- "If we needed [X], I would use [Y] instead"
- "The trade-off would be..."
- "Performance would change because..."

### "Can you code this?"
Be ready to write:
- Clean, readable code
- With proper error handling
- Comments explaining key parts
- Examples of use

## Next Steps After Interview Questions

1. **Review**: Re-read sections you struggled with
2. **Code**: Write your own examples
3. **Teach**: Explain concepts to someone else
4. **Apply**: Use in real projects
5. **Deepen**: Study related topics (LINQ, Async, etc.)

---

**Tip**: The best interview preparation is understanding the concepts deeply, not memorizing answers. Focus on understanding WHY these answers are correct.
