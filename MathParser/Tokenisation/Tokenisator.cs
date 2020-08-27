using System;
using System.Globalization;
using System.IO;
using System.Text;

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

        public Token CurrentToken => this.currentToken;
        public int CurrentPosition => this.currentPos;
        public string Identifier => this.identifier;
        public double Value => this.value;

        private void NextChar ( )
        {
            int ch = this.reader.Read();
            if ( ch > 0 ) {
                this.currentChar = (char)ch;
                this.currentPos++;
            }
            else {
                if ( !this.nextCharNull ) {
                    this.currentPos++;
                    this.nextCharNull = true;
                }
                this.currentChar = '\0';
            }
        }

        public void NextToken ( )
        {
            this.currentToken = Token.Error;
            this.identifier = null;
            this.value = 0;

            //remove spaces
            while ( IsSpace(this.currentChar) ) { NextChar(); }

            if ( IsEOF() ) {
                this.currentToken = Token.EOF;
                return;
            }

            if ( ScanIdentifier() )
                return;

            if ( ScanNumber() )
                return;

            if ( ScanSign() )
                return;



            NextChar();
            throw new LexerException("Unknown token : " + this.currentChar);
        }

        private bool ScanNumber ( )
        {

            if ( !char.IsDigit(this.currentChar) )
                return false;

            bool hasPoint = false;
            StringBuilder builder = new StringBuilder();
            NumberFormatInfo info = CultureInfo.CurrentCulture.NumberFormat;

            while ( true ) {
                if ( char.IsDigit(this.currentChar) ) {
                    builder.Append(this.currentChar);
                }
                else if ( (this.currentChar == '.') && !hasPoint ) {
                    hasPoint = true;
                    builder.Append(this.currentChar);
                }
                else if ( this.currentChar == '_' ) {

                }
                else {
                    break;
                }
                NextChar();
            }

            string extracted = builder.ToString();
            double res = 0;

            if ( double.TryParse(extracted, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out res) ) {
                this.currentToken = Token.Number;
                this.value = res;
                return true;
            }
            else {
                throw new Exception();
            }
        }

        private bool ScanIdentifier ( )
        {
            if ( !char.IsLetter(this.currentChar) && this.currentChar != '_' )
                return false;

            StringBuilder builder = new StringBuilder();

            while ( char.IsLetter(this.currentChar) || this.currentChar == '_' ) {
                builder.Append(this.currentChar);
                NextChar();
            }//while close

            this.identifier = builder.ToString();
            this.currentToken = Token.Identifier;
            return true;

        }

        private bool ScanSign ( )
        {

            switch ( this.currentChar ) {
                case '+':
                    this.currentToken = Token.Plus;
                    NextChar();
                    return true;

                case '-':
                    this.currentToken = Token.Minus;
                    NextChar();
                    return true;

                case '*':
                    this.currentToken = Token.Star;
                    NextChar();
                    return true;

                case '/':
                    this.currentToken = Token.Slash;
                    NextChar();
                    return true;

                case '%':
                    this.currentToken = Token.Percent;
                    NextChar();
                    return true;

                case '(':
                    this.currentToken = Token.LPar;
                    NextChar();
                    return true;

                case ')':
                    this.currentToken = Token.RPar;
                    NextChar();
                    return true;

                case ',':
                    this.currentToken = Token.Comma;
                    NextChar();
                    return true;

                /*                case '^':
                                    this.currentToken = Token.Exp;
                                    NextChar();
                                    return true;*/

                default:
                    return false;
            }
        }

        private bool IsSpace (char c) => c == ' ' || c == '\t' || c == '\n' || c == '\r';

        private bool IsEOF ( ) => this.currentChar == '\0';

    }
}
