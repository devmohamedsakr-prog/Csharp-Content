# Advanced Features - Interview Questions & Answers

## 1. What are generics and why are they useful?

**Answer:**

Generics allow you to write type-safe code that works with different types without casting.

```csharp
// Without generics (unsafe)
ArrayList list = new ArrayList();
list.Add(5);
int num = (int)list[0];  // Casting required, type-unsafe

// With generics (type-safe)
List<int> list = new List<int>();
list.Add(5);
int num = list[0];  // No casting needed
```

**Benefits**:
- Type safety at compile time
- No performance overhead from boxing/unboxing
- Code reusability
- Better IntelliSense and documentation

```csharp
// Generic method
public T GetFirst<T>(List<T> items) {
    return items[0];
}

// Generic class
public class Repository<T> {
    private List<T> items = new List<T>();
    
    public void Add(T item) { items.Add(item); }
    public T GetById(int id) { return items[id]; }
}
```

---

## 2. What are generic constraints?

**Answer:**

Constraints limit which types can be used with a generic.

```csharp
// where T : class - T must be reference type
public void ProcessClass<T>(T item) where T : class { }

// where T : struct - T must be value type
public void ProcessStruct<T>(T item) where T : struct { }

// where T : new() - T must have parameterless constructor
public T CreateInstance<T>() where T : new() {
    return new T();
}

// where T : BaseClass - T must inherit from BaseClass
public class Repository<T> where T : Entity {
    public void Save(T item) { }
}

// Multiple constraints
public void Process<T>(T item) 
    where T : class, IEntity, new() { }
```

---

## 3. What are delegates and what problem do they solve?

**Answer:**

Delegates are type-safe function pointers or callbacks.

```csharp
// Define a delegate type
public delegate void MyDelegate(string message);

// Use delegate
public class EventPublisher {
    public MyDelegate OnEventRaised;
    
    public void RaiseEvent(string msg) {
        OnEventRaised?.Invoke(msg);
    }
}

// Subscribe
EventPublisher publisher = new EventPublisher();
publisher.OnEventRaised += (msg) => Console.WriteLine($"Received: {msg}");
publisher.RaiseEvent("Hello");  // Output: Received: Hello
```

**Common Delegates**:
```csharp
// Predefined delegates
Func<int, int, int> Add = (a, b) => a + b;
Action<string> Print = (msg) => Console.WriteLine(msg);
Predicate<int> IsEven = (n) => n % 2 == 0;
```

---

## 4. What are events and how do they differ from delegates?

**Answer:**

Events are a wrapper around delegates that provide encapsulation.

```csharp
// Delegate alone (unsafe - anyone can reassign)
public delegate void NotifyDelegate(string message);
public NotifyDelegate OnNotify;  // Can be reassigned!

// Event (safe - only can += and -=)
public event EventHandler OnNotify;  // Only +=, -=, null check
```

**Key Difference**:
```csharp
// Delegates - unrestricted
MyDelegate del = () => Console.WriteLine("1");
del = () => Console.WriteLine("2");  // Completely replaced!

// Events - controlled
public event EventHandler MyEvent;
// Can only do: MyEvent += handler; MyEvent -= handler;
// Cannot do: MyEvent = handler; (outside the class)
```

---

## 5. What are lambda expressions?

**Answer:**

Anonymous functions with concise syntax.

```csharp
// Traditional delegate
Func<int, int, int> Add = delegate(int a, int b) {
    return a + b;
};

// Lambda expression
Func<int, int, int> Add = (a, b) => a + b;

// Lambda with multiple statements
Func<int, string> Describe = (age) => {
    if (age < 18) return "Minor";
    return "Adult";
};

// Lambda with collections
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
var evens = numbers.Where(n => n % 2 == 0);  // [2, 4]
```

---

## 6. What is the difference between Action, Func, and Predicate?

**Answer:**

| Delegate | Returns | Use Case |
|----------|---------|----------|
| Action<T> | void | Perform action |
| Func<T, R> | R | Transform/compute |
| Predicate<T> | bool | Test/filter |

```csharp
// Action - no return
Action<int> Print = (n) => Console.WriteLine(n);
Print(5);  // Output: 5

// Func - returns value
Func<int, int, int> Add = (a, b) => a + b;
int result = Add(5, 3);  // 8

// Predicate - returns bool
Predicate<int> IsPositive = (n) => n > 0;
bool check = IsPositive(5);  // true
```

---

## 7. What is reflection and what are common use cases?

