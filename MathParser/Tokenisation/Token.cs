namespace MathParser
{
    public enum Token
    {
        Error,
        Null,
        EOF,

        Plus,
        Minus,
        Star,
        Percent,
        Slash,
        And,
        Or,

        Exp,
        Exclamation,

        Comma,
        AntiSlash,

        LPar,
        RPar,

        Number,
        Identifier,
    }
}
