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

            ResxConverters.Android.Convert(resxFolder, androidOutputFile);
            ResxConverters.iOS.Convert(resxFolder, iosOutputFile);
        }
    }
}
