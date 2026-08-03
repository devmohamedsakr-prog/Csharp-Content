# Scope Fundamentals

## Overview

This category covers the foundational concepts of scope in C#. Scope determines **where** a variable or member can be accessed within your code. Understanding scope is essential for writing maintainable, bug-free code.

## Topics Covered

### 1. Block Scope
**File**: `01-Block-Scope/00-Block-Scope.md`

Learn how variables declared in code blocks (if statements, loops, try-catch, etc.) are only accessible within those blocks.

**Key Concepts**:
- Block scope definition and hierarchy
- Scope in control flow structures (if, for, while, switch)
- Nested block scopes
- Loop variable scope
- Scope extends to closing brace

**When to Use**: Understanding block scope helps you:
- Declare variables close to where they're used
- Prevent unintended variable access
- Keep scope as narrow as possible
- Avoid variable shadowing

**Example**:
```csharp
if (condition)
{
    int x = 10; // Block scope
    Console.WriteLine(x); // OK
}
// Console.WriteLine(x); // ERROR - x out of scope
```

---

### 2. Method Scope
**File**: `02-Method-Scope/00-Method-Scope.md`

Understand how method local variables and parameters are scoped to individual method calls.

**Key Concepts**:
- Method-local variables
- Parameter scope
- Stack frames and method calls
- Recursive methods
- Scope differences (instance vs static methods)
- Passing variables between methods

**When to Use**: Method scope helps you:
- Understand stack allocation
- Design proper method signatures
- Manage parameter passing
- Prevent unintended variable sharing

**Example**:
```csharp
public void MethodA()
{
    int x = 5; // Method scope
}

public void MethodB()
{
    // Console.WriteLine(x); // ERROR - x not in scope
}
```

---

### 3. Class Scope and Access Modifiers
**File**: `03-Class-Scope/00-Class-Scope.md`

Master access modifiers (public, private, protected, internal, private protected) that control class member visibility.

**Key Concepts**:
- Public access - accessible everywhere
- Private access - class members only (default)
- Protected access - class and derived classes
- Internal access - same assembly only
- Private protected - derived classes in same assembly
- Access modifier combinations
- Inheritance and scope
- Default access levels

**When to Use**: Access modifiers help you:
- Encapsulate implementation details
- Create clear public APIs
- Prevent misuse of internal members
- Design secure class hierarchies
- Follow the principle of least privilege

**Example**:
```csharp
public class MyClass
{
    public string PublicField; // Accessible everywhere
    private int _privateField; // Only in this class
    protected string ProtectedField; // In this and derived classes
    internal int InternalField; // Same assembly only
}
```

---

### 4. Namespace Scope
**File**: `04-Namespace-Scope/00-Namespace-Scope.md`

Organize your code with namespaces and understand how they control type visibility.

