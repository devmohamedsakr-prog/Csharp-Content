# Concurrent Collections and Thread Safety

## Overview
Concurrent collections are thread-safe data structures for multi-threaded scenarios without explicit locking.

## ConcurrentBag<T>

### Unordered Thread-Safe Collection
```csharp
public class ConcurrentBagDemo
{
    public static async Task Main()
    {
        var bag = new ConcurrentBag<int>();
        
        // Add items from multiple threads
        var tasks = Enumerable.Range(0, 10).Select(i =>
            Task.Run(() =>
            {
                for (int j = 0; j < 100; j++)
                {
                    bag.Add(i * 100 + j); // Thread-safe add
                }
            })
        ).ToArray();
        
        await Task.WhenAll(tasks);
        
        // Try remove items
        while (bag.TryTake(out int item))
        {
            Console.WriteLine(item);
        }
    }
}

// Producer-Consumer Pattern
public class Producer
{
    private readonly ConcurrentBag<string> _queue = new();
    
    public void ProduceItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _queue.Add($"Item-{i}");
            Thread.Sleep(100);
        }
    }
    
    public bool TryGetItem(out string item)
    {
        return _queue.TryTake(out item);
    }
}
```

## ConcurrentQueue<T>

### FIFO Thread-Safe Collection
```csharp
public class ConcurrentQueueDemo
{
    public static async Task Main()
    {
        var queue = new ConcurrentQueue<int>();
        
        // Producer threads
        var producers = Enumerable.Range(0, 3).Select(producer =>
            Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    queue.Enqueue(producer * 100 + i);
                    Console.WriteLine($"Produced: {producer * 100 + i}");
                    Thread.Sleep(100);
                }
            })
        ).ToArray();
        
        // Consumer threads
        var consumers = Enumerable.Range(0, 2).Select(consumer =>
            Task.Run(async () =>
            {
                while (queue.Count > 0 || !Task.WaitAll(producers, 1000))
                {
                    if (queue.TryDequeue(out int item))
                    {
                        Console.WriteLine($"Consumed by {consumer}: {item}");
                    }
                    await Task.Delay(50);
                }
            })
        ).ToArray();
        
        await Task.WhenAll(producers.Concat(consumers).ToArray());
    }
}
```

## ConcurrentStack<T>

### LIFO Thread-Safe Collection
```csharp
public class ConcurrentStackDemo
{
    public static void Main()
    {
        var stack = new ConcurrentStack<string>();
        
        // Multiple threads push
        Parallel.For(0, 10, i =>
        {
            stack.Push($"Item-{i}");
        });
        
        Console.WriteLine($"Items in stack: {stack.Count}");
        
        // Multiple threads pop
        Parallel.For(0, 5, i =>
        {
            if (stack.TryPop(out string item))
            {
                Console.WriteLine($"Popped: {item}");
            }
        });
        
        // Peek at top without removing
        if (stack.TryPeek(out string top))
        {
            Console.WriteLine($"Top item: {top}");
        }
    }
}
```

## ConcurrentDictionary<TKey, TValue>

### Thread-Safe Dictionary
```csharp
public class ConcurrentDictionaryDemo
{
    public static async Task Main()
    {
        var dict = new ConcurrentDictionary<int, string>();
        
        // Add items safely
        dict.TryAdd(1, "One");
        dict.TryAdd(2, "Two");
        dict.TryAdd(3, "Three");
        
        // Update with AddOrUpdate
        dict.AddOrUpdate(1, "One-Updated", (key, oldValue) => oldValue + "-Modified");
        
        // Get value
        if (dict.TryGetValue(2, out string value))
        {
            Console.WriteLine($"Value: {value}");
        }
        
        // Remove item
        dict.TryRemove(3, out string removed);
        
        // Concurrent update from multiple threads
        var tasks = Enumerable.Range(0, 10).Select(i =>
            Task.Run(() =>
            {
                dict.AddOrUpdate("counter", 1, (k, v) => v + 1);
            })
        ).ToArray();
        
        await Task.WhenAll(tasks);
        
        Console.WriteLine($"Counter: {dict["counter"]}"); // 10
    }
}

// Cache with TTL
public class CacheWithExpiration
{
    private class CacheItem
    {
        public object Value { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
    
    private readonly ConcurrentDictionary<string, CacheItem> _cache = new();
    
    public void Set(string key, object value, TimeSpan ttl)
    {
        var item = new CacheItem
        {
            Value = value,
            ExpirationTime = DateTime.UtcNow.Add(ttl)
        };
        _cache.AddOrUpdate(key, item, (k, old) => item);
    }
    
    public bool TryGet(string key, out object value)
    {
        if (_cache.TryGetValue(key, out var item))
        {
            if (DateTime.UtcNow < item.ExpirationTime)
            {
                value = item.Value;
                return true;
            }
            else
            {
                _cache.TryRemove(key, out _); // Remove expired
            }
        }
        value = null;
        return false;
    }
}
```

## BlockingCollection<T>

