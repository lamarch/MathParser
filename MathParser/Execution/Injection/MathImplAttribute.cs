using System;

namespace MathParser.Execution.Injection
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
    internal class MathImplAttribute : Attribute
    {
    }
}
