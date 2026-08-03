# Boxing Fundamentals

## Overview

This section covers the foundational concepts of boxing in C#. Master these basics before moving to unboxing and optimization.

## Learning Path

### Beginner
1. **[Boxing-Basics](01-Boxing-Basics/00-Boxing-Basics.md)** - Start here
   - What is boxing?
   - How boxing works
   - Implicit vs explicit boxing
   - Memory layout

2. **[Value-Reference-Types](02-Value-Reference-Types/00-Value-Reference-Types.md)** - Type system
   - Value types vs reference types
   - Stack vs heap allocation
   - Struct vs class
   - Why boxing exists

3. **[Boxing-Conversions](03-Boxing-Conversions/00-Boxing-Conversions.md)** - Converting values
   - Implicit boxing
   - Boxing different types
   - Boxing in collections
   - Boxing with interfaces

4. **[Boxing-Collections](04-Boxing-Collections/00-Boxing-Collections.md)** - Collections
   - Non-generic collections (ArrayList, Hashtable)
   - Generic alternatives
   - Boxing performance in collections
   - Best practices

### Intermediate
- Review all four files
- Understand boxing mechanism completely
- Ready for Unboxing-Type-Safety section

### Advanced
- Study unboxing in detail
- Learn optimization strategies
- Prepare for interviews

## Quick Reference

### Boxing Overview

```csharp
// Value type (stack)
int value = 42;

// Boxing to object reference
object boxed = value;  // Allocates on heap

// Memory:
// Stack: [reference]
// Heap:  [object wrapper with 42]
```

### When Boxing Happens

1. Assigning to object reference
2. Passing to object parameter
3. Adding to non-generic collection
4. Implementing non-generic interface
5. String concatenation/conversion

## Topics Covered

### Boxing Basics
- Boxing mechanism
- Memory impact
- Performance cost
- When boxing occurs

### Type System
- Value types (int, double, bool, struct, enum)
- Reference types (class, interface, delegate)
- Stack allocation
- Heap allocation

### Boxing Conversions
- Implicit boxing
- Explicit casting
- Boxing expressions
- Boxing different types (primitives, structs, enums)

### Collections
- ArrayList (non-generic, boxes)
- Hashtable (non-generic, boxes)
- Stack and Queue (non-generic)
- Generic alternatives (List<T>, Dictionary<K,V>)
- Performance comparison

## Code Examples

### Example 1: Simple Boxing

```csharp
int number = 42;
object boxed = number;  // Boxing

// What happens:
// 1. Create new object on heap
// 2. Copy value to heap
// 3. Return reference
```

### Example 2: Collections

```csharp
// Non-generic: Boxing
ArrayList list = new ArrayList();
list.Add(42);  // Boxes

// Generic: No boxing
List<int> genericList = new List<int>();
genericList.Add(42);  // No boxing
```

### Example 3: Performance

```csharp
// Benchmark
// ArrayList: 50-100ms (1M items with boxing)
// List<int>: 2-5ms (1M items, no boxing)
// Difference: 10-20x
```

## Key Concepts

1. **Boxing** = value type → object reference
2. **Occurs** when assigning to object
3. **Performance cost** of ~10-20x per operation
4. **Memory overhead** of 24+ bytes per boxed value
5. **Generics** eliminate boxing entirely

## Practice Exercises

### Exercise 1: Identify Boxing
```csharp
// Which lines cause boxing?
object o1 = 42;        // Boxing
object o2 = "text";    // No boxing
int[] arr = { 1, 2 };  // No boxing
ArrayList list = new ArrayList();
list.Add(3);           // Boxing
```

### Exercise 2: Compare Collections
```csharp
// Time both approaches
ArrayList nonGeneric = new ArrayList();
List<int> generic = new List<int>();
// Add 100k items to each
// Measure and compare
```

### Exercise 3: Memory Impact
```csharp
// Calculate memory usage
// 1000 boxed ints = ? bytes
// 1000 ints in int[] = ? bytes
// Difference = ?
```

## Performance Benchmarks

| Scenario | Time | Notes |
|----------|------|-------|
| Direct int | 1x | Baseline |
| Boxed int | 20-50x | Allocation + copy |
| ArrayList (100k) | 20ms | Boxing overhead |
| List<int> (100k) | 1ms | No boxing |

## Best Practices Summary

1. **Use generics** for collections
2. **Avoid boxing in loops**
3. **Understand the cost** (10-20x slower)
4. **Profile real code** before optimizing
5. **Know the mechanism** (understand, don't guess)

## Common Mistakes

- Using ArrayList instead of List<T>
- Boxing value types in tight loops
- Not measuring performance
- Assuming boxing is free
- Using object parameters unnecessarily

## Related Topics

- [Unboxing-Type-Safety](../02-Unboxing-Type-Safety/README.md) - Reverse process
- [Performance-Memory](../03-Performance-Memory/README.md) - Performance details
- [Best-Practices-Interview](../04-Best-Practices-Interview/README.md) - Best practices

## Next Steps

1. **Read** each file in order
2. **Understand** the boxing mechanism
3. **Practice** identifying boxing in code
4. **Measure** boxing performance
5. **Move to** Unboxing-Type-Safety section

## Summary

Boxing fundamentals teach you:
- What boxing is and how it works
- When boxing happens automatically
- Performance impact of boxing
- Why generics were invented
- Foundation for unboxing concepts

**Key Takeaway:** Understand boxing to appreciate why generics exist.

---

**Ready to dive deeper?**

- **Basics:** Start with [Boxing-Basics](01-Boxing-Basics/00-Boxing-Basics.md)
- **Types:** Learn in [Value-Reference-Types](02-Value-Reference-Types/00-Value-Reference-Types.md)
- **Practical:** See [Boxing-Collections](04-Boxing-Collections/00-Boxing-Collections.md)
