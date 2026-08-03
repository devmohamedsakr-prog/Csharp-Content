# Boxing Best Practices

## Overview

This section covers 10 essential best practices for avoiding boxing issues and writing efficient code.

## 1. Prefer Generics Over Non-Generic Collections

Use generic collections exclusively in modern code.

```csharp
// BAD: Non-generic collection
ArrayList list = new ArrayList();
list.Add(42);  // Boxing

// GOOD: Generic collection
List<int> list = new List<int>();
list.Add(42);  // No boxing
```

## 2. Avoid Boxing in Loops

Be especially careful about boxing in hot loops.

```csharp
// BAD: Boxing every iteration
for (int i = 0; i < 1_000_000; i++)
{
    object boxed = i;  // Boxes 1M times
    Process(boxed);
}

// GOOD: Direct access
for (int i = 0; i < 1_000_000; i++)
{
    Process(i);  // No boxing
}
```

## 3. Use Type-Safe Method Overloads

Provide overloads for common types instead of using object parameters.

```csharp
// BAD: Single object parameter
public void Log(object value)
{
    Console.WriteLine(value);
}
Log(42);  // Boxes

// GOOD: Typed overloads
public void Log(int value) => Console.WriteLine(value);
public void Log(string value) => Console.WriteLine(value);
public void Log(double value) => Console.WriteLine(value);

Log(42);     // No boxing
Log("text"); // No boxing
```

## 4. Check Type Before Unboxing

Always verify type before unboxing to prevent runtime exceptions.

```csharp
// BAD: Assume type
int value = (int)unknownObject;  // May throw!

// GOOD: Check type first
if (unknownObject is int intVal)
{
    int value = intVal;  // Safe
}

// GOOD: Use pattern matching
int? result = unknownObject as int?;
if (result.HasValue)
{
    int value = result.Value;
}
```

## 5. Use Generic Methods

Leverage generics to avoid boxing with method parameters.

```csharp
// BAD: Object parameter
public T Process<T>(object value) where T : struct
{
    return (T)value;  // Unboxes
}

// GOOD: Generic parameter
public T Process<T>(T value) where T : struct
{
    return value;  // No unboxing
}
```

## 6. Minimize Object[] Usage

Avoid object arrays for value types.

```csharp
// BAD: object[] with value types
object[] mixed = new object[1000];
for (int i = 0; i < 1000; i++)
    mixed[i] = i;  // Boxes each

// GOOD: Type-specific array or generic list
int[] typed = new int[1000];
for (int i = 0; i < 1000; i++)
    typed[i] = i;  // No boxing

// GOOD: Generic list for mixed types
List<int> list = new List<int>();
for (int i = 0; i < 1000; i++)
    list.Add(i);
```

## 7. Handle Null Correctly

When working with nullable types, handle null explicitly.

```csharp
// BAD: Assume value is non-null
int value = (int)unknownObject;  // May throw NullReferenceException

// GOOD: Check and handle null
int? nullable = unknownObject as int?;
if (nullable.HasValue)
{
    int value = nullable.Value;
}
else
{
    // Handle null case
}
```

## 8. Optimize String Operations

Use StringBuilder instead of concatenation to avoid boxing.

```csharp
// BAD: Concatenation (boxes during ToString)
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += "Value: " + i;  // Boxes each iteration
}

// GOOD: StringBuilder
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append("Value: ");
    sb.Append(i);  // No boxing
}
string result = sb.ToString();
```

## 9. Use Struct for Data Containers

For lightweight data structures, use struct instead of class to avoid heap allocation.

```csharp
// GOOD: Lightweight struct
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

List<Point> points = new List<Point>();
points.Add(new Point { X = 10, Y = 20 });  // No heap allocation for Point

// vs Class (heap allocation)
public class PointClass
{
    public int X { get; set; }
    public int Y { get; set; }
}

List<PointClass> classPoints = new List<PointClass>();
classPoints.Add(new PointClass { X = 10, Y = 20 });  // Heap allocation
```

## 10. Profile and Measure

Always measure performance before and after changes.

