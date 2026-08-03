# Optimization Strategies

## Overview

This section covers practical strategies to eliminate or minimize boxing overhead in your code.

## Strategy 1: Use Generics

The most important optimization: **replace non-generic with generic**.

### Collections

```csharp
// BAD: Non-generic collection (boxing)
ArrayList list = new ArrayList();
list.Add(42);          // Boxes
list.Add(3.14);        // Boxes
foreach (object item in list)
{
    // Must unbox
    if (item is int i) Console.WriteLine(i);
}

// GOOD: Generic collection (no boxing)
List<int> intList = new List<int>();
intList.Add(42);       // No boxing
foreach (int item in intList)
{
    Console.WriteLine(item);  // Direct access
}
```

### Hashtable → Dictionary

```csharp
// BAD: Hashtable (boxing)
Hashtable hash = new Hashtable();
hash[1] = "One";  // Key and value might be boxed
foreach (DictionaryEntry entry in hash)
{
    int key = (int)entry.Key;  // Unboxes
}

// GOOD: Dictionary (no boxing for keys)
Dictionary<int, string> dict = new Dictionary<int, string>();
dict[1] = "One";  // No boxing
foreach (var kvp in dict)
{
    int key = kvp.Key;  // Direct access
}
```

### Stack and Queue

```csharp
// BAD: Non-generic Stack (boxing)
Stack nonGeneric = new Stack();
nonGeneric.Push(42);  // Boxes

// GOOD: Generic Stack (no boxing)
Stack<int> stack = new Stack<int>();
stack.Push(42);  // No boxing
```

## Strategy 2: Avoid Boxing in Loops

### Identify Boxing in Loops

```csharp
// BAD: Boxing every iteration
for (int i = 0; i < 1_000_000; i++)
{
    object boxed = i;  // Boxes each iteration
    Process(boxed);
}

// GOOD: Direct access
for (int i = 0; i < 1_000_000; i++)
{
    Process(i);  // No boxing
}
```

### Collection Iteration

```csharp
// BAD: Boxing on add + unboxing on iterate
ArrayList list = new ArrayList();
for (int i = 0; i < 100_000; i++)
    list.Add(i);  // Boxes

foreach (object item in list)
    Process((int)item);  // Unboxes

// GOOD: No boxing at all
List<int> list = new List<int>();
for (int i = 0; i < 100_000; i++)
    list.Add(i);

foreach (int item in list)
    Process(item);
```

## Strategy 3: Use Value Types (Structs)

For lightweight data, use structs instead of classes to avoid heap allocation.

### Struct for Data Container

```csharp
// Struct: No heap allocation
public struct Measurement
{
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    public int Count { get; set; }
}

// Usage: No boxing
List<Measurement> measurements = new List<Measurement>();
measurements.Add(new Measurement 
{ 
    Timestamp = DateTime.Now,
    Value = 42.5,
    Count = 100
});

// vs Class approach (heap allocation)
public class MeasurementClass
{
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    public int Count { get; set; }
}

// More memory, heap allocation, GC pressure
List<MeasurementClass> classData = new List<MeasurementClass>();
```

## Strategy 4: Generic Methods

Use generic type parameters to avoid boxing.

### Generic Method Example

```csharp
// BAD: Object parameter (causes boxing)
public void Process(object value)
{
    int intValue = (int)value;  // Unboxes
}

// Call with boxing
Process(42);  // Boxes int

// GOOD: Generic method (no boxing)
public void Process<T>(T value) where T : struct
{
    if (value is int intValue)
        HandleInt(intValue);
}

// Call without boxing
Process(42);  // No boxing
```

### Generic Collections

```csharp
// BAD: Object collection
public void StoreData(ArrayList list)
{
    foreach (object item in list)
    {
        // Must handle as object, likely unbox
    }
}

// GOOD: Generic method
public void StoreData<T>(List<T> list)
{
    foreach (T item in list)
    {
        // Direct type access
    }
}
```

## Strategy 5: Avoid Object Parameters

Minimize use of object parameters, which often require boxing.

### Parameter Design

```csharp
// BAD: Object parameter
public void LogValue(object value)
{
    Console.WriteLine($"Value: {value}");
}

LogValue(42);  // Boxes int

// GOOD: Overloads for common types
public void LogValue(int value)
{
    Console.WriteLine($"Value: {value}");
}

public void LogValue(double value)
{
    Console.WriteLine($"Value: {value}");
}

LogValue(42);    // No boxing - calls int overload
LogValue(3.14);  // No boxing - calls double overload
```

## Strategy 6: Use Interfaces Carefully

Non-generic interfaces cause boxing for value types.

### Generic Interfaces

```csharp
// BAD: Non-generic interface (causes boxing)
IComparable comparable = 42;  // Boxes int
int result = comparable.CompareTo(50);

// GOOD: Generic interface (no boxing)
IComparable<int> genericComparable = 42;  // No boxing - doesn't work like this
int i = 42;
int result = i.CompareTo(50);  // Direct call, no boxing
```

## Strategy 7: LINQ Optimization

Use LINQ efficiently to minimize boxing.

### LINQ with Non-Generic Source

