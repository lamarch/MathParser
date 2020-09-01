using System;

namespace MathParser.Parsing.Nodes.BinaryNodes
{
    public class Add : Binary
    {
        private static readonly Func<double, double, double> op = (a, b) => a + b;
        public Add (int pos, Expression lhs, Expression rhs) : base(pos, op, lhs, rhs)
        {
        }

        public override string ToString ( ) => "+";
    }
}
