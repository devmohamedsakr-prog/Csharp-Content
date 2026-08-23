/*
 * Control Flow Examples
 * Demonstrating if, else if, else, and switch statements
 */

using System;

namespace CSharp.Fundamentals.ControlFlow
{
    class ControlFlowExamples
    {
        static void Main()
        {
            Console.WriteLine("=== IF-ELSE STATEMENTS ===\n");
            IfElseExamples();
            
            Console.WriteLine("\n=== SWITCH STATEMENTS ===\n");
            SwitchExamples();
            
            Console.WriteLine("\n=== NESTED CONTROL STRUCTURES ===\n");
            NestedExamples();
        }
        
        static void IfElseExamples()
        {
            // Simple if
            Console.WriteLine("--- Simple If ---");
            int age = 18;
            if (age >= 18)
            {
                Console.WriteLine("You are an adult.");
            }
            
            // If-else
            Console.WriteLine("\n--- If-Else ---");
            int number = 10;
            if (number > 0)
            {
                Console.WriteLine($"{number} is positive");
            }
            else
            {
                Console.WriteLine($"{number} is not positive");
            }
            
            // If-else if-else
            Console.WriteLine("\n--- If-Else If-Else ---");
            int score = 75;
            if (score >= 90)
            {
                Console.WriteLine("Grade: A");
            }
            else if (score >= 80)
            {
                Console.WriteLine("Grade: B");
            }
            else if (score >= 70)
            {
                Console.WriteLine("Grade: C");
            }
            else if (score >= 60)
            {
                Console.WriteLine("Grade: D");
            }
            else
            {
                Console.WriteLine("Grade: F");
            }
            
            // Complex conditions
            Console.WriteLine("\n--- Complex Conditions ---");
            int salary = 50000;
            int yearsExperience = 5;
            
            if (salary > 40000 && yearsExperience >= 3)
            {
                Console.WriteLine("Eligible for promotion");
            }
            else if (salary > 30000 || yearsExperience > 2)
            {
                Console.WriteLine("Eligible for raise");
            }
            else
            {
                Console.WriteLine("Continue working");
            }
        }
        
        static void SwitchExamples()
        {
            // Switch with integers
            Console.WriteLine("--- Switch with Integers ---");
            int day = 3;
            switch (day)
            {
                case 1:
                    Console.WriteLine("Monday");
                    break;
                case 2:
                    Console.WriteLine("Tuesday");
                    break;
                case 3:
                    Console.WriteLine("Wednesday");
                    break;
                case 4:
                    Console.WriteLine("Thursday");
                    break;
                case 5:
                    Console.WriteLine("Friday");
                    break;
                case 6:
                    Console.WriteLine("Saturday");
                    break;
                case 7:
                    Console.WriteLine("Sunday");
                    break;
                default:
                    Console.WriteLine("Invalid day");
                    break;
            }
            
            // Switch with strings
            Console.WriteLine("\n--- Switch with Strings ---");
            string color = "red";
            switch (color.ToLower())
            {
                case "red":
                    Console.WriteLine("Color: Red");
                    break;
                case "green":
                    Console.WriteLine("Color: Green");
                    break;
                case "blue":
                    Console.WriteLine("Color: Blue");
                    break;
                default:
                    Console.WriteLine("Unknown color");
                    break;
            }
            
            // Switch with fall-through
            Console.WriteLine("\n--- Switch with Fall-through ---");
            char grade = 'A';
            switch (grade)
            {
                case 'A':
                case 'B':
                    Console.WriteLine("Excellent performance");
                    break;
                case 'C':
                    Console.WriteLine("Good performance");
                    break;
                case 'D':
                case 'F':
                    Console.WriteLine("Needs improvement");
                    break;
                default:
                    Console.WriteLine("Invalid grade");
                    break;
            }
            
            // Switch with complex logic
            Console.WriteLine("\n--- Switch with Complex Logic ---");
            int month = 2;
            int year = 2024;
            int daysInMonth;
            
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    daysInMonth = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    daysInMonth = 30;
                    break;
                case 2:
                    daysInMonth = (year % 4 == 0) ? 29 : 28;
                    break;
                default:
                    daysInMonth = 0;
                    break;
            }
            
            Console.WriteLine($"Days in month {month} of year {year}: {daysInMonth}");
        }
        
        static void NestedExamples()
        {
            // Nested if
            Console.WriteLine("--- Nested If ---");
            int age = 25;
            string hasLicense = "yes";
            
            if (age >= 18)
            {
                if (hasLicense.ToLower() == "yes")
                {
                    Console.WriteLine("You can drive a car");
                }
                else
                {
                    Console.WriteLine("You need a driving license");
                }
            }
            else
            {
                Console.WriteLine("You are too young to drive");
            }
            
            // If inside switch
            Console.WriteLine("\n--- If Inside Switch ---");
            int userType = 1;
            int balance = 500;
            
            switch (userType)
            {
                case 1:
                    Console.WriteLine("Regular User");
                    if (balance >= 100)
                    {
                        Console.WriteLine("Sufficient balance for transaction");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient balance");
                    }
                    break;
                case 2:
                    Console.WriteLine("Premium User");
                    if (balance >= 50)
                    {
                        Console.WriteLine("Sufficient balance for transaction");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient balance");
                    }
                    break;
                default:
                    Console.WriteLine("Unknown user type");
                    break;
            }
            
            // Multiple nested conditions
            Console.WriteLine("\n--- Multiple Nested Conditions ---");
            int income = 80000;
            int creditScore = 750;
            int employmentYears = 3;
            
            if (income >= 50000)
            {
                if (creditScore >= 700)
                {
                    if (employmentYears >= 2)
                    {
                        Console.WriteLine("Approved for loan with best terms");
                    }
                    else
                    {
                        Console.WriteLine("Approved for loan with standard terms");
                    }
                }
                else
                {
                    Console.WriteLine("Denied: Low credit score");
                }
            }
            else
            {
                Console.WriteLine("Denied: Insufficient income");
            }
        }
    }
}
