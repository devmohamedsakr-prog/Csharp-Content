# Boxing and Unboxing Overhead

## Overview

Boxing and unboxing have measurable performance costs. Understanding these costs helps you write efficient code and avoid performance bottlenecks.

## The Cost of Boxing

Boxing requires several operations:

1. **Allocate memory** on the heap
2. **Copy value** from stack to heap
3. **Create wrapper object** with type information
4. **Return reference** to caller

Each of these takes CPU cycles.

### Simple Boxing Cost

```csharp
int value = 42;
object boxed = value;

// What happens internally:
// 1. Allocate new object on heap (~24 bytes for int wrapper)
// 2. Copy 42 to heap location
// 3. Create type metadata pointer
// 4. Return reference to caller
// Total: ~100-200 CPU cycles per box
```

## The Cost of Unboxing

Unboxing is simpler than boxing but still has cost:

```csharp
object boxed = 42;
int unboxed = (int)boxed;

// What happens internally:
// 1. Check type matches
// 2. Copy value from heap to stack
// Total: ~50-100 CPU cycles per unbox
```

## Performance Benchmarks

### Benchmark 1: Simple Boxing Loop

```csharp
using System.Diagnostics;

// Setup
const int iterations = 1_000_000;
var sw = Stopwatch.StartNew();

// Test 1: No boxing (baseline)
int sum = 0;
for (int i = 0; i < iterations; i++)
{
    sum += i;
}
sw.Stop();
Console.WriteLine($"No boxing: {sw.ElapsedMilliseconds}ms");
// Typical result: 2-3ms

// Test 2: Boxing in loop
sw.Restart();
object objSum = 0;
for (int i = 0; i < iterations; i++)
{
    objSum = (int)objSum + i;  // Box, unbox, box each iteration
}
sw.Stop();
Console.WriteLine($"Boxing: {sw.ElapsedMilliseconds}ms");
// Typical result: 50-150ms (20-50x slower!)
```

### Benchmark 2: Collection Storage

```csharp
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

const int itemCount = 100_000;

// ArrayList (with boxing)
var sw = Stopwatch.StartNew();
ArrayList nonGeneric = new ArrayList();
for (int i = 0; i < itemCount; i++)
    nonGeneric.Add(i);  // Boxes

int sum = 0;
foreach (object item in nonGeneric)
    sum += (int)item;  // Unboxes
sw.Stop();
Console.WriteLine($"ArrayList: {sw.ElapsedMilliseconds}ms");
// Typical: 50-100ms

// List<int> (no boxing)
sw.Restart();
List<int> generic = new List<int>();
for (int i = 0; i < itemCount; i++)
    generic.Add(i);  // No boxing

sum = 0;
foreach (int item in generic)
    sum += item;  // No unboxing
sw.Stop();
Console.WriteLine($"List<int>: {sw.ElapsedMilliseconds}ms");
// Typical: 2-5ms (10-20x faster!)
```

### Benchmark 3: Interface Calls

```csharp
using System.Diagnostics;

int value = 42;
const int iterations = 1_000_000;

// Direct call (no boxing)
var sw = Stopwatch.StartNew();
int result = 0;
for (int i = 0; i < iterations; i++)
{
    result = value.CompareTo(i);  // Direct struct method
}
sw.Stop();
Console.WriteLine($"Direct struct call: {sw.ElapsedMilliseconds}ms");
// Typical: 2-3ms

// Interface call (with boxing)
sw.Restart();
IComparable comparable = value;  // Boxes
for (int i = 0; i < iterations; i++)
{
    result = comparable.CompareTo(i);
}
sw.Stop();
Console.WriteLine($"Interface call (boxed): {sw.ElapsedMilliseconds}ms");
// Typical: 5-10ms (2-3x slower due to box overhead)
```

## Memory Impact

### Memory Overhead Per Boxed Value

Every boxed value takes additional memory:

