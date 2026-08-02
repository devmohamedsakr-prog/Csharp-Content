# Strategy Pattern

## Overview
Strategy Pattern defines a family of algorithms, encapsulates each, and makes them interchangeable at runtime.

## Basic Strategy

### Algorithm Families
```csharp
// Strategy interface
public interface IPaymentStrategy
{
    void Pay(decimal amount);
}

// Concrete strategies
public class CreditCardPayment : IPaymentStrategy
{
    private string _cardNumber;
    
    public CreditCardPayment(string cardNumber)
    {
        _cardNumber = cardNumber;
    }
    
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Charging ${amount} to card {_cardNumber}");
    }
}

public class PayPalPayment : IPaymentStrategy
{
    private string _email;
    
    public PayPalPayment(string email)
    {
        _email = email;
    }
    
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Sending ${amount} via PayPal to {_email}");
    }
}

public class CryptoPayment : IPaymentStrategy
{
    private string _wallet;
    
    public CryptoPayment(string wallet)
    {
        _wallet = wallet;
    }
    
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Sending {amount} cryptocurrency to {_wallet}");
    }
}

// Context
public class ShoppingCart
{
    private decimal _total;
    private IPaymentStrategy _strategy;
    
    public ShoppingCart(decimal total)
    {
        _total = total;
    }
    
    public void SetPaymentMethod(IPaymentStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public void Checkout()
    {
        if (_strategy == null)
            throw new InvalidOperationException("Payment method not set");
        
        _strategy.Pay(_total);
    }
}

// Usage
var cart = new ShoppingCart(99.99m);

cart.SetPaymentMethod(new CreditCardPayment("1234-5678-9012-3456"));
cart.Checkout(); // Charging $99.99 to card...

cart.SetPaymentMethod(new PayPalPayment("user@example.com"));
cart.Checkout(); // Sending $99.99 via PayPal...
```

## Sorting Strategies

### Flexible Sorting
```csharp
public interface ISortingStrategy
{
    void Sort<T>(List<T> items) where T : IComparable<T>;
}

public class BubbleSortStrategy : ISortingStrategy
{
    public void Sort<T>(List<T> items) where T : IComparable<T>
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = 0; j < items.Count - i - 1; j++)
            {
                if (items[j].CompareTo(items[j + 1]) > 0)
                {
                    var temp = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = temp;
                }
            }
        }
    }
}

public class QuickSortStrategy : ISortingStrategy
{
    public void Sort<T>(List<T> items) where T : IComparable<T>
    {
        QuickSort(items, 0, items.Count - 1);
    }
    
    private void QuickSort<T>(List<T> items, int left, int right) where T : IComparable<T>
    {
        if (left < right)
        {
            int pivot = Partition(items, left, right);
            QuickSort(items, left, pivot - 1);
            QuickSort(items, pivot + 1, right);
        }
    }
    
    private int Partition<T>(List<T> items, int left, int right) where T : IComparable<T>
    {
        T pivot = items[right];
        int i = left - 1;
        
        for (int j = left; j < right; j++)
        {
            if (items[j].CompareTo(pivot) < 0)
            {
                i++;
                var temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }
        
        var t = items[i + 1];
        items[i + 1] = items[right];
        items[right] = t;
        
        return i + 1;
    }
}

public class Sorter
{
    private ISortingStrategy _strategy;
    
    public void SetStrategy(ISortingStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public void Sort<T>(List<T> items) where T : IComparable<T>
    {
        _strategy?.Sort(items);
    }
}

// Usage
var numbers = new List<int> { 5, 2, 8, 1, 9 };

var sorter = new Sorter();

sorter.SetStrategy(new BubbleSortStrategy());
sorter.Sort(numbers); // Bubble sort

sorter.SetStrategy(new QuickSortStrategy());
sorter.Sort(numbers); // Quick sort
```

## Compression Strategies

