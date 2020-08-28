namespace MathParser.Tokenisation
{
    public readonly struct Symbol
    {

        public Symbol (Token token, double value, string id, int position)
        {
            Token = token;
            Value = value;
            Position = position;
            Id = id;
        }

        public readonly Token Token;
        public readonly double Value;
        public readonly int Position;
        public readonly string Id;
    }
}
