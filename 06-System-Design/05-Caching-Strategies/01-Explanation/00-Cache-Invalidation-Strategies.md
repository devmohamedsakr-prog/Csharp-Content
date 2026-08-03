# Cache Invalidation Strategies

## The Challenge

"There are only two hard things in Computer Science: cache invalidation and naming things." - Phil Karlton

Cache invalidation is deciding when cached data becomes stale and needs to be refreshed.

## Why Invalidation Matters

- **Consistency**: Keep cache synchronized with source
- **Correctness**: Ensure users see recent data
- **Performance**: Balance freshness vs cache hit rate
- **Complexity**: Most difficult aspect of caching

## Cache Invalidation Strategies

### 1. TTL (Time-To-Live)

**Concept**: Cache entry expires after fixed duration

**Implementation**:
```csharp
cache.Set(key, value, TimeSpan.FromMinutes(5));
// Expires after 5 minutes automatically
```

**Pros**:
- Simple to implement
- No tracking needed
- Automatic cleanup

**Cons**:
- Stale data served until expiration
- Wasted space caching unused items
- Cold cache on expiration (miss spike)

**Best For**:
- Non-critical data (recommendations, trending)
- Slowly changing data (user profiles)
- Acceptable staleness window

**Duration Examples**:
```
User profile:      5-10 minutes (low change frequency)
Product pricing:   1-5 minutes (may change)
Trending topics:   30-60 seconds (frequent changes)
Search results:    5 minutes (stable)
Session data:      30 minutes (standard session)
```

### 2. Active/Explicit Invalidation

**Concept**: Actively remove cache when data changes

**Implementation**:
```csharp
// When data updated
user.Name = "John";
dbContext.SaveChanges();
cache.Remove($"user:{userId}"); // Explicit invalidation
```

**Pros**:
- Fresh data immediately after update
- No stale window

**Cons**:
- Must track all keys affected
- Complex with multiple caches
- Can be error-prone

**Best For**:
- Critical data (payments, accounts)
- Frequently updated data
- Multi-cache systems

**Challenge**: Knowing all cache keys affected
```
Example: User profile changed
- Must invalidate: user:{id}, user-profile:{id}, 
  user-summary:{id}, all-users-list, etc.
```

### 3. Event-Based Invalidation

**Concept**: Subscribe to data change events, invalidate accordingly

**Implementation**:
```csharp
// Publisher: When user updated
user.Name = "John";
await dbContext.SaveChangesAsync();
await eventBus.PublishAsync(new UserUpdatedEvent(userId));

// Subscriber: Cache listener
eventBus.Subscribe<UserUpdatedEvent>(async evt => {
    await cache.RemoveAsync($"user:{evt.UserId}");
});
```

**Pros**:
- Decoupled from business logic
- Flexible, can add new listeners
- Handles cascading invalidations

**Cons**:
- Event bus complexity
- Potential delays
- Event delivery guarantees matter

**Best For**:
- Microservices architecture
- Multiple cache layers
- Complex invalidation rules

**Example: Multi-Level Invalidation**:
```
Order.Status changed:
- Event: OrderStatusChanged
- Invalidate: order:{id}, user-orders:{userId}, 
  dashboard-orders, trending-orders, etc.
```

### 4. Tag-Based Invalidation

**Concept**: Group related cache entries with tags, invalidate all with tag

**Implementation**:
```csharp
// Redis example
cache.StringSet($"order:{orderId}", serialized, tags: ["orders", $"user:{userId}"]);

// Invalidate all orders for user
cache.InvalidateByTag($"user:{userId}");
```

**Pros**:
- Batch invalidate related entries
- Prevents missing keys
- Flexible grouping

**Cons**:
- Requires special cache support
- Tag management overhead
- May over-invalidate

**Best For**:
- Related entity groups
- Batch operations
- Simplifying invalidation logic

**Example Tags**:
```
Tags for order: ["order", "orders", "user:123", "pending-orders", "customer:ABC"]
Invalidate user:123 → removes all that user's data
```

### 5. Hybrid Approach (TTL + Active)

**Concept**: Combine TTL (safety net) with active invalidation (freshness)

**Implementation**:
```csharp
// Active invalidation
when_data_changes => cache.Remove(key);

// TTL as safety net (even if invalidation missed)
cache.Set(key, value, TimeSpan.FromMinutes(5));
```

**Benefits**:
- Active invalidation ensures freshness
- TTL catches missed invalidations
- Automatic recovery from bugs

**Best For**:
- Production systems
- Critical data
- Risk-tolerant scenarios

**Example Strategy**:
```
User profile changes:
1. Actively invalidate cache (immediate)
2. TTL expires in 10 minutes (backup)
   → If invalidation missed, user gets fresh data after 10 min
```

### 6. Probabilistic Early Expiration

**Concept**: Refresh cache probabilistically before true expiration

**Implementation**:
```csharp
public T GetWithEarlyExpiration<T>(string key, Func<T> loader, 
    TimeSpan ttl, double refreshProbability = 0.1)
{
    if (cache.TryGet(key, out T value, out TimeSpan remainingTtl))
    {
        // Check if should refresh early
        if (remainingTtl < ttl * 0.25 && 
            Random.NextDouble() < refreshProbability)
        {
            // Refresh in background
            _ = Task.Run(() => {
                T newValue = loader();
                cache.Set(key, newValue, ttl);
            });
        }
        return value;
    }
    
    // Cache miss
    T freshValue = loader();
    cache.Set(key, freshValue, ttl);
    return freshValue;
}
```

