# NUnit Framework Snippets

NUnit testing framework specific patterns and assertions.

## Test Class Structure

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
        Assert.That(result, Is.EqualTo(5));
    }
    
    [TearDown]
    public void TearDown()
    {
        _calculator = null;
    }
}
```

---

## Lifecycle Attributes

**Test Method:**
```csharp
[Test]
public void TestMethod()
{
    Assert.That(true, Is.True);
}
```

**SetUp (Before Each Test):**
```csharp
[SetUp]
public void Setup()
{
    // Run before each test method
}
```

**TearDown (After Each Test):**
```csharp
[TearDown]
public void TearDown()
{
    // Run after each test method
}
```

**OneTimeSetUp (Before All Tests):**
```csharp
[OneTimeSetUp]
public void OneTimeSetup()
{
    // Run once before all tests in class
}
```

**OneTimeTearDown (After All Tests):**
```csharp
[OneTimeTearDown]
public void OneTimeTearDown()
{
    // Run once after all tests in class
}
```

---

## Parameterized Tests

**TestCase - Single Parameters:**
```csharp
[TestCase(2, 3, 5)]
[TestCase(0, 0, 0)]
[TestCase(-1, 1, 0)]
public void Add_WithVariousNumbers_ReturnsSum(int a, int b, int expected)
{
    var result = _calculator.Add(a, b);
    Assert.That(result, Is.EqualTo(expected));
}
```

**TestCase - Named Parameters:**
```csharp
[TestCase(2, 3, 5, TestName = "PositiveNumbers")]
[TestCase(0, 0, 0, TestName = "Zeros")]
[TestCase(-1, 1, 0, TestName = "MixedSigns")]
public void Add_WithVariousNumbers_ReturnsSum(int a, int b, int expected)
{
    Assert.That(_calculator.Add(a, b), Is.EqualTo(expected));
}
```

**ValueSource:**
```csharp
private static readonly object[] Numbers = { 2, 3, 5 };

[Test]
[ValueSource(nameof(Numbers))]
public void IsEven_WithVariousNumbers(int number)
{
    Assert.That(number, Is.Positive);
}
```

**TestCaseSource:**
```csharp
[TestCaseSource(nameof(GetTestData))]
public void Divide_WithVariousNumbers_ReturnsCorrectResult(int a, int b, int expected)
{
    var result = _calculator.Divide(a, b);
    Assert.That(result, Is.EqualTo(expected));
}

private static IEnumerable<TestCaseData> GetTestData()
{
    yield return new TestCaseData(10, 2, 5).SetName("BasicDivision");
    yield return new TestCaseData(0, 1, 0).SetName("ZeroNumerator");
}
```

---

## Constraint-Based Assertions

**Equality:**
```csharp
Assert.That(5, Is.EqualTo(5));
Assert.That("hello", Is.EqualTo("hello"));
Assert.That(5, Is.Not.EqualTo(3));
```

**Comparison:**
```csharp
Assert.That(5, Is.GreaterThan(3));
Assert.That(3, Is.LessThan(5));
Assert.That(5, Is.GreaterThanOrEqualTo(5));
Assert.That(5, Is.LessThanOrEqualTo(5));
```

**Null/Instance:**
```csharp
Assert.That(obj, Is.Null);
Assert.That(obj, Is.Not.Null);
Assert.That(obj, Is.InstanceOf<MyClass>());
```

**Boolean:**
```csharp
Assert.That(true, Is.True);
Assert.That(false, Is.False);
Assert.That(condition, Is.EqualTo(true));
```

**String Constraints:**
```csharp
Assert.That("Hello World", Does.Contain("World"));
Assert.That("Hello World", Does.Not.Contain("Goodbye"));
Assert.That("hello", Does.StartWith("he"));
Assert.That("hello", Does.EndWith("lo"));
Assert.That("HELLO", Does.Match("^[A-Z]+$"));
```

---

## Collection Assertions

**Existence:**
```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };

Assert.That(list, Does.Contain(3));
Assert.That(list, Does.Not.Contain(10));
Assert.That(list, Is.Not.Empty);
Assert.That(list, Has.Count.EqualTo(5));
```

**Properties:**
```csharp
var list = new List<int> { 1, 2, 3 };

Assert.That(list, Has.Length.EqualTo(3));
Assert.That(list, Has.Exactly(3).Items);
Assert.That(list, Has.Some.EqualTo(2));
Assert.That(list, Has.All.LessThan(10));
```

**Type Checking:**
```csharp
var items = new List<string> { "a", "b", "c" };

Assert.That(items, Is.All.InstanceOf<string>());
Assert.That(items, Has.All.InstanceOf<string>());
```

**Ordering:**
```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };
Assert.That(list, Is.Ordered.Ascending);

var descending = new List<int> { 5, 4, 3, 2, 1 };
Assert.That(descending, Is.Ordered.Descending);
```

---

## Exception Testing

**ExpectedException:**
```csharp
[Test]
[ExpectedException(typeof(ArgumentNullException))]
public void MethodName_WithNullParameter_ThrowsArgumentNullException()
{
    var service = new MyService();
    service.DoSomething(null);
}
```

**Throws:**
```csharp
[Test]
public void Divide_ByZero_ThrowsDivideByZeroException()
{
    var calculator = new Calculator();
    
    Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
}

