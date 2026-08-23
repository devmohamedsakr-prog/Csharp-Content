# Examples and Projects

Practical implementations of OOP principles.

## 🔧 Quick Examples

### Example 1: Bank Account System
Demonstrates encapsulation and data protection.

```csharp
public class BankAccount
{
    private decimal _balance;
    public string AccountNumber { get; private set; }
    
    public BankAccount(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        _balance = initialBalance;
    }
    
    public decimal GetBalance()
    {
        return _balance;
    }
    
    public bool Deposit(decimal amount)
    {
        if (amount <= 0)
            return false;
        
        _balance += amount;
        return true;
    }
    
    public bool Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > _balance)
            return false;
        
        _balance -= amount;
        return true;
    }
}

// Usage
var account = new BankAccount("ACC001", 1000);
account.Deposit(500);      // ✅ Works
account.Withdraw(200);     // ✅ Works
var balance = account.GetBalance();  // 1300
```

### Example 2: Shape Hierarchy
Demonstrates inheritance and polymorphism.

```csharp
public abstract class Shape
{
    public string Name { get; set; }
    
    public abstract double GetArea();
    public abstract double GetPerimeter();
    
    public void DisplayInfo()
    {
        Console.WriteLine($"{Name}: Area = {GetArea()}, Perimeter = {GetPerimeter()}");
    }
}

public class Circle : Shape
{
    public double Radius { get; set; }
    
    public override double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
    
    public override double GetPerimeter()
    {
        return 2 * Math.PI * Radius;
    }
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    
    public override double GetArea()
    {
        return Width * Height;
    }
    
    public override double GetPerimeter()
    {
        return 2 * (Width + Height);
    }
}

// Usage
List<Shape> shapes = new()
{
    new Circle { Name = "Circle", Radius = 5 },
    new Rectangle { Name = "Rectangle", Width = 4, Height = 6 }
};

foreach (var shape in shapes)
{
    shape.DisplayInfo();
}
```

### Example 3: E-Commerce Order System
Demonstrates abstraction and interfaces.

```csharp
public interface IOrderProcessor
{
    void ProcessOrder(Order order);
}

public class Order
{
    public string OrderId { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal Total { get; set; }
}

public class OrderItem
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class PaymentProcessor : IOrderProcessor
{
    public void ProcessOrder(Order order)
    {
        Console.WriteLine($"Processing payment for order {order.OrderId}: ${order.Total}");
    }
}

public class InventoryProcessor : IOrderProcessor
{
    public void ProcessOrder(Order order)
    {
        Console.WriteLine($"Updating inventory for order {order.OrderId}");
    }
}

public class ShippingProcessor : IOrderProcessor
{
    public void ProcessOrder(Order order)
    {
        Console.WriteLine($"Shipping order {order.OrderId}");
    }
}

// Usage
var processors = new List<IOrderProcessor>
{
    new PaymentProcessor(),
    new InventoryProcessor(),
    new ShippingProcessor()
};

var order = new Order 
{ 
    OrderId = "ORD001", 
    Total = 99.99m,
    Items = new List<OrderItem>()
};

foreach (var processor in processors)
{
    processor.ProcessOrder(order);
}
```

---

## 🎯 Mini Projects

### Project 1: Library Management System
**Concepts:** Classes, inheritance, collections

**Structure:**
```
- Book class
- Member class
- Library class (manages books and members)
- Loan tracking
```

### Project 2: Game Character System
**Concepts:** Polymorphism, interfaces, abstraction

**Structure:**
```
- Character base class
- Player : Character
- Enemy : Character
- Item system
- Combat system
```

### Project 3: Restaurant Management
**Concepts:** Encapsulation, composition, abstraction

**Structure:**
```
- Menu with dishes
- Order management
- Table tracking
- Payment processing
```

---

## 📂 Files in This Section

- `01-Simple-Examples.md` - Getting started examples
- `02-Bank-System.md` - Complete bank account system
- `03-Game-Example.md` - Game character system
- `04-E-Commerce.md` - Order processing system
- `05-Projects.md` - Complete projects with explanations

---

## 💡 Learning Approach

1. **Start Simple** - Understand basic class structure
2. **Build Examples** - Follow the provided examples
3. **Extend Projects** - Modify projects with new features
4. **Create Your Own** - Implement personal projects using OOP

---

## 🚀 Next Steps

After completing examples:
1. Review SOLID principles
2. Study design patterns
3. Work on larger applications
4. Practice refactoring code to use OOP