```csharp
// BAD: Non-generic source with OfType
ArrayList list = new ArrayList { 1, 2, 3 };
var query = list.OfType<int>()  // Unboxes during iteration
    .Where(x => x > 1);

// GOOD: Generic source
List<int> genericList = new List<int> { 1, 2, 3 };
var query = genericList
    .Where(x => x > 1);  // No unboxing
```

## Strategy 8: String Operations

Optimize string building to minimize boxing.

### String Concatenation

```csharp
// BAD: Concatenation in loop (boxes on each iteration)
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += "Value: " + i;  // Boxes i for ToString
}

// GOOD: StringBuilder (no boxing)
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append("Value: ");
    sb.Append(i);  // Append directly
}
string result = sb.ToString();
```

### String Interpolation

```csharp
// Still boxes internally, but optimized
string s = $"Value: {42}";  // Better than concatenation

// But still slower than direct append
StringBuilder sb = new StringBuilder();
sb.Append("Value: ");
sb.Append(42);  // Fastest
```

## Strategy 9: Delegate and Lambda Optimization

Minimize boxing in delegates.

### Generic Delegates

```csharp
// BAD: Non-generic delegate (might box)
public void ProcessValue(Action<object> handler)
{
    handler(42);  // Boxes int
}

// GOOD: Generic delegate (no boxing)
public void ProcessValue<T>(Action<T> handler)
{
    handler(42);  // No boxing if T is int
}
```

## Strategy 10: Profile and Measure

Always measure before and after optimization.

### Profiling Strategy

```csharp
using System.Diagnostics;

public class OptimizationProfile
{
    public static void Main()
    {
        // Measure baseline
        var baseline = MeasurePerformance(BadApproach, "Bad");
        var optimized = MeasurePerformance(GoodApproach, "Good");
        
        Console.WriteLine($"Improvement: {baseline / optimized:F1}x faster");
    }
    
    private static long MeasurePerformance(Action approach, string name)
    {
        var sw = Stopwatch.StartNew();
        approach();
        sw.Stop();
        Console.WriteLine($"{name}: {sw.ElapsedMilliseconds}ms");
        return sw.ElapsedMilliseconds;
    }
    
    private static void BadApproach()
    {
        ArrayList list = new ArrayList();
        for (int i = 0; i < 1_000_000; i++)
            list.Add(i);
    }
    
    private static void GoodApproach()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 1_000_000; i++)
            list.Add(i);
    }
}
```

## Optimization Checklist

During code review, check for:

- [ ] Non-generic collections used?
- [ ] ArrayList, Hashtable, Stack, Queue used?
- [ ] Boxing in loops?
- [ ] Object parameters where specific types possible?
- [ ] Non-generic interfaces used?
- [ ] Unboxing in hot loops?
- [ ] String concatenation instead of StringBuilder?
- [ ] Object[] used for value types?

## Real-World Optimization Example

### Before: Logging System

```csharp
public class BadLogger
{
    private ArrayList _logs = new ArrayList();
    
    public void LogValue(object value)
    {
        _logs.Add(value);  // Boxes value types
    }
    
    public void ProcessLogs()
    {
        foreach (object log in _logs)
        {
            if (log is int i)
                Console.WriteLine($"Int: {i}");  // Unboxes
        }
    }
}

// Usage: Boxes and unboxes every call
logger.LogValue(42);
logger.LogValue(3.14);
```

### After: Optimized Logging

```csharp
public class GoodLogger
{
    private List<int> _intLogs = new List<int>();
    private List<double> _doubleLogs = new List<double>();
    
    public void LogValue(int value)
    {
        _intLogs.Add(value);  // No boxing
    }
    
    public void LogValue(double value)
    {
        _doubleLogs.Add(value);  // No boxing
    }
    
    public void ProcessLogs()
    {
        foreach (int log in _intLogs)
            Console.WriteLine($"Int: {log}");  // Direct access
        
        foreach (double log in _doubleLogs)
            Console.WriteLine($"Double: {log}");  // Direct access
    }
}

// Usage: No boxing or unboxing
logger.LogValue(42);      // Direct call, no boxing
logger.LogValue(3.14);    // Direct call, no boxing
```

## Performance Improvements Summary

| Optimization | Improvement | Effort |
|--------------|-------------|--------|
| ArrayList → List<T> | 10-20x | Easy |
| Remove loop boxing | 10-50x | Easy |
| Generic methods | 5-10x | Medium |
| StringBuilder | 5-50x | Easy |
| Struct for data | 2-5x | Medium |
| Generic interfaces | 2-3x | Hard |

## When Optimization Matters

**High Priority:** (Optimize first)
- Tight loops
- Collection operations
- High-throughput scenarios
- Real-time applications

**Medium Priority:** (Optimize if measured as slow)
- Regular loops
- Moderate collections
- Background operations
- Non-critical paths

**Low Priority:** (Premature optimization?)
- One-time operations
- Startup code
- Error handling paths
- Unlikely scenarios

## Key Takeaways

1. **Generics are the primary solution**
2. **Measure before optimizing**
3. **Focus on loops and collections**
4. **Consider memory impact**
5. **Use profiler to identify hotspots**

## Next Steps

- Review best practices in [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
- Study common mistakes in [Common-Mistakes](../../04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md)
- Prepare for interviews in [Interview-Questions](../../04-Best-Practices-Interview/03-Interview-Questions/README.md)
