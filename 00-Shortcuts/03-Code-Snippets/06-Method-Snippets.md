# Method Snippets

Generate methods and functions with built-in snippets.

## method - Method with Return Type

**Shortcut:** `method` + Tab

**Generates:**
```csharp
public void MethodName()
{
}
```

**Placeholders:**
- void: Return type
- MethodName: Replace with method name

**Usage:**
```csharp
public class Calculator
{
    method → Tab
    // Now: public void MethodName() { }
}
```

**Examples:**
```csharp
public int Add(int a, int b)
{
    return a + b;
}

public string GetName()
{
    return "John";
}

public double CalculateArea(double radius)
{
    return Math.PI * radius * radius;
}
```

---

## void - Void Method (No Return)

**Pattern:**
```csharp
public void ProcessData()
{
    // Does something but returns nothing
}

public void PrintMessage(string message)
{
    Console.WriteLine(message);
}

public void SaveToFile(string path, string content)
{
    File.WriteAllText(path, content);
}
```

**Usage:**
```csharp
// No return value needed
ProcessData();
PrintMessage("Hello");
```

---

## Method with Parameters

**Pattern:**
```csharp
public int Multiply(int a, int b)
{
    return a * b;
}

public string Greet(string firstName, string lastName)
{
    return $"Hello, {firstName} {lastName}!";
}

public bool ValidateEmail(string email)
{
    return email.Contains("@") && email.Length > 5;
}
```

---

## Return Type - Various Types

**Returning different types:**
```csharp
public int GetCount() => 42;

public string GetName() => "Alice";

public bool IsValid() => true;

public List<int> GetNumbers() => new List<int> { 1, 2, 3 };

public Person GetPerson() => new Person { Name = "Bob" };
```

---

## static - Static Method

**Pattern:**
```csharp
public static double Square(double number)
{
    return number * number;
}

public static void PrintLine(string text)
{
    Console.WriteLine(text);
}

public static int Add(int a, int b) => a + b;
```

**Usage:**
```csharp
// Call on class, not instance
double result = MathHelper.Square(5);
StringHelper.PrintLine("Hello");
int sum = Calculator.Add(10, 20);
```

---

## private - Private Method

**Pattern:**
```csharp
public class User
{
    public void Login(string password)
    {
        if (ValidatePassword(password))
        {
            // Login logic
        }
    }
    
    private bool ValidatePassword(string password)
    {
        return !string.IsNullOrEmpty(password) && password.Length >= 8;
    }
}
```

---

## async - Async Method

**Pattern:**
```csharp
public async Task<string> FetchDataAsync()
{
    using var client = new HttpClient();
    return await client.GetStringAsync("https://api.example.com/data");
}

public async Task ProcessAsync()
{
    await Task.Delay(1000);
    Console.WriteLine("Done");
}
```

**Usage:**
```csharp
var data = await FetchDataAsync();
await ProcessAsync();
```

---

## Expression Bodied Member

**Pattern:**
```csharp
public int Add(int a, int b) => a + b;

public string GetFullName(string first, string last) 
    => $"{first} {last}";

public bool IsEven(int num) => num % 2 == 0;
```

**Shorter than block body:**
```csharp
// Instead of:
public int Add(int a, int b)
{
    return a + b;
}

// Use:
public int Add(int a, int b) => a + b;
```

---

## Method with Optional Parameters

**Pattern:**
```csharp
public string Greet(string name = "Guest")
{
    return $"Hello, {name}!";
}

public void Configure(int timeout = 30, bool debug = false)
{
    // Configuration logic
}

public List<T> GetItems<T>(int limit = 10, bool ascending = true)
{
    // Get items with defaults
}
```

**Usage:**
```csharp
Greet();              // "Hello, Guest!"
Greet("Alice");       // "Hello, Alice!"
Configure();          // timeout=30, debug=false
Configure(60, true);  // timeout=60, debug=true
```

---

## Method with Variable Arguments (params)

**Pattern:**
```csharp
public int Sum(params int[] numbers)
{
    return numbers.Sum();
}

public void PrintItems(params string[] items)
{
    foreach (var item in items)
        Console.WriteLine(item);
}
```

**Usage:**
```csharp
Sum(1, 2, 3);                              // 6
Sum(10, 20, 30, 40);                       // 100
PrintItems("A", "B", "C");
```

---

## Generic Method

**Pattern:**
```csharp
public T GetFirst<T>(List<T> items)
{
    return items.Count > 0 ? items[0] : default(T);
}

public T Parse<T>(string value) where T : struct
{
    return (T)Convert.ChangeType(value, typeof(T));
}
```

---

## Quick Reference

| Type | Purpose |
|------|---------|
| `method` | Regular method |
| `void` | No return value |
| `static` | Class method |
| `private` | Hidden from outside |
| `async` | Asynchronous |
| `=>` | Expression bodied |
| `params` | Variable arguments |
| `<T>` | Generic method |

---

## Best Practices

- Method names should be verbs: GetName(), CalculateTotal()
- Keep methods focused (single responsibility)
- Use meaningful parameter names
- Add XML comments for public methods
- Prefer expression bodied members for simple logic
- Use async for I/O operations

