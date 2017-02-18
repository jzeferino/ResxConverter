using System.IO;
using System.Text.RegularExpressions;

namespace ResxConverter.Core
{
    internal static class ResxConverterExtensions
    {
        public static string GetExtensionWitoutDot(this string fileName) => Path.GetExtension(fileName).Replace(".", string.Empty);
    }
}
