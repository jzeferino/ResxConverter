using ResxParser;

namespace ResxConverterRunner
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var resxFolder = "Resources";
            var androidOutputFile = "res";
            var iosOutputFile = "Resources";

            ResxConverter.Instance.ConvertToAndroid(resxFolder, androidOutputFile);
            ResxConverter.Instance.ConvertToiOS(resxFolder, iosOutputFile);
        }
    }
}
