# String Formatting and Output

## Overview
Format strings for display using interpolation, format specifiers, and custom formatting.

---

## String Interpolation (C# 6+)

### Basic Interpolation

```csharp
string name = "Alice";
int age = 30;

// Simple interpolation
string message = $"Name: {name}, Age: {age}";
// "Name: Alice, Age: 30"

// Multiple expressions
string info = $"{name} is {age} years old";
// "Alice is 30 years old"

// Expressions in braces
string math = $"2 + 2 = {2 + 2}";
// "2 + 2 = 4"

string comparison = $"Is 5 > 3? {5 > 3}";
// "Is 5 > 3? True"
```

### Format Specifiers

```csharp
// Decimal places
decimal price = 19.99m;
string formatted = $"Price: {price:F2}";
// "Price: 19.99"

// Currency
string currency = $"Cost: {price:C}";
// "Cost: $19.99"  (depends on culture)

// Percentage
double percent = 0.85;
string percentage = $"Progress: {percent:P}";
// "Progress: 85.00%"

// Numbers with leading zeros
int id = 42;
string padded = $"ID: {id:D5}";
// "ID: 00042"

// Hexadecimal
int hex = 255;
string hexStr = $"Hex: {hex:X}";
// "Hex: FF"
```

### Advanced Format Specifiers

```csharp
// Decimal with precision
double value = 123.456;
string precise = $"{value:F3}";  // "123.456"
string short = $"{value:F1}";  // "123.5"

// Thousands separator
int large = 1000000;
string withSeparator = $"{large:N0}";  // "1,000,000"

// Scientific notation
double scientific = 0.000123;
string sci = $"{scientific:E}";  // "1.230000E-004"

// Percentage with decimals
double rate = 0.8567;
string pct = $"{rate:P2}";  // "85.67%"
```

### Date/Time Formatting

```csharp
DateTime now = DateTime.Now;

// Common formats
string shortDate = $"{now:d}";  // "8/3/2024"
string longDate = $"{now:D}";  // "Saturday, August 3, 2024"
string time = $"{now:t}";  // "2:30 PM"
string fullDateTime = $"{now:F}";  // "Saturday, August 3, 2024 2:30:45 PM"

// Custom formats
string custom1 = $"{now:yyyy-MM-dd}";  // "2024-08-03"
string custom2 = $"{now:MMM dd, yyyy}";  // "Aug 03, 2024"
string custom3 = $"{now:HH:mm:ss}";  // "14:30:45"
```

### Conditional Interpolation

```csharp
int score = 85;

// Ternary operator
string result = $"Score: {score} - Grade: {(score >= 90 ? "A" : score >= 80 ? "B" : "C")}";

// Method calls
string text = "hello";
string upper = $"Uppercase: {text.ToUpper()}";

// Property access
Person person = new Person { Name = "Alice", Age = 30 };
string info = $"Person: {person.Name}, {person.Age} years old";

// LINQ
List<int> numbers = new() { 1, 2, 3, 4, 5 };
string stats = $"Sum: {numbers.Sum()}, Avg: {numbers.Average():F2}";
```

---

## String.Format Method

### Composite Formatting

```csharp
// Basic format string
string formatted = string.Format("Hello {0}, you are {1} years old", "Alice", 30);
// "Hello Alice, you are 30 years old"

// Multiple uses of same placeholder
string msg = string.Format("{0} {1} {0}", "A", "B");
// "A B A"

// With format specifiers
int id = 42;
string formatted2 = string.Format("ID: {0:D5}", id);
// "ID: 00042"

decimal price = 19.99m;
string formatted3 = string.Format("Price: {0:C}", price);
// "Price: $19.99"
```

---

## Format Specifiers Reference

### Number Formats

```csharp
int num = 42;

// D - Decimal (integers)
$"{num:D}";      // "42"
$"{num:D5}";     // "00042"

// F - Fixed-point
$"{42.56:F}";    // "42.56"
$"{42.56:F2}";   // "42.56"
$"{42.5:F2}";    // "42.50"

// E - Exponential
$"{0.000123:E}"; // "1.230000E-004"
$"{0.000123:E2}";// "1.23E-004"

// G - General
$"{42:G}";       // "42"
$"{42.5:G}";     // "42.5"

// N - Number with thousands separator
$"{1000000:N}";  // "1,000,000.00"
$"{1000000:N0}"; // "1,000,000"

// X - Hexadecimal
$"{255:X}";      // "FF"
$"{255:x}";      // "ff"

// P - Percentage
$"{0.85:P}";     // "85.00%"
$"{0.85:P0}";    // "85%"
```

### Date/Time Formats

```csharp
DateTime dt = new DateTime(2024, 8, 3, 14, 30, 45);

// Standard formats
$"{dt:d}";    // "8/3/2024" (short date)
$"{dt:D}";    // "Saturday, August 3, 2024" (long date)
$"{dt:t}";    // "2:30 PM" (short time)
$"{dt:T}";    // "2:30:45 PM" (long time)
$"{dt:f}";    // "Saturday, August 3, 2024 2:30 PM" (full short)
$"{dt:F}";    // "Saturday, August 3, 2024 2:30:45 PM" (full long)

// Custom formats
$"{dt:yyyy-MM-dd}";       // "2024-08-03"
$"{dt:MM/dd/yyyy}";       // "08/03/2024"
$"{dt:dddd, MMMM dd}";    // "Saturday, August 03"
$"{dt:HH:mm:ss}";         // "14:30:45"
$"{dt:ddd MMM dd yyyy}";  // "Sat Aug 03 2024"
```

