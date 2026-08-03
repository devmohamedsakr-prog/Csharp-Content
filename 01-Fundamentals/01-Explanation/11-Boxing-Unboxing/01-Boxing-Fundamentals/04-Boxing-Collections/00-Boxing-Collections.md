# Boxing in Collections

## Overview

Collections provide fertile ground for boxing. Non-generic collections automatically box value types, while generic collections avoid boxing entirely.

## Non-Generic Collections (Old .NET)

### ArrayList

The canonical example of boxing in collections:

```csharp
// ArrayList stores object references
ArrayList list = new ArrayList();

// Adding value types causes boxing
list.Add(42);          // int boxed
list.Add(3.14);        // double boxed
list.Add(true);        // bool boxed
list.Add(new Point()); // struct boxed

// list.Add("text");   // string NOT boxed (already reference type)

// Retrieving requires casting (which unboxes)
foreach (object item in list)
{
    if (item is int intValue)
    {
        int unboxed = intValue;  // Unboxes
    }
}
```

### Internal Representation

```csharp
// ArrayList internally:
public class ArrayList
{
    private object[] _items;  // Array of object references
    
    public void Add(object value)
    {
        // If value is value type, box it first
        _items[count++] = value;
    }
}

// When you add int 42:
// 1. Create boxed int object on heap
// 2. Store reference in _items array
// 3. GC tracks the boxed object
```

### Hashtable

```csharp
// Hashtable boxes both keys and values
Hashtable hash = new Hashtable();

// Adding with value type key
hash[1] = "One";       // key 1 boxed, value not (string)
hash[2] = 200;         // key 2 boxed, value 200 boxed
hash["three"] = 300;   // key not boxed, value 300 boxed

// Iteration shows boxing
foreach (DictionaryEntry entry in hash)
{
    object key = entry.Key;      // May be boxed int
    object value = entry.Value;  // May be boxed int
}
```

### Stack (Non-Generic)

```csharp
// Stack boxes value types
Stack stack = new Stack();
stack.Push(42);     // int boxed
stack.Push(100);    // int boxed
stack.Push("text"); // string NOT boxed

// Popping requires casting
object popped = stack.Pop();  // Returns boxed int or string
if (popped is int intValue)
{
    int unboxed = intValue;  // Unboxes
}
```

### Queue (Non-Generic)

```csharp
// Queue boxes value types
Queue queue = new Queue();
queue.Enqueue(10);  // int boxed
queue.Enqueue(20);  // int boxed
queue.Enqueue(30);  // int boxed

// Dequeuing requires casting
while (queue.Count > 0)
{
    object item = queue.Dequeue();  // Boxed int
    int value = (int)item;           // Unboxes
}
```

## Generic Collections (Modern .NET)

### List<T>

```csharp
// List<T> avoids boxing
List<int> list = new List<int>();
list.Add(42);   // No boxing
list.Add(100);  // No boxing

// No casting needed
foreach (int item in list)
{
    // item is int directly, no unboxing
    Console.WriteLine(item);
}

// Performance is significantly better
```

### Dictionary<K,V>

```csharp
// Dictionary<K,V> avoids boxing
Dictionary<int, string> dict = new Dictionary<int, string>();
dict[1] = "One";   // key not boxed
dict[2] = "Two";   // key not boxed

// Iteration is type-safe
foreach (var kvp in dict)
{
    int key = kvp.Key;      // No unboxing
    string value = kvp.Value; // Already typed
}
```

### Stack<T>

```csharp
// Stack<T> avoids boxing
Stack<int> stack = new Stack<int>();
stack.Push(42);   // No boxing
stack.Push(100);  // No boxing

// Pop is type-safe
int popped = stack.Pop();  // No unboxing needed
```

### Queue<T>

```csharp
// Queue<T> avoids boxing
Queue<int> queue = new Queue<int>();
queue.Enqueue(10);  // No boxing
queue.Enqueue(20);  // No boxing

// Dequeue is type-safe
int dequeued = queue.Dequeue();  // No unboxing
```

## Comparison: Non-Generic vs Generic

### ArrayList vs List<int>

```csharp
// Non-generic: Boxing overhead
ArrayList arrayList = new ArrayList();
for (int i = 0; i < 1000; i++)
    arrayList.Add(i);  // Boxes 1000 times

// Generic: No boxing
List<int> list = new List<int>();
for (int i = 0; i < 1000; i++)
    list.Add(i);  // No boxing

// Iteration with boxing
foreach (object item in arrayList)
    int value = (int)item;  // Unboxes 1000 times

// Iteration without boxing
foreach (int item in list)
    int value = item;  // Direct access
```

### Memory Impact

```csharp
// Non-generic storage
ArrayList list = new ArrayList();
for (int i = 0; i < 100; i++)
    list.Add(i);

// Memory: 100 boxed int objects on heap
// + ArrayList array of references
// Significant GC pressure

// Generic storage
List<int> genericList = new List<int>();
for (int i = 0; i < 100; i++)
    genericList.Add(i);

// Memory: Single array with 100 int values
// Minimal GC pressure
// Much better cache locality
```

## Performance Benchmarks

### Benchmark: ArrayList vs List<int>

