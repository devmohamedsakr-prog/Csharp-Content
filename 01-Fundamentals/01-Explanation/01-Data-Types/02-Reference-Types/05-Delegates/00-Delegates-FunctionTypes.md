# Delegates: Type-Safe Function References

## Overview

A `delegate` is a type-safe reference to a method. It defines the signature (parameters and return type) that a method must match.

### Characteristics
```csharp
public delegate void LogDelegate(string message);

// Reference type: stores reference to method
// Type-safe: method signature must match
// Can reference instance or static methods
// Foundation for events and callbacks
```

## Delegate Basics

### Defining Delegates

#### Basic Delegate Definition
```csharp
// Define delegate type
public delegate void Notify(string message);
public delegate int Calculate(int a, int b);
public delegate string Transform(string input);
```

#### Generic Delegates
```csharp
public delegate T Transform<T>(T input);
public delegate TResult Calculator<T, TResult>(T a, T b);
```

### Using Delegates

#### Assigning Methods

```csharp
public delegate void Notify(string message);

// Method to assign
static void LogMessage(string msg) {
    Console.WriteLine($"Log: {msg}");
}

// Assign method to delegate
Notify notifier = LogMessage;

// Call through delegate
notifier("Hello");  // "Log: Hello"
```

#### Delegate with Different Methods

```csharp
public delegate int Calculate(int a, int b);

static int Add(int x, int y) => x + y;
static int Multiply(int x, int y) => x * y;

Calculate calc = Add;
Console.WriteLine(calc(5, 3));  // 8

calc = Multiply;
Console.WriteLine(calc(5, 3));  // 15
```

#### Lambda Expressions with Delegates

```csharp
public delegate int Calculator(int a, int b);

// Lambda expression
Calculator add = (x, y) => x + y;
Console.WriteLine(add(5, 3));  // 8

Calculator multiply = (x, y) => x * y;
Console.WriteLine(multiply(5, 3));  // 15

// Anonymous method (older syntax)
Calculator subtract = delegate(int x, int y) {
    return x - y;
};
Console.WriteLine(subtract(5, 3));  // 2
```

## Multicast Delegates

### Combining Delegates

```csharp
public delegate void Notify(string message);

static void LogConsole(string msg) => Console.WriteLine($"[Console] {msg}");
static void LogFile(string msg) => Console.WriteLine($"[File] {msg}");
static void LogEmail(string msg) => Console.WriteLine($"[Email] {msg}");

// Combine with +=
Notify notifier = LogConsole;
notifier += LogFile;
notifier += LogEmail;

// Call all in sequence
notifier("Error occurred");
// Output:
// [Console] Error occurred
// [File] Error occurred
// [Email] Error occurred
```

### Removing Delegates

```csharp
Notify notifier = LogConsole;
notifier += LogFile;
notifier += LogEmail;

// Remove with -=
notifier -= LogFile;

// Now only console and email notified
notifier("Warning");
```

## Built-In Generic Delegates

### Action<T> (Void Return)

```csharp
// Action<T> - performs action, no return
Action<string> log = msg => Console.WriteLine(msg);
log("Hello");

// Multiple parameters
Action<int, int> add = (x, y) => Console.WriteLine(x + y);
add(5, 3);  // 8

// No parameters
Action greet = () => Console.WriteLine("Hello!");
greet();
```

### Func<T, TResult> (Returns Value)

```csharp
// Func<T, TResult> - takes T, returns TResult
Func<int, int> square = x => x * x;
Console.WriteLine(square(5));  // 25

// Multiple parameters
Func<int, int, int> multiply = (x, y) => x * y;
Console.WriteLine(multiply(5, 3));  // 15

// Complex logic
Func<string, int> parseNumber = str => {
    if (int.TryParse(str, out int result)) {
        return result;
    }
    return 0;
};

Console.WriteLine(parseNumber("42"));  // 42
```

### Predicate<T> (Boolean Return)

```csharp
// Predicate<T> - returns bool
Predicate<int> isEven = x => x % 2 == 0;
Console.WriteLine(isEven(4));  // true

// Use with collections
int[] numbers = { 1, 2, 3, 4, 5 };
int found = Array.Find(numbers, isEven);  // 2

List<int> list = new() { 1, 2, 3, 4, 5 };
list.RemoveAll(isEven);  // Remove all even numbers
```

## Delegate Patterns

### Callback Pattern

```csharp
public delegate void Callback<T>(T result);

public class DataFetcher {
    public void FetchData(string url, Callback<string> onComplete) {
        // Simulate fetching
        string data = $"Data from {url}";
        
        // Call callback
        onComplete(data);
    }
}

// Usage
var fetcher = new DataFetcher();
fetcher.FetchData("http://example.com", data => {
    Console.WriteLine($"Received: {data}");
});
```

### Strategy Pattern with Delegates

```csharp
public class DataProcessor {
    private Func<int[], int> _strategy;
    
    public DataProcessor(Func<int[], int> strategy) {
        _strategy = strategy;
    }
    
    public int Process(int[] data) {
        return _strategy(data);
    }
}

// Different strategies
Func<int[], int> sumStrategy = arr => {
    int sum = 0;
    foreach (int item in arr) sum += item;
    return sum;
};

Func<int[], int> averageStrategy = arr => {
    return arr.Length > 0 ? sumStrategy(arr) / arr.Length : 0;
};

// Use strategies
var processor1 = new DataProcessor(sumStrategy);
var processor2 = new DataProcessor(averageStrategy);

int[] data = { 10, 20, 30, 40 };
Console.WriteLine(processor1.Process(data));  // 100
Console.WriteLine(processor2.Process(data));  // 25
```

