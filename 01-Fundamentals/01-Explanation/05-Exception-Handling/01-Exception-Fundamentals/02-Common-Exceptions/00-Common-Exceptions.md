# Common Exceptions in C#

## Overview
C# provides a rich set of built-in exceptions for various error conditions. Understanding common exceptions helps write better error handling code.

## Input/Parsing Exceptions

### FormatException
Thrown when parsing fails due to invalid format.

```csharp
// Invalid format for int
int num = int.Parse("abc");
// System.FormatException: Input string was not in a correct format

// Null or whitespace
int num = int.Parse(null);  // FormatException

// DateTime parsing
DateTime date = DateTime.Parse("invalid-date");
// System.FormatException
```

**Prevention**:
```csharp
// Use TryParse instead
if (int.TryParse(userInput, out int number)) {
    Console.WriteLine($"Valid: {number}");
} else {
    Console.WriteLine("Invalid format");
}
```

### OverflowException
Value is too large or too small for the data type.

```csharp
// Number too large
int large = int.Parse("999999999999999999");
// System.OverflowException: Value was either too large or too small for an Int32

// Checked arithmetic
checked {
    int result = int.MaxValue + 1;  // OverflowException
}
```

## Null Reference Exceptions

### NullReferenceException
Accessing member on null object.

```csharp
string text = null;
int length = text.Length;
// System.NullReferenceException: Object reference not set to an instance of an object

// Calling method on null
var list = (List<int>)null;
list.Add(5);  // NullReferenceException

// Accessing property on null
var person = (Person)null;
string name = person.Name;  // NullReferenceException
```

**Prevention**:
```csharp
// Check for null
if (text != null) {
    int length = text.Length;
}

// Null conditional operator
int? length = text?.Length;  // Returns null if text is null

// Null coalescing
int length = text?.Length ?? 0;  // 0 if text is null
```

### ArgumentNullException
Method received null argument when it shouldn't.

```csharp
public void ProcessData(string data) {
    if (data == null) {
        throw new ArgumentNullException(nameof(data));
    }
    // Process data
}

// Usage
try {
    ProcessData(null);  // Throws ArgumentNullException
} catch (ArgumentNullException ex) {
    Console.WriteLine($"Null argument: {ex.ParamName}");
}
```

## Collection/Array Exceptions

### IndexOutOfRangeException
Index is outside array bounds.

```csharp
int[] numbers = new int[5] { 1, 2, 3, 4, 5 };
int value = numbers[10];
// System.IndexOutOfRangeException: Index was outside the bounds of the array

// Negative index
int value = numbers[-1];  // IndexOutOfRangeException

// List index out of range
var list = new List<int> { 1, 2, 3 };
int item = list[5];  // IndexOutOfRangeException
```

**Prevention**:
```csharp
// Check length
if (index >= 0 && index < array.Length) {
    int value = array[index];
}

// Use TryGetValue for dictionaries
Dictionary<string, int> dict = new();
if (dict.TryGetValue("key", out int value)) {
    Console.WriteLine(value);
}

// LINQ - First returns exception, FirstOrDefault returns default
var first = list.FirstOrDefault();  // Safe
```

### ArgumentException
Invalid argument value (not null, but invalid).

```csharp
public void SetAge(int age) {
    if (age < 0 || age > 150) {
        throw new ArgumentException("Age must be between 0 and 150", nameof(age));
    }
}

// Usage
try {
    SetAge(-5);
} catch (ArgumentException ex) {
    Console.WriteLine($"{ex.ParamName}: {ex.Message}");
}
```

## Mathematical Exceptions

### DivideByZeroException
Integer division by zero.

```csharp
int result = 10 / 0;
// System.DivideByZeroException: Attempted to divide by zero

// Floating point - no exception, returns Infinity
double result = 10.0 / 0.0;  // Infinity (no exception)
```

**Prevention**:
```csharp
// Check divisor
if (divisor != 0) {
    int result = dividend / divisor;
} else {
    Console.WriteLine("Cannot divide by zero");
}
```

## Invalid Operation Exceptions

### InvalidOperationException
Operation cannot be performed in current state.

```csharp
// Empty list - no items
var list = new List<int>();
int first = list.First();
// System.InvalidOperationException: Sequence contains no elements

// Enumerate already disposed
IEnumerable<int> items = GetItems();
items.Dispose();
foreach (var item in items) {  // InvalidOperationException
    Console.WriteLine(item);
}
```

