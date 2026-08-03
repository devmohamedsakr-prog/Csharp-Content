# Advanced Patterns and Special Methods

## Overview

This category covers advanced method concepts including recursion, method scope and interaction, and special methods like constructors and operators.

## Files in This Category

### 1. [Recursion](01-Recursion/00-Recursion.md)
**Focus:** Methods that call themselves
- What is recursion?
- Recursive structure (base case, recursive case)
- Simple recursion examples (countdown, factorial, Fibonacci)
- Array and collection recursion
- Tree traversal with recursion
- String recursion patterns
- Understanding the call stack
- Recursion vs iteration
- Performance considerations and memoization
- When to use recursion

**When to Read:**
- Need to solve tree/graph problems
- Confused about how recursion works
- Want to understand call stack
- Need to optimize recursive solutions

**Key Concepts:**
- Base case and recursive case
- Call stack mechanics
- Recursion vs iteration trade-offs
- Performance optimization

---

### 2. [Method-Scope](02-Method-Scope/00-Method-Scope.md)
**Focus:** Method visibility and interaction patterns
- Method visibility (public, private, protected, internal)
- Local variables vs class members
- Methods calling methods
- Method chaining
- Method dependencies
- Private helper methods
- Method scope rules (class, derived, outside)
- Static methods calling conventions
- Parameter scope
- Block scope and variable shadowing
- Lambda scope

**When to Read:**
- Confused about access modifiers
- Need to understand scope rules
- Want to organize methods properly
- Need to call methods from other methods

**Key Concepts:**
- Access modifier visibility
- Scope rules
- Method interaction patterns
- Static vs instance methods

---

### 3. [Special-Methods](03-Special-Methods/00-Special-Methods.md)
**Focus:** Methods with special purposes and behaviors
- Constructors (initialization methods)
- Constructor overloading and chaining
- Destructors (cleanup methods)
- ref/out/in parameter modifiers
- TryParse pattern (safe parsing)
- Operator overloading (+, -, ==, etc.)
- ToString override (string representation)
- GetHashCode and Equals (object comparison)
- params keyword (variable arguments)
- Extension methods (add to existing types)
- Async methods (returning Tasks)

**When to Read:**
- Need to initialize objects with constructors
- Want to implement TryParse pattern
- Need to override operators
- Want to create extension methods

**Key Concepts:**
- Constructors and initialization
- Parameter modifiers (ref/out/in)
- Common method patterns
- Special method signatures

---

## Learning Paths

### Path 1: Fundamentals to Advanced
1. Review [Method-Scope](02-Method-Scope/00-Method-Scope.md) - Understand method organization
2. Study [Recursion](01-Recursion/00-Recursion.md) - Learn recursive patterns
3. Explore [Special-Methods](03-Special-Methods/00-Special-Methods.md) - Master special methods

**Estimated Time:** 4-5 hours
**Outcome:** Comfortable with advanced patterns

### Path 2: Recursion Focus
1. Deep dive [Recursion](01-Recursion/00-Recursion.md)
2. Practice with tree traversal
3. Implement memoization
4. Compare with iteration

**Estimated Time:** 2-3 hours
**Outcome:** Recursion expert

### Path 3: Special Methods Deep Dive
1. Focus on [Special-Methods](03-Special-Methods/00-Special-Methods.md)
2. Learn constructors and initialization
3. Practice operator overloading
4. Implement custom patterns

**Estimated Time:** 2-3 hours
**Outcome:** Master special methods

---

## Quick Reference

### Recursion Pattern
```csharp
public int Factorial(int n)
{
    // Base case - stop recursion
    if (n <= 1)
        return 1;
    
    // Recursive case - call itself
    return n * Factorial(n - 1);
}
```

### Method Scope
```csharp
public class Example
{
    public void PublicMethod() { }     // Accessible everywhere
    private void PrivateMethod() { }   // Accessible only here
    protected void ProtectedMethod() { }  // Accessible in derived classes
    internal void InternalMethod() { }    // Accessible in same assembly
}
```

### Special Methods
```csharp
public class MyClass
{
    // Constructor - called on new
    public MyClass(string name) { Name = name; }
    
    // Destructor - called on garbage collection
    ~MyClass() { /* cleanup */ }
    
    // Operator overload
    public static MyClass operator +(MyClass a, MyClass b) { }
    
    // ToString override
    public override string ToString() => Name;
    
    // Extension method pattern
    public static bool IsEmpty(this string str) => 
        string.IsNullOrEmpty(str);
}
```

---

## Common Tasks

