using Cake.Core;
using Cake.Core.Annotations;
using System;

namespace Cake.ResxConverter
{
    /// <summary>
    /// Contains alias for converting .NET Resx files to different output formats.
    /// </summary>
    [CakeAliasCategory("ResxConverter")]
    public static class ResxConverterAliases
    {
        /// <summary>
        /// ResxConverter alias.
        /// </summary>
        /// <example>
        /// <para>Cake task:</para>
        /// <code>
        /// <![CDATA[
        /// Task("ResxConverter")
        /// .Does(() =>
        /// {
        ///     ResxConverter.ConvertToAndroid("./Resources", "./Generated");
        /// });
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="context">The Cake context</param>
        /// <returns>ResxCoverter conversion options</returns>
        [CakePropertyAlias(Cache = true)]
        public static ResxConverterProvider ResxConverter(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new ResxConverterProvider();
        }
    }
}
