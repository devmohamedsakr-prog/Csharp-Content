# Increment and Decrement Operators

## Overview

Increment and decrement operators increase or decrease a value by 1. They have two forms: prefix and postfix, which behave differently in expressions.

## Pre-Increment (++x)

Increments the variable and returns the new value.

```csharp
int x = 5;
int result = ++x;

// x is now 6
// result = 6 (uses new value)

// In a loop
int count = 0;
while (count < 3) {
    Console.WriteLine(++count);  // Prints: 1, 2, 3
}
```

**Behavior**: Increment first, then use value

---

## Post-Increment (x++)

Returns the current value, then increments.

```csharp
int x = 5;
int result = x++;

// x is now 6
// result = 5 (uses old value)

// In a loop
int count = 0;
while (count < 3) {
    Console.WriteLine(count++);  // Prints: 0, 1, 2
}
```

**Behavior**: Use value first, then increment

---

## Pre-Decrement (--x)

Decrements the variable and returns the new value.

```csharp
int x = 5;
int result = --x;

// x is now 4
// result = 4 (uses new value)

// Countdown
for (int i = 3; i > 0; --i) {
    Console.WriteLine(i);  // Prints: 3, 2, 1
}
```

---

## Post-Decrement (x--)

Returns the current value, then decrements.

```csharp
int x = 5;
int result = x--;

// x is now 4
// result = 5 (uses old value)

// Array processing
int[] items = { 'a', 'b', 'c' };
int index = items.Length - 1;
while (index >= 0) {
    Console.WriteLine(items[index--]);  // Prints: c, b, a
}
```

---

## Prefix vs Postfix Comparison

| Aspect | ++x (prefix) | x++ (postfix) |
|--------|------------|------------|
| **Return Value** | New value | Old value |
| **When Used** | Before expression | After expression |
| **Performance** | Slightly faster | Slightly slower (creates temp) |
| **Readability** | Clearer in loops | Common in post-processing |

**When it matters**:
```csharp
int a = 5;
int b = ++a;  // a=6, b=6

int x = 5;
int y = x++;  // x=6, y=5
```

**When it doesn't matter**:
```csharp
int a = 5;
++a;   // a becomes 6
a++;   // a becomes 7 (either works same)

// Both increment, difference in return value doesn't matter
```

---

## Common Use Cases

### Loop Counters
```csharp
// Pre-increment (preferred)
for (int i = 0; i < 10; ++i) {
    Console.WriteLine(i);
}

// Post-increment (also common)
for (int i = 0; i < 10; i++) {
    Console.WriteLine(i);
}
```

### Array/Collection Processing
```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
int index = 0;

while (index < numbers.Length) {
    Console.WriteLine(numbers[index++]);
}
```

### Counters and Accumulators
```csharp
int attempts = 0;
while (attempts < 3) {
    Console.WriteLine("Attempt " + (++attempts));
}

// Prints: Attempt 1, Attempt 2, Attempt 3
```

### ID Generation
```csharp
class IdGenerator {
    private int _nextId = 0;
    
    public int GetNextId() {
        return ++_nextId;  // Increment before returning
    }
}
```

---

## Prefix vs Postfix Performance

In compiled code, the difference is often negligible:

```csharp
// Both usually optimize to same machine code
for (int i = 0; i < 1000000; i++) { }
for (int i = 0; i < 1000000; ++i) { }
```

However, with complex objects:

```csharp
class Counter {
    private int _value = 0;
    
    // Postfix: creates temporary copy
    public Counter operator++(int) {
        Counter temp = new Counter();
        temp._value = _value + 1;
        _value++;
        return temp;  // Returns old value
    }
    
    // Prefix: no temporary
    public Counter operator++() {
        _value++;
        return this;  // Returns new value
    }
}

Counter c = new Counter();
Counter old = c++;  // Creates temporary
Counter old = ++c;  // No temporary (faster)
```

---

## Practical Examples

### Processing Array
```csharp
char[] chars = { 'A', 'B', 'C', 'D' };
int index = 0;

while (index < chars.Length) {
    Console.WriteLine(chars[index++]);
}
// Output: A, B, C, D
```

### Stack-like Operations
```csharp
int[] stack = new int[10];
int top = -1;

void Push(int value) {
    stack[++top] = value;  // Increment then use
}

int Pop() {
    return stack[top--];  // Use then decrement
}
```

### Generating Sequences
```csharp
int id = 0;
for (int i = 0; i < 5; i++) {
    Console.WriteLine("ID: " + ++id);  // 1, 2, 3, 4, 5
}
```

### Counting Down
```csharp
for (int countdown = 5; countdown > 0; countdown--) {
    Console.WriteLine(countdown);  // 5, 4, 3, 2, 1
}
```

---

## Increment/Decrement in Complex Expressions

```csharp
// Prefix (often preferred)
int x = 5;
int y = ++x * 2;  // x becomes 6, y = 6 * 2 = 12

// Postfix
int a = 5;
int b = a++ * 2;  // b = 5 * 2 = 10, then a becomes 6

// Multiple increments
int n = 5;
int result = ++n + ++n;  // Undefined behavior! (evaluation order uncertain)
```

**Note**: Avoid incrementing same variable multiple times in one expression.

---

## Best Practices

✓ **Use for simple increment/decrement**
```csharp
count++;
index--;
id++;
```

✓ **Prefer prefix for performance with objects**
```csharp
++iterator;  // Slightly faster than iterator++
```

✓ **Use in loops when return value ignored**
```csharp
for (int i = 0; i < 10; i++) {  // Either works
    // ...
}
```

✓ **Be explicit in complex expressions**
```csharp
// Clear
int value = count + 1;
++count;

// Less clear (though valid)
int value = ++count;
```

---

## Common Mistakes

❌ **Depending on return value incorrectly**
```csharp
int x = 5;
int y = x++;  // y = 5, not 6!
```

✓ **Use prefix if you need new value**
```csharp
int x = 5;
int y = ++x;  // y = 6
```

---

❌ **Incrementing multiple times in expression**
```csharp
int x = 5;
int y = ++x + ++x;  // Undefined behavior!
```

✓ **Increment separately**
```csharp
int x = 5;
++x;
++x;
int y = x;
```

---

❌ **Over-use with compound operators**
```csharp
int x = 5;
x++ += 1;  // Error: can't chain
```

✓ **Use compound operators**
```csharp
int x = 5;
x += 1;  // Same as x++
```

---

## Quick Reference

| Operator | Name | Example | Behavior |
|----------|------|---------|----------|
| ++x | Pre-increment | ++i | Increment, return new value |
| x++ | Post-increment | i++ | Return old value, increment |
| --x | Pre-decrement | --i | Decrement, return new value |
| x-- | Post-decrement | i-- | Return old value, decrement |

---

## Next Steps

- Review [Arithmetic Operators](../01-Arithmetic/00-Arithmetic-Operators.md)
- Study [Assignment Operators](../02-Assignment/00-Assignment-Operators.md)
- Learn [Comparison Operators](../../02-Comparison-Logical/01-Comparison/00-Comparison-Operators.md)
