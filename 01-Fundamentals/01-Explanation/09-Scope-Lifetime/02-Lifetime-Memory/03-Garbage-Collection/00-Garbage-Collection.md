# Garbage Collection in C#

## Overview

Garbage Collection (GC) is an automatic memory management mechanism that frees heap memory used by objects that are no longer referenced. Understanding GC helps write efficient code and avoid memory issues.

## How Garbage Collection Works

### Reference Counting (Conceptual)

```csharp
public class GCDemo
{
    public void Demo()
    {
        // Reference count = 1
        var person = new Person { Name = "Alice" };
        
        // Reference count = 2
        var person2 = person;
        
        person = null; // Reference count = 1
        person2 = null; // Reference count = 0 - eligible for GC
        
        // The Person object is now garbage and will be collected
    }
}

public class Person
{
    public string Name { get; set; }
}
```

### When Objects Become Eligible for Garbage Collection

```csharp
public class GCEligibility
{
    public void Demo()
    {
        var list = new List<int> { 1, 2, 3 };
        
        // list is in scope here, not eligible
        Console.WriteLine(list.Count);
        
        // After method ends, list reference destroyed
        // List object on heap becomes eligible for GC
    }
    
    public void MultipleReferences()
    {
        var obj1 = new object();
        var obj2 = obj1;
        var obj3 = obj1;
        
        obj1 = null; // Still 2 references
        obj2 = null; // Still 1 reference
        obj3 = null; // Now 0 references - eligible for GC
    }
}
```

### Unreachable Objects

```csharp
public class UnreachableObjects
{
    private List<object> _items = new List<object>();
    
    public void AddAndRemove()
    {
        var item = new object();
        _items.Add(item);
        
        // item reference goes out of scope
        // But object is still referenced in _items
        // NOT eligible for GC yet
    }
    
    public void Demonstrate()
    {
        AddAndRemove();
        _items.Clear(); // Now objects are unreachable
        // All removed objects eligible for GC
    }
}
```

## Garbage Collection Generations

### Generation 0, 1, and 2

```csharp
public class GenerationDemo
{
    public void Demo()
    {
        // Short-lived objects - Gen 0
        for (int i = 0; i < 100; i++)
        {
            var temp = new object(); // Created and discarded
            // Most of these will be collected in Gen 0 GC
        }
        
        // Medium-lived objects - Gen 1
        var mediumLived = new List<object>();
        for (int i = 0; i < 10; i++)
        {
            mediumLived.Add(new object());
        }
        // These survive Gen 0 GC, move to Gen 1
        
        // Long-lived objects - Gen 2
        var longLived = new List<object>(); // Persists for app lifetime
        // Survives multiple GCs, moves to Gen 2
    }
}
```

### Collection Frequency

```csharp
public class CollectionFrequency
{
    // Gen 0 collected most frequently (fast)
    // Gen 1 collected less frequently (medium)
    // Gen 2 collected least frequently (slow but thorough)
    
    public void Demonstrate()
    {
        // Gen 0 collection is very fast
        for (int i = 0; i < 1_000_000; i++)
        {
            var temp = new object(); // Lightweight allocation
        }
        
        // One Gen 0 collection freed most of these
    }
}
```

## Finalizers and Object Cleanup

### Finalizer (Destructor)

```csharp
public class FileWrapper : IDisposable
{
    private IntPtr _fileHandle;
    private bool _disposed = false;
    
    ~FileWrapper() // Finalizer - called by GC
    {
        Cleanup();
    }
    
    private void Cleanup()
    {
        if (!_disposed)
        {
            // Close file handle
            _disposed = true;
        }
    }
    
    public void Dispose() // IDisposable
    {
        Cleanup();
        GC.SuppressFinalize(this); // Tell GC finalizer already ran
    }
}
```

### Finalizer Overhead

```csharp
public class FinalizerOverhead
{
    public void Demo()
    {
        // Objects with finalizers have overhead
        var file = new FileWrapper();
        // During GC:
        // 1. Object marked for collection
        // 2. Finalizer is queued
        // 3. Finalizer thread runs it
        // 4. Object finally collected
        
        // This takes more time than object without finalizer
    }
}

public class FileWrapper
{
    ~FileWrapper()
    {
        // Cleanup
    }
}
```

## Controlling Garbage Collection

### Explicit GC.Collect()

```csharp
public class ExplicitGC
{
    public void Demo()
    {
        var list = new List<object>();
        for (int i = 0; i < 1_000_000; i++)
        {
            list.Add(new object());
        }
        
        list.Clear(); // Objects eligible for collection
        
        // Force garbage collection (not recommended in most cases)
        GC.Collect(); // Collect all generations
        GC.WaitForPendingFinalizers(); // Wait for finalizers
        
        Console.WriteLine("GC completed");
    }
    
    public void Demonstrate()
    {
        // RARELY NEEDED:
        // GC is already optimized
        // Manual GC usually hurts performance
        
        // Exceptions: Server apps during maintenance window, benchmarking
    }
}
```

### GC.GetTotalMemory()

```csharp
public class MemoryMonitoring
{
    public void Monitor()
    {
        long before = GC.GetTotalMemory(false);
        Console.WriteLine($"Memory before: {before} bytes");
        
        var list = new List<byte[]>();
        for (int i = 0; i < 100; i++)
        {
            list.Add(new byte[1_000_000]); // Allocate 1MB each
        }
        
        long after = GC.GetTotalMemory(false);
        Console.WriteLine($"Memory after: {after} bytes");
        
        list.Clear();
        
        long after_clear = GC.GetTotalMemory(true); // Force collection
        Console.WriteLine($"Memory after clear: {after_clear} bytes");
    }
}
```

