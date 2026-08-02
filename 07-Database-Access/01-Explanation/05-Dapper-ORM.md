# Dapper ORM - Lightweight Data Access

## Overview
Dapper is a lightweight ORM (Object-Relational Mapper) for .NET that provides fast, simple data access with minimal overhead and maximum performance.

## Installation and Setup

### NuGet Package
```bash
dotnet add package Dapper
dotnet add package Dapper.Contrib  # For additional extensions
```

### Basic Configuration
```csharp
// Program.cs
builder.Services.AddScoped<IDataAccess, DapperDataAccess>();

// IDataAccess interface
public interface IDataAccess
{
    Task<T> GetAsync<T>(string sql, object parameters = null);
    Task<List<T>> QueryAsync<T>(string sql, object parameters = null);
    Task<int> ExecuteAsync(string sql, object parameters = null);
    Task<T> InsertAsync<T>(T entity);
    Task<int> UpdateAsync<T>(T entity);
    Task<int> DeleteAsync<T>(int id);
}

// Implementation
public class DapperDataAccess : IDataAccess
{
    private readonly string _connectionString;
    
    public DapperDataAccess(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }
    
    public async Task<T> GetAsync<T>(string sql, object parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }
    }
}
```

## Query Operations

### SELECT Queries
```csharp
public class UserRepository
{
    private readonly IDbConnection _db;
    
    public UserRepository(string connectionString)
    {
        _db = new SqlConnection(connectionString);
    }
    
    // Single row query
    public async Task<User> GetUserByIdAsync(int id)
    {
        const string sql = "SELECT Id, Name, Email, CreatedAt FROM Users WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }
    
    // Multiple rows query
    public async Task<List<User>> GetAllUsersAsync()
    {
        const string sql = "SELECT Id, Name, Email, CreatedAt FROM Users";
        var users = await _db.QueryAsync<User>(sql);
        return users.ToList();
    }
    
    // Query with multiple parameters
    public async Task<List<User>> SearchUsersAsync(string name, string email, int minAge)
    {
        const string sql = @"
            SELECT Id, Name, Email, Age, CreatedAt 
            FROM Users 
            WHERE Name LIKE @Name 
            AND Email LIKE @Email 
            AND Age >= @MinAge
            ORDER BY Name";
        
        var parameters = new 
        { 
            Name = $"%{name}%", 
            Email = $"%{email}%", 
            MinAge = minAge 
        };
        
        var users = await _db.QueryAsync<User>(sql, parameters);
        return users.ToList();
    }
    
    // Scalar query
    public async Task<int> GetUserCountAsync()
    {
        const string sql = "SELECT COUNT(*) FROM Users";
        return await _db.ExecuteScalarAsync<int>(sql);
    }
    
    // Query with LIKE
    public async Task<List<User>> SearchByNameAsync(string namePattern)
    {
        const string sql = @"
            SELECT * FROM Users 
            WHERE Name LIKE @Pattern
            ORDER BY Name";
        
        var users = await _db.QueryAsync<User>(sql, new { Pattern = $"%{namePattern}%" });
        return users.ToList();
    }
}
```

## Insert, Update, Delete

