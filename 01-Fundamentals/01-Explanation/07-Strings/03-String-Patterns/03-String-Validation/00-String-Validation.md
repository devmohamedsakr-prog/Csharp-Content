# String Validation and Regex

## Overview
Validate string input using simple checks and regular expressions. Ensure data quality and security.

---

## Basic Validation

### Null and Empty Checks

```csharp
string input = GetUserInput();

// Check null
if (input == null) {
    Console.WriteLine("Input is null");
}

// Check empty
if (input == "") {
    Console.WriteLine("Input is empty");
}

// Check null or empty
if (string.IsNullOrEmpty(input)) {
    Console.WriteLine("Input is null or empty");
}

// Check null, empty, or whitespace
if (string.IsNullOrWhiteSpace(input)) {
    Console.WriteLine("Input is null, empty, or whitespace");
}
```

### Length Validation

```csharp
string password = GetUserPassword();

// Minimum length
if (password.Length < 8) {
    Console.WriteLine("Password must be at least 8 characters");
}

// Maximum length
if (password.Length > 128) {
    Console.WriteLine("Password too long (max 128)");
}

// Exact length
if (password.Length != 6) {
    Console.WriteLine("Code must be exactly 6 characters");
}

// Length range
if (password.Length < 8 || password.Length > 128) {
    Console.WriteLine("Password length must be 8-128");
}
```

### Character Type Checks

```csharp
string input = GetInput();

// All digits
bool isNumeric = input.All(char.IsDigit);

// All letters
bool isAlpha = input.All(char.IsLetter);

// All alphanumeric
bool isAlphaNum = input.All(char.IsLetterOrDigit);

// Contains digit
bool hasDigit = input.Any(char.IsDigit);

// Contains uppercase
bool hasUpper = input.Any(char.IsUpper);

// Contains lowercase
bool hasLower = input.Any(char.IsLower);

// Contains special character
bool hasSpecial = input.Any(c => !char.IsLetterOrDigit(c));
```

---

## Email Validation

### Basic Email Check

```csharp
string email = GetUserEmail();

// Simple validation (not comprehensive)
bool isEmail = email.Contains("@") && 
               email.Contains(".") && 
               email.IndexOf("@") > 0 &&
               email.LastIndexOf("@") == email.IndexOf("@") &&
               email.IndexOf("@") < email.LastIndexOf(".");
```

### Email Regex Validation

```csharp
using System.Text.RegularExpressions;

string email = GetUserEmail();

// Simple regex
bool isEmail1 = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

// More comprehensive
bool isEmail2 = Regex.IsMatch(email, 
    @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

// RFC 5322 compliant (complex)
string rfcPattern = @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$";
bool isEmailRFC = Regex.IsMatch(email, rfcPattern);
```

---

## Phone Number Validation

### Basic Phone Check

```csharp
string phone = GetUserPhone();

// Simple check (US format)
bool isPhoneSimple = Regex.IsMatch(phone, @"^\d{3}-\d{3}-\d{4}$");

// Various formats
bool isPhoneFlexible = Regex.IsMatch(phone, 
    @"^(\+?1[-.\s]?)?\(?[0-9]{3}\)?[-.\s]?[0-9]{3}[-.\s]?[0-9]{4}$");

// Extract digits only
string digitsOnly = Regex.Replace(phone, @"\D", "");
bool isPhoneDigits = digitsOnly.Length == 10;
```

---

## URL/URI Validation

### Basic URL Check

```csharp
string url = GetUserUrl();

// Simple HTTPS/HTTP check
bool isUrl = url.StartsWith("https://") || url.StartsWith("http://");

// Using Uri class (recommended)
bool isValidUri = Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) &&
                  (uriResult.Scheme == Uri.UriSchemeHttp || 
                   uriResult.Scheme == Uri.UriSchemeHttps);

// Regex pattern
bool isUrlRegex = Regex.IsMatch(url, 
    @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$");
```

---

## Password Validation

### Strong Password Requirements

```csharp
bool IsStrongPassword(string password) {
    if (string.IsNullOrWhiteSpace(password)) return false;
    
    // Length requirement (at least 8)
    if (password.Length < 8) return false;
    
    // Must contain uppercase
    if (!password.Any(char.IsUpper)) return false;
    
    // Must contain lowercase
    if (!password.Any(char.IsLower)) return false;
    
    // Must contain digit
    if (!password.Any(char.IsDigit)) return false;
    
    // Must contain special character
    if (!password.Any(c => !char.IsLetterOrDigit(c))) return false;
    
    return true;
}

// Usage
string pwd = GetPassword();
if (IsStrongPassword(pwd)) {
    Console.WriteLine("Password is strong");
} else {
    Console.WriteLine("Password is weak");
}
```

### Regex Password Validation

```csharp
using System.Text.RegularExpressions;

// At least 8 chars, 1 upper, 1 lower, 1 digit, 1 special
string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
bool isStrong = Regex.IsMatch(password, pattern);
```

---

## Credit Card Validation

### Luhn Algorithm

