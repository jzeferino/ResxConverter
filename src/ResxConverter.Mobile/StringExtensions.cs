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

        /// <summary>
        /// Escapes the " ' \ \n characteres.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="includeSingleQuotes">True to escape single quotes; false otherwise.</param>
        /// <returns>Escpaed string</returns>
        public static string EscapeSpecialCharacters(this string value, bool includeSingleQuotes)
        {
            // Search for " ' \ or \n using the regex "|\\|\n|'
            var regex = "\"|\\\\|\n";
            if (includeSingleQuotes)
            {
                regex += "|'";
            }

            return Regex.Replace(value, regex, EscapeSpecialCharacters);
        }

        private static string EscapeSpecialCharacters(Match m) => m.Value == "\n" ? "\\n" : '\\' + m.Value;
    }
}
