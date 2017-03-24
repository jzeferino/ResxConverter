using Cake.Core;
using Cake.Core.Diagnostics;
using ResxConverter.Mobile;

namespace Cake.ResxConverter
{
    /// <summary>
    /// Provides different conversion methods.
    /// </summary>
    public class ResxConverterProvider
    {
        private readonly ICakeContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResxConverterProvider"/> class.
        /// </summary>
        /// <param name="context">The Cake context</param>
        public ResxConverterProvider(ICakeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Converts Resx files to Android <code>strings.xml</code> files.
        /// </summary>
        /// <param name="inputFolder">The input folder for Resx files</param>
        /// <param name="outputFolder">The output folder for string files</param>
        public void ConvertToAndroid(string inputFolder, string outputFolder)
        {
            _context.Log.Verbose("Converting Resx files in {0} to Android resource files in {1}", inputFolder, outputFolder);
            ResxConverters.Android.Convert(inputFolder, outputFolder);
        }

        /// <summary>
        /// Converts Resx files to iOS <code>Localizable.strings</code> files.
        /// </summary>
        /// <param name="inputFolder">The input folder for Resx files</param>
        /// <param name="outputFolder">The output folder</param>
        public void ConvertToiOS(string inputFolder, string outputFolder)
        {
            _context.Log.Verbose("Converting Resx files in {0} to iOS resource files in {1}", inputFolder, outputFolder);
            ResxConverters.iOS.Convert(inputFolder, outputFolder);
        }
    }
}
