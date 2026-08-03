# Performance and Memory Impact

## Overview

This section covers the performance costs of boxing and memory optimization strategies.

## Learning Path

### Beginner
1. **[Boxing-Overhead](01-Boxing-Overhead/00-Boxing-Overhead.md)** - Start here
   - Performance cost of boxing
   - Benchmarks and measurements
   - GC pressure impact
   - Real-world scenarios

2. **[Memory-Impact](02-Memory-Impact/00-Memory-Impact.md)** - Memory analysis
   - Object header overhead
   - Memory usage calculations
   - GC pressure details
   - Large Object Heap

3. **[Optimization-Strategies](03-Optimization-Strategies/00-Optimization-Strategies.md)** - How to fix it
   - 10 optimization strategies
   - When to optimize
   - Measurement techniques
   - Priority checklist

### Intermediate
- Understand performance trade-offs
- Measure real applications
- Apply optimizations
- Ready for best practices

### Advanced
- Optimize complex systems
- Handle large-scale scenarios
- Make architecture decisions

## Quick Reference

### Performance Impact

```csharp
// Benchmark typical results
ArrayList (1M items):     50-100ms (with boxing)
List<int> (1M items):     2-5ms    (no boxing)
Ratio:                    10-20x slower with boxing
```

### Memory Impact

```csharp
// Memory overhead per boxed value
int (unboxed):     4 bytes
int (boxed):       24 bytes (object header + value + padding)
Overhead:          6x more memory

bool (unboxed):    1 byte
bool (boxed):      24 bytes
Overhead:          24x more memory!
```

## Topics Covered

### Boxing Overhead
- Performance cost breakdown
- Benchmarks on different scenarios
- Measurements and profiling
- GC pressure analysis
- Real-world impact

### Memory Impact
- Object header overhead
- Size calculations by type
- GC pressure and collection pressure
- Large Object Heap implications
- Memory optimization

### Optimization Strategies
- 10 proven strategies
- When to optimize
- How to measure
- Priority framework
- Real-world examples

## Code Examples

### Example 1: Performance Benchmark

```csharp
// Measure boxing cost
ArrayList list = new ArrayList();
for (int i = 0; i < 1_000_000; i++)
    list.Add(i);  // 50-100ms with boxing

// vs Generic
List<int> genericList = new List<int>();
for (int i = 0; i < 1_000_000; i++)
    genericList.Add(i);  // 2-5ms, no boxing
```

### Example 2: Memory Analysis

```csharp
// 100k integers
int[] unboxed = new int[100_000];    // ~400 KB
ArrayList boxed = new ArrayList();
for (int i = 0; i < 100_000; i++)
    boxed.Add(i);  // ~2.4 MB (6x more!)
```

### Example 3: Optimization

```csharp
// Strategy: Use generics
// Before: ArrayList (boxing)
// After:  List<T> (no boxing)
// Result: 10-20x faster
```

## Key Metrics

| Metric | Value | Notes |
|--------|-------|-------|
| Box cost | 100-200 cycles | Per operation |
| Unbox cost | 50-100 cycles | Per operation |
| Overhead per int | 20 bytes | Additional 6x |
| Overhead per bool | 23 bytes | Additional 24x |
| GC impact | 10x overhead | Multiple collections |

## Practice Exercises

### Exercise 1: Measure Performance

```csharp
// Time ArrayList vs List<int>
// 100k items
// Calculate speedup
```

### Exercise 2: Calculate Memory

```csharp
// How much memory for:
// 1000 boxed ints?
// 10000 boxed doubles?
// 100k boxed bools?
```

### Exercise 3: Optimize Real Code

```csharp
// Given: Slow application
// Find: Boxing issues
// Fix: Apply strategies
// Measure: Improvement
```

## Optimization Priority

### Critical (Do First)
- [ ] Replace ArrayList with List<T>
- [ ] Remove boxing from loops
- [ ] Use StringBuilder for strings

### High (Do Soon)
- [ ] Replace object parameters with overloads
- [ ] Add type checking before unboxing
- [ ] Profile to identify hotspots

### Medium (Do When Measured)
- [ ] Optimize complex scenarios
- [ ] Use structs for data
- [ ] Implement pooling if needed

### Low (Premature Optimization)
- [ ] Most one-time operations
- [ ] Startup code
- [ ] Error paths

## Measurement Tools

### Stopwatch for Performance

```csharp
using System.Diagnostics;

var sw = Stopwatch.StartNew();
// Code to measure
sw.Stop();
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");
```

### GC for Memory

```csharp
using System;

long before = GC.GetTotalMemory(true);
// Code to measure
long after = GC.GetTotalMemory(false);
Console.WriteLine($"Memory: {(after - before) / 1024 / 1024} MB");
```

## Real-World Impact

### Scenario: Data Processing Service

```
Without optimization:
- 1000 req/sec
- Processing time: 50ms avg
- Capacity: 20 concurrent requests

With generics instead of ArrayList:
- 1000 req/sec
- Processing time: 5ms avg (10x faster!)
- Capacity: 200 concurrent requests
- Result: 10x scalability improvement
```

## Related Topics

- [Boxing-Fundamentals](../01-Boxing-Fundamentals/README.md) - What is boxing
- [Unboxing-Type-Safety](../02-Unboxing-Type-Safety/README.md) - How to unbox
- [Best-Practices-Interview](../04-Best-Practices-Interview/README.md) - Best practices

## Next Steps

1. **Read** Boxing-Overhead (understand cost)
2. **Study** Memory-Impact (understand memory)
3. **Learn** Optimization-Strategies (how to fix)
4. **Practice** measuring and optimizing
5. **Move to** Best-Practices-Interview

## Summary

Performance and Memory teach you:
- Boxing has 10-20x cost
- Memory overhead is significant
- GC pressure increases
- Optimization strategies are simple
- Generics solve most issues

**Key Takeaway:** Boxing is expensive—use generics instead.

---

**Ready to optimize?**

- **Overhead:** Learn in [Boxing-Overhead](01-Boxing-Overhead/00-Boxing-Overhead.md)
- **Memory:** Study [Memory-Impact](02-Memory-Impact/00-Memory-Impact.md)
- **Strategies:** Apply [Optimization-Strategies](03-Optimization-Strategies/00-Optimization-Strategies.md)
