# Test Coverage and Measurement

## Overview
Code coverage metrics, coverage tools, and strategies for comprehensive testing.

## Coverage Metrics

### Basic Coverage Types
```csharp
// Line Coverage: Are all lines executed?
public class CoverageExample
{
    public int CalculateDiscount(int totalAmount, bool isVip)
    {
        if (totalAmount < 0) // Line 1
        {
            throw new ArgumentException("Invalid amount"); // Line 2 - covered if tested
        }
        
        if (isVip) // Line 3
        {
            return totalAmount * 20 / 100; // Line 4 - VIP branch
        }
        
        return totalAmount * 10 / 100; // Line 5 - Regular branch
    }
}

// To achieve 100% line coverage:
[Fact]
public void CalculateDiscount_ValidAmountVip_Returns20Percent()
{
    var result = new CoverageExample().CalculateDiscount(100, true);
    Assert.Equal(20, result);
}

[Fact]
public void CalculateDiscount_ValidAmountRegular_Returns10Percent()
{
    var result = new CoverageExample().CalculateDiscount(100, false);
    Assert.Equal(10, result);
}

[Fact]
public void CalculateDiscount_NegativeAmount_ThrowsException()
{
    Assert.Throws<ArgumentException>(() => 
        new CoverageExample().CalculateDiscount(-100, false));
}
```

### Branch Coverage
```csharp
public class BranchCoverageExample
{
    public string ValidateUser(User user)
    {
        if (user == null) // Branch 1
            return "User is null";
        
        if (string.IsNullOrEmpty(user.Name)) // Branch 2
            return "Name is required";
        
        if (user.Age < 18) // Branch 3
            return "Must be 18 or older";
        
        if (user.Email.Contains("@")) // Branch 4
            return "Valid user";
        
        return "Invalid email"; // Branch 5
    }
}

// Tests for 100% branch coverage
public class BranchCoverageTests
{
    [Fact]
    public void ValidateUser_WithNull_ReturnsNullMessage()
    {
        var result = new BranchCoverageExample().ValidateUser(null);
        Assert.Equal("User is null", result);
    }
    
    [Fact]
    public void ValidateUser_WithEmptyName_ReturnsNameRequired()
    {
        var user = new User { Name = "", Age = 25 };
        var result = new BranchCoverageExample().ValidateUser(user);
        Assert.Equal("Name is required", result);
    }
    
    [Fact]
    public void ValidateUser_UnderAge_ReturnsAgeError()
    {
        var user = new User { Name = "John", Age = 15 };
        var result = new BranchCoverageExample().ValidateUser(user);
        Assert.Equal("Must be 18 or older", result);
    }
    
    [Fact]
    public void ValidateUser_ValidWithEmail_ReturnsValid()
    {
        var user = new User { Name = "John", Age = 25, Email = "john@example.com" };
        var result = new BranchCoverageExample().ValidateUser(user);
        Assert.Equal("Valid user", result);
    }
    
    [Fact]
    public void ValidateUser_InvalidEmail_ReturnsInvalidEmail()
    {
        var user = new User { Name = "John", Age = 25, Email = "invalid-email" };
        var result = new BranchCoverageExample().ValidateUser(user);
        Assert.Equal("Invalid email", result);
    }
}
```

## OpenCover and ReportGenerator

### Configuration
```xml
<!-- .editorconfig or project file -->
<PropertyGroup>
    <CollectCoverage>true</CollectCoverage>
    <CoverageReportFormats>opencover;lcov</CoverageReportFormats>
    <CoverageDirectory>$(ProjectDir)\..\coverage</CoverageDirectory>
</PropertyGroup>
```

### Command Line Usage
```bash
# Install tools
dotnet tool install -g ReportGenerator

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover

# Generate HTML report
ReportGenerator -reports:"coverage/coverage.opencover.xml" -targetdir:"coverage/report" -reporttypes:Html
```

## Measuring Coverage

