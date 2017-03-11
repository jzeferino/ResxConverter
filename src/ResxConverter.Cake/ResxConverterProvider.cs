using ResxConverter.Mobile;

namespace ResxConverter.Cake
{
    public class ResxConverterProvider
    {
        public void ConvertToAndroid(string inputFolder, string outputFolder) => ResxConverters.Android.Convert(inputFolder, outputFolder);
        public void ConvertToiOS(string inputFolder, string outputFolder) => ResxConverters.iOS.Convert(inputFolder, outputFolder);
    }
}
