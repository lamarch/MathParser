using System;
using System.Collections.Generic;
using System.Text;
using MathParser.Execution;

namespace FastMaths
{
    internal static class CommandManager
    {
        public static void ShowHelp(Segment segment)
        {
            Console.WriteLine("\nVoici toutes les fonctions disponibles dans le segment global :");


            int i = 1;
            foreach ( var fct in segment.Functions ) {
                Console.WriteLine($"{i++} - {fct.Name}");
            }

            Console.WriteLine("\nVoici toutes les propriétés disponibles dans le segment global :");

            i = 1;
            foreach ( var fct in segment.Properties ) {
                Console.WriteLine($"{i++} - {fct.Name}");
            }
        }

        public static void Clear ( )
        {
            Console.Clear();
            Program.Greetings();
        }
    }
}
