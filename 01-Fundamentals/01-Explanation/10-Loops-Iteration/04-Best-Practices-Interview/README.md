# Best Practices & Interview Preparation

## Overview

This section covers loop best practices, common mistakes, and interview preparation. Prepare for technical discussions and write production-quality loop code.

## Learning Path

### Beginner
- Finish other sections first (Fundamentals, Control, Advanced)
- Then come here

### Intermediate
1. **[Best Practices](01-Best-Practices/00-Best-Practices.md)** - Start here
   - Loop selection guidelines
   - Code clarity
   - Performance optimization
   - Testing strategies

2. **[Common Mistakes](02-Common-Mistakes/00-Common-Mistakes.md)** - Learn from errors
   - Off-by-one errors
   - Infinite loops
   - Collection modification
   - Performance pitfalls

### Interview Prep
3. **[Interview Questions](03-Interview-Questions/README.md)** - 15 progressive questions
   - Easy questions (foundation)
   - Medium questions (application)
   - Hard questions (expertise)
   - Interview strategies

## Quick Reference

### Best Practices Checklist

- [ ] Chose right loop type
- [ ] Code is simple and readable
- [ ] Variable names are descriptive
- [ ] No collection modification during iteration
- [ ] Performance acceptable
- [ ] Handles edge cases
- [ ] Proper break/continue usage
- [ ] Documented complex logic

### Common Mistakes Checklist

- [ ] Off-by-one errors? (use <, not <=)
- [ ] Infinite loops? (check condition and counter)
- [ ] Modifying collection? (iterate copy)
- [ ] Loop variable closure? (create local copy)
- [ ] O(n²) performance? (nested loops)
- [ ] Array bounds? (check before access)
- [ ] Variable names? (descriptive)
- [ ] Tested edge cases? (empty, single, large)

## Topics Covered

### Best Practices
1. Choose the right loop type
2. Keep loops simple
3. Use meaningful names
4. Avoid collection modification
5. Cache collection length
6. Use LINQ for complex operations
7. Use break/continue appropriately
8. Optimize nested loops
9. Use yield for large sequences
10. Document behavior

### Common Mistakes
1. Off-by-one errors
2. Infinite loops
3. Collection modification
4. Loop variable closure
5. Forgetting break in switch
6. Break/continue scope confusion
7. Wrong loop type
8. Inefficient nested loops
9. Array bounds errors
10. Performance issues

### Interview Topics
1. Loop type selection
2. Performance analysis
3. Optimization strategies
4. Edge case handling
5. Design patterns
6. Real-world scenarios
7. LINQ vs loops
8. Parallel iteration
9. Custom iterators
10. Code review skills

## Code Examples

### Best Practice Example

```csharp
// GOOD: Demonstrates best practices
public int FindFirst(List<int> numbers, int target)
{
    // Clear name, specific purpose
    if (numbers == null || numbers.Count == 0)
        return -1; // Edge case
    
    // Cache collection length
    int count = numbers.Count;
    
    // Use for when index needed
    for (int i = 0; i < count; i++)
    {
        if (numbers[i] == target)
            return i; // Early exit with break alternative
    }
    
    return -1;
}
```

### Mistake Example with Fix

```csharp
// WRONG: Multiple mistakes
var result = new List<string>();
foreach (var item in items)
{
    items.Remove(item); // Modifying during iteration!
    if (item.Length > 0) // Redundant, should use continue
        result.Add(item.ToUpper());
}

// FIXED: Best practices applied
var result = items
    .Where(i => i.Length > 0)
    .Select(i => i.ToUpper())
    .ToList();
```

### Interview Question Example

**Q: How would you optimize this nested loop?**

```csharp
// SLOW: O(n²)
bool found = false;
for (int i = 0; i < list1.Count; i++)
{
    for (int j = 0; j < list2.Count; j++)
    {
        if (list1[i].Id == list2[j].Id)
        {
            found = true;
            break;
        }
    }
    if (found) break;
}

// FAST: O(n)
var ids = new HashSet<int>(list2.Select(x => x.Id));
bool found = list1.Any(x => ids.Contains(x.Id));

// Or even simpler
bool found = list1.Select(x => x.Id)
    .Intersect(list2.Select(x => x.Id))
    .Any();
```

## Practice Path

