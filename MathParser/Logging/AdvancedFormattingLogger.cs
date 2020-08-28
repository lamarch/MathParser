using System.Collections.Generic;
using System.Text;

namespace MathParser.Logging
{
    public class AdvancedFormattingLogger
    {
        private readonly Logger logger;
        private readonly string id;

        private int branchCount = 0;
        private readonly Stack<string> branchesName = new Stack<string>();

        public AdvancedFormattingLogger (Logger logger, string id)
        {
            this.logger = logger;
            this.id = id;
        }

        public void OpenBranch (string name)
        {
            if ( !logger.IsOn )
                return;

            branchesName.Push(name);
            branchCount++;

            //logger.Info(GetName(), new string('-', 2 * branchCount));
        }

        public void CloseBranch ( )
        {
            if ( !logger.IsOn )
                return;

            //logger.Info(GetName(), new string('-', 2 * branchCount));
            if ( branchesName.Count > 0 )
                branchesName.Pop();
            branchCount--;
        }

        public void Info (string message)
        {
            if ( !logger.IsOn )
                return;

            logger.Info(GetName(), Format(message));
        }

        public void Warn (string message)
        {
            if ( !logger.IsOn )
                return;

            logger.Warn(GetName(), Format(message));
        }

        public void Err (string message)
        {
            if ( !logger.IsOn )
                return;

            logger.Error(GetName(), Format(message));
        }

        private string GetName ( )
        {
            if ( !logger.IsOn )
                return string.Empty;

            string name;
            if ( !branchesName.TryPeek(out name) )
                name = "none";
            return id + "::" + name;
        }

        private string Format (string message)
        {
            if ( !logger.IsOn )
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(' ', branchCount * 2);
            sb.Append(message);
            return sb.ToString();
        }
    }
}
