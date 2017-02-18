#r ResxParser/bin/Debug/ResxParser.dll
using ResxParser;

Task("Default")
  .Does(() =>
{
    var resxFolder = "ResxConverterRunner/Resources";
    var androidOutputFile = "artifacts/res";
    var iosOutputFile = "artifacts/Resources";

    ResxConverter.Instance.ConvertToAndroid(resxFolder, androidOutputFile);
    ResxConverter.Instance.ConvertToiOS(resxFolder, iosOutputFile);
});

RunTarget("Default");