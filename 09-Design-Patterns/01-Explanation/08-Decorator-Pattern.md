# Decorator Pattern

## Overview
Decorator Pattern attaches additional responsibilities to an object dynamically, providing flexible alternative to subclassing.

## Basic Decorator

### Wrapping Objects
```csharp
// Component interface
public interface IComponent
{
    string GetDescription();
    decimal GetCost();
}

// Concrete component
public class SimpleCoffee : IComponent
{
    public string GetDescription() => "Simple Coffee";
    public decimal GetCost() => 5.00m;
}

// Decorator base
public abstract class CoffeeDecorator : IComponent
{
    protected IComponent _component;
    
    public CoffeeDecorator(IComponent component)
    {
        _component = component;
    }
    
    public virtual string GetDescription() => _component.GetDescription();
    public virtual decimal GetCost() => _component.GetCost();
}

// Concrete decorators
public class MilkDecorator : CoffeeDecorator
{
    public MilkDecorator(IComponent component) : base(component) { }
    
    public override string GetDescription() => $"{_component.GetDescription()}, Milk";
    public override decimal GetCost() => _component.GetCost() + 0.50m;
}

public class SugarDecorator : CoffeeDecorator
{
    public SugarDecorator(IComponent component) : base(component) { }
    
    public override string GetDescription() => $"{_component.GetDescription()}, Sugar";
    public override decimal GetCost() => _component.GetCost() + 0.25m;
}

public class WhippedCreamDecorator : CoffeeDecorator
{
    public WhippedCreamDecorator(IComponent component) : base(component) { }
    
    public override string GetDescription() => $"{_component.GetDescription()}, Whipped Cream";
    public override decimal GetCost() => _component.GetCost() + 1.00m;
}

// Usage
IComponent coffee = new SimpleCoffee();
Console.WriteLine($"{coffee.GetDescription()}: ${coffee.GetCost()}");
// Output: Simple Coffee: $5.00

coffee = new MilkDecorator(coffee);
coffee = new SugarDecorator(coffee);
coffee = new WhippedCreamDecorator(coffee);
Console.WriteLine($"{coffee.GetDescription()}: ${coffee.GetCost()}");
// Output: Simple Coffee, Milk, Sugar, Whipped Cream: $6.75
```

## Stream Decorators

### Wrapping Functionality
```csharp
// Component
public interface IDataSource
{
    string Read();
    void Write(string data);
}

public class FileDataSource : IDataSource
{
    private string _filename;
    
    public FileDataSource(string filename) => _filename = filename;
    
    public string Read() => File.ReadAllText(_filename);
    public void Write(string data) => File.WriteAllText(_filename, data);
}

// Decorator base
public abstract class DataSourceDecorator : IDataSource
{
    protected IDataSource _wrappee;
    
    public DataSourceDecorator(IDataSource source) => _wrappee = source;
    
    public virtual string Read() => _wrappee.Read();
    public virtual void Write(string data) => _wrappee.Write(data);
}

// Concrete decorators
public class CompressionDecorator : DataSourceDecorator
{
    public CompressionDecorator(IDataSource source) : base(source) { }
    
    public override string Read()
    {
        var compressed = _wrappee.Read();
        return Decompress(compressed); // Decompresses on read
    }
    
    public override void Write(string data)
    {
        var compressed = Compress(data);
        _wrappee.Write(compressed); // Compresses on write
    }
    
    private string Compress(string data) => "COMPRESSED[" + data + "]";
    private string Decompress(string data) => data.Replace("COMPRESSED[", "").Replace("]", "");
}

public class EncryptionDecorator : DataSourceDecorator
{
    public EncryptionDecorator(IDataSource source) : base(source) { }
    
    public override string Read()
    {
        var encrypted = _wrappee.Read();
        return Decrypt(encrypted);
    }
    
    public override void Write(string data)
    {
        var encrypted = Encrypt(data);
        _wrappee.Write(encrypted);
    }
    
    private string Encrypt(string data) => "ENCRYPTED[" + data + "]";
    private string Decrypt(string data) => data.Replace("ENCRYPTED[", "").Replace("]", "");
}

// Usage
IDataSource source = new FileDataSource("data.txt");
source = new CompressionDecorator(source);
source = new EncryptionDecorator(source);

source.Write("Secret Data"); // Compressed then encrypted
var data = source.Read(); // Decrypted then decompressed
Console.WriteLine(data); // "Secret Data"
```

