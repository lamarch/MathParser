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
        //Exp,

        Comma,
        AntiSlash,

        LPar,
        RPar,

        Number,
        Identifier,
    }
}
