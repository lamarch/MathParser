using System.Collections.Generic;
using System.Linq;

using MathParser.Parsing.Nodes;

namespace MathParser.Execution.Injection.Expressions
{
    public class ExpressionLoader : ILoader
    {

        private readonly List<Function> functions = new List<Function>();
        private readonly List<Property> properties = new List<Property>();

        public ExpressionLoader (Expression expression, string name, List<string> args)
        {
            LoadFunction(expression, name, args);

        }

        public ExpressionLoader (Expression expression, string name)
        {
            LoadProperty(expression, name);
        }

        public ExpressionLoader LoadFunction(Expression expression, string name, List<string> args)
        {
            functions.Add(new Function(name, ctx => expression.Eval(ctx), args));
            return this;
        }

        public ExpressionLoader LoadProperty (Expression expression, string name)
        {
            properties.Add(new Property(name, ctx => expression.Eval(ctx)));
            return this;
        }

        public Result<List<Function>> GetFunctions ( ) => functions;
        public Result<List<Property>> GetProperties ( ) => properties;
    }
}
