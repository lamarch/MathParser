using System.Collections.Generic;

namespace MathParser.Execution.Injection
{
    public interface ILoader
    {
        Result<List<Property>> GetProperties ( );
        Result<List<Function>> GetFunctions ( );
    }
}
