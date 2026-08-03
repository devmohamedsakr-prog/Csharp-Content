# Control Flow: Common Mistakes

## Mistake 1: Forgetting Break in Switch

❌ Wrong
```csharp
switch (day) {
    case "Monday":
        Console.WriteLine("Work");
        // Falls through!
    case "Friday":
        Console.WriteLine("Almost weekend");
        break;
}
```

✓ Correct
```csharp
switch (day) {
    case "Monday":
        Console.WriteLine("Work");
        break;  // Required
    case "Friday":
        Console.WriteLine("Almost weekend");
        break;
}
```

---

## Mistake 2: Off-by-One Error

❌ Wrong
```csharp
for (int i = 0; i <= 5; i++) {  // Includes 5 (6 items: 0-5)
    Console.WriteLine(i);
}
```

✓ Correct
```csharp
for (int i = 0; i < 5; i++) {   // Excludes 5 (5 items: 0-4)
    Console.WriteLine(i);
}
```

---

## Mistake 3: Infinite Loop

❌ Wrong
```csharp
int i = 0;
while (i < 10) {
    Console.WriteLine(i);
    // Forgot i++!
}
```

✓ Correct
```csharp
int i = 0;
while (i < 10) {
    Console.WriteLine(i);
    i++;  // Update
}
```

---

## Mistake 4: Modifying Collection During Iteration

❌ Wrong
```csharp
foreach (var item in list) {
    if (item > 100) {
        list.Remove(item);  // InvalidOperationException!
    }
}
```

✓ Correct
```csharp
var toRemove = list.Where(x => x > 100).ToList();
foreach (var item in toRemove) {
    list.Remove(item);  // Safe
}
```

---

## Mistake 5: Deep Nesting

❌ Hard to Read
```csharp
if (condition1) {
    if (condition2) {
        if (condition3) {
            if (condition4) {
                Process();
            }
        }
    }
}
```

✓ Better
```csharp
if (!condition1) return;
if (!condition2) return;
if (!condition3) return;
if (!condition4) return;

Process();
```

---

## Mistake 6: Using Goto

❌ Wrong
```csharp
int count = 0;
start:
    Console.WriteLine(count);
    count++;
    if (count < 5) {
        goto start;  // Creates spaghetti code!
    }
```

✓ Correct
```csharp
for (int count = 0; count < 5; count++) {
    Console.WriteLine(count);
}
```

---

## Mistake 7: Wrong Loop Type

❌ Overcomplicated
```csharp
for (int i = 0; i < items.Count; i++) {
    Console.WriteLine(items[i]);  // Don't need index
}
```

✓ Simple
```csharp
foreach (var item in items) {
    Console.WriteLine(item);  // Simpler
}
```

---

## Mistake 8: Missing Default in Switch

❌ Incomplete
```csharp
switch (value) {
    case 1:
        // Handle 1
        break;
    case 2:
        // Handle 2
        break;
    // What about other values?
}
```

✓ Complete
```csharp
switch (value) {
    case 1:
        // Handle 1
        break;
    case 2:
        // Handle 2
        break;
    default:
        // Handle unexpected
        break;
}
```

---

## Mistake 9: Assignment in Condition

❌ Wrong
```csharp
while (shouldContinue = true) {  // Assignment, not comparison!
    // Always true - infinite loop!
}
```

✓ Correct
```csharp
while (shouldContinue == true) {  // Comparison
    // Correct
}
```

---

## Mistake 10: Trying to Modify Loop Variable (For)

❌ Doesn't Work
```csharp
int[] numbers = { 1, 2, 3 };
foreach (int x in numbers) {
    x = x * 2;  // Doesn't affect original
}
// numbers still { 1, 2, 3 }
```

✓ Use For Loop
```csharp
int[] numbers = { 1, 2, 3 };
for (int i = 0; i < numbers.Length; i++) {
    numbers[i] = numbers[i] * 2;  // Modifies
}
// numbers now { 2, 4, 6 }
```

---

## Mistake 11: Complex Break Conditions

❌ Hard to Understand
```csharp
if (condition1 && condition2 || condition3 && !condition4) {
    break;
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

## Mistake 12: Forgetting Braces (Risky)

❌ Dangerous
```csharp
if (condition)
    Statement1();
    Statement2();  // Always executes!
```

✓ Safe
```csharp
if (condition) {
    Statement1();
    Statement2();
}
```

---

## Mistake 13: Using = Instead of ==

❌ Wrong
```csharp
if (x = 5) { }  // Assignment, always true!
```

✓ Correct
```csharp
if (x == 5) { }  // Comparison
```

---

## Mistake 14: Nested Ternary Complexity

❌ Unreadable
```csharp
string result = a ? b ? c : d : e ? f : g;
```

✓ Use Switch Expression
```csharp
string result = condition switch {
    case1 => value1,
    case2 => value2,
    _ => default
};
```

---

## Mistake 15: Not Handling Loop Boundaries

❌ Incomplete
```csharp
for (int i = 1; i < 10; i++) {
    // What about 0 and 10?
}
```

✓ Explicit
```csharp
// Clearly handles 0-9
for (int i = 0; i < 10; i++) {
    // Process
}
```

---

## Quick Checklist

- [ ] Break in all switch cases
- [ ] Check loop boundaries
- [ ] Update loop variables
- [ ] Don't modify collection during iteration
- [ ] Reduce nesting
- [ ] Never use goto
- [ ] Use correct loop type
- [ ] Include default case
- [ ] Use == not =
- [ ] Use braces for safety

---

## Next Steps

- Review [Best Practices](../01-Best-Practices/00-Best-Practices.md)
- Practice with [Interview Questions](../03-Interview-Questions/README.md)
