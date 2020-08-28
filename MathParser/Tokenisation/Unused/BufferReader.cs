using System;
using System.Collections.Generic;
using System.Text;

namespace MathParser.Tokenisation.Unused
{
    class BufferReader<T>
    {
        private readonly int size;
        private T[] buffer;
        private int pos;

        public BufferReader(int size)
        {
            buffer = new T[size];
            this.size = size;
        }

        public T Current ( )
        {
            return buffer[pos];
        }

        public T Next ( )
        {
            return buffer[pos++];
        }

        public void Clear ( )
        {
            pos = 0;
        }

        public bool IsEmpty => pos >= size;
        public T[] Buffer => buffer;
        public int Position => pos;
        public int Size => size;
    }
}
