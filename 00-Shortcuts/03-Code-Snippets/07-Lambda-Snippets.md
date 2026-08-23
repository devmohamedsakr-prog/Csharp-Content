# Lambda & Delegate Snippets

Generate lambda expressions and delegates.

## Lambda Expression - Basic

**Pattern:**
```csharp
var add = (int a, int b) => a + b;
var square = (int x) => x * x;
var isPositive = (int n) => n > 0;
```

**Usage:**
```csharp
int result = add(5, 3);        // 8
int squared = square(4);       // 16
bool check = isPositive(-5);   // false
```

---

## Lambda with LINQ

**With Where:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var evens = numbers.Where(n => n % 2 == 0);  // [2, 4]
```

**With Select:**
```csharp
var names = new List<string> { "Alice", "Bob", "Charlie" };
var lengths = names.Select(n => n.Length);   // [5, 3, 7]
```

**With FirstOrDefault:**
```csharp
var items = new List<int> { 1, 2, 3, 4, 5 };
var firstEven = items.FirstOrDefault(x => x % 2 == 0);  // 2
```

**Complex Lambda:**
```csharp
var students = new List<Student>();
var results = students
    .Where(s => s.Score >= 60)
    .Select(s => new { s.Name, s.Score })
    .OrderByDescending(s => s.Score)
    .ToList();
```

---

## Multiline Lambda

**Pattern:**
```csharp
Func<int, int> calculate = (x) =>
{
    int result = x * 2;
    result += 10;
    return result;
};

// Usage
int value = calculate(5);  // 20
```

---

## Delegate - Delegate Declaration

**Pattern:**
```csharp
public delegate void Notify(string message);

public delegate int Calculate(int a, int b);

public delegate bool Validate(object obj);
```

**Usage:**
```csharp
Notify notify = (msg) => Console.WriteLine(msg);
notify("Hello");

Calculate calc = (a, b) => a + b;
int sum = calc(10, 20);  // 30
```

---

## Action - Void Delegate

**Pattern:**
```csharp
Action<string> print = (msg) => Console.WriteLine(msg);
print("Hello");

Action<int> increment = (x) => Console.WriteLine(x + 1);
increment(5);  // 6

Action greet = () => Console.WriteLine("Hi!");
greet();  // Hi!
```

**Multiple Parameters:**
```csharp
Action<string, int> logWithLevel = (msg, level) =>
{
    Console.WriteLine($"[{level}] {msg}");
};

logWithLevel("Error occurred", 1);
```

---

## Func - Return Value Delegate

**Pattern:**
```csharp
Func<int, int> square = (x) => x * x;
int result = square(5);  // 25

Func<int, int, int> add = (a, b) => a + b;
int sum = add(10, 20);   // 30

Func<string, bool> isEmpty = (s) => string.IsNullOrEmpty(s);
```

**Return Complex Types:**
```csharp
Func<string, string> toUpper = (s) => s.ToUpper();
string result = toUpper("hello");  // HELLO

Func<int, List<int>> getMultiples = (n) =>
    Enumerable.Range(1, 10)
        .Select(x => x * n)
        .ToList();

var multiples = getMultiples(5);  // [5, 10, 15, ...]
```

---

## Predicate - Boolean Delegate

**Pattern:**
```csharp
Predicate<int> isEven = (x) => x % 2 == 0;
bool check = isEven(4);  // true

Predicate<string> isValid = (s) => !string.IsNullOrEmpty(s);
bool valid = isValid("hello");  // true
```

**With Collections:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var evens = numbers.FindAll(n => n % 2 == 0);  // [2, 4]
```

---

## Closure - Lambda Capturing Variables

**Pattern:**
```csharp
int factor = 5;
Func<int, int> multiply = (x) => x * factor;
Console.WriteLine(multiply(10));  // 50

factor = 10;
Console.WriteLine(multiply(10));  // 100 (factor changed)
```

**Common Use:**
```csharp
var funcs = new List<Func<int>>();
for (int i = 1; i <= 3; i++)
{
    int copy = i;  // Avoid closure trap
    funcs.Add(() => copy * 2);
}

foreach (var func in funcs)
    Console.WriteLine(func());  // 2, 4, 6
```

---

## Event Handler - Lambda for Events

**Pattern:**
```csharp
button.Click += (sender, e) =>
{
    MessageBox.Show("Button clicked!");
};

textBox.TextChanged += (sender, e) =>
{
    Console.WriteLine($"Text changed to: {textBox.Text}");
};
```

---

## Anonymous Function vs Lambda

**Anonymous Function (older):**
```csharp
Func<int, int> square = delegate (int x)
{
    return x * x;
};
```

**Lambda (preferred):**
```csharp
Func<int, int> square = (x) => x * x;
```

---

## Expression Tree (Advanced)

**Pattern:**
```csharp
Expression<Func<int, bool>> isPositive = x => x > 0;
// Can be compiled and used dynamically
Func<int, bool> compiled = isPositive.Compile();
bool result = compiled(5);  // true
```

---

## Quick Reference

| Type | Purpose | Syntax |
|------|---------|--------|
| Lambda | Inline function | `(x) => x * 2` |
| Action | Void delegate | `Action<T> action = (x) => { }` |
| Func | Return value | `Func<T, R> func = (x) => result` |
| Predicate | Boolean | `Predicate<T> pred = (x) => x > 0` |
| Delegate | Custom type | `public delegate void MyDelegate(string s)` |

---

## Best Practices

- Use lambda for simple operations
- Use named methods for complex logic
- Avoid deep nesting of lambdas
- Be careful with closures capturing variables
- Prefer Func/Action over custom delegates
- Use expression bodied members for clarity