### For Producer-Consumer Coordination
```csharp
public class BlockingCollectionDemo
{
    public static async Task Main()
    {
        var collection = new BlockingCollection<int>(10); // Bounded capacity
        
        // Producer
        var producer = Task.Run(() =>
        {
            for (int i = 0; i < 20; i++)
            {
                collection.Add(i); // Blocks if full
                Console.WriteLine($"Produced: {i}");
                Thread.Sleep(100);
            }
            collection.CompleteAdding(); // Signal end
        });
        
        // Consumer
        var consumer = Task.Run(() =>
        {
            foreach (int item in collection.GetConsumingEnumerable())
            {
                Console.WriteLine($"Consumed: {item}");
                Thread.Sleep(150);
            }
        });
        
        await Task.WhenAll(producer, consumer);
    }
}

// Multiple consumers
public class MultiConsumerDemo
{
    public static async Task Main()
    {
        var collection = new BlockingCollection<string>();
        
        // Producer
        var producer = Task.Run(async () =>
        {
            for (int i = 0; i < 10; i++)
            {
                collection.Add($"Item-{i}");
                await Task.Delay(100);
            }
            collection.CompleteAdding();
        });
        
        // Multiple consumers
        var consumers = Enumerable.Range(0, 3).Select(consumerId =>
            Task.Run(() =>
            {
                foreach (var item in collection.GetConsumingEnumerable())
                {
                    Console.WriteLine($"Consumer {consumerId}: {item}");
                }
            })
        ).ToArray();
        
        await Task.WhenAll(new[] { producer }.Concat(consumers).ToArray());
    }
}
```

## ConcurrentBag vs List for Parallel Work

### Performance Comparison
```csharp
public class PerformanceComparison
{
    public static void Main()
    {
        const int iterations = 100000;
        
        // Lock-based approach
        var lockList = new List<int>();
        var lockObj = new object();
        
        var stopwatch = Stopwatch.StartNew();
        Parallel.For(0, iterations, i =>
        {
            lock (lockObj)
            {
                lockList.Add(i);
            }
        });
        stopwatch.Stop();
        Console.WriteLine($"Lock-based: {stopwatch.ElapsedMilliseconds}ms");
        
        // ConcurrentBag approach
        var bag = new ConcurrentBag<int>();
        stopwatch.Restart();
        Parallel.For(0, iterations, i => bag.Add(i));
        stopwatch.Stop();
        Console.WriteLine($"ConcurrentBag: {stopwatch.ElapsedMilliseconds}ms");
        
        // Usually ConcurrentBag is faster
    }
}
```

## Best Practices

1. **Use Appropriate Collection Type**
```csharp
// Unordered, no duplicates needed
var bag = new ConcurrentBag<int>();

// FIFO queue
var queue = new ConcurrentQueue<int>();

// LIFO stack
var stack = new ConcurrentStack<int>();

// Key-value pairs
var dict = new ConcurrentDictionary<string, int>();

// Producer-Consumer with blocking
var collection = new BlockingCollection<int>();
```

2. **Use TryXxx Methods**
```csharp
// Good: Handle failure gracefully
if (queue.TryDequeue(out int item))
{
    Process(item);
}

// Good: Add with condition
if (dict.TryAdd(key, value))
{
    Console.WriteLine("Added successfully");
}
```

3. **Complete Adding When Done**
```csharp
// Good: Signal completion
var collection = new BlockingCollection<int>();
// Add items...
collection.CompleteAdding();

// Consumer knows when done
foreach (var item in collection.GetConsumingEnumerable())
{
    Process(item);
}
```

## Common Mistakes

1. **Lock Inside Concurrent Collection**
```csharp
// Bad: Defeats purpose
var bag = new ConcurrentBag<int>();
lock (_lock)
{
    bag.Add(value); // Already thread-safe!
}

// Good: Use directly
bag.Add(value);
```

2. **Checking Count and Then Acting**
```csharp
// Bad: Race condition
var queue = new ConcurrentQueue<int>();
if (queue.Count > 0) // Count changed between check and dequeue
{
    queue.TryDequeue(out int item);
}

// Good: TryDequeue handles it
if (queue.TryDequeue(out int item))
{
    // Process
}
```

3. **Not Signaling Completion**
```csharp
// Bad: Consumer waits forever
var collection = new BlockingCollection<int>();
foreach (var item in collection.GetConsumingEnumerable())
{
    // Waits indefinitely if never called CompleteAdding
}

// Good: Signal when done
collection.CompleteAdding();
```

## Quick Summary
- ConcurrentBag for unordered items
- ConcurrentQueue for FIFO
- ConcurrentStack for LIFO
- ConcurrentDictionary for key-value
- BlockingCollection for producer-consumer
- No external locking needed
- Use TryXxx methods for safe access
- Call CompleteAdding() when done
- Thread-safe by default
- Better performance than lock for high contention

## Resources
- System.Collections.Concurrent namespace
- Thread-Safe Collections
- Producer-Consumer Pattern
- Parallel Programming Best Practices
