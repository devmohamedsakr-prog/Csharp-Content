# Binary Search: Complete Guide

## Overview

Binary Search is one of the most important search algorithms. It finds a target value in a **sorted array** by repeatedly dividing the search interval in half.

## Key Properties

- **Time Complexity**: O(log n)
- **Space Complexity**: O(1) iterative, O(log n) recursive
- **Prerequisite**: Array must be sorted
- **Stability**: Not applicable (returns index)

## How Binary Search Works

### Concept
```
Sorted Array: [1, 3, 5, 7, 9, 11, 13, 15, 17, 19]
Target: 7

Step 1: Check middle (9)
  9 > 7, so search left half
  
Step 2: Check middle of left half (5)
  5 < 7, so search right of this
  
Step 3: Check middle (7)
  7 == 7, FOUND!
```

### Algorithm
```
1. Initialize left = 0, right = n - 1
2. While left <= right:
   a. mid = (left + right) / 2
   b. If arr[mid] == target: return mid
   c. Else if arr[mid] < target: left = mid + 1
   d. Else: right = mid - 1
3. Not found: return -1
```

## Implementation Examples

### Iterative Binary Search
```csharp
public int BinarySearch(int[] arr, int target)
{
    int left = 0;
    int right = arr.Length - 1;
    
    while (left <= right)
    {
        int mid = left + (right - left) / 2;
        
        if (arr[mid] == target)
            return mid;
        else if (arr[mid] < target)
            left = mid + 1;
        else
            right = mid - 1;
    }
    
    return -1; // Not found
}
```

### Recursive Binary Search
```csharp
public int BinarySearchRecursive(int[] arr, int target, int left, int right)
{
    if (left > right)
        return -1;
    
    int mid = left + (right - left) / 2;
    
    if (arr[mid] == target)
        return mid;
    else if (arr[mid] < target)
        return BinarySearchRecursive(arr, target, mid + 1, right);
    else
        return BinarySearchRecursive(arr, target, left, mid - 1);
}
```

## Common Variations

### 1. Find First Occurrence
**Problem**: Array has duplicates, find leftmost occurrence
```
Input: [5, 5, 5, 5, 7, 9, 11]
Target: 5
Output: 0 (first index)
```

**Approach**:
```csharp
public int FindFirst(int[] arr, int target)
{
    int left = 0, right = arr.Length - 1;
    int result = -1;
    
    while (left <= right)
    {
        int mid = left + (right - left) / 2;
        
        if (arr[mid] == target)
        {
            result = mid;
            right = mid - 1; // Continue searching left
        }
        else if (arr[mid] < target)
            left = mid + 1;
        else
            right = mid - 1;
    }
    
    return result;
}
```

### 2. Find Last Occurrence
**Problem**: Find rightmost occurrence of target
```
Input: [5, 5, 5, 5, 7, 9, 11]
Target: 5
Output: 3 (last index)
```

**Approach**: Similar to first occurrence, but continue searching right

### 3. Search Insert Position
**Problem**: Find where to insert target to keep array sorted
```
Input: [1, 3, 5, 6]
Target: 5
Output: 2 (target exists at 2)

Input: [1, 3, 5, 6]
Target: 4
Output: 2 (insert at 2)
```

**Approach**: When not found, left pointer indicates insert position

### 4. Search in Rotated Sorted Array
**Problem**: Array is sorted but rotated
```
Input: [4, 5, 6, 7, 0, 1, 2]
Target: 0
Output: 4

Approach: Find rotation pivot, then binary search
```

### 5. Peak Element Finding
**Problem**: Find local maximum (not applicable to simple sorted)
```
Input: [1, 3, 4, 2, 5]
Output: 2 (index of 4)

Can use binary search variant
```

## Why Avoid Common Mistakes

### Mistake 1: Integer Overflow
**Wrong**: `int mid = (left + right) / 2;`
**Fixed**: `int mid = left + (right - left) / 2;`

Reason: left + right can overflow in languages like Java/C#

### Mistake 2: Off-by-One Errors
**Wrong**: `while (left < right)` - misses last element
**Fixed**: `while (left <= right)` - includes all elements

### Mistake 3: Infinite Loop
**Wrong**: `mid = (left + right) / 2;` then never updating left/right
**Fixed**: Always ensure left or right moves each iteration

## Time Complexity Analysis

### Why O(log n)?

After each iteration, search space is halved:
```
n → n/2 → n/4 → n/8 → ... → 1

Number of halvings = log₂(n)
Time Complexity = O(log n)
```

### Example with n = 16:
```
Iteration 1: 16 elements left
Iteration 2: 8 elements left
Iteration 3: 4 elements left
Iteration 4: 2 elements left
Iteration 5: 1 element left

Maximum iterations = 5 ≈ log₂(16) = 4 (with base)
```

## When to Use Binary Search

### Prerequisites
- Array must be sorted
- Random access to elements (arrays, not linked lists)

### Use Cases
1. **Finding elements**: Existence check
2. **Insertion position**: Where to insert new element
3. **Boundary finding**: First/last occurrence
4. **Range searching**: Count elements in range

### Real-World Applications
- Database indexing
- Library search systems
- Auto-complete suggestions
- Version control systems (finding specific version)

## Comparison with Linear Search

| Aspect | Linear | Binary |
|--------|--------|--------|
| Prerequisite | None | Sorted |
| Time | O(n) | O(log n) |
| Best Case | O(1) | O(1) |
| Implementation | Simple | More complex |
| When Beneficial | Small n, unsorted | Large n, sorted |

## Advanced Topics

### Binary Search on Answer
**Concept**: Binary search for feasible value, not direct answer
```
Problem: Find minimum speed to reach destination
- Binary search on speed value
- Check if speed sufficient
- Adjust based on feasibility
```

### Ternary Search
**Concept**: Divide into 3 parts instead of 2
```
Use case: Unimodal function (single peak)
Time: O(log₃ n) ≈ O(log n)
```

## Practice Problems

### Easy
1. Implement basic binary search
2. Find first/last occurrence
3. Search insert position

### Medium
1. Search in rotated array
2. Peak element finding
3. Binary search on answer

### Hard
1. Median of two sorted arrays
2. Smallest missing number
3. Minimum time to make bouquet

## Interview Tips

1. **Verify Sorted**: Always ask if array is sorted
2. **Edge Cases**: Test empty, single element, not found
3. **Walk Through**: Trace through simple example before coding
4. **Explain**: Clearly state time/space complexity
5. **Variations**: Be ready for variations (first occurrence, etc.)

## Practice Checklist

- [ ] Implement iterative binary search
- [ ] Implement recursive binary search
- [ ] Handle first/last occurrence
- [ ] Find search insert position
- [ ] Search in rotated array
- [ ] Peak element in array
- [ ] Explain time complexity
- [ ] Handle all edge cases