### Stage 1: Foundation (Days 1-3)
- [ ] Read Best Practices
- [ ] Read Common Mistakes
- [ ] Run examples
- [ ] Try exercises

### Stage 2: Application (Days 4-6)
- [ ] Answer Easy interview questions
- [ ] Explain your reasoning
- [ ] Consider alternatives
- [ ] Write test cases

### Stage 3: Mastery (Days 7-10)
- [ ] Answer Medium interview questions
- [ ] Discuss with peer
- [ ] Review real code
- [ ] Build projects

### Stage 4: Expert (Days 11+)
- [ ] Answer Hard interview questions
- [ ] Lead code reviews
- [ ] Optimize production code
- [ ] Mentor others

## Interview Strategy

### Before Interview
1. Review all loop types
2. Study common mistakes
3. Understand LINQ
4. Practice problems
5. Prepare examples

### During Interview

**Listen carefully**
```
Take time to understand the question.
Ask clarifying questions if needed.
```

**Think out loud**
```
Explain your reasoning.
Show your thought process.
Discuss trade-offs.
```

**Provide solutions**
```
Start with simple solution.
Optimize if needed.
Discuss complexity.
```

**Verify edge cases**
```
Empty collections
Single item
Large data
Null values
```

## Performance Quick Facts

| Scenario | Time | Notes |
|----------|------|-------|
| Single loop | O(n) | Linear, acceptable |
| Nested loops | O(n²) | Quadratic, often slow |
| Break early | O(n) avg | Best case faster |
| HashSet lookup | O(1) | Use for duplicates |
| LINQ chain | O(n) | Multiple operations |
| Parallel | ~n/cores | Multi-threaded benefit |
| Yield | O(n) | Memory efficient |

## Real-World Scenarios

### Scenario 1: Processing Large Files
```csharp
// Use yield for memory efficiency
public IEnumerable<string> ReadLines(string filePath)
{
    using (var reader = new StreamReader(filePath))
    {
        string line;
        while ((line = reader.ReadLine()) != null)
            yield return line;
    }
}

// Usage - processes one line at a time
foreach (var line in ReadLines("huge_file.txt"))
    ProcessLine(line);
```

### Scenario 2: Data Filtering
```csharp
// Use LINQ for clarity
var activeCustomers = customers
    .Where(c => c.Status == "Active")
    .OrderBy(c => c.Name)
    .ToList();
```

### Scenario 3: Batch Processing
```csharp
// Use chunks with LINQ
var batches = items
    .Select((item, index) => new { item, index })
    .GroupBy(x => x.index / batchSize)
    .Select(g => g.Select(x => x.item).ToList());
```

## Common Interview Questions

### Easy (Foundation)
1. Difference between for, while, foreach?
2. When would you use each loop type?
3. What does break do?
4. What does continue do?
5. How do you iterate with index?

### Medium (Application)
6. How would you optimize nested loops?
7. When would you use LINQ vs loops?
8. How do you handle collection modification?
9. What's an off-by-one error?
10. How do you exit nested loops?

### Hard (Expert)
11. Design a custom iterator.
12. When would you use yield?
13. Parallel vs sequential - trade-offs?
14. Optimize an O(n²) algorithm.
15. Architecture for large-scale iteration?

## Assessment Checklist

After completing this section, you should be able to:

✓ Choose the right loop type
✓ Write clean, readable loops
✓ Identify and fix loop bugs
✓ Optimize loop performance
✓ Answer interview questions
✓ Discuss trade-offs
✓ Design custom iterators
✓ Handle edge cases

## Recommended Reading Order

1. **First Time**: Best Practices → Common Mistakes → Interview-Overview
2. **Before Interview**: Easy → Medium → Hard questions
3. **Code Review**: Use Practices checklist
4. **On the Job**: Reference Common Mistakes

## Links

- **Previous**: [Advanced-Iteration](../03-Advanced-Iteration/README.md)
- **Best Practices**: [01-Best-Practices](01-Best-Practices/00-Best-Practices.md)
- **Common Mistakes**: [02-Common-Mistakes](02-Common-Mistakes/00-Common-Mistakes.md)
- **Interview Prep**: [03-Interview-Questions](03-Interview-Questions/README.md)
- **Main Loops**: [Loops Overview](../README.md)

---

**Pro Tip**: When stuck on an interview question, ask: "Is there a simpler way?" Usually there is.
