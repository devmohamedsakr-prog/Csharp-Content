# MSTest Framework Snippets

Microsoft Testing Framework specific patterns and assertions.

## Test Class Structure

**Pattern:**
```csharp
[TestClass]
public class CalculatorTests
{
    private Calculator _calculator;
    
    [TestInitialize]
    public void Initialize()
    {
        _calculator = new Calculator();
    }
    
    [TestMethod]
    public void Add_WithTwoNumbers_ReturnsSum()
    {
        // Arrange
        // Act
        var result = _calculator.Add(2, 3);
        
        // Assert
        Assert.AreEqual(5, result);
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        _calculator = null;
    }
}
```

---

## Test Method Attributes

**TestMethod:**
```csharp
[TestMethod]
public void TestMethodName()
{
    Assert.IsTrue(true);
}
```

**TestInitialize (Setup):**
```csharp
[TestInitialize]
public void Setup()
{
    // Run before each test
}
```

**TestCleanup (Teardown):**
```csharp
[TestCleanup]
public void Cleanup()
{
    // Run after each test
}
```

**ClassInitialize (One-time Setup):**
```csharp
[ClassInitialize]
public static void ClassSetup(TestContext context)
{
    // Run once before all tests in class
}
```

**ClassCleanup (One-time Teardown):**
```csharp
[ClassCleanup]
public static void ClassCleanup()
{
    // Run once after all tests in class
}
```

---

## Data-Driven Tests

**DataTestMethod with DataRow:**
```csharp
[DataTestMethod]
[DataRow(2, 3, 5)]
[DataRow(0, 0, 0)]
[DataRow(-1, 1, 0)]
public void Add_WithVariousNumbers_ReturnsSum(int a, int b, int expected)
{
    var result = _calculator.Add(a, b);
    Assert.AreEqual(expected, result);
}
```

**DataTestMethod with DynamicData:**
```csharp
[DataTestMethod]
[DynamicData(nameof(GetTestData))]
public void Divide_WithVariousNumbers_ReturnsCorrectResult(int a, int b, int expected)
{
    var result = _calculator.Divide(a, b);
    Assert.AreEqual(expected, result);
}

public static IEnumerable<object[]> GetTestData()
{
    yield return new object[] { 10, 2, 5 };
    yield return new object[] { 20, 4, 5 };
    yield return new object[] { 0, 1, 0 };
}
```

**CSV Data Source:**
```csharp
[DataTestMethod]
[CsvData("TestData.csv")]
public void TestWithCsvData(string input, string expected)
{
    Assert.AreEqual(expected, ProcessData(input));
}
```

---

## Assertions - String

**Pattern:**
```csharp
string result = "Hello World";

// Equality
Assert.AreEqual("Hello World", result);
Assert.AreNotEqual("Goodbye", result);

// Contains
StringAssert.Contains(result, "World");
StringAssert.DoesNotContain(result, "Goodbye");

// Case sensitivity
StringAssert.Equals(result, "hello world", ignoreCase: true);

// Pattern matching
StringAssert.Matches(result, @"^Hello");
```

---

## Assertions - Numeric

**Pattern:**
```csharp
int value = 10;

Assert.AreEqual(10, value);
Assert.AreNotEqual(5, value);
Assert.IsTrue(value > 0);
Assert.IsFalse(value < 0);

// Floating point with tolerance
double pi = 3.14159;
Assert.AreEqual(3.14, pi, 0.01);  // Within 0.01
```

---

## Assertions - Collections

**Pattern:**
```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };

// Existence
CollectionAssert.AllItemsAreNotNull(list);
CollectionAssert.AllItemsAreUnique(list);

// Contains
CollectionAssert.Contains(list, 3);
CollectionAssert.DoesNotContain(list, 10);

// Equality
var expected = new List<int> { 1, 2, 3, 4, 5 };
CollectionAssert.AreEqual(expected, list);

// Size
Assert.AreEqual(5, list.Count);
```

---

## Assertions - Objects

**Pattern:**
```csharp
object obj = new object();
string str = "test";
User user = new User { Name = "John" };

// Null checks
Assert.IsNull(value);
Assert.IsNotNull(value);

// Type checks
Assert.IsInstanceOfType(obj, typeof(object));
Assert.IsNotInstanceOfType(str, typeof(int));

// Same reference
Assert.AreSame(obj, obj);
Assert.AreNotSame(obj, new object());
```

---

## Exception Testing

**Pattern:**
```csharp
[TestMethod]
[ExpectedException(typeof(ArgumentNullException))]
public void MethodName_WithNullParameter_ThrowsArgumentNullException()
{
    var service = new MyService();
    service.DoSomething(null);
}

// With exception details
[TestMethod]
public void Divide_ByZero_ThrowsDivideByZeroException()
{
    var calculator = new Calculator();
    
    try
    {
        calculator.Divide(10, 0);
        Assert.Fail("Expected DivideByZeroException");
    }
    catch (DivideByZeroException ex)
    {
        Assert.IsTrue(ex.Message.Contains("zero"));
    }
}

// Using Assert.ThrowsException
[TestMethod]
public void Add_WithInvalidInput_ThrowsException()
{
    var service = new MathService();
    
    var ex = Assert.ThrowsException<ArgumentException>(
        () => service.Add(null, "5"));
    
    Assert.IsTrue(ex.Message.Contains("Invalid"));
}
```

