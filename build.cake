#r ResxConverter.Mobile/bin/Debug/ResxConverter.Mobile.dll
using ResxConverter.Mobile;

Task("Default")
  .Does(() =>
{
    var resxFolder = "ResxConverter.Runner/Resources";
    var androidOutputFile = "artifacts/res";
    var iosOutputFile = "artifacts/Resources";

    ResxMobileConverters.ConvertToAndroid(resxFolder, androidOutputFile);
    ResxMobileConverters.ConvertToiOS(resxFolder, iosOutputFile);
});

RunTarget("Default");