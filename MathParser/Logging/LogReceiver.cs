using System;

namespace MathParser.Logging
{
    public class LogReceiver
    {
        public LogReceiver (Logger logger)
        {
            logger.Setup(Log);
        }

        private void Log (Log log) => OnLog?.Invoke(log);

        public event Action<Log> OnLog;

    }
}
