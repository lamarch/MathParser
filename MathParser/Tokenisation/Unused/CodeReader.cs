using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Tokenisation.Unused
{
    class CodeReader
    {
        private const int bufferSize = 1024;

        private readonly TextReader textReader;

        private BufferReader<char> reader = new BufferReader<char>(bufferSize);
        private bool reachEnd = false;
        private int pos = 0;

        public CodeReader(TextReader textReader)
        {
            this.textReader = textReader;
        }

        public async Task<char> NextChar ( )
        {
            if ( reader.IsEmpty ) {
                if ( reachEnd ) {
                    return '\0';
                }
                else {
                    reader.Clear();
                    int remain = await textReader.ReadAsync(reader.Buffer);
                    if ( remain != bufferSize ) {
                        reachEnd = true;
                    }
                }
            }
            pos++;
            return reader.Next();
        }

        public int Position => pos;
    }
}
