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
                errors.Add(new Error(null, -1, $"Aucune fonction correspondante nommée '{id}' avec {args.Count} paramètres, cependant, peut-être voulez vous appeler '{id}' avec {fct.GetArgs().Count} paramètres.", Error.FormatSource("Segment", true)));
            }

            if ( errors.Count > 0 )
                return new Result<Function>(errors);

            return new Result<Function>(new Error(null, -1, $"La fonction {id} n'existe pas ou n'est pas atteignable.", Error.FormatSource("Segment", true)));
        }

        public Result<Property> GetProperty (string id)
        {
            var pseudoProp = new Property(id, null);


            foreach ( var prop in Properties ) {
                if ( prop.GetHash() == pseudoProp.GetHash() ) {
                    return new Result<Property>(prop);
                }
            }

            return new Result<Property>(new Error(null, -1, $"La propriété {id} n'existe pas ou n'est pas atteignable.", Error.FormatSource("Segment", true)));
        }

        public Result<Property> AddProperty (Property prop)
        {
            if ( Properties.Any(p => p.GetHash() == prop.GetHash()) )
                return new Result<Property>(new Error(null, -1, $"Impossible d'ajouter la propriété {prop.Name} car elle existe déjà dans ce segment.", Error.FormatSource("Segment", false)));

            Properties.Add(prop);
            return new Result<Property>(prop);
        }

        public Result<Function> AddFunction (Function func)
        {
            if ( Functions.Any(f => f.GetHash() == func.GetHash()) )
                return new Result<Function>(new Error(null, -1, $"Impossible d'ajouter la fonction {func.Name} car elle existe déjà dans ce segment.", Error.FormatSource("Segment", false)));

            Functions.Add(func);
            return new Result<Function>(func);
        }

    }
}
