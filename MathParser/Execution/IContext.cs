using System.Collections.Generic;

using MathParser.Parsing.Nodes;

namespace MathParser.Execution
{
    public interface IContext
    {
        Result<double> ResolveProp (string id);
        Result<double> ResolveFunction (string id, List<double> args);
    }
}
