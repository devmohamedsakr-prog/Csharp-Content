# Data Types Interview - Easy Level Questions

## Question 1: What are Value Types and Reference Types?

### Question
Explain the difference between value types and reference types in C#.

### Answer
**Value Types** store data directly in memory (stack) and are copied by value.
**Reference Types** store a reference to data on the heap and are copied by reference.

**Example**:
```csharp
// Value Type
int x = 10;
int y = x;  // Copies value
y = 20;
Console.WriteLine(x);  // 10 (unchanged)

// Reference Type
List<int> list1 = new() { 1, 2, 3 };
List<int> list2 = list1;  // Copies reference
list2.Add(4);
Console.WriteLine(list1.Count);  // 4 (changed!)
```

**Key Points**:
- Value types on stack, reference types on heap
- Value types copied entirely, reference types share data
- Value types fast but limited, reference types flexible but slower

---

## Question 2: What is the Default Value for Each Data Type?

### Question
What are the default values for int, decimal, bool, string, and a custom class?

### Answer
```csharp
int defaultInt = default;              // 0
decimal defaultDecimal = default;      // 0.0m
bool defaultBool = default;            // false
string defaultString = default;        // null
MyClass defaultClass = default;        // null

// In class fields (implicit defaults)
public class Example {
    public int count;                  // 0
    public decimal price;              // 0.0m
    public string name;                // null
}
```

**Summary**:
- Value types default to 0/false/null char
- Reference types default to null

---

## Question 3: When Should You Use int vs long?

### Question
When would you use `long` instead of `int` in C#?

### Answer
**Use `int` by default** - it's optimized and sufficient for most cases.

**Use `long` when**:
- Dealing with very large numbers (> 2 billion)
- Working with timestamps or milliseconds
- Specific API requirements
- Large data sets that need the range

**Example**:
```csharp
// Use int - most cases
int userId = 12345;
int count = 1000000;

// Use long - when necessary
long timestamp = DateTime.Now.Ticks;
long population = 8000000000;  // 8 billion
long fileSize = 5000000000;    // 5GB in bytes
```

---

## Question 4: Why Use decimal for Money, Not float?

### Question
Why should you use `decimal` for financial calculations instead of `float` or `double`?

### Answer
**Because float/double have precision issues with decimal numbers.**

**Problem with float**:
```csharp
float total = 0.1f + 0.2f;
Console.WriteLine(total == 0.3f);  // false! Not exactly 0.3
```

**Correct with decimal**:
```csharp
decimal total = 0.1m + 0.2m;
Console.WriteLine(total == 0.3m);  // true! Exactly 0.3
```

**Why**:
- Float/double use binary representation
- Decimal numbers can't be represented exactly in binary
- `decimal` designed specifically for financial calculations
- Preserves precision for money

---

## Question 5: Is string a Value Type or Reference Type?

### Question
Is `string` a value type or reference type? Explain why.

### Answer
**`string` is a Reference Type** - it stores a reference to text data on the heap.

**But it has special characteristics**:
```csharp
string s1 = "Hello";
string s2 = "Hello";
Console.WriteLine(s1 == s2);  // true - value semantics

string s3 = s1;
s3 = "World";
Console.WriteLine(s1);  // "Hello" - appears immutable

// But it's still reference type
Console.WriteLine(ReferenceEquals(s1, s2));  // Could be true (interning)
```

**Key Points**:
- Reference type (on heap)
- Immutable (cannot change after creation)
- Implements value equality (== compares content, not reference)
- String interning optimization

---

## Question 6: What's the Difference Between Array and List<T>?

### Question
What's the difference between an array and `List<T>`?

### Answer
| Aspect | Array | List<T> |
|--------|-------|---------|
| **Size** | Fixed | Dynamic |
| **Type Safety** | Generic type-safe | Generic type-safe |
| **Performance** | Faster (fixed) | Slightly slower |
| **Memory** | Contiguous | Contiguous (reallocates) |
| **Add/Remove** | Not built-in | Easy |

**Example**:
```csharp
// Array - fixed size
int[] arr = new int[5];
arr[0] = 1;
// arr[10] = 5;  // IndexOutOfRangeException

// List - dynamic size
List<int> list = new();
list.Add(1);
list.Add(2);
list.Add(3);
// Automatically grows
```

**When to Use**:
- **Array**: When size is fixed and known
- **List**: Default choice for most scenarios

---

## Question 7: When Should You Use struct Instead of class?

### Question
When would you create a `struct` instead of a `class`?

### Answer
**Use `struct` only for small, immutable data.**

**Example - Good struct**:
```csharp
public readonly struct Point {
    public int X { get; }
    public int Y { get; }
    
    public Point(int x, int y) {
        X = x;
        Y = y;
    }
}
```

**Example - Bad struct** (use class instead):
```csharp
// Wrong - mutable
public struct Person {
    public string Name { get; set; }
    public List<Order> Orders { get; set; }
}
// Should be class - mutable, complex
```