### Filter Pattern

```csharp
public class FileFilter {
    public List<string> Filter(List<string> files, Predicate<string> criteria) {
        var result = new List<string>();
        foreach (string file in files) {
            if (criteria(file)) {
                result.Add(file);
            }
        }
        return result;
    }
}

// Usage
var filter = new FileFilter();
var files = new List<string> {
    "document.txt",
    "image.jpg",
    "data.csv",
    "backup.txt"
};

// Filter by extension
var textFiles = filter.Filter(files, f => f.EndsWith(".txt"));
// Result: ["document.txt", "backup.txt"]
```

## Events (Delegates in Practice)

### Event Definition

```csharp
// Define event
public class Button {
    // Delegate type
    public delegate void ClickDelegate();
    
    // Event based on delegate
    public event ClickDelegate OnClick;
    
    public void Click() {
        // Raise event
        OnClick?.Invoke();
    }
}

// Usage
var button = new Button();

button.OnClick += () => Console.WriteLine("Button clicked!");
button.OnClick += () => Console.WriteLine("Processing click...");

button.Click();
// Output:
// Button clicked!
// Processing click...
```

### Standard Event Pattern

```csharp
public class Form {
    // Standard EventHandler pattern
    public event EventHandler OnSubmit;
    
    public void Submit() {
        OnSubmit?.Invoke(this, EventArgs.Empty);
    }
}

// Usage
var form = new Form();
form.OnSubmit += (sender, e) => Console.WriteLine("Form submitted");
form.Submit();
```

## Delegate Performance

### Invocation

```csharp
Func<int, int> square = x => x * x;

// Direct invocation
for (int i = 0; i < 1000000; i++) {
    square(i);  // Slightly slower than direct method call
}

// Method invocation (for comparison)
static int Square(int x) => x * x;
for (int i = 0; i < 1000000; i++) {
    Square(i);  // Slightly faster (no indirection)
}
```

### Delegate Allocation

```csharp
// Reuse delegate instance (better)
Action<string> log = Console.WriteLine;
foreach (var item in items) {
    log(item);
}

// Create new delegate each iteration (worse)
foreach (var item in items) {
    Action<string> log = Console.WriteLine;  // Allocation each time
    log(item);
}
```

## Comparison: Delegates vs Events

### Delegates Alone

```csharp
public class Logger {
    public Action<string> Log;
}

// Problem: Can be reassigned externally
var logger = new Logger();
logger.Log = x => Console.WriteLine(x);
logger.Log("Original");

logger.Log = x => Console.WriteLine("Different");  // Can override!
logger.Log("Lost original");
```

### Events (Recommended)

```csharp
public class Logger {
    // Event - can only += and -=, cannot reassign
    public event Action<string> OnLog;
    
    public void Log(string message) {
        OnLog?.Invoke(message);
    }
}

// Usage
var logger = new Logger();
logger.OnLog += x => Console.WriteLine(x);
logger.OnLog += x => File.AppendAllText("log.txt", x);

logger.Log("Error");
// Both handlers called
```

## Common Delegate Patterns

### Return Values

```csharp
// Delegate that returns value
Func<int, int, bool> isGreater = (a, b) => a > b;

if (isGreater(10, 5)) {
    Console.WriteLine("10 is greater");
}
```

### Null Safety

```csharp
Func<int, int> calculator = null;

// Safe invocation
int result = calculator?.Invoke(5) ?? 0;

// Or check before invoking
if (calculator != null) {
    result = calculator(5);
}
```

### Delegate Composition

```csharp
Func<int, int> addOne = x => x + 1;
Func<int, int> double_ = x => x * 2;

// Compose functions
Func<int, int> addOneThenDouble = x => double_(addOne(x));
Console.WriteLine(addOneThenDouble(5));  // (5+1)*2 = 12

// With lambdas
Func<int, int> composed = x => double_(addOne(x));
```

## Common Delegate Mistakes

❌ **Forgetting null check before Invoke**
```csharp
Action<string> handler = null;
handler?.Invoke("Message");  // Could throw NullReferenceException
```

✓ **Use null-conditional operator**
```csharp
Action<string> handler = null;
handler?.Invoke("Message");  // Safe
```

❌ **Multicast delegate with void return losing early error**
```csharp
Action handler = () => throw new Exception("Error");
handler += () => Console.WriteLine("Still runs");

handler();  // Exception throws, second still runs
```

✓ **Handle exceptions in delegates**
```csharp
Action handler = () => { };
try {
    handler?.Invoke();
} catch (Exception ex) {
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Summary

**Delegate Characteristics**:
- Type-safe reference to method
- Supports multicast (multiple methods)
- Can reference instance or static methods
- Foundation for events and callbacks

**When to Use**:
- Callbacks and asynchronous operations
- Strategy pattern (interchangeable algorithms)
- Event handling
- Filtering and transformation operations

**Best Practices**:
- Use `Func<T, TResult>` and `Action<T>` over custom delegates
- Check for null before invoking
- Use null-conditional operator `?.Invoke()`
- Prefer events over public delegates
- Use delegates for flexibility, not for simple direct calls

---

**Key Takeaway**: Delegates are type-safe function pointers that enable callbacks, strategies, and event handling. Use built-in `Action<T>` and `Func<T, TResult>` for most scenarios, and always check for null before invoking.
