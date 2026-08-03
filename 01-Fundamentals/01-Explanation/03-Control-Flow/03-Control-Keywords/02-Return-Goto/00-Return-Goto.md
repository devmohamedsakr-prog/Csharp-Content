# Return and Goto

## Return

Exit method immediately and optionally return a value.

### Basic Return (Void)
```csharp
public void Greet(string name) {
    if (string.IsNullOrEmpty(name)) {
        Console.WriteLine("No name provided");
        return;  // Exit method
    }
    
    Console.WriteLine($"Hello, {name}");
}
```

### Return with Value
```csharp
public int GetFirstPositive(int[] numbers) {
    foreach (int num in numbers) {
        if (num > 0) {
            return num;  // Exit, return value
        }
    }
    return 0;  // Default if none found
}
```

### Early Exit Pattern
```csharp
public void ProcessUser(User user) {
    if (user == null) return;
    if (!user.IsActive) return;
    if (user.IsBlocked) return;
    
    // Only active, non-blocked users reach here
    DoActualProcessing(user);
}
```

---

## Best Use of Return

✓ Exit early for invalid cases
```csharp
public bool ValidateData(Data data) {
    if (data == null) return false;
    if (string.IsNullOrEmpty(data.Name)) return false;
    if (data.Age < 0) return false;
    
    return true;
}
```

✓ Reduce nesting
```csharp
// Good: early returns, no nesting
public string GetStatus(Order order) {
    if (order == null) return "Invalid";
    if (!order.IsPaid) return "Unpaid";
    if (order.IsShipped) return "Shipped";
    return "Processing";
}
```

---

## Goto (Avoid!)

Jump to labeled location. Generally considered poor practice.

### Basic Goto (NOT Recommended)
```csharp
// DON'T USE THIS
int count = 0;
start:
    Console.WriteLine(count);
    count++;
    if (count < 5) {
        goto start;  // Jump back to 'start' label
    }
```

### Why Avoid Goto

- Creates "spaghetti code" - hard to follow
- Makes code unpredictable
- Causes maintenance problems
- Violates structured programming

```csharp
// Bad: goto makes flow unclear
for (int i = 0; i < 10; i++) {
    if (error) {
        goto ErrorHandler;
    }
    Process();
}

ErrorHandler:
    Console.WriteLine("Error occurred");
```

---

## Alternatives to Goto

### Instead of Goto Loop - Use Break
```csharp
// Bad with goto
int i = 0;
loop:
    if (i >= 5) goto end;
    Console.WriteLine(i);
    i++;
    goto loop;
end:

// Good: use loop
for (int i = 0; i < 5; i++) {
    Console.WriteLine(i);
}
```

### Instead of Goto Return - Use Return
```csharp
// Bad: goto to return
if (condition) {
    goto DoneWithError;
}
ProcessData();
return true;

DoneWithError:
    return false;

// Good: direct return
if (condition) {
    return false;
}
ProcessData();
return true;
```

### Instead of Goto Exception - Use Exception
```csharp
// Bad: goto for errors
if (error) {
    goto ErrorHandler;
}

ErrorHandler:
    Console.WriteLine("Error");

// Good: use try-catch
try {
    if (error) {
        throw new InvalidOperationException();
    }
} catch (InvalidOperationException) {
    Console.WriteLine("Error");
}
```

---

## Real-World Return Examples

### Validation
```csharp
public bool IsValidEmail(string email) {
    if (string.IsNullOrWhiteSpace(email)) return false;
    if (!email.Contains("@")) return false;
    if (!email.Contains(".")) return false;
    return true;
}
```

### Search
```csharp
public User FindUser(string name) {
    foreach (var user in users) {
        if (user.Name == name) {
            return user;  // Found
        }
    }
    return null;  // Not found
}
```

### Processing
```csharp
public string ProcessOrder(Order order) {
    if (order == null) return "Order not found";
    if (order.Items.Count == 0) return "Order is empty";
    if (order.Total <= 0) return "Invalid total";
    
    order.Status = "Processing";
    return "Success";
}
```

---

## Summary

| Keyword | Use | Recommendation |
|---------|-----|-----------------|
| return | Exit method, return value | ✓ USE |
| goto | Jump to label | ✗ AVOID |

---

## Best Practices

✓ Use return for early exit
```csharp
if (!condition) return;
```

✓ Use break for loops
```csharp
if (found) break;
```

✓ Use exceptions for errors
```csharp
if (invalid) throw new ArgumentException();
```

✓ Never use goto
```csharp
// Don't do this ever!
goto SomeLabel;
```

---

## Next Steps

- Review [All Control Keywords](../README.md)
- Study [Best Practices](../../04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md)
- Check [Interview Questions](../../04-Best-Practices-Interview/03-Interview-Questions/README.md)
