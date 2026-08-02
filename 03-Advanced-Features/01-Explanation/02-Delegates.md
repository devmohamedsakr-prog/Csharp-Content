# Delegates

## Overview
Delegates are type-safe function pointers or callback methods.

---

## What are Delegates?

```csharp
// Define delegate type
public delegate void Notify(string message);

// Create delegate instance
Notify notifier = Console.WriteLine;

// Use delegate
notifier("Hello");  // Output: Hello

// Multiple methods
notifier += (msg) => Console.WriteLine($"Log: {msg}");
notifier("World");  // Calls both methods
```

---

## Declaring Delegates

```csharp
// Delegate with no parameters, no return
public delegate void Action();

// Delegate with parameters, no return
public delegate void Action<T>(T arg);

// Delegate with return type
public delegate int Func<T, TResult>(T arg);

// Custom delegate
public delegate bool ValidationDelegate(string input);
```

---

## Using Delegates

```csharp
public delegate int Calculate(int a, int b);

public class Calculator {
    public static int Add(int a, int b) {
        return a + b;
    }
    
    public static int Multiply(int a, int b) {
        return a * b;
    }
}

// Create delegate instances
Calculate add = Calculator.Add;
Calculate multiply = Calculator.Multiply;

// Use delegates
Console.WriteLine(add(5, 3));  // 8
Console.WriteLine(multiply(5, 3));  // 15
```

---

## Lambda with Delegates

```csharp
public delegate int Operate(int x, int y);

// Lambda expression
Operate add = (a, b) => a + b;
Operate subtract = (a, b) => a - b;

Console.WriteLine(add(10, 5));  // 15
Console.WriteLine(subtract(10, 5));  // 5
```

---

## Multicasting

Multiple methods called in sequence.

```csharp
public delegate void Notify(string message);

Notify notifier = null;
notifier += Console.WriteLine;
notifier += (msg) => System.Diagnostics.Debug.WriteLine(msg);
notifier += (msg) => File.AppendAllText("log.txt", msg + "\n");

// Calls all three methods
notifier("User logged in");
```

---

## Predefined Delegates

```csharp
// Action - no return
Action greet = () => Console.WriteLine("Hello");
Action<string> greetName = (name) => Console.WriteLine($"Hello {name}");

// Func - returns value
Func<int, int, int> add = (a, b) => a + b;
Func<string, bool> isEmpty = (s) => string.IsNullOrEmpty(s);

// Predicate - returns bool
Predicate<int> isPositive = (x) => x > 0;
Predicate<string> isValidEmail = (email) => email.Contains("@");
```

---

## Callbacks

Using delegates as callbacks.

```csharp
public class Button {
    // Delegate for click event
    public delegate void ClickHandler();
    
    public event ClickHandler OnClick;
    
    public void Click() {
        OnClick?.Invoke();
    }
}

Button button = new Button();
button.OnClick += () => Console.WriteLine("Button clicked!");
button.OnClick += () => Console.WriteLine("Saving...");

button.Click();
// Output:
// Button clicked!
// Saving...
```

---

## Quick Summary

- Delegates are type-safe function pointers
- Can hold reference to methods
- Support multicasting
- Predefined: Action, Func, Predicate
- Used for callbacks and events
