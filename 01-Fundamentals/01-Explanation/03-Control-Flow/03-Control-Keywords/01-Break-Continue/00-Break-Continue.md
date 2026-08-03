# Break and Continue

## Overview

Break and continue control loop execution flow without completing all iterations.

## Break

Exit loop immediately.

### Basic Break
```csharp
for (int i = 0; i < 10; i++) {
    if (i == 5) {
        break;  // Exit loop
    }
    Console.WriteLine(i);
}

// Output: 0, 1, 2, 3, 4
```

### Search Example
```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
int target = 6;
int position = -1;

foreach (var num in numbers) {
    if (num == target) {
        position = numbers.IndexOf(num);
        break;  // Found it, exit
    }
}

Console.WriteLine(position);  // 5
```

### In Switch
```csharp
switch (choice) {
    case "1":
        Console.WriteLine("Option 1");
        break;  // Exit switch
    case "2":
        Console.WriteLine("Option 2");
        break;
}
```

---

## Continue

Skip current iteration, go to next.

### Basic Continue
```csharp
for (int i = 0; i < 10; i++) {
    if (i % 2 == 0) {
        continue;  // Skip even numbers
    }
    Console.WriteLine(i);
}

// Output: 1, 3, 5, 7, 9
```

### Filter Example
```csharp
List<User> users = GetUsers();

foreach (var user in users) {
    if (!user.IsActive) {
        continue;  // Skip inactive
    }
    
    SendEmail(user);
}
```

### Nested Loop
```csharp
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 3; j++) {
        if (j == 1) {
            continue;  // Skip j=1, continue inner loop
        }
        Console.WriteLine($"({i},{j})");
    }
}
```

---

## Break vs Continue

| Keyword | Action | Effect |
|---------|--------|--------|
| break | Exit loop | Loop terminates |
| continue | Skip iteration | Goes to next iteration |

**Example**:
```csharp
for (int i = 0; i < 5; i++) {
    if (i == 2) {
        break;  // Exits: 0, 1, then stop
    }
    Console.WriteLine(i);
}

for (int i = 0; i < 5; i++) {
    if (i == 2) {
        continue;  // Skips: 0, 1, 3, 4
    }
    Console.WriteLine(i);
}
```

---

## Real-World Patterns

### Validation Loop
```csharp
while (true) {
    Console.Write("Enter positive number: ");
    if (int.TryParse(Console.ReadLine(), out int num) && num > 0) {
        break;  // Valid input, exit
    }
    Console.WriteLine("Invalid, try again");
}
```

### Data Processing
```csharp
foreach (var record in records) {
    if (record.IsDeleted) {
        continue;  // Skip deleted
    }
    
    if (record.Total > 10000) {
        break;  // Stop if large amount
    }
    
    ProcessRecord(record);
}
```

### Search and Early Exit
```csharp
public bool Contains(int target) {
    foreach (var item in items) {
        if (item == target) {
            return true;  // Found it, exit
        }
    }
    return false;
}
```

---

## Best Practices

✓ Use break to exit early
```csharp
// Good: exit when found
foreach (var item in items) {
    if (IsTarget(item)) {
        Process(item);
        break;
    }
}
```

✓ Use continue to skip
```csharp
// Good: skip invalid items
foreach (var item in items) {
    if (!IsValid(item)) {
        continue;
    }
    Process(item);
}
```

✓ Keep logic simple
```csharp
// Good: clear conditions
if (shouldExit) {
    break;
}

// Complex: hard to follow
if (condition1 && condition2 || condition3) {
    break;
}
```

✓ Consider return instead
```csharp
// Good: return immediately
public bool Find(int target) {
    foreach (var item in items) {
        if (item == target) {
            return true;  // Better than break
        }
    }
    return false;
}
```

---

## Common Mistakes

❌ Break in nested loop
```csharp
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 3; j++) {
        if (j == 1) {
            break;  // Only exits inner loop, not outer
        }
    }
}
```

✓ Use return or separate method
```csharp
for (int i = 0; i < 3; i++) {
    if (!ProcessRow(i)) {
        break;  // Exits outer loop when needed
    }
}
```

---

❌ Complex break conditions
```csharp
if (condition1 && condition2 || condition3 && !condition4) {
    break;  // Hard to understand
}
```

✓ Simplify
```csharp
bool shouldExit = condition1 && condition2 || condition3 && !condition4;
if (shouldExit) {
    break;
}
```

---

## Next Steps

- Study [Return and Goto](../02-Return-Goto/00-Return-Goto.md)
- Review [Best Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