### CRUD Operations
```csharp
public class UserRepository
{
    private readonly IDbConnection _db;
    
    // INSERT
    public async Task<int> InsertUserAsync(User user)
    {
        const string sql = @"
            INSERT INTO Users (Name, Email, Age, CreatedAt) 
            VALUES (@Name, @Email, @Age, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int);";
        
        var userId = await _db.ExecuteScalarAsync<int>(sql, new
        {
            user.Name,
            user.Email,
            user.Age,
            user.CreatedAt
        });
        
        return userId;
    }
    
    // Bulk INSERT
    public async Task<int> BulkInsertAsync(List<User> users)
    {
        const string sql = @"
            INSERT INTO Users (Name, Email, Age, CreatedAt) 
            VALUES (@Name, @Email, @Age, @CreatedAt)";
        
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            using (var transaction = connection.BeginTransaction())
            {
                int rowsAffected = 0;
                
                foreach (var batch in users.Batch(1000)) // Batch of 1000
                {
                    rowsAffected += await connection.ExecuteAsync(sql, batch, transaction);
                }
                
                transaction.Commit();
                return rowsAffected;
            }
        }
    }
    
    // UPDATE
    public async Task<int> UpdateUserAsync(User user)
    {
        const string sql = @"
            UPDATE Users 
            SET Name = @Name, Email = @Email, Age = @Age 
            WHERE Id = @Id";
        
        return await _db.ExecuteAsync(sql, new
        {
            user.Id,
            user.Name,
            user.Email,
            user.Age
        });
    }
    
    // UPDATE multiple rows
    public async Task<int> UpdateUserStatusAsync(string newStatus, List<int> userIds)
    {
        const string sql = "UPDATE Users SET Status = @Status WHERE Id IN @Ids";
        return await _db.ExecuteAsync(sql, new { Status = newStatus, Ids = userIds });
    }
    
    // DELETE
    public async Task<int> DeleteUserAsync(int userId)
    {
        const string sql = "DELETE FROM Users WHERE Id = @Id";
        return await _db.ExecuteAsync(sql, new { Id = userId });
    }
    
    // Soft DELETE
    public async Task<int> SoftDeleteUserAsync(int userId)
    {
        const string sql = @"
            UPDATE Users 
            SET IsDeleted = 1, DeletedAt = GETUTCDATE() 
            WHERE Id = @Id";
        
        return await _db.ExecuteAsync(sql, new { Id = userId });
    }
}
```

## Multi-Result Sets

### Reading Multiple Result Sets
```csharp
public class ReportRepository
{
    private readonly IDbConnection _db;
    
    public async Task<(List<User>, List<Order>, int)> GetUserDashboardAsync(int userId)
    {
        const string sql = @"
            SELECT Id, Name, Email FROM Users WHERE Id = @UserId;
            SELECT OrderId, Amount, CreatedAt FROM Orders WHERE UserId = @UserId;
            SELECT COUNT(*) FROM Orders WHERE UserId = @UserId;";
        
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            using (var reader = await connection.QueryMultipleAsync(sql, new { UserId = userId }))
            {
                var users = reader.Read<User>().ToList();
                var orders = reader.Read<Order>().ToList();
                var orderCount = reader.ReadFirst<int>();
                
                return (users, orders, orderCount);
            }
        }
    }
}
```

## Stored Procedures

### Execute Stored Procedures
```csharp
public class StoredProcRepository
{
    private readonly IDbConnection _db;
    
    // Simple stored procedure
    public async Task<List<Order>> GetUserOrdersAsync(int userId)
    {
        const string sql = "sp_GetUserOrders";
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId);
        
        var orders = await _db.QueryAsync<Order>(
            sql, 
            parameters, 
            commandType: CommandType.StoredProcedure
        );
        
        return orders.ToList();
    }
    
    // Stored procedure with output parameters
    public async Task<(List<User>, int)> GetPaginatedUsersAsync(int pageNumber, int pageSize)
    {
        const string sql = "sp_GetPaginatedUsers";
        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", pageNumber);
        parameters.Add("@PageSize", pageSize);
        parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
        
        var users = await _db.QueryAsync<User>(
            sql, 
            parameters, 
            commandType: CommandType.StoredProcedure
        );
        
        int totalCount = parameters.Get<int>("@TotalCount");
        
        return (users.ToList(), totalCount);
    }
    
    // Stored procedure with INSERT/UPDATE
    public async Task<int> SaveUserAsync(User user)
    {
        const string sql = "sp_SaveUser";
        var parameters = new DynamicParameters();
        parameters.Add("@Id", user.Id);
        parameters.Add("@Name", user.Name);
        parameters.Add("@Email", user.Email);
        parameters.Add("@ReturnId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        
        await _db.ExecuteAsync(
            sql, 
            parameters, 
            commandType: CommandType.StoredProcedure
        );
        
        return parameters.Get<int>("@ReturnId");
    }
}
```

