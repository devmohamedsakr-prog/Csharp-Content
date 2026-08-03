# Data Type Best Practices

## Numeric Types

### 1. Choose the Right Integer Type

#### ✓ Best Practice
```csharp
// Use int by default - it's optimized
int count = 100;
int userId = 12345;
int temperature = -5;

// Use long only when necessary
long timestamp = DateTime.Now.Ticks;
long population = 8000000000;

// Use byte for small values or arrays
byte[] imageData = File.ReadAllBytes("image.jpg");
byte percentage = 100;
```

#### ❌ Avoid
```csharp
// Don't use long unnecessarily
long count = 100;  // Should be int

// Don't use different types inconsistently
int a = 10;
long b = 20;  // Mix of types
```

### 2. Use Decimal for Money, Not Float

#### ✓ Best Practice
```csharp
// Always use decimal for financial calculations
decimal price = 99.99m;
decimal tax = 19.99m;
decimal total = price + tax;

// Decimal preserves precision
decimal exactAmount = 0.1m + 0.2m;  // Exactly 0.3
```

#### ❌ Avoid
```csharp
// Never use float or double for money
float price = 99.99f;  // Wrong - precision issues
double tax = 19.99;    // Wrong - rounding errors

// This will give wrong result
double total = 0.1 + 0.2;  // Not exactly 0.3!
```

### 3. Handle Integer Division Carefully

#### ✓ Best Practice
```csharp
// Convert to decimal/double for decimal result
int total = 10;
int count = 3;
decimal average = (decimal)total / count;  // 3.333...

// Or use double
double result = (double)10 / 3;  // 3.333...
```

#### ❌ Avoid
```csharp
// Integer division loses decimal part
int average = 10 / 3;  // Result is 3, not 3.333

// This is wrong for calculations
decimal result = 10 / 3;  // Still 3 (both operands int)
```

## String Type

### 1. Use String Interpolation

#### ✓ Best Practice
```csharp
string name = "Alice";
int age = 30;

// String interpolation - readable
string message = $"Hello {name}, you are {age} years old";

// With formatting
decimal price = 99.99m;
string display = $"Price: {price:C2}";  // $99.99
```

#### ❌ Avoid
```csharp
// Concatenation - less readable
string message = "Hello " + name + ", you are " + age + " years old";

// String.Format - verbose
string message = string.Format("Hello {0}, you are {1} years old", name, age);
```

### 2. Use StringBuilder for Many Concatenations

#### ✓ Best Practice
```csharp
// StringBuilder for loops
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.AppendLine($"Line {i}");
}
string result = sb.ToString();
```

#### ❌ Avoid
```csharp
// String concatenation in loops - very slow
string result = "";
for (int i = 0; i < 1000; i++) {
    result += $"Line {i}\n";  // Creates 1000 new strings!
}
```

### 3. Check for Null/Empty Properly

#### ✓ Best Practice
```csharp
string text = GetText();

// Check for null/empty
if (string.IsNullOrEmpty(text)) {
    return;
}

// Check for whitespace
if (string.IsNullOrWhiteSpace(text)) {
    return;
}

// Null-conditional operator
string display = text?.Trim() ?? "N/A";
```

#### ❌ Avoid
```csharp
// Don't compare to empty string
if (text == "") { }  // Don't do this

// Don't forget null check
int length = text.Length;  // Could throw NullReferenceException!

// Don't check length for null
if (text == null || text.Length == 0) { }  // Redundant check
```

### 4. Use Verbatim Strings for Paths

#### ✓ Best Practice
```csharp
// Verbatim strings for file paths
string path = @"C:\Users\Documents\file.txt";

// Multiline strings
string json = @"{
    ""name"": ""John"",
    ""age"": 30
}";

// Regex patterns
string pattern = @"^\d{3}-\d{3}-\d{4}$";  // Phone pattern
```

#### ❌ Avoid
```csharp
// Don't escape backslashes
string path = "C:\\Users\\Documents\\file.txt";

// Don't use string concatenation for multiline
string json = "{" +
    "\"name\": \"John\"," +
    "\"age\": 30" +
    "}";
```

