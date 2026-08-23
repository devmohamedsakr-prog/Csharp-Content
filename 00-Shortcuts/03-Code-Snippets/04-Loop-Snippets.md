# Loop Snippets

Generate loops quickly with built-in snippets.

## for - For Loop

**Shortcut:** `for` + Tab

**Generates:**
```csharp
for (int i = 0; i < length; i++)
{
}
```

**Placeholders:**
- i: Loop counter
- length: Replace with array/collection length

**Usage:**
```csharp
for → Tab
// Type array name or number
```

**Examples:**
```csharp
// Count from 0 to 9
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}

// Iterate through array
int[] numbers = { 1, 2, 3, 4, 5 };
for (int i = 0; i < numbers.Length; i++)
{
    Console.WriteLine(numbers[i]);
}

// Reverse loop
for (int i = 9; i >= 0; i--)
{
    Console.WriteLine(i);
}
```

---

## foreach - Foreach Loop

**Shortcut:** `foreach` + Tab

**Generates:**
```csharp
foreach (var item in collection)
{
}
```

**Placeholders:**
- item: Item variable
- collection: Replace with collection name

**Usage:**
```csharp
foreach → Tab
// Select from IntelliSense
```

**Examples:**
```csharp
// Iterate list
List<string> names = new List<string> { "Alice", "Bob", "Charlie" };
foreach (var name in names)
{
    Console.WriteLine(name);
}

// Iterate array
int[] numbers = { 1, 2, 3, 4, 5 };
foreach (int num in numbers)
{
    Console.WriteLine(num * 2);
}

// Iterate with type
foreach (var person in people)
{
    Console.WriteLine(person.Name);
}
```

**Note:** Use `foreach` when you don't need the index

---

## while - While Loop

**Shortcut:** `while` + Tab

**Generates:**
```csharp
while (true)
{
}
```

**Placeholders:**
- true: Replace with condition

**Usage:**
```csharp
while → Tab
// Replace true with actual condition
```

**Examples:**
```csharp
// Count down
int count = 5;
while (count > 0)
{
    Console.WriteLine(count);
    count--;
}

// Read input until specific value
while (true)
{
    Console.Write("Enter quit to exit: ");
    string input = Console.ReadLine();
    if (input == "quit") break;
}

// Process items while available
while (queue.Count > 0)
{
    string item = queue.Dequeue();
    ProcessItem(item);
}
```

---

## do - Do-While Loop

**Shortcut:** `do` + Tab

**Generates:**
```csharp
do
{
} while (true);
```

**Usage:**
```csharp
do → Tab
// Executes at least once before checking condition
```

**Examples:**
```csharp
// Menu loop - runs at least once
do
{
    Console.WriteLine("1. Add");
    Console.WriteLine("2. Delete");
    Console.WriteLine("3. Exit");
    Console.Write("Choose: ");
    
    string choice = Console.ReadLine();
    if (choice == "3") break;
    
} while (true);

// Repeat until valid input
int num = 0;
do
{
    Console.Write("Enter positive number: ");
} while (!int.TryParse(Console.ReadLine(), out num) || num <= 0);
```

**Note:** Use do-while for operations that must execute at least once

---

## for with Decrement

**Pattern:**
```csharp
for (int i = 10; i > 0; i--)
{
    Console.WriteLine(i);
}
```

---

## Nested Loops

**Pattern:**
```csharp
// Print multiplication table
for (int i = 1; i <= 10; i++)
{
    for (int j = 1; j <= 10; j++)
    {
        Console.Write($"{i * j,3} ");
    }
    Console.WriteLine();
}

// Search in 2D array
for (int row = 0; row < array.GetLength(0); row++)
{
    for (int col = 0; col < array.GetLength(1); col++)
    {
        if (array[row, col] == target)
        {
            Console.WriteLine($"Found at {row},{col}");
        }
    }
}
```

---

## Loop Control

**Break - Exit loop**
```csharp
for (int i = 0; i < 10; i++)
{
    if (i == 5) break;
    Console.WriteLine(i);
}
```

**Continue - Skip iteration**
```csharp
for (int i = 0; i < 10; i++)
{
    if (i % 2 == 0) continue;
    Console.WriteLine(i); // Only odd numbers
}
```

---

## Quick Reference

| Loop Type | Shortcut | When to Use |
|-----------|----------|------------|
| for | `for` | Known count, need index |
| foreach | `foreach` | Iterate collection |
| while | `while` | Unknown count, check condition first |
| do-while | `do` | Run at least once |

---

## Performance Tips

- Use `foreach` for simpler iteration (more readable)
- Use `for` when you need index
- Avoid modifying collection while iterating
- Break early to avoid unnecessary iterations
- Consider LINQ for complex filtering

