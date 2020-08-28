using System;
using System.Collections.Generic;
using System.Text;

using BenchmarkDotNet.Attributes;

using MathParser;

namespace Benchmarking
{
    public class Benchmark
    {
        private readonly Compiler compiler;

        private string code;

        private const int count = 10_000;

        public Benchmark ( )
        {
            compiler = new Compiler();
            StringBuilder sb = new StringBuilder();

            for ( int i = 0; i < count; i++ ) {
                sb.Append("1+(2*8)/7(5^0.22)%15.12+");
            }
            sb.Remove(sb.Length - 1, 1);

            code = sb.ToString();
        }

        [Benchmark]
        public void Compile ( )
        {
            compiler.Compile(code);
        }

        [Benchmark]
        public void CompileAdd ( )
        {
            Compiler compiler = new Compiler();
            StringBuilder sb = new StringBuilder();

            for ( int i = 0; i < count; i++ ) {
                sb.Append("1+");
            }
            sb.Remove(sb.Length - 1, 1);

            compiler.Compile(sb.ToString());
        }

        [Benchmark]
        public void CompileSub ( )
        {
            Compiler compiler = new Compiler();
            StringBuilder sb = new StringBuilder();

            for ( int i = 0; i < count; i++ ) {
                sb.Append("1-");
            }
            sb.Remove(sb.Length - 1, 1);

            compiler.Compile(sb.ToString());
        }

        [Benchmark]
        public void CompileMul ( )
        {
            Compiler compiler = new Compiler();
            StringBuilder sb = new StringBuilder();

            for ( int i = 0; i < count; i++ ) {
                sb.Append("1*");
            }
            sb.Remove(sb.Length - 1, 1);

            compiler.Compile(sb.ToString());
        }

        [Benchmark]
        public void CompileDiv ( )
        {
            Compiler compiler = new Compiler();
            StringBuilder sb = new StringBuilder();

            for ( int i = 0; i < count; i++ ) {
                sb.Append("1/");
            }
            sb.Remove(sb.Length - 1, 1);

            compiler.Compile(sb.ToString());
        }

        [Benchmark]
        public void CompilePow ( )
        {
            Compiler compiler = new Compiler();
            StringBuilder sb = new StringBuilder();

            for ( int i = 0; i < count; i++ ) {
                sb.Append("1^");
            }
            sb.Remove(sb.Length - 1, 1);

            compiler.Compile(sb.ToString());
        }
    }
}
