using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace MathParser.Tokenisation
{
    public class Lexer
    {
        private Tokenisator reader;

        public void Lex (TextReader textReader)
        {
            reader = new Tokenisator(textReader);
            Next();
        }

        public void Next ( )
        {
            Current = reader.NextToken();
            if (Current.Token == Token.Error)
            {
                Errors.Add(ErrorCodes.UNKNOWN_CHAR(Current.Position));
            }

        }

        public List<Error> Errors { get; private set; } = new List<Error>();
        public Symbol Current { get; private set; }
    }
}
