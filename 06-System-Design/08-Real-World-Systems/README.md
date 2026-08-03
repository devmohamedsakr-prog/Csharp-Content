# Real-World Systems

## Overview
Case studies of actual systems and how they address design challenges at scale.

## 1. URL Shortening Service (Bit.ly, TinyURL)

### Requirements
- Shorten long URLs to short codes
- Redirect short URL to original
- Track analytics (clicks, referrers)
- Custom short codes optional

### Key Challenges
- **Collision Handling**: Different long URLs might hash to same code
- **Analytics**: Track all clicks without impacting redirect
- **Scale**: Billions of shortened URLs

### Architecture
```
1. URL Shortening:
   - Generate or hash long URL
   - Check collision, retry if needed
   - Store mapping in database
   - Return short code

2. Redirection:
   - Lookup short code → long URL
   - Record analytics (async)
   - Redirect user (301/302)

3. Analytics:
   - Count clicks
   - Track referrer, user agent, IP
   - Generate reports
```

### Encoding Strategies
- **Base62**: Use 0-9, a-z, A-Z (62 chars)
- **Base64**: More compact but includes special chars
- **Counter**: Sequential IDs encoded (avoids collisions)

### Scaling
- Short code generation: Stateless, can scale horizontally
- Redirects: Cache with CDN, database replicas
- Analytics: Asynchronous processing with queues

## 2. Social Media Feed

### Requirements
- Display timeline of posts from followed users
- Real-time updates
- Rank by relevance/recency
- Personalization

### Key Challenges
- **Fan-out**: One user posts, millions need update
- **Scalability**: Billions of users, posts
- **Personalization**: Different feed for each user

### Approaches

#### Pull Model (On-Demand)
```
User requests feed → 
Query posts from followers' timelines → 
Merge and rank → 
Return to user
```
- **Pros**: Storage efficient
- **Cons**: Slow (multiple database queries), O(n) complexity

#### Push Model (Pre-computed)
```
User posts → 
Send to all followers' feeds (asynchronously) → 
User reads pre-computed feed
```
- **Pros**: Fast reads, O(1) lookup
- **Cons**: Write amplification, storage

#### Hybrid Approach
- **Active Users**: Push model (followers < 5000)
- **Celebrities**: Pull model (followers > 100k)
- **Mix**: Combine approaches based on user profile

### Architecture
```
User Service → Timeline Service (read)
            → Fan-out Service (write)
            → Feed Cache (Redis)
            → Post Database
```

## 3. Search Engine

### Requirements
- Index web pages
- Return relevant results to queries
- Real-time or near-real-time indexing
- Rank results by relevance

### Components

#### Crawler
- **Purpose**: Discover and download web pages
- **Challenge**: Billions of pages, crawl frequency
- **Implementation**: Distributed, respectful of robots.txt

#### Indexer
- **Purpose**: Extract words and links from pages
- **Output**: Inverted index (word → document IDs)
- **Storage**: Massive distributed database

#### Ranker
- **Signals**: Relevance, authority (PageRank), freshness
- **Algorithm**: Machine learning based
- **Result**: Ordered list of documents

### Architecture
```
Query → Query Processor → Ranker → Results
                       → Index Lookup
```

## 4. Recommendation System

### Approaches

#### Collaborative Filtering
- **Idea**: Users with similar preferences like similar items
- **Data**: User-item interaction matrix
- **Algorithm**: Find similar users, recommend their favorites
- **Pros**: Works without item knowledge
- **Cons**: Cold start problem (new users/items)

#### Content-Based
- **Idea**: Recommend similar items to user's past likes
- **Features**: Genre, actor, director (for movies)
- **Algorithm**: Find items similar to user's history
- **Pros**: Works for new items
- **Cons**: Requires item features, prone to over-personalization

#### Hybrid Approach
- Combine both approaches
- Use collaborative filtering primarily
- Content-based for cold start
- Blend recommendations

### Implementation
```
User Profile → Feature Engineering → Model Training → Ranking → 
Recommendations
```

### Challenges
- **Scale**: Millions of users, billions of items
- **Real-time**: Rankings change constantly
- **Cold Start**: New users have no history
- **Diversity**: Avoid filter bubble, show variety

## 5. Live Streaming Service (Twitch, YouTube Live)

