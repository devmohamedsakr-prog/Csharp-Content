# Expression Trees

## Overview
Expression trees represent code as data structures, enabling dynamic code generation, compilation, and analysis at runtime.

## Basic Concepts

### What Are Expression Trees?
```csharp
// Traditional lambda
Func<int, int> lambda = x => x * 2;

// Expression tree - represents the same logic as data
Expression<Func<int, int>> tree = x => x * 2;

// Both work, but expression tree is analyzable
var result1 = lambda(5); // Direct execution: 10
var compiled = tree.Compile();
var result2 = compiled(5); // 10

// But we can also inspect/modify the tree
var body = tree.Body; // The multiplication operation
var parameter = tree.Parameters[0]; // The 'x' parameter
```

## Building Expression Trees Manually

### Creating Nodes
```csharp
// Build: x => x * 2
var parameter = Expression.Parameter(typeof(int), "x");
var constant = Expression.Constant(2);
var multiply = Expression.Multiply(parameter, constant);
var lambda = Expression.Lambda<Func<int, int>>(multiply, parameter);

// Compile and use
var compiled = lambda.Compile();
Console.WriteLine(compiled(5)); // 10

// Inspect
Console.WriteLine(lambda); // x => (x * 2)
```

### Complex Expressions
```csharp
// Build: (x, y) => x + y > 10 ? x : y
var x = Expression.Parameter(typeof(int), "x");
var y = Expression.Parameter(typeof(int), "y");

var add = Expression.Add(x, y);
var constant10 = Expression.Constant(10);
var condition = Expression.GreaterThan(add, constant10);
var conditional = Expression.Condition(condition, x, y);

var lambda = Expression.Lambda<Func<int, int, int>>(conditional, x, y);
var compiled = lambda.Compile();

Console.WriteLine(compiled(6, 7)); // 6 (6+7 > 10)
Console.WriteLine(compiled(2, 3)); // 3 (2+3 < 10)
```

## LINQ Expression Trees

### Query Expression Trees
```csharp
// Expression tree implicitly created
Expression<Func<Person, bool>> filter = p => p.Age > 25;

// In LINQ to Entities
var adults = dbContext.Users.Where(p => p.Age > 25);
// The Where gets: Expression<Func<User, bool>>
// DbContext translates to SQL

// Without expression tree (LINQ to Objects)
IEnumerable<Person> people = new List<Person> { /* ... */ };
var result = people.Where(p => p.Age > 25); // Func<Person, bool>
```

### Custom Query Provider
```csharp
public class CustomQueryable<T> : IQueryable<T>
{
    private IQueryProvider _provider;
    private Expression _expression;
    
    public CustomQueryable(IQueryProvider provider, Expression expression)
    {
        _provider = provider;
        _expression = expression;
    }
    
    public Type ElementType => typeof(T);
    public Expression Expression => _expression;
    public IQueryProvider Provider => _provider;
    
    public IEnumerator<T> GetEnumerator() => _provider.Execute<IEnumerable<T>>(_expression).GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

## Analyzing Expression Trees

### Expression Visitors
```csharp
// Custom visitor to analyze expression tree
public class ExpressionAnalyzer : ExpressionVisitor
{
    private List<string> _operations = new();
    
    public List<string> Analyze(Expression expression)
    {
        Visit(expression);
        return _operations;
    }
    
    public override Expression VisitBinary(BinaryExpression node)
    {
        _operations.Add($"{node.NodeType}: {node.Left} {node.Right}");
        return base.VisitBinary(node);
    }
    
    public override Expression VisitMethodCall(MethodCallExpression node)
    {
        _operations.Add($"Method: {node.Method.Name}");
        return base.VisitMethodCall(node);
    }
}

// Usage
Expression<Func<int, int, int>> expr = (x, y) => x + y > 10 ? x : y;
var analyzer = new ExpressionAnalyzer();
var operations = analyzer.Analyze(expr.Body);
foreach (var op in operations)
{
    Console.WriteLine(op);
}
```

### Modifying Expression Trees
```csharp
// Replace all constants with doubled values
public class ConstantDoubler : ExpressionVisitor
{
    public override Expression VisitConstant(ConstantExpression node)
    {
        if (node.Type == typeof(int))
        {
            return Expression.Constant((int)node.Value * 2);
        }
        return base.VisitConstant(node);
    }
}

// Original: x => x + 5
var original = (Expression<Func<int, int>>)(x => x + 5);

// Modified: x => x + 10
var doubler = new ConstantDoubler();
var modified = doubler.Visit(original) as Expression<Func<int, int>>;

var originalCompiled = original.Compile();
var modifiedCompiled = modified.Compile();

Console.WriteLine(originalCompiled(3)); // 8
Console.WriteLine(modifiedCompiled(3)); // 13
```

## Dynamic Query Building

### Building Queries at Runtime
```csharp
public class QueryBuilder<T> where T : class
{
    public Expression<Func<T, bool>> BuildFilter(string propertyName, string op, object value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);
        
        Expression comparison = op switch
        {
            "==" => Expression.Equal(property, constant),
            ">" => Expression.GreaterThan(property, constant),
            "<" => Expression.LessThan(property, constant),
            ">=" => Expression.GreaterThanOrEqual(property, constant),
            "<=" => Expression.LessThanOrEqual(property, constant),
            _ => throw new ArgumentException($"Unknown operator: {op}")
        };
        
        return Expression.Lambda<Func<T, bool>>(comparison, parameter);
    }
}

