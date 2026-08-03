# API Design

## Overview
Designing robust, scalable, and maintainable APIs that clients can use effectively.

## REST Principles

### Resources
- **Model**: Everything as resource (users, posts, comments)
- **Identification**: URIs identify resources uniquely
- **Examples**: 
  - `/users/123` - user with ID 123
  - `/users/123/posts` - posts by user 123
  - `/posts/456` - post with ID 456

### HTTP Verbs
- **GET**: Retrieve resource (safe, idempotent)
- **POST**: Create new resource (not idempotent)
- **PUT**: Replace entire resource (idempotent)
- **PATCH**: Partial update (idempotent)
- **DELETE**: Remove resource (idempotent)
- **HEAD**: Like GET but no body (metadata only)

### HTTP Status Codes
- **2xx Success**:
  - 200: OK - Request succeeded
  - 201: Created - Resource created
  - 204: No Content - Success, no response body

- **3xx Redirection**:
  - 301: Moved Permanently
  - 302: Found - Temporary redirect
  - 304: Not Modified - Cached version valid

- **4xx Client Error**:
  - 400: Bad Request - Invalid input
  - 401: Unauthorized - Authentication required
  - 403: Forbidden - Authenticated but not authorized
  - 404: Not Found - Resource doesn't exist
  - 409: Conflict - Request conflicts with current state
  - 429: Too Many Requests - Rate limited

- **5xx Server Error**:
  - 500: Internal Server Error - Unexpected error
  - 503: Service Unavailable - Temporary outage

### Headers
- **Content-Type**: Media type of response body
- **Accept**: Client's preferred response format
- **Authorization**: Authentication credentials
- **Cache-Control**: Cache instructions
- **ETag**: Version identifier for resource
- **Link**: Related resources
- **Vary**: Conditions affecting response

## API Versioning

### URL Path Versioning
```
/v1/users
/v2/users
```
- **Pros**: Clear, easy to understand
- **Cons**: URL proliferation, parallel versions

### Query Parameter
```
/users?version=2
/users?api_version=2
```
- **Pros**: Single URL
- **Cons**: Less obvious, harder to route

### Header-Based
```
Accept: application/json; version=2
API-Version: 2
```
- **Pros**: Clean URLs
- **Cons**: Less discoverable

### Accept Header (Content Negotiation)
```
Accept: application/vnd.api+json; version=2
```
- **Pros**: Pure REST approach
- **Cons**: Complex for clients

### Strategy Recommendation
- Start with URL path versioning for clarity
- Deprecate old versions on schedule
- Provide migration path for clients

## Rate Limiting

### Purpose
- Prevent abuse
- Ensure fair usage
- Protect against DOS attacks
- Encourage caching

### Strategies

#### Token Bucket
- **Bucket**: Fixed capacity
- **Tokens**: Added at fixed rate
- **Request**: Costs 1 token
- **Pros**: Allows bursts, fair over time

#### Sliding Window
- **Tracking**: Requests in time window
- **Limit**: Max requests per window
- **Pros**: Simple, accurate
- **Cons**: Memory overhead

#### Leaky Bucket
- **Queue**: Requests queue up
- **Processing**: Drained at fixed rate
- **Pros**: Smooth, predictable
- **Cons**: Requests might queue indefinitely

### Implementation
- **Per IP**: Rate limit by IP address
- **Per User**: Rate limit by authenticated user
- **Per Endpoint**: Different limits for different endpoints
- **Global**: Total system limit

### Communicating Limits
```
Headers:
X-RateLimit-Limit: 1000
X-RateLimit-Remaining: 543
X-RateLimit-Reset: 1356998400
```

## Error Handling

### Error Response Format
```json
{
  "error": {
    "code": "INVALID_INPUT",
    "message": "User email is invalid",
    "details": [
      {
        "field": "email",
        "message": "Invalid email format"
      }
    ]
  }
}
```

### Error Codes
- **Business Logic**: Meaningful, user-friendly
- **Consistency**: Same errors always same code
- **Documentation**: All possible errors documented
- **Examples**: INVALID_INPUT, USER_NOT_FOUND, PERMISSION_DENIED

### Logging
- **Log All Errors**: Track failures
- **Correlation ID**: Trace request through system
- **Context**: Include request details
- **Sensitive Data**: Avoid logging PII

## Documentation

