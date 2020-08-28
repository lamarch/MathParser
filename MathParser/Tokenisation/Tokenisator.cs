using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Tokenisation
{
    public class Tokenisator
    {
        private readonly TextReader reader;

        private char currentChar;
        private bool nextCharNull = false;

        private int currentPos;
        private Token currentToken;
        private string identifier;
        private double value;

        public Tokenisator (TextReader reader)
        {
            this.reader = reader;

            NextChar();
        }

        public Token CurrentToken => currentToken;
        public int CurrentPosition => currentPos;
        public string Identifier => identifier;
        public double Value => value;

        private void NextChar ( )
        {
            int ch = reader.Read();
            if ( ch > 0 ) {
                currentChar = (char)ch;
                currentPos++;
            }
            else {
                if ( !nextCharNull ) {
                    currentPos++;
                    nextCharNull = true;
                }
                currentChar = '\0';
            }
        }

        public void NextToken ( )
        {
            currentToken = Token.Error;
            identifier = null;
            value = 0;

            //remove spaces
            while ( IsSpace(currentChar) ) { NextChar(); }

            if ( IsEOF() ) {
                currentToken = Token.EOF;
                return;
            }

            if ( ScanIdentifier() )
                return;

            if ( ScanNumber() )
                return;

            if ( ScanSign() )
                return;



            NextChar();
            throw new LexerException("Unknown token : " + currentChar);
        }

        private bool ScanNumber ( )
        {

            if ( !char.IsDigit(currentChar) )
                return false;

            bool hasPoint = false;
            StringBuilder builder = new StringBuilder();
            NumberFormatInfo info = CultureInfo.CurrentCulture.NumberFormat;

            while ( true ) {
                if ( char.IsDigit(currentChar) ) {
                    builder.Append(currentChar);
                }
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
            double res = 0;

            if ( double.TryParse(extracted, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out res) ) {
                currentToken = Token.Number;
                value = res;
                return true;
            }
            else {
                throw new Exception();
            }
        }

        private bool ScanIdentifier ( )
        {
            StringBuilder builder = new StringBuilder();

            while ( char.IsLetter(currentChar) || currentChar == '_' ) {
                builder.Append(currentChar);
                NextChar();
            }//while close

            if ( builder.Length == 0 )
                return false;

            identifier = builder.ToString();
            currentToken = Token.Identifier;
            return true;

        }

        private bool ScanSign ( )
        {

            switch ( currentChar ) {
                case '+':
                    currentToken = Token.Plus;
                    NextChar();
                    return true;

                case '-':
                    currentToken = Token.Minus;
                    NextChar();
                    return true;

                case '*':
                    currentToken = Token.Star;
                    NextChar();
                    return true;

                case '/':
                    currentToken = Token.Slash;
                    NextChar();
                    return true;

                case '%':
                    currentToken = Token.Percent;
                    NextChar();
                    return true;
                case '(':
                    currentToken = Token.LPar;
                    NextChar();
                    return true;

                case ')':
                    currentToken = Token.RPar;
                    NextChar();
                    return true;

                case ',':
                    currentToken = Token.Comma;
                    NextChar();
                    return true;

                case '^':
                    this.currentToken = Token.Exp;
                    NextChar();
                    return true;
                default:
                    return false;
            }
        }

        private bool IsSpace (char c) => c == ' ' || c == '\t' || c == '\n' || c == '\r';

        private bool IsEOF ( ) => currentChar == '\0';

    }
}
