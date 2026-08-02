# C# Fundamentals - Interview Questions & Answers

## 1. What are the different data types in C#?

**Answer:**
C# data types are divided into two categories:

### Value Types (Stack)
- **Numeric Types**: byte, short, int, long, float, double, decimal
- **Boolean**: bool (true/false)
- **Character**: char (single Unicode character)
- **Struct**: User-defined value types

### Reference Types (Heap)
- **String**: Immutable sequence of characters
- **Class**: User-defined reference types
- **Interface**: Contract for classes
- **Delegate**: Type-safe function pointers
- **Array**: Collection of elements

**Key Difference**: Value types store data directly; reference types store a reference to data on the heap.

---

## 2. What is the difference between value types and reference types?

**Answer:**

| Feature | Value Type | Reference Type |
|---------|-----------|-----------------|
| Storage | Stack | Heap |
| Assignment | Copies data | Copies reference |
| Default Value | 0/false | null |
| Memory | Small, predictable | Variable size |
| Garbage Collection | No | Yes |
| Nullable | With ? modifier | Inherently nullable |

**Example:**
```csharp
// Value Type
int x = 5;
int y = x;  // Copies value
y = 10;
Console.WriteLine(x);  // 5 (unchanged)

// Reference Type
List<int> list1 = new List<int> { 1, 2 };
List<int> list2 = list1;  // Copies reference
list2.Add(3);
Console.WriteLine(list1.Count);  // 3 (both point to same list)
```

---

## 3. What are boxing and unboxing?

**Answer:**

**Boxing**: Converting a value type to an object (reference type)
**Unboxing**: Converting a boxed object back to value type

```csharp
// Boxing
int num = 123;
object obj = num;  // Boxing

// Unboxing
int num2 = (int)obj;  // Unboxing

// Common mistake - type mismatch
double d = 10.5;
object boxed = d;
int unboxed = (int)boxed;  // Runtime error - InvalidCastException
```

**Performance Note**: Boxing/unboxing has performance overhead, avoid in performance-critical code.

---

## 4. What is the difference between string and StringBuilder?

**Answer:**

**String**: Immutable - creates new object for each change
**StringBuilder**: Mutable - modifies existing object

```csharp
// String (inefficient for multiple concatenations)
string result = "";
for (int i = 0; i < 1000; i++) {
    result += "a";  // Creates new string each iteration
}

// StringBuilder (efficient)
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append("a");  // Modifies existing object
}
string result = sb.ToString();
```

**When to Use**:
- **String**: Single concatenation, displaying data
- **StringBuilder**: Multiple concatenations, loops

---

## 5. What are nullable types?

**Answer:**

Nullable types allow value types to have null values using the `?` modifier.

```csharp
// Non-nullable
int x = null;  // Compile error

// Nullable
int? x = null;  // OK
int? y = 5;     // OK

// Checking for null
if (x.HasValue) {
    Console.WriteLine(x.Value);  // 5
}

// Null coalescing operator
int result = x ?? 10;  // 10 if x is null, x value otherwise
```

---

## 6. What is the difference between == and Equals()?

**Answer:**

**==**: Reference equality (for reference types) or value equality (for value types)
**Equals()**: Compares actual values (can be overridden)

```csharp
// Reference types
object a = "hello";
object b = "hello";
Console.WriteLine(a == b);        // false (different references)
Console.WriteLine(a.Equals(b));   // true (same value)

// Value types
int x = 5;
int y = 5;
Console.WriteLine(x == y);        // true
Console.WriteLine(x.Equals(y));   // true
```

---

## 7. What are keywords const, readonly, and static?

**Answer:**

| Keyword | Scope | When Set | Modifiable | Use Case |
|---------|-------|----------|-----------|----------|
| const | Compile-time constant | Compile time | No | Fixed values |
| readonly | Instance/class | Constructor/declaration | No | Final values |
| static | Class-level | Once | Yes (via method) | Shared data |

```csharp
const int MAX_SIZE = 100;  // Compile-time constant

class MyClass {
    readonly string name;  // Set in constructor
    static int count = 0;   // Shared across all instances
    
    public MyClass(string n) {
        name = n;
    }
}
```

---

## 8. What are the different loop types and when to use each?

**Answer:**

```csharp
// for loop - when you know iteration count
for (int i = 0; i < 10; i++) {
    Console.WriteLine(i);
}

// while loop - when condition is uncertain
int num = 0;
while (num < 10) {
    Console.WriteLine(num);
    num++;
}

// do-while loop - executes at least once
do {
    Console.WriteLine("Enter something: ");
} while (input == "");

// foreach loop - iterating collections
int[] numbers = { 1, 2, 3 };
foreach (int n in numbers) {
    Console.WriteLine(n);
}
```

---

## 9. What is the difference between break, continue, and return?

**Answer:**

```csharp
// break - exits loop immediately
for (int i = 0; i < 10; i++) {
    if (i == 5) break;  // Loop ends at 5
    Console.WriteLine(i);
}

// continue - skips current iteration
for (int i = 0; i < 10; i++) {
    if (i == 5) continue;  // Skips 5, continues loop
    Console.WriteLine(i);
}

// return - exits method
public int GetValue() {
    return 42;  // Exits method immediately
}
```

---

## 10. What is exception handling and what are try-catch-finally?

**Answer:**

```csharp
try {
    // Code that might throw exception
    int x = int.Parse("not a number");
} 
catch (FormatException ex) {
    // Handle specific exception
    Console.WriteLine($"Format error: {ex.Message}");
}
catch (Exception ex) {
    // Handle general exception
    Console.WriteLine($"Error: {ex.Message}");
}
finally {
    // Always executes, with or without exception
    Console.WriteLine("Cleanup code");
}
```

**Best Practices**:
- Catch specific exceptions first, general exceptions last
- Always use finally for cleanup (or use `using` statement)
- Don't catch and ignore exceptions silently

---

## Quick Tips for Interview

✓ Know the difference between value and reference types
✓ Understand boxing/unboxing implications
✓ Be ready to explain string vs StringBuilder
✓ Know when to use const vs readonly
✓ Understand exception handling best practices
✓ Be comfortable with all loop types and control flow
