[![Build status](https://ci.appveyor.com/api/projects/status/ig9llpalkl1hynxh?svg=true
)](https://ci.appveyor.com/project/jzeferino/resxconverter/)   

ResxConverter
===================

<p align="center">
  <img src="https://github.com/jzeferino/ResxConverter/blob/master/art/icon.png?raw=true"/>
</p>

ResxConverter is a tool that helps convert resx files to any format.

ResxConverter is shipped into three main packages.
* Core - includes all the resx parsing code and allows extending the conversion to any format.
* Cake - allows its use in a [Cake](http://cakebuild.net/) script.
* CLI - allows its use from the command-line.

Both the Cake and CLI packages include all the available converters, namely support for conversion to **iOS and Android resource files**.

| ResxConverter.Core | ResxConverter.CLI | Cake.ResxConverter | ResxConverter.Mobile |
|    :---:     |     :---:      |     :---:     |     :---:     |
| [![NuGet](https://img.shields.io/nuget/v/ResxConverter.Core.svg?label=NuGet)](https://www.nuget.org/packages/ResxConverter.Core/)   | [![NuGet](https://img.shields.io/nuget/v/ResxConverter.CLI.svg?label=NuGet)](https://www.nuget.org/packages/ResxConverter.CLI/)     | [![NuGet](https://img.shields.io/nuget/v/Cake.ResxConverter.svg?label=NuGet)](https://www.nuget.org/packages/Cake.ResxConverter/)    | [![NuGet](https://img.shields.io/nuget/v/ResxConverter.Mobile.svg?label=NuGet)](https://www.nuget.org/packages/ResxConverter.Mobile/)    |

### Usage from Cake
```c#
#addin "Cake.ResxConverter"

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

### Usage from command-line

ResxConverter can be used from the command line via the `ResxConverter.CLI` package.

```
Install-Package ResxConverter.CLI
```

On the command line, converters can be invoked as follows:

```
ResxConverter.CLI android ./resources ./generated
```

For complete usage information invoke the CLI without any arguments.

### How to extend the Core

ResxConverter core interfaces are defined in the `ResxConverter.Core` package.

```
Install-Package ResxConverter.Core
```

The library can be extended by defining new types of outputs. To that end, a new `IResxConverterOutput` should be defined.

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

### License
[MIT Licence](LICENSE) 
