using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

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
            var resxPerCulture = Directory.EnumerateFiles(folder, "*.resx");

            // TODO
            // group by culture if exist {Culture, Name, Path}

            //FIXME:
            // element.NodeType == XmlNodeType.Comment don't work.

            using (var outout = outputFactory("culture"))
            {
                foreach (string file in resxPerCulture)
                {
                    foreach (var element in XDocument.Load(file).Descendants())
                    {
                        if (element.NodeType == XmlNodeType.Element && element.Name == "data")
                        {
                            outout.WriteString((new StringElement
                            {
                                Key = element.Attribute("name").Value,
                                Value = element.Value.Trim()
                            }));
                        }
                        else if (element.NodeType == XmlNodeType.Comment)
                        {
                            outout.WriteComment(element.Value);
                        }
                    }
                }
            }
        }
    }
}
