# Testing - Interview Questions & Answers

## 1. Why is testing important?

**Answer:**

Testing ensures code quality, catches bugs early, and provides confidence in changes.

**Benefits**:
- **Bug Detection**: Catch issues before production
- **Regression Prevention**: Ensure changes don't break existing functionality
- **Documentation**: Tests show how code should work
- **Design Improvement**: Testable code is usually better designed
- **Refactoring Confidence**: Safely improve code quality
- **Cost Reduction**: Cheaper to fix bugs early

```csharp
// Without tests - manually verify every change
public class Calculator {
    public int Add(int a, int b) {
        return a + b;  // Works initially
        // return a * b;  // Oops! Change breaks it, but who knows?
    }
}

// With tests - automatically verify
[TestClass]
public class CalculatorTests {
    [TestMethod]
    public void Add_WithPositiveNumbers_ReturnsSum() {
        var calc = new Calculator();
        int result = calc.Add(5, 3);
        Assert.AreEqual(8, result);  // Catches the bug!
    }
}
```

---

## 2. What are unit tests and what makes a good unit test?

**Answer:**

Unit tests test individual components in isolation.

```csharp
[TestClass]
public class UserServiceTests {
    private UserService _service;
    
    [TestInitialize]
    public void Setup() {
        _service = new UserService();
    }
    
    // Good test - clear, specific, focused
    [TestMethod]
    public void CreateUser_WithValidData_ReturnsNewUser() {
        // Arrange
        var userData = new { Name = "John", Email = "john@example.com" };
        
        // Act
        var user = _service.CreateUser(userData.Name, userData.Email);
        
        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual("John", user.Name);
        Assert.AreEqual("john@example.com", user.Email);
    }
}
```

**Characteristics of Good Tests**:
- **Independent**: Don't depend on other tests
- **Isolated**: Test one thing in isolation
- **Deterministic**: Always pass or always fail
- **Fast**: Complete quickly
- **Clear**: Easy to understand the purpose
- **Focused**: Test one behavior

**AAA Pattern**:
- **Arrange**: Set up test data
- **Act**: Execute the code
- **Assert**: Verify the result

---

## 3. What is mocking and when is it used?

**Answer:**

Mocking replaces dependencies with test doubles to isolate code.

```csharp
// Production code
public class UserService {
    private readonly IEmailService _emailService;
    private readonly IUserRepository _repository;
    
    public UserService(IEmailService emailService, IUserRepository repository) {
        _emailService = emailService;
        _repository = repository;
    }
    
    public async Task<User> RegisterUserAsync(string email, string password) {
        var user = new User { Email = email, Password = password };
        await _repository.SaveAsync(user);
        await _emailService.SendConfirmationAsync(user.Email);
        return user;
    }
}

// Test with mocks
[TestClass]
public class UserServiceTests {
    [TestMethod]
    public async Task RegisterUser_WithValidData_SavesAndSendsEmail() {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockEmailService = new Mock<IEmailService>();
        var service = new UserService(mockEmailService.Object, mockRepository.Object);
        
        // Act
        var user = await service.RegisterUserAsync("john@example.com", "password123");
        
        // Assert
        mockRepository.Verify(r => r.SaveAsync(It.IsAny<User>()), Times.Once);
        mockEmailService.Verify(e => e.SendConfirmationAsync("john@example.com"), Times.Once);
    }
}

// Using NSubstitute (alternative to Moq)
[TestMethod]
public async Task RegisterUser_SendsEmail() {
    // Arrange
    var mockEmailService = Substitute.For<IEmailService>();
    var mockRepository = Substitute.For<IUserRepository>();
    var service = new UserService(mockEmailService, mockRepository);
    
    // Act
    await service.RegisterUserAsync("john@example.com", "password123");
    
    // Assert
    await mockEmailService.Received(1).SendConfirmationAsync("john@example.com");
}
```

---

## 4. What is the difference between mocks, stubs, fakes, and spies?

**Answer:**

| Type | Purpose | Returns | Example |
|------|---------|---------|---------|
| **Mock** | Verify interactions | Predetermined | Verify called, verify parameter |
| **Stub** | Provide predetermined response | Fixed value | Return fake data |
| **Fake** | Working implementation | Working behavior | In-memory database |
| **Spy** | Track calls while using real object | Real result | Verify called on real object |

