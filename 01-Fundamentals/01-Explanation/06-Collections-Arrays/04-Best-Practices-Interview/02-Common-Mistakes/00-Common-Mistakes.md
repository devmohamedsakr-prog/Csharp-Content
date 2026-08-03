# Common Collections and Arrays Mistakes

## 1. Array Index Out of Bounds

```csharp
// WRONG
int[] arr = new int[5];
int value = arr[10];  // IndexOutOfRangeException!

// RIGHT
if (index >= 0 && index < arr.Length) {
    int value = arr[index];
}
```

## 2. Dictionary Key Not Found

```csharp
// WRONG
Dictionary<string, int> ages = new Dictionary<string, int> { { "Alice", 30 } };
int age = ages["Bob"];  // KeyNotFoundException!

// RIGHT
if (ages.TryGetValue("Bob", out int age)) {
    Console.WriteLine(age);
}
```

## 3. Modifying Collection While Iterating

```csharp
// WRONG - Iterator invalidated
List<int> list = new List<int> { 1, 2, 3, 4, 5 };
foreach (int item in list) {
    if (item > 2) list.Remove(item);  // Breaks iteration!
}

// RIGHT - Iterate copy
foreach (int item in list.ToList()) {
    if (item > 2) list.Remove(item);
}

// RIGHT - Use RemoveAll
list.RemoveAll(x => x > 2);
```

## 4. Assuming Empty Collection

```csharp
// WRONG
List<int> numbers = new List<int>();
int first = numbers[0];  // IndexOutOfRangeException!

// RIGHT
if (numbers.Count > 0) {
    int first = numbers[0];
}

// RIGHT
int first = numbers.FirstOrDefault();  // Returns 0 if empty
```

## 5. Using Wrong Collection Type

```csharp
// WRONG - Dictionary needed but using List
List<(string name, int age)> people = new List<(string, int)>();
// To find by name: O(n) search

// RIGHT - Dictionary for fast lookup
Dictionary<string, int> ages = new Dictionary<string, int>();
int age = ages["Alice"];  // O(1)
```

## 6. Removing from List Front Repeatedly

```csharp
// WRONG - O(n) operation repeated
while (list.Count > 0) {
    Process(list[0]);
    list.RemoveAt(0);  // Very inefficient!
}

// RIGHT - Use Queue
Queue<Item> queue = new Queue<Item>(list);
while (queue.Count > 0) {
    Process(queue.Dequeue());  // O(1)
}
```

## 7. Not Disposing IDisposable Collections

```csharp
// For collections containing IDisposable items
// WRONG
var resources = GetResources();  // List of StreamReaders
// Resources never disposed

// RIGHT
using (var resources = GetResources()) {
    // Use resources
}  // Automatically disposed
```

## 8. Comparing Collections with ==

```csharp
// WRONG - Compares references
List<int> list1 = new List<int> { 1, 2, 3 };
List<int> list2 = new List<int> { 1, 2, 3 };
if (list1 == list2) { }  // false (different objects)

// RIGHT - Compare contents
if (list1.SequenceEqual(list2)) { }  // true
```

## 9. Creating Array of Wrong Type

```csharp
// WRONG - Runtime error
object[] arr = new int[] { 1, 2, 3 };
arr[0] = "string";  // ArrayTypeMismatchException!

// RIGHT - Correct type
int[] arr = new int[] { 1, 2, 3 };
arr[0] = 5;  // OK
```

## 10. Multiple Enumeration of LINQ Results

```csharp
// WRONG - Multiple iterations
var filtered = list.Where(x => x > 5);
int count = filtered.Count();
var first = filtered.First();  // Re-iterated again

// RIGHT - Materialize once
var filtered = list.Where(x => x > 5).ToList();
int count = filtered.Count;  // No re-enumeration
var first = filtered.First();  // No re-enumeration
```

## 11. Null Reference on Collection

```csharp
// WRONG
List<int> list = null;
list.Add(5);  // NullReferenceException!

// RIGHT
List<int> list = new List<int>();
list.Add(5);

// RIGHT - Null check
if (list != null) {
    list.Add(5);
}
```

## 12. Adding Wrong Type to Array/Collection

```csharp
// WRONG - Type mismatch
List<int> numbers = new List<int>();
numbers.Add("string");  // Won't compile (compiler catches)

// WRONG - With ArrayList (non-generic)
ArrayList arr = new ArrayList();
arr.Add(5);
arr.Add("string");  // Compiles but causes runtime issues

// RIGHT - Use generic List
List<int> numbers = new List<int>();
numbers.Add(5);  // Type-safe
```

## 13. Not Initializing Jagged Array Rows

