# Easy Interview Questions: Scope and Lifetime

## Q1: Explain the Difference Between Scope and Lifetime

### Question
What is the difference between scope and lifetime in C#? Provide an example.

### Answer

**Scope** is the region of code where a variable can be accessed. It determines visibility and accessibility at compile-time.

**Lifetime** is how long the variable exists in memory at runtime. When does it get created and destroyed?

```csharp
public class Example
{
    public void Method()
    {
        int x = 5; // SCOPE: This method only
                   // LIFETIME: From this line until method ends
        
        if (x > 0)
        {
            int y = 10; // SCOPE: This if block only
                        // LIFETIME: From this line until if block ends
            
            Console.WriteLine(y); // y is in scope here
        }
        
        // Console.WriteLine(y); // y is OUT OF SCOPE here
        // But y was in scope inside the if block
    }
}
```

### Key Points
- **Scope**: Compile-time concept - where can I use this variable?
- **Lifetime**: Runtime concept - when is this variable in memory?
- They're related but independent
- Variable can be out of scope but still in memory (e.g., event handler closure)
- Variable can be in scope but not created yet (uninitialized)

### Follow-up
"Can a variable be in scope but not in lifetime?" 
Yes - if it's declared but not initialized yet, or after it goes out of scope but is still referenced by a closure.

---

## Q2: Explain Stack vs Heap - What Goes Where?

### Question
Where are value types and reference types stored? Why does it matter?

### Answer

**Stack** stores:
- Value types (int, double, bool, struct, decimal)
- Method references and parameters
- Automatically cleaned up when out of scope
- Faster access, limited size

**Heap** stores:
- Reference type objects (class instances)
- Strings and arrays
- Cleaned up by garbage collector
- Slower access, larger size

```csharp
public class StackVsHeap
{
    public void Demonstrate()
    {
        // VALUE TYPES - on STACK
        int age = 30; // Stack
        DateTime date = DateTime.Now; // Stack
        
        // REFERENCE TYPES - reference on stack, object on heap
        var person = new Person { Name = "Alice" }; // Reference on stack, object on heap
        string name = "Bob"; // Reference on stack, string on heap
        var numbers = new int[] { 1, 2, 3 }; // Reference on stack, array on heap
    }
}
```

### Why It Matters

```csharp
// Performance: Stack allocation is faster
int x = 5; // Very fast - just push to stack

// Memory: Value types copied on assignment
int a = 5;
int b = a; // Entire value copied
b = 10;
// a still 5

// References: Only reference copied
Person p1 = new Person { Name = "Alice" };
Person p2 = p1; // Just copy reference
p2.Name = "Bob";
// p1.Name is now "Bob" too!

// Lifetime: Different cleanup mechanisms
void Method()
{
    int x = 5; // Stack - freed when method returns
    var obj = new Object(); // Object on heap - freed by GC when unreferenced
}
```

### Key Points
- Stack is faster but limited
- Heap is larger but requires GC
- Value types suitable for small data
- Reference types for complex objects
- Understanding affects performance and design

---

## Q3: What is Variable Shadowing? Identify the Bug

### Question
Identify the shadowing in this code and explain why it's problematic.

```csharp
public class MyClass
{
    private int value = 10;
    
    public void Process()
    {
        int value = 20; // What happens here?
        Console.WriteLine(value); // What prints?
    }
    
    public void Display()
    {
        Console.WriteLine(value); // What prints?
    }
}
```

### Answer

This is **variable shadowing**. The local `value` variable hides the class field `value`.

```csharp
public class MyClass
{
    private int value = 10; // Class field
    
    public void Process()
    {
        int value = 20; // Local variable SHADOWS class field
        Console.WriteLine(value); // Prints 20 (local variable)
        Console.WriteLine(this.value); // Prints 10 (class field via explicit reference)
    }
    
    public void Display()
    {
        Console.WriteLine(value); // Prints 10 (class field, no shadowing here)
    }
}
```

### Why It's Problematic
- Confusing - which `value` are we using?
- Error-prone - might use wrong one accidentally
- Maintenance nightmare - hard to debug
- Violates principle of clear code

### Solution: Use distinct names

```csharp
public class MyClass
{
    private int _classValue = 10; // Prefix with underscore
    
    public void Process()
    {
        int localValue = 20; // Clear name
        Console.WriteLine(localValue); // Obviously the local
        Console.WriteLine(_classValue); // Obviously the field
    }
}
```

### Key Points
- Shadowing is legal but bad practice
- Use different names for different scopes
- Prefix fields with underscore for clarity
- IDEs can warn about shadowing

---

