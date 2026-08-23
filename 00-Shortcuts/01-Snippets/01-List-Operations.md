# List Operations

Quick snippets for common list operations.

## Create and Initialize

```csharp
// Empty list
var numbers = new List<int>();

// With initialization
var items = new List<string> { "apple", "banana", "cherry" };

// From array
var fromArray = new List<int>(new[] { 1, 2, 3, 4, 5 });

// From LINQ
var fromLinq = Enumerable.Range(1, 5).ToList();
```

## Add & Remove

```csharp
var list = new List<int> { 1, 2, 3 };

// Add single item
list.Add(4);

// Add multiple items
list.AddRange(new[] { 5, 6, 7 });

// Remove by value
list.Remove(3);

// Remove by index
list.RemoveAt(0);

// Clear all
list.Clear();
```

## Find & Check

```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Contains
bool has3 = numbers.Contains(3);

// Find first
int first = numbers.FirstOrDefault(x => x > 3);

// Find all
var evens = numbers.Where(x => x % 2 == 0).ToList();

// Index of
int index = numbers.IndexOf(3);
```

## Dictionary Operations

```csharp
// Create dictionary
var dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

// Add key-value
dict["c"] = 3;
dict.Add("d", 4);

// Get value
int value = dict["a"];
bool exists = dict.TryGetValue("a", out int val);

// Remove
dict.Remove("a");

// Keys and values
var allKeys = dict.Keys.ToList();
var allValues = dict.Values.ToList();
```

## Queue Operations

```csharp
var queue = new Queue<string>();

// Enqueue (add to end)
queue.Enqueue("first");
queue.Enqueue("second");

// Dequeue (remove from front)
string item = queue.Dequeue();

// Peek (view without removing)
string peek = queue.Peek();

// Check if empty
bool isEmpty = queue.Count == 0;
```

