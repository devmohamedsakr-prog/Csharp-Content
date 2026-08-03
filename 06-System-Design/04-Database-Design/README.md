# Database Design

## Overview
Choosing, optimizing, and scaling databases for different system requirements.

## SQL vs NoSQL

### SQL Databases (Relational)
- **Structure**: Tables with predefined schema
- **ACID**: Strong consistency guarantees
- **Queries**: SQL language, powerful joins
- **Examples**: PostgreSQL, MySQL, Oracle, SQL Server

#### When to Use SQL
- Structured, well-defined data
- Complex queries and relationships
- Need ACID guarantees
- Regulatory compliance
- Traditional business applications

#### Limitations
- Scaling: Difficult to shard
- Schema Evolution: Breaking changes risky
- Performance: Complex joins slow
- Flexibility: Fixed schema

### NoSQL Databases

#### Document Stores
- **Structure**: JSON-like documents
- **Examples**: MongoDB, CouchDB
- **Pros**: Flexible schema, natural for hierarchical data
- **Cons**: No joins, eventual consistency
- **Use**: Content management, user profiles

#### Key-Value Stores
- **Structure**: Key → Value pairs
- **Examples**: Redis, Memcached, DynamoDB
- **Pros**: Simple, fast, scalable
- **Cons**: No queries, limited filtering
- **Use**: Caching, sessions, real-time data

#### Column-Family Stores
- **Structure**: Columns grouped into families
- **Examples**: HBase, Cassandra
- **Pros**: Efficient for wide rows, time series
- **Cons**: Complex API, learning curve
- **Use**: Time series, analytics, massive scale

#### Graph Databases
- **Structure**: Nodes and relationships
- **Examples**: Neo4j, Amazon Neptune
- **Pros**: Efficient relationship queries
- **Cons**: Limited to relationship data
- **Use**: Social networks, recommendation engines

#### Search Engines
- **Structure**: Inverted indices for full-text search
- **Examples**: Elasticsearch, Solr
- **Pros**: Powerful search, analytics
- **Cons**: Not a primary database
- **Use**: Search functionality, logging

### CAP Trade-off
- **Consistency Priority**: SQL, traditional NoSQL
- **Availability Priority**: Many NoSQL databases
- **Partition Tolerance**: Given, choose between C and A

## Relational Database Design

### Normalization
- **Purpose**: Reduce redundancy, maintain integrity
- **Levels**:
  1. **1NF**: Atomic values, no repeating groups
  2. **2NF**: No partial dependencies
  3. **3NF**: No transitive dependencies
  4. **BCNF**: Stricter 3NF
  5. **4NF, 5NF**: Higher levels for complex cases

- **Trade-off**: Normalization helps consistency, hurts performance (joins)

### Denormalization
- **Reverse Normalization**: Add redundancy for performance
- **When**: After profiling shows join bottleneck
- **Maintain**: Keep redundant data consistent (triggers, apps)
- **Document**: Explicitly mark denormalized fields

### Indexing Strategy

#### When Indexes Help
- **Lookups**: WHERE clause on indexed column
- **Sorting**: ORDER BY on indexed column
- **Ranges**: WHERE age BETWEEN 20 AND 30
- **Joins**: Foreign key relationships

#### Types
- **Primary Key**: Unique, identifies row
- **Unique Index**: Constraint on uniqueness
- **Composite Index**: Multiple columns
- **Full-Text Index**: Text search

#### Index Trade-offs
- **Pros**: Faster reads
- **Cons**: Slower writes (update index), extra storage
- **Decision**: High read:write ratio = index worth it

#### Best Practices
- Index WHERE, ORDER BY, JOIN columns
- Don't index low-cardinality columns
- Maintain index statistics
- Monitor unused indexes

### Query Optimization

#### Execution Plans
- **Tool**: Explain/analyze query execution
- **Look For**: Sequential scans (slow), index scans (fast)
- **Understand**: Join order, filter pushdown

#### Common Optimizations
- Add indexes on WHERE columns
- Reorder JOINs for efficiency
- Use EXISTS instead of IN for subqueries
- Batch operations when possible

#### Denormalization Trade-off
- Profile first: Identify actual bottleneck
- Denormalize only when necessary
- Document denormalization reason
- Keep code maintainable

## Database Replication

### Master-Slave Setup
```
Master ←→ Slave 1
    ↓
    Slave 2
    ↓
    Slave 3
```

#### Replication Lag
- **Cause**: Network latency, slave processing speed
- **Impact**: Slaves may serve stale data
- **Handling**: Understand and communicate lag to application
- **Monitoring**: Track replication lag

#### Failure Handling
- **Slave Failure**: Start new slave, catch up from master
- **Master Failure**: Promote slave to master (careful: some data may be lost)

