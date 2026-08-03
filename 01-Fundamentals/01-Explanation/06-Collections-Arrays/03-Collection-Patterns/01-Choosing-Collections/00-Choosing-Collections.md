# Choosing the Right Collection

## Overview
Selecting the correct collection type is crucial for performance and code clarity. This guide helps you choose based on your use case.

## Quick Decision Tree

```
Do you need key-value pairs?
├─ YES → Dictionary<K,V>
└─ NO → Continue...

Do you need ordered, indexed access?
├─ YES → Array or List<T>
└─ NO → Continue...

Do you need unique values only?
├─ YES → HashSet<T>
└─ NO → Continue...

Do you need FIFO (first in, first out)?
├─ YES → Queue<T>
└─ NO → Continue...

Do you need LIFO (last in, first out)?
├─ YES → Stack<T>
└─ NO → Use List<T> (default)
```

## Collection Comparison Table

| Collection | Access | Size | Unique | Order | Best For |
|-----------|--------|------|--------|-------|----------|
| Array | Fast (O(1)) | Fixed | No | Yes | Known size, fast access |
| List<T> | Fast (O(1)) | Dynamic | No | Yes | General purpose, dynamic |
| Dictionary<K,V> | Fast (O(1)) | Dynamic | Yes* | No | Key-value lookup |
| HashSet<T> | Fast (O(1)) | Dynamic | Yes | No | Membership testing |
| Queue<T> | O(1) | Dynamic | No | FIFO | Task processing |
| Stack<T> | O(1) | Dynamic | No | LIFO | Undo/redo |

*Dictionary keys are unique; values can duplicate

## Detailed Comparison

### Array vs List

**Use Array when:**
- Size is known and fixed
- Need maximum performance
- Working with numeric indices extensively

```csharp
// Array - fixed 5 elements
int[] scores = new int[5];
scores[0] = 100;
// scores[10] = 50;  // Error!
```

**Use List when:**
- Size varies or unknown
- Frequently adding/removing elements
- Need convenience methods

```csharp
// List - dynamic growth
List<int> scores = new List<int>();
scores.Add(100);
scores.Add(95);
// Can grow indefinitely
```

**Performance Comparison:**
```csharp
// Access by index - both O(1)
int arr_val = array[5];      // Very fast
int list_val = list[5];      // Very fast

// Adding element - different
array.Length;                // Fixed, can't add
list.Add(new_item);          // O(1) amortized
```

### List vs Dictionary

**Use List when:**
- Order matters
- Sequential access important
- Keys are indices

```csharp
List<Person> people = new List<Person>();
// Access by position: people[0], people[1]
```

**Use Dictionary when:**
- Need fast lookup by key
- Order doesn't matter
- Key is not an integer index

```csharp
Dictionary<string, Person> byName = 
    new Dictionary<string, Person>();
// Access by name: byName["Alice"]
```

### Dictionary vs HashSet

**Use Dictionary when:**
- Need to associate value with key
- Example: age by name

```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};
```

**Use HashSet when:**
- Only need to know if item exists
- Don't need associated values
- Example: allowed words

```csharp
HashSet<string> allowed = new HashSet<string> {
    "apple", "banana", "cherry"
};
bool valid = allowed.Contains(word);
```

### List vs HashSet

**Use List when:**
- Order matters
- May contain duplicates
- Need indexed access

```csharp
List<int> numbers = new List<int> { 1, 2, 2, 3 };
// Keeps duplicates and order
```

**Use HashSet when:**
- Order doesn't matter
- Only want unique values
- Need fast membership testing

```csharp
HashSet<int> unique = new HashSet<int> { 1, 2, 3 };
// Removes duplicates automatically
if (unique.Contains(2)) { }  // O(1)
```

### List vs Queue vs Stack

**Use Queue when:**
- Process in FIFO order
- Example: task queue, print queue

```csharp
Queue<string> tasks = new Queue<string>();
tasks.Enqueue("Task 1");
tasks.Enqueue("Task 2");
string next = tasks.Dequeue();  // Task 1
```

**Use Stack when:**
- Process in LIFO order
- Example: undo/redo, call stack

```csharp
Stack<string> history = new Stack<string>();
history.Push("State 1");
history.Push("State 2");
string prev = history.Pop();  // State 2
```

**Use List when:**
- Access in any order
- Not strictly FIFO or LIFO

```csharp
List<string> items = new List<string>();
items.Add("item 1");
items.Add("item 2");
string any = items[0];  // Any index
```

## Real-World Scenarios

