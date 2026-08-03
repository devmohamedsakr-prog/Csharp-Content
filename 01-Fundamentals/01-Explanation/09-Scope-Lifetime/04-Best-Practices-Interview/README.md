# Best Practices, Common Mistakes, and Interview Questions

## Overview

This category consolidates scope and lifetime knowledge into actionable best practices, highlights common pitfalls to avoid, and provides comprehensive interview preparation. Use these resources to reinforce learning and prepare for technical interviews.

## Topics Covered

### 1. Best Practices
**File**: `01-Best-Practices/00-Best-Practices.md`

Learn the 10 essential best practices for working with scope and lifetime in C#.

**Key Practices**:
1. Keep variable scope as narrow as possible
2. Use access modifiers appropriately
3. Distinguish class members from local variables
4. Avoid variable shadowing
5. Manage object lifetime with IDisposable
6. Avoid unintended variable capture
7. Understand stack vs heap memory
8. Use namespaces effectively
9. Prevent memory leaks in event handlers
10. Use modern C# features

**When to Use**: Apply these practices to:
- Write clearer, maintainable code
- Design better APIs
- Prevent common bugs
- Improve code reviews
- Establish team standards

**Example Practice #1: Narrow Scope**
```csharp
// BAD: Wide scope
public int Calculate(int[] numbers)
{
    int total = 0; // Declared at top
    // ... hundreds of lines ...
}

// GOOD: Narrow scope
public int Calculate(int[] numbers)
{
    int total = 0;
    for (int i = 0; i < numbers.Length; i++)
    {
        total += numbers[i];
    }
    return total;
}
```

---

### 2. Common Mistakes
**File**: `02-Common-Mistakes/00-Common-Mistakes.md`

Understand 10 common mistakes and how to avoid them.

**Mistakes Covered**:
1. Accessing variables out of scope
2. Loop variable capture in closures
3. Variable shadowing
4. Not disposing resources
5. Unintended variable capture
6. Forgetting static access modifiers
7. Closure over uninitialized variables
8. Not using protected internal correctly
9. Recursive method stack overflow
10. Event handler memory leaks

**When to Use**: Study these to:
- Recognize patterns in your own code
- Debug similar issues faster
- Prevent mistakes proactively
- Understand why certain practices matter
- Learn from others' experiences

**Example Mistake #2: Loop Closure**
```csharp
// MISTAKE: All closures see final i
for (int i = 0; i < 3; i++)
{
    actions.Add(() => Console.WriteLine(i)); // All print 3!
}

// FIX: Create local copy
for (int i = 0; i < 3; i++)
{
    int copy = i;
    actions.Add(() => Console.WriteLine(copy)); // Prints 0, 1, 2
}
```

---

### 3. Interview Questions
**File**: `03-Interview-Questions/`

15 comprehensive interview questions organized by difficulty level.

#### Easy Questions (Foundation)
- Q1: Explain scope vs lifetime
- Q2: Explain stack vs heap
- Q3: Identify variable shadowing
- Q4: Name access modifiers
- Q5: What is IDisposable?

#### Medium Questions (Core Knowledge)
- Q6: Identify and fix loop closure bug
- Q7: Trace closure variable changes
- Q8: Design cache class with disposal
- Q9: Explain event handler memory leaks
- Q10: Trace multi-method scope scenarios

#### Hard Questions (Expert Level)
- Q11: Design thread-safe factory with closures
- Q12: Analyze memory and performance implications
- Q13: Inherit and override scope rules
- Q14: Design resource pool pattern
- Q15: Real-world complexity analysis

**When to Use**: Use these questions to:
- Prepare for technical interviews
- Self-assess your understanding
- Learn advanced concepts
- Practice explaining concepts
- Identify knowledge gaps

---

## How to Use This Category

### For Learning
1. **Study Best Practices**: Understand the "why" behind each practice
2. **Review Common Mistakes**: See anti-patterns and fixes
3. **Attempt Questions**: Start with Easy, progress to Hard
4. **Implement Fixes**: Apply solutions to your code

### For Interviews
1. **Easy Questions**: Warm-up, build confidence
2. **Medium Questions**: Demonstrate practical knowledge
3. **Hard Questions**: Show expertise and design thinking
4. **Explain Reasoning**: Most important - explain your thinking

