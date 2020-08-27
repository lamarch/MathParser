using System.Collections.Generic;
using System.Linq;

using MathParser.Execution;

namespace MathParser.Parsing.Nodes
{
    public class FunctionCall : Expression
    {
        private readonly List<Expression> args;
        private readonly string id;

        public FunctionCall (int pos, string id, List<Expression> args) : base(pos)
        {
            this.args = args;
            this.id = id;
        }

        public override Result<double> Eval (IContext ctx)
        {
            List<Result<double>> values = args.Select(a => a.Eval(ctx)).ToList();

            if ( values.Any(v => v.HasErrors) ) {
                return values.Aggregate((f1, f2) => f1.Merge(f2, (a, b) => 0));
            }
            return ctx.ResolveFunction(id, values.Select(r => r.Value).ToList()).SetErrorsPosition(Position);
        }
    }
}
