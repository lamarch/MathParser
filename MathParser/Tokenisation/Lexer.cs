namespace MathParser.Tokenisation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public class Lexer
    {

        private readonly TextReader reader;

        public Lexer (TextReader reader)
        {
            this.reader = reader;

            NextChar();
            Next();
        }


        public List<Error> Errors { get; private set; } = new List<Error>();
        public Symbol Current { get; private set; } = Symbol.Error(-1);

        private int currentPosition;
        private char currentChar;

        public void Next()
        {
            Current = ScanNext();
            if (Current.Token == Token.Error)
            {
                Errors.Add(ErrorCodes.UNKNOWN_CHAR(Current.Position));
            }

        }

        private void NextChar ( )
        {
            // TODO : optimisation : read by async buffer ?
            int ch = reader.Read();
            if ( ch > 0 ) {
                currentChar = (char)ch;
                currentPosition++;
            }
            else if (currentChar != '\0') {
                currentChar = '\0';
            }
        }

        private Symbol ScanNext ( )
        {

            //remove spaces
            while ( IsSpace(currentChar) ) { NextChar(); }

            if ( IsEOF() ) {
                return Symbol.EOF(currentPosition);
            }

            if ( ScanIdentifier() is string identifier)
            {
                return Symbol.Identifier(identifier, currentPosition);
            }

            if ( ScanNumber() is double value)
            {
                return Symbol.Number(value, currentPosition);
            }

            if ( ScanSign() is Token t)
            {
                return Symbol.Sign(t, currentPosition);
            }

            NextChar();
            return Symbol.Error(currentPosition);
        }

        private double? ScanNumber ( )
        {

            if ( !char.IsDigit(currentChar) )
                return null;

            bool hasPoint = false;
            StringBuilder builder = new StringBuilder();

            while ( true ) {
                if ( char.IsDigit(currentChar) ) {
                    builder.Append(currentChar);
                }
                // TODO : add comma support
                else if ( (currentChar == '.') && !hasPoint ) {
                    hasPoint = true;
                    builder.Append(currentChar);
                }
                else if ( currentChar == '_' ) {

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

            while ( char.IsLetter(currentChar) || currentChar == '_' ) {
                builder.Append(currentChar);
                NextChar();
            }

            return builder.Length == 0 ? null : builder.ToString();
        }

        private Token? ScanSign ( )
        {
            Token? token;
            switch ( currentChar ) {
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

        private bool IsEOF ( ) => currentChar == '\0';

    }
}
