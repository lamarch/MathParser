using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MathParser.Execution
{
    public static class ReflectionHelper
    {
        public static Function GenerateFunction (MethodInfo info, object rtObjRef)
        {
            List<string> paramaters = info.GetParameters().Select(p => p.Name).ToList();

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
                    var arguments = bindedParams.Select(k => k.Value.Value).Cast<object>().ToArray();


                    try {
                        //Invoke method
                        var d = (double)info.Invoke(rtObjRef, arguments);
                        return new Result<double>(d);

                    }
                    catch ( Exception e ) {
                        return new Result<double>(new Error(e, -1, $"Impossible d'appeler la méthode dynamique '{info.Name}'.", Error.FormatSource("ExecutionContext", true)));
                    }
                };

            return new Function(info.Name, binding, paramaters);
        }
    }
}
