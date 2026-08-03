# Capacity Planning and Back-of-Envelope Estimation

## Introduction

Capacity planning is estimating resources needed to handle expected load. Back-of-envelope calculations help make these estimates without detailed metrics.

## Key Numbers to Remember

### Time (Latency)
```
L1 Cache Reference:              4 ns
L2 Cache Reference:             10 ns
L3 Cache Reference:             40 ns
RAM access:                    100 ns
Reading 1 MB sequentially:   250 μs
Random disk read:             10 ms
Network round trip:        100-500 ms (local to intercontinental)
```

### Operations Per Second
```
Modern CPU:         ~1-2 billion operations per second (10^9)
SSD random reads:   ~100,000 operations per second (10^5)
HDD random reads:   ~1,000 operations per second (10^3)
Memory bandwidth:   ~10 GB/s on typical systems
```

### Storage
```
1 Kilobyte (KB)   = 10^3 bytes
1 Megabyte (MB)   = 10^6 bytes
1 Gigabyte (GB)   = 10^9 bytes
1 Terabyte (TB)   = 10^12 bytes
1 Petabyte (PB)   = 10^15 bytes
```

### Networking
```
1 Kilobit per second (Kbps)   = 10^3 bits/s
1 Megabit per second (Mbps)   = 10^6 bits/s
1 Gigabit per second (Gbps)   = 10^9 bits/s
Typical bandwidth:              ~1 Gbps to 10 Gbps per link
```

## Estimation Framework

### Step 1: Understand the Problem
- **What**: Type of system (social network, e-commerce, etc.)
- **Users**: How many monthly/daily active users (DAU)
- **Geography**: Global or regional?
- **Peak**: When is usage highest?

### Step 2: Calculate Daily/Peak Metrics
```
Formula: QPS = (DAU × Requests/User × Peak Multiplier) / (Seconds in a day)

Example:
DAU = 100 million
Requests per user per day = 10
Peak multiplier = 5x (peak is 5x average)

Average QPS = (100M × 10) / 86,400 ≈ 11,600 QPS
Peak QPS = 11,600 × 5 ≈ 58,000 QPS
```

### Step 3: Calculate Data Requirements

#### Database Size
```
Formula: Data Size = DAU × Data per user × Retention days

Example (Social Media):
DAU = 100 million
Data per user = 100 KB (profile, messages, metadata)
Retention = 365 days

Database Size = 100M × 100 KB × 365 ≈ 3.65 PB
```

#### Bandwidth
```
Formula: Bandwidth = QPS × Data per request

Example (Video Streaming):
QPS = 50,000
Video per request = 500 KB

Bandwidth = 50,000 × 500 KB = 25 GB/s
```

### Step 4: Calculate Server Requirements

#### Request Handling
```
Formula: Servers = QPS / (Requests per second per server)

Typical request handling per server:
- Simple request: 1,000-10,000 QPS per server
- Complex request: 100-1,000 QPS per server
- Machine learning: 10-100 QPS per server

Example:
Peak QPS = 50,000
Requests per server = 1,000
Servers needed = 50,000 / 1,000 = 50 servers
+ redundancy (2x) = 100 servers
+ growth (3x) = 300 servers for 3-year horizon
```

#### Storage (per server)
```
Formula: Total Storage = Data Size / Replication Factor

Example:
Database Size = 3.65 PB
Replication Factor = 3 (for high availability)
Shards = 1,000

Storage per shard = 3.65 PB / 1,000 = 3.65 TB
Servers per shard = 3 (replication)
Total servers = 1,000 × 3 = 3,000 servers
```

## Real-World Examples

### Example 1: Twitter-like Social Network

**Given**:
- 300 million monthly active users (MAU)
- 50% daily active users (150 million DAU)
- Average 5 posts per user per day
- Average 1,000 followers per user
- Each post: 280 characters + metadata ≈ 1 KB

**Calculations**:

1. **Tweets per day**:
   ```
   150M DAU × 5 posts = 750M tweets/day
   Average QPS = 750M / 86,400 ≈ 8,680 QPS
   Peak QPS = 8,680 × 10 (peak ratio) ≈ 86,800 QPS
   ```

2. **Database size**:
   ```
   750M tweets/day × 365 days × 1 KB = 273 TB
   With replication (3x) = 819 TB
   ```

