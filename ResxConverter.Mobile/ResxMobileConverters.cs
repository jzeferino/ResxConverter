namespace ResxConverter.Mobile
{
    public static class ResxMobileConverters
    {
        public static void ConvertToAndroid(string folder, string outputProjectFolder)
        {
            Core.ResxConverter.Instance.Convert(folder, culture => new AndroidResxConverterOutput(outputProjectFolder, culture));
        }

        public static void ConvertToiOS(string folder, string outputProjectFolder)
        {
            Core.ResxConverter.Instance.Convert(folder, culture => new iOSResxConverterOutput(outputProjectFolder, culture));
        }
    }
}
