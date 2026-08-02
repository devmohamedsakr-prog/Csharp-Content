# Events

## Overview
Events are a wrapper around delegates providing encapsulation and preventing external code from clearing or reassigning.

---

## Event Basics

```csharp
public class Button {
    // Delegate for event
    public delegate void ClickHandler();
    
    // Event wraps delegate - only += and -=
    public event ClickHandler OnClick;
    
    public void Click() {
        OnClick?.Invoke();
    }
}

Button button = new Button();

// Subscribe to event
button.OnClick += () => Console.WriteLine("Clicked!");

// Cannot reassign (only with delegate)
// button.OnClick = () => Console.WriteLine("Wrong!");  // Error
```

---

## Event Pattern

```csharp
public class Publisher {
    // EventArgs for event data
    public class MessageEventArgs : EventArgs {
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
    
    // Event using EventHandler
    public event EventHandler<MessageEventArgs> OnMessage;
    
    public void SendMessage(string message) {
        // Raise event
        OnMessage?.Invoke(this, new MessageEventArgs {
            Message = message,
            Timestamp = DateTime.Now
        });
    }
}

// Subscriber
public class Subscriber {
    public void Subscribe(Publisher publisher) {
        publisher.OnMessage += HandleMessage;
    }
    
    private void HandleMessage(object sender, Publisher.MessageEventArgs e) {
        Console.WriteLine($"Message: {e.Message} at {e.Timestamp}");
    }
}

// Usage
Publisher pub = new Publisher();
Subscriber sub = new Subscriber();
sub.Subscribe(pub);

pub.SendMessage("Hello World");
```

---

## Event Handlers

```csharp
public delegate void EventHandler(object sender, EventArgs e);
public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e);

// Predefined events
public event EventHandler Click;
public event EventHandler<CustomEventArgs> ValueChanged;

// Raising events
protected void OnClick() {
    Click?.Invoke(this, EventArgs.Empty);
}

protected void OnValueChanged(CustomEventArgs args) {
    ValueChanged?.Invoke(this, args);
}
```

---

## Publisher-Subscriber Pattern

```csharp
// Publisher
public class Model {
    private int value;
    
    public int Value {
        get { return value; }
        set {
            if (this.value != value) {
                this.value = value;
                OnValueChanged(new ValueChangedArgs { OldValue = this.value, NewValue = value });
            }
        }
    }
    
    public event EventHandler<ValueChangedArgs> OnValueChanged;
    
    protected void OnValueChanged(ValueChangedArgs args) {
        OnValueChanged?.Invoke(this, args);
    }
}

public class ValueChangedArgs : EventArgs {
    public int OldValue { get; set; }
    public int NewValue { get; set; }
}

// Subscriber 1
public class Logger {
    public void Subscribe(Model model) {
        model.OnValueChanged += Log;
    }
    
    private void Log(object sender, ValueChangedArgs e) {
        Console.WriteLine($"Value changed: {e.OldValue} -> {e.NewValue}");
    }
}

// Subscriber 2
public class UI {
    public void Subscribe(Model model) {
        model.OnValueChanged += UpdateDisplay;
    }
    
    private void UpdateDisplay(object sender, ValueChangedArgs e) {
        Console.WriteLine($"Updating UI: {e.NewValue}");
    }
}

// Usage
Model model = new Model();
Logger logger = new Logger();
UI ui = new UI();

logger.Subscribe(model);
ui.Subscribe(model);

model.Value = 42;
// Output:
// Value changed: 0 -> 42
// Updating UI: 42
```

---

## Best Practices

✓ **Always check before invoking**
```csharp
OnValueChanged?.Invoke(this, args);
```

✓ **Use EventArgs for data**
```csharp
public class CustomEventArgs : EventArgs {
    public string Data { get; set; }
}
```

✓ **Unsubscribe when done**
```csharp
model.OnValueChanged -= UpdateDisplay;
```

---

## Quick Summary

- Events wrap delegates for encapsulation
- Publisher raises events
- Subscribers subscribe with +=
- Only += and -= allowed (not reassignment)
- Use EventHandler and EventArgs
- Implement publisher-subscriber pattern
