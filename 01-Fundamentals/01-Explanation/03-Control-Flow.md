# Control Flow Statements

## Overview
Control flow statements direct the order of code execution based on conditions.

---

## If-Else Statements

Execute code based on conditions.

### Simple If
```csharp
int age = 18;

if (age >= 18) {
    Console.WriteLine("You are an adult");
}
```

### If-Else
```csharp
int score = 75;

if (score >= 90) {
    Console.WriteLine("A");
} else if (score >= 80) {
    Console.WriteLine("B");
} else if (score >= 70) {
    Console.WriteLine("C");
} else {
    Console.WriteLine("F");
}
```

### Nested If
```csharp
int age = 25;
bool hasLicense = true;

if (age >= 18) {
    if (hasLicense) {
        Console.WriteLine("Can drive");
    } else {
        Console.WriteLine("Need a license");
    }
} else {
    Console.WriteLine("Too young");
}
```

---

## Switch Statements

Choose one path from multiple options.

### Basic Switch
```csharp
string day = "Monday";

switch (day) {
    case "Monday":
        Console.WriteLine("Start of week");
        break;
    case "Friday":
        Console.WriteLine("Almost weekend");
        break;
    case "Saturday":
    case "Sunday":
        Console.WriteLine("Weekend");
        break;
    default:
        Console.WriteLine("Midweek");
        break;
}
```

### Switch Expression (C# 8+)
```csharp
string day = "Monday";

string message = day switch {
    "Monday" => "Start of week",
    "Friday" => "Almost weekend",
    "Saturday" or "Sunday" => "Weekend",
    _ => "Midweek"
};
```

### Pattern Matching in Switch
```csharp
object obj = "Hello";

string result = obj switch {
    string s => $"String: {s}",
    int i => $"Integer: {i}",
    bool b => $"Boolean: {b}",
    null => "Null value",
    _ => "Unknown type"
};
```

---

## For Loop

Execute code a specific number of times.

### Standard For Loop
```csharp
for (int i = 0; i < 5; i++) {
    Console.WriteLine($"Iteration {i}");
}

// Output:
// Iteration 0
// Iteration 1
// Iteration 2
// Iteration 3
// Iteration 4
```

### Nested For Loop
```csharp
// Multiplication table
for (int i = 1; i <= 3; i++) {
    for (int j = 1; j <= 3; j++) {
        Console.WriteLine($"{i} * {j} = {i * j}");
    }
}
```

### For Loop Variations
```csharp
// Multiple initializers
for (int i = 0, j = 10; i < j; i++, j--) {
    Console.WriteLine($"i={i}, j={j}");
}

// Infinite loop (use break to exit)
for (;;) {
    // Code here
}

// Loop backwards
for (int i = 10; i >= 0; i--) {
    Console.WriteLine(i);
}
```

---

## While Loop

Execute code while condition is true.

### Standard While
```csharp
int count = 0;

while (count < 5) {
    Console.WriteLine($"Count: {count}");
    count++;
}
```

### Infinite Loop with Break
```csharp
while (true) {
    Console.Write("Enter a positive number (or 'exit'): ");
    string input = Console.ReadLine();
    
    if (input == "exit") {
        break;  // Exit loop
    }
    
    if (int.Parse(input) > 0) {
        Console.WriteLine("Valid!");
        break;
    }
}
```

---

## Do-While Loop

Execute code at least once, then check condition.

### Basic Do-While
```csharp
int count = 0;

do {
    Console.WriteLine($"Count: {count}");
    count++;
} while (count < 5);

// Always executes at least once, even if condition is false
```

### Menu Example
```csharp
string choice = "";

do {
    Console.WriteLine("1. Play\n2. Settings\n3. Exit");
    choice = Console.ReadLine();
    
    switch (choice) {
        case "1":
            Console.WriteLine("Playing...");
            break;
        case "2":
            Console.WriteLine("Settings...");
            break;
        case "3":
            Console.WriteLine("Goodbye!");
            break;
    }
} while (choice != "3");
```

