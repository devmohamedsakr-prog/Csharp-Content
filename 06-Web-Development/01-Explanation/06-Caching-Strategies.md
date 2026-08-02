# Caching Strategies

## Overview
In-memory caching, distributed caching, cache invalidation, and performance optimization patterns.

## In-Memory Caching

### IMemoryCache
```csharp
public class UserService
{
    private readonly IMemoryCache _cache;
    private readonly IUserRepository _repository;
    private const string USER_CACHE_KEY = "user_{0}";
    
    public UserService(IMemoryCache cache, IUserRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }
    
    public async Task<User> GetUserAsync(int userId)
    {
        var cacheKey = string.Format(USER_CACHE_KEY, userId);
        
        if (!_cache.TryGetValue(cacheKey, out User user))
        {
            user = await _repository.GetAsync(userId);
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));
            
            _cache.Set(cacheKey, user, cacheOptions);
        }
        
        return user;
    }
}

// Program.cs
builder.Services.AddMemoryCache();
```

### Cache Invalidation
```csharp
public class UserService
{
    private readonly IMemoryCache _cache;
    
    public async Task UpdateUserAsync(User user)
    {
        await _repository.UpdateAsync(user);
        
        // Remove from cache
        _cache.Remove($"user_{user.Id}");
    }
    
    public async Task DeleteUserAsync(int userId)
    {
        await _repository.DeleteAsync(userId);
        
        // Remove from cache
        _cache.Remove($"user_{userId}");
        
        // Invalidate related caches
        _cache.Remove("users_list");
    }
}
```

## Distributed Caching with Redis

### Configuration
```csharp
// appsettings.json
{
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}

// Program.cs
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
```

### Usage
```csharp
public class CachedUserService
{
    private readonly IDistributedCache _cache;
    private readonly IUserRepository _repository;
    
    public CachedUserService(IDistributedCache cache, IUserRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }
    
    public async Task<User> GetUserAsync(int userId)
    {
        var cacheKey = $"user_{userId}";
        var cachedUser = await _cache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedUser))
            return JsonSerializer.Deserialize<User>(cachedUser);
        
        var user = await _repository.GetAsync(userId);
        
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        
        await _cache.SetStringAsync(cacheKey, 
            JsonSerializer.Serialize(user), options);
        
        return user;
    }
}
```

## Cache-Aside Pattern
```csharp
public class CacheAsidePattern
{
    private readonly IDistributedCache _cache;
    private readonly IDataService _dataService;
    
    public async Task<T> GetAsync<T>(string key, 
        Func<Task<T>> dataFactory, 
        TimeSpan? expiration = null)
    {
        // Try cache first
        var cached = await _cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
            return JsonSerializer.Deserialize<T>(cached);
        
        // Cache miss - get from data source
        var data = await dataFactory();
        
        // Store in cache
        var options = new DistributedCacheEntryOptions();
        if (expiration.HasValue)
            options.SetAbsoluteExpiration(expiration.Value);
        
        await _cache.SetStringAsync(key, 
            JsonSerializer.Serialize(data), options);
        
        return data;
    }
}

// Usage
var user = await _cacheAside.GetAsync("user_5", 
    async () => await _repository.GetAsync(5),
    TimeSpan.FromMinutes(5));
```

## Write-Through Cache
```csharp
public class WriteThroughCache
{
    private readonly IDistributedCache _cache;
    private readonly IRepository _repository;
    
    public async Task<T> SaveAsync<T>(string key, T data)
    {
        // Write to database first
        var result = await _repository.SaveAsync(data);
        
        // Then update cache
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        
        await _cache.SetStringAsync(key, 
            JsonSerializer.Serialize(result), options);
        
        return result;
    }
}
```


## Write-Behind (Write-Back) Cache
```csharp
public class WriteBehindCache
{
    private readonly IDistributedCache _cache;
    private readonly IRepository _repository;
    private readonly IBackgroundJobClient _jobClient;
    
    public async Task UpdateAsync<T>(string key, T data)
    {
        // Update cache immediately
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        
        await _cache.SetStringAsync(key, 
            JsonSerializer.Serialize(data), options);
        
        // Queue database update for later
        _jobClient.Enqueue(() => _repository.SaveAsync(data));
    }
}
```