### OpenAPI/Swagger
- **Standard**: Machine-readable API specification
- **Benefits**: Auto-generated docs, client SDKs, testing tools
- **Sections**:
  - Paths: Available endpoints
  - Parameters: Query, path, header, body
  - Responses: Status codes, schemas
  - Examples: Sample requests/responses

### Best Practices
- **Examples**: Include actual examples
- **Schemas**: Define all request/response formats
- **Authentication**: Document auth requirements
- **Rate Limits**: Document limits per endpoint
- **Updates**: Keep docs in sync with code

## Pagination

### Limit/Offset
```
GET /users?limit=20&offset=40
```
- **Pros**: Simple, works for sorting
- **Cons**: Doesn't handle insertions/deletions well

### Cursor-Based
```
GET /users?limit=20&cursor=abc123
```
- **Pros**: Handles insertions/deletions, faster
- **Cons**: More complex

### Key-Set Pagination
```
GET /users?limit=20&last_id=123
```
- **Pros**: Efficient, no offset scanning
- **Cons**: Must sort consistently

## API Design Patterns

### HATEOAS (Hypermedia)
- Include links to related resources
- **Pros**: Self-documenting, discoverable
- **Cons**: Increased response size, less common

### JSON API Standard
- Standardized format for APIs
- Conventions for:
  - Error responses
  - Pagination
  - Relationships
  - Filtering/sorting

### GraphQL (Alternative to REST)
- **Approach**: Query language for APIs
- **Pros**: 
  - Clients specify exactly what data needed
  - Single endpoint
  - Avoid over/under-fetching
- **Cons**: 
  - Complexity, steeper learning curve
  - Caching more complex
  - N+1 query problem

### gRPC (Alternative to REST)
- **Protocol**: Binary, HTTP/2
- **Pros**: Very fast, streaming support, strong typing
- **Cons**: Less human-readable, requires code generation

## WebSockets and Real-time

### Long Polling
- Client repeatedly polls server for updates
- **Pros**: Works with standard HTTP
- **Cons**: Inefficient, high latency

### Server-Sent Events (SSE)
- Server pushes data to client
- **Pros**: Simple, works with HTTP
- **Cons**: One-way communication

### WebSocket
- Bidirectional communication over TCP
- **Pros**: True real-time, efficient
- **Cons**: Stateful, more complex infrastructure

## API Security

### Authentication
- **Basic Auth**: Username/password in header
- **API Key**: Token provided by client
- **OAuth**: Delegated access
- **JWT**: Self-contained tokens

### HTTPS/TLS
- **Requirement**: Encrypt all traffic
- **Certificates**: Valid, updated regularly
- **Certificate Pinning**: Enhanced security for mobile

### CORS (Cross-Origin Resource Sharing)
- **Problem**: Browsers restrict cross-domain requests
- **Solution**: Server specifies allowed origins
- **Headers**: Access-Control-Allow-Origin, etc.

### Input Validation
- **Sanitization**: Remove dangerous characters
- **Type Checking**: Validate data types
- **Range Checking**: Values within acceptable ranges
- **Business Logic**: Validate against rules

## API Versioning Strategy

### Backward Compatibility
- Maintain old API versions
- New versions for breaking changes
- Deprecation schedule announced

### Deprecation Process
1. **Announce**: Communicate deprecation date
2. **Grace Period**: Time for clients to migrate (6-12 months typical)
3. **Sunset**: Remove old version
4. **Support**: Help clients migrate

## Monitoring APIs

### Metrics
- **Latency**: Response time p50, p95, p99
- **Throughput**: Requests per second
- **Error Rate**: Failed requests percentage
- **Status Code**: Distribution of response codes

### Logging
- **Request/Response**: Payload details
- **Timing**: How long each part took
- **Errors**: Stack traces, error messages
- **User Context**: User ID, correlation ID

## API Design Checklist

- [ ] Resource-oriented design (not action-oriented)
- [ ] Proper HTTP verbs and status codes
- [ ] Consistent error responses
- [ ] Documentation complete and current
- [ ] Versioning strategy clear
- [ ] Rate limiting implemented
- [ ] Authentication and authorization
- [ ] Input validation
- [ ] CORS handled if needed
- [ ] Monitoring and alerting
- [ ] Pagination for list endpoints
- [ ] Filtering and sorting options
- [ ] Response formats documented
- [ ] Performance acceptable
- [ ] Security reviewed

## Practice Files
- **01-Explanation**: REST principles, API patterns, best practices
- **02-Architecture-Diagrams**: API architecture, authentication flows
- **03-Code-Examples**: REST API implementation, error handling, documentation
