using System;
using System.Runtime.CompilerServices;

namespace MathParser
{
    public class Error
    {
        private Exception e;
        private int position;
        private string message;
        private string source;

        public Error (Exception e, int position, string message, string source)
        {
            E = e;
            Position = position;
            Message = message;
            Source = source;
        }

        public static string FormatSource (string simpleName, bool isRuntime, [CallerLineNumber] int ln = -1, [CallerMemberName] string mm = "unknown") => $"{(isRuntime ? "Runtime" : "Compiletime")} : {simpleName} [{mm} at {ln}]";

        public Exception E { get => this.e; set => this.e = value; }
        public int Position { get => this.position; set => this.position = value; }
        public string Message { get => this.message; set => this.message = value; }
        public string Source { get => this.source; set => this.source = value; }

        public override string ToString ( ) => $"{Message} [{Source}] ({Position})";
    }
}
