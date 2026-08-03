# Using Statement

## Overview
The using statement ensures that resources like file handles and database connections are properly disposed of, even if an exception occurs. It's the C# way to manage resource cleanup automatically.

## Why Using Statement

Without proper cleanup, resources leak:

```csharp
// BAD - Manual cleanup with try-finally
StreamReader reader = null;
try {
    reader = new StreamReader("file.txt");
    string content = reader.ReadToEnd();
    Process(content);
} finally {
    reader?.Dispose();  // Must remember to dispose
}

// GOOD - Using statement
using (StreamReader reader = new StreamReader("file.txt")) {
    string content = reader.ReadToEnd();
    Process(content);
}  // Dispose called automatically
```

## Basic Using Statement

### Using Block (All C# Versions)
```csharp
using (var resource = new MyResource()) {
    resource.DoWork();
}
// Dispose called automatically here
```

The using statement:
1. Creates the resource
2. Executes the code block
3. Calls Dispose on the resource
4. Even if exception occurs, Dispose is called

### Using Declaration (C# 8+)
```csharp
using StreamReader reader = new StreamReader("file.txt");
string content = reader.ReadToEnd();
Process(content);
// Dispose called automatically at end of method/scope
```

Simpler and cleaner syntax!

## Common Resources

### File I/O
```csharp
// Traditional
using (var reader = new StreamReader("file.txt")) {
    string line = reader.ReadLine();
}

// C# 8+
using StreamReader reader = new StreamReader("file.txt");
string line = reader.ReadLine();

// Even simpler with File API
string content = File.ReadAllText("file.txt");  // Manages resource internally
```

### Database Connections
```csharp
// Traditional
using (var connection = new SqlConnection(connectionString)) {
    connection.Open();
    var command = new SqlCommand("SELECT * FROM Users", connection);
    command.ExecuteReader();
}

// C# 8+
using SqlConnection connection = new SqlConnection(connectionString);
connection.Open();
var command = new SqlCommand("SELECT * FROM Users", connection);
command.ExecuteReader();
```

### Http Clients
```csharp
using (var client = new HttpClient()) {
    var response = await client.GetAsync("https://api.example.com");
    return await response.Content.ReadAsStringAsync();
}
```

## Nested Using Statements

### Traditional Syntax
```csharp
using (var conn = new SqlConnection(connectionString)) {
    using (var command = new SqlCommand("SELECT * FROM Users", conn)) {
        conn.Open();
        command.ExecuteReader();
    }
}
```

### C# 8+ Multiple Resources
```csharp
using SqlConnection conn = new SqlConnection(connectionString);
using SqlCommand command = new SqlCommand("SELECT * FROM Users", conn);
conn.Open();
command.ExecuteReader();
// Both disposed automatically in reverse order
```

### Using Multiple Resources (Older Syntax)
```csharp
using (StreamReader file = new StreamReader("file.txt"))
using (StreamWriter writer = new StreamWriter("output.txt")) {
    string line;
    while ((line = file.ReadLine()) != null) {
        writer.WriteLine(line);
    }
}
// Both disposed automatically
```

## Using with Exceptions

Using ensures Dispose even with exceptions:

```csharp
try {
    using (var resource = new MyResource()) {
        throw new Exception("Error!");
        // Dispose still called
    }
} catch (Exception) {
    Console.WriteLine("Disposed before catch");
}
```

## Using with Return

Dispose happens before return:

```csharp
public string ReadFile(string path) {
    using (var reader = new StreamReader(path)) {
        return reader.ReadToEnd();
    }
    // Dispose called before return
}
```

## Using Declaration Scope

C# 8+ using declaration disposes at end of scope:

```csharp
public void ProcessFiles() {
    using var file1 = new StreamReader("file1.txt");
    var line1 = file1.ReadLine();
    
    {
        using var file2 = new StreamReader("file2.txt");
        var line2 = file2.ReadLine();
    }  // file2 disposed here
    
    var line3 = file1.ReadLine();
}  // file1 disposed here
```

## Multiple Resources in Using

### Traditional - Comma Separated
```csharp
using (StreamReader file1 = new StreamReader("file1.txt"),
       file2 = new StreamReader("file2.txt")) {
    // Use both files
}  // Both disposed
```

### C# 8+ - Multiple Declarations
```csharp
using StreamReader file1 = new StreamReader("file1.txt");
using StreamReader file2 = new StreamReader("file2.txt");

// Use both files

// Both disposed at end of scope
```

## Combining Using with Try-Catch-Finally

```csharp
try {
    using (var conn = new SqlConnection(connectionString)) {
        conn.Open();
        ExecuteQuery(conn);
    }
} catch (SqlException ex) {
    Console.WriteLine($"Database error: {ex.Message}");
} finally {
    Console.WriteLine("Operation completed");
}
// Connection disposed before catch
```

