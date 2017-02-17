using System.IO;
using System.Text.RegularExpressions;

namespace ResxParser
{
    public static class ResxConverterExtensions
    {
        public static void ConvertToAndroid(this ResxConverter converter, string folder, string outputProjectFolder)
        {
            converter.Convert(folder, culture => new AndroidResxConverterOutput(outputProjectFolder, culture));
        }

        public static void ConvertToiOS(this ResxConverter converter, string folder, string outputProjectFolder)
        {
            converter.Convert(folder, culture => new iOSResxConverterOutput(outputProjectFolder, culture));
        }
        public static string ToLowerUnderScoreFromCamelCase(this string value) => Regex.Replace(value, @"(\p{Ll})(\p{Lu})", "$1_$2").ToLower();

        public static string GetExtensionWitoutDot(this string fileName) => Path.GetExtension(fileName).Replace(".", string.Empty);
    }
}
