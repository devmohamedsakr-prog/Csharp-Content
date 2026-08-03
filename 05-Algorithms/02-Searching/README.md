# Searching Algorithms

## Overview
Searching algorithms find the presence or location of a target value in a collection. Efficiency depends on the data organization and search constraints.

## Categories

### Linear Search
- **Time**: O(n)
- **Use When**: Unsorted data, small datasets, linked lists
- **Implementation**: Sequential comparison

### Binary Search
- **Time**: O(log n)
- **Prerequisite**: Sorted data
- **Variants**: 
  - Standard binary search
  - Finding first/last occurrence
  - Finding closest element
  - Search in rotated sorted array

### Hash-Based Search
- **Time**: O(1) average, O(n) worst
- **Use When**: Fast lookup needed
- **Applications**: Hash tables, hash sets

### Advanced Search Techniques
- **Interpolation Search**: O(log log n) average on uniform data
- **Exponential Search**: O(log n) for unbounded arrays
- **Jump Search**: O(√n) for sorted data
- **Ternary Search**: O(log₃ n) for sorted data

## Time Complexity Comparison

| Algorithm | Best | Average | Worst | Space | Prerequisites |
|-----------|------|---------|-------|-------|----------------|
| Linear | O(1) | O(n) | O(n) | O(1) | None |
| Binary | O(1) | O(log n) | O(log n) | O(1) | Sorted |
| Jump | O(1) | O(√n) | O(√n) | O(1) | Sorted |
| Interpolation | O(1) | O(log log n) | O(n) | O(1) | Sorted, Uniform |
| Exponential | O(1) | O(log n) | O(log n) | O(1) | Sorted |
| Ternary | O(1) | O(log₃ n) | O(log₃ n) | O(1) | Sorted |
| Hash | O(1) | O(1) | O(n) | O(n) | Hash function |

## Binary Search Variants

### Standard Binary Search
Find existence of target in sorted array.

### Find First Occurrence
Find leftmost position of target element.

### Find Last Occurrence
Find rightmost position of target element.

### Search Insert Position
Where to insert element to maintain sorted order.

### Search in Rotated Array
Handle arrays rotated at pivot point.

### Peak Element Finding
Find local maximum in array.

## Key Concepts

### Search Space Reduction
- Binary search cuts search space in half each iteration
- Crucial for interviewing questions

### Boundary Conditions
- Off-by-one errors common in implementation
- Edge cases: empty array, single element, target not found

### Sorted vs Unsorted Data
- Sorting enables faster search at cost of preprocessing
- Trade-off between one-time sort vs multiple searches

## When to Use

- **Linear Search**: Unsorted, small data, linked lists
- **Binary Search**: Sorted data, frequent searches
- **Jump Search**: Sorted data, sequential access preferred
- **Interpolation**: Uniformly distributed sorted data
- **Exponential**: Unbounded or very large datasets
- **Hash Search**: Key-value lookups, in-memory storage

## Practice Files
- **01-Explanation**: Search algorithm theory and analysis
- **02-Examples**: Sample search scenarios
- **03-Code-Implementations**: C# implementations with variants
