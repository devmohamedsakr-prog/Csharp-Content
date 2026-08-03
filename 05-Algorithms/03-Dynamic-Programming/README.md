# Dynamic Programming

## Overview
Dynamic Programming is an optimization technique for solving problems with overlapping subproblems and optimal substructure. It avoids redundant calculations through memoization or tabulation.

## Core Principles

### 1. Optimal Substructure
- Optimal solution contains optimal solutions to subproblems
- Problem can be decomposed into independent subproblems

### 2. Overlapping Subproblems
- Same subproblem is solved multiple times
- Results can be stored and reused

### 3. State Definition
- Define what represents a unique subproblem
- Critical for DP solution design

## Approaches

### Top-Down (Memoization)
- **Style**: Recursive with caching
- **Complexity**: O(number of states) × O(work per state)
- **Advantages**: Intuitive, only compute needed states
- **Disadvantages**: Recursion overhead, stack space

### Bottom-Up (Tabulation)
- **Style**: Iterative building from base cases
- **Complexity**: Same as memoization
- **Advantages**: Faster, better space optimization possible
- **Disadvantages**: Must compute all states, harder to understand

## Classic DP Problems

### Fibonacci Sequence
- Simple base case demonstration
- Exponential vs Linear solution

### 0/1 Knapsack
- Item selection with weight constraint
- Core DP interview problem

### Coin Change
- Minimum coins for target amount
- Unbounded selection variant

### Longest Common Subsequence (LCS)
- String comparison problem
- Foundation for edit distance

### Longest Increasing Subsequence (LIS)
- Array analysis problem
- Binary search optimization possible

### Edit Distance (Levenshtein)
- String transformation cost
- Classic interview question

### Matrix Chain Multiplication
- Optimal parenthesization
- Intermediate results optimization

### Rod Cutting Problem
- Optimal division strategy
- Maximize profit with constraints

## Advanced DP Patterns

### Multi-Dimensional DP
- 2D tables for LCS, LIS variations
- 3D for complex state space

### Digit DP
- Process numbers digit by digit
- Range query optimization

### Tree DP
- Dynamic programming on tree structures
- DFS with DP on subtrees

### Bitmask DP
- Use bitmask for subset representation
- Traveling Salesman Problem (TSP)

### String DP
- Pattern matching, counting patterns
- Sequence alignment

### Probability/Expected Value DP
- Compute probabilities in games/simulations
- Markov decision processes

## Space Optimization Techniques

### Rolling Array
- Reduce space from O(n²) to O(n)
- Keep only current and previous rows

### Prefix Optimization
- Reduce recalculation of sums
- Pre-compute prefixes

### Dimension Reduction
- Recognize patterns to reduce dimensions
- Analyze dependencies between states

## Comparison with Other Approaches

| Approach | Complexity | Best For | Drawback |
|----------|-----------|----------|----------|
| Brute Force | Exponential | Small inputs | Too slow |
| Greedy | O(n log n) | Optimal substructure verified | Not always correct |
| DP (Memoization) | O(states × work) | Recursive thinkers | Stack overhead |
| DP (Tabulation) | O(states × work) | Speed critical | Must handle all states |
| Divide & Conquer | Varies | Independent subproblems | Overhead if overlap |

## Problem Classification

### Linear DP
- House Robber series
- Best Time to Buy Stock variations

### Interval DP
- Matrix chain multiplication
- Palindrome partitioning

### Knapsack Variants
- 0/1 Knapsack
- Unbounded Knapsack
- Multiple Knapsacks

### Sequence DP
- LCS, LIS, Edit Distance
- Regex matching

### Game Theory DP
- Nim game
- Minimax problems

## Interview Tips

1. **Recognize DP**: Look for overlapping subproblems
2. **Define State**: Clear state representation
3. **State Transition**: Express how to move between states
4. **Base Cases**: Identify termination conditions
5. **Implement**: Choose memoization or tabulation
6. **Optimize Space**: Reduce unnecessary storage
7. **Verify**: Test with examples

## Practice Files
- **01-Explanation**: DP theory, state definition, transitions
- **02-Examples**: Problem walkthroughs with solutions
- **03-Code-Implementations**: Multiple C# implementations with optimization
