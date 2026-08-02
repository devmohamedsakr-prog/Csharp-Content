# ADO.NET - Direct Database Access

## Overview
ADO.NET fundamentals, SqlConnection, SqlCommand, DataReader, and direct database access patterns.

## SqlConnection and SqlCommand

### Basic Connection and Query
```csharp
public class AdoNetBasics
{
    private readonly string _connectionString;
    
    public AdoNetBasics(string connectionString) => _connectionString = connectionString;
    
    public async Task<List<User>> GetUsersAsync()
    {
        var users = new List<User>();
        
        // Create connection
        using (var connection = new SqlConnection(_connectionString))
        {
            // Create command
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Id, Name, Email FROM Users";
                command.CommandType = CommandType.Text;
                
                // Open connection and execute
                await connection.OpenAsync();
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    // Read results
                    while (await reader.ReadAsync())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2)
                        });
                    }
                }
            }
        } // Connection automatically closed
        
        return users;
    }
}
```

### Parameterized Queries
```csharp
public class ParameterizedQueries
{
    private readonly string _connectionString;
    
    public async Task<User> GetUserByEmailAsync(string email)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        
        // Use parameters to prevent SQL injection
        command.CommandText = @"
            SELECT Id, Name, Email, CreatedAt 
            FROM Users 
            WHERE Email = @Email";
        
        command.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value);
        
        await connection.OpenAsync();
        
        using (var reader = await command.ExecuteReaderAsync())
        {
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Email = reader.GetString("Email"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                };
            }
        }
        
        return null;
    }
    
    public async Task UpdateUserAsync(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        
        command.CommandText = @"
            UPDATE Users 
            SET Name = @Name, Email = @Email 
            WHERE Id = @Id";
        
        command.Parameters.AddWithValue("@Name", user.Name ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Id", user.Id);
        
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}
```

## DataReader vs DataTable

### SqlDataReader (Recommended)
```csharp
public class DataReaderExample
{
    private readonly string _connectionString;
    
    public async Task<List<User>> GetUsersWithReaderAsync()
    {
        var users = new List<User>();
        
        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        
        command.CommandText = "SELECT Id, Name, Email FROM Users";
        
        await connection.OpenAsync();
        
        // Forward-only, read-only access - memory efficient
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Email = reader.GetString("Email")
                });
            }
        }
        
        return users;
    }
}
```

### DataTable (When Needed)
```csharp
public class DataTableExample
{
    private readonly string _connectionString;
    
    public DataTable GetUsersAsDataTable()
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("SELECT * FROM Users", connection);
        
        var dataTable = new DataTable();
        
        using (var adapter = new SqlDataAdapter(command))
        {
            // Fills DataTable with all results
            adapter.Fill(dataTable);
        }
        
        return dataTable;
    }
    
    public void DisplayDataTable(DataTable table)
    {
        // Can access by column name or index
        foreach (DataRow row in table.Rows)
        {
            var userId = row["Id"];
            var userName = row["Name"];
        }
    }
}
```

## Stored Procedures

### Executing Stored Procedures
```csharp
public class StoredProcedureExample
{
    private readonly string _connectionString;
    
    public async Task<List<Order>> GetUserOrdersAsync(int userId)
    {
        var orders = new List<Order>();
        
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_GetUserOrders", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        
        // Add parameters
        command.Parameters.AddWithValue("@UserId", userId);
        
        await connection.OpenAsync();
        
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                orders.Add(new Order
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    Amount = reader.GetDecimal("Amount"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                });
            }
        }
        
        return orders;
    }
    
    public async Task<(List<User>, int)> GetPaginatedUsersAsync(int pageNumber, int pageSize)
    {
        var users = new List<User>();
        int totalCount = 0;
        
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_GetPaginatedUsers", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        
        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@PageSize", pageSize);
        
        // Output parameter
        var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(totalCountParam);
        
        await connection.OpenAsync();
        
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                users.Add(MapToUser(reader));
            }
        }
        
        totalCount = (int)totalCountParam.Value;
        
        return (users, totalCount);
    }
    
    private User MapToUser(SqlDataReader reader)
    {
        return new User
        {
            Id = reader.GetInt32("Id"),
            Name = reader.GetString("Name"),
            Email = reader.GetString("Email")
        };
    }
}
```

## Bulk Operations

