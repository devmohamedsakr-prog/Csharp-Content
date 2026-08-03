# Common OOP Mistakes

## Overview

Common mistakes in object-oriented programming and how to avoid them.

## 1. Inheritance Misuse

### Mistake: Inheritance for Code Reuse Only

```csharp
// Bad - Inheritance just to reuse code
public class Employee : Person
{
    // Only inherits to reuse Name, Age properties
}

// Good - Composition or inheritance for IS-A relationship
public class Employee
{
    public Person Person { get; set; }  // Composition
}

// Or use inheritance appropriately
public class Manager : Employee
{
    // IS-A: Manager IS-A Employee
}
```

### Mistake: Deep Inheritance Hierarchies

```csharp
// Bad - Too many levels
public class A { }
public class B : A { }
public class C : B { }
public class D : C { }
public class E : D { }
public class F : E { }

// Good - Flatten hierarchy
public class Base { }
public class Derived1 : Base { }
public class Derived2 : Base { }
```

## 2. Encapsulation Violations

### Mistake: Public Fields

```csharp
// Bad - Public field, no protection
public class Account
{
    public decimal Balance;  // Can be set to any value
}

account.Balance = -1000;  // Allowed!

// Good - Private field with property
public class Account
{
    private decimal _balance;
    
    public decimal Balance
    {
        get { return _balance; }
        set { _balance = value >= 0 ? value : 0; }
    }
}
```

### Mistake: Over-Exposure

```csharp
// Bad - Too many public methods
public class Order
{
    public void Process() { }
    public void Validate() { }
    public void Calculate() { }
    public void Save() { }
    public void Email() { }
    // All implementation details exposed
}

// Good - Hide implementation
public class Order
{
    public void Submit() { }
    
    private void Validate() { }
    private void Calculate() { }
    private void Save() { }
    private void Email() { }
}
```

## 3. Static Misuse

### Mistake: Static State

```csharp
// Bad - Static state causes problems
public static class UserService
{
    public static User CurrentUser;  // Shared state
}

// Thread-safe issues, testing issues

// Good - Instance-based
public class UserService
{
    public User CurrentUser { get; set; }
}
```

### Mistake: Hiding Static

```csharp
// Bad - Static hiding
public class Bad
{
    public static void Process() { }
}

var obj = new Bad();
obj.Process();  // Misleading - it's static

// Good - Clear static usage
public static class Processor
{
    public static void Process() { }
}

Processor.Process();  // Clear intent
```

## 4. Interface Problems

### Mistake: Fat Interfaces

```csharp
// Bad - Interface does too much
public interface IService
{
    void Create();
    void Read();
    void Update();
    void Delete();
    void Email();
    void Log();
}

// Good - Segregated interfaces
public interface IRepository
{
    void Create();
    void Read();
    void Update();
    void Delete();
}

public interface INotifier
{
    void Email();
}

public interface ILogger
{
    void Log();
}
```

### Mistake: Forcing Implementations

```csharp
// Bad - Implementing unused methods
public interface IShape
{
    void Draw();
    void Rotate();
    void Scale();
}

public class Dot : IShape
{
    public void Draw() { }
    
    // Forced to implement but doesn't apply to Dot
    public void Rotate() { throw new NotImplementedException(); }
    public void Scale() { throw new NotImplementedException(); }
}

// Good - More specific interfaces
public interface IDrawable { void Draw(); }
public interface ITransformable { void Rotate(); void Scale(); }

public class Dot : IDrawable { }
public class Rectangle : IDrawable, ITransformable { }
```

## 5. Design Pattern Misuse

### Mistake: Singleton Abuse

```csharp
// Bad - Over-use of singleton
public static class Logger { }
public static class Configuration { }
public static class Database { }

// Hard to test, couples components

// Good - Inject dependencies
public class Service
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    
    public Service(ILogger logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }
}
```

### Mistake: Factory Anti-Pattern

