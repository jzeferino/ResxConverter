using ResxConverter.Core;
using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace ResxConverter.Mobile
{
    /// <summary>
    /// Android RESX converter output.
    /// </summary>
    public class AndroidResxConverterOutput : IResxConverterOutput
    {
        public string OutputFilePath { get; }
        private readonly XDocument _xDocument;

        public AndroidResxConverterOutput(string outputFolder, string culture)
        {
            if (outputFolder == null)
            {
                throw new ArgumentNullException(nameof(outputFolder));
            }

            OutputFilePath = GetOutputFilePath(outputFolder, culture);
            _xDocument = new XDocument(new XElement("resources"));
        }

        private static string GetOutputFilePath(string outputFolder, string culture)
        {
            var cultureSuffix = string.Empty;

            if (!string.IsNullOrEmpty(culture))
            {
                var cultureInfo = new CultureInfo(culture);
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

            return Path.Combine(outputFolder, $"values{cultureSuffix}", "strings.xml");
        }

        public void Dispose()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(OutputFilePath));
            _xDocument.Save(OutputFilePath);
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