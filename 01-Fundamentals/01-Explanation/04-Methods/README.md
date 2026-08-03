# Methods in C#

## Overview

Methods are reusable blocks of code that perform specific tasks. They're fundamental to writing organized, maintainable C# applications. This comprehensive guide covers everything from basic method syntax to advanced patterns and professional practices.

**Total Content:** ~82,000 words across 16 files organized in 4 categories
**Skill Levels:** Beginner to Advanced
**Topics:** Fundamentals, Parameters, Advanced Patterns, Best Practices, Interview Preparation

---

## Quick Start

### 30-Second Overview

A method is a named block of code that does something:

```csharp
// Method definition
public int Add(int a, int b)
{
    return a + b;
}

// Method call
int result = Add(5, 3);  // result = 8
```

**Key Components:**
- **public** - accessibility (who can use it)
- **int** - return type (what you get back)
- **Add** - method name (what it does)
- **(int a, int b)** - parameters (what you pass in)
- **{ return a + b; }** - body (the code that runs)

---

## 4 Learning Categories

### 1. 📚 [Method-Fundamentals](01-Method-Fundamentals/README.md)
**Start here if you're new to methods**

Core concepts: method structure, access modifiers, return types

**3 Files:**
- [Method-Basics](01-Method-Fundamentals/01-Method-Basics/00-Method-Basics.md) - What is a method?
- [Return-Types](01-Method-Fundamentals/02-Return-Types/00-Return-Types.md) - Different return values
- [Method-Structure](01-Method-Fundamentals/03-Method-Structure/00-Method-Structure.md) - Best structure

**Time:** 2-3 hours | **Words:** ~14,000

---

### 2. ⚙️ [Parameters-Overloading](02-Parameters-Overloading/README.md)
**Learn how to pass data and create multiple methods**

Core concepts: parameters, modifiers, method overloading

**3 Files:**
- [Parameter-Types](02-Parameters-Overloading/01-Parameter-Types/00-Parameter-Types.md) - Different parameter types
- [Advanced-Parameters](02-Parameters-Overloading/02-Advanced-Parameters/00-Advanced-Parameters.md) - ref, out, in
- [Method-Overloading](02-Parameters-Overloading/03-Method-Overloading/00-Method-Overloading.md) - Same name, different signatures

**Time:** 3-4 hours | **Words:** ~15,000

---

### 3. 🚀 [Advanced-Patterns](03-Advanced-Patterns/README.md)
**Master advanced method concepts**

Core concepts: recursion, scope, special methods

**3 Files:**
- [Recursion](03-Advanced-Patterns/01-Recursion/00-Recursion.md) - Methods calling themselves
- [Method-Scope](03-Advanced-Patterns/02-Method-Scope/00-Method-Scope.md) - Visibility and interaction
- [Special-Methods](03-Advanced-Patterns/03-Special-Methods/00-Special-Methods.md) - Constructors, operators, etc.

**Time:** 4-5 hours | **Words:** ~17,000

---

### 4. ✅ [Best-Practices & Interview](04-Best-Practices-Interview/README.md)
**Professional standards and interview preparation**

Core concepts: quality, common mistakes, interview questions

**3 Files:**
- [Best-Practices](04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) - 12 guidelines
- [Common-Mistakes](04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md) - 12 mistakes to avoid
- [Interview-Questions](04-Best-Practices-Interview/03-Interview-Questions/00-Interview-Overview.md) - 15 progressive questions

**Time:** 5-8 hours | **Words:** ~22,000

---

## Choose Your Learning Path

### 🟢 Beginner Path (10-12 hours)
1. [Method-Fundamentals](01-Method-Fundamentals/README.md) - Learn basics
2. [Parameters-Overloading](02-Parameters-Overloading/README.md) - Work with parameters
3. [Advanced-Patterns](03-Advanced-Patterns/README.md) - Master advanced concepts
4. [Best-Practices](04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) - Professional standards

**Outcome:** Comfortable writing methods in production code

---

### 🟡 Intermediate Path (6-8 hours)
1. Quick skim [Method-Fundamentals](01-Method-Fundamentals/README.md)
2. [Parameters-Overloading](02-Parameters-Overloading/README.md) - Deep dive
3. [Advanced-Patterns](03-Advanced-Patterns/README.md) - Selected topics
4. [Best-Practices](04-Best-Practices-Interview/README.md) - Apply to your code

**Outcome:** Write high-quality, maintainable methods

---

