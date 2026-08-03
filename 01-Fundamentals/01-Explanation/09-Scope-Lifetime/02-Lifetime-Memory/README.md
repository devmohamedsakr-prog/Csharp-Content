# Lifetime and Memory Management

## Overview

This category explores how long variables exist in memory and where they're stored. Understanding lifetime and memory allocation is crucial for writing efficient, bug-free C# code and preventing memory leaks.

## Topics Covered

### 1. Stack vs Heap
**File**: `01-Stack-vs-Heap/00-Stack-vs-Heap.md`

Understand the two memory regions where variables and objects are stored, and how they differ fundamentally.

**Key Concepts**:
- Stack memory allocation and deallocation
- Heap memory and garbage collection
- Value types on stack
- Reference types on heap
- Memory addresses and references
- Stack overflow and limitations
- Heap fragmentation
- Method stack frames

**When to Use**: Stack vs Heap understanding helps you:
- Design efficient data structures
- Understand performance implications
- Prevent stack overflows
- Manage object lifetimes
- Make informed design decisions

**Example**:
```csharp
public void MemoryAllocation()
{
    // STACK: Value types
    int x = 5; // Stack
    double y = 3.14; // Stack
    
    // HEAP: Reference types
    var person = new Person(); // Reference on stack, object on heap
    var list = new List<int>(); // Reference on stack, list on heap
    
    // When method returns:
    // - Stack variables (x, y, references) freed immediately
    // - Heap objects eligible for garbage collection
}
```

---

### 2. Variable Shadowing
**File**: `02-Variable-Shadowing/00-Variable-Shadowing.md`

Learn how variables in inner scopes can "shadow" (hide) variables from outer scopes.

**Key Concepts**:
- Shadowing definition and examples
- Block-level shadowing
- Class member shadowing
- Parameter shadowing
- Constructor shadowing
- Lambda and closure shadowing
- LINQ query variable shadowing
- Compiler warnings and errors
- Detection and prevention

**When to Use**: Understanding shadowing helps you:
- Avoid confusing code
- Prevent accidental variable conflicts
- Write clearer, more maintainable code
- Use the new keyword intentionally
- Understand compiler warnings

**Example**:
```csharp
public class Example
{
    private int value = 5; // Class scope
    
    public void Method()
    {
        int value = 10; // Local scope - shadows class field
        Console.WriteLine(value); // 10 (local)
        Console.WriteLine(this.value); // 5 (class field)
    }
}
```

---

### 3. Garbage Collection
**File**: `03-Garbage-Collection/00-Garbage-Collection.md`

Master automatic memory management and how the garbage collector frees heap memory.

**Key Concepts**:
- Reference counting concept
- Reachability and eligibility
- Generational collection (Gen 0, 1, 2)
- Collection frequency
- Finalizers and cleanup
- IDisposable pattern
- Memory leaks in C#
- GC pressure and performance
- Profiling memory usage

**When to Use**: GC knowledge helps you:
- Design efficient memory usage
- Prevent memory leaks
- Implement IDisposable correctly
- Use object pools for high-frequency allocation
- Profile and optimize memory

**Example**:
```csharp
public class GarbageCollectionDemo
{
    public void Demo()
    {
        var obj = new object(); // On heap
        
        // obj goes out of scope - eligible for GC
        obj = null; // Explicitly unreference (optional)
        
        // GC will collect when next collection runs
        GC.Collect(); // Force collection (rarely needed)
    }
}
```

---

## Learning Path

### Beginner
1. Understand Stack vs Heap basics
2. Know the difference between value and reference types
3. Learn allocation and cleanup happens automatically in most cases

### Intermediate
1. Understand memory allocation patterns
2. Learn about garbage collection generations
3. Avoid variable shadowing
4. Implement IDisposable for resources

### Advanced
1. Profile memory usage
2. Optimize allocation patterns
3. Design memory-efficient systems
4. Prevent memory leaks in complex scenarios

---

## Key Concepts Quick Reference

### Stack Characteristics
- **Speed**: Very fast allocation/deallocation
- **Size**: Limited (typically 1-8 MB per thread)
- **Cleanup**: Automatic when out of scope
- **Contents**: Value types, method references
- **Lifetime**: Until end of scope

### Heap Characteristics
- **Speed**: Slower allocation, GC overhead
- **Size**: Large, limited by available RAM
- **Cleanup**: Automatic via garbage collector
- **Contents**: Reference type objects
- **Lifetime**: Until no references exist

### Value vs Reference Types

| Aspect | Value Types | Reference Types |
|--------|-----------|-----------------|
| Stored | Stack | Heap |
| Contains | Actual data | Reference (pointer) |
| Copy | Entire value | Reference only |
| Null | No (except nullable) | Yes |
| Examples | int, double, struct | class, string, array |
| Performance | Faster | Slower |

---

## Memory Patterns

### Pattern 1: Short-Lived Objects
```csharp
// Allocated and freed quickly
for (int i = 0; i < 1_000_000; i++)
{
    var temp = new object(); // Allocated
    // ... use temp ...
} // Freed (Gen 0 collection)
```

