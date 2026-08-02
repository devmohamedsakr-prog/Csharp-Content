# Web Development - Interview Questions & Answers

## 1. What is ASP.NET Core and how is it different from ASP.NET Framework?

**Answer:**

ASP.NET Core is a modern, cross-platform rewrite of ASP.NET Framework.

| Feature | ASP.NET Framework | ASP.NET Core |
|---------|------------------|-------------|
| Platform | Windows only | Cross-platform |
| Performance | Moderate | Very fast |
| Architecture | Monolithic | Modular |
| Dependency Injection | Optional add-on | Built-in |
| Configuration | Web.config | appsettings.json |
| Hosting | IIS only | Any host |
| License | Closed | Open source |

**Why ASP.NET Core**:
- Better performance (industry benchmarks show 2-5x faster)
- Works on Linux, Mac, Windows
- Modern design patterns built-in
- Lightweight, can run on any infrastructure

---

## 2. What is the MVC pattern and how does ASP.NET Core implement it?

**Answer:**

MVC (Model-View-Controller) separates concerns into three components.

```csharp
// Model - Data and business logic
public class Student {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal GPA { get; set; }
}

// Controller - Handles requests and orchestrates
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase {
    private readonly IStudentService _service;
    
    public StudentController(IStudentService service) {
        _service = service;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id) {
        var student = await _service.GetStudentAsync(id);
        if (student == null) return NotFound();
        return Ok(student);
    }
}

// View - Razor template (returns HTML)
@model Student
<h1>@Model.Name</h1>
<p>GPA: @Model.GPA</p>
```

---

## 3. What is dependency injection and why is it important?

**Answer:**

Dependency Injection (DI) provides dependencies to a class rather than having it create them.

```csharp
// Without DI - tightly coupled
public class StudentController {
    private StudentService _service;  // Created here
    
    public StudentController() {
        _service = new StudentService();  // Tightly coupled
    }
}

// With DI - loosely coupled
public class StudentController {
    private readonly IStudentService _service;
    
    public StudentController(IStudentService service) {
        _service = service;  // Injected
    }
}

// Registration in Startup
services.AddScoped<IStudentService, StudentService>();
services.AddScoped<StudentController>();
```

**Benefits**:
- Easier testing (mock dependencies)
- Loose coupling (easy to change implementations)
- Better maintainability
- More flexible architecture

---

## 4. What are the different lifetimes for dependency injection?

**Answer:**

```csharp
// Transient - new instance every time
services.AddTransient<IService, Service>();
// Each request gets new instance

// Scoped - new instance per request/scope
services.AddScoped<IService, Service>();
// Same instance within a request, new for next request

// Singleton - single instance for application lifetime
services.AddSingleton<IService, Service>();
// Same instance for entire application

// Practical example
services.AddTransient<ILogger, Logger>();      // Always new
services.AddScoped<IRepository, Repository>();  // Per request
services.AddSingleton<ICache, Cache>();        // Shared
```

**When to Use**:
- **Transient**: Stateless services, lightweight objects
- **Scoped**: Database context, per-request services
- **Singleton**: Caches, configuration, loggers

---

## 5. What is routing in ASP.NET Core?

**Answer:**

Routing maps URLs to controller actions.

```csharp
// Attribute-based routing (recommended)
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase {
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id) {
        // GET /api/student/5
        return await _service.GetStudentAsync(id);
    }
    
    [HttpPost]
    public async Task<ActionResult<Student>> CreateStudent([FromBody] Student student) {
        // POST /api/student
        return await _service.CreateStudentAsync(student);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStudent(int id, [FromBody] Student student) {
        // PUT /api/student/5
        await _service.UpdateStudentAsync(id, student);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudent(int id) {
        // DELETE /api/student/5
        await _service.DeleteStudentAsync(id);
        return NoContent();
    }
}

// Convention-based routing (less common now)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

---

## 6. What is middleware and how does the request pipeline work?

**Answer:**

Middleware processes HTTP requests and responses.

```csharp
// Configure middleware in Startup.cs
public void Configure(IApplicationBuilder app) {
    // Middleware order matters!
    
    app.UseExceptionHandler();        // Exception handling
    app.UseHttpsRedirection();        // HTTPS redirect
    app.UseRouting();                 // Route matching
    app.UseAuthentication();          // Check credentials
    app.UseAuthorization();           // Check permissions
    
    app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
    });
}

// Custom middleware
public class LoggingMiddleware {
    private readonly RequestDelegate _next;
    
    public LoggingMiddleware(RequestDelegate next) {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context) {
        Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
        await _next(context);  // Call next middleware
        Console.WriteLine($"Response: {context.Response.StatusCode}");
    }
}

// Register custom middleware
app.UseMiddleware<LoggingMiddleware>();
```

**Request Pipeline**: Request → Middleware 1 → Middleware 2 → ... → Endpoint → Response

---

## 7. What is authentication and authorization?

**Answer:**

**Authentication**: Verify who you are
**Authorization**: Verify what you can do

```csharp
// Authentication - Login
[HttpPost("login")]
public async Task<ActionResult<string>> Login([FromBody] LoginRequest request) {
    var user = await _userService.AuthenticateAsync(request.Username, request.Password);
    if (user == null) return Unauthorized();
    
    var token = GenerateJwtToken(user);
    return Ok(new { token });
}

// Authorization - Role-based
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<ActionResult> DeleteUser(int id) {
    await _userService.DeleteUserAsync(id);
    return NoContent();
}

// Policy-based authorization
services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

[Authorize(Policy = "AdminOnly")]
public IActionResult AdminPanel() {
    return View();
}

