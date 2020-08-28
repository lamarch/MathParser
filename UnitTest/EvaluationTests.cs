using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using MathParser;
    using MathParser.Execution;
    using MathParser.Parsing;
    using MathParser.Parsing.Nodes;
    using MathParser.Tokenisation;
    using MathParser.Utilities;

    [TestClass]
    public class EvaluationTests
    {

        static EvaluationTests ( )
        {
            InitGlobals();
        }

        private static Segment Global = new Segment();
        private static Random random = new Random();

        private static void InitGlobals ( )
        {
            Global.AddProperty( new Property("pi", ctx => Math.PI));

            Global.AddFunction( new Function("v", ctx => {
                var n = ctx.ResolveProp("n");

                if ( n.HasErrors )
                    return new Result<double>(n.Errors);

                return Math.Sqrt(n.Value);
            }, "n"));

            Global.AddFunction( new Function("p", ctx => {
                var x = ctx.ResolveProp("_x");
                var y = ctx.ResolveProp("_y");

                if ( x.HasErrors || y.HasErrors )
                    return new Result<double>(new List<Error>().Concat(x.Errors).Concat(y.Errors).ToList());


                return Math.Pow(x.Value, y.Value);
            }, "_x", "_y"));

            Global.AddFunction( new Function("r", ctx => {
                var min = ctx.ResolveProp("min");
                var max = ctx.ResolveProp("max");

                if ( min.HasErrors || max.HasErrors )
                    return new Result<double>(new List<Error>().Concat(min.Errors).Concat(max.Errors).ToList());

                return random.NextDouble() % (max.Value - min.Value) + min.Value;
            }, "min", "max"));
        }

        public static Result<double> Eval (string code)
        {
            Compiler compiler = new Compiler();

            var comp = compiler.Compile(code);

            if ( comp.Errors.Count != 0 )
                return new Result<double>(double.NaN, comp.Errors);

            ExecutionContext ctx = new ExecutionContext(Global);

            return new Result<double>(comp.Value.Eval(ctx).Value, ctx.Errors);
        }

        [TestMethod]
        public void BinaryNodesTesting ( )
        {
            double leftVal = 10;
            double rightVal = 8.2;
            double finalVal = 18.2;

            Expression expr = new Binary(0, (left, right) => left + right, new Const(1, leftVal), new Const(2, rightVal));

            double result = expr.Eval(null).Value;

            Assert.AreEqual(finalVal, result);
        }

        [TestMethod]
        public void ParserTesting ( )
        {

            var r = Eval("1 + 2 + 5 -8 -1");
            Assert.IsTrue(r.Errors.Count == 0);


            Assert.AreEqual(-1, r.Value);
        }

        [TestMethod]
        public void ParserUnaryTesting ( )
        {

            var r = Eval("----+8");
            Assert.IsTrue(r.Errors.Count == 0);


            Assert.AreEqual(8, r.Value);
        }

        [TestMethod]
        public void ParserMultDiv ( )
        {

            var r = Eval("(8+2++2--2*3+1)%8");
            Assert.IsTrue(r.Errors.Count == 0);
            Assert.IsTrue(r.Errors.Count == 0);


            Assert.AreEqual(19%8, r.Value);
        }

        [TestMethod]
        public void ParserPar ( )
        {

            var r = Eval("(1+1)");
            Assert.IsTrue(r.Errors.Count == 0);

            Assert.AreEqual(2, r.Value);
        }

        [TestMethod]
        public void ParseExp ( )
        {

            var r = Eval("(-8)^-2*3");
            Assert.IsTrue(r.Errors.Count == 0);

            Assert.AreEqual(Math.Pow(-8,-2)*3, r.Value);
        }

        [TestMethod]
        public void ParserTotal ( )
        {

            var r = Eval("(8+9*3)/8--2");
            Assert.IsTrue(r.Errors.Count == 0);

            Assert.AreEqual(6.375, r.Value);
        }

        [TestMethod]
        public void ParserVariables ( )
        {

            var r = Eval("pi * 3");
            Assert.IsTrue(r.Errors.Count == 0);

            Assert.AreEqual((Math.PI * 3d), r.Value);
        }

        [TestMethod]
        public void ParserUltimate ( )
        {

            var r = Eval("(p(8+6*8-23/54.2, p(9, -89)))/v(8)");
            Assert.IsTrue(r.Errors.Count == 0);

            Assert.AreEqual(Math.Pow(8 + 6 * 8- 23 / 54.2, Math.Pow(9, -89)) / Math.Sqrt(8), r.Value);
        }

        [TestMethod]
        public void ParserFunction ( )
        {

            var r = Eval("3 * v(2)");

            Assert.IsTrue(r.Errors.Count == 0);

            Assert.AreEqual(3 * Math.Sqrt(2), r.Value);
        }

        [TestMethod]
        public void ParserLParAfterFactor ( )
        {

            var r = Eval("3(1+2)(8/2)");
            Assert.IsTrue(r.Errors.Count == 0);

            Assert.AreEqual(3 * (1 + 2) * (8 / 2), r.Value);
        }


    }
}
