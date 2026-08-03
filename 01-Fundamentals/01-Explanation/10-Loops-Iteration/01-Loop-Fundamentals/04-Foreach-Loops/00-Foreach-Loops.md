# Foreach Loops in C#

## Overview

The `foreach` loop is the simplest and most intuitive way to iterate over collections. It iterates through each element without needing an index or explicit iteration control.

## Basic Foreach Syntax

```csharp
foreach (ElementType element in collection)
{
    // Executes for each element in collection
}
```

**Characteristics:**
- Automatically iterates through collection elements
- No manual index management
- Cannot skip elements (without continue)
- Cannot modify iteration count mid-loop
- Works with any IEnumerable

## Simple Foreach Examples

### Iterating Arrays

```csharp
string[] fruits = { "Apple", "Banana", "Cherry" };

foreach (string fruit in fruits)
{
    Console.WriteLine(fruit);
}
// Output: Apple, Banana, Cherry

int[] numbers = { 10, 20, 30, 40, 50 };

foreach (int num in numbers)
{
    Console.WriteLine(num * 2); // 20, 40, 60, 80, 100
}
```

### Iterating Lists

```csharp
var students = new List<string> { "Alice", "Bob", "Charlie", "Diana" };

foreach (var student in students)
{
    Console.WriteLine($"Student: {student}");
}

// Iterating list of objects
var people = new List<Person>
{
    new Person { Name = "Alice", Age = 30 },
    new Person { Name = "Bob", Age = 25 }
};

foreach (var person in people)
{
    Console.WriteLine($"{person.Name} is {person.Age} years old");
}
```

### Iterating Dictionaries

```csharp
var scores = new Dictionary<string, int>
{
    { "Alice", 95 },
    { "Bob", 87 },
    { "Charlie", 92 }
};

// Iterate KeyValuePairs
foreach (var kvp in scores)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

// Iterate only keys
foreach (var name in scores.Keys)
{
    Console.WriteLine(name);
}

// Iterate only values
foreach (var score in scores.Values)
{
    Console.WriteLine(score);
}
```

### Deconstruction in Foreach

```csharp
// Deconstruct KeyValuePair
foreach (var (name, score) in scores)
{
    Console.WriteLine($"{name}: {score}");
}

// Deconstruct custom tuple
var people = new List<(string Name, int Age)>
{
    ("Alice", 30),
    ("Bob", 25),
    ("Charlie", 35)
};

foreach (var (name, age) in people)
{
    Console.WriteLine($"{name} is {age}");
}
```

## Collections Supported by Foreach

### Arrays

```csharp
int[] numbers = { 1, 2, 3 };
foreach (int num in numbers) { } // Works

string[,] matrix = new string[2, 2]; // Multidimensional
foreach (string item in matrix) { } // Works
```

### Generic Collections

```csharp
var list = new List<int> { 1, 2, 3 };
foreach (int item in list) { } // Works

var set = new HashSet<string> { "a", "b" };
foreach (string item in set) { } // Works

var dict = new Dictionary<string, int>();
foreach (var kvp in dict) { } // Works

var queue = new Queue<int>();
foreach (int item in queue) { } // Works

var stack = new Stack<int>();
foreach (int item in stack) { } // Works
```

### IEnumerable Implementations

```csharp
// Any class implementing IEnumerable
public class CustomCollection : IEnumerable<int>
{
    private int[] data = { 1, 2, 3, 4, 5 };
    
    public IEnumerator<int> GetEnumerator()
    {
        foreach (int item in data)
            yield return item;
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

var custom = new CustomCollection();
foreach (int item in custom)
{
    Console.WriteLine(item);
}
```

## Foreach with Index

### Using Select with Index (LINQ)

```csharp
var items = new[] { "apple", "banana", "cherry" };

// LINQ approach: Select with index
foreach (var (item, index) in items.Select((x, i) => (x, i)))
{
    Console.WriteLine($"{index}: {item}");
}
// Output: 0: apple / 1: banana / 2: cherry

// Alternative syntax
foreach (var item in items.Select((value, index) => new { value, index }))
{
    Console.WriteLine($"{item.index}: {item.value}");
}
```

### Custom Extension Method

```csharp
public static class EnumerableExtensions
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        int index = 0;
        foreach (var item in source)
        {
            yield return (item, index);
            index++;
        }
    }
}

// Usage
var items = new[] { "a", "b", "c" };
foreach (var (item, index) in items.WithIndex())
{
    Console.WriteLine($"{index}: {item}");
}
```

### Index Type (C# 8.0+)

```csharp
// New Index pattern (C# 8.0+)
int[] numbers = { 10, 20, 30, 40, 50 };

// With Index from System
using System.Collections.Generic;

foreach (var item in numbers.Select((value, index) => (Index: index, Value: value)))
{
    Console.WriteLine($"[{item.Index}] = {item.Value}");
}
```

## Foreach with Continue and Break

### Continue

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6 };

foreach (int num in numbers)
{
    if (num % 2 == 0)
        continue; // Skip even numbers
    
    Console.WriteLine(num); // Prints: 1, 3, 5
}
```

### Break

```csharp
string[] names = { "Alice", "Bob", "Charlie", "Diana" };

