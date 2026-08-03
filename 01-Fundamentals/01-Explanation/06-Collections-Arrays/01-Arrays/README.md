# Arrays

## Overview
Arrays are fixed-size collections providing fast indexed access. Master array fundamentals, multi-dimensional arrays, and array operations.

## Learning Path

### 1. Array Basics (Start here)
- Declaring and initializing arrays
- Accessing elements by index
- Iterating with for and foreach
- Array length and bounds

**Time:** 15-20 minutes

### 2. Multi-Dimensional Arrays
- 2D arrays (matrices)
- 3D arrays
- Jagged arrays (variable row lengths)
- Iterating nested arrays

**Time:** 20-25 minutes

### 3. Array Operations
- Static methods (Sort, Reverse, Copy)
- Search operations (IndexOf, Find)
- LINQ with arrays
- Performance considerations

**Time:** 20-25 minutes

## Files in This Section

1. **00-Array-Basics.md** - Array fundamentals and operations
2. **00-Multi-Dimensional-Arrays.md** - 2D, 3D, and jagged arrays
3. **00-Array-Operations.md** - Methods, LINQ, and patterns

## Quick Reference

```csharp
// Declare and initialize
int[] numbers = { 1, 2, 3, 4, 5 };
int[] arr = new int[10];

// Access element
int value = numbers[0];

// Get length
int length = numbers.Length;

// Iterate
foreach (int num in numbers) { }

// 2D array
int[,] matrix = new int[3, 3];

// Jagged array
int[][] jagged = new int[3][];
```

## Key Concepts

- **Fixed size** - Cannot grow or shrink
- **0-indexed** - First element is at index 0
- **Fast access** - O(1) lookup by index
- **Type-safe** - All elements same type
- **Default values** - int=0, string=null, bool=false

## Common Patterns

✓ **Array iteration**
```csharp
for (int i = 0; i < arr.Length; i++) { }
foreach (var item in arr) { }
```

✓ **Array manipulation**
```csharp
Array.Sort(arr);
Array.Reverse(arr);
var found = Array.Find(arr, x => condition);
```

✓ **Multi-dimensional access**
```csharp
int[,] m = new int[3, 3];
m[0, 1] = 5;

int[][] j = new int[3][];
j[0] = new int[5];
```

## When to Use Arrays

✓ Size is known and fixed
✓ Need fast indexed access
✓ Performance critical
✓ Working with numeric data

✗ Size varies frequently
✗ Need dynamic growth
✗ Frequent insertions/removals

## Best Practices

✓ Check bounds before access
✓ Use foreach for simple iteration
✓ Use LINQ for transformations
✓ Prefer List if size changes

## Common Mistakes

❌ IndexOutOfRangeException - accessing invalid index
❌ Assuming non-empty array
❌ Forgetting to initialize jagged rows
❌ Using wrong array type

## Self-Assessment

Can you:
- [ ] Declare and initialize arrays?
- [ ] Access elements by index?
- [ ] Iterate arrays correctly?
- [ ] Use multi-dimensional arrays?
- [ ] Perform array operations?
- [ ] Use LINQ with arrays?

---

## Related Topics

- **Collections** - List, Dictionary, HashSet
- **LINQ** - Filtering, transforming arrays
- **Iteration** - For, foreach, while loops
- **Performance** - Time complexity analysis

## Next Steps

1. ✓ Learn Array Basics
2. ✓ Study Multi-Dimensional Arrays  
3. ✓ Master Array Operations
4. → Move to Generic Collections
5. → Study Collection Patterns