```csharp
bool IsValidCreditCard(string cardNumber) {
    // Remove spaces and hyphens
    cardNumber = Regex.Replace(cardNumber, @"[\s-]", "");
    
    // Must be digits only
    if (!Regex.IsMatch(cardNumber, @"^\d{13,19}$")) {
        return false;
    }
    
    // Luhn algorithm
    int sum = 0;
    bool isEven = false;
    
    for (int i = cardNumber.Length - 1; i >= 0; i--) {
        int digit = cardNumber[i] - '0';
        
        if (isEven) {
            digit *= 2;
            if (digit > 9) {
                digit -= 9;
            }
        }
        
        sum += digit;
        isEven = !isEven;
    }
    
    return sum % 10 == 0;
}

// Usage
if (IsValidCreditCard("4111-1111-1111-1111")) {
    Console.WriteLine("Valid card");
}
```

---

## Regular Expression Basics

### Common Patterns

```csharp
using System.Text.RegularExpressions;

// Numbers only
Regex.IsMatch("12345", @"^\d+$");  // true

// Alphabetic only
Regex.IsMatch("HELLO", @"^[a-zA-Z]+$");  // true

// Alphanumeric with underscores
Regex.IsMatch("user_123", @"^[a-zA-Z0-9_]+$");  // true

// Starts with letter
Regex.IsMatch("abc123", @"^[a-zA-Z].*$");  // true

// Ends with .com
Regex.IsMatch("example.com", @"\.com$");  // true

// Between 3-10 chars
Regex.IsMatch("hello", @"^.{3,10}$");  // true

// One or more spaces
Regex.IsMatch("hello   world", @"\s+");  // true

// Exactly 3 digits
Regex.IsMatch("123", @"^\d{3}$");  // true
```

### Pattern Components

```csharp
// .       = Any character
// ^       = Start of string
// $       = End of string
// *       = 0 or more
// +       = 1 or more
// ?       = 0 or 1
// {n}     = Exactly n
// {n,m}   = Between n and m
// [abc]   = a, b, or c
// [^abc]  = Not a, b, or c
// [a-z]   = a through z
// \d      = Digit [0-9]
// \D      = Non-digit
// \w      = Word character [a-zA-Z0-9_]
// \W      = Non-word
// \s      = Whitespace
// \S      = Non-whitespace
// |       = Or
// ()      = Group
// (?=...) = Lookahead (positive)
// (?!...) = Lookahead (negative)
```

---

## Practical Validation Examples

### Username Validation

```csharp
bool IsValidUsername(string username) {
    if (string.IsNullOrWhiteSpace(username)) return false;
    if (username.Length < 3 || username.Length > 20) return false;
    
    // Alphanumeric and underscore only, must start with letter
    return Regex.IsMatch(username, @"^[a-zA-Z][a-zA-Z0-9_]{2,19}$");
}
```

### IP Address Validation

```csharp
bool IsValidIPAddress(string ip) {
    // IPv4 pattern
    return Regex.IsMatch(ip, 
        @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
}
```

### Filename Validation

```csharp
bool IsValidFilename(string filename) {
    if (string.IsNullOrWhiteSpace(filename)) return false;
    
    // No path separators or special characters
    char[] invalidChars = Path.GetInvalidFileNameChars();
    return !filename.Any(c => invalidChars.Contains(c));
}
```

---

## Performance Considerations

### Compiled Regex

```csharp
// Create once, reuse many times
static readonly Regex EmailRegex = 
    new Regex(@"^[^@]+@[^@]+\.[^@]+$", RegexOptions.Compiled);

// Use efficiently
bool isEmail = EmailRegex.IsMatch(userEmail);
```

### Validation Caching

```csharp
// Cache validation results if input doesn't change
Dictionary<string, bool> validationCache = new();

bool IsValidEmail(string email) {
    if (validationCache.TryGetValue(email, out bool isValid)) {
        return isValid;
    }
    
    isValid = Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
    validationCache[email] = isValid;
    return isValid;
}
```

---

## Common Mistakes

### ❌ Over-Complex Email Regex

```csharp
// RFC 5322 is overly complex
// Most apps don't need perfect RFC compliance

// SIMPLE SUFFICIENT
bool isEmail = Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
```

✓ **Keep it reasonable:**
```csharp
bool isEmail = Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
```

### ❌ Not Escaping Special Characters

```csharp
// Dot matches any character (wrong!)
Regex.IsMatch(email, @"^[^@]+@[^@]+.[^@]+$");  // Wrong: . matches anything

// Escape the dot (right)
Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");  // Right: \. matches dot only
```

### ❌ Forgetting to Trim

```csharp
string input = GetUserInput();  // " email@example.com "
bool isEmail = Regex.IsMatch(input, @"^[^@]+@[^@]+\.[^@]+$");  // false!

// Trim first
bool isEmail2 = Regex.IsMatch(input.Trim(), @"^[^@]+@[^@]+\.[^@]+$");  // true
```

---

## Best Practices

✓ Always validate user input
✓ Use appropriate validation for context
✓ Keep regex patterns simple and readable
✓ Use `string.IsNullOrWhiteSpace()` for basic checks
✓ Use `Uri.TryCreate()` for URL validation
✓ Compile frequently-used regex patterns
✓ Add comments explaining complex patterns
✓ Test validation with edge cases

---

## Next Steps

1. Study StringBuilder Performance
2. Review Best Practices
3. Practice Interview Questions
