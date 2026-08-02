# Transactions and ACID Principles

## Overview
Database transactions, ACID properties, isolation levels, and distributed transactions.

## ACID Principles

### Atomicity
```csharp
public class TransferService
{
    private readonly AppDbContext _context;
    
    // All operations succeed or all fail
    public async Task TransferMoneyAsync(int fromAccountId, int toAccountId, decimal amount)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var fromAccount = await _context.Accounts.FindAsync(fromAccountId);
            var toAccount = await _context.Accounts.FindAsync(toAccountId);
            
            if (fromAccount.Balance < amount)
                throw new InvalidOperationException("Insufficient funds");
            
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
```

### Consistency
```csharp
public class OrderService
{
    private readonly AppDbContext _context;
    
    // Database remains in valid state
    public async Task CreateOrderAsync(Order order)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            _context.Orders.Add(order);
            
            foreach (var item in order.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                
                // Consistency check: ensure valid state
                if (product.Stock < item.Quantity)
                    throw new InvalidOperationException("Insufficient stock");
                
                product.Stock -= item.Quantity;
            }
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
```

### Isolation
```csharp
// Different isolation levels available
public async Task DemonstrateIsolationAsync()
{
    var strategy = _context.Database.CreateExecutionStrategy();
    
    await strategy.ExecuteAsync(async () =>
    {
        using var transaction = await _context.Database
            .BeginTransactionAsync(IsolationLevel.ReadCommitted);
        
        // ReadUncommitted: Dirty reads possible
        // ReadCommitted: No dirty reads (SQL Server default)
        // RepeatableRead: No lost updates
        // Serializable: Fully isolated
        
        try
        {
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    });
}
```

### Durability
```csharp
public class DurableTransactionExample
{
    private readonly AppDbContext _context;
    
    // Committed data survives system failures
    public async Task PersistDataAsync(User user)
    {
        _context.Users.Add(user);
        
        // SaveChangesAsync sends to database
        // Database confirms write to persistent storage
        await _context.SaveChangesAsync();
        
        // Even if app crashes after this, data is safe
    }
}
```

## Isolation Levels

### ReadUncommitted
```csharp
// Dirty reads allowed - fastest, least safe
using var transaction = await _context.Database
    .BeginTransactionAsync(IsolationLevel.ReadUncommitted);

// Can read uncommitted changes from other transactions
var users = await _context.Users.ToListAsync();
```

### ReadCommitted (Default)
```csharp
// Only committed data visible
using var transaction = await _context.Database
    .BeginTransactionAsync(IsolationLevel.ReadCommitted);

var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == 1);
// May see different value on second read if updated by other transaction
```

### RepeatableRead
```csharp
// Same data on repeated reads
using var transaction = await _context.Database
    .BeginTransactionAsync(IsolationLevel.RepeatableRead);

var balance1 = await _context.Accounts
    .Where(a => a.Id == 1)
    .Select(a => a.Balance)
    .FirstOrDefaultAsync();

var balance2 = await _context.Accounts
    .Where(a => a.Id == 1)
    .Select(a => a.Balance)
    .FirstOrDefaultAsync();

// balance1 == balance2 guaranteed
```

### Serializable
```csharp
// Fully isolated - slowest, most safe
using var transaction = await _context.Database
    .BeginTransactionAsync(IsolationLevel.Serializable);

var users = await _context.Users.Where(u => u.Active).ToListAsync();
// No phantom reads, repeatable reads, dirty reads
```

## Savepoints

```csharp
public async Task SavepointExampleAsync()
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    
    try
    {
        var user = new User { Name = "John" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        // Create savepoint
        await transaction.CreateSavepointAsync("BeforeUpdate");
        
        // More operations
        user.Email = "john@example.com";
        await _context.SaveChangesAsync();
        
        // If error occurs, rollback to savepoint
        if (SomeErrorCondition)
        {
            await transaction.RollbackToSavepointAsync("BeforeUpdate");
        }
        
        await transaction.CommitAsync();
    }
    catch (Exception)
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

## Distributed Transactions

```csharp
public class DistributedTransactionExample
{
    private readonly AppDbContext _context1;
    private readonly AppDbContext _context2;
    
