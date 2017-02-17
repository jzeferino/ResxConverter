using System;
using System.IO;
using System.Xml;

namespace ResxParser
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            var outputFile = "example.xml";
            using (var file = new StreamWriter(outputFile))
            {
                ParseResx((arg) => file, (arg) => $"{arg.Key}={arg.Value}", (arg) => $"/*{arg}*/");
            }
        }

        public static void ParseResx(Func<string, StreamWriter> createExternalFile, Func<StringElement, string> writeString, Func<string, string> writeComment)
        {
            foreach (string file in Directory.EnumerateFiles("Resources", "*.resx"))
            {
                var externalFile = createExternalFile(Path.GetFileName(file));

                var doc = new XmlDocument();
                doc.Load(file);

                foreach (XmlNode node in doc.DocumentElement)
                {
                    if (node.NodeType == XmlNodeType.Element && node.Name == "data")
                    {
                        externalFile.WriteLine(writeString(new StringElement
                        {
                            Key = node.Attributes.GetNamedItem("name").Value,
                            Value = node.InnerText
                        }));
                    }
                    else if (node.NodeType == XmlNodeType.Comment)
                    {
                        externalFile.WriteLine(writeComment(node.Value));
                    }
                }
            }
        }

        public class StringElement
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
