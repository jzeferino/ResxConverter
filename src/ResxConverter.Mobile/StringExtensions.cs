using System.Text.RegularExpressions;

namespace ResxConverter.Mobile
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Convert camel-case to lower-case separated with underscore.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Lower case underscore string.</returns>
        public static string ToLowerUnderScoreFromCamelCase(this string value) => Regex.Replace(value, @"(\p{Ll})(\p{Lu})", "$1_$2").ToLower();
    }
}
