using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using IronPython.Hosting;
using MathParser;
using MathParser.Addons;
using MathParser.Execution;
using MathParser.Execution.Injection;
using MathParser.Execution.Injection.CSharp;
using MathParser.Execution.Injection.Expressions;
using MathParser.Execution.Injection.Python;

namespace FastMaths
{
    internal static class Program
    {
        private static readonly Compiler compiler = new Compiler();
        private static readonly Segment global = new Segment();

        private const char MetaChar = '/';
        private const string prompt = ">";

        private static bool debug = false;

        private static void Main ( )
        {

            Greetings();

            Console.WriteLine("Chargement des données...\n");

            var result = InitGlobals();

            if ( result.HasErrors )
                Console.WriteLine("Des erreurs sont survenues :\n");
            else
                Console.WriteLine("Aucune erreur a déplorer !\n");

            for ( int i = 0; i < result.Errors.Count; i++ ) {
                PrintError(result.Errors[i], i + 1);
            }

            Console.WriteLine("Additions chargés avec succès :\n");

            for ( int i = 0; i < result.Value.Count; i++ ) {
                Console.WriteLine($"{i + 1} - {result.Value[i]}");
            }


            Separate();


            while ( true ) {
                Console.Write(prompt);
                string command = Console.ReadLine();

                if ( command.StartsWith(MetaChar) ) {
                    ParseMeta(command);
                }
                else {
                    ParseMaths(command);
                }

                Separate();

            }
        }

        private static void ParseMeta (string command)
        {
            command = command.TrimStart(MetaChar);

            List<string> args = command.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            if ( args.Count > 0 ) {
                switch ( args[0] ) {
                    case "help":
                        CommandManager.ShowHelp(global);

                        Console.WriteLine("\nCommandes disponibles :");
                        Console.WriteLine("help -> affiche ce message");
                        Console.WriteLine("clear -> nettoie la console");
                        Console.WriteLine("pload -> charge un script python");
                        Console.WriteLine("csload -> charge un script csharp");

                        break;
                    case "clear":
                        CommandManager.Clear();
                        break;
                    case "pload":
                        Console.WriteLine("Cette fonction n'est pas encore disponible.");
                        break;
                    case "csload":
                        Console.WriteLine("Cette fonction n'est pas encore disponible.");
                        break;
                    case "debug":
                        debug = !debug;
                        Console.WriteLine("Debug : " + debug);
                        break;
                    default:
                        Console.WriteLine("Command inconnue.");
                        break;
                }
            }
        }

        private static void ParseMaths (string command)
        {
            var result = compiler.Evaluate(command, new ExecutionContext(global));

            if ( result.HasErrors ) {

                for ( int i = 0; i < result.Errors.Count; i++ ) {

                    Error error = result.Errors[i];

                    if ( i != 0 )
                        Console.WriteLine("\n" + prompt + command);

                    Console.ForegroundColor = ConsoleColor.White;
                    for ( int x = 1; x < error.Position - 1 + prompt.Length; x++ ) {
                        Console.Write('~');
                    }
                    Console.WriteLine("^\n");
                    Console.ResetColor();


                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Erreur n°{i + 1} : {error.Message} [{error.Source}].");
                    Console.ResetColor();

                }

            }
            else {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("Resultat : " + result.Value.ToString(CultureInfo.CurrentCulture));
                Console.ResetColor();

            }

        }

        private static Result<List<Callable>> InitGlobals ( )
        {
            var result = new Result<List<Callable>>(new List<Callable>());

            SegmentInjector injector = new SegmentInjector(global);


            var assemblyLoad = injector.Load(new CSharpLoader(typeof(MathAddons).Assembly, "in_"));

            result.Merge(assemblyLoad, (r, list) => list.Concat(r).ToList());

            var mathsClassLoad = injector.Load(new CSharpLoader(typeof(Math), "lib_"));

            result.Merge(mathsClassLoad, (r, list) => list.Concat(r).ToList());

            var exprLoad = injector.Load(new ExpressionLoader(new Compiler().Compile("a+b*2").Value, "test", "b", "a"));

            result.MergeIf(exprLoad, (r, list) => list.AddRange(r));

            var pythonLoad = injector.Load(new PythonLoader("", "pytest_", "def a():\n\treturn 13.5\n"));

            result.Merge(pythonLoad, (r, list) => list.Concat(r).ToList());

            return result;
        }

        public static void Greetings ( ) => Console.WriteLine($"Bienvenue dans {Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version} (API-version {typeof(Compiler).Assembly.GetName().Version}) !\n");

        private static void Separate ( )
        {
            StringBuilder bl = new StringBuilder();
            Console.WriteLine();

            for ( int i = 0; i < Console.BufferWidth - 1; i++ ) {
                bl.Append('-');
            }

            Console.WriteLine(bl.ToString());
            Console.WriteLine();

            bl.Clear();
        }

        public static void PrintError (Error error, int index)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Erreur n°{index} : {error}.");
            Console.ResetColor();
        }
    }
}
