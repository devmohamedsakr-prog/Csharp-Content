# Common Boxing Mistakes

## Overview

This section covers 10 common mistakes developers make with boxing and unboxing, along with fixes.

## Mistake 1: Using Non-Generic Collections

The most common mistake: continuing to use ArrayList, Hashtable, Stack, Queue.

```csharp
// ✗ WRONG: Non-generic collection
ArrayList list = new ArrayList();
list.Add(42);  // Boxes

// ✓ CORRECT: Generic collection
List<int> list = new List<int>();
list.Add(42);  // No boxing
```

**Impact:** 10-20x performance penalty, extra GC pressure

## Mistake 2: Unboxing Without Type Check

Assuming you know the type before unboxing causes runtime exceptions.

```csharp
// ✗ WRONG: Assume type
object obj = GetValue();
int value = (int)obj;  // May throw InvalidCastException!

// ✓ CORRECT: Check type first
if (obj is int intVal)
{
    int value = intVal;  // Safe
}

// ✓ CORRECT: Use pattern matching
int? result = obj as int?;
if (result.HasValue)
{
    int value = result.Value;
}
```

**Impact:** Runtime exception, application crash

## Mistake 3: Unboxing Null to Non-Nullable

Boxing null produces a null reference that cannot unbox to non-nullable types.

```csharp
// ✗ WRONG: Unbox null to non-nullable
int? nullableNull = null;
object boxedNull = nullableNull;

int value = (int)boxedNull;  // NullReferenceException!

// ✓ CORRECT: Unbox to nullable
int? value = (int?)boxedNull;  // OK - null
if (value.HasValue)
{
    int actualValue = value.Value;
}
```

**Impact:** Runtime exception

## Mistake 4: Boxing in Hot Loops

Tight loops with boxing create massive performance problems.

```csharp
// ✗ WRONG: Boxing every iteration
for (int i = 0; i < 1_000_000; i++)
{
    object boxed = i;  // Boxes 1M times
    Process(boxed);    // Unboxes 1M times
}
// Result: 50-100ms (very slow!)

// ✓ CORRECT: Direct access
for (int i = 0; i < 1_000_000; i++)
{
    Process(i);  // No boxing
}
// Result: 2-3ms (30-50x faster!)
```

**Impact:** Dramatic performance penalty

## Mistake 5: Object Parameters for Value Types

Using object parameters forces boxing at call sites.

```csharp
// ✗ WRONG: Object parameter
public void Display(object value)
{
    Console.WriteLine(value);
}

Display(42);  // Boxes int

// ✓ CORRECT: Type-specific overloads
public void Display(int value) => Console.WriteLine(value);
public void Display(double value) => Console.WriteLine(value);

Display(42);     // No boxing
Display(3.14);   // No boxing
```

**Impact:** Unnecessary boxing at every call

## Mistake 6: Wrong Type Unboxing

Boxing one type and unboxing as another causes InvalidCastException.

```csharp
// ✗ WRONG: Type mismatch
object boxedInt = 42;  // Boxes as int
long value = (long)boxedInt;  // InvalidCastException!

// ✓ CORRECT: Match types
int correctValue = (int)boxedInt;  // OK
long convertedValue = (long)correctValue;  // Then convert
```

**Impact:** Runtime exception

## Mistake 7: LINQ with Non-Generic Collections

LINQ requires unboxing when used with non-generic collections.

```csharp
// ✗ WRONG: Non-generic ArrayList with LINQ
ArrayList list = new ArrayList { 1, 2, 3 };
var query = list.OfType<int>()  // Unboxes on iteration
    .Where(x => x > 1);

// ✓ CORRECT: Use generic collection
List<int> genericList = new List<int> { 1, 2, 3 };
var query = genericList
    .Where(x => x > 1);  // No unboxing
```

**Impact:** Unnecessary unboxing overhead

## Mistake 8: String Concatenation with Boxing

String concatenation boxes value types for ToString conversion.

```csharp
// ✗ WRONG: Concatenation in loop (boxes each iteration)
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += "Value: " + i;  // Boxes i
}
// Result: slow and creates many temporary strings

// ✓ CORRECT: Use StringBuilder
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append("Value: ");
    sb.Append(i);  // More efficient
}
string result = sb.ToString();
```

**Impact:** Performance penalty, memory waste

## Mistake 9: Variadic Object Parameters

Variadic object parameters force boxing of value types.

