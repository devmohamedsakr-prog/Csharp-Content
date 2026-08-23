# Class Snippets

Generate class structures with built-in snippets.

## class - Class Declaration

**Shortcut:** `class` + Tab

**Generates:**
```csharp
public class ClassName
{
}
```

**Placeholders:**
- ClassName: Replace with your class name

**Usage:**
```csharp
class → Tab
// Now: public class ClassName { }
// Edit ClassName
```

**Examples:**
```csharp
public class Person
{
    public string Name { get; set; }
}

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public List<Product> Items { get; set; }
}
```

---

## cw - Class with Constructor

**Pattern:**
```csharp
public class User
{
    public User()
    {
    }
}
```

**Usage:**
```csharp
class → Tab
ctor → Tab
// Class with constructor
```

---

## interface - Interface Declaration

**Shortcut:** `interface` + Tab

**Generates:**
```csharp
public interface IClassName
{
}
```

**Usage:**
```csharp
interface → Tab
// Creates interface with I prefix convention
```

**Examples:**
```csharp
public interface IRepository
{
    T GetById(int id);
    void Add(T item);
    void Remove(int id);
}

public interface ILogger
{
    void Log(string message);
    void LogError(string error);
}

public interface IValidator
{
    bool IsValid(object obj);
    string GetErrorMessage();
}
```

---

## struct - Struct Declaration

**Shortcut:** `struct` + Tab

**Generates:**
```csharp
public struct StructName
{
}
```

**Usage:**
```csharp
struct → Tab
// Creates value type structure
```

**Examples:**
```csharp
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

public struct Date
{
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}
```

**Note:** Use struct for small, immutable value types

---

## enum - Enum Declaration

**Shortcut:** `enum` + Tab

**Generates:**
```csharp
public enum EnumName
{
}
```

**Usage:**
```csharp
enum → Tab
// Creates enumeration
```

**Examples:**
```csharp
public enum Status
{
    Active,
    Inactive,
    Pending
}

public enum Priority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

public enum DayOfWeek
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}
```

**Usage:**
```csharp
Status status = Status.Active;
Priority priority = Priority.High;

if (status == Status.Active)
{
    Console.WriteLine("Active");
}
```

---

## Abstract Class

**Pattern:**
```csharp
public abstract class Animal
{
    public string Name { get; set; }
    
    public abstract void MakeSound();
    
    public virtual void Sleep()
    {
        Console.WriteLine("Sleeping...");
    }
}

public class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }
}
```

---

## Sealed Class

**Pattern:**
```csharp
public sealed class FinalClass
{
    // Cannot be inherited
    public string Name { get; set; }
}
```

**Note:** Cannot be inherited, useful for security

---

## Static Class

**Pattern:**
```csharp
public static class MathHelper
{
    public static double Square(double num) => num * num;
    
    public static double Cube(double num) => num * num * num;
}

// Usage
double result = MathHelper.Square(5);
```

---

## Nested Class

**Pattern:**
```csharp
public class Outer
{
    public int OuterValue { get; set; }
    
    public class Inner
    {
        public int InnerValue { get; set; }
    }
}

// Usage
var outer = new Outer();
var inner = new Outer.Inner();
```

---

## Quick Reference

| Type | Shortcut | Purpose |
|------|----------|---------|
| Class | `class` | Regular class |
| Interface | `interface` | Contract definition |
| Struct | `struct` | Value type |
| Enum | `enum` | Fixed set of values |
| Abstract | Manual | Base for inheritance |
| Static | Manual | Utility class |
| Sealed | Manual | No inheritance |

