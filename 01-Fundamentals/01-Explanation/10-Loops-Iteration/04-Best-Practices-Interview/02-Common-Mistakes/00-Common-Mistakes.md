# Common Loop Mistakes

## 1. Off-by-One Errors

### The Mistake

```csharp
// WRONG: Only processes 4 items (0-3), not 5
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(i); // 0, 1, 2, 3, 4 is actually CORRECT
}

// WRONG: Skips last element
for (int i = 0; i < array.Length - 1; i++)
{
    ProcessItem(array[i]); // Misses last item
}
```

### The Fix

```csharp
// CORRECT: Processes all 5
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(i); // 0, 1, 2, 3, 4
}

// CORRECT: Include last element
for (int i = 0; i < array.Length; i++)
{
    ProcessItem(array[i]);
}
```

## 2. Infinite Loops

### The Mistake

```csharp
// WRONG: i decrements, never < 5
for (int i = 5; i < 10; i--)
{
    Console.WriteLine(i); // Infinite!
}

// WRONG: Condition always true
while (true)
{
    ProcessData(); // Never exits
}

// WRONG: Variable never changes
int count = 0;
while (count < 10)
{
    DoWork(); // count never incremented
}
```

### The Fix

```csharp
// CORRECT: Increment
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(i);
}

// CORRECT: Include break or exit condition
while (true)
{
    if (shouldExit)
        break;
    ProcessData();
}

// CORRECT: Update the variable
while (count < 10)
{
    DoWork();
    count++; // Must increment
}
```

## 3. Modifying Collection During Iteration

### The Mistake

```csharp
// WRONG: InvalidOperationException
var list = new List<int> { 1, 2, 3, 4, 5 };
foreach (var item in list)
{
    if (item == 3)
        list.Remove(item); // Modifying during iteration!
}
```

### The Fix

```csharp
// FIX 1: Iterate over copy
foreach (var item in list.ToList())
{
    if (item == 3)
        list.Remove(item);
}

// FIX 2: Use RemoveAll
list.RemoveAll(x => x == 3);

// FIX 3: Use Where
var filtered = list.Where(x => x != 3).ToList();
```

## 4. Loop Variable Closure

### The Mistake

```csharp
// WRONG: All closures see final value of i
var actions = new List<Action>();
for (int i = 0; i < 3; i++)
{
    actions.Add(() => Console.WriteLine(i));
}

foreach (var action in actions)
    action(); // All print 3, not 0, 1, 2!
```

### The Fix

```csharp
// FIX: Create local copy
for (int i = 0; i < 3; i++)
{
    int copy = i; // New variable each iteration
    actions.Add(() => Console.WriteLine(copy));
}

foreach (var action in actions)
    action(); // Prints 0, 1, 2
```

## 5. Forgetting Break in Switch

### The Mistake

```csharp
// WRONG: Fall-through case
for (int i = 0; i < 3; i++)
{
    switch (i)
    {
        case 0:
            Console.WriteLine("Zero");
            // Missing break - falls through!
        case 1:
            Console.WriteLine("One"); // Prints for case 0 too!
            break;
    }
}
```

### The Fix

```csharp
// CORRECT: Each case has break
switch (i)
{
    case 0:
        Console.WriteLine("Zero");
        break;
    case 1:
        Console.WriteLine("One");
        break;
}
```

## 6. Continue/Break Scope Confusion

### The Mistake

```csharp
// WRONG: Assumes break exits both loops
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        if (someCondition)
            break; // Only exits inner loop!
    }
    // Outer loop continues
}
```

### The Fix

```csharp
// FIX 1: Use flag
bool found = false;
for (int i = 0; i < 3 && !found; i++)
{
    for (int j = 0; j < 3; j++)
    {
        if (someCondition)
        {
            found = true;
            break;
        }
    }
}

// FIX 2: Use method return
public bool Search()
{
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            if (someCondition)
                return true; // Exit all loops
        }
    }
    return false;
}
```

