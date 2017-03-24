#r src/Cake.ResxConverter/bin/Release/ResxConverter.Core.dll
#r src/Cake.ResxConverter/bin/Release/ResxConverter.Mobile.dll
#r src/Cake.ResxConverter/bin/Release/Cake.ResxConverter.dll
using ResxConverter.Mobile;

Task("Run")
  .Does(() =>
{
  CleanDirectory("artifacts/generated");

  var resxFolder = "test/ResxConverter.Mobile.Tests/Resources";
  ResxConverters.Android.Convert(resxFolder, "artifacts/generated/android");
  ResxConverters.iOS.Convert(resxFolder, "artifacts/generated/ios");
});

// #addin nuget:?package=Cake.ResxConverter&prerelease

// Task("Run")
//   .Does(() =>
// {
//   var resxFolder = "test/ResxConverter.Mobile.Tests/Resources";
//   ResxConverter.ConvertToAndroid(resxFolder, "artifacts/generated/android");
//   ResxConverter.ConvertToiOS(resxFolder, "artifacts/generated/ios");
// });

RunTarget("Run");