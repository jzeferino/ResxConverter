#addin nuget:?package=Cake.SemVer&version=3.0.0&loaddependencies=true

#tool "xunit.runner.console&version=2.2.0"

// Enviroment
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;

// Arguments.
var target = Argument("target", "Default");
var configuration = "Release";

// Define directories.
var solutionFile = File("ResxConverter.sln");
var artifactsDirectory = Directory("artifacts");

// Tests.
var testsDllPath = string.Format("./test/**/bin/{0}/*.Tests.dll", configuration);

// Versioning. Used for all the packages and assemblies for now.
var version = CreateSemVer(1, 0, 1);
var nugetVersion = version.Change(prerelease: "local" + DateTime.Now.Ticks);

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
  var setings = new DotNetCoreBuildSettings
  {
    Configuration = configuration,
    NoRestore = true,
    Verbosity = DotNetCoreVerbosity.Minimal,
    MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersion(version.ToString())
  };

  DotNetCoreBuild(solutionFile, setings);
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

Task("Update-AppVeyor-Version")
  .WithCriteria(isRunningOnAppVeyor)
  .Does(() =>
{
  AppVeyor.UpdateBuildVersion($"{version}-{AppVeyor.Environment.Repository.Branch}-build{AppVeyor.Environment.Build.Number}");
  nugetVersion = AppVeyor.Environment.Repository.Branch == "master" ? version : version.Change(prerelease: "pre" + AppVeyor.Environment.Build.Number);
});

Task ("NuGet")
	.IsDependentOn ("Update-AppVeyor-Version")
	.IsDependentOn ("Run-Tests")
	.Does (() =>
{
  var settings = new DotNetCorePackSettings
  {
      OutputDirectory = artifactsDirectory,
      WorkingDirectory = "./",
      NoBuild = true,
      NoRestore = true,
      Configuration = configuration,
      MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersion(nugetVersion.ToString())
  };
 
  DotNetCorePack("src/ResxConverter.Core/ResxConverter.Core.csproj", settings);
  DotNetCorePack("src/ResxConverter.Mobile/ResxConverter.Mobile.csproj", settings);
  DotNetCorePack("src/ResxConverter.CLI/ResxConverter.CLI.csproj", settings);
});

Task("Default")
	.IsDependentOn("NuGet");

RunTarget(target);