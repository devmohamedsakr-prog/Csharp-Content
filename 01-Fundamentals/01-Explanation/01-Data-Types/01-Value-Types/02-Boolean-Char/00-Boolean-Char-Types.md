# Boolean and Char Types in C#

## Boolean Type

### Overview
The `bool` type represents a logical value: **true** or **false**.

### Characteristics
```csharp
bool isActive = true;
bool isComplete = false;

// Size: 1 byte
// Values: true or false only
// Default value: false
// Stored on: Stack (value type)
```

### Basic Usage

#### In Conditionals
```csharp
bool isLoggedIn = true;

if (isLoggedIn) {
    Console.WriteLine("User is logged in");
}

if (!isLoggedIn) {
    Console.WriteLine("User is NOT logged in");
}
```

#### In Loops
```csharp
bool isRunning = true;
int count = 0;

while (isRunning) {
    count++;
    if (count > 10) {
        isRunning = false;  // Exit loop
    }
}
```

#### Ternary Operator
```csharp
bool isPassed = score >= 60;
string result = isPassed ? "Pass" : "Fail";
```

### Logical Operations

#### AND (&&)
```csharp
bool hasLicense = true;
bool passedTest = true;
bool canDrive = hasLicense && passedTest;  // true

// Short-circuit: stops evaluating if first is false
if (IsUser() && IsAdmin()) {  // Won't call IsAdmin() if IsUser() is false
    // ...
}
```

#### OR (||)
```csharp
bool isWeekend = false;
bool isHoliday = true;
bool isOffDay = isWeekend || isHoliday;  // true

// Short-circuit: stops evaluating if first is true
if (IsError() || IsWarning()) {  // Won't call IsWarning() if IsError() is true
    // ...
}
```

#### NOT (!)
```csharp
bool isActive = true;
bool isInactive = !isActive;  // false
```

#### XOR (^)
```csharp
bool a = true;
bool b = false;
bool xor = a ^ b;  // true (different values)

bool c = true;
bool d = true;
bool xor2 = c ^ d;  // false (same values)
```

### Practical Examples

#### Validation Flags
```csharp
public class Form {
    private bool isValidated;
    private bool hasErrors;
    private bool canSubmit;
    
    public void Validate() {
        isValidated = true;
        hasErrors = CheckForErrors();
        canSubmit = isValidated && !hasErrors;
    }
}
```

#### State Management
```csharp
public class GameState {
    public bool IsGameRunning { get; set; }
    public bool IsPaused { get; set; }
    public bool IsGameOver { get; set; }
    
    public void Update() {
        if (IsGameRunning && !IsPaused) {
            // Game logic
        }
    }
}
```

#### Conditional Configuration
```csharp
bool isDevelopment = Environment.GetEnvironmentVariable("ENV") == "DEV";
bool enableLogging = isDevelopment || Environment.GetEnvironmentVariable("LOG") == "true";
bool verboseOutput = isDevelopment && enableLogging;
```

### Common Boolean Mistakes

❌ **Comparing to true/false**
```csharp
bool isActive = true;
if (isActive == true) {  // Redundant
    // ...
}
```

✓ **Direct boolean check**
```csharp
if (isActive) {  // Clean and simple
    // ...
}
```

❌ **Ternary for boolean**
```csharp
bool result = condition ? true : false;  // Verbose
```

✓ **Direct assignment**
```csharp
bool result = condition;  // Simple
```

❌ **Double negation**
```csharp
bool isNotInvalid = !(!isValid);  // Confusing
```

✓ **Use positive variable names**
```csharp
bool isValid = true;  // Clear
```

---

## Char Type

### Overview
The `char` type represents a **single Unicode character**.

### Characteristics
```csharp
char letter = 'A';
char digit = '5';
char symbol = '@';

// Size: 2 bytes (Unicode character)
// Range: U+0000 to U+FFFF (65,536 possible values)
// Default value: '\0' (null character)
// Stored on: Stack (value type)
```

### Character Literals

#### Basic Characters
```csharp
char letter = 'A';
char lowercase = 'a';
char digit = '5';
char symbol = '!';
char space = ' ';
```

#### Escape Sequences
```csharp
char newline = '\n';        // Line break
char tab = '\t';            // Tab character
char backslash = '\\';      // Backslash
char quote = '\'';          // Single quote
char doubleQuote = '\"';    // Double quote
char nullChar = '\0';       // Null character
char carriage = '\r';       // Carriage return
char backspace = '\b';      // Backspace
```

#### Unicode Escape
```csharp
char greekAlpha = '\u03B1';  // α (Greek alpha)
char chineseChar = '\u4E2D'; // 中 (Chinese character)
char emoji = '\uD83D\uDE00'; // 😀 (emoji, requires surrogate pair)
```

### Common char Operations

#### Checking Character Type
```csharp
char ch = 'A';

// Using Char static methods
bool isDigit = char.IsDigit(ch);          // false
bool isLetter = char.IsLetter(ch);        // true
bool isWhitespace = char.IsWhiteSpace(ch);// false
bool isUpper = char.IsUpper(ch);          // true
bool isLower = char.IsLower(ch);          // false
bool isPunctuation = char.IsPunctuation(ch); // false
```