### Master-Master Replication
- **Complexity**: Conflict resolution for concurrent writes
- **Use Case**: Geographic distribution, write availability
- **Risk**: Split-brain scenarios

## Database Sharding

### Sharding Architecture
```
App → Router → Shard 1 (Database 1)
           → Shard 2 (Database 2)
           → Shard 3 (Database 3)
           → Shard 4 (Database 4)
```

### Sharding Schemes

#### Range Sharding
```
Shard 1: IDs 1-10M
Shard 2: IDs 10M-20M
Shard 3: IDs 20M-30M
```
- **Issue**: Uneven distribution over time

#### Hash Sharding
```
Shard = hash(user_id) % num_shards
```
- **Benefit**: Even distribution
- **Challenge**: Resharding requires remapping all data

#### Directory Sharding
```
Lookup table: user_id → shard_id
```
- **Benefit**: Flexible, can rebalance
- **Cost**: Directory is critical infrastructure

### Consistent Hashing
```
Shard = hash(key) mod num_shards (adjusted for ring)
```
- **Benefit**: Minimal remapping when shards added/removed
- **Trade-off**: Somewhat complex to understand

### Resharding
1. **Add new shards**: Set up new shard cluster
2. **Migration**: Move data from old shards to new shards
3. **Dual write**: Write to both old and new (temporary)
4. **Cutover**: Switch reads to new shards
5. **Validation**: Verify no data loss
6. **Deprecate**: Remove old shards

## NoSQL Database Considerations

### Document Stores (MongoDB)
- **Schema**: Flexible documents
- **Consistency**: Single document ACID, multi-doc eventual
- **Sharding**: Built-in support
- **Use**: Content, user data, flexible schema

### Key-Value Stores (Redis)
- **Data Structures**: Strings, lists, sets, sorted sets, hashes
- **Persistence**: Optional (RDB snapshots, AOF log)
- **Use**: Cache, sessions, real-time features

### Cassandra
- **Design**: Distributed from ground up
- **Consistency**: Tunable (strong to eventual)
- **Replication**: Multi-datacenter native
- **Use**: Time series, massive scale, high write throughput

### DynamoDB
- **Model**: Key-value with optional range key
- **Consistency**: Strong or eventual
- **Performance**: Predictable (provisioned capacity)
- **Cost**: Pay for throughput

## Backup and Disaster Recovery

### Backup Strategies

#### Full Backup
- **Method**: Complete database copy
- **Frequency**: Daily or less
- **Restore Time**: Fast (single restore operation)
- **Storage**: Large

#### Incremental Backup
- **Method**: Only changes since last backup
- **Frequency**: Frequent (multiple per day)
- **Restore Time**: Slower (apply incremental backups)
- **Storage**: Small

#### Point-in-Time Recovery
- **Method**: Binary logs + backups
- **Ability**: Recover to any point in past
- **Use**: Undo accidental deletions

#### Replication as Backup
- **Method**: Replicas act as backup
- **Limitation**: Only protects against logical corruption with delay
- **Combine**: With traditional backups for complete protection

### Disaster Recovery
- **RPO (Recovery Point Objective)**: How much data loss acceptable (hours, minutes, seconds)
- **RTO (Recovery Time Objective)**: How long allowed to restore (hours, minutes)
- **Strategy**: Based on these objectives

## Monitoring Database Health

### Key Metrics
- **Query Performance**: Slow query log, execution time
- **Replication Lag**: Monitor slave lag
- **Connection Pool**: Exhaustion warnings
- **Disk Usage**: Growth rate, when full
- **CPU/Memory**: Resource utilization

### Alerts
- **Replication Lag > X seconds**: Data consistency risk
- **Disk > 80% full**: Space running out
- **Slow Queries**: Performance degradation
- **Connection Pool Nearly Full**: Application may hang

## Database Selection Matrix

| Requirement | Good Choice |
|------------|-------------|
| Transactional, complex queries | PostgreSQL, MySQL |
| Document storage | MongoDB, CouchDB |
| Cache/sessions | Redis, Memcached |
| Time series | Cassandra, InfluxDB |
| Full-text search | Elasticsearch |
| Graph data | Neo4j |
| Massive scale writes | Cassandra, HBase |

## Best Practices

1. **Right Tool for Job**: Don't use one database for everything
2. **Monitor**: Understand query patterns, performance
3. **Backup**: Regular backups, test recovery
4. **Replica**: For availability and read scaling
5. **Shard**: Only when necessary (after vertical scaling)
6. **Optimize**: Index, denormalize, cache smartly
7. **Document**: Schema, denormalization decisions, trade-offs

## Practice Files
- **01-Explanation**: Database design principles, indexing strategy
- **02-Architecture-Diagrams**: Replication layouts, sharding patterns
- **03-Code-Examples**: Schema design, queries, index optimization