// Setup authentication
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });
```

---

## 8. What is a REST API and what are HTTP status codes?

**Answer:**

REST (Representational State Transfer) uses HTTP methods for operations.

```csharp
// REST endpoints
GET    /api/students         // Retrieve all
GET    /api/students/5       // Retrieve one
POST   /api/students         // Create
PUT    /api/students/5       // Update
DELETE /api/students/5       // Delete

// Common HTTP Status Codes
200 OK                   - Request successful
201 Created              - Resource created
204 No Content           - Successful, no response body
400 Bad Request          - Invalid request
401 Unauthorized         - Authentication required
403 Forbidden            - Authorized, but no permission
404 Not Found            - Resource not found
500 Internal Server Error - Server error

// Using status codes in ASP.NET Core
[HttpGet("{id}")]
public async Task<ActionResult<Student>> GetStudent(int id) {
    var student = await _service.GetStudentAsync(id);
    if (student == null) return NotFound();  // 404
    return Ok(student);  // 200
}

[HttpPost]
public async Task<ActionResult<Student>> CreateStudent([FromBody] Student student) {
    var created = await _service.CreateStudentAsync(student);
    return CreatedAtAction(nameof(GetStudent), new { id = created.Id }, created);  // 201
}

[HttpDelete("{id}")]
public async Task<ActionResult> DeleteStudent(int id) {
    await _service.DeleteStudentAsync(id);
    return NoContent();  // 204
}
```

---

## 9. What is model binding and validation?

**Answer:**

Model Binding: Converting HTTP request data to action parameters
Validation: Ensuring data meets requirements

```csharp
// Model with validation attributes
public class Student {
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }
    
    [Range(0, 4.0, ErrorMessage = "GPA must be between 0 and 4.0")]
    public decimal GPA { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    [Range(1, 150)]
    public int Age { get; set; }
}

// Controller with validation
[HttpPost]
public async Task<ActionResult<Student>> CreateStudent([FromBody] Student student) {
    if (!ModelState.IsValid) {
        return BadRequest(ModelState);  // Returns validation errors
    }
    
    var created = await _service.CreateStudentAsync(student);
    return CreatedAtAction(nameof(GetStudent), new { id = created.Id }, created);
}

// Custom validation
public class CustomAgeAttribute : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext context) {
        if (value is int age && age >= 18 && age <= 100) {
            return ValidationResult.Success;
        }
        return new ValidationResult("Age must be between 18 and 100");
    }
}

public class Student {
    [CustomAge]
    public int Age { get; set; }
}
```

---

## 10. What is configuration in ASP.NET Core?

**Answer:**

Configuration from appsettings.json and environment variables.

```csharp
// appsettings.json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information"
        }
    },
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=MyDb"
    },
    "Jwt": {
        "Key": "your-secret-key-here",
        "Issuer": "your-issuer",
        "Audience": "your-audience"
    }
}

// Access configuration
public class Startup {
    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services) {
        // Get connection string
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        
        // Get JWT settings
        var jwtKey = Configuration["Jwt:Key"];
        
        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}

// Environment-specific settings
// appsettings.Development.json
// appsettings.Production.json
```

---

## 11. What is CORS and when is it needed?

**Answer:**

CORS (Cross-Origin Resource Sharing) allows frontend and backend on different domains.

```csharp
// Configure CORS
public void ConfigureServices(IServiceCollection services) {
    services.AddCors(options => {
        options.AddPolicy("AllowReactApp", builder =>
            builder.WithOrigins("https://myreactapp.com")
                   .AllowAnyMethod()
                   .AllowAnyHeader());
    });
}

public void Configure(IApplicationBuilder app) {
    app.UseCors("AllowReactApp");
    
    // Or more permissive (development only)
    app.UseCors(builder => builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader());
}

// Decorate controller or action
[EnableCors("AllowReactApp")]
public class StudentController : ControllerBase {
}

// Disable CORS for specific action
[DisableCors]
[HttpGet("public")]
public IActionResult GetPublic() {
}
```

---

## 12. What are common security concerns in web APIs?

**Answer:**

```csharp
// 1. SQL Injection - use parameterized queries
// ✗ Bad
var students = context.Students.FromSqlRaw($"SELECT * FROM Students WHERE Name = '{name}'");

// ✓ Good
var students = context.Students.FromSqlRaw("SELECT * FROM Students WHERE Name = @name", 
    new SqlParameter("@name", name));

// 2. Cross-Site Scripting (XSS) - sanitize user input
[HttpPost]
public async Task<ActionResult> CreatePost([FromBody] Post post) {
    post.Content = HtmlSanitizer.Sanitize(post.Content);
    await _service.CreatePostAsync(post);
    return Ok();
}

// 3. Cross-Site Request Forgery (CSRF) - use tokens
services.AddAntiforgery();

[ValidateAntiForgeryToken]
[HttpPost]
public IActionResult DeleteItem(int id) {
    // Safe from CSRF
}

// 4. Authentication & Authorization
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<ActionResult> DeleteUser(int id) {
    // Protected endpoint
}

// 5. Use HTTPS
app.UseHttpsRedirection();

// 6. Validate input
[HttpPost]
public async Task<ActionResult> Create([FromBody] [Required] User user) {
    if (!ModelState.IsValid) {
        return BadRequest(ModelState);
    }
}
```

---

## Quick Tips for Interview

✓ Know ASP.NET Core advantages over Framework
✓ Understand MVC pattern
✓ Know dependency injection lifetimes
✓ Comfortable with routing and middleware
✓ Understand authentication vs authorization
✓ Know REST principles and status codes
✓ Explain model binding and validation
✓ Know configuration sources
✓ Understand CORS use cases
✓ Know common security concerns
