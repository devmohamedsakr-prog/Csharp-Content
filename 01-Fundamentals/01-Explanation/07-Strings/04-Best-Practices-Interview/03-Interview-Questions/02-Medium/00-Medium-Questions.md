# Strings - Medium Interview Questions

## Q1: When should you use StringBuilder over string concatenation?

**Answer:**
```csharp
// StringBuilder needed for loops
int iterations = 10000;

// BAD - O(n²)
string result = "";
for (int i = 0; i < iterations; i++) {
    result += i.ToString();  // ~1-2 seconds
}

// GOOD - O(n)
var sb = new StringBuilder();
for (int i = 0; i < iterations; i++) {
    sb.Append(i.ToString());  // ~5-10ms
}
string result = sb.ToString();
```

**Rule:** Use StringBuilder if concatenating 3+ times in a loop

---

## Q2: Design a function to validate email addresses

**Answer:**
```csharp
bool IsValidEmail(string email) {
    if (string.IsNullOrWhiteSpace(email)) return false;
    
    // Simple but sufficient
    return Regex.IsMatch(email, 
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
}

// Alternative: Use Uri
bool IsValidEmailAlt(string email) {
    try {
        var addr = new System.Net.Mail.MailAddress(email);
        return addr.Address == email;
    } catch {
        return false;
    }
}
```

---

## Q3: What are StringComparison options?

**Answer:**
```csharp
// Ordinal - byte-by-byte, culture-independent
"Hello".Equals("hello", StringComparison.Ordinal);  // false

// OrdinalIgnoreCase - case-insensitive, fast
"Hello".Equals("hello", StringComparison.OrdinalIgnoreCase);  // true

// CurrentCulture - culture-aware
"Café".Equals("Cafe", StringComparison.CurrentCulture);  // Depends on culture

// Use Ordinal for identifiers/keys
// Use OrdinalIgnoreCase for user input comparison
// Avoid CurrentCulture unless locale matters
```

---

## Q4: How do you properly replace text?

**Answer:**
```csharp
// Simple replacement
string result = text.Replace("old", "new");

// Case-insensitive (using Regex)
string resultCI = Regex.Replace(text, "OLD", "new", RegexOptions.IgnoreCase);

// Replace with capture groups
string phone = "123-456-7890";
string formatted = Regex.Replace(phone, @"(\d{3})-(\d{3})-(\d{4})", "($1) $2-$3");

// Multiple operations efficiently
var sb = new StringBuilder(text);
sb.Replace("old1", "new1");
sb.Replace("old2", "new2");
string result = sb.ToString();
```

---

## Q5: Extract and parse string data

**Answer:**
```csharp
// Parse CSV
string csv = "Alice,30,NYC";
string[] parts = csv.Split(',');
if (parts.Length >= 3) {
    string name = parts[0];
    if (int.TryParse(parts[1], out int age)) {
        // Use age safely
    }
}

// Extract number from mixed string
string data = "Price: $19.99";
if (double.TryParse(Regex.Match(data, @"\d+\.\d{2}").Value, out double price)) {
    // Use price
}

// Always validate parsing results
```

---

## Q6: Build a string builder pattern for CSV

**Answer:**
```csharp
public string BuildCsv(IEnumerable<(string Name, int Age, string City)> data) {
    var sb = new StringBuilder();
    
    // Header
    sb.AppendLine("Name,Age,City");
    
    // Rows
    foreach (var item in data) {
        sb.AppendLine($"{EscapeCsv(item.Name)},{item.Age},{EscapeCsv(item.City)}");
    }
    
    return sb.ToString();
}

private string EscapeCsv(string field) {
    if (field.Contains(",") || field.Contains("\"")) {
        return $"\"{field.Replace("\"", "\"\"")}\"";
    }
    return field;
}
```

---

## Q7: Analyze string performance issues

**Answer:**
```csharp
// Problem: Multiple string operations
foreach (var item in items) {
    string formatted = $"{item.Id:D5}: {item.Name}";
    if (formatted.Contains(searchTerm)) {
        results.Add(formatted);
    }
}

// Optimization: Filter before formatting
foreach (var item in items) {
    if (item.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) {
        string formatted = $"{item.Id:D5}: {item.Name}";
        results.Add(formatted);
    }
}

// Key: Expensive operations (Contains with case-insensitive) before formatting
```

---

## Q8: Validate password strength

**Answer:**
```csharp
bool IsStrongPassword(string password) {
    if (string.IsNullOrWhiteSpace(password)) return false;
    if (password.Length < 8) return false;
    if (!password.Any(char.IsUpper)) return false;
    if (!password.Any(char.IsLower)) return false;
    if (!password.Any(char.IsDigit)) return false;
    
    // At least one special character
    if (!password.Any(c => !char.IsLetterOrDigit(c))) return false;
    
    return true;
}

// Or using regex (less readable)
string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
bool isStrong = Regex.IsMatch(password, pattern);
```

---

## Q9: Differences between string comparison methods

**Answer:**
```csharp
string a = "café";
string b = "cafe";

// Reference equality (rare)
bool refEqual = ReferenceEquals(a, b);  // false

// Content equality (different by accents)
bool contentEqual = a == b;  // false (culture-dependent)

// Ordinal (byte-by-byte, different)
bool ordinal = a.Equals(b, StringComparison.Ordinal);  // false

// Culture-aware (might be true depending on culture)
bool culture = a.Equals(b, StringComparison.CurrentCulture);  // May be true

// Use Ordinal for comparison (consistent)
// Use culture for display/sorting
```

---

## Q10: Handle internationalization

**Answer:**
```csharp
// WRONG - Culture-dependent
string key = userInput.ToLower();

// RIGHT - Consistent across cultures
string key = userInput.ToLowerInvariant();

// Formatting with culture
CultureInfo enUS = CultureInfo.GetCultureInfo("en-US");
CultureInfo deDe = CultureInfo.GetCultureInfo("de-DE");

decimal price = 1234.56m;
string enPrice = price.ToString("C", enUS);  // "$1,234.56"
string dePrice = price.ToString("C", deDe);  // "1.234,56 €"
```

---

## Summary of Medium Concepts

✓ StringBuilder for loops (3+ concatenations)
✓ Email validation with regex
✓ StringComparison options
✓ Case-insensitive replacement
✓ Safe parsing with TryParse
✓ CSV escaping
✓ Performance optimization
✓ Password validation
✓ String comparison methods
✓ Internationalization

---

## Next Steps

1. Study Hard questions
2. Practice complex scenarios