```csharp
// Stub - just return data
var stubRepository = new Mock<IUserRepository>();
stubRepository.Setup(r => r.GetUserAsync(1))
    .ReturnsAsync(new User { Id = 1, Name = "John" });

// Mock - verify interaction
var mockEmailService = new Mock<IEmailService>();
await service.RegisterUserAsync("john@example.com", "password");
mockEmailService.Verify(e => e.SendConfirmationAsync("john@example.com"), Times.Once);

// Fake - working implementation
public class FakeUserRepository : IUserRepository {
    private List<User> _users = new List<User>();
    
    public Task SaveAsync(User user) {
        _users.Add(user);
        return Task.CompletedTask;
    }
    
    public Task<User> GetUserAsync(int id) {
        return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    }
}

// Spy - verify calls on real object
var emailService = new EmailService();  // Real object
var spyService = new Mock<EmailService> { CallBase = true };
spyService.Object.SendEmail("john@example.com");
spyService.Verify(e => e.SendEmail("john@example.com"), Times.Once);
```

---

## 5. What are integration tests?

**Answer:**

Integration tests verify multiple components work together.

```csharp
[TestClass]
public class UserServiceIntegrationTests {
    private readonly ApplicationDbContext _context;
    private readonly UserService _service;
    
    [TestInitialize]
    public void Setup() {
        // Use in-memory database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
        
        _context = new ApplicationDbContext(options);
        var emailService = new EmailService();  // Real service
        var repository = new UserRepository(_context);  // Real repository
        _service = new UserService(emailService, repository);
    }
    
    [TestMethod]
    public async Task RegisterUser_SavesUserToDatabase() {
        // Arrange - real database
        
        // Act
        var user = await _service.RegisterUserAsync("john@example.com", "password123");
        
        // Assert - verify in real database
        var savedUser = await _context.Users.FindAsync(user.Id);
        Assert.IsNotNull(savedUser);
        Assert.AreEqual("john@example.com", savedUser.Email);
    }
}
```

**Unit vs Integration**:
```
Unit Test:        UserService + Mock Repository + Mock EmailService
Integration Test: UserService + Real Repository + In-Memory Database
End-to-End Test:  UserService + Real Database + Real Email Service
```

---

## 6. What are test fixtures and test setup?

**Answer:**

Test fixtures prepare test data and setup for tests.

```csharp
[TestClass]
public class CalculatorTests {
    private Calculator _calculator;
    
    // Runs before each test
    [TestInitialize]
    public void Setup() {
        _calculator = new Calculator();
    }
    
    // Runs after each test
    [TestCleanup]
    public void Teardown() {
        _calculator = null;
    }
    
    [TestMethod]
    public void Add_ReturnsSum() {
        var result = _calculator.Add(5, 3);
        Assert.AreEqual(8, result);
    }
}

// Shared fixture across all tests
[TestClass]
public class DatabaseTests {
    private static ApplicationDbContext _context;
    
    // Runs once per class
    [ClassInitialize]
    public static void ClassSetup(TestContext context) {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("SharedDatabase")
            .Options;
        _context = new ApplicationDbContext(options);
    }
    
    // Runs once after all tests
    [ClassCleanup]
    public static void ClassTeardown() {
        _context?.Dispose();
    }
}

// xUnit approach
public class CalculatorTests : IDisposable {
    private Calculator _calculator;
    
    public CalculatorTests() {
        _calculator = new Calculator();  // Constructor = setup
    }
    
    public void Dispose() {
        _calculator = null;  // Cleanup
    }
    
    [Fact]
    public void Add_ReturnsSum() {
        var result = _calculator.Add(5, 3);
        Assert.Equal(8, result);
    }
}
```

---

## 7. What is test-driven development (TDD)?

**Answer:**

TDD is writing tests before writing code: Red → Green → Refactor.

