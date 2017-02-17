using System.Xml.Linq;
using System.IO;

namespace ResxParser
{
    public class iOSResxConverterOutput : IResxConverterOutput
    {
        private string _culture;
        private string _outputProjectFolder;
        private StreamWriter _streamWriter;
        private string _finalOutPutPath;

        public iOSResxConverterOutput(string outputProjectFolder, string culture)
        {
            _outputProjectFolder = outputProjectFolder;
            _culture = culture;

            CreateFile();
        }

        private void CreateFile()
        {
            var cultureInfo = string.IsNullOrEmpty(_culture) ? "Base" : _culture;

            _finalOutPutPath = Path.Combine(_outputProjectFolder, $"{cultureInfo}.Iproj", "Localizable.strings");
            Directory.CreateDirectory(Path.GetDirectoryName(_finalOutPutPath));

            _streamWriter = new StreamWriter(_finalOutPutPath);
        }

        public void Dispose() => _streamWriter.Dispose();

        public void WriteComment(string comment) => _streamWriter.WriteLine($"/* {comment} */");

        public void WriteString(StringElement stringElement) => _streamWriter.WriteLine($"\"{stringElement.Key}\" = \"{stringElement.Value}\";");
    }
}