foreach (string name in names)
{
    if (name == "Charlie")
        break; // Exit loop
    
    Console.WriteLine(name); // Prints: Alice, Bob
}
```

## Nested Foreach

```csharp
// Two lists - nested iteration
var departments = new[] { "Sales", "IT", "HR" };
var employees = new[] { "Alice", "Bob", "Charlie" };

foreach (var dept in departments)
{
    Console.WriteLine($"Department: {dept}");
    foreach (var emp in employees)
    {
        Console.WriteLine($"  - {emp}");
    }
}

// 2D Array
int[,] matrix = new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } };

foreach (int item in matrix) // Simple iteration
{
    Console.WriteLine(item); // 1, 2, 3, 4, 5, 6
}

// Manual index-based (if needed)
for (int row = 0; row < matrix.GetLength(0); row++)
{
    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        Console.WriteLine(matrix[row, col]);
    }
}
```

## Foreach Patterns

### Pattern 1: Processing Collections

```csharp
public void ProcessOrders(List<Order> orders)
{
    foreach (var order in orders)
    {
        Console.WriteLine($"Processing order #{order.Id}");
        ProcessOrderItems(order.Items);
    }
}

private void ProcessOrderItems(List<OrderItem> items)
{
    foreach (var item in items)
    {
        Console.WriteLine($"  - {item.Product}: ${item.Price}");
    }
}
```

### Pattern 2: Aggregation

```csharp
public decimal CalculateTotal(List<Order> orders)
{
    decimal total = 0;
    
    foreach (var order in orders)
    {
        total += order.Amount;
    }
    
    return total;
}
```

### Pattern 3: Filtering (with continue)

```csharp
public void DisplayActiveCustomers(List<Customer> customers)
{
    foreach (var customer in customers)
    {
        if (!customer.IsActive)
            continue; // Skip inactive
        
        Console.WriteLine(customer.Name);
    }
}
```

### Pattern 4: Early Termination (with break)

```csharp
public Customer FindCustomer(List<Customer> customers, string id)
{
    foreach (var customer in customers)
    {
        if (customer.Id == id)
        {
            return customer; // Found - exit
        }
    }
    
    return null; // Not found
}
```

### Pattern 5: Transformation

```csharp
public List<string> GetCustomerNames(List<Customer> customers)
{
    var names = new List<string>();
    
    foreach (var customer in customers)
    {
        names.Add(customer.Name);
    }
    
    return names;
}

// Better: Use LINQ
public List<string> GetCustomerNamesLinq(List<Customer> customers)
{
    return customers.Select(c => c.Name).ToList();
}
```

## Cannot Modify Collection During Iteration

```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };

// BAD: InvalidOperationException
foreach (var item in list)
{
    if (item == 3)
        list.Remove(item); // WRONG - modifying during iteration
}

// GOOD: Create copy or use separate collection
foreach (var item in list.ToList()) // Iterate copy
{
    if (item == 3)
        list.Remove(item); // Safe - modifying original
}

// GOOD: Use RemoveAll or Where
list.RemoveAll(x => x == 3);

var filtered = list.Where(x => x != 3).ToList();
```

## Foreach vs For vs LINQ

### Use Foreach When:
- Simply iterating collection
- Don't need index
- Readable is priority

```csharp
foreach (var item in items)
{
    ProcessItem(item);
}
```

### Use For When:
- Need index access
- Need to skip elements
- Need to iterate in reverse

```csharp
for (int i = 0; i < items.Count; i++)
{
    ProcessItem(items[i], i);
}
```

### Use LINQ When:
- Complex transformations
- Filtering and projecting
- Chaining operations

```csharp
var result = items
    .Where(x => x.IsActive)
    .Select(x => x.Name)
    .OrderBy(x => x)
    .ToList();
```

## Performance Considerations

### Allocation

```csharp
// EFFICIENT: No extra allocation
foreach (var item in array) { }

// INEFFICIENT: Creates enumerator allocation
List<int> list = new();
foreach (var item in list) { } // Allocates enumerator

// EFFICIENT: Value type struct enumerator (C# 7.0+)
foreach (var item in list) { } // May be optimized away
```

### Break Early

```csharp
// EFFICIENT: Stops early
foreach (var item in items)
{
    if (item == target)
    {
        return true; // Early exit
    }
}

// INEFFICIENT: Processes all even after finding
var found = items.FirstOrDefault(x => x == target);
```

## Best Practices

1. **Use Foreach for Simple Iteration**
   ```csharp
   foreach (var item in items)
   {
       Console.WriteLine(item);
   }
   ```

2. **Use Clear Variable Names**
   ```csharp
   foreach (var customer in customers)
   {
       // Clear what 'customer' represents
   }
   ```

3. **Don't Modify Collection During Iteration**
   ```csharp
   foreach (var item in items.ToList())
   {
       items.Remove(item); // Iterate copy
   }
   ```

4. **Consider LINQ for Complex Operations**
   ```csharp
   var result = items.Where(x => x > 5).Select(x => x * 2).ToList();
   ```

5. **Use Deconstruction When Applicable**
   ```csharp
   foreach (var (key, value) in dict)
   {
       // Clear extraction of key/value
   }
   ```

## Summary

- **Foreach**: Simplest for collection iteration
- **No index access**: Use for loop or LINQ if needed
- **Cannot modify collection**: Iterate copy if necessary
- **Works with any IEnumerable**: Maximum flexibility
- **Most readable**: Usually preferred for simple iteration
