using System;
using Moq;
using Ploeh.AutoFixture;
using Xunit;

namespace ResxConverter.Core.Tests
{
    public class ResxConverterTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void Creates_One_Output_File_Per_Culture()
        {
            var factoryMock = new Mock<IResxConverterOutputFactory>();
            var outputMock = new Mock<IResxConverterOutput>();
            var outputFolder = _fixture.Create<string>();

            factoryMock
                .Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(outputMock.Object);

            var sut = new ResxConverter(factoryMock.Object);
            sut.Convert("Resources/Empty", outputFolder);

            factoryMock.Verify(f => f.Create("", outputFolder), Times.Once);
            factoryMock.Verify(f => f.Create("pt-PT", outputFolder), Times.Once);
        }

        [Fact]
        public void Writes_One_Comment_Per_Resx_File()
        {
            var factoryMock = new Mock<IResxConverterOutputFactory>();
            var outputMock = new Mock<IResxConverterOutput>();

            factoryMock
                .Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(outputMock.Object);

            var sut = new ResxConverter(factoryMock.Object);
            sut.Convert("Resources/SingleCulture", _fixture.Create<string>());

            outputMock.Verify(o => o.WriteComment("R1.resx"), Times.Once);
            outputMock.Verify(o => o.WriteComment("R2.resx"), Times.Once);
            outputMock.Verify(o => o.WriteComment("R3.resx"), Times.Once);
        }

        [Fact]
        public void Writes_Expected_Strings()
        {
            var factoryMock = new Mock<IResxConverterOutputFactory>();
            var outputMock = new Mock<IResxConverterOutput>();

            factoryMock
                .Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(outputMock.Object);

            var sut = new ResxConverter(factoryMock.Object);
            sut.Convert("Resources/SingleCulture", _fixture.Create<string>());

            outputMock.Verify(o => o.WriteString(It.Is<ResxString>(s => s.Key == "R1S1" && s.Value == "R1S1")), Times.Once);
            outputMock.Verify(o => o.WriteString(It.Is<ResxString>(s => s.Key == "R1S2" && s.Value == "R1S2")), Times.Once);
            outputMock.Verify(o => o.WriteString(It.Is<ResxString>(s => s.Key == "R2S1" && s.Value == "R2S1")), Times.Once);
            outputMock.Verify(o => o.WriteString(It.Is<ResxString>(s => s.Key == "R3S1" && s.Value == "R3S1")), Times.Once);
        }

        [Fact]
        public void Checks_Null_Convert_Input_Parameters()
        {
            Assert.Throws<ArgumentNullException>(() => new ResxConverter(null));
        }

        [Fact]
        public void Checks_Null_ResxConverter_Input_Parameter()
        {
            var factoryMock = new Mock<IResxConverterOutputFactory>();
            var outputMock = new Mock<IResxConverterOutput>();

            factoryMock
                .Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(outputMock.Object);

            var sut = new ResxConverter(factoryMock.Object);

            Assert.Throws<ArgumentNullException>(() => sut.Convert(null, _fixture.Create<string>()));
            Assert.Throws<ArgumentNullException>(() => sut.Convert("Resources/SingleCulture", null));
            Assert.Throws<ArgumentNullException>(() => sut.Convert(null, null));
        }
    }
}
