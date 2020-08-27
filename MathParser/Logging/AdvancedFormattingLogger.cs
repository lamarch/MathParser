using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MathParser.Logging
{
    public class AdvancedFormattingLogger
    {
        private readonly Logger logger;
        private readonly string id;

        private int branchCount = 0;
        private Stack<string> branchesName = new Stack<string>();

        public AdvancedFormattingLogger (Logger logger, string id)
        {
            this.logger = logger;
            this.id = id;
        }

        public void OpenBranch(string name)
        {
            branchesName.Push(name);
            branchCount++;

            //logger.Info(GetName(), new string('-', 2 * branchCount));
        }

        public void CloseBranch ( )
        {
            //logger.Info(GetName(), new string('-', 2 * branchCount));
            if ( branchesName.Count > 0 )
                branchesName.Pop();
            branchCount--;
        }

        public void Info(string message)
        {
            logger.Info(GetName() , Format(message));
        }

        public void Warn (string message)
        {
            logger.Warn(GetName(), Format(message));

        }

        public void Err (string message)
        {
            logger.Error(GetName(), Format(message));

        }

        private string GetName ( )
        {
            string name;
            if ( !branchesName.TryPeek(out name) )
                name = "none";
            return id+"::"+ name;
        }

        private string Format(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(' ', branchCount * 2);
            sb.Append(message);
            return sb.ToString();
        }
    }
}
