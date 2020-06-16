using System.Collections.Generic;

using MathParser.Tokenisation;

namespace MathParser.Parsing
{
    public class SymbolStream
    {
        private readonly Queue<Symbol> symbols;
        private int stuffAmount = 0;
        private Symbol EOFSymbol = null;
        public SymbolStream (List<Symbol> symbols)
        {
            this.symbols = new Queue<Symbol>(symbols);
        }

        public Symbol Current {
            get {
                if(symbols.Count > 0 ) {
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

        public Symbol Next ( )
        {
            if ( this.stuffAmount < 1 ) {
                if ( symbols.Count > 0 )
                    return this.symbols.Dequeue();
                else
                    return Current;
            }
            //somebody stuff the queue
            else {
                this.stuffAmount--;
                return Current;
            }

        }

        public void Stuff (int amount) => this.stuffAmount += amount;
    }
}