```csharp
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

// ArrayList (with boxing)
var sw = Stopwatch.StartNew();
ArrayList nonGeneric = new ArrayList();
for (int i = 0; i < 100_000; i++)
    nonGeneric.Add(i);

int sum = 0;
foreach (object item in nonGeneric)
    sum += (int)item;  // Unboxes

sw.Stop();
Console.WriteLine($"ArrayList: {sw.ElapsedMilliseconds}ms, Sum: {sum}");

// List<int> (no boxing)
sw.Restart();
List<int> generic = new List<int>();
for (int i = 0; i < 100_000; i++)
    generic.Add(i);

sum = 0;
foreach (int item in generic)
    sum += item;  // No unboxing

sw.Stop();
Console.WriteLine($"List<int>: {sw.ElapsedMilliseconds}ms, Sum: {sum}");

// Result: List<int> is typically 5-10x faster!
```

## Boxing with Heterogeneous Data

### When You Need Mixed Types

```csharp
// Need to store different types
var mixedData = new ArrayList();
mixedData.Add(42);        // int boxed
mixedData.Add(3.14);      // double boxed
mixedData.Add("text");    // string not boxed
mixedData.Add(true);      // bool boxed

// Process mixed data
foreach (object item in mixedData)
{
    if (item is int intVal)
        Console.WriteLine($"Int: {intVal}");
    else if (item is double doubleVal)
        Console.WriteLine($"Double: {doubleVal}");
    else if (item is string strVal)
        Console.WriteLine($"String: {strVal}");
}
```

### Better: Use object[] or List<object>

```csharp
// Explicit object array
object[] mixed = new object[4];
mixed[0] = 42;        // int boxed
mixed[1] = 3.14;      // double boxed
mixed[2] = "text";    // string not boxed
mixed[3] = true;      // bool boxed

// Type checking on retrieval
for (int i = 0; i < mixed.Length; i++)
{
    switch (mixed[i])
    {
        case int intVal:
            Console.WriteLine($"Int: {intVal}");
            break;
        // ... other cases
    }
}
```

## Boxing in LINQ

### LINQ with Non-Generic Source

```csharp
// ArrayList source (causes boxing)
ArrayList numbers = new ArrayList { 1, 2, 3, 4, 5 };

// Cast unboxes on each iteration
var query = numbers.Cast<int>()
    .Where(x => x > 2)
    .Select(x => x * 2);

// Each iteration involves unboxing
```

### LINQ with Generic Source

```csharp
// List<int> source (no boxing)
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

// No unboxing needed
var query = numbers
    .Where(x => x > 2)
    .Select(x => x * 2);

// Each iteration is direct access
```

## Best Practices

### Pattern 1: Prefer Generics

```csharp
// BAD: Non-generic collection
ArrayList list = new ArrayList();
list.Add(1);

// GOOD: Generic collection
List<int> list = new List<int>();
list.Add(1);
```

### Pattern 2: Use Correct Generic Type

```csharp
// BAD: object collection (allows boxing)
List<object> objects = new List<object>();
objects.Add(42);  // Boxes int

// GOOD: Specific type
List<int> ints = new List<int>();
ints.Add(42);  // No boxing
```

### Pattern 3: Cast Once

```csharp
// BAD: Cast in loop
foreach (object item in list)
{
    int value = (int)item;  // Unboxes each iteration
}

// GOOD: Use generic collection
foreach (int item in genericList)
{
    // Direct access, no unboxing
}
```

## Legacy Code Considerations

### When You Must Use Non-Generic

Sometimes you need to work with legacy non-generic collections:

```csharp
public void LegacyMethod(ArrayList list)
{
    // Must work with ArrayList (boxing happens)
    foreach (object item in list)
    {
        if (item is int intValue)
        {
            ProcessInt(intValue);  // Unboxes
        }
    }
}

// Convert if possible
public void ModernMethod(List<int> list)
{
    // No boxing
    foreach (int value in list)
    {
        ProcessInt(value);
    }
}
```

## Avoiding Boxing Checklist

- [ ] Using List<T> instead of ArrayList?
- [ ] Using Dictionary<K,V> instead of Hashtable?
- [ ] Using Stack<T> instead of Stack?
- [ ] Using Queue<T> instead of Queue?
- [ ] No object[] for value types?
- [ ] Using typed variables instead of object?
- [ ] Avoiding foreach on non-generic collections?

## Summary

| Collection | Boxing | Use When |
|-----------|--------|----------|
| ArrayList | Yes | Legacy code only |
| List<T> | No | Normal collections |
| Hashtable | Yes | Legacy code only |
| Dictionary<K,V> | No | Key-value pairs |
| Stack | Yes | Legacy code only |
| Stack<T> | No | Stack data structure |
| Queue | Yes | Legacy code only |
| Queue<T> | No | Queue data structure |

## Next Steps

- Learn unboxing in [Unboxing-Rules](../../02-Unboxing-Type-Safety/01-Unboxing-Rules/00-Unboxing-Rules.md)
- Study performance impact in [Boxing-Overhead](../../03-Performance-Memory/01-Boxing-Overhead/00-Boxing-Overhead.md)
- Review best practices in [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
