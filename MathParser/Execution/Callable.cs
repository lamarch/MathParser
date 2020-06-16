using System.Collections.Generic;

namespace MathParser.Execution
{
    public abstract class Callable
    {
        public Callable (string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public abstract List<string> GetArgs ( );
        public abstract Result<double> Init (IContext context);
        public abstract Result<double> Call (IContext context);
        public abstract string GetHash ( );
        public override string ToString ( ) => $"{Name} [{GetArgs().Count} args]";
    }
}
