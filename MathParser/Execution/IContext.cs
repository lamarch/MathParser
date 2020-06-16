using System.Collections.Generic;

namespace MathParser.Execution
{
    public interface IContext
    {
        Result<double> ResolveProp (string id);
        Result<double> ResolveFunction (string id, List<double> args);

        List<Error> Errors { get; set; }
    }
}
