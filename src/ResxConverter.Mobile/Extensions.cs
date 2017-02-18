using System.Text.RegularExpressions;

namespace ResxConverter.Mobile
{
    public static class Extensions
    {
        public static string ToLowerUnderScoreFromCamelCase(this string value) => Regex.Replace(value, @"(\p{Ll})(\p{Lu})", "$1_$2").ToLower();
    }
}
