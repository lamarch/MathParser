using MathParser.Execution;

namespace MathParser.Parsing.Nodes
{
    public class Accessor : Expression
    {
        private readonly string id;

        public Accessor (int pos, string id) : base(pos)
        {
            this.id = id;
        }

        public override double Eval (IContext ctx)
        {
            var result = ctx.ResolveProp(this.id);

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
