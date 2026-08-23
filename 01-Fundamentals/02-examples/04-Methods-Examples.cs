/*
 * Methods Examples
 * Demonstrating method declaration, parameters, return types, and overloading
 */

using System;

namespace CSharp.Fundamentals.Methods
{
    class MethodsExamples
    {
        static void Main()
        {
            Console.WriteLine("=== METHOD BASICS ===\n");
            MethodBasicsDemo();
            
            Console.WriteLine("\n=== METHOD PARAMETERS ===\n");
            MethodParametersDemo();
            
            Console.WriteLine("\n=== METHOD OVERLOADING ===\n");
            MethodOverloadingDemo();
            
            Console.WriteLine("\n=== RECURSIVE METHODS ===\n");
            RecursiveMethodsDemo();
        }
        
        // Simple method with no parameters and no return
        static void GreetUser()
        {
            Console.WriteLine("Hello, welcome to C# methods!");
        }
        
        // Method with return type
        static int Add(int a, int b)
        {
            return a + b;
        }
        
        // Method with multiple parameters
        static double CalculateAverage(double num1, double num2, double num3)
        {
            return (num1 + num2 + num3) / 3;
        }
        
        // Method with optional parameters
        static void PrintMessage(string message, int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                Console.WriteLine(message);
            }
        }
        
        // Method with named parameters
        static string CreateUserProfile(string name, int age, string email, string city = "Unknown")
        {
            return $"Name: {name}, Age: {age}, Email: {email}, City: {city}";
        }
        
        // Method with reference parameter (ref)
        static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        
        // Method with output parameter (out)
        static bool DivideSafely(int dividend, int divisor, out int result, out string message)
        {
            result = 0;
            message = "";
            
            if (divisor == 0)
            {
                message = "Cannot divide by zero";
                return false;
            }
            
            result = dividend / divisor;
            message = "Division successful";
            return true;
        }
        
        // Method with params array
        static int SumAll(params int[] numbers)
        {
            int sum = 0;
            foreach (int num in numbers)
            {
                sum += num;
            }
            return sum;
        }
        
        // Overloaded method - same name, different parameters
        static int Multiply(int a, int b)
        {
            return a * b;
        }
        
        static double Multiply(double a, double b)
        {
            return a * b;
        }
        
        static int Multiply(int a, int b, int c)
        {
            return a * b * c;
        }
        
        // Recursive factorial method
        static int Factorial(int n)
        {
            if (n <= 1)
                return 1;
            return n * Factorial(n - 1);
        }
        
        // Recursive Fibonacci
        static int Fibonacci(int n)
        {
            if (n <= 1)
                return n;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }
        
        // Method that returns multiple values using out
        static void GetMinMax(int[] numbers, out int min, out int max)
        {
            min = numbers[0];
            max = numbers[0];
            
            foreach (int num in numbers)
            {
                if (num < min) min = num;
                if (num > max) max = num;
            }
        }
        
        static void MethodBasicsDemo()
        {
            Console.WriteLine("--- No Parameters, No Return ---");
            GreetUser();
            
            Console.WriteLine("\n--- With Return Type ---");
            int result = Add(10, 20);
            Console.WriteLine($"Add(10, 20) = {result}");
            
            Console.WriteLine("\n--- Multiple Parameters ---");
            double avg = CalculateAverage(85, 90, 95);
            Console.WriteLine($"Average of 85, 90, 95 = {avg}");
        }
        
        static void MethodParametersDemo()
        {
            Console.WriteLine("--- Optional Parameters ---");
            PrintMessage("Hello");
            PrintMessage("Hi", 3);
            
            Console.WriteLine("\n--- Named Parameters ---");
            string profile1 = CreateUserProfile("John", 30, "john@example.com");
            Console.WriteLine(profile1);
            
            string profile2 = CreateUserProfile(age: 25, name: "Jane", email: "jane@example.com", city: "NYC");
            Console.WriteLine(profile2);
            
            Console.WriteLine("\n--- Ref Parameter ---");
            int x = 5;
            int y = 10;
            Console.WriteLine($"Before swap: x = {x}, y = {y}");
            Swap(ref x, ref y);
            Console.WriteLine($"After swap: x = {x}, y = {y}");
            
            Console.WriteLine("\n--- Out Parameter ---");
            if (DivideSafely(20, 5, out int quotient, out string msg))
            {
                Console.WriteLine($"Result: {quotient}, Message: {msg}");
            }
            
            if (DivideSafely(20, 0, out int quotient2, out string msg2))
            {
                Console.WriteLine($"Result: {quotient2}, Message: {msg2}");
            }
            else
            {
                Console.WriteLine($"Error: {msg2}");
            }
            
            Console.WriteLine("\n--- Params Array ---");
            int sum1 = SumAll(1, 2, 3, 4, 5);
            int sum2 = SumAll(10, 20, 30);
            int sum3 = SumAll(100);
            
            Console.WriteLine($"SumAll(1, 2, 3, 4, 5) = {sum1}");
            Console.WriteLine($"SumAll(10, 20, 30) = {sum2}");
            Console.WriteLine($"SumAll(100) = {sum3}");
        }
        
        static void MethodOverloadingDemo()
        {
            Console.WriteLine("--- Overloading with Different Types ---");
            Console.WriteLine($"Multiply(5, 10) = {Multiply(5, 10)}");
            Console.WriteLine($"Multiply(3.5, 2.5) = {Multiply(3.5, 2.5)}");
            
            Console.WriteLine("\n--- Overloading with Different Number of Parameters ---");
            Console.WriteLine($"Multiply(5, 10) = {Multiply(5, 10)}");
            Console.WriteLine($"Multiply(5, 10, 2) = {Multiply(5, 10, 2)}");
        }
        
        static void RecursiveMethodsDemo()
        {
            Console.WriteLine("--- Factorial ---");
            for (int i = 1; i <= 6; i++)
            {
                Console.WriteLine($"Factorial({i}) = {Factorial(i)}");
            }
            
            Console.WriteLine("\n--- Fibonacci ---");
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"Fibonacci({i}) = {Fibonacci(i)}");
            }
            
            Console.WriteLine("\n--- Min and Max ---");
            int[] numbers = { 15, 3, 45, 12, 67, 8, 42 };
            GetMinMax(numbers, out int min, out int max);
            Console.WriteLine($"Array: {string.Join(", ", numbers)}");
            Console.WriteLine($"Min: {min}, Max: {max}");
        }
    }
}