```csharp
using System.Diagnostics;

public void OptimizeWithMeasurement()
{
    var sw = Stopwatch.StartNew();
    
    // Measure current approach
    ArrayList list = new ArrayList();
    for (int i = 0; i < 1_000_000; i++)
        list.Add(i);
    
    sw.Stop();
    Console.WriteLine($"Current: {sw.ElapsedMilliseconds}ms");
    
    // Measure optimized approach
    sw.Restart();
    List<int> genericList = new List<int>();
    for (int i = 0; i < 1_000_000; i++)
        genericList.Add(i);
    
    sw.Stop();
    Console.WriteLine($"Optimized: {sw.ElapsedMilliseconds}ms");
}
```

## Code Review Checklist

When reviewing code, look for:

- [ ] Non-generic collections (ArrayList, Hashtable, Stack, Queue)?
- [ ] Boxing in loops?
- [ ] Object parameters where specific types possible?
- [ ] Unboxing without type check?
- [ ] Null values unboxed to non-nullable?
- [ ] String concatenation in loops?
- [ ] Collections of object[] for value types?
- [ ] Interface calls on value types (non-generic)?

## Real-World Examples

### Example 1: Data Processing

```csharp
// BEFORE: Boxing overhead
public class DataProcessor
{
    private ArrayList _data = new ArrayList();
    
    public void AddData(int value)
    {
        _data.Add(value);  // Boxes
    }
    
    public int SumData()
    {
        int sum = 0;
        foreach (object item in _data)
        {
            sum += (int)item;  // Unboxes
        }
        return sum;
    }
}

// AFTER: No boxing
public class DataProcessor
{
    private List<int> _data = new List<int>();
    
    public void AddData(int value)
    {
        _data.Add(value);  // No boxing
    }
    
    public int SumData()
    {
        int sum = 0;
        foreach (int item in _data)
        {
            sum += item;  // Direct access
        }
        return sum;
    }
}
```

### Example 2: Type-Safe Logging

```csharp
// BEFORE: Object parameter causes boxing
public class Logger
{
    public void Log(object value)
    {
        Console.WriteLine($"[LOG] {value}");
    }
}

logger.Log(42);      // Boxes
logger.Log(3.14);    // Boxes
logger.Log("text");  // No boxing

// AFTER: Typed overloads
public class Logger
{
    public void Log(int value) => 
        Console.WriteLine($"[LOG] Int: {value}");
    
    public void Log(double value) => 
        Console.WriteLine($"[LOG] Double: {value}");
    
    public void Log(string value) => 
        Console.WriteLine($"[LOG] String: {value}");
}

logger.Log(42);      // No boxing
logger.Log(3.14);    // No boxing
logger.Log("text");  // No boxing
```

## Performance Impact

Following these practices typically results in:

- **10-20x performance improvement** for collection-heavy code
- **5-10x performance improvement** for loop-heavy code
- **Significantly reduced GC pressure**
- **Better memory efficiency** (3-10x improvement)

## Migration Guide

When updating legacy code:

1. **Identify** non-generic collections
2. **Replace** with generic equivalents
3. **Update** method signatures to use generics
4. **Remove** unnecessary casts and type checks
5. **Test** for correctness
6. **Measure** performance improvement

## Summary Table

| Practice | Impact | Effort | Priority |
|----------|--------|--------|----------|
| Use generics | 10-20x | Easy | Critical |
| Avoid loop boxing | 10-50x | Easy | Critical |
| Type overloads | 2-3x | Medium | High |
| Type checking | Correctness | Easy | High |
| Generic methods | 5-10x | Medium | Medium |
| StringBuilder | 5-50x | Easy | Medium |
| Structs | 2-5x | Medium | Low |
| Profiling | Identifies issues | Medium | High |

## Next Steps

- Study common mistakes in [Common-Mistakes](../02-Common-Mistakes/00-Common-Mistakes.md)
- Prepare for interviews in [Interview-Questions](../03-Interview-Questions/README.md)
- Review optimization strategies in [Optimization-Strategies](../../03-Performance-Memory/03-Optimization-Strategies/00-Optimization-Strategies.md)
