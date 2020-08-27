using System;

namespace MathParser
{
    public class Error
    {

        public Error (string name = "unknown name", string message = "unknown message", string source = "unknown source", string code = "0x0000", int position = -1, bool isRuntime = false, int calleLine = -1, string memberName = "unknown member", string filePath = "unknown file path", Exception exception = null)
        {
            Name = name;
            Message = message;
            Source = source;
            Code = code;
            Position = position;
            IsRuntime = isRuntime;
            CallerLine = calleLine;
            CallerMember = memberName;
            CallerFilePath = filePath;
            Exception = exception;
        }

        public string Name { get; private set; }
        public string Message { get; private set; }
        public string Source { get; private set; }
        public string Code { get; private set; }
        public int Position { get; private set; }
        public bool IsRuntime { get; private set; }
        public int CallerLine { get; private set; }
        public string CallerMember { get; private set; }
        public string CallerFilePath { get; private set; }
        public Exception Exception { get; private set; }

        public void SetPosition (int pos) { if ( Position == -1 ) Position = pos; }


        public override string ToString ( ) => $"{Name} ({(IsRuntime ? "run-time" : "compile-time")} {Code}) : \"{Message}\" [position : {Position} from {Source}]";


    }
}
