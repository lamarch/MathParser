using System;
using System.Collections.Generic;
using System.Text;

using MathParser.Parsing.Nodes;

namespace MathParser.Parsing
{
    public abstract class ParsingSection
    {
        public abstract Expression Parse ( );
    }
}