### Bulk Insert
```csharp
public class BulkOperations
{
    private readonly string _connectionString;
    
    public async Task BulkInsertUsersAsync(List<User> users)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var transaction = connection.BeginTransaction();
        
        try
        {
            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = "Users";
                bulkCopy.BatchSize = 1000; // Insert 1000 at a time
                bulkCopy.BulkCopyTimeout = 300; // 5 minutes
                
                // Map DataTable columns
                var dataTable = new DataTable();
                dataTable.Columns.Add("Id", typeof(int));
                dataTable.Columns.Add("Name", typeof(string));
                dataTable.Columns.Add("Email", typeof(string));
                
                foreach (var user in users)
                {
                    dataTable.Rows.Add(user.Id, user.Name, user.Email);
                }
                
                await bulkCopy.WriteToServerAsync(dataTable);
            }
            
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
}
```

## Multi-Result Sets

### Reading Multiple Result Sets
```csharp
public class MultiResultSetExample
{
    private readonly string _connectionString;
    
    public async Task<(List<User>, List<Role>)> GetUsersAndRolesAsync()
    {
        var users = new List<User>();
        var roles = new List<Role>();
        
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(@"
            SELECT Id, Name, Email FROM Users;
            SELECT Id, Name FROM Roles", connection);
        
        await connection.OpenAsync();
        
        using (var reader = await command.ExecuteReaderAsync())
        {
            // First result set - Users
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Email = reader.GetString("Email")
                });
            }
            
            // Move to next result set
            if (await reader.NextResultAsync())
            {
                // Second result set - Roles
                while (await reader.ReadAsync())
                {
                    roles.Add(new Role
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name")
                    });
                }
            }
        }
        
        return (users, roles);
    }
}
```

## Best Practices

1. **Always Use Parameterized Queries**
```csharp
// Good: Protected from SQL injection
command.CommandText = "SELECT * FROM Users WHERE Email = @Email";
command.Parameters.AddWithValue("@Email", userEmail);

// Bad: Vulnerable to SQL injection
command.CommandText = $"SELECT * FROM Users WHERE Email = '{userEmail}'";
```

2. **Use SqlDataReader for Large Result Sets**
```csharp
// Good: Memory efficient for large datasets
using (var reader = await command.ExecuteReaderAsync())
{
    while (await reader.ReadAsync())
    {
        // Process one row at a time
    }
}

// Bad: Loads entire result into memory
var dataTable = new DataTable();
adapter.Fill(dataTable);
```

3. **Handle NULL Values Properly**
```csharp
// Good: Check for NULL
if (!reader.IsDBNull(columnIndex))
{
    var value = reader.GetString(columnIndex);
}

// Or use parameters with DBNull
command.Parameters.AddWithValue("@Value", value ?? (object)DBNull.Value);

// Bad: No NULL handling
var value = reader.GetString(columnIndex); // Throws exception if NULL
```

## Common Mistakes

1. **SQL Injection**
```csharp
// Bad: String concatenation
var email = userInput;
command.CommandText = $"SELECT * FROM Users WHERE Email = '{email}'";

// Good: Parameterized query
command.CommandText = "SELECT * FROM Users WHERE Email = @Email";
command.Parameters.AddWithValue("@Email", email);
```

2. **Not Disposing Resources**
```csharp
// Bad: Resource leak
var command = new SqlCommand("SELECT * FROM Users", connection);
var reader = command.ExecuteReader();

// Good: Proper disposal
using var command = new SqlCommand("SELECT * FROM Users", connection);
using (var reader = await command.ExecuteReaderAsync())
{
    // Process results
}
```

3. **Incorrect Data Type Mapping**
```csharp
// Bad: Wrong type
var userId = reader.GetString(0); // Column is int!

// Good: Correct type
var userId = reader.GetInt32(0);
// Or use GetOrdinal if names change
var userId = reader.GetInt32(reader.GetOrdinal("Id"));
```

## Quick Summary
- Use SqlDataReader for forward-only, memory-efficient access
- Always use parameterized queries to prevent SQL injection
- Properly dispose connections, commands, and readers
- Use stored procedures for complex operations
- Handle NULL values explicitly
- Use SqlBulkCopy for large inserts
- Read multiple result sets with NextResultAsync()
- Map data types correctly
- Use transactions for multi-step operations
- Monitor connection pool usage

## Resources
- ADO.NET Overview
- SqlConnection and SqlCommand
- SqlDataReader Best Practices
- Stored Procedures in .NET
- SQL Injection Prevention
