# Integration Testing

## Overview
Integration testing patterns, testing real components, database integration, and test setup strategies.

## Test Infrastructure

### WebApplicationFactory
```csharp
// TestHostBuilder.cs
public class CustomWebApplicationFactory<TProgram> 
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace services with test implementations
            var descriptor = services.SingleOrDefault(d => 
                d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            
            if (descriptor != null)
                services.Remove(descriptor);
            
            // Use in-memory database for tests
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
            
            // Replace HTTP client factory
            services.AddScoped<IHttpClientFactory>(sp =>
                new MockHttpClientFactory());
        });
        
        builder.UseEnvironment("Test");
    }
}

// Program.cs - Make Program public for testing
public partial class Program { }
```

### Test Setup
```csharp
[Collection("Database collection")]
public class UserControllerTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly AppDbContext _context;
    
    public UserControllerTests()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
        _context = GetDbContext();
    }
    
    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await SeedDatabaseAsync();
    }
    
    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        _factory.Dispose();
        _client.Dispose();
    }
    
    private AppDbContext GetDbContext()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }
    
    private async Task SeedDatabaseAsync()
    {
        _context.Users.AddRange(
            new User { Id = 1, Name = "John", Email = "john@example.com" },
            new User { Id = 2, Name = "Jane", Email = "jane@example.com" }
        );
        await _context.SaveChangesAsync();
    }
}
```

## API Testing

### HTTP Integration Tests
```csharp
public class UserControllerIntegrationTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    
    public UserControllerIntegrationTests()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }
    
    public async Task InitializeAsync() { }
    public async Task DisposeAsync() { }
    
    [Fact]
    public async Task GetUser_WithValidId_ReturnsOkAndUser()
    {
        // Arrange
        var userId = 1;
        
        // Act
        var response = await _client.GetAsync($"/api/users/{userId}");
        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserDto>(content);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(user);
        Assert.Equal("John", user.Name);
    }
    
    [Fact]
    public async Task CreateUser_WithValidData_ReturnsCreatedStatus()
    {
        // Arrange
        var createRequest = new CreateUserRequest 
        { 
            Name = "Alice", 
            Email = "alice@example.com" 
        };
        var content = new StringContent(
            JsonSerializer.Serialize(createRequest),
            Encoding.UTF8,
            "application/json"
        );
        
        // Act
        var response = await _client.PostAsync("/api/users", content);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
    }
    
    [Fact]
    public async Task UpdateUser_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var updateRequest = new UpdateUserRequest { Name = "Updated" };
        var content = new StringContent(
            JsonSerializer.Serialize(updateRequest),
            Encoding.UTF8,
            "application/json"
        );
        
        // Act
        var response = await _client.PutAsync("/api/users/999", content);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteUser_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var userId = 1;
        
        // Act
        var response = await _client.DeleteAsync($"/api/users/{userId}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
```

## Database Integration Testing

### Repository Pattern Testing
```csharp
[Collection("Database collection")]
public class UserRepositoryIntegrationTests : IAsyncLifetime
{
    private readonly AppDbContext _context;
    private readonly IUserRepository _repository;
    
    public UserRepositoryIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb" + Guid.NewGuid())
            .Options;
        
        _context = new AppDbContext(options);
        _repository = new UserRepository(_context);
    }
    
    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }
    
    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        _context.Dispose();
    }
    
    [Fact]
    public async Task AddUser_WithValidData_PersistsUser()
    {
        // Arrange
        var user = new User { Name = "John", Email = "john@example.com" };
        
        // Act
        var result = await _repository.AddAsync(user);
        var retrievedUser = await _repository.GetByIdAsync(result.Id);
        
        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal("John", retrievedUser.Name);
        Assert.Equal("john@example.com", retrievedUser.Email);
    }
    
    [Fact]
    public async Task GetUsersByEmail_WithMatchingEmail_ReturnsUser()
    {
        // Arrange
        var user = new User { Name = "Jane", Email = "jane@example.com" };
        await _repository.AddAsync(user);
        
        // Act
        var result = await _repository.GetByEmailAsync("jane@example.com");
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Jane", result.Name);
    }
    
    [Fact]
    public async Task UpdateUser_WithChangedData_PersistsChanges()
    {
        // Arrange
        var user = new User { Name = "Original", Email = "original@example.com" };
        var added = await _repository.AddAsync(user);
        
        // Act
        added.Name = "Updated";
        await _repository.UpdateAsync(added);
        var retrieved = await _repository.GetByIdAsync(added.Id);
        
        // Assert
        Assert.Equal("Updated", retrieved.Name);
    }
    
    [Fact]
    public async Task DeleteUser_WithValidId_RemovesUser()
    {
        // Arrange
        var user = new User { Name = "ToDelete", Email = "delete@example.com" };
        var added = await _repository.AddAsync(user);
        
        // Act
        await _repository.DeleteAsync(added.Id);
        var retrieved = await _repository.GetByIdAsync(added.Id);
        
        // Assert
        Assert.Null(retrieved);
    }
}
```

## Mocking External Services

