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

Task("Patch-AssemblyInfo")
	.WithCriteria(isRunningOnAppVeyor)
	.Does(() =>
{
  CreateAssemblyInfo("./CommonAssemblyInfo.cs", new AssemblyInfoSettings
  {
    // Keep only the major and minor for assembly versions
    Version = version.Change(patch: 0).ToString()
  });
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
    .IsDependentOn("Patch-AssemblyInfo")
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
 	.WithCriteria(isRunningOnAppVeyor)
	.Does (() =>
{
  AppVeyor.UpdateBuildVersion($"{version.ToString()}-{AppVeyor.Environment.Repository.Branch}-build{AppVeyor.Environment.Build.Number}");

  var nugetVersion = AppVeyor.Environment.Repository.Branch == "master" ? version.ToString() : version.Change(prerelease: "pre" + AppVeyor.Environment.Build.Number).ToString();

  Package("./nuspec/ResxConverter.Core.nuspec", nugetVersion);
  Package("./nuspec/ResxConverter.CLI.nuspec", nugetVersion);
  Package("./nuspec/ResxConverter.Mobile.nuspec", nugetVersion);

});

Task("Default")
	.IsDependentOn("NuGet");

RunTarget(target);