using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace ResxConverter.Core
{
    public sealed class ResxConverter
    {
        private readonly IResxConverterOutputFactory _outputFactory;

        public ResxConverter(IResxConverterOutputFactory outputFactory)
        {
            _outputFactory = outputFactory;
        }

        public void Convert(string folder, string outputFolder)
        {
            var resxPerCulture = Directory.EnumerateFiles(folder, "*.resx")
                  .Select(path => new ResxCulture(path))
                  .GroupBy(resxCulture => resxCulture.Culture);

            foreach (var resxGroup in resxPerCulture)
            {
                using (var output = _outputFactory.Create(resxGroup.Key, outputFolder))
                {
                    foreach (var resxCulture in resxGroup)
                    {
                        // Writes the file we are converting into a comment.
                        output.WriteComment(resxCulture.FileName);

                        foreach (var node in XDocument.Load(resxCulture.FilePath).DescendantNodes())
                        {
                            var element = node as XElement;
                            var comment = node as XComment;

                            if (element != null && element.Name == "data")
                            {
                                output.WriteString(new ResxString
                                {
                                    Key = element.Attribute("name").Value,
                                    Value = element.Value.Trim()
                                });
                            }
                            else if (comment != null)
                            {
                                output.WriteComment(comment.Value);
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
            Culture = Path.GetFileNameWithoutExtension(path).GetExtensionWitoutDot();
            FileName = Path.GetFileName(path);
            FilePath = path;
        }

        public string Culture { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}

