namespace MathParser.Parsing
{
    using MathParser.Tokenisation;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ParsingContext
    {
        public readonly Lexer Lexer;
        public readonly List<Error> Errors;
    }
}
