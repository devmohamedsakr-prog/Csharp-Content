# Data Types Interview Questions

## Overview

This section contains common interview questions about C# data types, organized by difficulty level. These questions test understanding of fundamental concepts, practical application, and decision-making.

## Question Categories

### Easy Level (Warm-up, 5-10 minutes)
Basic understanding of data types and their properties.
- Type definitions and characteristics
- Simple comparisons
- Basic conversions
- Common usage patterns

### Medium Level (Core Knowledge, 15-20 minutes)
Practical application and deeper understanding.
- Type selection decisions
- Performance implications
- Memory behavior
- Collections selection

### Hard Level (Expert Level, 20-30 minutes)
Complex scenarios and advanced concepts.
- Edge cases and gotchas
- Performance optimization
- System design with types
- Trade-offs and trade-off decisions

## Interview Preparation Tips

### 1. Understand, Don't Memorize
- Know WHY you choose a type, not just WHAT to choose
- Be able to explain trade-offs
- Understand memory implications

### 2. Code Examples
- Be ready to write quick code examples
- Show practical understanding
- Can explain your reasoning

### 3. Common Questions
- These are real questions asked in interviews
- Companies test fundamentals first
- Can lead to architecture discussions

### 4. Think Aloud
- Explain your reasoning
- Show decision-making process
- Ask clarifying questions

## How to Use This Section

1. **Quick Review**: Read all questions and answers (30 minutes)
2. **Deep Study**: Read each question, think about answer, then check solution (2-3 hours)
3. **Practice**: Answer without looking, then verify (1 hour)
4. **Mock Interview**: Have someone ask you questions (30 minutes)

## Key Concepts to Know

Before answering questions, ensure you understand:

### Value Types
- Stack allocation
- Copied by value
- No garbage collection
- Examples: int, bool, struct

### Reference Types
- Heap allocation
- Copied by reference
- Garbage collected
- Examples: string, class, array

### Collections
- When to use List vs Dictionary vs HashSet
- Performance implications
- Memory considerations

### Strings
- Immutability
- StringBuilder usage
- Performance considerations

### Classes vs Structs
- When to use each
- Inheritance possibilities
- Performance trade-offs

## Quick Reference: Things to Remember

✓ **int** - use for most integers
✓ **decimal** - use for money, NEVER float
✓ **string** - immutable, use StringBuilder for loops
✓ **List<T>** - default for collections
✓ **Dictionary<K,V>** - for key-value lookups
✓ **struct** - only for small immutable data
✓ **class** - default for complex objects
✓ Check for **null** before using reference types

## Common Interview Traps

❌ **Using float for money**
```csharp
float total = 0.1f + 0.2f;  // Not exactly 0.3!
```

❌ **String concatenation in loops**
```csharp
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;  // Creates 1000 strings!
}
```

❌ **Not checking null**
```csharp
string text = GetText();
int length = text.Length;  // Could crash!
```

❌ **Modifying collection during iteration**
```csharp
foreach (var item in list) {
    if (item > 5) list.Remove(item);  // Exception!
}
```

---

## Questions by Topic

### Value Types
- Easy: What are value types?
- Easy: Difference between int and long?
- Medium: Why use struct instead of class?
- Medium: What is boxing/unboxing?
- Hard: When should you use nullable value types?

### Reference Types
- Easy: What is a reference type?
- Easy: Why is string immutable?
- Medium: When should you return IReadOnlyList?
- Medium: How do interfaces support polymorphism?
- Hard: How would you design a type for [specific scenario]?

### Collections
- Easy: What's the difference between array and List?
- Easy: When should you use Dictionary?
- Medium: Why use HashSet instead of List for lookups?
- Medium: What's the performance of each collection?
- Hard: How would you choose a collection for [specific use case]?

### Strings
- Easy: Is string a value or reference type?
- Easy: Why use StringBuilder?
- Medium: How does string interning work?
- Medium: What's the cost of string operations?
- Hard: Design a string processing solution for [specific scenario]?

### Classes and Inheritance
- Easy: What are the access modifiers?
- Easy: What's the difference between class and struct?
- Medium: When should you use abstract classes vs interfaces?
- Medium: Explain method override vs virtual?
- Hard: Design a class hierarchy for [specific domain]?

---

## How to Answer Interview Questions

### 1. Clarify the Question
```
"Did you mean... " or "Let me make sure I understand..."
```

### 2. Think Out Loud
```
"Let me think about this... First I need to consider..."
```

### 3. Provide Code Example
```
Show a quick code snippet
Explain what it does
Point out why it matters
```

### 4. Mention Trade-offs
```
"The benefit is X, the drawback is Y"
```

### 5. Show Your Reasoning
```
"I would choose this because..."
```

---

## Success Criteria for Interview

✓ Answer questions correctly
✓ Explain your reasoning
✓ Provide working code examples
✓ Discuss trade-offs
✓ Ask clarifying questions
✓ Show deep understanding, not just surface knowledge
✓ Can explain "why" not just "what"

---

## Navigation

- **Easy Questions**: `01-Easy/00-Easy-Questions.md`
- **Medium Questions**: `02-Medium/00-Medium-Questions.md`
- **Hard Questions**: `03-Hard/00-Hard-Questions.md`

---

**Tip**: Start with Easy, move to Medium, then Hard. Don't skip the explanation of WHY each answer is correct.