### For Code Reviews
1. **Reference Best Practices**: Check against the 10 practices
2. **Watch for Common Mistakes**: Flag when you see them
3. **Suggest Improvements**: Link to specific guidance
4. **Learn from Peers**: Understand why they chose certain approaches

---

## Interview Preparation Strategy

### Week 1: Foundation
- [ ] Read all Best Practices
- [ ] Study Common Mistakes
- [ ] Answer Easy Questions
- [ ] Research concepts you don't understand

### Week 2: Core Knowledge
- [ ] Answer Medium Questions
- [ ] Trace through code examples
- [ ] Implement fixes from mistakes
- [ ] Practice explaining answers

### Week 3: Expert Level
- [ ] Answer Hard Questions
- [ ] Design solutions to complex scenarios
- [ ] Think about trade-offs
- [ ] Prepare real-world examples

### Week 4: Review
- [ ] Review weakest areas
- [ ] Practice explaining answers verbally
- [ ] Time yourself answering questions
- [ ] Mock interview practice

---

## Interview Tips

### During the Interview

1. **Take Your Time**: Think before answering
2. **Show Your Thinking**: Explain your reasoning
3. **Use Examples**: Concrete code examples strengthen answers
4. **Acknowledge Trade-offs**: Discuss pros and cons
5. **Ask Clarifications**: If unsure about the question
6. **Be Honest**: Say "I don't know" rather than guessing
7. **Follow Up**: Ask what else they'd like to know

### Answering Strategies

```csharp
// GOOD ANSWER FORMAT:

// 1. Define the concept
"Scope is where a variable can be accessed in code..."

// 2. Explain why it matters
"Understanding scope helps prevent bugs and write clear code..."

// 3. Provide code example
public void Example()
{
    int x = 5; // Method scope
}

// 4. Discuss implications
"This keeps x isolated to this method, preventing..."

// 5. Mention best practices
"Best practice is to keep scope as narrow as possible..."
```

---

## Quick Reference Tables

### Best Practices Checklist

| Practice | Why Important | When to Apply |
|----------|--------------|--------------|
| Narrow scope | Prevents bugs, clearer code | Declaring variables |
| Access modifiers | Encapsulation, security | Designing classes |
| Distinguish members | Clarity, maintainability | Naming variables |
| Avoid shadowing | Prevents confusion | Nested scopes |
| IDisposable | Resource cleanup | Managing resources |
| Avoid capture | Prevents memory leaks | Using closures |
| Know memory | Performance, reliability | Design decisions |
| Organize namespaces | Code navigation | Project structure |
| Event cleanup | Memory leak prevention | Event subscription |
| Modern features | Cleaner code | C# 8.0+ projects |

### Common Mistakes Prevention

| Mistake | Root Cause | Prevention |
|---------|-----------|-----------|
| Out of scope | Forgot scope ends | Declare near use |
| Loop closure | Capture by reference | Create local copy |
| Shadowing | Same name used | Distinct names |
| No disposal | Forgot cleanup | Use 'using' |
| Unintended capture | Didn't think | Be explicit |
| Static confusion | Mixed concepts | Use class name |
| Uninitialized | Initialize missed | Initialize first |
| Over-expose | Default public | Start restrictive |
| Stack overflow | No base case | Add termination |
| Event leak | Forgot unsubscribe | Track subscriptions |

---

## Real-World Examples

### Example 1: Code Review Comment

```csharp
// Submitted code
public class UserManager
{
    public List<User> users = new(); // ❌ Public field!
    
    public void AddUser(string name)
    {
        int id = users.Count + 1;
        var user = new User { Id = id, Name = name };
        users.Add(user);
    }
}

// Reviewer comment (referencing Best Practice #2):
// "Suggestion: Make 'users' private and expose through a property or method.
// See: Best Practices #2 - Use access modifiers appropriately"

// Fixed version
public class UserManager
{
    private List<User> _users = new();
    
    public IReadOnlyList<User> Users => _users.AsReadOnly();
    
    public void AddUser(string name)
    {
        int id = _users.Count + 1;
        var user = new User { Id = id, Name = name };
        _users.Add(user);
    }
}
```

