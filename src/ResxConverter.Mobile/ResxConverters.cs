namespace ResxConverter.Mobile
{
    public static class ResxConverters
    {
        public static readonly Core.ResxConverter Android = new Core.ResxConverter(new Core.ResxConverterOutputFactory((culture, outputFolder) => new AndroidResxConverterOutput(outputFolder, culture)));
        public static readonly Core.ResxConverter iOS = new Core.ResxConverter(new Core.ResxConverterOutputFactory((culture, outputFolder) => new iOSResxConverterOutput(outputFolder, culture)));
    }
}
