using System;
using System.IO;

using MathParser;
using MathParser.Tokenisation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void TestLexer ( )
        {
            TokenStream lex = new TokenStream(new StringReader("a 12 12,3 45 -+*/+"));

            Token tok = Token.EOF;

            do {
                lex.NextToken();
                tok = lex.CurrentToken;
            } while ( tok != Token.EOF );
        }
    }
}