## Collections

### 1. Use Specific Collections

#### ✓ Best Practice
```csharp
// List<T> for ordered collection
List<string> names = new() { "Alice", "Bob" };

// Dictionary<K,V> for key-value lookup
Dictionary<int, string> users = new() { { 1, "Alice" } };

// HashSet<T> for unique items
HashSet<int> unique = new(new[] { 1, 2, 2, 3, 3 });  // { 1, 2, 3 }

// Queue<T> for FIFO
Queue<string> tasks = new();
tasks.Enqueue("task1");
string task = tasks.Dequeue();
```

#### ❌ Avoid
```csharp
// Don't use ArrayList - not type-safe
ArrayList list = new();  // Runtime type checking!

// Don't use non-generic collections
Hashtable table = new();  // Boxing/unboxing overhead

// Wrong collection for use case
List<int> numbers = new();  // Should be HashSet if checking membership
```

### 2. Initialize Collections Properly

#### ✓ Best Practice
```csharp
// Collection initializer
var list = new List<int> { 1, 2, 3 };

// Target-typed new (C# 9+)
List<string> names = new() { "Alice", "Bob" };

// Dictionary initializer
var dict = new Dictionary<string, int> {
    { "Alice", 30 },
    { "Bob", 25 }
};
```

#### ❌ Avoid
```csharp
// Manual adds
List<int> list = new();
list.Add(1);
list.Add(2);
list.Add(3);

// Old syntax
Dictionary<string, int> dict = new Dictionary<string, int>();
dict.Add("Alice", 30);
dict.Add("Bob", 25);
```

### 3. Handle Collection Mutations Safely

#### ✓ Best Practice
```csharp
// Collect what to remove first
var toRemove = list.Where(x => x < 0).ToList();
foreach (var item in toRemove) {
    list.Remove(item);
}

// Or use RemoveAll
list.RemoveAll(x => x < 0);

// For read-only return
public IReadOnlyList<Item> GetItems() {
    return _items.AsReadOnly();
}
```

#### ❌ Avoid
```csharp
// Don't modify during iteration
foreach (var item in list) {
    if (item < 0) {
        list.Remove(item);  // InvalidOperationException!
    }
}

// Don't expose internal collections
public List<Item> Items => _items;  // Can be modified externally!
```

## Classes vs Structs

### 1. Use Classes by Default

#### ✓ Best Practice
```csharp
// Use class for most domain objects
public class Customer {
    public string Name { get; set; }
    public List<Order> Orders { get; set; }
}

// Use class for services and behavior
public class CustomerService {
    public void ProcessCustomer(Customer customer) { }
}
```

#### ❌ Avoid
```csharp
// Don't use struct for complex objects
public struct LargeObject {
    public string name;
    public List<string> items;  // Doesn't make sense
}

// Don't use struct for mutable domain objects
public struct Person {
    public string Name { get; set; }  // Can cause issues
}
```

### 2. Use Structs Sparingly and for Immutability

#### ✓ Best Practice
```csharp
// Struct for small, immutable data
public readonly struct Point {
    public int X { get; }
    public int Y { get; }
    
    public Point(int x, int y) {
        X = x;
        Y = y;
    }
}

// Struct for grouping small related values
public readonly struct Color {
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }
    
    public Color(byte r, byte g, byte b) {
        R = r;
        G = g;
        B = b;
    }
}
```

#### ❌ Avoid
```csharp
// Don't use mutable structs
public struct MutablePoint {
    public int X { get; set; }  // Can cause bugs
    public int Y { get; set; }
}

// Don't use large structs
public struct LargeData {
    public byte[] buffer;  // Large, copying expensive
    public string text;
}
```

## Nullable Types

### 1. Use Nullable Reference Types (C# 8+)

#### ✓ Best Practice
```csharp
#nullable enable

public class Customer {
    public string Name { get; set; }  // Non-null
    public string? MiddleName { get; set; }  // Nullable
    
    public void ProcessCustomer() {
        string nm = Name;  // Required
        string? mid = MiddleName;  // Can be null
        
        // Must check before using
        if (MiddleName is not null) {
            Console.WriteLine(MiddleName.Length);
        }
    }
}
```

