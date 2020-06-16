using System;
using System.Collections.Generic;

namespace MathParser.Execution
{
    public class ExecutionContext : IContext
    {
        private readonly Stack<Segment> segments = new Stack<Segment>();

        public Segment Global { get; private set; }

        public List<Error> Errors { get; set; } = new List<Error>();

        public int MaxStackCapacity = 0;

        public ExecutionContext (Segment global)
        {
            Global = global ?? new Segment();
        }

        public Result<double> ResolveFunction (string id, List<double> args)
        {
            Result<Function> res = null;
            if ( this.segments.Count > 0 )
                res = this.segments.Peek().GetFunction(id, args);

            if ( res == null || res.HasErrors )
                res = Global.GetFunction(id, args);

            return res.HasErrors ? new Result<double>(res.Errors) : Execute(res.Value, args);

        }

        public Result<double> Execute (Callable f, List<double> args)
        {



            Segment s = new Segment();

            for ( int i = 0; i < f.GetArgs().Count; i++ ) {
                string param = f.GetArgs()[i];

                s.AddProperty(new Property(param, args[i]));
            }

            Alloc(s);
            var result = Call(f);
            Free();


            return result;
        }

        public void AddProperty (Property p) => Global.AddProperty(p);

        public void AddFunction (Function f) => Global.AddFunction(f);

        public Result<double> Call (Callable callable)
        {
            if ( this.MaxStackCapacity++ == int.MaxValue ) {
                return new Result<double>(new Error(new StackOverflowException(), -1, "La limite de la pile des appels a été dépassée. Vérifié que vous n'avez pas fait de boucle infini via des appels récursifs.", Error.FormatSource("ExecutionContext", true)));
            }

            var result = callable.Call(this);
            this.MaxStackCapacity--;
            return result;
        }

        public void Alloc (Segment s) => this.segments.Push(s);

        public void Free ( ) => this.segments.Pop();

        public Result<double> ResolveProp (string id)
        {
            Result<Property> res = null;
            if ( this.segments.Count > 0 ) {
                res = this.segments.Peek().GetProperty(id);
            }

            if ( res == null || res.HasErrors )
                res = Global.GetProperty(id);

            return res.HasErrors ? new Result<double>(res.Errors) : Call(res.Value);

        }


    }
}
