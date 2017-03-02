using ResxConverter.Core;
using System.IO;

namespace ResxConverter.Mobile
{
    public class iOSResxConverterOutput : IResxConverterOutput
    {
        public string OutputFilePath { get; }
        private readonly StreamWriter _streamWriter;

        public iOSResxConverterOutput(string outputProjectFolder, string culture)
        {
            culture = string.IsNullOrEmpty(culture) ? "Base" : culture;
            OutputFilePath = Path.Combine(outputProjectFolder, $"{culture}.Iproj", "Localizable.strings");
            Directory.CreateDirectory(Path.GetDirectoryName(OutputFilePath));
            _streamWriter = new StreamWriter(OutputFilePath);
        }

        public void Dispose() => _streamWriter.Dispose();

        public void WriteComment(string comment) => _streamWriter.WriteLine($"/* {comment} */");

        public void WriteString(ResxString stringElement) => _streamWriter.WriteLine($"\"{stringElement.Key.ToLowerUnderScoreFromCamelCase()}\" = \"{stringElement.Value}\";");
    }
}