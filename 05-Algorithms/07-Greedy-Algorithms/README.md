# Greedy Algorithms

## Overview
Greedy algorithms make locally optimal choices at each step, hoping to find a global optimum. They're efficient but don't always guarantee optimal solutions.

## Core Principles

### 1. Greedy Choice Property
- Making locally optimal choice leads to globally optimal solution
- Decision doesn't depend on future choices

### 2. Optimal Substructure
- Optimal solution contains optimal solutions to subproblems
- Not unique to greedy; also applies to DP

### 3. When Greedy Works
- Problem has both greedy choice and optimal substructure
- No dependencies between decisions
- Proof by exchange argument often possible

### 4. When Greedy Fails
- Interdependencies between choices
- Needs to look ahead (DP required)
- Counter-examples can disprove greedy optimality

## Classic Greedy Problems

### Selection Problems

#### Activity Selection
- **Problem**: Select maximum non-overlapping activities
- **Solution**: Sort by end time, greedily pick earliest ending
- **Time**: O(n log n)
- **Proof**: Earliest end leaves most room for remaining activities

#### Interval Scheduling
- **Problem**: Schedule maximum non-overlapping intervals
- **Solution**: Same as activity selection
- **Weighted Variant**: Use DP instead

#### Job Sequencing with Deadlines
- **Problem**: Maximize profit with deadline constraints
- **Solution**: Sort by profit, fill latest available slot
- **Time**: O(n²) with array, O(n log n) with Union-Find

### Fractional Knapsack
- **Problem**: Maximize value with weight limit, fractional items allowed
- **Solution**: Sort by value/weight ratio, fill greedily
- **Time**: O(n log n)
- **Note**: 0/1 knapsack requires DP

### Huffman Coding
- **Problem**: Optimal prefix-free code with minimum length
- **Algorithm**: Build tree bottom-up using min-heap
- **Time**: O(n log n)
- **Application**: Lossless compression

### Minimum Spanning Tree (MST)

#### Kruskal's Algorithm
- **Approach**: Greedy edge selection by weight
- **Time**: O(E log E)
- **Structure**: Union-Find to avoid cycles

#### Prim's Algorithm
- **Approach**: Greedy vertex expansion
- **Time**: O((V+E) log V) with priority queue
- **Advantage**: Better for dense graphs

### Dijkstra's Shortest Path
- **Approach**: Greedily pick nearest unvisited vertex
- **Time**: O((V+E) log V) with priority queue
- **Constraint**: Non-negative weights

## Scheduling Problems

### Greedy Approaches for Scheduling
- **FIFO (First In First Out)**: Simple, not always optimal
- **SJF (Shortest Job First)**: Minimize average wait time
- **Priority-Based**: Handle different priorities
- **EDF (Earliest Deadline First)**: Real-time systems

### Load Balancing
- **Problem**: Distribute tasks across machines
- **Greedy**: Assign to least loaded machine
- **Time**: O(n)
- **Note**: Not always optimal, approximation algorithm

## Approximation Algorithms

### When Optimal is Intractable
- NP-complete problems often use greedy for approximation
- Measure: Ratio of greedy solution to optimal

### Examples
- **Vertex Cover**: 2-approximation possible
- **Set Cover**: O(log n)-approximation
- **TSP (General)**: No constant approximation
- **TSP (Metric)**: 1.5-approximation (Christofides)

## Problem Patterns

### Stay Ahead Argument
- Greedy choice always as good as any other choice
- Example: Activity selection, interval scheduling

### Exchange Argument
- Any optimal solution can be transformed to greedy
- Maintains optimality after exchanges
- Example: Huffman coding

### Greedy Over Permutations
- Sort in specific order, then greedy selection
- Example: Job sequencing, fractional knapsack

## Complexity Reference

| Problem | Approach | Time | Optimal? |
|---------|----------|------|----------|
| Activity Selection | Greedy | O(n log n) | Yes |
| Fractional Knapsack | Greedy | O(n log n) | Yes |
| 0/1 Knapsack | DP Required | O(nW) | Yes |
| MST (Kruskal) | Greedy | O(E log E) | Yes |
| Dijkstra | Greedy | O((V+E) log V) | Yes |
| Job Sequencing | Greedy | O(n²) | Yes |
| Huffman Coding | Greedy | O(n log n) | Yes |
| General TSP | Greedy | O(n²) | Not always |

## Decision Framework

### Use Greedy When:
1. Problem has greedy choice property
2. Can prove optimal substructure
3. Local optimality implies global optimality
4. Need efficient solution over optimal

### Use DP When:
1. Greedy doesn't work
2. Overlapping subproblems exist
3. Need guaranteed optimal solution
4. Complexity acceptable (exponential → polynomial)

### Use Other Approaches When:
1. Problem is NP-complete
2. Approximation acceptable
3. Heuristics needed
4. Exact solution not feasible

## Interview Tips

1. **Identify Structure**: Does greedy choice property exist?
2. **Consider Counter-examples**: Find cases where greedy fails
3. **Prove Correctness**: Exchange or stay-ahead argument
4. **Analyze Complexity**: Often simpler than DP
5. **Test Edge Cases**: Tie-breaking, boundary conditions
6. **Be Skeptical**: Many problems look greedy but aren't

## Practice Files
- **01-Explanation**: Greedy theory, proof techniques, when to use
- **02-Examples**: Problems with greedy and non-greedy solutions
- **03-Code-Implementations**: C# implementations of greedy algorithms
