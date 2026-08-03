# Sorting Algorithms Overview

## Introduction

Sorting is the process of arranging elements in a specific order. It's one of the most fundamental operations in computer science with applications in databases, search engines, graphics, and more.

## Why Learn Sorting?

1. **Foundation**: Basis for other algorithms (binary search, merge operations)
2. **Performance**: Understanding sorting helps optimize systems
3. **Interview**: Very common interview topic
4. **Problem Solving**: Many problems reduce to sorting

## Sorting Problem Statement

**Input**: Array/list of comparable elements
**Output**: Same elements arranged in order (ascending or descending)
**Constraint**: Generally preserve order of equal elements (stability)

## Classification of Sorting Algorithms

### By Comparison Method
- **Comparison-Based**: Compare elements to determine order
  - Bubble, Selection, Insertion, Merge, Quick, Heap
  - Lower bound: O(n log n) for worst case
- **Non-Comparison**: Exploit properties of elements
  - Counting, Radix, Bucket
  - Can achieve O(n) or O(n + k)

### By Space Usage
- **In-Place**: Use O(1) or O(log n) extra space
  - Quick Sort, Heap Sort, Insertion Sort
- **Not In-Place**: Require O(n) extra space
  - Merge Sort, Counting Sort

### By Stability
- **Stable**: Equal elements maintain relative order
  - Bubble, Insertion, Merge, Counting, Radix
- **Unstable**: Equal elements may reorder
  - Quick Sort, Heap Sort, Selection Sort

### By Approach
- **Divide & Conquer**: Merge Sort, Quick Sort
- **Selection**: Selection Sort, Heap Sort
- **Insertion**: Insertion Sort, Shell Sort
- **Exchange**: Bubble Sort
- **Distribution**: Counting Sort, Radix Sort, Bucket Sort

## Complexity Analysis

### Time Complexity
- **Best Case**: Favorable conditions (e.g., already sorted)
- **Average Case**: Random input
- **Worst Case**: Unfavorable conditions (e.g., reverse sorted)

### Space Complexity
- **Auxiliary Space**: Extra memory beyond input
- **In-Place**: Minimal extra space

## Comparison Table

| Algorithm | Best | Average | Worst | Space | Stable | In-Place |
|-----------|------|---------|-------|-------|--------|----------|
| Bubble | O(n) | O(n²) | O(n²) | O(1) | Yes | Yes |
| Selection | O(n²) | O(n²) | O(n²) | O(1) | No | Yes |
| Insertion | O(n) | O(n²) | O(n²) | O(1) | Yes | Yes |
| Shell | O(n) | O(n log n) | O(n²) | O(1) | No | Yes |
| Merge | O(n log n) | O(n log n) | O(n log n) | O(n) | Yes | No |
| Quick | O(n log n) | O(n log n) | O(n²) | O(log n) | No | Yes |
| Heap | O(n log n) | O(n log n) | O(n log n) | O(1) | No | Yes |
| Counting | O(n+k) | O(n+k) | O(n+k) | O(k) | Yes | No |
| Radix | O(d·n) | O(d·n) | O(d·n) | O(n) | Yes | No |
| Bucket | O(n+k) | O(n+k) | O(n²) | O(n) | Yes | No |

## When to Use Each Algorithm

### Simple Datasets (< 50 elements)
**Insertion Sort** is best
- Simple to implement
- Low overhead
- Optimal for small data

### Random Unordered Data
**Quick Sort** is typical choice
- Fast on average
- Good cache locality
- Practical performance

### Need Guaranteed O(n log n)
**Merge Sort** or **Heap Sort**
- Predictable performance
- Merge Sort also stable
- Heap Sort in-place

### Small Range of Integers
**Counting Sort**
- Linear time
- Simple implementation
- Not comparison-based

### Very Large Integers/Strings
**Radix Sort**
- Process digit by digit
- Linear time
- Stable

### Need Stability
**Merge Sort** or **Insertion Sort**
- Preserve relative order
- Merge Sort for large data
- Insertion Sort for small data

## Practical Considerations

### Real-World Performance
- **Constants Matter**: O(n²) with small constant can beat O(n log n) for small n
- **Memory**: In-place vs external memory
- **Cache Locality**: Quick Sort has better cache behavior
- **Processor**: Modern CPUs favor certain patterns

### Language/Framework
- **C# (.NET)**: Arrays.Sort uses Introsort (Quick Sort + Heap Sort hybrid)
- **Java**: Arrays.sort uses Dual-Pivot Quick Sort
- **Python**: Timsort (Merge Sort + Insertion Sort hybrid)
- **JavaScript**: V8 uses Quicksort (arrays) and Mergesort (typed arrays)

## Optimization Techniques

### Hybrid Approaches
- **Introsort**: Quick Sort, switch to Heap Sort if depth exceeds limit
- **Timsort**: Merge Sort for long runs + Insertion Sort for small runs
- **3-Way Quick Sort**: Handle duplicates efficiently

### Adaptive Sorting
- Exploit partially sorted data
- Insertion Sort excellent for nearly sorted
- Binary Insertion Sort reduces comparisons

### Parallel Sorting
- Multi-threaded merge sort
- Parallel quick sort
- Sample-based partitioning

## Interview Tips

1. **Choose Wisely**: Understand requirements (stability, space, time)
2. **Implement Cleanly**: Clear code is more important than optimization
3. **Explain**: Walk through simple example while coding
4. **Complexity**: Always discuss time and space
5. **Edge Cases**: Empty array, single element, duplicates
6. **Improvements**: Be ready to optimize or stabilize

## Common Mistakes

1. **Off-by-one errors**: Loop boundaries critical
2. **Forgetting base case**: Recursion must terminate
3. **In-place modification**: Can overwrite data incorrectly
4. **Stability**: Stable sorting matters for multi-key sorting
5. **Performance assumptions**: Test, don't assume

## Practice Problems

### Easy
- Sort array of integers
- Sort in descending order
- Stable sort with custom comparator

### Medium
- k largest elements
- Merge sorted arrays
- Sort by multiple keys

### Hard
- Merge k sorted lists
- Median of two sorted arrays
- Sort with custom comparator (complex)

## Next Steps

1. Study simple sorts: Bubble, Selection, Insertion
2. Practice implementation by hand
3. Learn efficient sorts: Merge, Quick, Heap
4. Study non-comparison sorts: Counting, Radix
5. Understand hybrid approaches used in practice
