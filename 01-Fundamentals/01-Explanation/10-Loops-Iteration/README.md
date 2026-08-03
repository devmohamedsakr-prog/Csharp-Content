# Loops and Iteration

## Overview

Comprehensive guide to loops and iteration in C#. Master loop types, control flow, advanced patterns, and interview preparation through 25+ focused files organized by complexity and use case.

## Quick Start

**New to loops?** Start with [Loop-Fundamentals](01-Loop-Fundamentals/README.md)

**Need to optimize?** See [Loop-Control](02-Loop-Control/README.md) for performance

**Interview coming up?** Jump to [Best-Practices-Interview](04-Best-Practices-Interview/README.md)

## Learning Paths

### Path 1: Complete Beginner (2-3 hours)

1. [Loop-Fundamentals](01-Loop-Fundamentals/README.md)
   - Understand for, while, foreach, do-while
   - Choose the right loop
   - Basic patterns and exercises

2. [Loop-Control](02-Loop-Control/README.md)
   - Break and continue statements
   - Nested loops
   - Early exit strategies

3. [Best-Practices-Interview → Best Practices](04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
   - 10 production guidelines
   - Code clarity checklist

### Path 2: Intermediate (1-2 hours)

1. [Loop-Control](02-Loop-Control/README.md) - Refresher on optimization
2. [Advanced-Iteration](03-Advanced-Iteration/README.md)
   - Yield and custom iterators
   - LINQ-based iteration
   - Parallel processing
3. [Best-Practices-Interview → Common Mistakes](04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md)
   - Identify and fix bugs
   - Performance pitfalls

### Path 3: Interview Prep (1 hour)

1. [Best-Practices-Interview → Interview-Overview](04-Best-Practices-Interview/03-Interview-Questions/README.md)
2. Easy Questions → Medium Questions → Hard Questions
3. Review Best Practices for reference

### Path 4: Optimization Focus (1-2 hours)

1. [Loop-Control](02-Loop-Control/README.md) - Understand O(n²) issues
2. [Advanced-Iteration](03-Advanced-Iteration/README.md) - Efficient patterns
3. [Best-Practices-Interview](04-Best-Practices-Interview/README.md) - Real-world scenarios

## What You'll Learn

### Loop Types & Selection
- For loops (index-based iteration)
- While loops (condition-based)
- Do-While loops (post-test condition)
- Foreach loops (collection iteration)
- When to use each

### Control Flow
- Break statement (exit loop)
- Continue statement (skip iteration)
- Nested loop control
- Early exit patterns
- Performance implications

### Advanced Patterns
- Yield and generators
- Lazy evaluation
- Custom iterators
- LINQ-based iteration
- Parallel.ForEach

### Production Skills
- Code clarity and readability
- Performance optimization
- Error prevention
- Edge case handling
- Testing strategies

## Directory Structure

```
10-Loops-Iteration/
├── 01-Loop-Fundamentals/
│   ├── 01-For-Loops/
│   ├── 02-While-Loops/
│   ├── 03-Do-While-Loops/
│   ├── 04-Foreach-Loops/
│   └── README.md
│
├── 02-Loop-Control/
│   ├── 01-Break-Continue/
│   ├── 02-Nested-Loops/
│   ├── 03-Loop-Optimization/
│   └── README.md
│
├── 03-Advanced-Iteration/
│   ├── 01-Yield-Iterators/
│   ├── 02-LINQ-Iteration/
│   ├── 03-Parallel-Iteration/
│   └── README.md
│
├── 04-Best-Practices-Interview/
│   ├── 01-Best-Practices/
│   ├── 02-Common-Mistakes/
│   ├── 03-Interview-Questions/
│   │   ├── 01-Easy/
│   │   ├── 02-Medium/
│   │   ├── 03-Hard/
│   │   └── README.md
│   └── README.md
│
└── README.md (this file)
```

## Quick Reference Tables

### Loop Type Comparison

| Feature | For | While | Do-While | Foreach |
|---------|-----|-------|----------|---------|
| Index access | ✓ | ✗ | ✗ | ✗ |
| Count known | ✓ | ✗ | ✗ | ✓ |
| Condition-based | ✓ | ✓ | ✓ | ✗ |
| Always executes | ✗ | ✗ | ✓ | ✗ |
| Collections | ✓ | Limited | Limited | ✓ |
| Readability | Good | Good | Fair | Best |
| Performance | Fast | Good | Good | Good |

### Loop Selection Matrix

| Scenario | Best | Reason |
|----------|------|--------|
| Iterate 0-100 | for | Count known, index useful |
| Process collection | foreach | Simple, readable |
| User input validation | do-while | At least once |
| File reading | while | Until EOF |
| Search with index | for | Need index |
| Filter items | foreach or LINQ | Clear intent |
| Large sequences | yield | Memory efficient |
| CPU-intensive | Parallel | Multi-core |

### Performance Guide

| Scenario | Complexity | Notes |
|----------|-----------|-------|
| Single iteration | O(n) | Linear, default |
| Nested loops | O(n²) | Quadratic, problematic |
| Break early | O(n) avg | Best case faster |
| HashSet lookup | O(1) | For duplicates |
| LINQ chain | O(n) | Multiple operations |
| Parallel | O(n/cores) | Multi-threaded |
| Yield | O(n) | Lazy evaluation |

## Code Examples at a Glance

### Simple Loop
```csharp
for (int i = 0; i < 10; i++)
    Console.WriteLine(i);
```

### Collection Iteration
```csharp
foreach (var item in items)
    ProcessItem(item);
```

### Condition-Based
```csharp
while (reader.HasData())
    ProcessData(reader);
```

### Optimization Pattern
```csharp
var ids = new HashSet<int>(list2.Select(x => x.Id));
var matches = list1.Where(x => ids.Contains(x.Id));
```

### Generator Function
```csharp
public IEnumerable<int> Fibonacci(int count)
{
    int a = 0, b = 1;
    for (int i = 0; i < count; i++)
    {
        yield return a;
        int temp = a + b;
        a = b;
        b = temp;
    }
}
```

### LINQ Iteration
```csharp
var result = items
    .Where(x => x.IsActive)
    .OrderBy(x => x.Name)
    .Select(x => x.Id)
    .ToList();
```

## Key Concepts

### Loop Selection
Choose the right loop type for your scenario. Most common: foreach for simple iteration, for when you need index.

### Performance
Watch for O(n²) nested loops. Use HashSet or LINQ to optimize. Single pass is usually sufficient.

### Readability
Clear loop names and structure beat clever optimizations. Comment complex logic.

### Edge Cases
Handle empty collections, null values, and boundary conditions. Test edge cases.

### LINQ vs Loops
LINQ is more readable for complex operations. Loops are faster for simple iteration. Both are valid.

### Memory Efficiency
Use yield for large sequences. Avoid loading entire collections if possible.

### Parallel Processing
Multi-thread CPU-intensive work. Not always faster due to overhead.

## Common Mistakes to Avoid

1. **Off-by-one errors** - Use `<` not `<=`
2. **Infinite loops** - Ensure condition can change
3. **Modifying collection** - Iterate over copy
4. **Loop variable closure** - Create local copy
5. **O(n²) performance** - Nested loops problematic
6. **Wrong loop type** - Choose for task, not habit
7. **Array bounds** - Check index before access
8. **Unclear names** - Use descriptive variables

See [Common Mistakes](04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md) for detailed examples.

## Best Practices Summary

1. Choose the right loop type
2. Keep loops simple and readable
3. Use meaningful variable names
4. Don't modify collections during iteration
5. Cache collection length
6. Use LINQ for complex operations
7. Use break/continue appropriately
8. Watch nested loop performance
9. Use yield for large sequences
10. Document complex behavior

See [Best Practices](04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) for details.

## Interview Preparation

### Self-Assessment

Can you answer these?

- [ ] Difference between for, while, foreach?
- [ ] How to optimize nested loops?
- [ ] When would you use LINQ vs loops?
- [ ] What's an off-by-one error?
- [ ] How to exit nested loops?
- [ ] Design a custom iterator?
- [ ] When to use yield?
- [ ] Parallel vs sequential trade-offs?

See [Interview Questions](04-Best-Practices-Interview/03-Interview-Questions/README.md) for 15 progressive questions.

### Interview Strategy

1. Understand the question fully
2. Think about edge cases
3. Discuss trade-offs
4. Show your reasoning
5. Offer multiple solutions

## Topic Index

### By Difficulty

**Foundational**
- [For Loops](01-Loop-Fundamentals/01-For-Loops/00-For-Loops.md)
- [While Loops](01-Loop-Fundamentals/02-While-Loops/00-While-Loops.md)
- [Foreach Loops](01-Loop-Fundamentals/03-Foreach-Loops/00-Foreach-Loops.md)

**Intermediate**
- [Break & Continue](02-Loop-Control/01-Break-Continue/00-Break-Continue.md)
- [Nested Loops](02-Loop-Control/02-Nested-Loops/00-Nested-Loops.md)
- [LINQ Iteration](03-Advanced-Iteration/02-LINQ-Iteration/00-LINQ-Iteration.md)

**Advanced**
- [Yield & Iterators](03-Advanced-Iteration/01-Yield-Iterators/00-Yield-Iterators.md)
- [Parallel Iteration](03-Advanced-Iteration/03-Parallel-Iteration/00-Parallel-Iteration.md)
- [Custom Iterators](03-Advanced-Iteration/01-Yield-Iterators/00-Yield-Iterators.md)

### By Use Case

**Simple Iteration**
- [Foreach Loops](01-Loop-Fundamentals/04-Foreach-Loops/00-Foreach-Loops.md)
- [LINQ Iteration](03-Advanced-Iteration/02-LINQ-Iteration/00-LINQ-Iteration.md)

**Index-Based**
- [For Loops](01-Loop-Fundamentals/01-For-Loops/00-For-Loops.md)

**Condition-Based**
- [While Loops](01-Loop-Fundamentals/02-While-Loops/00-While-Loops.md)
- [Do-While Loops](01-Loop-Fundamentals/03-Do-While-Loops/00-Do-While-Loops.md)

**Performance-Critical**
- [Loop Optimization](02-Loop-Control/03-Loop-Optimization/00-Loop-Optimization.md)
- [Nested Loops](02-Loop-Control/02-Nested-Loops/00-Nested-Loops.md)

**Memory-Critical**
- [Yield & Iterators](03-Advanced-Iteration/01-Yield-Iterators/00-Yield-Iterators.md)

**Large Data**
- [Parallel Iteration](03-Advanced-Iteration/03-Parallel-Iteration/00-Parallel-Iteration.md)
- [Yield & Iterators](03-Advanced-Iteration/01-Yield-Iterators/00-Yield-Iterators.md)

### By Category

| Category | Files | Focus |
|----------|-------|-------|
| [Loop-Fundamentals](01-Loop-Fundamentals/README.md) | 4 | Core loop types |
| [Loop-Control](02-Loop-Control/README.md) | 3 | Break, continue, nesting |
| [Advanced-Iteration](03-Advanced-Iteration/README.md) | 3 | Yield, LINQ, parallel |
| [Best-Practices-Interview](04-Best-Practices-Interview/README.md) | 3 | Production skills |

## Recommended Study Schedule

### Week 1: Foundation
- Monday: For and While loops
- Tuesday: Foreach and Do-While
- Wednesday: Break and Continue
- Thursday: Nested loops and optimization
- Friday: Review and exercises

### Week 2: Advanced
- Monday: Yield and generators
- Tuesday: LINQ iteration
- Wednesday: Parallel processing
- Thursday: Best practices review
- Friday: Common mistakes deep dive

### Week 3: Interview
- Monday-Wednesday: Easy questions
- Thursday-Friday: Medium questions

### Week 4: Expert
- Monday-Wednesday: Hard questions
- Thursday-Friday: Mock interviews

## Performance Benchmarks

These are typical relative speeds (for 1M items):

- **Foreach**: 1x (baseline)
- **For with index**: 1x (same)
- **LINQ Where**: 1.2x (slight overhead)
- **Nested loops**: n² (very slow)
- **Nested with HashSet**: 1x (optimized)
- **Parallel.ForEach**: 0.3x (with 4 cores)
- **Yield**: 1x (lazy evaluation)

See [Advanced-Iteration](03-Advanced-Iteration/README.md) for measurement details.

## FAQ

**Q: Which loop should I use?**
A: Foreach for collections, for when you need index, while for conditions.

**Q: When should I optimize?**
A: Measure first. Usually simple loops are fast enough.

**Q: Should I always use LINQ?**
A: For clarity yes. For performance, benchmark.

**Q: How do I avoid off-by-one errors?**
A: Use `<` not `<=`, test boundaries, verify length.

**Q: When is parallel worth it?**
A: CPU-intensive work with large datasets, after measuring.

**Q: Should I use yield?**
A: For large/infinite sequences, or deferred execution.

See [Best-Practices-Interview](04-Best-Practices-Interview/README.md) for more FAQs.

## Resources

### Theory
- [Loop-Fundamentals](01-Loop-Fundamentals/README.md) - Core concepts
- [Advanced-Iteration](03-Advanced-Iteration/README.md) - Advanced patterns
- [Best-Practices-Interview](04-Best-Practices-Interview/README.md) - Production skills

### Practice
- Exercises in each section
- Real-world scenarios
- Interview questions

### Reference
- Quick reference tables (above)
- Code examples (above)
- Performance guide (above)

## Next Steps

1. **Choose your level**: Beginner / Intermediate / Advanced
2. **Pick a learning path** from "Learning Paths" section
3. **Read the category README** for that section
4. **Study specific topics** and code examples
5. **Practice exercises** at end of each section
6. **Test your knowledge** with interview questions
7. **Apply to your code** in real projects

## Related Topics

- [Collections & Arrays](../06-Collections-Arrays/README.md) - Lists, arrays
- [Strings](../07-Strings/README.md) - String iteration
- [LINQ-Introduction](../12-LINQ-Introduction/README.md) - Query syntax
- [Nullable Types](../08-Nullable-Types/README.md) - null-safe iteration

## Summary

Loops are fundamental to programming. Master loop types, understand performance implications, and know when to use advanced patterns. This comprehensive guide covers everything from basics to expert-level optimization.

**Start with [Loop-Fundamentals](01-Loop-Fundamentals/README.md) if you're new. Choose a learning path above if you're ready to dive deeper.**

---

**Last Updated**: 2026-08-03
**Total Content**: 25+ focused files, 50+ code examples, 15 interview questions
**Estimated Study Time**: 4-6 hours for complete coverage
