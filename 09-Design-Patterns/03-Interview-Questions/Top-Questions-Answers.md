# Design Patterns - Interview Questions & Answers

## 1. What are design patterns and why are they important?

**Answer:**

Design patterns are proven solutions to common programming problems that promote code reusability and maintainability.

**Benefits**:
- **Reusability**: Proven solutions to recurring problems
- **Maintainability**: Well-known patterns are easier to understand
- **Communication**: Common vocabulary for developers
- **Best Practices**: Encapsulates design expertise
- **Scalability**: Patterns enable flexible architectures

```csharp
// Without pattern - tight coupling
public class ReportGenerator {
    public void GenerateReport(string format) {
        if (format == "PDF") {
            // PDF generation code
        } else if (format == "Excel") {
            // Excel generation code
        } else if (format == "HTML") {
            // HTML generation code
        }
    }
}

// With Strategy pattern - loose coupling
public interface IReportFormatter {
    string Format(ReportData data);
}

public class ReportGenerator {
    private readonly IReportFormatter _formatter;
    
    public ReportGenerator(IReportFormatter formatter) {
        _formatter = formatter;
    }
    
    public string GenerateReport(ReportData data) {
        return _formatter.Format(data);
    }
}
```

---

## 2. What is the Singleton pattern?

**Answer:**

Ensures a class has only one instance and provides a global point of access.

```csharp
// Basic Singleton
public class Logger {
    private static Logger _instance;
    
    private Logger() { }  // Private constructor
    
    public static Logger GetInstance() {
        if (_instance == null) {
            _instance = new Logger();
        }
        return _instance;
    }
}

// Thread-safe Singleton (double-checked locking)
public class Logger {
    private static Logger _instance;
    private static readonly object _lock = new object();
    
    private Logger() { }
    
    public static Logger GetInstance() {
        if (_instance == null) {
            lock (_lock) {
                if (_instance == null) {
                    _instance = new Logger();
                }
            }
        }
        return _instance;
    }
}

// Lazy<T> Singleton (recommended)
public class Logger {
    private static readonly Lazy<Logger> _instance = 
        new Lazy<Logger>(() => new Logger());
    
    private Logger() { }
    
    public static Logger Instance => _instance.Value;
}

// Usage
Logger logger1 = Logger.Instance;
Logger logger2 = Logger.Instance;
// logger1 == logger2  (same instance)
```

**Use Cases**:
- Database connections
- Logger instances
- Configuration managers
- Thread pools

---

## 3. What is the Factory pattern?

**Answer:**

Factory pattern creates objects without specifying their exact classes.

```csharp
// Simple Factory
public class DatabaseFactory {
    public static IDatabase CreateDatabase(string dbType) {
        switch (dbType.ToLower()) {
            case "sqlserver":
                return new SqlServerDatabase();
            case "mysql":
                return new MySqlDatabase();
            case "postgresql":
                return new PostgreSqlDatabase();
            default:
                throw new ArgumentException("Unknown database type");
        }
    }
}

// Usage
IDatabase db = DatabaseFactory.CreateDatabase("sqlserver");

// Factory Method Pattern
public abstract class DatabaseCreator {
    public abstract IDatabase CreateDatabase();
    
    public void Connect() {
        var db = CreateDatabase();
        db.Open();
    }
}

public class SqlServerDatabaseCreator : DatabaseCreator {
    public override IDatabase CreateDatabase() {
        return new SqlServerDatabase();
    }
}

public class MySqlDatabaseCreator : DatabaseCreator {
    public override IDatabase CreateDatabase() {
        return new MySqlDatabase();
    }
}

// Usage
DatabaseCreator creator = new SqlServerDatabaseCreator();
creator.Connect();  // Uses SqlServerDatabase
```

---

## 4. What is the Builder pattern?

**Answer:**

Builder pattern constructs complex objects step by step.