**Prevention**:
```csharp
// Check if any items exist
if (list.Count > 0) {
    int first = list[0];
}

// Or use FirstOrDefault
int first = list.FirstOrDefault();  // Returns default if empty

// Check state before operation
if (resource.IsDisposed) {
    throw new ObjectDisposedException("resource");
}
```

## File/IO Exceptions

### FileNotFoundException
File does not exist.

```csharp
var reader = new StreamReader("nonexistent.txt");
// System.IO.FileNotFoundException: Could not find file 'nonexistent.txt'
```

**Prevention**:
```csharp
if (File.Exists("file.txt")) {
    var reader = new StreamReader("file.txt");
}
```

### IOException
General I/O error.

```csharp
// File in use
File.Delete("file.txt");  // IOException if file locked

// Read-only file
File.WriteAllText("readonly.txt", "data");  // IOException
```

### DirectoryNotFoundException
Directory does not exist.

```csharp
var files = Directory.GetFiles("C:\\nonexistent");
// System.IO.DirectoryNotFoundException
```

## Type Exceptions

### InvalidCastException
Cast operation fails.

```csharp
object obj = "text";
int num = (int)obj;
// System.InvalidCastException: Unable to cast object of type 'System.String' to type 'System.Int32'

// Generic cast
var person = (Employee)employee;  // May fail
```

**Prevention**:
```csharp
// Use 'as' operator
var employee = person as Employee;
if (employee != null) {
    // Safe to use as Employee
}

// Or 'is' keyword
if (person is Employee emp) {
    // Safe to use emp
}
```

### NotSupportedException
Operation not supported on this object.

```csharp
// Array with fixed size
Array.Resize(ref fixedArray, 20);
// System.NotSupportedException
```

## Timeout Exceptions

### TimeoutException
Operation timed out.

```csharp
// Task timeout
var task = LongRunningOperation();
if (!task.Wait(TimeSpan.FromSeconds(5))) {
    // Timeout occurred
}

// Explicitly throw
throw new TimeoutException("Operation took too long");
```

## Common Exception Hierarchy

```
Exception
├── SystemException
│   ├── NullReferenceException
│   ├── IndexOutOfRangeException
│   ├── FormatException
│   ├── OverflowException
│   ├── DivideByZeroException
│   ├── InvalidOperationException
│   ├── InvalidCastException
│   └── IOException
│       ├── FileNotFoundException
│       └── DirectoryNotFoundException
│
└── ApplicationException
    └── CustomException
```

## Exception Handling Patterns

### Pattern 1: Multiple Specific Catches
```csharp
try {
    ProcessData(userInput);
} catch (FormatException ex) {
    Console.WriteLine("Invalid format");
} catch (OverflowException ex) {
    Console.WriteLine("Value too large");
} catch (InvalidOperationException ex) {
    Console.WriteLine("Invalid operation");
}
```

### Pattern 2: Selective Handling
```csharp
try {
    ProcessData();
} catch (ArgumentException) {
    // Handle argument errors
} catch (IOException) {
    // Handle file errors
} catch (Exception ex) {
    // Catch everything else
    logger.Error($"Unexpected error: {ex.Message}");
}
```

### Pattern 3: TryParse for Input
```csharp
string userInput = Console.ReadLine();
if (int.TryParse(userInput, out int number)) {
    ProcessNumber(number);
} else {
    Console.WriteLine("Please enter a valid number");
}
```

## Best Practices

✓ Catch specific exceptions
✓ Use TryParse for user input
✓ Check for null before operations
✓ Verify collection non-empty before First()
✓ Log exception details

## Summary

| Exception | Cause | Prevention |
|-----------|-------|-----------|
| FormatException | Invalid parse format | Use TryParse |
| OverflowException | Value too large/small | Check range |
| NullReferenceException | Access null object | Check for null |
| IndexOutOfRangeException | Index out of bounds | Check bounds |
| DivideByZeroException | Divide by zero | Check divisor |
| InvalidOperationException | Invalid state | Check state |
| FileNotFoundException | File doesn't exist | Check File.Exists |
| InvalidCastException | Invalid cast | Use 'as' or 'is' |

---

## Next Steps

1. Learn Exception Hierarchy
2. Master Try-Catch patterns
3. Create Custom Exceptions
4. Study Best Practices