#### ❌ Avoid
```csharp
// Don't assume non-null strings
public class Customer {
    public string Name { get; set; }  // Could be null!
    public string MiddleName { get; set; }  // Could be null!
}

// Don't use nullable carelessly
public void Process(string? text) {
    Console.WriteLine(text.Length);  // Could crash!
}
```

### 2. Use Nullable Value Types Appropriately

#### ✓ Best Practice
```csharp
// Nullable for optional values
int? age = GetAge();  // Might not have age

// Check before using
if (age.HasValue) {
    Console.WriteLine($"Age: {age.Value}");
}

// Null coalescing
int displayAge = age ?? 0;  // 0 if null
```

#### ❌ Avoid
```csharp
// Don't use nullable for default values
int? count = null;  // Should use int with 0 default

// Don't forget to check HasValue
int value = age.Value;  // Could throw!
```

## Type Conversion

### 1. Use Appropriate Casting

#### ✓ Best Practice
```csharp
// Explicit cast when needed
object obj = 42;
if (obj is int intValue) {
    Console.WriteLine(intValue);
}

// Safe casting
int? result = obj as int?;
if (result.HasValue) {
    Console.WriteLine(result.Value);
}

// Parse with TryParse
if (int.TryParse("42", out int number)) {
    Console.WriteLine(number);
}
```

#### ❌ Avoid
```csharp
// Don't use unsafe casts
object obj = "string";
int num = (int)obj;  // InvalidCastException!

// Don't assume type conversions succeed
int number = int.Parse(userInput);  // Could throw!

// Don't mix types in operations
int a = 10;
double b = 20.5;
// Better to be explicit
```

## Enums for Named Values

### ✓ Best Practice
```csharp
public enum OrderStatus {
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

public class Order {
    public OrderStatus Status { get; set; }
}

var order = new Order { Status = OrderStatus.Processing };
```

### ❌ Avoid
```csharp
// Don't use magic numbers
public class Order {
    public int Status { get; set; }  // What do 0,1,2,3 mean?
}

// Don't use strings for status
public class Order {
    public string Status { get; set; }  // "Processing" vs "processing"?
}
```

## Memory Considerations

### 1. Be Aware of Struct Copying

```csharp
// Be careful with struct method calls
public struct Person {
    public string Name { get; set; }
    
    public void UpdateName(string newName) {
        Name = newName;  // Updates local copy, not original!
    }
}

var person = new Person { Name = "Alice" };
person.UpdateName("Bob");
// person.Name is still "Alice"! Struct was copied.
```

### 2. Avoid Boxing Value Types

```csharp
// ❌ Avoid boxing
ArrayList list = new();
list.Add(42);  // Boxing

object boxed = 42;  // Boxing
int unboxed = (int)boxed;  // Unboxing

// ✓ Use generics
List<int> list = new();
list.Add(42);  // No boxing
```

## Summary Checklist

- [ ] Use `int` for most integers, `long` only when needed
- [ ] Use `decimal` for money, never `float`/`double`
- [ ] Use string interpolation `$"..."` not concatenation
- [ ] Use `StringBuilder` for many string operations
- [ ] Check for null with `IsNullOrEmpty` or `?.` operator
- [ ] Use specific collection types (List, Dictionary, HashSet)
- [ ] Use classes by default, structs for small immutable data
- [ ] Use enums for named values, not magic numbers/strings
- [ ] Use nullable reference types (C# 8+)
- [ ] Avoid boxing value types
- [ ] Use `TryParse` instead of `Parse` for conversions
- [ ] Return `IReadOnlyList`/`IReadOnlyDictionary` from public properties
- [ ] Don't modify collections during iteration
- [ ] Use `const` for compile-time constants
- [ ] Use `readonly` for runtime constants

---

**Key Takeaway**: Choose the right type for the job, use built-in patterns (StringBuilder, TryParse, null-conditional operators), and be aware of performance implications of your choices.
