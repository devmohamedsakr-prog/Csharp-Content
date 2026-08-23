# Try-Catch & Exception Snippets

Generate exception handling with built-in snippets.

## try - Try-Catch Block

**Shortcut:** `try` + Tab

**Generates:**
```csharp
try
{
}
catch (Exception)
{
}
```

**Usage:**
```csharp
try → Tab
// Now: try { } catch (Exception) { }
```

**Basic Example:**
```csharp
try
{
    int result = 10 / int.Parse("0");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

---

## Specific Exception Handling

**Pattern:**
```csharp
try
{
    int num = int.Parse(userInput);
}
catch (FormatException ex)
{
    Console.WriteLine("Invalid number format");
}
catch (OverflowException ex)
{
    Console.WriteLine("Number too large");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

**Example:**
```csharp
try
{
    File.ReadAllText("missing.txt");
}
catch (FileNotFoundException)
{
    Console.WriteLine("File not found");
}
catch (UnauthorizedAccessException)
{
    Console.WriteLine("Access denied");
}
catch (IOException ex)
{
    Console.WriteLine($"IO error: {ex.Message}");
}
```

---

## try-catch-finally - Finally Block

**Pattern:**
```csharp
try
{
    // Code that might throw exception
}
catch (Exception ex)
{
    // Handle exception
}
finally
{
    // Always executes
    // Used for cleanup
}
```

**Example - Database Connection:**
```csharp
SqlConnection conn = null;
try
{
    conn = new SqlConnection("connection_string");
    conn.Open();
    // Execute query
}
catch (SqlException ex)
{
    Console.WriteLine($"Database error: {ex.Message}");
}
finally
{
    conn?.Close();
    conn?.Dispose();
}
```

**Example - File Operations:**
```csharp
StreamReader reader = null;
try
{
    reader = new StreamReader("data.txt");
    string line = reader.ReadLine();
}
catch (FileNotFoundException)
{
    Console.WriteLine("File not found");
}
finally
{
    reader?.Dispose();
}
```

---

## try-finally (No Catch)

**Pattern:**
```csharp
try
{
    // Code
}
finally
{
    // Always cleanup
}
```

**Example:**
```csharp
var resource = GetResource();
try
{
    UseResource(resource);
}
finally
{
    resource.Cleanup();
}
```

---

## Using Statement - Automatic Disposal

**Pattern:**
```csharp
using (var file = File.OpenRead("data.txt"))
{
    // Use file
}
// Automatically disposed
```

**Examples:**
```csharp
using (var client = new HttpClient())
{
    var response = await client.GetAsync("https://api.example.com");
}

using (var conn = new SqlConnection("connection_string"))
{
    conn.Open();
    // Execute queries
}

using (StreamReader reader = File.OpenText("file.txt"))
{
    string line = reader.ReadLine();
}
```

---

## Using Declaration (C# 8+)

**Pattern:**
```csharp
using var file = File.OpenRead("data.txt");
// Use file
// Automatically disposed at end of scope
```

**Examples:**
```csharp
public void ProcessFile(string path)
{
    using var reader = File.OpenText(path);
    string line = reader.ReadLine();
    // Disposed automatically at method end
}

public async Task<string> FetchDataAsync(string url)
{
    using var client = new HttpClient();
    var response = await client.GetAsync(url);
    return await response.Content.ReadAsStringAsync();
}
```

---

## Nested Try-Catch

**Pattern:**
```csharp
try
{
    try
    {
        // Inner operation
    }
    catch (SpecificException ex)
    {
        // Handle inner exception
        throw;  // Rethrow
    }
}
catch (Exception ex)
{
    // Handle outer exception
}
```

---

## Throwing Exceptions

**Pattern:**
```csharp
public void ValidateAge(int age)
{
    if (age < 0)
        throw new ArgumentException("Age cannot be negative");
    
    if (age < 18)
        throw new InvalidOperationException("Must be 18 or older");
}

public string GetValue(string key)
{
    if (string.IsNullOrEmpty(key))
        throw new ArgumentNullException(nameof(key));
    
    if (!_dictionary.ContainsKey(key))
        throw new KeyNotFoundException($"Key '{key}' not found");
    
    return _dictionary[key];
}
```

---

## Custom Exception Handling

**Pattern:**
```csharp
public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message) 
        : base(message) { }
}

// Usage
try
{
    Login(username, password);
}
catch (InvalidCredentialsException ex)
{
    Console.WriteLine($"Login failed: {ex.Message}");
}
```

---

## Exception Filtering (C# 6+)

**Pattern:**
```csharp
try
{
    ProcessData();
}
catch (IOException ex) when (ex.Message.Contains("timeout"))
{
    Console.WriteLine("Timeout occurred");
}
catch (IOException ex)
{
    Console.WriteLine("Other IO error");
}
```

---

## AggregateException - Multiple Exceptions

**Pattern:**
```csharp
try
{
    Parallel.ForEach(items, item => ProcessItem(item));
}
catch (AggregateException ae)
{
    foreach (var innerEx in ae.InnerExceptions)
    {
        Console.WriteLine($"Error: {innerEx.Message}");
    }
}
```

---

## Quick Reference

| Type | Purpose |
|------|---------|
| `try` | Try-catch block |
| `catch` | Specific exception |
| `finally` | Always executes |
| `throw` | Throw exception |
| `using` | Auto dispose |
| `when` | Exception filter |

---

## Best Practices

- Catch specific exceptions, not generic Exception
- Always use finally for cleanup
- Use using statements for resources
- Provide meaningful exception messages
- Log exceptions before rethrowing
- Don't catch and ignore silently
- Use custom exceptions for domain-specific errors

