# Namespace Scope in C#

## Overview

Namespace scope determines where types (classes, structs, interfaces, enums) can be accessed. Namespaces organize code into logical groups and control type visibility and naming conflicts.

## Understanding Namespaces

### Basic Namespace Declaration

```csharp
namespace MyCompany.MyApplication
{
    public class Customer
    {
        public string Name { get; set; }
    }
}

namespace MyCompany.MyApplication.Data
{
    public class Repository
    {
        public Customer GetCustomer(int id)
        {
            return new Customer { Name = "John" };
        }
    }
}
```

### Accessing Types from Namespaces

```csharp
// Using full qualified name
MyCompany.MyApplication.Customer customer1 = new MyCompany.MyApplication.Customer();

// Using directive to import namespace
using MyCompany.MyApplication;

Customer customer2 = new Customer(); // Now accessible without prefix
```

## Namespace Organization

### Hierarchical Namespace Structure

```csharp
namespace CompanyName
{
    // Top-level namespace
}

namespace CompanyName.ProductName
{
    // Child namespace - good for organizing products
}

namespace CompanyName.ProductName.Features
{
    // Grandchild namespace - specific features
}

namespace CompanyName.ProductName.Features.Authentication
{
    // Great-grandchild namespace - specific components
    
    public class LoginService
    {
        public bool ValidateCredentials(string username, string password)
        {
            return true;
        }
    }
}

// Access from outside
CompanyName.ProductName.Features.Authentication.LoginService service = new();
```

### Logical Organization Patterns

```csharp
// Common pattern: Organization by feature/layer

namespace MyApp.Features.Users
{
    public class User { }
    public class UserService { }
    public class UserRepository { }
}

namespace MyApp.Features.Products
{
    public class Product { }
    public class ProductService { }
    public class ProductRepository { }
}

namespace MyApp.Infrastructure.Data
{
    public class DatabaseConnection { }
}

namespace MyApp.Infrastructure.Logging
{
    public class Logger { }
}

namespace MyApp.Core.Utilities
{
    public class StringHelper { }
}
```

## Using Directives

### Basic Using Statements

```csharp
using System; // Import System namespace
using System.Collections.Generic; // Import Collections
using MyApp.Features.Users; // Import custom namespace

public class Program
{
    public static void Main()
    {
        var user = new User(); // User accessible due to using directive
        var list = new List<string>(); // List<> accessible
    }
}
```

### Namespace Aliasing

```csharp
using System.Collections.Generic;
using CollectionsAlias = System.Collections;
using UserNamespace = MyApp.Features.Users;

public class Program
{
    public static void Main()
    {
        var list = new List<string>(); // Via System.Collections.Generic using
        var hashtable = new CollectionsAlias.Hashtable(); // Via alias
        var user = new UserNamespace.User(); // Via alias
    }
}
```

### Static Using (C# 6.0+)

```csharp
using static System.Console; // Import static members of Console class
using static System.Math; // Import static members of Math class

public class Program
{
    public static void Main()
    {
        WriteLine("Hello"); // Console. prefix not needed
        double result = Sqrt(16); // Math. prefix not needed
    }
}
```

### Global Using Directives (C# 10.0+)

```csharp
// In a single file (often GlobalUsings.cs)
global using System;
global using System.Collections.Generic;
global using MyApp.Features;

// These using statements are available in ALL files in the project
// No need to repeat them in each file
```

## Type Visibility and Namespace Scope

### Public Types in Namespaces

```csharp
namespace MyApp.Public
{
    // Public types are accessible from other namespaces
    public class PublicClass
    {
        public void DoWork() { }
    }
}

namespace MyApp.Consumer
{
    using MyApp.Public; // Import the namespace
    
    public class MyClass
    {
        public void DoSomething()
        {
            var obj = new PublicClass(); // Accessible
            obj.DoWork();
        }
    }
}
```

### Internal Types

```csharp
namespace MyApp.Internal
{
    // Internal types are only accessible within the same assembly
    internal class InternalClass
    {
        public void DoWork() { }
    }
}

// From outside the assembly:
// using MyApp.Internal; - won't help
// var obj = new MyApp.Internal.InternalClass(); - COMPILE ERROR
```

## Namespace Conflicts and Resolution

### Name Conflicts

```csharp
using System.Collections.Generic;
using MyApp.Collections; // Also has List<>

namespace MyApp
{
    public class Consumer
    {
        public void Demo()
        {
            // Ambiguous - which List?
            // var list = new List<int>(); // COMPILE ERROR
            
            // Solution 1: Use fully qualified name
            var list1 = new System.Collections.Generic.List<int>();
            var list2 = new MyApp.Collections.List<int>();
            
            // Solution 2: Use namespace alias
            // (see using directives section)
        }
    }
}
```

### Name Shadowing in Namespaces

```csharp
namespace MyApp
{
    public class Customer { }
}

namespace MyApp.Data
{
    // This Customer shadows the one in MyApp
    public class Customer { }
    
    public class Repository
    {
        public void Demo()
        {
            var customer1 = new Customer(); // Uses MyApp.Data.Customer
            var customer2 = new MyApp.Customer(); // Access outer with fully qualified
        }
    }
}
```