**Answer:**

Reflection allows inspecting and manipulating types at runtime.

```csharp
// Get type information
Type type = typeof(Person);
PropertyInfo[] properties = type.GetProperties();
MethodInfo[] methods = type.GetMethods();

// Create instance dynamically
object instance = Activator.CreateInstance(type);

// Get property value
PropertyInfo prop = type.GetProperty("Name");
string name = (string)prop.GetValue(instance);

// Invoke method dynamically
MethodInfo method = type.GetMethod("Display");
method.Invoke(instance, null);

// Common use cases
// - Serialization/Deserialization
// - Dependency Injection
// - ORM frameworks
// - Unit testing
```

**Performance Note**: Reflection is slow, avoid in tight loops.

---

## 8. What are attributes and how are they used?

**Answer:**

Metadata attached to code elements for runtime inspection.

```csharp
// Custom attribute
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorAttribute : Attribute {
    public string Name { get; set; }
    public AuthorAttribute(string name) { Name = name; }
}

// Using attribute
[Author("John Doe")]
public class MyClass {
    [Obsolete("Use NewMethod instead")]
    public void OldMethod() { }
}

// Reading attributes
Type type = typeof(MyClass);
var authorAttr = type.GetCustomAttribute<AuthorAttribute>();
Console.WriteLine(authorAttr.Name);  // John Doe

// Built-in attributes
[Serializable]
[Obsolete]
[Conditional("DEBUG")]
```

---

## 9. What are extension methods?

**Answer:**

Methods added to existing types without modifying the original class.

```csharp
// Extend string class
public static class StringExtensions {
    public static int WordCount(this string str) {
        return str.Split(' ').Length;
    }
    
    public static string Reverse(this string str) {
        return new string(str.Reverse().ToArray());
    }
}

// Use extension method
string text = "Hello World";
int count = text.WordCount();  // 2
string reversed = text.Reverse();  // dlroW olleH

// LINQ uses extension methods
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
var evens = numbers.Where(n => n % 2 == 0);  // Extension method
```

**Constraints**:
- Must be in static class
- First parameter prefixed with `this`
- Cannot access private members
- Cannot override instance methods

---

## 10. What are nullable types and the null-coalescing operator?

**Answer:**

```csharp
// Nullable types
int? nullableInt = null;
int? x = 5;

// Check for null
if (nullableInt.HasValue) {
    Console.WriteLine(nullableInt.Value);
}

// Null coalescing operator (??)
int result = nullableInt ?? 10;  // 10 (if null)
int result = x ?? 10;             // 5 (if not null)

// Null conditional operator (?.)
string name = person?.Name;  // null if person is null

// Null coalescing assignment (??=)
nullableInt ??= 10;  // Assigns 10 only if null
```

---

## 11. What are records and how do they differ from classes?

**Answer:**

Records are immutable reference types optimized for data.

```csharp
// Traditional class
class Person {
    public string Name { get; set; }
    public int Age { get; set; }
}

// Record
record Person(string Name, int Age);

// Key differences
var p1 = new Person("John", 30);
var p2 = new Person("John", 30);

// Value equality (records)
p1 == p2;  // true

// Reference equality (classes)
p1 == p2;  // false (different instances)

// Immutability
record ImmutablePerson {
    public string Name { get; init; }
    public int Age { get; init; }
}

var person = new ImmutablePerson { Name = "John", Age = 30 };
// person.Name = "Jane";  // Error - can't modify init property
```

---

## 12. What is the difference between value tuples and Tuple class?

**Answer:**

```csharp
// Tuple class (older)
Tuple<int, string> tuple1 = new Tuple<int, string>(1, "John");
int id = tuple1.Item1;
string name = tuple1.Item2;

// Value Tuple (modern, recommended)
var tuple2 = (id: 1, name: "John");
int id = tuple2.id;
string name = tuple2.name;

// Returning multiple values
public (int status, string message) GetResult() {
    return (200, "Success");
}

var (code, msg) = GetResult();  // Deconstruction
```

**Advantages of ValueTuple**:
- Lighter weight
- Named fields
- Better performance
- Supports deconstruction

---

## Quick Tips for Interview

✓ Understand generics and constraints
✓ Know difference between delegates and events
✓ Explain lambda expressions with examples
✓ Know Action vs Func vs Predicate
✓ Understand reflection limitations and use cases
✓ Explain attributes and extension methods
✓ Know nullable types and null operators
✓ Understand records vs classes