```csharp
// Bad - Unnecessary factory
public class UserFactory
{
    public static User CreateUser(string name)
    {
        return new User { Name = name };
    }
}

// Good - Direct instantiation
var user = new User { Name = "Alice" };

// Factory useful only for:
// - Complex initialization
// - Multiple types
public interface IRepository { }
public class SqlRepository : IRepository { }
public class MongoRepository : IRepository { }

public class RepositoryFactory
{
    public static IRepository Create(string type)
    {
        return type switch
        {
            "sql" => new SqlRepository(),
            "mongo" => new MongoRepository(),
            _ => throw new ArgumentException()
        };
    }
}
```

## 6. NULL Reference Issues

### Mistake: Null Checking Everywhere

```csharp
// Bad - Excessive null checks
public void Process(User user)
{
    if (user != null && user.Orders != null)
    {
        foreach (var order in user.Orders)
        {
            if (order != null)
            {
                // Process
            }
        }
    }
}

// Good - Design prevents nulls
public void Process(User user)
{
    ArgumentNullException.ThrowIfNull(user);
    
    foreach (var order in user.Orders)
    {
        // Process - Order is never null
    }
}
```

## 7. Virtual Method Issues

### Mistake: Virtual Everything

```csharp
// Bad - Unnecessary virtuals
public class Base
{
    public virtual void Method1() { }
    public virtual void Method2() { }
    public virtual void Method3() { }
    // Everything can be overridden
}

// Good - Virtual only when needed
public class Base
{
    public virtual void Hook() { }  // Intentional override point
    
    public void Template()
    {
        Setup();
        Hook();
        Cleanup();
    }
    
    private void Setup() { }      // Not virtual
    private void Cleanup() { }    // Not virtual
}
```

## 8. Testing Problems

### Mistake: Hard-to-Test Classes

```csharp
// Bad - Difficult to test
public class PaymentService
{
    private Database _db = new();
    private EmailService _email = new();
    
    public void ProcessPayment(decimal amount)
    {
        _db.Save();
        _email.Send();  // Hard to mock
    }
}

// Good - Testable
public class PaymentService
{
    private readonly IRepository _repo;
    private readonly IEmailService _email;
    
    public PaymentService(IRepository repo, IEmailService email)
    {
        _repo = repo;
        _email = email;
    }
    
    public void ProcessPayment(decimal amount)
    {
        _repo.Save();
        _email.Send();  // Easy to mock
    }
}

// Test
var mockRepo = new MockRepository();
var mockEmail = new MockEmailService();
var service = new PaymentService(mockRepo, mockEmail);
```

## 9. Naming Problems

### Mistake: Unclear Names

```csharp
// Bad
public class Thing
{
    public void Do() { }
    public void Handle(object data) { }
}

// Good
public class OrderProcessor
{
    public void ProcessOrder(Order order) { }
    public void HandlePayment(Payment payment) { }
}
```

## 10. Mixing Concerns

### Mistake: Multiple Responsibilities

```csharp
// Bad - Too many concerns
public class UserManager
{
    public void CreateUser() { }       // Business logic
    public void ValidateEmail() { }    // Validation
    public void LogActivity() { }      // Logging
    public void SendEmail() { }        // Email
    public void SaveToDatabase() { }   // Data access
}

// Good - Separated concerns
public class UserService { public void CreateUser() { } }
public class EmailValidator { public bool IsValid(string email) { } }
public class ActivityLogger { public void Log(string message) { } }
public class EmailSender { public void Send(string to) { } }
public class UserRepository { public void Save(User user) { } }
```

## Summary

**10 Common Mistakes:**
1. Inheritance misuse
2. Encapsulation violations
3. Static state abuse
4. Fat interfaces
5. Design pattern misuse
6. Null reference issues
7. Over-virtual methods
8. Hard-to-test design
9. Unclear naming
10. Mixed concerns

## Next Steps

- Review [Best-Practices](../01-Best-Practices/00-Best-Practices.md)
- Study [Interview-Questions](../03-Interview-Questions/00-Interview-Overview.md)
