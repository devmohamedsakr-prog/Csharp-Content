# REST APIs and HTTP

## Overview
RESTful principles for building web APIs using HTTP methods and status codes effectively.

## HTTP Methods

### GET - Retrieve Data
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<User>>> GetUsers()
{
    var users = await _repository.GetAllAsync();
    return Ok(users); // 200 OK
}

[HttpGet("{id}")]
public async Task<ActionResult<User>> GetUser(int id)
{
    var user = await _repository.GetAsync(id);
    if (user == null)
        return NotFound(); // 404 Not Found
    
    return Ok(user); // 200 OK
}
```

### POST - Create Data
```csharp
[HttpPost]
public async Task<ActionResult<UserDto>> CreateUser(CreateUserRequest request)
{
    var user = new User { Name = request.Name, Email = request.Email };
    await _repository.AddAsync(user);
    await _repository.SaveAsync();
    
    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    // 201 Created with Location header
}
```

### PUT - Replace Entire Resource
```csharp
[HttpPut("{id}")]
public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
{
    var user = await _repository.GetAsync(id);
    if (user == null)
        return NotFound(); // 404
    
    user.Name = request.Name;
    user.Email = request.Email;
    
    await _repository.SaveAsync();
    return NoContent(); // 204 No Content
}
```

### PATCH - Partial Update
```csharp
[HttpPatch("{id}")]
public async Task<IActionResult> PartialUpdateUser(int id, JsonPatchDocument<UpdateUserRequest> patchDoc)
{
    var user = await _repository.GetAsync(id);
    if (user == null)
        return NotFound();
    
    var updateRequest = new UpdateUserRequest { Name = user.Name, Email = user.Email };
    patchDoc.ApplyTo(updateRequest, ModelState);
    
    if (!ModelState.IsValid)
        return BadRequest(ModelState); // 400 Bad Request
    
    user.Name = updateRequest.Name;
    user.Email = updateRequest.Email;
    
    await _repository.SaveAsync();
    return NoContent(); // 204
}
```

### DELETE - Remove Resource
```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteUser(int id)
{
    var user = await _repository.GetAsync(id);
    if (user == null)
        return NotFound(); // 404
    
    _repository.Remove(user);
    await _repository.SaveAsync();
    
    return NoContent(); // 204 No Content
}
```

## HTTP Status Codes

### Success (2xx)
```csharp
// 200 OK - Request succeeded
return Ok(data);

// 201 Created - Resource created
return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);

// 202 Accepted - Request accepted but not complete
return Accepted();

// 204 No Content - Request succeeded but no content to return
return NoContent();
```

### Redirection (3xx)
```csharp
// 301 Moved Permanently
return MovedPermanently("/api/newpath");

// 302 Found
return Redirect("/api/newpath");

// 304 Not Modified
return StatusCode(304);
```

### Client Errors (4xx)
```csharp
// 400 Bad Request - Invalid input
return BadRequest("Invalid email format");

// 401 Unauthorized - Not authenticated
return Unauthorized();

// 403 Forbidden - No permission
return Forbid();

// 404 Not Found - Resource doesn't exist
return NotFound();

// 409 Conflict - Request conflicts with current state
return Conflict("Email already exists");

// 422 Unprocessable Entity - Validation failed
return UnprocessableEntity(errors);
```

### Server Errors (5xx)
```csharp
// 500 Internal Server Error
return StatusCode(500, "An error occurred");

// 503 Service Unavailable
return StatusCode(503, "Service temporarily unavailable");
```

## RESTful Design

### Resource-Based URLs
```csharp
// Good: Resource-based (nouns)
GET    /api/users           // Get all users
POST   /api/users           // Create user
GET    /api/users/5         // Get user 5
PUT    /api/users/5         // Update user 5
DELETE /api/users/5         // Delete user 5

GET    /api/users/5/posts   // Get user 5's posts
POST   /api/users/5/posts   // Create post for user 5

// Bad: Action-based (verbs)
GET    /api/getUsers
POST   /api/createUser
GET    /api/deleteUser/5
```

### Versioning
```csharp
// URL versioning
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        // V1 implementation
    }
}

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersV2Controller : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        // V2 implementation with breaking changes
    }
}

