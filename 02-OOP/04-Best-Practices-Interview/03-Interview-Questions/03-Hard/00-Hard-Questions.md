# Hard OOP Interview Questions

## 1. Design a payment processing system using OOP principles

**Answer:**
Consider SOLID principles and design patterns:

```csharp
// Define payment contract
public interface IPaymentProcessor {
    bool Process(decimal amount);
    bool Refund(decimal amount);
}

// Payment processors for different methods
public class CreditCardProcessor : IPaymentProcessor {
    private string _cardNumber;
    
    public CreditCardProcessor(string cardNumber) {
        if (string.IsNullOrEmpty(cardNumber))
            throw new ArgumentException("Card number required");
        _cardNumber = cardNumber;
    }
    
    public bool Process(decimal amount) {
        if (amount <= 0) return false;
        // Process with credit card
        return true;
    }
    
    public bool Refund(decimal amount) {
        // Refund logic
        return true;
    }
}

// Service - depends on interface, not concrete class
public class PaymentService {
    private IPaymentProcessor _processor;
    
    public PaymentService(IPaymentProcessor processor) {
        _processor = processor;
    }
    
    public bool ChargeUser(decimal amount) {
        return _processor.Process(amount);
    }
}

// Usage
IPaymentProcessor processor = new CreditCardProcessor("1234-5678-9012-3456");
var service = new PaymentService(processor);
service.ChargeUser(99.99m);
```

**Design principles applied:**
- Dependency Injection
- Interface Segregation
- Open/Closed Principle
- Strategy Pattern

---

## 2. Implement a generic repository pattern with proper inheritance

**Answer:**

```csharp
// Generic repository base
public abstract class Repository<T> where T : class {
    protected List<T> _data = new();
    
    public virtual void Add(T item) {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        _data.Add(item);
    }
    
    public virtual T GetById(int id) {
        // Override in derived class for specific implementation
        return null;
    }
    
    public virtual IEnumerable<T> GetAll() {
        return _data;
    }
}

// Specific repository
public class UserRepository : Repository<User> {
    public override User GetById(int id) {
        return _data.FirstOrDefault(u => u.Id == id);
    }
    
    public User GetByEmail(string email) {
        return _data.FirstOrDefault(u => u.Email == email);
    }
}

// Usage
var userRepo = new UserRepository();
userRepo.Add(new User { Id = 1, Email = "alice@example.com" });
var user = userRepo.GetByEmail("alice@example.com");
```

**Principles:**
- Generic constraints
- Template Method Pattern
- Inheritance for specialization

---

## 3. Explain SOLID principles with code examples

**Answer:**

**S - Single Responsibility**
```csharp
// Bad
public class Report {
    public void Generate() { }
    public void Save() { }
    public void Email() { }
}

// Good
public class ReportGenerator {
    public void Generate() { }
}

public class ReportRepository {
    public void Save(Report report) { }
}

public class EmailService {
    public void Send(Report report) { }
}
```

**O - Open/Closed**
```csharp
// Bad - closed to extension
public class ReportGenerator {
    public void Generate(string type) {
        if (type == "PDF") { }
        else if (type == "Excel") { }
    }
}

// Good - open to extension
public interface IReportFormat {
    void Generate(Report report);
}

public class PdfReport : IReportFormat {
    public void Generate(Report report) { }
}

public class ExcelReport : IReportFormat {
    public void Generate(Report report) { }
}
```

**L - Liskov Substitution**
```csharp
// Violation
public class Bird {
    public virtual void Fly() { }
}

public class Penguin : Bird {
    public override void Fly() {
        throw new NotImplementedException();  // Violates LSP
    }
}

// Fix
public abstract class Bird { }

public interface IFlying {
    void Fly();
}

public class Eagle : Bird, IFlying {
    public void Fly() { }
}

public class Penguin : Bird {
    // Doesn't implement IFlying - correct!
}
```

**I - Interface Segregation**
```csharp
// Bad - fat interface
public interface IWorker {
    void Work();
    void Manage();
    void Evaluate();
}

// Good - segregated
public interface IWorker {
    void Work();
}

public interface IManager {
    void Manage();
    void Evaluate();
}

public class Developer : IWorker {
    public void Work() { }
}

public class TeamLead : IWorker, IManager {
    public void Work() { }
    public void Manage() { }
    public void Evaluate() { }
}
```

**D - Dependency Inversion**
```csharp
// Bad - depends on concrete class
public class EmailService {
    private SmtpClient _client = new SmtpClient();
}

// Good - depends on abstraction
public interface IEmailClient {
    void Send(string to, string message);
}

public class EmailService {
    private IEmailClient _client;
    
    public EmailService(IEmailClient client) {
        _client = client;
    }
}
```

