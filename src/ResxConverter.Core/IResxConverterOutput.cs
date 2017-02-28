using System;

namespace ResxConverter.Core
{
    public interface IResxConverterOutput : IDisposable
    {
        void WriteString(ResxString stringElement);
        void WriteComment(string comment);
    }
}
