using Moq;
using Xunit;

namespace ResxConverter.Core.Tests
{
    public class ResxConverterTests
    {
        public interface IOutputFactory
        {
            IResxConverterOutput Create(string culture);
        }

        [Fact]
        public void Creates_One_Output_File_Per_Culture()
        {
            var factoryMock = new Mock<IOutputFactory>();
            var outputMock = new Mock<IResxConverterOutput>();

            factoryMock
                .Setup(f => f.Create(It.IsAny<string>()))
                .Returns(outputMock.Object);

            ResxConverter.Instance.Convert("Resources/Empty", c => factoryMock.Object.Create(c));

            factoryMock.Verify(f => f.Create(""), Times.Once);
            factoryMock.Verify(f => f.Create("pt-PT"), Times.Once);
        }

        [Fact]
        public void Writes_One_Comment_Per_Resx_File()
        {
            var outputMock = new Mock<IResxConverterOutput>();

            ResxConverter.Instance.Convert("Resources/SingleCulture", c => outputMock.Object);

            outputMock.Verify(o => o.WriteComment("Empty1.resx"), Times.Once);
            outputMock.Verify(o => o.WriteComment("Empty2.resx"), Times.Once);
            outputMock.Verify(o => o.WriteComment("Empty3.resx"), Times.Once);
        }
    }
}
