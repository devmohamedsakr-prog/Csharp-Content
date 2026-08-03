# Queue<T> and Stack<T> - Specialized Collections

## Queue<T> - First-In-First-Out (FIFO)

A Queue follows FIFO principle - first element added is first removed.

### Creating Queues

```csharp
// Empty queue
Queue<int> queue = new Queue<int>();

// With initialization
Queue<string> names = new Queue<string> { "Alice", "Bob", "Charlie" };

// With capacity hint
Queue<int> numbers = new Queue<int>(10);
```

### Queue Operations

#### Enqueue - Add to back

```csharp
Queue<int> queue = new Queue<int>();

queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);

// Queue: [1, 2, 3]  (1 at front)
```

#### Dequeue - Remove from front

```csharp
Queue<int> queue = new Queue<int> { 1, 2, 3 };

int first = queue.Dequeue();  // 1
int second = queue.Dequeue();  // 2

// Queue now: [3]
```

#### Peek - View front without removing

```csharp
Queue<int> queue = new Queue<int> { 1, 2, 3 };

int front = queue.Peek();  // 1
// Queue unchanged: [1, 2, 3]
```

### Queue Properties

```csharp
Queue<int> queue = new Queue<int> { 1, 2, 3 };

// Count
int count = queue.Count;  // 3

// Check if empty
if (queue.Count > 0) {
    int front = queue.Peek();
}

// TryDequeue (safe)
if (queue.TryDequeue(out int value)) {
    Console.WriteLine($"Got: {value}");
}
```

### Queue Examples

#### Example 1: Task Queue

```csharp
Queue<string> tasks = new Queue<string>();

// Add tasks
tasks.Enqueue("Task 1");
tasks.Enqueue("Task 2");
tasks.Enqueue("Task 3");

// Process tasks in order
while (tasks.Count > 0) {
    string task = tasks.Dequeue();
    Console.WriteLine($"Processing: {task}");
}
```

#### Example 2: Printer Queue

```csharp
class PrinterQueue {
    private Queue<string> jobs = new Queue<string>();
    
    public void AddJob(string document) {
        jobs.Enqueue(document);
    }
    
    public void PrintNext() {
        if (jobs.Count > 0) {
            string doc = jobs.Dequeue();
            Console.WriteLine($"Printing: {doc}");
        }
    }
}
```

---

## Stack<T> - Last-In-First-Out (LIFO)

A Stack follows LIFO principle - last element added is first removed.

### Creating Stacks

```csharp
// Empty stack
Stack<int> stack = new Stack<int>();

// With initialization
Stack<string> names = new Stack<string> { "Alice", "Bob", "Charlie" };

// With capacity hint
Stack<int> numbers = new Stack<int>(10);
```

### Stack Operations

#### Push - Add to top

```csharp
Stack<int> stack = new Stack<int>();

stack.Push(1);
stack.Push(2);
stack.Push(3);

// Stack: [3, 2, 1]  (3 at top)
```

#### Pop - Remove from top

```csharp
Stack<int> stack = new Stack<int> { 1, 2, 3 };

int top = stack.Pop();      // 3
int next = stack.Pop();     // 2

// Stack now: [1]
```

#### Peek - View top without removing

```csharp
Stack<int> stack = new Stack<int> { 1, 2, 3 };

int top = stack.Peek();  // 3
// Stack unchanged: [3, 2, 1]
```

### Stack Properties

```csharp
Stack<int> stack = new Stack<int> { 1, 2, 3 };

// Count
int count = stack.Count;  // 3

// Check if empty
if (stack.Count > 0) {
    int top = stack.Peek();
}

// TryPop (safe)
if (stack.TryPop(out int value)) {
    Console.WriteLine($"Got: {value}");
}
```

### Stack Examples

#### Example 1: Undo/Redo

```csharp
class TextEditor {
    private Stack<string> undoStack = new Stack<string>();
    private string currentText = "";
    
    public void Edit(string text) {
        undoStack.Push(currentText);
        currentText = text;
    }
    
    public void Undo() {
        if (undoStack.Count > 0) {
            currentText = undoStack.Pop();
        }
    }
}
```

#### Example 2: Browser Back Button