```csharp
// Step 1: RED - Write failing test
[TestMethod]
[ExpectedException(typeof(ArgumentException))]
public void Withdraw_WithInsufficientFunds_ThrowsException() {
    var account = new BankAccount(100);
    account.Withdraw(150);  // Not implemented yet, test fails
}

// Step 2: GREEN - Write minimal code to pass
public class BankAccount {
    private decimal _balance;
    
    public BankAccount(decimal initial) {
        _balance = initial;
    }
    
    public void Withdraw(decimal amount) {
        if (amount > _balance) {
            throw new ArgumentException("Insufficient funds");
        }
        _balance -= amount;
    }
}

// Step 3: REFACTOR - Improve code
public class BankAccount {
    private decimal _balance;
    
    public BankAccount(decimal initialBalance) {
        if (initialBalance < 0) throw new ArgumentException("Balance cannot be negative");
        _balance = initialBalance;
    }
    
    public void Withdraw(decimal amount) {
        ValidateWithdrawalAmount(amount);
        _balance -= amount;
    }
    
    private void ValidateWithdrawalAmount(decimal amount) {
        if (amount <= 0) throw new ArgumentException("Amount must be positive");
        if (amount > _balance) throw new ArgumentException("Insufficient funds");
    }
}
```

**Benefits**:
- Better design
- Better test coverage
- Less debugging
- Confidence in refactoring

---

## 8. What are code coverage metrics?

**Answer:**

Code coverage measures what percentage of code is tested.

```csharp
public class UserValidator {
    public bool ValidateEmail(string email) {
        if (string.IsNullOrEmpty(email)) {
            return false;  // Not tested = 0% coverage
        }
        return email.Contains("@");  // 100% coverage
    }
    
    public bool ValidateAge(int age) {
        if (age < 0) return false;      // Not tested = 0% coverage
        if (age > 150) return false;    // Not tested = 0% coverage
        return true;                    // 100% coverage
    }
}

// Tests
[TestMethod]
public void ValidateEmail_WithValidEmail_ReturnsTrue() {
    var result = ValidateEmail("john@example.com");
    Assert.IsTrue(result);  // Only tests one path
}

// Coverage tools
// - OpenCover
// - CodeCov
// - Coverlet (dotnet)
```

**Coverage Types**:
- **Line Coverage**: % of lines executed
- **Branch Coverage**: % of code branches taken
- **Method Coverage**: % of methods called

**Rule of Thumb**: Aim for 80%+ coverage, but focus on important code.

---

## 9. What are common testing frameworks in C#?

**Answer:**

**xUnit** (Modern, recommended):
```csharp
public class CalculatorTests {
    [Fact]
    public void Add_WithPositiveNumbers_ReturnsSum() {
        var calc = new Calculator();
        int result = calc.Add(5, 3);
        Assert.Equal(8, result);
    }
    
    [Theory]
    [InlineData(5, 3, 8)]
    [InlineData(10, 5, 15)]
    public void Add_WithVariousInputs_ReturnsCorrectSum(int a, int b, int expected) {
        var calc = new Calculator();
        Assert.Equal(expected, calc.Add(a, b));
    }
}
```

**NUnit** (Traditional):
```csharp
[TestFixture]
public class CalculatorTests {
    [Test]
    public void Add_ReturnsSum() {
        var calc = new Calculator();
        Assert.AreEqual(8, calc.Add(5, 3));
    }
}
```

**MSTest** (Microsoft):
```csharp
[TestClass]
public class CalculatorTests {
    [TestMethod]
    public void Add_ReturnsSum() {
        var calc = new Calculator();
        Assert.AreEqual(8, calc.Add(5, 3));
    }
}
```

---

## 10. What is behavior-driven development (BDD)?

**Answer:**

BDD uses human-readable test descriptions in Given-When-Then format.

```csharp
// Using SpecFlow (BDD framework)
/*
Feature: Bank Account Withdrawal
  Scenario: Withdraw money with sufficient funds
    Given I have a bank account with £100
    When I withdraw £50
    Then my balance should be £50
*/

[Binding]
public class BankAccountSteps {
    private BankAccount _account;
    private decimal _withdrawAmount;
    
    [Given("I have a bank account with £(.*)")]
    public void GivenBankAccount(decimal balance) {
        _account = new BankAccount(balance);
    }
    
    [When("I withdraw £(.*)")]
    public void WhenWithdraw(decimal amount) {
        _withdrawAmount = amount;
        _account.Withdraw(amount);
    }
    
    [Then("my balance should be £(.*)")]
    public void ThenBalanceShouldBe(decimal expected) {
        Assert.AreEqual(expected, _account.Balance);
    }
}
```

