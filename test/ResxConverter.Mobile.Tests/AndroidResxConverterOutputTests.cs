using Ploeh.AutoFixture;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace ResxConverter.Mobile.Tests
{
    public class AndroidResxConverterOutputTests : IDisposable
    {
        private readonly Fixture _fixture;
        private readonly DirectoryInfo _folder;

        public AndroidResxConverterOutputTests()
        {
            _fixture = new Fixture();
            _folder = new DirectoryInfo(Guid.NewGuid().ToString());
            _folder.Create();
        }

        public void Dispose()
        {
            _folder.Delete(true);
        }

        [Fact]
        public void Creates_Correct_File_For_Empty_Culture()
        {
            var expectedPath = Path.Combine(_folder.FullName, "values", "strings.xml");
            using (var sut = new AndroidResxConverterOutput(_folder.FullName, ""))
            {
                Assert.Equal(expectedPath, sut.OutputFilePath);
            }

            Assert.True(File.Exists(expectedPath));
        }

        [Fact]
        public void Creates_Correct_File_For_Simple_Culture()
        {
            var expectedPath = Path.Combine(_folder.FullName, "values-pt", "strings.xml");
            using (var sut = new AndroidResxConverterOutput(_folder.FullName, "pt"))
            {
                Assert.Equal(expectedPath, sut.OutputFilePath);
            }

            Assert.True(File.Exists(expectedPath));
        }

        [Fact]
        public void Creates_Correct_File_For_Composite_Culture()
        {
            var expectedPath = Path.Combine(_folder.FullName, "values-pt-rPT", "strings.xml");
            using (var sut = new AndroidResxConverterOutput(_folder.FullName, "pt-PT"))
            {
                Assert.Equal(expectedPath, sut.OutputFilePath);
            }

            Assert.True(File.Exists(expectedPath));
        }

        [Fact]
        public void Adds_Simple_Strings()
        {
            string filePath, value1, value2;

            using (var sut = new AndroidResxConverterOutput(_folder.FullName, ""))
            {
                filePath = sut.OutputFilePath;
                value1 = _fixture.Create<string>();
                value2 = _fixture.Create<string>();

                sut.WriteString(new Core.ResxString { Key = "myString1", Value = value1 });
                sut.WriteString(new Core.ResxString { Key = "superString", Value = value2 });
            }

            var xDoc = XDocument.Load(filePath);
            var strings = xDoc.Descendants("string").ToList();

            Assert.Equal(2, strings.Count);
            Assert.NotNull(strings.SingleOrDefault(s => s.Attribute("name").Value == "my_string1" && s.Value == value1));
            Assert.NotNull(strings.SingleOrDefault(s => s.Attribute("name").Value == "super_string" && s.Value == value2));
        }

        [Fact]
        public void Escapes_Strings()
        {
            string filePath, value = "\" text \\ text ' text \n"; // In XML, only \n is used

            using (var sut = new AndroidResxConverterOutput(_folder.FullName, ""))
            {
                filePath = sut.OutputFilePath;
                sut.WriteString(new Core.ResxString { Key = "str", Value = value });
            }

            var xDoc = XDocument.Load(filePath);
            var strings = xDoc.Descendants("string").ToList();

            Assert.Equal(1, strings.Count);
            Assert.Equal("\\\" text \\\\ text \\' text \\n", strings[0].Value);
        }
    }
}