```csharp
// ✗ WRONG: params object[] boxes value types
public void Log(params object[] values)
{
    foreach (var val in values)
        Console.WriteLine(val);
}

Log(42, 3.14, "text");  // 42 and 3.14 boxed

// ✓ CORRECT: Use generic overloads or specific parameters
public void Log(int value) => Console.WriteLine(value);
public void Log(double value) => Console.WriteLine(value);
public void Log(string value) => Console.WriteLine(value);

Log(42);     // No boxing
Log(3.14);   // No boxing
Log("text"); // No boxing
```

**Impact:** Unnecessary boxing for variadic calls

## Mistake 10: Not Profiling Performance

Assuming boxing isn't a problem without measurement.

```csharp
// ✗ WRONG: Assume ArrayList is fine
ArrayList list = new ArrayList();
for (int i = 0; i < 1_000_000; i++)
    list.Add(i);
// Slow, but developer didn't notice

// ✓ CORRECT: Measure before and after
var sw = Stopwatch.StartNew();
ArrayList list = new ArrayList();
for (int i = 0; i < 1_000_000; i++)
    list.Add(i);
sw.Stop();
Console.WriteLine($"ArrayList: {sw.ElapsedMilliseconds}ms");

sw.Restart();
List<int> genericList = new List<int>();
for (int i = 0; i < 1_000_000; i++)
    genericList.Add(i);
sw.Stop();
Console.WriteLine($"List<int>: {sw.ElapsedMilliseconds}ms");
// Now you see the difference!
```

**Impact:** Missed optimization opportunities

## Error Patterns and Fixes

### Pattern 1: Type Mismatch in Collections

```csharp
// Problem
ArrayList list = new ArrayList();
list.Add(42);
list.Add("text");

foreach (object item in list)
{
    int value = (int)item;  // Throws on "text"!
}

// Fix
foreach (object item in list)
{
    if (item is int intVal)
        HandleInt(intVal);
    else if (item is string strVal)
        HandleString(strVal);
}
```

### Pattern 2: Null in Collections

```csharp
// Problem
ArrayList list = new ArrayList();
list.Add(null);
list.Add((int?)42);

foreach (object item in list)
{
    int value = (int)item;  // Throws on null!
}

// Fix
foreach (object item in list)
{
    int? nullable = item as int?;
    if (nullable.HasValue)
        HandleInt(nullable.Value);
    else if (item == null)
        HandleNull();
}
```

### Pattern 3: Performance Regression

```csharp
// Problem
ArrayList results = new ArrayList();
for (int i = 0; i < 100_000; i++)
{
    results.Add(Expensive Calculation(i));  // Boxes, slow
}

// Fix
List<Result> results = new List<Result>();
for (int i = 0; i < 100_000; i++)
{
    results.Add(ExpensiveCalculation(i));  // No boxing
}
```

## Debugging Checklist

If boxing-related issues suspected:

- [ ] Check for non-generic collections in code
- [ ] Look for loops with boxing operations
- [ ] Verify type checks before unboxing
- [ ] Check for null handling in unboxing
- [ ] Profile performance with Stopwatch
- [ ] Use memory profiler to measure allocations
- [ ] Look for InvalidCastException in logs
- [ ] Check for NullReferenceException on unboxing

## Quick Fix Priority

1. **Critical** - Replace ArrayList, Hashtable, Stack, Queue with generics
2. **High** - Remove boxing from loops
3. **High** - Add type checks before unboxing
4. **Medium** - Replace object parameters with overloads
5. **Medium** - Use StringBuilder for string ops
6. **Low** - Optimize nullable handling
7. **Low** - Use struct for data types

## Summary

| Mistake | Fix | Impact |
|---------|-----|--------|
| ArrayList | List<T> | 10-20x |
| Loop boxing | Direct access | 10-50x |
| No type check | Pattern matching | Correctness |
| Wrong type | Match types | Correctness |
| Object params | Overloads | 2-3x |
| Null unboxing | Use nullable | Correctness |
| LINQ on ArrayList | Use generics | 5-10x |
| String concat | StringBuilder | 5-50x |

## Real-World Impact

A single ArrayList in a hot loop can:
- Reduce throughput by 90%
- Double memory usage
- Increase GC pause time by 10x

Fix it: Replace with generic collection and see immediate improvements.

## Next Steps

- Review best practices in [Best-Practices](../01-Best-Practices/00-Best-Practices.md)
- Prepare for interviews in [Interview-Questions](../03-Interview-Questions/README.md)
- Study optimization strategies in [Optimization-Strategies](../../03-Performance-Memory/03-Optimization-Strategies/00-Optimization-Strategies.md)
