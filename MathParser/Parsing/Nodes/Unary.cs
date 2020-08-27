using System;

using MathParser.Execution;

namespace MathParser.Parsing.Nodes
{
    public class Unary : Expression
    {
        private readonly Func<double, double> op;
        private readonly Expression leaf;

        public Unary (int pos, Func<double, double> op, Expression leaf) : base(pos)
        {
            this.op = op ?? throw new ArgumentNullException(nameof(op));
            this.leaf = leaf ?? throw new ArgumentNullException(nameof(leaf));
        }

        public override Result<double> Eval (IContext ctx)
        {
            var leafResult = leaf.Eval(ctx);

            if ( leafResult.HasErrors )
                return leafResult.SetErrorsPosition(Position);
            return this.op(leafResult.Value);
        }
    }
}
