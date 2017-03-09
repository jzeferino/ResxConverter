using System;
using System.Collections.Generic;
using System.Linq;

namespace ResxConverter.Mobile.CLI
{
    class Program
    {
        private static readonly Dictionary<string, Core.ResxConverter> Converters = new Dictionary<string, Core.ResxConverter>(StringComparer.OrdinalIgnoreCase)
        {
            { "android", ResxConverters.Android },
            { "ios", ResxConverters.iOS },
        };

        static void Main(string[] args)
        {
            if (args.Length == 3)
            {
                Core.ResxConverter converter;
                if (Converters.TryGetValue(args[0], out converter))
                {
                    converter.Convert(args[1], args[2]);
                    return;
                }
            }

            PrintUsage();
        }

        static void PrintUsage()
        {
            var exeName = typeof(Program).Assembly.GetName().Name;

            Console.WriteLine();
            Console.WriteLine("Usage: {0} <platform> <input folder> <output folder>", exeName);
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine();
            Console.WriteLine("  <platform>        The platform to which resource files should be generated. Possible values: ios, android.");
            Console.WriteLine("  <input folder>    The source folder for .resx files, searched recursively.");
            Console.WriteLine("  <output folder>   The root destination folder for generated resources files.");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine();
            Console.WriteLine("  {0} android ./resources ./generated", exeName);
            Console.WriteLine("  {0} ios ./resources ./generated", exeName);
            Console.WriteLine();
        }
    }
}