---

## 11. What are some testing best practices?

**Answer:**

```csharp
// ✓ Good: Clear, descriptive test names
[TestMethod]
public void CreateUser_WithValidEmail_ReturnsUserWithEmail() { }

// ✗ Bad: Unclear test names
[TestMethod]
public void Test1() { }

// ✓ Good: Arrange-Act-Assert pattern
[TestMethod]
public void Transfer_WithSufficientFunds_UpdatesBalances() {
    // Arrange
    var account1 = new BankAccount(100);
    var account2 = new BankAccount(50);
    
    // Act
    account1.TransferTo(account2, 30);
    
    // Assert
    Assert.AreEqual(70, account1.Balance);
    Assert.AreEqual(80, account2.Balance);
}

// ✓ Good: Test one thing
[TestMethod]
public void Add_WithPositiveNumbers_ReturnsSum() {
    Assert.AreEqual(8, _calc.Add(5, 3));
}

// ✗ Bad: Test multiple things
[TestMethod]
public void Calculate() {
    Assert.AreEqual(8, _calc.Add(5, 3));
    Assert.AreEqual(2, _calc.Subtract(5, 3));
    Assert.AreEqual(15, _calc.Multiply(5, 3));
}

// ✓ Good: Don't test implementation details
[TestMethod]
public void GetUserName_ReturnsCorrectName() {
    var user = new User { Name = "John" };
    Assert.AreEqual("John", user.Name);
}

// ✓ Good: Use meaningful assertions
Assert.IsNotNull(user);
Assert.IsTrue(result);
Assert.AreEqual(expected, actual);
Assert.Contains(item, collection);

// ✗ Bad: Vague assertions
Assert.IsTrue(user != null);
Assert.IsTrue(result == true);
```

---

## 12. What are common testing anti-patterns to avoid?

**Answer:**

```csharp
// ❌ Test interdependence (tests depend on order)
[TestMethod]
[TestOrder(1)]
public void CreateUser_AddUserToDatabase() {
    user = _service.CreateUser("John");
    Assert.IsNotNull(user);
}

[TestMethod]
[TestOrder(2)]
public void GetUser_ReturnsCreatedUser() {
    Assert.AreEqual("John", user.Name);  // Depends on previous test!
}

// ✓ Isolate each test
[TestMethod]
public void CreateUser_AddUserToDatabase() {
    var user = _service.CreateUser("John");
    Assert.IsNotNull(user);
}

[TestMethod]
public void GetUser_ReturnsCreatedUser() {
    var user = _service.CreateUser("John");  // Create fresh data
    Assert.AreEqual("John", user.Name);
}

// ❌ Testing implementation details
private int _count = 0;
[TestMethod]
public void Add_IncrementCount() {
    _service.Add(5);
    Assert.AreEqual(1, _count);  // Testing private variable
}

// ✓ Test behavior, not implementation
[TestMethod]
public void Add_IncreasesTotal() {
    var total = _service.GetTotal();
    _service.Add(5);
    Assert.AreEqual(total + 5, _service.GetTotal());
}

// ❌ Sleeping in tests (unreliable)
[TestMethod]
public async Task ProcessAsync_Completes() {
    _service.ProcessAsync();
    Thread.Sleep(5000);  // Flaky!
    Assert.IsTrue(_service.IsComplete);
}

// ✓ Wait for actual event
[TestMethod]
public async Task ProcessAsync_Completes() {
    var task = _service.ProcessAsync();
    var completed = await Task.WhenAny(task, Task.Delay(5000));
    Assert.AreEqual(task, completed);
}
```

---

## Quick Tips for Interview

✓ Know AAA pattern (Arrange, Act, Assert)
✓ Understand unit tests vs integration tests
✓ Know what makes a good unit test
✓ Understand mocking and test doubles
✓ Know TDD workflow (Red-Green-Refactor)
✓ Comfortable with test fixtures and setup
✓ Know common testing frameworks
✓ Understand code coverage metrics
✓ Know testing anti-patterns to avoid
✓ Explain why testing is important
