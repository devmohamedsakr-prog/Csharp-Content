# Null Patterns

## Overview
Practical patterns for handling null: guard clauses, pattern matching, and real-world scenarios.

## Files

1. **00-Guard-Clauses.md** - Early validation and returns, fail fast
2. **00-Pattern-Matching.md** - is null, is not null, switch expressions
3. **00-Real-World-Scenarios.md** - Database, API, configuration, user input

## Quick Reference

```csharp
// Guard clause
ArgumentNullException.ThrowIfNull(data);

// Pattern matching
if (value is not null) { }

// Real-world
string email = response?.Email ?? "default@example.com";
```

## Key Patterns

✓ Guard clauses for validation
✓ Pattern matching for clarity
✓ Early returns for simplicity
✓ Database null handling
✓ API response handling

---

[← Back to Main](../README.md) | [Next: Best Practices →](../04-Best-Practices-Interview/README.md)