## Dapper Contrib - Extensions

### Using Contrib Attributes
```csharp
using Dapper.Contrib.Extensions;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    [Computed]
    public DateTime CreatedAt { get; set; }
    
    [Computed]
    public DateTime UpdatedAt { get; set; }
}

public class UserContribRepository
{
    private readonly IDbConnection _db;
    
    // Get all
    public async Task<List<User>> GetAllAsync()
    {
        var users = await _db.GetAllAsync<User>();
        return users.ToList();
    }
    
    // Get by ID
    public async Task<User> GetByIdAsync(int id)
    {
        return await _db.GetAsync<User>(id);
    }
    
    // Insert
    public async Task<int> InsertAsync(User user)
    {
        return await _db.InsertAsync(user);
    }
    
    // Update
    public async Task<bool> UpdateAsync(User user)
    {
        return await _db.UpdateAsync(user);
    }
    
    // Delete
    public async Task<bool> DeleteAsync(User user)
    {
        return await _db.DeleteAsync(user);
    }
    
    // Delete by ID
    public async Task<bool> DeleteAsync(int id)
    {
        var user = new User { Id = id };
        return await _db.DeleteAsync(user);
    }
}
```

## Query Performance

### SqlMapper with Custom Mapping
```csharp
public class PerformanceOptimization
{
    private readonly IDbConnection _db;
    
    // Custom type map for better performance
    public async Task<List<User>> GetUsersWithCustomMapAsync()
    {
        // Set up custom mapping
        Dapper.SqlMapper.SetTypeMap(typeof(User), new CustomPropertyTypeMap(
            typeof(User),
            (type, columnName) => type.GetProperty(
                columnName, 
                System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase
            )
        ));
        
        const string sql = "SELECT Id, Name, Email FROM Users";
        var users = await _db.QueryAsync<User>(sql);
        
        return users.ToList();
    }
    
    // Buffered queries (default)
    public async Task<List<User>> GetUsersBufferedAsync()
    {
        const string sql = "SELECT * FROM Users";
        // buffered = true (default) - entire result loaded into memory
        var users = await _db.QueryAsync<User>(sql, buffered: true);
        return users.ToList();
    }
    
    // Non-buffered queries (forward-only)
    public async Task ProcessUsersAsync(Func<User, Task> processor)
    {
        const string sql = "SELECT * FROM Users";
        // buffered = false - stream-like reading
        var users = await _db.QueryAsync<User>(sql, buffered: false);
        
        foreach (var user in users)
        {
            await processor(user);
        }
    }
}
```

## Best Practices

1. **Use Parameters to Prevent SQL Injection**
```csharp
// Good: Parameterized query
const string sql = "SELECT * FROM Users WHERE Email = @Email";
var user = await _db.QueryFirstOrDefaultAsync<User>(sql, new { Email = userEmail });

// Bad: String concatenation (vulnerable)
var sql = $"SELECT * FROM Users WHERE Email = '{userEmail}'";
var user = await _db.QueryFirstOrDefaultAsync<User>(sql);
```

2. **Use DynamicParameters for Complex Cases**
```csharp
// Good: Clear parameter management
var parameters = new DynamicParameters();
parameters.Add("@Id", id);
parameters.Add("@Status", status);
parameters.Add("@Count", dbType: DbType.Int32, direction: ParameterDirection.Output);

await _db.ExecuteAsync(sql, parameters);
int count = parameters.Get<int>("@Count");

// Bad: Inline anonymous objects for output parameters
// (can't capture output parameter values)
```