### Coverage Analysis Code
```csharp
public class CoverageAnalyzer
{
    public void AnalyzeCoverage()
    {
        // Use Roslyn to analyze code
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var compilation = CSharpCompilation.Create("Analysis")
            .AddSyntaxTrees(syntaxTree);
        
        var root = syntaxTree.GetCompilationUnitSyntax();
        var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
        
        var coveredMethods = 0;
        var totalMethods = 0;
        
        foreach (var method in methods)
        {
            totalMethods++;
            if (IsCovered(method))
                coveredMethods++;
        }
        
        Console.WriteLine($"Coverage: {coveredMethods}/{totalMethods} = " +
            $"{(coveredMethods * 100.0 / totalMethods):F2}%");
    }
    
    private bool IsCovered(MethodDeclarationSyntax method)
    {
        // Implementation: check if method is tested
        return true;
    }
}
```

### Coverage Thresholds
```csharp
public class CoverageThresholds
{
    // Define acceptable coverage levels
    public const decimal MinimumOverallCoverage = 0.80m; // 80%
    public const decimal CriticalPathCoverage = 0.95m; // 95%
    public const decimal UtilityFunctionCoverage = 0.70m; // 70%
    
    public void ValidateCoverage(CoverageReport report)
    {
        if (report.OverallCoverage < MinimumOverallCoverage)
            throw new InvalidOperationException($"Coverage {report.OverallCoverage} below minimum");
        
        foreach (var module in report.CriticalPaths)
        {
            if (module.Coverage < CriticalPathCoverage)
                throw new InvalidOperationException($"Critical path {module.Name} below threshold");
        }
    }
}
```

## Effective Coverage Strategies

### Critical Path Testing
```csharp
public class PaymentProcessor
{
    // CRITICAL: Must be thoroughly tested
    public async Task<PaymentResult> ProcessPaymentAsync(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));
        
        if (payment.Amount <= 0)
            throw new ArgumentException("Amount must be positive");
        
        var result = await _gateway.ChargeAsync(payment.Amount);
        
        if (result.Success)
        {
            await _db.SavePaymentAsync(payment);
            await _emailService.SendReceiptAsync(payment.Email);
        }
        
        return result;
    }
}

// Comprehensive tests for critical path
public class PaymentProcessorTests
{
    [Fact]
    public async Task ProcessPayment_Success_SavesAndNotifies()
    {
        // Arrange
        var payment = new Payment { Amount = 100, Email = "user@example.com" };
        
        // Act
        var result = await _processor.ProcessPaymentAsync(payment);
        
        // Assert
        Assert.True(result.Success);
        _mockDb.Verify(x => x.SavePaymentAsync(payment), Times.Once);
        _mockEmail.Verify(x => x.SendReceiptAsync(payment.Email), Times.Once);
    }
    
    [Fact]
    public async Task ProcessPayment_Fails_DoesNotSaveOrNotify()
    {
        // Arrange
        var payment = new Payment { Amount = 100 };
        _gateway.ThrowOn<Payment>(payment);
        
        // Act
        var result = await _processor.ProcessPaymentAsync(payment);
        
        // Assert
        Assert.False(result.Success);
        _mockDb.Verify(x => x.SavePaymentAsync(It.IsAny<Payment>()), Times.Never);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-100)]
    public async Task ProcessPayment_InvalidAmount_Throws(decimal? amount)
    {
        var payment = new Payment { Amount = amount ?? 0 };
        
        if (payment.Amount <= 0)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _processor.ProcessPaymentAsync(payment));
        }
    }
}
```

### Utility Function Testing
```csharp
public class StringUtils
{
    // Less critical - lower coverage requirement
    public static string Truncate(string text, int length)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        
        if (text.Length <= length)
            return text;
        
        return text.Substring(0, length) + "...";
    }
}

// Basic coverage is sufficient
public class StringUtilsTests
{
    [Fact]
    public void Truncate_NormalString_Truncates()
    {
        var result = StringUtils.Truncate("Hello World", 5);
        Assert.Equal("Hello...", result);
    }
    
    [Fact]
    public void Truncate_NullString_ReturnsNull()
    {
        var result = StringUtils.Truncate(null, 5);
        Assert.Null(result);
    }
}
```

## Coverage Gaps

