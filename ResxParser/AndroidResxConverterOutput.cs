using System.Xml.Linq;
using System.IO;

namespace ResxParser
{
    public class AndroidResxConverterOutput : IResxConverterOutput
    {
        private string _culture;
        private string _outputProjectFolder;
        private XDocument _xDocument;
        private string _finalOutPutPath;

        public AndroidResxConverterOutput(string outputProjectFolder, string culture)
        {
            _outputProjectFolder = outputProjectFolder;
            _culture = culture;

            CreateXml();
        }

        private void CreateXml()
        {
            var cultureInfo = string.IsNullOrEmpty(_culture) ? string.Empty : $"-{_culture}";

            _finalOutPutPath = Path.Combine(_outputProjectFolder, $"values{cultureInfo}", "strings.xml");
            Directory.CreateDirectory(Path.GetDirectoryName(_finalOutPutPath));

            _xDocument = new XDocument(new XElement("resources"));
        }

        public void Dispose()
        {
            _xDocument.Save(_finalOutPutPath);
        }

        public void WriteComment(string comment)
        {
            _xDocument.Root.Add(new XComment(comment));
        }

        public void WriteString(StringElement stringElement)
        {
            _xDocument.Root.Add(CreateString(stringElement));
        }

        private XElement CreateString(StringElement stringElement)
        {
            var xStringElement = new XElement("string");
            xStringElement.SetAttributeValue(stringElement.Key.ToLowerUnderScoreFromCamelCase(), stringElement.Value);
            return xStringElement;
        }
    }
}