

namespace MathParser.Parsing
{
    using System.Collections.Generic;
    using System.Linq;

    using MathParser.Parsing.Nodes;
    using MathParser.Parsing.Nodes.BinaryNodes;
    using MathParser.Parsing.Nodes.UnaryNodes;
    using MathParser.Tokenisation.Unused;

    public class Parser
    {
        private NewLexer lexer;

        private readonly List<Error> errors = new List<Error>();

        //this parser work from left to right

        /*
         * the logging system use 'branches' for hierarchy
         */


        /*
         * NOTES ABOUT OPERATOR PRECEDENCE & ASSOCIATIVITY :
         * 
         * --> PRECEDENCE (functions order) :
         * + -
         * * / %
         * + - [as unary]
         * %
         * number, identifiers and parentheses
         * 
         * --> ASSOCIATIVITY (parsing by loop or by recursion) :
         * 
         * + - * / % are left-associative operators
         * ^ is right-associative operator
         * 
         */

        public Result<Expression> Parse (NewLexer lexer)
        {
            errors.Clear();

            this.lexer = lexer;

            var final = ParseTerms();

            Expect(Token.EOF);


            return new Result<Expression>(final, errors.Concat(this.lexer.Errors).ToList());
        }

        //
        // level 1
        //
        // + and - operators
        //
        // lower priority
        //
        // call parse factors then
        //
        private Expression ParseTerms ( )
        {

            var lhs = ParseFactors();

            while ( true ) {
                Expression rhs_;

                Token sign;

                switch ( lexer.Current.Token ) {
                    case Token.Plus:
                    case Token.Minus:
                        sign = lexer.Current.Token;
                        lexer.Next();
                        rhs_ = ParseFactors();
                        break;
                    default:
                        return lhs;

                }

                lhs = sign switch
                {
                    Token.Plus => new Add(lexer.Current.Position, lhs, rhs_),
                    Token.Minus => new Sub(lexer.Current.Position, lhs, rhs_),
                    _ => null,
                };

            }
        }

        //
        // level 2
        //
        // * and / and % operators
        //
        // medium priority
        //
        // call parse unary then
        //
        private Expression ParseFactors ( )
        {

            Expression lhs = ParseUnary();

            while ( true ) {
                Expression rhs_;

                Token sign;

                switch ( lexer.Current.Token ) {
                    case Token.Star:
                    case Token.Slash:
                    case Token.Percent:
                        sign = lexer.Current.Token;
                        lexer.Next();
                    Next:
                        rhs_ = ParseUnary();
                        break;
                    case Token.LPar:
                        sign = Token.Star;
                        goto Next;
                    default:
                        return lhs;

                }

                lhs = sign switch
                {
                    Token.Star => new Mul(GetSymbolPosition(), lhs, rhs_),
                    Token.Slash => new Div(GetSymbolPosition(), lhs, rhs_),
                    Token.Percent => new Mod(GetSymbolPosition(), lhs, rhs_),
                    _ => null,
                };

            }
        }

        //
        // level 3
        //
        // + and - operators as unary
        //
        // high priority
        //
        // call parse leaf then
        //

        private Expression ParseUnary ( )
        {
            switch ( lexer.Current.Token ) {
                case Token.Plus:
                    lexer.Next();
                    return ParseUnary();
                case Token.Minus:
                    lexer.Next();
                    //recursion
                    return new Neg(GetSymbolPosition(), ParseUnary());
                default:
                    return ParsePow();
            }
        }


        //
        // level 4
        //
        // ^ operator
        //
        // --> right associative (recursion instead of loop)
        //
        // very high priority
        //
        // call parse leaf then
        //
        private Expression ParsePow ( )
        {


            var lhs = ParseLeaf();

            //
            // ^
            //
            if ( IsTypeOf(Token.Exp) ) {

                lexer.Next();


                var rhs = ParseUnary();


                return new Pow(GetSymbolPosition(), lhs, rhs);
            }

            return lhs;
        }


        //
        // level 4.5
        //
        // act as values
        //
        // higher priority
        //

        private Expression ParseLeaf ( )
        {
            //logger.OpenBranch("leaf");

            //It's a number
            if ( IsTypeOf(Token.Number) ) {
                double v = lexer.Current.Value;
                lexer.Next();


                return new Const(GetSymbolPosition(), v);
            }
            //It's a name/identifier (function or property)
            else if ( IsTypeOf(Token.Identifier) ) {
                string id = lexer.Current.Id;
                int position = GetSymbolPosition();
                lexer.Next();

                //That's a function call
                if ( IsTypeOf(Token.LPar) ) {

                    List<Expression> args = new List<Expression>();

                    do {
                        lexer.Next();
                        if ( lexer.Current.Token == Token.RPar )
                            break;
                        args.Add(ParseTerms());

                    } while ( IsTypeOf(Token.Comma) );

                    Expect(Token.RPar);



                    return new FunctionCall(position, id, args);

                }
                //That's variable
                else {


                    return new Accessor(position, id);
                }
            }
            //Parentheses detected
            else if ( IsTypeOf(Token.LPar) ) {
                lexer.Next();



                var leaf = ParseTerms();

                Expect(Token.RPar);


                return leaf;
            }


            errors.Add(ErrorCodes.VALUE_EXPECTED(GetSymbolPosition(), lexer.Current.Token));


            return new Const(GetSymbolPosition(), 0);
        }

        /*
         *  Helper functions
         */

        private bool IsTypeOf (Token type) => lexer.Current.Token == type;


        /// <summary>
        /// Loop if the given type is the current type, and otherwise create an error
        /// </summary>
        private void Expect (Token type)
        {
            if ( !IsTypeOf(type) )
                errors.Add(ErrorCodes.TOKEN_EXPECTED("Parser.Expect", type, lexer.Current.Token, GetSymbolPosition()));
            lexer.Next();
        }

        //avoid some problemes with EOF token
        private int GetSymbolPosition ( ) => lexer.Current.Position + (lexer.Current.Token == Token.EOF ? 1 : 0);

    }
}
