using System.Collections.Generic;
using System.Linq;

using MathParser.Parsing.Nodes;

namespace MathParser.Execution.Injection.Expressions
{
    public class ExpressionLoader : ILoader
    {
        private readonly Expression expression;
        private readonly string name;
        private readonly List<string> args;

        public ExpressionLoader (Expression expression, string name, List<string> args)
        {
            this.expression = expression;
            this.name = name;
            this.args = args;
        }

        public ExpressionLoader (Expression expression, string name, params string[] args) : this(expression, name, args.ToList()) { }

        public Result<List<Function>> GetFunctions ( ) => new Result<List<Function>>(
            new List<Function>() { new Function(name, ctx => expression.Eval(ctx), args) });
        public Result<List<Property>> GetProperties ( ) => new Result<List<Property>>(new List<Property>());
    }
}
