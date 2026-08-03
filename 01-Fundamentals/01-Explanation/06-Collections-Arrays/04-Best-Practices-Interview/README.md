# Best Practices & Interview Questions

## Overview
Learn professional practices for working with collections and prepare for technical interviews with 18 comprehensive questions across difficulty levels.

## Section 1: Best Practices

### 15 Essential Guidelines
- Choose right collection type
- Use LINQ for clarity
- Check bounds safely
- Don't modify while iterating
- Understand performance
- Implement proper equality
- Handle null safely
- Materialize LINQ when needed

**Time:** 20-30 minutes

**File:** 01-Best-Practices/00-Best-Practices.md

---

## Section 2: Common Mistakes

### 20 Critical Mistakes to Avoid
- Array index out of bounds
- Dictionary key not found
- Modifying while iterating
- Wrong collection type choice
- Null reference errors
- Multiple LINQ enumeration
- Mutable dictionary keys
- Collection comparison errors

**Time:** 20-30 minutes

**File:** 02-Common-Mistakes/00-Common-Mistakes.md

---

## Section 3: Interview Questions

### 18 Questions Across 3 Difficulty Levels

#### Easy (10 questions) - 8-10 min each
Topics: Array/List basics, Dictionary, HashSet, Queue/Stack, LINQ fundamentals

**File:** 03-Interview-Questions/01-Easy/00-Easy-Questions.md

#### Medium (10 questions) - 10-15 min each
Topics: Cache design, performance optimization, LINQ materialization, complex queries

**File:** 03-Interview-Questions/02-Medium/00-Medium-Questions.md

#### Hard (10 questions) - 15-20 min each
Topics: LRU cache, thread safety, large-scale optimization, architecture patterns

**File:** 03-Interview-Questions/03-Hard/00-Hard-Questions.md

---

## Quick Reference: Key Concepts

### Best Practices Checklist

✓ **Choose Right Collection**
```csharp
Dictionary<K, V> for fast key lookup
List<T> for dynamic indexed access
HashSet<T> for unique values only
Queue<T> for FIFO processing
Stack<T> for LIFO processing
```

✓ **Safe Access**
```csharp
if (dict.TryGetValue(key, out var value)) { }
if (index >= 0 && index < list.Count) { }
if (collection != null && collection.Count > 0) { }
```

✓ **Efficient Iteration**
```csharp
foreach (var item in list) { }  // Simple iteration
for (int i = 0; i < list.Count; i++) { }  // Need index
var result = list.Where(x => condition);  // LINQ
```

✓ **Performance**
```csharp
HashSet<T>.Contains(item)  // O(1)
Dictionary<K,V>[key]       // O(1)
List<T>[index]             // O(1)
List<T>.Contains(item)     // O(n)
List<T>.Remove(item)       // O(n)
```

### Common Mistakes to Avoid

❌ **Array bounds**
```csharp
// WRONG
int value = arr[10];  // May throw!

// RIGHT
if (index >= 0 && index < arr.Length) {
    int value = arr[index];
}
```

❌ **Dictionary access**
```csharp
// WRONG
int age = dict["Bob"];  // KeyNotFoundException!

// RIGHT
if (dict.TryGetValue("Bob", out int age)) { }
```

❌ **Modify while iterating**
```csharp
// WRONG
foreach (var item in list) {
    if (condition) list.Remove(item);  // Breaks!
}

// RIGHT
foreach (var item in list.ToList()) {
    if (condition) list.Remove(item);
}
```

---

## Interview Preparation Guide

### Difficulty Levels

**Easy Questions**
- Basic concepts
- Collection types
- Common operations
- Simple patterns

✓ **Topics:** Array, List, Dictionary, HashSet, Queue, Stack, LINQ basics

✓ **Success:** Know definitions, common methods, basic use cases

### Medium Questions
- Design scenarios
- Performance analysis
- Complex queries
- Pattern implementation

✓ **Topics:** Cache design, LINQ materialization, grouping, joining, complex filtering

✓ **Success:** Explain trade-offs, write working code, discuss performance

### Hard Questions
- Large-scale optimization
- Thread safety
- Architecture decisions
- Complex real-world patterns

✓ **Topics:** LRU cache, circular buffers, thread-safe collections, benchmarking

✓ **Success:** Deep understanding, production-ready code, explain implications

---

## Interview Tips & Strategies

### Before the Interview

1. **Understand fundamentals**
   - Know each collection type
   - Understand performance (O notation)
   - Practice writing code

2. **Study patterns**
   - Common scenarios
   - Real-world examples
   - Trade-offs analysis

3. **Practice coding**
   - Write solutions
   - Test your code
   - Discuss performance

### During the Interview

1. **Clarify the question**
   - Ask for requirements
   - Discuss constraints
   - Confirm understanding

2. **Think aloud**
   - Explain your approach
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
// Pattern 1: Cache lookup
Dictionary<K, V> cache = new Dictionary<K, V>();
if (!cache.TryGetValue(key, out var value)) {
    value = ComputeExpensive(key);
    cache[key] = value;
}

// Pattern 2: Frequency counting
Dictionary<T, int> frequency = new Dictionary<T, int>();
foreach (var item in items) {
    frequency[item] = frequency.ContainsKey(item) 
        ? frequency[item] + 1 
        : 1;
}

// Pattern 3: Deduplication
HashSet<T> unique = new HashSet<T>(items);

// Pattern 4: Top-N selection
var topN = items
    .OrderByDescending(x => x.Score)
    .Take(10)
    .ToList();

// Pattern 5: Grouping and aggregation
var grouped = items
    .GroupBy(x => x.Category)
    .Select(g => new {
        Category = g.Key,
        Count = g.Count(),
        Total = g.Sum(x => x.Value)
    })
    .ToList();
```

---

## Study Schedule

### Day 1-2: Best Practices (2-3 hours)
- Read all 15 guidelines
- Study code examples
- Understand trade-offs

### Day 3-4: Common Mistakes (2-3 hours)
- Review each mistake
- Study solutions
- Practice avoiding them

### Day 5-7: Interview Questions (6-8 hours)
- **Day 5:** Easy questions (8-10 questions)
- **Day 6:** Medium questions (10 questions)
- **Day 7:** Hard questions (10 questions)

### Day 8: Review & Practice
- Re-read key concepts
- Write code for scenarios
- Mock interview practice

---

## Self-Assessment

Can you:
- [ ] List best practices for each collection?
- [ ] Identify and fix common mistakes?
- [ ] Answer Easy questions confidently?
- [ ] Explain Medium question solutions?
- [ ] Solve Hard questions with production-ready code?
- [ ] Discuss performance implications?
- [ ] Design real-world collection solutions?

---

## Quick Links

- [Best Practices](01-Best-Practices/00-Best-Practices.md)
- [Common Mistakes](02-Common-Mistakes/00-Common-Mistakes.md)
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