// Usage: /api/v1/users vs /api/v2/users
```

### Pagination
```csharp
[HttpGet]
public async Task<ActionResult<PagedResult<User>>> GetUsers(int page = 1, int pageSize = 10)
{
    var skip = (page - 1) * pageSize;
    
    var users = await _repository
        .Query()
        .Skip(skip)
        .Take(pageSize)
        .ToListAsync();
    
    var total = await _repository.CountAsync();
    
    return Ok(new PagedResult<User>
    {
        Data = users,
        Page = page,
        PageSize = pageSize,
        Total = total,
        TotalPages = (total + pageSize - 1) / pageSize
    });
}

public class PagedResult<T>
{
    public List<T> Data { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }
}
```

### Filtering and Sorting
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<User>>> GetUsers(
    string name = null,
    string sortBy = "name",
    bool descending = false)
{
    var query = _repository.Query();
    
    // Filter
    if (!string.IsNullOrEmpty(name))
        query = query.Where(u => u.Name.Contains(name));
    
    // Sort
    query = sortBy?.ToLower() switch
    {
        "email" => descending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
        _ => descending ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name)
    };
    
    var users = await query.ToListAsync();
    return Ok(users);
}

// Usage: GET /api/users?name=John&sortBy=email&descending=true
```

## Best Practices

1. **Use Appropriate HTTP Methods**
```csharp
// Good: Method matches operation
POST   /api/users        // Create
GET    /api/users/5      // Read
PUT    /api/users/5      // Update
DELETE /api/users/5      // Delete

// Bad: All operations via POST
POST   /api/getUser/5
POST   /api/createUser
POST   /api/updateUser/5
```

2. **Return Correct Status Codes**
```csharp
// Good
if (user == null) return NotFound();
if (!ModelState.IsValid) return BadRequest(ModelState);
return Ok(data);

// Bad
return Ok(null); // Should be NotFound
return Ok("Error: Invalid input"); // Should be BadRequest
```

3. **Use Content Negotiation**
```csharp
// Supports both JSON and XML
[HttpGet]
public async Task<ActionResult<User>> GetUser(int id)
{
    var user = await _repository.GetAsync(id);
    return Ok(user); // Returns JSON or XML based on Accept header
}

// Usage:
// Accept: application/json -> Returns JSON
// Accept: application/xml -> Returns XML
```

4. **Include Relevant Headers**
```csharp
[HttpPost]
public async Task<ActionResult<User>> CreateUser(CreateUserRequest request)
{
    var user = new User { /* ... */ };
    await _repository.AddAsync(user);
    
    Response.Headers.Add("X-Total-Count", totalCount.ToString());
    
    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
}
```

## Common Mistakes

1. **Inconsistent HTTP Methods**
```csharp
// Bad: Using GET to modify
[HttpGet("{id}")]
public IActionResult DeleteUser(int id) // Should be DELETE
{
    _repository.Remove(id);
    return Ok();
}

// Good
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteUser(int id)
{
    await _repository.RemoveAsync(id);
    return NoContent();
}
```

2. **Poor Error Responses**
```csharp
// Bad: Generic error
return BadRequest("Error");

// Good: Descriptive error
return BadRequest(new { error = "Email is required", field = "email" });
```

3. **Over-fetching Data**
```csharp
// Bad: Returns everything
[HttpGet("{id}")]
public async Task<User> GetUser(int id)
{
    return await _repository.GetAsync(id); // Includes all properties
}

// Good: Return only needed fields
[HttpGet("{id}")]
public async Task<ActionResult<UserDto>> GetUser(int id)
{
    var user = await _repository.GetAsync(id);
    var dto = new UserDto { Id = user.Id, Name = user.Name };
    return Ok(dto);
}
```

## Quick Summary
- GET: Retrieve, safe and idempotent
- POST: Create, not idempotent
- PUT: Replace entire resource, idempotent
- PATCH: Partial update, idempotent
- DELETE: Remove, idempotent
- 2xx: Success, 4xx: Client error, 5xx: Server error
- Resource-based URLs (nouns not verbs)
- Pagination, filtering, sorting
- Proper status codes matter
- Include Location header for 201
- Use DTOs to control response shape

## Resources
- REST API Best Practices
- HTTP Status Codes Reference
- API Design Patterns
- ASP.NET Core Routing
