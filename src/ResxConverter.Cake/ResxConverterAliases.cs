using Cake.Core;
using Cake.Core.Annotations;
using System;

namespace ResxConverter.Cake
{
    /// <summary>
    /// Contains alias for converting .NET Resx files to different output formats.
    /// </summary>
    [CakeAliasCategory("ResxConverter")]
    public static class ResxConverterAliases
    {
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
