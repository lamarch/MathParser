using System.Collections.Generic;

using MathParser.Tokenisation;

namespace MathParser.Parsing
{
    public class SymbolStream
    {
        private readonly Queue<Symbol> symbols;
        private int stuffAmount = 0;
        private Symbol EOFSymbol = new Symbol(Token.Null, -1, string.Empty, -1);

        public SymbolStream (Queue<Symbol> symbols)
        {
            this.symbols = symbols;
        }

        public Symbol Current {
            get {
                if ( symbols.Count > 0 ) {
                    var symbol = symbols.Peek();

                    //save the EOF symbol
                    //thereby we can restore it when queue is empty
                    if ( symbol.Token == Token.EOF )
                        EOFSymbol = symbol;

                    return symbol;
                }
                else {
                    return EOFSymbol;
                }
            }
        }

        public void Next ( )
        {
            if ( stuffAmount < 1 ) {
                symbols.Dequeue();
            }
            //somebody stuff the queue
            else {
                stuffAmount--;
            }

        }

        public void Stuff (int amount) => stuffAmount += amount;
    }
}
