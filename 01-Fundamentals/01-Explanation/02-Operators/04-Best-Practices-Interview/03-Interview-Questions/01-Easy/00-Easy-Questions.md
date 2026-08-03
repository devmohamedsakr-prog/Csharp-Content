# Operators Interview - Easy Level

## Q1: What's the difference between = and ==?

**Answer**:
- `=` is **assignment** (assigns value to variable)
- `==` is **comparison** (checks if values are equal)

```csharp
int x = 5;        // Assignment
if (x == 5) { }   // Comparison
```

---

## Q2: What does this return? `10 / 3`

**Answer**: 
- Returns `3` (integer division truncates)
- Integer divided by integer = integer
- To get `3.333...` cast to double: `(double)10 / 3`

---

## Q3: Explain short-circuit evaluation

**Answer**:
- With `&&`: If first is false, second not evaluated
- With `||`: If first is true, second not evaluated

```csharp
if (list != null && list.Count > 0) {
    // Second only checked if first is true
}
```

**Benefit**: Performance and prevents errors

---

## Q4: Which has higher precedence: * or +?

**Answer**: 
`*` (multiplication) has higher precedence

```csharp
int x = 5 + 3 * 2;  // 11 (multiply first)
int y = (5 + 3) * 2;  // 16 (parentheses override)
```

---

## Q5: What is `5 & 3` in C#?

**Answer**: 
- Bitwise AND operation
- `5` = `0101`, `3` = `0011`
- Result: `0001` = `1`

```csharp
int result = 5 & 3;  // 1
```

---

## Q6: What does null-coalescing (??) do?

**Answer**: 
Returns left value if not null, else right value

```csharp
string name = null;
string result = name ?? "Unknown";  // "Unknown"
```

---

## Q7: What does the ternary operator do?

**Answer**: 
Shorthand if-else returning one of two values

```csharp
string status = age >= 18 ? "Adult" : "Minor";
```

---

## Q8: What's the difference between & and &&?

**Answer**:
- `&` = bitwise AND (always evaluates both)
- `&&` = logical AND (short-circuits)

```csharp
if (a & b) { }   // Both always evaluated
if (a && b) { }  // Second skipped if a is false
```

---

## Summary

- Know = vs ==
- Understand integer division
- Know short-circuit evaluation
- Remember operator precedence
- Distinguish bitwise vs logical operators
- Know null-coalescing
- Understand ternary operator

---

**Next**: Move to [Medium Questions](../02-Medium/00-Medium-Questions.md)
