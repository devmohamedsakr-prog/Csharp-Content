# Factory Pattern

## Overview
Factory Pattern creates objects without specifying exact classes, decoupling object creation from usage.

## Simple Factory

### Basic Implementation
```csharp
public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}

public class FileLogger : ILogger
{
    private readonly string _filePath;
    
    public FileLogger(string filePath) => _filePath = filePath;
    
    public void Log(string message) => File.AppendAllText(_filePath, message + "\n");
}

// Simple factory
public class LoggerFactory
{
    public static ILogger CreateLogger(string type)
    {
        return type switch
        {
            "console" => new ConsoleLogger(),
            "file" => new FileLogger("app.log"),
            _ => throw new ArgumentException($"Unknown logger type: {type}")
        };
    }
}

// Usage
ILogger logger = LoggerFactory.CreateLogger("console");
logger.Log("Hello");
```

## Factory Method Pattern

### Virtual Factory Method
```csharp
public abstract class Document
{
    public abstract void Open();
    public abstract void Save();
    
    // Factory method
    public abstract void Create();
}

public class WordDocument : Document
{
    public override void Open() => Console.WriteLine("Opening Word document");
    public override void Save() => Console.WriteLine("Saving Word document");
    public override void Create() => Console.WriteLine("Creating Word document");
}

public class PdfDocument : Document
{
    public override void Open() => Console.WriteLine("Opening PDF document");
    public override void Save() => Console.WriteLine("Saving PDF document");
    public override void Create() => Console.WriteLine("Creating PDF document");
}

// Application uses factory method
public class Application
{
    protected virtual Document CreateDocument()
    {
        return new WordDocument();
    }
    
    public void NewDocument()
    {
        var doc = CreateDocument();
        doc.Create();
    }
}

// Derived applications override factory method
public class WordApplication : Application
{
    protected override Document CreateDocument() => new WordDocument();
}

public class PdfApplication : Application
{
    protected override Document CreateDocument() => new PdfDocument();
}

// Usage
Application app = new WordApplication();
app.NewDocument(); // Creates Word document
```

## Abstract Factory Pattern

### Family of Related Objects
```csharp
// Abstract products
public interface IButton
{
    void Click();
}

public interface ITextBox
{
    void SetText(string text);
}

// Concrete products - Windows theme
public class WindowsButton : IButton
{
    public void Click() => Console.WriteLine("Windows button clicked");
}

public class WindowsTextBox : ITextBox
{
    public void SetText(string text) => Console.WriteLine($"Windows TextBox: {text}");
}

// Concrete products - macOS theme
public class MacButton : IButton
{
    public void Click() => Console.WriteLine("Mac button clicked");
}

public class MacTextBox : ITextBox
{
    public void SetText(string text) => Console.WriteLine($"Mac TextBox: {text}");
}

// Abstract factory
public interface IUIFactory
{
    IButton CreateButton();
    ITextBox CreateTextBox();
}

// Concrete factories
public class WindowsUIFactory : IUIFactory
{
    public IButton CreateButton() => new WindowsButton();
    public ITextBox CreateTextBox() => new WindowsTextBox();
}

public class MacUIFactory : IUIFactory
{
    public IButton CreateButton() => new MacButton();
    public ITextBox CreateTextBox() => new MacTextBox();
}

// Dialog uses factory to create UI elements
public class Dialog
{
    private readonly IUIFactory _factory;
    
    public Dialog(IUIFactory factory) => _factory = factory;
    
    public void Render()
    {
        var button = _factory.CreateButton();
        var textBox = _factory.CreateTextBox();
        
        button.Click();
        textBox.SetText("Enter your name");
    }
}

// Usage
IUIFactory windowsFactory = new WindowsUIFactory();
Dialog windowsDialog = new Dialog(windowsFactory);
windowsDialog.Render();

IUIFactory macFactory = new MacUIFactory();
Dialog macDialog = new Dialog(macFactory);
macDialog.Render();
```

## Factory Async Pattern

