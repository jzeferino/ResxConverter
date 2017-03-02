using System.IO;
using System.Xml.Linq;
using System.Linq;
using System;

namespace ResxConverter.Core
{
    public sealed class ResxConverter
    {
        private readonly IResxConverterOutputFactory _outputFactory;

        public ResxConverter(IResxConverterOutputFactory outputFactory)
        {
            if (outputFactory == null)
            {
                throw new ArgumentNullException(nameof(outputFactory));
            }

            _outputFactory = outputFactory;
        }

        public void Convert(string inputFolder, string outputFolder)
        {
            if (inputFolder == null)
            {
                throw new ArgumentNullException(nameof(inputFolder));
            }

            if (outputFolder == null)
            {
                throw new ArgumentNullException(nameof(outputFolder));
            }

            var resxPerCulture = Directory.EnumerateFiles(inputFolder, "*.resx")
                  .Select(path => new
                  {
                      Culture = Path.GetFileNameWithoutExtension(path).GetExtensionWitoutDot(),
                      FileName = Path.GetFileName(path),
                      FilePath = path
                  })
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

                            if (element?.Name == "data")
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
}

