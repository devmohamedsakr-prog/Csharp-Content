# Collections and Arrays

## Overview

Master collections and arrays - the fundamental data structures in C#. This comprehensive guide covers arrays, generic collections, common patterns, best practices, and real-world design scenarios.

**Total Learning Time:** 6-8 hours  
**Depth:** Beginner to Advanced  
**Files:** 20+ focused documents with code examples

---

## Quick Navigation

### 📚 Learning Sections

| Section | Time | Level | Topics |
|---------|------|-------|--------|
| **Arrays** | 1 hour | Beginner | Basics, Multi-dimensional, Operations |
| **Generic Collections** | 1.5 hours | Beginner | List, Dictionary, HashSet, Queue, Stack |
| **Collection Patterns** | 1.5 hours | Intermediate | Choosing, Iteration, LINQ |
| **Best Practices & Interview** | 2-3 hours | Intermediate-Advanced | Patterns, Mistakes, 18 Interview Questions |

---

## Section 1: Arrays 📦
**Master fixed-size collections with fast indexed access**

- **Array Basics** - Declaration, access, iteration
- **Multi-Dimensional Arrays** - 2D, 3D, jagged arrays
- **Array Operations** - Sorting, searching, LINQ
- **README** - Full learning guide

**Key concepts:**
- Fixed size, 0-indexed, O(1) access
- Type-safe, memory efficient
- Fast iteration, direct element access

[→ Go to Arrays](01-Arrays/README.md)

---

## Section 2: Generic Collections 🏗️
**Flexible, type-safe collections for dynamic data**

- **List<T>** - Dynamic arrays
- **Dictionary<K,V>** - Key-value pairs
- **HashSet<T>** - Unique values
- **Queue<T> & Stack<T>** - Specialized collections
- **README** - Collection comparison and selection

**Collection Types:**
```
List<T>        → O(1) access, O(n) remove, indexed
Dictionary     → O(1) lookup, key-based access
HashSet<T>     → O(1) contains, unique only
Queue<T>       → FIFO, O(1) operations
Stack<T>       → LIFO, O(1) operations
```

**When to use each:**
- **List** - Indexed access, dynamic size
- **Dictionary** - Fast key lookup
- **HashSet** - Unique values, membership testing
- **Queue** - FIFO processing (tasks, events)
- **Stack** - LIFO processing (undo, call stack)

[→ Go to Generic Collections](02-Generic-Collections/README.md)

---

## Section 3: Collection Patterns 🎯
**Practical patterns for real-world collection usage**

- **Choosing Collections** - Decision matrix
- **Iteration Patterns** - For, foreach, while, LINQ
- **LINQ with Collections** - Filtering, grouping, aggregation
- **README** - Pattern reference and examples

**Common Patterns:**
```csharp
// Caching
Dictionary<int, User> cache = new();
if (!cache.TryGetValue(id, out var user)) {
    user = LoadUser(id);
    cache[id] = user;
}

// Deduplication
var unique = new HashSet<int>(items);

// Frequency counting
var freq = new Dictionary<string, int>();
foreach (var word in words) {
    freq[word] = freq.ContainsKey(word) 
        ? freq[word] + 1 : 1;
}

// Top-N selection
var top10 = items
    .OrderByDescending(x => x.Score)
    .Take(10)
    .ToList();
```

[→ Go to Collection Patterns](03-Collection-Patterns/README.md)

---

## Section 4: Best Practices & Interview 🎓
**Professional practices and interview preparation**

### Best Practices (15 Guidelines)
- Choose right collection type
- Use LINQ for clarity
- Check bounds safely
- Don't modify while iterating
- Understand performance
- Implement proper equality
- Handle nulls safely
- Much more...

### Common Mistakes (20 Items)
- Array bounds
- Dictionary key lookup
- Modifying during iteration
- Wrong collection choice
- Null references
- Multiple LINQ enumeration
- And more...

### Interview Questions (18 Total)
- **Easy (10 questions)** - 8-10 min each
  - Array/List basics, Dictionary, HashSet, Queue/Stack
- **Medium (10 questions)** - 10-15 min each
  - Cache design, performance, complex LINQ
- **Hard (10 questions)** - 15-20 min each
  - LRU cache, threading, large-scale optimization

[→ Go to Best Practices & Interview](04-Best-Practices-Interview/README.md)

---

## Learning Paths

### Path 1: Beginner (4-5 hours)
Perfect for learning the fundamentals

1. **Arrays** (1 hour)
   - Array Basics
   - Multi-Dimensional Arrays

2. **Generic Collections** (1.5 hours)
   - List<T>, Dictionary, HashSet
   - Queue/Stack basics

