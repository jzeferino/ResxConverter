using ResxConverter.Core;
using System;
using System.IO;

namespace ResxConverter.Mobile
{
    public class iOSResxConverterOutput : IResxConverterOutput
    {
        public string OutputFilePath { get; }
        private readonly StreamWriter _streamWriter;

        public iOSResxConverterOutput(string outputFolder, string culture)
        {
            if (outputFolder == null)
            {
                throw new ArgumentNullException(nameof(outputFolder));
            }

            culture = string.IsNullOrEmpty(culture) ? "Base" : culture;
            OutputFilePath = Path.Combine(outputFolder, $"{culture}.Iproj", "Localizable.strings");
            Directory.CreateDirectory(Path.GetDirectoryName(OutputFilePath));
            _streamWriter = new StreamWriter(OutputFilePath);
        }

        public void Dispose() => _streamWriter.Dispose();

        public void WriteComment(string comment) => _streamWriter.WriteLine($"/* {comment} */");

        public void WriteString(ResxString stringElement) => _streamWriter.WriteLine($"\"{stringElement.Key.ToLowerUnderScoreFromCamelCase()}\" = \"{stringElement.Value}\";");
    }
}