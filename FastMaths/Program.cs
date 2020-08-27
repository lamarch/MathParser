using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

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
        private static readonly SegmentInjector injector = new SegmentInjector(global);

        private const char MetaChar = '/';
        private const string prompt = ">";

        private static bool debug = true;

        private static void Main ( )
        {
            compiler.LogReceiver.OnLog += log => {
                if ( debug ) {
                    var pre = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Magenta;

                    Console.WriteLine($"[{log.PostedAt}, {log.LogLevel}] {string.Format("{0,-15}", log.From)} : {log.Message} ");
                }
            };

            Greetings();

            Console.WriteLine("Chargement des données...\n");

            //Load global functions & property

            #region Globals
            var result = InitGlobals();

            if ( result.HasErrors ) {

                Console.WriteLine("Des erreurs sont survenues :\n");

                for ( int i = 0; i < result.Errors.Count; i++ ) {
                    Util.PrintError(result.Errors[i], i + 1);
                }
            }
            else {

                Console.WriteLine("Additions chargés avec succès :\n");

                for ( int i = 0; i < result.Value.Count; i++ ) {
                    Console.WriteLine($"{i + 1:D2} - {result.Value[i]}");
                }
            }


            #endregion

            while ( true ) {
                Console.Write(prompt);
                string command = Console.ReadLine();

                if ( command.StartsWith(MetaChar) ) {
                    ParseMeta(command);
                }
                else {
                    ParseMaths(command);
                }
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
                    case "define":
                        var loader = CommandManager.Define(args);
                        if(!(loader is null) ) {
                            var result = injector.Load(loader);
                            if ( result.HasErrors ) {
                                Util.PrintErrors(result.Errors);
                            }
                        }
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
                Util.PrintErrors(command, result.Errors);
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


            var assemblyLoad = injector.Load(new CSharpLoader(typeof(MathAddons).Assembly, "cs_"));

            result.Merge(assemblyLoad, (r, list) => list.Concat(r).ToList());

            var mathsClassLoad = injector.Load(new CSharpLoader(typeof(Math), "lib_"));

            result.Merge(mathsClassLoad, (r, list) => list.Concat(r).ToList());

            var exprLoad = injector.Load(new ExpressionLoader(new Compiler().Compile("a+b*2").Value, "test", new List<string>() { "b", "a" }));

            result.MergeIf(exprLoad, (r, list) => list.AddRange(r));

            var pythonLoad = injector.Load(new PythonLoader("", "pytest_", "def a():\n\treturn 13.5\n"));

            result.Merge(pythonLoad, (r, list) => list.Concat(r).ToList());

            return result;
        }

        public static void Greetings ( ) => Console.WriteLine($"Bienvenue dans {Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version} (API-version {typeof(Compiler).Assembly.GetName().Version}) !\n");


    }
}
