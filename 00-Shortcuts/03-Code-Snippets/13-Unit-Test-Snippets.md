# Unit Test Snippets

Common patterns for unit testing in C#.

## Test Class Structure (XUnit)

**Pattern:**
```csharp
public class CalculatorTests
{
    [Fact]
    public void Add_WithTwoNumbers_ReturnsSum()
    {
        // Arrange
        var calculator = new Calculator();
        
        // Act
        var result = calculator.Add(2, 3);
        
        // Assert
        Assert.Equal(5, result);
    }
}
```

---

## Test Class Structure (NUnit)

**Pattern:**
```csharp
[TestFixture]
public class CalculatorTests
{
    private Calculator _calculator;
    
    [SetUp]
    public void Setup()
    {
        _calculator = new Calculator();
    }
    
    [Test]
    public void Add_WithTwoNumbers_ReturnsSum()
    {
        // Arrange
        // Act
        var result = _calculator.Add(2, 3);
        
        // Assert
        Assert.AreEqual(5, result);
    }
}
```

---

## Arrange-Act-Assert (AAA) Pattern

**Pattern:**
```csharp
[Fact]
public void ProcessOrder_WithValidOrder_MarkAsProcessed()
{
    // Arrange
    var order = new Order { Id = 1, Total = 100 };
    var processor = new OrderProcessor();
    
    // Act
    processor.Process(order);
    
    // Assert
    Assert.True(order.IsProcessed);
}
```

---

## Parameterized Tests (XUnit - Theory)

**Pattern:**
```csharp
[Theory]
[InlineData(2, 3, 5)]
[InlineData(0, 0, 0)]
[InlineData(-1, 1, 0)]
public void Add_WithVariousNumbers_ReturnsCorrectSum(int a, int b, int expected)
{
    var calculator = new Calculator();
    var result = calculator.Add(a, b);
    Assert.Equal(expected, result);
}
```

**With MemberData:**
```csharp
[Theory]
[MemberData(nameof(GetTestData))]
public void Divide_WithVariousNumbers_ReturnsCorrectResult(int a, int b, int expected)
{
    var result = _calculator.Divide(a, b);
    Assert.Equal(expected, result);
}

public static IEnumerable<object[]> GetTestData()
{
    yield return new object[] { 10, 2, 5 };
    yield return new object[] { 20, 4, 5 };
    yield return new object[] { 0, 1, 0 };
}
```

---

## Parameterized Tests (NUnit - TestCase)

**Pattern:**
```csharp
[TestCase(2, 3, 5)]
[TestCase(0, 0, 0)]
[TestCase(-1, 1, 0)]
public void Add_WithVariousNumbers_ReturnsCorrectSum(int a, int b, int expected)
{
    var result = _calculator.Add(a, b);
    Assert.AreEqual(expected, result);
}
```

---

## Exception Testing

**XUnit:**
```csharp
[Fact]
public void Divide_ByZero_ThrowsDivideByZeroException()
{
    var calculator = new Calculator();
    
    Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
}

[Fact]
public void Divide_ByZero_ThrowsWithMessage()
{
    var calculator = new Calculator();
    
    var exception = Assert.Throws<ArgumentException>(
        () => calculator.Divide(10, 0));
    
    Assert.Contains("Cannot divide by zero", exception.Message);
}
```

**NUnit:**
```csharp
[Test]
public void Divide_ByZero_ThrowsDivideByZeroException()
{
    Assert.Throws<DivideByZeroException>(() => _calculator.Divide(10, 0));
}

[Test]
public void Divide_ByZero_ThrowsWithMessage()
{
    var exception = Assert.Throws<ArgumentException>(
        () => _calculator.Divide(10, 0));
    
    Assert.That(exception.Message, Does.Contain("Cannot divide by zero"));
}
```

---

## Setup & Teardown (XUnit)

**Pattern:**
```csharp
public class DatabaseTests : IDisposable
{
    private TestDatabase _db;
    
    public DatabaseTests()
    {
        _db = new TestDatabase();
        _db.Initialize();
    }
    
    [Fact]
    public void Insert_WithValidData_AddsToDatabase()
    {
        _db.Insert(new User { Name = "John" });
        
        Assert.NotEmpty(_db.GetAll());
    }
    
    public void Dispose()
    {
        _db?.Cleanup();
    }
}
```

---

## Setup & Teardown (NUnit)

