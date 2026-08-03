# Memory Impact of Boxing

## Overview

Boxing creates significant memory overhead. Understanding memory usage helps optimize applications, especially those with tight memory constraints or high throughput.

## Object Header Overhead

Every boxed value inherits the object header:

```
Object header structure:
┌─────────────────────────────────┐
│ Sync Block Index (8 bytes)      │
│ Type Handle (8 bytes)           │
├─────────────────────────────────┤
│ Actual Value (variable)         │
├─────────────────────────────────┤
│ Padding (alignment)             │
└─────────────────────────────────┘

Total minimum: 16 bytes overhead + value size
```

## Memory Usage by Type

### Primitive Types

```csharp
// Unboxed vs boxed memory

// int
unboxed:  4 bytes
boxed:    16 bytes (header) + 4 bytes (value) + 4 bytes (padding) = 24 bytes
overhead: 20 bytes (6x)

// double
unboxed:  8 bytes
boxed:    16 bytes (header) + 8 bytes (value) = 24 bytes
overhead: 16 bytes (3x)

// bool
unboxed:  1 byte
boxed:    16 bytes (header) + 1 byte (value) + 7 bytes (padding) = 24 bytes
overhead: 23 bytes (24x!)

// byte
unboxed:  1 byte
boxed:    16 bytes (header) + 1 byte (value) + 7 bytes (padding) = 24 bytes
overhead: 23 bytes (24x!)
```

### Memory Calculation Table

| Type | Unboxed | Boxed | Overhead | Ratio |
|------|---------|-------|----------|-------|
| byte | 1 | 24 | 23 | 24x |
| short | 2 | 24 | 22 | 12x |
| int | 4 | 24 | 20 | 6x |
| long | 8 | 24 | 16 | 3x |
| double | 8 | 24 | 16 | 3x |
| bool | 1 | 24 | 23 | 24x |
| struct (4 bytes) | 4 | 24+ | 20+ | 6x+ |

## Practical Memory Examples

### Example 1: ArrayList of Integers

```csharp
// Scenario: Store 1 million integers

// Using int[]
int[] array = new int[1_000_000];
// Memory: ~4 MB
// + array object header: ~56 bytes
// Total: ~4 MB

// Using ArrayList
ArrayList list = new ArrayList(1_000_000);
for (int i = 0; i < 1_000_000; i++)
    list.Add(i);  // Boxes each int
// Memory breakdown:
// - ArrayList object header: 56 bytes
// - Internal object[] (1M references): 8 MB
// - 1M boxed int objects: 24 MB each = 24 GB theoretical
// + Object headers for each
// Total: ~32+ MB
// vs int[]: ~4 MB
// Difference: 8x more memory!
```

### Example 2: Dictionary Lookup

```csharp
// Scenario: Cache values in dictionary

// Good: Generic dictionary
Dictionary<int, object> dict = new Dictionary<int, object>(10_000);
for (int i = 0; i < 10_000; i++)
    dict[i] = i.ToString();  // String, not boxed
// Memory: ~10,000 * (8 bytes key + 24 bytes ref + string) 
//         ~10,000 * (32 + 50) = ~800 KB

// Bad: Object values containing boxed ints
Dictionary<string, object> cache = new Dictionary<string, object>(10_000);
for (int i = 0; i < 10_000; i++)
    cache[$"item_{i}"] = i;  // Boxing
// Memory: ~10,000 * (string key + 24 bytes boxed int)
//         ~10,000 * (30 + 24) = ~540 KB
// Similar, but with more boxing overhead
```

### Example 3: Jagged Arrays

```csharp
// Scenario: Store matrix of integers

// Using jagged int[][]
int[][] matrix = new int[1000][];
for (int i = 0; i < 1000; i++)
    matrix[i] = new int[1000];
// Memory: ~4 MB + array overhead

// Using ArrayList of ArrayLists
ArrayList matrix2 = new ArrayList();
for (int i = 0; i < 1000; i++)
{
    ArrayList row = new ArrayList();
    for (int j = 0; j < 1000; j++)
        row.Add(42);  // Boxes each int
    matrix2.Add(row);
}
// Memory: 1M boxes * 24 bytes = 24 MB + reference overhead
// Difference: 6x more memory!
```

## Memory Allocation Patterns

### Stack vs Heap Allocation

```csharp
// Stack allocation (no boxing)
public void StackAllocation()
{
    int x = 42;
    double y = 3.14;
    bool z = true;
    // All on stack, automatic cleanup
}

// Heap allocation (boxing)
public void HeapAllocation()
{
    object x = 42;      // On heap
    object y = 3.14;    // On heap
    object z = true;    // On heap
    // GC must clean up
}
```

## Garbage Collection Impact

### GC Pressure from Boxing

```csharp
// High boxing = high GC pressure
public void HighGCPressure()
{
    ArrayList list = new ArrayList();
    for (int i = 0; i < 1_000_000; i++)
    {
        list.Add(i);  // Creates 1M objects
    }
    // GC sees 1M objects to track
    // GC pause time increases
    // Heap fragmentation increases
}

// Low GC pressure (generics)
public void LowGCPressure()
{
    List<int> list = new List<int>();
    for (int i = 0; i < 1_000_000; i++)
    {
        list.Add(i);  // No objects created
    }
    // GC barely affected
    // No fragmentation
    // No pause time increase
}
```

