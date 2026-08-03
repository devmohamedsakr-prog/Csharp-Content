# Control Flow Interview - Medium Level

## Q1: How would you rewrite nested if as switch?

**Answer**:
```csharp
// Nested if
if (status == "Active") {
    if (type == "Premium") {
        ApplyPremiumBenefit();
    } else {
        ApplyStandardBenefit();
    }
}

// Better: switch or use && 
if (status == "Active" && type == "Premium") {
    ApplyPremiumBenefit();
}
```

---

## Q2: Pattern matching - give an example

**Answer**:
```csharp
public string Categorize(object obj) => obj switch {
    string s => $"String: {s}",
    int i when i > 0 => $"Positive: {i}",
    int i => $"Integer: {i}",
    _ => "Unknown"
};
```

---

## Q3: What's wrong with modifying collection during foreach?

**Answer**:
```csharp
// Bad
foreach (var item in list) {
    if (item > 100) list.Remove(item);
}

// Good
var toRemove = list.Where(x => x > 100).ToList();
foreach (var item in toRemove) {
    list.Remove(item);
}
```

---

## Q4: Optimize nested loops for readability

**Answer**:
```csharp
// Extract method
for (int i = 0; i < rows; i++) {
    ProcessRow(i);
}

private void ProcessRow(int row) {
    for (int col = 0; col < cols; col++) {
        // Process
    }
}
```

---

## Q5: When would continue vs break?

**Answer**:
- **Continue**: Skip current, process next
- **Break**: Exit completely

```csharp
foreach (var item in items) {
    if (!IsValid(item)) continue;  // Skip
    if (item.Total > 10000) break;  // Stop
    Process(item);
}
```

---

## Q6: Design a loop to process with early termination

**Answer**:
```csharp
public bool ProcessRecords(List<Record> records) {
    foreach (var record in records) {
        if (record.IsDeleted) continue;  // Skip
        if (!Process(record)) break;     // Stop on error
    }
    return true;
}
```

---

## Summary

- Use pattern matching for type checks
- Don't modify collection during iteration
- Extract nested loops to methods
- Know break vs continue difference
- Early termination saves resources

---

**Next**: Move to [Hard Questions](../03-Hard/00-Hard-Questions.md)
