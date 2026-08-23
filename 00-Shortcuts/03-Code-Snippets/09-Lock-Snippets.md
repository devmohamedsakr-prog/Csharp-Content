# Lock & Thread-Safe Snippets

Generate thread synchronization code with built-in snippets.

## lock - Lock Statement

**Shortcut:** `lock` + Tab

**Generates:**
```csharp
lock (this)
{
}
```

**Placeholders:**
- this: Replace with object to lock on

**Usage:**
```csharp
lock → Tab
// Replace lock object
```

**Example:**
```csharp
private object _lockObject = new object();

public void IncrementCounter()
{
    lock (_lockObject)
    {
        _counter++;
    }
}
```

---

## Simple Lock Example

**Pattern:**
```csharp
private int _count = 0;
private object _lock = new object();

public void Increment()
{
    lock (_lock)
    {
        _count++;
    }
}

public int GetCount()
{
    lock (_lock)
    {
        return _count;
    }
}
```

---

## Lock with Static Object

**Pattern:**
```csharp
private static object _staticLock = new object();
private static int _sharedCounter = 0;

public static void IncrementShared()
{
    lock (_staticLock)
    {
        _sharedCounter++;
    }
}
```

---

## Locking Collections

**Pattern:**
```csharp
private List<string> _items = new List<string>();
private object _lock = new object();

public void AddItem(string item)
{
    lock (_lock)
    {
        _items.Add(item);
    }
}

public List<string> GetAllItems()
{
    lock (_lock)
    {
        return new List<string>(_items);  // Return copy
    }
}

public void RemoveItem(string item)
{
    lock (_lock)
    {
        _items.Remove(item);
    }
}
```

---

## Nested Locks (Caution)

**Pattern:**
```csharp
private object _lock1 = new object();
private object _lock2 = new object();

public void Operation()
{
    lock (_lock1)
    {
        // Do something
        lock (_lock2)
        {
            // Do something else
        }
    }
}
```

**Warning:** Risk of deadlock if locks acquired in different order elsewhere

---

## ReaderWriterLockSlim - Better for Read-Heavy Operations

**Pattern:**
```csharp
private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
private string _data = "";

public string ReadData()
{
    _rwLock.EnterReadLock();
    try
    {
        return _data;
    }
    finally
    {
        _rwLock.ExitReadLock();
    }
}

public void WriteData(string value)
{
    _rwLock.EnterWriteLock();
    try
    {
        _data = value;
    }
    finally
    {
        _rwLock.ExitWriteLock();
    }
}
```

---

## Mutex - Inter-Process Locking

**Pattern:**
```csharp
private Mutex _mutex = new Mutex();

public void CriticalSection()
{
    _mutex.WaitOne();
    try
    {
        // Critical section
    }
    finally
    {
        _mutex.ReleaseMutex();
    }
}
```

---

## Semaphore - Limiting Concurrent Access

**Pattern:**
```csharp
private Semaphore _semaphore = new Semaphore(3, 3);  // Max 3 threads

public void LimitedResource()
{
    _semaphore.WaitOne();
    try
    {
        // Only 3 threads can execute this at once
    }
    finally
    {
        _semaphore.Release();
    }
}
```

---

## using Statement with Lock

**Pattern:**
```csharp
private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

public string ReadData()
{
    using (_rwLock.EnterReadLock())
    {
        return _data;
    }
}
```

---

## Interlocked - Atomic Operations

**Pattern:**
```csharp
private int _counter = 0;

public void Increment()
{
    Interlocked.Increment(ref _counter);
}

public int GetValue()
{
    return Interlocked.Read(ref _counter);
}

public void SetValue(int value)
{
    Interlocked.Exchange(ref _counter, value);
}
```

**Examples:**
```csharp
// Atomic increment
Interlocked.Increment(ref count);

// Atomic decrement
Interlocked.Decrement(ref count);

// Atomic add
Interlocked.Add(ref counter, 10);

// Atomic compare and exchange
Interlocked.CompareExchange(ref value, 100, 50);
```

---

## Volatile - Memory Visibility

**Pattern:**
```csharp
private volatile bool _shouldStop = false;

public void WorkerThread()
{
    while (!_shouldStop)
    {
        DoWork();
    }
}

public void SignalStop()
{
    _shouldStop = true;
}
```

---

## SynchronizationContext

**Pattern:**
```csharp
public async void ButtonClick()
{
    var context = SynchronizationContext.Current;
    
    await Task.Run(() =>
    {
        DoBackgroundWork();
    });
    
    // Posts back to UI thread
    context.Post(_ =>
    {
        UpdateUI();
    }, null);
}
```

---

## Concurrent Collections

**Instead of Lock + Collection:**
```csharp
// Old way: Lock + List
private object _lock = new object();
private List<string> _items = new List<string>();

// New way: Concurrent collection
private ConcurrentBag<string> _items = new ConcurrentBag<string>();

public void AddItem(string item)
{
    _items.Add(item);  // Already thread-safe
}
```

**Concurrent Collections:**
- `ConcurrentBag<T>` - Unordered, thread-safe
- `ConcurrentQueue<T>` - FIFO queue
- `ConcurrentStack<T>` - LIFO stack
- `ConcurrentDictionary<K,V>` - Thread-safe dictionary

---

## Quick Reference

| Synchronization | Use Case |
|-----------------|----------|
| `lock` | Simple mutual exclusion |
| `ReaderWriterLockSlim` | Many readers, few writers |
| `Mutex` | Inter-process synchronization |
| `Semaphore` | Limit concurrent access |
| `Interlocked` | Atomic operations |
| `volatile` | Memory visibility |
| `Concurrent*` | Thread-safe collections |

---

## Best Practices

- Keep lock scope small
- Avoid nested locks
- Use `try-finally` to ensure unlock
- Consider concurrent collections
- Use `volatile` for flags
- Use `Interlocked` for counters
- Avoid busy waiting (spinlock)
- Document locking strategy

