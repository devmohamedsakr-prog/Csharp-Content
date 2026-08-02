# Generic Constraints

## Overview
Constraints limit which types can be used as generic parameters.

---

## Class Constraint

T must be a reference type.

```csharp
public class Repository<T> where T : class {
    public void Save(T item) {
        // T is guaranteed to be reference type
    }
}

Repository<string> repo1 = new Repository<string>();  // OK
Repository<Person> repo2 = new Repository<Person>();  // OK
// Repository<int> repo3 = new Repository<int>();  // Error - int is struct
```

---

## Struct Constraint

T must be a value type.

```csharp
public class Calculator<T> where T : struct {
    public T Add(T a, T b) {
        // Complex implementation for value types
        return a;
    }
}

Calculator<int> calc1 = new Calculator<int>();  // OK
Calculator<double> calc2 = new Calculator<double>();  // OK
// Calculator<string> calc3 = new Calculator<string>();  // Error - string is class
```

---

## Base Class Constraint

T must inherit from specific class.

```csharp
public class Entity {
    public int Id { get; set; }
}

public class Repository<T> where T : Entity {
    public void SaveWithId(T item) {
        Console.WriteLine($"Saving with ID: {item.Id}");
    }
}

public class User : Entity {
    public string Name { get; set; }
}

Repository<User> userRepo = new Repository<User>();  // OK
userRepo.SaveWithId(new User { Id = 1, Name = "Alice" });

// Repository<string> stringRepo = new Repository<string>();  // Error
```

---

## Interface Constraint

T must implement specific interface.

```csharp
public interface IComparable<T> {
    int CompareTo(T other);
}

public class Sorter<T> where T : IComparable<T> {
    public void Sort(List<T> items) {
        for (int i = 0; i < items.Count; i++) {
            for (int j = i + 1; j < items.Count; j++) {
                if (items[i].CompareTo(items[j]) > 0) {
                    T temp = items[i];
                    items[i] = items[j];
                    items[j] = temp;
                }
            }
        }
    }
}

public class Person : IComparable<Person> {
    public string Name { get; set; }
    public int Age { get; set; }
    
    public int CompareTo(Person other) {
        return this.Age.CompareTo(other.Age);
    }
}

Sorter<Person> sorter = new Sorter<Person>();  // OK
```

---

## Constructor Constraint

T must have parameterless constructor.

```csharp
public class Factory<T> where T : new() {
    public T CreateInstance() {
        return new T();  // T must have parameterless constructor
    }
}

public class DefaultValue {
    // Has default constructor
}

public class CustomClass {
    public CustomClass(string name) { }
    // No parameterless constructor
}

Factory<DefaultValue> factory1 = new Factory<DefaultValue>();  // OK
factory1.CreateInstance();

// Factory<CustomClass> factory2 = new Factory<CustomClass>();  // Error
```

---

## Multiple Constraints

T must satisfy all constraints.

```csharp
public interface IEntity {
    int Id { get; set; }
}

public abstract class BaseEntity : IEntity {
    public int Id { get; set; }
}

public class Repository<T> where T : BaseEntity, IEntity, new() {
    // T must:
    // - Be class (inherit from BaseEntity)
    // - Implement IEntity
    // - Have parameterless constructor
}

public class User : BaseEntity { }

Repository<User> userRepo = new Repository<User>();  // OK
```

---

## Multiple Type Parameters

Each can have constraints.

```csharp
public class Converter<TSource, TTarget> 
    where TSource : class
    where TTarget : class, new() {
    
    public TTarget Convert(TSource source) {
        var target = new TTarget();
        // Convert properties
        return target;
    }
}

Converter<User, UserDto> converter = new Converter<User, UserDto>();  // OK
```

---

## Covariant and Contravariant Constraints

```csharp
// Covariant - out
public interface IRepository<out T> where T : Entity {
    T Get(int id);
}

// Contravariant - in
public interface ILogger<in T> where T : class {
    void Log(T item);
}

// Use in method
public T GetIfExists<T>(IRepository<T> repo, int id) 
    where T : Entity {
    return repo.Get(id);
}
```

---

## Real-World Example

```csharp
public interface IEntity {
    int Id { get; set; }
}

public class Repository<T> where T : class, IEntity, new() {
    private List<T> items = new List<T>();
    
    public void Add(T item) {
        items.Add(item);
    }
    
    public T Get(int id) {
        return items.FirstOrDefault(x => x.Id == id);
    }
    
    public void Remove(int id) {
        var item = Get(id);
        if (item != null) {
            items.Remove(item);
        }
    }
    
    public List<T> GetAll() {
        return new List<T>(items);
    }
}

public class User : IEntity {
    public int Id { get; set; }
    public string Name { get; set; }
}

// Usage
Repository<User> userRepo = new Repository<User>();
userRepo.Add(new User { Id = 1, Name = "Alice" });
User user = userRepo.Get(1);
```

---

## Constraint Guidelines

✓ **Use constraints for safety**
```csharp
// Good - ensures T is usable
public T CreateAndInitialize<T>() where T : class, new() {
    return new T();
}
```

✓ **Make constraints as specific as needed**
```csharp
// More specific - better design
public class EntityRepository<T> where T : IEntity { }

// Too generic - might allow anything
public class GenericRepository<T> { }
```

---

## Quick Summary

- Constraints limit generic type parameters
- class = reference types only
- struct = value types only
- new() = must have parameterless constructor
- Interface constraints = must implement interface
- Base class constraints = must inherit from class
- Multiple constraints possible
- Improves type safety