---

## 4. Design a caching system with inheritance and polymorphism

**Answer:**

```csharp
// Cache interface
public interface ICache {
    void Add<T>(string key, T value);
    T Get<T>(string key);
    void Remove(string key);
}

// Abstract cache with common logic
public abstract class CacheBase : ICache {
    protected Dictionary<string, object> _data = new();
    
    public virtual void Add<T>(string key, T value) {
        _data[key] = value;
    }
    
    public virtual T Get<T>(string key) {
        return _data.ContainsKey(key) ? (T)_data[key] : default;
    }
    
    public virtual void Remove(string key) {
        _data.Remove(key);
    }
}

// Memory cache
public class MemoryCache : CacheBase {
    // Simple in-memory storage
}

// Time-based expiring cache
public class ExpiringCache : CacheBase {
    private Dictionary<string, DateTime> _expiry = new();
    
    public override void Add<T>(string key, T value) {
        base.Add(key, value);
        _expiry[key] = DateTime.Now.AddMinutes(5);
    }
    
    public override T Get<T>(string key) {
        if (_expiry.ContainsKey(key) && DateTime.Now > _expiry[key]) {
            Remove(key);
            return default;
        }
        return base.Get<T>(key);
    }
}

// Usage - depends on interface
public class UserRepository {
    private ICache _cache;
    
    public UserRepository(ICache cache) {
        _cache = cache;
    }
    
    public User GetUser(int id) {
        string key = $"user_{id}";
        
        var user = _cache.Get<User>(key);
        if (user != null) return user;
        
        // Load from database
        user = LoadFromDatabase(id);
        _cache.Add(key, user);
        return user;
    }
    
    private User LoadFromDatabase(int id) {
        // Database access
        return new User { Id = id };
    }
}
```

---

## 5. Explain the difference between composition and inheritance, with examples

**Answer:**

**Inheritance (IS-A)**
```csharp
public class Animal {
    public void Eat() { }
}

public class Dog : Animal {
    // Inherits Eat()
}
```

**Composition (HAS-A)**
```csharp
public class Dog {
    private Tail _tail;  // HAS-A tail
    private Brain _brain; // HAS-A brain
    
    public void Move() {
        _tail.Wag();
    }
}
```

**When to use:**
- **Inheritance**: IS-A relationship (Dog IS-A Animal)
- **Composition**: HAS-A relationship (Dog HAS-A Tail)

**Principle: Favor composition over inheritance**
- More flexible
- Easier to modify
- Avoids deep hierarchies

---

## 6. Design a logging system with multiple handlers using polymorphism

**Answer:**

```csharp
// Logger interface
public abstract class Logger {
    protected LogLevel _level;
    
    public void Log(string message, LogLevel level) {
        if (level >= _level)
            WriteLog(message, level);
    }
    
    protected abstract void WriteLog(string message, LogLevel level);
}

// Console logger
public class ConsoleLogger : Logger {
    public ConsoleLogger(LogLevel level = LogLevel.Info) {
        _level = level;
    }
    
    protected override void WriteLog(string message, LogLevel level) {
        Console.WriteLine($"[{level}] {message}");
    }
}

// File logger
public class FileLogger : Logger {
    private string _filePath;
    
    public FileLogger(string filePath, LogLevel level = LogLevel.Info) {
        _filePath = filePath;
        _level = level;
    }
    
    protected override void WriteLog(string message, LogLevel level) {
        File.AppendAllText(_filePath, $"[{level}] {message}\n");
    }
}

// Composite logger - handles multiple loggers
public class CompositeLogger : Logger {
    private List<Logger> _loggers = new();
    
    public void AddLogger(Logger logger) {
        _loggers.Add(logger);
    }
    
    protected override void WriteLog(string message, LogLevel level) {
        foreach (var logger in _loggers) {
            logger.Log(message, level);
        }
    }
}

// Usage
var composite = new CompositeLogger();
composite.AddLogger(new ConsoleLogger());
composite.AddLogger(new FileLogger("app.log"));
composite.Log("Application started", LogLevel.Info);
```

---

## Summary - Advanced Patterns

- **Repository Pattern**: Generic data access
- **Dependency Injection**: Loose coupling
- **Composite Pattern**: Tree structures
- **Strategy Pattern**: Algorithm selection
- **Template Method**: Define algorithm skeleton
- **SOLID Principles**: Maintainable design

## Next Steps

- Implement these patterns in your projects
- Study design pattern catalogs
- Review production code from open-source
- Practice system design interviews
