# OOP Paradigms and Approaches

Different paradigms and methodologies within Object-Oriented Programming.

## 📚 OOP Paradigms

### 1️⃣ Class-Based OOP
Traditional OOP where classes are blueprints for objects.

**Characteristics:**
- Classes define structure and behavior
- Objects are instances of classes
- Inheritance through class hierarchies
- Most common in enterprise applications

**Example:**
```csharp
public class Animal
{
    public string Name { get; set; }
    public virtual void Speak() => Console.WriteLine("Some sound");
}

public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof!");
}

var dog = new Dog { Name = "Buddy" };
dog.Speak();  // Woof!
```

**Best For:**
- Large applications
- Team projects
- Clear structure and hierarchy
- Predictable behavior

---

### 2️⃣ Prototype-Based OOP
Objects inherit directly from other objects (not classes).

**Note:** C# uses class-based, but we can simulate prototype patterns:

```csharp
// Simulating prototype pattern in C#
public interface IPrototype<T>
{
    T Clone();
}

public class Employee : IPrototype<Employee>
{
    public string Name { get; set; }
    public decimal Salary { get; set; }
    public string Department { get; set; }
    
    public Employee Clone()
    {
        return (Employee)this.MemberwiseClone();
    }
}

// Usage - clone prototype
var originalEmployee = new Employee { Name = "John", Salary = 5000 };
var clonedEmployee = originalEmployee.Clone();
clonedEmployee.Name = "Jane";  // Different object
```

**Best For:**
- Creating similar objects
- Avoiding class hierarchies
- Dynamic behavior modification

---

### 3️⃣ Composition-Based OOP
Objects are composed of other objects rather than inheriting from them.

**Example:**
```csharp
// Instead of deep inheritance
public class Vehicle { }
public class Car : Vehicle { }
public class Engine { }
public class Wheel { }

// Use composition
public class Car
{
    public Engine Engine { get; set; }
    public List<Wheel> Wheels { get; set; }
    public Transmission Transmission { get; set; }
    
    public void Drive()
    {
        Engine.Start();
        Transmission.Engage();
    }
}

// Usage
var car = new Car
{
    Engine = new Engine { Type = "V8" },
    Wheels = new List<Wheel> { new(), new(), new(), new() },
    Transmission = new Transmission { Type = "Automatic" }
};
car.Drive();
```

**Benefits:**
- More flexible than inheritance
- Easier to test
- Reduces coupling
- Follows "composition over inheritance"

---

### 4️⃣ Interface-Based OOP
Focus on contracts and interfaces rather than implementations.

**Example:**
```csharp
// Define contracts
public interface IRepository<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

public interface INotificationService
{
    void SendEmail(string email, string message);
    void SendSMS(string phone, string message);
    void SendPushNotification(string userId, string message);
}

// Multiple implementations of same interface
public class EmailNotificationService : INotificationService
{
    public void SendEmail(string email, string message) { }
    public void SendSMS(string phone, string message) { }
    public void SendPushNotification(string userId, string message) { }
}

public class SMSNotificationService : INotificationService
{
    public void SendEmail(string email, string message) { }
    public void SendSMS(string phone, string message) { }
    public void SendPushNotification(string userId, string message) { }
}
```

**Best For:**
- Plugin architectures
- Dependency injection
- Testing and mocking
- Loose coupling

---

### 5️⃣ Mixin-Based OOP
Combining multiple classes into one (mixins provide shared functionality).

**Example:**
```csharp
// Mixin interfaces
public interface IAuditableMixin
{
    DateTime CreatedAt { get; set; }
    DateTime ModifiedAt { get; set; }
    string CreatedBy { get; set; }
}

public interface ISoftDeleteMixin
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
}

public interface ITimestampMixin
{
    void UpdateTimestamp();
}

// Class with mixins
public class User : IAuditableMixin, ISoftDeleteMixin, ITimestampMixin
{
    public string Email { get; set; }
    public string Name { get; set; }
    
    // Auditable
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string CreatedBy { get; set; }
    
    // Soft delete
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    // Timestamp
    public void UpdateTimestamp()
    {
        ModifiedAt = DateTime.UtcNow;
    }
}
```

**Best For:**
- Cross-cutting concerns
- Shared functionality
- Avoiding duplication

---

## 🏗️ OOP Architectural Patterns

### Model-View-Controller (MVC)
Separates application into three interconnected components.

```csharp
// Model - Data and business logic
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// View - Presentation logic (UI)
public class ProductView
{
    public void Display(Product product)
    {
        Console.WriteLine($"{product.Name}: ${product.Price}");
    }
}

// Controller - Handles user input and updates model
public class ProductController
{
    private readonly ProductView _view;
    private readonly Product _model;
    
    public ProductController(Product model, ProductView view)
    {
        _model = model;
        _view = view;
    }
    
    public void UpdateProduct(string name, decimal price)
    {
        _model.Name = name;
        _model.Price = price;
        _view.Display(_model);
    }
}
```

---

### Model-View-ViewModel (MVVM)
Separates UI from business logic with a dedicated view model.

