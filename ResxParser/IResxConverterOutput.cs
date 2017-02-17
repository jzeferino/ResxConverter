using System;
namespace ResxParser
{
    public interface IResxConverterOutput : IDisposable
    {
        void WriteString(StringElement stringElement);
        void WriteComment(string comment);
    }
}