### Requirements
- Stream video in real-time
- Multiple quality levels
- Chat and interactions
- Handle bandwidth constraints

### Key Challenges
- **Streaming Protocol**: Low latency, reliable
- **Quality Adaptation**: Adjust for user bandwidth
- **Distribution**: Get stream to millions

### Architecture
```
Broadcaster → Streaming Server → CDN Edge Servers
           → Transcoder (multiple bitrates)
           → Chat/Interaction Service → Viewers
```

### Protocols
- **RTMP**: Push protocol from broadcaster
- **HLS**: HTTP Live Streaming (segments, adaptive bitrate)
- **DASH**: Dynamic Adaptive Streaming over HTTP
- **WebRTC**: Low latency, peer-to-peer possible

### Quality Adaptation
- Monitor bandwidth
- Dynamically switch bitrate
- Buffering vs quality trade-off
- Start low, increase if possible

## 6. Ride-Sharing (Uber, Lyft)

### Requirements
- Match drivers with riders
- Location tracking
- Payment processing
- Driver ratings

### Key Challenges
- **Real-time Matching**: Minimize wait time
- **Geographic Distribution**: Demand varies by location
- **Surge Pricing**: Adjust price when demand high

### Architecture
```
Rider App → Location Service (real-time)
         → Matching Service
         → Driver Service (real-time)
         → Payment Service
         → Rating Service
```

### Matching Algorithm
- Find nearby available drivers
- Consider: Distance, driver rating, surge multiplier
- Optimization: Minimize rider wait, driver deadhead time
- Challenge: Complex optimization problem at scale

## 7. Payment System

### Requirements
- Process payments reliably
- Handle multiple payment methods
- Reconciliation and fraud detection
- Compliance (PCI-DSS)

### Key Challenges
- **Reliability**: Cannot lose payment data
- **Fraud**: Detect and prevent fraudulent transactions
- **Compliance**: Strict security and data requirements
- **Reconciliation**: Ensure all transactions accounted for

### Architecture
```
Payment Request → Payment Processor (external provider)
              → Transaction Logger
              → Reconciliation Service
              → Fraud Detection
              → Settlement
```

### Key Components
- **Payment Gateway**: Interface to card networks
- **Tokenization**: Store card data securely
- **Encryption**: PCI compliance
- **Audit Trail**: Every transaction logged
- **Reconciliation**: Match payments with deposits

### Failures Handling
- **Idempotency**: Same request processed once
- **Retry Logic**: Transient failures auto-retry
- **Manual Reconciliation**: Humans resolve edge cases

## System Design Patterns Across Examples

### Pattern 1: Fan-Out
- **Use**: Social networks, notifications
- **Implementation**: Push to many recipients asynchronously

### Pattern 2: Caching
- **Use**: Frequently accessed data
- **Implementation**: Redis, CDN, browser cache

### Pattern 3: Async Processing
- **Use**: Long operations
- **Implementation**: Message queues, workers

### Pattern 4: Replication
- **Use**: High availability
- **Implementation**: Master-slave, multi-master

### Pattern 5: Sharding
- **Use**: Massive scale
- **Implementation**: Partition by user ID, geographic region

## Lessons Learned

### 1. Start Simple
- Monolithic first
- Optimize when needed
- Measure before optimizing

### 2. Know Your Bottleneck
- Different systems have different bottlenecks
- Must monitor and measure
- Database vs network vs compute

### 3. Plan for Scale
- 10x growth expected
- Architecture should support
- But don't over-engineer initially

### 4. Embrace Redundancy
- Components fail
- Multiple replicas essential
- Failure should degrade gracefully

### 5. Asynchronous Everything
- Reduce blocking operations
- Use queues for async work
- Improves responsiveness

### 6. Cache Strategically
- Not all data needs caching
- Find the hot data
- Invalidation strategy critical

### 7. Know Your Data
- Schema design critical
- Denormalization for performance
- Sharding strategy early decision

### 8. Monitor and Alert
- Can't fix what you can't measure
- Real-time dashboards
- Alert thresholds

## Practice Files
- **01-Explanation**: Deep dive into each real system
- **02-Architecture-Diagrams**: Full system architectures with data flow
- **03-Code-Examples**: Simplified implementations of key components