### 🔴 Interview Prep Path (8-12 hours)
1. Review [Method-Fundamentals](01-Method-Fundamentals/README.md) - Quick refresh
2. Study [Parameters-Overloading](02-Parameters-Overloading/README.md) - Interview topics
3. Deep dive [Advanced-Patterns](03-Advanced-Patterns/README.md) - Design patterns
4. **Practice** [Interview-Questions](04-Best-Practices-Interview/03-Interview-Questions/00-Interview-Overview.md) - All 15 questions
5. Review [Common-Mistakes](04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md) - Avoid pitfalls

**Outcome:** Prepared for technical interviews

---

### ⚡ Quick Reference Path (30 minutes)
1. This README for overview
2. [Method-Basics](01-Method-Fundamentals/01-Method-Basics/00-Method-Basics.md) - 5 min
3. [Parameter-Types](02-Parameters-Overloading/01-Parameter-Types/00-Parameter-Types.md) - 10 min
4. [Method-Overloading](02-Parameters-Overloading/03-Method-Overloading/00-Method-Overloading.md) - 10 min

**Outcome:** Quick refresh on key concepts

---

## Core Concepts at a Glance

### Method Anatomy
```csharp
public int Add(int a, int b)  // Signature (access + return type + name + parameters)
{                              // Body starts
    return a + b;              // Implementation
}                              // Body ends
```

### Access Modifiers
| Modifier | Accessible From |
|----------|-----------------|
| `public` | Everywhere |
| `private` | This class only |
| `protected` | Derived classes |
| `internal` | Same assembly |

### Return Types
- `void` - No return value
- `int`, `double`, `string`, etc. - Primitive types
- `MyClass`, `List<T>`, etc. - Reference types
- `int?`, `string?` - Nullable types

### Parameters
- Value parameters (default) - Passed by value
- `ref` - Pass by reference, can modify
- `out` - Return multiple values
- `in` - Read-only reference
- `params` - Variable number of arguments

### Special Methods
- Constructors (`public ClassName()`) - Initialize objects
- Destructors (`~ClassName()`) - Clean up resources
- Extension methods (`static this Type`) - Add to types
- Operator overloads (`operator +`) - Custom operators

---

## Common Tasks Quick Links

### Basic Method Tasks
- Write a simple method → [Method-Basics](01-Method-Fundamentals/01-Method-Basics/00-Method-Basics.md)
- Return a value → [Return-Types](01-Method-Fundamentals/02-Return-Types/00-Return-Types.md)
- Document a method → [Method-Structure](01-Method-Fundamentals/03-Method-Structure/00-Method-Structure.md)

