using System;
using System.Collections.Generic;
using System.Linq;

using MathParser;
using MathParser.Execution;
using MathParser.Execution.Injection;
using MathParser.Execution.Injection.Expressions;

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

        internal static ILoader Define (System.Collections.Generic.List<string> args)
        {
            //remove "define"
            args.RemoveAt(0);

            if(args.Count > 0 ) {
                if(args[0] == "function" ) {
                    Console.Write("name :");
                    string name = Console.ReadLine();

                    if ( MathParser.Utilities.Helper.IsValidName(name) ) {
                        Console.Write("\nparameters (separate by spaces) :");
                        List<string> parameters = Console.ReadLine().Split(' ').ToList();

                        foreach ( var p in parameters ) {
                            if ( !MathParser.Utilities.Helper.IsValidName(p) ) {
                                Console.WriteLine(name + " is not a valid name !");
                                return null;
                            }
                        }

                        Console.Write("expression : ");
                        string expression = Console.ReadLine();

                        var compilationResult = new Compiler().Compile(expression);

                        if ( compilationResult.HasErrors ) {
                            Util.PrintErrors(expression, compilationResult.Errors);
                        }
                        else {
                            Console.WriteLine("Function has been created succesfully !");
                            return new ExpressionLoader(compilationResult.Value, name, parameters);
                        }

                    }
                    else {
                        Console.WriteLine(name + " is not a valid name !");
                    }

                }else if(args[0] == "property" ) {
                    Console.Write("name :");
                    string name = Console.ReadLine();

                    if ( MathParser.Utilities.Helper.IsValidName(name) ) {

                        Console.Write("expression : ");
                        string expression = Console.ReadLine();

                        var compilationResult = new Compiler().Compile(expression);

                        if ( compilationResult.HasErrors ) {
                            Util.PrintErrors(expression, compilationResult.Errors);
                        }
                        else {
                            Console.WriteLine("Property has been created succesfully !");
                            return new ExpressionLoader(compilationResult.Value, name);
                        }

                    }
                    else {
                        Console.WriteLine(name + " is not a valid name !");
                    }
                }
                else {
                    Console.WriteLine("Unknown type, please chose between function and property.");
                }
            }
            else {
                Console.WriteLine("You need to specify a type to define (function or property) !");
            }

            return null;
        }
    }
}
