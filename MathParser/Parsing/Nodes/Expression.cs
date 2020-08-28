using System.Collections.Generic;

using IronPython.Runtime;

using MathParser.Execution;

namespace MathParser.Parsing.Nodes
{
    public abstract class Expression
    {
        public int Position { get; private set; }

        public Expression (int pos)
        {
            Position = pos;
        }

        public abstract Result<double> Eval (IContext ctx);

        public abstract override string ToString ( );
        public abstract List<Expression> GetChilds ( );
    }
}
