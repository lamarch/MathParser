using System;
using System.Collections.Generic;
using System.Text;

using MathParser;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void SpeedTest ( )
        {
            Compiler compiler = new Compiler();

            compiler.Compile("1+2*v(2.5)%3(0.001/95)");
        }
    }
}
