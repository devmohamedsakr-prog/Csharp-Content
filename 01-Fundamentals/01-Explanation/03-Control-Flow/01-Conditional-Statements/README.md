# Conditional Statements

## Overview

Conditional statements execute code based on boolean conditions. They're the foundation of decision-making.

## Categories

### If-Else Statements
Simple to complex conditions with branching logic.

**Files**: `01-If-Else/00-If-Else-Statements.md`

- Simple if, if-else, if-else-if chains
- Nested if (avoid with && instead)
- Early return pattern

### Switch Statements
Select one path from many based on a value.

**Files**: `02-Switch-Statements/00-Switch-Statements.md`

- Traditional switch with break
- Switch expressions (C# 8+)
- Pattern matching in switch
- Fall-through behavior

### Pattern Matching
Modern way to test values and extract information.

**Files**: `03-Pattern-Matching/00-Pattern-Matching.md`

- Type patterns
- Property patterns (C# 8+)
- List patterns (C# 9+)
- Relational patterns
- When guard clauses

## Quick Reference

| Pattern | Use |
|---------|-----|
| `if (condition) { }` | Single condition |
| `if-else` | Two paths |
| `if-else-if` | Multiple paths |
| `switch` | Many cases for one value |
| `is` pattern | Type checking |
| `is { }` property | Property matching |

## Best Practices

✓ Use if for complex conditions
✓ Use switch for many options
✓ Use switch expressions for simple returns
✓ Use pattern matching for type tests
✓ Keep conditions readable

## Common Mistakes

❌ Forgetting break in switch
❌ Deep nesting (use early return)
❌ Assignment (=) instead of comparison (==)
❌ Missing default case
❌ Complex conditions

## Learning Path

1. Master If-Else
2. Learn Switch statements
3. Explore Pattern Matching
4. Combine for complex logic

---

## Navigation

- **Parent**: [Control Flow](../README.md)
- **If-Else**: `01-If-Else/00-If-Else-Statements.md`
- **Switch**: `02-Switch-Statements/00-Switch-Statements.md`
- **Pattern Matching**: `03-Pattern-Matching/00-Pattern-Matching.md`
- **Loops**: `../02-Loop-Statements/README.md`