### Example 2: Bug Investigation

```csharp
// Bug report: "Loop actions all do the same thing"

var actions = new List<Action>();
for (int i = 0; i < 3; i++)
{
    actions.Add(() => Console.WriteLine($"Item {i}")); // BUG
}

// Investigation (referencing Common Mistake #2):
// Root cause: Loop variable closure
// Solution: Create local copy

var actions = new List<Action>();
for (int i = 0; i < 3; i++)
{
    int copy = i; // FIX
    actions.Add(() => Console.WriteLine($"Item {copy}"));
}
```

### Example 3: Interview Question

```csharp
// Q: "What's wrong with this code?"

public void ProcessData(string[] items)
{
    foreach (var action in CreateActions(items.Length))
    {
        action();
    }
}

private List<Action> CreateActions(int count)
{
    var actions = new List<Action>();
    
    for (int i = 0; i < count; i++)
    {
        actions.Add(() => Console.WriteLine(i)); // Loop closure bug!
    }
    
    return actions;
}

// Answer structure:
// 1. Identify issue: Loop variable closure (Common Mistake #2)
// 2. Explain problem: All actions see final value of i
// 3. Show fix: Create local copy or use foreach/LINQ
// 4. Discuss implications: Why this matters for correctness
```

---

## Self-Assessment Rubric

### After Completing This Category

**Excellent (90-100%)**
- [ ] Explain all best practices with confidence
- [ ] Identify common mistakes immediately
- [ ] Answer all interview questions thoroughly
- [ ] Apply practices to your own code
- [ ] Help others understand concepts

**Good (80-90%)**
- [ ] Understand most best practices
- [ ] Recognize most common mistakes
- [ ] Answer medium difficulty questions
- [ ] Apply some practices regularly
- [ ] Explain concepts with some prompting

**Passing (70-80%)**
- [ ] Know basic best practices
- [ ] Aware of common mistakes
- [ ] Answer easy questions
- [ ] Apply some practices inconsistently
- [ ] Need reference material for explanations

**Needs Work (<70%)**
- [ ] Struggle with best practices
- [ ] Frequently make common mistakes
- [ ] Can't answer interview questions
- [ ] Don't apply practices
- [ ] Need significant study

---

## Recommended Resources

### Books
- "Clean Code" by Robert C. Martin - Code quality and design
- "Effective C#" by Bill Wagner - C# best practices
- "C# in Depth" by Jon Skeet - Advanced C# concepts

### Online Resources
- Microsoft Docs: C# Programming Guide
- Microsoft Learn: C# fundamentals
- Stack Overflow: Scope and closure questions

### Tools
- Visual Studio Analyzer - Code analysis
- Roslyn analyzers - Scope and lifetime checks
- Memory profilers - Understand allocations

---

## Summary

This category provides everything needed to master scope and lifetime in C#:

- **Best Practices**: 10 proven principles
- **Common Mistakes**: 10 pitfalls to avoid
- **Interview Questions**: 15 progressive challenges

By studying these materials, you'll:
1. Write better C# code
2. Debug scope-related issues faster
3. Pass technical interviews with confidence
4. Understand design trade-offs
5. Mentor others effectively

---

## Next Steps

### Immediate
1. Choose your current level (Easy/Medium/Hard)
2. Study Best Practices for your level
3. Review Common Mistakes
4. Attempt interview questions

### Short Term (1-2 weeks)
1. Apply best practices to current projects
2. Review team code for common mistakes
3. Answer all interview questions
4. Practice explaining answers

### Long Term
1. Reference these materials in code reviews
2. Help team members learn
3. Contribute to coding standards
4. Continue learning advanced concepts

---

## Final Thoughts

Scope and lifetime mastery doesn't happen overnight. It requires:
- **Consistent practice**: Apply concepts regularly
- **Active learning**: Don't just read, code it
- **Reflection**: Think about why things work
- **Teaching**: Explain to others
- **Growth mindset**: Mistakes are learning opportunities

You now have the knowledge. The next step is practice. Start with your current projects and watch your code quality improve.

**Good luck! 🚀**
