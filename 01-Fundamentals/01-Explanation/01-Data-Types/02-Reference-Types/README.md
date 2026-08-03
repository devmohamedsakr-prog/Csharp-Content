# Reference Types

## Overview

Reference types in C# store a reference (memory address) to data on the heap. Multiple variables can point to the same object in memory. When you assign a reference type to another variable or pass it as a parameter, you're copying the reference, not the data.

## Key Characteristics

- **Storage**: Heap memory (referenced via stack)
- **Copy Behavior**: Only reference (pointer) is copied
- **Garbage Collection**: Automatically cleaned up when no references exist
- **Default Value**: null
- **Inheritance**: Can inherit from other classes
- **Performance**: Slightly slower due to heap indirection, but more flexible

## Categories in This Section

### 1. Strings
Immutable sequences of characters. Special in C# with string interning and special operators.

**Files**: `01-String/00-String-ReferenceType.md`

### 2. Classes
Blueprint for creating objects with data (fields/properties) and behavior (methods). Supports inheritance and polymorphism.

**Files**: `02-Classes/00-Classes-ReferenceType.md`

### 3. Interfaces
Contracts that define what methods and properties a type must implement. Supports polymorphism without inheritance.

**Files**: `03-Interfaces/00-Interfaces-Contracts.md`

### 4. Arrays and Collections
Built-in types for storing multiple items: arrays, List<T>, Dictionary<K,V>, HashSet<T>, etc.

**Files**: `04-Arrays-Collections/00-Arrays-Collections.md`

### 5. Delegates
Type-safe references to methods. Foundation for events and callbacks.

**Files**: `05-Delegates/00-Delegates-FunctionTypes.md`

## Reference Type Behavior

### Heap Allocation
```csharp
List<int> list1 = new() { 1, 2, 3 };
List<int> list2 = list1;  // Copy reference only
list2.Add(4);
Console.WriteLine(list1.Count);  // 4 (both point to same object)
```

### Null References
```csharp
string text = null;  // Valid reference type
if (text != null) {
    int length = text.Length;  // Safe
}
```

### Default Values
```csharp
string defaultString = default;        // null
List<int> defaultList = default;       // null
MyClass defaultClass = default;        // null
```

## Quick Comparison

| Aspect | String | Class | Interface | Array | List |
|--------|--------|-------|-----------|-------|------|
| **Mutable** | No | Yes | N/A | Yes | Yes |
| **Inheritance** | N/A | Yes | Yes | N/A | N/A |
| **Generics** | No | Yes | Yes | Yes | Yes |
| **Performance** | Fast (immutable) | Medium | Medium | Fast | Medium |

## Common Reference Types

```csharp
// String - immutable text
string name = "Alice";

// Class - custom object
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
}

// Array - fixed collection
int[] numbers = new int[10];

// List - dynamic collection
List<string> items = new();

// Dictionary - key-value pairs
Dictionary<string, int> ages = new();

// HashSet - unique items only
HashSet<string> tags = new();

// Interface - contract
public interface IRepository<T> {
    T GetById(int id);
}
```

## When to Use Reference Types

✓ Complex objects with behavior
✓ Collections of items
✓ Inheritance hierarchies
✓ Polymorphic designs
✓ Large or variable-sized data

## Memory Management

Reference types are automatically managed by garbage collection:

```csharp
// Allocation
List<int> list = new() { 1, 2, 3 };  // Allocated on heap

// No explicit deallocation needed
list = null;  // Reference dropped, GC will clean up
// Heap memory freed when no other references exist
```

## Common Pitfalls

❌ Not checking for null before use
❌ Modifying shared references unintentionally
❌ Large collections creating memory pressure
❌ Circular references (though GC handles this)

## Learning Path

1. Start with **Strings** - most common reference type
2. Learn **Classes** - foundation of OOP
3. Explore **Interfaces** - abstract design patterns
4. Study **Arrays and Collections** - working with multiple items
5. Understand **Delegates** - functional programming support
6. Compare with **Value Types** for design decisions

## Navigation

- **Parent**: [Data Types](../README.md)
- **Strings**: `01-String/00-String-ReferenceType.md`
- **Classes**: `02-Classes/00-Classes-ReferenceType.md`
- **Interfaces**: `03-Interfaces/00-Interfaces-Contracts.md`
- **Arrays & Collections**: `04-Arrays-Collections/00-Arrays-Collections.md`
- **Delegates**: `05-Delegates/00-Delegates-FunctionTypes.md`
- **Comparison & Practices**: `../03-Comparison-Practices/README.md`
