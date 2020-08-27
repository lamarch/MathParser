using System;
using System.Collections.Generic;
using System.Linq;

using IronPython.Runtime;

using Microsoft.Scripting.Hosting;

namespace MathParser.Execution.Injection.Python
{
    public class PythonLoader : Loader<PythonFunction>
    {
        private readonly string funcPrefix;
        private readonly ScriptEngine engine;
        private readonly ScriptScope scope;
        private readonly ScriptSource source;
        private readonly CompiledCode compiled;

        public PythonLoader (string funcPrefix, string prefix, string code) : base(prefix)
        {
            this.funcPrefix = funcPrefix;

            this.engine = IronPython.Hosting.Python.CreateEngine();
            this.scope = this.engine.CreateScope();
            this.source = this.engine.CreateScriptSourceFromString(code, Microsoft.Scripting.SourceCodeKind.Statements);
            this.compiled = this.source.Compile();
            this.compiled.Execute(this.scope);

        }

        protected override List<PythonFunction> GetFunctionsType ( )
        {
            var pyFunctions = (from i in this.scope.GetItems().ToList()
                               where i.Value.GetType() == typeof(PythonFunction)
                               where ((PythonFunction)i.Value).func_name.StartsWith(this.funcPrefix)
                               select (PythonFunction)i.Value).ToList();

            return pyFunctions;
        }

        protected override List<string> GetParameters (PythonFunction funcType) => funcType.func_code.co_varnames.Select(o => ParameterPrefix() + (string)o).ToList();

        public override Result<List<Property>> GetProperties ( ) => new List<Property>();


        protected override Result<double> CallFunction (PythonFunction funcType, List<double> args)
        {
            try {
                if ( args.Count == 0 )
                    return new Result<double>((double)this.engine.Operations.InvokeMember(this.scope, funcType.func_name));
                return new Result<double>((double)this.engine.Operations.InvokeMember(this.scope, funcType.func_name, args.ToArray()));
            }
            catch ( Exception e ) {
                return new Result<double>(ErrorCodes.FUNCTION_CALL(funcType.func_name, "Python", e));
            }
        }

        protected override string GetFunctionName (PythonFunction funcType) => funcType.func_name;
    }
}
