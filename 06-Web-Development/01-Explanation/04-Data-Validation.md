# Data Validation

## Overview
Input validation strategies using Data Annotations, Fluent Validation, and custom validators.

## Data Annotations

### Basic Validation Attributes
```csharp
public class CreateUserRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, 
        ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    
    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
    public int Age { get; set; }
    
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be 10 digits")]
    public string Phone { get; set; }
    
    [Url(ErrorMessage = "Invalid URL")]
    public string Website { get; set; }
    
    [Compare("ConfirmPassword", ErrorMessage = "Passwords don't match")]
    public string Password { get; set; }
    
    public string ConfirmPassword { get; set; }
}

// Controller usage
[HttpPost]
public async Task<IActionResult> CreateUser(CreateUserRequest request)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState); // Validation attributes checked
    
    // Process request
    return Ok();
}
```

### Custom Validation Attributes
```csharp
[AttributeUsage(AttributeTargets.Property)]
public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (value == null)
            return ValidationResult.Success;
        
        var email = value.ToString();
        var userService = (IUserService)context.GetService(typeof(IUserService));
        
        if (userService.UserExists(email))
            return new ValidationResult("Email already exists");
        
        return ValidationResult.Success;
    }
}

public class CreateUserRequest
{
    [Required]
    [UniqueEmail]
    public string Email { get; set; }
}
```

## Fluent Validation

### Validator Classes
```csharp
public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    private readonly IUserService _userService;
    
    public CreateUserValidator(IUserService userService)
    {
        _userService = userService;
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 100).WithMessage("Name must be 2-100 characters")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email")
            .MustAsync(async (email, ct) => !await _userService.EmailExistsAsync(email))
            .WithMessage("Email already registered");
        
        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18).WithMessage("Must be 18 or older")
            .LessThanOrEqualTo(120).WithMessage("Invalid age");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"[A-Z]").WithMessage("Must contain uppercase")
            .Matches(@"[a-z]").WithMessage("Must contain lowercase")
            .Matches(@"[0-9]").WithMessage("Must contain number")
            .Matches(@"[!@#$%^&*]").WithMessage("Must contain special character");
        
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords don't match");
    }
}

// Program.cs registration
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Automatic validation in controllers
[HttpPost]
public async Task<IActionResult> CreateUser(CreateUserRequest request)
{
    // Validation happens automatically
    // if invalid, returns 400 Bad Request
    return Ok();
}
```

### Custom Rules
```csharp
public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Price)
            .Must(BeValidPrice)
            .WithMessage("Price must be positive");
        
        RuleFor(x => x.Quantity)
            .MustAsync(CheckStock)
            .WithMessage("Insufficient stock available");
        
        RuleFor(x => x.Category)
            .Must(x => new[] { "Electronics", "Books", "Clothing" }.Contains(x))
            .WithMessage("Invalid category");
        
        // Conditional rules
        When(x => x.IsPhysical, () =>
        {
            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight required for physical items");
            
            RuleFor(x => x.Dimensions)
                .NotEmpty().WithMessage("Dimensions required for physical items");
        });
    }
    
    private bool BeValidPrice(decimal price) => price > 0;
    
    private async Task<bool> CheckStock(int quantity, CancellationToken ct)
    {
        // Check database
        return quantity <= 1000;
    }
}
```

## Manual Validation

### Property Validation
```csharp
public class ValidatorService : IValidatorService
{
    public ValidationResult ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return ValidationResult.Failure("Email is required");
        
        if (!email.Contains("@"))
            return ValidationResult.Failure("Invalid email format");
        
        if (email.Length > 254)
            return ValidationResult.Failure("Email too long");
        
        return ValidationResult.Success();
    }
    
    public ValidationResult ValidatePassword(string password)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(password))
            errors.Add("Password is required");
        
        if (password.Length < 8)
            errors.Add("Password must be at least 8 characters");
        
        if (!password.Any(char.IsUpper))
            errors.Add("Password must contain uppercase");
        
        if (!password.Any(char.IsLower))
            errors.Add("Password must contain lowercase");
        
        if (!password.Any(char.IsDigit))
            errors.Add("Password must contain digit");
        
        return errors.Any() 
            ? ValidationResult.Failure(errors) 
            : ValidationResult.Success();
    }
}

public class ValidationResult
{
    public bool IsValid { get; }
    public List<string> Errors { get; }
    
    public static ValidationResult Success() => new(true, new());
    public static ValidationResult Failure(string error) => new(false, new() { error });
    public static ValidationResult Failure(List<string> errors) => new(false, errors);
    
    private ValidationResult(bool isValid, List<string> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }
}
```