### Scenario 1: Student Grades Lookup

**Requirement:** Given a student name, get their grade

```csharp
// BEST: Dictionary
Dictionary<string, double> grades = 
    new Dictionary<string, double> {
    { "Alice", 95 },
    { "Bob", 87 }
};
double aliceGrade = grades["Alice"];  // O(1)

// NOT IDEAL: List
List<(string name, double grade)> list = 
    new List<(string, double)>();
// Would need to search through: O(n)
```

### Scenario 2: Unique IP Addresses

**Requirement:** Track unique visitors

```csharp
// BEST: HashSet
HashSet<string> visitors = new HashSet<string>();
visitors.Add("192.168.1.1");
visitors.Add("192.168.1.1");  // Duplicate ignored
int uniqueCount = visitors.Count;  // Fast

// NOT IDEAL: List
List<string> ips = new List<string>();
// Would have many duplicates
```

### Scenario 3: Process Tasks in Order

**Requirement:** Process tasks as they arrive

```csharp
// BEST: Queue
Queue<string> taskQueue = new Queue<string>();
taskQueue.Enqueue("task1");
taskQueue.Enqueue("task2");
while (taskQueue.Count > 0) {
    string task = taskQueue.Dequeue();  // FIFO
}

// NOT IDEAL: List
List<string> tasks = new List<string>();
// Must remove from front: O(n)
```

### Scenario 4: Undo Functionality

**Requirement:** Undo recent actions

```csharp
// BEST: Stack
Stack<string> undoHistory = new Stack<string>();
undoHistory.Push("action1");
undoHistory.Push("action2");
string lastAction = undoHistory.Pop();  // LIFO

// NOT IDEAL: List
List<string> actions = new List<string>();
// Must remove from end each time
```

### Scenario 5: Leaderboard Scores

**Requirement:** Top 10 highest scores in order

```csharp
// BEST: SortedList or List ordered by Score
List<(string name, int score)> leaderboard = 
    new List<(string, int)>();
leaderboard = leaderboard
    .OrderByDescending(x => x.score)
    .Take(10)
    .ToList();

// Not Dictionary (doesn't maintain order for display)
```

## Performance Quick Reference

### Access Performance

```csharp
// O(1) - constant time (fastest)
array[index];
list[index];
dict[key];
hashSet.Contains(item);

// O(n) - linear time
list.IndexOf(item);
list.Contains(item);
array.IndexOf(item);  // If using Array.IndexOf
```

### Insertion Performance

```csharp
// O(1) amortized
list.Add(item);
queue.Enqueue(item);
stack.Push(item);
dict[key] = value;
hashSet.Add(item);

// O(n) - shifts elements
list.Insert(0, item);  // At front
array.Resize();        // If growing
```

### Deletion Performance

```csharp
// O(1)
queue.Dequeue();
stack.Pop();
hashSet.Remove(item);

// O(n) - shifts elements
list.RemoveAt(0);      // From front
list.Remove(item);     // Search + shift
```

## Decision Flowchart Example

```
Need to store test scores and look up by name?
├─ Need order? 
│  ├─ YES (for sorted display) → SortedDictionary
│  └─ NO → Dictionary (faster)
│
Need to track all unique visitors?
├─ YES → HashSet
│
Need to process tasks in received order?
├─ YES → Queue
│
Need undo/redo?
├─ YES → Stack
│
Need general-purpose collection?
└─ YES → List (default safe choice)
```

## Best Practices

✓ **Choose based on operations**
```csharp
// Need fast lookup? Dictionary
// Need unique values? HashSet
// Need order? List
// Need FIFO? Queue
// Need LIFO? Stack
```

✓ **Default to List unless specific need**
```csharp
// If unsure, start with List
List<T> items = new List<T>();
// Change only if profiling shows need
```

✓ **Reconsider as requirements evolve**
```csharp
// Started with List for lookup?
List<(string key, int value)> list;

// Profiling shows lookups are slow?
// Switch to Dictionary
Dictionary<string, int> dict;
```

## Summary

- **Array** - Fixed size, fast access, known length
- **List** - Dynamic, fast access, general purpose
- **Dictionary** - Fast key lookup, no order
- **HashSet** - Unique values, fast membership test
- **Queue** - FIFO processing, task ordering
- **Stack** - LIFO reversal, undo/redo
- Choose based on operations (access, search, order)
- Performance matters - profile if in doubt

---

## Next Steps

1. Learn Iteration Patterns
2. Study LINQ with Collections
3. Review Best Practices
