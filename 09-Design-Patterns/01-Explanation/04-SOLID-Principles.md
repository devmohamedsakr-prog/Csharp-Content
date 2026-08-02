# SOLID Principles

## Overview
SOLID principles are five fundamental design principles for writing maintainable, scalable, and testable code.

## Single Responsibility Principle (SRP)

### One Reason to Change
```csharp
// Bad: Multiple responsibilities
public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    
    // Responsibility 1: User data
    public void SaveToDatabase()
    {
        // Save to DB
    }
    
    // Responsibility 2: Email validation
    public bool ValidateEmail()
    {
        return Email.Contains("@");
    }
    
    // Responsibility 3: Logging
    public void LogLogin()
    {
        File.AppendAllText("log.txt", $"{Name} logged in");
    }
}

// Good: Separate concerns
public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class UserRepository
{
    public void Save(User user)
    {
        // Save to DB
    }
}

public class EmailValidator
{
    public bool Validate(string email)
    {
        return email.Contains("@");
    }
}

public class Logger
{
    public void LogLogin(string username)
    {
        File.AppendAllText("log.txt", $"{username} logged in");
    }
}
```

## Open/Closed Principle (OCP)

### Open for Extension, Closed for Modification
```csharp
// Bad: Modify for each new payment type
public class PaymentProcessor
{
    public void ProcessPayment(string type, decimal amount)
    {
        if (type == "CreditCard")
        {
            // Process credit card
        }
        else if (type == "PayPal")
        {
            // Process PayPal
        }
        else if (type == "Bitcoin") // Always modifying!
        {
            // Process Bitcoin
        }
    }
}

// Good: Extend without modifying
public interface IPaymentMethod
{
    void Process(decimal amount);
}

public class CreditCardPayment : IPaymentMethod
{
    public void Process(decimal amount) => Console.WriteLine($"Charged ${amount} to credit card");
}

public class PayPalPayment : IPaymentMethod
{
    public void Process(decimal amount) => Console.WriteLine($"Sent ${amount} via PayPal");
}

public class BitcoinPayment : IPaymentMethod // Add without modifying processor
{
    public void Process(decimal amount) => Console.WriteLine($"Sent {amount / 50000m} BTC");
}

public class PaymentProcessor
{
    private readonly IPaymentMethod _paymentMethod;
    
    public PaymentProcessor(IPaymentMethod paymentMethod)
    {
        _paymentMethod = paymentMethod;
    }
    
    public void Process(decimal amount)
    {
        _paymentMethod.Process(amount); // Same for all types!
    }
}

// Usage
var processor = new PaymentProcessor(new BitcoinPayment());
processor.Process(1000);
```

## Liskov Substitution Principle (LSP)

### Subtypes Must Be Substitutable
```csharp
// Bad: Violates LSP
public class Bird
{
    public virtual void Fly()
    {
        Console.WriteLine("Flying");
    }
}

public class Penguin : Bird
{
    public override void Fly()
    {
        throw new InvalidOperationException("Penguins can't fly");
    }
}

// Breaks contract
public void MakeBirdFly(Bird bird)
{
    bird.Fly(); // Might throw for Penguin!
}

// Good: Respect substitutability
public interface IFlying
{
    void Fly();
}

public interface ISwimming
{
    void Swim();
}

public class Eagle : IFlying
{
    public void Fly() => Console.WriteLine("Eagle flying");
}

public class Penguin : ISwimming
{
    public void Swim() => Console.WriteLine("Penguin swimming");
}

// Code using interfaces stays correct
public void MakeFly(IFlying flyer)
{
    flyer.Fly(); // Always works
}

public void MakeSwim(ISwimming swimmer)
{
    swimmer.Swim(); // Always works
}
```

## Interface Segregation Principle (ISP)

### Many Focused Interfaces
```csharp
// Bad: Too many unrelated methods
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
    void Drive();
    void Manage();
}

public class Robot : IWorker
{
    public void Work() { }
    public void Eat() { throw new NotImplementedException(); } // Forced!
    public void Sleep() { throw new NotImplementedException(); }
    public void Drive() { throw new NotImplementedException(); }
    public void Manage() { throw new NotImplementedException(); }
}

// Good: Segregated interfaces
public interface IWorking
{
    void Work();
}

public interface IEating
{
    void Eat();
}

public interface IResting
{
    void Sleep();
}

public interface IDriving
{
    void Drive();
}

public interface IManaging
{
    void Manage();
}

public class Robot : IWorking
{
    public void Work() { } // Only implements what it needs
}

public class Person : IWorking, IEating, IResting, IDriving, IManaging
{
    public void Work() { }
    public void Eat() { }
    public void Sleep() { }
    public void Drive() { }
    public void Manage() { }
}
```

## Dependency Inversion Principle (DIP)