### Async Factory Method
```csharp
public class DatabaseConnection
{
    private string _connectionString;
    
    private DatabaseConnection() { }
    
    // Async factory method
    public static async Task<DatabaseConnection> CreateAsync(string connectionString)
    {
        var connection = new DatabaseConnection { _connectionString = connectionString };
        await connection.InitializeAsync();
        return connection;
    }
    
    private async Task InitializeAsync()
    {
        await Task.Delay(100); // Simulate connection
        Console.WriteLine("Connected to database");
    }
    
    public async Task<IEnumerable<string>> QueryAsync(string sql)
    {
        await Task.Delay(50); // Simulate query
        return new[] { "row1", "row2", "row3" };
    }
}

// Usage
var connection = await DatabaseConnection.CreateAsync("Server=localhost");
var results = await connection.QueryAsync("SELECT * FROM Users");
```

## DI Container Factory

### Using Dependency Injection
```csharp
// Services
public interface IUserRepository
{
    Task<User> GetAsync(int id);
}

public class SqlUserRepository : IUserRepository
{
    public async Task<User> GetAsync(int id)
    {
        await Task.Delay(100);
        return new User { Id = id, Name = "Alice" };
    }
}

public interface IUserService
{
    Task<User> GetUserAsync(int id);
}

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    
    public UserService(IUserRepository repository) => _repository = repository;
    
    public async Task<User> GetUserAsync(int id) => await _repository.GetAsync(id);
}

// DI configuration
public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, SqlUserRepository>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}

// Usage
var services = new ServiceCollection();
services.AddApplicationServices();
var provider = services.BuildServiceProvider();

var userService = provider.GetRequiredService<IUserService>();
var user = await userService.GetUserAsync(1);
```

## Best Practices

1. **Return Interface, Not Concrete Class**
```csharp
// Good: Depends on abstraction
public ILogger CreateLogger(string type)
{
    return type switch
    {
        "console" => new ConsoleLogger(),
        "file" => new FileLogger(),
    };
}

// Bad: Exposes implementation
public ConsoleLogger CreateConsoleLogger()
{
    return new ConsoleLogger();
}
```

2. **Use Factory for Complex Initialization**
```csharp
// Good: Factory handles complexity
public static async Task<DbContext> CreateAsync(string connectionString)
{
    var context = new AppDbContext(connectionString);
    await context.Database.MigrateAsync();
    return context;
}

// Bad: Constructor does heavy work
public AppDbContext(string connectionString)
{
    // initialization and migration here
}
```

3. **Parameterize Factories**
```csharp
// Good: Flexible
public IRepository CreateRepository(RepositoryType type) => type switch
{
    RepositoryType.Sql => new SqlRepository(),
    RepositoryType.MongoDB => new MongoRepository(),
    _ => throw new NotSupportedException()
};

// Bad: Hard-coded options
public IRepository CreateSqlRepository() => new SqlRepository();
public IRepository CreateMongoRepository() => new MongoRepository();
```

## Common Mistakes

1. **Over-Engineering Simple Scenarios**
```csharp
// Bad: Overkill factory for simple creation
public class StringFactory
{
    public string CreateEmpty() => "";
    public string CreateDefault() => "default";
}

// Good: Simple direct creation
string empty = "";
string defaultValue = "default";
```

2. **Leaky Abstractions**
```csharp
// Bad: Client must know types anyway
public ILogger CreateLogger(string type)
{
    // Client code still knows "ConsoleLogger", "FileLogger"
}

// Good: Configuration-driven
public ILogger CreateLogger(IConfiguration config)
{
    var loggerType = config["Logging:Type"];
    // Client doesn't know implementations
}
```

3. **Not Handling Invalid Parameters**
```csharp
// Bad: Silent failures
public ILogger CreateLogger(string type)
{
    switch(type)
    {
        case "console": return new ConsoleLogger();
        default: return null; // Dangerous!
    }
}

// Good: Explicit error handling
public ILogger CreateLogger(string type)
{
    return type switch
    {
        "console" => new ConsoleLogger(),
        "file" => new FileLogger(),
        _ => throw new ArgumentException($"Unknown type: {type}")
    };
}
```

## Quick Summary
- Simple Factory for basic object creation
- Factory Method for inheritance-based creation
- Abstract Factory for families of objects
- Async Factory for async initialization
- Decouple creation from usage
- Return interfaces, not concrete types
- Handle invalid parameters explicitly
- Use DI containers for complex factories
- Avoid over-engineering simple cases

## Resources
- Factory Pattern (Gang of Four)
- Factory Method vs Abstract Factory
- Dependency Injection patterns
