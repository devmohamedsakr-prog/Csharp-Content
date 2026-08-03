# Return Types and Values

## Overview

Return types define what a method gives back to its caller. Understanding return types is essential for designing methods that work correctly.

## Void Return Type

Void means the method doesn't return a value:

```csharp
public void PrintMessage(string message)
{
    Console.WriteLine(message);
    // No return statement needed
}

public void DoSomething()
{
    // Does work but returns nothing
    Console.WriteLine("Working...");
}

// Usage
PrintMessage("Hello");
DoSomething();
```

### Void with Early Exit

Even void methods can use return to exit early:

```csharp
public void ValidateAndPrint(int number)
{
    if (number < 0)
    {
        Console.WriteLine("Negative number");
        return;  // Exit early
    }
    
    if (number == 0)
    {
        Console.WriteLine("Zero");
        return;  // Exit early
    }
    
    Console.WriteLine($"Positive: {number}");
}
```

## Primitive Return Types

### Return Integer

```csharp
public int GetCount()
{
    return 42;
}

public int Add(int a, int b)
{
    return a + b;
}

public int GetLength(string text)
{
    return text.Length;
}

// Usage
int count = GetCount();           // 42
int sum = Add(5, 3);              // 8
int len = GetLength("Hello");     // 5
```

### Return Double/Float

```csharp
public double GetPi()
{
    return 3.14159;
}

public double CalculateArea(double radius)
{
    return 3.14 * radius * radius;
}

public float GetPercentage()
{
    return 75.5f;
}

// Usage
double pi = GetPi();
double area = CalculateArea(5.0);
float percentage = GetPercentage();
```

### Return Boolean

```csharp
public bool IsPositive(int number)
{
    return number > 0;
}

public bool IsEmpty(string text)
{
    return string.IsNullOrEmpty(text);
}

public bool IsPrime(int number)
{
    if (number < 2) return false;
    for (int i = 2; i < number; i++)
    {
        if (number % i == 0)
            return false;
    }
    return true;
}

// Usage
bool positive = IsPositive(5);     // true
bool empty = IsEmpty("");          // true
bool prime = IsPrime(7);           // true
```

### Return String

```csharp
public string GetGreeting()
{
    return "Hello!";
}

public string GetFullName(string first, string last)
{
    return $"{first} {last}";
}

public string GetDayName(int day)
{
    return day switch
    {
        1 => "Monday",
        2 => "Tuesday",
        3 => "Wednesday",
        _ => "Unknown"
    };
}

// Usage
string greeting = GetGreeting();
string name = GetFullName("John", "Doe");
string day = GetDayName(1);
```

### Return Char

```csharp
public char GetFirstChar(string text)
{
    return text[0];
}

public char GetGrade(int score)
{
    return score switch
    {
        >= 90 => 'A',
        >= 80 => 'B',
        >= 70 => 'C',
        _ => 'F'
    };
}

// Usage
char first = GetFirstChar("Hello");  // 'H'
char grade = GetGrade(85);           // 'B'
```

## Reference Type Returns

### Return Array

```csharp
public int[] GetNumbers()
{
    return new int[] { 1, 2, 3, 4, 5 };
}

public string[] GetNames()
{
    return new string[] { "Alice", "Bob", "Charlie" };
}

// Usage
int[] numbers = GetNumbers();
string[] names = GetNames();
```

### Return List

```csharp
public List<int> GetIntList()
{
    return new List<int> { 10, 20, 30 };
}

public List<string> GetUsers()
{
    var users = new List<string>();
    users.Add("Alice");
    users.Add("Bob");
    return users;
}

// Usage
List<int> ints = GetIntList();
List<string> users = GetUsers();
```

### Return Dictionary

```csharp
public Dictionary<string, int> GetScores()
{
    return new Dictionary<string, int>
    {
        { "Alice", 95 },
        { "Bob", 87 },
        { "Charlie", 92 }
    };
}

// Usage
Dictionary<string, int> scores = GetScores();
```

### Return Object

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public Person CreatePerson()
{
    return new Person { Name = "Alice", Age = 30 };
}