## IDisposable Pattern

### Implementing Proper Cleanup

```csharp
public class Resource : IDisposable
{
    private IntPtr _unManagedResource;
    private bool _disposed = false;
    
    // Finalizer for safety net
    ~Resource()
    {
        Dispose(false);
    }
    
    // IDisposable implementation
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Tell GC we already cleaned up
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (disposing)
        {
            // Clean up managed resources
            // Dispose other IDisposable objects
        }
        
        // Clean up unmanaged resources
        if (_unManagedResource != IntPtr.Zero)
        {
            // Release unmanaged resource
            _unManagedResource = IntPtr.Zero;
        }
        
        _disposed = true;
    }
    
    // Throw if someone tries to use after disposal
    protected void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);
    }
}
```

### Using Statement (Automatic Disposal)

```csharp
public class ResourceUsage
{
    public void Demo()
    {
        // Old way - manual try-finally
        var file = File.OpenRead("data.txt");
        try
        {
            // Use file
        }
        finally
        {
            file?.Dispose();
        }
        
        // Modern way - using statement (C# 8.0+)
        using var file2 = File.OpenRead("data.txt");
        // Use file2
        // file2.Dispose() automatically called at end of method
        
        // Try-using combo (older C#)
        using (var file3 = File.OpenRead("data.txt"))
        {
            // Use file3
        } // file3.Dispose() called here
    }
}
```

## Memory Leaks in C#

### Holding Unnecessary References

```csharp
public class MemoryLeakExample
{
    private List<object> _cache = new List<object>();
    
    public void BadCaching()
    {
        var data = new object();
        _cache.Add(data);
        
        // Even after data goes out of scope,
        // it's still referenced in _cache
        // GC can't collect it
        
        // This is a memory leak if _cache isn't cleared
    }
    
    public void FixedCaching()
    {
        _cache.Clear(); // Explicitly clear references
        
        // Now unreachable objects can be collected
    }
}
```

### Event Handler Leaks

```csharp
public class EventLeak
{
    private List<Publisher> _publishers = new List<Publisher>();
    
    public void BadEventHandling()
    {
        var pub = new Publisher();
        pub.OnData += Handler; // Subscribe
        _publishers.Add(pub);
        
        // Even if pub goes out of scope,
        // it's referenced by _publishers
        // AND event handler is referenced by pub
        // MEMORY LEAK!
    }
    
    public void FixedEventHandling()
    {
        var pub = new Publisher();
        pub.OnData += Handler; // Subscribe
        
        // Later: Unsubscribe
        pub.OnData -= Handler;
        
        // Now pub can be collected if no other references
    }
    
    private void Handler(object sender, EventArgs e)
    {
        Console.WriteLine("Event fired");
    }
}

public class Publisher
{
    public event EventHandler OnData;
}
```

## GC Performance Considerations

### Allocation Efficiency

```csharp
public class AllocationEfficiency
{
    // INEFFICIENT: Frequent allocations in tight loop
    public void BadApproach()
    {
        for (int i = 0; i < 1_000_000; i++)
        {
            var temp = new List<int>();
            temp.Add(i);
            // temp destroyed, GC pressure
        }
    }
    
    // BETTER: Reuse allocations
    public void GoodApproach()
    {
        var temp = new List<int>();
        for (int i = 0; i < 1_000_000; i++)
        {
            temp.Clear();
            temp.Add(i);
            // Reuse same list, less GC pressure
        }
    }
    
    // BEST: Use value types or collections built for performance
    public void BestApproach()
    {
        var buffer = new int[1000];
        int count = 0;
        
        for (int i = 0; i < 1_000_000; i++)
        {
            if (count >= buffer.Length)
            {
                count = 0;
            }
            buffer[count++] = i;
            // Minimal allocations
        }
    }
}
```

## Best Practices

1. **Let GC Work**: Don't call GC.Collect() unless truly necessary
2. **Implement IDisposable**: For objects managing resources
3. **Use Using Statements**: Ensure disposal of resources
4. **Avoid Finalizers**: Unless managing unmanaged resources
5. **Cache Wisely**: Clear caches to allow collection
6. **Unsubscribe Events**: Prevent event handler memory leaks
7. **Profile Memory**: Identify actual memory issues before optimizing
8. **Prefer Value Types**: For short-lived small objects
9. **Array Pooling**: For temporary large allocations

## Common Mistakes

1. **Calling GC.Collect()**: Disrupts GC optimization
2. **Finalizers Without Need**: Adds unnecessary overhead
3. **Forgetting IDisposable**: For resource-holding objects
4. **Event Leak**: Not unsubscribing from events
5. **Cache Bloat**: Unbounded caches causing memory issues
6. **Large Object Heap**: Allocating very large objects (>85KB)

## Summary

Garbage collection in C# automatically manages heap memory, freeing objects no longer referenced. The generational approach optimizes for common patterns where young objects die quickly. Understanding GC helps write efficient code - proper use of IDisposable, avoiding memory leaks, and minimizing allocations in performance-critical sections. Most of the time, GC "just works" - explicit GC calls and finalizers should be rare exceptions, not the rule.
