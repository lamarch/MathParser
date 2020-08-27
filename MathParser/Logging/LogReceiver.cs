using System;
using System.Collections.Generic;
using System.Text;

namespace MathParser.Logging
{
    public class LogReceiver
    {
        public LogReceiver(Logger logger)
        {
            logger.Setup(Log);
        }

        private void Log (Log log)
        {
            OnLog?.Invoke(log);
        }

        public event Action<Log> OnLog;

    }
}
