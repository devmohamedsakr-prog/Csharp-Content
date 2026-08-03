# C# Data Types: Complete Learning Guide

## Overview

This comprehensive guide covers all C# data types - from fundamental value types like integers and booleans to complex reference types like classes and collections. Each type is thoroughly explained with practical examples, best practices, and common mistakes to avoid.

## Folder Structure

```
Data-Types/
├── 01-Value-Types/                    [Stack-based, copied by value]
│   ├── 01-Numeric/                    [int, long, float, decimal]
│   ├── 02-Boolean-Char/               [bool, char types]
│   ├── 03-Structs/                    [User-defined value types]
│   └── README.md                      [Value Types overview]
│
├── 02-Reference-Types/                [Heap-based, copied by reference]
│   ├── 01-String/                     [Text and string operations]
│   ├── 02-Classes/                    [User-defined reference types]
│   ├── 03-Interfaces/                 [Contracts and polymorphism]
│   ├── 04-Arrays-Collections/         [Arrays, List, Dictionary, etc.]
│   ├── 05-Delegates/                  [Type-safe function references]
│   └── README.md                      [Reference Types overview]
│
├── 03-Comparison-Practices/           [Analysis and guidance]
│   ├── 01-Value-vs-Reference/         [Detailed comparison]
│   ├── 02-Best-Practices/             [Do's and don'ts]
│   ├── 03-Common-Mistakes/            [13 mistakes + solutions]
│   └── README.md                      [Comparison & Practices overview]
│
├── 04-Interview-Questions/            [Real interview questions]
│   ├── 01-Easy/                       [12 basic questions]
│   ├── 02-Medium/                     [9 intermediate questions]
│   ├── 03-Hard/                       [6 expert questions]
│   ├── 00-Interview-Questions-Overview.md  [Complete interview guide]
│   └── README.md                      [Interview section overview]
│
└── README.md                          [This file - Main guide]
```

---

## Quick Navigation

### 🔹 Value Types (Stack Memory)

For small, immutable data stored on the stack.

#### 01-Numeric Types
- **File**: `01-Value-Types/01-Numeric/00-Numeric-Types-Overview.md`
- **Covers**: Integer types (byte, short, int, long), floating-point types (float, double), decimal for money
- **Key Topics**: 
  - Type selection guide
  - Overflow handling
  - Integer division
  - Precision considerations
- **When to Read**: When working with numbers and need to choose appropriate type

#### 02-Boolean and Char Types
- **File**: `01-Value-Types/02-Boolean-Char/00-Boolean-Char-Types.md`
- **Covers**: Boolean logic, character handling, Unicode support
- **Key Topics**:
  - Logical operations (&&, ||, !)
  - Character encoding and escape sequences
  - Case conversion
  - Common mistakes
- **When to Read**: When working with true/false values or individual characters

#### 03-Structs (User-Defined Value Types)
- **File**: `01-Value-Types/03-Structs/00-Structs-ValueTypes.md`
- **Covers**: Creating value types, mutable vs immutable, struct vs class
- **Key Topics**:
  - Struct definition and initialization
  - When to use structs
  - Performance considerations
  - Boxing and unboxing
- **When to Read**: When designing custom value types or choosing between struct and class

---

### 🔴 Reference Types (Heap Memory)

For complex objects and large data stored on the heap.

#### 01-String Type
- **File**: `02-Reference-Types/01-String/00-String-ReferenceType.md`
- **Covers**: String operations, immutability, formatting, and manipulation
- **Key Topics**:
  - String creation (literals, verbatim, interpolation)
  - Common string operations (Substring, Replace, Split, etc.)
  - Formatting and parsing
  - Performance with StringBuilder
- **When to Read**: When working with text and need comprehensive string knowledge

#### 02-Classes (User-Defined Reference Types)
- **File**: `02-Reference-Types/02-Classes/00-Classes-ReferenceType.md`
- **Covers**: Class structure, inheritance, constructors, properties, and patterns
- **Key Topics**:
  - Class members (fields, properties, methods)
  - Constructors and initialization
  - Inheritance and virtual methods
  - Access modifiers and encapsulation
