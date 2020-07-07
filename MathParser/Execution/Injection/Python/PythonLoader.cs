using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;

namespace MathParser.Execution.Injection.Python
{
    public class PythonLoader : Loader<PythonFunction>
    {
        private readonly string funcPrefix;

        ScriptEngine engine;
        ScriptScope scope;
        ScriptSource source;
        CompiledCode compiled;

        public PythonLoader (string funcPrefix, string prefix, string code) : base(prefix)
        {
            this.funcPrefix = funcPrefix;

            engine = IronPython.Hosting.Python.CreateEngine();
            scope = engine.CreateScope();
            source = engine.CreateScriptSourceFromString(code, Microsoft.Scripting.SourceCodeKind.Statements);
            compiled = source.Compile();
            compiled.Execute(scope);

        }

        protected override List<PythonFunction> GetFunctionsType ( )
        {
            var pyFunctions = (from i in scope.GetItems().ToList()
                            where i.Value.GetType() == typeof(PythonFunction)
                            where ((PythonFunction)i.Value).func_name.StartsWith(funcPrefix)
                            select (PythonFunction)i.Value).ToList();

            return pyFunctions;
        }

        protected override List<string> GetParameters (PythonFunction funcType) => funcType.func_code.co_varnames.Select(o => ParameterPrefix() + (string)o).ToList();

        public override Result<List<Property>> GetProperties ( )
        {
            return new List<Property>();
        }


        protected override Result<double> CallFunction (PythonFunction funcType, List<double> args)
        {
            try{
                if(args.Count == 0)
                    return new Result<double>((double)engine.Operations.InvokeMember(scope, funcType.func_name));
                return new Result<double>((double)engine.Operations.InvokeMember(scope, funcType.func_name, args.ToArray()));
            }catch(Exception e ) {
                return new Result<double>(ErrorCodes.FUNCTION_CALL(funcType.func_name, "Python", e));
            }
        }

        protected override string GetFunctionName (PythonFunction funcType) => funcType.func_name;
    }
}
