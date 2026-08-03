# Parameters and Method Overloading

## Overview

This category covers how to work with method parameters and how to create multiple methods with the same name but different signatures (overloading).

## Files in This Category

### 1. [Parameter-Types](01-Parameter-Types/00-Parameter-Types.md)
**Focus:** Different parameter passing mechanisms and variations
- No parameters (parameterless methods)
- Single parameters (one input)
- Multiple parameters (several inputs)
- Default parameter values
- Named parameters (pass by name)
- params keyword (variable number of arguments)
- ref, out, in parameters (reference passing)

**When to Read:**
- Need to pass data to methods
- Confused about ref vs out parameters
- Want to use default values
- Need variable-length parameter lists

**Key Concepts:**
- Parameter types and modifiers
- Value vs reference passing
- Default values
- Variable arguments

---

### 2. [Advanced-Parameters](02-Advanced-Parameters/00-Advanced-Parameters.md)
**Focus:** Advanced parameter techniques and patterns
- Deep dive into ref parameters (modify and return)
- Deep dive into out parameters (return multiple values)
- in parameters (read-only references)
- Default parameter values and overloads
- Named parameters for clarity
- Parameter validation patterns
- Common parameter patterns

**When to Read:**
- Need to modify caller's variables
- Want to return multiple values
- Need optimization with ref
- Working with complex parameter combinations

**Key Concepts:**
- ref vs out vs in differences
- Parameter validation
- Return multiple values safely
- Parameter combinations

---

### 3. [Method-Overloading](03-Method-Overloading/00-Method-Overloading.md)
**Focus:** Creating multiple methods with same name
- Overloading by parameter count
- Overloading by parameter type
- Overloading by parameter combination
- Overloading rules and limitations
- Overloading patterns
- When to use overloading vs default parameters
- Best practices for overloading

**When to Read:**
- Want same method name for similar operations
- Have methods that differ only in parameters
- Need different behaviors for different types
- Confused about overloading vs other patterns

**Key Concepts:**
- Overload resolution
- Parameter count variations
- Type variations
- Overloading vs other patterns

---

## Learning Paths

### Path 1: Complete Beginner to Parameters
1. Start with [Parameter-Types](01-Parameter-Types/00-Parameter-Types.md) - Learn basic parameters
2. Practice with simple methods
3. Review [Advanced-Parameters](02-Advanced-Parameters/00-Advanced-Parameters.md) when ready
4. Learn [Method-Overloading](03-Method-Overloading/00-Method-Overloading.md) - Multiple methods

**Estimated Time:** 3-4 hours
**Outcome:** Comfortable with parameters and overloading

### Path 2: Parameters Focus
1. Study [Parameter-Types](01-Parameter-Types/00-Parameter-Types.md) thoroughly
2. Deep dive [Advanced-Parameters](02-Advanced-Parameters/00-Advanced-Parameters.md)
3. Practice writing complex parameter combinations

**Estimated Time:** 2-3 hours
**Outcome:** Expert in parameter handling

### Path 3: Quick Overloading Overview
1. Review [Parameter-Types](01-Parameter-Types/00-Parameter-Types.md) basics
2. Focus on [Method-Overloading](03-Method-Overloading/00-Method-Overloading.md)
3. Learn when to use vs alternatives

**Estimated Time:** 1-2 hours
**Outcome:** Understand overloading patterns

---

## Quick Reference

### Parameter Types Syntax
```csharp
// No parameters
public void PrintMessage() { }

// Single parameter
public int Add(int a, int b) { }

// Multiple parameters
public void Process(string name, int age, bool active) { }

// Default parameter
public void Greet(string name = "Guest") { }

// Named parameters
Greet(name: "Alice");

// Variable parameters
public int Sum(params int[] numbers) { }

// Reference parameters
public void Increment(ref int value) { }
public bool TryGet(out string result) { }
public void Display(in Point p) { }
```

### Parameter Modifiers
| Modifier | Meaning | Use Case |
|----------|---------|----------|
| (none) | Pass by value | Default, safe |
| `ref` | Pass by reference, modify | Change caller's variable |
| `out` | Return parameter | Return multiple values |
| `in` | Read-only reference | Optimization, large structs |
| `params` | Variable length array | Unknown number of args |

### Method Overloading
```csharp
// Overload 1: Two integers
public int Add(int a, int b) => a + b;

// Overload 2: Two doubles
public double Add(double a, double b) => a + b;

// Overload 3: Three parameters
public int Add(int a, int b, int c) => a + b + c;

// Overload 4: Array of integers
public int Add(params int[] numbers) => 
    numbers.Sum();
```

---

## Common Tasks

