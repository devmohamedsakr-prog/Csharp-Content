# Generics

## Overview
Generics allow you to write type-safe code that works with different types without casting.

---

## What are Generics?

```csharp
// Without generics - type-unsafe
ArrayList list = new ArrayList();
list.Add(5);
int num = (int)list[0];  // Casting needed

// With generics - type-safe
List<int> list = new List<int>();
list.Add(5);
int num = list[0];  // No casting
```

---

## Generic Classes

```csharp
// Generic class definition
public class Container<T> {
    private T item;
    
    public void Add(T value) {
        item = value;
    }
    
    public T Get() {
        return item;
    }
}

// Usage with different types
Container<int> intContainer = new Container<int>();
intContainer.Add(42);
int value = intContainer.Get();  // 42

Container<string> stringContainer = new Container<string>();
stringContainer.Add("Hello");
string text = stringContainer.Get();  // "Hello"

Container<Person> personContainer = new Container<Person>();
personContainer.Add(new Person { Name = "Alice" });
Person person = personContainer.Get();
```

---

## Generic Methods

```csharp
public class Utilities {
    // Generic method
    public static void PrintArray<T>(T[] array) {
        foreach (T item in array) {
            Console.WriteLine(item);
        }
    }
    
    public static T GetFirst<T>(List<T> list) {
        return list.Count > 0 ? list[0] : default(T);
    }
    
    public static T GetMax<T>(T a, T b) where T : IComparable<T> {
        return a.CompareTo(b) > 0 ? a : b;
    }
}

// Usage
Utilities.PrintArray(new int[] { 1, 2, 3 });
Utilities.PrintArray(new string[] { "a", "b", "c" });

int max = Utilities.GetMax(5, 10);  // 10
string maxStr = Utilities.GetMax("apple", "zebra");  // "zebra"
```

---

## Generic Constraints

Limit which types can be used.

```csharp
// Where T is a class
public class Repository<T> where T : class {
    public void Save(T item) {
        // T must be reference type
    }
}

// Where T is a struct
public class Validator<T> where T : struct {
    public void Validate(T value) {
        // T must be value type
    }
}

// Where T has parameterless constructor
public class Factory<T> where T : new() {
    public T CreateInstance() {
        return new T();
    }
}

// Where T inherits from base class
public class Repository<T> where T : Entity {
    public void Save(T item) {
        // T must inherit from Entity
    }
}

// Where T implements interface
public class Collection<T> where T : IComparable {
    public void Sort(List<T> items) {
        // T must implement IComparable
    }
}

// Multiple constraints
public class Service<T> where T : class, IEntity, new() {
    // T must be class, implement IEntity, have parameterless constructor
}
```

---

## Covariance and Contravariance

### Covariance (out)
Can return more derived type.

```csharp
public interface IRepository<out T> {
    T Get(int id);  // out - only return T
}

public class AnimalRepository : IRepository<Animal> {
    public Animal Get(int id) {
        return new Animal();
    }
}

IRepository<Animal> repo = new AnimalRepository();
IRepository<Dog> dogRepo = repo;  // OK - covariance
```

### Contravariance (in)
Can accept less derived type.

```csharp
public interface ILogger<in T> {
    void Log(T item);  // in - only accept T
}

public class ObjectLogger : ILogger<object> {
    public void Log(object item) {
        Console.WriteLine(item);
    }
}

ILogger<object> logger = new ObjectLogger();
ILogger<string> stringLogger = logger;  // OK - contravariance
stringLogger.Log("Hello");
```

---

## Benefits

✓ **Type Safety**
Compile-time checking instead of runtime casting.

✓ **Performance**
No boxing for value types.

✓ **Reusability**
Single implementation works for many types.

✓ **Cleaner Code**
No casting needed.

---

## Common Generic Types

```csharp
// List<T>
List<string> names = new List<string>();

// Dictionary<K, V>
Dictionary<string, int> ages = new Dictionary<string, int>();

// Queue<T>
Queue<int> queue = new Queue<int>();

// Stack<T>
Stack<double> stack = new Stack<double>();

// HashSet<T>
HashSet<Guid> ids = new HashSet<Guid>();

// Tuple<T1, T2, ...>
Tuple<string, int> person = new Tuple<string, int>("Alice", 30);

// Nullable<T>
int? nullableInt = null;
```

---

## Quick Summary

- Generics provide type safety
- Work with different types without casting
- Generic constraints limit which types can be used
- Improves performance (no boxing)
- Makes code more reusable
