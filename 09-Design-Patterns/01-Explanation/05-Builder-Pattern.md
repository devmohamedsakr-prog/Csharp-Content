# Builder Pattern

## Overview
Builder Pattern constructs complex objects step-by-step, separating construction from representation.

## Basic Builder

### Fluent Interface
```csharp
public class SqlQueryBuilder
{
    private string _select;
    private string _from;
    private string _where;
    private string _orderBy;
    
    public SqlQueryBuilder Select(string columns)
    {
        _select = columns;
        return this;
    }
    
    public SqlQueryBuilder From(string table)
    {
        _from = table;
        return this;
    }
    
    public SqlQueryBuilder Where(string condition)
    {
        _where = condition;
        return this;
    }
    
    public SqlQueryBuilder OrderBy(string columns)
    {
        _orderBy = columns;
        return this;
    }
    
    public string Build()
    {
        var query = $"SELECT {_select} FROM {_from}";
        
        if (!string.IsNullOrEmpty(_where))
            query += $" WHERE {_where}";
        
        if (!string.IsNullOrEmpty(_orderBy))
            query += $" ORDER BY {_orderBy}";
        
        return query;
    }
}

// Usage
var query = new SqlQueryBuilder()
    .Select("Id, Name, Email")
    .From("Users")
    .Where("IsActive = 1")
    .OrderBy("Name")
    .Build();

// SELECT Id, Name, Email FROM Users WHERE IsActive = 1 ORDER BY Name
```

## Object Builder

### Building Complex Objects
```csharp
public class HttpRequestBuilder
{
    private string _method = "GET";
    private string _url;
    private Dictionary<string, string> _headers = new();
    private string _body;
    
    public HttpRequestBuilder Method(string method)
    {
        _method = method;
        return this;
    }
    
    public HttpRequestBuilder Url(string url)
    {
        _url = url;
        return this;
    }
    
    public HttpRequestBuilder Header(string name, string value)
    {
        _headers[name] = value;
        return this;
    }
    
    public HttpRequestBuilder Body(string body)
    {
        _body = body;
        return this;
    }
    
    public HttpRequestMessage Build()
    {
        var request = new HttpRequestMessage(
            new HttpMethod(_method),
            _url
        );
        
        foreach (var header in _headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }
        
        if (_body != null)
        {
            request.Content = new StringContent(_body);
        }
        
        return request;
    }
}

// Usage
var request = new HttpRequestBuilder()
    .Method("POST")
    .Url("https://api.example.com/users")
    .Header("Authorization", "Bearer token123")
    .Header("Content-Type", "application/json")
    .Body("{\"name\": \"Alice\"}")
    .Build();
```

## Separate Builder Class

### Delegated Building
```csharp
public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    
    // Private constructor - can only build via builder
    private Person() { }
    
    public class PersonBuilder
    {
        private readonly Person _person = new Person();
        
        public PersonBuilder WithFirstName(string firstName)
        {
            _person.FirstName = firstName;
            return this;
        }
        
        public PersonBuilder WithLastName(string lastName)
        {
            _person.LastName = lastName;
            return this;
        }
        
        public PersonBuilder WithAge(int age)
        {
            if (age < 0 || age > 150)
                throw new ArgumentException("Invalid age");
            _person.Age = age;
            return this;
        }
        
        public PersonBuilder WithEmail(string email)
        {
            _person.Email = email;
            return this;
        }
        
        public PersonBuilder WithAddress(string address)
        {
            _person.Address = address;
            return this;
        }
        
        public PersonBuilder WithPhoneNumber(string phone)
        {
            _person.PhoneNumber = phone;
            return this;
        }
        
        public Person Build()
        {
            if (string.IsNullOrEmpty(_person.FirstName))
                throw new InvalidOperationException("FirstName is required");
            if (string.IsNullOrEmpty(_person.LastName))
                throw new InvalidOperationException("LastName is required");
            
            return _person;
        }
    }
}

// Usage
var person = new Person.PersonBuilder()
    .WithFirstName("John")
    .WithLastName("Doe")
    .WithAge(30)
    .WithEmail("john@example.com")
    .Build();
```

## Builder with Validation

