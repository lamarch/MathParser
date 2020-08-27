using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MathParser.Execution.Injection.CSharp
{
    public class CSharpLoader : Loader<MethodInfo>
    {
        private readonly List<Type> typesToLoad = new List<Type>();

        public CSharpLoader (Assembly assembly, string prefix) : base(prefix)
        {
            this.typesToLoad = assembly.GetTypes().ToList();
        }

        public CSharpLoader (Type type, string prefix) : base(prefix)
        {
            this.typesToLoad.Add(type);
        }

        public override Result<List<Property>> GetProperties ( ) => new List<Property>();

        //This call a function from her MethodInfo (funcType)
        protected override Result<double> CallFunction (MethodInfo funcType, List<double> args)
        {
            try {
                return (double)funcType.Invoke(null, args.Cast<object>().ToArray());
            }
            catch ( Exception e ) {
                return new Result<double>(ErrorCodes.FUNCTION_CALL(funcType.Name, "CSharp", e));
            }
        }

        protected override List<MethodInfo> GetFunctionsType ( ) => (from t in this.typesToLoad
                                                                     from m in t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)

                                                                     where m.ReturnType == typeof(double)
                                                                     where m.GetParameters().All(p => p.ParameterType == typeof(double))

                                                                     select m).ToList();

        protected override List<string> GetParameters (MethodInfo funcType) => funcType.GetParameters().Select(p => ParameterPrefix() + p.Name).ToList();

        protected override string GetFunctionName (MethodInfo funcType) => funcType.Name;
    }
}
