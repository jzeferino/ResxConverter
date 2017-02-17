using System;
using System.Text.RegularExpressions;

namespace ResxParser
{
    public static class ResxConverterExtensions
    {
        public static void ConvertToAndroid(this ResxConverter converter, string folder, string outputProjectFolder)
        {
            converter.Convert(folder, culture => new AndroidResxConverterOutput(outputProjectFolder, culture));
        }

        public static string ToLowerUnderScoreFromCamelCase(this string value) => Regex.Replace(value, @"(\p{Ll})(\p{Lu})", "$1_$2").ToLower();
    }
}
