# Scalability and Performance

## Overview
Techniques for handling growth and optimizing system performance to maintain speed and reliability as demands increase.

## Scalability Strategies

### Vertical Scaling Limitations
- **Hardware Limits**: CPUs max out, memory finite
- **Cost**: Exponentially more expensive for better hardware
- **Availability Risk**: Single server = single point of failure
- **When Practical**: Initial stages, budget constraints, simple systems

### Horizontal Scaling
- **Basic Approach**: Add more servers
- **Requirements**: Stateless application design, distributed data
- **Benefits**: Fault tolerance, cost efficiency at scale
- **Challenges**: Data consistency, complex deployments

## Load Balancing

### Load Balancer Role
- **Traffic Distribution**: Spread requests across servers
- **Health Checking**: Detect and remove failed servers
- **Session Handling**: Route user sessions appropriately
- **Reverse Proxy**: Hide backend complexity

### Load Balancing Algorithms

#### Round Robin
- **Method**: Distribute requests sequentially
- **Pros**: Simple, fair
- **Cons**: Ignores server capacity, connection time
- **Use**: Identical servers, minimal variance

#### Least Connections
- **Method**: Send to server with fewest connections
- **Pros**: Adapts to varying connection lengths
- **Cons**: Overhead to track connections
- **Use**: Long-lived connections (HTTP keep-alive)

#### IP Hash (Sticky Session)
- **Method**: Hash client IP to determine server
- **Pros**: Same user always same server, caching benefits
- **Cons**: Uneven distribution, breaks load balancer transparency
- **Use**: Sessions requiring server-local state

#### Weighted Round Robin
- **Method**: Assign weights to servers based on capacity
- **Pros**: Handles heterogeneous hardware
- **Cons**: Static weights, needs manual adjustment
- **Use**: Mixed server capabilities

#### Resource-Based (Adaptive)
- **Method**: Consider server CPU, memory, disk usage
- **Pros**: Optimal routing
- **Cons**: Complex, overhead to gather metrics
- **Use**: Mission-critical systems

### Load Balancer Placement
- **External**: Internet-facing, handles all traffic
- **Internal**: Between tiers (web-app, app-database)
- **Multiple Layers**: Redundancy and failover capability

### Session Management
- **Sticky Sessions**: Route user to same server (problematic)
- **Shared Session Store**: Redis, cache storing sessions
- **Stateless Apps**: No server-side session state needed

## Database Scaling

### Replication Strategies

#### Master-Slave Replication
- **Write Path**: All writes to master
- **Read Path**: Reads from slaves for scaling
- **Pros**: Read scalability, automatic failover possible
- **Cons**: Slave lag, eventual consistency, writes bottleneck

#### Master-Master Replication
- **Write Path**: Write to either master
- **Sync**: Both masters sync changes
- **Pros**: Write scaling, availability
- **Cons**: Conflict resolution complexity, network overhead

#### Multi-Master Replication
- **Setup**: Multiple masters across regions
- **Pros**: Geographic distribution, write availability
- **Cons**: Conflict resolution, consistency challenges

### Sharding (Horizontal Partitioning)

#### Sharding Key Selection
- **Good Keys**: Even distribution, minimal joins
- **Bad Keys**: Hot spots, frequent changes
- **Examples**: User ID, timestamp, geographic location

#### Sharding Methods

##### Range-Based Sharding
```
Shard 1: IDs 1-1000000
Shard 2: IDs 1000001-2000000
Shard 3: IDs 2000001-3000000
```
- **Pros**: Simple to implement
- **Cons**: Uneven distribution (hot spots), range changes difficult

##### Hash-Based Sharding
```
Shard = hash(key) mod num_shards
```
- **Pros**: Even distribution
- **Cons**: Resharding difficult when adding shards

##### Directory-Based Sharding
```
Directory lookup: key → shard_id
```
- **Pros**: Flexible, easy to modify
- **Cons**: Directory can become bottleneck

#### Resharding Challenges
- **Migration**: Moving data between shards
- **Downtime**: Service interruption during migration
- **Complexity**: Handling concurrent reads/writes
- **Solution**: Consistent hashing for smooth redistribution

### Denormalization vs Normalization
- **Normalization**: Reduce redundancy, ACID compliance
- **Denormalization**: Faster reads, redundant data
- **Trade-off**: Data consistency vs query performance
- **In Sharded Systems**: Denormalization often necessary for joins

## Caching

### Cache Placement

#### Client-Side Caching
- **Browser Cache**: HTTP headers (Cache-Control, ETag)
- **Application Cache**: In-memory collections
- **Pros**: Reduces server load, fast access
- **Cons**: Stale data, memory overhead

#### Server-Side Caching
- **Application Cache**: Redis, Memcached
- **Pros**: Shared across users, larger datasets
- **Cons**: Network latency, complexity
- **Use**: Frequently accessed data

