namespace MathParser.Tokenisation
{
    public class Symbol
    {
        private Token token;
        private double value;
        private string id;
        private int position;

        public Symbol (Token token, double value, string id, int position)
        {
            Token = token;
            Value = value;
            Position = position;
            Id = id;
        }

        public Token Token { get => token; set => token = value; }
        public double Value { get => value; set => this.value = value; }
        public int Position { get => position; set => position = value; }
        public string Id { get => id; set => id = value; }
    }
}
