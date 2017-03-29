using Ploeh.AutoFixture;
using System;
using System.IO;
using Xunit;

namespace ResxConverter.Mobile.Tests
{
    public class iOSResxConverterOutputTests : IDisposable
    {
        private readonly Fixture _fixture;
        private readonly DirectoryInfo _folder;

        public iOSResxConverterOutputTests()
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
            var expectedPath = Path.Combine(_folder.FullName, "Base.lproj", "Localizable.strings");
            using (var sut = new iOSResxConverterOutput(_folder.FullName, ""))
            {
                Assert.Equal(expectedPath, sut.OutputFilePath);
            }

            Assert.True(File.Exists(expectedPath));
        }

        [Fact]
        public void Creates_Correct_File_For_Culture()
        {
            var expectedPath = Path.Combine(_folder.FullName, "pt-PT.lproj", "Localizable.strings");
            using (var sut = new iOSResxConverterOutput(_folder.FullName, "pt-PT"))
            {
                Assert.Equal(expectedPath, sut.OutputFilePath);
            }

            Assert.True(File.Exists(expectedPath));
        }

        [Fact]
        public void Adds_Simple_Strings()
        {
            string filePath, value1, value2;

            using (var sut = new iOSResxConverterOutput(_folder.FullName, ""))
            {
                filePath = sut.OutputFilePath;
                value1 = _fixture.Create<string>();
                value2 = _fixture.Create<string>();

                sut.WriteString(new Core.ResxString { Key = "myString1", Value = value1 });
                sut.WriteString(new Core.ResxString { Key = "superString", Value = value2 });
            }

            var strings = File.ReadAllLines(filePath);

            Assert.Equal(2, strings.Length);
            var s = strings[0];
            Assert.Equal($"\"my_string1\" = \"{value1}\";", s);
            s = strings[1];
            Assert.Equal($"\"super_string\" = \"{value2}\";", s);
        }

        [Fact]
        public void Escapes_Strings()
        {
            string filePath, value = "\" text \\ text \n"; // In XML, only \n is used

            using (var sut = new iOSResxConverterOutput(_folder.FullName, ""))
            {
                filePath = sut.OutputFilePath;
                sut.WriteString(new Core.ResxString { Key = "str", Value = value });
            }

            var strings = File.ReadAllLines(filePath);

            Assert.Equal(1, strings.Length);
            var s = strings[0];
            Assert.Equal("\"str\" = \"\\\" text \\\\ text \\n\";", s);
        }
    }
}