```
Unboxed int on stack: 4 bytes

Boxed int on heap:
├─ Object header: 16 bytes (reference count, type pointer, etc.)
├─ Actual value: 4 bytes
└─ Padding: 4 bytes (alignment)
Total: 24 bytes per boxed int
```

### Memory Example

```csharp
// 1000 unboxed ints
int[] unboxed = new int[1000];
// Memory: 4,000 bytes (plus array overhead)

// 1000 boxed ints
ArrayList boxed = new ArrayList();
for (int i = 0; i < 1000; i++)
    boxed.Add(i);
// Memory: 1000 * 24 bytes + 8000 bytes (references) = ~32,000 bytes
// 8x more memory!
```

## GC Pressure

Boxing creates pressure on the garbage collector:

```csharp
// High boxing in loop = high GC pressure
public void BadPerformance()
{
    ArrayList list = new ArrayList();
    for (int i = 0; i < 1_000_000; i++)
    {
        list.Add(i);  // Boxes each iteration
    }
    // 1 million objects created
    // GC must clean up all boxed integers
    // Significant GC pressure
}

// Better: Use generic collection
public void GoodPerformance()
{
    List<int> list = new List<int>();
    for (int i = 0; i < 1_000_000; i++)
    {
        list.Add(i);  // No boxing
    }
    // No objects created
    // No GC pressure
}
```

## Performance Degradation Scenarios

### Scenario 1: Boxing in Tight Loop

```csharp
// BAD: Each iteration boxes and unboxes
int total = 0;
for (int i = 0; i < 1_000_000; i++)
{
    object boxed = i;
    total += (int)boxed;  // Box, unbox
}
// Result: ~50-100ms

// GOOD: Direct access
int total = 0;
for (int i = 0; i < 1_000_000; i++)
{
    total += i;
}
// Result: ~2-3ms (30-50x faster!)
```

### Scenario 2: Boxing in Collections

```csharp
// BAD: Non-generic collection
ArrayList list = new ArrayList();
for (int i = 0; i < 100_000; i++)
    list.Add(i);  // 100k boxes

int sum = 0;
foreach (object item in list)
    sum += (int)item;  // 100k unboxes
// Result: ~50-80ms

// GOOD: Generic collection
List<int> list = new List<int>();
for (int i = 0; i < 100_000; i++)
    list.Add(i);  // No boxes

int sum = 0;
foreach (int item in list)
    sum += item;  // No unboxes
// Result: ~2-5ms (10-20x faster!)
```

### Scenario 3: Boxing with Strings

```csharp
// BAD: Boxing in string concatenation
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += "Value: " + i;  // Boxes i for ToString
}
// Result: ~100-500ms

// GOOD: Direct ToString
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append("Value: ");
    sb.Append(i);  // Append directly, no boxing
}
string result = sb.ToString();
// Result: ~5-10ms (50-100x faster!)
```

## Measuring Boxing Costs

### Profiling Example

```csharp
using System.Diagnostics;

public class BoxingProfiler
{
    public static void Main()
    {
        // Warm up
        TestBoxing(100);
        
        // Actual measurement
        Console.WriteLine("Profiling boxing overhead...");
        
        for (int size = 1000; size <= 100_000; size *= 10)
        {
            var result = TestBoxing(size);
            Console.WriteLine(
                $"Items: {size:D6} | Boxing: {result.boxingMs}ms | " +
                $"Generic: {result.genericMs}ms | " +
                $"Ratio: {result.ratio:F1}x"
            );
        }
    }
    
    private static (long boxingMs, long genericMs, double ratio) 
        TestBoxing(int itemCount)
    {
        var sw = Stopwatch.StartNew();
        
        // Boxing version
        ArrayList list = new ArrayList();
        for (int i = 0; i < itemCount; i++)
            list.Add(i);
        int sum = 0;
        foreach (object item in list)
            sum += (int)item;
        
        sw.Stop();
        long boxingMs = sw.ElapsedMilliseconds;
        
        sw.Restart();
        
        // Generic version
        List<int> genericList = new List<int>();
        for (int i = 0; i < itemCount; i++)
            genericList.Add(i);
        sum = 0;
        foreach (int item in genericList)
            sum += item;
        
        sw.Stop();
        long genericMs = sw.ElapsedMilliseconds;
        
        return (boxingMs, genericMs, (double)boxingMs / genericMs);
    }
}

// Output:
// Items:   1000 | Boxing: 1ms  | Generic: 0ms | Ratio: Infinity
// Items:  10000 | Boxing: 2ms  | Generic: 0ms | Ratio: Infinity
// Items: 100000 | Boxing: 45ms | Generic: 2ms | Ratio: 22.5x
```

