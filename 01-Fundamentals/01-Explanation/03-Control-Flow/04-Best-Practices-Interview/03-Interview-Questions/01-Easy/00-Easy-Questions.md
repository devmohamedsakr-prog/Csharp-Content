# Control Flow Interview - Easy Level

## Q1: When would you use if-else vs switch?

**Answer**:
- **If-else**: Complex conditions, boolean logic, ranges
- **Switch**: Single value against many options, cleaner syntax

```csharp
// If-else for complex
if (age >= 18 && hasLicense) { }

// Switch for many cases
switch (day) {
    case "Monday": break;
    case "Friday": break;
}
```

---

## Q2: What's the difference between for and foreach?

**Answer**:
- **For**: Need index or exact count
- **Foreach**: Iterate all items without index

```csharp
for (int i = 0; i < 10; i++) {
    items[i] = i;  // Need index
}

foreach (var item in items) {
    Process(item);  // Just iterate
}
```

---

## Q3: When would you use do-while?

**Answer**:
- When loop must execute at least once
- Menu systems, user input validation

```csharp
do {
    Console.WriteLine("Menu");
} while (choice != "3");  // Always runs once
```

---

## Q4: What does break do?

**Answer**:
- Exits loop immediately
- Stops further iterations

```csharp
for (int i = 0; i < 10; i++) {
    if (i == 5) break;  // Exit at 5
}
```

---

## Q5: What does continue do?

**Answer**:
- Skip current iteration
- Go to next iteration

```csharp
for (int i = 0; i < 10; i++) {
    if (i % 2 == 0) continue;  // Skip even
    Console.WriteLine(i);  // Only odd
}
```

---

## Q6: Why not use goto?

**Answer**:
- Creates "spaghetti code"
- Hard to follow logic
- Use break/return instead

---

## Summary

- If-else for complex, switch for many cases
- For for index, foreach for iteration
- Do-while for at least one execution
- Break exits, continue skips
- Never use goto

---

**Next**: Move to [Medium Questions](../02-Medium/00-Medium-Questions.md)
