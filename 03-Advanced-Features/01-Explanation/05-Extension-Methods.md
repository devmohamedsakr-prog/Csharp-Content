# Extension Methods

## Overview
Extension methods allow adding methods to existing types without modifying the original type.

---

## Declaring Extension Methods

```csharp
// Extension class (must be static)
public static class StringExtensions {
    // Extension method - first parameter has 'this'
    public static int WordCount(this string str) {
        return str.Split(' ').Length;
    }
    
    public static string Reverse(this string str) {
        char[] chars = str.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
    
    public static string Capitalize(this string str) {
        if (str.Length == 0) return str;
        return char.ToUpper(str[0]) + str.Substring(1);
    }
}

// Usage - looks like instance method
string text = "hello world";
int count = text.WordCount();  // 2
string reversed = text.Reverse();  // "dlrow olleh"
string capitalized = text.Capitalize();  // "Hello world"
```

---

## Extending Collections

```csharp
public static class CollectionExtensions {
    // Extend List<T>
    public static void RemoveAll<T>(this List<T> list, Predicate<T> predicate) {
        for (int i = list.Count - 1; i >= 0; i--) {
            if (predicate(list[i])) {
                list.RemoveAt(i);
            }
        }
    }
    
    // Extend IEnumerable<T>
    public static List<T> Flatten<T>(this IEnumerable<List<T>> source) {
        List<T> result = new List<T>();
        foreach (var list in source) {
            result.AddRange(list);
        }
        return result;
    }
    
    // Safe index access
    public static T GetSafe<T>(this List<T> list, int index) {
        if (index >= 0 && index < list.Count) {
            return list[index];
        }
        return default(T);
    }
}

// Usage
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
numbers.RemoveAll(n => n > 3);  // [1, 2, 3]

List<List<int>> lists = new List<List<int>> {
    new List<int> { 1, 2 },
    new List<int> { 3, 4 }
};
var flattened = lists.Flatten();  // [1, 2, 3, 4]

var safe = numbers.GetSafe(10);  // default(int)
```

---

## Extending Value Types

```csharp
public static class IntExtensions {
    public static bool IsEven(this int number) {
        return number % 2 == 0;
    }
    
    public static bool IsPrime(this int number) {
        if (number < 2) return false;
        for (int i = 2; i * i <= number; i++) {
            if (number % i == 0) return false;
        }
        return true;
    }
    
    public static int Square(this int number) {
        return number * number;
    }
}

// Usage
int x = 5;
bool even = x.IsEven();  // false
bool prime = x.IsPrime();  // true
int squared = x.Square();  // 25
```

---

## Real-World Examples

### Validation Extension

```csharp
public static class ValidationExtensions {
    public static bool IsValidEmail(this string email) {
        return email.Contains("@") && email.Contains(".");
    }
    
    public static bool IsNullOrEmpty(this string str) {
        return string.IsNullOrEmpty(str);
    }
    
    public static bool IsInRange(this int value, int min, int max) {
        return value >= min && value <= max;
    }
}

// Usage
if (email.IsValidEmail()) {
    // Process email
}

if (value.IsInRange(0, 100)) {
    // Value is valid
}
```

### Utility Extension

```csharp
public static class DateTimeExtensions {
    public static bool IsWeekend(this DateTime date) {
        return date.DayOfWeek == DayOfWeek.Saturday || 
               date.DayOfWeek == DayOfWeek.Sunday;
    }
    
    public static DateTime NextDay(this DateTime date) {
        return date.AddDays(1);
    }
}

// Usage
DateTime today = DateTime.Now;
if (today.IsWeekend()) {
    Console.WriteLine("It's weekend!");
}

DateTime tomorrow = today.NextDay();
```

---

## Important Notes

⚠️ **Extension methods cannot**:
- Access private members
- Override existing methods
- Be used for operators or events (mostly)
- Be used as named parameters (extension method syntax only)

✓ **They are called on**:
- Static method calls under the hood
- Instance like static calls

```csharp
// These are equivalent
int result = StringExtensions.WordCount("hello world");
int result = "hello world".WordCount();  // Much cleaner!
```

---

## Namespace Considerations

```csharp
// Extension methods must be in scope
using MyNamespace.Extensions;  // Must include namespace

// Without using statement, must use full path
int count = MyNamespace.Extensions.StringExtensions.WordCount("hello");
```

---

## Best Practices

✓ **Group related extensions**
```csharp
public static class StringExtensions {
    public static string Reverse(this string str) { }
    public static int WordCount(this string str) { }
    public static bool IsValidEmail(this string str) { }
}
```

✓ **Use descriptive names**
```csharp
// Good
public static bool IsValidEmail(this string email) { }

// Bad
public static bool Check(this string s) { }
```

✓ **Document with XML comments**
```csharp
/// <summary>
/// Counts the number of words in a string
/// </summary>
public static int WordCount(this string str) { }
```

---

## Quick Summary

- Extend existing types without modification
- Use 'this' for first parameter
- Must be in static class
- Called like instance methods
- Great for utility and validation methods
- Commonly used in LINQ
