using System;
using System.Collections.Generic;
using System.Linq;

namespace MathParser.Execution.Injection
{
    /// <summary>
    /// A parent class that help to create simple loaders.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Loader<T> : ILoader
    {
        private readonly string prefix;
        public Loader (string prefix)
        {
            this.prefix = prefix;
        }

        protected abstract List<string> GetParameters (T funcType);
        protected abstract List<T> GetFunctionsType ( );
        public abstract Result<List<Property>> GetProperties ( );
        protected abstract Result<double> CallFunction (T funcType, List<double> args);
        protected abstract string GetFunctionName (T funcType);

        protected virtual Function GetFunction (T funcType, string name)
        {
            List<string> paramaters = GetParameters(funcType);

            Func<IContext, Result<double>> binding =
                ctx => {
                    Dictionary<string, Result<double>> bindedParams = new Dictionary<string, Result<double>>();

                    //Get all the parameters values
                    foreach ( var p in paramaters ) {
                        bindedParams.Add(p, ctx.ResolveProp(p));
                    }

                    List<Error> errors = new List<Error>();

                    //Check if errors
                    foreach ( (_, var val) in bindedParams ) {
                        if ( val.HasErrors ) {
                            errors.AddRange(val.Errors);
                        }
                    }

                    if ( errors.Any() )
                        return new Result<double>(errors);

                    //Create arguments
                    var arguments = bindedParams.Select(k => k.Value.Value).ToList();


                    return CallFunction(funcType, arguments);

                };

            return new Function(name, binding, paramaters);
        }

        public virtual Result<List<Function>> GetFunctions ( )
        {
            Result<List<Function>> result = new Result<List<Function>>(new List<Function>());


            foreach ( var tFunc in GetFunctionsType() ) {

                string funcName = "ERROR";

                try {

                    funcName = prefix + GetFunctionName(tFunc);

                    Function function = GetFunction(tFunc, funcName);

                    result.Value.Add(function);

                }
                catch ( Exception e ) {
                    result.Errors.Add(ErrorCodes.FUNCTION_CREATION(funcName, e));
                }
            }

            return result;
        }

        protected string ParameterPrefix ( ) => "__p__";
    }
}
