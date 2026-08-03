# Multi-Dimensional Arrays

## Overview
Multi-dimensional arrays store data in multiple dimensions - typically 2D for matrices or 3D for volumes. C# supports both rectangular arrays and jagged arrays.

## 2D Arrays (Matrices)

A 2D array represents a table with rows and columns.

### Declaring 2D Arrays

```csharp
// Rectangular 2D array (3 rows, 3 columns)
int[,] matrix = new int[3, 3];

// 2D array with values
int[,] numbers = new int[,] {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Inferred type
var data = new int[,] {
    { 10, 20, 30 },
    { 40, 50, 60 }
};

// Without new
int[,] simple = {
    { 1, 2 },
    { 3, 4 },
    { 5, 6 }
};
```

### Accessing 2D Array Elements

```csharp
int[,] matrix = new int[,] {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Access element at row 0, column 1
int value = matrix[0, 1];  // 2

// Access element at row 2, column 2
int diagonal = matrix[2, 2];  // 9

// Set element
matrix[1, 1] = 100;  // Changes middle element
```

### 2D Array Dimensions

```csharp
int[,] matrix = new int[,] {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Get dimensions
int rows = matrix.GetLength(0);  // 3
int cols = matrix.GetLength(1);  // 3

// Total elements
int total = rows * cols;  // 9
```

### Iterating 2D Arrays

#### Nested For Loops

```csharp
int[,] matrix = new int[,] {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

for (int row = 0; row < matrix.GetLength(0); row++) {
    for (int col = 0; col < matrix.GetLength(1); col++) {
        Console.WriteLine($"matrix[{row},{col}] = {matrix[row, col]}");
    }
}
```

#### Foreach Loop

```csharp
int[,] matrix = new int[,] {
    { 1, 2, 3 },
    { 4, 5, 6 }
};

foreach (int value in matrix) {
    Console.WriteLine(value);
}
// Outputs: 1, 2, 3, 4, 5, 6 (row by row)
```

## Jagged Arrays

Jagged arrays are arrays of arrays - each row can have different length.

### Declaring Jagged Arrays

```csharp
// Jagged array - 3 rows, different column counts
int[][] jagged = new int[3][];

// Initialize each row
jagged[0] = new int[5];  // First row has 5 elements
jagged[1] = new int[3];  // Second row has 3 elements
jagged[2] = new int[4];  // Third row has 4 elements

// Jagged array with values
int[][] numbers = new int[][] {
    new int[] { 1, 2, 3 },
    new int[] { 4, 5 },
    new int[] { 6, 7, 8, 9 }
};

// Simplified syntax
int[][] simple = {
    new[] { 1, 2, 3 },
    new[] { 4, 5 },
    new[] { 6, 7, 8, 9 }
};
```

### Accessing Jagged Array Elements

```csharp
int[][] jagged = {
    new[] { 1, 2, 3 },
    new[] { 4, 5 },
    new[] { 6, 7, 8, 9 }
};

// Access element in first row, third column
int value = jagged[0][2];  // 3

// Access element in second row, first column
int value2 = jagged[1][0];  // 4

// Set element
jagged[1][1] = 100;  // Changes to 100
```

### Jagged Array Dimensions

```csharp
int[][] jagged = {
    new[] { 1, 2, 3 },
    new[] { 4, 5 },
    new[] { 6, 7, 8, 9 }
};

// Outer dimension (number of rows)
int rows = jagged.Length;  // 3

// Inner dimension (length of specific row)
int firstRowLength = jagged[0].Length;  // 3
int secondRowLength = jagged[1].Length;  // 2

// Each row can have different length
for (int i = 0; i < jagged.Length; i++) {
    Console.WriteLine($"Row {i} has {jagged[i].Length} elements");
}
```

### Iterating Jagged Arrays

```csharp
int[][] jagged = {
    new[] { 1, 2, 3 },
    new[] { 4, 5 },
    new[] { 6, 7, 8, 9 }
};

// Nested for loops
for (int row = 0; row < jagged.Length; row++) {
    for (int col = 0; col < jagged[row].Length; col++) {
        Console.WriteLine($"jagged[{row}][{col}] = {jagged[row][col]}");
    }
}

// Nested foreach
foreach (int[] row in jagged) {
    foreach (int value in row) {
        Console.WriteLine(value);
    }
}
```

## 3D Arrays

Arrays with three dimensions (depth, rows, columns).

### Declaring 3D Arrays

