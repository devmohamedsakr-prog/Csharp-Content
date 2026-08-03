# Operators: Common Mistakes

## Mistake 1: Confusing = (Assignment) with == (Comparison)

❌ **Wrong**
```csharp
if (x = 5) { }  // Assigns instead of compares
```

✓ **Correct**
```csharp
if (x == 5) { }  // Comparison
```

**Why**: Single = is assignment, == is comparison for equality.

---

## Mistake 2: Integer Division Truncation

❌ **Wrong**
```csharp
int total = 10;
int count = 3;
int average = total / count;  // 3, not 3.333...
```

✓ **Correct**
```csharp
double average = (double)total / count;  // 3.333...
```

**Why**: Integer division truncates. Cast to double for precision.

---

## Mistake 3: String Concatenation in Loops

❌ **Wrong - Very Slow**
```csharp
string result = "";
for (int i = 0; i < 1000; i++) {
    result += i;  // Creates 1000 strings!
}
// O(n²) complexity
```

✓ **Correct**
```csharp
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i);
}
string result = sb.ToString();
// O(n) complexity
```

**Why**: Each += creates new string. StringBuilder reuses buffer.

---

## Mistake 4: Not Checking Null Before Dereferencing

❌ **Wrong**
```csharp
string text = GetText();
if (text.Length > 0) { }  // NullReferenceException if null!
```

✓ **Correct**
```csharp
if (text != null && text.Length > 0) { }
// Null check first
```

✓ **Modern Approach**
```csharp
if (text?.Length > 0) { }
// Null-conditional operator
```

**Why**: Must check for null before accessing members.

---

## Mistake 5: Wrong Operator Precedence

❌ **Wrong**
```csharp
int result = 5 + 3 * 2;  // 11, not 16
// Multiplication happens first!
```

✓ **Correct**
```csharp
int result = (5 + 3) * 2;  // 16
// Parentheses make intent clear
```

**Why**: * and / have higher precedence than + and -.

---

## Mistake 6: Bitwise AND (&) Instead of Logical AND (&&)

❌ **Wrong**
```csharp
if (x & y) { }  // Bitwise AND, evaluates both
// Slower, evaluates both conditions always
```

✓ **Correct**
```csharp
if (x && y) { }  // Logical AND, short-circuits
// Faster, stops if x is false
```

**Why**: && is logical (short-circuits), & is bitwise (always evaluates both).

---

## Mistake 7: Using Float for Money

❌ **Wrong**
```csharp
float total = 0.1f + 0.2f;
if (total == 0.3f) { }  // FALSE! Precision error
```

✓ **Correct**
```csharp
decimal total = 0.1m + 0.2m;
if (total == 0.3m) { }  // TRUE
```

**Why**: float has precision issues. decimal designed for money.

---

## Mistake 8: Forgetting Short-Circuit Evaluation

❌ **Risky**
```csharp
if (list.Count > 0 && list != null) { }
// NullReferenceException if list is null!
```

✓ **Correct**
```csharp
if (list != null && list.Count > 0) { }
// Checks null first, second condition not evaluated if null
```

**Why**: Must check for null before accessing properties.

---

## Mistake 9: Complex Nested Ternary

❌ **Hard to Read**
```csharp
string x = a ? b ? c : d : e ? f : g;
// What does this even do?
```

✓ **Use Switch Expression**
```csharp
string x = condition switch {
    case1 => value1,
    case2 => value2,
    _ => default
};
// Much clearer
```

**Why**: Nested ternary is confusing. Switch expressions are clearer.

---

## Mistake 10: Modifying Collection During Iteration

❌ **Wrong**
```csharp
foreach (var item in list) {
    if (item.Value > 100) {
        list.Remove(item);  // InvalidOperationException!
    }
}
```

✓ **Correct Option 1: Use LINQ**
```csharp
var filtered = list.Where(x => x.Value <= 100).ToList();
```

✓ **Correct Option 2: Iterate Copy**
```csharp
foreach (var item in list.ToList()) {
    if (item.Value > 100) {
        list.Remove(item);  // Safe - iterating copy
    }
}
```

**Why**: Can't modify collection during enumeration.

---

## Mistake 11: Double Negation

❌ **Confusing**
```csharp
bool notNotValid = !!isValid;
// Just use isValid!
```

✓ **Clear**
```csharp
bool isValid = true;
if (isValid) { }
```

**Why**: Double negation is confusing and unnecessary.

---

## Mistake 12: Wrong Comparison for Strings

❌ **Case Sensitivity Issue**
```csharp
string input = "Active";
if (input == "active") { }  // FALSE
// Should be case-insensitive
```

✓ **Correct**
```csharp
if (input.Equals("active", StringComparison.OrdinalIgnoreCase)) { }
// TRUE
```

**Why**: String == is case-sensitive by default.

---

## Mistake 13: Incrementing Multiple Times in Expression

❌ **Undefined Behavior**
```csharp
int x = 5;
int y = ++x + ++x;  // Undefined! Don't do this
```

✓ **Correct**
```csharp
int x = 5;
++x;
++x;
int y = x;  // Clear and defined
```

**Why**: Order of evaluation is undefined for multiple increments.

---

## Mistake 14: Division by Zero

❌ **Crashes**
```csharp
int x = 10;
int result = x / 0;  // DivideByZeroException
```

✓ **Correct**
```csharp
if (divisor != 0) {
    int result = x / divisor;
}
```

✓ **With Nullable**
```csharp
int result = divisor != 0 ? x / divisor : 0;
```

**Why**: Must check for zero before dividing.

---

## Mistake 15: Overflow Not Handled

❌ **Wraps Around (Silent)**
```csharp
int max = int.MaxValue;
int overflow = max + 1;  // Wraps to -2147483648 (no error)
```

✓ **Detect Overflow**
```csharp
try {
    int result = checked(int.MaxValue + 1);
} catch (OverflowException) {
    Console.WriteLine("Overflow!");
}
```

✓ **Use Larger Type**
```csharp
long result = (long)int.MaxValue + 1;  // Safe
```

**Why**: Integer overflow wraps silently unless checked.

---

## Quick Mistake Checklist

- [ ] Used == for comparison, not =
- [ ] Cast to double/decimal for division precision
- [ ] Used StringBuilder for string loops
- [ ] Checked null before dereferencing
- [ ] Used correct operator (&&, not &)
- [ ] Used decimal, not float for money
- [ ] Short-circuit evaluation understood
- [ ] No complex nested ternary
- [ ] Don't modify collections during iteration
- [ ] Avoided double negation
- [ ] String case handling correct
- [ ] No multiple increments in one expression
- [ ] Checked division by zero
- [ ] Handled overflow where needed

---

## Summary

**Most Common Mistakes**:
1. = vs == confusion
2. Integer division truncation
3. String concatenation in loops
4. Missing null checks
5. Operator precedence errors

**Most Critical to Fix**:
1. Null reference exceptions
2. String concatenation performance
3. Type mismatches (float vs decimal)

---

## Next Steps

- Review [Best Practices](../01-Best-Practices/00-Best-Practices.md)
- Practice with [Interview Questions](../03-Interview-Questions/README.md)