### Parameter Tasks
- Pass parameters → [Parameter-Types](02-Parameters-Overloading/01-Parameter-Types/00-Parameter-Types.md)
- Use default values → [Parameter-Types](02-Parameters-Overloading/01-Parameter-Types/00-Parameter-Types.md#default-parameters)
- Return multiple values with `out` → [Advanced-Parameters](02-Parameters-Overloading/02-Advanced-Parameters/00-Advanced-Parameters.md#out-parameters)
- Modify caller's variable with `ref` → [Advanced-Parameters](02-Parameters-Overloading/02-Advanced-Parameters/00-Advanced-Parameters.md#ref-parameters)

### Advanced Tasks
- Write recursive method → [Recursion](03-Advanced-Patterns/01-Recursion/00-Recursion.md)
- Create constructor → [Special-Methods](03-Advanced-Patterns/03-Special-Methods/00-Special-Methods.md#constructors)
- Overload method → [Method-Overloading](02-Parameters-Overloading/03-Method-Overloading/00-Method-Overloading.md)
- Implement TryParse → [Special-Methods](03-Advanced-Patterns/03-Special-Methods/00-Special-Methods.md#tryparse-pattern)

### Quality Tasks
- Refactor large method → [Best-Practices](04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md#1-single-responsibility-principle)
- Handle null safely → [Common-Mistakes](04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md#1-forgetting-to-handle-null)
- Validate inputs → [Common-Mistakes](04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md#3-not-validating-method-arguments)

---

## Difficulty Levels

### ✅ Beginner (Easy)
- Know method syntax
- Write simple methods
- Understand parameters and returns
- Call methods correctly

**Files:** [Method-Basics](01-Method-Fundamentals/01-Method-Basics/00-Method-Basics.md), [Parameter-Types](02-Parameters-Overloading/01-Parameter-Types/00-Parameter-Types.md)

---

### ⚙️ Intermediate (Medium)
- Write well-structured methods
- Use all parameter types
- Create method overloads
- Implement common patterns

**Files:** [Method-Structure](01-Method-Fundamentals/03-Method-Structure/00-Method-Structure.md), [Advanced-Parameters](02-Parameters-Overloading/02-Advanced-Parameters/00-Advanced-Parameters.md), [Method-Overloading](02-Parameters-Overloading/03-Method-Overloading/00-Method-Overloading.md)

---

### 🚀 Advanced (Hard)
- Master recursion
- Understand scope and visibility
- Use special methods effectively
- Apply design patterns
- Write production-quality code

**Files:** [Recursion](03-Advanced-Patterns/01-Recursion/00-Recursion.md), [Method-Scope](03-Advanced-Patterns/02-Method-Scope/00-Method-Scope.md), [Special-Methods](03-Advanced-Patterns/03-Special-Methods/00-Special-Methods.md), [Best-Practices](04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)

---

## Interview Question Preview

### Easy Questions (5)
1. What is a method and how do you call it?
2. What's the difference between parameters and return values?
3. What does void mean?
4. What is method overloading?
5. What's the difference between ref and out?

→ Full questions with answers: [Easy Questions](04-Best-Practices-Interview/03-Interview-Questions/00-Interview-Overview.md#easy-questions)

### Medium Questions (5)
1. How would you refactor a method that does too many things?
2. Explain recursion and when to use it
3. How do you handle null references safely?
4. How would you design a method to safely parse user input?
5. Explain static vs instance methods

→ Full questions with answers: [Medium Questions](04-Best-Practices-Interview/03-Interview-Questions/00-Interview-Overview.md#medium-questions)

### Hard Questions (5)
1. Design a method that safely processes a file with proper resource management
2. Create a method that implements the builder pattern
3. How would you implement method caching to avoid recalculation?
4. Design a method for complex business logic with multiple validation levels
5. How would you implement a decorator pattern for method enhancement?

→ Full questions with answers: [Hard Questions](04-Best-Practices-Interview/03-Interview-Questions/00-Interview-Overview.md#hard-questions)

---

## Key Statistics

| Metric | Value |
|--------|-------|
| Total Content | ~82,000 words |
| Number of Files | 16 (12 content + 4 README) |
| Number of Categories | 4 |
| Code Examples | 200+ |
| Interview Questions | 15 |
| Best Practices | 12 |
| Common Mistakes | 12 |
| Learning Paths | 8 |
| Exercise Ideas | 20+ |

---

## File Organization

```
04-Methods/
├── README.md (this file)
├── 01-Method-Fundamentals/
│   ├── README.md
│   ├── 01-Method-Basics/
│   │   └── 00-Method-Basics.md
│   ├── 02-Return-Types/
│   │   └── 00-Return-Types.md
│   └── 03-Method-Structure/
│       └── 00-Method-Structure.md
├── 02-Parameters-Overloading/
│   ├── README.md
│   ├── 01-Parameter-Types/
│   │   └── 00-Parameter-Types.md
│   ├── 02-Advanced-Parameters/
│   │   └── 00-Advanced-Parameters.md
│   └── 03-Method-Overloading/
│       └── 00-Method-Overloading.md
├── 03-Advanced-Patterns/
│   ├── README.md
│   ├── 01-Recursion/
│   │   └── 00-Recursion.md
│   ├── 02-Method-Scope/
│   │   └── 00-Method-Scope.md
│   └── 03-Special-Methods/
│       └── 00-Special-Methods.md
└── 04-Best-Practices-Interview/
    ├── README.md
    ├── 01-Best-Practices/
    │   └── 00-Best-Practices.md
    ├── 02-Common-Mistakes/
    │   └── 00-Common-Mistakes.md
    └── 03-Interview-Questions/
        └── 00-Interview-Overview.md
```

---

## How to Use This Guide

### If You're New to Methods
1. Start with [Method-Fundamentals](01-Method-Fundamentals/README.md)
2. Read each file in order
3. Do the exercises
4. Practice writing methods

### If You're Reviewing or Improving Code
1. Check [Best-Practices](04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
2. Review [Common-Mistakes](04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md)
3. Use as code review checklist

### If You're Preparing for Interviews
1. Quick review [Method-Fundamentals](01-Method-Fundamentals/README.md)
2. Study [Advanced-Patterns](03-Advanced-Patterns/README.md)
3. Practice [Interview-Questions](04-Best-Practices-Interview/03-Interview-Questions/00-Interview-Overview.md)
4. Review [Common-Mistakes](04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md)

### If You Need a Specific Topic
Use this reference guide to find files by topic:

**Basics & Syntax:**
- Method definition → [Method-Basics](01-Method-Fundamentals/01-Method-Basics/00-Method-Basics.md)
- Access modifiers → [Method-Basics](01-Method-Fundamentals/01-Method-Basics/00-Method-Basics.md#access-modifiers)
- Naming conventions → [Method-Structure](01-Method-Fundamentals/03-Method-Structure/00-Method-Structure.md)

**Parameters:**
- Basic parameters → [Parameter-Types](02-Parameters-Overloading/01-Parameter-Types/00-Parameter-Types.md)
- ref/out/in → [Advanced-Parameters](02-Parameters-Overloading/02-Advanced-Parameters/00-Advanced-Parameters.md)
- Default values → [Parameter-Types](02-Parameters-Overloading/01-Parameter-Types/00-Parameter-Types.md#default-parameters)
- Method overloading → [Method-Overloading](02-Parameters-Overloading/03-Method-Overloading/00-Method-Overloading.md)

**Returns:**
- Return types → [Return-Types](01-Method-Fundamentals/02-Return-Types/00-Return-Types.md)
- void vs others → [Return-Types](01-Method-Fundamentals/02-Return-Types/00-Return-Types.md#void-methods)
- Nullable returns → [Return-Types](01-Method-Fundamentals/02-Return-Types/00-Return-Types.md#nullable-reference-types)

**Advanced:**
- Recursion → [Recursion](03-Advanced-Patterns/01-Recursion/00-Recursion.md)
- Scope & visibility → [Method-Scope](03-Advanced-Patterns/02-Method-Scope/00-Method-Scope.md)
- Constructors → [Special-Methods](03-Advanced-Patterns/03-Special-Methods/00-Special-Methods.md#constructors)
- Operators → [Special-Methods](03-Advanced-Patterns/03-Special-Methods/00-Special-Methods.md#operator-overloading)
- Extension methods → [Special-Methods](03-Advanced-Patterns/03-Special-Methods/00-Special-Methods.md#extension-methods)

**Quality & Practices:**
- Best practices → [Best-Practices](04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
- Common mistakes → [Common-Mistakes](04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md)
- Refactoring → [Best-Practices](04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md#1-single-responsibility-principle)
- Error handling → [Common-Mistakes](04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md#7-swallowing-exceptions)

---

## Learning Outcomes

By completing this guide, you will:

✅ Understand method fundamentals and syntax
✅ Write methods with various parameter types
✅ Use ref, out, and in parameters correctly
✅ Create method overloads
✅ Understand and implement recursion
✅ Master method scope and visibility
✅ Use constructors and special methods
✅ Apply best practices in your code
✅ Avoid common mistakes
✅ Answer interview questions confidently

---

## Self-Assessment Checklist

### Fundamentals
- [ ] Understand method syntax and components
- [ ] Know four access modifiers
- [ ] Can write methods with return values
- [ ] Understand void methods

### Parameters
- [ ] Write methods with parameters
- [ ] Use default parameters
- [ ] Understand ref, out, in
- [ ] Create method overloads

### Advanced
- [ ] Write recursive methods
- [ ] Understand method scope
- [ ] Use constructors effectively
- [ ] Implement TryParse pattern

### Professional
- [ ] Apply best practices
- [ ] Avoid common mistakes
- [ ] Write professional code
- [ ] Prepare for interviews

---

## Tips for Success

1. **Read actively** - Take notes, don't just skim
2. **Practice coding** - Write examples from the guide
3. **Experiment** - Try variations and see what happens
4. **Test edge cases** - Null, empty, zero, negative, etc.
5. **Review others' code** - Learn from patterns and mistakes
6. **Do the exercises** - Apply knowledge to real problems
7. **Ask questions** - If something is unclear, investigate
8. **Connect concepts** - Link fundamentals to advanced patterns

---

## Next Steps

### Immediate (Next 30 minutes)
1. Pick your learning path above
2. Start with the first file
3. Read the introduction and overview

### This Week
1. Complete one category
2. Do the exercises
3. Write practice code

### Long-term
1. Apply best practices to all your code
2. Practice with advanced patterns
3. Prepare for technical interviews
4. Mentor others

---

## Related Topics in Fundamentals

- **[05-Exception-Handling](../05-Exception-Handling/README.md)** - Error handling in methods
- **[12-LINQ-Introduction](../12-LINQ-Introduction/README.md)** - Functional methods
- **[02-Operators](../02-Operators/README.md)** - Operator usage in methods
- **[03-Control-Flow](../03-Control-Flow/README.md)** - Control structures in methods

---

## Summary

Methods are the building blocks of C# programs. This comprehensive guide covers:

- **Fundamentals** - How methods work
- **Parameters** - How to pass data
- **Advanced Patterns** - Special techniques
- **Best Practices** - Professional standards
- **Interview Prep** - Technical questions

Choose your learning path above and start improving your method skills today!

---

**Last Updated:** 2024
**Difficulty Range:** Beginner to Advanced
**Total Study Time:** 12-20 hours for complete mastery
**Recommendation:** Start with Method-Fundamentals and progress through categories
