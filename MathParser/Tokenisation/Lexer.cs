using System;
using System.Collections.Generic;
using System.IO;

using MathParser.Utilities;

namespace MathParser.Tokenisation
{
    public class Lexer
    {
        public Lexer ()
        {
        }

        public Result<List<Symbol>> Lex (string code)
        {
            List<Symbol> symbols = new List<Symbol>();
            List<Error> errors = new List<Error>();
            TokenStream stream = new TokenStream(new StringReader(code));
            Token tok = Token.Null;


            while ( tok != Token.EOF ) {

                try {
                    stream.NextToken();

                    tok = stream.CurrentToken;

                    symbols.Add(GetSymbolFromToken(stream));
                }
                catch ( LexerException ) {
                    errors.Add(ErrorCodes.UNKNOWN_CHAR(stream.CurrentPosition));
                }

            }

            return new Result<List<Symbol>>(symbols, errors);
        }

        public Symbol GetSymbolFromToken (TokenStream stream) => new Symbol(stream.CurrentToken, stream.Value, stream.Identifier, stream.CurrentPosition);
    }
}
