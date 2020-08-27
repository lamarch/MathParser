using System;

using MathParser.Execution;

namespace FastMaths
{
    internal static class CommandManager
    {
        internal static void ShowHelp (Segment segment)
        {
            Console.WriteLine("\nHere are all functions available in global segment :");


            int i = 1;
            foreach ( var fct in segment.Functions ) {
                Console.WriteLine($"{i++} - {fct.Name}");
            }

            Console.WriteLine("\nHere are all properties available in global segment :");

            i = 1;
            foreach ( var fct in segment.Properties ) {
                Console.WriteLine($"{i++} - {fct.Name}");
            }
        }

        internal static void Clear ( )
        {
            Console.Clear();
            Program.Greetings();
        }

        internal static void Define (System.Collections.Generic.List<string> args)
        {
            //remove "define"
            args.RemoveAt(0);

            if(args.Count > 0 ) {
                if(args[0] == "function" ) {

                }else if(args[0] == "property" ) {

                }
                else {

                }
            }
            else {
                Console.WriteLine("You need to specify a type to define !");
            }
        }
    }
}
