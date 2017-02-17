using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace ResxParser
{
    public sealed class ResxConverter
    {
        private static readonly Lazy<ResxConverter> lazy = new Lazy<ResxConverter>(() => new ResxConverter());

        public static ResxConverter Instance { get { return lazy.Value; } }

        private ResxConverter()
        {
        }

        public void Convert(string folder, Func<string, IResxConverterOutput> outputFactory)
        {
            var resxPerCulture = Directory.EnumerateFiles(folder, "*.resx")
                  .Select(path => new ResxCulture(path))
                  .GroupBy(resxCulture => resxCulture.Culture);

            foreach (var resxGroup in resxPerCulture)
            {
                using (var output = outputFactory(resxGroup.Key))
                {
                    foreach (var resxCulture in resxGroup)
                    {
                        // Writes the file we are converting into a comment.
                        output.WriteComment(resxCulture.FileName);

                        foreach (var element in XDocument.Load(resxCulture.FilePath).Descendants())
                        {
                            if (element.NodeType == XmlNodeType.Element && element.Name == "data")
                            {
                                output.WriteString((new StringElement
                                {
                                    Key = element.Attribute("name").Value,
                                    Value = element.Value.Trim()
                                }));
                            }
                            //FIXME:
                            // element.NodeType == XmlNodeType.Comment don't work.
                            else if (element.NodeType == XmlNodeType.Comment)
                            {
                                output.WriteComment(element.Value);
                            }
                        }
                    }
                }
            }
        }


    }

    internal class ResxCulture
    {
        public ResxCulture(string path)
        {
            Culture = ExtractCulture(path);
            FileName = Path.GetFileName(path);
            FilePath = path;
        }

        public string Culture { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public string ExtractCulture(string filename)
        {
            // TODO: use regex here?
            return Path.GetFileNameWithoutExtension(filename).GetExtensionWitoutDot();
        }
    }
}

