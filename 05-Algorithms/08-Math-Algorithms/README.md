# Math Algorithms

## Overview
Mathematical algorithms handle numerical computations, number theory, and computational geometry. Essential for optimization, cryptography, and scientific computing.

## Number Theory

### Prime Numbers

#### Primality Testing
- **Trial Division**: O(√n)
- **Sieve of Eratosthenes**: O(n log log n) for all primes up to n
- **Fermat's Test**: Probabilistic O(k log n)
- **Miller-Rabin**: Probabilistic O(k log³ n)

#### Prime Factorization
- **Trial Division**: O(√n)
- **Pollard's Rho**: Faster for composite numbers
- **Application**: RSA cryptography, GCD calculation

### GCD and LCM

#### Euclidean Algorithm (GCD)
- **Time**: O(log min(a,b))
- **Approach**: Repeated remainder until zero
- **Extended GCD**: Find Bezout coefficients

#### Least Common Multiple
- **Formula**: LCM(a,b) = (a × b) / GCD(a,b)
- **Use**: Fraction operations, scheduling

### Modular Arithmetic

#### Modular Exponentiation
- **Problem**: Compute (base^exp) mod m efficiently
- **Approach**: Binary exponentiation
- **Time**: O(log exp)
- **Use**: Large number handling, cryptography

#### Modular Inverse
- **Problem**: Find x where (a × x) ≡ 1 (mod m)
- **Condition**: a and m must be coprime
- **Methods**: Extended Euclidean, Fermat's Little Theorem
- **Use**: Cryptography, modular division

#### Chinese Remainder Theorem
- **Problem**: Solve system of modular equations
- **Condition**: Moduli must be coprime
- **Time**: O(log m)
- **Application**: Number reconstruction

## Combinatorics

### Permutations and Combinations

#### Factorial
- **Time**: O(n) iterative
- **Space**: O(1)
- **Note**: Grows very fast, often use modular arithmetic

#### Permutations (n P r)
- **Formula**: n! / (n-r)!
- **Use**: Arrangement problems
- **Generation**: Lexicographic or recursive approach

#### Combinations (n C r)
- **Formula**: n! / (r! × (n-r)!)
- **Property**: C(n,r) = C(n,n-r)
- **Use**: Selection problems
- **Optimization**: Pascal's triangle for precomputation

#### Pascal's Triangle
- **Construction**: C(n,k) = C(n-1,k-1) + C(n-1,k)
- **Space**: O(n²) for all combinations
- **Time**: O(n²) precomputation, O(1) lookup

### Catalan Numbers
- **Formula**: C(n) = (2n)! / ((n+1)! × n!)
- **Recurrence**: C(n) = Σ(C(i) × C(n-1-i))
- **Use**: Balanced parentheses, BST structures, path counting
- **Time**: O(n) or O(n log n) depending on method

## Geometry

### Distance Calculations
- **Euclidean**: √((x₂-x₁)² + (y₂-y₁)²)
- **Manhattan**: |x₂-x₁| + |y₂-y₁|
- **Chebyshev**: max(|x₂-x₁|, |y₂-y₁|)

### Polygon Algorithms

#### Convex Hull
- **Graham Scan**: O(n log n)
- **Andrew's Algorithm**: O(n log n)
- **Jarvis March**: O(n × h) where h = hull size
- **Use**: Computational geometry, optimization

#### Point in Polygon
- **Ray Casting**: O(n)
- **Winding Number**: O(n)
- **Optimization**: Precompute for multiple queries

#### Polygon Area
- **Shoelace Formula**: O(n)
- **Application**: Grid point counting (Pick's theorem)

### Line and Segment Operations
- **Intersection**: Parametric approach
- **Closest Points**: Divide and conquer
- **Angle Calculations**: Dot product, cross product

## Matrix Operations

### Matrix Multiplication
- **Naive**: O(n³)
- **Strassen**: O(n^2.807)
- **Use**: Systems of equations, transformations

### Matrix Exponentiation
- **Time**: O(n³ log exp)
- **Use**: Fibonacci computation, graph adjacency power
- **Optimization**: Binary exponentiation approach

### Determinant and Inverse
- **Gaussian Elimination**: O(n³)
- **Use**: Solving linear systems
- **Application**: Transformation matrices

### Rank and Eigenvalues
- **Rank**: O(n² × m) or O(n³) via SVD
- **Eigenvalues**: Various iterative methods
- **Application**: PCA, stability analysis

## Numerical Methods

### Root Finding
- **Bisection**: O(log(1/ε))
- **Newton-Raphson**: Quadratic convergence
- **Secant Method**: Faster than bisection
- **Use**: Solving equations, optimization

### Integration
- **Trapezoidal Rule**: O(1/n)
- **Simpson's Rule**: O(1/n⁴)
- **Monte Carlo**: Probabilistic
- **Use**: Area under curve, probability

### Linear System Solving
- **Gaussian Elimination**: O(n³)
- **LU Decomposition**: O(n³)
- **Iterative Methods**: Gauss-Seidel, Conjugate Gradient

## Optimization

### Convex Optimization
- **Gradient Descent**: O(1/ε²) iterations
- **Newton's Method**: O(1/ε) iterations
- **Application**: Machine learning, resource allocation

### Linear Programming
- **Simplex Method**: Average O(n × m)
- **Interior Point**: Polynomial guaranteed
- **Use**: Resource allocation, production planning

### Bitwise Operations
- **Bit Manipulation**: O(1) per operation
- **Population Count**: Hamming weight
- **GCD via Bitwise**: Binary GCD algorithm
- **Use**: Optimization, low-level programming

## Complexity Reference

| Operation | Time | Space |
|-----------|------|-------|
| GCD (Euclidean) | O(log min(a,b)) | O(1) |
| Primality (Trial) | O(√n) | O(1) |
| Prime Sieve | O(n log log n) | O(n) |
| Modular Exp | O(log exp) | O(1) |
| Combinations | O(n²) precompute | O(n²) |
| Catalan Numbers | O(n) | O(n) |
| Convex Hull | O(n log n) | O(n) |
| Matrix Multiply | O(n³) | O(n²) |
| Root Finding | O(log(1/ε)) | O(1) |

## Interview Tips

1. **Identify Mathematical Pattern**: GCD? Combinations? Geometry?
2. **Consider Overflow**: Use modular arithmetic or big integers
3. **Floating Point**: Be aware of precision issues
4. **Precomputation**: Often efficient for queries
5. **Bit Manipulation**: Sometimes faster for specific operations
6. **Mathematical Properties**: Know shortcuts and identities

## Practice Files
- **01-Explanation**: Number theory, combinatorics, geometry fundamentals
- **02-Examples**: Mathematical problems with step-by-step solutions
- **03-Code-Implementations**: C# implementations of all math algorithms