## Request Validation Middleware

### Global Validation Pipeline
```csharp
[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected IActionResult ValidateAndProcess<T>(T request, Func<T, Task<IActionResult>> handler)
        where T : IValidatable
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return BadRequest(new { errors = validationResult.Errors });
        
        return handler(request).Result;
    }
}

public interface IValidatable
{
    ValidationResult Validate();
}

// Usage
[HttpPost]
public async Task<IActionResult> CreateUser(CreateUserRequest request)
{
    return ValidateAndProcess(request, async req => 
    {
        await _userService.CreateAsync(req);
        return Ok();
    });
}
```

### Custom Validation Middleware
```csharp
public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationExceptionMiddleware> _logger;
    
    public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning($"Validation failed: {ex.Message}");
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Validation failed",
                details = ex.Errors
            });
        }
    }
}

public class ValidationException : Exception
{
    public List<string> Errors { get; }
    
    public ValidationException(string message, List<string> errors) : base(message)
    {
        Errors = errors;
    }
}

// Program.cs
app.UseMiddleware<ValidationExceptionMiddleware>();
```

## Best Practices

1. **Validate Early**
```csharp
// Good: Validate at entry point
[HttpPost]
public async Task<IActionResult> CreateUser(CreateUserRequest request)
{
    var result = await _validator.ValidateAsync(request);
    if (!result.IsSuccessful)
        return BadRequest(result.Errors);
    
    return Ok();
}

// Bad: Validate deep in business logic
public async Task CreateUserAsync(CreateUserRequest request)
{
    // ... 50 lines of code ...
    if (string.IsNullOrEmpty(request.Name)) // Too late!
        throw new Exception("Invalid");
}
```

2. **Separate Validation Rules**
```csharp
// Good: Separate validators
public class UserEmailValidator : AbstractValidator<string>
{
    public UserEmailValidator()
    {
        RuleFor(x => x)
            .EmailAddress()
            .MustAsync(CheckUnique);
    }
}

// Bad: Mixed concerns
public bool IsValidUser(User user)
{
    // Email, password, age, format all mixed together
}
```

3. **Use Type-Safe Validation**
```csharp
// Good: Fluent validation with strongly-typed rules
RuleFor(x => x.Email).EmailAddress();

// Bad: String-based magic validation
ValidateProperty("Email", "email");
```

## Common Mistakes

1. **No Server-Side Validation**
```csharp
// Bad: Only client-side (browser can be bypassed)
// No validation in controller

// Good: Always validate server-side
[HttpPost]
public async Task<IActionResult> CreateUser(CreateUserRequest request)
{
    var validationResult = await _validator.ValidateAsync(request);
    if (!validationResult.IsValid)
        return BadRequest(validationResult.Errors);
}
```

2. **Vague Error Messages**
```csharp
// Bad: Generic message
return BadRequest("Invalid input");

// Good: Specific error per field
return BadRequest(new 
{ 
    email = "Email already exists",
    password = "Must contain uppercase letter"
});
```

3. **Validation Logic in Domain Model**
```csharp
// Bad: Business logic mixed with validation
public class User
{
    public void UpdateEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) // Validation here
            throw new Exception();
        Email = email;
    }
}

// Good: Separate validation concern
public class UpdateEmailValidator : AbstractValidator<UpdateEmailRequest>
{
    public UpdateEmailValidator() 
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}
```

## Quick Summary
- Data Annotations: Simple, built-in validation
- Fluent Validation: Complex, reusable, async support
- Custom attributes: Extensible validation logic
- Validate early at API boundary
- Always validate server-side, never trust client
- Return specific error messages per field
- Use async validators for database checks
- Separate validation concerns from domain logic
- Test validation rules independently
- Provide clear error messages to clients

## Resources
- Data Annotations
- Fluent Validation
- ASP.NET Core Validation
- OWASP Input Validation
