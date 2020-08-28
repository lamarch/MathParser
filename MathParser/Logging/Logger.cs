using System;

namespace MathParser.Logging
{
    public class Logger
    {
        private Action<Log> logging;
        public void Setup (Action<Log> logging) => this.logging = logging;

        public void Info (string from, string message)
        {
            if ( IsOn )
                logging?.Invoke(new Log(LogLevel.Info, DateTime.Now, from, message));
        }

        public void Warn (string from, string message)
        {
            if ( IsOn )
                logging?.Invoke(new Log(LogLevel.Warning, DateTime.Now, from, message));
        }

        public void Error (string from, string message)
        {
            if ( IsOn )
                logging?.Invoke(new Log(LogLevel.Error, DateTime.Now, from, message));
        }

        public bool IsOn { get; set; } = false;
    }
}
