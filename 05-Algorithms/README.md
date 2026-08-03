# Algorithms in C#

This comprehensive guide covers essential algorithms for technical interviews and software development. Each algorithm category includes detailed explanations, practical examples, and clean code implementations in C#.

## Folder Structure

### 1. **01-Sorting**
Sorting algorithms with various time and space complexity trade-offs.
- **Comparison-based**: Bubble Sort, Selection Sort, Insertion Sort, Merge Sort, Quick Sort, Heap Sort
- **Non-comparison-based**: Counting Sort, Radix Sort, Bucket Sort
- Stability, adaptive sorting, and performance analysis

### 2. **02-Searching**
Efficient search algorithms for different data structures.
- **Linear Search**: Basic and variations
- **Binary Search**: Iterative and recursive implementations
- **Hash-based Search**: Hash tables and lookups
- **Advanced Search**: Interpolation search, exponential search

### 3. **03-Dynamic-Programming**
Problem-solving technique for optimization problems.
- **Fundamentals**: Memoization vs Tabulation, state definition
- **Classic Problems**: Fibonacci, Coin Change, Knapsack, LCS, LIS
- **Optimization Techniques**: Space optimization, rolling arrays
- **Advanced Patterns**: Digit DP, Tree DP, Graph DP

### 4. **04-Graph-Algorithms**
Algorithms for graph traversal and optimization.
- **Traversal**: BFS (Breadth-First Search), DFS (Depth-First Search)
- **Shortest Path**: Dijkstra, Bellman-Ford, Floyd-Warshall
- **Minimum Spanning Tree**: Kruskal, Prim
- **Advanced**: Topological Sort, Strongly Connected Components, Max Flow

### 5. **05-Tree-Algorithms**
Tree traversal, construction, and manipulation techniques.
- **Traversal**: Inorder, Preorder, Postorder, Level-order (BFS)
- **Binary Search Trees**: Insertion, Deletion, Balancing
- **Balanced Trees**: AVL Trees, Red-Black Trees
- **Advanced Trees**: Segment Trees, Fenwick Trees, Tries

### 6. **06-String-Algorithms**
String processing and pattern matching algorithms.
- **Pattern Matching**: Naive approach, KMP, Boyer-Moore, Rabin-Karp
- **String Manipulation**: Reversing, Rotation, Anagrams
- **Advanced Techniques**: Suffix Arrays, Suffix Trees, Trie-based searches
- **Interview Patterns**: Palindromes, Subsequences, Substrings

### 7. **07-Greedy-Algorithms**
Optimization using greedy strategy.
- **Classic Problems**: Activity Selection, Fractional Knapsack, Huffman Coding
- **Scheduling Problems**: Job Sequencing, Task Scheduling
- **Graph Problems**: Dijkstra's Algorithm (Greedy approach)
- **Verification**: When Greedy works and when it fails

### 8. **08-Math-Algorithms**
Mathematical computations and number theory.
- **Number Theory**: GCD, LCM, Prime numbers, Modular arithmetic
- **Combinatorics**: Permutations, Combinations, Factorial
- **Geometry**: Distance calculations, Polygon algorithms
- **Advanced**: Matrix operations, Fast Fourier Transform (FFT)

### 9. **09-Recursion**
Recursive problem-solving techniques.
- **Fundamentals**: Base case, recursive case, call stack
- **Backtracking**: N-Queens, Sudoku solver, Permutations
- **Divide and Conquer**: Merge Sort, Quick Sort, Binary Search
- **Tail Recursion**: Optimization techniques

### 10. **10-Interview-Questions**
Real interview questions organized by difficulty and category.
- **Easy**: Basic algorithm implementations
- **Medium**: Combination of multiple techniques
- **Hard**: Complex optimization problems
- **Solutions**: With explanations and multiple approaches

---

## How to Use This Guide

1. **Start with Fundamentals**: Begin with Searching and Sorting to understand basic concepts
2. **Progress to Complex Topics**: Move to Dynamic Programming and Graph Algorithms
3. **Practice Implementation**: Use the code implementations as reference
4. **Solve Interview Questions**: Test your understanding with real interview problems
5. **Optimize**: Learn complexity analysis and optimization techniques

## Complexity Analysis Reference

| Algorithm | Best Case | Average Case | Worst Case | Space |
|-----------|-----------|--------------|-----------|-------|
| Bubble Sort | O(n) | O(n²) | O(n²) | O(1) |
| Merge Sort | O(n log n) | O(n log n) | O(n log n) | O(n) |
| Quick Sort | O(n log n) | O(n log n) | O(n²) | O(log n) |
| Binary Search | O(1) | O(log n) | O(log n) | O(1) |
| Dijkstra | O(E log V) | O(E log V) | O(E log V) | O(V) |

## Key Concepts

- **Time Complexity**: Analysis of algorithm execution time
- **Space Complexity**: Memory usage of algorithm
- **Asymptotic Notation**: Big-O, Omega, Theta
- **Trade-offs**: Speed vs Space, Simplicity vs Efficiency
- **Problem-solving Approach**: Understanding problem requirements before coding

## Resources for Further Learning

- Cracking the Coding Interview by Gayle Laakmann McDowell
- Introduction to Algorithms (CLRS) by Cormen, Leiserson, Rivest, Stein
- LeetCode, HackerRank for practice
- GeeksforGeeks algorithm tutorials

---

**Last Updated**: 2026
**Focus**: Technical Interview Preparation & Practical Implementation
