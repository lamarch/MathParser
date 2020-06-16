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

        public Token Token { get => this.token; set => this.token = value; }
        public double Value { get => this.value; set => this.value = value; }
        public int Position { get => this.position; set => this.position = value; }
        public string Id { get => this.id; set => this.id = value; }
    }
}