```csharp
// WRONG
int[][] jagged = new int[3][];
jagged[0][0] = 5;  // NullReferenceException!

// RIGHT - Initialize each row
int[][] jagged = new int[3][];
jagged[0] = new int[5];
jagged[0][0] = 5;  // OK
```

## 14. HashSet Not Preserving Order

```csharp
// WRONG - Assuming order preserved
HashSet<int> set = new HashSet<int> { 5, 1, 3, 2, 4 };
var list = set.ToList();  // Order is undefined!

// RIGHT - Use SortedSet if order matters
SortedSet<int> sorted = new SortedSet<int> { 5, 1, 3, 2, 4 };
// Result: {1, 2, 3, 4, 5} in sorted order

// RIGHT - Use List if insertion order matters
List<int> list = new List<int> { 5, 1, 3, 2, 4 };
// Order preserved as given
```

## 15. Dictionary with Mutable Key

```csharp
// WRONG - Mutable list as key
var list = new List<int> { 1, 2, 3 };
Dictionary<List<int>, string> dict = 
    new Dictionary<List<int>, string>();
dict[list] = "value";
list.Add(4);  // Hash breaks!
dict[list] = "value2";  // May not work correctly

// RIGHT - Use immutable key
Dictionary<string, string> dict = 
    new Dictionary<string, string>();
dict["key"] = "value";

// RIGHT - Use tuple as key
Dictionary<(int, string), double> dict = 
    new Dictionary<(int, string), double>();
```

## 16. First() vs FirstOrDefault()

```csharp
// WRONG - Throws if empty
var empty = new List<int>();
int first = empty.First();  // InvalidOperationException!

// RIGHT - Safe default
int first = empty.FirstOrDefault();  // Returns 0

// RIGHT - Explicit check
int first = empty.Any() ? empty.First() : 0;
```

## 17. Inefficient String Concatenation in Loop

```csharp
// WRONG - Creates new string each iteration
string result = "";
foreach (var item in items) {
    result += item.ToString();  // O(n²) complexity!
}

// RIGHT - Use StringBuilder
var sb = new StringBuilder();
foreach (var item in items) {
    sb.Append(item.ToString());
}
string result = sb.ToString();  // O(n)

// RIGHT - Use LINQ
string result = string.Join(",", items);
```

## 18. Queue/Stack Count Check

```csharp
// WRONG - May throw
string item = queue.Dequeue();  // If empty, throws!

// RIGHT - Check first
if (queue.Count > 0) {
    string item = queue.Dequeue();
}

// RIGHT - Use TryDequeue (C# 7+)
if (queue.TryDequeue(out string item)) {
    // Process item
}
```

## 19. Nested Loops Inefficiency

```csharp
// WRONG - O(n*m) search
foreach (var outer in list1) {
    foreach (var inner in list2) {
        if (outer.Id == inner.Id) {
            // Found match
        }
    }
}

// RIGHT - Use HashSet for O(n + m)
var set = new HashSet<int>(list2.Select(x => x.Id));
foreach (var item in list1) {
    if (set.Contains(item.Id)) {
        // Found match
    }
}

// RIGHT - Use Dictionary for O(1) lookup
var dict = list2.ToDictionary(x => x.Id);
foreach (var item in list1) {
    if (dict.TryGetValue(item.Id, out var match)) {
        // Found match
    }
}
```

## 20. Modifying List While Sorting

```csharp
// WRONG - Undefined behavior
List<int> list = new List<int> { 3, 1, 4, 1, 5 };
var sorted = list.OrderBy(x => x).ToList();
list.Add(2);  // Original modified while using sorted

// RIGHT - Work with materialized list
List<int> list = new List<int> { 3, 1, 4, 1, 5 };
var sorted = list.OrderBy(x => x).ToList();
// sorted is independent of list changes
```

## Summary of Common Mistakes

| Mistake | Problem | Solution |
|---------|---------|----------|
| Array index out of bounds | IndexOutOfRangeException | Check bounds first |
| Dictionary key not found | KeyNotFoundException | Use TryGetValue |
| Modify while iterating | Iterator invalidation | Iterate copy or use RemoveAll |
| Assuming non-empty | IndexOutOfRangeException | Check Count > 0 |
| Wrong collection type | Performance/functionality | Choose based on use case |
| Null collection | NullReferenceException | Initialize or check null |
| Multiple LINQ enumeration | Inefficiency | Materialize with ToList() |
| Mutable dictionary key | Hash breaks | Use immutable keys |
| Comparing lists with == | Wrong result | Use SequenceEqual |
| Jagged array not initialized | NullReferenceException | Initialize each row |

---

## Next Steps

1. Study Interview Questions
2. Practice Avoiding These Mistakes
3. Review Collection Selection
