/*
 * Data Types Examples
 * Demonstrating various data types in C# with practical examples
 */

using System;

namespace CSharp.Fundamentals.DataTypes
{
    class DataTypesExamples
    {
        static void Main()
        {
            // Value Types - Stored on the Stack
            Console.WriteLine("=== VALUE TYPES ===\n");
            
            // Integer Types
            byte byteValue = 255;
            sbyte sbyteValue = -128;
            short shortValue = 32767;
            ushort ushortValue = 65535;
            int intValue = 2147483647;
            uint uintValue = 4294967295;
            long longValue = 9223372036854775807;
            ulong ulongValue = 18446744073709551615;
            
            Console.WriteLine($"Byte: {byteValue} (Range: 0 to 255)");
            Console.WriteLine($"Short: {shortValue} (Range: -32,768 to 32,767)");
            Console.WriteLine($"Int: {intValue} (Default integer type)");
            Console.WriteLine($"Long: {longValue}L (Suffix required)");
            
            // Floating Point Types
            Console.WriteLine("\n--- Floating Point Types ---");
            float floatValue = 3.14f;
            double doubleValue = 3.14159265359;
            decimal decimalValue = 3.141592653589793m;
            
            Console.WriteLine($"Float: {floatValue} (Single precision, 32-bit)");
            Console.WriteLine($"Double: {doubleValue} (Double precision, 64-bit)");
            Console.WriteLine($"Decimal: {decimalValue} (Precise decimal, used for financial)");
            
            // Boolean Type
            Console.WriteLine("\n--- Boolean Type ---");
            bool isActive = true;
            bool isComplete = false;
            
            Console.WriteLine($"Boolean true: {isActive}");
            Console.WriteLine($"Boolean false: {isComplete}");
            
            // Character Type
            Console.WriteLine("\n--- Character Type ---");
            char character = 'A';
            char digit = '5';
            char special = '@';
            
            Console.WriteLine($"Character: {character}");
            Console.WriteLine($"Digit char: {digit}");
            Console.WriteLine($"Special char: {special}");
            
            // Reference Types - Stored on the Heap
            Console.WriteLine("\n\n=== REFERENCE TYPES ===\n");
            
            // String Type
            string stringValue = "Hello, C#!";
            string multiLine = "Line 1\nLine 2\nLine 3";
            string verbatim = @"C:\Users\Documents\Files";
            
            Console.WriteLine($"String: {stringValue}");
            Console.WriteLine($"Multiline: {multiLine}");
            Console.WriteLine($"Verbatim: {verbatim}");
            
            // Object Type (base type for all types)
            Console.WriteLine("\n--- Object Type ---");
            object obj1 = 100;
            object obj2 = "String in object";
            object obj3 = 3.14;
            
            Console.WriteLine($"Object with int: {obj1}");
            Console.WriteLine($"Object with string: {obj2}");
            Console.WriteLine($"Object with double: {obj3}");
            
            // Dynamic Type (resolved at runtime)
            Console.WriteLine("\n--- Dynamic Type ---");
            dynamic dynamicValue = 42;
            Console.WriteLine($"Dynamic int: {dynamicValue}");
            dynamicValue = "Now it's a string";
            Console.WriteLine($"Dynamic string: {dynamicValue}");
            
            // Type Inference with var
            Console.WriteLine("\n--- Type Inference with var ---");
            var inferredInt = 100;
            var inferredString = "Inferred";
            var inferredDouble = 3.14;
            
            Console.WriteLine($"var inferredInt: {inferredInt} (Type: {inferredInt.GetType()})");
            Console.WriteLine($"var inferredString: {inferredString} (Type: {inferredString.GetType()})");
            Console.WriteLine($"var inferredDouble: {inferredDouble} (Type: {inferredDouble.GetType()})");
            
            // Type Conversion
            Console.WriteLine("\n\n=== TYPE CONVERSION ===\n");
            TypeConversionExamples();
        }
        
        static void TypeConversionExamples()
        {
            // Implicit Conversion (Smaller to Larger)
            Console.WriteLine("--- Implicit Conversion ---");
            int intValue = 100;
            long longValue = intValue;  // Implicit conversion
            double doubleValue = intValue;  // Implicit conversion
            
            Console.WriteLine($"Int to Long: {longValue}");
            Console.WriteLine($"Int to Double: {doubleValue}");
            
            // Explicit Conversion (Larger to Smaller or different type)
            Console.WriteLine("\n--- Explicit Conversion ---");
            double doubleVal = 123.45;
            int intVal = (int)doubleVal;  // Explicit cast - loses decimal part
            
            Console.WriteLine($"Double to Int (explicit): {intVal}");
            
            // String Conversion
            Console.WriteLine("\n--- String Conversion ---");
            int number = 123;
            string strFromInt = number.ToString();
            
            string strNumber = "456";
            int parsedInt = int.Parse(strNumber);
            
            Console.WriteLine($"Int to String: {strFromInt}");
            Console.WriteLine($"String to Int (Parse): {parsedInt}");
            
            // TryParse (Safe conversion)
            Console.WriteLine("\n--- Safe Conversion with TryParse ---");
            string validNumber = "789";
            string invalidNumber = "ABC";
            
            if (int.TryParse(validNumber, out int result1))
                Console.WriteLine($"Successfully parsed: {result1}");
            
            if (int.TryParse(invalidNumber, out int result2))
                Console.WriteLine($"Successfully parsed: {result2}");
            else
                Console.WriteLine($"Failed to parse: {invalidNumber}");
            
            // Convert class
            Console.WriteLine("\n--- Using Convert Class ---");
            string value = "255";
            byte convertedByte = Convert.ToByte(value);
            double convertedDouble = Convert.ToDouble(value);
            
            Console.WriteLine($"String to Byte: {convertedByte}");
            Console.WriteLine($"String to Double: {convertedDouble}");
        }
    }
}