```csharp
// Object being built
public class DatabaseConnection {
    public string Server { get; set; }
    public string Database { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool UseEncryption { get; set; }
}

// Builder
public class DatabaseConnectionBuilder {
    private readonly DatabaseConnection _connection = new();
    
    public DatabaseConnectionBuilder WithServer(string server) {
        _connection.Server = server;
        return this;
    }
    
    public DatabaseConnectionBuilder WithDatabase(string database) {
        _connection.Database = database;
        return this;
    }
    
    public DatabaseConnectionBuilder WithPort(int port) {
        _connection.Port = port;
        return this;
    }
    
    public DatabaseConnectionBuilder WithUsername(string username) {
        _connection.Username = username;
        return this;
    }
    
    public DatabaseConnectionBuilder WithPassword(string password) {
        _connection.Password = password;
        return this;
    }
    
    public DatabaseConnectionBuilder UseEncryption() {
        _connection.UseEncryption = true;
        return this;
    }
    
    public DatabaseConnection Build() {
        if (string.IsNullOrEmpty(_connection.Server)) {
            throw new InvalidOperationException("Server is required");
        }
        return _connection;
    }
}

// Usage
var connection = new DatabaseConnectionBuilder()
    .WithServer("localhost")
    .WithDatabase("MyDb")
    .WithPort(1433)
    .WithUsername("admin")
    .UseEncryption()
    .Build();

// Alternative: C# with expression-bodied constructor
public class QueryBuilder {
    public string Query { get; set; }
    public QueryBuilder Select(string columns) { Query += $"SELECT {columns}"; return this; }
    public QueryBuilder From(string table) { Query += $" FROM {table}"; return this; }
    public QueryBuilder Where(string condition) { Query += $" WHERE {condition}"; return this; }
    public string Build() => Query;
}

string sql = new QueryBuilder()
    .Select("*")
    .From("Users")
    .Where("Age > 18")
    .Build();
```

---

## 5. What is the Strategy pattern?

**Answer:**

Strategy pattern defines a family of algorithms and makes them interchangeable.

```csharp
// Strategy interface
public interface IPaymentStrategy {
    bool ProcessPayment(decimal amount);
}

// Concrete strategies
public class CreditCardPayment : IPaymentStrategy {
    private string _cardNumber;
    
    public CreditCardPayment(string cardNumber) {
        _cardNumber = cardNumber;
    }
    
    public bool ProcessPayment(decimal amount) {
        Console.WriteLine($"Processing credit card payment: ${amount}");
        return true;
    }
}

public class PayPalPayment : IPaymentStrategy {
    private string _email;
    
    public PayPalPayment(string email) {
        _email = email;
    }
    
    public bool ProcessPayment(decimal amount) {
        Console.WriteLine($"Processing PayPal payment: ${amount}");
        return true;
    }
}

public class BitcoinPayment : IPaymentStrategy {
    private string _walletAddress;
    
    public BitcoinPayment(string walletAddress) {
        _walletAddress = walletAddress;
    }
    
    public bool ProcessPayment(decimal amount) {
        Console.WriteLine($"Processing Bitcoin payment: ${amount}");
        return true;
    }
}

// Context
public class ShoppingCart {
    private IPaymentStrategy _paymentStrategy;
    
    public void SetPaymentStrategy(IPaymentStrategy strategy) {
        _paymentStrategy = strategy;
    }
    
    public bool Checkout(decimal amount) {
        if (_paymentStrategy == null) {
            throw new InvalidOperationException("Payment strategy not set");
        }
        return _paymentStrategy.ProcessPayment(amount);
    }
}

// Usage
var cart = new ShoppingCart();

cart.SetPaymentStrategy(new CreditCardPayment("1234-5678-9012-3456"));
cart.Checkout(99.99);

cart.SetPaymentStrategy(new PayPalPayment("user@example.com"));
cart.Checkout(99.99);
```

---

## 6. What is the Observer pattern?

**Answer:**

