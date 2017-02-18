using ResxConverter.Mobile;

namespace ResxConverter.Runner
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
