# Graph Algorithms

## Overview
Graph algorithms are fundamental for solving problems on networks, relationships, and connected structures. Mastery is essential for system design and complex problem-solving.

## Graph Basics

### Representations
- **Adjacency Matrix**: Space O(V²), lookup O(1)
- **Adjacency List**: Space O(V+E), traversal efficient
- **Edge List**: Simple, good for sparse graphs

### Types
- **Directed vs Undirected**: Direction of connections
- **Weighted vs Unweighted**: Edge weights
- **Cyclic vs Acyclic**: Presence of cycles
- **Dense vs Sparse**: Ratio of edges to vertices

## Fundamental Traversals

### Breadth-First Search (BFS)
- **Time**: O(V + E)
- **Space**: O(V)
- **Use**: Shortest path (unweighted), level-order, connectivity
- **Implementation**: Queue-based

### Depth-First Search (DFS)
- **Time**: O(V + E)
- **Space**: O(V) stack space
- **Use**: Topological sort, cycle detection, connected components
- **Implementation**: Recursive or stack-based

## Shortest Path Algorithms

### Dijkstra's Algorithm
- **Time**: O((V + E) log V) with priority queue
- **Constraint**: Non-negative weights
- **Use**: Single-source shortest path
- **Greedy approach**: Always pick minimum distance

### Bellman-Ford Algorithm
- **Time**: O(V × E)
- **Constraint**: No negative cycles (can detect them)
- **Use**: Negative weight handling
- **Dynamic Programming**: Based on relaxation

### Floyd-Warshall Algorithm
- **Time**: O(V³)
- **Purpose**: All-pairs shortest path
- **Constraint**: No negative cycles
- **Use**: Dense graphs, moderate V

### A* Search
- **Time**: Depends on heuristic
- **Use**: Path finding with heuristic guidance
- **Heuristic**: Domain-specific optimization

## Minimum Spanning Tree (MST)

### Kruskal's Algorithm
- **Time**: O(E log E)
- **Approach**: Greedy edge selection with Union-Find
- **Best For**: Sparse graphs
- **Property**: Finds MST without cycles

### Prim's Algorithm
- **Time**: O((V + E) log V)
- **Approach**: Greedy vertex expansion
- **Best For**: Dense graphs
- **Implementation**: Priority queue optimized

## Advanced Algorithms

### Topological Sorting
- **Time**: O(V + E)
- **Use**: Task scheduling, dependency resolution
- **Constraint**: Directed acyclic graph (DAG)
- **Variations**: DFS-based, Kahn's algorithm

### Strongly Connected Components (SCC)
- **Time**: O(V + E)
- **Algorithms**: Kosaraju, Tarjan
- **Use**: Finding cycles, condensation graph
- **Application**: Network analysis

### Bipartite Checking
- **Time**: O(V + E)
- **Method**: BFS/DFS coloring
- **Use**: 2-coloring problems, matching

### Cycle Detection
- **Undirected**: DFS/BFS with parent tracking
- **Directed**: DFS with recursion stack
- **Time**: O(V + E)

### Maximum Flow / Min Cut
- **Ford-Fulkerson**: O(E × max flow)
- **Edmonds-Karp**: O(V × E²)
- **Dinic's**: O(V² × E)
- **Use**: Network flow, bipartite matching

## Problem Patterns

### Connectivity Problems
- Connected components
- Bridges, articulation points
- Cycle detection

### Shortest Path Problems
- Single-source, single-target
- All-pairs, multi-source
- Weighted and unweighted variants

### Matching Problems
- Bipartite matching (maximum flow)
- General matching
- Assignment problem

### Flow Problems
- Maximum flow
- Min-cost max flow
- Circulation problems

### Coloring Problems
- Graph coloring
- Bipartite checking
- Chromatic number

## Special Graph Types

### Trees
- No cycles, V-1 edges
- Rooted vs unrooted
- Ancestor queries, path finding

### DAGs
- Directed acyclic graphs
- Topological ordering
- Longest/shortest path in DAG

### Bipartite Graphs
- 2-colorable
- Matching algorithms
- Cover problems

### Trees/Forests
- Union-Find structure
- Hierarchical relationships

## Complexity Reference

| Algorithm | Time | Space | Best For |
|-----------|------|-------|----------|
| BFS | O(V+E) | O(V) | Unweighted SP, connectivity |
| DFS | O(V+E) | O(V) | Cycle detection, SCC, topo sort |
| Dijkstra | O(ElogV) | O(V) | Non-negative weights |
| Bellman-Ford | O(VE) | O(V) | Negative weights allowed |
| Floyd-Warshall | O(V³) | O(V²) | All-pairs, dense |
| Kruskal | O(ElogE) | O(E) | Sparse MST |
| Prim | O(ElogV) | O(V) | Dense MST |

## Interview Strategies

1. **Clarify Graph Type**: Directed? Weighted? Cyclic?
2. **Choose Representation**: Matrix vs List based on density
3. **Identify Pattern**: Shortest path? Connectivity? Cycle?
4. **Select Algorithm**: Match problem to algorithm
5. **Optimize**: Greedy vs DP vs BFS/DFS
6. **Code**: Clean implementation with edge cases

## Practice Files
- **01-Explanation**: Graph theory, algorithm details, complexity analysis
- **02-Examples**: Graph problems with walkthroughs
- **03-Code-Implementations**: C# implementations of all algorithms
