[![Build status](https://ci.appveyor.com/api/projects/status/ig9llpalkl1hynxh?svg=true
)](https://ci.appveyor.com/project/jzeferino/resxconverter/)   

ResxConverter
===================

ResxConverter is a tool that helps convert resx files to any format.

ResxConverter is shipped into three main packages.
* The Core, that has the all the resx parsing code and allows you to extend the convertion to any format.
* The Mobile, that uses the Core and adds support for iOS and Android convertion.
* The Cake, that wraps the Core and Mobile (or future packages) and allows its use inside the Cake enviroment.

| Core.ResxConverter | Mobile.ResxConverter | Cake.ResxConverter |
|    :---:     |     :---:      |     :---:     |
| [![NuGet](https://img.shields.io/nuget/v/Core.ResxConverter.svg?label=NuGet)](https://www.nuget.org/packages/Core.ResxConverter/)   |[![NuGet](https://img.shields.io/nuget/v/Mobile.ResxConverter.svg?label=NuGet)](https://www.nuget.org/packages/Mobile.ResxConverter/)     | [![NuGet](https://img.shields.io/nuget/v/Cake.ResxConverter.svg?label=NuGet)](https://www.nuget.org/packages/Cake.ResxConverter/)    |

### Usage from Cake:
```c#
#addin nuget:?package=Cake.ResxConverter&prerelease

Task("Run")
  .Does(() =>
{
  // The path for the folder with resx files.
  var resxFolder = "test/ResxConverter.Mobile.Tests/Resources"; 
  
  // Convert the resx files to android into the specified folder.
  ResxConverter.ConvertToAndroid(resxFolder, "artifacts/generated/android");
  
  // Convert the resx files to ios into the specified folder.
  ResxConverter.ConvertToiOS(resxFolder, "artifacts/generated/ios");
});
```

### Usage from code:

### How to extend the Core:

### Remarks:

### License
[MIT Licence](LICENSE) 