3. **Use Transactions for Multi-Step Operations**
```csharp
// Good: Explicit transaction handling
public async Task TransferMoneyAsync(int fromUserId, int toUserId, decimal amount)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                var sql1 = "UPDATE Accounts SET Balance = Balance - @Amount WHERE UserId = @UserId";
                await connection.ExecuteAsync(sql1, new { Amount = amount, UserId = fromUserId }, transaction);
                
                var sql2 = "UPDATE Accounts SET Balance = Balance + @Amount WHERE UserId = @UserId";
                await connection.ExecuteAsync(sql2, new { Amount = amount, UserId = toUserId }, transaction);
                
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

// Bad: No transaction control
await _db.ExecuteAsync(sql1, param1);
await _db.ExecuteAsync(sql2, param2); // Could fail, leaving inconsistent state
```

4. **Handle NULL Values Properly**
```csharp
// Good: Explicit NULL handling
var parameters = new DynamicParameters();
parameters.Add("@MiddleName", user.MiddleName ?? (object)DBNull.Value);

// Or using extension
parameters.AddNullableParameter("@MiddleName", user.MiddleName);

// Bad: Passing null without DBNull.Value
var result = await _db.QueryAsync<User>(sql, new { Name = null }); // Might not work as expected
```

5. **Dispose Connections Properly**
```csharp
// Good: Using using statement
using (var connection = new SqlConnection(_connectionString))
{
    await connection.OpenAsync();
    var users = await connection.QueryAsync<User>(sql);
}

// Bad: Not disposing
var connection = new SqlConnection(_connectionString);
await connection.OpenAsync();
var users = await connection.QueryAsync<User>(sql);
// Connection left open!
```

## Common Mistakes

1. **Forgetting CommandType.StoredProcedure**
```csharp
// Bad: Treats stored procedure name as SQL query
const string sql = "sp_GetUsers";
var users = await _db.QueryAsync<User>(sql);

// Good: Specify CommandType
var users = await _db.QueryAsync<User>(
    "sp_GetUsers", 
    commandType: CommandType.StoredProcedure
);
```

2. **Not Parameterizing IN Clauses**
```csharp
// Bad: String concatenation for list
var ids = new[] { 1, 2, 3 };
var sql = $"SELECT * FROM Users WHERE Id IN ({string.Join(",", ids)})";
var users = await _db.QueryAsync<User>(sql);

// Good: Use parameter array
var sql = "SELECT * FROM Users WHERE Id IN @Ids";
var users = await _db.QueryAsync<User>(sql, new { Ids = ids });
```

3. **Type Mismatch in Results**
```csharp
// Bad: Wrong type expectation
var result = await _db.QueryFirstOrDefaultAsync<string>(
    "SELECT 1"
); // Throws - result is int, not string

// Good: Correct type
var result = await _db.QueryFirstOrDefaultAsync<int>(
    "SELECT 1"
);
```

4. **Buffering Large Result Sets**
```csharp
// Bad: Large data loaded into memory
var largeset = await _db.QueryAsync<LargeData>(sql, buffered: true);
var count = largeset.Count();

// Good: Stream for large datasets
var count = 0;
var data = await _db.QueryAsync<LargeData>(sql, buffered: false);
foreach (var item in data)
{
    count++;
    ProcessItem(item);
}
```

5. **Not Using Using for Transactions**
```csharp
// Bad: Potential resource leak
var transaction = connection.BeginTransaction();
// code
transaction.Commit();

// Good: Proper resource management
using (var transaction = connection.BeginTransaction())
{
    // code
    transaction.Commit();
}
```

## Quick Summary
- Dapper: Lightweight, fast, minimal overhead
- SqlMapper handles all query execution
- Parameterized queries prevent SQL injection
- DynamicParameters for complex scenarios
- Support for stored procedures
- Contrib extensions for basic CRUD
- Transactions for data consistency
- Multiple result sets with QueryMultiple
- Custom type mapping available
- No lazy loading by default
- Must dispose connections properly
- Excellent performance for high-throughput apps

## Resources
- Dapper GitHub Repository
- Dapper Documentation
- Dapper Contrib
- Dapper vs EF Core Performance
- SQL Injection Prevention
