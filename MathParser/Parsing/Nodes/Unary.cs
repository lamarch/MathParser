using System;

using MathParser.Execution;

namespace MathParser.Parsing.Nodes
{
    public class Unary : Expression
    {
        private readonly Func<double, double> op;
        private readonly Expression lhs;

        public Unary (int pos, Func<double, double> op, Expression leaf) : base(pos)
        {
            this.op = op ?? throw new ArgumentNullException(nameof(op));
            this.lhs = leaf ?? throw new ArgumentNullException(nameof(leaf));
        }

        public override double Eval (IContext ctx) => this.op(this.lhs.Eval(ctx));
    }
}
