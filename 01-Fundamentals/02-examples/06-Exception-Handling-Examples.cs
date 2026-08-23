/*
 * Exception Handling Examples
 * Demonstrating try-catch-finally and custom exceptions
 */

using System;

namespace CSharp.Fundamentals.ExceptionHandling
{
    class ExceptionHandlingExamples
    {
        static void Main()
        {
            Console.WriteLine("=== BASIC TRY-CATCH ===\n");
            BasicTryCatchDemo();
            
            Console.WriteLine("\n=== MULTIPLE CATCH BLOCKS ===\n");
            MultipleCatchBlocksDemo();
            
            Console.WriteLine("\n=== TRY-CATCH-FINALLY ===\n");
            TryCatchFinallyDemo();
            
            Console.WriteLine("\n=== THROWING EXCEPTIONS ===\n");
            ThrowingExceptionsDemo();
            
            Console.WriteLine("\n=== CUSTOM EXCEPTIONS ===\n");
            CustomExceptionsDemo();
        }
        
        static void BasicTryCatchDemo()
        {
            Console.WriteLine("--- Division by Zero ---");
            try
            {
                int a = 10;
                int b = 0;
                int result = a / b;
                Console.WriteLine($"Result: {result}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine("\n--- Array Index Out of Range ---");
            try
            {
                int[] numbers = { 1, 2, 3 };
                Console.WriteLine(numbers[5]);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine("\n--- Format Exception ---");
            try
            {
                string value = "NotANumber";
                int num = int.Parse(value);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        static void MultipleCatchBlocksDemo()
        {
            Console.WriteLine("--- Multiple Catch Blocks ---");
            try
            {
                Console.WriteLine("Enter a number:");
                // Simulating user input
                string input = "abc";
                int number = int.Parse(input);
                
                int result = 100 / number;
                Console.WriteLine($"Result: {result}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Input is not a valid number format");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Error: Number is too large or too small");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error: Cannot divide by zero");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
        
        static void TryCatchFinallyDemo()
        {
            Console.WriteLine("--- Try-Catch-Finally ---");
            
            try
            {
                Console.WriteLine("Inside try block");
                int[] numbers = { 1, 2, 3 };
                Console.WriteLine(numbers[0]);
                
                // This will throw an exception
                Console.WriteLine(numbers[10]);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Inside catch block: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Inside finally block (always executes)");
            }
            
            Console.WriteLine("\n--- Finally with Return ---");
            Console.WriteLine($"Result: {TryFinallyWithReturn()}");
        }
        
        static int TryFinallyWithReturn()
        {
            try
            {
                Console.WriteLine("  In try block, returning 10");
                return 10;
            }
            finally
            {
                Console.WriteLine("  In finally block (executes even with return)");
            }
        }
        
        static void ThrowingExceptionsDemo()
        {
            Console.WriteLine("--- Throwing Exceptions ---");
            
            try
            {
                int age = -5;
                ValidateAge(age);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }
            
            Console.WriteLine("\n--- Rethrowing Exception ---");
            try
            {
                try
                {
                    throw new InvalidOperationException("Original error");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Inner catch: {ex.Message}");
                    throw;  // Rethrow the exception
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Outer catch: {ex.Message}");
            }
        }
        
        static void ValidateAge(int age)
        {
            if (age < 0)
            {
                throw new ArgumentException("Age cannot be negative");
            }
            if (age > 150)
            {
                throw new ArgumentException("Age is unrealistic");
            }
            Console.WriteLine($"Age {age} is valid");
        }
        
        static void CustomExceptionsDemo()
        {
            Console.WriteLine("--- Custom Exception ---");
            
            try
            {
                BankAccount account = new BankAccount(100);
                account.Withdraw(150);
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"Custom exception caught: {ex.Message}");
                Console.WriteLine($"Available balance: {ex.AvailableBalance}");
                Console.WriteLine($"Requested amount: {ex.RequestedAmount}");
            }
            
            Console.WriteLine("\n--- Multiple Custom Exceptions ---");
            try
            {
                ProcessOrder("", 5);
            }
            catch (InvalidOrderException ex)
            {
                Console.WriteLine($"Order error: {ex.Message}");
            }
            
            try
            {
                ProcessOrder("Product123", 0);
            }
            catch (InvalidOrderException ex)
            {
                Console.WriteLine($"Order error: {ex.Message}");
            }
        }
        
        static void ProcessOrder(string productId, int quantity)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new InvalidOrderException("Product ID cannot be empty");
            }
            
            if (quantity <= 0)
            {
                throw new InvalidOrderException("Quantity must be greater than zero");
            }
            
            Console.WriteLine($"Order processed: {productId}, Quantity: {quantity}");
        }
    }
    
    // Custom Exception Classes
    public class InsufficientFundsException : Exception
    {
        public decimal AvailableBalance { get; set; }
        public decimal RequestedAmount { get; set; }
        
        public InsufficientFundsException(string message, decimal availableBalance, decimal requestedAmount)
            : base(message)
        {
            AvailableBalance = availableBalance;
            RequestedAmount = requestedAmount;
        }
    }
    
    public class InvalidOrderException : Exception
    {
        public InvalidOrderException(string message) : base(message) { }
    }
    
    public class BankAccount
    {
        private decimal balance;
        
        public BankAccount(decimal initialBalance)
        {
            balance = initialBalance;
        }
        
        public void Withdraw(decimal amount)
        {
            if (amount > balance)
            {
                throw new InsufficientFundsException(
                    $"Insufficient funds. You need {amount - balance} more.",
                    balance,
                    amount
                );
            }
            balance -= amount;
            Console.WriteLine($"Withdrawn: ${amount}, New balance: ${balance}");
        }
        
        public decimal GetBalance()
        {
            return balance;
        }
    }
}