**Rules**:
- ✓ Small (< 16 bytes typically)
- ✓ Immutable (doesn't change)
- ✓ No inheritance needed
- ✓ Performance critical

---

## Question 8: What Does "Immutable" Mean for Strings?

### Question
What does it mean that strings are immutable? Show an example.

### Answer
**Immutable** means once created, a string cannot be changed. Operations create new strings.

**Example**:
```csharp
string text = "Hello";
text = text + " World";  // Creates NEW string, doesn't modify original

// Each operation creates new string
string upper = text.ToUpper();
string sub = text.Substring(0, 5);

// Original text unchanged
Console.WriteLine(text);  // Still "Hello"
```

**Performance Implication**:
```csharp
// Bad - creates 1000 strings!
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;
}

// Good - uses StringBuilder
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
```

**Why Immutable**:
- Thread safety
- Caching efficiency
- Predictable behavior

---

## Question 9: What are Access Modifiers and How Many Are There?

### Question
List the access modifiers in C# and explain each one.

### Answer
| Modifier | Visible In | Use Case |
|----------|-----------|----------|
| `public` | Everywhere | API surface, external use |
| `private` | Same class | Implementation details |
| `protected` | Derived classes | Inheritance support |
| `internal` | Same assembly | Internal implementation |
| `protected internal` | Derived + assembly | Inheritance within assembly |

**Example**:
```csharp
public class BankAccount {
    // Anyone can see
    public string AccountNumber { get; set; }
    
    // Only this class
    private decimal _balance;
    
    // Derived classes only
    protected virtual void UpdateBalance(decimal amount) { }
    
    // Within assembly only
    internal void AuditLog() { }
}
```

---

## Question 10: What's the Difference Between == and Equals() for Types?

### Question
When comparing objects, when should you use `==` vs `Equals()`?

### Answer
**For Value Types**:
```csharp
int a = 5;
int b = 5;
Console.WriteLine(a == b);          // true
Console.WriteLine(a.Equals(b));     // true
// Both work the same for primitives
```

**For Reference Types (Classes)**:
```csharp
class Person {
    public string Name { get; set; }
}

Person p1 = new() { Name = "Alice" };
Person p2 = new() { Name = "Alice" };

Console.WriteLine(p1 == p2);        // false (different objects)
Console.WriteLine(p1.Equals(p2));   // false (unless overridden)
Console.WriteLine(ReferenceEquals(p1, p2));  // false
```

**For Strings** (special case):
```csharp
string s1 = "Hello";
string s2 = "Hello";
Console.WriteLine(s1 == s2);        // true (value equality)
Console.WriteLine(s1.Equals(s2));   // true (value equality)
```

**Best Practice**:
- Use `==` for comparisons
- Override `Equals()` for custom equality logic
- `string` is special - both work as value equality

---

## Question 11: What's Boxing and Unboxing?

### Question
What are boxing and unboxing in C#? Provide an example.

### Answer
**Boxing** - Convert value type to reference type (copies to heap)
**Unboxing** - Convert reference type back to value type (copies from heap)

**Example**:
```csharp
int value = 42;

// Boxing - wrap in object
object boxed = value;  // Copies 42 to heap, stores reference

// Unboxing - extract value
int unboxed = (int)boxed;  // Copies from heap
```

**Performance Impact**:
```csharp
// Bad - boxing in loop (very slow)
ArrayList list = new();
for (int i = 0; i < 1000000; i++) {
    list.Add(i);  // Boxing each iteration!
}

// Good - use generic collections
List<int> list = new();
for (int i = 0; i < 1000000; i++) {
    list.Add(i);  // No boxing
}
```

**Why Avoid**:
- Unnecessary memory allocation
- Performance penalty
- Unpredictable behavior

---

## Question 12: What's the Difference Between struct and class?

### Question
Compare and contrast struct and class.

### Answer
| Aspect | Struct | Class |
|--------|--------|-------|
| **Type** | Value | Reference |
| **Storage** | Stack | Heap |
| **Copy** | Entire value | Reference only |
| **Default** | 0/false | null |
| **GC** | Not needed | Required |
| **Inheritance** | No (except interface) | Yes |
| **Mutable** | Usually yes | Yes |
| **Use Case** | Small immutable data | Complex objects |

**Example**:
```csharp
struct Point {           // Stack-based
    public int X, Y;
}

class Person {           // Heap-based
    public string Name;
    public int Age;
}
```

---

## Summary: Easy Questions Checklist

- [ ] Understand value vs reference types
- [ ] Know default values
- [ ] Know when to use int vs long
- [ ] Know to use decimal for money
- [ ] Understand string is reference type
- [ ] Know difference between array and List
- [ ] Understand when to use struct
- [ ] Know strings are immutable
- [ ] Know access modifiers
- [ ] Understand == vs Equals
- [ ] Know boxing/unboxing concept
- [ ] Can compare struct vs class

---

**Next**: Move to [Medium Questions](../02-Medium/00-Medium-Questions.md)
