# IDisposable Pattern

## Overview
IDisposable is the .NET interface for managing unmanaged resources like file handles, database connections, and memory. Understanding the IDisposable pattern is essential for writing reliable code.

## What is IDisposable

```csharp
public interface IDisposable {
    void Dispose();
}
```

Dispose() releases unmanaged resources explicitly.

## Why IDisposable Matters

### Without IDisposable
```csharp
// BAD - Resource leak
public class FileProcessor {
    private FileStream stream;
    
    public void ProcessFile(string path) {
        stream = new FileStream(path, FileMode.Open);
        // If exception occurs, stream never closes!
    }
}
```

### With IDisposable
```csharp
// GOOD - Resource cleanup guaranteed
public class FileProcessor : IDisposable {
    private FileStream stream;
    
    public void ProcessFile(string path) {
        stream = new FileStream(path, FileMode.Open);
    }
    
    public void Dispose() {
        stream?.Dispose();  // Always cleaned up
    }
}

// Usage
using (var processor = new FileProcessor()) {
    processor.ProcessFile("data.txt");
}  // Dispose called automatically
```

## Basic IDisposable Implementation

### Simplest Pattern
```csharp
public class SimpleResource : IDisposable {
    private StreamReader reader;
    
    public SimpleResource(string path) {
        reader = new StreamReader(path);
    }
    
    public string ReadAll() => reader.ReadToEnd();
    
    public void Dispose() {
        reader?.Dispose();
    }
}

// Usage
using (var resource = new SimpleResource("file.txt")) {
    string content = resource.ReadAll();
}
```

## Proper IDisposable Pattern (Recommended)

### Best Practice Pattern
```csharp
public class ProperResource : IDisposable {
    private StreamReader reader;
    private bool disposed = false;
    
    public ProperResource(string path) {
        reader = new StreamReader(path);
    }
    
    public string ReadAll() {
        ThrowIfDisposed();
        return reader.ReadToEnd();
    }
    
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing) {
        if (!disposed) {
            if (disposing) {
                reader?.Dispose();  // Managed resources
            }
            // Unmanaged resources cleanup here
            disposed = true;
        }
    }
    
    ~ProperResource() {
        Dispose(false);
    }
    
    private void ThrowIfDisposed() {
        if (disposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}
```

## Managed vs Unmanaged Resources

### Managed Resources
Created by .NET, garbage collector handles:
- Objects, strings, arrays
- Other .NET objects

```csharp
public void Dispose() {
    managedResource?.Dispose();  // Call Dispose on managed resources
}
```

### Unmanaged Resources
Not managed by .NET, need manual cleanup:
- File handles
- Database connections
- Window handles
- Network sockets

```csharp
public void Dispose() {
    // Clean up unmanaged resources
    CloseHandle(fileHandle);
    ReleaseMemory(nativePointer);
}
```

## Disposing Pattern Steps

### Step 1: Implement IDisposable
```csharp
public class DatabaseConnection : IDisposable {
    public void Dispose() {
        // Cleanup
    }
}
```

### Step 2: Track Disposed State
```csharp
public class DatabaseConnection : IDisposable {
    private bool disposed = false;
    
    public void Dispose() {
        Dispose(true);
    }
}
```

### Step 3: Separate Managed and Unmanaged Cleanup
```csharp
public class DatabaseConnection : IDisposable {
    private bool disposed = false;
    
    protected virtual void Dispose(bool disposing) {
        if (!disposed) {
            if (disposing) {
                // Clean up managed resources
                managedResource?.Dispose();
            }
            
            // Clean up unmanaged resources
            CloseNativeHandle();
            
            disposed = true;
        }
    }
    
    public void Dispose() {
        Dispose(true);
    }
}
```

### Step 4: Add Finalizer
```csharp
public class DatabaseConnection : IDisposable {
    ~DatabaseConnection() {
        Dispose(false);  // Cleanup as fallback
    }
    
    protected virtual void Dispose(bool disposing) {
        if (!disposed) {
            if (disposing) {
                managedResource?.Dispose();
            }
            disposed = true;
        }
    }
    
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);  // Tell GC not to call finalizer
    }
}
```

## Using IDisposable

### With Using Statement
```csharp
public void ProcessData() {
    using (var conn = new DatabaseConnection("server")) {
        conn.Execute("SELECT * FROM Users");
    }  // Dispose called automatically
}
```

### With Using Declaration (C# 8+)
```csharp
public void ProcessData() {
    using var conn = new DatabaseConnection("server");
    conn.Execute("SELECT * FROM Users");
}  // Dispose called at end of scope
```

### Manual Disposal
```csharp
public void ProcessData() {
    var conn = new DatabaseConnection("server");
    try {
        conn.Execute("SELECT * FROM Users");
    } finally {
        conn?.Dispose();
    }
}
```

## Common Patterns

### Pattern 1: Simple Cleanup
```csharp
public class FileWriter : IDisposable {
    private StreamWriter writer;
    
    public FileWriter(string path) {
        writer = new StreamWriter(path);
    }
    
    public void WriteLine(string line) => writer.WriteLine(line);
    
    public void Dispose() {
        writer?.Dispose();
    }
}
```

