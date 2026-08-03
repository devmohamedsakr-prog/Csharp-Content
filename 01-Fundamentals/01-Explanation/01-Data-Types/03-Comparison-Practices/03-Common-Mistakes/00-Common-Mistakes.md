# Common Data Type Mistakes and How to Fix Them

## Numeric Type Mistakes

### Mistake 1: Using Float for Money

#### ❌ Problem
```csharp
// Wrong - precision issues!
float price = 0.1f;
float tax = 0.2f;
float total = price + tax;

Console.WriteLine(total);  // Not exactly 0.3!
// Output: 0.30000001

// Financial disaster
decimal accountBalance = 100.00m;
float spent = 99.99f;
decimal remaining = accountBalance - (decimal)spent;
// Result: not exactly 0.01
```

**Why It's Wrong**:
- Float uses binary representation
- Decimal numbers can't be represented exactly in binary
- Results in rounding errors
- Accumulates errors in calculations

#### ✓ Solution
```csharp
// Correct - use decimal for money
decimal price = 0.1m;
decimal tax = 0.2m;
decimal total = price + tax;

Console.WriteLine(total);  // Exactly 0.3

// Correct accounting
decimal accountBalance = 100.00m;
decimal spent = 99.99m;
decimal remaining = accountBalance - spent;
// Result: exactly 0.01
```

### Mistake 2: Integer Division When Expecting Decimal

#### ❌ Problem
```csharp
// Wrong - loses decimal part
int total = 10;
int count = 3;
int average = total / count;

Console.WriteLine(average);  // 3, not 3.33!

// Used in calculation
decimal result = total / count;  // Still 3! (both operands int)
```

**Why It's Wrong**:
- When both operands are integers, division is integer division
- Decimal part is truncated
- Result is unexpected

#### ✓ Solution
```csharp
// Correct - convert to decimal
int total = 10;
int count = 3;
decimal average = (decimal)total / count;

Console.WriteLine(average);  // 3.333...

// Or use double
double average2 = (double)total / count;
Console.WriteLine(average2);  // 3.333...

// Or make one operand decimal
decimal average3 = total / (decimal)count;
```

### Mistake 3: Using Wrong Integer Type

#### ❌ Problem
```csharp
// Unnecessary - int is enough
long userId = 12345;  // Should be int

// Overflow not detected
byte age = 256;  // Should be 0 (overflows)

// Performance impact
long[] largeArray = new long[1000000];  // Double the memory!
```

**Why It's Wrong**:
- Wastes memory
- Potential overflow issues
- Performance penalty

#### ✓ Solution
```csharp
// Correct - use appropriate type
int userId = 12345;  // int is default for numbers

byte age = 100;  // Good for small range

// For large arrays
int[] largeArray = new int[1000000];  // Efficient

// Use long only when necessary
long timestamp = DateTime.Now.Ticks;
long largeNumber = 999999999999;
```

## String Mistakes

### Mistake 4: String Concatenation in Loops

#### ❌ Problem
```csharp
// Very slow - creates new string each iteration
string result = "";
for (int i = 0; i < 10000; i++) {
    result += $"Item {i}\n";
    // Each += creates new string!
    // After 10000 iterations: creates 10000 strings!
}

// Time: O(n²) - quadratic time!
```

**Why It's Wrong**:
- Strings are immutable
- Each `+=` creates new string
- Old string discarded
- Memory churn and slowdown

#### ✓ Solution
```csharp
// Correct - use StringBuilder
var sb = new StringBuilder();
for (int i = 0; i < 10000; i++) {
    sb.AppendLine($"Item {i}");
    // StringBuilder appends efficiently
}
string result = sb.ToString();  // Single final string

// Time: O(n) - linear time!
```

### Mistake 5: Not Checking for Null Before Using

#### ❌ Problem
```csharp
string text = GetText();

// NullReferenceException if text is null!
int length = text.Length;
string upper = text.ToUpper();
bool contains = text.Contains("x");

// In collections
foreach (string item in items) {
    Console.WriteLine(item.Length);  // Could crash!
}
```

**Why It's Wrong**:
- Method returns null
- No null check performed
- Runtime exception at unpredictable time

#### ✓ Solution
```csharp
string text = GetText();

// Check for null first
if (!string.IsNullOrEmpty(text)) {
    int length = text.Length;
}

// Or use null-conditional operator
int? length = text?.Length;

// Or use null coalescing
string display = text ?? "N/A";

// In collections with validation
foreach (string item in items) {
    if (!string.IsNullOrEmpty(item)) {
        Console.WriteLine(item.Length);
    }
}
```