---

## Foreach Loop

Iterate through collections.

### Array Iteration
```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

foreach (int num in numbers) {
    Console.WriteLine(num);
}
```

### List Iteration
```csharp
List<string> names = new List<string> { "Alice", "Bob", "Charlie" };

foreach (string name in names) {
    Console.WriteLine(name);
}
```

### Dictionary Iteration
```csharp
Dictionary<string, int> ages = new Dictionary<string, int> {
    { "Alice", 25 },
    { "Bob", 30 },
    { "Charlie", 35 }
};

foreach (var person in ages) {
    Console.WriteLine($"{person.Key}: {person.Value}");
}

// Alternative: iterate only values
foreach (int age in ages.Values) {
    Console.WriteLine(age);
}

// Alternative: iterate only keys
foreach (string name in ages.Keys) {
    Console.WriteLine(name);
}
```

---

## Control Flow Keywords

### Break
Stop loop execution immediately.

```csharp
for (int i = 0; i < 10; i++) {
    if (i == 5) {
        break;  // Exit loop when i equals 5
    }
    Console.WriteLine(i);
}
// Output: 0 1 2 3 4
```

### Continue
Skip current iteration, go to next.

```csharp
for (int i = 0; i < 10; i++) {
    if (i % 2 == 0) {
        continue;  // Skip even numbers
    }
    Console.WriteLine(i);
}
// Output: 1 3 5 7 9
```

### Return
Exit method immediately.

```csharp
public int GetFirstPositive(int[] numbers) {
    foreach (int num in numbers) {
        if (num > 0) {
            return num;  // Exit method, return value
        }
    }
    return 0;  // Default if none found
}
```

### Goto (avoid!)
Jump to labeled location (generally bad practice).

```csharp
// NOT RECOMMENDED - use break/continue instead
int count = 0;
start:
    Console.WriteLine(count);
    count++;
    if (count < 5) {
        goto start;
    }
```

---

## Comparing Loop Types

| Loop Type | Use Case | Executes When |
|-----------|----------|--------------|
| **for** | Know iteration count | Condition true |
| **while** | Unknown iteration count | Condition true |
| **do-while** | Need at least one execution | Condition true after first |
| **foreach** | Iterate collections | More items in collection |

---

## Best Practices

✓ Use foreach for collections instead of for
```csharp
// Good
foreach (var item in items) { }

// Less ideal
for (int i = 0; i < items.Count; i++) { }
```

✓ Use meaningful loop variables
```csharp
// Good
foreach (var student in students) { }

// Less clear
foreach (var s in list) { }
```

✓ Avoid deeply nested loops
```csharp
// Good: extract to separate method
foreach (var item in items) {
    ProcessItem(item);
}

// Avoid: deep nesting
foreach (var a in list1) {
    foreach (var b in list2) {
        foreach (var c in list3) {
            // Hard to read
        }
    }
}
```

✓ Use LINQ for complex iterations
```csharp
// Good: LINQ is cleaner
var adults = people.Where(p => p.Age >= 18).ToList();

// Less ideal: manual loop
List<Person> adults = new List<Person>();
foreach (var person in people) {
    if (person.Age >= 18) {
        adults.Add(person);
    }
}
```

---

## Common Mistakes

❌ Infinite loop by accident
```csharp
while (true) {  // Forgot break condition
    Console.WriteLine("Stuck!");
}
```

❌ Off-by-one error
```csharp
for (int i = 0; i <= 5; i++) {  // Includes 5 (0-5 = 6 items)
    Console.WriteLine(i);
}
```

✓ Know your boundaries
```csharp
for (int i = 0; i < 5; i++) {  // Correct: 0-4 = 5 items
    Console.WriteLine(i);
}
```

❌ Forgetting break in switch
```csharp
switch (value) {
    case 1:
        DoSomething();
        // Forgot break - falls through to case 2!
    case 2:
        DoSomethingElse();
        break;
}
```