**Pros**:
- Prevents cache stampede
- Smoother performance
- Probabilistic load distribution

**Cons**:
- Still serves stale data
- Slightly more complex

**Best For**:
- High-traffic systems
- Preventing thundering herd
- Acceptable staleness

### 7. Write-Through Cache

**Concept**: Update cache AND database, ensure consistency

**Implementation**:
```csharp
cache.Set(key, value);        // Write to cache
database.Save(key, value);    // Write to database
```

**Pros**:
- Cache always consistent
- Simple approach

**Cons**:
- Both must succeed
- Slower writes
- Doesn't handle cache misses

**Best For**:
- Small critical data
- Combined with cache-aside

### 8. Write-Behind Cache

**Concept**: Write to cache, asynchronously write to database

**Implementation**:
```csharp
cache.Set(key, value);              // Immediate
queue.Enqueue(WriteJob(key, value)); // Background
// Separate worker processes queue
```

**Pros**:
- Fast writes
- Non-blocking

**Cons**:
- Data loss risk if cache fails
- Complexity with ordering
- Eventual consistency

**Best For**:
- Non-critical data
- High write throughput
- Analytics/logs

## Decision Framework

### Choose Based On

| Requirement | Best Strategy |
|------------|--------------|
| Simple, any staleness OK | TTL (1-10 minutes) |
| Fresh data, critical | Active invalidation |
| Complex rules, events | Event-based |
| Related groups | Tag-based |
| Production, resilient | Hybrid (TTL + Active) |
| High-traffic, prevent stampede | Probabilistic early refresh |
| Consistency guaranteed | Write-through |
| Fastest writes | Write-behind |

## Real-World Examples

### Example 1: User Profile Cache
```
Strategy: Hybrid (TTL + Active)

TTL: 5 minutes (stale after 5 min)
Active: Invalidate when user updates profile

Flow:
1. User updates name → actively invalidate cache
2. Another user views profile → reads from DB, caches with 5 min TTL
3. If invalidation fails, stale data served max 5 minutes
```

### Example 2: Product Pricing Cache
```
Strategy: TTL + Probabilistic refresh

TTL: 1 minute (prices change frequently)
Refresh probability: 20% when < 15 seconds left

Benefits:
- Mostly fresh prices
- Prevents stampede on expiration
- Some acceptable staleness
```

### Example 3: Search Results Cache
```
Strategy: Tag-based + TTL

Tags: ["search", "query:smartphones", "category:electronics"]
TTL: 5 minutes

Invalidation:
- New product added → invalidate "category:electronics"
- Price changes → invalidate affected categories
- Manual admin clear → invalidate "search"
```

### Example 4: Session Cache
```
Strategy: TTL only

TTL: 30 minutes (standard session duration)

Reasoning:
- Sessions have natural expiration
- Active invalidation on logout
- No complex rules
```

## Common Mistakes to Avoid

### 1. Assuming Always Consistent
```
Wrong: Don't invalidate, just use TTL
Problem: User might see day-old data

Right: Use strategy appropriate for data criticality
```

### 2. Complex Rules Without Tracking
```
Wrong: Invalidate manually, no central tracking
Problem: Miss some cache keys, inconsistency

Right: Use event-based or tag-based for complexity
```

### 3. No Monitoring
```
Wrong: Cache invalidation, no monitoring
Problem: Don't know if working correctly

Right: Track hit rate, invalidation frequency, staleness
```

### 4. Over-Invalidation
```
Wrong: Invalidate entire cache on any change
Problem: Cache becomes ineffective

Right: Granular invalidation by key/tag
```

### 5. Missing Edge Cases
```
Wrong: Only invalidate on normal update
Problem: Admin actions, imports miss invalidation

Right: Invalidate on all data-modifying operations
```

## Monitoring Cache Invalidation

### Key Metrics
- **Hit Rate**: % of requests served from cache
- **Invalidation Rate**: How often cache invalidated
- **Staleness Duration**: Time between data change and cache clear
- **Miss Storm**: Sudden spike in misses after expiration

### Alerts
- Hit rate drops below 70%
- Invalidation failures
- Excessive invalidations (over-invalidating)
- Query latency spikes

## Best Practices

1. **Start Simple**: Begin with TTL
2. **Measure**: Track hit rates and performance
3. **Evolve**: Add active invalidation as needed
4. **Use Hybrid**: TTL + Active is safest
5. **Document**: Explain invalidation strategy clearly
6. **Test**: Invalidation logic is error-prone
7. **Monitor**: Can't improve what you don't measure
8. **Plan Growth**: Invalidation complexity increases with scale

## Interview Tips

1. **Mention Trade-offs**: Consistency vs Performance
2. **Start Simple**: TTL is foundation
3. **Explain Strategy**: Why chosen approach?
4. **Handle Evolution**: "We'd start with TTL, add active invalidation..."
5. **Monitoring**: "We'd track hit rates..."

---

**Key Takeaway**: Cache invalidation requires matching strategy to data criticality and change frequency. Most production systems use hybrid approaches combining multiple strategies.
