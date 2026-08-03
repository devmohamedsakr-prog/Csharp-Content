# Tree Algorithms

## Overview
Trees are hierarchical data structures critical for many applications. Understanding tree algorithms is essential for interviews and practical development.

## Tree Basics

### Terminology
- **Root**: Top node with no parent
- **Leaf**: Node with no children
- **Parent/Child**: Directional relationships
- **Sibling**: Nodes sharing parent
- **Height**: Longest path from root to leaf
- **Depth**: Distance from root to node
- **Subtree**: Tree formed by node and descendants

### Types of Trees
- **Binary Tree**: Max 2 children per node
- **Binary Search Tree (BST)**: Ordered, left < parent < right
- **Balanced Trees**: AVL, Red-Black (maintain height balance)
- **N-ary Trees**: Multiple children per node
- **Heap**: Complete binary tree with heap property
- **Trie**: Prefix tree for strings
- **Segment Tree**: For range queries
- **Fenwick Tree**: Binary Indexed Tree for prefix sums

## Traversal Algorithms

### Depth-First Traversals
- **Inorder** (Left-Root-Right): BST gives sorted order
- **Preorder** (Root-Left-Right): Root processed first
- **Postorder** (Left-Right-Root): Root processed last
- **All**: O(n) time, O(h) space (h = height)

### Breadth-First Traversal
- **Level Order**: Process nodes level by level
- **Time**: O(n)
- **Space**: O(width) - maximum nodes at any level
- **Implementation**: Queue-based

### Morris Traversal
- **Space**: O(1) without recursion stack
- **Time**: O(n)
- **Use**: Threaded binary trees, constant space requirement

## Binary Search Tree Operations

### Search
- **Time**: O(log n) average, O(n) worst
- **Implementation**: Compare and recurse left/right

### Insertion
- **Time**: O(log n) average, O(n) worst
- **Maintain**: BST property with new node

### Deletion
- **Cases**: 
  - No children: Remove directly
  - One child: Replace with child
  - Two children: Find in-order successor/predecessor
- **Time**: O(log n) average, O(n) worst

### Balancing
- Rotation operations to maintain height balance
- Critical for worst-case performance

## Balanced Trees

### AVL Trees
- **Balance Factor**: Height difference ≤ 1
- **Rotations**: Single (LL, RR) or Double (LR, RL)
- **Operations**: O(log n) guaranteed
- **Use**: When strict balance critical

### Red-Black Trees
- **Properties**: Color-based balance
- **Operations**: O(log n) with simpler rotations
- **Use**: More practical than AVL for most cases

### B-Trees
- **Node Capacity**: Multiple keys per node
- **Use**: Disk-based storage, databases
- **Balance**: Automatically maintained

## Advanced Tree Structures

### Segment Trees
- **Purpose**: Range queries and updates
- **Time**: O(log n) per operation
- **Use**: Sum/min/max over intervals
- **Space**: O(n)

### Fenwick Tree (Binary Indexed Tree)
- **Purpose**: Prefix sum queries and updates
- **Time**: O(log n) per operation
- **Space**: O(n)
- **Advantage**: Simpler than Segment Tree for some queries

### Tries (Prefix Trees)
- **Purpose**: String prefix matching
- **Time**: O(m) where m = string length
- **Space**: O(alphabet size × average depth)
- **Use**: Autocomplete, spell checking, IP routing

### Suffix Trees/Arrays
- **Purpose**: Pattern matching, string operations
- **Time**: O(m + n) suffix tree, O(m log n) suffix array
- **Use**: Longest repeated substring, all occurrences

## Common Tree Problems

### Path Finding
- Path sum from root to leaf
- Longest/shortest paths
- K-sum paths

### Lowest Common Ancestor (LCA)
- Binary lifting: O(log n) after O(n log n) preprocess
- Tarjan's Algorithm: O(n + q) for q queries
- Simple DFS: O(n) per query

### Construction
- Build tree from traversals (inorder + preorder/postorder)
- Build tree from sorted array (balanced BST)

### Views
- Top view, bottom view, left view, right view
- Vertical order traversal
- Diagonal traversal

### Properties
- Diameter (longest path between any two nodes)
- Height balanced check
- Symmetric/identical tree check
- Validate BST property

## Comparison of Tree Types

| Tree Type | Search | Insert | Delete | Space | Use Case |
|-----------|--------|--------|--------|-------|----------|
| Binary Tree | O(n) | O(1) | O(1) | O(n) | General structure |
| BST | O(log n)† | O(log n)† | O(log n)† | O(n) | Simple ordered storage |
| AVL Tree | O(log n) | O(log n) | O(log n) | O(n) | Strict balance required |
| Red-Black | O(log n) | O(log n) | O(log n) | O(n) | Practical balanced tree |
| Heap | O(n) | O(log n) | O(log n) | O(n) | Priority queue |
| Trie | O(m) | O(m) | O(m) | O(alphabet×n) | Prefix matching |
| Segment Tree | O(log n) | O(log n) | O(log n) | O(n) | Range queries |

† Average case; worst case O(n) if unbalanced

## Complexity Reference

| Operation | Binary Tree | BST (avg) | Balanced | Trie | Segment Tree |
|-----------|------------|----------|---------|------|--------------|
| Search | O(n) | O(log n) | O(log n) | O(m) | O(log n) |
| Insert | O(1) | O(log n) | O(log n) | O(m) | O(log n) |
| Delete | O(n) | O(log n) | O(log n) | O(m) | O(log n) |
| Range Query | O(n) | O(n) | O(n) | N/A | O(log n) |
| Space | O(n) | O(n) | O(n) | O(alphabet×depth) | O(n) |

## Interview Tips

1. **Clarify Tree Type**: BST? Balanced? Special properties?
2. **Traversal Choice**: Which traversal fits problem?
3. **Recursive vs Iterative**: Trade-offs in clarity and space
4. **Edge Cases**: Null nodes, single node, skewed tree
5. **Balance Consideration**: When is balancing necessary?
6. **Space Optimization**: Can Morris traversal help?

## Practice Files
- **01-Explanation**: Tree theory, traversals, operations
- **02-Examples**: Tree problems with walkthroughs
- **03-Code-Implementations**: C# implementations of all tree types and algorithms