**Key Concepts**:
- Namespace declaration and hierarchy
- Using statements and directives
- Namespace aliasing
- Static using (C# 6.0+)
- Global using (C# 10.0+)
- File-scoped namespaces (C# 10.0+)
- Type visibility in namespaces
- Namespace organization patterns

**When to Use**: Namespaces help you:
- Organize code logically
- Avoid naming conflicts
- Create clear code structure
- Follow team conventions
- Improve code navigation

**Example**:
```csharp
namespace MyApp.Features.Users;

using System;
using System.Collections.Generic;

public class User { }
public class UserService { }
```

---

## Learning Path

### Beginner
1. Start with Block Scope - understand how braces define scope
2. Learn Method Scope - understand stack frames and method calls
3. Study Access Modifiers - learn to control visibility

### Intermediate
1. Review all four scope types
2. Understand interactions between scope types
3. Apply access modifiers appropriately
4. Organize code with namespaces

### Advanced
1. Combine scope concepts with closures and lifetime
2. Optimize scope for performance
3. Design secure APIs with proper access control
4. Organize large projects with namespaces

---

## Quick Reference

### Scope Types Comparison

| Scope Type | Size | Duration | Examples |
|-----------|------|----------|----------|
| Block | Smallest | Closing brace | if, loops, try-catch |
| Method | Method | Method return | Local variables |
| Class | Class | Class lifetime | Fields, properties, methods |
| Namespace | Assembly | Assembly/Project | Types, classes |
| Global | Everything | Program | External assemblies |

### Access Modifiers Comparison

| Modifier | Class | Derived | Assembly | Other |
|----------|-------|---------|----------|-------|
| public | ✓ | ✓ | ✓ | ✓ |
| protected | ✓ | ✓ | ✓ | ✗ |
| internal | ✓ | ✓ | ✓ | ✗ |
| private protected | ✓ | ✓ | ✗ | ✗ |
| private (default) | ✓ | ✗ | ✗ | ✗ |

---

## Best Practices in This Category

1. **Keep Scope Narrow**: Declare variables close to their use
2. **Use Appropriate Access Modifiers**: Start with private, broaden only when needed
3. **Avoid Variable Shadowing**: Use distinct names in different scopes
4. **Organize with Namespaces**: Match folder structure to namespace hierarchy
5. **Document Scope**: Make visibility intent clear through code structure

---

## Common Mistakes to Avoid

1. **Accessing Out-of-Scope Variables**: Variables destroyed at block/method end
2. **Over-Exposing Members**: Using public when private would be sufficient
3. **Inconsistent Namespace Organization**: Namespace doesn't match folder structure
4. **Shadowing Variables**: Same name in nested scope causes confusion
5. **Missing Using Statements**: Forgetting to import needed namespaces

---

## Practical Examples

### Example 1: Proper Scope Management

```csharp
// BAD: Wide scope
public int Calculate(int[] numbers)
{
    int total = 0;
    int count = 0;
    
    // 100 lines of other code...
    
    for (int i = 0; i < numbers.Length; i++)
    {
        total += numbers[i];
    }
    
    return total;
}

// GOOD: Narrow scope
public int Calculate(int[] numbers)
{
    int total = 0;
    
    for (int i = 0; i < numbers.Length; i++)
    {
        total += numbers[i];
    }
    
    return total;
}
```

### Example 2: Access Modifiers for Encapsulation

```csharp
// BAD: Exposed implementation
public class User
{
    public List<string> roles = new();
}

// GOOD: Encapsulated with property
public class User
{
    private List<string> _roles = new();
    public IReadOnlyList<string> Roles => _roles.AsReadOnly();
    
    public void AddRole(string role)
    {
        _roles.Add(role);
    }
}
```

### Example 3: Namespace Organization

```csharp
// Folder: MyApp/Features/Users/
namespace MyApp.Features.Users;

public class User { }
public class UserService { }

// Folder: MyApp/Infrastructure/Data/
namespace MyApp.Infrastructure.Data;

public class Repository { }
```

---

## Exercises

### Exercise 1: Block Scope
```csharp
// Fix the scope issues
public void Exercise1()
{
    for (int i = 0; i < 5; i++)
    {
        int square = i * i;
    }
    
    Console.WriteLine(square); // ERROR: square not in scope
}
```

**Solution**: Move println inside loop or declare square outside.

### Exercise 2: Access Modifiers
```csharp
// Improve encapsulation
public class Account
{
    public decimal Balance = 0;
    public string Pin = "1234";
}

// Should be private with controlled access
```

### Exercise 3: Namespace Organization
Design a namespace hierarchy for an e-commerce application covering:
- User management
- Product catalog
- Order processing
- Payment handling

---

## Related Topics

- **Lifetime and Memory**: How scope relates to variable lifetime
- **Closures**: Capturing variables across scope boundaries
- **Access Control**: Combining access modifiers with inheritance

---

## Summary

Scope fundamentals are the foundation of clean, maintainable C# code. By understanding block scope, method scope, class scope, and namespace scope, combined with appropriate access modifiers, you create code that is:

- **Clear**: Obvious where variables are used
- **Safe**: Prevents unintended access
- **Maintainable**: Easy to understand and modify
- **Professional**: Follows industry standards
- **Secure**: Protects implementation details

Master these concepts and you'll write better C# code from day one.

---

## Next Steps

1. Read through each section carefully
2. Study the code examples
3. Try the exercises
4. Apply these concepts to your projects
5. Move to **Lifetime and Memory** category to understand when variables exist

Happy learning!