    public async Task TransferAcrossDatabasesAsync()
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions 
            { 
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(30)
            }
        );
        
        try
        {
            // Update Database 1
            var account1 = await _context1.Accounts.FindAsync(1);
            account1.Balance -= 100;
            await _context1.SaveChangesAsync();
            
            // Update Database 2
            var account2 = await _context2.Accounts.FindAsync(2);
            account2.Balance += 100;
            await _context2.SaveChangesAsync();
            
            transaction.Complete();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Distributed transaction failed");
            throw;
        }
    }
}
```

## Retry Logic

```csharp
public class RetryableTransactionService
{
    private readonly AppDbContext _context;
    private const int MaxRetries = 3;
    
    public async Task<T> ExecuteWithRetryAsync<T>(
        Func<Task<T>> operation)
    {
        int retryCount = 0;
        
        while (retryCount < MaxRetries)
        {
            using var transaction = await _context.Database
                .BeginTransactionAsync();
            
            try
            {
                var result = await operation();
                await transaction.CommitAsync();
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync();
                retryCount++;
                
                if (retryCount >= MaxRetries)
                    throw;
                
                // Exponential backoff
                await Task.Delay(TimeSpan.FromMilliseconds(100 * retryCount));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        
        throw new InvalidOperationException("Max retries exceeded");
    }
}

// Usage
await _retryService.ExecuteWithRetryAsync(async () =>
{
    var user = await _context.Users.FindAsync(1);
    user.Name = "Updated";
    await _context.SaveChangesAsync();
    return user;
});
```

## Best Practices

1. **Keep Transactions Short**
```csharp
// Good: Minimal work in transaction
public async Task UpdateUserAsync(int userId, string name)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    
    try
    {
        var user = await _context.Users.FindAsync(userId);
        user.Name = name;
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
    catch (Exception)
    {
        await transaction.RollbackAsync();
        throw;
    }
}

// Bad: Long-running transaction
public async Task UpdateUserAsync(int userId, string name)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    
    // Long-running operation here
    await Task.Delay(10000);
    
    var user = await _context.Users.FindAsync(userId);
    user.Name = name;
    await _context.SaveChangesAsync();
    await transaction.CommitAsync();
}
```

2. **Use Appropriate Isolation Level**
```csharp
// Good: Balance safety and performance
using var transaction = await _context.Database
    .BeginTransactionAsync(IsolationLevel.ReadCommitted);

// Bad: Serializable for simple reads
using var transaction = await _context.Database
    .BeginTransactionAsync(IsolationLevel.Serializable);
```

3. **Handle Deadlocks**
```csharp
public async Task HandleDeadlocksAsync()
{
    const int maxRetries = 3;
    int retries = 0;
    
    while (retries < maxRetries)
    {
        try
        {
            using var transaction = await _context.Database
                .BeginTransactionAsync();
            
            // Do work
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return;
        }
        catch (SqlException ex) when (ex.Number == 1205) // Deadlock
        {
            await transaction.RollbackAsync();
            retries++;
            await Task.Delay(100 * retries);
        }
    }
}
```

## Common Mistakes

1. **Forgetting to Rollback**
```csharp
// Bad: Transaction left open
public async Task CreateUserAsync(User user)
{
    var transaction = await _context.Database.BeginTransactionAsync();
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    // Forgot to commit or rollback!
}

// Good: Always use using
public async Task CreateUserAsync(User user)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
    catch (Exception)
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

2. **Nested Transactions Without Savepoints**
```csharp
// Bad: No savepoint handling
using var transaction1 = await _context.Database.BeginTransactionAsync();
using var transaction2 = await _context.Database.BeginTransactionAsync();

// Good: Use savepoints or named transactions
using var transaction = await _context.Database.BeginTransactionAsync();
await transaction.CreateSavepointAsync("Point1");
```

3. **Blocking Other Transactions**
```csharp
// Bad: Long operation in transaction
using var transaction = await _context.Database.BeginTransactionAsync();

await LongRunningOperationAsync(); // Blocks others!

var user = await _context.Users.FindAsync(1);
await transaction.CommitAsync();

// Good: Minimize transaction scope
await LongRunningOperationAsync();

using var transaction = await _context.Database.BeginTransactionAsync();
var user = await _context.Users.FindAsync(1);
await transaction.CommitAsync();
```

## Quick Summary
- Atomicity: All or nothing
- Consistency: Valid state maintained
- Isolation: Transactions don't interfere
- Durability: Committed data persists
- Isolation levels: ReadUncommitted → Serializable
- Savepoints for partial rollbacks
- Keep transactions short
- Handle deadlocks with retry logic
- Use appropriate isolation level
- Always rollback on errors
- Consider distributed transactions carefully

## Resources
- Entity Framework Core Transactions
- Database Isolation Levels
- ACID Properties
- Deadlock Detection and Handling