// With message check
[Test]
public void Divide_ByZero_ThrowsWithCorrectMessage()
{
    var ex = Assert.Throws<DivideByZeroException>(
        () => _calculator.Divide(10, 0));
    
    Assert.That(ex.Message, Does.Contain("zero"));
}
```

**DoesNotThrow:**
```csharp
[Test]
public void Add_WithValidNumbers_DoesNotThrow()
{
    Assert.DoesNotThrow(() => _calculator.Add(5, 3));
}
```

---

## Async Tests

**Pattern:**
```csharp
[Test]
public async Task FetchUser_WithValidId_ReturnsUserAsync()
{
    // Arrange
    var service = new UserService();
    
    // Act
    var result = await service.FetchUserAsync(1);
    
    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Name, Is.EqualTo("John"));
}
```

---

## Timeout & Test Duration

**Pattern:**
```csharp
[Test]
[Timeout(1000)]
public void QuickOperation_CompletesWithin1Second()
{
    var result = ExpensiveCalculation();
    Assert.That(result, Is.Not.Null);
}
```

---

## Ignoring Tests

**Pattern:**
```csharp
[Test]
[Ignore("Not implemented yet")]
public void FeatureNotImplemented_IgnoredTest()
{
    Assert.Fail("This test is ignored");
}

// Conditional ignore
[Test]
[IgnoreIf("SKIP_SLOW_TESTS", true)]
public void SlowTest()
{
    Assert.Pass();
}
```

---

## Test Categories

**Pattern:**
```csharp
[TestFixture]
[Category("Unit")]
public class UnitTests
{
    [Test]
    [Category("Fast")]
    public void FastTest()
    {
        Assert.That(true, Is.True);
    }
}

[TestFixture]
[Category("Integration")]
public class IntegrationTests
{
    [Test]
    [Category("Slow")]
    public void SlowTest()
    {
        Assert.That(true, Is.True);
    }
}

// Run: dotnet test --filter "Category=Fast"
```

---

## Sequential Execution

**Pattern:**
```csharp
[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class SequentialTests
{
    [Test]
    [Order(1)]
    public void FirstTest()
    {
        Assert.Pass();
    }
    
    [Test]
    [Order(2)]
    public void SecondTest()
    {
        Assert.Pass();
    }
}
```

---

## Mocking with NUnit & Moq

**Pattern:**
```csharp
[TestFixture]
public class ServiceTests
{
    private Mock<IRepository> _mockRepository;
    private Service _service;
    
    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IRepository>();
        _service = new Service(_mockRepository.Object);
    }
    
    [Test]
    public void GetUser_WithValidId_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "John" };
        _mockRepository.Setup(r => r.GetById(1)).Returns(user);
        
        // Act
        var result = _service.GetUser(1);
        
        // Assert
        Assert.That(result.Name, Is.EqualTo("John"));
        _mockRepository.Verify(r => r.GetById(1), Times.Once);
    }
}
```

---

## Test Result Options

**Pattern:**
```csharp
[Test]
public void TestCanHaveMultipleResults()
{
    if (SomeCondition())
    {
        Assert.Pass("Specific condition passed");
    }
    else if (AnotherCondition())
    {
        Assert.Inconclusive("Test is inconclusive");
    }
    else
    {
        Assert.Fail("Test failed");
    }
}
```

---

## Custom Assertions

**Pattern:**
```csharp
public static class CustomAssertions
{
    public static void IsEven(int number, string message = null)
    {
        Assert.That(number % 2, Is.EqualTo(0), message);
    }
    
    public static void IsPositive(int number)
    {
        Assert.That(number, Is.GreaterThan(0));
    }
}

// Usage
[Test]
public void Number_IsEven()
{
    CustomAssertions.IsEven(4);
}
```

---

## Running Tests

```powershell
# Run all tests
dotnet test

# Run specific fixture
dotnet test --filter "TestFixture=CalculatorTests"

# Run specific category
dotnet test --filter "Category=Unit"

# Run with verbosity
dotnet test --verbosity detailed

# Run tests in order
dotnet test -- NUnit.Where="cat==Sequential"

# Generate coverage
dotnet test /p:CollectCoverage=true
```

---

## Quick Reference

| Attribute | Purpose |
|-----------|---------|
| `[TestFixture]` | Test class |
| `[Test]` | Test method |
| `[TestCase]` | Parameterized test |
| `[SetUp]` | Before each test |
| `[TearDown]` | After each test |
| `[OneTimeSetUp]` | Before all tests |
| `[OneTimeTearDown]` | After all tests |
| `[Ignore]` | Ignore test |
| `[Category]` | Test category |
| `[Order]` | Test order |
| `[Timeout]` | Test timeout |

---

## Best Practices

- Use constraint-based assertions (`Is.EqualTo`, `Does.Contain`)
- One logical assertion per test
- Use `[SetUp]` and `[TearDown]` for setup/cleanup
- Use `[TestCase]` for parameterized tests
- Name tests descriptively
- Keep tests independent
- Use categories for filtering
- Mock external dependencies
- Test edge cases
- Maintain test code quality