### Mock Implementations
```csharp
public class MockEmailService : IEmailService
{
    public List<EmailMessage> SentEmails { get; } = new();
    
    public async Task SendAsync(string to, string subject, string body)
    {
        SentEmails.Add(new EmailMessage { To = to, Subject = subject, Body = body });
        await Task.CompletedTask;
    }
}

public class MockPaymentGateway : IPaymentGateway
{
    public decimal LastProcessedAmount { get; set; }
    public bool ShouldFail { get; set; }
    
    public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string cardToken)
    {
        if (ShouldFail)
            return PaymentResult.Failure("Payment failed");
        
        LastProcessedAmount = amount;
        return PaymentResult.Success(Guid.NewGuid().ToString());
    }
}

// Test with mocks
public class OrderServiceIntegrationTests
{
    [Fact]
    public async Task CreateOrder_SendsConfirmationEmail()
    {
        // Arrange
        var emailService = new MockEmailService();
        var paymentGateway = new MockPaymentGateway();
        var orderService = new OrderService(emailService, paymentGateway);
        
        var order = new Order 
        { 
            CustomerEmail = "customer@example.com",
            Amount = 100
        };
        
        // Act
        await orderService.CreateAsync(order);
        
        // Assert
        Assert.Single(emailService.SentEmails);
        Assert.Equal("customer@example.com", emailService.SentEmails[0].To);
    }
}
```

## Fixture-Based Testing

### Shared Fixtures
```csharp
[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // Collection for shared database fixture
}

public class DatabaseFixture : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    public AppDbContext Context { get; private set; }
    public HttpClient Client { get; private set; }
    
    public DatabaseFixture()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        Client = _factory.CreateClient();
        Context = GetDbContext();
    }
    
    public async Task InitializeAsync()
    {
        await Context.Database.EnsureCreatedAsync();
    }
    
    public async Task DisposeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
        Client.Dispose();
        _factory.Dispose();
    }
    
    private AppDbContext GetDbContext()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }
}

// Usage in tests
[Collection("Database collection")]
public class UserServiceTests
{
    private readonly DatabaseFixture _fixture;
    
    public UserServiceTests(DatabaseFixture fixture) => _fixture = fixture;
    
    [Fact]
    public async Task GetUsers_ReturnsAll()
    {
        var users = await _fixture.Context.Users.ToListAsync();
        Assert.NotEmpty(users);
    }
}
```

## Best Practices

1. **Isolate Tests with Fresh Data**
```csharp
// Good: Each test gets fresh database
public async Task InitializeAsync()
{
    await _context.Database.EnsureDeletedAsync();
    await _context.Database.EnsureCreatedAsync();
    await SeedTestDataAsync();
}

// Bad: Shared state between tests
public static List<User> TestUsers { get; set; }
```

2. **Test Real Workflows**
```csharp
// Good: Full workflow
[Fact]
public async Task CompleteOrder_Updates_Inventory_And_Notifies()
{
    var order = await _orderService.CreateAsync(orderRequest);
    var product = await _context.Products.FindAsync(order.Items[0].ProductId);
    
    Assert.True(product.Stock < initialStock);
    Assert.Single(_emailService.SentEmails);
}

// Bad: Testing implementation details
[Fact]
public void OrderService_Calls_InventoryService()
{
    _mockInventory.Verify(x => x.DecrementStock(It.IsAny<int>()));
}
```

3. **Use Descriptive Test Names**
```csharp
// Good: Clear what's being tested
[Fact]
public async Task CreateUser_WithDuplicateEmail_ReturnsBadRequest()

// Bad: Vague name
[Fact]
public async Task TestCreate()
```

## Common Mistakes

1. **Not Cleaning Up Resources**
```csharp
// Bad: No cleanup
[Fact]
public async Task GetUser_ReturnsUser()
{
    var context = new AppDbContext(options);
    // No disposal
}

// Good: Proper cleanup
public async Task DisposeAsync()
{
    await _context.Database.EnsureDeletedAsync();
    _context.Dispose();
}
```

2. **Over-Mocking**
```csharp
// Bad: Mocking everything defeats integration testing
var mockRepository = new Mock<IUserRepository>();
var mockEmailService = new Mock<IEmailService>();
var service = new UserService(mockRepository.Object, mockEmailService.Object);

// Good: Use real implementations where possible
var repository = new UserRepository(_context);
var service = new UserService(repository, mockEmailService);
```

3. **Ignoring Async Properly**
```csharp
// Bad: Not awaiting async operations
public async Task CreateUser_PersistsData()
{
    var result = _repository.AddAsync(user); // Missing await!
    var retrieved = await _repository.GetByIdAsync(result.Id);
}

// Good: Proper async/await
public async Task CreateUser_PersistsData()
{
    var result = await _repository.AddAsync(user);
    var retrieved = await _repository.GetByIdAsync(result.Id);
}
```

## Quick Summary
- Use WebApplicationFactory for full app testing
- Test with real database (in-memory for speed)
- Mock external services only
- Each test should be independent
- Clean database between tests
- Test complete workflows
- Use descriptive test names
- Proper resource cleanup
- Integration tests complement unit tests

## Resources
- WebApplicationFactory Documentation
- xUnit Integration Testing
- Integration Testing Best Practices
- Test Data Builders
