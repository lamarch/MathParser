using System;
using System.Collections.Generic;
using System.Text;

namespace MathParser.Execution.Injection
{
    public interface ILoader
    {
        Result<List<Property>> GetProperties ( );
        Result<List<Function>> GetFunctions ( );
    }
}
