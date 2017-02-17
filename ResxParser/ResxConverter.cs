using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ResxParser
{
    public sealed class ResxConverter
    {
        private static readonly Lazy<ResxConverter> lazy =
            new Lazy<ResxConverter>(() => new ResxConverter());

        public static ResxConverter Instance { get { return lazy.Value; } }

        private ResxConverter()
        {
        }

        public void Convert(string folder, Func<string, IResxConverterOutput> outputFactory)
        {
            foreach (string file in Directory.EnumerateFiles(folder, "*.resx"))
            {
                using (var outout = outputFactory(file))
                {
                    var doc = XDocument.Load(file);

                    foreach (var element in doc.Descendants())
                    {
                        if (element.NodeType == XmlNodeType.Element && element.Name == "data")
                        {
                            outout.WriteString((new StringElement
                            {
                                Key = element.FirstAttribute.Value,
                                Value = element.Value
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
