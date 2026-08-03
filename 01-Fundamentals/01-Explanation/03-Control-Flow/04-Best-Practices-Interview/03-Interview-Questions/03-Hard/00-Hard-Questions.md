# Control Flow Interview - Hard Level

## Q1: Design a state machine using switch

**Answer**:
```csharp
public class StateMachine {
    private State _current = State.Initial;
    
    public void Process(Event evt) {
        _current = (_current, evt) switch {
            (State.Initial, Event.Start) => State.Running,
            (State.Running, Event.Pause) => State.Paused,
            (State.Paused, Event.Resume) => State.Running,
            (State.Running, Event.Stop) => State.Stopped,
            _ => _current  // No change
        };
    }
}

enum State { Initial, Running, Paused, Stopped }
enum Event { Start, Pause, Resume, Stop }
```

---

## Q2: Optimize loop performance - when to use which?

**Answer**:
```csharp
// Array: fastest for large data
public void Process(int[] items) {
    for (int i = 0; i < items.Length; i++) {
        items[i] = Transform(items[i]);
    }
}

// List: safe bounds checking
public void Process(List<int> items) {
    for (int i = 0; i < items.Count; i++) {
        items[i] = Transform(items[i]);
    }
}

// Foreach: cleaner when no index needed
public void Process(IEnumerable<int> items) {
    foreach (var item in items) {
        Process(item);
    }
}
```

---

## Q3: Handle complex control flow cleanly

**Answer**:
```csharp
// Extract methods to reduce nesting
public bool ValidateAndProcess(Order order) {
    if (!ValidateOrder(order)) return false;
    if (!AuthorizePayment(order)) return false;
    return ProcessOrder(order);
}

// Each method handles one concern
private bool ValidateOrder(Order order) => 
    order != null && order.Items.Count > 0;

private bool AuthorizePayment(Order order) =>
    paymentService.Authorize(order.Total);

private bool ProcessOrder(Order order) {
    // Process logic
    return true;
}
```

---

## Q4: When to use recursion vs loops?

**Answer**:
```csharp
// Use loops (more efficient)
public void TraverseList(List<int> items) {
    foreach (var item in items) {
        Console.WriteLine(item);
    }
}

// Use recursion (for tree/graph structures)
public void TraverseTree(TreeNode node) {
    if (node == null) return;
    Console.WriteLine(node.Value);
    TraverseTree(node.Left);
    TraverseTree(node.Right);
}
```

---

## Q5: Design loop with LINQ vs traditional

**Answer**:
```csharp
// Traditional
List<int> result = new();
foreach (var item in items) {
    if (item > 10 && item < 100) {
        result.Add(item * 2);
    }
}

// LINQ (cleaner for complex queries)
var result = items
    .Where(x => x > 10 && x < 100)
    .Select(x => x * 2)
    .ToList();
```

---

## Q6: Tail recursion optimization

**Answer**:
```csharp
// Not tail recursive (multiple recursive calls)
public int Fibonacci(int n) {
    if (n <= 1) return n;
    return Fibonacci(n - 1) + Fibonacci(n - 2);  // 2 calls
}

// Use loop instead (no stack overflow risk)
public int Fibonacci(int n) {
    if (n <= 1) return n;
    int a = 0, b = 1;
    for (int i = 2; i <= n; i++) {
        int temp = a + b;
        a = b;
        b = temp;
    }
    return b;
}
```

---

## Summary

- State machines with switch/pattern matching
- Choose loop type by access pattern
- Extract methods to reduce nesting
- Use recursion for tree/graph, loops for sequences
- LINQ for complex queries
- Avoid deep recursion (stack overflow)

---

**Complete**: All control flow interview questions covered
