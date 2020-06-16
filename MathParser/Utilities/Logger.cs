using System.IO;

namespace MathParser.Utilities
{
    public class Logger
    {
        public LogLevel LogLevel { get; set; }

        public bool On { get; set; } = true;

        public TextWriter Out { get; set; }

        public Logger (TextWriter writer, LogLevel logLevel = LogLevel.Info)
        {
            Out = writer;
            LogLevel = logLevel;
        }

        public void Log (string message, LogLevel level)
        {
            if ( (int)level <= (int)LogLevel ) {
                Log(message);
            }
        }

        public void Log (string message) => Out.WriteLine(message);

        public void Info (string message) => Log(message, LogLevel.Info);

        public void Warn (string message) => Log(message, LogLevel.Warning);

        public void Error (string message) => Log(message, LogLevel.Error);
    }
}
