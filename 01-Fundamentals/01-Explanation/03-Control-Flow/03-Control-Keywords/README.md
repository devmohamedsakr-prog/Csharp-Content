# Control Keywords

## Overview

Keywords that control loop and method execution flow.

## Categories

### Break and Continue
Control loop iteration flow.

**Files**: `01-Break-Continue/00-Break-Continue.md`

- Break: Exit loop immediately
- Continue: Skip to next iteration
- In nested loops
- With switch statements

### Return and Goto
Exit methods or jump locations.

**Files**: `02-Return-Goto/00-Return-Goto.md`

- Return: Exit method, optionally return value
- Early exit pattern
- Goto: AVOID (poor practice)
- Alternatives to goto

## Quick Reference

| Keyword | Effect | Use |
|---------|--------|-----|
| break | Exit loop | Loop early termination |
| continue | Skip iteration | Skip current, go next |
| return | Exit method | Exit, optionally return |
| goto | Jump to label | AVOID - don't use |

## Usage Patterns

### Break
```csharp
for (...) {
    if (found) break;  // Exit loop
}
```

### Continue
```csharp
foreach (var item in items) {
    if (!valid) continue;  // Skip, go next
}
```

### Return
```csharp
if (condition) return;  // Exit method early
```

### Never Goto
```csharp
// DON'T do this
goto Label;
// ...
Label:
```

## Best Practices

✓ Use return for early exit
✓ Use break to exit loops
✓ Use continue to skip
✓ Never use goto
✓ Consider return over break

## Common Mistakes

❌ Forgetting break (falls through in switch)
❌ Using goto (creates spaghetti code)
❌ Complex break conditions
❌ Break in nested loop (only exits inner)

## Decision Tree

Need to exit method?
├─ Yes → Use `return`

Need to exit loop?
├─ Yes → Use `break`

Need to skip iteration?
├─ Yes → Use `continue`

---

## Navigation

- **Parent**: [Control Flow](../README.md)
- **Break/Continue**: `01-Break-Continue/00-Break-Continue.md`
- **Return/Goto**: `02-Return-Goto/00-Return-Goto.md`
- **Conditionals**: `../01-Conditional-Statements/README.md`