### Identifying Gaps
```csharp
// Look for untested code patterns
public class GapAnalysis
{
    public void FindGaps()
    {
        var gaps = new List<string>();
        
        // Exception paths not tested
        gaps.Add("Exception handling not covered");
        
        // Null checks uncovered
        gaps.Add("Null parameter validation");
        
        // Edge cases missing
        gaps.Add("Boundary conditions");
        
        // Async/await not properly tested
        gaps.Add("Concurrent operations");
        
        // Integration failures
        gaps.Add("External service failures");
    }
}

// Example: Properly testing exception paths
[Fact]
public async Task SaveUser_DbThrows_ThrowsApplicationException()
{
    var user = new User { Name = "John" };
    _mockDb.Setup(x => x.SaveAsync(It.IsAny<User>()))
        .ThrowsAsync(new SqlException());
    
    var ex = await Assert.ThrowsAsync<ApplicationException>(() =>
        _userService.SaveAsync(user));
    
    Assert.Contains("Failed to save user", ex.Message);
}
```

## Best Practices

1. **Aim for Realistic Coverage Targets**
```csharp
// Good: 80-90% coverage with focus on critical paths
if (coverage >= 0.85m)
    Console.WriteLine("Good coverage level");

// Bad: Forcing 100% coverage is counterproductive
// Might result in meaningless tests that don't add value
```

2. **Test Behavior, Not Implementation**
```csharp
// Good: Tests what the code should do
[Fact]
public void GetUser_WithValidId_ReturnsUser()
{
    var user = _service.GetUser(1);
    Assert.NotNull(user);
    Assert.Equal(1, user.Id);
}

// Bad: Tests internal implementation
[Fact]
public void GetUser_CallsCacheFirst()
{
    _service.GetUser(1);
    _mockCache.Verify(x => x.Get("user_1"));
}
```

3. **Exclude Generated and Trivial Code**
```csharp
// Exclude from coverage measurement
[ExcludeFromCodeCoverage]
public class GeneratedCode
{
    public string Property { get; set; }
}

// Or use:
#pragma warning disable CS0162 // Unreachable code
// code not meant to be executed
#pragma warning restore CS0162
```

## Common Mistakes

1. **High Coverage, Low Quality**
```csharp
// Bad: Coverage without assertions
[Fact]
public void GetUser_HasHighCoverage()
{
    var user = _service.GetUser(1);
    // No assertions - just touching code
}

// Good: Coverage with meaningful assertions
[Fact]
public void GetUser_ReturnsCorrectUser()
{
    var user = _service.GetUser(1);
    Assert.NotNull(user);
    Assert.Equal(1, user.Id);
    Assert.Equal("John", user.Name);
}
```

2. **Testing Private Methods**
```csharp
// Bad: Breaking encapsulation
[Fact]
public void PrivateHelper_ReturnsValue()
{
    var result = (int)typeof(MyClass)
        .GetMethod("Helper", System.Reflection.BindingFlags.NonPublic)
        .Invoke(obj, null);
}

// Good: Test public API that uses private methods
[Fact]
public void PublicMethod_UsesHelper_ReturnsExpected()
{
    var result = obj.PublicMethod();
    Assert.Equal(expected, result);
}
```

3. **Ignoring Edge Cases**
```csharp
// Bad: Only happy path
[Fact]
public void Process_WithData_Returns()
{
    var result = Process(validData);
    Assert.NotNull(result);
}

// Good: Including edge cases
[Theory]
[InlineData(null)]
[InlineData("")]
[InlineData("a")]
[InlineData(new int[0])]
public void Process_WithEdgeCases_HandlesGracefully(object data)
{
    var result = Process(data);
    Assert.NotNull(result);
}
```

## Quick Summary
- Aim for 80-90% coverage, not 100%
- Focus on critical paths (higher coverage)
- Utility functions can have lower coverage
- Line coverage > Branch coverage > Path coverage
- Use coverage tools (OpenCover, ReportGenerator)
- Exclude generated/trivial code
- Test behavior, not implementation
- Comprehensive tests beat high coverage count
- Monitor coverage trends over time
- Coverage != quality of tests

## Resources
- OpenCover
- ReportGenerator
- Code Coverage Best Practices
- Testing Strategies for Coverage
