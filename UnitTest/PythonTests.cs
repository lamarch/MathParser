using System;
using System.Collections.Generic;
using System.Text;
using MathParser.Execution.Injection;
using MathParser.Execution.Injection.Python;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class PythonTests
    {
        [TestMethod]
        public void MainTest ( )
        {
            PythonLoader loader = new PythonLoader("", "py_test_", "" +
                "def a():\n" +
                "\treturn 0\n" +
                "b = 2\n" +
                "");

            var functions = loader.GetFunctions();

            Assert.IsFalse(functions.HasErrors);
            Assert.IsTrue(functions.Value.Count == 1);
        }
    }
}