// Usage
var builder = new QueryBuilder<User>();
var filter = builder.BuildFilter("Age", ">", 25);
var compiled = filter.Compile();

var user = new User { Age = 30 };
Console.WriteLine(compiled(user)); // true
```

## Real-World Examples

### Mapping Expression
```csharp
public class AutoMapper
{
    public static Expression<Func<TSource, TDest>> CreateMapperExpression<TSource, TDest>()
        where TSource : class
        where TDest : class, new()
    {
        var sourceParam = Expression.Parameter(typeof(TSource), "source");
        var destVar = Expression.Variable(typeof(TDest), "dest");
        
        var assignments = new List<Expression>();
        
        // For each property, create assignment
        foreach (var sourceProp in typeof(TSource).GetProperties())
        {
            var destProp = typeof(TDest).GetProperty(sourceProp.Name);
            if (destProp != null && destProp.CanWrite)
            {
                var sourceAccess = Expression.Property(sourceParam, sourceProp);
                var destAccess = Expression.Property(destVar, destProp);
                assignments.Add(Expression.Assign(destAccess, sourceAccess));
            }
        }
        
        assignments.Add(destVar);
        
        var block = Expression.Block(new[] { destVar }, assignments);
        var lambda = Expression.Lambda<Func<TSource, TDest>>(block, sourceParam);
        
        return lambda;
    }
}

// Usage
var mapper = AutoMapper.CreateMapperExpression<User, UserDto>();
var compiled = mapper.Compile();
var userDto = compiled(user);
```

### Validation Expression
```csharp
public class ValidationRuleBuilder<T>
{
    public Expression<Func<T, bool>> BuildValidation(string propertyName, Func<object, bool> rule)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        
        var ruleMethod = Expression.Constant(rule).Type
            .GetMethod("Invoke", new[] { typeof(object) });
        
        var call = Expression.Call(
            Expression.Constant(rule),
            ruleMethod,
            Expression.Convert(property, typeof(object))
        );
        
        return Expression.Lambda<Func<T, bool>>(call, parameter);
    }
}
```

## Performance Considerations

### Compilation Overhead
```csharp
// Expression trees are slower initially
var sw = Stopwatch.StartNew();

var expr = (Expression<Func<int, int>>)(x => x * 2);
var compiled = expr.Compile(); // Compilation happens here

sw.Stop();
Console.WriteLine($"Compilation: {sw.ElapsedMilliseconds}ms");

// But execution is fast if called many times
sw.Restart();
for (int i = 0; i < 1_000_000; i++)
{
    compiled(i);
}
sw.Stop();
Console.WriteLine($"1M executions: {sw.ElapsedMilliseconds}ms");

// Direct lambda is comparable after compilation
```

## Best Practices

1. **Use Expressions for Dynamic Scenarios**
```csharp
// Good: Dynamic filtering at runtime
public IEnumerable<T> Filter<T>(IEnumerable<T> items, 
    Expression<Func<T, bool>> filter)
{
    return items.Where(filter.Compile());
}

// Not necessary: Static known filters
public IEnumerable<User> GetActiveUsers(IEnumerable<User> users)
{
    return users.Where(u => u.IsActive);
}
```

2. **Cache Compiled Expressions**
```csharp
// Bad: Recompiling each time
for (int i = 0; i < 1000; i++)
{
    var expr = (Expression<Func<int, int>>)(x => x * 2);
    var compiled = expr.Compile(); // Recompiling!
}

// Good: Compile once, use many times
var expr = (Expression<Func<int, int>>)(x => x * 2);
var compiled = expr.Compile();
for (int i = 0; i < 1000; i++)
{
    compiled(i);
}
```

3. **Use ExpressionVisitor for Complex Modifications**
```csharp
// Good: Systematic tree traversal
public class RenameParameterVisitor : ExpressionVisitor
{
    private readonly string _oldName;
    private readonly string _newName;
    
    public RenameParameterVisitor(string oldName, string newName)
    {
        _oldName = oldName;
        _newName = newName;
    }
    
    public override Expression VisitParameter(ParameterExpression node)
    {
        if (node.Name == _oldName)
            return Expression.Parameter(node.Type, _newName);
        return base.VisitParameter(node);
    }
}
```

## Common Mistakes

1. **Not Compiling Before Execution**
```csharp
// Bad: Directly using expression
var expr = (Expression<Func<int, int>>)(x => x * 2);
// var result = expr(5); // ERROR: Can't invoke expression directly

// Good: Compile first
var compiled = expr.Compile();
var result = compiled(5);
```

2. **Performance Without Understanding Overhead**
```csharp
// Bad: Heavy compilation for simple operation
var expr = (Expression<Func<int, int>>)(x => x * 2);
var compiled = expr.Compile();
var result = compiled(5); // Overkill, just use x => x * 2

// Good: Use expressions when needed (dynamic, translatable)
var filter = BuildDynamicFilter(userInput);
var results = dbContext.Users.Where(filter);
```

## Quick Summary
- Expression trees represent code as data
- Can be compiled to delegates for execution
- Can be analyzed and modified with visitors
- Used in LINQ to Entities, dynamic queries
- Compilation has overhead, but execution is fast
- Cache compiled expressions
- Use visitors for systematic tree traversal
- Power feature for frameworks and dynamic code
- Complex but powerful for advanced scenarios

## Resources
- Expression Trees (C# documentation)
- ExpressionVisitor Pattern
- System.Linq.Expressions
- Building Query Providers