### Ensuring Valid Objects
```csharp
public class ConfigurationBuilder
{
    private string _apiUrl;
    private int _timeout = 30;
    private int _maxRetries = 3;
    private bool _enableLogging = false;
    
    public ConfigurationBuilder WithApiUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentException("URL cannot be empty");
        _apiUrl = url;
        return this;
    }
    
    public ConfigurationBuilder WithTimeout(int seconds)
    {
        if (seconds <= 0)
            throw new ArgumentException("Timeout must be positive");
        _timeout = seconds;
        return this;
    }
    
    public ConfigurationBuilder WithMaxRetries(int retries)
    {
        if (retries < 0)
            throw new ArgumentException("Retries cannot be negative");
        _maxRetries = retries;
        return this;
    }
    
    public ConfigurationBuilder WithLogging(bool enable)
    {
        _enableLogging = enable;
        return this;
    }
    
    public Configuration Build()
    {
        if (string.IsNullOrEmpty(_apiUrl))
            throw new InvalidOperationException("ApiUrl is required");
        
        return new Configuration
        {
            ApiUrl = _apiUrl,
            Timeout = _timeout,
            MaxRetries = _maxRetries,
            EnableLogging = _enableLogging
        };
    }
}

public class Configuration
{
    public string ApiUrl { get; set; }
    public int Timeout { get; set; }
    public int MaxRetries { get; set; }
    public bool EnableLogging { get; set; }
}
```

## Record Builders (C# 9+)

### With Expression
```csharp
public record User(string Name, int Age, string Email);

// Built-in with expression
var user1 = new User("Alice", 30, "alice@example.com");
var user2 = user1 with { Age = 31 }; // Creates new copy with Age = 31

// Functional approach
public class UserBuilder
{
    public User Build(string name, int age, string email) =>
        new User(name, age, email);
    
    public User BuildCopy(User existing, string name = null, int? age = null, string email = null) =>
        existing with
        {
            Name = name ?? existing.Name,
            Age = age ?? existing.Age,
            Email = email ?? existing.Email
        };
}
```

## Best Practices

1. **Return this for Chaining**
```csharp
// Good: Enables fluent interface
public SqlBuilder Select(string columns)
{
    _select = columns;
    return this; // Return this
}

// Bad: Breaks chaining
public void Select(string columns)
{
    _select = columns;
    // Can't chain
}
```

2. **Validate in Build Method**
```csharp
// Good: Validation at end
public Result Build()
{
    if (string.IsNullOrEmpty(_required))
        throw new InvalidOperationException("Required field missing");
    return new Result { /* ... */ };
}

// Bad: Building invalid object
public Result Build() => new Result { /* ... */ };
```

3. **Immutable Results**
```csharp
// Good: Returned object is immutable
public class QueryBuilder
{
    public Query Build()
    {
        return new Query(_sql, _parameters); // Immutable
    }
}

// Bad: Mutable result can be modified
public class BadBuilder
{
    public Query Build()
    {
        var query = new Query();
        query.Sql = _sql; // Can be changed!
        return query;
    }
}
```

## Common Mistakes

1. **Not Returning this**
```csharp
// Bad: Can't chain
public class Builder
{
    public Builder Option1(string value)
    {
        // No return this
    }
}

// Good: Fluent chaining
public class GoodBuilder
{
    public GoodBuilder Option1(string value)
    {
        return this;
    }
}
```

2. **Mutable Builders Shared Across Threads**
```csharp
// Bad: Not thread-safe
var builder = new QueryBuilder();
var query1 = builder.Select("*").From("Users").Build();
var query2 = builder.Select("Id").From("Orders").Build(); // Overwrites query1!

// Good: New builder each time
var query1 = new QueryBuilder().Select("*").From("Users").Build();
var query2 = new QueryBuilder().Select("Id").From("Orders").Build();
```

3. **No Validation**
```csharp
// Bad: Can build invalid object
public Result Build() =>
    new Result { Url = _url, Timeout = _timeout }; // No checks!

// Good: Validate requirements
public Result Build()
{
    if (string.IsNullOrEmpty(_url))
        throw new InvalidOperationException("URL required");
    if (_timeout <= 0)
        throw new InvalidOperationException("Timeout must be positive");
    
    return new Result { Url = _url, Timeout = _timeout };
}
```

## Quick Summary
- Builder: Step-by-step object construction
- Fluent interface: return this for chaining
- Separate builder for complex objects
- Validate in Build() method
- Immutable results
- Great for objects with many options
- Improves readability over many constructors
- Provides default values easily
- Enables partial object construction
- More expressive than telescoping constructors

## Resources
- Builder Pattern (Gang of Four)
- Fluent Interface Pattern
- Object Construction Patterns
- C# Records and with expression