## Q4: Name the Five Access Modifiers and Their Scope

### Question
List C# access modifiers and describe their visibility scope.

### Answer

```csharp
public class AccessModifiers
{
    // 1. PUBLIC - accessible everywhere
    public string PublicField = "Accessible from anywhere";
    
    // 2. PRIVATE - only within this class (default)
    private string PrivateField = "Only in this class";
    
    // 3. PROTECTED - in this class and derived classes
    protected string ProtectedField = "In this class and derived";
    
    // 4. INTERNAL - within same assembly
    internal string InternalField = "Same assembly only";
    
    // 5. PRIVATE PROTECTED - in this class and derived classes in same assembly (C# 7.2+)
    private protected string PrivateProtectedField = "Derived in same assembly";
}

public class Derived : AccessModifiers
{
    public void AccessFromDerived()
    {
        // Console.WriteLine(PrivateField); // ERROR - private
        Console.WriteLine(ProtectedField); // OK - protected
        Console.WriteLine(PublicField); // OK - public
        Console.WriteLine(PrivateProtectedField); // OK - private protected in derived
    }
}

public class Other
{
    public void AccessFromOther()
    {
        var obj = new AccessModifiers();
        Console.WriteLine(obj.PublicField); // OK
        // Console.WriteLine(obj.PrivateField); // ERROR
        // Console.WriteLine(obj.ProtectedField); // ERROR
    }
}
```

### Scope Table

| Modifier | Class | Derived | Same Asm | Diff Asm |
|----------|-------|---------|----------|----------|
| public | ✓ | ✓ | ✓ | ✓ |
| protected | ✓ | ✓ | ✓ | ✗ |
| internal | ✓ | ✓ | ✓ | ✗ |
| private protected | ✓ | ✓ | ✗ | ✗ |
| private | ✓ | ✗ | ✗ | ✗ |

### Best Practice
Start with most restrictive (private) and broaden only when needed.

---

## Q5: What is IDisposable? When Should I Implement It?

### Question
What is the IDisposable interface? When and how should you implement it?

### Answer

**IDisposable** is an interface for releasing unmanaged resources. Implement it when your class manages resources that need explicit cleanup.

```csharp
// Simple implementation
public class FileReader : IDisposable
{
    private StreamReader _reader;
    
    public FileReader(string path)
    {
        _reader = File.OpenText(path);
    }
    
    public string ReadLine()
    {
        return _reader.ReadLine();
    }
    
    // Implement Dispose to clean up
    public void Dispose()
    {
        _reader?.Dispose();
    }
}

// Usage with using statement
using var reader = new FileReader("data.txt");
string line = reader.ReadLine();
// reader.Dispose() automatically called
```

### When to Implement IDisposable

Implement when your class manages:
- File handles
- Database connections
- Network sockets
- Unmanaged memory
- Other system resources

### Don't Implement If
- Managing only other managed objects
- No unmanaged resources
- Not owning the resources

```csharp
// BAD: IDisposable not needed
public class User : IDisposable
{
    public string Name { get; set; }
    
    public void Dispose()
    {
        // Nothing to dispose - don't implement!
    }
}

// GOOD: Only when needed
public class DatabaseConnection : IDisposable
{
    private SqlConnection _conn;
    
    public DatabaseConnection(string connString)
    {
        _conn = new SqlConnection(connString);
    }
    
    public void Dispose()
    {
        _conn?.Dispose(); // Properly dispose database connection
    }
}
```

### Best Practice: Always Use Using

```csharp
// GOOD
using var connection = new DatabaseConnection("...");
connection.DoWork();
// Automatically disposed

// BAD
var connection = new DatabaseConnection("...");
connection.DoWork();
// Dispose() never called!
```

### Key Points
- IDisposable is for resource cleanup
- Always use `using` for IDisposable objects
- Don't implement unnecessarily
- Use modern `using var` syntax

---

## Summary of Easy Topics

| Topic | Key Concept |
|-------|------------|
| Scope vs Lifetime | Scope = where, Lifetime = when |
| Stack vs Heap | Stack for values, Heap for objects |
| Variable Shadowing | Same name in different scope - avoid it |
| Access Modifiers | Control visibility: public, private, protected, internal |
| IDisposable | Clean up resources - use `using` |

## Self-Check

Before moving to Medium questions, ensure you can:
- [ ] Explain scope vs lifetime clearly
- [ ] Describe stack and heap allocation
- [ ] Identify shadowing in code
- [ ] Recall all five access modifiers
- [ ] Explain when to use IDisposable
- [ ] Show proper using statement usage

If you checked all boxes, you're ready for Medium difficulty questions!