3. **Collection Patterns** (1.5 hours)
   - Choosing Collections
   - Iteration Patterns

4. **Best Practices** (30 min)
   - Read best practices overview
   - Review common mistakes

### Path 2: Intermediate (6-7 hours)
Deepening understanding with patterns

1. **Complete Beginner Path** (4-5 hours)

2. **Advanced Arrays** (30 min)
   - Array Operations and LINQ

3. **Advanced Collections** (1 hour)
   - All collection types deeply
   - Performance characteristics

4. **Collection Patterns Deep Dive** (1 hour)
   - LINQ mastery
   - Complex scenarios

5. **Best Practices & Common Mistakes** (1 hour)

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

### Collection Selection

```csharp
// Need to access by index?
List<T> list = new();

// Need fast key lookup?
Dictionary<K, V> dict = new();

// Need unique values only?
HashSet<T> set = new();

// Need FIFO processing?
Queue<T> queue = new();

// Need LIFO processing?
Stack<T> stack = new();

// Need fixed size?
T[] array = new T[size];
```

### Performance Cheat Sheet

| Operation | List | Dictionary | HashSet | Queue | Stack |
|-----------|------|-----------|---------|-------|-------|
| Add | O(1)* | O(1) | O(1) | O(1) | O(1) |
| Remove | O(n) | O(1) | O(1) | O(1) | O(1) |
| Lookup | O(n) | O(1) | O(1) | - | - |
| Access | O(1) | - | - | - | - |

*List is amortized O(1)

### Common LINQ Operations

```csharp
// Filter
numbers.Where(x => x > 5)

// Transform
numbers.Select(x => x * 2)

// Sort
numbers.OrderBy(x => x)

// Group
items.GroupBy(x => x.Category)

// Aggregate
numbers.Sum(), .Average(), .Max()

// Combine
numbers.Where(...).Select(...).OrderBy(...)
```

---

## Key Takeaways

✓ **Arrays** are fixed-size, fast indexed access  
✓ **Collections** are flexible, type-safe, dynamic  
✓ **Dictionary** for fast key lookup  
✓ **HashSet** for unique values  
✓ **Queue/Stack** for specialized patterns  
✓ **LINQ** for clear data queries  
✓ **Understand performance** (O notation)  
✓ **Choose the right type** for your use case  
✓ **Safe access** prevents runtime errors  
✓ **Best practices** improve code quality  

---

## Self-Assessment Checklist

### Arrays
- [ ] Declare and initialize arrays
- [ ] Access elements by index
- [ ] Use multi-dimensional arrays
- [ ] Perform array operations
- [ ] Use LINQ with arrays

### Generic Collections
- [ ] Use List<T> effectively
- [ ] Work with Dictionary safely
- [ ] Understand HashSet uniqueness
- [ ] Use Queue for FIFO
- [ ] Use Stack for LIFO

### Patterns
- [ ] Choose right collection
- [ ] Iterate appropriately
- [ ] Write efficient LINQ
- [ ] Apply common patterns
- [ ] Understand performance

### Best Practices
- [ ] Know 15 best practices
- [ ] Avoid 20 common mistakes
- [ ] Answer Easy questions
- [ ] Solve Medium questions
- [ ] Tackle Hard questions

---

## Common Interview Questions

**Easy Level**
- What's the difference between Array and List?
- When would you use Dictionary?
- Explain Queue vs Stack

**Medium Level**
- Design a cache system
- Optimize collection performance
- Complex LINQ queries

**Hard Level**
- Implement LRU Cache
- Thread-safe collections
- Large-scale optimization

---

## Resources & Files

```
06-Collections-Arrays/
├── 01-Arrays/
│   ├── 00-Array-Basics.md
│   ├── 00-Multi-Dimensional-Arrays.md
│   ├── 00-Array-Operations.md
│   └── README.md
├── 02-Generic-Collections/
│   ├── 00-List.md
│   ├── 00-Dictionary.md
│   ├── 00-HashSet.md
│   ├── 00-Queue-Stack.md
│   └── README.md
├── 03-Collection-Patterns/
│   ├── 00-Choosing-Collections.md
│   ├── 00-Iteration-Patterns.md
│   ├── 00-LINQ-Collections.md
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
2. **Start with Arrays** if new to C#
3. **Move to Generic Collections** for flexibility
4. **Study Patterns** for real-world usage
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

- **Control Flow** - If/else, loops
- **Methods** - Function parameters, return types
- **LINQ** - Advanced querying
- **OOP** - Classes, inheritance, interfaces
- **Exception Handling** - Error management

---

**Happy learning! Master collections and arrays to write better C# code.** 🚀