### Pattern 2: Multiple Resources
```csharp
public class DataProcessor : IDisposable {
    private StreamReader reader;
    private StreamWriter writer;
    private bool disposed = false;
    
    public DataProcessor(string input, string output) {
        reader = new StreamReader(input);
        writer = new StreamWriter(output);
    }
    
    protected virtual void Dispose(bool disposing) {
        if (!disposed) {
            if (disposing) {
                reader?.Dispose();
                writer?.Dispose();
            }
            disposed = true;
        }
    }
    
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    ~DataProcessor() => Dispose(false);
}
```

### Pattern 3: Wrapping Unmanaged Code
```csharp
public class NativeResource : IDisposable {
    private IntPtr nativeHandle;
    private bool disposed = false;
    
    public NativeResource() {
        nativeHandle = AllocateNativeMemory();
    }
    
    protected virtual void Dispose(bool disposing) {
        if (!disposed) {
            if (disposing) {
                // Managed cleanup
            }
            
            if (nativeHandle != IntPtr.Zero) {
                FreeNativeMemory(nativeHandle);  // Unmanaged cleanup
                nativeHandle = IntPtr.Zero;
            }
            
            disposed = true;
        }
    }
    
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    ~NativeResource() => Dispose(false);
}
```

## Disposing Collections

```csharp
public class ResourceCollection : IDisposable {
    private List<IDisposable> resources = new List<IDisposable>();
    
    public void Add(IDisposable resource) {
        resources.Add(resource);
    }
    
    public void Dispose() {
        // Dispose all resources
        foreach (var resource in resources) {
            resource?.Dispose();
        }
        resources.Clear();
    }
}
```

## Preventing Double Dispose

```csharp
public class SafeResource : IDisposable {
    private bool disposed = false;
    private StreamWriter writer;
    
    public void WriteLine(string line) {
        if (disposed) {
            throw new ObjectDisposedException(GetType().Name);
        }
        writer.WriteLine(line);
    }
    
    public void Dispose() {
        if (!disposed) {
            writer?.Dispose();
            disposed = true;
        }
    }
}
```

## IAsyncDisposable (C# 8+)

For async cleanup:

```csharp
public class AsyncResource : IAsyncDisposable {
    private DatabaseConnection connection;
    
    public async ValueTask DisposeAsync() {
        if (connection != null) {
            await connection.CloseAsync();
        }
    }
}

// Usage
await using (var resource = new AsyncResource()) {
    // Use resource
}  // Async dispose called
```

## Best Practices

✓ Always implement Dispose pattern correctly
```csharp
public class MyResource : IDisposable {
    private bool disposed = false;
    
    protected virtual void Dispose(bool disposing) {
        if (!disposed) {
            if (disposing) {
                managedResource?.Dispose();
            }
            disposed = true;
        }
    }
    
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    ~MyResource() => Dispose(false);
}
```

✓ Use using statements
```csharp
using (var resource = new MyResource()) {
    // Use resource
}
```

✓ Check disposed state
```csharp
public void Operation() {
    if (disposed) throw new ObjectDisposedException(...);
    // Safe operation
}
```

✓ Document what needs disposal
```csharp
/// <summary>
/// Remember to call Dispose() or use 'using' statement
/// </summary>
public class MyResource : IDisposable { }
```

## Anti-Patterns

❌ No check for disposed state
```csharp
public void Operation() {
    // What if already disposed?
}
```

❌ Forgetting GC.SuppressFinalize
```csharp
public void Dispose() {
    Dispose(true);
    // Missing: GC.SuppressFinalize(this);
}
```

❌ Not calling base.Dispose in derived classes
```csharp
public class Derived : BaseResource {
    public override void Dispose() {
        // Missing: base.Dispose();
    }
}
```

❌ Throwing in finalizer
```csharp
~MyResource() {
    // Never throw in finalizer!
    throw new Exception();
}
```

## Checklist for IDisposable Implementation

- [ ] Implement IDisposable interface
- [ ] Add private `disposed` field
- [ ] Implement Dispose(bool) method
- [ ] Call `Dispose(true)` from Dispose()
- [ ] Call `GC.SuppressFinalize(this)` from Dispose()
- [ ] Implement finalizer calling `Dispose(false)`
- [ ] Check disposed state in public methods
- [ ] Throw ObjectDisposedException if accessed after dispose
- [ ] Document disposal requirement
- [ ] Test with using statement

## Summary

- IDisposable manages resource cleanup
- Implement Dispose pattern with managed/unmanaged separation
- Use `using` statements to ensure cleanup
- Check disposed state in public methods
- Implement finalizer as safety net
- Call GC.SuppressFinalize in Dispose()
- Never throw in finalizers
- Always dispose nested IDisposable objects

---

## Next Steps

1. Study Best Practices
2. Learn Common Mistakes
3. Master Interview Questions
4. Review All Patterns