## File-Scoped Namespaces (C# 10.0+)

```csharp
// Old style
namespace MyApp.Features.Users
{
    public class User { }
    
    public class UserService { }
}

// New style (C# 10.0+) - cleaner for single-namespace files
namespace MyApp.Features.Users;

public class User { }

public class UserService { }

// No braces or indentation needed - entire file is in this namespace
```

## Nested Namespaces

### Explicit Nesting

```csharp
namespace MyApp
{
    public class AppConfiguration { }
    
    namespace Features
    {
        public class FeatureRegistry { }
        
        namespace Users
        {
            public class User { }
        }
    }
}

// Access:
var config = new MyApp.AppConfiguration();
var feature = new MyApp.Features.FeatureRegistry();
var user = new MyApp.Features.Users.User();
```

### Dot Notation (Preferred)

```csharp
namespace MyApp.Features.Users
{
    public class User { }
}

// Same result, cleaner and more common
```

## Namespace Scope Rules

### Rule 1: Types Are Scoped to Their Namespace

```csharp
namespace MyApp.A
{
    public class TypeA { }
}

namespace MyApp.B
{
    public class TypeA { } // Different type, same name
}

// Not a conflict - they're in different namespaces
var objA = new MyApp.A.TypeA();
var objB = new MyApp.B.TypeA();
```

### Rule 2: Using Statements Import Namespace Members

```csharp
namespace Provider
{
    public class Helper { }
}

namespace Consumer
{
    using Provider; // Imports Provider namespace
    
    public class MyClass
    {
        public void Demo()
        {
            var helper = new Helper(); // OK - Helper found via using
        }
    }
}

namespace OtherConsumer
{
    // No using Provider;
    public class MyClass
    {
        public void Demo()
        {
            // var helper = new Helper(); // ERROR - Helper not found
            var helper = new Provider.Helper(); // OK - fully qualified
        }
    }
}
```

### Rule 3: Nested Namespaces Must Be Fully Qualified

```csharp
namespace MyApp.Features.Users
{
    public class User { }
}

namespace MyApp.Features
{
    // Cannot access User directly
    // var user = new User(); // ERROR
    
    // Must use fully qualified or access via Users namespace
    var user = new MyApp.Features.Users.User(); // OK
}
```

### Rule 4: Global Namespace

```csharp
// Types without explicit namespace are in global namespace
public class GlobalClass { }

namespace MyApp
{
    public class AppClass
    {
        public void Demo()
        {
            // Cannot access GlobalClass without qualification
            // var global = new GlobalClass(); // ERROR
            
            var global = new global::GlobalClass(); // OK - explicit global
        }
    }
}
```

## Practical Namespace Organization

### Example Project Structure

```csharp
// MyApp/Program.cs - Entry point in global namespace or MyApp
global using System;
global using System.Collections.Generic;

namespace MyApp;

class Program
{
    static void Main()
    {
        // Application entry point
    }
}

// MyApp/Features/Users/User.cs
namespace MyApp.Features.Users;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// MyApp/Features/Users/UserService.cs
namespace MyApp.Features.Users;

public class UserService
{
    public User GetUser(int id) => new() { Id = id };
}

// MyApp/Infrastructure/Data/DatabaseConnection.cs
namespace MyApp.Infrastructure.Data;

public class DatabaseConnection
{
    public void Connect() { }
}

// MyApp/Core/Utilities/StringHelper.cs
namespace MyApp.Core.Utilities;

public static class StringHelper
{
    public static string Truncate(string text, int length)
    {
        return text.Length > length ? text.Substring(0, length) : text;
    }
}
```

## Best Practices

1. **Use Meaningful Namespace Names**: Follow reverse domain convention (Company.Product.Feature)
2. **Organize by Feature/Layer**: Group related functionality together
3. **Avoid Deep Nesting**: More than 3 levels often indicates poor organization
4. **Keep Namespaces Parallel to Folder Structure**: MyApp.Features.Users matches MyApp/Features/Users/ folder
5. **Use Root Namespace**: C# projects support root namespace property in .csproj
6. **Limit Using Directives**: Import only what's needed
7. **Use Aliases for Conflicts**: Instead of fully qualifying repeatedly
8. **Consider Global Usings**: For common namespaces in modern C#

## Common Issues

1. **Namespace Conflicts**: Two types with same name in different using namespaces
2. **Deep Nesting**: Over-complicated namespace hierarchies
3. **Unclear Organization**: Namespaces that don't reflect code organization
4. **Incomplete Using Statements**: Forgetting to import needed namespaces
5. **Global Namespace Pollution**: Too many types without namespace
6. **Inconsistent Naming**: Namespace names don't match folder structure

## Summary

Namespace scope provides organizational structure for C# code, preventing naming conflicts and improving maintainability. Proper namespace organization follows project structure, uses meaningful names, and limits nesting depth. Using directives make code more readable by importing namespaces, while fully qualified names resolve conflicts. Modern C# features like file-scoped namespaces and global usings simplify namespace management in larger projects.
