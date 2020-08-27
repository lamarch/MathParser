using System;
using System.Linq;

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

        public override Result<double> Eval (IContext ctx) 
        {
            var lhs_result = this.lhs.Eval(ctx);
            var rhs_result = this.rhs.Eval(ctx);

            var errors = lhs_result.Errors.Concat(rhs_result.Errors).ToList();

            if(errors.Count > 0 ) {
                return new Result<double>(0, errors);
            }

            return this.op(lhs_result.Value, rhs_result.Value); 
        }
    }
}
