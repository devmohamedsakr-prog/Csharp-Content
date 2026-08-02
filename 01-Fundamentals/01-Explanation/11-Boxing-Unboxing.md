# Boxing and Unboxing

## Overview
Boxing converts value types to reference types (heap). Unboxing reverses this. Understanding boxing prevents performance issues and runtime errors.

## Boxing

### Converting Value to Object
```csharp
// Boxing: value type → reference type
int number = 42;
object boxed = number; // Boxes to heap

// Implicit boxing
object obj = 10; // Automatically boxed

// Boxing collections
var list = new ArrayList();
list.Add(5); // int boxed to object
list.Add("Hello"); // string already reference type
list.Add(3.14); // double boxed to object

// Boxing with interfaces
int value = 100;
IComparable comparable = value; // Boxed
```

### Memory Impact
```csharp
// Unboxed value on stack
int x = 5;
// Memory: Stack contains 5 directly

// Boxed value
object boxedX = x;
// Memory: Stack contains reference to heap
// Memory: Heap contains int wrapper object
```

## Unboxing

### Converting Object Back to Value
```csharp
// Boxing first
int original = 42;
object boxed = original;

// Unboxing: must match exact type
int unboxed = (int)boxed; // OK - matches original type

// Unboxing type mismatch
double? mismatch = (double?)boxed; // Throws InvalidCastException
```

### Unboxing Rules
```csharp
// Rule 1: Must unbox to original type
int intValue = 10;
object boxedInt = intValue;

int restored = (int)boxedInt; // OK
// short shortValue = (short)boxedInt; // InvalidCastException!

// Rule 2: Can unbox to nullable type
object num = 5;
int? nullableInt = (int?)num; // OK
double? nullableDouble = (double?)num; // InvalidCastException

// Rule 3: Null is valid
object nullObj = null;
int? nullable = (int?)nullObj; // OK - null
// int value = (int)nullObj; // NullReferenceException!
```

## Performance Impact

### Boxing Overhead
```csharp
using System.Diagnostics;

public class BoxingPerformance
{
    public static void Main()
    {
        const int iterations = 1_000_000;
        var sw = Stopwatch.StartNew();
        
        // No boxing
        int sum = 0;
        for (int i = 0; i < iterations; i++)
        {
            sum += i;
        }
        sw.Stop();
        Console.WriteLine($"No boxing: {sw.ElapsedMilliseconds}ms");
        
        // Boxing
        sw.Restart();
        object objSum = 0;
        for (int i = 0; i < iterations; i++)
        {
            objSum = (int)objSum + i; // Box, unbox, box
        }
        sw.Stop();
        Console.WriteLine($"Boxing: {sw.ElapsedMilliseconds}ms");
        
        // Boxing is significantly slower!
    }
}
```

## Avoiding Boxing

### Use Generics
```csharp
// Bad: Boxing
ArrayList list = new ArrayList();
for (int i = 0; i < 100; i++)
{
    list.Add(i); // Boxes every int
}

foreach (object item in list)
{
    int value = (int)item; // Unboxes
}

// Good: Generics avoid boxing
List<int> genericList = new List<int>();
for (int i = 0; i < 100; i++)
{
    genericList.Add(i); // No boxing
}

foreach (int item in genericList)
{
    // No unboxing needed
}
```

### Generic Collections
```csharp
// Bad: Non-generic causes boxing
Hashtable hash = new Hashtable();
hash[1] = "One";
hash[2] = "Two";

foreach (DictionaryEntry entry in hash)
{
    int key = (int)entry.Key; // Unboxed
    string value = (string)entry.Value;
}

// Good: Generic Dictionary avoids boxing
Dictionary<int, string> dict = new Dictionary<int, string>();
dict[1] = "One";
dict[2] = "Two";

foreach (var kvp in dict)
{
    int key = kvp.Key; // No unboxing
    string value = kvp.Value;
}
```

## Boxing with Interfaces

