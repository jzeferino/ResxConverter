using System;

namespace ResxConverter.Core
{
    /// <summary>
    /// Represents how a string and comment must be materialized.
    /// </summary>
    public interface IResxConverterOutput : IDisposable
    {
        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="stringElement">String. <see cref="ResxString"/></param>
        void WriteString(ResxString stringElement);

        /// <summary>
        /// Writes the comment.
        /// </summary>
        /// <param name="comment">Comment.</param>
        void WriteComment(string comment);
    }
}
