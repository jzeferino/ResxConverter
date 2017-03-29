[![Build status](https://ci.appveyor.com/api/projects/status/ig9llpalkl1hynxh?svg=true
)](https://ci.appveyor.com/project/jzeferino/resxconverter/)   

ResxConverter
===================

<p align="center">
  <img src="https://github.com/jzeferino/ResxConverter/blob/master/art/icon.png?raw=true"/>
</p>

ResxConverter is a tool that helps convert resx files to any format.

ResxConverter is shipped into three main packages.
* The Core, that has the all the resx parsing code and allows you to extend the convertion to any format.
* The Mobile, that uses the Core and adds support for iOS and Android convertion.
* The Cake, that wraps the Core and Mobile (or future packages) and allows its use inside the Cake enviroment.

| Core.ResxConverter | CLI.ResxConverter | Cake.ResxConverter |
|    :---:     |     :---:      |     :---:     |
| [![NuGet](https://img.shields.io/nuget/v/Core.ResxConverter.svg?label=NuGet)](https://www.nuget.org/packages/Core.ResxConverter/)   |[![NuGet](https://img.shields.io/nuget/v/CLI.ResxConverter.svg?label=NuGet)](https://www.nuget.org/packages/CLI.ResxConverter/)     | [![NuGet](https://img.shields.io/nuget/v/Cake.ResxConverter.svg?label=NuGet)](https://www.nuget.org/packages/Cake.ResxConverter/)    |

### Usage from Cake
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

### Usage from code

The converters for mobile platforms can be used via the `ResxConverters` class. You'll need to reference `ResxConverter.Mobile` and `ResxConverter.Core`.

```c#
ResxConverters.Android.Convert(resxFolder, "artifacts/generated/android");
ResxConverters.iOS.Convert(resxFolder, "artifacts/generated/ios");
```

### How to extend the Core

ResxConverter can be extended by defining new types of outputs. To that end, a new `IResxConverterOutput` should be defined.

```c#
public class CustomResxOutput : IResxConverterOutput
{
    public void Dispose() { ... }

    public void WriteComment(string comment) { ... }

    public void WriteString(ResxString stringElement) { ... }
}
```

Note that outputs must implement `IDisposable` and they will be disposed after all content is written.

ResxConverter aggregates content by culture. This means that a new `IResxConverterOutput` is requested for each culture being processed from the source resx files. Outputs are created via the `IResxConverterOutputFactory` interface.

```c#
public interface IResxConverterOutputFactory
{
    IResxConverterOutput Create(string culture, string outputFolder);
}
```

You can define a custom factory or reuse the built in `ResxConverterOutputFactory` that accepts a lambda. Finally, you use the factory to create an instance of `ResxConverter`.

```c#
var converter = new ResxConverter(new ResxConverterOutputFactory((culture, outputFolder) => new CustomResxOutput(outputFolder, culture)));
```

### Remarks

### License
[MIT Licence](LICENSE) 
