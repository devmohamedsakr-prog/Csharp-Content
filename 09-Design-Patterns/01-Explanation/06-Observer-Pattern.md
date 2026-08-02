# Observer Pattern

## Overview
Observer Pattern defines a one-to-many dependency where when one object changes state, all dependents are notified automatically.

## IObserver and IObservable

### Built-in Interfaces
```csharp
public class StockPrice : IObservable<decimal>
{
    private string _symbol;
    private decimal _price;
    private List<IObserver<decimal>> _observers = new();
    
    public StockPrice(string symbol, decimal initialPrice)
    {
        _symbol = symbol;
        _price = initialPrice;
    }
    
    public IDisposable Subscribe(IObserver<decimal> observer)
    {
        _observers.Add(observer);
        
        // Return disposable to unsubscribe
        return new Unsubscriber(_observers, observer);
    }
    
    public void UpdatePrice(decimal newPrice)
    {
        _price = newPrice;
        
        // Notify all observers
        foreach (var observer in _observers)
        {
            observer.OnNext(newPrice);
        }
    }
    
    private class Unsubscriber : IDisposable
    {
        private List<IObserver<decimal>> _observers;
        private IObserver<decimal> _observer;
        
        public Unsubscriber(List<IObserver<decimal>> observers, IObserver<decimal> observer)
        {
            _observers = observers;
            _observer = observer;
        }
        
        public void Dispose()
        {
            _observers.Remove(_observer);
        }
    }
}

public class StockTracker : IObserver<decimal>
{
    private string _name;
    private IDisposable _unsubscriber;
    
    public StockTracker(string name)
    {
        _name = name;
    }
    
    public virtual void Subscribe(IObservable<decimal> observable)
    {
        _unsubscriber = observable.Subscribe(this);
    }
    
    public void OnNext(decimal value)
    {
        Console.WriteLine($"{_name} notified: Price = {value}");
    }
    
    public void OnError(Exception error)
    {
        Console.WriteLine($"{_name} error: {error.Message}");
    }
    
    public void OnCompleted()
    {
        Console.WriteLine($"{_name} completed");
    }
    
    public void Unsubscribe()
    {
        _unsubscriber?.Dispose();
    }
}

// Usage
var stock = new StockPrice("AAPL", 150);
var tracker1 = new StockTracker("Tracker1");
var tracker2 = new StockTracker("Tracker2");

tracker1.Subscribe(stock);
tracker2.Subscribe(stock);

stock.UpdatePrice(151); // Both trackers notified
// Output:
// Tracker1 notified: Price = 151
// Tracker2 notified: Price = 151

tracker1.Unsubscribe();
stock.UpdatePrice(152); // Only Tracker2 notified
```

## Event-Based Observer

### Using Events
```csharp
public class Button
{
    // Define event
    public event EventHandler Clicked;
    
    public void Click()
    {
        // Raise event
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}

public class UI
{
    private Button _submitButton;
    
    public UI()
    {
        _submitButton = new Button();
        // Subscribe to event
        _submitButton.Clicked += OnSubmitClicked;
    }
    
    private void OnSubmitClicked(object sender, EventArgs e)
    {
        Console.WriteLine("Submit clicked!");
    }
}

// Usage
var ui = new UI();
ui._submitButton.Click(); // Output: Submit clicked!
```

### Generic Event Handler
```csharp
public class PropertyChanged<T>
{
    public T OldValue { get; set; }
    public T NewValue { get; set; }
}

public class ObservableProperty<T>
{
    private T _value;
    
    public event EventHandler<PropertyChanged<T>> ValueChanged;
    
    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                var oldValue = _value;
                _value = value;
                
                ValueChanged?.Invoke(this, new PropertyChanged<T>
                {
                    OldValue = oldValue,
                    NewValue = value
                });
            }
        }
    }
}

// Usage
var property = new ObservableProperty<int> { Value = 5 };
property.ValueChanged += (sender, args) =>
{
    Console.WriteLine($"Changed from {args.OldValue} to {args.NewValue}");
};

property.Value = 10; // Output: Changed from 5 to 10
```

## Property Change Notifications

### INotifyPropertyChanged
```csharp
public class User : INotifyPropertyChanged
{
    private string _name;
    private int _age;
    
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
    
    public int Age
    {
        get => _age;
        set
        {
            if (_age != value)
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// Usage in UI binding
var user = new User();
user.PropertyChanged += (s, e) =>
{
    Console.WriteLine($"Property changed: {e.PropertyName}");
};

user.Name = "Alice"; // Output: Property changed: Name
user.Age = 30;       // Output: Property changed: Age
```

## Best Practices

1. **Use Events for Simple Notifications**
```csharp
// Good: Simple, familiar pattern
public event EventHandler DataChanged;

protected void OnDataChanged()
{
    DataChanged?.Invoke(this, EventArgs.Empty);
}

// Listen
instance.DataChanged += (s, e) => ProcessChange();
```

2. **Unsubscribe When Done**
```csharp
// Good: Clean up subscriptions
public class Consumer
{
    private Producer _producer;
    
    public void Subscribe(Producer producer)
    {
        _producer = producer;
        _producer.EventRaised += OnEventRaised;
    }
    
    public void Unsubscribe()
    {
        if (_producer != null)
        {
            _producer.EventRaised -= OnEventRaised;
        }
    }
}
```

3. **Use WeakEventManager for Memory Leaks**
```csharp
// Good: Prevents memory leaks in WPF
WeakEventManager<Observable, EventArgs>.AddHandler(
    source,
    nameof(Observable.Changed),
    OnChanged
);

private void OnChanged(object sender, EventArgs e)
{
    // Handle change
}
```

## Common Mistakes

1. **Not Unsubscribing**
```csharp
// Bad: Memory leak
public class View
{
    public View(ViewModel vm)
    {
        vm.PropertyChanged += HandlePropertyChanged; // Never unsubscribed
    }
    
    private void HandlePropertyChanged(object s, PropertyChangedEventArgs e)
    {
    }
}

// Good: Unsubscribe
public class GoodView : IDisposable
{
    private ViewModel _vm;
    
    public GoodView(ViewModel vm)
    {
        _vm = vm;
        _vm.PropertyChanged += HandlePropertyChanged;
    }
    
    public void Dispose()
    {
        _vm.PropertyChanged -= HandlePropertyChanged;
    }
}
```

2. **Throwing Exceptions in Handlers**
```csharp
// Bad: One exception stops other handlers
PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
// If first handler throws, second never called

// Good: Handle exceptions
foreach (PropertyChangedEventHandler handler in PropertyChanged?.GetInvocationList() ?? Array.Empty<Delegate>())
{
    try
    {
        handler(this, new PropertyChangedEventArgs(name));
    }
    catch (Exception ex)
    {
        Logger.LogError(ex);
    }
}
```

3. **Not Using WeakReferences in Long-Lived Objects**
```csharp
// Bad: Strong reference keeps object alive
public event EventHandler Changed;

// Good: WeakEventManager (WPF)
WeakEventManager<MyClass, EventArgs>.AddHandler(
    source, nameof(Changed), Handler);
```

## Quick Summary
- Observer: Notify multiple subscribers of changes
- IObserver/IObservable for push pattern
- Events for notification-based patterns
- INotifyPropertyChanged for UI binding
- Always unsubscribe to avoid memory leaks
- Use WeakEventManager to prevent leaks
- Handle exceptions in handlers
- One-to-many relationships
- Loose coupling between subject and observers

## Resources
- Observer Pattern (Gang of Four)
- Events in C#
- INotifyPropertyChanged
- WeakEventManager (WPF)