**Pattern:**
```csharp
[TestFixture]
public class DatabaseTests
{
    private TestDatabase _db;
    
    [SetUp]
    public void Setup()
    {
        _db = new TestDatabase();
        _db.Initialize();
    }
    
    [TearDown]
    public void Teardown()
    {
        _db?.Cleanup();
    }
    
    [Test]
    public void Insert_WithValidData_AddsToDatabase()
    {
        _db.Insert(new User { Name = "John" });
        
        Assert.IsNotEmpty(_db.GetAll());
    }
}
```

---

## Mocking with Moq

**Pattern:**
```csharp
[Fact]
public void GetUser_WithValidId_ReturnsUserFromRepository()
{
    // Arrange
    var mockRepository = new Mock<IUserRepository>();
    var expectedUser = new User { Id = 1, Name = "John" };
    
    mockRepository
        .Setup(r => r.GetById(1))
        .Returns(expectedUser);
    
    var service = new UserService(mockRepository.Object);
    
    // Act
    var result = service.GetUser(1);
    
    // Assert
    Assert.Equal(expectedUser, result);
    mockRepository.Verify(r => r.GetById(1), Times.Once);
}
```

**Verify Mock Calls:**
```csharp
mockRepository.Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
mockRepository.Verify(r => r.GetById(1), Times.AtLeastOnce);
mockRepository.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
```

---

## Async Testing

**XUnit:**
```csharp
[Fact]
public async Task FetchUser_WithValidId_ReturnsUserAsync()
{
    // Arrange
    var service = new UserService();
    
    // Act
    var result = await service.FetchUserAsync(1);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal("John", result.Name);
}
```

**NUnit:**
```csharp
[Test]
public async Task FetchUser_WithValidId_ReturnsUserAsync()
{
    var result = await _service.FetchUserAsync(1);
    Assert.IsNotNull(result);
}
```

---

## Collections & Assertions

**XUnit:**
```csharp
[Fact]
public void GetUsers_WithMultipleUsers_ReturnsAll()
{
    var service = new UserService();
    var result = service.GetUsers();
    
    Assert.NotEmpty(result);
    Assert.Equal(3, result.Count());
    Assert.Contains(result, u => u.Name == "John");
    Assert.All(result, user => Assert.NotNull(user.Name));
}
```

**NUnit:**
```csharp
[Test]
public void GetUsers_WithMultipleUsers_ReturnsAll()
{
    var result = _service.GetUsers();
    
    Assert.IsNotEmpty(result);
    Assert.That(result.Count(), Is.EqualTo(3));
    Assert.That(result, Does.Contain(It.IsAny<User>()));
}
```

---

## Property Testing with FluentAssertions

**Pattern:**
```csharp
[Fact]
public void User_WithValidData_ShouldHaveCorrectProperties()
{
    // Arrange
    var user = new User { Id = 1, Name = "John", Age = 30 };
    
    // Assert
    user.Should()
        .NotBeNull()
        .And.Match<User>(u => u.Id == 1)
        .And.Match<User>(u => u.Name == "John");
    
    user.Name.Should().Be("John").And.HaveLength(4);
    user.Age.Should().Be(30).And.BeGreaterThan(18);
}
```

---

## Testing Interfaces

**Pattern:**
```csharp
public interface ICalculator
{
    int Add(int a, int b);
}

public class CalculatorTests
{
    [Fact]
    public void Calculator_ShouldImplementICalculator()
    {
        var calculator = new Calculator();
        Assert.IsAssignableFrom<ICalculator>(calculator);
    }
}
```

---

## Code Coverage

```csharp
// Run tests with coverage
// dotnet test /p:CollectCoverage=true
// dotnet test --collect:"XPlat Code Coverage"

// NUnit
// dotnet test --logger "trx" --collect:"OpenCover"
```

---

## Quick Reference

| Type | Framework | Attribute |
|------|-----------|-----------|
| Test | XUnit | `[Fact]` |
| Test | NUnit | `[Test]` |
| Parameterized | XUnit | `[Theory]` |
| Parameterized | NUnit | `[TestCase]` |
| Setup | XUnit | `Constructor` |
| Setup | NUnit | `[SetUp]` |
| Teardown | XUnit | `IDisposable` |
| Teardown | NUnit | `[TearDown]` |

---

## Best Practices

- One assertion concept per test
- Use descriptive test names
- Follow AAA pattern
- Mock external dependencies
- Keep tests independent
- Test edge cases
- Don't test implementation details
- Maintain test code quality
- Run tests frequently
- Aim for 80%+ code coverage

