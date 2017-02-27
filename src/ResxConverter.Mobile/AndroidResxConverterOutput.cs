using System.IO;
using System.Xml.Linq;
using ResxConverter.Core;
using System.Globalization;

namespace ResxConverter.Mobile
{
    public class AndroidResxConverterOutput : IResxConverterOutput
    {
        private string _culture;
        private string _outputProjectFolder;
        private XDocument _xDocument;
        private string _finalOutputPath;

        public AndroidResxConverterOutput(string outputProjectFolder, string culture)
        {
            _outputProjectFolder = outputProjectFolder;
            _culture = culture;

            CreateXml();
        }

        private void CreateXml()
        {
            var cultureSuffix = string.Empty;

            if (!string.IsNullOrEmpty(_culture))
            {
                var cultureInfo = new CultureInfo(_culture);
                if (cultureInfo.IsNeutralCulture)
                {
                    cultureSuffix = $"-{cultureInfo.TwoLetterISOLanguageName}";
                }
                else
                {
                    var regionInfo = new RegionInfo(cultureInfo.LCID);
                    cultureSuffix = $"-{cultureInfo.TwoLetterISOLanguageName}-r{regionInfo.TwoLetterISORegionName}";
                }
            }

            _finalOutputPath = Path.Combine(_outputProjectFolder, $"values{cultureSuffix}", "strings.xml");
            Directory.CreateDirectory(Path.GetDirectoryName(_finalOutputPath));

            _xDocument = new XDocument(new XElement("resources"));
        }

        public void Dispose()
        {
            _xDocument.Save(_finalOutputPath);
        }

        public void WriteComment(string comment)
        {
            _xDocument.Root.Add(new XComment(comment));
        }

        public void WriteString(ResxString stringElement)
        {
            _xDocument.Root.Add(CreateString(stringElement));
        }

        private XElement CreateString(ResxString stringElement)
        {
            var xStringElement = new XElement("string")
            {
                Value = stringElement.Value
            };
            xStringElement.SetAttributeValue("name", stringElement.Key.ToLowerUnderScoreFromCamelCase());
            return xStringElement;
        }
    }
}