## UI Component Decorators

### Adding Features Dynamically
```csharp
public interface IComponent
{
    void Render();
}

public class TextBox : IComponent
{
    public void Render() => Console.WriteLine("TextBox");
}

public abstract class ComponentDecorator : IComponent
{
    protected IComponent _component;
    
    public ComponentDecorator(IComponent component) => _component = component;
    
    public virtual void Render() => _component.Render();
}

public class ScrollbarDecorator : ComponentDecorator
{
    public ScrollbarDecorator(IComponent component) : base(component) { }
    
    public override void Render()
    {
        base.Render();
        Console.WriteLine("  With Scrollbar");
    }
}

public class BorderDecorator : ComponentDecorator
{
    public BorderDecorator(IComponent component) : base(component) { }
    
    public override void Render()
    {
        Console.WriteLine("===");
        base.Render();
        Console.WriteLine("===");
    }
}

// Usage
IComponent component = new TextBox();
component = new ScrollbarDecorator(component);
component = new BorderDecorator(component);
component.Render();
// Output:
// ===
// TextBox
//   With Scrollbar
// ===
```

## Best Practices

1. **Use Composition Instead of Inheritance**
```csharp
// Good: Decorator - flexible
public abstract class Decorator : IComponent
{
    protected IComponent _wrapped;
    public Decorator(IComponent component) => _wrapped = component;
}

// Bad: Inheritance - rigid
public class TextBoxWithScrollbar : TextBox { }
public class TextBoxWithBorder : TextBox { }
public class TextBoxWithScrollbarAndBorder : TextBox { } // Explosion!
```

2. **Preserve Component Interface**
```csharp
// Good: Decorator implements same interface
public class MyDecorator : IComponent
{
    private IComponent _wrapped;
    public MyDecorator(IComponent component) => _wrapped = component;
    public void Operation() => _wrapped.Operation();
}

// Bad: Changes interface
public class MyDecorator
{
    public void OperationExtended() { }
}
```

3. **Keep Decorators Lightweight**
```csharp
// Good: Decorator adds minimal overhead
public class LoggingDecorator : IService
{
    private readonly IService _inner;
    
    public LoggingDecorator(IService inner) => _inner = inner;
    
    public void Execute()
    {
        Console.WriteLine("Starting...");
        _inner.Execute();
        Console.WriteLine("Done");
    }
}
```

## Common Mistakes

1. **Not Delegating to Wrapped Object**
```csharp
// Bad: Doesn't call wrapped object
public class BadDecorator : IComponent
{
    public void Operation()
    {
        Console.WriteLine("Just me");
        // Forgot to call wrapped!
    }
}

// Good
public class GoodDecorator : IComponent
{
    private IComponent _wrapped;
    
    public void Operation()
    {
        Console.WriteLine("Before");
        _wrapped.Operation(); // Delegate!
        Console.WriteLine("After");
    }
}
```

2. **Changing Component Interface**
```csharp
// Bad: Breaks substitutability
public class MyDecorator : IComponent
{
    public void ExtendedOperation() { } // New method!
}

// Client can't use as IComponent for new method

// Good: Keep interface same
public class GoodDecorator : IComponent
{
    public void Operation() { } // Same interface
}
```

3. **Type Checking**
```csharp
// Bad: Type-checking to use decorator
if (component is LoggingDecorator decorator)
{
    decorator.GetLog();
}

// Good: Use interface
if (component is ILoggable loggable)
{
    var log = loggable.GetLog();
}
```

## Quick Summary
- Decorator: Add behavior dynamically
- Wraps component, implements same interface
- Chain multiple decorators
- Alternative to subclassing
- Preserves single responsibility
- Flexible composition
- Add features at runtime
- Maintains interface contract
- Better than inheritance hierarchies
- Follows Open/Closed Principle

## Resources
- Decorator Pattern (Gang of Four)
- Composition vs Inheritance
- Wrapper Pattern
- Chain of Responsibility vs Decorator