## Custom Classes with Using

Implement IDisposable to use with using statement:

```csharp
public class FileReader : IDisposable {
    private StreamReader reader;
    
    public FileReader(string path) {
        reader = new StreamReader(path);
    }
    
    public string ReadAll() => reader.ReadToEnd();
    
    public void Dispose() {
        reader?.Dispose();
    }
}

// Usage
using (var fr = new FileReader("file.txt")) {
    string content = fr.ReadAll();
}  // Dispose called automatically
```

## Using with IAsyncDisposable (C# 8+)

For async cleanup:

```csharp
public class AsyncResource : IAsyncDisposable {
    public async ValueTask DisposeAsync() {
        // Async cleanup
        await Task.Delay(100);
    }
}

// Usage
await using (var resource = new AsyncResource()) {
    // Use resource
}  // Async dispose called
```

## Patterns

### Pattern 1: Simple File Reading
```csharp
public string ReadFile(string path) {
    using (var reader = new StreamReader(path)) {
        return reader.ReadToEnd();
    }
}

// C# 8+
public string ReadFile(string path) {
    using StreamReader reader = new StreamReader(path);
    return reader.ReadToEnd();
}
```

### Pattern 2: Database Operation
```csharp
public List<User> GetUsers() {
    using (var conn = new SqlConnection(connectionString)) {
        using (var command = new SqlCommand("SELECT * FROM Users", conn)) {
            conn.Open();
            var reader = command.ExecuteReader();
            return MapUsers(reader);
        }
    }
}

// C# 8+
public List<User> GetUsers() {
    using SqlConnection conn = new SqlConnection(connectionString);
    using SqlCommand command = new SqlCommand("SELECT * FROM Users", conn);
    conn.Open();
    var reader = command.ExecuteReader();
    return MapUsers(reader);
}
```

### Pattern 3: Multiple File Operations
```csharp
public void CopyFile(string source, string destination) {
    using (var srcReader = new StreamReader(source))
    using (var destWriter = new StreamWriter(destination)) {
        string line;
        while ((line = srcReader.ReadLine()) != null) {
            destWriter.WriteLine(line);
        }
    }
}

// C# 8+
public void CopyFile(string source, string destination) {
    using StreamReader srcReader = new StreamReader(source);
    using StreamWriter destWriter = new StreamWriter(destination);
    string line;
    while ((line = srcReader.ReadLine()) != null) {
        destWriter.WriteLine(line);
    }
}
```

## Best Practices

✓ Always use using for IDisposable resources
```csharp
using (var conn = new SqlConnection(connStr)) {
    // Use connection
}
```

✓ Use C# 8+ using declarations
```csharp
using var conn = new SqlConnection(connStr);
```

✓ Nest using statements logically
```csharp
using var conn = new SqlConnection(connStr);
using var command = new SqlCommand(sql, conn);
```

✓ Use high-level APIs when available
```csharp
// High-level (manages resources internally)
string content = File.ReadAllText("file.txt");

// Instead of low-level
using (var reader = new StreamReader("file.txt")) {
    string content = reader.ReadToEnd();
}
```

## Anti-Patterns

❌ Forgetting using statement
```csharp
var conn = new SqlConnection(connStr);
conn.Open();
// Disposed manually or not at all - error prone!
```

❌ Not disposing in finally
```csharp
try {
    // Resource created but not disposed if exception
}
```

❌ Creating without disposing
```csharp
StreamReader r = new StreamReader("file.txt");
string line = r.ReadLine();
// r never disposed!
```

## When Not to Use Using

Sometimes creating disposable resource but not disposing is intentional:

```csharp
public class Application {
    private SqlConnection persistentConnection;
    
    public void Initialize() {
        // Don't use 'using' - connection should persist
        persistentConnection = new SqlConnection(connStr);
        persistentConnection.Open();
    }
    
    public void Shutdown() {
        // Dispose manually when application ends
        persistentConnection?.Dispose();
    }
}
```

## Comparison: Old vs New Syntax

### Old Syntax (Pre-C# 8)
```csharp
using (StreamReader reader = new StreamReader("file.txt")) {
    string content = reader.ReadToEnd();
}
```

### New Syntax (C# 8+)
```csharp
using StreamReader reader = new StreamReader("file.txt");
string content = reader.ReadToEnd();
// More concise, disposes at end of scope
```

## Summary

- Using statement automatically calls Dispose
- Works with any IDisposable type
- Ensures cleanup even with exceptions
- C# 8+ using declaration is simpler
- Use for file handles, connections, streams
- High-level APIs often manage resources internally
- Always use using with disposable resources

---

## Next Steps

1. Learn Guard Clauses
2. Master IDisposable Pattern
3. Study Best Practices
4. Learn Common Mistakes
