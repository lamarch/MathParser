using System.Collections.Generic;

using MathParser.Execution;

namespace MathParser.Parsing.Nodes
{
    public class Const : Expression
    {
        private readonly double value;

        public Const (int pos, double value) : base(pos)
        {
            this.value = value;
        }

        public override Result<double> Eval (IContext ctx) => value;
        public override List<Expression> GetChilds ( ) => null;
        public override string ToString ( ) => value.ToString();
    }
}