#### CDN (Content Delivery Network)
- **Purpose**: Cache static content globally
- **Pros**: Geographic distribution, reduced latency
- **Use**: Images, JS, CSS, static content
- **Providers**: CloudFront, Akamai, Cloudflare

### Cache Invalidation
"Two hard things in computer science: cache invalidation and naming things"

#### TTL (Time-To-Live)
- **Method**: Expire cache after set duration
- **Pros**: Simple to implement
- **Cons**: Stale data served, resource waste on cold caches

#### Active Invalidation
- **Method**: Explicitly invalidate when data changes
- **Pros**: Fresh data
- **Cons**: Must track all caches, complex dependencies

#### Event-Based Invalidation
- **Method**: Invalidate based on events
- **Pros**: Flexible, responsive
- **Cons**: Complex event system needed

#### Hybrid Approach
- **Strategy**: TTL + active invalidation
- **Benefit**: TTL as safety net, active for critical data

### Cache Patterns

#### Cache-Aside (Lazy Loading)
```
1. Try to fetch from cache
2. If miss, fetch from DB
3. Write to cache
```
- **Pros**: Only caches accessed data
- **Cons**: Cache miss penalty, stale data possible

#### Write-Through
```
1. Write to cache
2. Write to DB
3. Return
```
- **Pros**: Cache always consistent
- **Cons**: Write latency doubled

#### Write-Behind (Write-Back)
```
1. Write to cache
2. Return to client
3. Asynchronously write to DB
```
- **Pros**: Fast writes
- **Cons**: Data loss risk, complexity

#### Refresh-Ahead
- **Method**: Preemptively refresh cache before expiration
- **Pros**: No cache misses for predicted patterns
- **Cons**: Wastes resources on unpredictable patterns

## Asynchronous Processing

### Synchronous Problems
- **Blocking**: Long operations block user
- **Resource**: Ties up server resources
- **Scale**: Limited by operation duration

### Asynchronous Solutions

#### Message Queues
- **Pattern**: Async task processing
- **Flow**: Producer → Queue → Consumer
- **Benefit**: Decoupling, resilience
- **Trade-off**: Eventual consistency, complexity

#### Job Scheduling
- **Purpose**: Scheduled background tasks
- **Examples**: Batch processing, cleanup, reports
- **Tools**: Quartz, Hangfire, APScheduler

#### Worker Pools
- **Purpose**: Process queued work items
- **Scaling**: Add workers as queue grows
- **Pattern**: Producer-consumer pattern

### When to Use Async
- **Long Operations**: Image processing, email sending
- **Independent Tasks**: Can process separately
- **Resilience**: Decouple services
- **Avoid**: User-blocking operations (need synchronous response)

## Performance Optimization

### Identifying Bottlenecks
- **Profiling**: CPU, memory, disk usage
- **Monitoring**: Real-time metrics
- **Logging**: Trace slow operations
- **User Feedback**: Actual performance issues

### Optimization Priorities
1. **Identify**: Don't optimize prematurely
2. **Measure**: Establish baseline
3. **Profile**: Find actual bottleneck
4. **Optimize**: Address root cause
5. **Verify**: Confirm improvement

### Database Optimization
- **Indexing**: Fast lookups, slower writes
- **Query Optimization**: Reduce full scans
- **Connection Pooling**: Reuse connections
- **Denormalization**: Reduce joins for common queries

### Network Optimization
- **Compression**: Gzip responses
- **HTTP/2**: Multiplexing, server push
- **Connection Pooling**: Reduce overhead
- **Geographic Distribution**: CDN, regional servers

## Monitoring & Metrics

### Key Metrics
- **Latency**: Response time percentiles (p50, p95, p99)
- **Throughput**: Requests per second
- **Error Rate**: Failed requests percentage
- **Resource Utilization**: CPU, memory, disk, network

### Alerting
- **Thresholds**: Define acceptable ranges
- **Notifications**: PagerDuty, email, Slack
- **Escalation**: Increase severity if unresolved
- **On-call**: Response procedures

## Scaling Timeline

### Phase 1: Single Server
- **Capacity**: 1-10k RPS
- **Technology**: Simple web app, single database
- **Challenge**: None yet

### Phase 2: Read Replicas
- **Capacity**: 10-100k RPS
- **Add**: Database read replicas, basic caching
- **Challenge**: Eventual consistency

### Phase 3: Horizontal Scaling
- **Capacity**: 100k-1M RPS
- **Add**: Load balancing, multiple app servers
- **Challenge**: Session management, complexity

### Phase 4: Sharding
- **Capacity**: 1M+ RPS
- **Add**: Database sharding, microservices
- **Challenge**: Data consistency, reshard complexity

### Phase 5: Advanced
- **Capacity**: 100M+ RPS
- **Add**: Multiple regions, mesh architecture
- **Challenge**: CAP theorem, global consistency

## Practice Files
- **01-Explanation**: Detailed scaling strategies and techniques
- **02-Architecture-Diagrams**: Scaling architecture progression
- **03-Code-Examples**: Load balancing, caching, queue implementations