### Understand Method Calling Methods
```csharp
public class Calculator
{
    public int Calculate(int a, int b)
    {
        int sum = Add(a, b);           // Call Add method
        return Multiply(sum, 2);       // Call Multiply method
    }
    
    private int Add(int a, int b) => a + b;
    private int Multiply(int a, int b) => a * b;
}
```
→ See: [Method-Scope](02-Method-Scope/00-Method-Scope.md#methods-calling-methods)

### Write Recursive Method
```csharp
public int Sum(int n)
{
    // Base case
    if (n <= 0)
        return 0;
    
    // Recursive case
    return n + Sum(n - 1);
}

Sum(5);  // 5 + 4 + 3 + 2 + 1 = 15
```
→ See: [Recursion](01-Recursion/00-Recursion.md#simple-recursion-examples)

### Create Constructor
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor initializes on creation
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

var person = new Person("Alice", 30);
```
→ See: [Special-Methods](03-Special-Methods/00-Special-Methods.md#constructors)

### Implement TryParse Pattern
```csharp
public bool TryParseInt(string input, out int result)
{
    result = 0;
    
    if (int.TryParse(input, out int parsed))
    {
        result = parsed;
        return true;
    }
    
    return false;
}

if (TryParseInt("42", out int value))
{
    Console.WriteLine(value);  // 42
}
```
→ See: [Special-Methods](03-Special-Methods/00-Special-Methods.md#tryparse-pattern)

### Create Tree Traversal
```csharp
public void PrintTree(TreeNode? node)
{
    // Base case
    if (node == null)
        return;
    
    Console.WriteLine(node.Value);
    
    // Recursive calls
    PrintTree(node.Left);
    PrintTree(node.Right);
}
```
→ See: [Recursion](01-Recursion/00-Recursion.md#tree-traversal)

---

## Exercise Ideas

### Exercise 1: Recursion
Implement:
1. Countdown from n to 1
2. Calculate factorial
3. Calculate Fibonacci number
4. Sum array elements recursively

→ Reference: [Recursion](01-Recursion/00-Recursion.md)

### Exercise 2: Method Scope
Practice:
1. Call methods from other methods
2. Use private helper methods
3. Understand access modifiers
4. Practice method visibility

→ Reference: [Method-Scope](02-Method-Scope/00-Method-Scope.md)

### Exercise 3: Constructors
Create:
1. Constructor with parameters
2. Multiple overloaded constructors
3. Constructor chaining
4. Proper initialization

→ Reference: [Special-Methods](03-Special-Methods/00-Special-Methods.md#constructors)

### Exercise 4: Special Methods
Implement:
1. Custom TryParse method
2. Operator overloading
3. ToString override
4. Equals override

→ Reference: [Special-Methods](03-Special-Methods/00-Special-Methods.md)

---

## Performance Considerations

### Recursion Performance
```csharp
// Slow - recalculates many times
public int FibonacciSlow(int n)
{
    if (n <= 1) return n;
    return FibonacciSlow(n - 1) + FibonacciSlow(n - 2);
}

// Fast - uses memoization
public int FibonacciMemo(int n, Dictionary<int, int> memo)
{
    if (n <= 1) return n;
    if (memo.ContainsKey(n)) return memo[n];
    
    int result = FibonacciMemo(n - 1, memo) + FibonacciMemo(n - 2, memo);
    memo[n] = result;
    return result;
}
```
→ See: [Recursion](01-Recursion/00-Recursion.md#performance-considerations)

---

## Self-Assessment

### Beginner Level
- [ ] Understand what recursion is
- [ ] Know access modifiers
- [ ] Can write simple constructor
- [ ] Understand method scope

### Intermediate Level
- [ ] Can write recursive methods
- [ ] Comfortable with access modifiers
- [ ] Can create overloaded constructors
- [ ] Understand method calling patterns

### Advanced Level
- [ ] Optimize recursive solutions
- [ ] Master method scope rules
- [ ] Design complex constructors
- [ ] Implement special method patterns

---

## Common Questions

**Q: When should I use recursion vs loops?**
A: Recursion for natural recursive structures (trees, graphs). Loops for simple iteration. Consider performance.

**Q: What's the difference between public and private?**
A: Public is accessible everywhere. Private is only accessible in that class.

**Q: Do I need a destructor?**
A: Rarely. Use only for managing unmanaged resources. Usually .NET garbage collection handles cleanup.

**Q: Can I call one constructor from another?**
A: Yes, using `: this(parameters)` syntax for constructor chaining.

**Q: Why would I overload operators?**
A: To make custom types work naturally with operators like +, -, ==, etc.

---

## Related Sections

- **[Method-Fundamentals](../01-Method-Fundamentals/README.md)** - Foundational concepts
- **[Parameters-Overloading](../02-Parameters-Overloading/README.md)** - Parameter techniques
- **[Best-Practices](../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)** - Advanced method guidelines
- **[Common-Mistakes](../04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md)** - Patterns to avoid

---

## Study Recommendations

1. **Understand recursion deeply** - Many problems use recursive thinking
2. **Master scope rules** - Prevents bugs and improves code organization
3. **Learn special methods** - Patterns you'll use constantly
4. **Practice optimization** - Especially memoization for recursion

---

## Next Steps

- Study [Best-Practices](../04-Best-Practices-Interview/README.md) for professional standards
- Review [Interview-Questions](../04-Best-Practices-Interview/03-Interview-Questions/00-Interview-Overview.md) for advanced concepts
- Practice implementing advanced patterns from this category

---

**Total Words in Category:** ~17,000 words across 3 focused files
