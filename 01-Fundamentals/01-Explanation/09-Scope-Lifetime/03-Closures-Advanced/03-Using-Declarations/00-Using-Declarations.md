# Using Declarations and Resource Management

## Overview

Using declarations automate resource cleanup for objects implementing IDisposable. They ensure resources are properly released even if exceptions occur, making code more concise and safer.

## The Problem: Manual Resource Management

### Manual Cleanup (Before Using)

```csharp
public class ManualCleanup
{
    public void OldWayWithoutUsing()
    {
        // Manual resource management - error prone
        StreamReader reader = null;
        try
        {
            reader = File.OpenText("data.txt");
            string line = reader.ReadLine();
            Console.WriteLine(line);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            // Must remember to dispose
            if (reader != null)
            {
                reader.Dispose();
            }
        }
    }
    
    public void ProneToLeaks()
    {
        // Easy to forget disposal or handle exceptions incorrectly
        SqlConnection conn = new SqlConnection("...");
        
        // If exception happens here, conn is never disposed
        SqlCommand cmd = conn.CreateCommand();
        cmd.ExecuteReader();
    }
}
```

## Using Statement (Before C# 8.0)

### Traditional Using Statement

```csharp
public class TraditionalUsing
{
    public void ModernWayWithUsing()
    {
        // Using ensures disposal even with exceptions
        using (StreamReader reader = File.OpenText("data.txt"))
        {
            string line = reader.ReadLine();
            Console.WriteLine(line);
        } // reader.Dispose() called automatically here
    }
    
    public void SafeResourceManagement()
    {
        using (SqlConnection conn = new SqlConnection("connection"))
        {
            // Use connection
            // Disposed automatically, even if exception occurs
        }
    }
    
    public void MultipleResources()
    {
        using (StreamReader reader = File.OpenText("input.txt"))
        using (StreamWriter writer = File.CreateText("output.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                writer.WriteLine(line);
            }
        } // Both disposed automatically
    }
}
```

### Using Statement with Try-Catch

```csharp
public class UsingWithException
{
    public void HandleExceptionsInUsing()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection("..."))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
        // Resources disposed before catch block
    }
}
```

## Using Declaration (C# 8.0+)

### File-Scoped Using Declaration

```csharp
public class FileOperations
{
    public void ModernFileRead()
    {
        // Using declaration - C# 8.0+
        using var reader = File.OpenText("data.txt");
        string line = reader.ReadLine();
        Console.WriteLine(line);
        // reader.Dispose() called at END OF METHOD
    }
    
    public void ModernFileWrite()
    {
        using var writer = File.CreateText("output.txt");
        writer.WriteLine("Hello, World!");
        // writer.Dispose() called at end of method
    }
    
    public void MultipleUsingDeclarations()
    {
        // Multiple using declarations
        using var file1 = File.OpenRead("file1.txt");
        using var file2 = File.OpenRead("file2.txt");
        using var file3 = File.OpenRead("file3.txt");
        
        // Use all three files
        
        // Disposed in reverse order at end of method:
        // file3.Dispose()
        // file2.Dispose()
        // file1.Dispose()
    }
}
```

### Using Declaration Scope

```csharp
public class UsingDeclarationScope
{
    public void DemonstrateScope()
    {
        // Using statement: scope = braces
        using (var reader1 = File.OpenText("file.txt"))
        {
            string line = reader1.ReadLine();
        } // Disposed here
        
        // Using declaration: scope = end of method
        using var reader2 = File.OpenText("file.txt");
        string line2 = reader2.ReadLine();
        // Disposed at end of method
    }
    
    public void ScopeInConditional()
    {
        using var reader = File.OpenText("data.txt");
        
        if (reader != null)
        {
            string line = reader.ReadLine();
            Console.WriteLine(line);
        }
        
        // Disposed at end of method, not end of if block
    }
}
```

## IDisposable Pattern

### Implementing IDisposable

```csharp
public class Resource : IDisposable
{
    private IntPtr _unManagedHandle;
    private bool _disposed = false;
    
    // Finalizer for safety (optional)
    ~Resource()
    {
        Dispose(false);
    }
    
    public void Dispose()
    {
        Dispose(true);
        // Tell GC this object is already cleaned up
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (disposing)
        {
            // Clean up managed resources
            // Example: Dispose other IDisposable objects
        }
        
        // Clean up unmanaged resources
        if (_unManagedHandle != IntPtr.Zero)
        {
            // Close handle
            _unManagedHandle = IntPtr.Zero;
        }
        
        _disposed = true;
    }
    
    // Throw if someone tries to use after disposal
    protected void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);
    }
}

// Usage
public class ResourceUsage
{
    public void UseResource()
    {
        using var resource = new Resource();
        // Use resource
        // Dispose() called automatically
    }
}
```

### IAsyncDisposable Pattern

```csharp
public class AsyncResource : IAsyncDisposable
{
    private Stream _stream;
    
    public async ValueTask DisposeAsync()
    {
        if (_stream != null)
        {
            await _stream.FlushAsync();
            _stream.Dispose();
        }
    }
}

// Usage with await using (C# 8.0+)
public class AsyncResourceUsage
{
    public async Task UseAsyncResource()
    {
        await using var resource = new AsyncResource();
        // Use resource
        // DisposeAsync() called automatically
    }
}
```