public Person GetUser(int id)
{
    // Fetch from database
    return new Person { Name = $"User{id}", Age = 25 };
}

// Usage
Person person = CreatePerson();
Person user = GetUser(1);
```

### Return String

Already covered in primitives, but worth noting strings are reference types:

```csharp
public string BuildMessage(string greeting, string name)
{
    return $"{greeting}, {name}!";
}

// Usage
string message = BuildMessage("Hello", "World");
```

## Nullable Return Types

Methods can return nullable types:

```csharp
public int? GetScore(string name)
{
    if (name == "Alice")
        return 95;
    if (name == "Bob")
        return 87;
    return null;  // No score found
}

public string? FindUser(int id)
{
    if (id == 1)
        return "Alice";
    return null;  // Not found
}

// Usage
int? score = GetScore("Alice");
if (score.HasValue)
{
    Console.WriteLine($"Score: {score.Value}");
}

string? user = FindUser(99);
if (user != null)
{
    Console.WriteLine($"User: {user}");
}
```

## Multiple Return Paths

Methods can return from different locations:

```csharp
public string ValidateAge(int age)
{
    if (age < 0)
        return "Age cannot be negative";
    
    if (age < 18)
        return "Minor";
    
    if (age >= 65)
        return "Senior";
    
    return "Adult";
}

public int GetMaximum(int a, int b, int c)
{
    if (a >= b && a >= c)
        return a;
    
    if (b >= a && b >= c)
        return b;
    
    return c;
}

// Usage
string status = ValidateAge(25);  // "Adult"
int max = GetMaximum(5, 10, 7);   // 10
```

## Implicit vs Explicit Return

### Explicit Return

```csharp
public int Add(int a, int b)
{
    return a + b;
}

public string GetName()
{
    return "Alice";
}
```

### Expression-Bodied Members (C# 6+)

```csharp
public int Add(int a, int b) => a + b;
public string GetName() => "Alice";
public bool IsPositive(int n) => n > 0;
```

## Return Statement Rules

1. **Match the return type**
   ```csharp
   public int GetNumber()
   {
       return 42;        // Correct - int
       // return "text"; // Error - string
   }
   ```

2. **Void methods don't need return**
   ```csharp
   public void Print()
   {
       Console.WriteLine("Done");
       // No return needed
   }
   ```

3. **All code paths must return (non-void)**
   ```csharp
   public int GetStatus()
   {
       if (true)
       {
           return 1;
       }
       // Error - missing return path
   }
   ```

4. **Return exits immediately**
   ```csharp
   public int GetValue()
   {
       return 42;
       Console.WriteLine("Never prints");  // Unreachable
   }
   ```

## Common Return Patterns

### Pattern 1: Success/Failure

```csharp
public bool TryLogin(string username, string password)
{
    if (ValidateCredentials(username, password))
        return true;
    return false;
}
```

### Pattern 2: Get or Default

```csharp
public int GetValueOrDefault(Dictionary<string, int> dict, string key)
{
    if (dict.ContainsKey(key))
        return dict[key];
    return 0;  // Default
}
```

### Pattern 3: Result with Status

```csharp
public class Result
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}

public Result SaveUser(string name)
{
    if (string.IsNullOrEmpty(name))
        return new Result { Success = false, Message = "Name required" };
    
    return new Result { Success = true, Message = "Saved", Data = name };
}
```

## Summary

- **Void** = no return value
- **Primitives** = int, double, bool, char, string
- **Reference types** = arrays, lists, objects
- **Nullable types** = can return null
- **Return statement** exits method immediately
- **All paths must return** (non-void methods)

## Next Steps

- Learn [Method-Structure](../03-Method-Structure/00-Method-Structure.md) for complete method design
- Study [Parameters](../../02-Parameters-Overloading/01-Parameter-Types/00-Parameter-Types.md) for method inputs
- Review [Advanced-Parameters](../../02-Parameters-Overloading/02-Advanced-Parameters/00-Advanced-Parameters.md) for special parameters
