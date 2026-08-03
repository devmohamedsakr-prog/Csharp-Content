# Best Practices, Common Mistakes, and Interview Questions

## Overview

This section provides guidance for effective operator usage, common pitfalls to avoid, and interview preparation.

## Categories

### Best Practices
Guidelines for writing correct and efficient code with operators.

**Files**: `01-Best-Practices/00-Best-Practices.md`

**Topics Covered**:
- Arithmetic best practices
- Assignment efficiency
- Comparison safety
- Logical operator ordering
- Null handling patterns
- Performance considerations
- Readability tips

**Key Principles**:
- Use appropriate types
- Handle edge cases
- Write clear conditions
- Optimize for safety first
- Enable nullable reference types

### Common Mistakes
15 real mistakes developers make and their solutions.

**Files**: `02-Common-Mistakes/00-Common-Mistakes.md`

**Mistakes Covered**:
1. = vs == confusion
2. Integer division truncation
3. String concatenation in loops
4. Missing null checks
5. Wrong operator precedence
6. Bitwise vs logical confusion
7. Float for money
8. Forgetting short-circuit
9. Complex nested ternary
10. Modifying collection during iteration
11. Double negation
12. String case sensitivity
13. Multiple increments in expression
14. Division by zero
15. Overflow not handled

**Pattern**: Mistake, why it's wrong, correct solution

### Interview Questions
21 real interview questions organized by difficulty.

**Files**: `03-Interview-Questions/README.md`

**Easy Questions** (8):
- = vs ==
- Integer division
- Short-circuit evaluation
- Operator precedence
- Bitwise operators
- Null-coalescing
- Ternary operator
- & vs &&

**Medium Questions** (7):
- Pre/post increment in expressions
- Bitwise permission systems
- Shift operators
- Pre vs post performance
- ?? vs ||
- Operator precedence chains
- Safe null-aware calculation

**Hard Questions** (6):
- Operator overloading design
- Expression-bodied members
- Power of 2 optimization
- Expression trees
- Checked vs unchecked arithmetic
- Bitwise vs arithmetic performance

## Learning Paths

### Path 1: Quick Review (30 minutes)
1. Read Best Practices overview
2. Skim Common Mistakes list
3. Review Easy interview questions

### Path 2: Intermediate Study (2-3 hours)
1. Study all Best Practices
2. Read all Common Mistakes
3. Answer Medium questions
4. Check answers

### Path 3: Interview Prep (1 day)
1. Master all Common Mistakes
2. Practice all Easy questions
3. Work through Medium questions
4. Study Hard questions
5. Mock interview session

### Path 4: Expert Deep Dive (2 days)
1. Study each section thoroughly
2. Write your own examples
3. Implement bitwise systems
4. Create custom operators
5. Teach someone else

## Key Takeaways

### Most Critical
- Know the 15 common mistakes
- Understand null handling patterns
- Remember operator precedence
- Know when short-circuit matters

### For Interviews
- Explain your reasoning
- Show code examples
- Discuss trade-offs
- Know precedence by heart
- Avoid the 15 mistakes

### For Production Code
- Use appropriate types
- Check for nulls/zeros
- Write clear conditions
- Avoid complex nesting
- Enable nullable reference types

## Quick Decision Guide

### Which operator?
```
Need to test values?         → Use comparison (==, <, >)
Need to combine conditions?  → Use logical (&&, ||, !)
Need to check null?          → Use null-coalescing (??)
Need safe navigation?        → Use null-conditional (?.)
Need default value?          → Use null-coalescing (??)
Need simple branch?          → Use ternary (? :)
Need complex logic?          → Use switch expression
Need bit manipulation?       → Use bitwise (&, |, ^)
```

## Interview Question Quick Ref

| Question | Key Points |
|----------|-----------|
| = vs == | Assignment vs comparison |
| Integer division | Truncates, cast for precision |
| Short-circuit | && stops if first false, \|\| stops if first true |
| Precedence | Multiplication before addition |
| Bitwise | & (AND), \| (OR), ^ (XOR), ~ (NOT), << , >> |
| Null-coalescing | Returns left if not null, else right |
| Ternary | condition ? true : false |
| & vs && | & is bitwise (always evaluates), && is logical (short-circuits) |

## Success Criteria

✓ Know all 15 common mistakes
✓ Answer all Easy questions correctly
✓ Answer Medium questions with explanation
✓ Discuss Hard questions thoughtfully
✓ Know operator precedence
✓ Understand null-handling
✓ Can explain trade-offs

## Navigation

- **Parent**: [Operators](../README.md)
- **Best Practices**: `01-Best-Practices/00-Best-Practices.md`
- **Common Mistakes**: `02-Common-Mistakes/00-Common-Mistakes.md`
- **Interview Overview**: `03-Interview-Questions/00-Interview-Overview.md`
- **Easy Questions**: `03-Interview-Questions/01-Easy/00-Easy-Questions.md`
- **Medium Questions**: `03-Interview-Questions/02-Medium/00-Medium-Questions.md`
- **Hard Questions**: `03-Interview-Questions/03-Hard/00-Hard-Questions.md`

---

**Tip**: The best way to learn is to understand concepts deeply, avoid mistakes, and practice real scenarios.
