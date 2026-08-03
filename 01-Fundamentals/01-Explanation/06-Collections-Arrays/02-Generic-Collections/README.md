# Generic Collections

## Overview
Generic collections (List, Dictionary, HashSet, Queue, Stack) provide flexible, type-safe storage. Master each collection type and know when to use each.

## Learning Path

### 1. List<T> - Dynamic Arrays
- Adding, removing, inserting elements
- Accessing by index
- Performance characteristics
- Best use cases

**Time:** 15-20 minutes

### 2. Dictionary<TKey, TValue> - Key-Value Pairs
- Creating and populating
- Fast key lookup
- Iterating entries
- Handling missing keys

**Time:** 20-25 minutes

### 3. HashSet<T> - Unique Values
- Removing duplicates
- Fast membership testing
- Set operations
- Performance benefits

**Time:** 15-20 minutes

### 4. Queue<T> & Stack<T> - Specialized Collections
- FIFO (Queue) operations
- LIFO (Stack) operations
- Real-world patterns
- Performance considerations

**Time:** 15-20 minutes

## Files in This Section

1. **00-List.md** - Dynamic collections
2. **00-Dictionary.md** - Key-value pairs
3. **00-HashSet.md** - Unique values
4. **00-Queue-Stack.md** - Specialized collections

## Quick Reference

```csharp
// List - O(1) add/access, O(n) remove
List<int> list = new List<int>();
list.Add(5);
int value = list[0];

// Dictionary - O(1) add/lookup/remove
Dictionary<string, int> dict = new Dictionary<string, int>();
dict["key"] = 5;
if (dict.TryGetValue("key", out int v)) { }

// HashSet - O(1) add/remove/contains, unique only
HashSet<int> set = new HashSet<int>();
set.Add(5);
bool has = set.Contains(5);

// Queue - FIFO, O(1) enqueue/dequeue
Queue<int> q = new Queue<int>();
q.Enqueue(5);
int first = q.Dequeue();

// Stack - LIFO, O(1) push/pop
Stack<int> s = new Stack<int>();
s.Push(5);
int top = s.Pop();
```

## Collection Comparison

| Collection | Add | Remove | Lookup | Duplicates | Order |
|-----------|-----|--------|--------|-----------|-------|
| List | O(1)* | O(n) | O(n) | Yes | Preserved |
| Dictionary | O(1) | O(1) | O(1) | No | Insertion |
| HashSet | O(1) | O(1) | O(1) | No | Undefined |
| Queue | O(1) | O(1) | - | Yes | FIFO |
| Stack | O(1) | O(1) | - | Yes | LIFO |

*List amortized time

## When to Use Each

### List<T>
✓ Need indexed access
✓ Size varies frequently
✓ Need fast iteration
✓ Allow duplicates

### Dictionary<TKey, TValue>
✓ Fast key-based lookup
✓ Key-value associations
✓ Cache implementation
✓ Counting/grouping

### HashSet<T>
✓ Unique values only
✓ Fast membership testing
✓ Remove duplicates
✓ Set operations

### Queue<T>
✓ FIFO processing
✓ Task scheduling
✓ BFS algorithms
✓ Print queuing

### Stack<T>
✓ LIFO processing
✓ Undo/redo functionality
✓ DFS algorithms
✓ Expression evaluation

## Best Practices

✓ Use TryGetValue for Dictionary safe access
✓ Choose right collection for use case
✓ Understand performance trade-offs
✓ Use generic collections, not ArrayList
✓ Initialize with expected capacity if known

## Common Mistakes

❌ Dictionary[key] throws if missing
❌ HashSet doesn't preserve order
❌ Queue.Dequeue() throws if empty
❌ Using List when Dictionary better
❌ Modifying while iterating

## Self-Assessment

Can you:
- [ ] Use List<T> effectively?
- [ ] Safely access Dictionary values?
- [ ] Understand HashSet uniqueness?
- [ ] Use Queue for FIFO?
- [ ] Use Stack for LIFO?
- [ ] Choose right collection?

---

## Related Topics

- **Arrays** - Fixed-size collections
- **Collection Patterns** - Selection and iteration
- **LINQ** - Filtering and transforming
- **Best Practices** - Performance optimization

## Next Steps

1. ✓ Learn List<T>
2. ✓ Study Dictionary<K,V>
3. ✓ Master HashSet<T>
4. ✓ Understand Queue/Stack
5. → Move to Collection Patterns
6. → Study Best Practices
