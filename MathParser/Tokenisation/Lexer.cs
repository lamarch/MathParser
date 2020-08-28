using System.Collections.Generic;
using System.IO;

namespace MathParser.Tokenisation
{
    public class Lexer
    {
        //TODO : bring together Lexer and SymbolStream
        public Result<Queue<Symbol>> Lex (string code)
        {
            Queue<Symbol> symbols = new Queue<Symbol>();
            List<Error> errors = new List<Error>();
            Tokenisator stream = new Tokenisator(new StringReader(code));
            Token tok = Token.Null;


            while ( tok != Token.EOF ) {

                try {
                    stream.NextToken();

                    tok = stream.CurrentToken;

                    symbols.Enqueue(GetSymbolFromToken(stream));
                }
                catch ( LexerException ) {
                    errors.Add(ErrorCodes.UNKNOWN_CHAR(stream.CurrentPosition));
                }

            }

            return new Result<Queue<Symbol>>(symbols, errors);
        }

        public Symbol GetSymbolFromToken (Tokenisator stream) => new Symbol(stream.CurrentToken, stream.Value, stream.Identifier, stream.CurrentPosition);
    }
}
