# C# Control Flow: Complete Guide

## Overview

Comprehensive guide to control flow statements: if-else, switch, all loop types, and control keywords. Master decision-making and iteration in C#.

## Folder Structure

```
03-Control-Flow/
├── 01-Conditional-Statements/        [Decision making]
│   ├── 01-If-Else/                   [if, if-else, if-else-if]
│   ├── 02-Switch-Statements/         [switch, switch expressions]
│   ├── 03-Pattern-Matching/          [Modern pattern tests]
│   └── README.md
├── 02-Loop-Statements/               [Iteration]
│   ├── 01-For-Loop/                  [for loops]
│   ├── 02-While-Do-While/            [while, do-while]
│   ├── 03-ForEach-Loop/              [foreach, LINQ]
│   └── README.md
├── 03-Control-Keywords/              [Flow control]
│   ├── 01-Break-Continue/            [break, continue]
│   ├── 02-Return-Goto/               [return, goto (avoid)]
│   └── README.md
├── 04-Best-Practices-Interview/      [Guidance & prep]
│   ├── 01-Best-Practices/            [9 guidelines]
│   ├── 02-Common-Mistakes/           [15 mistakes]
│   ├── 03-Interview-Questions/       [18 questions]
│   └── README.md
└── README.md                         [This file]
```

---

## Quick Navigation

### 🔴 Conditional Statements (Decision Making)

Execute code based on conditions.

#### If-Else Statements
- **File**: `01-Conditional-Statements/01-If-Else/00-If-Else-Statements.md`
- **Covers**: if, if-else, if-else-if chains, nested if
- **Key**: Use && instead of nesting, use early return
- **When**: Complex boolean logic

#### Switch Statements
- **File**: `01-Conditional-Statements/02-Switch-Statements/00-Switch-Statements.md`
- **Covers**: switch statements, switch expressions, pattern matching
- **Key**: Don't forget break, use switch expressions for clean returns
- **When**: Single value against many options

#### Pattern Matching
- **File**: `01-Conditional-Statements/03-Pattern-Matching/00-Pattern-Matching.md`
- **Covers**: Type patterns, property patterns, list patterns, relational patterns
- **Key**: Modern, expressive way to test values
- **When**: Complex type checking or value extraction

---

### 🔵 Loop Statements (Iteration)

Execute code multiple times.

#### For Loop
- **File**: `02-Loop-Statements/01-For-Loop/00-For-Loop.md`
- **Covers**: Standard for loop, variations, nested loops
- **Key**: Good for known count, array indexing
- **When**: Need exact iteration count

#### While and Do-While
- **File**: `02-Loop-Statements/02-While-Do-While/00-While-Do-While.md`
- **Covers**: while loop, do-while loop, difference, patterns
- **Key**: while checks before, do-while checks after
- **When**: Condition-based, unknown count

#### Foreach Loop
- **File**: `02-Loop-Statements/03-ForEach-Loop/00-ForEach-Loop.md`
- **Covers**: Array, list, dictionary, LINQ iteration
- **Key**: Simplest for collection iteration
- **When**: Default for iterating collections

---

### ⚙️ Control Keywords (Flow Control)

Control loop and method execution.

#### Break and Continue
- **File**: `03-Control-Keywords/01-Break-Continue/00-Break-Continue.md`
- **Covers**: break (exit), continue (skip)
- **Key**: break exits loop, continue skips iteration
- **When**: Early exit or skip logic

