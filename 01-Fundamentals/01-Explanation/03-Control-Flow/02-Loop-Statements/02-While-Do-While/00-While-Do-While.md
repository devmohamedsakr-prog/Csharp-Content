# While and Do-While Loops

## While Loop

Execute code while condition is true. Condition checked before each iteration.

### Standard While
```csharp
int count = 0;

while (count < 5) {
    Console.WriteLine($"Count: {count}");
    count++;
}

// Output: 0, 1, 2, 3, 4
```

**Use When**: Unknown iteration count, condition-based looping

---

### Infinite Loop with Break
```csharp
while (true) {
    Console.Write("Enter positive number (or 'exit'): ");
    string input = Console.ReadLine();
    
    if (input == "exit") {
        break;  // Exit loop
    }
    
    if (int.TryParse(input, out int num) && num > 0) {
        Console.WriteLine($"Valid: {num}");
        break;
    }
    
    Console.WriteLine("Invalid input");
}
```

---

### Real-World Example
```csharp
// Read until valid
public int GetPositiveNumber() {
    while (true) {
        if (int.TryParse(Console.ReadLine(), out int num) && num > 0) {
            return num;
        }
        Console.WriteLine("Enter positive number");
    }
}
```

---

## Do-While Loop

Execute code at least once, then check condition. Condition checked after each iteration.

### Standard Do-While
```csharp
int count = 0;

do {
    Console.WriteLine($"Count: {count}");
    count++;
} while (count < 5);

// Output: 0, 1, 2, 3, 4
// Executes at least once even if condition false
```

**Difference**: Do-While always runs at least once

---

### Menu Example
```csharp
string choice;

do {
    Console.WriteLine("=== MENU ===");
    Console.WriteLine("1. Play");
    Console.WriteLine("2. Settings");
    Console.WriteLine("3. Exit");
    Console.Write("Choose: ");
    
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
        default:
            Console.WriteLine("Invalid choice");
            break;
    }
} while (choice != "3");
```

---

## While vs Do-While

| Aspect | While | Do-While |
|--------|-------|----------|
| Condition check | Before iteration | After iteration |
| Minimum executions | 0 (might not run) | 1 (always runs) |
| Use case | Unknown count | Menu, validation |

**Example**:
```csharp
// While: might not execute
int x = 10;
while (x < 5) {
    Console.WriteLine(x);  // Never executes
}

// Do-While: always executes at least once
int y = 10;
do {
    Console.WriteLine(y);  // Executes once
} while (y < 5);
```

---

## Best Practices

✓ Use while for condition-based loops
```csharp
while (reader.Read()) {
    ProcessRecord();
}
```

✓ Use do-while for user interaction
```csharp
do {
    Console.WriteLine("Continue? (y/n): ");
} while (Console.ReadLine().ToLower() == "y");
```

✓ Always ensure loop terminates
```csharp
// Good: counter ensures termination
int attempts = 0;
while (attempts < 3) {
    if (TryConnect()) break;
    attempts++;
}
```

✓ Avoid complex conditions
```csharp
// Good: simple condition
while (isRunning) {
    // Process
}

// Complex: harder to understand
while (isRunning && !shouldShutdown && retryCount < 5) {
    // Process
}
```

---

## Common Mistakes

❌ Infinite loop - forgetting update
```csharp
int i = 0;
while (i < 10) {
    Console.WriteLine(i);
    // Forgot i++! Infinite loop
}
```

✓ Update loop variable
```csharp
int i = 0;
while (i < 10) {
    Console.WriteLine(i);
    i++;  // Update
}
```

---

❌ Wrong condition
```csharp
while (shouldContinue = true) {  // Always true!
    // Infinite loop
}
```

✓ Use comparison
```csharp
while (shouldContinue == true) {
    // Correct
}
```

---

## Real-World Patterns

### Retry Logic
```csharp
int retries = 0;
while (retries < 3) {
    if (TryConnect()) {
        return true;
    }
    retries++;
    Thread.Sleep(1000);  // Wait before retry
}
return false;
```

### Data Processing
```csharp
while (reader.Read()) {
    string name = reader["Name"].ToString();
    int age = (int)reader["Age"];
    ProcessUser(name, age);
}
```

### User Input Validation
```csharp
bool valid = false;
do {
    Console.Write("Enter age: ");
    valid = int.TryParse(Console.ReadLine(), out int age) && age > 0;
    if (!valid) {
        Console.WriteLine("Invalid input");
    }
} while (!valid);
```

---

## Next Steps

- Study [Foreach Loop](../03-ForEach-Loop/00-ForEach-Loop.md)
- Learn [For Loop](../01-For-Loop/00-For-Loop.md)
- Review [Control Keywords](../../03-Control-Keywords/README.md)
