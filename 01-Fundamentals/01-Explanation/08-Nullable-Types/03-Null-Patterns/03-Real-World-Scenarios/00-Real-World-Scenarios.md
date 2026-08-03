# Real-World Null Handling Scenarios

## Overview
Common patterns and solutions for handling null in production code.

---

## Database Operations

### Nullable Database Values
```csharp
// Get employee bonus (might be null)
decimal? bonus = employee?.Bonus ?? 0;

// Calculate total
decimal salary = employee?.Salary ?? 0;
decimal total = salary + bonus;
```

---

## API Responses

### Optional Fields
```csharp
public class ApiResponse {
    public string? Message { get; set; }
    public int? StatusCode { get; set; }
    public object? Data { get; set; }
}

var response = GetApiResponse();
string message = response?.Message ?? "No message";
int code = response?.StatusCode ?? 500;
```

---

## Configuration

### Default Values
```csharp
public class Config {
    public int? MaxConnections { get; set; }
    public string? ConnectionString { get; set; }
}

var config = LoadConfig();
int maxConn = config.MaxConnections ?? 10;
string connStr = config.ConnectionString ?? "DefaultConnection";
```

---

## User Input

### Optional Parameters
```csharp
public void CreateUser(string name, string? email = null) {
    email ??= $"{name}@example.com";
}
```

---

## Summary

✓ Database nulls common
✓ APIs have optional fields
✓ Configuration with defaults
✓ User input often incomplete
✓ Always handle null safely
