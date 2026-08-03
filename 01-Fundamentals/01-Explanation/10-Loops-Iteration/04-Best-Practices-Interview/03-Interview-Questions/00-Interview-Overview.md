# Loops and Iteration Interview Questions

## Overview

This section contains 15 progressive interview questions on loops and iteration in C#, organized by difficulty level. These questions test practical knowledge and design thinking around loop selection and performance.

## Question Distribution

| Difficulty | Count | Topics |
|-----------|-------|--------|
| Easy | 5 | Loop types, basic concepts, simple patterns |
| Medium | 5 | Loop selection, optimization, design patterns |
| Hard | 5 | Complex scenarios, performance analysis, architecture |

## Key Concepts Tested

### Loop Types and Selection
- Choosing between for, while, foreach, do-while
- Understanding when to use each
- Performance implications

### Iteration Patterns
- Common loop patterns
- Generator functions with yield
- LINQ vs loops
- Parallel iteration

### Performance and Optimization
- O(n) vs O(n²) complexity
- Cache efficiency
- Memory usage
- Early exit strategies

### Problem-Solving
- Identifying loop bugs
- Optimizing inefficient code
- Handling edge cases

## Interview Tips

### Before the Interview
1. Review all loop types
2. Study common mistakes
3. Understand LINQ alternatives
4. Think about performance

### During the Interview
1. Take time to understand the question
2. Think about edge cases
3. Discuss trade-offs
4. Show your reasoning
5. Offer multiple solutions

### Answering Strategies

**Strategy 1: Explain Your Thinking**
```csharp
// "I would use foreach here because:
// 1. No index needed
// 2. Cleaner, more readable
// 3. Works with any IEnumerable"
foreach (var item in items)
{
    ProcessItem(item);
}
```

**Strategy 2: Consider Alternatives**
```csharp
// "This could be done with for loop,
// but LINQ is more expressive:
var result = items
    .Where(x => x.IsActive)
    .Select(x => x.Name)
    .ToList();
```

**Strategy 3: Discuss Performance**
```csharp
// "This is O(n²) with nested loops.
// Better approach using HashSet is O(n):"
var ids = new HashSet<int>(list2.Select(x => x.Id));
var matches = list1.Where(x => ids.Contains(x.Id));
```

## Quick Reference

### Loop Type Selection Matrix

| Scenario | Best Choice | Reason |
|----------|------------|--------|
| Simple collection iteration | foreach | Cleanest, no index |
| Need index access | for | Index required |
| Reverse iteration | for | Control direction |
| Condition-based | while | No count known |
| At least once | do-while | Post-test check |
| Lazy evaluation | yield | Memory efficient |

### Performance Quick Facts

- foreach: Fast, clean
- for: Fastest, has index
- while: Good for conditions
- Nested loops: O(n²), watch out!
- LINQ: Optimized, readable
- yield: Memory efficient

## Question Topics

### Easy Questions (Foundation)
1. Loop type differences
2. For loop variations
3. While vs do-while
4. Foreach on collections
5. Break and continue

### Medium Questions (Application)
6. Loop type selection
7. Performance optimization
8. Pattern identification
9. LINQ vs loops
10. Iterator design

### Hard Questions (Expert)
11. Complex performance analysis
12. Concurrent iteration
13. Custom iterators
14. Generator optimization
15. Real-world design decisions

## Self-Assessment

After completing these questions, you should be able to:

✓ Choose the right loop for any scenario
✓ Identify and fix loop bugs
✓ Analyze loop performance
✓ Design generators with yield
✓ Decide between LINQ and loops
✓ Optimize nested loop structures
✓ Handle edge cases
✓ Explain trade-offs clearly

## Practice Approach

### Level 1: Foundation
- [ ] Read Easy questions
- [ ] Attempt without help
- [ ] Check answers
- [ ] Understand mistakes

### Level 2: Building Skill
- [ ] Read Medium questions
- [ ] Attempt with time limit
- [ ] Explain out loud
- [ ] Consider alternatives

### Level 3: Mastery
- [ ] Read Hard questions
- [ ] Discuss with peer
- [ ] Defend your choice
- [ ] Consider edge cases

## Common Interview Patterns

### Pattern 1: "Why This Loop?"
**Question**: "Why did you choose a for loop here?"
**Good Answer**: "I need the index to process pairs. I could use for or foreach with LINQ, but for is most direct here."

### Pattern 2: "Can You Optimize?"
**Question**: "Can you optimize this nested loop?"
**Good Answer**: "Yes, this is O(n²). Using a HashSet makes it O(n)..."

### Pattern 3: "Edge Cases?"
**Question**: "What about empty collections?"
**Good Answer**: "For and foreach handle empty gracefully. Do-while always executes once..."

### Pattern 4: "LINQ vs Loops?"
**Question**: "Would you use LINQ instead?"
**Good Answer**: "LINQ is cleaner here: items.Where(...).Select(...). Loop is more explicit..."

## Resources

- Benchmark different loop types
- Analyze Big O complexity
- Read LINQ source code
- Study generator functions
- Profile real applications

## Next Steps

1. Choose your level (Easy/Medium/Hard)
2. Read question carefully
3. Attempt solution
4. Check provided answer
5. Understand reasoning
6. Try variations

## Good Luck!

Remember: Interviewers want to see:
- Clear thinking
- Knowledge of trade-offs
- Practical experience
- Ability to optimize
- Good communication

Now pick a difficulty level and get started!

- **[🟢 Easy Questions](01-Easy/00-Easy-Questions.md)** - Start here
- **[🟡 Medium Questions](02-Medium/00-Medium-Questions.md)** - After easy
- **[🔴 Hard Questions](03-Hard/00-Hard-Questions.md)** - Challenge yourself
