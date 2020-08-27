using System.Collections.Generic;

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
        private readonly Logger logger;
        public LogReceiver LogReceiver { get; private set; }

        public Compiler ( )
        {
            this.logger = new Logger();
            this.parser = new Parser(logger);
            this.lexer = new Lexer();

            LogReceiver = new LogReceiver(logger);
        }


        public Result<Expression> Compile (string code)
        {

            var lexResult = this.lexer.Lex(code);

            if ( lexResult.Errors.Count > 0 ) {
                return new Result<Expression>(null, lexResult.Errors);
            }


            var parseResult = this.parser.Parse(new SymbolStream(lexResult.Value));

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
