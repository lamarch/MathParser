using System;
using System.Collections.Generic;

using MathParser.Utilities;

namespace MathParser.Execution
{
    public class Property : Callable
    {
        private readonly Func<IContext, Result<double>> action;
        private double value;
        private bool isInit = false;


        public Property (string name, double d) : this(name, _ => d)
        {
        }

        public Property (string name, Func<IContext, Result<double>> action) : base(name)
        {
            this.action = action;
        }

        public override Result<double> Call (IContext context) => isInit ? new Result<double>(value) : Init(context);
        public override List<string> GetArgs ( ) => new List<string>();
        public override string GetHash ( ) => Helper.Hash(Name + -1);

        public override Result<double> Init (IContext context)
        {
            var result = action(context);
            value = result.Value;
            if ( !result.HasErrors )
                isInit = true;
            return result;
        }
    }
}