Observer pattern defines a one-to-many relationship where multiple observers listen to one subject.

```csharp
// Subject/Observable
public class StockPrice {
    private decimal _price;
    private List<IStockPriceObserver> _observers = new();
    
    public decimal Price {
        get => _price;
        set {
            if (_price != value) {
                _price = value;
                NotifyObservers();
            }
        }
    }
    
    public void Subscribe(IStockPriceObserver observer) {
        _observers.Add(observer);
    }
    
    public void Unsubscribe(IStockPriceObserver observer) {
        _observers.Remove(observer);
    }
    
    private void NotifyObservers() {
        foreach (var observer in _observers) {
            observer.OnPriceChanged(_price);
        }
    }
}

// Observer interface
public interface IStockPriceObserver {
    void OnPriceChanged(decimal newPrice);
}

// Concrete observers
public class TradingBot : IStockPriceObserver {
    public void OnPriceChanged(decimal newPrice) {
        Console.WriteLine($"Trading bot: Price changed to ${newPrice}, executing trade");
    }
}

public class PriceDisplay : IStockPriceObserver {
    public void OnPriceChanged(decimal newPrice) {
        Console.WriteLine($"Display: Stock price updated to ${newPrice}");
    }
}

// Usage
var stock = new StockPrice();
var display = new PriceDisplay();
var bot = new TradingBot();

stock.Subscribe(display);
stock.Subscribe(bot);

stock.Price = 100;  // Notifies both observers
// Output: Display: Stock price updated to $100
//         Trading bot: Price changed to $100, executing trade
```

---

## 7. What is the Decorator pattern?

**Answer:**

Decorator pattern adds responsibilities to objects dynamically.

```csharp
// Component interface
public interface IComponent {
    void Operation();
}

// Concrete component
public class ConcreteComponent : IComponent {
    public void Operation() {
        Console.WriteLine("ConcreteComponent operation");
    }
}

// Decorator base
public abstract class Decorator : IComponent {
    protected IComponent _component;
    
    public Decorator(IComponent component) {
        _component = component;
    }
    
    public virtual void Operation() {
        _component.Operation();
    }
}

// Concrete decorators
public class LoggingDecorator : Decorator {
    public LoggingDecorator(IComponent component) : base(component) { }
    
    public override void Operation() {
        Console.WriteLine("Logging: Starting operation");
        base.Operation();
        Console.WriteLine("Logging: Operation completed");
    }
}

public class ValidationDecorator : Decorator {
    public ValidationDecorator(IComponent component) : base(component) { }
    
    public override void Operation() {
        Console.WriteLine("Validation: Checking data");
        if (IsValid()) {
            base.Operation();
        }
    }
    
    private bool IsValid() => true;
}

// Usage - stack decorators
IComponent component = new ConcreteComponent();
component = new LoggingDecorator(component);
component = new ValidationDecorator(component);
component.Operation();
// Output: Logging: Starting operation
//         Validation: Checking data
//         ConcreteComponent operation
//         Logging: Operation completed
```

**Real Example - Stream Decorators**:
```csharp
Stream stream = new FileStream("file.txt", FileMode.Read);
stream = new BufferedStream(stream);  // Decorator
stream = new CryptoStream(stream, algorithm, mode);  // Another decorator
```

---

## 8. What is the Adapter pattern?

**Answer:**

Adapter pattern converts interface of a class to another interface clients expect.

```csharp
// Incompatible interface
public class LegacySystem {
    public void OldMethod() {
        Console.WriteLine("Using old system method");
    }
}

// Target interface
public interface IModernSystem {
    void NewMethod();
}

// Adapter
public class SystemAdapter : IModernSystem {
    private readonly LegacySystem _legacySystem;
    
    public SystemAdapter(LegacySystem legacySystem) {
        _legacySystem = legacySystem;
    }
    
    public void NewMethod() {
        _legacySystem.OldMethod();  // Translates old to new
    }
}

// Usage
LegacySystem legacy = new LegacySystem();
IModernSystem modern = new SystemAdapter(legacy);
modern.NewMethod();  // Uses legacy under the hood
```

