using System;
using System.Collections.Generic;
using System.Linq;

using IronPython.Compiler.Ast;

using MathParser.Execution;

namespace MathParser.Parsing.Nodes.BinaryNodes
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
            var lhs_result = lhs.Eval(ctx);
            var rhs_result = rhs.Eval(ctx);

            var errors = lhs_result.Errors.Concat(rhs_result.Errors).ToList();

            if ( errors.Count > 0 ) {
                return new Result<double>(0, errors);
            }

            return op(lhs_result.Value, rhs_result.Value);
        }

        public override List<Expression> GetChilds ( ) => new List<Expression>() { lhs, rhs };

        public override string ToString ( ) => string.Empty;
    }
}