## Cache Warming
```csharp
public class CacheWarmupService
{
    private readonly IDistributedCache _cache;
    private readonly IRepository _repository;
    
    public async Task WarmupAsync()
    {
        var categories = await _repository.GetAllCategoriesAsync();
        
        foreach (var category in categories)
        {
            var cacheKey = $"category_{category.Id}";
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            
            await _cache.SetStringAsync(cacheKey, 
                JsonSerializer.Serialize(category), options);
        }
    }
}

// Program.cs
var app = builder.Build();

// Warmup cache on startup
using (var scope = app.Services.CreateScope())
{
    var warmupService = scope.ServiceProvider
        .GetRequiredService<CacheWarmupService>();
    await warmupService.WarmupAsync();
}

app.Run();
```

## Caching Decorators
```csharp
public interface IUserRepository
{
    Task<User> GetAsync(int id);
}

public class UserRepository : IUserRepository
{
    private readonly DbContext _context;
    
    public async Task<User> GetAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}

public class CachedUserRepository : IUserRepository
{
    private readonly IUserRepository _inner;
    private readonly IDistributedCache _cache;
    
    public CachedUserRepository(
        IUserRepository inner, 
        IDistributedCache cache)
    {
        _inner = inner;
        _cache = cache;
    }
    
    public async Task<User> GetAsync(int id)
    {
        var key = $"user_{id}";
        var cached = await _cache.GetStringAsync(key);
        
        if (!string.IsNullOrEmpty(cached))
            return JsonSerializer.Deserialize<User>(cached);
        
        var user = await _inner.GetAsync(id);
        
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        
        await _cache.SetStringAsync(key, 
            JsonSerializer.Serialize(user), options);
        
        return user;
    }
}

// Program.cs
builder.Services.AddScoped<UserRepository>();
builder.Services.Decorate<IUserRepository, CachedUserRepository>();
```

## Best Practices

1. **Choose Appropriate Expiration**
```csharp
// Good: Different TTLs for different data
var userOptions = new DistributedCacheEntryOptions()
    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30)); // User data

var categoryOptions = new DistributedCacheEntryOptions()
    .SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Less frequent changes

// Bad: Same TTL for everything
var options = new DistributedCacheEntryOptions()
    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
```

2. **Use Proper Cache Keys**
```csharp
// Good: Hierarchical, descriptive keys
var keys = new[]
{
    "user:5",
    "user:5:posts",
    "user:5:settings",
    "category:electronics"
};

// Bad: Generic keys that collide
var keys = new[] { "data", "cache", "item" };
```

3. **Implement Cache Invalidation**
```csharp
public class ProductService
{
    public async Task UpdateProductAsync(Product product)
    {
        await _repository.UpdateAsync(product);
        
        // Clear related caches
        await _cache.RemoveAsync($"product:{product.Id}");
        await _cache.RemoveAsync("products:all");
        await _cache.RemoveAsync($"category:{product.CategoryId}");
    }
}
```

## Common Mistakes

1. **Cache Stampede**
```csharp
// Bad: Multiple requests hit database on cache expiry
if (!_cache.Exists(key))
    return await _repository.GetAsync(id); // Everyone hits DB

// Good: Lock while loading
var value = await _cache.GetAsync(key);
if (value == null)
{
    using (var lockObj = new ReaderWriterLockSlim())
    {
        lockObj.EnterWriteLock();
        value = await _repository.GetAsync(id);
        await _cache.SetAsync(key, value);
        lockObj.ExitWriteLock();
    }
}
```

2. **Storing Large Objects**
```csharp
// Bad: Cache entire large object
await _cache.SetStringAsync(key, JsonSerializer.Serialize(largeObject));

// Good: Cache only needed properties
var cached = new { largeObject.Id, largeObject.Name };
await _cache.SetStringAsync(key, JsonSerializer.Serialize(cached));
```

3. **Inconsistent Cache Invalidation**
```csharp
// Bad: Forget to invalidate in some update paths
public async Task UpdateAsync(Product product)
{
    await _repository.UpdateAsync(product);
    // Forgot to remove cache!
}

// Good: Always invalidate
public async Task UpdateAsync(Product product)
{
    await _repository.UpdateAsync(product);
    await _cache.RemoveAsync($"product:{product.Id}");
}
```

## Quick Summary
- In-Memory: Fast but single-process, limited size
- Distributed: Shared across instances, scalable
- Cache-Aside: Read-through pattern, app controls loading
- Write-Through: Consistent, slower writes
- Write-Behind: Fast writes, eventual consistency
- Cache Warming: Preload frequently accessed data
- TTL prevents stale data
- Proper invalidation is critical
- Cache keys should be hierarchical
- Monitor cache hit/miss rates
- Don't cache large objects
- Use decorators for transparent caching

## Resources
- Redis Caching Guide
- Cache-Aside Pattern
- Distributed Caching in ASP.NET Core
- Redis Best Practices
