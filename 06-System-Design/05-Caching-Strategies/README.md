# Caching Strategies

## Overview
Caching is crucial for system performance, reducing latency and database load by serving frequently accessed data from fast storage.

## Cache Fundamentals

### Why Cache
- **Reduce Latency**: In-memory access much faster than database
- **Reduce Load**: Database doesn't service every request
- **Improve Availability**: Degraded service if DB down but cache available
- **Reduce Cost**: Fewer database servers needed

### Cache Trade-offs
- **Consistency**: Stale data served
- **Complexity**: Cache invalidation, extra infrastructure
- **Storage**: Limited by memory
- **Maintenance**: Keep cache up-to-date

## Cache Placement

### Client-Side Caching

#### Browser Cache
- **HTTP Headers**: Cache-Control, ETag, Last-Modified
- **Duration**: Set by server via headers
- **Validation**: If-None-Match (ETag), If-Modified-Since
- **Pros**: No server load, fastest response
- **Cons**: Staleness, user frustration with refresh

#### Application Cache
- **Storage**: In-process memory (C#, Java objects)
- **Scope**: Single application instance
- **Pros**: Very fast, no network latency
- **Cons**: Not shared across servers, memory limited

### Server-Side Caching

#### Application Cache
- **Storage**: Redis, Memcached (external)
- **Access**: Network request (slight latency)
- **Scope**: Shared across all app servers
- **Pros**: Shared, persistent (Redis), large capacity
- **Cons**: Network latency, management complexity

#### Database Query Cache
- **Storage**: Database caches query results
- **Automatic**: Invalidated on table changes
- **Pros**: Transparent to application
- **Cons**: Limited control, not all databases support well

#### CDN (Content Delivery Network)

##### Purpose
- Cache static content globally
- Serve from edge server close to user
- Reduce latency, bandwidth

##### Content Suitable
- Images, videos, JS, CSS
- Static HTML pages
- Any content not changing frequently

##### Trade-offs
- **Pros**: Geographic distribution, huge latency reduction
- **Cons**: Cost, eventual consistency with main site

### Hybrid Caching
Combine multiple levels for maximum benefit:
```
Browser Cache → App Cache → CDN → Database Cache → Database
```

## Cache Invalidation

### Problem
Cache invalidation and naming are "two hard things in CS"

### Strategies

#### TTL (Time-To-Live)
- **Method**: Expire cache after set duration
- **Examples**: 5 minutes, 1 hour, 24 hours
- **Pros**: Simple to implement, automatic cleanup
- **Cons**: 
  - Stale data served until expiration
  - Resource waste caching unpopular items
  - Cold cache on expiration

#### Active Invalidation
- **Method**: Explicitly invalidate when data changes
- **Trigger**: 
  - Database write
  - Admin action
  - Message queue event
- **Pros**: Fresh data immediately
- **Cons**: Complex (must track all cache keys affected)

#### Event-Based Invalidation
- **Mechanism**: Publish events when data changes
- **Subscribers**: Cache listeners react to events
- **Pros**: Flexible, reactive
- **Cons**: Complex event system needed

#### Tag-Based Invalidation
- **Method**: Group cache entries with tags
- **Invalidation**: Clear all entries with tag
- **Example**: Tag all user-123 data, invalidate all together
- **Pros**: Batches related entries
- **Cons**: Potential over-invalidation

#### Hybrid Approach
- **Strategy**: TTL + active invalidation
- **Benefit**: 
  - Active invalidation for immediate updates
  - TTL as safety net for missed invalidations
- **Trade-off**: Additional complexity

## Cache Patterns

### Cache-Aside (Lazy Loading)
```
1. Application checks cache
2. If miss, fetch from DB
3. Store in cache
4. Return to user
```

**Pseudo-code:**
```
value = cache.get(key)
if value == nil
    value = database.get(key)
    cache.set(key, value)
return value
```

- **Pros**: Only caches accessed data
- **Cons**: 
  - Cache miss penalty on first access
  - Stale data possible
  - Stampede on concurrent misses

#### Stampede Prevention
- **Problem**: Multiple requests miss cache simultaneously, all query DB
- **Solutions**:
  - Locks: Only one request fetches, others wait
  - Probabilistic early expiration: Refresh before true expiration
  - Queuing: Queue additional requests while first fetches

### Write-Through
```
1. Write to cache
2. Write to database
3. Return to user
```

**Pseudo-code:**
```
cache.set(key, value)
database.set(key, value)
return success
```

- **Pros**: Cache always consistent with database
- **Cons**: 
  - Write latency: Must write to both
  - Slower than write-behind
  - Doesn't handle cache misses (use with cache-aside)

### Write-Behind (Write-Back)
```
1. Write to cache
2. Return to user immediately
3. Asynchronously write to database
```

**Pseudo-code:**
```
cache.set(key, value)
queue.enqueue(write_task(key, value))
return success
// Separate worker processes queue
```

- **Pros**: Fast writes
- **Cons**: 
  - Data loss risk (if cache fails before DB write)
  - Complexity (asynchronous coordination)
  - Conflicting writes possible

### Refresh-Ahead
- **Method**: Proactively refresh cache before expiration
- **Trigger**: 
  - Periodic refresh of popular items
  - Access pattern prediction
- **Pros**: No cache misses for predicted access
- **Cons**: Wastes resources on unpredictable patterns
- **Use**: Highly predictable access patterns

## Distributed Caching

### Shared Cache Architecture
```
App 1  \
App 2  -- Redis Cluster
App 3  /
```

### Cache Clusters
- **Replication**: Backup nodes for availability
- **Sharding**: Distribute cache across nodes
- **Consistency**: All nodes have consistent view

### Cache Coherence
- **Problem**: Same key in multiple caches
- **Solutions**:
  - Single source (one cache instance for all)
  - Write-through (write to all)
  - Invalidation protocol (notify all on changes)

## Cache Eviction Policies

### When Cache Full
Must make room for new entries by evicting old ones

### LRU (Least Recently Used)
- **Evict**: Item accessed least recently
- **Pros**: Works well for temporal locality
- **Cons**: Expensive to track access time

### LFU (Least Frequently Used)
- **Evict**: Item accessed least frequently
- **Pros**: Prioritizes popular items
- **Cons**: Expensive to track access count, adapts slowly to patterns

### FIFO (First In, First Out)
- **Evict**: Oldest item
- **Pros**: Simple to implement
- **Cons**: Doesn't consider usage

### Random
- **Evict**: Random selection
- **Pros**: Simple, quick
- **Cons**: No locality consideration

## Cache Technologies

### Redis
- **Type**: In-memory data structure store
- **Structures**: Strings, lists, sets, hashes, sorted sets
- **Persistence**: RDB snapshots, AOF log
- **Use**: General-purpose cache, sessions, real-time
- **Pros**: Fast, persistent, rich data types
- **Cons**: Single-threaded for writes, memory limited

### Memcached
- **Type**: Simple key-value cache
- **Features**: Minimal, focused on speed
- **Persistence**: None (cache loss on restart)
- **Use**: Distributed caching layer
- **Pros**: Very fast, simple, scales well
- **Cons**: No persistence, no complex data types

### CDN (CloudFront, Cloudflare)
- **Type**: Geographically distributed cache
- **Content**: Static assets (JS, CSS, images)
- **Pros**: Massive latency reduction, global coverage
- **Cons**: Cost, limited content types

## Cache Sizing

### Estimation
1. **Identify** frequently accessed data
2. **Estimate** size of dataset
3. **Calculate** cache size needed:
   ```
   Cache Size = Dataset Size / Hit Rate / (1 - Hit Rate)
   ```
4. **Monitor** and adjust

### Example
- Dataset: 10GB
- Hit Rate Target: 95%
- Minimum Cache Size: 10GB × (0.95 / 0.05) = 190GB minimum (impractical)
- More realistic: Cache top 1GB (hot data), hit rate 80%

## Monitoring Cache Performance

### Key Metrics
- **Hit Rate**: Cache hits / (cache hits + misses)
- **Miss Rate**: 1 - hit rate
- **Eviction Rate**: How often items evicted
- **Memory Usage**: Percent of capacity
- **Latency**: Response time for cache hits vs DB

### Benchmarks
- **Good Hit Rate**: 80%+ typically
- **Hit Rate Trend**: Should be stable or improving
- **Eviction**: Should be predictable and balanced

## Cache Warm-up
- **Problem**: Cold cache causes poor performance on startup
- **Solutions**:
  - Pre-populate on startup: Load common data
  - Gradual warmup: Build up over time
  - Use replicated cache: Existing cluster retains data

## Anti-Patterns to Avoid

### Inconsistent Cache
- **Problem**: Cache and database out of sync
- **Fix**: Proper invalidation strategy

### Unbounded Cache Growth
- **Problem**: Cache grows until out of memory
- **Fix**: Set TTL, proper eviction policy

### Stampede on Cold Start
- **Problem**: All requests hit DB simultaneously
- **Fix**: Stagger cache loading or use locks

### Cache as Primary Storage
- **Problem**: Losing cache = data loss
- **Fix**: Cache should be supplement, not replacement

## Best Practices

1. **Measure First**: Profile to identify bottlenecks
2. **Simple Strategy**: Start with cache-aside
3. **Appropriate TTL**: Balance freshness vs cache value
4. **Monitoring**: Track hit rate, performance
5. **Warm-up**: Pre-populate on startup
6. **Failover**: Handle cache unavailability gracefully
7. **Document**: Cache strategy and invalidation approach

## Practice Files
- **01-Explanation**: Caching theory, invalidation strategies
- **02-Architecture-Diagrams**: Cache placement patterns
- **03-Code-Examples**: Redis client, cache-aside pattern, TTL handling
