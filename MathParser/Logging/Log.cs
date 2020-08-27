using System;

namespace MathParser.Logging
{
    public class Log
    {
        public Log (LogLevel logLevel, DateTime postedAt, string from, string message)
        {
            LogLevel = logLevel;
            PostedAt = postedAt;
            From = from;
            Message = message;
        }

        public LogLevel LogLevel { get; private set; }
        public DateTime PostedAt { get; private set; }
        public string From { get; private set; }
        public string Message { get; private set; }
    }
}