## Practical Examples

### File Operations

```csharp
public class FileOperationExamples
{
    public void ReadFile()
    {
        using var reader = File.OpenText("data.txt");
        string line = reader.ReadLine();
        Console.WriteLine(line);
    }
    
    public void WriteFile()
    {
        using var writer = File.CreateText("output.txt");
        writer.WriteLine("Hello, World!");
    }
    
    public void CopyFile()
    {
        using var source = File.OpenRead("source.txt");
        using var destination = File.Create("destination.txt");
        
        source.CopyTo(destination);
    }
    
    public void ReadAllLines()
    {
        var lines = new List<string>();
        using var reader = File.OpenText("data.txt");
        
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            lines.Add(line);
        }
        
        return lines;
    }
}
```

### Database Operations

```csharp
public class DatabaseOperations
{
    private const string _connectionString = "...";
    
    public void QueryDatabase()
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        
        connection.Open();
        command.CommandText = "SELECT * FROM Users";
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            string name = reader["Name"].ToString();
            Console.WriteLine(name);
        }
    }
    
    public int ExecuteStoredProcedure(string param)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        
        connection.Open();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "sp_ProcessData";
        command.Parameters.AddWithValue("@param", param);
        
        return command.ExecuteNonQuery();
    }
}
```

### HTTP Requests

```csharp
public class HttpOperations
{
    public async Task GetWebContent()
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync("https://api.example.com/data");
        
        response.EnsureSuccessStatusCode();
        using var content = response.Content;
        string data = await content.ReadAsStringAsync();
        
        Console.WriteLine(data);
    }
    
    public async Task PostData(string jsonData)
    {
        using var client = new HttpClient();
        using var content = new StringContent(jsonData);
        
        using var response = await client.PostAsync("https://api.example.com/save", content);
        response.EnsureSuccessStatusCode();
    }
}
```

### Stream Operations

```csharp
public class StreamOperations
{
    public string DecompressFile(string compressedFile)
    {
        using var source = File.OpenRead(compressedFile);
        using var decompressed = new GZipStream(source, CompressionMode.Decompress);
        using var reader = new StreamReader(decompressed);
        
        return reader.ReadToEnd();
    }
    
    public byte[] ReadBinaryFile(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var memoryStream = new MemoryStream();
        
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
```

## Using Declaration Order and Disposal

### Reverse Disposal Order

```csharp
public class DisposalOrder
{
    public void Demonstrate()
    {
        using var first = new Resource("First");
        using var second = new Resource("Second");
        using var third = new Resource("Third");
        
        // Disposed in REVERSE order at method end:
        // third.Dispose()
        // second.Dispose()
        // first.Dispose()
    }
}

public class Resource
{
    private string _name;
    
    public Resource(string name)
    {
        _name = name;
        Console.WriteLine($"{_name} created");
    }
    
    public void Dispose()
    {
        Console.WriteLine($"{_name} disposed");
    }
}
```

## Exception Handling with Using

### Exceptions Don't Prevent Disposal

```csharp
public class ExceptionSafety
{
    public void SafetyGuarantee()
    {
        using var resource = new Resource("Test");
        
        try
        {
            throw new Exception("Something went wrong");
            // resource.Dispose() still called
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }
        // resource is disposed
    }
    
    public void DisposalBeforeCatch()
    {
        try
        {
            using var resource = new Resource("Test");
            throw new Exception("Error");
            // resource disposed here before catch
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }
    }
}

public class Resource
{
    public void Dispose()
    {
        Console.WriteLine("Resource disposed");
    }
}
```

## Best Practices

1. **Use 'using' for IDisposable**: Always dispose resources properly
2. **Choose Using Declaration**: Preferred over using statement (C# 8.0+)
3. **Multiple Resources**: Each needs its own using declaration
4. **Implement IDisposable Correctly**: Follow the standard pattern
5. **Use 'using var'**: Instead of declaring type explicitly when obvious
6. **Chain Operations**: Take advantage of disposal guarantee
7. **Document Resource Requirements**: Make dependencies clear

## Anti-Patterns to Avoid

```csharp
public class AntiPatterns
{
    // ANTI-PATTERN 1: Not using using statement
    public void BadNoUsing()
    {
        var file = File.OpenText("data.txt");
        string line = file.ReadLine();
        // file.Dispose() never called!
    }
    
    // ANTI-PATTERN 2: Wrong scope assumption
    public void BadScope()
    {
        using var reader = File.OpenText("data.txt");
        {
            string line = reader.ReadLine();
        } // Block ends, but reader NOT disposed here
    } // reader disposed here, not at block end
    
    // ANTI-PATTERN 3: Not disposing exceptions
    public void BadException()
    {
        try
        {
            var resource = new Resource();
            throw new Exception();
            // resource not disposed if exception occurs
        }
        catch { }
    }
}

public class Resource : IDisposable
{
    public void Dispose() { }
}
```

## Summary

Using declarations automate resource management, ensuring cleanup occurs even when exceptions are thrown. They are essential for working with files, database connections, HTTP clients, and any resource implementing IDisposable. Modern C# (8.0+) using declarations provide cleaner syntax than traditional using statements while maintaining the same safety guarantees. Proper use of using prevents resource leaks and improves code reliability.