### Pass Multiple Values
```csharp
public void DisplayUser(string name, int age, string email)
{
    Console.WriteLine($"{name}, {age}, {email}");
}

DisplayUser("Alice", 30, "alice@example.com");
```
→ See: [Parameter-Types](01-Parameter-Types/00-Parameter-Types.md#multiple-parameters)

### Use Default Values
```csharp
public void Greet(string name = "Guest", string greeting = "Hello")
{
    Console.WriteLine($"{greeting}, {name}!");
}

Greet();                           // Hello, Guest!
Greet("Alice");                    // Hello, Alice!
Greet("Alice", "Hi");              // Hi, Alice!
```
→ See: [Parameter-Types](01-Parameter-Types/00-Parameter-Types.md#default-parameters)

### Return Multiple Values with out
```csharp
public bool TryParse(string input, out int value, out string error)
{
    value = 0;
    error = "";
    
    if (int.TryParse(input, out int parsed))
    {
        value = parsed;
        return true;
    }
    error = "Invalid input";
    return false;
}
```
→ See: [Advanced-Parameters](02-Advanced-Parameters/00-Advanced-Parameters.md#out-parameters)

### Modify Caller's Variable with ref
```csharp
public void Increment(ref int value)
{
    value++;
}

int x = 5;
Increment(ref x);  // x is now 6
```
→ See: [Advanced-Parameters](02-Advanced-Parameters/00-Advanced-Parameters.md#ref-parameters)

### Variable Number of Arguments
```csharp
public int Sum(params int[] numbers)
{
    return numbers.Sum();
}

Sum(1, 2, 3);          // 6
Sum(1, 2, 3, 4, 5);    // 15
```
→ See: [Parameter-Types](01-Parameter-Types/00-Parameter-Types.md#params-keyword)

### Method Overloading by Type
```csharp
public class Converter
{
    public string ToText(int number) => number.ToString();
    public string ToText(double number) => number.ToString("F2");
    public string ToText(bool flag) => flag ? "Yes" : "No";
}

var c = new Converter();
c.ToText(42);      // "42"
c.ToText(3.14);    // "3.14"
c.ToText(true);    // "Yes"
```
→ See: [Method-Overloading](03-Method-Overloading/00-Method-Overloading.md#overloading-by-type)

---

## Exercise Ideas

### Exercise 1: Parameter Practice
Create methods for:
1. Add two numbers
2. Add three numbers
3. Add any number of integers (params)

→ Reference: [Parameter-Types](01-Parameter-Types/00-Parameter-Types.md)

### Exercise 2: Default Parameters
Write methods using default parameters:
1. Greet with optional name
2. Create rectangle with optional size
3. Format date with optional format

→ Reference: [Parameter-Types](01-Parameter-Types/00-Parameter-Types.md#default-parameters)

### Exercise 3: ref and out
Practice:
1. Swap two variables with ref
2. Try parsing with out
3. Split string parts with out

→ Reference: [Advanced-Parameters](02-Advanced-Parameters/00-Advanced-Parameters.md)

### Exercise 4: Method Overloading
Create overloaded methods:
1. Add for int, double, decimal
2. Convert for different types
3. Parse for different input types

→ Reference: [Method-Overloading](03-Method-Overloading/00-Method-Overloading.md)

---

## Self-Assessment

### Beginner Level
- [ ] Can write methods with parameters
- [ ] Understand default parameters
- [ ] Know when to use ref vs out
- [ ] Can create simple overloads

### Intermediate Level
- [ ] Comfortable with ref/out/in parameters
- [ ] Use named parameters effectively
- [ ] Create multiple overloads
- [ ] Choose between overloads and defaults

### Advanced Level
- [ ] Master parameter passing mechanisms
- [ ] Design efficient method signatures
- [ ] Create intuitive overload sets
- [ ] Follow parameter best practices

---

## Common Questions

**Q: When should I use out vs ref?**
A: Use `ref` to modify existing variables. Use `out` to return multiple values from methods.

**Q: When should I use default parameters vs overloading?**
A: Default parameters for minor variations. Overloading for different types or significant logic differences.

**Q: Is ref or in better for performance?**
A: Both avoid copying. Use `in` for read-only data. Use `ref` when modification needed.

**Q: Can I overload by return type?**
A: No. C# determines overload by parameters only, not return type.

**Q: How many overloads is too many?**
A: Usually 2-5. More than that, consider alternatives like parameters object or different method names.

---

## Related Sections

- **[Method-Basics](../01-Method-Fundamentals/01-Method-Basics/00-Method-Basics.md)** - Foundational concepts
- **[Return-Types](../01-Method-Fundamentals/02-Return-Types/00-Return-Types.md)** - Understanding return values
- **[Special-Methods](../03-Advanced-Patterns/03-Special-Methods/00-Special-Methods.md)** - Operator overloading and TryParse pattern
- **[Best-Practices](../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)** - Parameter best practices

---

## Study Recommendations

1. **Understand the basics first** - Master simple parameters before advanced techniques
2. **Practice each modifier** - Write code using ref, out, in separately
3. **Compare approaches** - See when to use each parameter technique
4. **Understand overload resolution** - Know how C# chooses which overload to call

---

## Next Steps

- Complete [Advanced-Patterns](../03-Advanced-Patterns/README.md) category
- Learn [Recursion](../03-Advanced-Patterns/01-Recursion/00-Recursion.md) patterns
- Study [Best-Practices](../04-Best-Practices-Interview/README.md) for professional code

---

**Total Words in Category:** ~15,000 words across 3 focused files