### Generation and GC Performance

```csharp
// Boxing creates short-lived Gen0 objects
// Frequent Gen0 collections impact performance

Boxed objects lifecycle:
- Created in Gen0 (boxing)
- Survive few collections or discarded
- Never reach Gen1/Gen2

Result: Frequent Gen0 garbage collection
```

## Memory Profiling

### Measuring Memory Usage

```csharp
using System;
using System.Diagnostics;

public class MemoryProfiler
{
    public static void Main()
    {
        // Get baseline memory
        GC.Collect();
        GC.WaitForPendingFinalizers();
        long baseline = GC.GetTotalMemory(true);
        
        // Test 1: ArrayList with boxing
        var list = new ArrayList(1_000_000);
        for (int i = 0; i < 1_000_000; i++)
            list.Add(i);
        
        long after1 = GC.GetTotalMemory(false);
        Console.WriteLine($"ArrayList: {(after1 - baseline) / 1024 / 1024} MB");
        
        // Clear and test 2: List<int>
        list = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
        baseline = GC.GetTotalMemory(true);
        
        var genericList = new List<int>(1_000_000);
        for (int i = 0; i < 1_000_000; i++)
            genericList.Add(i);
        
        long after2 = GC.GetTotalMemory(false);
        Console.WriteLine($"List<int>: {(after2 - baseline) / 1024 / 1024} MB");
    }
}

// Typical output:
// ArrayList: 38 MB
// List<int>: 4 MB
// Difference: 9.5x
```

## Large Object Heap (LOH)

### When Objects Go to LOH

```csharp
// Objects >= 85KB go to Large Object Heap (LOH)
// LOH is not compacted

// Scenario: Storing large number of boxed values
ArrayList list = new ArrayList(1_000_000);
for (int i = 0; i < 1_000_000; i++)
    list.Add(i);

// 1M boxed ints = 24 MB total
// Might trigger LOH if not managed carefully
// LOH fragmentation can impact performance
```

## Memory Efficiency Comparison

### Scenario: Processing Data Stream

```csharp
// Data: 10,000 measurements of multiple types
// Store: {timestamp, int value, double data}

// Approach 1: Boxed objects in ArrayList
public class BoxedApproach
{
    public void Store()
    {
        ArrayList list = new ArrayList();
        for (int i = 0; i < 10_000; i++)
        {
            var obj = new object[] {
                DateTime.Now,  // datetime (not boxed)
                i,             // int (boxed - 24 bytes)
                i * 1.5        // double (boxed - 24 bytes)
            };
            list.Add(obj);
        }
        // Memory: 10k * (8 bytes ref + (24 + 24 + 48)) = 640 KB + overhead
    }
}

// Approach 2: Struct collection (no boxing)
public struct Measurement
{
    public DateTime Timestamp;
    public int Value;
    public double Data;
}

public class StructApproach
{
    public void Store()
    {
        List<Measurement> list = new List<Measurement>();
        for (int i = 0; i < 10_000; i++)
        {
            list.Add(new Measurement
            {
                Timestamp = DateTime.Now,
                Value = i,
                Data = i * 1.5
            });
        }
        // Memory: 10k * (8 + 4 + 8) = 200 KB
        // No boxing overhead
    }
}

// Difference: 3x less memory with struct approach
```

## Memory Allocation Peaks

### Allocation During Boxing Loop

```csharp
// Each iteration allocates new object
// Creates temporary memory spikes

public void AllocationPeak()
{
    ArrayList list = new ArrayList();
    for (int i = 0; i < 1_000_000; i++)
    {
        int value = i;
        object boxed = value;  // Allocate on heap
        list.Add(boxed);       // Store reference
    }
    
    // Peak memory during loop: 1M objects
    // GC collects Gen0 during iteration
    // Multiple GC pauses possible
}

// With generics: No allocation peak
public void NoPeak()
{
    List<int> list = new List<int>();
    for (int i = 0; i < 1_000_000; i++)
    {
        list.Add(i);  // No allocation
    }
}
```

## Optimization Checklist

- [ ] Using generics instead of non-generic collections?
- [ ] Avoiding boxing in loops?
- [ ] Using value types (structs) for data?
- [ ] Avoiding boxing in recursive calls?
- [ ] Profiled actual memory usage?
- [ ] Considering LOH impact?
- [ ] Minimizing temporary object creation?

## Memory Impact Summary

| Scenario | Boxed | Unboxed | Ratio | Notes |
|----------|-------|---------|-------|-------|
| 1k ints in ArrayList | 24 KB | 4 KB | 6x | Per int: 24 bytes vs 4 bytes |
| 1M ints in ArrayList | 24 MB | 4 MB | 6x | GC pressure significant |
| Mixed types | Varies | N/A | 3-10x | Depends on types |
| Collections | Worst | Best | 10x+ | Use generics |

## Key Takeaways

- **Boxed values have 24-byte minimum** (plus header)
- **Small types (byte, bool) worst** (24x overhead)
- **Larger types better** (3-6x overhead)
- **Generics eliminate overhead** entirely
- **GC pressure significant** with boxing
- **Profile real applications** to measure

## Next Steps

- Learn optimization strategies in [Optimization-Strategies](../03-Optimization-Strategies/00-Optimization-Strategies.md)
- Review best practices in [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
- Study common mistakes in [Common-Mistakes](../../04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md)