### Algorithm Selection
```csharp
public interface ICompressionStrategy
{
    byte[] Compress(byte[] data);
    byte[] Decompress(byte[] compressedData);
}

public class ZipCompressionStrategy : ICompressionStrategy
{
    public byte[] Compress(byte[] data)
    {
        using (var output = new MemoryStream())
        {
            using (var gzip = new GZipStream(output, CompressionMode.Compress))
            {
                gzip.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }
    }
    
    public byte[] Decompress(byte[] compressedData)
    {
        using (var input = new MemoryStream(compressedData))
        using (var gzip = new GZipStream(input, CompressionMode.Decompress))
        using (var output = new MemoryStream())
        {
            gzip.CopyTo(output);
            return output.ToArray();
        }
    }
}

public class FileArchiver
{
    private ICompressionStrategy _compression;
    
    public void SetCompressionMethod(ICompressionStrategy strategy)
    {
        _compression = strategy;
    }
    
    public void Archive(string inputFile, string outputFile)
    {
        var data = File.ReadAllBytes(inputFile);
        var compressed = _compression.Compress(data);
        File.WriteAllBytes(outputFile, compressed);
    }
}
```

## Best Practices

1. **Favor Composition Over Inheritance**
```csharp
// Good: Strategy pattern - flexible
public class PaymentProcessor
{
    private IPaymentStrategy _strategy;
    
    public void Process(IPaymentStrategy strategy)
    {
        _strategy = strategy;
        _strategy.Process();
    }
}

// Bad: Inheritance - rigid
public abstract class PaymentProcessor
{
    public abstract void Process();
}

public class CreditCardProcessor : PaymentProcessor
{
    public override void Process() { }
}
```

2. **Use Dependency Injection**
```csharp
// Good: Strategy injected
public class Service
{
    private readonly IStrategy _strategy;
    
    public Service(IStrategy strategy)
    {
        _strategy = strategy;
    }
}

// Bad: Strategy hard-coded
public class Service
{
    private readonly ConcreteStrategy _strategy = new ConcreteStrategy();
}
```

3. **Keep Strategies Stateless When Possible**
```csharp
// Good: Stateless, reusable
public class PaymentStrategy : IPaymentStrategy
{
    public void Pay(PaymentContext context)
    {
        // Use context for state
    }
}

// Acceptable: Immutable state
public class ConfiguredStrategy : IPaymentStrategy
{
    private readonly string _config;
    
    public ConfiguredStrategy(string config)
    {
        _config = config;
    }
}
```

## Common Mistakes

1. **Too Many Strategies**
```csharp
// Bad: Strategy for simple if/else
public interface ISimpleDecision { bool Decide(); }
public class TrueStrategy : ISimpleDecision { public bool Decide() => true; }
public class FalseStrategy : ISimpleDecision { public bool Decide() => false; }

// Good: Just use bool/if
if (someCondition) { }
```

2. **Not Encapsulating Algorithm Complexity**
```csharp
// Bad: Complexity in multiple places
var result1 = Algorithm1();
var result2 = Algorithm1(); // Repeated

// Good: Strategy hides complexity
_strategy.Execute();
_strategy.Execute(); // Clean
```

3. **Runtime Strategy Selection Issues**
```csharp
// Bad: String-based selection (fragile)
var strategy = strategyName switch
{
    "A" => new StrategyA(),
    "B" => new StrategyB(),
    _ => throw new ArgumentException()
};

// Good: Dependency injection with factory
var strategy = _factory.CreateStrategy(type);
```

## Quick Summary
- Strategy: Family of interchangeable algorithms
- Encapsulate each algorithm in separate class
- Context uses strategy interface
- Switch strategies at runtime
- Prefer composition over inheritance
- Use dependency injection
- Each strategy handles one algorithm variant
- Reduces conditional logic
- Makes testing easier
- Follows Open/Closed Principle

## Resources
- Strategy Pattern (Gang of Four)
- Composition vs Inheritance
- Dependency Injection Patterns
- Algorithm Selection at Runtime
