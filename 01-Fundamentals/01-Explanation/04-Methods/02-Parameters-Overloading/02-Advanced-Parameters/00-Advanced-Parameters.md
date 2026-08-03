# Advanced Parameters

## Overview

Advanced parameter techniques enable more sophisticated method design patterns.

## Ref Parameters

Pass by reference - method can modify original variable:

```csharp
public void Increment(ref int value)
{
    value++;
}

int x = 5;
Increment(ref x);
Console.WriteLine(x);  // 6 - changed!
```

### Ref with Multiple Parameters

```csharp
public void Swap(ref int a, ref int b)
{
    int temp = a;
    a = b;
    b = temp;
}

int x = 5, y = 10;
Swap(ref x, ref y);
// x = 10, y = 5
```

## Out Parameters

Method must assign value before returning:

```csharp
public bool TryParse(string input, out int result)
{
    result = 0;  // Must initialize
    
    if (int.TryParse(input, out result))
        return true;
    return false;
}

if (TryParse("42", out int value))
    Console.WriteLine(value);
```

### Multiple Out Parameters

```csharp
public bool SplitName(string fullName, out string first, out string last)
{
    first = "";
    last = "";
    
    var parts = fullName.Split(' ');
    if (parts.Length != 2)
        return false;
    
    first = parts[0];
    last = parts[1];
    return true;
}

if (SplitName("John Doe", out string fname, out string lname))
    Console.WriteLine($"{fname} {lname}");
```

## In Parameters

Pass by reference but read-only (optimization):

```csharp
public void ProcessLargeData(in LargeStruct data)
{
    // Can read data but cannot modify
    // Avoids copying large struct
}

public struct LargeStruct
{
    public int[] Data { get; set; }
}
```

## Default Parameters

Optional with default values:

```csharp
public void Log(string message, LogLevel level = LogLevel.Info)
{
    Console.WriteLine($"[{level}] {message}");
}

enum LogLevel { Info, Warning, Error }

// Usage
Log("Hello");                           // Uses Info
Log("Warning", LogLevel.Warning);
Log("Error", LogLevel.Error);
```

## Named Parameters

Call using parameter names:

```csharp
public void CreateReport(string title, int pages, 
    string format, bool summary)
{
}

// Positional
CreateReport("Sales", 10, "PDF", true);

// Named (any order)
CreateReport(title: "Sales", format: "Excel", 
    pages: 15, summary: false);
```

## Params Keyword

Variable number of arguments:

```csharp
public int Sum(params int[] numbers)
{
    int total = 0;
    foreach (int num in numbers)
        total += num;
    return total;
}

Sum(1, 2, 3);           // 6
Sum(1, 2, 3, 4, 5);     // 15
```

## Optional vs Default

Difference between optional and default:

```csharp
// Default parameter
public void Method(string text = "default")
{
}

// Both are optional
Method();              // Uses "default"
Method("custom");      // Uses "custom"
```

## Parameter Validation

Validate parameters early:

```csharp
public void ProcessData(int[] data, int minSize)
{
    if (data == null)
        throw new ArgumentNullException(nameof(data));
    
    if (data.Length < minSize)
        throw new ArgumentException("Too small");
    
    // Process...
}
```

## Common Patterns

### Builder Pattern

```csharp
public class QueryBuilder
{
    private string query = "";
    
    public QueryBuilder Select(params string[] columns)
    {
        query = $"SELECT {string.Join(", ", columns)}";
        return this;
    }
    
    public QueryBuilder From(string table)
    {
        query += $" FROM {table}";
        return this;
    }
    
    public string Build() => query;
}

var sql = new QueryBuilder()
    .Select("id", "name", "email")
    .From("Users")
    .Build();
```

### Try Pattern

```csharp
public bool TryGetValue(string key, out object? value)
{
    value = null;
    // Try to get value
    return true;  // or false
}
```

## Summary

- **Ref**: Pass by reference, can modify
- **Out**: Output parameter, must assign
- **In**: Pass by reference, read-only
- **Default**: Optional with default value
- **Named**: Call using parameter names
- **Params**: Variable arguments
- **Validation**: Check parameters early
- **Patterns**: Builder, Try, validator patterns

## Next Steps

- Learn [Method-Overloading](../03-Method-Overloading/00-Method-Overloading.md) for multiple method signatures
- Review [Best-Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md) for parameter guidelines
