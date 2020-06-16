using System;
using System.Collections.Generic;

using MathParser.Execution;
using MathParser.Parsing;
using MathParser.Parsing.Nodes;
using MathParser.Tokenisation;
using MathParser.Utilities;
using Serilog;

namespace MathParser
{
    public class Compiler
    {
        private readonly Parser parser;
        private readonly Lexer lexer;


        public Compiler ( )
        {
            this.parser = new Parser();
            this.lexer = new Lexer();
        }



        public Result<Expression> Compile (string code)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            var lexResult = this.lexer.Lex(code);

            if ( lexResult.Errors.Count > 0 ) {
                return new Result<Expression>(null, lexResult.Errors);
            }


            var parseResult = parser.Parse(new SymbolStream(lexResult.Value));

            return parseResult;
        }

        public Result<double> Evaluate (string code) => Evaluate(code, new ExecutionContext(new Segment()));

        public Result<double> Evaluate (string code, IContext context)
        {

            var preResult = Compile(code);

            var evalValue = preResult.Value?.Eval(context);

            var errors = new List<Error>();

            errors.AddRange(preResult.Errors);
            errors.AddRange(context.Errors);

            return new Result<double>(evalValue ?? 0, errors);
        }
    }
}
