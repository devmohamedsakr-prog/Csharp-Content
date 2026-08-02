# Performance Testing and Load Testing

## Overview
Performance benchmarking, load testing, profiling, and optimization verification.

## BenchmarkDotNet

### Basic Setup
```csharp
// Install NuGet: BenchmarkDotNet
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, targetCount: 5)]
public class StringConcatenationBenchmark
{
    private const int N = 1000;
    private string[] _strings;
    
    [GlobalSetup]
    public void Setup()
    {
        _strings = Enumerable.Range(0, N)
            .Select(i => $"String {i}")
            .ToArray();
    }
    
    [Benchmark]
    public string StringConcatenation()
    {
        string result = "";
        for (int i = 0; i < N; i++)
        {
            result += _strings[i];
        }
        return result;
    }
    
    [Benchmark]
    public string StringBuilderApproach()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < N; i++)
        {
            sb.Append(_strings[i]);
        }
        return sb.ToString();
    }
    
    [Benchmark]
    public string StringJoin()
    {
        return string.Join("", _strings);
    }
}

// Program.cs
static void Main(string[] args)
{
    var summary = BenchmarkRunner.Run<StringConcatenationBenchmark>();
}
```

### Collection Benchmark
```csharp
[MemoryDiagnoser]
public class CollectionBenchmark
{
    private const int N = 10000;
    
    [Benchmark]
    public int ListLookup()
    {
        var list = Enumerable.Range(0, N).ToList();
        int sum = 0;
        
        for (int i = 0; i < N; i++)
        {
            sum += list[i];
        }
        
        return sum;
    }
    
    [Benchmark]
    public int DictionaryLookup()
    {
        var dict = Enumerable.Range(0, N)
            .ToDictionary(x => x, x => x);
        int sum = 0;
        
        for (int i = 0; i < N; i++)
        {
            if (dict.TryGetValue(i, out var value))
                sum += value;
        }
        
        return sum;
    }
    
    [Benchmark]
    public int HashSetContains()
    {
        var set = new HashSet<int>(Enumerable.Range(0, N));
        int count = 0;
        
        for (int i = 0; i < N; i++)
        {
            if (set.Contains(i))
                count++;
        }
        
        return count;
    }
}
```

## Load Testing with NBomber

### Installation and Setup
```
// NuGet: NBomber
using NBomber.CSharp;

var scenario = Scenario.Create("get_user", async context =>
{
    var request = new HttpRequestMessage(
        HttpMethod.Get, 
        "http://localhost:5000/api/users/1"
    );
    
    var response = await client.SendAsync(request);
    
    return response.IsSuccessStatusCode 
        ? Response.Ok() 
        : Response.Fail();
})
    .WithoutWarmup()
    .WithLoadSimulations(
        Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), duration: TimeSpan.FromSeconds(30))
    );

NBomberRunner
    .RegisterScenarios(scenario)
    .Run();
```

### Complex Load Test
```csharp
public class LoadTestScenarios
{
    private readonly HttpClient _client;
    private static int _userId = 1;
    
    public LoadTestScenarios()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
    }
    
    public void RunLoadTests()
    {
        var getScenario = Scenario.Create("get_user", async context =>
        {
            var response = await _client.GetAsync($"/api/users/{_userId}");
            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        })
            .WithLoadSimulations(
                Simulation.Inject(rate: 50, interval: TimeSpan.FromSeconds(1), 
                    duration: TimeSpan.FromSeconds(60))
            );
        
        var createScenario = Scenario.Create("create_user", async context =>
        {
            var user = new { Name = $"User{_userId++}", Email = $"user{_userId}@example.com" };
            var content = JsonContent.Create(user);
            
            var response = await _client.PostAsync("/api/users", content);
            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        })
            .WithLoadSimulations(
                Simulation.Inject(rate: 10, interval: TimeSpan.FromSeconds(1), 
                    duration: TimeSpan.FromSeconds(60))
            );
        
        NBomberRunner
            .RegisterScenarios(getScenario, createScenario)
            .Run();
    }
}
```

## Profiling

### Memory Profiling
```csharp
public class MemoryProfiler
{
    public static void ProfileMemoryUsage()
    {
        var sw = Stopwatch.StartNew();
        
        // Measure before
        var beforeMem = GC.GetTotalMemory(true);
        
        // Code to profile
        var list = new List<User>();
        for (int i = 0; i < 1000000; i++)
        {
            list.Add(new User { Id = i, Name = $"User {i}" });
        }
        
        // Measure after
        var afterMem = GC.GetTotalMemory(true);
        
        sw.Stop();
        
        Console.WriteLine($"Memory used: {(afterMem - beforeMem) / 1024 / 1024}MB");
        Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");
    }
}

public class AllocationCounter
{
    public static void CountAllocations()
    {
        var before = GC.GetTotalAllocatedBytes();
        
        // Operation
        var result = ProblematicMethod();
        
        var after = GC.GetTotalAllocatedBytes();
        
        Console.WriteLine($"Allocated: {(after - before) / 1024}KB");
    }
    
    private static string ProblematicMethod()
    {
        string result = "";
        for (int i = 0; i < 1000; i++)
        {
            result += i.ToString(); // String allocation each iteration!
        }
        return result;
    }
}
```

