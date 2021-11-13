namespace MathParser.Tokenisation
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public class Tokenisator
    {
        private readonly TextReader reader;

        public Tokenisator (TextReader reader)
        {
            this.reader = reader;

            NextChar();
        }

        public int CurrentPosition { get; private set; }
        public char CurrentChar { get; set; }


        private void NextChar ( )
        {
            // TODO : optimisation : read by async buffer 
            int ch = reader.Read();
            if ( ch > 0 ) {
                CurrentChar = (char)ch;
                CurrentPosition++;
            }
            else if (CurrentChar != '\0') {
                CurrentChar = '\0';
            }
        }

        public Symbol NextToken ( )
        {

            //remove spaces
            while ( IsSpace(CurrentChar) ) { NextChar(); }

            if ( IsEOF() ) {
                return Symbol.EOF(CurrentPosition);
            }

            if ( ScanIdentifier() is string identifier)
            {
                return Symbol.Identifier(identifier, CurrentPosition);
            }

            if ( ScanNumber() is double value)
            {
                return Symbol.Number(value, CurrentPosition);
            }

            if ( ScanSign() is Token t)
            {
                return Symbol.Sign(t, CurrentPosition);
            }

            NextChar();
            return Symbol.Error(CurrentPosition);
        }

        private double? ScanNumber ( )
        {

            if ( !char.IsDigit(CurrentChar) )
                return null;

            bool hasPoint = false;
            StringBuilder builder = new StringBuilder();

            while ( true ) {
                if ( char.IsDigit(CurrentChar) ) {
                    builder.Append(CurrentChar);
                }
                // TODO : add comma support
                else if ( (CurrentChar == '.') && !hasPoint ) {
                    hasPoint = true;
                    builder.Append(CurrentChar);
                }
                else if ( CurrentChar == '_' ) {

                }
                else {
                    break;
                }
                NextChar();
            }

            string extracted = builder.ToString();

            if ( double.TryParse(extracted, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double res) ) {
                return res;
            }
            else {
                throw new Exception("Tokenisator : Cannot parse double.");
            }
        }

        private string ScanIdentifier ( )
        {
            StringBuilder builder = new StringBuilder();

            while ( char.IsLetter(CurrentChar) || CurrentChar == '_' ) {
                builder.Append(CurrentChar);
                NextChar();
            }

            return builder.Length == 0 ? null : builder.ToString();
        }

        private Token? ScanSign ( )
        {
            Token? token;
            switch ( CurrentChar ) {
                case '+':
                    token = Token.Plus; break;

                case '-':
                    token = Token.Minus; break;

                case '*':
                    token = Token.Star; break;

                case '/':
                    token = Token.Slash; break;

                case '%':
                    token = Token.Percent; break;

                case '(':
                    token = Token.LPar; break;

                case ')':
                    token = Token.RPar; break;

                case ',':
                    token = Token.Comma; break;

                case '^':
                    token = Token.Exp; break;

                default:
                    token = null; break;
            }
            NextChar();
            return token;
        }

        private bool IsSpace (char c) => c == ' ' || c == '\t' || c == '\n' || c == '\r';

        private bool IsEOF ( ) => CurrentChar == '\0';

    }
}
