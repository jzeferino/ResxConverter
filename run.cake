#r src/ResxConverter.Mobile/bin/Release/ResxConverter.Mobile.dll
#r src/ResxConverter.Mobile/bin/Release/ResxConverter.dll
using ResxConverter.Mobile;

Task("RunResxConverter")
  .Does(() =>
{
    var resxFolder = "src/ResxConverter.Runner/Resources";
    var androidOutputFile = "artifacts/res";
    var iosOutputFile = "artifacts/Resources";

    ResxConverters.Android.Convert(resxFolder, androidOutputFile);
    ResxConverters.iOS.Convert(resxFolder, iosOutputFile);
});

RunTarget("RunResxConverter");