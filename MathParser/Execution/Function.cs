using System;
using System.Collections.Generic;
using System.Linq;

using MathParser.Utilities;

namespace MathParser.Execution
{
    public class Function : Callable
    {
        private readonly Func<IContext, Result<double>> func;
        private readonly List<string> arguments;

        public Function (string name, Func<IContext, Result<double>> func, List<string> arguments) : base(name)
        {
            this.func = func;
            this.arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        public Function (string name, Func<IContext, Result<double>> func, params string[] args) : this(name, func, args.ToList()) { }

        public override Result<double> Call (IContext ctx) => func(ctx);
        public override List<string> GetArgs ( ) => arguments;
        public override string GetHash ( ) => Helper.Hash(Name + arguments.Count);
        public override Result<double> Init (IContext context) => 0.0d;
    }
}