### Mistake 6: Comparing Strings Incorrectly

#### ❌ Problem
```csharp
string input = GetUserInput();  // User types "QUIT"

// Wrong - case sensitive!
if (input == "quit") {
    // Never executes if user typed "QUIT"
}

// This approach fails
if (input == "quit" || input == "Quit" || input == "QUIT") {
    // Verbose and error-prone
}
```

**Why It's Wrong**:
- User input unpredictable
- Case mismatches break logic
- Verbose multiple checks

#### ✓ Solution
```csharp
string input = GetUserInput();

// Case-insensitive comparison
if (input.Equals("quit", StringComparison.OrdinalIgnoreCase)) {
    // Works for "quit", "QUIT", "Quit", etc.
}

// Or convert to consistent case
if (input.ToLower() == "quit") {
    // Also works
}
```

## Collection Mistakes

### Mistake 7: Modifying Collection During Iteration

#### ❌ Problem
```csharp
List<int> numbers = new() { 1, 2, 3, 4, 5 };

foreach (int num in numbers) {
    if (num % 2 == 0) {
        numbers.Remove(num);  // InvalidOperationException!
    }
}
```

**Why It's Wrong**:
- Iterator state becomes invalid
- Collection structure changed during iteration
- Runtime exception

#### ✓ Solution
```csharp
// Option 1: Collect items to remove first
var toRemove = new List<int>();
foreach (int num in numbers) {
    if (num % 2 == 0) {
        toRemove.Add(num);
    }
}
foreach (int num in toRemove) {
    numbers.Remove(num);
}

// Option 2: Use RemoveAll
numbers.RemoveAll(num => num % 2 == 0);

// Option 3: Use Where for new collection
var odd = numbers.Where(num => num % 2 != 0).ToList();
```

### Mistake 8: Exposing Internal Collection

#### ❌ Problem
```csharp
public class Team {
    public List<string> Members { get; set; }
}

// External code can modify internal list!
var team = new Team { Members = new() { "Alice", "Bob" } };
team.Members.Add("Eve");  // Unwanted modification
team.Members.Clear();     // Catastrophic!
```

**Why It's Wrong**:
- Breaks encapsulation
- External code modifies internal state
- No validation possible

#### ✓ Solution
```csharp
public class Team {
    private readonly List<string> _members = new();
    
    // Return read-only collection
    public IReadOnlyList<string> Members => _members.AsReadOnly();
    
    // Provide controlled access
    public void AddMember(string name) {
        if (!string.IsNullOrWhiteSpace(name)) {
            _members.Add(name);
        }
    }
}

// Usage
var team = new Team();
team.AddMember("Alice");

// Can't modify from outside
// team.Members.Add("Eve");  // Won't compile!
```

### Mistake 9: Using Wrong Collection Type

#### ❌ Problem
```csharp
// Wrong - checking membership in List is O(n)
List<int> allowed = new() { 1, 2, 3, 4, 5 };
for (int i = 0; i < 1000000; i++) {
    if (allowed.Contains(i)) {  // Slow - checks entire list
        // Process
    }
}

// Wrong - LinkedList for indexed access
LinkedList<string> items = new();
for (int i = 0; i < items.Count; i++) {
    // Can't use indexed access efficiently
}
```

**Why It's Wrong**:
- Inefficient operations
- Performance degrades with size
- Wrong tool for the job

#### ✓ Solution
```csharp
// Correct - use HashSet for membership O(1)
HashSet<int> allowed = new() { 1, 2, 3, 4, 5 };
for (int i = 0; i < 1000000; i++) {
    if (allowed.Contains(i)) {  // Fast - O(1) lookup
        // Process
    }
}

// Correct - use List for indexed access
List<string> items = new();
for (int i = 0; i < items.Count; i++) {
    string item = items[i];  // O(1) indexed access
}
```

## Class vs Struct Mistakes

### Mistake 10: Using Mutable Struct

#### ❌ Problem
```csharp
public struct Person {
    public string Name { get; set; }
}

// Unexpected behavior
List<Person> list = new();
list.Add(new Person { Name = "Alice" });

Person p = list[0];
p.Name = "Bob";  // Only changes local copy!

Console.WriteLine(list[0].Name);  // Still "Alice"!
```

**Why It's Wrong**:
- Struct is value type - copied on assignment
- Modifications don't affect original
- Confusing and error-prone

