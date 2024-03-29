﻿
using System.IO;

using MathParser.Execution;
using MathParser.Logging;
using MathParser.Parsing;
using MathParser.Parsing.Nodes;
using MathParser.Tokenisation;
using MathParser.Tokenisation.Unused;

namespace MathParser
{
    public class Compiler
    {

        public Compiler ( )
        {
        }

        public Result<Expression> Compile (string code)
        {
            Parser parser = new Parser(new Lexer(new StringReader(code)));
            
            var parseResult = parser.Parse();

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