- **When to Read**: When designing object-oriented solutions and domain models

#### 03-Interfaces (Contracts)
- **File**: `02-Reference-Types/03-Interfaces/00-Interfaces-Contracts.md`
- **Covers**: Interface design, polymorphism, and design patterns
- **Key Topics**:
  - Defining and implementing interfaces
  - Multiple interface implementation
  - Default members (C# 8+)
  - Dependency injection patterns
- **When to Read**: When designing extensible systems or applying SOLID principles

#### 04-Arrays and Collections
- **File**: `02-Reference-Types/04-Arrays-Collections/00-Arrays-Collections.md`
- **Covers**: Arrays, List<T>, Dictionary<K,V>, HashSet<T>, and other collections
- **Key Topics**:
  - Array creation and operations
  - Collection types and their use cases
  - LINQ operations
  - Performance considerations
- **When to Read**: When storing and managing groups of data

#### 05-Delegates (Function References)
- **File**: `02-Reference-Types/05-Delegates/00-Delegates-FunctionTypes.md`
- **Covers**: Delegates, Action<T>, Func<T>, events, and callbacks
- **Key Topics**:
  - Delegate definition and usage
  - Multicast delegates
  - Built-in generic delegates
  - Event pattern
- **When to Read**: When implementing callbacks, strategies, or event handling

---

### 📊 Comparison and Best Practices

#### 01-Value Types vs Reference Types
- **File**: `03-Comparison-Practices/01-Value-vs-Reference/00-Value-Reference-Comparison.md`
- **Covers**: Complete side-by-side comparison
- **Key Topics**:
  - Storage location (stack vs heap)
  - Copy behavior
  - Default values
  - Equality comparison
  - Performance implications
- **When to Read**: When deciding between value and reference types, understanding memory behavior

#### 02-Best Practices
- **File**: `03-Comparison-Practices/02-Best-Practices/00-DataType-BestPractices.md`
- **Covers**: Guidelines for each data type
- **Key Topics**:
  - Choosing right numeric type
  - String handling best practices
  - Collection selection guide
  - Nullable type usage
  - Type conversion safety
- **When to Read**: Before writing code to follow proven patterns

#### 03-Common Mistakes
- **File**: `03-Comparison-Practices/03-Common-Mistakes/00-Common-Mistakes.md`
- **Covers**: 13 common mistakes with solutions
- **Key Topics**:
  - Float for money (wrong!)
  - String concatenation loops
  - Collection modification during iteration
  - Type casting errors
  - Defensive programming
- **When to Read**: To avoid pitfalls and write more robust code

---

### 📝 Interview Questions (Real Interview Scenarios)

Comprehensive collection of real interview questions organized by difficulty level.

#### Interview Questions Overview
- **File**: `04-Interview-Questions/README.md`
- **Covers**: How to use this section, tips, and success criteria
- **Key Topics**:
  - Interview tips and techniques
  - Common traps to avoid
  - Success criteria
  - How to prepare effectively
- **When to Read**: Before diving into specific questions

#### Easy Level Questions (12 Questions)
- **File**: `04-Interview-Questions/01-Easy/00-Easy-Questions.md`
- **Covers**: Basic data type concepts
- **Questions Include**:
  - Value types vs reference types
  - Default values
  - Type selection (int vs long, float vs decimal)
  - String immutability
  - Access modifiers
  - Array vs List
  - Boxing/unboxing
- **Time**: ~5-10 minutes per question
- **When to Read**: As warm-up before interviews

#### Medium Level Questions (9 Questions)
- **File**: `04-Interview-Questions/02-Medium/00-Medium-Questions.md`
- **Covers**: Practical application and design
- **Questions Include**:
  - Designing a Money type
  - Collection selection (HashSet vs List)
  - Nullable reference types
  - Cache design patterns
  - String interning
  - Generic patterns
  - Covariance/contravariance
- **Time**: ~15-20 minutes per question
- **When to Read**: Core interview preparation

#### Hard Level Questions (6 Questions)
- **File**: `04-Interview-Questions/03-Hard/00-Hard-Questions.md`
- **Covers**: Expert-level scenarios
- **Questions Include**:
  - Generic repository pattern
  - Type-safe event systems
  - Large collection processing
  - Immutable collection APIs
  - Memory leak prevention
  - Type-safe configuration
- **Time**: ~20-30 minutes per question
- **When to Read**: Deep preparation for senior positions

#### All Questions Quick Guide
- **File**: `04-Interview-Questions/00-Interview-Questions-Overview.md`
- **Covers**: Complete overview with quick reference
- **Content**:
  - 27 total questions with full answers
  - Common mistakes and solutions
  - Preparation tips
  - Quick reference guide
- **When to Read**: For complete question reference

---

## Learning Paths

### 🚀 Path 1: Complete Beginner (2-3 weeks)
Start here if new to C# or programming:

1. **Week 1**: Numeric types and boolean
   - `01-Numeric-Types-Overview.md`
   - `02-Boolean-Char-Types.md`

2. **Week 2**: Strings and basic classes
   - `03-Structs-ValueTypes.md`
   - `02-Classes-ReferenceType.md`
   - `01-String-ReferenceType.md`

3. **Week 3**: Collections and practices
   - `04-Arrays-Collections.md`
   - `02-Best-Practices.md`

### 🎯 Path 2: Intermediate Developer (1 week)
Already familiar with basics:

1. **Deep Dives**:
   - `01-Value-Reference-Comparison.md` (understand memory)
   - `03-Interfaces-Contracts.md` (OOP design)
   - `05-Delegates-FunctionTypes.md` (advanced patterns)

2. **Practical**:
   - `00-Common-Mistakes.md` (avoid pitfalls)
   - `02-Best-Practices.md` (improve code)

### 🔬 Path 3: Advanced/Interview Prep (2-3 days)
Brush up before interviews:

1. **Review Quick**:
   - `01-Value-Reference-Comparison.md` (5 min)
   - `00-DataType-BestPractices.md` (10 min)

2. **Deep Topics**:
   - `02-Classes-ReferenceType.md` - Inheritance section
   - `03-Interfaces-Contracts.md` - Polymorphism
   - `04-Arrays-Collections.md` - Performance section

3. **Interview Prep**:
   - `04-Interview-Questions/01-Easy/00-Easy-Questions.md` (30 min)
   - `04-Interview-Questions/02-Medium/00-Medium-Questions.md` (2 hours)
   - `04-Interview-Questions/03-Hard/00-Hard-Questions.md` (2 hours)

4. **Mistakes**:
   - `00-Common-Mistakes.md` (avoid during interviews)

---

## Key Concepts Quick Reference

### Value vs Reference at a Glance

```
VALUE TYPES (Stack)          REFERENCE TYPES (Heap)
─────────────────────        ─────────────────────
int, double, decimal         string, class
bool, char                   array, List<T>
struct                       Dictionary, HashSet
DateTime, Guid              delegate, interface

Copy Value                   Copy Reference
Independent                  Shared
Default: 0/false             Default: null
No GC needed                 Garbage collected
Stack allocation (fast)      Heap allocation (slower)
Limited size                 Unlimited size
```

### When to Use Each Type

| Type | Best For | Example |
|------|----------|---------|
| **int** | General integers | `int count = 100;` |
| **decimal** | Money/financial | `decimal price = 99.99m;` |
| **string** | Text | `string name = "Alice";` |
| **bool** | Conditions | `bool isActive = true;` |
| **struct** | Small immutable data | `struct Point { int X, Y; }` |
| **class** | Complex objects | `class Customer { ... }` |
| **List<T>** | Dynamic arrays | `List<int> numbers = new();` |
| **Dictionary<K,V>** | Key-value pairs | `Dictionary<string, int> ages = new();` |
| **HashSet<T>** | Unique items | `HashSet<int> unique = new();` |
| **interface** | Contracts | `interface IAnimal { void MakeSound(); }` |

---

## Study Tips

### 1. Read in Order
- Start with Value Types (simpler to understand)
- Move to Reference Types (more complex)
- Study Comparison & Best Practices (cement knowledge)

### 2. Code Along
- Don't just read - write code!
- Try the examples yourself
- Modify examples to experiment
- Create small projects combining concepts

### 3. Focus on Understanding
- Why choose int over long?
- Why is string immutable?
- How does garbage collection affect performance?
- When should you use struct vs class?

### 4. Practice Problems
- After each section, write code using that type
- Solve challenges using multiple types
- Refactor code following best practices

### 5. Review Common Mistakes
- Study each mistake
- Understand why it's wrong
- Remember the solution
- Avoid in your own code

---

## File Organization

Each category has consistent structure:

```
Category/
├── 00-FileName.md          [Main comprehensive guide]
├── 01-Explanation/         [Detailed theory]
├── 02-Examples/            [Practical examples (to be added)]
└── 03-Code-Implementations/ [Working code (to be added)]
```

**Status**:
- ✅ README files and main guides complete (11 files total)
- 🔲 Examples section (to be populated)
- 🔲 Code implementation section (to be populated)

---

## Cross-References

### By Problem
- **Need to store money?** → Numeric Types (decimal section)
- **Working with text?** → String Type
- **Storing multiple items?** → Arrays & Collections
- **Creating custom types?** → Classes or Structs
- **Need shared behavior?** → Interfaces
- **Want callbacks/strategies?** → Delegates

### By Performance Concern
- **Stack vs Heap?** → Value vs Reference Comparison
- **String performance?** → String Type (StringBuilder section)
- **Collection performance?** → Arrays & Collections (Performance section)
- **Boxing overhead?** → Structs (Boxing section)

### By Design Pattern
- **Dependency Injection?** → Interfaces (Dependency Injection section)
- **Strategy Pattern?** → Delegates or Interfaces
- **Singleton Pattern?** → Classes (Common Class Patterns)
- **Factory Pattern?** → Interfaces (Factory Pattern section)

---

## Common Questions Answered

**Q: Should I use int or long?**
A: Use `int` by default. Only use `long` for very large numbers or specific cases like timestamps. See Numeric Types.

**Q: Is decimal always better than double?**
A: Not always. Use `decimal` for money, `double` for scientific calculations. See Best Practices.

**Q: When should I use struct instead of class?**
A: Use struct for small, immutable data only. Classes are the default. See Structs guide.

**Q: Why can't I modify strings?**
A: Strings are immutable. Use `StringBuilder` for many string operations. See String Type.

**Q: What's the difference between List and HashSet?**
A: List maintains order, HashSet ensures uniqueness. See Arrays & Collections.

**Q: Should I use null or null-coalescing?**
A: Use null-coalescing (`??`) and null-conditional (`?.`) operators. See Best Practices.

---

## Performance Quick Tips

1. **Use `int` not `long`** (unless necessary)
2. **Use `decimal` for money** (avoid float/double)
3. **Use `StringBuilder` for many string operations**
4. **Use `HashSet<T>` for membership tests** (not `List<T>`)
5. **Avoid boxing value types** (use generic collections)
6. **Use `struct` for small immutable data** only
7. **Return `IReadOnlyList<T>`** not `List<T>`
8. **Avoid string concatenation in loops**

---

## Next Steps After Completing This Guide

1. **Practice**: Write programs using various data types
2. **Refactor**: Review old code and improve type usage
3. **Interview**: Test knowledge with Data Types interview questions
4. **Deep Dive**: Study individual topics in more detail if needed
5. **Apply**: Use best practices in production code

---

## Summary

This guide provides complete coverage of C# data types:

- **8 Value Type files** covering all built-in and user-defined value types
- **5 Reference Type files** covering strings, classes, interfaces, collections, and delegates
- **3 Comparison files** with detailed analysis, best practices, and common mistakes
- **Focused, modular learning** - read what you need
- **Practical examples** and real-world scenarios
- **Interview preparation** - includes questions and pitfalls to avoid

**Total Content**: 11 comprehensive markdown files + 4 interview question files (~25,000 words)
**Estimated Reading Time**: 2-3 weeks for complete learning, 2-3 days for review

---

**Last Updated**: August 2026
**Focus**: Comprehensive understanding of C# data types with practical application + interview preparation
