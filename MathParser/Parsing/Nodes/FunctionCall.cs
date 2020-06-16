using System.Linq;

using MathParser.Execution;

namespace MathParser.Parsing.Nodes
{
    public class FunctionCall : Expression
    {
        private readonly Expression[] args;
        private readonly string id;

        public FunctionCall (int pos, string id, Expression[] args) : base(pos)
        {
            this.args = args;
            this.id = id;
        }

        public override double Eval (IContext ctx)
        {
            var result = ctx.ResolveFunction(this.id, this.args.Select(a => a.Eval(ctx)).ToList());

            if ( result.HasErrors ) {
                result.SetErrorsPosition(Position);
                ctx.Errors.AddRange(result.Errors);
                return 0;
            }
            else {

                return result.Value;
            }

        }
    }
}
