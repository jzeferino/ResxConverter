#addin Cake.SemVer
#addin nuget:?package=semver&version=2.0.4

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

// Versioning. Used for all the packages and assemblies for now.
var version = CreateSemVer(1, 0, 1);

// Reusable Packaging
Action<string, string> Package = (nuspec, nugetVersion) =>
{
    NuGetPack (nuspec, 
    new NuGetPackSettings 
      { 
        Version = nugetVersion,
        Verbosity = NuGetVerbosity.Normal,
        OutputDirectory = artifactsDirectory,
        BasePath = "./",
        ArgumentCustomization = args => args.Append("-NoDefaultExcludes")		
      });	
};

Setup(context =>
{
	Information("AppVeyor: {0}", isRunningOnAppVeyor);
	Information("Configuration: {0}", configuration);
  Information("Version: {0}", version);
});

Task("Clean")
	.Does(() =>
{	
  CleanDirectory(artifactsDirectory);

  MSBuild(solutionFile, settings => settings
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
  MSBuild(solutionFile, settings => settings
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
  //AppVeyor.UpdateBuildVersion(string.Format("{0}-{1}-build{2}", version.ToString(), AppVeyor.Environment.Repository.Branch, AppVeyor.Environment.Build.Number));

  var nugetVersion = "2.0.0";//AppVeyor.Environment.Repository.Branch == "master" ? version.ToString() : version.Change(prerelease: "pre" + AppVeyor.Environment.Build.Number).ToString();

  var settings = new DotNetCorePackSettings
  {
      OutputDirectory = artifactsDirectory,
      WorkingDirectory = "./",
      NoBuild = true,
      NoRestore = true,
      Configuration = configuration
  };

  settings.MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersion(nugetVersion);
  DotNetCorePack("src/ResxConverter.Core/ResxConverter.Core.csproj", settings);
  DotNetCorePack("src/ResxConverter.Mobile/ResxConverter.Mobile.csproj", settings);
  DotNetCorePack("src/ResxConverter.CLI/ResxConverter.CLI.csproj", settings);

});

Task("Default")
	.IsDependentOn("NuGet");

RunTarget(target);