using System.IO;

namespace ResxConverter.Core
{
    internal static class StringExtensions
    {
        public static string GetExtensionWitoutDot(this string fileName) => Path.GetExtension(fileName).Replace(".", string.Empty);
    }
}
