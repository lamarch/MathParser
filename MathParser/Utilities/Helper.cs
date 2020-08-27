using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MathParser.Utilities
{
    public static class Helper
    {
        public static int LineNumber ([CallerLineNumber] int line = -1) => line;
        public static string MemberName ([CallerMemberName] string name = "unknown") => name;
        public static string FilePath ([CallerFilePath] string fPath = "unknown") => fPath;

        public static string Hash (string str)
        {
            HashAlgorithm algo = SHA256.Create();
            return Encoding.ASCII.GetString(algo.ComputeHash(Encoding.UTF8.GetBytes(str)));
        }

        private static readonly Regex _onlyLetters = new Regex("[^a-zA-Z_]");

        public static bool IsValidName(string str)
        {
            return !_onlyLetters.IsMatch(str);
        }
    }
}