### Pattern 2: Long-Lived Objects
```csharp
// Created once, used throughout app
private static Logger _logger = new Logger();

public void DoWork()
{
    _logger.Log("Working");
    // _logger never freed, survives Gen 2 collections
}
```

### Pattern 3: Event Handler Closure
```csharp
// Keeps objects alive as long as event subscribed
public void Subscribe(Publisher pub)
{
    pub.OnEvent += (s, e) =>
    {
        // Closure keeps this object alive
        DoWork();
    };
}
```

---

## Common Memory Issues and Solutions

### Issue 1: Memory Leak - Event Handler

**Problem**:
```csharp
publisher.OnData += Handler; // Subscribed but never unsubscribed
// Memory leak - handler keeps objects alive
```

**Solution**:
```csharp
// Implement IDisposable
public void Dispose()
{
    publisher.OnData -= Handler; // Unsubscribe
}
```

### Issue 2: Excessive Allocation

**Problem**:
```csharp
for (int i = 0; i < 1_000_000; i++)
{
    var data = new byte[1_000_000]; // 1MB allocation each iteration
    // Memory pressure, excessive GC
}
```

**Solution**:
```csharp
var buffer = new byte[1_000_000];
for (int i = 0; i < 1_000_000; i++)
{
    Array.Clear(buffer, 0, buffer.Length);
    // Reuse same buffer
}
```

### Issue 3: Large Object Heap

**Problem**:
```csharp
// Objects >85KB go to separate heap (less efficient)
var largeArray = new byte[100_000]; // 100KB
```

**Solution**:
```csharp
// Consider using ArrayPool for temporary large buffers
using (var handle = MemoryPool<byte>.Shared.Rent(100_000))
{
    var buffer = handle.Memory.Span;
    // Use buffer
} // Returned to pool
```

---

## Best Practices in This Category

1. **Understand Memory Allocation**: Know what goes on stack vs heap
2. **Implement IDisposable**: For resources that need cleanup
3. **Use Using Statements**: Ensure disposal of resources
4. **Avoid Shadowing**: Use distinct names in different scopes
5. **Monitor Memory**: Profile apps to identify issues
6. **Object Pooling**: For frequently allocated short-lived objects
7. **Don't Call GC.Collect()**: Let GC manage memory
8. **Unsubscribe Events**: Prevent event handler memory leaks

---

## Performance Implications

### Stack Allocation (Fast)
```csharp
int x = 5; // ~1 CPU cycle
```

### Heap Allocation (Slower)
```csharp
var obj = new object(); // ~100-1000 CPU cycles
```

### Garbage Collection (Unpredictable)
```csharp
// Hundreds of thousands of cycles when collection runs
// Causes pause in application execution
```

---

## Exercises

### Exercise 1: Stack vs Heap
Identify where each variable is allocated:
```csharp
public void Exercise()
{
    int x = 5; // ?
    var person = new Person { Name = "Alice" }; // ?
    string name = "Bob"; // ? (both reference and object)
    int[] numbers = { 1, 2, 3 }; // ? (both reference and array)
}
```

**Answer**: x=stack, person reference=stack/object=heap, name reference=stack/object=heap, numbers reference=stack/array=heap

### Exercise 2: Memory Leak Prevention
Fix the memory leak:
```csharp
public class Subscriber
{
    public Subscriber(Publisher pub)
    {
        pub.OnData += Handler; // Memory leak!
    }
    
    private void Handler(object sender, EventArgs e) { }
}
```

**Solution**: Implement IDisposable and unsubscribe.

### Exercise 3: Shadowing Resolution
Identify and fix shadowing:
```csharp
public class Example
{
    private int value = 5;
    
    public void Method()
    {
        int value = 10; // Shadowing
        Console.WriteLine(value);
        Console.WriteLine(this.value);
    }
}
```

**Solution**: Rename local variable to `localValue`.

---

## Related Topics

- **Scope Fundamentals**: Where variables can be accessed
- **Closures**: How closures affect variable lifetime
- **Using Declarations**: Managing resource cleanup

---

## Memory Profiling Tools

- **Visual Studio Profiler**: Built-in memory profiling
- **dotTrace**: JetBrains memory profiler
- **PerfView**: Windows Performance Toolkit
- **ANTS Memory Profiler**: RedGate profiler

---

## Summary

Understanding lifetime and memory management is essential for writing efficient C# applications. Key takeaways:

1. **Stack is for short-term**: Value types and references
2. **Heap is for objects**: Reference types with GC cleanup
3. **Avoid shadowing**: Use distinct variable names
4. **Implement IDisposable**: For resource cleanup
5. **Unsubscribe events**: Prevent memory leaks
6. **Profile memory**: Identify and fix issues

Master these concepts to write code that's:
- **Efficient**: Minimal allocations and GC pressure
- **Reliable**: No memory leaks or unintended lifetimes
- **Clear**: No confusing variable shadowing
- **Professional**: Following industry best practices

---

## Next Steps

1. Study each section thoroughly
2. Run the code examples locally
3. Try memory profiling on your applications
4. Apply IDisposable pattern to resource classes
5. Move to **Closures and Advanced** category to see advanced lifetime management

Keep learning!
