<div align="center">

# 🤝 Contributing to C# Learning Content

**Making knowledge accessible, one contribution at a time.**

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-Welcome-brightgreen.svg)](CONTRIBUTING.md)
[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.1-4baadc.svg)](CODE_OF_CONDUCT.md)

</div>

---

## 🎯 Quick Start

```bash
# 1. Fork & clone
git clone https://github.com/YOUR-USERNAME/Csharp-Content.git
cd Csharp-Content

# 2. Setup environment
npm install

# 3. Create feature branch
git checkout -b feature/your-feature-name

# 4. Make changes & validate
npm run lint:md:fix

# 5. Commit & push
git push origin feature/your-feature-name

# 6. Create pull request
```

---

## 📖 Table of Contents

| Section | Purpose |
|---------|---------|
| [🚀 Getting Started](#-getting-started) | Setup instructions |
| [✨ What to Contribute](#-what-to-contribute) | Contribution types |
| [💡 Content Guidelines](#-content-guidelines) | Quality standards |
| [🛠️ Technical Setup](#-technical-setup) | Tools & validation |
| [📝 Commit Standards](#-commit-standards) | Message format |
| [🔄 Pull Request Process](#-pull-request-process) | Submission workflow |
| [❓ FAQ](#-faq) | Common questions |

---

## 🚀 Getting Started

### Requirements

| Tool | Version | Purpose |
|------|---------|---------|
| Git | Latest | Version control |
| Node.js | 16+ | Build tooling |
| NPM | 8+ | Package management |

### Setup Steps

```bash
# Clone your fork
git clone https://github.com/YOUR-USERNAME/Csharp-Content.git
cd Csharp-Content

# Install dependencies
npm install

# Verify everything works
npm run validate
```

### Pre-Commit Hooks (Automatic)

This repository uses **Husky** to enforce quality automatically:

- ✅ Markdown syntax validation
- ✅ Format consistency checks
- ✅ Link verification
- ✅ File organization standards

**No manual setup needed** — hooks activate on `npm install`

---

## ✨ What to Contribute

### ✅ We Welcome

| Type | Examples |
|------|----------|
| **Explanations** | Clear concepts, deeper insights, analogies |
| **Code Examples** | Runnable solutions, patterns, anti-patterns |
| **Interview Q&A** | Questions with comprehensive answers |
| **Fixes** | Typos, corrections, clarifications |
| **Organization** | Better structure, improved navigation |
| **Resources** | Links, references, additional context |

### ❌ We Decline

| Type | Reason |
|------|--------|
| Duplicate content | Keep repo DRY (Don't Repeat Yourself) |
| Promotional/spam | Off-topic commercial content |
| Bad practices | Code violating C# standards |
| Out of scope | Unrelated to C# learning |
| Plagiarized content | Copyright violations |

---

## 💡 Content Guidelines

### ✍️ Writing Standards

#### Explanations

```markdown
✅ Good:
- Clear, progressive complexity
- Practical examples included
- Links to related topics
- Common mistakes highlighted

❌ Avoid:
- Overly technical jargon
- Vague descriptions
- Unsourced claims
```

#### Code Examples

```csharp
✅ Good:
// Clear, compilable code
public class Example
{
    // XML docs for clarity
    /// <summary>Does something important</summary>
    public void DoWork() { }
}

❌ Avoid:
// Pseudo code
// Cryptic variable names
// No documentation
```

#### Interview Questions

```markdown
✅ Structure:
Q: Clear, unambiguous question
A: Comprehensive answer with:
   - Explanation
   - Code example
   - Related concepts
   - Follow-up considerations

❌ Avoid:
Q: Vague/ambiguous wording
A: One-line answer with no depth
```

### 📁 File Organization

```
Topic/
├── 01-Explanation/
│   ├── README.md
│   └── 01-Concept.md
│   └── 02-Advanced.md
│
├── 02-Examples/
│   ├── README.md
│   └── 01-Basic.cs
│   └── 02-Advanced.cs
│
└── 03-Interview-Prep/
    ├── README.md
    └── Questions.md
```

---

## 🛠️ Technical Setup

### Development Commands

```bash
# Lint all markdown files
npm run lint:md

# Auto-fix linting issues
npm run lint:md:fix

# Full validation suite
npm run validate
```

### Markdown Standards

**Configuration:** `.markdownlint.json`

| Rule | Standard | Example |
|------|----------|---------|
| Line Length | 120 chars | Flexible for code/URLs |
| Headings | Blank lines before/after | `## Heading` |
| Lists | Blank lines surrounding | `- Item` |
| Code Blocks | Language specified | ` ```csharp ` |

### Before Every Commit

```bash
# 1. Check formatting
npm run lint:md

# 2. Auto-fix issues
npm run lint:md:fix

# 3. Verify locally
npm run validate

# 4. Then commit
git add .
git commit -m "docs: your message here"
```

---

## 📝 Commit Standards

### Format

```
type(scope): subject

description (optional)

footer (optional)
```

### Types

| Type | Usage | Example |
|------|-------|---------|
| `feat` | New content | `feat(linq): add query operators` |
| `fix` | Corrections | `fix(oop): typo in inheritance` |
| `docs` | Documentation | `docs(readme): update navigation` |
| `refactor` | Reorganization | `refactor: restructure fundamentals` |
| `test` | Examples | `test(async): add task examples` |
| `chore` | Maintenance | `chore: update dependencies` |

### Real Examples

```bash
# Good ✅
git commit -m "feat(async): add Task cancellation patterns with examples"
git commit -m "fix(fundamentals): correct boxing example output"
git commit -m "docs(oob): improve inheritance explanation with diagram"

# Bad ❌
git commit -m "updates"
git commit -m "fixed stuff"
git commit -m "random changes"
```

---

## 🔄 Pull Request Process

### Before Submitting

- [ ] Content is accurate & complete
- [ ] Code examples compile & run
- [ ] Ran `npm run lint:md:fix`
- [ ] Following commit standards
- [ ] Links are working
- [ ] No duplicate content

### Creating PR

**Title:** Clear, descriptive, follows commit format

**Description Template:**

```markdown
## What's Changed
Brief description of changes

## Type
- [ ] New content
- [ ] Fix/correction
- [ ] Organization
- [ ] Other: ___

## Related Issues
Closes #123

## Changes
- Specific change 1
- Specific change 2
- Specific change 3

## Quality Checklist
- [ ] Linting passed locally
- [ ] Content verified
- [ ] Examples tested
- [ ] No duplicates
```

### Review Timeline

| Stage | Expected Time |
|-------|----------------|
| Automated checks | < 2 min |
| Initial review | 24-48 hours |
| Feedback cycle | Variable |
| Merge | Once approved |

---

## ❓ FAQ

**Q: How do I report a bug?**  
A: Open an issue with details on what's broken and how to reproduce it.

**Q: Can I add a new topic area?**  
A: Propose it via issue first. We plan major additions carefully.

**Q: What if my PR is rejected?**  
A: We provide feedback. You can revise and resubmit!

**Q: Do I get credited?**  
A: Yes! Major contributors are recognized in README & release notes.

**Q: Can I translate content?**  
A: Propose it as a separate branch/repo. Discuss with maintainers first.

---

## 📋 Code of Conduct

We're committed to providing a welcoming environment:

- ✅ Be respectful and inclusive
- ✅ Provide constructive feedback
- ✅ Help others learn and grow
- ✅ Report inappropriate behavior

---

## 📄 License

By contributing, you agree your work will be licensed under **MIT License**.

---

## 🙏 Recognition

Contributors are recognized in:

- 👥 **README.md** — Major contributors
- 📋 **Release Notes** — All significant contributions
- 🌟 **GitHub** — Automatic contributor page

---

<div align="center">

### ⭐ Every Contribution Counts

**Your knowledge helps thousands of learners worldwide.**

[🚀 Start Contributing](https://github.com/devmohamedsakr-prog/Csharp-Content/fork) • [📖 Read Docs](#table-of-contents) • [💬 Discuss](https://github.com/devmohamedsakr-prog/Csharp-Content/discussions)

</div>
