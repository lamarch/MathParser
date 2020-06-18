using System;

namespace MathParser.Execution.Injection.CSharp
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
    public class MathImplAttribute : Attribute { }
}
