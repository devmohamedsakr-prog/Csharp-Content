# Recursion

## Overview
Recursion is a fundamental programming technique where a function calls itself to solve smaller instances of the same problem. Mastery is essential for interviews and complex algorithm design.

## Core Concepts

### Components of Recursion

#### 1. Base Case
- **Purpose**: Stops infinite recursion
- **Critical**: Without it, stack overflow occurs
- **Design**: Simplest problem that can be solved directly

#### 2. Recursive Case
- **Purpose**: Break problem into smaller subproblems
- **Key**: Must progress toward base case
- **Pattern**: Problem(n) = operation + Problem(n-1) or similar

#### 3. Assumption
- **Trust**: Assume recursive call works correctly
- **Focus**: Only solve for current level
- **Avoid**: Don't trace entire recursion mentally

### Recursion vs Iteration
- **Recursion**: More elegant, easier to understand complex problems
- **Iteration**: More efficient, prevents stack overflow
- **Trade-off**: Readability vs Performance

## Types of Recursion

### Linear Recursion
- **Pattern**: Each call leads to at most one recursive call
- **Complexity**: O(n) time typically
- **Example**: Binary search, linear search

### Tree Recursion
- **Pattern**: Multiple recursive calls per level
- **Complexity**: Can be exponential
- **Example**: Fibonacci, N-Queens, permutations

### Mutual Recursion
- **Pattern**: Function A calls B, B calls A (directly or indirectly)
- **Use**: State machines, grammar parsing
- **Challenge**: More difficult to understand

### Tail Recursion
- **Definition**: Recursive call is last operation
- **Optimization**: Can be optimized to iteration by compiler
- **Benefit**: Constant stack space with optimization

## Classic Recursion Problems

### Fibonacci Sequence
- **Naive Recursion**: O(2^n) - exponential, many redundant calls
- **Memoization**: O(n) - cache results
- **Iteration**: O(n) - bottom-up approach
- **Optimization**: Matrix exponentiation for O(log n)

### Factorial
- **Time**: O(n)
- **Space**: O(n) call stack
- **Tail Recursive**: Last operation is return n * factorial(n-1)

### Power Function
- **Naive**: O(n) with n multiplications
- **Exponentiation by Squaring**: O(log n)
- **Key**: 2^n = (2^(n/2))²

### Tree Problems
- **Traversal**: Inorder, preorder, postorder naturally recursive
- **Search**: Binary search, balanced tree operations
- **Construction**: Build trees from descriptions

### Backtracking Problems

#### N-Queens
- **Problem**: Place n queens so none attack each other
- **Approach**: Recursive placement with constraint checking
- **Complexity**: O(n!) worst case
- **Use**: Learn recursive decision making

#### Sudoku Solver
- **Approach**: Fill cells recursively with constraint checking
- **Backtrack**: When no valid number found
- **Optimization**: Choose cell with fewest possibilities first

#### Permutations
- **Generate**: All arrangements of n elements
- **Approach**: Fix first element, recurse on rest
- **Complexity**: O(n!)

#### Combinations
- **Generate**: All selections of k from n elements
- **Approach**: Include/exclude first element
- **Complexity**: O(C(n,k))

#### Word Search
- **Problem**: Find word in letter grid
- **Approach**: DFS from each position
- **Backtrack**: Mark visited, unmark when backtracking

### Divide and Conquer

#### Binary Search
- **Divide**: Split array in half
- **Conquer**: Recurse on relevant half
- **Combine**: Not needed (solution is in recursive call)
- **Time**: O(log n)

#### Merge Sort
- **Divide**: Split array in two
- **Conquer**: Recursively sort both halves
- **Combine**: Merge sorted halves
- **Time**: O(n log n)

#### Quick Sort
- **Divide**: Partition by pivot
- **Conquer**: Recursively sort partitions
- **Combine**: Not needed (in-place)
- **Time**: O(n log n) average

## Advanced Recursion Patterns

### Dynamic Programming (Memoization)
- **Pattern**: Recursion + caching results
- **Benefit**: Avoid recomputing subproblems
- **Implementation**: Use dictionary or array for cache

### Top-Down DP
- **Approach**: Recursive with memoization
- **Intuitive**: Think recursively, cache results
- **Implementation**: Start with recursive solution, add caching

### Multiple Recursion
- **Pattern**: Recurse multiple times at same level
- **Example**: Fibonacci (2 calls), N-Queens (multiple placements)
- **Complexity**: Often exponential without optimization

## Recursion on Complex Structures

### Trees
- **Simple**: Recurse on left and right subtrees
- **Combine**: Results from both subtrees
- **Example**: Calculate tree height, sum of nodes

### Graphs
- **Challenge**: Cycles possible, need visited tracking
- **Pattern**: Mark visited, recurse on unvisited neighbors
- **Example**: DFS, connected components

### Strings
- **Pattern**: Recurse on substring (exclude first, exclude last, etc.)
- **Example**: Palindrome checking, permutations

## Recursion Optimization Techniques

### Memoization
- **Method**: Cache results of function calls
- **Data Structure**: Dictionary or array
- **Time Saved**: Avoid recomputing identical subproblems

### Tail Recursion
- **Pattern**: Recursive call as last statement
- **Compiler**: Modern compilers optimize to iteration
- **Benefit**: Prevents stack overflow

### Iterative Conversion
- **Method**: Use explicit stack to simulate recursion
- **Benefit**: Control memory, prevent stack overflow
- **Complexity**: Same but with explicit stack management

### Pruning
- **Idea**: Skip branches that can't lead to solution
- **Example**: Alpha-beta pruning in game trees
- **Benefit**: Exponential improvement for some problems

## Complexity Analysis

### Time Complexity
- **Linear Recursion**: Usually O(n) or recursive formula
- **Tree Recursion**: Often O(2^n) without memoization
- **Memoized**: O(number of states)
- **Use Master Theorem**: For divide-and-conquer recurrences

### Space Complexity
- **Call Stack**: O(depth of recursion tree)
- **Memoization Cache**: O(number of states)
- **Total**: Sum of both

### Master Theorem
- **Formula**: T(n) = aT(n/b) + f(n)
- **Cases**: Three cases based on f(n) vs n^(log_b a)
- **Example**: Merge Sort: a=2, b=2, f(n)=n → O(n log n)

## Common Mistakes

### Stack Overflow
- **Cause**: Base case never reached or infinite recursion
- **Fix**: Verify base case, ensure progress toward it

### Redundant Computation
- **Cause**: Same subproblem solved multiple times
- **Fix**: Use memoization

### Memory Leak
- **Cause**: Circular references in recursion
- **Fix**: Careful design, avoid circular structures

### Hard to Debug
- **Cause**: Difficult to trace recursive calls
- **Fix**: Add logging, understand at one level at a time

## Interview Tips

1. **Start Simple**: Identify base case first
2. **Verify Progress**: Ensure recursive case reaches base case
3. **Don't Over-Think**: Trust recursion, solve one level
4. **Memoization**: Consider for optimization
5. **Test**: Simple cases, edge cases
6. **Convert**: Be ready to convert to iteration if needed

## Practice Files
- **01-Explanation**: Recursion theory, types, optimization
- **02-Examples**: Classic problems with explanations
- **03-Code-Implementations**: C# recursive and iterative implementations
