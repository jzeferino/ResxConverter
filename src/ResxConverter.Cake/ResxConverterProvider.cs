using ResxConverter.Mobile;

namespace ResxConverter.Cake
{
    /// <summary>
    /// Provides different conversion methods.
    /// </summary>
    public class ResxConverterProvider
    {
        /// <summary>
        /// Converts Resx files to Android <code>string.xml</code> files.
        /// </summary>
        /// <param name="inputFolder">The input folder for Resx files</param>
        /// <param name="outputFolder">The output folder for string files</param>
        public void ConvertToAndroid(string inputFolder, string outputFolder) => ResxConverters.Android.Convert(inputFolder, outputFolder);

        /// <summary>
        /// Converts Resx files to iOS <code>Localizable.strings</code> files.
        /// </summary>
        /// <param name="inputFolder">The input folder for Resx files</param>
        /// <param name="outputFolder">The output folder</param>
        public void ConvertToiOS(string inputFolder, string outputFolder) => ResxConverters.iOS.Convert(inputFolder, outputFolder);
    }
}