```csharp
// Model
public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

// ViewModel
public class UserViewModel
{
    private User _user;
    
    public string DisplayName
    {
        get => _user.Name;
        set => _user.Name = value;
    }
    
    public bool IsValidEmail => !string.IsNullOrEmpty(_user.Email);
    
    public void Save()
    {
        // Validation and saving logic
    }
}

// View
public class UserView
{
    private UserViewModel _viewModel;
    
    public void Render()
    {
        Console.WriteLine(_viewModel.DisplayName);
    }
}
```

---

### Model-View-Presenter (MVP)
View is passive, Presenter handles all UI logic.

```csharp
// Model
public class LoginModel
{
    public bool Authenticate(string username, string password)
    {
        // Authentication logic
        return true;
    }
}

// View Interface
public interface ILoginView
{
    string Username { get; }
    string Password { get; }
    void ShowSuccess();
    void ShowError(string message);
}

// Presenter
public class LoginPresenter
{
    private readonly ILoginView _view;
    private readonly LoginModel _model;
    
    public LoginPresenter(ILoginView view, LoginModel model)
    {
        _view = view;
        _model = model;
    }
    
    public void Login()
    {
        if (_model.Authenticate(_view.Username, _view.Password))
        {
            _view.ShowSuccess();
        }
        else
        {
            _view.ShowError("Invalid credentials");
        }
    }
}
```

---

### Repository Pattern
Abstracts data access logic.

```csharp
// Generic repository interface
public interface IRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    void Save();
}

// Concrete repository
public class UserRepository : IRepository<User>
{
    private readonly DbContext _context;
    
    public User GetById(int id) => _context.Users.Find(id);
    public IEnumerable<User> GetAll() => _context.Users.ToList();
    public void Add(User entity) => _context.Users.Add(entity);
    public void Update(User entity) => _context.Users.Update(entity);
    public void Delete(int id) => _context.Users.Remove(_context.Users.Find(id));
    public void Save() => _context.SaveChanges();
}
```

---

### Service Layer Pattern
Encapsulates business logic separate from data access.

```csharp
// Service interface
public interface IUserService
{
    User RegisterUser(string email, string password);
    bool Login(string email, string password);
    void UpdateProfile(int userId, UserProfile profile);
}

// Service implementation
public class UserService : IUserService
{
    private readonly IRepository<User> _repository;
    private readonly IEmailService _emailService;
    
    public UserService(IRepository<User> repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }
    
    public User RegisterUser(string email, string password)
    {
        var user = new User { Email = email, Password = HashPassword(password) };
        _repository.Add(user);
        _repository.Save();
        _emailService.SendWelcomeEmail(email);
        return user;
    }
    
    public bool Login(string email, string password)
    {
        var user = _repository.GetAll().FirstOrDefault(u => u.Email == email);
        return user != null && VerifyPassword(password, user.Password);
    }
    
    public void UpdateProfile(int userId, UserProfile profile)
    {
        var user = _repository.GetById(userId);
        user.Profile = profile;
        _repository.Update(user);
        _repository.Save();
    }
    
    private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    private bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}
```

---

## 🎯 Dependency Injection Pattern
Managing object dependencies automatically.

```csharp
// Without DI - tightly coupled
public class OrderService
{
    private readonly PaymentProcessor _paymentProcessor = new();
    
    public void ProcessOrder(Order order)
    {
        _paymentProcessor.Process(order.Total);
    }
}

// With DI - loosely coupled
public class OrderService
{
    private readonly IPaymentProcessor _paymentProcessor;
    
    public OrderService(IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;
    }
    
    public void ProcessOrder(Order order)
    {
        _paymentProcessor.Process(order.Total);
    }
}

// DI Container setup
var services = new ServiceCollection();
services.AddScoped<IPaymentProcessor, CreditCardProcessor>();
services.AddScoped<OrderService>();
var serviceProvider = services.BuildServiceProvider();

var orderService = serviceProvider.GetRequiredService<OrderService>();
```

---

## 📊 Paradigm Comparison

| Paradigm | Best For | Pros | Cons |
|----------|----------|------|------|
| Class-Based | Enterprise apps | Clear structure, Scalable | Rigid hierarchies |
| Composition | Flexible designs | Reusable, Testable | More code upfront |
| Interface-Based | Testable systems | Loose coupling, Swappable | Abstract contracts |
| Mixin-Based | Shared functionality | Reduces duplication | Can get complex |
| MVC | Web applications | Separation of concerns | Steep learning curve |
| Repository | Data access | Abstraction, Testable | Extra layer |
| Dependency Injection | All patterns | Testability, Flexibility | Setup complexity |

---

## 📚 Files in This Section

- `01-Paradigms-Overview.md` - Different OOP approaches
- `02-Class-Based-OOP.md` - Traditional approach
- `03-Composition-vs-Inheritance.md` - Design decision
- `04-Interface-Based-Design.md` - Contract-driven
- `05-Architectural-Patterns.md` - Design patterns
- `06-Repository-Pattern.md` - Data access
- `07-Service-Layer.md` - Business logic
- `08-Dependency-Injection.md` - IoC container

---

## 🚀 Choosing the Right Paradigm

**Ask yourself:**
1. What's the application complexity?
2. Do I need testability?
3. How likely are requirements to change?
4. Is there an established pattern for this domain?
5. What's the team's expertise?

**Rule of Thumb:**
- Start simple with class-based OOP
- Introduce composition as complexity grows
- Use interfaces for contracts and flexibility
- Apply architectural patterns as needed
- Use dependency injection for testability

