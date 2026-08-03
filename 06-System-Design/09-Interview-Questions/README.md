# Interview Questions - System Design

## Overview
Real system design interview questions with guidance on approaching each problem.

## Interview Structure

### Time Allocation (45-60 minutes typical)
1. **Clarify Requirements** (5-10 min)
   - Ask about scale, features, constraints
   - No correct answer yet

2. **High-Level Design** (10-15 min)
   - Main components
   - Data flow
   - Technology choices
   - Back-of-envelope math

3. **Detailed Design** (15-20 min)
   - Deep dive into critical components
   - Data schema
   - API contracts
   - Algorithms if needed

4. **Optimization & Edge Cases** (10-15 min)
   - Identify bottlenecks
   - Address failure scenarios
   - Scaling considerations
   - Trade-off discussions

## Question Difficulty Levels

### Easy (Warm-up)
- Simple system, limited scale
- Basic architectural patterns sufficient
- Few optimization challenges

### Medium (Core)
- Popular system (Twitter, Instagram, Uber)
- Moderate scale (10M-100M users)
- Multiple design decisions

### Hard (Expert)
- Complex system, very high scale
- Multiple interdependent components
- Requires deep optimization

## Easy Level Questions

### 1. Tiny URL Service
**Prompt**: Design a URL shortening service like TinyURL.

**Expected Discussion**:
- Unique short code generation (base62, counter with hashing)
- Collision handling
- Scale: billions of URLs
- Analytics requirements
- Redirection: fast redirect via cache/CDN

**Key Points**:
- Stateless service can scale horizontally
- Trade-off: storage vs computation
- Counter-based encoding beats hashing

### 2. Key-Value Cache (Memcached)
**Prompt**: Design an in-memory cache like Memcached.

**Expected Discussion**:
- Hash table based storage
- LRU eviction policy
- Multi-threaded or event-loop?
- Replication considerations
- Consistent hashing for sharding

**Key Points**:
- Efficiency is critical
- Simple data structure (hash table)
- LRU provides good performance

### 3. Unique ID Generator (UUID/Snowflake)
**Prompt**: Design a system to generate unique IDs for millions of objects.

**Expected Discussion**:
- UUID: Simple, no coordination needed
- Snowflake: Structure for sortability (timestamp + machine + sequence)
- Centralized vs Distributed generation
- Collision handling

**Key Points**:
- Trade-off: simplicity vs sortability
- Distributed generation requires coordination
- Sorting by timestamp valuable for efficiency

## Medium Level Questions

### 1. Design Twitter
**Prompt**: Design a social media platform (Twitter, X).

**Clarifying Questions**:
- How many users? Scale: ~300M users, peak 5000 concurrent
- Functional: Post, follow, timeline, search, trends
- Non-functional: <200ms latency, high availability

**High-Level**:
- Web tier: Load balanced stateless services
- Post service: Write to database, cache
- Feed service: Pull/push hybrid, cache layer
- Search: Elasticsearch or similar
- Notification: Message queue based

**Details**:
- Feed generation: Push model for active users
- Timeline database: Time-series data, sharded by user
- Search: Inverted index on Elasticsearch
- Notifications: Async with message queue

**Scaling**:
- Database replication for read scaling
- Cache hot data (trending tweets)
- CDN for media
- Sharding by user ID

### 2. Design Instagram Feed
**Prompt**: Design the Instagram/Facebook feed system.

**Key Decisions**:
- Timeline: Push with fan-out (write complexity)
- Ranking: Engagement signals, recency
- Personalization: ML-based for you page
- Real-time: Should updates appear immediately?

**Architecture**:
- Post service: Stores posts, images
- Graph service: Follow relationships
- Timeline service: Pre-computed (push model)
- Feed service: Read from timeline
- Search service: Posts, hashtags, locations

**Optimization**:
- Cache aggregation: Redis clusters
- Media: S3 with CDN (CloudFront)
- Analytics: Async processing

### 3. Design Uber/Lyft
**Prompt**: Design a ride-sharing platform.

**Clarifying Questions**:
- Functional: Request ride, match with driver, navigation, payment
- Scale: 10M drivers, 100M users
- Geography: Global, location-based

**Architecture**:
- Location service: Real-time tracking (WebSocket)
- Matching service: Find nearby drivers
- Payment service: Process payments securely
- Rating service: Track scores

**Challenges**:
- Real-time matching: Optimal algorithm
- Surge pricing: Adjust price when demand high
- Driver deadheading: Minimize unproductive time
- Fraud detection: Suspicious patterns

### 4. Design YouTube
**Prompt**: Design a video streaming platform.

**Key Components**:
- Upload service: Handle video ingestion
- Transcoding: Multiple quality levels
- Storage: S3-like distributed storage
- Streaming: Adaptive bitrate (HLS/DASH)
- Recommendation: ML-based suggestions
- Search: Full-text on video metadata

**Challenges**:
- Transcoding bottleneck: Queue-based, distributed
- Bandwidth: CDN essential for geographic distribution
- Scale: Millions of concurrent streams

### 5. Design WhatsApp
**Prompt**: Design a messaging application.

**Functional**:
- Send messages, group chats
- End-to-end encryption
- Delivery confirmation
- Last seen timestamp

