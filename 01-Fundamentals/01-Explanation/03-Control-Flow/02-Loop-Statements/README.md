# Loop Statements

## Overview

Loops execute code repeatedly. Choose based on known count, condition, or collection.

## Categories

### For Loop
Execute code a specific number of times.

**Files**: `01-For-Loop/00-For-Loop.md`

- Standard for loop
- Loop variations (backward, step)
- Nested for loops
- Infinite loops with break

### While and Do-While
Execute code while condition is true.

**Files**: `02-While-Do-While/00-While-Do-While.md`

- Standard while loop
- Do-while (at least one execution)
- Infinite loop with break
- User input validation

### Foreach Loop
Iterate through collections.

**Files**: `03-ForEach-Loop/00-ForEach-Loop.md`

- Array iteration
- List iteration
- Dictionary iteration
- LINQ results

## Quick Reference

| Loop | When | Syntax |
|------|------|--------|
| For | Know count | `for (init; cond; inc)` |
| While | Unknown count | `while (condition)` |
| Do-While | At least once | `do { } while (condition)` |
| Foreach | Iterate items | `foreach (var x in collection)` |

## Best Practices

✓ Use foreach for collections
✓ Use for when you need index
✓ Use while for conditions
✓ Avoid deep nesting
✓ Extract complex logic

## Common Mistakes

❌ Off-by-one errors
❌ Infinite loops
❌ Modifying collection during iteration
❌ Wrong loop type
❌ Deep nesting

## Loop Anatomy

```
for (initialize; condition; increment)
├─ Initialize: runs once
├─ Condition: checked before each iteration
└─ Increment: runs after each iteration
```

## Performance

- Foreach: Most readable, efficient for collections
- For: Good for arrays with index access
- While: Best for condition-based loops

---

## Navigation

- **Parent**: [Control Flow](../README.md)
- **For Loop**: `01-For-Loop/00-For-Loop.md`
- **While/Do-While**: `02-While-Do-While/00-While-Do-While.md`
- **Foreach Loop**: `03-ForEach-Loop/00-ForEach-Loop.md`
- **Keywords**: `../03-Control-Keywords/README.md`
