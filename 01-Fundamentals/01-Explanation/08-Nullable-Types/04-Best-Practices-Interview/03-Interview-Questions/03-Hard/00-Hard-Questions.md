# Nullable Types - Hard Questions

## Q1: Design a nullable-aware cache system
```csharp
public class NullableCache<TKey, TValue> where TValue : struct {
    private Dictionary<TKey, TValue?> cache = new();
    
    public TValue? Get(TKey key) {
        cache.TryGetValue(key, out var value);
        return value;
    }
    
    public void Set(TKey key, TValue? value) {
        cache[key] = value;
    }
}
```

## Q2: Handle three-valued logic (true/false/null)
```csharp
bool? result = GetBool();

string meaning = result switch {
    true => "Yes",
    false => "No",
    null => "Unknown"
};
```

## Q3: Implement optional method chaining safely
```csharp
public async Task<string?> GetUserCityAsync(int id) {
    var user = await GetUserAsync(id);
    var address = await GetAddressAsync(user?.Id);
    return address?.City;
}
```

## Q4: Design configuration with nullable overrides
```csharp
public class Config {
    public int? MaxConnectionsOverride { get; set; }
    
    public int GetMaxConnections(int defaultValue) {
        return MaxConnectionsOverride ?? defaultValue;
    }
}
```

## Q5: Thread-safe null handling
```csharp
public class ThreadSafeValue<T> where T : struct {
    private T? value;
    private readonly object lockObj = new();
    
    public T? GetValue() {
        lock (lockObj) {
            return value;
        }
    }
    
    public void SetValue(T? newValue) {
        lock (lockObj) {
            value = newValue;
        }
    }
}
```
