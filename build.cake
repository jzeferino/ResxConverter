#addin Cake.SemVer

#tool "xunit.runner.console&version=2.2.0"

// Enviroment
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;

// Arguments.
var target = Argument("target", "Default");
var configuration = "Release";

// Define directories.
var solutionFile = new FilePath("ResxConverter.sln");
var artifactsDirectory = new DirectoryPath("artifacts");

// Tests.
var testsDllPath = string.Format("./test/**/bin/{0}/*.Tests.dll", configuration);

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

Task("Run-Tests")
  .IsDependentOn("Build")
  .Does(() =>
{
  foreach(var testDll in GetFiles(testsDllPath))
  {
    XUnit2(testDll.FullPath, 
      new XUnit2Settings 
      {
        XmlReport = true,
        OutputDirectory = artifactsDirectory
      });
  }
});

Task ("NuGet")
	.IsDependentOn ("Run-Tests")
	.Does (() =>
{
  var sv = ParseSemVer (version);
  var nugetVersion = isRunningOnAppVeyor ? CreateSemVer(sv.Major, sv.Minor, sv.Patch).ToString() : sv.ToString();
    
  NuGetPack ("./nuspec/Cake.ResxConverter.nuspec", 
    new NuGetPackSettings 
      { 
        Version = nugetVersion,
        Verbosity = NuGetVerbosity.Normal,
        OutputDirectory = artifactsDirectory,
        BasePath = "./",
        ArgumentCustomization = args => args.Append("-NoDefaultExcludes")		
      });	
});

Task("Default")
	.IsDependentOn("NuGet") 
	.Does(() => {});

RunTarget(target);