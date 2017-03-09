using System;
using System.IO;
using Xunit;

namespace ResxConverter.Mobile.Tests
{
    public class UsageSamples : IDisposable
    {
        private readonly DirectoryInfo _rootFolder, _androidFolder, _iOSFolder;

        public UsageSamples()
        {
            _rootFolder = new DirectoryInfo(Guid.NewGuid().ToString());
            _androidFolder = _rootFolder.CreateSubdirectory("android");
            _iOSFolder = _rootFolder.CreateSubdirectory("ios");
        }

        public void Dispose()
        {
            _rootFolder.Delete(true);
        }

        [Fact]
        public void Run()
        {
            ResxConverters.Android.Convert("Resources", _androidFolder.FullName);
            ResxConverters.iOS.Convert("Resources", _iOSFolder.FullName);
        }
    }
}
