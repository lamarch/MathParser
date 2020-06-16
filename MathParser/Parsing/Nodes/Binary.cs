using System;

using MathParser.Execution;

namespace MathParser.Parsing.Nodes
{
    public class Binary : Expression
    {
        private readonly Func<double, double, double> op;
        private readonly Expression lhs;
        private readonly Expression rhs;

        public Binary (int pos, Func<double, double, double> op, Expression lhs, Expression rhs) : base(pos)
        {
            this.op = op ?? throw new ArgumentNullException(nameof(op));
            this.lhs = lhs ?? throw new ArgumentNullException(nameof(lhs));
            this.rhs = rhs ?? throw new ArgumentNullException(nameof(rhs));
        }

        public override double Eval (IContext ctx) => this.op(this.lhs.Eval(ctx), this.rhs.Eval(ctx));
    }
}
