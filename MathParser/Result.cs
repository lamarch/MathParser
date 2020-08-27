using System;
using System.Collections.Generic;

namespace MathParser
{
    public class Result<T>
    {
        public Result (T value, List<Error> error)
        {
            Value = value;
            Errors = error;
        }

        public Result (T value, Error e) : this(value, new List<Error> { e }) { }

        public Result (T value) : this(value, new List<Error>()) { }

        public Result (Error e) : this(default, e) { }

        public Result (List<Error> errors) : this(default, errors) { }

        public Result ( ) : this(default, new List<Error>()) { }

        public Result<T> SetErrorsPosition (int p)
        {
            foreach ( var e in Errors ) {
                e.SetPosition(p);
            }
            return this;
        }

        public Result<T> MergeIf<T2> (Result<T2> result, Action<T2, T> convert)
        {
            if ( result.HasErrors )
                Errors.AddRange(result.Errors);
            else
                convert(result.Value, Value);
            return this;
        }

        public Result<T> Merge<T2> (Result<T2> result, Func<T2, T, T> convert)
        {
            if ( result.HasErrors )
                Errors.AddRange(result.Errors);

            Value = convert(result.Value, Value);
            return Value;
        }

        public static implicit operator Result<T> (T val) => new Result<T>(val);


        public T Value { get; private set; }
        public List<Error> Errors { get; private set; }
        public bool HasErrors => Errors.Count != 0;
    }
}
