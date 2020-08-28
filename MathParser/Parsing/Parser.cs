using System;
using System.Collections.Generic;

using MathParser.Logging;
using MathParser.Parsing.Nodes;



namespace MathParser.Parsing
{
    using UnaryOP = System.Func<double, double>;
    using BinaryOP = System.Func<double, double, double>;

    public class Parser
    {
        private SymbolStream symStream;

        private readonly List<Error> errors = new List<Error>();
        private readonly AdvancedFormattingLogger logger;

        public Parser (Logger logger)
        {
            this.logger = new AdvancedFormattingLogger(logger, "Parse");
        }

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

        public Result<Expression> Parse (SymbolStream stream)
        {
            errors.Clear();

            symStream = stream;

            logger.OpenBranch("main");

            var final = ParseTerms();

            Expect(Token.EOF);

            logger.CloseBranch();

            return new Result<Expression>(final, errors);
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
            Func<Expression> lowerOPLevel = ParseFactors;


            logger.OpenBranch("terms");
            logger.Info("parse lhs");

            var lhs = lowerOPLevel();

            while ( true ) {
                BinaryOP op = null;

                //
                // +
                //
                if ( IsTypeOf(Token.Plus) ) {
                    op = OP_add;
                    logger.Info("found OP_add");

                }
                //
                // -
                //
                else if ( IsTypeOf(Token.Minus) ) {
                    op = OP_sub;
                    logger.Info("found OP_sub");
                }

                if ( op == null ) {
                    //Exit point
                    logger.CloseBranch();

                    return lhs;
                }

                symStream.Next();

                logger.Info("parse rhs");
                var rhs = lowerOPLevel();

                logger.Info("asm lhs");
                lhs = new Binary(GetSymbolPosition(), op, lhs, rhs);
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
            Func<Expression> lowerOPLevel = ParseUnary;


            logger.OpenBranch("factors");
            logger.Info("parse lhs");

            var lhs = lowerOPLevel();

            while ( true ) {
                BinaryOP op = null;

                //
                // *
                //
                if ( IsTypeOf(Token.Star) ) {
                    op = OP_mul;
                    logger.Info("found OP_mul");

                }
                //When parenthesis follow a factor, that's an implicit *
                else if ( IsTypeOf(Token.LPar) ) {
                    op = OP_mul;

                    //stuff stream to keep LPar
                    symStream.Stuff(1);

                    logger.Info("found implicit OP_mul");

                }
                //
                // /
                //
                else if ( IsTypeOf(Token.Slash) ) {
                    op = OP_div;
                    logger.Info("found OP_div");

                }
                //
                // %
                //
                else if ( IsTypeOf(Token.Percent) ) {
                    op = OP_mod;
                    logger.Info("found OP_mod");

                }

                if ( op == null ) {
                    //exit point
                    logger.CloseBranch();

                    return lhs;
                }

                symStream.Next();

                logger.Info("parse rhs");
                var rhs = lowerOPLevel();

                logger.Info("asm lhs");
                lhs = new Binary(GetSymbolPosition(), op, lhs, rhs);

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
            Func<Expression> lowerOPLevel = ParsePow;


            logger.OpenBranch("unary");

            //
            // +
            //
            if ( IsTypeOf(Token.Plus) ) {
                logger.Info("found plus");

                //Unary plus does nothing
                symStream.Next();

                return ParseUnary();
            }
            //
            // -
            //
            else if ( IsTypeOf(Token.Minus) ) {
                logger.Info("found minus");

                symStream.Next();

                var rhs = ParseUnary();

                return new Unary(GetSymbolPosition(), OP_neg, rhs);
            }

            logger.Info("parse leaf");
            var leaf = lowerOPLevel();

            logger.CloseBranch();
            return leaf;
        }

        #region pow in construct

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
            Func<Expression> lowerOPLevel = ParseLeaf;

            logger.OpenBranch("pow");
            logger.Info("parse lhs");

            var lhs = lowerOPLevel();

            //
            // ^
            //
            if ( IsTypeOf(Token.Exp) ) {

                symStream.Next();

                logger.Info("parse rhs (recursion)");

                var rhs = ParseUnary();

                logger.CloseBranch();

                return new Binary(symStream.Current.Position, OP_pow, lhs, rhs);
            }

            return lhs;
        }

        #endregion

        //
        // level 4.5
        //
        // act as values
        //
        // higher priority
        //

        private Expression ParseLeaf ( )
        {
            logger.OpenBranch("leaf");

            //It's a number
            if ( IsTypeOf(Token.Number) ) {
                double v = symStream.Current.Value;
                symStream.Next();

                logger.Info("found number (" + v + ")");
                logger.CloseBranch();

                return new Const(GetSymbolPosition(), v);
            }
            //It's a name/identifier (function or property)
            else if ( IsTypeOf(Token.Identifier) ) {
                string id = symStream.Current.Id;
                int position = GetSymbolPosition();
                symStream.Next();

                //That's a function call
                if ( IsTypeOf(Token.LPar) ) {
                    logger.Info("found function (" + id + ")");

                    List<Expression> args = new List<Expression>();

                    do {
                        symStream.Next();
                        if ( symStream.Current.Token == Token.RPar )
                            break;
                        args.Add(ParseTerms());

                    } while ( IsTypeOf(Token.Comma) );

                    Expect(Token.RPar);

                    logger.Info(args.Count + " args");
                    logger.CloseBranch();


                    return new FunctionCall(position, id, args);

                }
                //That's variable
                else {
                    logger.Info("found property (" + id + ")");
                    logger.CloseBranch();

                    return new Accessor(position, id);
                }
            }
            //Parentheses detected
            else if ( IsTypeOf(Token.LPar) ) {
                symStream.Next();

                logger.Info("found parentheses");


                var leaf = ParseTerms();

                Expect(Token.RPar);

                logger.CloseBranch();

                return leaf;
            }

            logger.Err("Value expected !");

            errors.Add(ErrorCodes.VALUE_EXPECTED(GetSymbolPosition()));

            logger.CloseBranch();

            return new Const(GetSymbolPosition(), 0);
        }

        /*
         *  Helper functions
         */

        private bool IsTypeOf (Token type) => symStream.Current.Token == type;

        /// <summary>
        /// Loop if the given type is the current type, and otherwise create an error
        /// </summary>
        private void Expect (Token type)
        {
            if ( !IsTypeOf(type) )
                errors.Add(ErrorCodes.TOKEN_EXPECTED("Parser.Expect", type, symStream.Current.Token, GetSymbolPosition()));
            symStream.Next();
        }

        //avoid some problemes with EOF token
        private int GetSymbolPosition ( ) => symStream.Current.Position + (symStream.Current.Token == Token.EOF ? 1 : 0);

        //plus
        private static readonly BinaryOP OP_add = (left, right) => left + right;
        //minus
        private static readonly BinaryOP OP_sub = (left, right) => left - right;
        //multiplication
        private static readonly BinaryOP OP_mul = (left, right) => left * right;
        //division
        private static readonly BinaryOP OP_div = (left, right) => left / right;
        //modulo
        private static readonly BinaryOP OP_mod = (left, right) => left % right;
        //power
        private static readonly BinaryOP OP_pow = (left, right) => Math.Pow(left, right);
        //negation
        private static readonly UnaryOP OP_neg = (right) => -right;
    }
}
