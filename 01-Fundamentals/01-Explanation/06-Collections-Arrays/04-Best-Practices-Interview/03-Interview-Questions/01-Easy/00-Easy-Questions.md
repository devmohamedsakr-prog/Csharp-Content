# Collections and Arrays - Easy Interview Questions

## Q1: What's the difference between Array and List?

**Answer:**
- **Array**: Fixed size, created with `new int[5]`, cannot grow/shrink, direct memory allocation
- **List**: Dynamic size, grows as needed, more convenient, wraps array internally

```csharp
// Array - fixed
int[] arr = new int[5];  // Always 5 elements
arr[10] = 5;  // Error!

// List - dynamic
List<int> list = new List<int>();
list.Add(5);  // Grows automatically
```

---

## Q2: When would you use Dictionary over List?

**Answer:** When you need fast lookup by key instead of sequential access.

```csharp
// Dictionary - O(1) lookup
Dictionary<string, int> ages = new Dictionary<string, int>();
int aliceAge = ages["Alice"];  // Fast

// List - O(n) search
List<(string, int)> people = new List<(string, int)>();
// Must iterate to find Alice
```

**Use Dictionary when:** Key-value relationship, fast lookup needed
**Use List when:** Sequential processing, any indexed access

---

## Q3: How do you safely access a Dictionary element?

**Answer:** Use `TryGetValue` instead of direct access:

```csharp
// SAFE - Won't throw
if (dict.TryGetValue("key", out int value)) {
    Console.WriteLine($"Found: {value}");
} else {
    Console.WriteLine("Not found");
}

// UNSAFE - Throws KeyNotFoundException
int value = dict["missing"];  // Exception!
```

---

## Q4: What is the purpose of HashSet?

**Answer:** Stores unique values only, fast membership testing, automatic duplicate removal.

```csharp
HashSet<int> numbers = new HashSet<int> { 1, 2, 2, 3, 3 };
// Automatically removes duplicates
// Result: {1, 2, 3}

bool has2 = numbers.Contains(2);  // O(1) very fast
```

**Use cases:**
- Remove duplicates: `new HashSet<T>(list)`
- Fast membership testing: `set.Contains(item)`
- Set operations: Union, Intersection, Difference

---

## Q5: Explain Queue vs Stack

**Answer:**
- **Queue**: FIFO (First In, First Out) - Dequeue from front
- **Stack**: LIFO (Last In, First Out) - Pop from top

```csharp
// Queue - FIFO
Queue<int> q = new Queue<int>();
q.Enqueue(1); q.Enqueue(2); q.Enqueue(3);
int first = q.Dequeue();  // 1

// Stack - LIFO
Stack<int> s = new Stack<int>();
s.Push(1); s.Push(2); s.Push(3);
int top = s.Pop();  // 3
```

**Queue uses:** Task processing, print queue, BFS
**Stack uses:** Undo/redo, call stack, DFS

---

## Q6: How do you safely remove items from a List while iterating?

**Answer:** Iterate over a copy or use RemoveAll:

```csharp
// Option 1 - Iterate copy
foreach (var item in list.ToList()) {
    if (condition) list.Remove(item);
}

// Option 2 - RemoveAll
list.RemoveAll(x => condition);

// WRONG - Breaks iteration
foreach (var item in list) {
    if (condition) list.Remove(item);  // Error!
}
```

---

## Q7: What's the best way to find an element in a collection?

**Answer:** Depends on collection type:

```csharp
// List - O(n)
int index = list.IndexOf(item);
bool has = list.Contains(item);

// Dictionary - O(1)
bool has = dict.ContainsKey(key);

// HashSet - O(1)
bool has = set.Contains(item);

// LINQ - O(n) but clear
var found = list.FirstOrDefault(x => x.Id == 5);
```

**Best practice:** Use appropriate collection for your lookup pattern

---

## Q8: How do you sort a collection?

**Answer:**
```csharp
// List - Sort in place
List<int> numbers = new List<int> { 5, 2, 8, 1 };
numbers.Sort();

// LINQ - Returns sorted copy
var sorted = numbers.OrderBy(x => x).ToList();

// Descending
var descending = numbers.OrderByDescending(x => x).ToList();
```

---

## Q9: What are multi-dimensional arrays?

**Answer:** Arrays with multiple dimensions:

```csharp
// 2D rectangular array
int[,] matrix = new int[3, 3];
matrix[0, 1] = 5;

// 3D array
int[,,] cube = new int[2, 3, 4];

// Jagged array (different row lengths)
int[][] jagged = new int[3][];
jagged[0] = new int[5];
jagged[1] = new int[3];  // Different length!
```

---

## Q10: Explain LINQ Where and Select

**Answer:**
```csharp
// Where - Filter
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
var evens = numbers.Where(x => x % 2 == 0);
// Result: {2, 4}

// Select - Transform
var doubled = numbers.Select(x => x * 2);
// Result: {2, 4, 6, 8, 10}

// Chain together
var result = numbers
    .Where(x => x > 2)      // {3, 4, 5}
    .Select(x => x * 2)     // {6, 8, 10}
    .ToList();
```

---

## Summary of Easy Concepts

✓ Array vs List - fixed vs dynamic
✓ Dictionary for key-value lookup
✓ HashSet for unique values
✓ Queue for FIFO, Stack for LIFO
✓ Safe collection modification
✓ LINQ basics (Where, Select)
✓ Multi-dimensional arrays
✓ Performance: O(1) vs O(n) operations

---

## Next Steps

1. Practice writing code examples
2. Understand performance implications
3. Move to Medium questions
