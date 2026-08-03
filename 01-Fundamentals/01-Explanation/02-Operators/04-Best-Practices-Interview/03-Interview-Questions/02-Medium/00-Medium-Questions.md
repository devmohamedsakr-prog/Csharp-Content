# Operators Interview - Medium Level

## Q1: What's the output? `int x = 5; int y = ++x + x++;`

**Answer**:
- `++x` makes x = 6, uses 6
- `x++` uses 6, makes x = 7
- `y = 6 + 6 = 12`

But this involves undefined behavior. Avoid!

---

## Q2: Design a permission system using bitwise operators

**Answer**:
```csharp
[Flags]
public enum Permissions {
    Read = 1 << 0,     // 1
    Write = 1 << 1,    // 2
    Delete = 1 << 2    // 4
}

// Check
if ((userPerms & Permissions.Read) != 0) { }

// Grant
userPerms |= Permissions.Write;

// Revoke
userPerms &= ~Permissions.Delete;
```

---

## Q3: What's `5 << 2`?

**Answer**:
- Left shift by 2 = multiply by 2²
- `5 << 2 = 20`
- `5 * 4 = 20`

Left shift by n = multiply by 2^n

---

## Q4: Performance: `++i` vs `i++` in loop?

**Answer**:
In modern compilers, usually same after optimization.

```csharp
for (int i = 0; i < 1000; ++i) { }
for (int i = 0; i < 1000; i++) { }
// Both same speed
```

But with objects, ++i slightly faster (no temporary).

---

## Q5: Why use `??` instead of `||`?

**Answer**:
```csharp
string x = null;
string a = x || "default";    // Can't: need bool
string b = x ?? "default";    // Correct

bool y = false;
bool c = y ?? true;           // Works but confusing
bool d = y || true;           // Correct
```

Use `??` for null coalescing, `||` for boolean OR

---

## Q6: What's operator precedence in `a || b && c`?

**Answer**:
- `&&` has higher precedence
- Evaluates as: `a || (b && c)`
- Use parentheses to clarify: `(a || b) && c`

---

## Q7: Create safe null-aware calculation

**Answer**:
```csharp
Person person = null;

// Multiple null-conditionals
string city = person?.Address?.City ?? "Unknown";

// Safe method calling
int? age = person?.GetAge() ?? 0;

// Dictionary lookup
int? value = dictionary?["key"] ?? 0;
```

---

## Summary

- Understand increment/decrement in expressions
- Know bitwise permission systems
- Shift operators for power-of-2 multiplication/division
- Precedence: && before ||
- Use ?? for null coalescing, || for boolean
- Combine null-conditional (?.) with null-coalescing (??)

---

**Next**: Move to [Hard Questions](../03-Hard/00-Hard-Questions.md)
