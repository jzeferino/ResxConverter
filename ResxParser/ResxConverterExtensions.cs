using System;
namespace ResxParser
{
    public static class ResxConverterExtensions
    {
        public static void ConvertToAndroid(this ResxConverter converter, string folder, string outputProjectFolder)
        {
            converter.Convert(folder, culture => new AndroidResxConverterOutput(outputProjectFolder, culture));
        }
    }
}
