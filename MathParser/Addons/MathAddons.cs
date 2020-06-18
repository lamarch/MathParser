using System;

using MathParser.Execution.Injection;
using MathParser.Execution.Injection.CSharp;

namespace MathParser.Addons
{
    public class MathAddons
    {
        private static readonly Random rand = new Random();
        [MathImpl]
        public static double Rand (double min, double max) => rand.NextDouble() % (min - max) + min;

    }
}
