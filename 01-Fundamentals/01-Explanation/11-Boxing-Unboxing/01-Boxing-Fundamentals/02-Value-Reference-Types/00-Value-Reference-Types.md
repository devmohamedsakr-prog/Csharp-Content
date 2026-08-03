# Value Types vs Reference Types

## Overview

Understanding the difference between value types and reference types is fundamental to understanding boxing. This section covers how they work in memory and why boxing exists.

## Value Types

Value types store data directly on the stack. The variable contains the actual value.

### Value Types Include

- **Primitives**: int, double, bool, char, etc.
- **Structs**: user-defined struct
- **Enums**: enum types
- **Nullable types**: int?, double?, etc.

### Value Type Memory Layout

```csharp
int x = 42;
// Memory:
// Stack: [x = 42]  (actual value stored)

struct Point
{
    public int X, Y;
}

Point p = new Point { X = 10, Y = 20 };
// Memory:
// Stack: [p = { X: 10, Y: 20 }]  (entire struct on stack)
```

### Value Type Behavior

```csharp
// Copying creates independent copy
int a = 10;
int b = a;      // b = 10 (copy of value)
a = 20;         // a = 20
// b is still 10 (not affected)

struct Point { public int X; }
Point p1 = new Point { X = 10 };
Point p2 = p1;  // p2 = { X: 10 } (copy of struct)
p1.X = 20;      // p1 = { X: 20 }
// p2 is still { X: 10 } (not affected)
```

### Stack Allocation Benefits

- Fast allocation
- Automatic cleanup (scope exit)
- No garbage collection needed
- Cache-friendly memory locality

### Stack Allocation Drawbacks

- Limited size (stack overflow risk)
- Size must be known at compile time
- Short lifetime (scope-based)

## Reference Types

Reference types store a reference (pointer) to data on the heap. The variable contains only the address.

### Reference Types Include

- **Classes**: user-defined classes
- **Interfaces**: interface types
- **Arrays**: array instances
- **Strings**: string instances
- **Delegates**: delegate instances

### Reference Type Memory Layout

```csharp
class Person
{
    public int Age;
    public string Name;
}

Person p = new Person { Age = 30, Name = "Alice" };
// Memory:
// Stack: [p = 0x00001234]  (reference/address)
// Heap:  [0x00001234: { Age: 30, Name: ... }]
```

### Reference Type Behavior

```csharp
// Copying shares reference
Person p1 = new Person { Age = 30 };
Person p2 = p1;         // p2 references same object
p1.Age = 40;            // Modify through p1
// p2.Age is also 40 (same object)

// Reference equals identity, not value
Person a = new Person { Age = 25 };
Person b = new Person { Age = 25 };
bool same = (a == b);   // false (different objects)
bool same2 = (a == a);  // true (same object)
```

### Heap Allocation Benefits

- Unlimited size
- Can outlive scope
- Shared references

### Heap Allocation Drawbacks

- Slower allocation
- Requires garbage collection
- Less cache-friendly
- Memory overhead

## Comparison Table

| Feature | Value Type | Reference Type |
|---------|-----------|-----------------|
| Storage | Stack | Heap |
| Size | Must be known | Dynamic |
| Default Value | Zero (0, false, etc.) | null |
| Assignment | Copies value | Copies reference |
| Equality | Value comparison | Reference comparison |
| Lifetime | Scope-based | GC-managed |
| Performance | Fast | Slower (GC) |
| Example | int, struct | class, string |

## Memory Comparison

### Value Type on Stack
```
Stack Layout:
┌─────────────────┐
│ int x = 42      │ (4 bytes, direct value)
│ double y = 3.14 │ (8 bytes, direct value)
│ bool z = true   │ (1 byte, direct value)
└─────────────────┘
Total: ~13 bytes on stack
Cleanup: automatic when scope exits
```

### Reference Type on Heap
```
Stack Layout:
┌──────────────────────┐
│ Person p = 0x2000    │ (8 bytes, address)
└──────────────────────┘

Heap Layout:
┌──────────────────────┐
│ Object at 0x2000:    │
│ - Type info          │
│ - Age field (int)    │
│ - Name field (ref)   │
│ - Other fields       │
└──────────────────────┘
Total: 8 bytes stack + object size on heap
Cleanup: GC when no references remain
```

## Passing Parameters

### Value Type Parameter

```csharp
void ModifyValue(int x)
{
    x = 100;  // Modifies copy
}

int original = 42;
ModifyValue(original);
// original is still 42 (copy was modified)
```

### Reference Type Parameter

```csharp
void ModifyReference(Person p)
{
    p.Age = 100;  // Modifies shared object
}

Person person = new Person { Age = 25 };
ModifyReference(person);
// person.Age is now 100 (shared object modified)
```

### Out Parameter (Value Type)

```csharp
void OutValueExample(out int x)
{
    x = 42;  // Sets caller's variable
}

int num;
OutValueExample(out num);
// num is 42 (caller's variable directly modified)
```

## Default Values

### Value Type Defaults

```csharp
// Default values for value types
int x = default;        // 0
double d = default;     // 0.0
bool b = default;       // false
char c = default;       // '\0'
struct S { }
S s = default;          // { } (zero-initialized)

// Arrays of value types
int[] arr = new int[10];  // { 0, 0, 0, ... }
```

### Reference Type Defaults

```csharp
// Default values for reference types
string s = default;     // null
object o = default;     // null
class C { }
C c = default;          // null
int[] arr = null;       // null initially

// Arrays of reference types
string[] names = new string[10];  // { null, null, ... }
```

## Boxing Connection

### Why Boxing Exists

Boxing exists because sometimes you need to treat a value type as a reference:

```csharp
// Value type on stack
int x = 42;

// Sometimes need reference
object boxed = x;  // Converts to reference type

// Why? For compatibility with code expecting object
void ProcessObject(object obj) { }
ProcessObject(x);  // x must be boxed
```

### The Problem Boxing Solves

```csharp
// Old .NET collections expected object
ArrayList list = new ArrayList();
list.Add(42);      // int must become object reference
// Internally: new int wrapper object created on heap

// Modern generics solve this
List<int> genericList = new List<int>();
genericList.Add(42);  // No boxing needed
```

## Struct vs Class

### When to Use Each

```csharp
// VALUE TYPE (struct) - when:
struct Point
{
    public int X, Y;
    // - Small amount of data
    // - Immutable
    // - Frequent allocation
    // - Want stack allocation
}

// REFERENCE TYPE (class) - when:
class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    // - Larger data
    // - Mutable
    // - Want identity
    // - Want polymorphism
}
```

## Key Concepts

1. **Value types** store values directly on stack
2. **Reference types** store references to heap
3. **Boxing** converts value to reference
4. **Unboxing** converts reference back to value
5. **Structs** are value types
6. **Classes** are reference types

## Performance Implications

```csharp
// Value type: fast, stack-allocated
int[] valueInts = new int[1000];  // 4KB on stack

// Reference type: slower, heap-allocated
object[] refObjects = new object[1000];  // references on stack + objects on heap
```

## Summary

- Value types: stored on stack, copy on assignment
- Reference types: stored on heap, share references
- Boxing: converts value type to reference type
- Understanding this is key to understanding boxing

## Next Steps

- Study [Boxing-Conversions](../03-Boxing-Conversions/00-Boxing-Conversions.md) for how to convert
- Learn [Boxing-Collections](../04-Boxing-Collections/00-Boxing-Collections.md) for practical patterns
- Explore unboxing in [Unboxing-Type-Safety](../../02-Unboxing-Type-Safety/01-Unboxing-Rules/00-Unboxing-Rules.md)
