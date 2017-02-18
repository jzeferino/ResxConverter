using ResxConverter.Mobile;

namespace ResxConverterRunner
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var resxFolder = "Resources";
            var androidOutputFile = "res";
            var iosOutputFile = "Resources";

            ResxMobileConverters.ConvertToAndroid(resxFolder, androidOutputFile);
            ResxMobileConverters.ConvertToiOS(resxFolder, iosOutputFile);
        }
    }
}
