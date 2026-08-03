# While Loops in C#

## Overview

The `while` loop repeats code as long as a condition is true. It's ideal when you don't know in advance how many iterations you need, or when iteration depends on runtime conditions.

## Basic While Loop Syntax

```csharp
while (condition)
{
    // Executed while condition is true
    // Must modify something that affects condition
}
```

**Key Characteristics:**
- Condition checked **before** each iteration
- Loop doesn't execute if condition is initially false
- Must modify condition variable inside loop to avoid infinite loop

## Simple While Loop Examples

### Counting with While

```csharp
int i = 0;
while (i < 5)
{
    Console.WriteLine(i); // 0, 1, 2, 3, 4
    i++;
}

// Counting backward
int count = 5;
while (count > 0)
{
    Console.WriteLine(count); // 5, 4, 3, 2, 1
    count--;
}
```

### Condition-Based Iteration

```csharp
public void ProcessUntilValid()
{
    string input = "";
    
    while (input != "exit")
    {
        Console.Write("Enter command (or 'exit'): ");
        input = Console.ReadLine();
        
        if (input != "exit")
        {
            ProcessCommand(input);
        }
    }
}

public void WaitForCompletion()
{
    var task = StartLongRunningTask();
    
    while (!task.IsCompleted)
    {
        Console.WriteLine("Still processing...");
        Thread.Sleep(1000); // Wait 1 second
    }
    
    Console.WriteLine("Task completed!");
}
```

## Do-While Loops

The `do-while` loop is similar to while, but **always executes at least once** because the condition is checked **after** the first iteration.

### Basic Do-While Syntax

```csharp
do
{
    // Executed at least once
    // Then repeated while condition is true
}
while (condition);
```

### Do-While Examples

```csharp
// Simple example: always runs once
int i = 0;
do
{
    Console.WriteLine(i);
    i++;
}
while (i < 3); // Output: 0, 1, 2

// Even if condition is false initially
int x = 10;
do
{
    Console.WriteLine(x); // Prints 10 even though x >= 5 is true
}
while (x < 5); // Condition false, but body executed once
```

### Do-While Use Cases

#### Menu Loop

```csharp
public void MainMenu()
{
    int choice;
    
    do
    {
        Console.WriteLine("\n=== Main Menu ===");
        Console.WriteLine("1. Add Item");
        Console.WriteLine("2. View Items");
        Console.WriteLine("3. Delete Item");
        Console.WriteLine("4. Exit");
        Console.Write("Enter choice: ");
        
        choice = int.Parse(Console.ReadLine());
        
        switch (choice)
        {
            case 1: AddItem(); break;
            case 2: ViewItems(); break;
            case 3: DeleteItem(); break;
            case 4: Console.WriteLine("Exiting..."); break;
            default: Console.WriteLine("Invalid choice"); break;
        }
    }
    while (choice != 4);
}
```

#### Validation Loop

```csharp
public int GetValidAge()
{
    int age;
    
    do
    {
        Console.Write("Enter age (18-120): ");
        
        if (!int.TryParse(Console.ReadLine(), out age))
        {
            Console.WriteLine("Invalid input. Please enter a number.");
            age = -1; // Force retry
        }
        else if (age < 18 || age > 120)
        {
            Console.WriteLine("Age must be between 18 and 120.");
            age = -1; // Force retry
        }
    }
    while (age < 18 || age > 120);
    
    return age;
}
```

#### Retry Logic

```csharp
public bool ConnectToDatabase(int maxAttempts)
{
    int attempts = 0;
    
    do
    {
        try
        {
            attempts++;
            Console.WriteLine($"Attempt {attempts}...");
            database.Connect();
            return true; // Success
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
            
            if (attempts < maxAttempts)
            {
                Thread.Sleep(1000); // Wait before retry
            }
        }
    }
    while (attempts < maxAttempts);
    
    return false; // All attempts failed
}
```

## Comparing While vs Do-While

### Execution Differences

```csharp
// WHILE: May not execute
int x = 10;
while (x < 5)
{
    Console.WriteLine(x); // Doesn't print - condition false
}

// DO-WHILE: Always executes at least once
int y = 10;
do
{
    Console.WriteLine(y); // Prints 10 - executes before checking condition
}
while (y < 5);
```

### When to Use Each

```csharp
// Use WHILE when:
// - Checking condition first is important
// - Loop might not need to execute
while (IsValid())
{
    ProcessData();
}

// Use DO-WHILE when:
// - Need to execute at least once
// - Typical for user input/menu loops
do
{
    var input = GetUserInput();
    ProcessInput(input);
}
while (ShouldContinue());
```

## Common While Loop Patterns

### Pattern 1: Read Until Specific Value

```csharp
public void ReadUntilEmpty()
{
    string line;
    
    while ((line = Console.ReadLine()) != "")
    {
        Console.WriteLine($"You entered: {line}");
    }
    
    Console.WriteLine("Done.");
}
```

### Pattern 2: File Reading

