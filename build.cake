#r ResxParser/bin/Debug/ResxParser.exe
using ResxParser;

Task("Default")
  .Does(() =>
{
    var resxFolder = "ResxParser/Resources";
    var androidOutputFile = "artifacts/res";
    var iosOutputFile = "artifacts/Resources";

    ResxConverter.Instance.ConvertToAndroid(resxFolder, androidOutputFile);
    ResxConverter.Instance.ConvertToiOS(resxFolder, iosOutputFile);
});

RunTarget("Default");