#r src/ResxConverter.Mobile/bin/Release/ResxConverter.Mobile.dll
using ResxConverter.Mobile;

#addin Cake.SemVer

// Enviroment
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;

// Arguments.
var target = Argument("target", "Default");
var configuration = "Release";

// Define directories.
var solutionFile = new FilePath("ResxConverter.sln");
var artifactsDirectory = new DirectoryPath("artifacts");

// Versioning.
var version = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "9.9.9-build9");

Setup((context) =>
{
	Information("AppVeyor: {0}", isRunningOnAppVeyor);
	Information("Configuration: {0}", configuration);
});

Task("Clean")
	.Does(() =>
{	
  CleanDirectory(artifactsDirectory);

  DotNetBuild(solutionFile, settings => settings
      .SetConfiguration(configuration)
      .WithTarget("Clean")
      .SetVerbosity(Verbosity.Minimal));
});

Task("Restore")
	.Does(() => 
{
  NuGetRestore(solutionFile);
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() =>  
{	
  DotNetBuild(solutionFile, settings => settings
        .SetConfiguration(configuration)
        .WithTarget("Build")
        .SetVerbosity(Verbosity.Minimal));
});

Task ("NuGet")
	.IsDependentOn ("Build")
	.Does (() =>
{
  var sv = ParseSemVer (version);
  var nugetVersion = CreateSemVer (sv.Major, sv.Minor, sv.Patch).ToString();
  
  NuGetPack ("./nuspec/ResxConverter.nuspec", 
    new NuGetPackSettings 
      { 
        Version = nugetVersion,
        Verbosity = NuGetVerbosity.Normal,
        OutputDirectory = artifactsDirectory,
        BasePath = "./",
        ArgumentCustomization = args => args.Append("-NoDefaultExcludes")		
      });	
});

Task("RunResxConverter")
  .Does(() =>
{
    var resxFolder = "src/ResxConverter.Runner/Resources";
    var androidOutputFile = "artifacts/res";
    var iosOutputFile = "artifacts/Resources";

    ResxMobileConverters.ConvertToAndroid(resxFolder, androidOutputFile);
    ResxMobileConverters.ConvertToiOS(resxFolder, iosOutputFile);
});

Task("Default")
	.IsDependentOn("Build")
  .IsDependentOn("RunResxConverter")  
	.Does(() => {});

RunTarget("Default");