```csharp
public void ProcessFile(string filePath)
{
    using (var reader = new StreamReader(filePath))
    {
        string line;
        
        while ((line = reader.ReadLine()) != null)
        {
            ProcessLine(line);
        }
    }
}
```

### Pattern 3: Countdown Timer

```csharp
public void Countdown(int seconds)
{
    while (seconds > 0)
    {
        Console.WriteLine($"{seconds}...");
        Thread.Sleep(1000);
        seconds--;
    }
    
    Console.WriteLine("Blastoff!");
}
```

### Pattern 4: Polling

```csharp
public bool WaitForResult(int timeoutMs)
{
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    
    while (stopwatch.ElapsedMilliseconds < timeoutMs)
    {
        if (CheckResult())
        {
            return true; // Result found
        }
        
        Thread.Sleep(100); // Check every 100ms
    }
    
    return false; // Timeout
}
```

### Pattern 5: Processing Queue

```csharp
public void ProcessQueue(Queue<Task> tasks)
{
    while (tasks.Count > 0)
    {
        var task = tasks.Dequeue();
        task.Execute();
        
        if (task.HasSubTasks)
        {
            foreach (var subTask in task.SubTasks)
            {
                tasks.Enqueue(subTask);
            }
        }
    }
}
```

### Pattern 6: State Machine

```csharp
public void RunStateMachine()
{
    State currentState = State.Initial;
    
    while (currentState != State.End)
    {
        switch (currentState)
        {
            case State.Initial:
                Console.WriteLine("Starting...");
                currentState = State.Processing;
                break;
                
            case State.Processing:
                Console.WriteLine("Processing...");
                currentState = State.Validating;
                break;
                
            case State.Validating:
                Console.WriteLine("Validating...");
                currentState = IsValid() ? State.Success : State.Error;
                break;
                
            case State.Success:
                Console.WriteLine("Success!");
                currentState = State.End;
                break;
                
            case State.Error:
                Console.WriteLine("Error occurred!");
                currentState = State.End;
                break;
        }
    }
}

enum State { Initial, Processing, Validating, Success, Error, End }
```

## While vs For: When to Use Each

### Use While When:
- Condition is complex (not simple counter)
- Don't know iteration count in advance
- Condition depends on external state
- Reading/processing stream of data

```csharp
// Good while example: condition depends on data read
while ((line = reader.ReadLine()) != null)
{
    ProcessLine(line);
}
```

### Use For When:
- Counting fixed number of iterations
- Need index access
- Iteration count known in advance
- Simple counter-based loop

```csharp
// Good for example: fixed count with index
for (int i = 0; i < items.Count; i++)
{
    Console.WriteLine($"{i}: {items[i]}");
}
```

### Use Foreach When:
- Simply iterating collection
- Don't need index
- No need for loop control

```csharp
// Good foreach example: simple iteration
foreach (var item in items)
{
    ProcessItem(item);
}
```

## Infinite Loops and Break

### Creating Infinite Loops

```csharp
// Intentional infinite loop (with break)
while (true)
{
    string command = GetCommand();
    
    if (command == "exit")
        break;
    
    ExecuteCommand(command);
}

// Another infinite loop
while (true)
{
    try
    {
        ProcessData();
        break; // Exit on success
    }
    catch
    {
        Console.WriteLine("Retrying...");
    }
}
```

## Performance Considerations

### Avoiding Infinite Loops

```csharp
// BAD: Can become infinite if not careful
while (condition)
{
    // If condition never becomes false, infinite loop!
}

// BETTER: Ensure condition changes
while (condition && !timeout)
{
    DoWork();
    CheckTimeout();
}
```

### Conditional Termination

```csharp
public void SafeLoop()
{
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    int maxDurationMs = 5000;
    
    while (Process() && stopwatch.ElapsedMilliseconds < maxDurationMs)
    {
        // If Process() returns false OR timeout reached, loop exits
    }
}
```

## Best Practices

1. **Always Ensure Condition Can Become False**
   ```csharp
   int count = 0;
   while (count < 10)
   {
       DoWork();
       count++; // Must increment!
   }
   ```

2. **Use Meaningful Condition Variables**
   ```csharp
   // CLEAR
   while (hasMoreData)
   {
       ProcessData();
       hasMoreData = reader.Read();
   }
   
   // UNCLEAR
   while (x)
   {
       ProcessData();
       x = reader.Read();
   }
   ```

3. **Consider Timeout for External Conditions**
   ```csharp
   var timeout = DateTime.Now.AddSeconds(30);
   while (IsProcessing() && DateTime.Now < timeout)
   {
       Thread.Sleep(100);
   }
   ```

4. **Use Do-While for User Input**
   ```csharp
   // Gets at least one input
   do
   {
       input = GetUserInput();
   }
   while (!IsValidInput(input));
   ```

## Summary

- **While**: Check condition before execution (pre-test)
- **Do-While**: Check condition after execution (post-test)
- **While**: Use for condition-based iteration
- **Do-While**: Use for input validation and menus
- Always ensure conditions can become false
- For fixed counts, use for or foreach instead