### Depend on Abstractions, Not Concretions
```csharp
// Bad: Depends on concrete classes
public class EmailService
{
    private readonly GmailProvider _gmailProvider = new GmailProvider();
    
    public void SendEmail(string to, string subject, string body)
    {
        _gmailProvider.Send(to, subject, body);
    }
}

// Can't switch providers, tightly coupled

// Good: Depend on interfaces
public interface IEmailProvider
{
    void Send(string to, string subject, string body);
}

public class GmailProvider : IEmailProvider
{
    public void Send(string to, string subject, string body)
    {
        Console.WriteLine($"Sending via Gmail: {to}");
    }
}

public class SmtpProvider : IEmailProvider
{
    public void Send(string to, string subject, string body)
    {
        Console.WriteLine($"Sending via SMTP: {to}");
    }
}

public class EmailService
{
    private readonly IEmailProvider _provider;
    
    public EmailService(IEmailProvider provider)
    {
        _provider = provider; // Injected
    }
    
    public void SendEmail(string to, string subject, string body)
    {
        _provider.Send(to, subject, body);
    }
}

// Usage - can switch providers easily
var gmailService = new EmailService(new GmailProvider());
var smtpService = new EmailService(new SmtpProvider());
```

## Combining SOLID Principles

### Real-World Example
```csharp
// Domain model (SRP)
public class Order
{
    public int Id { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal Total { get; set; }
}

// Payment interface (DIP, OCP)
public interface IPaymentProcessor
{
    Task<bool> ProcessAsync(decimal amount, string token);
}

// Email interface (DIP, ISP)
public interface IEmailSender
{
    Task SendOrderConfirmationAsync(Order order, string email);
}

// Logger interface (DIP, ISP)
public interface ILogger
{
    void LogOrder(Order order);
}

// Order repository (DIP)
public interface IOrderRepository
{
    Task SaveAsync(Order order);
}

// Order service - focused (SRP), extensible (OCP)
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IEmailSender _emailSender;
    private readonly ILogger _logger;
    
    // All dependencies injected (DIP)
    public OrderService(
        IOrderRepository repository,
        IPaymentProcessor paymentProcessor,
        IEmailSender emailSender,
        ILogger logger)
    {
        _repository = repository;
        _paymentProcessor = paymentProcessor;
        _emailSender = emailSender;
        _logger = logger;
    }
    
    public async Task<Order> CreateOrderAsync(Order order, string paymentToken, string email)
    {
        // Process payment through interface (OCP - can add new payment types)
        var paymentSucceeded = await _paymentProcessor.ProcessAsync(order.Total, paymentToken);
        if (!paymentSucceeded)
            throw new InvalidOperationException("Payment failed");
        
        // Save through interface (DIP)
        await _repository.SaveAsync(order);
        
        // Send email through interface (ISP - just one responsibility)
        await _emailSender.SendOrderConfirmationAsync(order, email);
        
        // Log through interface (DIP)
        _logger.LogOrder(order);
        
        return order;
    }
}

// Different payment implementations (OCP)
public class StripePaymentProcessor : IPaymentProcessor
{
    public async Task<bool> ProcessAsync(decimal amount, string token)
    {
        await Task.Delay(100);
        return true; // Stripe integration
    }
}

public class PayPalPaymentProcessor : IPaymentProcessor
{
    public async Task<bool> ProcessAsync(decimal amount, string token)
    {
        await Task.Delay(100);
        return true; // PayPal integration
    }
}

// DI configuration
var services = new ServiceCollection();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IPaymentProcessor, StripePaymentProcessor>();
services.AddScoped<IEmailSender, EmailSender>();
services.AddScoped<ILogger, ConsoleLogger>();
services.AddScoped<OrderService>();

var provider = services.BuildServiceProvider();
var orderService = provider.GetRequiredService<OrderService>();
```

## Benefits of SOLID

1. **Maintainability** - Code is easier to understand and modify
2. **Extensibility** - Add features without changing existing code
3. **Testability** - Easy to mock dependencies and write tests
4. **Reusability** - Components can be reused in different contexts
5. **Flexibility** - Swap implementations without affecting consumers

## Common Mistakes

1. **Over-Engineering for Future Needs**
```csharp
// Bad: Too many abstractions for simple code
public interface IUserValidator { }
public interface IUserMapper { }
public interface IUserFactory { }

// Good: Simple code stays simple until complexity demands it
public class User
{
    public string Email { get; set; }
    public bool IsValidEmail() => Email.Contains("@");
}
```

2. **Not Actually Using Dependency Injection**
```csharp
// Bad: DIP without DI
public class Service
{
    private readonly IRepository _repo;
    
    public Service(IRepository repo) => _repo = repo;
}

// But always created with
new Service(new Repository()); // No flexibility

// Good: Actually use DI container
services.AddScoped<IRepository, Repository>();
services.AddScoped<Service>();
```

3. **Violating LSP with Inheritance**
```csharp
// Bad: Not truly substitutable
public class Square : Rectangle
{
    // Square breaks Rectangle's setWidth/setHeight contract
}

// Good: Use composition or separate hierarchies
public class Shape { }
public class Rectangle : Shape { }
public class Square : Shape { }
```

## Quick Summary
- SRP: One responsibility per class
- OCP: Open for extension, closed for modification
- LSP: Subtypes must be substitutable
- ISP: Many focused interfaces
- DIP: Depend on abstractions, not concretions
- Work together to create flexible, maintainable code
- Apply incrementally as complexity demands
- Don't over-engineer simple code
- Use dependency injection with SOLID

## Resources
- SOLID Principles (Uncle Bob)
- Design Principles and Design Patterns
- Dependency Injection in .NET
- Clean Architecture in C#