---

## Custom Formatting Examples

### Money/Currency

```csharp
decimal amount = 1234.56m;

// Currency
$"{amount:C}";       // "$1,234.56" (culture-dependent)
$"{amount:C2}";      // "$1,234.56"

// Custom currency format
string formatted = amount.ToString("C", CultureInfo.GetCultureInfo("en-US"));
// "$1,234.56"

string euro = amount.ToString("C", CultureInfo.GetCultureInfo("de-DE"));
// "1.234,56 €"
```

### Phone Numbers

```csharp
string phone = "1234567890";

// Using string.Format or interpolation with manual formatting
string formatted = $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6)}";
// "(123) 456-7890"

// Using Regex
using System.Text.RegularExpressions;
string formatted2 = Regex.Replace(phone, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
// "(123) 456-7890"
```

### Padding and Alignment

```csharp
// Left-align (default)
$"{42,-5}";      // "42   "

// Right-align
$"{42,5}";       // "   42"

// With format specifier
$"{42,5:D3}";    // "  042"

// Table formatting
string header1 = "Name".PadRight(20) + "Age".PadRight(10) + "City";
string row1 = "Alice".PadRight(20) + "30".PadRight(10) + "NYC";
Console.WriteLine(header1);
Console.WriteLine(row1);
```

---

## Console Output

### WriteLine with Formatting

```csharp
// Using interpolation
string name = "Alice";
Console.WriteLine($"Hello, {name}!");

// Using string.Format
Console.WriteLine("Hello, {0}!", name);

// Using composite string
Console.WriteLine("The answer is {0}", 42);
```

### Table Output

```csharp
var data = new[] {
    (Name: "Alice", Age: 30, City: "NYC"),
    (Name: "Bob", Age: 25, City: "LA"),
    (Name: "Charlie", Age: 35, City: "Boston")
};

// Header
Console.WriteLine("{0,-15} {1,5} {2,-15}", "Name", "Age", "City");
Console.WriteLine(new string('-', 35));

// Rows
foreach (var item in data) {
    Console.WriteLine("{0,-15} {1,5} {2,-15}", item.Name, item.Age, item.City);
}

// Output:
// Name            Age City
// --------------- --- ---------------
// Alice            30 NYC
// Bob              25 LA
// Charlie          35 Boston
```

---

## Performance Considerations

### Interpolation vs Concatenation

```csharp
// Interpolation (recommended)
string msg = $"Hello {name}";

// Concatenation (avoid)
string msg2 = "Hello " + name;

// Performance: Similar after compilation
// Use interpolation for readability
```

### Formatting Many Items

```csharp
// INEFFICIENT - Multiple interpolations
string output = "";
foreach (var item in items) {
    output += $"{item}\n";  // Creates many strings
}

// EFFICIENT - Use StringBuilder
var sb = new StringBuilder();
foreach (var item in items) {
    sb.AppendLine($"{item}");
}
string output = sb.ToString();
```

---

## Common Mistakes

### ❌ Forgetting Format Specifier

```csharp
decimal price = 19.99m;
string formatted = $"Price: {price}";  // "Price: 19.99" (may not be currency)
```

✓ **Use format specifier:**
```csharp
string formatted = $"Price: {price:C}";  // "Price: $19.99"
```

### ❌ Assuming Culture Independence

```csharp
// May show different currency symbol/format
string currency = $"{price:C}";  // Depends on system culture
```

✓ **Specify culture if needed:**
```csharp
var culture = CultureInfo.GetCultureInfo("en-US");
string currency = price.ToString("C", culture);  // Always "$19.99"
```

### ❌ Not Padding Numbers

```csharp
// Looks unprofessional
$"ID: {id}";  // "ID: 42"
```

✓ **Pad with zeros:**
```csharp
$"ID: {id:D5}";  // "ID: 00042"
```

---

## Summary of Common Format Specifiers

| Specifier | Purpose | Example | Result |
|-----------|---------|---------|--------|
| `D5` | Decimal, 5 digits | `{42:D5}` | `00042` |
| `F2` | Fixed-point, 2 decimals | `{19.99:F2}` | `19.99` |
| `C` | Currency | `{19.99:C}` | `$19.99` |
| `P` | Percentage | `{0.85:P}` | `85.00%` |
| `N0` | Number with separators | `{1000000:N0}` | `1,000,000` |
| `X` | Hexadecimal | `{255:X}` | `FF` |
| `d` | Short date | `{dt:d}` | `8/3/2024` |
| `D` | Long date | `{dt:D}` | `Saturday, August 3, 2024` |
| `yyyy-MM-dd` | Custom date | `{dt:yyyy-MM-dd}` | `2024-08-03` |

---

## Best Practices

✓ Use string interpolation for readability
✓ Use appropriate format specifiers
✓ Use `CultureInfo` for culture-specific formatting
✓ Use `StringBuilder` for many format operations
✓ Pad numbers for aligned output
✓ Document custom format strings

---

## Next Steps

1. Study String Validation
2. Learn StringBuilder Performance
3. Review Best Practices
