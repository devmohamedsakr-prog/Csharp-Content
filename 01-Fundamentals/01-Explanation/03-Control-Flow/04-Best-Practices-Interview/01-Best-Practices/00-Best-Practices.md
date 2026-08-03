# Control Flow: Best Practices

## If-Else Statements

✓ Use && instead of nested if
```csharp
if (age >= 18 && hasLicense) {
    Console.WriteLine("Can drive");
}
```

✓ Early return to reduce nesting
```csharp
if (user == null) return;
if (!user.IsActive) return;
Process(user);
```

✓ Avoid deeply nested conditions
```csharp
// Extract to method
if (IsEligible(user)) {
    Process();
}
```

---

## Switch Statements

✓ Use switch expressions for simple returns
```csharp
string result = day switch {
    "Monday" => "Work",
    "Saturday" or "Sunday" => "Rest",
    _ => "Other"
};
```

✓ Include default case
```csharp
switch (value) {
    case 1:
        // Handle 1
        break;
    default:
        // Handle unexpected
        break;
}
```

✓ Don't forget break
```csharp
case "A":
    Process();
    break;  // Prevents fallthrough
```

---

## Loops

✓ Use foreach for collections
```csharp
foreach (var item in items) {
    Console.WriteLine(item);
}
```

✓ Use for when you need index
```csharp
for (int i = 0; i < items.Count; i++) {
    items[i] = Process(items[i]);
}
```

✓ Use while for condition-based
```csharp
while (reader.Read()) {
    ProcessRecord();
}
```

✓ Use meaningful variable names
```csharp
foreach (var student in students) {
    Console.WriteLine(student.Name);
}
```

✓ Avoid deep nesting (extract methods)
```csharp
for (int i = 0; i < 10; i++) {
    ProcessRow(i);
}
```

---

## Control Keywords

✓ Use return for early exit
```csharp
if (!condition) return;
```

✓ Use break to exit loop
```csharp
if (found) break;
```

✓ Use continue to skip
```csharp
if (!IsValid(item)) continue;
```

✓ Never use goto
```csharp
// Don't use goto - use break/return instead
```

---

## Performance

✓ Use appropriate loop type
```csharp
// Fast: foreach (no indexing overhead)
foreach (var item in items) { }

// Still fast but unnecessary indexing
for (int i = 0; i < items.Count; i++) { }
```

✓ Avoid modifying collections during iteration
```csharp
var toRemove = items.Where(x => x > 100).ToList();
foreach (var item in toRemove) {
    items.Remove(item);
}
```

---

## Readability

✓ Keep conditions simple
```csharp
if (age >= 18 && hasLicense) { }
```

✓ Use descriptive conditions
```csharp
bool isAdult = age >= 18;
bool isQualified = score > 80;

if (isAdult && isQualified) {
    Process();
}
```

✓ Avoid complex nesting
```csharp
// Extract method
if (MeetsRequirements(user)) {
    Process(user);
}
```

---

## Summary Checklist

- [ ] Use early returns to reduce nesting
- [ ] Use foreach for collections
- [ ] Use switch expressions for simple returns
- [ ] Include break in switch cases
- [ ] Never use goto
- [ ] Use meaningful variable names
- [ ] Keep conditions simple
- [ ] Avoid modifying collections during iteration
- [ ] Extract complex logic to methods

---

## Next Steps

- Review [Common Mistakes](../02-Common-Mistakes/00-Common-Mistakes.md)
- Practice with [Interview Questions](../03-Interview-Questions/README.md)