## 7. Wrong Loop Type

### The Mistake

```csharp
// WRONG: For loop when you just need to iterate
for (int i = 0; i < items.Count; i++)
{
    Console.WriteLine(items[i]); // Don't need index
}

// WRONG: While when you know the count
int i = 0;
while (i < 10)
{
    Console.WriteLine(i);
    i++;
}
```

### The Fix

```csharp
// RIGHT: Foreach when no index needed
foreach (var item in items)
{
    Console.WriteLine(item);
}

// RIGHT: For when count known and index needed
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}
```

## 8. Inefficient Nested Loops

### The Mistake

```csharp
// INEFFICIENT: O(n²) - checks every combination
for (int i = 0; i < list1.Count; i++)
{
    for (int j = 0; j < list2.Count; j++)
    {
        if (list1[i].Id == list2[j].Id)
            match = true;
    }
}
```

### The Fix

```csharp
// EFFICIENT: O(n) - uses HashSet
var ids = new HashSet<int>(list2.Select(x => x.Id));
foreach (var item in list1)
{
    if (ids.Contains(item.Id))
        match = true;
}

// OR: LINQ join
var matches = list1.Join(list2, x => x.Id, y => y.Id, (x, y) => x);
```

## 9. Accessing Out-of-Bounds Index

### The Mistake

```csharp
// WRONG: Off-by-one error
for (int i = 0; i <= array.Length; i++)
{
    Console.WriteLine(array[i]); // IndexOutOfRangeException on last iteration
}

// WRONG: Assumes next element exists
for (int i = 0; i < items.Count; i++)
{
    ProcessPair(items[i], items[i + 1]); // IndexOutOfRangeException on last
}
```

### The Fix

```csharp
// CORRECT: Use <, not <=
for (int i = 0; i < array.Length; i++)
{
    Console.WriteLine(array[i]);
}

// CORRECT: Check bounds
for (int i = 0; i < items.Count - 1; i++)
{
    ProcessPair(items[i], items[i + 1]);
}
```

## 10. Performance Issues

### The Mistake

```csharp
// INEFFICIENT: Creates enumerator multiple times
for (int i = 0; i < GetItems().Count; i++)
{
    ProcessItem(GetItems()[i]); // GetItems() called multiple times
}

// INEFFICIENT: LINQ called in loop
foreach (var item in items)
{
    var filtered = items.Where(x => x.Value > 0).ToList(); // Called every iteration!
}
```

### The Fix

```csharp
// EFFICIENT: Cache the collection
var itemList = GetItems();
for (int i = 0; i < itemList.Count; i++)
{
    ProcessItem(itemList[i]);
}

// EFFICIENT: Perform LINQ once
var filtered = items.Where(x => x.Value > 0).ToList();
foreach (var item in filtered)
{
    Process(item);
}
```

## Debugging Tips

### Check for Infinite Loops
- Print the loop counter
- Verify condition can become false
- Check if counter is being updated

### Check for Off-by-One
- Print array length vs loop count
- Verify boundary conditions (<, <=, etc.)
- Test edge cases (empty, single item)

### Check for Collection Issues
- Verify collection is not null
- Check for modification during iteration
- Verify bounds before access

## Quick Checklist

- [ ] Loop counter increments/decrements correctly
- [ ] Loop condition is correct (< not <=, etc.)
- [ ] No modification during iteration
- [ ] Correct loop type for task
- [ ] No out-of-bounds access
- [ ] No infinite loops
- [ ] Performance acceptable
- [ ] Clear variable names
- [ ] Break/continue scope correct

## Summary

- Off-by-one errors: Use correct comparison (<, <=, etc.)
- Infinite loops: Ensure condition can change
- Collection modification: Iterate copy
- Loop variable closure: Create local copy
- Wrong loop type: Choose for, while, or foreach
- Inefficiency: Avoid nested O(n²) loops
- Bounds errors: Check array length
- Performance: Cache values, use LINQ wisely