```csharp
// 2x3x4 3D array
int[,,] cube = new int[2, 3, 4];

// With values
int[,,] data = new int[,,] {
    {
        { 1, 2, 3, 4 },
        { 5, 6, 7, 8 },
        { 9, 10, 11, 12 }
    },
    {
        { 13, 14, 15, 16 },
        { 17, 18, 19, 20 },
        { 21, 22, 23, 24 }
    }
};
```

### Accessing 3D Array Elements

```csharp
int[,,] cube = new int[2, 3, 4];

// Set element
cube[0, 1, 2] = 99;

// Get element
int value = cube[0, 1, 2];  // 99

// Dimensions
int depth = cube.GetLength(0);  // 2
int rows = cube.GetLength(1);   // 3
int cols = cube.GetLength(2);   // 4
```

### Iterating 3D Arrays

```csharp
int[,,] cube = new int[2, 3, 4];

for (int d = 0; d < cube.GetLength(0); d++) {
    for (int r = 0; r < cube.GetLength(1); r++) {
        for (int c = 0; c < cube.GetLength(2); c++) {
            int value = cube[d, r, c];
            // Process value
        }
    }
}
```

## Rectangular vs Jagged

### Rectangular 2D Array

```csharp
// All rows same length
int[,] rectangular = new int[,] {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Access: rectangular[row, col]
int value = rectangular[1, 2];  // 6
```

### Jagged Array

```csharp
// Each row different length
int[][] jagged = new int[][] {
    new int[] { 1, 2, 3 },
    new int[] { 4, 5 },
    new int[] { 6, 7, 8, 9 }
};

// Access: jagged[row][col]
int value = jagged[1][1];  // 5
```

## Real-World Examples

### Example 1: Matrix Operations

```csharp
// Matrix multiplication
int[,] MatrixMultiply(int[,] a, int[,] b) {
    int rows = a.GetLength(0);
    int cols = b.GetLength(1);
    int[,] result = new int[rows, cols];
    
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            result[i, j] = 0;
            for (int k = 0; k < a.GetLength(1); k++) {
                result[i, j] += a[i, k] * b[k, j];
            }
        }
    }
    
    return result;
}
```

### Example 2: Game Board

```csharp
// Chess board representation
char[,] board = new char[8, 8];

// Initialize empty squares
for (int row = 0; row < 8; row++) {
    for (int col = 0; col < 8; col++) {
        board[row, col] = '.';
    }
}

// Place piece
board[4, 4] = 'K';  // King at center
```

### Example 3: Irregular Data

```csharp
// Student scores by class (different sizes)
int[][] classScores = new int[][] {
    new int[] { 85, 90, 92, 88 },      // Class A - 4 students
    new int[] { 78, 82 },              // Class B - 2 students
    new int[] { 95, 89, 91, 87, 93 }  // Class C - 5 students
};

// Get average for each class
for (int i = 0; i < classScores.Length; i++) {
    double avg = classScores[i].Average();
    Console.WriteLine($"Class {(char)('A' + i)} average: {avg}");
}
```

## Best Practices

✓ **Use appropriate array type**
```csharp
// Rectangular 2D - uniform grid
int[,] matrix = new int[3, 3];

// Jagged - irregular dimensions
int[][] jagged = new int[3][];
```

✓ **Check bounds in nested loops**
```csharp
for (int row = 0; row < matrix.GetLength(0); row++) {
    for (int col = 0; col < matrix.GetLength(1); col++) {
        // Safe access
    }
}
```

✓ **Initialize jagged array properly**
```csharp
int[][] jagged = new int[3][];
for (int i = 0; i < jagged.Length; i++) {
    jagged[i] = new int[someLength];
}
```

## Anti-Patterns

❌ **Wrong indexing**
```csharp
int[,] matrix = new int[3, 3];
int value = matrix[3, 3];  // Out of bounds!
```

❌ **Forgetting to initialize jagged rows**
```csharp
int[][] jagged = new int[3][];
jagged[0][0] = 1;  // NullReferenceException!
```

❌ **Mixing rectangular and jagged notation**
```csharp
int[,] matrix = new int[3, 3];
int value = matrix[0][0];  // Wrong! Use [0, 0]
```

## Summary

- **2D arrays** - Rectangular grids with uniform dimensions
- **3D arrays** - Cubes with three dimensions
- **Jagged arrays** - Irregular arrays where rows have different lengths
- **Access pattern** - Rectangular uses [row, col]; jagged uses [row][col]
- **Use case** - Rectangular for matrices; jagged for irregular data

---

## Next Steps

1. Learn Array Operations
2. Master Generic Collections
3. Study Iteration Patterns
4. Review Best Practices
