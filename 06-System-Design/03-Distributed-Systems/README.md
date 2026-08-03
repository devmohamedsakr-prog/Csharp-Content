# Distributed Systems

## Overview
Design and implementation challenges of systems spanning multiple machines and networks.

## Distributed System Challenges

### Network Issues

#### Latency
- **Definition**: Time for data to travel between nodes
- **Sources**: Speed of light, routing, switching
- **Impact**: 
  - Database call: 1-10ms
  - Inter-datacenter: 10-100ms
  - Internet: 100-500ms

#### Bandwidth
- **Definition**: Data transfer rate
- **Bottleneck**: Network links, often at edges
- **Planning**: Must account for growth

#### Packet Loss
- **Definition**: Some packets never arrive
- **Reliability**: TCP handles retransmission
- **Impact**: Increased latency on retries

#### Network Partitions
- **Definition**: Groups of nodes isolated from each other
- **Inevitability**: Will happen at scale
- **CAP Implication**: Choose consistency or availability

### Failure Modes

#### Server Failure
- **Causes**: Hardware failure, software crash, power loss
- **Detection**: Heartbeat timeouts, explicit health checks
- **Recovery**: Failover to replicas, restart, replacement

#### Network Failure
- **Causes**: Cable cut, switch failure, routing issues
- **Detection**: Connection timeouts
- **Recovery**: Rerouting, partition handling

#### Byzantine Failures
- **Definition**: Component sends incorrect data (malicious or corrupt)
- **Difficulty**: Cannot simply use majority vote
- **Solution**: Byzantine Fault Tolerance (BFT) algorithms
- **Use**: Blockchains, critical military systems

### Clock Synchronization

#### Problem
- **Fact**: Clocks on different servers drift
- **Impact**: Cannot rely on absolute timestamps
- **Challenge**: Determining event order

#### Solutions

##### NTP (Network Time Protocol)
- **Method**: Synchronize clocks to reference server
- **Accuracy**: 1-50ms typically
- **Use**: General distributed systems

##### Logical Clocks
- **Lamport Clocks**: Monotonic counter, capture ordering
- **Vector Clocks**: Track causality, detect concurrent events
- **Use**: When physical synchronization insufficient

## Consistency Models

### Strong Consistency
- **Guarantee**: All nodes see same data immediately
- **Implementation**: Synchronous replication
- **Pros**: Simple application logic
- **Cons**: Higher latency, reduced availability

### Weak Consistency
- **Guarantee**: Updates propagate eventually
- **Variants**: Causal, FIFO, session consistency
- **Pros**: High performance, availability
- **Cons**: Complex application logic

### Eventual Consistency
- **Guarantee**: All replicas converge to same value
- **Timeline**: Seconds to minutes typically
- **Pros**: High availability, partition tolerance
- **Cons**: Stale reads, concurrent write conflicts
- **Use**: NoSQL databases, caches, social networks

### Causal Consistency
- **Guarantee**: Causally related operations ordered consistently
- **Definition**: A happens before B if A affects B
- **Use**: Social media feeds, collaborative editing

### Read Your Writes
- **Guarantee**: Client sees own writes
- **Implementation**: Route subsequent reads to replicas after write
- **Use**: Common requirement in modern apps

### Monotonic Reads
- **Guarantee**: If read value v1, all later reads ≥ v1
- **Use**: Prevents going backward in time

## Replication

### Master-Slave Replication
```
Master (Write) → Network → Slaves (Read)
```
- **Synchronous**: Master waits for slave acknowledgment
- **Asynchronous**: Master returns immediately
- **Pros**: Simple, read scaling
- **Cons**: Slave lag, writes bottleneck

### Multi-Master Replication
```
Master 1 ↔ Network ↔ Master 2
```
- **Use**: Geographic distribution, write availability
- **Challenge**: Conflict resolution for concurrent writes
- **Approaches**: Last-write-wins, custom merge logic, operational transformation

### Replication Protocols

#### Synchronous Replication
- **Guarantee**: Durability on majority before ACK
- **Pros**: Strong consistency guarantees
- **Cons**: Slower writes, availability impact

#### Asynchronous Replication
- **Guarantee**: ACK before replication complete
- **Pros**: Fast writes
- **Cons**: Data loss risk, eventual consistency

#### Semi-Synchronous
- **Hybrid**: Synchronous to one replica, async to others
- **Balance**: Consistency vs performance

## Partitioning (Sharding)