3. **Bandwidth**:
   ```
   Each user sees ~1,000 tweets in feed daily
   150M users × 1,000 tweets × 1 KB = 150 TB/day
   150 TB / 86,400 sec ≈ 1.7 GB/s
   ```

4. **Servers needed**:
   ```
   Web servers: 86,800 QPS / 1,000 QPS per server ≈ 87 servers
   With redundancy (2x) = 174 web servers
   
   Database servers: 819 TB / 2 TB per server ≈ 410 servers
   With replication and distribution = 1,000+ database servers
   ```

### Example 2: YouTube

**Given**:
- 2 billion monthly active users
- 500 million daily active users
- Average watch time: 30 minutes per user per day
- Video quality: multiple bitrates (360p to 1080p)
- Average bitrate: 1 Mbps

**Calculations**:

1. **Video viewing QPS**:
   ```
   500M users × 30 min = 15B minutes watched/day
   15B minutes / 86,400 sec ≈ 173,611 concurrent viewers
   
   If each viewer takes 1 server:
   173,611 streaming servers needed
   + redundancy = 500,000+ servers globally
   ```

2. **Bandwidth**:
   ```
   173,611 concurrent viewers × 1 Mbps = 173,611 Mbps = 173 Gbps
   ```

3. **Storage**:
   ```
   500+ hours uploaded per minute
   500 hours × 60 min × 3 formats × 5 GB/format = 45 TB/minute
   45 TB × 60 × 24 × 365 ≈ 23.7 EB/year
   ```

## Sanity Checks

### Is Estimate Reasonable?

1. **Order of Magnitude**: Does answer seem reasonable?
   - 10 servers for 100M users? Too low
   - 1M servers for 1M users? Too high

2. **Compare with Known Systems**:
   - AWS regions have ~1000+ servers
   - Netflix has ~1000+ servers
   - Google has millions of servers

3. **Growth Trajectory**:
   - Can you handle 3-5x growth with current plan?
   - Scaling breaks at what threshold?

4. **Cost Reasonableness**:
   ```
   Server cost: $10,000-50,000 per year
   Bandwidth: $10-100 per TB
   Storage: $1,000-10,000 per TB per year
   
   Does total annual cost seem reasonable?
   ```

## Bottleneck Identification

### What Bottlenecks First?

1. **Network**: Calculate bandwidth vs available
2. **Compute**: Calculate QPS capacity vs needed
3. **Storage**: Calculate storage capacity vs needed

**Example**:
```
If bandwidth is 1 Gbps per link
And you need 10 Gbps total
Then network is bottleneck

OR if you need 1000 servers
But server cost is prohibitive
Then compute is bottleneck
```

## Growth Planning

### 3-Year Projections

```
Year 1: 100M users, baseline
Year 2: 200M users (2x growth)
Year 3: 400M users (4x growth)

Calculate for year 3:
- Database size increases 4x
- QPS increases 4x
- Need to plan for 4x resources
+ Extra capacity for sudden spikes
```

## Common Pitfalls

1. **Confusing bits and bytes**: 1 byte = 8 bits
2. **Peak vs average**: Peak is much higher (typically 5-10x)
3. **Forgetting replication**: Always account for redundancy
4. **Network is last mile**: Often overlooked bottleneck
5. **Underestimating growth**: Plan for 3-5x future growth
6. **Not accounting for caching**: Can reduce DB load by 90%

## Estimation Checklist

- [ ] Understand total users
- [ ] Calculate daily active users
- [ ] Estimate peak QPS (peak:average ratio)
- [ ] Calculate requests per user
- [ ] Estimate data per user
- [ ] Calculate database size
- [ ] Calculate required bandwidth
- [ ] Determine servers needed
- [ ] Add redundancy buffer
- [ ] Plan for 3-5x growth
- [ ] Identify bottleneck
- [ ] Sanity check numbers
- [ ] Compare with known systems

## Interview Tips

1. **Communicate assumptions**: "Assuming X..."
2. **Show work**: Walk through calculations
3. **Adjust on feedback**: If interviewer says X, recalculate
4. **Be approximate**: Use orders of magnitude
5. **Admit uncertainty**: "I'm not sure, but estimate..."

## Resources for Practice

- Use calculator for conversions
- Know key numbers (above) by heart
- Practice with different scales
- Compare your estimates with actual systems

---

**Key Takeaway**: Capacity planning is about understanding system requirements and ensuring resources match demand. Approximate calculations are better than no calculations.