### Calling Interface Methods
```csharp
public interface IComparable
{
    int CompareTo(object obj);
}

// Value type implementing interface
struct Point : IComparable
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public int CompareTo(object obj)
    {
        if (!(obj is Point other))
            return -1;
        return this.X.CompareTo(other.X);
    }
}

public class BoxingWithInterfaces
{
    public static void Main()
    {
        Point p1 = new Point { X = 1, Y = 2 };
        
        // Calling method on value type - no boxing
        int result = p1.CompareTo(new Point { X = 2, Y = 3 });
        
        // Casting to interface - boxes
        IComparable comparable = p1; // Boxes!
        result = comparable.CompareTo(new Point { X = 2, Y = 3 }); // Already boxed
    }
}
```

## Boxing with Strings

### String Concatenation
```csharp
// Boxing in string concatenation
int number = 42;
string result = "Number: " + number; // Boxes int, converts to string

// Better: Use string interpolation (no boxing)
string better = $"Number: {number}";

// Or StringBuilder (no boxing in loops)
var sb = new StringBuilder();
for (int i = 0; i < 100; i++)
{
    sb.Append($"Value: {i}"); // Still boxes but more efficient
}
```

## Nullable Value Types and Boxing

### Boxing Nullables
```csharp
// Nullable boxing
int? nullable = 42;
object boxed = nullable; // Boxes to int, not Nullable<int>

int unboxed = (int)boxed; // OK
int? restored = (int?)boxed; // OK - unboxes to nullable

// Nullable with null
int? nullableNull = null;
object boxedNull = nullableNull; // Boxes as null
int? restoredNull = (int?)boxedNull; // null
// int value = (int)boxedNull; // NullReferenceException!
```

## Best Practices

1. **Prefer Generics Over Collections.IEnumerable**
```csharp
// Bad: Generic collection with non-generic interface
IEnumerable items = new List<int> { 1, 2, 3 };
foreach (object item in items)
{
    int value = (int)item; // Unboxes
}

// Good: Use generic all the way
IEnumerable<int> genericItems = new List<int> { 1, 2, 3 };
foreach (int item in genericItems)
{
    // No boxing
}
```

2. **Avoid Boxing in Loops**
```csharp
// Bad: Boxing in loop
ArrayList list = new ArrayList();
for (int i = 0; i < 1000; i++)
{
    list.Add(i); // Boxes 1000 times
}

// Good: Use generic collection
List<int> genericList = new List<int>();
for (int i = 0; i < 1000; i++)
{
    genericList.Add(i); // No boxing
}
```

3. **Use Appropriate Data Structures**
```csharp
// Bad: Old non-generic collections cause boxing
Hashtable hash = new Hashtable();
Stack stack = new Stack();
Queue queue = new Queue();

// Good: Generic versions
Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
Stack<T> genericStack = new Stack<T>();
Queue<T> genericQueue = new Queue<T>();
```

## Common Mistakes

1. **Boxing Due to IEnumerable**
```csharp
// Bad: foreach over non-generic collection boxes
ArrayList list = new ArrayList { 1, 2, 3 };
foreach (object item in list)
{
    int value = (int)item; // Unboxes on every iteration
}

// Good
List<int> list = new List<int> { 1, 2, 3 };
foreach (int item in list)
{
    // No boxing
}
```

2. **Unboxing to Wrong Type**
```csharp
// Bad: Runtime error
object boxedInt = 42;
// long value = (long)boxedInt; // InvalidCastException

// Good: Unbox to original type
int value = (int)boxedInt;
long converted = (long)value; // Then convert
```

3. **Hidden Boxing in LINQ**
```csharp
// Bad: Boxing in LINQ with non-generic collection
var list = new ArrayList { 1, 2, 3 };
var query = list.OfType<int>(); // Unboxes on iteration

// Good: Generic collection, no boxing
var genericList = new List<int> { 1, 2, 3 };
var query = genericList.Where(x => x > 1); // No boxing
```

## Quick Summary
- Boxing: value type → object (allocates heap memory)
- Unboxing: object → value type (copies from heap)
- Must unbox to exact original type
- Generics avoid boxing overhead
- Use List<T> instead of ArrayList
- Use Dictionary<K,V> instead of Hashtable
- Nullable types can be boxed as null
- Performance impact significant in loops
- Modern .NET favors generics, reducing boxing

## Resources
- Boxing and Unboxing (C# documentation)
- Value Types vs Reference Types
- Performance Best Practices
- Generic Collections
