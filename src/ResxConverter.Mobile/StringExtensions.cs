using System.Text.RegularExpressions;

namespace ResxConverter.Mobile
{
    public static class StringExtensions
    {
        /// <summary>
        /// Convert camel case to lower case separated with underscore.
        /// </summary>
        /// <returns>Lower case underscore string.</returns>
        /// <param name="value">Value.</param>
        public static string ToLowerUnderScoreFromCamelCase(this string value) => Regex.Replace(value, @"(\p{Ll})(\p{Lu})", "$1_$2").ToLower();
    }
}