#### ✓ Solution
```csharp
// Option 1: Use class
public class Person {
    public string Name { get; set; }
}

// Option 2: Use immutable struct
public readonly struct PersonStruct {
    public string Name { get; }
    
    public PersonStruct(string name) {
        Name = name;
    }
}

// Now modifications work as expected
List<Person> list = new();
list.Add(new Person { Name = "Alice" });
Person p = list[0];
p.Name = "Bob";
// (Changes on reference type, original updated)
```

### Mistake 11: Large Mutable Struct

#### ❌ Problem
```csharp
public struct LargeData {
    public byte[] buffer;      // Large array
    public string description; // String
    public DateTime created;   // Datetime
}

// Expensive copy!
LargeData data = new();
LargeData copy = data;  // Copies entire struct including array

// Confusing behavior
void ProcessData(LargeData d) {
    // 'data' variable in caller unchanged
}
```

**Why It's Wrong**:
- Large struct copy overhead
- Memory waste
- Confusing semantics

#### ✓ Solution
```csharp
// Use class for large/complex data
public class LargeData {
    public byte[] Buffer { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
}

// Reference copy only
LargeData data = new();
LargeData copy = data;  // Only reference copied

// Behavior clear
void ProcessData(LargeData d) {
    // 'd' is reference to original
}
```

## Type Conversion Mistakes

### Mistake 12: Unsafe Type Casting

#### ❌ Problem
```csharp
object obj = "string";

// InvalidCastException!
int num = (int)obj;

// Could be null
string text = (string)obj;
int length = text.Length;  // NullReferenceException

// In loop - multiple exceptions
foreach (object item in items) {
    int num = (int)item;  // Crashes if item not int
}
```

**Why It's Wrong**:
- Runtime exceptions
- Type mismatches not detected until execution
- Breaks program flow

#### ✓ Solution
```csharp
object obj = "string";

// Safe type checking
if (obj is int intValue) {
    // Use intValue safely
}

// Safe casting with as
int? num = obj as int?;
if (num.HasValue) {
    // Use num.Value
}

// Pattern matching (C# 7+)
if (obj is string text) {
    // Use text safely
}

// For parsing
if (int.TryParse("42", out int num)) {
    // Use num
}
```

### Mistake 13: Forgetting Null Coalescing

#### ❌ Problem
```csharp
string result = GetValue();  // Might be null

// Wrong - could crash
int length = result.Length;

// Wrong - displays "null" or crashes
string display = result.ToUpper();
```

**Why It's Wrong**:
- Assumes non-null
- NullReferenceException at runtime
- Poor user experience

#### ✓ Solution
```csharp
string result = GetValue();

// Use null coalescing
string display = result ?? "N/A";

// Use null-conditional
int length = result?.Length ?? 0;

// Check first
if (!string.IsNullOrEmpty(result)) {
    string display = result.ToUpper();
}
```

## Defensive Programming

### General Pattern: Validate Everything

#### ✓ Best Practice
```csharp
public decimal CalculateDiscount(decimal price, int discountPercent) {
    // Validate inputs
    if (price < 0) throw new ArgumentException("Price cannot be negative");
    if (discountPercent < 0 || discountPercent > 100) 
        throw new ArgumentException("Discount must be 0-100");
    
    // Calculate safely
    decimal discount = price * (discountPercent / 100m);
    return price - discount;
}

public void ProcessItems(List<string> items) {
    // Check for null
    if (items == null) throw new ArgumentNullException(nameof(items));
    
    // Iterate safely
    foreach (string item in items) {
        if (!string.IsNullOrEmpty(item)) {
            // Process
        }
    }
}
```

## Summary of Common Mistakes

| Mistake | Issue | Solution |
|---------|-------|----------|
| Float for money | Precision loss | Use decimal |
| Integer division | Loses decimals | Cast to decimal |
| String concat loop | O(n²) time | Use StringBuilder |
| No null check | NullReferenceException | Check before use |
| Modify during iterate | InvalidOperationException | Collect removals first |
| Expose collection | Breaks encapsulation | Return IReadOnlyList |
| Mutable struct | Unexpected behavior | Use class or readonly struct |
| Unsafe cast | InvalidCastException | Use `is` or `as` |
| Large struct | Memory waste | Use class |
| Wrong collection | Performance issue | Choose appropriate type |

---

**Key Takeaway**: Validate inputs, check for null, use appropriate types for collections, and be defensive about type conversions. Most runtime errors can be prevented with proper type selection and validation.