---

## 9. What is the Template Method pattern?

**Answer:**

Template Method defines skeleton of algorithm in base class, letting subclasses override steps.

```csharp
// Abstract base class
public abstract class DataProcessor {
    // Template method - defines algorithm structure
    public void Process(string input) {
        var data = ReadData(input);
        var processed = ProcessData(data);
        WriteData(processed);
    }
    
    protected abstract string ReadData(string input);
    protected abstract string ProcessData(string data);
    protected abstract void WriteData(string data);
}

// Concrete implementations
public class CsvDataProcessor : DataProcessor {
    protected override string ReadData(string input) {
        Console.WriteLine("Reading CSV data");
        return "csv data";
    }
    
    protected override string ProcessData(string data) {
        Console.WriteLine("Processing CSV data");
        return data.ToUpper();
    }
    
    protected override void WriteData(string data) {
        Console.WriteLine($"Writing processed CSV: {data}");
    }
}

public class JsonDataProcessor : DataProcessor {
    protected override string ReadData(string input) {
        Console.WriteLine("Reading JSON data");
        return "json data";
    }
    
    protected override string ProcessData(string data) {
        Console.WriteLine("Processing JSON data");
        return data.ToUpper();
    }
    
    protected override void WriteData(string data) {
        Console.WriteLine($"Writing processed JSON: {data}");
    }
}

// Usage
DataProcessor processor = new CsvDataProcessor();
processor.Process("input.csv");  // Uses CSV-specific implementations
```

---

## 10. What is the State pattern?

**Answer:**

State pattern allows object to change behavior when internal state changes.

```csharp
// State interface
public interface IOrderState {
    void Process(Order order);
    void Cancel(Order order);
}

// Concrete states
public class PendingState : IOrderState {
    public void Process(Order order) {
        Console.WriteLine("Processing pending order");
        order.SetState(new ShippedState());
    }
    
    public void Cancel(Order order) {
        Console.WriteLine("Order cancelled from pending state");
        order.SetState(new CancelledState());
    }
}

public class ShippedState : IOrderState {
    public void Process(Order order) {
        Console.WriteLine("Order already shipped");
    }
    
    public void Cancel(Order order) {
        Console.WriteLine("Cannot cancel - order already shipped");
    }
}

public class DeliveredState : IOrderState {
    public void Process(Order order) {
        Console.WriteLine("Order already delivered");
    }
    
    public void Cancel(Order order) {
        Console.WriteLine("Cannot cancel - order already delivered");
    }
}

public class CancelledState : IOrderState {
    public void Process(Order order) {
        Console.WriteLine("Cannot process - order cancelled");
    }
    
    public void Cancel(Order order) {
        Console.WriteLine("Order already cancelled");
    }
}

// Context
public class Order {
    private IOrderState _state;
    
    public Order() {
        _state = new PendingState();
    }
    
    public void SetState(IOrderState state) {
        _state = state;
    }
    
    public void Process() {
        _state.Process(this);
    }
    
    public void Cancel() {
        _state.Cancel(this);
    }
}

// Usage
var order = new Order();
order.Process();  // Pending -> Shipped
order.Process();  // Already shipped
order.Cancel();   // Cannot cancel
```

---

## Quick Tips for Interview

✓ Know difference between Creational, Structural, Behavioral patterns
✓ Explain Singleton and when to use it
✓ Understand Factory pattern variations
✓ Know Builder for complex objects
✓ Explain Strategy pattern for algorithms
✓ Understand Observer pattern for events
✓ Know Decorator for adding responsibilities
✓ Understand Adapter for incompatible interfaces
✓ Know Template Method for algorithm skeletons
✓ Understand State pattern for state management
✓ Be ready to code examples
✓ Know SOLID principles and how patterns relate
