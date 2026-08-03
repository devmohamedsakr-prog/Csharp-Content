# Loop Best Practices

## 1. Choose the Right Loop Type

### For Loops
- When you need index access
- When you know iteration count
- When you need to skip elements

```csharp
// GOOD: For loop with index
for (int i = 0; i < items.Count; i++)
{
    Console.WriteLine($"{i}: {items[i]}");
}
```

### Foreach Loops
- Simple collection iteration
- No index needed
- Most readable

```csharp
// GOOD: Foreach for simple iteration
foreach (var item in items)
{
    ProcessItem(item);
}
```

### While Loops
- Condition-based iteration
- Unknown iteration count
- External dependencies

```csharp
// GOOD: While for condition-based
while (reader.Read())
{
    ProcessData(reader);
}
```

## 2. Keep Loops Simple

```csharp
// BAD: Complex loop logic
for (int i = 0; i < list.Count; i++)
{
    if (list[i].IsValid && list[i].Value > 10 && !list[i].IsProcessed)
    {
        list[i].Process();
        list[i].IsProcessed = true;
    }
}

// GOOD: Extract complexity
foreach (var item in list.Where(x => x.IsValid && x.Value > 10 && !x.IsProcessed))
{
    item.Process();
    item.IsProcessed = true;
}
```

## 3. Use Meaningful Variable Names

```csharp
// BAD: Single letter index
for (int i = 0; i < customers.Count; i++)
{
    var c = customers[i];
}

// GOOD: Descriptive names
for (int customerIndex = 0; customerIndex < customers.Count; customerIndex++)
{
    var customer = customers[customerIndex];
}
```

## 4. Avoid Modifying Collections During Iteration

```csharp
// BAD: Modifying while iterating
foreach (var item in list)
{
    if (item.IsExpired)
        list.Remove(item); // InvalidOperationException
}

// GOOD: Iterate over copy
foreach (var item in list.ToList())
{
    if (item.IsExpired)
        list.Remove(item);
}

// BETTER: Use RemoveAll or Where
list.RemoveAll(x => x.IsExpired);
```

## 5. Cache Collection Length

```csharp
// INEFFICIENT: Calls .Count every iteration
for (int i = 0; i < items.Count; i++)
{
    ProcessItem(items[i]);
}

// EFFICIENT: Cache the count
int count = items.Count;
for (int i = 0; i < count; i++)
{
    ProcessItem(items[i]);
}

// OR: Use foreach (handles internally)
foreach (var item in items)
{
    ProcessItem(item);
}
```

## 6. Consider LINQ for Complex Operations

```csharp
// IMPERATIVE: Complex with manual loops
var result = new List<string>();
for (int i = 0; i < customers.Count; i++)
{
    if (customers[i].Status == "Active")
    {
        result.Add(customers[i].Name);
    }
}

// DECLARATIVE: Clear intent with LINQ
var result = customers
    .Where(c => c.Status == "Active")
    .Select(c => c.Name)
    .ToList();
```

## 7. Use Appropriate Break/Continue

```csharp
// GOOD: Break for early exit
for (int i = 0; i < list.Count; i++)
{
    if (list[i] == target)
    {
        return i;
    }
}

// GOOD: Continue for skipping
foreach (var item in items)
{
    if (!item.IsValid)
        continue;
    
    Process(item);
}
```

## 8. Be Careful with Nested Loops

```csharp
// INEFFICIENT: O(n²) complexity
for (int i = 0; i < list1.Count; i++)
{
    for (int j = 0; j < list2.Count; j++)
    {
        if (list1[i].Id == list2[j].Id)
        {
            matches.Add(list1[i]);
        }
    }
}

// EFFICIENT: Use HashSet or LINQ
var set = new HashSet<int>(list2.Select(x => x.Id));
var matches = list1.Where(x => set.Contains(x.Id)).ToList();
```

## 9. Use Yield for Large Sequences

```csharp
// BAD: Loads entire collection into memory
public List<int> GetNumbers(int count)
{
    var result = new List<int>();
    for (int i = 0; i < count; i++)
        result.Add(i);
    return result;
}

// GOOD: Lazy evaluation
public IEnumerable<int> GetNumbers(int count)
{
    for (int i = 0; i < count; i++)
        yield return i;
}
```

## 10. Document Loop Behavior

```csharp
/// <summary>
/// Processes items until a timeout or error occurs.
/// Breaks on first error.
/// </summary>
public void ProcessItems(List<Item> items, TimeSpan timeout)
{
    var deadline = DateTime.Now.AddMilliseconds(timeout.TotalMilliseconds);
    
    foreach (var item in items)
    {
        if (DateTime.Now > deadline)
            break; // Timeout
        
        try
        {
            item.Process();
        }
        catch
        {
            break; // Error
        }
    }
}
```

## Common Performance Patterns

### Pattern: Early Exit

```csharp
// GOOD: Exit when found
public bool Contains(int[] array, int target)
{
    for (int i = 0; i < array.Length; i++)
    {
        if (array[i] == target)
            return true; // Early exit
    }
    return false;
}
```

### Pattern: Aggregation

```csharp
// GOOD: Single pass
public int Sum(int[] numbers)
{
    int total = 0;
    foreach (var num in numbers)
    {
        total += num;
    }
    return total;
}
```

### Pattern: Transform

```csharp
// GOOD: Use LINQ for clarity
var doubled = numbers.Select(n => n * 2).ToList();
```

## Loop Testing Checklist

- [ ] Handles empty collections
- [ ] Correct boundary conditions
- [ ] Efficient for large data
- [ ] No collection modification during iteration
- [ ] Proper exception handling
- [ ] Clear variable names
- [ ] Documented complex logic
- [ ] Performance acceptable

## Summary

1. Choose the right loop type
2. Keep loops simple and readable
3. Use descriptive names
4. Don't modify during iteration
5. Cache collection length
6. Use LINQ for complex operations
7. Use break/continue appropriately
8. Watch nested loop performance
9. Use yield for large sequences
10. Document behavior
