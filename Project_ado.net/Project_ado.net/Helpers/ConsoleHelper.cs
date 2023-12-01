using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_ado.net.Helpers
{
    internal static class ConsoleHelper
    {
        public static void WriteLineError(string message)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"Error: {message}");

            Console.ForegroundColor = currentColor;
        }
        public static void WriteError(string message)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Write($"Error: {message}");

            Console.ForegroundColor = currentColor;
        }
        public static void WriteLineSuccess(string message)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(message);

            Console.ForegroundColor = currentColor;
        }
        public static void WriteSuccess(string message)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.Write(message);

            Console.ForegroundColor = currentColor;
        }
        public static int GetOptionInput()
        {
            int input;
            while (!int.TryParse(Console.ReadLine(), out input))
            {
                ClearCurrentConsoleLine();
                Console.WriteLine("Please, try again.");
            }
            return input;
        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

       
    }
}