---

## Async Tests

**Pattern:**
```csharp
[TestMethod]
public async Task FetchUser_WithValidId_ReturnsUserAsync()
{
    // Arrange
    var service = new UserService();
    
    // Act
    var result = await service.FetchUserAsync(1);
    
    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual("John", result.Name);
}

[TestMethod]
[ExpectedException(typeof(TimeoutException))]
[Timeout(5000)]  // 5 second timeout
public async Task LongRunningOperation_ExceedsTimeout()
{
    await Task.Delay(10000);
}
```

---

## Timeout & Test Duration

**Pattern:**
```csharp
[TestMethod]
[Timeout(1000)]  // 1 second timeout
public void QuickOperation_CompletesWithin1Second()
{
    var result = ExpensiveCalculation();
    Assert.IsNotNull(result);
}
```

---

## Test Categories

**Pattern:**
```csharp
[TestClass]
[TestCategory("Unit")]
public class UnitTests
{
    [TestMethod]
    [TestCategory("Fast")]
    public void FastTest()
    {
        Assert.IsTrue(true);
    }
}

[TestClass]
[TestCategory("Integration")]
public class IntegrationTests
{
    [TestMethod]
    [TestCategory("Slow")]
    public void SlowTest()
    {
        Assert.IsTrue(true);
    }
}

// Run: dotnet test --filter "Category=Fast"
```

---

## Owner & Priority

**Pattern:**
```csharp
[TestMethod]
[Owner("john.doe@company.com")]
[Priority(1)]
public void CriticalTest()
{
    Assert.IsTrue(true);
}

[TestMethod]
[Owner("jane.smith@company.com")]
[Priority(2)]
public void ImportantTest()
{
    Assert.IsTrue(true);
}
```

---

## TestContext - Additional Info

**Pattern:**
```csharp
[TestClass]
public class TestsWithContext
{
    public TestContext TestContext { get; set; }
    
    [TestMethod]
    public void TestWithContext()
    {
        TestContext.WriteLine($"Test: {TestContext.TestName}");
        TestContext.WriteLine($"Result: {TestContext.CurrentResult}");
        
        Assert.IsTrue(true);
    }
}
```

---

## Mocking with MSTest

**Pattern:**
```csharp
[TestClass]
public class ServiceTests
{
    private Mock<IRepository> _mockRepository;
    private Service _service;
    
    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IRepository>();
        _service = new Service(_mockRepository.Object);
    }
    
    [TestMethod]
    public void GetUser_WithValidId_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "John" };
        _mockRepository.Setup(r => r.GetById(1)).Returns(user);
        
        // Act
        var result = _service.GetUser(1);
        
        // Assert
        Assert.AreEqual(user.Name, result.Name);
        _mockRepository.Verify(r => r.GetById(1), Times.Once);
    }
}
```

---

## Running Tests

```powershell
# Run all tests
dotnet test

# Run specific class
dotnet test --filter "ClassName=CalculatorTests"

# Run specific category
dotnet test --filter "Category=Unit"

# Run with verbosity
dotnet test --verbosity detailed

# Run and collect coverage
dotnet test /p:CollectCoverage=true

# Run in CI/CD
dotnet test --logger "trx" --results-directory "TestResults"
```

---

## Quick Reference

| Attribute | Purpose |
|-----------|---------|
| `[TestClass]` | Test class |
| `[TestMethod]` | Test method |
| `[DataTestMethod]` | Parameterized test |
| `[DataRow]` | Test data |
| `[TestInitialize]` | Setup before each test |
| `[TestCleanup]` | Cleanup after each test |
| `[ClassInitialize]` | Setup before all tests |
| `[ClassCleanup]` | Cleanup after all tests |
| `[ExpectedException]` | Expect exception |
| `[Timeout]` | Test timeout |
| `[TestCategory]` | Test category |
| `[Owner]` | Test owner |
| `[Priority]` | Test priority |

---

## Assertions Quick Reference

| Assertion | Purpose |
|-----------|---------|
| `Assert.AreEqual` | Compare values |
| `Assert.IsTrue/False` | Boolean check |
| `Assert.IsNull/NotNull` | Null check |
| `Assert.IsInstanceOfType` | Type check |
| `Assert.ThrowsException` | Exception check |
| `CollectionAssert.AreEqual` | Compare collections |
| `StringAssert.Contains` | String contains |

---

## Best Practices

- Use `[TestInitialize]` instead of constructor
- Use `[DataTestMethod]` for parameterized tests
- Use descriptive test names
- Keep tests focused and independent
- Use proper assertion messages
- Organize tests with categories
- Use mocking for dependencies
- Test edge cases and error conditions

