using System;
using System.Collections.Generic;
using System.Text;

using MathParser;

namespace FastMaths
{
    internal static class Util
    {
        public const string prompt = ">";
        public static void PrintErrors (string command, List<Error> errors)
        {
            for ( int i = 0; i < errors.Count; i++ ) {

                Error error = errors[i];

                if ( i != 0 )
                    Console.WriteLine("\n" + prompt + command);

                Console.ForegroundColor = ConsoleColor.White;
                for ( int x = 1; x < error.Position - 1 + prompt.Length; x++ ) {
                    Console.Write('~');
                }
                Console.WriteLine("^\n");
                Console.ResetColor();


                Console.ForegroundColor = ConsoleColor.Yellow;
                PrintError(error, i + 1);
                Console.ResetColor();

            }
        }

        public static void PrintErrors(List<Error> errors)
        {
            for ( int i = 0; i < errors.Count; i++ ) {
                PrintError(errors[i], i + 1);
            }
        }

        public static void PrintError (Error error, int index)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Erreur n°{index} : {error}.");
            Console.ResetColor();
        }
    }
}
