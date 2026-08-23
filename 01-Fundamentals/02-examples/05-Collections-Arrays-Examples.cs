/*
 * Collections and Arrays Examples
 * Demonstrating arrays, lists, and other collections
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp.Fundamentals.Collections
{
    class CollectionsArraysExamples
    {
        static void Main()
        {
            Console.WriteLine("=== ARRAYS ===\n");
            ArraysDemo();
            
            Console.WriteLine("\n=== LISTS ===\n");
            ListsDemo();
            
            Console.WriteLine("\n=== DICTIONARIES ===\n");
            DictionariesDemo();
            
            Console.WriteLine("\n=== OTHER COLLECTIONS ===\n");
            OtherCollectionsDemo();
        }
        
        static void ArraysDemo()
        {
            Console.WriteLine("--- Single-Dimensional Array ---");
            int[] numbers = new int[5];
            numbers[0] = 10;
            numbers[1] = 20;
            numbers[2] = 30;
            numbers[3] = 40;
            numbers[4] = 50;
            
            Console.WriteLine("Array elements:");
            foreach (int num in numbers)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
            
            // Array initialization shorthand
            Console.WriteLine("\n--- Array Initialization Shorthand ---");
            int[] scores = { 85, 90, 78, 95, 88 };
            Console.WriteLine($"Scores: {string.Join(", ", scores)}");
            
            // Multi-dimensional array
            Console.WriteLine("\n--- Multi-Dimensional Array (2D) ---");
            int[,] matrix = new int[3, 3]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };
            
            Console.WriteLine("Matrix:");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            
            // Jagged array
            Console.WriteLine("\n--- Jagged Array ---");
            int[][] jaggedArray = new int[3][];
            jaggedArray[0] = new int[2];
            jaggedArray[1] = new int[3];
            jaggedArray[2] = new int[4];
            
            // Fill with values
            int value = 1;
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                for (int j = 0; j < jaggedArray[i].Length; j++)
                {
                    jaggedArray[i][j] = value++;
                }
            }
            
            Console.WriteLine("Jagged array:");
            foreach (int[] row in jaggedArray)
            {
                Console.WriteLine(string.Join(", ", row));
            }
            
            // Array methods
            Console.WriteLine("\n--- Array Methods ---");
            int[] nums = { 5, 2, 8, 1, 9, 3 };
            Console.WriteLine($"Original: {string.Join(", ", nums)}");
            
            Array.Sort(nums);
            Console.WriteLine($"After Sort: {string.Join(", ", nums)}");
            
            Array.Reverse(nums);
            Console.WriteLine($"After Reverse: {string.Join(", ", nums)}");
            
            Console.WriteLine($"Array.IndexOf(nums, 8): {Array.IndexOf(nums, 8)}");
            Console.WriteLine($"Array.Length: {nums.Length}");
        }
        
        static void ListsDemo()
        {
            Console.WriteLine("--- List Basics ---");
            List<int> numbers = new List<int>();
            numbers.Add(10);
            numbers.Add(20);
            numbers.Add(30);
            numbers.AddRange(new int[] { 40, 50 });
            
            Console.WriteLine($"List: {string.Join(", ", numbers)}");
            Console.WriteLine($"Count: {numbers.Count}");
            Console.WriteLine($"First element: {numbers[0]}");
            
            Console.WriteLine("\n--- List Operations ---");
            numbers.Insert(2, 25);  // Insert at index 2
            Console.WriteLine($"After Insert(2, 25): {string.Join(", ", numbers)}");
            
            numbers.Remove(25);  // Remove first occurrence
            Console.WriteLine($"After Remove(25): {string.Join(", ", numbers)}");
            
            numbers.RemoveAt(1);  // Remove at index
            Console.WriteLine($"After RemoveAt(1): {string.Join(", ", numbers)}");
            
            // List of strings
            Console.WriteLine("\n--- List of Strings ---");
            List<string> fruits = new List<string> { "Apple", "Banana", "Orange", "Mango" };
            Console.WriteLine($"Fruits: {string.Join(", ", fruits)}");
            
            foreach (string fruit in fruits)
            {
                Console.WriteLine($"  - {fruit}");
            }
            
            // LINQ operations on List
            Console.WriteLine("\n--- LINQ on List ---");
            List<int> nums = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            var evens = nums.Where(n => n % 2 == 0).ToList();
            Console.WriteLine($"Even numbers: {string.Join(", ", evens)}");
            
            var doubled = nums.Select(n => n * 2).ToList();
            Console.WriteLine($"Doubled: {string.Join(", ", doubled)}");
            
            int sum = nums.Sum();
            int max = nums.Max();
            int min = nums.Min();
            Console.WriteLine($"Sum: {sum}, Max: {max}, Min: {min}");
        }
        
        static void DictionariesDemo()
        {
            Console.WriteLine("--- Dictionary Basics ---");
            Dictionary<string, int> ages = new Dictionary<string, int>();
            ages.Add("John", 30);
            ages.Add("Jane", 25);
            ages.Add("Bob", 35);
            ages["Alice"] = 28;  // Alternative syntax
            
            Console.WriteLine("Dictionary:");
            foreach (var kvp in ages)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            
            Console.WriteLine($"\nJohn's age: {ages["John"]}");
            
            // Check if key exists
            Console.WriteLine("\n--- Dictionary Operations ---");
            if (ages.ContainsKey("Jane"))
            {
                Console.WriteLine("Jane is in the dictionary");
            }
            
            Console.WriteLine($"Count: {ages.Count}");
            
            // Try to get value safely
            if (ages.TryGetValue("Bob", out int age))
            {
                Console.WriteLine($"Bob's age: {age}");
            }
            
            // Remove
            ages.Remove("Alice");
            Console.WriteLine("Alice removed");
            
            // Get keys and values
            Console.WriteLine("\nAll names: " + string.Join(", ", ages.Keys));
            Console.WriteLine("All ages: " + string.Join(", ", ages.Values));
            
            // Dictionary with custom objects
            Console.WriteLine("\n--- Dictionary with Custom Key/Value ---");
            Dictionary<int, string> statusCodes = new Dictionary<int, string>
            {
                { 200, "OK" },
                { 404, "Not Found" },
                { 500, "Internal Server Error" },
                { 403, "Forbidden" }
            };
            
            foreach (var code in statusCodes)
            {
                Console.WriteLine($"  {code.Key}: {code.Value}");
            }
        }
        
        static void OtherCollectionsDemo()
        {
            Console.WriteLine("--- HashSet ---");
            HashSet<int> uniqueNumbers = new HashSet<int> { 1, 2, 3, 2, 4, 3, 5 };
            Console.WriteLine($"Unique numbers: {string.Join(", ", uniqueNumbers)}");
            Console.WriteLine($"Count: {uniqueNumbers.Count}");
            
            Console.WriteLine("\n--- Queue ---");
            Queue<string> queue = new Queue<string>();
            queue.Enqueue("First");
            queue.Enqueue("Second");
            queue.Enqueue("Third");
            
            Console.WriteLine("Queue (FIFO):");
            while (queue.Count > 0)
            {
                Console.WriteLine($"  Dequeue: {queue.Dequeue()}");
            }
            
            Console.WriteLine("\n--- Stack ---");
            Stack<string> stack = new Stack<string>();
            stack.Push("First");
            stack.Push("Second");
            stack.Push("Third");
            
            Console.WriteLine("Stack (LIFO):");
            while (stack.Count > 0)
            {
                Console.WriteLine($"  Pop: {stack.Pop()}");
            }
            
            Console.WriteLine("\n--- Tuple ---");
            var person = ("John", 30, "Engineer");
            Console.WriteLine($"Tuple: {person}");
            Console.WriteLine($"Name: {person.Item1}, Age: {person.Item2}, Job: {person.Item3}");
            
            // Named tuples
            var employee = (Name: "Jane", Age: 28, Department: "HR");
            Console.WriteLine($"\nNamed Tuple: Name={employee.Name}, Age={employee.Age}, Dept={employee.Department}");
        }
    }
}