```csharp
class Browser {
    private Stack<string> history = new Stack<string>();
    private string current = "Home";
    
    public void Navigate(string url) {
        history.Push(current);
        current = url;
        Console.WriteLine($"Navigated to: {url}");
    }
    
    public void Back() {
        if (history.Count > 0) {
            current = history.Pop();
            Console.WriteLine($"Back to: {current}");
        }
    }
}
```

#### Example 3: Balanced Parentheses

```csharp
bool IsBalanced(string text) {
    Stack<char> stack = new Stack<char>();
    
    foreach (char c in text) {
        if (c == '(' || c == '{' || c == '[') {
            stack.Push(c);
        } else if (c == ')' || c == '}' || c == ']') {
            if (stack.Count == 0) return false;
            char open = stack.Pop();
            if (!Matches(open, c)) return false;
        }
    }
    
    return stack.Count == 0;
}

bool Matches(char open, char close) {
    return (open == '(' && close == ')') ||
           (open == '{' && close == '}') ||
           (open == '[' && close == ']');
}
```

---

## Queue vs Stack Comparison

| Operation | Queue | Stack | Result |
|-----------|-------|-------|--------|
| Add [1,2,3] | Enqueue | Push | Queue: 1,2,3 / Stack: 3,2,1 |
| Remove first | Dequeue → 1 | Pop → 3 | Different! |

### Visual Comparison

```csharp
// Queue (FIFO)
Queue<int> q = new Queue<int>();
q.Enqueue(1); q.Enqueue(2); q.Enqueue(3);
// Front: [1] [2] [3] :Back
// Dequeue: 1, 2, 3

// Stack (LIFO)
Stack<int> s = new Stack<int>();
s.Push(1); s.Push(2); s.Push(3);
// Top: [3]
//      [2]
//      [1]
// Pop: 3, 2, 1
```

## Use Cases

### Queue Use Cases

- Task scheduling
- Print queues
- Message processing
- BFS (Breadth-First Search)
- Round-robin scheduling

```csharp
Queue<Task> taskQueue = new Queue<Task>();
// Process in order: first task added, first task executed
```

### Stack Use Cases

- Undo/Redo functionality
- Browser history
- Expression evaluation
- DFS (Depth-First Search)
- Function call stack

```csharp
Stack<string> undoHistory = new Stack<string>();
// Most recent change removed first when undo
```

## Performance

### Queue

```csharp
// Enqueue - O(1)
queue.Enqueue(item);

// Dequeue - O(1)
queue.Dequeue();

// Peek - O(1)
queue.Peek();
```

### Stack

```csharp
// Push - O(1)
stack.Push(item);

// Pop - O(1)
stack.Pop();

// Peek - O(1)
stack.Peek();
```

## Iterating

### Queue Iteration

```csharp
Queue<int> queue = new Queue<int> { 1, 2, 3, 4, 5 };

foreach (int item in queue) {
    Console.WriteLine(item);
}
// Note: Doesn't remove items
```

### Stack Iteration

```csharp
Stack<int> stack = new Stack<int> { 1, 2, 3, 4, 5 };

foreach (int item in stack) {
    Console.WriteLine(item);
}
// Note: Iteration order is top-to-bottom
```

## Best Practices

✓ **Use Queue for FIFO**
```csharp
Queue<string> queue = new Queue<string>();
// First in, first out
```

✓ **Use Stack for LIFO**
```csharp
Stack<string> stack = new Stack<string>();
// Last in, first out
```

✓ **Check Count before Pop/Dequeue**
```csharp
if (stack.Count > 0) {
    int item = stack.Pop();
}

// Or use TryPop
if (stack.TryPop(out int item)) { }
```

## Anti-Patterns

❌ **Using List for Queue behavior**
```csharp
list.Remove(list[0]);  // Inefficient, O(n)
// Use Queue instead
```

❌ **Using List for Stack behavior**
```csharp
list.RemoveAt(list.Count - 1);  // Works but inefficient
// Use Stack instead
```

❌ **Popping without checking**
```csharp
int item = stack.Pop();  // May throw if empty!
```

## Summary

- **Queue** - FIFO (First In, First Out)
- **Stack** - LIFO (Last In, First Out)
- Both O(1) for add/remove operations
- Queue for sequential processing
- Stack for reversible operations (undo, DFS)
- Both faster than List for these specific patterns

---

## Next Steps

1. Study Collection Patterns
2. Review Collection Selection Guide
3. Study Best Practices
