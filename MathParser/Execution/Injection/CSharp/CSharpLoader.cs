using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace MathParser.Execution.Injection.CSharp
{
    public class CSharpLoader : Loader<MethodInfo>
    {

        List<Type> typesToLoad = new List<Type>();

        public CSharpLoader(Assembly assembly, string prefix) : base(prefix)
        {
            typesToLoad = assembly.GetTypes().ToList();
        }

        public CSharpLoader(Type type, string prefix) : base(prefix)
        {
            typesToLoad.Add(type);
        }

        public override Result<List<Property>> GetProperties ( )
        {
            return new List<Property>();
        }

        //This call a function from her MethodInfo (funcType)
        protected override Result<double> CallFunction (MethodInfo funcType, List<double> args)
        {
            try {
                return (double)funcType.Invoke(null, args.Cast<object>().ToArray());
            }catch(Exception e ) {
                return new Result<double>(ErrorCodes.FUNCTION_CALL(funcType.Name, "CSharp", e));
            }
        }

        protected override List<MethodInfo> GetFunctionsType ( )
        {
            return (from t in typesToLoad
                    from m in t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)

                    where m.ReturnType == typeof(double)
                    where m.GetParameters().All(p => p.ParameterType == typeof(double))

                    select m).ToList();
        }

        protected override List<string> GetParameters (MethodInfo funcType)
        {
            return funcType.GetParameters().Select(p => ParameterPrefix() + p.Name).ToList();
        }

        protected override string GetFunctionName (MethodInfo funcType) => funcType.Name;
    }
}