### CPU Profiling
```csharp
public class CpuProfiler
{
    public static void ProfileCpuUsage()
    {
        var sw = Stopwatch.StartNew();
        
        // Code to profile
        var sum = 0;
        for (int i = 0; i < 100000000; i++)
        {
            sum += i;
        }
        
        sw.Stop();
        
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms");
        Console.WriteLine($"Sum: {sum}");
    }
}
```

## Stress Testing

### Connection Pool Stress Test
```csharp
[Fact]
public async Task ConnectionPool_UnderLoad_RemainsStable()
{
    const int concurrentRequests = 100;
    const int requestsPerThread = 1000;
    
    var tasks = new List<Task>();
    
    for (int i = 0; i < concurrentRequests; i++)
    {
        tasks.Add(Task.Run(async () =>
        {
            for (int j = 0; j < requestsPerThread; j++)
            {
                var user = await _userRepository.GetByIdAsync(1);
                Assert.NotNull(user);
            }
        }));
    }
    
    await Task.WhenAll(tasks);
    
    // No deadlocks or connection pool exhaustion
}
```

### Cache Stress Test
```csharp
[Fact]
public async Task Cache_UnderConcurrentLoad_Remains Consistent()
{
    const int concurrentTasks = 50;
    const int operationsPerTask = 100;
    var errors = new ConcurrentBag<Exception>();
    
    var tasks = Enumerable.Range(0, concurrentTasks)
        .Select(async _ =>
        {
            try
            {
                for (int i = 0; i < operationsPerTask; i++)
                {
                    var key = $"key_{i % 10}";
                    
                    if (Random.Shared.Next(2) == 0)
                    {
                        await _cache.SetAsync(key, new { Value = i });
                    }
                    else
                    {
                        var value = await _cache.GetAsync(key);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        })
        .ToArray();
    
    await Task.WhenAll(tasks);
    
    Assert.Empty(errors);
}
```

## Query Performance Testing

### EF Core Profiling
```csharp
public class QueryPerformanceTests
{
    private readonly AppDbContext _context;
    
    public QueryPerformanceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .Build();
        
        _context = new AppDbContext(options);
    }
    
    [Fact]
    public async Task GetUserWithPosts_Lazy_VsEager()
    {
        // Lazy Loading - N+1 problem
        var sw = Stopwatch.StartNew();
        var users = await _context.Users.ToListAsync();
        
        foreach (var user in users)
        {
            var postCount = user.Posts.Count; // Additional query for each user
        }
        
        sw.Stop();
        Console.WriteLine($"Lazy loading: {sw.ElapsedMilliseconds}ms");
        
        // Eager Loading - Single query
        sw.Restart();
        var usersEager = await _context.Users
            .Include(u => u.Posts)
            .ToListAsync();
        
        foreach (var user in usersEager)
        {
            var postCount = user.Posts.Count; // Already loaded
        }
        
        sw.Stop();
        Console.WriteLine($"Eager loading: {sw.ElapsedMilliseconds}ms");
    }
}
```

## Best Practices

1. **Benchmark Before Optimizing**
```csharp
// Good: Measure first
[Benchmark]
public void OriginalMethod() { }

[Benchmark]
public void OptimizedMethod() { }

// Bad: Optimize without data
void OptimizeWithoutMeasuring() { }
```

2. **Test Realistic Scenarios**
```csharp
// Good: Realistic load distribution
Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), 
    duration: TimeSpan.FromSeconds(300))

// Bad: Artificial small test
Simulation.Inject(rate: 1, interval: TimeSpan.FromSeconds(10), 
    duration: TimeSpan.FromSeconds(10))
```

3. **Monitor Baselines**
```csharp
// Good: Track performance over time
public class PerformanceBaseline
{
    [Fact]
    public void GetUsers_CompletesFast()
    {
        var sw = Stopwatch.StartNew();
        var users = _service.GetUsers();
        sw.Stop();
        
        Assert.True(sw.ElapsedMilliseconds < 100, "Should complete in <100ms");
    }
}
```

## Common Mistakes

1. **Not Warming Up**
```csharp
// Bad: No warmup
var sw = Stopwatch.StartNew();
for (int i = 0; i < 1000; i++)
{
    DoWork();
}
sw.Stop();

// Good: Warm up first
for (int i = 0; i < 100; i++) DoWork(); // Warmup
var sw = Stopwatch.StartNew();
for (int i = 0; i < 1000; i++)
{
    DoWork();
}
sw.Stop();
```

2. **Measuring in Debug Mode**
```csharp
// Bad: Debug has optimizations disabled
#if DEBUG
    MeasurePerformance(); // Wrong results!
#endif

// Good: Measure in Release
#if RELEASE
    MeasurePerformance();
#endif
```

3. **Ignoring GC Pressure**
```csharp
// Bad: No GC consideration
var result = ExpensiveOperation();

// Good: Force GC before measuring
GC.Collect();
GC.WaitForPendingFinalizers();
var result = ExpensiveOperation();
```

## Quick Summary
- Use BenchmarkDotNet for accurate microbenchmarks
- Test with realistic load profiles
- Monitor memory allocations
- Measure before optimizing
- Warm up before benchmarking
- Run tests in Release mode
- Use nbomber for load testing
- Profile CPU and memory separately
- Stress test for stability
- Track performance baselines over time

## Resources
- BenchmarkDotNet Documentation
- NBomber Load Testing
- Performance Profiling in .NET
- Query Performance Analyzer
