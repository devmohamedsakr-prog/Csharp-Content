# Sorting Algorithms

## Overview
Sorting algorithms arrange elements in a specific order (ascending or descending). This is one of the most fundamental concepts in computer science with numerous practical applications.

## Categories

### Comparison-Based Sorting
Algorithms that compare elements to determine order.

**Simple Sorts:**
- Bubble Sort - O(n²) - Easy to understand, rarely used in practice
- Selection Sort - O(n²) - Minimum element selection
- Insertion Sort - O(n²) - Online sorting, good for small datasets

**Efficient Sorts:**
- Merge Sort - O(n log n) - Divide and conquer, stable
- Quick Sort - O(n log n) average - In-place partitioning
- Heap Sort - O(n log n) - Heap data structure based

### Non-Comparison Based Sorting
Algorithms that don't compare elements directly.

- Counting Sort - O(n + k) - For integer ranges
- Radix Sort - O(d × n) - Digit-by-digit sorting
- Bucket Sort - O(n + k) - Distribution-based

## Key Concepts

### Stability
An algorithm is stable if equal elements maintain their relative order.
- Stable: Bubble Sort, Insertion Sort, Merge Sort
- Unstable: Quick Sort, Heap Sort, Selection Sort

### Adaptivity
How well algorithm performs on partially sorted data.
- Adaptive: Bubble Sort, Insertion Sort
- Non-adaptive: Merge Sort, Heap Sort

### In-Place Sorting
Algorithms that don't require extra space for another copy of data.
- In-place: Quick Sort, Heap Sort, Insertion Sort
- Not In-place: Merge Sort, Counting Sort

## Complexity Comparison

| Algorithm | Best | Average | Worst | Space | Stable | In-Place |
|-----------|------|---------|-------|-------|--------|----------|
| Bubble | O(n) | O(n²) | O(n²) | O(1) | Yes | Yes |
| Selection | O(n²) | O(n²) | O(n²) | O(1) | No | Yes |
| Insertion | O(n) | O(n²) | O(n²) | O(1) | Yes | Yes |
| Merge | O(n log n) | O(n log n) | O(n log n) | O(n) | Yes | No |
| Quick | O(n log n) | O(n log n) | O(n²) | O(log n) | No | Yes |
| Heap | O(n log n) | O(n log n) | O(n log n) | O(1) | No | Yes |
| Counting | O(n+k) | O(n+k) | O(n+k) | O(k) | Yes | No |
| Radix | O(d×n) | O(d×n) | O(d×n) | O(n) | Yes | No |

## When to Use

- **Bubble Sort**: Educational purposes only
- **Selection Sort**: When memory writes are expensive
- **Insertion Sort**: Small datasets (< 50 elements)
- **Merge Sort**: Need stability, linked lists, external sorting
- **Quick Sort**: General purpose, good cache locality
- **Heap Sort**: When worst-case O(n log n) is required, in-place
- **Counting Sort**: Small range of integers
- **Radix Sort**: Integers or strings with fixed length

## Practice Files
- **01-Explanation**: Detailed theory and algorithm descriptions
- **02-Examples**: Sample inputs and outputs
- **03-Code-Implementations**: Complete C# implementations
