using System;
using System.Collections.Generic;
using System.Text;

namespace MathParser.Parsing.Nodes.BinaryNodes
{
    public class Mod : Binary
    {
        private static readonly Func<double, double, double> op = (a,b) => a % b;
        public Mod (int pos, Expression lhs, Expression rhs) : base(pos, op, lhs, rhs)
        {
        }

        public override string ToString ( ) => "%";
    }
}
