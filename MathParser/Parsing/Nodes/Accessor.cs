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

        public override Result<double> Eval (IContext ctx) => ctx.ResolveProp(id).SetErrorsPosition(Position);


    }
}
