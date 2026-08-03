# Exception Fundamentals

## Overview
This section covers the foundational concepts of exception handling in C#. Understand what exceptions are, why they're important, and how they work.

## Learning Path

### 1. What is an Exception? (Start here)
- Definition and purpose
- Benefits of exception handling
- Exception vs error
- Basic try-catch example

**Time:** 10-15 minutes

### 2. Common Exceptions
- FormatException (parsing failures)
- ArgumentException family
- NullReferenceException
- IndexOutOfRangeException
- IOException family
- Prevention strategies

**Time:** 20-25 minutes

### 3. Exception Hierarchy
- System.Exception base class
- SystemException vs ApplicationException
- Predefined exception hierarchy
- Catching by type

**Time:** 15-20 minutes

## Quick Reference

### Exception Handling Basics
```csharp
try {
    // Code that might throw
} catch (SpecificException ex) {
    // Handle specific exception
} catch (GeneralException ex) {
    // Handle more general
} finally {
    // Cleanup - always runs
}
```

### Common Exception Types
| Type | Cause |
|------|-------|
| FormatException | Invalid parse format |
| ArgumentNullException | Null argument |
| IndexOutOfRangeException | Array index out of bounds |
| DivideByZeroException | Division by zero |
| InvalidOperationException | Invalid state |

### Prevention Patterns
```csharp
// Use guard clauses
if (value == null) throw new ArgumentNullException(nameof(value));

// Use TryParse for input
if (int.TryParse(input, out int number)) { }

// Check before operations
if (list.Count > 0) { var first = list[0]; }
```

## Key Concepts

- **Exception** - Object representing error condition
- **Try block** - Code that might throw
- **Catch block** - Handles specific exception type
- **Finally block** - Guaranteed cleanup
- **Exception hierarchy** - Type-based handling

## Common Mistakes

❌ Catching too broadly
```csharp
try { } catch (Exception) { }  // Too general
```

✓ Catch specific exceptions first
```csharp
try { } catch (ArgumentNullException) { }
catch (ArgumentException) { }
catch (Exception) { }
```

## Study Tips

1. **Understand the why** - Why exceptions exist
2. **Practice** - Write try-catch examples
3. **Test** - Trigger different exception types
4. **Hierarchy** - Understand inheritance
5. **Prevention** - Use guard clauses first

## Next Steps

1. ✓ Read "What is an Exception?"
2. ✓ Study "Common Exceptions"
3. ✓ Learn "Exception Hierarchy"
4. → Move to "Exception Handling" section
5. → Study exception patterns

## Self-Assessment

Can you:
- [ ] Explain what an exception is?
- [ ] Write basic try-catch blocks?
- [ ] Identify common exception types?
- [ ] Understand exception hierarchy?
- [ ] Write guard clauses?
- [ ] Prevent common exceptions?
- [ ] Explain why exceptions matter?
- [ ] Order catch blocks correctly?

---

## Files in This Section

1. **00-What-Is-Exception.md** - Exception basics and benefits
2. **00-Common-Exceptions.md** - Built-in exception types and prevention
3. **00-Exception-Hierarchy.md** - Exception type hierarchy and catching strategies

---

## Related Sections

- **Exception Handling** - Try-catch patterns and flow
- **Exception Management** - Throwing and custom exceptions
- **Resource Management** - Using statements and disposal
- **Best Practices** - Recommended patterns
- **Common Mistakes** - Things to avoid
- **Interview Questions** - Test your knowledge