#### Case Conversion
```csharp
char upper = char.ToUpper('a');   // 'A'
char lower = char.ToLower('A');   // 'a'
```

#### Getting Character Info
```csharp
char ch = 'A';
int ascii = (int)ch;  // 65 (ASCII value)
char fromAscii = (char)65;  // 'A'
```

### Practical Examples

#### Character Validation
```csharp
public class InputValidator {
    public static bool IsValidPasswordChar(char ch) {
        return char.IsLetterOrDigit(ch) || 
               ch == '!' || ch == '@' || ch == '#';
    }
    
    public static bool IsNumericChar(char ch) {
        return char.IsDigit(ch);
    }
}
```

#### String Character Processing
```csharp
string text = "Hello123";

foreach (char ch in text) {
    if (char.IsDigit(ch)) {
        Console.WriteLine($"Digit: {ch}");
    } else if (char.IsLetter(ch)) {
        Console.WriteLine($"Letter: {ch}");
    }
}
// Output:
// Letter: H
// Letter: e
// Letter: l
// Letter: l
// Letter: o
// Digit: 1
// Digit: 2
// Digit: 3
```

#### Character Array Manipulation
```csharp
string word = "hello";
char[] chars = word.ToCharArray();

// Convert to uppercase
for (int i = 0; i < chars.Length; i++) {
    chars[i] = char.ToUpper(chars[i]);
}

string result = new string(chars);  // "HELLO"
```

### Char vs String

#### Key Differences

| Aspect | char | string |
|--------|------|--------|
| Size | 1 character | Multiple characters |
| Type | Value type | Reference type |
| Storage | Stack | Heap |
| Default | '\0' | null |
| Literal | Single quotes | Double quotes |
| Example | 'A' | "Hello" |

#### Comparison

```csharp
// Char - single character
char single = 'A';

// String - sequence of characters
string multiple = "Hello";

// Converting
string fromChar = single.ToString();      // "A"
char fromString = "Hello"[0];             // 'H'

// Checking length
// single.Length;  // Compile error - char has no Length
// multiple.Length;  // "Hello".Length = 5
```

### Unicode Considerations

```csharp
// Single char (ASCII range)
char ascii = 'A';  // U+0041

// Extended Unicode
char extended = '\u00E9';  // é (Latin small letter e with acute)

// Outside Basic Multilingual Plane
// Requires surrogate pairs (two chars)
string emoji = "😀";  // Stored as 2 chars internally

// Iterating correctly over Unicode strings
string text = "Hello 😀 World";
foreach (char ch in text) {
    Console.WriteLine(ch);  // May need surrogate pair handling
}
```

### Performance Considerations

```csharp
// Efficient character check
char ch = 'A';
if (ch >= 'A' && ch <= 'Z') {  // Direct comparison
    // Is uppercase letter
}

// Compare with method call (slightly slower)
if (char.IsUpper(ch)) {  // Method call overhead
    // Is uppercase letter
}

// For performance-critical code
// Direct comparisons may be better
// For readability, use char methods
```

### Common Char Operations Reference

```csharp
char ch = 'A';

// Type checking
char.IsDigit(ch);           // Is digit 0-9?
char.IsLetter(ch);          // Is letter A-Z a-z?
char.IsLetterOrDigit(ch);   // Is alphanumeric?
char.IsWhiteSpace(ch);      // Is whitespace?
char.IsUpper(ch);           // Is uppercase?
char.IsLower(ch);           // Is lowercase?
char.IsPunctuation(ch);     // Is punctuation?

// Case conversion
char.ToUpper(ch);           // Convert to uppercase
char.ToLower(ch);           // Convert to lowercase

// Getting numeric value
char.GetNumericValue(ch);   // Get numeric value if exists

// Unicode category
char.GetUnicodeCategory(ch); // Get Unicode category
```

### Common Mistakes

❌ **Using double quotes for single character**
```csharp
char ch = "A";  // Compile error - string not char
```

✓ **Use single quotes for char**
```csharp
char ch = 'A';  // Correct
```

❌ **Forgetting escape sequences**
```csharp
char newline = 'n';  // Just 'n', not newline
```

✓ **Use proper escape sequence**
```csharp
char newline = '\n';  // Actual newline
```

❌ **Treating char as number**
```csharp
char digit = '5';
int result = digit + 10;  // Result is 53 (char '5' = 53 in ASCII)
```

✓ **Convert explicitly**
```csharp
char digit = '5';
int result = char.GetNumericValue(digit) + 10;  // Result is 15
```

---

## Boolean vs Char Summary

| Aspect | Boolean | Char |
|--------|---------|------|
| Purpose | Logical true/false | Single character |
| Values | true, false | 0 to 65,535 (Unicode) |
| Size | 1 byte | 2 bytes |
| Default | false | '\0' |
| Literals | true, false | 'A', 'x', '5', etc. |
| Operations | &&, \|\|, ! | Comparisons, case conversion |

---

**Key Takeaway**: Use `bool` for logical decisions and `char` for single character operations. For multiple characters, use `string` instead.
