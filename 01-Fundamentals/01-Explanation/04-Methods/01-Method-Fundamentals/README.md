# Method Fundamentals

## Overview

This category covers the foundational concepts of methods in C#. Start here if you're new to methods or need to solidify your understanding of core concepts.

## Files in This Category

### 1. [Method-Basics](01-Method-Basics/00-Method-Basics.md)
**Focus:** Structure and components of methods
- What is a method?
- Method components (signature, body, parameters, return)
- Access modifiers (public, private, protected, internal)
- Method naming conventions
- Simple examples

**When to Read:**
- First time learning about methods
- Need to understand method structure
- Confused about method components

**Key Concepts:**
- Method definition and syntax
- Parameters vs arguments
- Return types
- Method calls and execution

---

### 2. [Return-Types](02-Return-Types/00-Return-Types.md)
**Focus:** Different return types and return statements
- void methods (no return value)
- Primitive return types (int, double, bool, etc.)
- Reference return types (objects, strings)
- Nullable return types
- Multiple return statements
- Early returns and guard clauses

**When to Read:**
- Unsure about void vs other return types
- Need to return different data types
- Confused about nullable types
- Want to understand return value patterns

**Key Concepts:**
- void methods and side effects
- Returning primitives vs objects
- Nullable reference types
- Return value usage

---

### 3. [Method-Structure](03-Method-Structure/00-Method-Structure.md)
**Focus:** Complete method breakdown and best practices
- Complete method anatomy
- Naming conventions (PascalCase, clarity)
- Method body organization
- Documentation and comments
- Common method patterns
- Method complexity

**When to Read:**
- Want to write well-structured methods
- Need naming convention guidance
- Unsure how to organize method logic
- Want to learn documentation practices

**Key Concepts:**
- Method organization
- Naming best practices
- Documentation standards
- Method patterns

---

## Learning Paths

### Path 1: Complete Beginner
1. Start with [Method-Basics](01-Method-Basics/00-Method-Basics.md) - Learn what methods are
2. Continue with [Return-Types](02-Return-Types/00-Return-Types.md) - Understand return values
3. Finish with [Method-Structure](03-Method-Structure/00-Method-Structure.md) - Learn best practices

**Estimated Time:** 2-3 hours
**Outcome:** Comfortable writing basic methods

### Path 2: Quick Review
1. Skim [Method-Basics](01-Method-Basics/00-Method-Basics.md) for syntax
2. Check [Method-Structure](03-Method-Structure/00-Method-Structure.md) for conventions

**Estimated Time:** 30-45 minutes
**Outcome:** Refresh on fundamentals

### Path 3: Deep Dive
1. Study all three files thoroughly
2. Write practice methods for each concept
3. Review [Best-Practices](../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) for guidelines

**Estimated Time:** 4-5 hours
**Outcome:** Deep understanding of method fundamentals

---

## Quick Reference

### Method Syntax
```csharp
public int Add(int a, int b)
{
    return a + b;
}
```

### Components
- **Access Modifier:** `public`
- **Return Type:** `int`
- **Method Name:** `Add`
- **Parameters:** `(int a, int b)`
- **Body:** `{ return a + b; }`

### Key Access Modifiers
- `public` - Accessible everywhere
- `private` - Accessible only in this class
- `protected` - Accessible in derived classes
- `internal` - Accessible in same assembly

### Return Types
- `void` - No return value
- `int` - Integer
- `string` - Text
- `bool` - True/false
- `MyClass` - Custom types
- `int?` - Nullable int

---

## Common Tasks

### Write a Simple Method
```csharp
public string Greet(string name)
{
    return $"Hello, {name}!";
}
```
→ See: [Method-Basics](01-Method-Basics/00-Method-Basics.md#simple-methods)

### Handle No Return Value
```csharp
public void PrintMessage(string message)
{
    Console.WriteLine(message);
}
```
→ See: [Return-Types](02-Return-Types/00-Return-Types.md#void-methods)

### Return Multiple Types
```csharp
public object GetValue(string type)
{
    return type switch
    {
        "number" => 42,
        "text" => "hello",
        _ => null
    };
}
```
→ See: [Return-Types](02-Return-Types/00-Return-Types.md#reference-types)

### Document a Method
```csharp
/// <summary>
/// Calculates sum of two numbers
/// </summary>
/// <param name="a">First number</param>
/// <param name="b">Second number</param>
/// <returns>Sum of a and b</returns>
public int Add(int a, int b)
{
    return a + b;
}
```
→ See: [Method-Structure](03-Method-Structure/00-Method-Structure.md#documentation)

---

## Exercise Ideas

### Exercise 1: Write Basic Methods
Create methods for:
1. Adding two numbers
2. Checking if a string is empty
3. Converting temperature from Celsius to Fahrenheit

→ Reference: [Method-Basics](01-Method-Basics/00-Method-Basics.md#simple-examples)

### Exercise 2: Practice Return Types
Write methods that return:
1. `void` (no return)
2. `int` (primitive type)
3. `string?` (nullable reference type)
4. Custom class instance

→ Reference: [Return-Types](02-Return-Types/00-Return-Types.md)

### Exercise 3: Structure and Document
Write a method that:
1. Has clear naming
2. Includes proper documentation
3. Uses meaningful parameter names
4. Follows single responsibility

→ Reference: [Method-Structure](03-Method-Structure/00-Method-Structure.md)

---

## Self-Assessment

### Beginner Level
- [ ] Can write a method with parameters and return value
- [ ] Understand difference between void and other return types
- [ ] Know the four access modifiers
- [ ] Can call a method

### Intermediate Level
- [ ] Can write well-documented methods
- [ ] Understand when to use different access modifiers
- [ ] Can return nullable types correctly
- [ ] Know method naming conventions

### Advanced Level
- [ ] Can design method signatures for clarity
- [ ] Understand method organization patterns
- [ ] Can write methods with proper documentation
- [ ] Follow method fundamentals best practices

---

## Common Questions

**Q: Should I always document my methods?**
A: Yes. XML documentation helps you and others understand purpose and usage.

**Q: What's the difference between void and returning null?**
A: `void` indicates no return value. If you return null, use nullable type like `string?`.

**Q: Can a method have multiple return statements?**
A: Yes, but prefer single exit point for clarity. Guard clauses are an exception.

**Q: How long should a method be?**
A: Generally 5-20 lines. If longer, consider breaking into smaller methods.

---

## Related Sections

- **[Method-Scope](../03-Advanced-Patterns/02-Method-Scope/00-Method-Scope.md)** - Understanding visibility and interaction between methods
- **[Parameters-Overloading](../02-Parameters-Overloading/README.md)** - Working with method parameters
- **[Best-Practices](../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)** - Writing high-quality methods

---

## Study Recommendations

1. **Read actively** - Don't just skim; read examples and understand them
2. **Practice** - Write methods after reading each section
3. **Experiment** - Try variations and see what happens
4. **Connect concepts** - Link fundamentals to advanced patterns

---

## Next Steps

- Complete [Parameters-Overloading](../02-Parameters-Overloading/README.md) category
- Learn [Advanced-Patterns](../03-Advanced-Patterns/README.md) like recursion
- Review [Best-Practices](../04-Best-Practices-Interview/README.md) for professional code

---

**Total Words in Category:** ~14,000 words across 3 focused files
