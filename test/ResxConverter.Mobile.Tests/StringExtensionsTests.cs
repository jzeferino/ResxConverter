using Xunit;

namespace ResxConverter.Mobile.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("String", "string")]
        [InlineData("SampleString", "sample_string")]
        [InlineData("ASampleString", "a_sample_string")]
        [InlineData("ThisIsASampleString", "this_is_a_sample_string")]
        [InlineData("SUPERSampleString", "super_sample_string")]
        public void ToLowerUnderScoreFromCamelCase_Success(string input, string expected)
        {
            var result = StringExtensions.ToLowerUnderScoreFromCamelCase(input);
            Assert.Equal(expected, result);
        }
    }
}
