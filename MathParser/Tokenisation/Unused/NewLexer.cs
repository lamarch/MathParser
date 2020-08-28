using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MathParser.Tokenisation.Unused
{
    public class NewLexer
    {
        private int stuffAmount = 0;
        private volatile bool reachEnd = false;
        private ConcurrentQueue<Symbol> symbols = new ConcurrentQueue<Symbol>();

        public void Lex(TextReader textReader)
        {
            stuffAmount = 0;
            reachEnd = false;
            symbols = new ConcurrentQueue<Symbol>();
            Task.Run(( ) => Tokenize(new Tokenisator(textReader)));
        }

        public void Next (  )
        {
            if ( reachEnd && symbols.IsEmpty)
                return;

            if ( stuffAmount < 1 ) {

                //wait for some symbols in the queue
                while ( symbols.IsEmpty ) {
                    Thread.Sleep(5);
                }

                Symbol symbol;

                while (!symbols.TryDequeue(out symbol) ) {
                    Thread.Sleep(5);
                }

                Current = symbol;

            }
            //somebody stuff the queue
            else {
                stuffAmount--;
            }

        }

        public ConcurrentBag<Error> Errors { get; private set; } = new ConcurrentBag<Error>();
        public Symbol Current { get; private set; }


        private void Tokenize(Tokenisator reader)
        {
            Token tok = Token.Null;

            while ( tok != Token.EOF ) {

                try {
                    reader.NextToken();

                    tok = reader.CurrentToken;

                    symbols.Enqueue(GetSymbolFromToken(reader));
                }
                catch ( LexerException ) {
                    Errors.Add(ErrorCodes.UNKNOWN_CHAR(reader.CurrentPosition));
                }

            }

            reachEnd = true;
        }

        private Symbol GetSymbolFromToken (Tokenisator stream) => new Symbol(stream.CurrentToken, stream.Value, stream.Identifier, stream.CurrentPosition);
        public void Stuff (int amount) => stuffAmount += amount;

    }
}
