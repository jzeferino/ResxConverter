namespace ResxParser
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            var resxFolder = "Resources";
            var outputFile = "res";
            ResxConverter.Instance.ConvertToAndroid(resxFolder, outputFile);
        }
    }
}
