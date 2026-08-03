# Try-Catch-Finally Block

## Overview
The finally block is guaranteed to execute whether an exception occurs or not. It's essential for cleanup operations like closing files, releasing resources, and disposing objects.

## Basic Structure

```csharp
try {
    // Code that might throw exception
    Operation();
} catch (ExceptionType ex) {
    // Handle exception
} finally {
    // Code that ALWAYS executes
    Cleanup();
}
```

## Finally Always Executes

The finally block runs regardless of what happens in try or catch:

```csharp
void Example() {
    try {
        Console.WriteLine("1. Try block");
        return;  // Even returns don't skip finally!
    } finally {
        Console.WriteLine("3. Finally block");
    }
}

// Output:
// 1. Try block
// 3. Finally block
```

## Finally with Exception

```csharp
try {
    Console.WriteLine("1. Try block");
    throw new Exception("Error!");
} catch (Exception ex) {
    Console.WriteLine("2. Catch block");
} finally {
    Console.WriteLine("3. Finally block");
}

// Output:
// 1. Try block
// 2. Catch block
// 3. Finally block
```

## Finally without Exception

```csharp
try {
    Console.WriteLine("1. Try block");
} catch (Exception ex) {
    Console.WriteLine("2. Catch block");
} finally {
    Console.WriteLine("3. Finally block");
}

// Output:
// 1. Try block
// 3. Finally block
// (Catch not executed because no exception)
```

## Cleanup Operations

### File Handling
```csharp
StreamReader reader = null;
try {
    reader = new StreamReader("file.txt");
    string content = reader.ReadToEnd();
    Console.WriteLine(content);
} catch (FileNotFoundException) {
    Console.WriteLine("File not found");
} finally {
    // Cleanup - close file
    reader?.Dispose();
}
```

### Database Connections
```csharp
SqlConnection conn = null;
try {
    conn = new SqlConnection("connection_string");
    conn.Open();
    // Execute query
} catch (SqlException ex) {
    Console.WriteLine($"Database error: {ex.Message}");
} finally {
    // Cleanup - close connection
    conn?.Close();
    conn?.Dispose();
}
```

### Resource Acquisition
```csharp
Lock lockObj = AcquireLock();
try {
    CriticalSection();
} finally {
    // Cleanup - release lock
    ReleaseLock(lockObj);
}
```

## Finally with Multiple Returns

Finally executes even with multiple return paths:

```csharp
public int ValidateInput(string input) {
    try {
        if (string.IsNullOrEmpty(input)) {
            return 0;  // Return 1
        }
        
        int value = int.Parse(input);
        return value;  // Return 2
    } catch (FormatException) {
        return -1;  // Return 3
    } finally {
        Console.WriteLine("Cleanup");  // Always executes
    }
}

// All three return paths hit finally first
ValidateInput(null);      // Prints "Cleanup", returns 0
ValidateInput("abc");     // Prints "Cleanup", returns -1
ValidateInput("42");      // Prints "Cleanup", returns 42
```

## Finally with Throw

Finally executes even with throw:

```csharp
try {
    Console.WriteLine("1. Try");
    throw new Exception("Error");
} catch (Exception) {
    Console.WriteLine("2. Catch");
    throw;  // Re-throw exception
} finally {
    Console.WriteLine("3. Finally");
}

// Output:
// 1. Try
// 2. Catch
// 3. Finally
// (Then exception propagates)
```

## Nested Try-Finally

```csharp
try {
    try {
        Console.WriteLine("Inner try");
        throw new Exception();
    } finally {
        Console.WriteLine("Inner finally");
    }
} finally {
    Console.WriteLine("Outer finally");
}

// Output:
// Inner try
// Inner finally
// Outer finally
```

## Finally Exception Handling

### Exception in Finally
If finally throws exception, it replaces original:

```csharp
try {
    throw new FormatException("Original error");
} finally {
    throw new Exception("Finally error");  // This replaces original!
}

// Caller catches Exception, not FormatException!
```

**Avoid throwing in finally**:

```csharp
try {
    operation();
} finally {
    try {
        Cleanup();
    } catch {
        // Log cleanup error without throwing
        logger.Error("Cleanup failed");
    }
}
```

## Using Statement (Modern Cleanup)

In C# 8+, using statement is simpler than try-finally:

```csharp
// Traditional try-finally
StreamReader reader = null;
try {
    reader = new StreamReader("file.txt");
    string line = reader.ReadLine();
} finally {
    reader?.Dispose();
}

// Using statement (C# 8+)
using StreamReader reader = new StreamReader("file.txt");
string line = reader.ReadLine();
// Dispose called automatically at end of scope

// Using block (earlier versions)
using (StreamReader reader = new StreamReader("file.txt")) {
    string line = reader.ReadLine();
}  // Dispose called automatically
```

## Using with Finally

Can combine using and finally:

```csharp
try {
    using (var conn = new SqlConnection("connection")) {
        conn.Open();
        // Query execution
    }
} catch (SqlException ex) {
    logger.Error($"Database error: {ex.Message}");
} finally {
    // Additional cleanup
    Console.WriteLine("Operation completed");
}
```

## Common Cleanup Patterns

### Pattern 1: Resource Disposal
```csharp
public void ProcessFile(string filename) {
    StreamReader reader = null;
    try {
        reader = new StreamReader(filename);
        string content = reader.ReadToEnd();
        return content;
    } finally {
        reader?.Dispose();
    }
}
```

### Pattern 2: State Restoration
```csharp
public void TemporarilyChangeState() {
    var originalState = GetCurrentState();
    try {
        SetTemporaryState();
        DoWork();
    } finally {
        RestoreState(originalState);  // Always restore
    }
}
```

### Pattern 3: Logging Operations
```csharp
public void LoggedOperation() {
    logger.Info("Operation started");
    try {
        DoWork();
        logger.Info("Operation completed successfully");
    } catch (Exception ex) {
        logger.Error($"Operation failed: {ex.Message}");
        throw;
    } finally {
        logger.Info("Operation finished");
    }
}
```

### Pattern 4: Multiple Resource Cleanup
```csharp
public void ProcessMultipleFiles() {
    StreamReader file1 = null;
    StreamReader file2 = null;
    
    try {
        file1 = new StreamReader("file1.txt");
        file2 = new StreamReader("file2.txt");
        
        string content1 = file1.ReadToEnd();
        string content2 = file2.ReadToEnd();
        ProcessBoth(content1, content2);
    } finally {
        // Always cleanup both files
        file1?.Dispose();
        file2?.Dispose();
    }
}
```

## Finally Gotchas

### Gotcha 1: Not Calling Dispose
```csharp
// WRONG - Resource not released
StreamReader reader = new StreamReader("file.txt");
try {
    // ...
} finally {
    reader = null;  // Doesn't dispose!
}

// RIGHT - Call dispose
finally {
    reader?.Dispose();
}
```

### Gotcha 2: Throwing in Finally
```csharp
// WRONG - Replaces original exception
try {
    throw new Exception("Original");
} finally {
    throw new Exception("Finally");  // Replaces original!
}

// RIGHT - Handle exceptions in finally
try {
    throw new Exception("Original");
} finally {
    try {
        Cleanup();
    } catch {
        logger.Error("Cleanup error");
    }
}
```

### Gotcha 3: Performance Impact
Finally blocks have minimal overhead but execute after each try:

```csharp
// Each iteration's finally executes
for (int i = 0; i < 1000000; i++) {
    try {
        Operation();
    } finally {
        Cleanup();  // 1 million times!
    }
}

// Better - cleanup outside loop
for (int i = 0; i < 1000000; i++) {
    Operation();
}
Cleanup();  // Once at end
```

## Best Practices

✓ Use finally for guaranteed cleanup

```csharp
try {
    operation();
} finally {
    CloseResources();
}
```

✓ Use using for automatic cleanup (simpler)

```csharp
using (var resource = AcquireResource()) {
    UseResource();
}
```

✓ Keep finally blocks focused

```csharp
finally {
    resource?.Dispose();  // Just cleanup
}
```

✓ Handle exceptions in finally

```csharp
finally {
    try {
        Cleanup();
    } catch (Exception ex) {
        logger.Error("Cleanup failed", ex);
    }
}
```

## Anti-Patterns

❌ Complex logic in finally
```csharp
finally {
    DoComplexCalculation();  // Belongs in try, not finally
}
```

❌ Throwing exceptions in finally
```csharp
finally {
    throw new Exception();  // Replaces original exception
}
```

❌ Forgetting to dispose resources
```csharp
finally {
    reader = null;  // Doesn't actually dispose
}
```

## Summary

- Finally always executes (try, catch, return, throw)
- Use for guaranteed cleanup operations
- Using statement preferred over try-finally for resources
- Never throw in finally without catching
- Handle exceptions in finally to avoid masking original exception

---

## Next Steps

1. Learn Exception Flow
2. Master Custom Exceptions
3. Study Exception Properties
4. Learn Best Practices
