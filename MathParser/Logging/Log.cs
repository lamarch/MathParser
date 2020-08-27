using System;
using System.Collections.Generic;
using System.Text;

namespace MathParser.Logging
{
    public class Log
    {
        public Log (LogLevel logLevel, DateTime postedAt, string from, string message)
        {
            this.LogLevel = logLevel;
            this.PostedAt = postedAt;
            this.From = from;
            this.Message = message;
        }

        public LogLevel LogLevel { get; private set; }
        public DateTime PostedAt { get; private set; }
        public string From { get; private set; }
        public string Message { get; private set; }
    }
}
