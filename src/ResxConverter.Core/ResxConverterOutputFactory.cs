using System;

namespace ResxConverter.Core
{
    public sealed class ResxConverterOutputFactory : IResxConverterOutputFactory
    {
        private readonly Func<string, string, IResxConverterOutput> _factory;

        public ResxConverterOutputFactory(Func<string, string, IResxConverterOutput> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _factory = factory;
        }

        public IResxConverterOutput Create(string culture, string outputFolder) => _factory(culture, outputFolder);
    }
}
