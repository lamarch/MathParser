using System;
using System.Collections.Generic;

using MathParser.Parsing.Nodes;
using MathParser.Tokenisation;

namespace MathParser.Parsing
{
    public class Parser
    {
        private SymbolStream symStream;

        private List<Error> errors = new List<Error>();

        public Parser ( )
        {
        }

        public Result<Expression> Parse (SymbolStream stream)
        {
            errors.Clear();

            this.symStream = stream;

            var final = ParseTherm();

            Expect(Token.EOF);

            return new Result<Expression>(final, errors);
        }

        private Expression ParseTherm ( )
        {
            var lhs = ParseMultDivMod();

            while ( true ) {
                Func<double, double, double> op = null;

                if ( IsTypeOf(Token.Plus) ) {
                    op = OP_add;
                }
                else if ( IsTypeOf(Token.Minus) ) {
                    op = OP_sub;
                }

                if ( op == null )
                    return lhs;

                this.symStream.Next();

                var rhs = ParseMultDivMod();

                lhs = new Binary(GetSymbolPosition(), op, lhs, rhs);
            }
        }

        private Expression ParseMultDivMod ( )
        {
            var lhs = ParseUnary();

            Expression rhs = null;

            while ( true ) {
                Func<double, double, double> op = null;

                if ( IsTypeOf(Token.Star) ) {
                    op = OP_mult;

                    //When parenthesis follow a factor, that's an implicit multiplication
                }
                else if ( IsTypeOf(Token.LPar) ) {
                    op = OP_mult;

                    this.symStream.Next();

                    rhs = ParseTherm();

                    Expect(Token.RPar);

                    this.symStream.Stuff(1);

                }
                else if ( IsTypeOf(Token.Slash) ) {
                    op = OP_div;
                }
                else if ( IsTypeOf(Token.Percent) ) {
                    op = OP_mod;
                }

                if ( op == null )
                    return lhs;

                this.symStream.Next();

                if ( rhs is null )
                    rhs = ParseUnary();

                lhs = new Binary(GetSymbolPosition(), op, lhs, rhs);

                rhs = null;
            }
        }

        private Expression ParseUnary ( )
        {
            if ( IsTypeOf(Token.Plus) ) {

                //Unary plus does nothing
                this.symStream.Next();

                return ParseUnary();
            }
            else if ( IsTypeOf(Token.Minus) ) {

                this.symStream.Next();
                var rhs = ParseUnary();

                return new Unary(GetSymbolPosition(), OP_neg, rhs);
            }

            return ParseLeaf();
        }

        private Expression ParseLeaf ( )
        {
            //It's a number
            if ( IsTypeOf(Token.Number) ) {
                double v = this.symStream.Current.Value;
                this.symStream.Next();
                return new Const(GetSymbolPosition(), v);
            }
            else if ( IsTypeOf(Token.Identifier) ) {
                string id = this.symStream.Current.Id;
                int position = GetSymbolPosition();
                this.symStream.Next();

                //That's a function call
                if ( IsTypeOf(Token.LPar) ) {

                    List<Expression> args = new List<Expression>();

                    do {
                        this.symStream.Next();
                        if ( this.symStream.Current.Token == Token.RPar )
                            break;
                        args.Add(ParseTherm());

                    } while ( IsTypeOf(Token.Comma) );

                    Expect(Token.RPar);

                    return new FunctionCall(position, id, args.ToArray());

                }
                //That's variable
                else {
                    return new Accessor(position, id);
                }
            }
            //Parenthesis detected
            else if ( IsTypeOf(Token.LPar) ) {
                this.symStream.Next();

                var leaf = ParseTherm();

                Expect(Token.RPar);

                return leaf;
            }

            errors.Add(ErrorCodes.VALUE_EXPECTED(GetSymbolPosition()));
            return new Const(GetSymbolPosition(), 0);
        }

        private bool IsTypeOf (Token type) => this.symStream.Current.Token == type;

        private void Expect ( Token type)
        {
            if ( !IsTypeOf(type) )
                errors.Add(ErrorCodes.TOKEN_EXPECTED("Parser.Expect", type, symStream.Current.Token, GetSymbolPosition()));
            this.symStream.Next();
        }

        private int GetSymbolPosition ( ) => this.symStream.Current.Position + (this.symStream.Current.Token == Token.EOF ? 1 : 0);

        private static readonly Func<double, double, double> OP_add = (left, right) => left + right;
        private static readonly Func<double, double, double> OP_sub = (left, right) => left - right;
        private static readonly Func<double, double, double> OP_mult = (left, right) => left * right;
        private static readonly Func<double, double, double> OP_div = (left, right) => left / right;
        private static readonly Func<double, double, double> OP_mod = (left, right) => left % right;

        private static readonly Func<double, double> OP_neg = (right) => -right;
    }
}
