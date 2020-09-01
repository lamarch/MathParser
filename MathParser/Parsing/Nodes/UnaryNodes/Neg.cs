using System;
using System.Collections.Generic;
using System.Text;

namespace MathParser.Parsing.Nodes.UnaryNodes
{
    class Neg : Unary
    {
        private static readonly Func<double, double> op = a => -a;
        public Neg (int pos, Expression leaf) : base(pos, op, leaf)
        {
        }

        public override string ToString ( ) => "-";
    }
}
