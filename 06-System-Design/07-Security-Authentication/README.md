# Security and Authentication

## Overview
Protecting systems from unauthorized access, data breaches, and attacks.

## Authentication

### Authentication vs Authorization
- **Authentication**: Verify user identity (who are you?)
- **Authorization**: Determine permissions (what can you do?)

### Authentication Methods

#### Basic Authentication
```
Authorization: Basic base64(username:password)
```
- **Pros**: Simple to implement
- **Cons**: 
  - Sends credentials with every request
  - No encryption (must use HTTPS)
  - Logout doesn't invalidate
- **Use**: Simple internal APIs, not recommended for public

#### API Keys
```
Authorization: Bearer abc123def456
```
- **Pros**: Simple, suitable for service-to-service
- **Cons**: 
  - No user context
  - Lost key = full account access
  - No expiration unless managed
- **Use**: Service authentication, third-party integrations

#### Session-Based (Cookies)
- **Flow**:
  1. User submits username/password
  2. Server validates, creates session
  3. Server returns session ID in cookie
  4. Browser includes cookie in subsequent requests
- **Pros**: Familiar to users, CSRF protection possible
- **Cons**: Stateful (server stores sessions), scaling challenges
- **Use**: Traditional web applications

#### JWT (JSON Web Token)
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```
- **Structure**: Header.Payload.Signature
- **Pros**: 
  - Stateless (no server-side storage)
  - Self-contained user information
  - Scales well
- **Cons**: 
  - Revocation complex
  - Token size larger
  - Subject to tampering if secret exposed
- **Use**: Modern APIs, microservices, mobile apps

#### OAuth 2.0
- **Purpose**: Delegated access without sharing password
- **Flow**: 
  1. User redirected to auth provider
  2. User authorizes app
  3. App receives access token
  4. App uses token to access resources
- **Pros**: Secure, user never shares password with app
- **Cons**: More complex to implement
- **Use**: Social logins (Google, GitHub), third-party access

#### SAML
- **Purpose**: Enterprise single sign-on
- **Pros**: Mature, widely used in enterprises
- **Cons**: Complexity, XML-based
- **Use**: Enterprise applications

### Best Practices
- **HTTPS Only**: Always encrypt credentials in transit
- **Password Hashing**: Use bcrypt, Argon2, not SHA
- **Salt**: Use unique salt per password
- **Secure Storage**: Never log credentials
- **Rate Limiting**: Prevent brute force attacks
- **2FA**: Multi-factor authentication when possible

## Authorization

### Role-Based Access Control (RBAC)
- **Concept**: Users assigned roles, roles have permissions
- **Example**:
  - Role: Admin (permissions: read all, write all, delete all)
  - Role: Editor (permissions: read all, write own posts)
  - Role: Viewer (permissions: read all)
- **Pros**: Simple, easy to manage
- **Cons**: Inflexible for complex scenarios

### Attribute-Based Access Control (ABAC)
- **Concept**: Access decisions based on attributes
- **Example**:
  - User can delete post if: user.id == post.user_id AND resource.status != 'published'
- **Pros**: Fine-grained control
- **Cons**: Complexity, harder to audit

### Access Control List (ACL)
- **Concept**: Explicit permissions per resource
- **Example**: User 123 can read/write Document ABC
- **Pros**: Explicit and secure
- **Cons**: Scalability with many resources

### Principle of Least Privilege
- **Concept**: Users have minimum permissions needed
- **Benefit**: Limits damage if account compromised
- **Practice**: Regularly audit and remove excessive permissions

## Encryption

### Symmetric Encryption
- **Concept**: Same key for encrypt and decrypt
- **Examples**: AES, ChaCha20
- **Pros**: Fast, suitable for bulk data
- **Cons**: Key distribution challenge
- **Use**: Data at rest, high-volume encryption

### Asymmetric Encryption
- **Concept**: Public key (encrypt) and private key (decrypt)
- **Examples**: RSA, elliptic curve
- **Pros**: Solves key distribution problem
- **Cons**: Slower than symmetric
- **Use**: Key exchange, digital signatures, HTTPS

### Hashing
- **Purpose**: One-way function, creates fingerprint
- **Examples**: SHA-256, bcrypt, Argon2
- **Use**: 
  - Password storage
  - Data integrity
  - Content addressing
- **Property**: Same input always produces same hash

### TLS/SSL
- **Purpose**: Encrypt communication channel
- **Certificates**: Verify server identity
- **Versions**: TLS 1.2+, avoid SSL 3.0 and TLS 1.0/1.1
- **Cipher Suites**: Use strong ciphers only

## Data Protection

### PII (Personally Identifiable Information)
- **Examples**: Name, email, phone, address, SSN
- **Protection**:
  - Encrypt in transit (HTTPS)
  - Encrypt at rest for sensitive data
  - Minimize collection
  - Secure backups
  - Access controls

### GDPR Compliance
- **Right to be Forgotten**: Ability to delete personal data
- **Consent**: User agreement before data processing
- **Privacy**: Data protection by default
- **Notification**: Breach notification within 72 hours
- **Access**: User can request their data

### Data Masking
- **Purpose**: Hide sensitive data in logs/test systems
- **Examples**: 
  - Credit card: 1234-****-****-5678
  - Email: j***@example.com
- **Use**: Reduce exposure, maintain privacy

## Common Attacks and Defense

### SQL Injection
- **Attack**: Malicious SQL in input parameters
- **Example**: `' OR '1'='1`
- **Defense**: Parameterized queries, input validation
- **Example (Safe)**:
  ```
  SELECT * FROM users WHERE id = @user_id
  ```

### Cross-Site Scripting (XSS)
- **Attack**: Inject JavaScript into web pages
- **Types**: Stored, reflected, DOM-based
- **Defense**: 
  - HTML escape output
  - Content Security Policy
  - Input validation

### Cross-Site Request Forgery (CSRF)
- **Attack**: Trick user into making unwanted request
- **Example**: Fake form on attacker's site that transfers money
- **Defense**: 
  - CSRF tokens
  - SameSite cookie attribute
  - Verify Origin/Referer headers

### DDoS (Distributed Denial of Service)
- **Attack**: Overwhelm system with traffic
- **Defense**:
  - Rate limiting
  - WAF (Web Application Firewall)
  - CDN protection
  - Capacity planning

### Man-in-the-Middle (MITM)
- **Attack**: Intercept unencrypted communication
- **Defense**: HTTPS/TLS encryption everywhere

## API Security

### CORS Headers
```
Access-Control-Allow-Origin: https://trusted-domain.com
Access-Control-Allow-Methods: GET, POST
Access-Control-Allow-Headers: Content-Type, Authorization
```

### CSRF Protection
- **Token-based**: Unique token per form/request
- **SameSite Cookies**: Prevent cross-site cookie sending
- **Check**: Verify Referer or Origin header

### Rate Limiting
- **Purpose**: Prevent abuse, brute force attacks
- **Implementation**: Limit requests per IP/user/endpoint
- **Response**: 429 Too Many Requests status

## Infrastructure Security

### Network Security
- **Firewall**: Control inbound/outbound traffic
- **VPC**: Virtual private cloud isolation
- **Security Groups**: Whitelist allowed ports/IPs
- **Network Segmentation**: Isolate sensitive systems

### Server Security
- **Patching**: Keep OS and software updated
- **Minimal Services**: Run only needed services
- **Strong Passwords**: Complex, non-default credentials
- **SSH Keys**: Use key-based auth, no password login

### Database Security
- **Access Control**: Restrict user permissions
- **Encryption**: Encrypt sensitive fields at rest
- **Backups**: Encrypt and secure backups
- **Audit Logging**: Track access and changes

## Monitoring and Incident Response

### Security Monitoring
- **Log Analysis**: Look for suspicious patterns
- **Intrusion Detection**: Identify attacks in progress
- **Vulnerability Scans**: Regular security audits
- **Penetration Testing**: Simulate attacks

### Incident Response
- **Detection**: Identify security breach
- **Containment**: Stop ongoing attack, isolate systems
- **Eradication**: Remove attacker access
- **Recovery**: Restore systems
- **Post-mortem**: Analyze and improve

### Metrics
- **Mean Time to Detect (MTTD)**: How quickly breach found
- **Mean Time to Respond (MTTR)**: How quickly responded
- **Mean Time to Recover (MTTR)**: How quickly restored

## Security Checklist

- [ ] HTTPS/TLS enforced everywhere
- [ ] Passwords hashed with strong algorithm
- [ ] Input validation on all inputs
- [ ] Authentication required for protected resources
- [ ] Authorization checks for operations
- [ ] Sensitive data encrypted at rest
- [ ] Sensitive data not in logs
- [ ] CORS properly configured
- [ ] CSRF protection implemented
- [ ] Rate limiting on APIs
- [ ] Security headers configured (CSP, X-Frame-Options, etc.)
- [ ] Secrets not in version control
- [ ] Regular security updates applied
- [ ] Monitoring and alerting active
- [ ] Incident response plan documented

## Common Security Mistakes to Avoid

1. **Storing plaintext passwords**: Always hash
2. **Disabling certificate validation**: Never skip TLS verification
3. **Hardcoded secrets**: Use environment variables/secrets manager
4. **No input validation**: Always validate and sanitize
5. **Overly broad permissions**: Follow least privilege
6. **No encryption for sensitive data**: Encrypt in transit and at rest
7. **Ignoring security headers**: Implement CSP, X-Frame-Options, etc.
8. **No logging of security events**: Log and monitor
9. **Weak algorithms**: Use current best practices (TLS 1.2+, bcrypt)
10. **Single point of failure**: Implement redundancy and monitoring

## Practice Files
- **01-Explanation**: Authentication methods, encryption, attack defense
- **02-Architecture-Diagrams**: Auth flows, security architecture
- **03-Code-Examples**: JWT implementation, password hashing, SQL injection prevention
