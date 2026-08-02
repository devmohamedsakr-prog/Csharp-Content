# Authentication and Authorization

## Overview
Authentication (who you are) vs Authorization (what you can do) with JWT and role-based access.

## Authentication with JWT

### Token Generation
```csharp
public interface ITokenService
{
    string GenerateToken(User user);
    ClaimsPrincipal ValidateToken(string token);
}

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _config;
    
    public JwtTokenService(IConfiguration config) => _config = config;
    
    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("Role", string.Join(",", user.Roles))
        };
        
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public ClaimsPrincipal ValidateToken(string token)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
        
        var handler = new JwtSecurityTokenHandler();
        
        try
        {
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = true
            }, out SecurityToken validatedToken);
            
            return principal;
        }
        catch
        {
            return null;
        }
    }
}
```

### Login Endpoint
```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var user = await _userService.GetByUsernameAsync(request.Username);
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials" });
        
        var token = _tokenService.GenerateToken(user);
        
        return Ok(new LoginResponse
        {
            Token = token,
            User = new { user.Id, user.Username, user.Email }
        });
    }
    
    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
    public object User { get; set; }
}
```

### JWT Configuration
```csharp
// appsettings.json
{
  "Jwt": {
    "SecretKey": "your-very-long-secret-key-at-least-32-chars",
    "Issuer": "https://yourapi.com",
    "Audience": "https://yourapp.com",
    "ExpirationMinutes": 60
  }
}

// Program.cs
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true
        };
    });

app.UseAuthentication();
app.UseAuthorization();
```

## Authorization

### Role-Based Authorization
```csharp
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllUsers()
    {
        // Only admins can access
        return Ok();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        // Admins or Moderators can access
        return Ok();
    }
}
```

### Policy-Based Authorization
```csharp
// Program.cs
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"))
    .AddPolicy("AtLeast21", policy =>
        policy.Requirements.Add(new MinimumAgeRequirement(21)))
    .AddPolicy("EmailVerified", policy =>
        policy.Requirements.Add(new EmailVerifiedRequirement()));

// Custom requirement
public class MinimumAgeRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }
    
    public MinimumAgeRequirement(int minimumAge)
    {
        MinimumAge = minimumAge;
    }
}

public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        if (context.User.FindFirst(ClaimTypes.DateOfBirth) is ClaimValue dob &&
            DateTime.TryParse(dob.Value, out var dateOfBirth))
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (age >= requirement.MinimumAge)
                context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}

// Usage
[Authorize(Policy = "AtLeast21")]
public IActionResult GetAgeRestrictedContent()
{
    return Ok();
}
```

### Attribute-Based Authorization
```csharp
[ApiController]
[Authorize] // Require authentication
public class ProtectedController : ControllerBase
{
    [HttpGet]
    public IActionResult GetPublicData()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(new { message = $"Hello User {userId}" });
    }
    
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAdminData()
    {
        return Ok(new { message = "Admin only" });
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login()
    {
        return Ok();
    }
}
```

## OAuth2 / OpenID Connect

### Third-Party Integration
```csharp
// Program.cs
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "google";
})
.AddCookie("cookie")
.AddGoogle("google", options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"];
    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
});

// Controller
[HttpGet("login-google")]
public IActionResult LoginWithGoogle()
{
    return Challenge(new AuthenticationProperties 
    { 
        RedirectUri = "/" 
    }, "google");
}

[HttpGet("callback")]
public async Task<IActionResult> GoogleCallback()
{
    var result = await HttpContext.AuthenticateAsync();
    if (!result.Succeeded)
        return BadRequest();
    
    var claims = result.Principal.Claims.ToList();
    var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
    
    return Ok(new { email });
}
```

## Best Practices

1. **Never Log Passwords**
```csharp
// Bad
_logger.LogInformation($"User {username} logged in with password {password}");

// Good
_logger.LogInformation($"User {username} logged in successfully");
```

2. **Hash Passwords**
```csharp
// Good: Use BCrypt
var hash = BCrypt.Net.BCrypt.HashPassword(password);

// Store hash in database, not plaintext
user.PasswordHash = hash;
```

3. **Validate Tokens on Every Protected Request**
```csharp
[Authorize]
[HttpGet]
public IActionResult Protected()
{
    // Token validated automatically by middleware
    return Ok();
}
```

4. **Use HTTPS Only**
```csharp
// Program.cs
app.UseHsts(); // HTTP Strict Transport Security
app.UseHttpsRedirection();
```

## Common Mistakes

1. **Storing Secrets in Code**
```csharp
// Bad
var secret = "my-secret-key-123";

// Good
var secret = _configuration["Jwt:SecretKey"];
```

2. **Long Token Expiration**
```csharp
// Bad
expires: DateTime.UtcNow.AddYears(1) // Way too long

// Good
expires: DateTime.UtcNow.AddHours(1) // Reasonable
```

3. **Not Validating Claims**
```csharp
// Bad: Trusts client-provided claims
var role = User.FindFirst("Role")?.Value;

// Good: Validate server-side
var role = User.IsInRole("Admin");
```

## Quick Summary
- Authentication: Verify identity (JWT, OAuth2)
- Authorization: Check permissions (Roles, Policies)
- JWT: Self-contained tokens with claims
- Always hash passwords (BCrypt, PBKDF2)
- Validate on every protected request
- Use HTTPS for token transmission
- Keep tokens short-lived
- Use policies for complex authorization
- Never log sensitive data
- Implement refresh tokens for long sessions

## Resources
- JWT Authentication
- OAuth2 / OpenID Connect
- ASP.NET Core Authentication
- OWASP Authentication Cheat Sheet
