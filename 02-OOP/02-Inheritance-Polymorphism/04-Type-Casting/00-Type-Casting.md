# Type Casting and Conversions

## Overview

Type casting converts between derived and base class types. Upcasting (derived to base) is always safe. Downcasting (base to derived) requires type checking to prevent runtime errors.

## Upcasting (Derived to Base)

Safe conversion - always works:

```csharp
public class Animal { }
public class Dog : Animal { }

// Upcasting - safe
Dog dog = new Dog();
Animal animal = dog;  // No cast needed, implicit
// Dog IS-A Animal, so this is always valid
```

Explicit cast is unnecessary but can be written:

```csharp
Dog dog = new Dog();
Animal animal = (Animal)dog;  // Explicit cast (optional)
```

## Downcasting (Base to Derived)

Potentially unsafe - requires type checking:

```csharp
Animal animal = new Dog();
Dog dog = (Dog)animal;  // Direct cast - risky if not actually a Dog
```

Safe downcasting requires verification.

## Type Checking: `is` Keyword

Check type before casting:

```csharp
public class Animal { }
public class Dog : Animal { }
public class Cat : Animal { }

Animal animal = GetAnimal();

// Check type before casting
if (animal is Dog)
{
    Dog dog = (Dog)animal;
    dog.Bark();
}
else if (animal is Cat)
{
    Cat cat = (Cat)animal;
    cat.Meow();
}
```

## Safe Casting: `as` Keyword

Returns null if cast fails (no exception):

```csharp
Animal animal = new Dog();

// as - returns null if not compatible
Dog dog = animal as Dog;
if (dog != null)
{
    dog.Bark();
}

// If wrong type, dog is null (no error thrown)
Animal cat = new Cat();
Dog dog2 = cat as Dog;  // dog2 is null
// No exception - just null
```

## Pattern Matching (C# 7+)

Modern syntax combining type check and cast:

```csharp
Animal animal = GetAnimal();

// Pattern matching - check and cast in one line
if (animal is Dog dog)
{
    dog.Bark();  // dog already cast, ready to use
}
else if (animal is Cat cat)
{
    cat.Meow();
}
```

## Comparison of Methods

| Method | Syntax | Safe | Throws | Returns |
|--------|--------|------|--------|---------|
| Direct cast | `(Dog)animal` | No | Yes | Reference |
| `is` check | `animal is Dog` | Yes | No | bool |
| `as` cast | `animal as Dog` | Yes | No | null if fail |
| Pattern match | `is Dog dog` | Yes | No | bool |

```csharp
Animal animal = GetAnimal();

// 1. Direct cast - throws if wrong type
Dog dog1 = (Dog)animal;  // InvalidCastException if not Dog

// 2. With is check - safe
if (animal is Dog) { Dog dog = (Dog)animal; }

// 3. With as - safe
Dog dog3 = animal as Dog;
if (dog3 != null) { }

// 4. Pattern matching - safest/cleanest
if (animal is Dog dog4) { }
```

## Polymorphic Collections

Casting with collections:

```csharp
List<Animal> animals = new List<Animal>
{
    new Dog(),
    new Cat(),
    new Animal()
};

foreach (var animal in animals)
{
    // Pattern matching to handle each type
    if (animal is Dog dog)
    {
        dog.Bark();
    }
    else if (animal is Cat cat)
    {
        cat.Meow();
    }
}
```

## Interface Type Checking

Check and cast to interfaces too:

```csharp
public interface IPet
{
    void Play();
}

public class Dog : Animal, IPet
{
    public void Play() { Console.WriteLine("Playing!"); }
}

Animal animal = new Dog();

// Check for interface
if (animal is IPet pet)
{
    pet.Play();
}
```

## Avoiding Invalid Casts

Common mistakes:

```csharp
// Bad - will throw exception
Animal animal = new Cat();
Dog dog = (Dog)animal;  // InvalidCastException!

// Good - check first
if (animal is Dog d)
{
    // Safe to use d
}

// Or with as
Dog dog2 = animal as Dog;
if (dog2 != null)
{
    // Safe to use dog2
}
```

## Generic Type Checking

With generic collections:

```csharp
List<Animal> animals = new List<Animal>();

// Safe casting in LINQ
var dogs = animals.OfType<Dog>();  // Gets only Dog instances

// Or pattern matching
var pets = animals.Where(a => a is IPet).Cast<IPet>();
```

## Summary

- **Upcasting** - Derived to Base (always safe)
- **Downcasting** - Base to Derived (requires checking)
- **Direct cast** - Fast but throws on failure
- **as operator** - Safe, returns null on failure
- **is check** - True/false type verification
- **Pattern matching** - Modern, clean syntax
- **Always verify** - Before downcasting

## Next Steps

- Learn [Polymorphism-Patterns](../05-Polymorphism-Patterns/00-Polymorphism-Patterns.md) for design patterns
- Study [Interfaces](../../03-Advanced-OOP/01-Interfaces/00-Interfaces.md) for contracts
- Review [Abstract-Classes](../../03-Advanced-OOP/02-Abstract-Classes/00-Abstract-Classes.md) for required implementations
