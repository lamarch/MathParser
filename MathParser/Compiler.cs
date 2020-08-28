
using System.IO;

using MathParser.Execution;
using MathParser.Logging;
using MathParser.Parsing;
using MathParser.Parsing.Nodes;
using MathParser.Tokenisation;

namespace MathParser
{
    public class Compiler
    {
        private readonly Parser parser;
        private readonly Lexer lexer;


        public Compiler ( )
        {
            parser = new Parser();
            lexer = new Lexer();

        }


        public Result<Expression> Compile (string code)
        {

            var lexResult = lexer.Lex(code);

            if ( lexResult.Errors.Count > 0 ) {
                return new Result<Expression>(null, lexResult.Errors);
            }

            
            var parseResult = parser.Parse(new SymbolStream(lexResult.Value));

            return parseResult;
        }

        public Result<double> Evaluate (string code) => Evaluate(code, new ExecutionContext(new Segment()));

        public Result<double> Evaluate (string code, IContext context)
        {

            var compilationResult = Compile(code);

            if ( compilationResult.HasErrors )
                return new Result<double>(0, compilationResult.Errors);

            Expression root = compilationResult.Value;

            var evaluationResult = root.Eval(context);

            return evaluationResult;
        }
    }
}