### Sharding Key Selection
- **Requirements**: 
  - Even distribution
  - Stable (doesn't change)
  - Enables local joins
  
- **Good Keys**: User ID, customer ID
- **Bad Keys**: Timestamp, status, geographic location (can be hot)

### Sharding Strategies
- **Range-based**: Problems with hot spots and skew
- **Hash-based**: Better distribution but resharding difficult
- **Directory-based**: Flexible but single point of failure
- **Consistent Hashing**: Minimizes resharding impact

### Handling Hot Spots
- **Identification**: Monitor shard load
- **Solutions**:
  - Rehashing: Add replica shards
  - Split shard: Divide large shard
  - Prefix key: Add artificial prefix to distribute

### Resharding
- **Challenge**: Massive data movement
- **Approach**: 
  1. Set up new shard cluster
  2. Gradual migration (dual-write phase)
  3. Cutover to new shards
  4. Deprecate old shards
- **Downtime**: Possible but minimizable with careful planning

## Consensus Algorithms

### Raft Algorithm
- **Purpose**: Achieve agreement on state across replicas
- **Components**: 
  - Leader election
  - Log replication
  - Safety guarantees
- **Property**: Majority needed for consensus
- **Use**: etcd, Consul, many databases

### Paxos Algorithm
- **Purpose**: Byzantine fault-tolerant consensus
- **Complexity**: Difficult to understand and implement
- **Property**: Tolerates ⅓ Byzantine failures
- **Use**: Google Chubby, some databases

### Byzantine Fault Tolerance (BFT)
- **Problem**: Malicious or corrupted nodes
- **Requirement**: Nodes determine truth despite false information
- **Tolerance**: Up to ⅓ nodes can be faulty
- **Use**: Blockchain, military systems

## Fault Tolerance

### Detection
- **Heartbeat**: Periodic "alive" signal
- **Timeout**: If no heartbeat in T, consider failed
- **Challenge**: Distinguish failure from slow network
- **Solution**: Adaptive timeouts based on network conditions

### Recovery Strategies
- **Failover**: Switch to replica
- **Restart**: Restart failed component
- **Replacement**: Physical hardware replacement
- **Rebalancing**: Redistribute load after failure

### Redundancy Types
- **N+1**: One spare
- **N+2**: Two spares (for simultaneous failures)
- **Geographically Distributed**: Survive datacenter failure

## Service Discovery

### Problem
- **Dynamic**: Servers join/leave constantly
- **Multiple**: Many service instances
- **Solution**: Central registry or peer-to-peer discovery

### Solutions

#### Centralized Registry
- **Server**: Consults registry to find service
- **Examples**: Consul, etcd, Eureka
- **Pros**: Simple, centralized control
- **Cons**: Registry is critical infrastructure

#### Client-Side Discovery
- **Client**: Requests registry, chooses server
- **Pros**: Client has control
- **Cons**: Logic in client, harder to evolve

#### DNS-Based
- **Method**: Service names resolve to multiple IPs
- **Pros**: Standard, ubiquitous
- **Cons**: Caching issues, limited control

## Load Balancing Across Services

### Between Replicas
- **Strategy**: Round-robin, least connections, etc.
- **Servers**: Load balancer identifies healthy replicas

### Across Geographic Regions
- **Strategy**: Route to nearest region
- **Implementation**: Global load balancer, geographic awareness
- **Use**: Reduce latency, improve availability

### Service Mesh
- **Layer**: Between services, handles routing
- **Examples**: Istio, Linkerd
- **Benefits**: Observability, traffic management, security

## Observability in Distributed Systems

### Logging
- **Challenge**: Log aggregation across nodes
- **Solution**: Centralized log collection (ELK, Splunk)
- **Information**: Include request IDs, timestamps, service name

### Metrics
- **Key Metrics**: Latency, throughput, error rate, resource usage
- **Collection**: Prometheus, Datadog, New Relic
- **Alerting**: Threshold-based notifications

### Tracing
- **Purpose**: Follow request through distributed system
- **Tools**: Jaeger, Zipkin, X-Ray
- **Value**: Identify bottlenecks and failures

## Distributed Transactions

### Challenges
- **Atomicity**: Cannot guarantee all-or-nothing across machines
- **Consistency**: Maintaining invariants across services
- **Durability**: Ensuring writes persist despite failures

### Two-Phase Commit (2PC)
- **Phase 1**: Prepare (can you commit?)
- **Phase 2**: Commit or Rollback
- **Pros**: Strong consistency
- **Cons**: Blocking, availability issues, slow

### Saga Pattern
- **Approach**: Sequence of local transactions with compensations
- **Choreography**: Events trigger steps
- **Orchestration**: Central coordinator
- **Pros**: No blocking, better availability
- **Cons**: Eventual consistency, compensation complexity

## Distributed System Design Principles

1. **Assume Failures**: Everything can fail
2. **Minimize Latency**: Cache, replicate, distribute geographically
3. **Consistency Trade-offs**: Choose appropriate model
4. **Monitor Everything**: Can't fix what you can't measure
5. **Embrace Async**: Decouple services, use queues
6. **Design for Resilience**: Circuit breakers, timeouts, retries
7. **Simple Communication**: REST, message queues
8. **Local Optimization**: Cache locally, sync asynchronously

## Practice Files
- **01-Explanation**: Distributed system theory and algorithms
- **02-Architecture-Diagrams**: Replication, partitioning, consensus patterns
- **03-Code-Examples**: Consistent hashing, leader election, consensus implementations