## Where Boxing is Expensive

### Expensive Operations

```csharp
// 1. Collections
ArrayList list = new ArrayList();
list.Add(42);  // Boxing

// 2. Interface calls
IComparable comp = 42;  // Boxing
comp.CompareTo(50);

// 3. Variadic object parameters
void LogValues(params object[] values)
{
    // values containing value types boxes them
}
LogValues(42, 3.14, true);  // All boxed

// 4. Conversion to string
string s = "Value: " + 42;  // Boxes for ToString

// 5. Dictionary with object values
Dictionary<string, object> dict = new Dictionary<string, object>();
dict["number"] = 42;  // Boxes
```

### Less Expensive Operations

```csharp
// 1. Generic collections
List<int> list = new List<int>();
list.Add(42);  // No boxing

// 2. Direct method calls
int i = 42;
i.CompareTo(50);  // No boxing

// 3. String interpolation
string s = $"Value: {42}";  // Still boxes, but optimized

// 4. Generic methods
void ProcessValue<T>(T value) { }
ProcessValue(42);  // No boxing (generic)
```

## Performance Best Practices

### Practice 1: Profile Before Optimizing

```csharp
// Don't guess - measure!
if (performanceIsSlow)
{
    // Use profiler to identify boxing hotspots
    // Then optimize based on data
}
```

### Practice 2: Use Generics

```csharp
// BAD: Non-generic collection
ArrayList list = new ArrayList();

// GOOD: Generic collection
List<T> list = new List<T>();
```

### Practice 3: Avoid Boxing in Loops

```csharp
// BAD: Boxing in loop
for (int i = 0; i < 1_000_000; i++)
{
    object boxed = i;
    ProcessBoxed(boxed);
}

// GOOD: Direct access
for (int i = 0; i < 1_000_000; i++)
{
    ProcessDirect(i);
}
```

### Practice 4: Cache Collections

```csharp
// BAD: Creates new collection each time
foreach (object item in GetList())
    ProcessItem((int)item);

// GOOD: Reuse collection
var list = GetList();
foreach (object item in list)
    ProcessItem((int)item);
```

## Real-World Impact

### Example: Data Processing

```csharp
// Processing 1 million records
// With boxing: 200-500ms
// With generics: 10-50ms
// Difference: 10-50x slower!

// In a web service handling 1000 requests/second:
// With boxing: 200-500 seconds/sec wasted
// With generics: 10-50 seconds/sec
// Difference: Can affect scalability
```

## Summary

| Operation | Time | Notes |
|-----------|------|-------|
| No boxing | 1x | Baseline |
| Boxing | 10-50x slower | Allocation + copy |
| Unboxing | 2-5x slower | Check + copy |
| ArrayList (1k items) | 10-20x slower | Boxing + GC |
| List<T> (1k items) | 1x | No overhead |

## Key Takeaways

- **Boxing has measurable cost** (10-50x slower per operation)
- **GC pressure increases** with boxing
- **Memory usage increases** significantly
- **Generics eliminate boxing** entirely
- **Profile actual code** before optimizing

## Next Steps

- Learn memory impact in [Memory-Impact](../02-Memory-Impact/00-Memory-Impact.md)
- Study optimization strategies in [Optimization-Strategies](../03-Optimization-Strategies/00-Optimization-Strategies.md)
- Review best practices in [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
