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

            try {
                reader.NextToken();

                Current = GetSymbolFromToken(reader);
            }
            catch ( LexerException ) {
                Errors.Add(ErrorCodes.UNKNOWN_CHAR(reader.CurrentPosition));
            }

        }

        public List<Error> Errors { get; private set; } = new List<Error>();
        public Symbol Current { get; private set; }

        private Symbol GetSymbolFromToken (Tokenisator stream) => new Symbol(stream.CurrentToken, stream.Value, stream.Identifier, stream.CurrentPosition);

    }
}
