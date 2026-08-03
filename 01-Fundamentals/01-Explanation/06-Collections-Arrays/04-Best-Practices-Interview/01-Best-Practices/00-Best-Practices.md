# Collections and Arrays Best Practices

## 1. Choose the Right Collection Type

```csharp
// GOOD - Dictionary for fast lookup by key
Dictionary<string, int> agesByName = new Dictionary<string, int>();

// AVOID - List when Dictionary would be better
List<(string name, int age)> people = new List<(string, int)>();
// Slower lookup: O(n) instead of O(1)
```

## 2. Use LINQ for Clarity

```csharp
// GOOD - Clear intent
var adults = people.Where(p => p.Age >= 18).ToList();

// AVOID - Verbose manual loop
List<Person> adults = new List<Person>();
foreach (var person in people) {
    if (person.Age >= 18) {
        adults.Add(person);
    }
}
```

## 3. Use Appropriate Iteration Method

```csharp
// GOOD - foreach for simple traversal
foreach (var item in collection) {
    Console.WriteLine(item);
}

// GOOD - for loop when need index
for (int i = 0; i < collection.Count; i++) {
    collection[i] = modified;
}

// AVOID - wrong loop type for task
List<int> numbers = new List<int> { 1, 2, 3 };
for (int i = 0; i < numbers.Count; i++) {  // Unnecessary complexity
    Console.WriteLine(numbers[i]);
}
```

## 4. Check Bounds Before Access

```csharp
// GOOD - Verify before access
if (index >= 0 && index < array.Length) {
    int value = array[index];
}

// GOOD - Use safe access methods
if (dict.TryGetValue(key, out var value)) {
    Console.WriteLine(value);
}

// AVOID - Direct access without checking
int value = array[unknownIndex];  // May throw!
```

## 5. Don't Modify While Iterating

```csharp
// GOOD - Iterate copy
foreach (var item in list.ToList()) {
    if (condition) list.Remove(item);
}

// GOOD - Use RemoveAll
list.RemoveAll(x => condition);

// AVOID - Modify collection during foreach
foreach (var item in list) {
    if (condition) list.Remove(item);  // Breaks iteration!
}
```

## 6. Use HashSet for Unique Values

```csharp
// GOOD - HashSet removes duplicates automatically
var unique = new HashSet<int> { 1, 2, 2, 3, 3 };
// Result: {1, 2, 3}

// AVOID - List with duplicates
var list = new List<int> { 1, 2, 2, 3, 3 };
// Duplicates remain
```

## 7. Materialize LINQ When Needed Multiple Times

```csharp
// GOOD - Materialize once
var filtered = list.Where(x => x > 5).ToList();
int count = filtered.Count;
var first = filtered.FirstOrDefault();  // No re-enumeration

// AVOID - Multiple enumerations
var filtered = list.Where(x => x > 5);
int count = filtered.Count();      // Iterates
var first = filtered.FirstOrDefault();  // Iterates again
```

## 8. Use Queue for FIFO, Stack for LIFO

```csharp
// GOOD - Queue for sequential processing
Queue<string> tasks = new Queue<string>();
tasks.Enqueue("task1");
string next = tasks.Dequeue();  // First in, first out

// GOOD - Stack for undo functionality
Stack<string> history = new Stack<string>();
history.Push("state1");
string prev = history.Pop();  // Last in, first out

// AVOID - List for these patterns
List<string> list = new List<string>();
list.Add("item");
var removed = list[0];  // Inefficient if used as queue
```

## 9. Use Immutable Collections When Appropriate

```csharp
// GOOD - Immutable for thread safety
ImmutableList<int> numbers = ImmutableList.Create(1, 2, 3);

// GOOD - Record with readonly
record Person(string Name, int Age);

// Use mutable when modification frequent
List<string> items = new List<string>();
```

## 10. Use Default Equality and Comparison

```csharp
// GOOD - Rely on default comparison
var sorted = numbers.OrderBy(x => x).ToList();

// GOOD - Implement IComparable if needed
class Person : IComparable<Person> {
    public int CompareTo(Person other) => Age.CompareTo(other.Age);
}

// AVOID - Custom comparer unless necessary
numbers.OrderBy(x => x, new CustomComparer()).ToList();
```

## 11. Avoid Unnecessary Array Resizing

```csharp
// GOOD - Use List which handles growth
List<int> items = new List<int>();
items.Add(1);
items.Add(2);  // Grows automatically

// GOOD - Pre-allocate if size known
List<int> items = new List<int>(expectedCapacity);

// AVOID - Manual array resizing
int[] arr = new int[5];
Array.Resize(ref arr, 10);  // Expensive operation
```

## 12. Use Explicit Null Checking

```csharp
// GOOD - Explicit check
if (collection != null && collection.Count > 0) {
    var first = collection[0];
}

// GOOD - Null coalescing
var items = collection ?? new List<int>();

// GOOD - Null conditional
int? count = collection?.Count;

// AVOID - Assuming non-null
int first = collection[0];  // May throw if null
```

## 13. Understand Performance Implications

```csharp
// GOOD - O(1) operations
dict[key] = value;  // Dictionary
list[index] = value;  // List by index

// AVOID - O(n) operations carelessly
list.RemoveAt(0);  // From front, shifts all elements
list.Remove(item);  // Searches then shifts
list.IndexOf(item);  // Linear search

// GOOD - Use appropriate structure
Queue<int> q = new Queue<int>();
int first = q.Dequeue();  // O(1) instead of RemoveAt(0)
```

## 14. Document Collection Constraints

```csharp
// GOOD - Document size and type constraints
/// <summary>
/// Gets the list of active users.
/// </summary>
/// <remarks>
/// - List is immutable after retrieval
/// - Maximum 1000 users per query
/// </remarks>
public List<User> GetActiveUsers() { }

// GOOD - Use return types to indicate constraints
IReadOnlyList<int> GetNumbers();  // Can't modify returned list
IEnumerable<int> GetSequence();   // Lazy evaluation expected
```

## 15. Use Proper Comparison for Elements

```csharp
// GOOD - Override Equals and GetHashCode for custom types
public class User : IEquatable<User> {
    public override bool Equals(object obj) => Equals(obj as User);
    public bool Equals(User other) => Id == other?.Id;
    public override int GetHashCode() => Id.GetHashCode();
}

// GOOD - Use SequenceEqual for collection comparison
bool same = list1.SequenceEqual(list2);

// AVOID - Direct == for collections
if (list1 == list2) { }  // Compares references, not contents
```

## Summary of Best Practices

✓ Choose right collection type for your use case
✓ Use LINQ for clarity and conciseness
✓ Use appropriate iteration method
✓ Check bounds and use safe access
✓ Don't modify while iterating
✓ Use HashSet for unique values
✓ Materialize LINQ when needed multiple times
✓ Use Queue/Stack for specific patterns
✓ Consider immutable collections
✓ Understand performance implications
✓ Document constraints
✓ Implement proper equality

---

## Next Steps

1. Learn Common Mistakes
2. Study Interview Questions
3. Practice Collection Usage