#### Return and Goto
- **File**: `03-Control-Keywords/02-Return-Goto/00-Return-Goto.md`
- **Covers**: return, goto (don't use!), alternatives
- **Key**: return exits method, never use goto
- **When**: Exit method early

---

### 📚 Best Practices and Interview

Guidelines and preparation.

#### Best Practices
- **File**: `04-Best-Practices-Interview/01-Best-Practices/00-Best-Practices.md`
- **Count**: 9 key guidelines
- **Topics**: Choosing right construct, readability, performance

#### Common Mistakes
- **File**: `04-Best-Practices-Interview/02-Common-Mistakes/00-Common-Mistakes.md`
- **Count**: 15 mistakes with solutions
- **Topics**: Real pitfalls and how to avoid them

#### Interview Questions
- **Files**: `04-Best-Practices-Interview/03-Interview-Questions/`
- **Count**: 18 questions (6 Easy, 6 Medium, 6 Hard)
- **Topics**: Real interview scenarios

---

## Decision Guide

### Choosing Conditionals

Need to test ONE VALUE against MANY options?
└─ Yes: Use **switch** or **switch expression**

Need COMPLEX BOOLEAN LOGIC?
└─ Yes: Use **if-else** with && and ||

Need to TEST A TYPE?
└─ Yes: Use **pattern matching** (is check)

---

### Choosing Loops

Need to KNOW EXACT ITERATION COUNT?
└─ Yes: Use **for** loop

Need to ITERATE A COLLECTION?
└─ Yes: Use **foreach** loop

Need CONDITION-BASED ITERATION?
└─ Yes: Use **while** loop

Need AT LEAST ONE EXECUTION?
└─ Yes: Use **do-while** loop

---

## Complete Operator Reference

### Conditionals

| Statement | Purpose | Example |
|-----------|---------|---------|
| if | Single condition | `if (x > 5) { }` |
| if-else | Two paths | `if (x > 5) { } else { }` |
| if-else-if | Multiple paths | `if (x > 5) { } else if (x > 0) { }` |
| switch | Many options | `switch (x) { case 1: break; }` |
| switch expr | Clean returns | `x switch { 1 => "one", _ => "other" }` |
| is pattern | Type check | `if (obj is string s) { }` |

### Loops

| Loop | When | Example |
|------|------|---------|
| for | Known count | `for (int i = 0; i < 10; i++)` |
| while | Unknown count | `while (condition)` |
| do-while | At least once | `do { } while (condition)` |
| foreach | Collections | `foreach (var x in items)` |

### Keywords

| Keyword | Effect |
|---------|--------|
| break | Exit loop/switch |
| continue | Skip to next iteration |
| return | Exit method |
| goto | AVOID - don't use |

---

## Learning Paths

### 🚀 Beginner (1 week)
Master fundamentals of decision-making and iteration.

**Week**:
- If-Else and Switch
- For and Foreach loops
- While loops
- Break and Continue
- Basic patterns

### 🎯 Intermediate (3-4 days)
Deeper understanding, edge cases, patterns.

**Day 1**: Review conditionals, switch expressions
**Day 2**: Advanced loops, nested patterns
**Day 3**: Control keywords, pattern matching
**Day 4**: Best practices, common mistakes

### 🔬 Advanced/Interview (2 days)
Master all concepts, interview preparation.

**Day 1**: Study 15 common mistakes, medium questions
**Day 2**: Practice hard questions, mock interviews

---

## Key Concepts

### If-Else
```csharp
if (condition1) { }
else if (condition2) { }
else { }
```

### Switch Expression
```csharp
result = value switch {
    1 => "one",
    2 => "two",
    _ => "other"
};
```

### For Loop
```csharp
for (int i = 0; i < count; i++) {
    // Execute count times
}
```

### Foreach Loop
```csharp
foreach (var item in collection) {
    // Process each item
}
```

### While Loop
```csharp
while (condition) {
    // Execute while true
}
```

---

## Common Mistakes Summary

**Top 5**:
1. Forgetting break in switch
2. Off-by-one in loops
3. Modifying collection during foreach
4. Deep nesting (use early return)
5. Using goto (never!)

**How to Avoid**:
- Use linters
- Code review
- Know the 15 mistakes
- Test edge cases

---

## Performance Tips

✓ Use foreach for collections (most readable)
✓ Use for for array indexing (efficient)
✓ Use while for conditions (natural)
✓ Avoid deep nesting (extract methods)
✓ Use pattern matching for type tests

---

## Real-World Applications

### Web Application
```csharp
foreach (var user in users) {
    if (!user.IsActive) continue;
    if (user.HasPremium) ApplyBenefit(user);
}
```

### Game Loop
```csharp
while (isRunning) {
    if (IsGameOver()) break;
    UpdateGame();
    RenderFrame();
}
```

### Data Processing
```csharp
for (int i = 0; i < data.Count; i++) {
    switch (data[i].Type) {
        case TypeA: HandleA(data[i]); break;
        case TypeB: HandleB(data[i]); break;
    }
}
```

---

## File Statistics

**Total Files**: 19
**Total Content**: ~18,000 words
**Code Examples**: 150+
**Interview Questions**: 18
**Best Practices**: 9
**Common Mistakes**: 15

---

## Next Steps

1. **Choose a learning path** above
2. **Read** each section carefully
3. **Practice** code examples
4. **Study** best practices
5. **Avoid** common mistakes
6. **Interview**: Practice questions

---

**Last Updated**: August 2026
**Status**: Complete - All control flow categories covered with comprehensive examples and interview preparation

---

## Navigation

- **Conditionals**: `01-Conditional-Statements/README.md`
- **Loops**: `02-Loop-Statements/README.md`
- **Keywords**: `03-Control-Keywords/README.md`
- **Practices**: `04-Best-Practices-Interview/README.md`
- **Parent**: [01-Fundamentals Explanation](../README.md)
