# Repository Pattern

## Overview
Repository Pattern abstracts data access, providing a collection-like interface to access domain objects while hiding data source details.

## Core Concept

### Basic Repository
```csharp
// Domain model
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

// Repository interface
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task SaveAsync();
}

// Concrete repository
public class UserRepository : IRepository<User>
{
    private readonly DbContext _context;
    
    public UserRepository(DbContext context) => _context = context;
    
    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
    
    public async Task AddAsync(User entity)
    {
        _context.Users.Add(entity);
    }
    
    public async Task UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
    }
    
    public async Task DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
            _context.Users.Remove(user);
    }
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

// Usage
public class UserService
{
    private readonly IRepository<User> _repository;
    
    public UserService(IRepository<User> repository) => _repository = repository;
    
    public async Task<User> GetUserAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task CreateUserAsync(string name, string email)
    {
        var user = new User { Name = name, Email = email };
        await _repository.AddAsync(user);
        await _repository.SaveAsync();
    }
}
```

## Unit of Work Pattern

### Coordinating Multiple Repositories
```csharp
// Unit of Work interface
public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Order> Orders { get; }
    IRepository<Product> Products { get; }
    
    Task<int> SaveAsync();
}

// Unit of Work implementation
public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IRepository<User> _userRepository;
    private IRepository<Order> _orderRepository;
    private IRepository<Product> _productRepository;
    
    public UnitOfWork(DbContext context) => _context = context;
    
    public IRepository<User> Users 
    { 
        get => _userRepository ??= new UserRepository(_context);
    }
    
    public IRepository<Order> Orders 
    { 
        get => _orderRepository ??= new OrderRepository(_context);
    }
    
    public IRepository<Product> Products 
    { 
        get => _productRepository ??= new ProductRepository(_context);
    }
    
    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    
    public void Dispose() => _context?.Dispose();
}

// Usage
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public OrderService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task CreateOrderAsync(int userId, List<int> productIds)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");
        
        var order = new Order { UserId = userId, CreatedDate = DateTime.Now };
        await _unitOfWork.Orders.AddAsync(order);
        
        foreach (var productId in productIds)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            var item = new OrderItem { OrderId = order.Id, ProductId = productId };
            // Add items...
        }
        
        await _unitOfWork.SaveAsync(); // All changes saved together
    }
}
```

## Specialized Repositories

### Custom Queries
```csharp
public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetActiveUsersAsync();
    Task<IEnumerable<User>> SearchAsync(string searchTerm);
}

public class UserRepository : IUserRepository
{
    private readonly DbContext _context;
    
    public UserRepository(DbContext context) => _context = context;
    
    // Generic methods...
    
    // Specialized methods
    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _context.Users.Where(u => u.IsActive).ToListAsync();
    }
    
    public async Task<IEnumerable<User>> SearchAsync(string searchTerm)
    {
        return await _context.Users
            .Where(u => u.Name.Contains(searchTerm) || u.Email.Contains(searchTerm))
            .ToListAsync();
    }
}
```

## In-Memory Repository for Testing

### Mock Repository
```csharp
public class InMemoryUserRepository : IRepository<User>
{
    private readonly List<User> _users = new();
    private int _nextId = 1;
    
    public Task<User> GetByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }
    
    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult(_users.AsEnumerable());
    }
    
    public Task AddAsync(User entity)
    {
        entity.Id = _nextId++;
        _users.Add(entity);
        return Task.CompletedTask;
    }
    
    public Task UpdateAsync(User entity)
    {
        var existing = _users.FirstOrDefault(u => u.Id == entity.Id);
        if (existing != null)
        {
            existing.Name = entity.Name;
            existing.Email = entity.Email;
        }
        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
            _users.Remove(user);
        return Task.CompletedTask;
    }
    
    public Task SaveAsync() => Task.CompletedTask;
}

// Unit testing
[Fact]
public async Task UserService_CreateUser_AddsToRepository()
{
    var repository = new InMemoryUserRepository();
    var service = new UserService(repository);
    
    await service.CreateUserAsync("Alice", "alice@example.com");
    
    var users = await repository.GetAllAsync();
    Assert.Single(users);
}
```

## Best Practices

1. **Keep Repository Generic**
```csharp
// Good: Generic CRUD operations
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

// Specialized queries in derived interface
public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
}
```

2. **Use Dependency Injection**
```csharp
// Good: Injected dependency
public class UserService
{
    private readonly IRepository<User> _repository;
    public UserService(IRepository<User> repository) => _repository = repository;
}

// Bad: Hard-coded dependency
public class UserService
{
    private readonly UserRepository _repository = new UserRepository();
}
```

3. **Implement Unit of Work for Complex Scenarios**
```csharp
// Good: Coordinated transactions
public async Task TransferAsync(int fromUserId, int toUserId, decimal amount)
{
    var fromUser = await _unitOfWork.Users.GetByIdAsync(fromUserId);
    var toUser = await _unitOfWork.Users.GetByIdAsync(toUserId);
    
    fromUser.Balance -= amount;
    toUser.Balance += amount;
    
    await _unitOfWork.SaveAsync(); // Single transaction
}
```

## Common Mistakes

1. **Over-Abstracting Simple Cases**
```csharp
// Bad: Too much abstraction
public interface IRepository<T> { /* 50+ methods */ }

// Good: Focus on essential operations
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

2. **Exposing Data Source in Repository**
```csharp
// Bad: Leaks DbContext
public IQueryable<User> GetQuery() => _context.Users;

// Good: Encapsulate queries
public async Task<IEnumerable<User>> GetActiveAsync()
{
    return await _context.Users.Where(u => u.IsActive).ToListAsync();
}
```

3. **Not Using Unit of Work for Related Changes**
```csharp
// Bad: Multiple saves
await userRepository.AddAsync(user);
await userRepository.SaveAsync();
await orderRepository.AddAsync(order);
await orderRepository.SaveAsync();

// Good: Single save
await _unitOfWork.Users.AddAsync(user);
await _unitOfWork.Orders.AddAsync(order);
await _unitOfWork.SaveAsync();
```

## Quick Summary
- Repository abstracts data access
- Generic interface for common operations
- Specialized repositories for custom queries
- Unit of Work coordinates multiple repositories
- In-memory repositories for testing
- Dependency injection for decoupling
- Hide data source implementation details
- Consistent transaction handling
- Keep repository focused and simple

## Resources
- Repository Pattern (Martin Fowler)
- Unit of Work Pattern
- Data Mapper Pattern
- Active Record vs Repository
