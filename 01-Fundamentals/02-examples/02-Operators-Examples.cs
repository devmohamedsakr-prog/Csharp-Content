/*
 * Operators Examples
 * Demonstrating arithmetic, logical, comparison, and assignment operators
 */

using System;

namespace CSharp.Fundamentals.Operators
{
    class OperatorsExamples
    {
        static void Main()
        {
            Console.WriteLine("=== ARITHMETIC OPERATORS ===\n");
            ArithmeticOperatorsDemo();
            
            Console.WriteLine("\n=== COMPARISON OPERATORS ===\n");
            ComparisonOperatorsDemo();
            
            Console.WriteLine("\n=== LOGICAL OPERATORS ===\n");
            LogicalOperatorsDemo();
            
            Console.WriteLine("\n=== ASSIGNMENT OPERATORS ===\n");
            AssignmentOperatorsDemo();
            
            Console.WriteLine("\n=== BITWISE OPERATORS ===\n");
            BitwiseOperatorsDemo();
            
            Console.WriteLine("\n=== TERNARY OPERATOR ===\n");
            TernaryOperatorDemo();
        }
        
        static void ArithmeticOperatorsDemo()
        {
            int a = 20;
            int b = 5;
            
            Console.WriteLine($"a = {a}, b = {b}");
            Console.WriteLine($"Addition (+): a + b = {a + b}");
            Console.WriteLine($"Subtraction (-): a - b = {a - b}");
            Console.WriteLine($"Multiplication (*): a * b = {a * b}");
            Console.WriteLine($"Division (/): a / b = {a / b}");
            Console.WriteLine($"Modulus (%): a % b = {a % b}");
            
            // Increment and Decrement
            int count = 5;
            Console.WriteLine($"\nIncrement/Decrement:");
            Console.WriteLine($"count = {count}");
            Console.WriteLine($"count++ (post-increment) = {count++}");
            Console.WriteLine($"After post-increment, count = {count}");
            Console.WriteLine($"++count (pre-increment) = {++count}");
            Console.WriteLine($"count-- (post-decrement) = {count--}");
            Console.WriteLine($"--count (pre-decrement) = {--count}");
        }
        
        static void ComparisonOperatorsDemo()
        {
            int x = 15;
            int y = 10;
            
            Console.WriteLine($"x = {x}, y = {y}");
            Console.WriteLine($"x == y: {x == y}  (Equal to)");
            Console.WriteLine($"x != y: {x != y}  (Not equal to)");
            Console.WriteLine($"x > y: {x > y}   (Greater than)");
            Console.WriteLine($"x < y: {x < y}   (Less than)");
            Console.WriteLine($"x >= y: {x >= y}  (Greater than or equal)");
            Console.WriteLine($"x <= y: {x <= y}  (Less than or equal)");
            
            // String comparison
            Console.WriteLine("\n--- String Comparison ---");
            string str1 = "Hello";
            string str2 = "Hello";
            string str3 = "hello";
            
            Console.WriteLine($"str1 = \"{str1}\", str2 = \"{str2}\", str3 = \"{str3}\"");
            Console.WriteLine($"str1 == str2: {str1 == str2}");
            Console.WriteLine($"str1 == str3: {str1 == str3}");
            Console.WriteLine($"str1.Equals(str2): {str1.Equals(str2)}");
            Console.WriteLine($"str1.Equals(str3, StringComparison.OrdinalIgnoreCase): {str1.Equals(str3, StringComparison.OrdinalIgnoreCase)}");
        }
        
        static void LogicalOperatorsDemo()
        {
            bool a = true;
            bool b = false;
            
            Console.WriteLine($"a = {a}, b = {b}");
            Console.WriteLine($"a && b (AND): {a && b}");
            Console.WriteLine($"a || b (OR): {a || b}");
            Console.WriteLine($"!a (NOT): {!a}");
            Console.WriteLine($"!b (NOT): {!b}");
            
            // Practical example
            Console.WriteLine("\n--- Practical Example ---");
            int age = 25;
            int income = 50000;
            
            bool isAdult = age >= 18;
            bool hasGoodIncome = income >= 30000;
            
            Console.WriteLine($"Age: {age}, Income: ${income}");
            Console.WriteLine($"Is Adult: {isAdult}");
            Console.WriteLine($"Has Good Income: {hasGoodIncome}");
            Console.WriteLine($"Eligible for loan (Adult AND Good Income): {isAdult && hasGoodIncome}");
            
            // Short-circuit evaluation
            Console.WriteLine("\n--- Short-circuit Evaluation ---");
            bool result1 = false && GetValue(true);  // GetValue won't be called
            bool result2 = true || GetValue(false);  // GetValue won't be called
            
            Console.WriteLine($"false && GetValue(true) = {result1}");
            Console.WriteLine($"true || GetValue(false) = {result2}");
        }
        
        static void AssignmentOperatorsDemo()
        {
            int num = 10;
            Console.WriteLine($"Initial: num = {num}");
            
            num += 5;   // num = num + 5
            Console.WriteLine($"After += 5: num = {num}");
            
            num -= 3;   // num = num - 3
            Console.WriteLine($"After -= 3: num = {num}");
            
            num *= 2;   // num = num * 2
            Console.WriteLine($"After *= 2: num = {num}");
            
            num /= 4;   // num = num / 4
            Console.WriteLine($"After /= 4: num = {num}");
            
            num %= 5;   // num = num % 5
            Console.WriteLine($"After %= 5: num = {num}");
        }
        
        static void BitwiseOperatorsDemo()
        {
            int a = 12;  // Binary: 1100
            int b = 25;  // Binary: 11001
            
            Console.WriteLine($"a = {a} (Binary: {Convert.ToString(a, 2)})");
            Console.WriteLine($"b = {b} (Binary: {Convert.ToString(b, 2)})");
            
            Console.WriteLine($"\nBitwise AND (&): {a} & {b} = {a & b}");
            Console.WriteLine($"  Binary: {Convert.ToString(a & b, 2)}");
            
            Console.WriteLine($"\nBitwise OR (|): {a} | {b} = {a | b}");
            Console.WriteLine($"  Binary: {Convert.ToString(a | b, 2)}");
            
            Console.WriteLine($"\nBitwise XOR (^): {a} ^ {b} = {a ^ b}");
            Console.WriteLine($"  Binary: {Convert.ToString(a ^ b, 2)}");
            
            Console.WriteLine($"\nBitwise NOT (~): ~{a} = {~a}");
            
            Console.WriteLine($"\nLeft Shift (<<): {a} << 2 = {a << 2}");
            Console.WriteLine($"Right Shift (>>): {b} >> 2 = {b >> 2}");
        }
        
        static void TernaryOperatorDemo()
        {
            int age = 20;
            string status = (age >= 18) ? "Adult" : "Minor";
            Console.WriteLine($"Age: {age} -> Status: {status}");
            
            int score = 75;
            string grade = score >= 90 ? "A" :
                          score >= 80 ? "B" :
                          score >= 70 ? "C" :
                          score >= 60 ? "D" :
                          "F";
            Console.WriteLine($"Score: {score} -> Grade: {grade}");
            
            // Nested ternary (readable with proper formatting)
            int number = 50;
            string classification = (number > 0) 
                ? ((number % 2 == 0) ? "Positive Even" : "Positive Odd")
                : ((number % 2 == 0) ? "Negative Even" : "Negative Odd");
            Console.WriteLine($"Number: {number} -> {classification}");
        }
        
        static bool GetValue(bool value)
        {
            Console.WriteLine($"GetValue called with: {value}");
            return value;
        }
    }
}
