using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using MathParser;
using MathParser.Parsing;
using MathParser.Tokenisation;
using MathParser.Tokenisation.Unused;

namespace Benchmarking
{
    internal static class Program
    {
        private const int count = 10000;
        private const int codeLength = 10;
        private const string code = "1+2+3+4+5+6+";

        private static string final;

        private static void Main (string[] args)
        {
            StringBuilder sb = new StringBuilder();

            for ( int i = 0; i < codeLength; i++ ) {
                sb.Append(code);
            }

            sb.Remove(sb.Length - 1, 1);

            final = sb.ToString();

            Stopwatch sp = new Stopwatch();
            Console.WriteLine("old:");

            sp.Start();

            for ( int i = 0; i < count; i++ ) {
                //Old();
            }

            sp.Stop();

            Console.WriteLine(sp.Elapsed);
            /*
            Console.WriteLine("new:");
            sp.Restart();


            for ( int i = 0; i < count; i++ ) {
                New();
            }


            Console.WriteLine(sp.Elapsed);

            Console.Read();
            sp.Stop();
            */

            Console.ReadLine();
        }

        private static void New ( )
        {
            Lexer nl = new Lexer(new StringReader(final));

            Symbol s;

            do {
                nl.Next();
                s = nl.Current;
            } while ( s.Token != Token.EOF );
        }
    }
}
