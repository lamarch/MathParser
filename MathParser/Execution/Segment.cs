using System.Collections.Generic;
using System.Linq;

namespace MathParser.Execution
{
    public class Segment
    {
        public List<Function> Functions { get; private set; } = new List<Function>();
        public List<Property> Properties { get; private set; } = new List<Property>();

        public Result<Function> GetFunction (string id, List<double> args)
        {
            var pseudoFunc = new Function(id, null, args.Select(d => d.ToString()).ToList());

            foreach ( var fct in Functions ) {
                if ( fct.GetHash() == pseudoFunc.GetHash() ) {
                    return new Result<Function>(fct);
                }
            }

            List<Error> errors = new List<Error>();
            foreach ( var fct in Functions.FindAll(f => f.Name == id) ) {
                errors.Add(ErrorCodes.ADJUST_FUNCTION_NOT_FOUND(id, args.Count, id, fct.GetArgs().Count));
            }

            if ( errors.Count > 0 )
                return new Result<Function>(errors);

            return new Result<Function>(ErrorCodes.FUNCTION_NOT_FOUND(id));
        }

        public Result<Property> GetProperty (string id)
        {
            var pseudoProp = new Property(id, null);


            foreach ( var prop in Properties ) {
                if ( prop.GetHash() == pseudoProp.GetHash() ) {
                    return new Result<Property>(prop);
                }
            }

            return new Result<Property>(ErrorCodes.PROPERTY_NOT_FOUND(id));
        }

        public Result<Property> AddProperty (Property prop)
        {
            if ( Properties.Any(p => p.GetHash() == prop.GetHash()) )
                return new Result<Property>(ErrorCodes.EXISTING_PROPERTY(prop.Name));

            Properties.Add(prop);
            return new Result<Property>(prop);
        }

        public Result<Function> AddFunction (Function func)
        {
            if ( Functions.Any(f => f.GetHash() == func.GetHash()) )
                return new Result<Function>(ErrorCodes.EXISTING_FUNCTION(func.Name));

            Functions.Add(func);
            return new Result<Function>(func);
        }

    }
}
