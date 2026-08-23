# Markdown Standards Guide

This document explains the markdown standards enforced in this repository and how to comply with them.

## 📋 Table of Contents

- [Overview](#overview)
- [Setup Instructions](#setup-instructions)
- [Standards Explained](#standards-explained)
- [Common Issues & Fixes](#common-issues--fixes)
- [Validation Commands](#validation-commands)
- [Troubleshooting](#troubleshooting)

## Overview

The C# Learning Content repository enforces markdown standards through:

1. **Local enforcement**: Pre-commit hooks prevent commits with issues
2. **CI/CD validation**: GitHub Actions checks all PRs
3. **Auto-fixing**: Automated tools can fix most issues
4. **Developer guidance**: Clear error messages and solutions

### Why Standards Matter

- **Consistency**: All docs look and feel the same
- **Readability**: Proper formatting improves comprehension
- **Automation**: Enables automated processing and generation
- **Maintenance**: Easier to update and refactor content
- **Professionalism**: Shows attention to quality

## Setup Instructions

### One-Time Setup

```bash
# Clone the repository
git clone https://github.com/devmohamedsakr-prog/Csharp-Content.git
cd Csharp-Content

# Install dependencies and setup pre-commit hooks
npm install

# Verify setup
npm run validate
```

### What Gets Installed

- **husky**: Git hooks manager
- **markdownlint-cli**: Markdown linter
- **pre-commit hook**: Automatic validation on commit

## Standards Explained

### 1. Line Length (120 characters)

**Rule**: Most lines should be ≤ 120 characters

**Exceptions**:
- URLs (don't break links)
- Code blocks (let code flow naturally)
- Pre-formatted text
- Table cells (acceptable to exceed)

#### ✅ Good

```markdown
## Getting Started

To get started with this project, follow these steps to ensure
everything is set up correctly on your local machine.
```

#### ❌ Bad

```markdown
## Getting Started
To get started with this project, follow these incredibly long steps to ensure everything is set up correctly on your local machine without any issues whatsoever.
```

#### Fix

```bash
npm run lint:md:fix
```

### 2. Headings Format

**Rule**: 
- Headings must have blank line before and after
- Use consistent heading style
- Don't skip heading levels

#### ✅ Good

```markdown
# Main Title

Some content here.

## Subheading

More content.

### Sub-subheading

Even more content.
```

#### ❌ Bad

```markdown
# Main Title
Some content here.
## Subheading
More content.
```

#### Fix

Add blank lines manually or use:

```bash
npm run lint:md:fix
```

### 3. Lists Format

**Rule**: Blank lines required before and after lists

#### ✅ Good

```markdown
Here's a list of items:

- Item 1
- Item 2
- Item 3

More content after list.
```

#### ❌ Bad

```markdown
Here's a list:
- Item 1
- Item 2
More content.
```

#### Fix

```bash
npm run lint:md:fix
```

### 4. Code Blocks Format

**Rule**: 
- Must specify language (e.g., `csharp`)
- Must have blank lines before and after
- Code should be valid for the language specified

#### ✅ Good

```markdown
Here's a code example:

```csharp
public class Example
{
    public void Method() { }
}
```

Next paragraph here.
```

#### ❌ Bad

```markdown
Here's code:
```
public class Example { }
```
Next paragraph.
```

#### Fix

```bash
# Manually edit to add language specifier and blank lines
# Language should match: csharp, bash, json, xml, etc.
```

### 5. Trailing Whitespace

**Rule**: No trailing spaces at end of lines

#### ✅ Good

```markdown
This line has no trailing spaces.
This one either.
```

#### ❌ Bad

```markdown
This line has trailing spaces.   
This one too.  
```

#### Fix

```bash
npm run lint:md:fix
```

### 6. Language-Specific Rules

#### C# Code

```markdown
```csharp
// Always use proper C# syntax
public class MyClass
{
    public void Method() { }
}
```
```

#### JSON

```markdown
```json
{
  "key": "value",
  "nested": {
    "property": "value"
  }
}
```
```

#### XML

```markdown
```xml
<?xml version="1.0"?>
<root>
  <element>Content</element>
</root>
```
```

#### Bash/Shell

```markdown
```bash
npm install
npm run lint:md:fix
```
```

## Common Issues & Fixes

### Issue 1: No Blank Line Before Heading

**Error**: `MD022/blanks-around-headings`

**Problem**:
```markdown
Some content.
## Heading
```

**Fix**:
```markdown
Some content.

## Heading
```

### Issue 2: Code Block Without Language

**Error**: `MD040/fenced-code-language`

**Problem**:
````markdown
```
code here
```
````

**Fix**:
````markdown
```csharp
code here
```
````

### Issue 3: Line Too Long

**Error**: `MD013/line-length`

**Problem**:
```markdown
This is an extremely long line that exceeds one hundred and twenty characters and should be broken into multiple lines for better readability.
```

**Fix**:
```markdown
This is an extremely long line that exceeds one hundred and twenty
characters and should be broken into multiple lines for better
readability.
```

### Issue 4: Missing Blank Line Around List

**Error**: `MD032/blanks-around-lists`

**Problem**:
```markdown
Here's a list:
- Item 1
- Item 2
Next paragraph.
```

**Fix**:
```markdown
Here's a list:

- Item 1
- Item 2

Next paragraph.
```

### Issue 5: Trailing Whitespace

**Error**: Shown in pre-commit output

**Problem**:
```markdown
Line with trailing spaces.   
Another line.  
```

**Fix**:
```bash
npm run lint:md:fix
```

## Validation Commands

### Check All Markdown Files

```bash
# Display all issues
npm run lint:md

# Example output:
# markdown-lint-cli 0.37.0
# README.md: 1: MD022 Headings should be surrounded by blank lines
# 10-Interview-Prep/README.md: 5: MD013 Line too long
```

### Auto-Fix Issues

```bash
# Automatically fix what can be fixed
npm run lint:md:fix

# Common fixes applied:
# - Add blank lines around headings
# - Add blank lines around lists
# - Remove trailing whitespace
# - Fix code block formatting
```

### Check Specific File

```bash
# Check one file
markdownlint -c .markdownlint.json "path/to/file.md"

# Fix one file
markdownlint -c .markdownlint.json --fix "path/to/file.md"
```

### Validate Everything

```bash
npm run validate
```

## Pre-Commit Hook Behavior

### What It Does

When you run `git commit`:

1. ✅ Checks if any `.md` files are being committed
2. ✅ Runs markdownlint on those files
3. ✅ Checks for trailing whitespace
4. ✅ Warns about TODO/FIXME comments
5. ⛔ **Blocks commit** if linting fails

### Bypassing (Not Recommended)

If absolutely necessary, bypass hooks:

```bash
git commit --no-verify
```

**⚠️ Warning**: Only use when you have a good reason. PRs will still fail CI checks.

## GitHub Actions Workflow

### On Pull Request

When you open a PR with markdown changes:

1. ✅ Workflow runs automatically
2. ✅ Checks all modified markdown files
3. 📝 Comments with results
4. ⛔ **Blocks merge** if linting fails

### PR Comment Example

#### ✅ Success

```
✅ All markdown files passed linting checks!
```

#### ❌ Failure

```
## 📝 Markdown Linting Report

```
README.md: 10: MD022 Headings should be surrounded by blank lines
CONTRIBUTING.md: 45: MD013 Line too long
```

**To fix issues locally:**
```bash
npm install
npm run lint:md:fix
```
```

## Troubleshooting

### Problem: Pre-commit Hook Not Running

**Solution**:
```bash
# Reinstall hooks
npm install

# Verify installation
npx husky install

# Test hook manually
npm run lint:md
```

### Problem: markdownlint Not Found

**Solution**:
```bash
# Install globally
npm install -g markdownlint-cli

# Or locally in project
npm install
```

### Problem: Too Many Errors to Fix Manually

**Solution**:
```bash
# Use auto-fix first, then review
npm run lint:md:fix

# Then check what changed
git diff

# Review and commit fixed version
```

### Problem: Can't Commit Despite Fixing

**Solution**:
```bash
# Run validation to see remaining issues
npm run lint:md

# Fix reported issues
npm run lint:md:fix

# Try committing again
git add .
git commit -m "message"
```

### Problem: Rule Seems Too Strict

**Action Items**:

1. Check `.markdownlint.json` configuration
2. Discuss in project issues if rule should change
3. Temporarily bypass with `git commit --no-verify` (not recommended)
4. Help improve guidelines for the community

## Configuration File

The rules are defined in `.markdownlint.json`:

```json
{
  "MD013": {
    "line_length": 120,
    "code_blocks": true
  },
  "MD024": {
    "siblings_only": true
  }
}
```

### Key Settings

- **MD013**: Line length (120 chars)
- **MD022**: Blank lines around headings
- **MD024**: Allow duplicate heading names (in different sections)
- **MD032**: Blank lines around lists
- **MD040**: Code block language required

## Best Practices

### ✅ Do's

- ✅ Run `npm run lint:md:fix` before committing
- ✅ Read error messages carefully
- ✅ Use appropriate code block language
- ✅ Keep lines readable (aim for 80-100 chars when possible)
- ✅ Use proper markdown structure
- ✅ Break long lines naturally (not mid-word)

### ❌ Don'ts

- ❌ Don't use `--no-verify` to bypass hooks
- ❌ Don't skip blank lines around lists
- ❌ Don't commit with linting errors
- ❌ Don't mix heading levels
- ❌ Don't include trailing whitespace
- ❌ Don't use unspecified code blocks

## Resources

- [Markdownlint Documentation](https://github.com/igorshevchenko/markdownlint)
- [Markdown Syntax](https://www.markdownguide.org/)
- [C# Style Guide](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

## Questions?

Check:
1. This document
2. `.markdownlint.json` configuration
3. CONTRIBUTING.md guide
4. GitHub Issues/Discussions
5. Maintainers

---

**Following these standards helps maintain a high-quality, professional repository! Thank you! 🙏**
