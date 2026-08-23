# Contributing to C# Learning Content

Thank you for your interest in contributing! This document provides guidelines and instructions for contributing to the C# Learning Content repository.

## 📋 Table of Contents

- [Getting Started](#getting-started)
- [Code of Conduct](#code-of-conduct)
- [Development Setup](#development-setup)
- [Contribution Guidelines](#contribution-guidelines)
- [Markdown Standards](#markdown-standards)
- [Pull Request Process](#pull-request-process)
- [Commit Message Format](#commit-message-format)

## Getting Started

1. **Fork the repository** on GitHub
2. **Clone your fork** locally
3. **Create a new branch** for your contribution
4. **Make your changes** following the guidelines below
5. **Submit a pull request** with a clear description

## Code of Conduct

- Be respectful and inclusive
- Help others learn and grow
- Provide constructive feedback
- Report inappropriate behavior to maintainers

## Development Setup

### Prerequisites

- Git
- Node.js (v16+)
- NPM

### Initial Setup

```bash
# Clone your fork
git clone https://github.com/YOUR-USERNAME/Csharp-Content.git
cd Csharp-Content

# Install dependencies and setup hooks
npm install

# Verify setup
npm run validate
```

### Pre-commit Hooks

This repository uses Husky to enforce standards automatically:

- **Markdown linting**: All `.md` files are checked before commit
- **Trailing whitespace**: Checked and reported
- **TODO/FIXME**: Warnings for unfinished work

### Local Validation

Run these commands before committing:

```bash
# Check all markdown files
npm run lint:md

# Fix markdown issues automatically
npm run lint:md:fix

# Run full validation
npm run validate
```

## Contribution Guidelines

### What to Contribute

✅ **Welcome contributions:**
- New explanations or clarifications
- Code examples and working solutions
- Interview questions and answers
- Bug fixes and corrections
- Improvements to existing content
- Better organization or structure
- Documentation updates

❌ **Avoid:**
- Duplicate content already in the repo
- Promotional or commercial content
- Code that violates C# best practices
- Content outside the scope (off-topic)

### Content Quality Standards

#### Explanations

- Clear and concise language
- Appropriate for the target audience
- Include practical examples when helpful
- Link to related topics and resources
- Highlight common mistakes

#### Code Examples

- **Must compile and run** without errors
- Include comments explaining key parts
- Follow C# naming conventions (PascalCase for public members)
- Use meaningful variable names
- Show both good and anti-patterns when applicable
- Include XML documentation comments for complex code

#### Interview Questions

- Clear, unambiguous wording
- Appropriate difficulty level
- Comprehensive and accurate answers
- Include practical examples or code samples
- Reference relevant concepts

### File Organization

```
Topic/
├── 01-Explanation/
│   ├── README.md
│   └── *.md (topic-specific explanations)
├── 02-examples/
│   ├── README.md
│   └── *.cs (C# code examples)
└── 03-Interview-Questions/
    ├── README.md
    └── *.md (Q&A content)
```

## Markdown Standards

### Configuration

We use `.markdownlint.json` to enforce consistent markdown. Key rules:

- **Line length**: 120 characters (flexible for code and URLs)
- **Headings**: Consistent style, surrounded by blank lines
- **Lists**: Blank lines before and after
- **Code blocks**: Must have language specified (e.g., ```csharp)
- **Lists & headings**: Must have blank line separation

### Quick Reference

#### ✅ Good Markdown

```markdown
## Main Heading

Explanation paragraph here.

### Subheading

- List item 1
- List item 2

#### Code Example

```csharp
// Your C# code here
public class Example { }
```

#### Another Subheading

More content.
```

#### ❌ Bad Markdown

```markdown
## Main Heading
Explanation without blank line
### Subheading
- Item 1
- Item 2
```

### Common Issues & Fixes

| Issue | Fix |
|-------|-----|
| Missing blank line before heading | Add blank line above `##` |
| List not separated | Add blank line before/after list |
| Code block without language | Add language: ` ```csharp ` |
| Line too long | Break into multiple lines or adjust wrapping |
| Trailing whitespace | Run `npm run lint:md:fix` |

## Pull Request Process

### Before Submitting

1. **Create feature branch** from `main`
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes** following guidelines

3. **Validate locally**
   ```bash
   npm run lint:md
   npm run lint:md:fix  # Auto-fix what you can
   ```

4. **Commit with clear message** (see format below)
   ```bash
   git add .
   git commit -m "docs: add new topic explanation"
   ```

### Submitting PR

1. **Push to your fork**
   ```bash
   git push origin feature/your-feature-name
   ```

2. **Create Pull Request** with:
   - Clear title describing the change
   - Description of what you added/changed
   - Reference any related issues (#123)
   - Screenshots if visual changes

3. **Wait for checks to pass**:
   - ✅ Markdown linting
   - ✅ CI/CD validation
   - ✅ Automated reviews

4. **Address feedback** if requested

5. **Merge** - maintainer will merge once approved

### PR Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Documentation update
- [ ] New explanation
- [ ] New examples
- [ ] New interview questions
- [ ] Bug fix
- [ ] Other

## Related Issues
Closes #123

## Changes Made
- Specific change 1
- Specific change 2

## Testing
How to verify the changes (if applicable)

## Checklist
- [ ] Followed contribution guidelines
- [ ] Ran `npm run lint:md:fix`
- [ ] Content is accurate and complete
- [ ] Code examples compile and run
- [ ] Links are working
```

## Commit Message Format

Follow conventional commit format:

```
type(scope): subject

body (optional)

footer (optional)
```

### Types

- **feat**: New content (explanation, example, question)
- **fix**: Bug fix or correction
- **docs**: Documentation updates
- **refactor**: Reorganize/restructure content
- **test**: Add tests or examples
- **chore**: Maintenance, dependencies

### Examples

```bash
# Good commits
git commit -m "feat(linq): add examples for LINQ queries"
git commit -m "fix(fundamentals): correct syntax error in data types"
git commit -m "docs(oob): improve inheritance explanation"
git commit -m "refactor: reorganize testing folder structure"
```

## Questions or Issues?

- **Found a bug?** Open an issue with details
- **Have a suggestion?** Create a discussion or issue
- **Need help?** Check existing issues or discussions first
- **Contact maintainers** for other concerns

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

## Recognition

Contributors will be recognized in:
- README.md (for significant contributions)
- Release notes
- GitHub contributors page

---

**Thank you for contributing! 🙏 Every contribution helps make this learning resource better for everyone.**
