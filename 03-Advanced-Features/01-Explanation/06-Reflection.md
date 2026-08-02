# Reflection

## Overview
Reflection allows inspecting and manipulating types and objects at runtime.

---

## Getting Type Information

```csharp
// Get type from object
Person person = new Person { Name = "Alice", Age = 30 };
Type type = person.GetType();
Console.WriteLine(type.Name);  // "Person"

// Get type directly
Type type2 = typeof(Person);

// Get type by name
Type type3 = Type.GetType("MyNamespace.Person");

// Get base type
Type baseType = type.BaseType;  // System.Object
```

---

## Inspecting Members

```csharp
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
    
    public void Greet() {
        Console.WriteLine($"Hello, I'm {Name}");
    }
}

Type type = typeof(Person);

// Get properties
PropertyInfo[] properties = type.GetProperties();
foreach (PropertyInfo prop in properties) {
    Console.WriteLine($"Property: {prop.Name}, Type: {prop.PropertyType}");
}

// Get methods
MethodInfo[] methods = type.GetMethods();
foreach (MethodInfo method in methods) {
    Console.WriteLine($"Method: {method.Name}");
}

// Get fields
FieldInfo[] fields = type.GetFields();

// Get specific member
PropertyInfo nameProp = type.GetProperty("Name");
MethodInfo greetMethod = type.GetMethod("Greet");
```

---

## Creating Instances Dynamically

```csharp
Type type = typeof(Person);

// Create instance using Activator
Person person1 = (Person)Activator.CreateInstance(type);

// With constructor parameters
Person person2 = (Person)Activator.CreateInstance(
    type, 
    new object[] { "Alice", 30 }
);

// Create without specifying type (returns object)
object obj = Activator.CreateInstance(type);
```

---

## Getting and Setting Properties

```csharp
Person person = new Person();
Type type = typeof(Person);

// Get property value
PropertyInfo nameProp = type.GetProperty("Name");
object name = nameProp.GetValue(person);  // null

// Set property value
nameProp.SetValue(person, "Bob");
name = nameProp.GetValue(person);  // "Bob"

// Get age
PropertyInfo ageProp = type.GetProperty("Age");
int age = (int)ageProp.GetValue(person);
```

---

## Invoking Methods

```csharp
Person person = new Person { Name = "Alice" };
Type type = typeof(Person);

// Get method
MethodInfo greetMethod = type.GetMethod("Greet");

// Invoke method
greetMethod.Invoke(person, null);  // Calls Greet()

// Method with parameters
MethodInfo method = type.GetMethod("Greet", new Type[] { typeof(string) });
method.Invoke(person, new object[] { "World" });
```

---

## Working with Attributes

```csharp
// Custom attribute
[AttributeUsage(AttributeTargets.Class)]
public class AuthorAttribute : Attribute {
    public string Name { get; set; }
}

// Apply attribute
[Author(Name = "John")]
public class Person { }

// Read attribute
Type type = typeof(Person);
AuthorAttribute attr = (AuthorAttribute)Attribute.GetCustomAttribute(
    type, 
    typeof(AuthorAttribute)
);

if (attr != null) {
    Console.WriteLine($"Author: {attr.Name}");
}

// Get all attributes
object[] attributes = type.GetCustomAttributes();
foreach (var attribute in attributes) {
    Console.WriteLine($"Attribute: {attribute.GetType().Name}");
}
```

---

## Real-World Example: Serialization

```csharp
public class ObjectSerializer {
    public static string Serialize(object obj) {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();
        
        var parts = new List<string>();
        foreach (PropertyInfo prop in properties) {
            object value = prop.GetValue(obj);
            parts.Add($"{prop.Name}:{value}");
        }
        
        return string.Join(";", parts);
    }
    
    public static T Deserialize<T>(string data) where T : new() {
        T obj = new T();
        Type type = typeof(T);
        
        foreach (string pair in data.Split(';')) {
            string[] parts = pair.Split(':');
            PropertyInfo prop = type.GetProperty(parts[0]);
            if (prop != null) {
                prop.SetValue(obj, Convert.ChangeType(parts[1], prop.PropertyType));
            }
        }
        
        return obj;
    }
}

// Usage
Person person = new Person { Name = "Alice", Age = 30 };
string serialized = ObjectSerializer.Serialize(person);  // "Name:Alice;Age:30"

Person deserialized = ObjectSerializer.Deserialize<Person>(serialized);
```

---

## Performance Note

⚠️ **Reflection is slow**

```csharp
// Slow
PropertyInfo prop = type.GetProperty("Name");
prop.SetValue(obj, "Value");

// Better - use delegates/expressions for repeated access
Action<object, object> setter = (obj, value) =>
    type.GetProperty("Name").SetValue(obj, value);

setter(obj, "Value");  // Faster on repeated calls
```

---

## Best Practices

✓ **Cache reflection results**
```csharp
private static PropertyInfo nameProperty = typeof(Person).GetProperty("Name");

// Reuse
nameProperty.SetValue(obj, "Value");
```

✓ **Use generics when possible**
```csharp
public T Get<T>(object obj) where T : class {
    return obj as T;
}
```

✓ **Avoid in performance-critical code**
```csharp
// Bad - in loop
for (int i = 0; i < 1000; i++) {
    typeof(Person).GetProperty("Name");
}

// Better - cache outside loop
PropertyInfo prop = typeof(Person).GetProperty("Name");
for (int i = 0; i < 1000; i++) {
    prop.GetValue(obj);
}
```

---

## Quick Summary

- Reflection inspects types at runtime
- Get properties, methods, fields, attributes
- Create instances dynamically
- Get/set values and invoke methods
- Used for serialization, ORM, frameworks
- Performance overhead - use carefully
