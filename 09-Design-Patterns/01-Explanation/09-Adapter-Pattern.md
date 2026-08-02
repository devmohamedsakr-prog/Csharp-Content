# Adapter Pattern

## Overview
Adapter Pattern converts interface of a class into another interface clients expect, enabling classes with incompatible interfaces to work together.

## Class Adapter

### Inheritance-Based
```csharp
// Target interface
public interface ITarget
{
    void Request();
}

// Adaptee - existing class with different interface
public class Adaptee
{
    public void SpecificRequest()
    {
        Console.WriteLine("Adaptee specific request");
    }
}

// Adapter using inheritance
public class ClassAdapter : Adaptee, ITarget
{
    public void Request()
    {
        // Adapt the interface
        SpecificRequest();
    }
}

// Usage
ITarget target = new ClassAdapter();
target.Request(); // Output: Adaptee specific request
```

## Object Adapter

### Composition-Based (Preferred)
```csharp
// Same interfaces as above
public interface ITarget
{
    void Request();
}

public class Adaptee
{
    public void SpecificRequest()
    {
        Console.WriteLine("Adaptee specific request");
    }
}

// Adapter using composition
public class ObjectAdapter : ITarget
{
    private Adaptee _adaptee;
    
    public ObjectAdapter(Adaptee adaptee)
    {
        _adaptee = adaptee;
    }
    
    public void Request()
    {
        _adaptee.SpecificRequest();
    }
}

// Usage
var adaptee = new Adaptee();
ITarget target = new ObjectAdapter(adaptee);
target.Request(); // Output: Adaptee specific request
```

## Real-World Example

### Legacy to Modern API
```csharp
// Legacy API (can't modify)
public class LegacyPaymentProcessor
{
    public bool ProcessPayment(string accountNumber, decimal amount)
    {
        Console.WriteLine($"Processing ${amount} via legacy system");
        return true;
    }
}

// New API interface
public interface IModernPaymentProcessor
{
    Task<PaymentResult> ProcessAsync(PaymentInfo info);
}

public class PaymentInfo
{
    public string AccountNumber { get; set; }
    public decimal Amount { get; set; }
}

public class PaymentResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
}

// Adapter
public class LegacyPaymentAdapter : IModernPaymentProcessor
{
    private readonly LegacyPaymentProcessor _legacy;
    
    public LegacyPaymentAdapter(LegacyPaymentProcessor legacy)
    {
        _legacy = legacy;
    }
    
    public Task<PaymentResult> ProcessAsync(PaymentInfo info)
    {
        // Adapt legacy sync method to async interface
        var success = _legacy.ProcessPayment(info.AccountNumber, info.Amount);
        
        return Task.FromResult(new PaymentResult
        {
            Success = success,
            Message = success ? "Payment processed" : "Payment failed"
        });
    }
}

// Usage
var legacyProcessor = new LegacyPaymentProcessor();
IModernPaymentProcessor adapter = new LegacyPaymentAdapter(legacyProcessor);

var result = await adapter.ProcessAsync(new PaymentInfo
{
    AccountNumber = "1234-5678",
    Amount = 100m
});
```

## Two-Way Adapter

### Bidirectional Adaptation
```csharp
// Interface A
public interface IFormatA
{
    string FormatA();
}

// Interface B
public interface IFormatB
{
    string FormatB();
}

// Adapter supporting both
public class TwoWayAdapter : IFormatA, IFormatB
{
    private string _data;
    
    public TwoWayAdapter(string data) => _data = data;
    
    public string FormatA() => _data.ToUpper();
    public string FormatB() => _data.ToLower();
}

// Usage
var adapter = new TwoWayAdapter("Hello");
IFormatA formatA = adapter;
IFormatB formatB = adapter;

Console.WriteLine(formatA.FormatA()); // HELLO
Console.WriteLine(formatB.FormatB()); // hello
```

## Library Integration

### Adapting Third-Party APIs
```csharp
// Our interface
public interface ILogger
{
    void Log(string message);
    void Error(string message);
}

// Third-party library (can't modify)
public class ThirdPartyLogger
{
    public void WriteInfo(string msg) => Console.WriteLine($"INFO: {msg}");
    public void WriteError(string msg) => Console.WriteLine($"ERROR: {msg}");
}

// Adapter
public class ThirdPartyLoggerAdapter : ILogger
{
    private readonly ThirdPartyLogger _logger;
    
    public ThirdPartyLoggerAdapter(ThirdPartyLogger logger)
    {
        _logger = logger;
    }
    
    public void Log(string message) => _logger.WriteInfo(message);
    public void Error(string message) => _logger.WriteError(message);
}

// Usage
ILogger logger = new ThirdPartyLoggerAdapter(new ThirdPartyLogger());
logger.Log("Application started");
logger.Error("An error occurred");
```

## Best Practices

1. **Prefer Composition Over Inheritance**
```csharp
// Good: Object adapter (composition)
public class Adapter : ITarget
{
    private Adaptee _adaptee;
    public Adapter(Adaptee adaptee) => _adaptee = adaptee;
}

// Less flexible: Class adapter (inheritance)
public class Adapter : Adaptee, ITarget
{
}
```

2. **Keep Adapter Simple**
```csharp
// Good: Single responsibility
public class SimpleAdapter : ITarget
{
    private Adaptee _adaptee;
    
    public void Request() => _adaptee.SpecificRequest();
}

// Bad: Complex logic
public class ComplexAdapter : ITarget
{
    public void Request()
    {
        // 50 lines of complex logic
    }
}
```

3. **Use Interface Segregation**
```csharp
// Good: Specific interfaces
public interface IReadAdapter
{
    T Read<T>();
}

public interface IWriteAdapter
{
    void Write<T>(T data);
}

// Bad: Too broad
public interface IAdapter
{
    void Read();
    void Write();
    void Delete();
    void Update();
}
```

## Common Mistakes

1. **Modifying Adaptee**
```csharp
// Bad: Changes existing class
public class Adaptee
{
    public void SpecificRequest() { } // Modified!
}

// Good: Leave adaptee alone
public class Adapter : ITarget
{
    private Adaptee _adaptee;
    public void Request() => _adaptee.SpecificRequest();
}
```

2. **Making Adapter Too Complex**
```csharp
// Bad: Adapter does more than adapt
public class BadAdapter : ITarget
{
    private Adaptee _adaptee;
    
    public void Request()
    {
        _adaptee.SpecificRequest();
        // Business logic here
        // Validation here
        // Logging here
    }
}

// Good: Only adapter work
public class GoodAdapter : ITarget
{
    private Adaptee _adaptee;
    
    public void Request() => _adaptee.SpecificRequest();
}
```

3. **Not Handling Incompatibilities**
```csharp
// Bad: Ignores incompatibility
public class BadAdapter : ITarget
{
    private Adaptee _adaptee; // Different behavior
    
    public void Request() { } // Doesn't call adaptee
}

// Good: Properly maps behavior
public class GoodAdapter : ITarget
{
    private Adaptee _adaptee;
    
    public void Request() => _adaptee.SpecificRequest();
}
```

## Quick Summary
- Adapter: Convert incompatible interfaces
- Object adapter: composition (preferred)
- Class adapter: inheritance (less flexible)
- Wraps adaptee with target interface
- Enables code reuse without modification
- Single responsibility
- Clean architecture when integrating legacy code
- Non-invasive way to work with existing classes
- Keep adapters simple and focused

## Resources
- Adapter Pattern (Gang of Four)
- Class vs Object Adapter
- Wrapper Pattern
- Facade vs Adapter
