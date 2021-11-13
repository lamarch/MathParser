namespace MathParser.Tokenisation
{
    public readonly struct Symbol
    {

        private Symbol (Token token, double value, string id, int position)
        {
            Token = token;
            Value = value;
            Position = position;
            Id = id;
        }

        public readonly Token Token;
        public readonly int Position;
        public readonly double Value;
        public readonly string Id;

        public static Symbol EOF(int position) => new Symbol(Token.EOF, 0, null, position);
        public static Symbol Error(int position) => new Symbol(Token.Error, 0, null, position);
        public static Symbol Identifier(string identifier, int position) => new Symbol (Token.Identifier, 0, identifier, position);
        public static Symbol Number(double value, int position) => new Symbol(Token.Number, value, null, position);
        public static Symbol Sign(Token token, int position) => new Symbol(token, 0, null, position);
    }
}