**Technical**:
- Connection: Long-polling or WebSocket
- Message queue: Handle delivery guarantees
- Encryption: AES symmetric per conversation
- Database: Store messages, user metadata

**Scaling**:
- Horizontal scaling of message servers
- Database replication
- Message queue for reliability
- Geo-distributed for latency

## Hard Level Questions

### 1. Design Google Docs
**Prompt**: Design a collaborative document editing system.

**Challenges**:
- Conflict resolution: Simultaneous edits
- Real-time sync: Low latency updates
- Persistence: Auto-save without freezing
- Offline support: Sync when back online

**Architecture**:
- Real-time collaboration: Operational transformation or CRDT
- Sync service: Send deltas, handle conflicts
- Storage: Versioning, snapshots
- Permissions: Document sharing, access control

### 2. Design Tinder
**Prompt**: Design a dating/matching application.

**Key Components**:
- Profile service: User info, photos, matching preferences
- Matching algorithm: Find compatible users
- Chat service: Messaging between matches
- Recommendation: Suggest potential matches
- Leaderboard: Popular users

**Challenges**:
- Geographic: Find users nearby
- Matching: Complex algorithm, fairness
- Fraud: Detect bots, fake profiles
- Scalability: Worldwide, millions of daily swipes

### 3. Design Airbnb
**Prompt**: Design a short-term rental marketplace.

**Components**:
- Listing service: Property listings, photos
- Search: By location, date, filters
- Booking: Reserve dates, handle overlaps
- Payment: Secure transactions
- Reviews: Host and guest ratings
- Recommendations: Personalized suggestions

**Challenges**:
- Geographic search: Find properties near location
- Availability: Calendar management, overlapping bookings
- Pricing: Dynamic pricing based on demand
- Fraud: Prevent fake listings, chargebacks

## Interview Strategies

### Do's
1. **Ask clarifying questions**: Scope is critical
2. **Make assumptions explicit**: Communicate what you assume
3. **Do math**: Back-of-envelope calculations show scale thinking
4. **Mention trade-offs**: No perfect solution, only trade-offs
5. **Deep dive selectively**: Focus on what matters most
6. **Be adaptable**: Adjust based on interviewer feedback
7. **Discuss monitoring**: Real systems need observability

### Don'ts
1. **Jump to coding**: Design comes first
2. **Assume wrong scale**: Ask before assuming
3. **Forget about failures**: Resilience matters
4. **Optimize prematurely**: Identify bottleneck first
5. **Use unknown technologies**: Stick to familiar solutions
6. **Ignore non-functional requirements**: Scalability, consistency matter
7. **Talk without thinking**: Pause, think, then speak

## Common Mistakes

1. **Wrong Scope**: 
   - Mistake: Over-engineering simple system
   - Fix: Understand requirements first

2. **Database Monolith**: 
   - Mistake: Using single database for everything
   - Fix: Right tool for right job

3. **No Caching**: 
   - Mistake: Missing obvious cache opportunities
   - Fix: Think about hot data

4. **Ignoring Failures**: 
   - Mistake: Assuming everything works
   - Fix: Design for failure

5. **No Monitoring**: 
   - Mistake: System runs but can't debug issues
   - Fix: Plan monitoring from start

6. **Bad Sharding Key**: 
   - Mistake: Choosing sharding key that creates hotspots
   - Fix: Even distribution key

7. **Circular Dependencies**: 
   - Mistake: Microservices calling each other in cycles
   - Fix: Clear dependency graph

## Preparation Strategy

### Week 1-2
- Learn fundamentals: CAP, consistency models, data structures
- Study 01-Fundamentals, 02-Scalability-Performance

### Week 3-4
- Understand distributed systems challenges
- Study 03-Distributed-Systems, 04-Database-Design

### Week 5-6
- Design simple systems
- Practice easy-level questions
- Study real-world examples

### Week 7-8
- Design medium-complexity systems
- Practice interview-style discussions
- Explain trade-offs clearly

### Week 9-10
- Design complex systems
- Handle edge cases
- Optimize deeply

## Practice Approach

1. **Pick a System**: Choose from easy level first
2. **Draw Diagram**: Visualize architecture
3. **Discuss Components**: Talk through each part
4. **Do Math**: Calculate capacity, latency
5. **Identify Issues**: What breaks at scale?
6. **Optimize**: Address bottlenecks
7. **Discuss Trade-offs**: Why these choices?
8. **Mock Interview**: Time-limited practice

## Resources

- Papers: BigTable, Dynamo, MapReduce, GFS
- Books: Designing Data-Intensive Applications
- Websites: System Design Primer, ByteByteGo
- Practice: LeetCode, interviewing.io

## Success Criteria

- Understand requirements completely
- Create reasonable architecture
- Discuss trade-offs thoughtfully
- Handle scale considerations
- Plan for failures
- Communicate clearly
- Adapt to feedback

---

**Remember**: System design interviews test thinking process more than specific solutions. Show your reasoning, ask questions, and be willing to adjust your design.

## Practice Files
- **01-Explanation**: Interview strategies, common mistakes, preparation
- **02-Architecture-Diagrams**: Solutions for all questions
- **03-Code-Examples**: Key algorithms and implementations
