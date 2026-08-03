# System Design Fundamentals

## Overview
Foundational concepts and tools necessary to understand and design scalable systems.

## Core Concepts

### 1. Scalability
The ability of a system to handle increasing loads.

#### Vertical Scaling (Scale-Up)
- Add more resources (CPU, RAM, disk) to single server
- **Pros**: Simpler to implement, easier data consistency
- **Cons**: Limited by hardware limits, single point of failure
- **Use When**: Simple systems, initial stages, high single-thread performance needed

#### Horizontal Scaling (Scale-Out)
- Add more servers to handle load
- **Pros**: Unlimited scalability, high availability, fault tolerance
- **Cons**: Complexity, data consistency challenges, network overhead
- **Use When**: Need to handle massive scale, require high availability

### 2. CAP Theorem
A fundamental trade-off in distributed systems.

#### Consistency (C)
- All nodes see same data at same time
- **Strong Consistency**: Immediate updates across all replicas
- **Eventual Consistency**: Updates propagate over time
- **Trade-off**: Consistency vs Availability

#### Availability (A)
- System remains operational despite failures
- Every request receives response (success or failure)
- **Achieved Through**: Replication, redundancy, load balancing
- **Risk**: Returning stale data during partitions

#### Partition Tolerance (P)
- System continues operating despite network partitions
- Network may split into isolated segments
- **Reality**: Network partitions will happen, P is mandatory
- **Choice**: Between C and A when partition occurs

#### Trade-offs
- **CA**: Achievable but not realistic (P always possible)
- **CP**: Consistent but unavailable during partition (traditional databases)
- **AP**: Available but potentially inconsistent (web caches, DNS)

### 3. SOLID Principles Applied to Systems

#### Single Responsibility
- Each component has one clear purpose
- Example: Separation of concerns - web, app, data layers

#### Open/Closed
- Open for extension, closed for modification
- Example: Plugin architecture, microservices

#### Liskov Substitution
- Components exchangeable without breaking contract
- Example: Load balancing across identical servers

#### Interface Segregation
- Clients depend only on needed interfaces
- Example: Specific APIs for specific purposes

#### Dependency Inversion
- High-level modules don't depend on low-level implementations
- Example: Message queues decoupling services

## Design Patterns

### Architectural Patterns

#### Layered Architecture
```
Client Layer → Web Server Layer → Application Layer → Data Layer
```
- **Pros**: Simple, clear separation of concerns
- **Cons**: Can become monolithic, performance overhead
- **Use**: Traditional web applications

#### Microservices Architecture
```
API Gateway → Multiple Independent Services → Databases
```
- **Pros**: Independent deployment, technology diversity, fault isolation
- **Cons**: Complex debugging, network overhead, eventual consistency
- **Use**: Large, rapidly evolving systems

#### Event-Driven Architecture
```
Event Source → Event Bus → Event Processors → Actions
```
- **Pros**: Loose coupling, reactive, scalable
- **Cons**: Complex debugging, eventual consistency
- **Use**: Real-time systems, notification services

### Data Patterns

#### Master-Slave Replication
- Master handles writes
- Slaves handle reads
- **Pros**: Read scalability, simple to understand
- **Cons**: Slave lag, master bottleneck for writes, failover complexity

#### Master-Master Replication
- Multiple masters for both reads and writes
- **Pros**: Write scalability, availability
- **Cons**: Conflict resolution, consistency challenges

#### Sharding
- Split data horizontally across databases
- **Pros**: Write scalability, massive dataset handling
- **Cons**: Complex joins, uneven shard sizes (hotspots)
- **Challenge**: Resharding and rebalancing

## Capacity Planning

### Traffic Estimation
- **Concurrent Users**: How many at same time?
- **Requests Per Second (RPS)**: Peak vs average
- **Bandwidth**: Data per request × RPS

### Example Calculation
```
100M daily active users
Peak usage: 5000 concurrent users
Avg session: 5 minutes
Active duration: 8 hours

Concurrent users at peak = (100M × 5 / (8 × 60)) ≈ 4.2M
Peak RPS = 4.2M × (average requests per minute)
```

### Server Capacity
- **CPU**: Operations per second
- **Memory**: Concurrent connections, cache size
- **Disk**: Data storage, throughput
- **Network**: Bandwidth per server

### Growth Planning
- Estimate 3-5 year growth
- Account for peak traffic (peak:average ratio)
- Build in redundancy and headroom

## Estimation Techniques

### Back-of-Envelope Calculations
Key numbers to remember:
- **Latencies**:
  - L1 cache: 4ns
  - RAM: 100ns
  - Disk seek: 10ms
  - Network round-trip: 1-100ms

- **Throughput**:
  - Modern CPU: ~10^9 operations/second
  - SSD: ~100k reads/second
  - HDD: ~1k reads/second
  - Bandwidth: ~1Gbps typical

- **Storage**:
  - 1GB = 10^9 bytes
  - 1TB = 10^12 bytes
  - 1 million strings (100 bytes each) = 100MB

### Fermi Estimation Method
1. Break problem into estimable components
2. Estimate each component
3. Combine estimates
4. Sanity check results

### Red Flags in Estimates
- Orders of magnitude off
- Exceeding available hardware capacity
- Network bandwidth insufficient
- Storage exceeding data center capacity

## System Components

### Load Balancer
- Distributes traffic across servers
- **Algorithms**: Round-robin, least connections, IP hash, weighted
- **Health Checks**: Detect failed servers
- **Sticky Sessions**: Route same user to same server

### Web Server
- Handles HTTP requests
- Examples: Nginx, Apache, .NET Kestrel
- **Stateless**: Better for horizontal scaling

### Application Server
- Business logic execution
- Database interaction
- Cache management

### Database
- Persistent data storage
- Transaction support (SQL) or flexibility (NoSQL)
- Replication and backup

### Cache
- Reduces database load
- Improves response time
- Examples: Redis, Memcached, CDN

### Message Queue
- Decouples services
- Asynchronous processing
- Examples: RabbitMQ, Kafka, SQS

### Monitoring & Logging
- Performance metrics
- Error tracking
- Performance debugging
- Usage analytics

## Technology Choices Matrix

| Requirement | Good Options |
|------------|--------------|
| Transactional consistency | SQL databases |
| Massive scale, reads | NoSQL with read replicas |
| Real-time analytics | Stream processing (Kafka) |
| Full-text search | Elasticsearch |
| Distributed caching | Redis, Memcached |
| Message queue | RabbitMQ, Kafka, AWS SQS |
| Quick prototyping | Python, Node.js |
| Performance-critical | Java, Go, C# |

## Key Interview Concepts

- Understand trade-offs, not absolutes
- Explain assumptions clearly
- Estimate capacity before designing
- Know when to scale vs optimize
- Consider failure modes
- Think about data consistency
- Account for growth and change

## Practice Files
- **01-Explanation**: Detailed concept explanations
- **02-Architecture-Diagrams**: System architecture examples
- **03-Code-Examples**: Sample implementations of concepts
