﻿using MathParser.Execution;

namespace MathParser.Parsing.Nodes
{
    public abstract class Expression
    {
        public int Position { get; private set; }

        public Expression (int pos)
        {
            Position = pos;
        }

        public abstract double Eval (IContext ctx);
    }
}
