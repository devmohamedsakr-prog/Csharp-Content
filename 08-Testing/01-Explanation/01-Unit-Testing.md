# Unit Testing

## Overview
Unit testing verifies individual components work correctly in isolation using frameworks like xUnit, NUnit, or MSTest.

## Basic Test Structure

### xUnit Example
```csharp
public class UserServiceTests
{
    [Fact]
    public void GetUser_WithValidId_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        var expectedName = "Alice";
        
        // Act
        var user = GetUser(userId);
        
        // Assert
        Assert.NotNull(user);
        Assert.Equal(expectedName, user.Name);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetUser_WithMultipleIds_ReturnsCorrectUser(int userId)
    {
        var user = GetUser(userId);
        Assert.NotNull(user);
    }
}
```

## Mocking with Moq

### Creating Mocks
```csharp
public class OrderServiceTests
{
    [Fact]
    public async Task CreateOrder_CallsRepository()
    {
        // Arrange
        var mockRepository = new Mock<IOrderRepository>();
        var mockNotification = new Mock<INotificationService>();
        
        var service = new OrderService(mockRepository.Object, mockNotification.Object);
        var order = new Order { Id = 1, Items = new[] { new OrderItem() } };
        
        // Act
        await service.CreateOrderAsync(order);
        
        // Assert
        mockRepository.Verify(r => r.AddAsync(order), Times.Once);
        mockNotification.Verify(n => n.SendAsync(It.IsAny<string>()), Times.Once);
    }
}
```

### Setup Return Values
```csharp
var mockUserRepository = new Mock<IUserRepository>();

// Setup specific return
mockUserRepository
    .Setup(r => r.GetUserAsync(1))
    .ReturnsAsync(new User { Id = 1, Name = "Alice" });

// Setup with matcher
mockUserRepository
    .Setup(r => r.GetUserAsync(It.IsAny<int>()))
    .ReturnsAsync((int id) => new User { Id = id });

// Setup to throw
mockUserRepository
    .Setup(r => r.GetUserAsync(-1))
    .ThrowsAsync(new ArgumentException("Invalid id"));

var service = new UserService(mockUserRepository.Object);
```

## Test Attributes

### xUnit
```csharp
[Fact] // Single test
public void TestMethod() { }

[Theory] // Parameterized test
[InlineData(1)]
[InlineData(2)]
public void TestMethodWithData(int value) { }

[MemberData(nameof(GetTestData))]
public void TestWithMemberData(int input, string expected) { }

public static TheoryData<int, string> GetTestData =>
    new TheoryData<int, string>
    {
        { 1, "one" },
        { 2, "two" }
    };
```

## Common Assertions

### xUnit Assertions
```csharp
// Equality
Assert.Equal(expected, actual);
Assert.NotEqual(expected, actual);

// Null checks
Assert.Null(value);
Assert.NotNull(value);

// Boolean
Assert.True(condition);
Assert.False(condition);

// Collections
Assert.Contains(item, collection);
Assert.DoesNotContain(item, collection);
Assert.Empty(collection);
Assert.NotEmpty(collection);
Assert.Single(collection);
Assert.Collection(collection, 
    item => Assert.Equal(1, item.Id),
    item => Assert.Equal(2, item.Id));

// Exceptions
var ex = Assert.Throws<ArgumentException>(() => Method());
Assert.Equal("parameter", ex.ParamName);

var ex = await Assert.ThrowsAsync<HttpRequestException>(() => MethodAsync());
```

## Async Testing

### Testing Async Methods
```csharp
[Fact]
public async Task GetUserAsync_WithValidId_ReturnsUserAsync()
{
    // Arrange
    var mockRepository = new Mock<IUserRepository>();
    mockRepository
        .Setup(r => r.GetUserAsync(1))
        .ReturnsAsync(new User { Id = 1, Name = "Alice" });
    
    var service = new UserService(mockRepository.Object);
    
    // Act
    var user = await service.GetUserAsync(1);
    
    // Assert
    Assert.NotNull(user);
    Assert.Equal("Alice", user.Name);
}
```

## Test Fixtures

### Reusable Setup
```csharp
public class UserServiceFixture : IDisposable
{
    public Mock<IUserRepository> MockRepository { get; }
    public UserService Service { get; }
    
    public UserServiceFixture()
    {
        MockRepository = new Mock<IUserRepository>();
        Service = new UserService(MockRepository.Object);
    }
    
    public void Dispose()
    {
        // Cleanup
    }
}

public class UserServiceTests : IClassFixture<UserServiceFixture>
{
    private readonly UserServiceFixture _fixture;
    
    public UserServiceTests(UserServiceFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task GetUserAsync_CallsRepository()
    {
        await _fixture.Service.GetUserAsync(1);
        _fixture.MockRepository.Verify(r => r.GetUserAsync(1), Times.Once);
    }
}
```

## Best Practices

1. **One Assertion Per Test (or Closely Related)**
```csharp
// Good: Single, clear purpose
[Fact]
public void IsValidEmail_WithValidEmail_ReturnsTrue()
{
    Assert.True(Validator.IsValidEmail("test@example.com"));
}

// Acceptable: Related assertions
[Fact]
public void CreateUser_CreatesUserCorrectly()
{
    var user = UserFactory.Create("Alice", 30);
    Assert.NotNull(user);
    Assert.Equal("Alice", user.Name);
    Assert.Equal(30, user.Age);
}
```

2. **Use Descriptive Test Names**
```csharp
// Good
[Fact]
public void CalculateDiscount_WithSeniorCitizen_Returns25Percent() { }

// Bad
[Fact]
public void Test1() { }
```

3. **Keep Tests Independent**
```csharp
// Good: Each test can run alone
[Fact]
public void Test1() { SetupData(); ActAndAssert(); }

[Fact]
public void Test2() { SetupData(); ActAndAssert(); }

// Bad: Test2 depends on Test1 running first
private bool _setup = false;

[Fact]
public void Test1() { _setup = true; }

[Fact]
public void Test2() { Assert.True(_setup); } // Fails if run alone
```

## Common Mistakes

1. **Testing Implementation Details**
```csharp
// Bad: Tests how it's implemented, not what it does
[Fact]
public void GetUser_UsesRepositoryAsync()
{
    var mock = new Mock<IRepository>();
    // Verify internals instead of behavior
    mock.Verify(r => r.GetAsync(1));
}

// Good: Tests behavior
[Fact]
public async Task GetUser_WithValidId_ReturnsUser()
{
    var user = await service.GetUserAsync(1);
    Assert.NotNull(user);
}
```

2. **Not Arranging Enough**
```csharp
// Bad: Ambiguous what's being tested
[Fact]
public void Test()
{
    var user = service.GetUser(1);
    Assert.Equal("Alice", user.Name);
}

// Good: Clear arrange, act, assert
[Fact]
public void GetUser_WithUserHavingNameAlice_ReturnsCorrectName()
{
    // Arrange
    var expectedName = "Alice";
    var userId = 1;
    
    // Act
    var user = service.GetUser(userId);
    
    // Assert
    Assert.Equal(expectedName, user.Name);
}
```

## Quick Summary
- [Fact] for single test, [Theory] for parameterized
- Mock dependencies using Moq
- Arrange-Act-Assert pattern
- One logical assertion per test
- Descriptive test names
- Test behavior, not implementation
- Keep tests independent
- Use fixtures for shared setup

## Resources
- xUnit documentation
- Moq documentation
- Unit Testing Best Practices
